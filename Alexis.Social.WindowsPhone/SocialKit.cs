using System;
using System.Net;
using System.Windows;
using System.Text;
using System.Runtime.Serialization.Json;
using Alexis.WindowsPhone.Social.Helper;

namespace Alexis.WindowsPhone.Social
{
    public class ClientInfo
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Tag { get; set; }

        public string RedirectUri { get; set; }

    }

    public class SocialKit
    {
        internal static string GetAuthorizeUrl(SocialType type, ClientInfo client)
        {
            string url = "";
            switch (type)
            {
                case SocialType.Weibo:
                    if (string.IsNullOrEmpty(client.RedirectUri))
                    {
                        client.RedirectUri = "https://api.weibo.com/oauth2/default.html";
                    }
                    url = "https://api.weibo.com/oauth2/authorize?client_id=" + client.ClientId + "&response_type=code&redirect_uri=" + client.RedirectUri + "&display=mobile";
                    break;
                case SocialType.Tencent:
                    if (string.IsNullOrEmpty(client.RedirectUri))
                    {
                        client.RedirectUri = "http://t.qq.com";
                    }
                    url = "https://open.t.qq.com/cgi-bin/oauth2/authorize?client_id=" + client.ClientId + "&response_type=code&redirect_uri=" + client.RedirectUri + "&wap=false";
                    break;
                case SocialType.Renren:
                    if (string.IsNullOrEmpty(client.RedirectUri))
                    {
                        client.RedirectUri = "http://graph.renren.com/oauth/login_success.html";
                    }
                    url = "https://graph.renren.com/oauth/authorize?response_type=code&client_id=" + client.ClientId + "&redirect_uri=" + client.RedirectUri + "&display=mobile&scope=photo_upload";
                    break;
                case SocialType.QZone:
                    if (string.IsNullOrEmpty(client.RedirectUri))
                    {
                        client.RedirectUri = "http://open.z.qq.com/moc2/success.jsp";
                    }
                    url = "https://openmobile.qq.com/oauth2.0/m_authorize?response_type=token&client_id=" + client.ClientId + "&redirect_uri=" + client.RedirectUri + "&display=mobile";
                    break;
                case SocialType.Twitter:
                    if (string.IsNullOrEmpty(client.RedirectUri))
                    {
                        
                    }
                    url = "";
                    break;
                case SocialType.Facebook:
                    break;
                case SocialType.Douban:
                    break;
                case SocialType.Net:
                    break;
                case SocialType.Sohu:
                    break;
                default:
                    break;
            }
            return url;
        }

        internal static string GetTokenUrl(SocialType type, ClientInfo client, string code)
        {
            string url = "";
            switch (type)
            {
                case SocialType.Weibo:
                    if (string.IsNullOrEmpty(client.RedirectUri))
                    {
                        client.RedirectUri = "https://api.weibo.com/oauth2/default.html";
                    }
                    url = "https://api.weibo.com/oauth2/access_token?client_id=" + client.ClientId + "&client_secret=" + client.ClientSecret + "&grant_type=authorization_code&redirect_uri=" + client.RedirectUri + "&" + code;
                    break;
                case SocialType.Tencent:
                    if (string.IsNullOrEmpty(client.RedirectUri))
                    {
                        client.RedirectUri = "http://t.qq.com";
                    }
                    url = "https://open.t.qq.com/cgi-bin/oauth2/access_token?client_id=" + client.ClientId + "&client_secret=" + client.ClientSecret + "&redirect_uri=" + client.RedirectUri + "&grant_type=authorization_code&" + code;
                    break;
                case SocialType.Renren:
                    if (string.IsNullOrEmpty(client.RedirectUri))
                    {
                        client.RedirectUri = "http://graph.renren.com/oauth/login_success.html";
                    }
                    url = "https://graph.renren.com/oauth/token?grant_type=authorization_code&client_id=" + client.ClientId + "&redirect_uri=" + client.RedirectUri + "&client_secret=" + client.ClientSecret + "&" + code;
                    break;
                case SocialType.QZone:
                    //QQ空间不需要Code换取token
                    break;
                case SocialType.Douban:
                    break;
                case SocialType.Net:
                    break;
                case SocialType.Sohu:
                    break;
                default:
                    break;
            }
            return url;
        }

        internal static void GetTwitterRequestToken()
        {
            
        }

        internal static void GetToken(SocialType type, ClientInfo client, string code, Action<AccessToken> action)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(GetTokenUrl(type, client, code));
            httpWebRequest.Method = "POST";

            httpWebRequest.BeginGetResponse((p) =>
            {
                HttpWebRequest request = (HttpWebRequest)p.AsyncState;
                HttpWebResponse httpWebResponse;
                try
                {
                    httpWebResponse = (HttpWebResponse)request.EndGetResponse(p);
                }
                catch (WebException ex)
                {
                    return;
                }
                if (httpWebResponse != null)
                {
                    using (var stream = httpWebResponse.GetResponseStream())
                    {
                        AccessToken token = new AccessToken();
                        if (type == SocialType.Tencent)
                        {
                            using (var reader = new System.IO.StreamReader(stream))
                            {
                                string text = reader.ReadToEnd();
                                if (!string.IsNullOrEmpty(text))
                                {
                                    //access_token=ec70e646177f025591e4282946c19b67&expires_in=604800&name=xshf12345
                                    var acc = text.Split('&');
                                    foreach (var item in acc)
                                    {
                                        var single = item.Split('=');
                                        if (single[0] == "access_token")
                                        {
                                            token.Token = single[1];
                                        }
                                        else if (single[0] == "expires_in")
                                        {
                                            token.ExpiresTime = DateTime.Now.AddSeconds(Convert.ToInt32(single[1]));
                                        }
                                        else if (single[0] == "name")
                                        {
                                            token.UserInfo = single[1];
                                        }
                                    }
                                    token.OpenId = client.Tag;
                                }
                            }
                        }
                        else if (type == SocialType.Weibo)
                        {
                            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Weibo.WeiboAccessToken));
                            var item = ser.ReadObject(stream) as Weibo.WeiboAccessToken;
                            item.ExpiresTime = DateTime.Now.AddSeconds(Convert.ToDouble(item.expires_in));
                            token.Token = item.access_token;
                            token.ExpiresTime = item.ExpiresTime;
                            token.UserInfo = item.uid;
                        }
                        else if (type == SocialType.Renren)
                        {
                            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Renren.RenrenAccessToken));
                            var item = ser.ReadObject(stream) as Renren.RenrenAccessToken;
                            item.ExpiresTime = DateTime.Now.AddSeconds(Convert.ToDouble(item.expires_in));
                            token.Token = item.access_token;
                            token.ExpiresTime = item.ExpiresTime;
                            token.UserInfo = item.user.name;
                        }
                        string filePath = string.Format(SocialAPI.ACCESS_TOKEN_PREFIX, type.ToString());
                        JsonHelper.SerializeData<AccessToken>(filePath, token);
                        action(token);                        
                    }
                }
            }, httpWebRequest);
        }
    }
}
