using Newtonsoft.Json;
using System;
using System.IO;

namespace VControllerHelper.Settings
{
    internal class CSecretSettings<T> : CSettings<T>
    {
        #region Private Fields

        private const string key = "{73AD6158-9371-4541-B9FC-1E83307F4E7B}";

        #endregion Private Fields

        #region Public Constructors

        public CSecretSettings(string _savepath) : base()
        {
            if (!Directory.Exists(settings_path))
                Directory.CreateDirectory(settings_path);
            savepath = $"{settings_path}{_savepath}";

            if (File.Exists(savepath))
            {
                string str = File.ReadAllText(savepath);
                str = Helpers.EncryptHelper.DesDecrypt(str, key);
                OutputTable<T> OT = JsonConvert.DeserializeObject<OutputTable<T>>(str);
                ST = OT.settings;
            }
            else
                ST = (T)Activator.CreateInstance(typeof(T));
        }

        public CSecretSettings()
        {
            ST = (T)Activator.CreateInstance(typeof(T));
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// 保存数据
        /// </summary>
        public override void Save()
        {
            OutputTable<T> OT = new OutputTable<T>(ST);
            string json = JsonConvert.SerializeObject(OT);
            json = Helpers.EncryptHelper.DesEncrypt(json, key);
            File.WriteAllText(savepath, json);
        }

        #endregion Public Methods
    }
}