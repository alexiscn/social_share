using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Alexis.WindowsPhone.Social;

namespace SocialSDKDemo
{
    public class Constants
    {
        public const string SHARE_IMAGE = "share.jpg";

        public static ClientInfo GetClient(SocialType type)
        {
            ClientInfo client = new ClientInfo();
            switch (type)
            {
                case SocialType.Weibo:
                    client.ClientId = "958678939";
                    client.ClientSecret = "8436644de1b06228d7f6195a6e0e5bd7";
                    client.RedirectUri = "http://tmango.com";//如果是新浪微博，需要在open.weibo.com-->我的应用-->应用信息-->高级信息 设置回调页面
                    break;
                case SocialType.Tencent:
                    client.ClientId = "801184653";
                    client.ClientSecret = "d26955f60eb7db07a2a1f62c4743edc0";
                    break;
                case SocialType.Renren:
                    client.ClientId = "96733da4fd3f459199c05f9b6c95f284";
                    client.ClientSecret = "8f49244064ea44c195bc9e0279080e4c";
                    break;
                case SocialType.Douban:
                    client.ClientId = "";
                    client.ClientSecret = "";
                    break;
                case SocialType.Net:
                    client.ClientId = "";
                    client.ClientSecret = "";
                    break;
                case SocialType.Sohu:
                    client.ClientId = "";
                    client.ClientSecret = "";
                    break;
                default:
                    break;
            }
            return client;
        }
    }
}
