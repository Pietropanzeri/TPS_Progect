using GameClient.Controller;
using GameClient.Helpers;

namespace GameClient.View;

public partial class Impostazioni : ContentPage
{
	public Impostazioni()
	{
		InitializeComponent();

		ImpostazioniController controller = ServiceHelper.GetService<ImpostazioniController>();
		BindingContext = controller;
		controller.Start();

	}
}