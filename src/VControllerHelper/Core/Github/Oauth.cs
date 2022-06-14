using Octokit;
using System.Diagnostics;

namespace VControllerHelper.Core.Github
{
    internal class Oauth
    {
        #region Public Delegates

        public delegate void AuthCompletedHandler(object sender, AuthCompletedEventArgs e);

        #endregion Public Delegates

        #region Public Events

        public static event AuthCompletedHandler AuthCompleted;

        #endregion Public Events

        #region Public Methods

        public static async void RaiseAuthCompleted(object sender, AuthCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                var request = new OauthTokenRequest(Properties.Resources.Github_Appkey, Properties.Resources.Github_Appsecret, e.Code);
                Settings.Items.GetItem<Settings.Account>().Github_AccessToken = (await ClientHolder.Client.Oauth.CreateAccessToken(request)).AccessToken;
                ClientHolder.Client.Credentials = new Credentials(Settings.Items.GetItem<Settings.Account>().Github_AccessToken);

                if (Settings.Items.GetItem<Settings.Account>().Cid != null)
                {
                    _ = Settings.Items.GetItem<Settings.Account>().Rating;
                }

                //View_Model.StartPageVM.Instance.RaiseWelcomeTextChanged();
            }
            else
            {
                throw new GithubAuthError($"{e.Error}: {e.ErrorDescription}");
            }

            AuthCompleted(sender, e);
        }

        public static void StartAuth()
        {
            string auth_url = GetAuthUrl();
            Process.Start(auth_url);
        }

        #endregion Public Methods

        #region Private Methods

        private static string GetAuthUrl()
        {

            var request = new OauthLoginRequest(Properties.Resources.Github_Appkey)
            {
                Scopes = { "user", "repo" },
                RedirectUri = new System.Uri($"http://127.0.0.1:{Server.Port}/auth")
            };

            return ClientHolder.Client.Oauth.GetGitHubLoginUrl(request).AbsoluteUri;
        }

        #endregion Private Methods
    }
}