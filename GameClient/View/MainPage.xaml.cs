using GameClient.Controller;
using CommunityToolkit.Maui.Views;
using GameClient.Helpers;

namespace GameClient.View;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
        Application.Current.UserAppTheme = AppTheme.Light;

        InitializeComponent();
        
        MainPageController controller = ServiceHelper.GetService<MainPageController>();
        BindingContext = controller;
        
        controller.Enable();
	}
}