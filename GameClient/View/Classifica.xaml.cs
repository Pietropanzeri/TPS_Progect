using GameClient.Controller;
using GameClient.model;

namespace GameClient.View;

public partial class Classifica : ContentPage
{
	public Classifica(List<Player> topPlayer)
	{
		InitializeComponent();
		BindingContext = new ClassificaController(topPlayer);
	}
}