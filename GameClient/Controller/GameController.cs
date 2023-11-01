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
        private SemaphoreSlim mossaLock = new SemaphoreSlim(1);
        
        [ObservableProperty]
        private Game game;
        
        private MainPageController _mainPageController;

        [ObservableProperty] Utente utente0;
        [ObservableProperty] int points0;

        [ObservableProperty] private Utente utente1;
        [ObservableProperty] private int points1;
        
        
        public GameController(Game game, IPopupService popupService)
        {
            _popupService = popupService;
            //impostare player e bot per vedere i nomi

            //trovare come fare per continuare partite e savare numero vittorie

            //assegna side a player che va salvato nel programma
            Game = game;
            _mainPageController = ServiceHelper.GetService<MainPageController>();
            
            StartGame();
        }

        private void StartGame()
        {
            if (Game.IsOnline)
            {
                _mainPageController.SocketController.SocketClient.OnMessage -= OnGameMessage;
                _mainPageController.SocketController.SocketClient.OnMessage += OnGameMessage;
            }

            if (Game.CurrentUser is Bot bot)
            {
                ApplicaMossa(bot.Mossa(Game)).Wait();
            }
            utente0 = game.Players[0];
            utente1 = game.Players[1];

            if (Game.IsOnline)
            {
                Points0 = Game.GamePoints[0];
                Points1 = Game.GamePoints[1];
            }
            
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
                case DataType.Restart:
                    GameSerializer gameSerializer = JsonSerializer.Deserialize<GameSerializer>(data.Data);
                    mossaLock.Wait();
                    Game = Game.FromGameTest(gameSerializer);
                    mossaLock.Release();
                    StartGame();
                    break;
                case DataType.QuitGame:
                    Exit().ContinueWith(result =>
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                            App.Current.MainPage.DisplayAlert("Attenzione", "Sei stato rimosso dal game, siccome un giocatore era inattivo", "Ok")
                        );
                    });
                    break;
            }
        }

        private async Task<bool> ApplicaMossa(Cell cell)
        {
            await mossaLock.WaitAsync();
            try
            {
                Utente user = Game.CurrentUser;

                cell.Content = user.Symbol;

                //TODO: Se e' online dovrebbe fare il server
                (GameResult, string) CheckWin = Game.CheckWin(_mainPageController, user.Symbol);
                if (CheckWin.Item1 == GameResult.Vittoria)
                {
                    Game.WinImage = CheckWin.Item2;

                    if (!Game.IsOnline)
                    {
                        Points0 += 1;
                        Points1 = Math.Max(0, Points1 - 1);
                    }

                    await Task.Delay(300);
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                        await _popupService.ShowPopup(new PopUpResult(GameResult.Vittoria, user.UserName))
                    );

                    if (!Game.IsOnline)
                    {
                        Game = await Game.ResetGame();
                        StartGame();
                    }

                    mossaLock.Release();
                    return false;
                }

                if (CheckWin.Item1 == GameResult.Pareggio)
                {
                    await Task.Delay(300);
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                        await _popupService.ShowPopup(new PopUpResult(GameResult.Pareggio, null)));
                    //await App.Current.MainPage.Navigation.PopAsync();

                    mossaLock.Release();
                    if (!Game.IsOnline)
                    {
                        Game = await Game.ResetGame();
                        StartGame();
                    }
                    return false;
                }

                if (CheckWin.Item1 == GameResult.Sconfitta)
                {
                    Game.WinImage = CheckWin.Item2;

                    if (!Game.IsOnline)
                    {
                        Points0 = Math.Max(0, Points0 - 1);
                        Points1 += 1;
                    }

                    await Task.Delay(300);
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                        await _popupService.ShowPopup(new PopUpResult(GameResult.Sconfitta, null)));

                    mossaLock.Release();
                    if (!Game.IsOnline)
                    {
                        Game = await Game.ResetGame();
                        StartGame();
                    }
                    return false;
                }

                updatePhase();

                mossaLock.Release();
                if (Game.CurrentUser is Bot bot)
                {
                    await Task.Delay(500);
                    await ApplicaMossa(bot.Mossa(Game));
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally { mossaLock.Release(); }

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
            if (Game.IsOnline)
            {
                int points = _mainPageController.CurrentPlayer.Id == Utente0.Id ? Points0 : Points1;
                _mainPageController.CurrentPlayer.Points = points;
            }
            
            await App.Current.MainPage.Navigation.PopAsync();
            _mainPageController.SocketController.SocketClient.OnMessage -= OnGameMessage;
        }

    }
}

