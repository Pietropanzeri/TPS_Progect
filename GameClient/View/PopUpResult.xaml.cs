using CommunityToolkit.Maui.Views;
using GameClient.Controller;

namespace GameClient.View;

public partial class PopUpResult : Popup
{
	public PopUpResult(GameResult result)
	{
		InitializeComponent();
		BindingContext = new PopUpResultController(result, this);
        FlipGif();
	}
    public async Task FlipGif()
    {

            resultgif.IsAnimationPlaying = !resultgif.IsAnimationPlaying;
            resultgif.IsAnimationPlaying = !resultgif.IsAnimationPlaying;
            await Task.Delay(1);
            resultgif.IsAnimationPlaying = !resultgif.IsAnimationPlaying;
            resultgif.IsAnimationPlaying = !resultgif.IsAnimationPlaying;

    }
}