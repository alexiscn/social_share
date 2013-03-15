using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Alexis.WindowsPhone.Social
{
    public partial class ShareControl : UserControl
    {
        public Action<SocialType> TypeSelected;
        public ShareControl()
        {
            InitializeComponent();
        }

        private void btnWeibo_Click(object sender, RoutedEventArgs e)
        {
            if (TypeSelected != null)
            {
                TypeSelected(SocialType.Weibo);
            }
        }

        private void btnTencent_Click(object sender, RoutedEventArgs e)
        {
            if (TypeSelected != null)
            {
                TypeSelected(SocialType.Tencent);
            }
        }

        private void btnRenren_Click(object sender, RoutedEventArgs e)
        {
            if (TypeSelected != null)
            {
                TypeSelected(SocialType.Renren);
            }
        }

        private void btnQZone_Click(object sender, RoutedEventArgs e)
        {
            if (TypeSelected != null)
            {
                TypeSelected(SocialType.QZone);
            }
        }
    }
}
