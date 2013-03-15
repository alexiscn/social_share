using System;
namespace Alexis.WindowsPhone.Social
{
    /// <summary>
    /// Access Token for Social Sharing Service
    /// </summary>
    public class AccessToken
    {
        /// <summary>
        /// access token string
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Date to Expire
        /// </summary>
        public DateTime ExpiresTime { get; set; }

        /// <summary>
        /// username
        /// </summary>
        public string UserInfo { get; set; }

        /// <summary>
        /// used only in Tencent
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// return if token has Expired
        /// </summary>
        public bool IsExpired
        {
            get
            {
                if (ExpiresTime == null)
                {
                    return false;
                }
                return ExpiresTime.AddSeconds(30) <= DateTime.Now;
            }
        }
    }
}
