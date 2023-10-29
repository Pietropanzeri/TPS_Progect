using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameClient.Helpers;
using GameClient.model;
using GameClient.Model;
using GameClient.Service;
using GameClient.View;
using GameServer.model;
using WebSocketSharp;
using Cell = GameClient.Model.Cell;

namespace GameClient.Controller
{
    public partial class GameController : ObservableObject
    {
        private IPopupService _popupService;

        [ObservableProperty]
        private Game game;
        
        private MainPageController _mainPageController;

        [ObservableProperty]
        string immagineWin;

        [ObservableProperty]
        Utente utente0;

        [ObservableProperty]
        Utente utente1;
        
        public GameController(Game game, IPopupService popupService)
        {
            _popupService = popupService;
            //impostare player e bot per vedere i nomi

            //trovare come fare per continuare partite e savare numero vittorie

            //assegna side a player che va salvato nel programma
            Game = game;
            _mainPageController = ServiceHelper.GetService<MainPageController>();
            
            if (Game.IsOnline) _mainPageController.SocketController.SocketClient.OnMessage += OnGameMessage;

            if (Game.CurrentUser is Bot bot)
            {
                ApplicaMossa(bot.Mossa(game));
            }
            utente0 = game.Players[0];
            utente1 = game.Players[1];
        }

        [RelayCommand]
        public async Task Select(Cell cell)
        {
            if (Game.CurrentUser.Id != _mainPageController.CurrentPlayer.Id) return;
            if (!cell.Content.IsNullOrEmpty()) return;
            
            if (Game.IsOnline)
            {
                _mainPageController.SocketController.Send(
                    new SocketData(DataType.Move, Game.CurrentUser.UserName, Game.Id + ":" + JsonSerializer.Serialize(cell)),
                    result =>
                    {
                        //Todo: In caso di risposta negativa rimuovi il coso (mossa illegale)
                    });
            }
            await ApplicaMossa(cell);
        }
        
        private void OnGameMessage(object sender, MessageEventArgs e)
        {
            SocketData data = JsonSerializer.Deserialize<SocketData>(e.Data);
            if (data == null) return;

            switch (data.DataType)
            {
                case DataType.Move:
                    Cell cell = JsonSerializer.Deserialize<Cell>(data.Data);
                    ApplicaMossa(Game.GameField[cell.Position]);
                    break;
            }
            
            
        }

        private async Task<bool> ApplicaMossa(Cell cell)
        {
            Utente user = Game.CurrentUser;
            
            cell.Content = user.Symbol;

            //TODO: Se e' online dovrebbe fare il server
            (bool, string) CheckWin = Game.CheckWin(user.Symbol);
            if (CheckWin.Item1)
            {
                ImmagineWin = CheckWin.Item2;
                await _popupService.ShowPopup(new PopUpResult(GameResult.Vittoria, user.UserName));
                await App.Current.MainPage.Navigation.PopAsync();
                return false;
            }
            if (Game.CheckDraw())
            {
                await _popupService.ShowPopup(new PopUpResult(GameResult.Pareggio, null));
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

        [RelayCommand]
        public async Task Exit()
        {
            await App.Current.MainPage.Navigation.PopAsync();
            _mainPageController.SocketController.SocketClient.OnMessage -= OnGameMessage;
        }

    }
}

