using System.Threading.Tasks;

namespace VControllerHelper.Core.Github
{
    /// <summary>
    /// 用户相关操作类
    /// </summary>
    internal class User
    {
        #region Public Methods

        /// <summary>
        /// 获取当前登录用户的登录名
        /// </summary>
        /// <returns>登录名</returns>
        public static string GetAuthedUserLogin()
        {
            Task<Octokit.User> Tuser = ClientHolder.Client.User.Current();
            Tuser.Wait();
            return Tuser.Result.Login;
        }

        #endregion Public Methods
    }
}