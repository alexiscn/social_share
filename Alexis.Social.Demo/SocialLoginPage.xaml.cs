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
using Alexis.WindowsPhone.Social;
using Alexis.Social.Demo;

namespace SocialSDKDemo
{
    public partial class SocialLoginPage : PhoneApplicationPage
    {
        private bool _isClearBackStack = false;
        public SocialLoginPage()
        {
            InitializeComponent();
            LoadLoginControl();
        }

        private void LoadLoginControl()
        {
            AuthControl control = new AuthControl();
            var type = App.CurrentSocialType;
            control.SetData(type, Constants.GetClient(type));
            control.action += (p) =>
            {
                if (App.IsLoginGoBack)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(delegate
                    {
                        if (NavigationService.CanGoBack)
                        {
                            NavigationService.GoBack();
                        }
                    });
                }
                else
                {
                    _isClearBackStack = true;
                    Deployment.Current.Dispatcher.BeginInvoke(delegate
                    {
                        NavigationService.Navigate(new Uri("/SocialSendPage.xaml", UriKind.Relative));
                    });
                }
            };
            this.LayoutRoot.Children.Add(control);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (_isClearBackStack)
            {
                if (NavigationService.CanGoBack)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(delegate
                    {
                        NavigationService.RemoveBackEntry();
                    });
                }
            }
            base.OnNavigatedFrom(e);
        }
    }
}