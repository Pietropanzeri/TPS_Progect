using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameClient.Helpers;
using GameClient.model;
using GameClient.Model;
using WebSocketSharp;

namespace GameClient.Controller
{
    public partial class GameController : ObservableObject
    {
        [ObservableProperty]
        private Game game;
        
        private MainPageController _mainPageController;

        [ObservableProperty]
        string immagineWin;

        public GameController(Game game)
        {
            //impostare player e bot per vedere i nomi

            //trovare come fare per continuare partite e savare numero vittorie

            //assegna side a player che va salvato nel programma
            Game = game;
            _mainPageController = ServiceHelper.GetService<MainPageController>();

            if (Game.CurrentUser is Bot bot)
            {
                ApplicaMossa(bot.Mossa(game));
            }
        }

        [RelayCommand]
        public async Task Select(Cella cella)
        {
            if (Game.CurrentUser.Id != _mainPageController.CurrentPlayer.Id) return;
            await ApplicaMossa(cella);
        }

        private async Task<bool> ApplicaMossa(Cella cella)
        {
            Utente user = Game.CurrentUser;
            
            if (!cella.Content.IsNullOrEmpty()) return false;
            cella.Content = user.Symbol;

            (bool, string) CheckWin = Game.CheckWin(user.Symbol);
            if (CheckWin.Item1)
            {
                ImmagineWin = CheckWin.Item2;
                await App.Current.MainPage.DisplayAlert("Vittoria", "Ha vinto: " + user.UserName , "OK");
                await App.Current.MainPage.Navigation.PopAsync();
                return false;
            }
            if (Game.CheckDraw())
            {
                await App.Current.MainPage.DisplayAlert("PAREGGIO", "scemo", "OK");
                await App.Current.MainPage.Navigation.PopAsync();
                return false;
            }

            updatePhase();
            
            if (Game.CurrentUser is Bot bot)
            {
                await Task.Delay(500);
                await ApplicaMossa(bot.Mossa(Game));
            }

            return true;
        }

        private bool updatePhase()
        {
            Game.Side = !Game.Side;
            Game.CurrentUser = Game.Side ? Game.Players[0] : Game.Players[1];

            return Game.Side;
        }

    }
}

