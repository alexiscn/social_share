using System;
namespace Alexis.WindowsPhone.Social
{
    public class AccessToken
    {
        public string Token { get; set; }

        public DateTime ExpiresTime { get; set; }

        public string UserInfo { get; set; }

        public string OpenId { get; set; }

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
