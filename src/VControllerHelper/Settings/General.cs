namespace VControllerHelper.Settings
{
    internal class General : CSettings<General.GeneralTable>
    {
        #region Public Constructors

        public General() : base("General.dms")
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public string EsInstallPath
        {
            get { return ST.es_install_path; }
            set
            {
                ST.es_install_path = value;
                PropertyChangedA("EsInstallPath");
                Save();
            }
        }

        public bool IsAutoUpdate
        {
            get { return ST.is_auto_update; }
            set
            {
                ST.is_first_run = value;
                PropertyChangedA("IsAutoUpdate");
                Save();
            }
        }

        public bool IsFirstRun
        {
            get { return ST.is_first_run; }
            set
            {
                ST.is_first_run = value;
                PropertyChangedA("IsFirstRun");
                Save();
            }
        }

        public string SectorFilesPath
        {
            get { return ST.sector_files_path; }
            set
            {
                ST.sector_files_path = value;
                PropertyChangedA("SectorFilesPath");
                Save();
            }
        }

        #endregion Public Properties

        #region Public Classes

        public class GeneralTable
        {
            #region Public Fields

            public string es_install_path;
            public bool is_auto_update = true;
            public bool is_first_run = true;
            public string sector_files_path;

            #endregion Public Fields
        }

        #endregion Public Classes
    }
}