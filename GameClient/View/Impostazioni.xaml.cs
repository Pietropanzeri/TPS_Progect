using GameClient.Controller;
using GameClient.Helpers;

namespace GameClient.View;

public partial class Impostazioni : ContentPage
{
	public Impostazioni()
	{
		InitializeComponent();
		BindingContext = ServiceHelper.GetService<ImpostazioniController>();
	}
}