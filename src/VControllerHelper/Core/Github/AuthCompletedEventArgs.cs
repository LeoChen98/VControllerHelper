namespace VControllerHelper.Core.Github
{
    public class AuthCompletedEventArgs
    {
        #region Public Fields

        public string Code;
        public string Error;
        public string ErrorDescription;
        public string FullUrl;
        public bool IsSuccess;
        public string State;

        #endregion Public Fields
    }
}