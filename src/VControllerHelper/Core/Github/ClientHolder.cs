using Octokit;

namespace VControllerHelper.Core.Github
{
    internal class ClientHolder
    {
        #region Public Fields

        public static GitHubClient Client = new GitHubClient(new ProductHeaderValue("PrfLauncher"));

        #endregion Public Fields
    }
}