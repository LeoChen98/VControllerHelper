using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;

namespace VControllerHelper.Settings
{
    public class CSettings<T> : CSettingsFlags, INotifyPropertyChanged
    {
        #region Protected Fields

        protected static string settings_path = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\VControllerHelper\\Profile\\";
        protected string savepath;
        protected T ST;

        #endregion Protected Fields

        #region Public Constructors

        public CSettings()
        {
            ST = (T)Activator.CreateInstance(typeof(T));
        }

        public CSettings(string _savepath)
        {
            if (!Directory.Exists(settings_path))
                Directory.CreateDirectory(settings_path);
            savepath = $"{settings_path}{_savepath}";

            if (File.Exists(savepath))
            {
                string str = File.ReadAllText(savepath);
                OutputTable<T> OT = JsonConvert.DeserializeObject<OutputTable<T>>(str);
                ST = OT.settings;
            }
            else
                ST = (T)Activator.CreateInstance(typeof(T));
        }

        #endregion Public Constructors

        #region Public Events

        /// <summary>
        /// 属性更改事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Methods

        /// <summary>
        /// 属性改变通知函数
        /// </summary>
        /// <param name="sender">发送对象</param>
        /// <param name="e">属性</param>
        public void PropertyChangedA(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// 属性改变通知函数
        /// </summary>
        /// <param name="sender">发送对象</param>
        /// <param name="e">属性</param>
        public void PropertyChangedA(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public virtual void Save()
        {
            OutputTable<T> OT = new OutputTable<T>(ST);
            string json = JsonConvert.SerializeObject(OT);
            File.WriteAllText(savepath, json);
        }

        #endregion Public Methods

        #region Public Classes

        /// <summary>
        /// 导出数据表
        /// </summary>
        public class OutputTable<T>
        {
            #region Public Fields

            [JsonProperty("pid")]
            public const int pid = 120;

            [JsonProperty("version")]
            public const int version = 1;

            public T settings;

            #endregion Public Fields

            #region Public Constructors

            public OutputTable(T ST)
            {
                settings = ST;
            }

            #endregion Public Constructors
        }

        #endregion Public Classes
    }

    public class CSettingsFlags
    { }
}