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
    public partial class SocialSendPage : PhoneApplicationPage
    {

        public SocialSendPage()
        {
            InitializeComponent();
            this.ptb_status.Text = App.Statues; 
            this.img.Source = App.ShareImage;
            this.ApplicationTitle.Text = "分享到 " + App.CurrentSocialType.ToString();
        }

        private void Send()
        {
            this.Focus();
            ApplicationBar.IsVisible = false;

            grid.Visibility = System.Windows.Visibility.Visible;
            tbk_busy.Text = "正在发送...";
            if (sb_busy != null)
            {
                sb_busy.Begin();
            }
            SocialAPI.Client = Constants.GetClient(App.CurrentSocialType);
            if (img.Visibility == Visibility.Visible)
            {
                SocialAPI.UploadStatusWithPic(App.CurrentSocialType, ptb_status.Text, Constants.SHARE_IMAGE, (isSuccess, err) =>
                {
                    Deployment.Current.Dispatcher.BeginInvoke(delegate
                    {
                        ApplicationBar.IsVisible = true;
                        grid.Visibility = System.Windows.Visibility.Collapsed;
                        if (isSuccess)
                        {
                            MessageBox.Show("发送成功");
                            if (NavigationService.CanGoBack)
                            {
                                NavigationService.GoBack();
                            }
                        }
                        else
                        {
                            MessageBox.Show("分享失败");
                        }
                    });
                });
            }
            else
            {
                SocialAPI.UpdateStatus(App.CurrentSocialType, ptb_status.Text, (isSuccess, err) =>
                {
                    Deployment.Current.Dispatcher.BeginInvoke(delegate
                    {
                        ApplicationBar.IsVisible = true;
                        grid.Visibility = System.Windows.Visibility.Collapsed;
                        if (isSuccess)
                        {
                            MessageBox.Show("发送成功");
                            if (NavigationService.CanGoBack)
                            {
                                NavigationService.GoBack();
                            }
                        }
                        else
                        {
                            MessageBox.Show("分享失败");
                        }
                    });
                });
            }
           
            
        }

        private void Appbar_Send_Click(object sender, EventArgs e)
        {
            Send();
        }

        private void Image_Tap(object sender, GestureEventArgs e)
        {
            deleteImg.Visibility = Visibility.Collapsed;
            img.Visibility = Visibility.Collapsed;
            border.Visibility = Visibility.Collapsed;
        }

       
    }
}