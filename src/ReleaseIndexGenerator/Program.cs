using Newtonsoft.Json;

namespace ReleaseIndexGenerator
{
    internal class Program
    {
        #region Private Methods

        /// <summary> 获取文件 </summary> 
        /// <param name="dir">查找目录</param> 
        /// <param name="top">根目录</param>
        /// <param name="files">引用，文件字典<相对路径,MD5></param>
        private static void GetFiles(DirectoryInfo dir, string top, ref Dictionary<string, string> files)
        {
            if (dir.GetFiles().ToList().Exists((e) => { return e.Name == ".releaseignoreflag"; }))
            {
                return;
            }

            foreach (FileInfo file in dir.GetFiles())
            {
                files.Add(file.FullName.Replace(top, ""), EncryptHelper.GetMD5FromFile_32(file.FullName).ToLower());
            }

            foreach (DirectoryInfo sub_dir in dir.GetDirectories())
            {
                GetFiles(sub_dir, top, ref files);
            }
        }

        private static void Main(string[] args)
        {
            if (args.Length > 0 && !string.IsNullOrEmpty(args[0]) && Directory.Exists(args[0]))
            {
                Dictionary<string, string> files = new Dictionary<string, string>();

                GetFiles(new DirectoryInfo(args[0]), args[0], ref files);

                File.WriteAllText($"{args[0]}\\index.json", JsonConvert.SerializeObject(files));
            }
        }

        #endregion Private Methods
    }
}