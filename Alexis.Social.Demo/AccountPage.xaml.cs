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
using Microsoft.Phone.Controls;
using Alexis.Social.Demo;

namespace SocialSDKDemo
{
    public partial class AccountPage : PhoneApplicationPage
    {
        public AccountPage()
        {
            InitializeComponent();
            accountControl.BindAction = ((p) =>
            {
                App.IsLoginGoBack = true;
                App.CurrentSocialType = p;
                NavigationService.Navigate(new Uri("/SocialLoginPage.xaml", UriKind.Relative));
            });
        }
    }
}