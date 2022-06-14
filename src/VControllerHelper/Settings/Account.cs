namespace VControllerHelper.Settings
{
    internal class Account : CSecretSettings<Account.AccountTable>
    {
        #region Private Fields

        private static Core.Vatsim.Rating.RatingType? _rating = null;

        #endregion Private Fields

        #region Public Constructors

        public Account() : base("Account.dmss")
        {
            if (ST == null)
                ST = new AccountTable();
        }

        public Account(object STFlag) : base()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public string Cid
        {
            get { return ST.cid; }
            set
            {
                ST.cid = value;
                Save();
                PropertyChangedA("Cid");
            }
        }

        public string Github_AccessToken
        {
            get { return ST.github_accesstoken; }
            set
            {
                ST.github_accesstoken = value;
                Save();
                PropertyChangedA("Github_AccessToken");
            }
        }

        public string HoppieLogonCode
        {
            get { return ST.hoppie_logon_code; }
            set
            {
                ST.hoppie_logon_code = value;
                Save();
                PropertyChangedA("HoppieLogonCode");
            }
        }

        public string Password
        {
            get { return ST.password; }
            set
            {
                ST.password = value;
                Save();
                PropertyChangedA("Password");
            }
        }

        public Core.Vatsim.Rating.RatingType Rating
        {
            get
            {
                if (_rating == null)
                    _rating = Core.Vatsim.Rating.GetRating(Cid);

                return (Core.Vatsim.Rating.RatingType)_rating;
            }
        }

        #endregion Public Properties

        #region Public Classes

        public class AccountTable
        {
            #region Public Fields

            public string cid;
            public string github_accesstoken;
            public string hoppie_logon_code;
            public string password;

            #endregion Public Fields
        }

        #endregion Public Classes
    }
}