using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using GameClient.Controller;

namespace GameClient.View;

public partial class PopUpLogin : Popup
{
	public PopUpLogin()
	{
		BindingContext = new PopUpLoginController(this);
		InitializeComponent();
		
    }
	public async Task ClosePopUp()
	{
		await CloseAsync();
	}
}