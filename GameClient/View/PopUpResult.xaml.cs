using CommunityToolkit.Maui.Views;
using GameClient.Controller;

namespace GameClient.View;

public partial class PopUpResult : Popup
{
	public PopUpResult(GameResult result, string user)
	{
		InitializeComponent();
		BindingContext = new PopUpResultController(result, user);
	}
}