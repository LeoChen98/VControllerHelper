using DmCommons;
using Newtonsoft.Json.Linq;
using System;
using System.Net;

namespace VControllerHelper.Core.Vatsim
{
    internal class Rating
    {
        #region Public Enums

        /// <summary>
        /// 管制等级枚举
        /// </summary>
        public enum RatingType
        {
            Error = -1,
            Suspended = 0,
            Observer = 1,
            S1 = 2,
            S2 = 3,
            S3 = 4,
            C1 = 5,
            C2 = 6,
            C3 = 7,
            I1 = 8,
            I2 = 9,
            I3 = 10,
            SUP = 11,
            ADM = 12
        }

        #endregion Public Enums

        #region Public Methods

        /// <summary>
        /// 获取管制等级
        /// </summary>
        /// <param name="cid">VATSIM cid</param>
        /// <returns><see cref="RatingType"/> 类型的rating</returns>
        public static RatingType GetRating(string cid)
        {
            string str = "";
            int recount = 0;

        redo:
            if (recount <= 3)
            {
                try
                {
                    str = Http.GetBody($"https://api.vatsim.net/api/ratings/{cid}/");
                }
                catch
                {
                    recount++;
                    goto redo;
                }
            }

            if (!string.IsNullOrEmpty(str))
            {
                JObject obj = JObject.Parse(str);
                return (RatingType)Enum.Parse(typeof(RatingType), obj["rating"].ToString());
            }

            return RatingType.Error;
        }

        #endregion Public Methods
    }
}