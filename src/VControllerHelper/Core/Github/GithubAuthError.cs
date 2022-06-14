using System;
using System.Runtime.Serialization;

namespace VControllerHelper.Core.Github
{
    [Serializable]
    internal class GithubAuthError : Exception
    {
        #region Public Constructors

        public GithubAuthError()
        {
        }

        public GithubAuthError(string message) : base(message)
        {
        }

        public GithubAuthError(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected GithubAuthError(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        #endregion Protected Constructors
    }
}