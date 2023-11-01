using CommunityToolkit.Mvvm.ComponentModel;
using GameClient.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Controller
{
    public enum GameResult
    {
        Vittoria,
        Sconfitta,
        Pareggio,
        Ongoing
    }
    public partial class PopUpResultController : ObservableObject
    {
        [ObservableProperty]
        string result;
        [ObservableProperty]
        string gifResult;
        public PopUpResultController(GameResult result, string user, PopUpResult view)
        {
            switch (result) 
            {
                case GameResult.Vittoria:
                    Result = "Ha vinto: " + user;
                    GifResult = "croce.gif";
                    break;
                 case GameResult.Pareggio:
                    Result = "Pareggio";
                    GifResult = "croce.gif";
                    break;
                case GameResult.Sconfitta:
                    Result = "Hai Perso";
                    GifResult = "cerchio.gif";
                    break;
            }
            Close(view);
        }
        public async Task Close(PopUpResult view)
        {
            await Task.Delay(5000);
            await view.CloseAsync();
        }
    }

}
