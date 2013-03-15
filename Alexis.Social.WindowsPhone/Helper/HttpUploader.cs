using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using Alexis.WindowsPhone;
using Alexis.WindowsPhone.Social.Tencent;
using Alexis.WindowsPhone.Social.Renren;
using Alexis.WindowsPhone.Social.Weibo;
using System.Text;

namespace Alexis.WindowsPhone.Social.Helper
{
    public class HttpUploader
    {
        public string url { get; set; }
        public Dictionary<string, object> parameters { get; set; }
        public SocialType Type { get; set; }
        string boundary = "----------" + DateTime.Now.Ticks.ToString();

        public event EventHandler UploadCompleted;

        public void Submit()
        {
            // Prepare web request...
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            myRequest.Method = "POST";
            myRequest.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);

            myRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), myRequest);
        }

        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            Stream postStream = request.EndGetRequestStream(asynchronousResult);

            writeMultipartObject(postStream, parameters);
            postStream.Close();

            request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);
        }

        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                #region parse the content
                #endregion
                try
                {
                    DataContractJsonSerializer ser = null;
                    
                    if (Type == SocialType.Tencent)
                    {
                        //{"data":{"id":"84291041886790","time":1337332120},"errcode":0,"imgurl":"http:\/\/t1.qpic.cn\/mblogpic\/c50c86e2a078ec4dfb9c","msg":"ok","ret":0}
                        ser= new DataContractJsonSerializer(typeof(TResponse));
                        TResponse tr = ser.ReadObject(streamResponse) as TResponse;
                        if (tr.errcode==0)
                        {
                            UploadCompleted.Invoke("ok", null);                            
                        }
                        else
                        {
                            UploadCompleted.Invoke(new Exception(tr.msg), null);
                        }
                    }
                    else if (Type == SocialType.Weibo)
                    {
                        UploadCompleted.Invoke("ok", null);
                    }
                    else if (Type == SocialType.Renren)
                    {
                        //{"uid":222808196,"src_small":"http://fmn.rrfmn.com/fmn058/20120521/1400/p_main_lHP2_25e400002c051261.jpg","caption":"12312312","pid":6036647931,"src":"http://fmn.rrfmn.com/fmn058/20120521/1400/p_head_W7RI_25e400002c051261.jpg","aid":629133235,"src_big":"http://fmn.rrfmn.com/fmn058/20120521/1400/p_large_6uUE_25e400002c051261.jpg"}
                        UploadCompleted.Invoke("ok", null);
                    }
                }
                catch (Exception)
                {
                }               
                streamResponse.Close();
                streamRead.Close();
                // Release the HttpWebResponse
                response.Close();                
            }
            catch (Exception ex)
            {
                UploadCompleted.Invoke(ex, null);
                return;
            }         
        }

        public void writeMultipartObject(Stream stream, object data)
        {
            StreamWriter writer = new StreamWriter(stream);
            if (data != null)
            {
                foreach (var entry in data as Dictionary<string, object>)
                {
                    WriteEntry(writer, entry.Key, entry.Value);
                }
            }
            writer.Write("--");
            writer.Write(boundary);
            writer.WriteLine("--");
            writer.Flush();
        }

        private void WriteEntry(StreamWriter writer, string key, object value)
        {
            if (value != null)
            {
                writer.Write("--");
                writer.WriteLine(boundary);
                if (value is byte[])
                {
                    byte[] ba = value as byte[];

                    writer.WriteLine(@"Content-Disposition: form-data; name=""{0}""; filename=""{1}""", key, "sentPhoto.jpg");
                    writer.WriteLine(@"Content-Type: application/octet-stream");
                    //writer.WriteLine(@"Content-Type: image / jpeg");
                    writer.WriteLine(@"Content-Length: " + ba.Length);
                    writer.WriteLine();
                    writer.Flush();
                    Stream output = writer.BaseStream;

                    output.Write(ba, 0, ba.Length);
                    output.Flush();
                    writer.WriteLine();
                }
                else
                {
                    writer.WriteLine(@"Content-Disposition: form-data; name=""{0}""", key);
                    writer.WriteLine();
                    writer.WriteLine(value.ToString());
                }
            }
        }
    }
    /// <summary>
    /// 发布不带图片的微博
    /// 史坤
    /// 2012.7.4
    /// </summary>
    public class HttpUpdate
    {
        public string url { get; set; }
        public Dictionary<string, object> parameters { get; set; }
        public SocialType Type { get; set; }

        public event EventHandler UploadCompleted;

        public void Submit()
        {
            // Prepare web request...
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded"; 
            myRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), myRequest);
        }

        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            Stream postStream = request.EndGetRequestStream(asynchronousResult);
            string parms="";
            foreach (var item in parameters)
            {
                parms+=item.Key+"="+item.Value.ToString()+"&";
            }
            //string parms = string.Format("access_token={0}&status={1}", parameters["access_token"].ToString(), HttpUtility.UrlEncode(parameters["status"].ToString()));
            byte[] buffer = Encoding.UTF8.GetBytes(parms);

            postStream.Write(buffer,0,buffer.Length);
            postStream.Close();
            
            request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);
        }
        void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                #region parse the content
                #endregion
                try
                {
                    DataContractJsonSerializer ser = null;

                    if (Type == SocialType.Tencent)
                    {
                        //{"data":{"id":"84291041886790","time":1337332120},"errcode":0,"imgurl":"http:\/\/t1.qpic.cn\/mblogpic\/c50c86e2a078ec4dfb9c","msg":"ok","ret":0}
                        ser = new DataContractJsonSerializer(typeof(TResponse));
                        TResponse tr = ser.ReadObject(streamResponse) as TResponse;
                        if (tr.errcode == 0)
                        {
                            UploadCompleted.Invoke("ok", null);
                        }
                        else
                        {
                            UploadCompleted.Invoke(new Exception(tr.msg), null);
                        }
                    }
                    else if (Type == SocialType.Weibo)
                    {
                        UploadCompleted.Invoke("ok", null);
                    }
                    else if (Type == SocialType.Renren)
                    {
                        //{"uid":222808196,"src_small":"http://fmn.rrfmn.com/fmn058/20120521/1400/p_main_lHP2_25e400002c051261.jpg","caption":"12312312","pid":6036647931,"src":"http://fmn.rrfmn.com/fmn058/20120521/1400/p_head_W7RI_25e400002c051261.jpg","aid":629133235,"src_big":"http://fmn.rrfmn.com/fmn058/20120521/1400/p_large_6uUE_25e400002c051261.jpg"}
                        UploadCompleted.Invoke("ok", null);
                    }
                }
                catch (Exception)
                {
                }
                streamResponse.Close();
                streamRead.Close();
                // Release the HttpWebResponse
                response.Close();
            }
            catch (Exception ex)
            {
                UploadCompleted.Invoke(ex, null);
                return;
            }         
        }
    }
}
