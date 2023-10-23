using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameClient.model;
using GameClient.Model;
using WebSocketSharp;

namespace GameClient.Controller
{
    public partial class GameController : ObservableObject
    {
        Random random = new Random();

        [ObservableProperty]
        private Game game;

        //0 player 1 bot
        int turno;

        [ObservableProperty]
        bool isBot;

        public GameController(MainPageController mainPage, bool bot, bool side)
        {
            //assegna side a player che va salvato nel programma
            turno = side ? 1 : 0;
            game = new Game(
                    new Utente[]
                    {
                        mainPage.CurrentPlayer,
                        new Bot { Id = 10, Symbol = "O"}
                    }
                );

            if (turno == 1)
            {
                Bot userBot = (Bot)Game.Players[turno];
                ApplicaMossa(userBot.Mossa(Game));
            }
        }
        public GameController(MainPageController mainPage, bool bot)
        {
            //game = new Game();
            //Do Server Side
        }

        [RelayCommand]
        public async Task Select(Cella cella)
        {
            if (turno == 1) return;
            await ApplicaMossa(cella);
        }

        private async Task<bool> ApplicaMossa(Cella cella)
        {
            Utente utente = Game.Players[turno];
            if (!cella.Content.IsNullOrEmpty()) return false;
            cella.Content = utente.Symbol;

            if (Game.CheckWin(utente.Symbol))
            {
                await App.Current.MainPage.DisplayAlert("Vittoria", "Ha vinto: " + (turno == 0 ? "Player" : "Bot"), "OK");
                await App.Current.MainPage.Navigation.PopAsync();
                return false;
            }
            if (Game.CheckDraw())
            {
                await App.Current.MainPage.DisplayAlert("PAREGGIO", "scemo", "OK");
                await App.Current.MainPage.Navigation.PopAsync();
                return false;
            }
            turno = turno == 1 ? 0 : 1;
            if (turno == 1)
            {
                Bot bot = (Bot)Game.Players[turno];
                await Task.Delay(500);
                await ApplicaMossa(bot.Mossa(Game));
            }

            return true;
        }

    }
}

