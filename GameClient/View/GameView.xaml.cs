
using GameClient.Controller;

namespace GameClient.View;

public partial class GameView : ContentPage
{
	public GameView(MainPageController mainPage, bool bot, bool side)
	{
		if(bot)
            BindingContext = new Controller.GameController(mainPage, bot, side);
        else
            BindingContext = new Controller.GameController(mainPage, bot);
        InitializeComponent();
	}
}