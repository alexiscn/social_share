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
using Alexis.WindowsPhone.Social.Resources;

namespace Alexis.WindowsPhone.Social
{
    public class Lang
    {
        public Lang()
        {
        }

        private static LangResource defaultlanguage = new LangResource();

        public LangResource Language
        {
            get
            {
                return defaultlanguage;
            }
        }

    }
}
