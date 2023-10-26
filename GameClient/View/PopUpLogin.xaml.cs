using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using GameClient.Controller;
using GameClient.Helpers;

namespace GameClient.View;

public partial class PopUpLogin : Popup
{
	public PopUpLogin()
	{
		BindingContext = new PopUpLoginController(this, ServiceHelper.GetService<MainPageController>());
		InitializeComponent();
		
    }
	
	public async Task ClosePopUp()
	{
		await CloseAsync();
	}
}