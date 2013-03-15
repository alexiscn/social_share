using System;
using System.Collections.Generic;

namespace Alexis.WindowsPhone.Social.Renren
{
    public class RenrenUser
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<RenrenAvatar> avatar { get; set; }
    }

    public class RenrenAvatar
    {
        public string type { get; set; }
        public string url { get; set; }
    }
}
