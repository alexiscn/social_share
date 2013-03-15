using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alexis.WindowsPhone.Social.Helper
{
    public class QueryStringHelper
    {
        /// <summary>
        /// 不区分大小写,获得querysring中的值
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetQueryString(Uri url, string key)
        {
            string retVal = "";
            string query = "";
            string abUrl = url.Fragment;

            if (abUrl != "")
            {
                abUrl = Uri.UnescapeDataString(abUrl);
                query = abUrl.Replace("#", "");
            }
            else
            {
                abUrl = url.AbsoluteUri;
                abUrl = Uri.UnescapeDataString(abUrl);
                query = abUrl.Substring(abUrl.IndexOf("?") + 1);
                query = query.Replace("?", "");
            }

            string[] querys = query.Split('&');
            foreach (string qu in querys)
            {
                string[] vals = qu.Split('=');
                if (vals[0].ToString().ToLower() == key.ToLower())
                {
                    retVal = vals[1].ToString();
                    break;
                }
            }
            return retVal;
        }
    }
}
