using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Views;
using GameClient.model;
using GameClient.Model;
using GameClient.Service;
using GameClient.View;
using GameServer.model;
using Microsoft.VisualBasic;

namespace GameClient.Controller
{
    public partial class MainPageController : ObservableObject
    {
        private IPopupService _popupService;
        private INavigationService _navigationService;

        [ObservableProperty]
        public Player currentPlayer = Player.Create();

        public SocketController SocketController { get; }
        
        
        public MainPageController(IPopupService popupService, INavigationService navigationService)
        {
            _popupService = popupService;
            _navigationService = navigationService;
            SocketController = new SocketController();
        }

        public void Enable()
        {
            SocketController.Start();
            _popupService.ShowPopup(new PopUpLogin());
        }
        
        [RelayCommand]
        public async Task OpenGame()
        {
            SocketController.Send(
                new SocketData(DataType.MatchMaking, CurrentPlayer.UserName, null),
                response =>
                {
                    _navigationService.OpenPage(
                        new GameView(JsonSerializer.Deserialize<Game>(response.Data))
                    );
                });
        }
        [RelayCommand]
        public async Task OpenGameBot()
        {
            bool startSide = (bool)await _popupService.ShowPopup(new PopUpMoneta());
            await _navigationService.OpenPage(new GameView(Game.CreateBotGame(CurrentPlayer, startSide)));
        }
        [RelayCommand]
        public async Task OpenImpostazioni()
        {
            await _navigationService.OpenPage(new Impostazioni());
        }
        [RelayCommand]
        public async Task OpenClassifica()
        {
            await _navigationService.OpenPage(new Classifica());
        }


    }
}
