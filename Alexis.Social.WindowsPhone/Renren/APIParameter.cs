using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alexis.WindowsPhone.Social.Renren
{
    public class APIParameter
    {
        private string name = null;
        private string value = null;

        public APIParameter(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public string Name
        {
            get { return name; }
        }

        public string Value
        {
            get { return value; }
        }
    }

    /// <summary>
    /// Comparer class used to perform the sorting of the query parameters
    /// </summary>
    public class ParameterComparer : IComparer<APIParameter>
    {
        public int Compare(APIParameter x, APIParameter y)
        {
            if (x.Name == y.Name)
            {
                return string.Compare(x.Value, y.Value);
            }
            else
            {
                return string.Compare(x.Name, y.Name);
            }
        }
    }
}
