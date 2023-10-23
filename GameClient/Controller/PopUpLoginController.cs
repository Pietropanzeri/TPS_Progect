using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameClient.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Controller
{
    public partial class PopUpLoginController : ObservableObject
    {
        public PopUpLogin popUpLogin;
        public PopUpLoginController(PopUpLogin popUpLogin) 
        {
            this.popUpLogin = popUpLogin;
        }
        [ObservableProperty]
        string username;

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
           popUpLogin.Close();
        }
        [RelayCommand]
        public async Task Ospite()
        {
            //imposta user
            popUpLogin.Close();
        }
    }
}
