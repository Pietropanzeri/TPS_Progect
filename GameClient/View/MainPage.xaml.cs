using GameClient.Controller;
using CommunityToolkit.Maui.Views;

namespace GameClient.View;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		BindingContext = new MainPageController(this);
        InitializeComponent();
	}
}