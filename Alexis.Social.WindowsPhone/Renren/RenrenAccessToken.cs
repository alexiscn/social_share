using System;
namespace Alexis.WindowsPhone.Social.Renren
{
    public class RenrenAccessToken
    {
        public string access_token { get; set; }

        public int expires_in { get; set; }

        public DateTime ExpiresTime { get; set; }

        public string refresh_token { get; set; }

        public string scope { get; set; }

        public RenrenUser user { get; set; }
    }
}
