using System;
namespace Alexis.WindowsPhone.Social.Weibo
{
    public class WeiboAccessToken
    {
        public string access_token { get; set; }

        /// <summary>
        /// 过期时间，默认为1天
        /// </summary>
        public string expires_in { get; set; }

        public DateTime ExpiresTime { get; set; }

        public string uid { get; set; }
    }
}
