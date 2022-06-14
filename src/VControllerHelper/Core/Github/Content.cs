using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace VControllerHelper.Core.Github
{
    /// <summary>
    /// GitHub文件内容操作
    /// </summary>
    internal class Content
    {
        #region Public Methods

        /// <summary>
        /// 获取GitHub文件
        /// </summary>
        /// <param name="savepath">文件存储位置</param>
        /// <param name="repo">库</param>
        /// <param name="path">库路径</param>
        /// <param name="reference">分支</param>
        public static void GetContentFile(string savepath, string repo, string path, string reference)
        {
            Task<byte[]> tb = ClientHolder.Client.Repository.Content.GetRawContentByRef("zhangbudademao-com", repo, path, reference);
            tb.Wait();
            using (FileStream fs = File.Create(savepath))
            {
                fs.Write(tb.Result, 0, tb.Result.Length);
            }
        }

        /// <summary>
        /// 获取GitHub文件内容字符串
        /// </summary>
        /// <param name="repo">库</param>
        /// <param name="path">库路径</param>
        /// <param name="reference">分支</param>
        /// <returns>文件内容字符串</returns>
        public static string GetContentString(string repo, string path, string reference)
        {
            Task<byte[]> tb = ClientHolder.Client.Repository.Content.GetRawContentByRef("zhangbudademao-com", repo, path, reference);
            tb.Wait();
            return Encoding.UTF8.GetString(tb.Result);
        }

        #endregion Public Methods
    }
}