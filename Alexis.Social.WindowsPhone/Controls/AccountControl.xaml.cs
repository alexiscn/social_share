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
using System.ComponentModel;
using Alexis.WindowsPhone.Social.Resources;

namespace Alexis.WindowsPhone.Social
{
    public partial class AccountControl : UserControl
    {
        public Action<SocialType> BindAction;
        public AccountControl()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(AccountControl_Loaded);
        }       

        void AccountControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetContent();
        }

        private void SetContent()
        {
            // Avoid handing off to the worker thread (can cause problems for design tools)
            if (DesignerProperties.IsInDesignTool)
            {
                return;
            }
            
            btnWeibo.Content = SocialAPI.WeiboAccessToken == null ? LangResource.BindWeibo: LangResource.UnBind;
            tbkWeibo.Text = SocialAPI.WeiboAccessToken == null ? LangResource.UnBinded : LangResource.Binded;

            btnTencent.Content = SocialAPI.TencentAccessToken == null ? LangResource.BindTencent: LangResource.UnBind;
            tbkTencent.Text = SocialAPI.TencentAccessToken == null ? LangResource.UnBinded : LangResource.Binded;

            btnRenren.Content = SocialAPI.RenrenAccessToken == null ? LangResource.BindRenren : LangResource.UnBind;
            tbkRenren.Text = SocialAPI.RenrenAccessToken == null ? LangResource.UnBinded : LangResource.Binded;

            btnQZone.Content = SocialAPI.QZoneAccessToken == null ? LangResource.BindQzone : LangResource.UnBind;
            tbkQzone.Text = SocialAPI.QZoneAccessToken == null ? LangResource.UnBind : LangResource.Binded;
        }

        private void btnWeibo_Click(object sender, RoutedEventArgs e)
        {
            if (tbkWeibo.Text == LangResource.UnBinded)
            {
                BindAction(SocialType.Weibo);
            }
            else
            {
                SocialAPI.LogOff(SocialType.Weibo);
                SetContent();
            }
        }

        private void btnTencent_Click(object sender, RoutedEventArgs e)
        {
            if (tbkTencent.Text == LangResource.UnBinded)
            {
                BindAction(SocialType.Tencent);
            }
            else
            {
                SocialAPI.LogOff(SocialType.Tencent);
                SetContent();
            }
        }

        private void btnRenren_Click(object sender, RoutedEventArgs e)
        {
            if (tbkRenren.Text == LangResource.UnBinded)
            {
                BindAction(SocialType.Renren);
            }
            else
            {
                SocialAPI.LogOff(SocialType.Renren);
                SetContent();
            }
        }

        private void btnQZone_Click(object sender, RoutedEventArgs e)
        {
            if (tbkRenren.Text== LangResource.UnBinded)
            {
                BindAction(SocialType.QZone);
            }
            else
            {
                SocialAPI.LogOff(SocialType.QZone);
                SetContent();
            }
        }
    }
}
