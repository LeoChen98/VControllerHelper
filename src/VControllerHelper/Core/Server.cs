using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VControllerHelper.Core
{
    internal class Server
    {
        #region Public Fields

        /// <summary>
        /// 服务运行端口
        /// </summary>
        public static int Port = 11452;

        #endregion Public Fields

        #region Private Fields

        private static HttpListener server;

        #endregion Private Fields

        #region Public Properties

        public static bool IsServerRuning
        {
            get
            {
                return server.IsListening;
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// 启动服务
        /// </summary>
        public static void Start()
        {
            if (server == null)
            {
                //获取可用端口号
                if (!PortIsAvailable(Port))
                {
                    Port = GetFirstAvailablePort();
                }

                if (!Directory.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\VControllerHelper\\"))
                    Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\VControllerHelper\\");

                using (Stream mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("VControllerHelper.Core.ServerSetup.bat"))
                {
                    byte[] datah = new byte[10240];
                    mrs.Read(datah, 0, (int)mrs.Length);

                    File.WriteAllBytes($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\VControllerHelper\\ServerSetup.bat", datah);
                }

                File.WriteAllText($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\VControllerHelper\\ServerPort.txt", Port.ToString());

                Process process = new Process
                {
                    StartInfo =
                    {
                        WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                        UseShellExecute = true,
                        FileName = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\VControllerHelper\\ServerSetup.bat",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Verb = "runas",
                        Arguments = Port.ToString()
                    }
                };
                process.Start();
                process.WaitForExit();

                server = new HttpListener();
                server.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
                server.Prefixes.Add($"http://+:{Port}/");
            }

            server.Start();
            server.BeginGetContext(ProcessRequest, server);
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public static void Stop()
        {
            server.Stop();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 获取第一个可用的端口号
        /// </summary>
        /// <returns></returns>
        private static int GetFirstAvailablePort(int BEGIN_PORT = 8000, int MAX_PORT = 10000)
        {
            for (int i = BEGIN_PORT; i < MAX_PORT; i++)
            {
                if (PortIsAvailable(i)) return i;
            }

            return -1;
        }

        /// <summary>
        /// 检查指定端口是否已用
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        private static bool PortIsAvailable(int port)
        {
            bool isAvailable = true;

            IList portUsed = PortIsUsed();

            foreach (int p in portUsed)
            {
                if (p == port)
                {
                    isAvailable = false; break;
                }
            }

            return isAvailable;
        }

        /// <summary>
        /// 获取操作系统已用的端口号
        /// </summary>
        /// <returns></returns>
        private static IList PortIsUsed()
        {
            //获取本地计算机的网络连接和通信统计数据的信息
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();

            //返回本地计算机上的所有Tcp监听程序
            IPEndPoint[] ipsTCP = ipGlobalProperties.GetActiveTcpListeners();

            //返回本地计算机上的所有UDP监听程序
            IPEndPoint[] ipsUDP = ipGlobalProperties.GetActiveUdpListeners();

            //返回本地计算机上的Internet协议版本4(IPV4) 传输控制协议(TCP)连接的信息。
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            IList allPorts = new ArrayList();
            foreach (IPEndPoint ep in ipsTCP) allPorts.Add(ep.Port);
            foreach (IPEndPoint ep in ipsUDP) allPorts.Add(ep.Port);
            foreach (TcpConnectionInformation conn in tcpConnInfoArray) allPorts.Add(conn.LocalEndPoint.Port);

            return allPorts;
        }

        /// <summary>
        /// 请求处理
        /// </summary>
        /// <param name="result">请求体</param>
        private static void ProcessRequest(IAsyncResult result)
        {
            try
            {
                HttpListenerContext context = server.EndGetContext(result);
                server.BeginGetContext(ProcessRequest, server);

                Task.Run(() =>
                {
                    switch (context.Request.Url.AbsolutePath.ToLower())
                    {
                        case "/metar":
                        case "/metar.php":
                            OnMetarRequested(context);
                            break;

                        case "/atis":
                            OnAtisRequested(context);
                            break;

                        case "/auth":
                            OnGitHubAuthCallback(context);
                            break;

                        case "/exit":
                            OnApplicationExitCalled(context);
                            break;

                        case "/showmainwindow":
                            OnShowMainWindowCalled(context);
                            break;

                        case "/cpdlc":
                        case "/pdc":
                        default:
                            OnContentNotFound(context);
                            break;
                    }
                });
            }
            catch
            {
            }
        }

        /// <summary>
        /// 程序退出请求处理函数
        /// </summary>
        /// <param name="context">请求体</param>
        private static void OnApplicationExitCalled(HttpListenerContext context)
        {
            using (Stream mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("VControllerHelper.Core.ExitCallbackCallback.html"))
            {
                context.Response.StatusCode = 200;
                context.Response.ContentType = "text/html;charset=utf-8";

                byte[] datah = new byte[5120];
                mrs.Read(datah, 0, (int)mrs.Length);
                context.Response.OutputStream.Write(datah, 0, datah.Length);

                context.Response.Close();
            }

            Environment.Exit(0);
        }

        /// <summary>
        /// 内容不存在错误请求处理函数
        /// </summary>
        /// <param name="context">请求体</param>
        private static void OnContentNotFound(HttpListenerContext context)
        {
            context.Response.StatusCode = 404;
            context.Response.ContentType = "text/plain;chartset=utf-8";
            byte[] data = Encoding.UTF8.GetBytes("Not found!");
            context.Response.OutputStream.Write(data, 0, data.Length);
            context.Response.Close();
        }



        /// <summary>
        /// ATIS请求处理函数
        /// </summary>
        /// <param name="context">请求体</param>
        private static void OnAtisRequested(HttpListenerContext context)
        {
            //SendResponse(context, Encoding.UTF8.GetBytes(Provider.Instance.StartATIS(context)));
        }

        /// <summary>
        /// 主窗口调起请求处理函数
        /// </summary>
        /// <param name="context">请求体</param>
        private static void OnShowMainWindowCalled(HttpListenerContext context)
        {
            //App.Current.Dispatcher.Invoke(() =>
            //{
            //    View.WindowManager.Instance.GetWindow<View.MainWindow>().Show();
            //    View.WindowManager.Instance.GetWindow<View.MainWindow>().WindowState = System.Windows.WindowState.Normal;
            //    View.WindowManager.Instance.GetWindow<View.MainWindow>().Activate();
            //});
            //SendResponse(context, new byte[0]);
        }

        /// <summary>
        /// GitHub登录回调请求处理函数
        /// </summary>
        /// <param name="context">请求体</param>
        private static void OnGitHubAuthCallback(HttpListenerContext context)
        {
            //AuthCompletedEventArgs e = new AuthCompletedEventArgs();
            //e.FullUrl = context.Request.Url.AbsoluteUri;
            //if (context.Request.QueryString["code"] != null)
            //{
            //    e.Code = context.Request.QueryString["code"];
            //    e.State = context.Request.QueryString["state"];
            //    e.IsSuccess = true;
            //}
            //else
            //{
            //    e.Error = context.Request.QueryString["error"];
            //    e.ErrorDescription = context.Request.QueryString["error_description"];
            //    e.IsSuccess = false;
            //}
            //Oauth.RaiseAuthCompleted(server, e);

            //using (Stream mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("PrfLauncher.Core.Github.AuthCallback.html"))
            //{
            //    context.Response.StatusCode = 200;
            //    context.Response.ContentType = "text/html;charset=utf-8";

            //    byte[] datah = new byte[5120];
            //    mrs.Read(datah, 0, (int)mrs.Length);
            //    context.Response.OutputStream.Write(datah, 0, datah.Length);

            //    context.Response.Close();
            //}
        }

        /// <summary>
        /// Metar获取请求处理函数
        /// </summary>
        /// <param name="context">请求体</param>
        private static void OnMetarRequested(HttpListenerContext context)
        {
            //string str_weather = "";
            //foreach (MetarProvider.MetarSource source in Enum.GetValues(typeof(MetarProvider.MetarSource)))
            //{
            //    str_weather = MetarProvider.GetMetarString(context.Request.QueryString["id"], source);
            //    if (str_weather != "No Data")
            //        break;
            //}
            //str_weather = str_weather.Replace("SPECI ", "").Replace("METAR ", "").Replace("=", "");
            //SendResponse(context, Encoding.UTF8.GetBytes(str_weather));
        }

        /// <summary>
        /// 发送返回包
        /// </summary>
        /// <param name="context">请求体</param>
        /// <param name="data">数据字节</param>
        private static void SendResponse(HttpListenerContext context, byte[] data)
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = "text/plain;chartset=utf-8";
            context.Response.OutputStream.Write(data, 0, data.Length);
            context.Response.Close();
        }

        #endregion Private Methods
    }
}