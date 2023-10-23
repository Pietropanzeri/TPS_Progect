using CommunityToolkit.Maui.Views;
using GameClient.Controller;

namespace GameClient.View;

public partial class PopUpMoneta : Popup
{
	public PopUpMoneta()
    {
        BindingContext = new PopUpMonetaController(this);
        InitializeComponent();
	}
}