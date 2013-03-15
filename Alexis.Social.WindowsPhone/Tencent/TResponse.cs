namespace Alexis.WindowsPhone.Social.Tencent
{
    public class TResponse
    {
        public TInfo data { get; set; }
        public int errcode { get; set; }
        public string imgurl { get; set; }
        public string msg { get; set; }
        public int ret { get; set; }
    }

    public class TInfo
    {
        public string id { get; set; }
        public int time { get; set; }
    }
}
