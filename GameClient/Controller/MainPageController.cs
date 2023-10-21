using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Views;
using GameClient.View;
using Microsoft.VisualBasic;

namespace GameClient.Controller
{
    public partial class MainPageController : ObservableObject
    {
        public MainPageController(MainPage mainPage)
        {
            mainPage.ShowPopup(new PopUpLogin());
        }
        [RelayCommand]
        public async Task OpenGame()
        {
            await App.Current.MainPage.Navigation.PushAsync(new GameView(false));
        }
        [RelayCommand]
        public async Task OpenGameBot()
        {
            await App.Current.MainPage.Navigation.PushAsync(new GameView(true));
        }
        [RelayCommand]
        public async Task OpenImpostazioni()
        {
            await App.Current.MainPage.Navigation.PushAsync(new Impostazioni());
        }
        [RelayCommand]
        public async Task OpenClassifica()
        {
            await App.Current.MainPage.Navigation.PushAsync(new Classifica());
        }


    }
}
