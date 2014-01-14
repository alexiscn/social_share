using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Alexis.WindowsPhone.Social.Twitter
{
    /// <summary>
    /// OAuth Helper for Twitter
    /// by alexiscn
    /// </summary>
    public class TwitterOauthClient
    {
        internal static void GetRequestToken(Dictionary<string, string> parameters, string url, string comsumeKey, string tokenSecret, Action<string> getTokenCallback)
        {
            string oathHeader = GetOAuthHeader(parameters, url, comsumeKey, tokenSecret);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Headers["Authorization"] = oathHeader;

            request.BeginGetResponse((ar) =>
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)ar.AsyncState;
                HttpWebResponse httpWebResponse = (HttpWebResponse)request.EndGetResponse(ar);
                if (httpWebResponse != null)
                {
                    using (var reader = new StreamReader(httpWebResponse.GetResponseStream()))
                    {
                        string text = reader.ReadToEnd();
                        if (!string.IsNullOrEmpty(text))
                        {
                            string[] data = text.Split(new char[] { '&' });
                            int index = data[0].IndexOf("=");
                            string token = data[0].Substring(index + 1, data[0].Length - 1);
                            getTokenCallback(token);
                        }
                        else
                        {
                            getTokenCallback(null);
                        }
                    }
                }
                else
                {
                    getTokenCallback(null);
                }
            }, request);
        }

        internal static string GetOAuthHeader(Dictionary<string, string> parameters, string url, string comsumeSercret, string tokenSecret)
        {
            parameters = parameters.OrderBy(x => x.Key).ToDictionary(v => v.Key, v => v.Value);

            string concat = string.Empty;

            string OAuthHeader = "OAuth ";
            foreach (var key in parameters.Keys)
            {
                concat += key + "=" + parameters[key] + "&";
                OAuthHeader += key + "=" + "\"" + parameters[key] + "\",";
            }

            concat = concat.Remove(concat.Length - 1, 1);
            concat = EncodeToUpper(concat);

            concat = "POST&" + EncodeToUpper(url) + "&" + concat;

            byte[] content = Encoding.UTF8.GetBytes(concat);

            HMACSHA1 hmac = new HMACSHA1(Encoding.UTF8.GetBytes(comsumeSercret + "&" + tokenSecret));
            hmac.ComputeHash(content);

            string hash = Convert.ToBase64String(hmac.Hash);

            hash = hash.Replace("-", "");

            OAuthHeader += "oauth_signature=\"" + EncodeToUpper(hash) + "\"";

            return OAuthHeader;
        }

        internal static string EncodeToUpper(string raw)
        {
            raw = HttpUtility.UrlEncode(raw);
            return Regex.Replace(raw, "(%[0-9a-f][0-9a-f])", c => c.Value.ToUpper());
        }
    }
}
