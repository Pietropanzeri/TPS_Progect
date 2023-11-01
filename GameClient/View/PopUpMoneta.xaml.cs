using CommunityToolkit.Maui.Views;
using GameClient.Controller;

namespace GameClient.View;

public partial class PopUpMoneta : Popup
{
	public PopUpMoneta(bool  side)
    {
        BindingContext = new PopUpMonetaController(this, side);
        InitializeComponent();
        FlipGif();
	}
    public async Task FlipGif()
    {
        
            coingif.IsAnimationPlaying = !coingif.IsAnimationPlaying;
            coingif.IsAnimationPlaying = !coingif.IsAnimationPlaying;
            await Task.Delay(1);
            coingif.IsAnimationPlaying = !coingif.IsAnimationPlaying;
            coingif.IsAnimationPlaying = !coingif.IsAnimationPlaying;
        
    }
}