using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameClient.View;
using System.Text.Json;
using GameClient.model;
using GameServer.model;

namespace GameClient.Controller
{
    public partial class PopUpLoginController : ObservableObject
    {
        private PopUpLogin _popUpLogin;
        private MainPageController _mainPage;
        private SocketController _socket;

        public PopUpLoginController(PopUpLogin popUpLogin, MainPageController mainPage) 
        {
            this._popUpLogin = popUpLogin;
            this._mainPage = mainPage;
            this._socket = mainPage.SocketController;
        }
        [ObservableProperty]
        string username;

        [ObservableProperty] private string error;

        [RelayCommand]
        public Task Exit()
        {
            App.Current.Quit();
            return Task.CompletedTask;
        }
        [RelayCommand]
        public async Task Enter()
        {
           //salva user e psw
           //Aspettare risposta dal server
           _socket.Send(
               new SocketData(DataType.Connect, Username, null),
               response =>
               {
                   if (response.DataType.Equals(DataType.Error))
                   {
                       Username = null;
                       Error = response.Data;
                       return;
                   }
                   
                   Player player = JsonSerializer.Deserialize<Player>(response.Data);
                   _mainPage.CurrentPlayer = player;

                   _popUpLogin.Close();
               }
            );
        }
        [RelayCommand]
        public async Task Ospite()
        {
            _mainPage.CurrentPlayer = Player.Create();
            _popUpLogin.Close();
        }
    }
}
