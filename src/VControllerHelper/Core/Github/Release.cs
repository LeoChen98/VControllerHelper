using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Reflection;

namespace VControllerHelper.Core.Github
{
    /// <summary>
    /// GitHub Release数据和操作类
    /// </summary>
    internal class Release
    {
        #region Public Properties

        /// <summary>
        /// 附件，{名字，url}
        /// </summary>
        public Dictionary<string, string> Assets { get; private set; }

        /// <summary>
        /// 发行信息
        /// </summary>
        public string Body { get; private set; }

        /// <summary>
        /// 发行tag，通常为版本号
        /// </summary>
        public string Tag { get; private set; }

        /// <summary>
        /// 发行标题
        /// </summary>
        public string Title { get; private set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// 下载附件
        /// </summary>
        /// <param name="url">附件url</param>
        /// <param name="filename">保存路径</param>
        /// <param name="download_completed">可选， <see cref="AsyncCompletedEventHandler"/> 下载完成回调，默认为空</param>
        /// <param name="download_progress_changed">
        /// 可选， <see cref="DownloadProgressChangedEventHandler"/> 下载进度更新回调，默认为空
        /// </param>
        public static void DownloadAssest(string url, string filename, AsyncCompletedEventHandler download_completed = null, DownloadProgressChangedEventHandler download_progress_changed = null)
        {
            WebClient downloader = new WebClient();
            downloader.Headers.Add("Authorization", $"token {Settings.Items.GetItem<Settings.Account>().Github_AccessToken}");
            downloader.Headers.Add("Accept", "application/octet-stream");
            if (download_completed != null) downloader.DownloadFileCompleted += download_completed;
            if (download_progress_changed != null) downloader.DownloadProgressChanged += download_progress_changed;
            downloader.DownloadFileAsync(new Uri(url), filename);
        }

        /// <summary>
        /// 获取所有发行
        /// </summary>
        /// <param name="repo">仓库名</param>
        /// <returns>发行数据集合</returns>
        public static List<Release> GetAllReleases(string repo)
        {
            HttpWebRequest req = null;
            HttpWebResponse rep = null;
            string str = "";
            List<Release> rs = new List<Release>();
            try
            {
                req = (HttpWebRequest)WebRequest.Create($"https://api.github.com/repos/zhangbudademao-com/{repo}/releases");

                req.Accept = "application/vnd.github.v3+json";
                req.Headers.Add("Authorization", $"token {Settings.Items.GetItem<Settings.Account>().Github_AccessToken}");

                rep = (HttpWebResponse)req.GetResponse();
                using (StreamReader reader = new StreamReader(rep.GetResponseStream()))
                {
                    str = reader.ReadToEnd();
                }
            }
            finally
            {
                if (rep != null) rep.Close();
                if (req != null) req.Abort();
            }

            JArray arr = JArray.Parse(str);
            foreach (JToken obj in arr)
            {
                rs.Add(new Release { Title = obj["name"].ToString(), Body = obj["body"].ToString(), Tag = obj["tag_name"].ToString(), Assets = new Dictionary<string, string>() });
                foreach (JToken t in (JArray)obj["assets"])
                {
                    rs[rs.Count - 1].Assets.Add(t["name"].ToString(), t["url"].ToString());
                }
            }

            return rs;
        }

        /// <summary>
        /// 获取最新发行
        /// </summary>
        /// <param name="repo">仓库名</param>
        /// <returns>发行数据</returns>
        public static Release GetLatestRelease(string repo)
        {
            HttpWebRequest req = null;
            HttpWebResponse rep = null;
            string str = "";
            Release rs = new Release();
            try
            {
                req = (HttpWebRequest)WebRequest.Create($"https://api.github.com/repos/zhangbudademao-com/{repo}/releases/latest");

                req.Accept = "application/vnd.github.v3+json";
                req.Headers.Add("Authorization", $"token {Settings.Items.GetItem<Settings.Account>().Github_AccessToken}");
                req.UserAgent = $"PrfLauncher/{Assembly.GetExecutingAssembly().GetName().Version.ToString(4)}";

                rep = (HttpWebResponse)req.GetResponse();
                using (StreamReader reader = new StreamReader(rep.GetResponseStream()))
                {
                    str = reader.ReadToEnd();
                }
            }
            finally
            {
                if (rep != null) rep.Close();
                if (req != null) req.Abort();
            }

            JObject obj = JObject.Parse(str);
            rs.Title = obj["name"].ToString();
            rs.Body = obj["body"].ToString();
            rs.Tag = obj["tag_name"].ToString();
            rs.Assets = new Dictionary<string, string>();

            foreach (JToken t in (JArray)obj["assets"])
            {
                rs.Assets.Add(t["name"].ToString(), t["url"].ToString());
            }

            return rs;
        }

        #endregion Public Methods
    }
}