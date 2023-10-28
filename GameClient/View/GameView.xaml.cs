
using GameClient.Controller;
using GameClient.Helpers;
using GameClient.Model;
using GameClient.Service;

namespace GameClient.View;

public partial class GameView : ContentPage
{
	public GameView(Game game)
	{
        InitializeComponent();
        BindingContext = new Controller.GameController(game, ServiceHelper.GetService<IPopupService>());
	}
}