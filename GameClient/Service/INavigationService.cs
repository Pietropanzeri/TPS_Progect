namespace GameClient.Service;

public interface INavigationService
{
    Task OpenPage(Page page);
}

public class NavigationService : INavigationService
{
    public async Task OpenPage(Page page)
    {
        await App.Current.MainPage.Navigation.PushAsync(page);
    }
}