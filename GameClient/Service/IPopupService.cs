using CommunityToolkit.Maui.Views;

namespace GameClient.Service;

public interface IPopupService
{ 
    Task<object> ShowPopup(Popup popup);
}

public class PopupService : IPopupService
{
    public async Task<object> ShowPopup(Popup popup)
    {
        Page page = Application.Current?.MainPage ?? throw new NullReferenceException();
        return await page.ShowPopupAsync(popup);
    }
}