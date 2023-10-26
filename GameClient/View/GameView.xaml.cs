
using GameClient.Controller;
using GameClient.Model;

namespace GameClient.View;

public partial class GameView : ContentPage
{
	public GameView(Game game)
	{
        InitializeComponent();
        BindingContext = new Controller.GameController(game);
	}
}