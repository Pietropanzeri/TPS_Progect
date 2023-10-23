using GameClient.Controller;

namespace GameClient.View;

public partial class GameView : ContentPage
{
	public GameView(bool bot, bool side)
	{
		if(bot)
            BindingContext = new GameController(bot, side);
        else
            BindingContext = new GameController(bot);
        InitializeComponent();
	}
}