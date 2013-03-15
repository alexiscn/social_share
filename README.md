social share lib for windows phone
============

Social sharing for windows phone. Including Weibo, Tencent Weibo, Renren and QQZone and others maybe supported in future.

##Features##
Saving your time sharing things(**text** or **image** or **both text and image**) to social using this sdk. Currently support following social networks:

- [Sina weibo](http://open.weibo.com  "weibo")
- [Tencent Weibo](http://dev.open.t.qq.com "tencent weibo")
- [Renren](http://dev.renren.com/  "renren")
- [QZone-**todo**](http://connect.qq.com "qzone")

Social Share use WebBrowser control to handle oauth2 authorizes and provides some useful  usercontrol like: 

- `AuthControl` to handle oauth2 authorize
- `AccountControl` to give user to manage their accounts
- `ShareControl` to send share text and image

## How to Use? ##
1. download proper dll and add dll reference to your main windows phone project.
2. define global variables in App.xaml.cs file
```C#
	public class App
	{
		/// <summary>
		/// current socail type
		/// </summary>
		public static SocialType CurrentSocialType { get; set; }
		
		/// <summary>
		/// if login from account page, then we should goback
		/// </summary>
		public static bool IsLoginGoBack { get; set; }
		
		/// <summary>
		/// shared text
		/// </summary>
		public static string Statues { get; set; }
		
		/// <summary>
		/// shared image
		/// </summary>
		public static WriteableBitmap ShareImage { get; set; }
	}
```
3. add new Class named Constants, and fill social keys and secrets in this Class
```C#
	public class Constants
	{
	    public const string SHARE_IMAGE = "share.jpg";
	
	    public static ClientInfo GetClient(SocialType type)
	    {
	        ClientInfo client = new ClientInfo();
	        switch (type)
	        {
	            case SocialType.Weibo:
	                client.ClientId = "YOUR_WEIBO_CLIENT_ID";
	                client.ClientSecret = "YOUR_WEIBO_CLIENT_SECRET";
	                //client.RedirectUri = "http://weibo.com/";//if not set,left this property empty
	                break;
	            case SocialType.Tencent:
	                client.ClientId = "";
	                client.ClientSecret = "";
	                break;
	            case SocialType.Renren:
	                client.ClientId = "";
	                client.ClientSecret = "";
	                break;
	            default:
	                break;
	        }
	        return client;
	    }
	}
```
4. add authorize page named `SocialLoginPage.xaml` to place auth control. XAML looks like:
```XAML
	<!--LayoutRoot is the root grid where all page content is placed-->
	<Grid x:Name="LayoutRoot" Background="Transparent">       
	</Grid>
```
5. add following method in `SocialLoginPage.xaml.cs` and call it in page constructor:
```C#
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
```
6. overwrite page `OnNavigatedFrom` event to clear history stack page.
```C#
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
```
7. add send status page named `SocialSendPage.xaml`. This got an applicationbar in page to fire send method.
```C#
	private void Send()
	{
	    this.Focus();
	    ApplicationBar.IsVisible = false;
	
	    grid.Visibility = System.Windows.Visibility.Visible;
	    tbk_busy.Text = "sending...";
	    if (sb_busy != null)
	    {
	        sb_busy.Begin();
	    }
	    SocialAPI.Client = Constants.GetClient(App.CurrentSocialType);
	
	    SocialAPI.UploadStatusWithPic(App.CurrentSocialType, ptb_status.Text, Constants.SHARE_IMAGE, (isSuccess, err) =>
	    {
	        Deployment.Current.Dispatcher.BeginInvoke(delegate
	        {
	            ApplicationBar.IsVisible = true;
	            grid.Visibility = System.Windows.Visibility.Collapsed;
	            if (isSuccess)
	            {
	                MessageBox.Show("success");
	                if (NavigationService.CanGoBack)
	                {
	                    NavigationService.GoBack();
	                }
	            }
	            else
	            {
	                MessageBox.Show("failed");
	            }
	        });
	    });
	}
```
8. If you want your user to manage their social accounts, you can also add an account management page named `AccountPage.xaml`
```C#
	<!--ContentPanel - place additional content here-->
    <Grid x:Name="ContentPanel" Grid.Row="1" Margin="12,0,12,0">
        <social:AccountControl x:Name="accountControl" />
    </Grid>
```
```C#
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
```

You can view full source code in this demo app.


## Contact me ##
If you have any question, please contact me via [Weibo](http://weibo.com/xshf12345) or [Twitter](https://twitter.com/alexis_cn). 