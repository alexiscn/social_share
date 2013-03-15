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
using System.Windows.Controls.Primitives;
using Alexis.WindowsPhone.Social;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using Alexis.Social.Demo;

namespace SocialSDKDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        Popup _popUp;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (_popUp!=null && _popUp.IsOpen)
            {
                _popUp.IsOpen = false;
                e.Cancel = true;
                return;
            }
            base.OnBackKeyPress(e);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (_popUp != null && _popUp.IsOpen)
            {
                _popUp.IsOpen = false;
            }
            base.OnNavigatedFrom(e);
        }

        private void btnAuth_Click(object sender, RoutedEventArgs e)
        {
            SnapShot();
            ShowShareControl();
        }

        /// <summary>
        /// popup menu let user to choose
        /// </summary>
        private void ShowShareControl()
        {

            _popUp = new Popup
            {
                Height = 800,
                Width = 480,
            };


            ShareControl sc = new ShareControl();
            sc.Height = 800;
            sc.Width = 480;
            sc.TypeSelected = (p) =>
            {
                DoShare(p);
            };

            _popUp.Child = sc;
            _popUp.IsOpen = true;
        }

        /// <summary>
        /// do share matters
        /// </summary>
        /// <param name="type"></param>
        private void DoShare(SocialType type)
        {
            App.CurrentSocialType = type;
            App.Statues = "share text only for test usage";
            bool isLogin = true;
            switch (type)
            {
                case SocialType.Weibo:
                    if (!(SocialAPI.WeiboAccessToken == null || SocialAPI.WeiboAccessToken.IsExpired))
                    {
                        isLogin = false; 
                    }
                    break;
                case SocialType.Tencent:
                    if (!(SocialAPI.TencentAccessToken == null || SocialAPI.TencentAccessToken.IsExpired))
                    {
                        isLogin = false;
                    }
                    break;
                case SocialType.Renren:
                    if (!(SocialAPI.RenrenAccessToken == null || SocialAPI.RenrenAccessToken.IsExpired))
                    {
                        isLogin = false;
                    }
                    break;
                case SocialType.Douban:
                    break;
                case SocialType.Net:
                    break;
                case SocialType.Sohu:
                    break;
                default:
                    break;
            }
            if (isLogin)
            {
                NavigationService.Navigate(new Uri("/SocialLoginPage.xaml", UriKind.Relative));
            }
            else
            {
                NavigationService.Navigate(new Uri("/SocialSendPage.xaml", UriKind.Relative));
            }
        }

        private void btnAccount_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AccountPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// 微博截图
        /// </summary>
        public static void SnapShot()
        {
            try
            {
                WriteableBitmap bitmap = new WriteableBitmap(480, 800);
                bitmap.Render(App.Current.RootVisual, null);
                bitmap.Invalidate();

                App.ShareImage = bitmap;
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var stream = store.OpenFile(Constants.SHARE_IMAGE, System.IO.FileMode.OpenOrCreate))
                    {
                        try
                        {
                            bitmap.SaveJpeg(stream, 480, 800, 0, 100);
                        }
                        catch (Exception ex)
                        {
                            
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                
            }
        }
    }
}