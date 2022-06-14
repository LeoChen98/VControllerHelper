using DmCommons;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VControllerHelper.Settings;

namespace VControllerHelper
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            App app = new App();

            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                try
                {
                    Http.GetBody($"http://127.0.0.1:{int.Parse(File.ReadAllText($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\zhangbudademao.com\\PrfLauncher\\ServerPort.txt"))}/ShowMainWindow");
                }
                finally
                {
                    Environment.Exit(1);
                }
            }

            //启动本地HTTP服务
            Core.Server.Start();

            ////初始化pipe server
            //Core.PipeServer.Instance.Start();

            ////如果已登录，设置GitHub凭证
            //if (!string.IsNullOrEmpty(Items.GetItem<Account>().Github_AccessToken)) Core.Github.ClientHolder.Client.Credentials = new Octokit.Credentials(Items.GetItem<Account>().Github_AccessToken);

            //启动系统托盘
            _ = NotifyIcons.NotifyIcon.Instance;

            //if (!Items.GetItem<General>().IsFirstRun && Items.GetItem<Account>().Cid != null)
            //{
            //    //初始化呼号列表
            //    Core.CallsignPickup.InitialCallsignList();

            //    //初始化vatsim rating
            //    _ = Items.GetItem<Account>().Rating;
            //}

            ////启动vatsim data loader
            //Core.Vatsim.Data.DataLoader.Instance.Start();

#if DEBUG
            //如果在调试模式，启动调试
            goDebug();
#endif
            app.Run();
            //app.Run(View.WindowManager.Instance.GetWindow<View.LaunchWindow>());
        }


#if DEBUG

        #region Private Methods

        private static void goDebug()
        {
            //Core.InformationProvider.ATISProvider.GetATIS("ZULS", "A", new Dictionary<string, int> { { "06", -1 } }, new Dictionary<string, string> { { "07", "IDM" } });
            //Core.InformationProvider.MetarProvider.GetMetarData("ZBTJ 051630Z ///// //// // VV600 12/11 Q1018 NOSIG", new object());
            //Core.InformationProvider.MetarProvider.GetMetarJson("ZGGG");
        }

        #endregion Private Methods

#endif
    }
}
