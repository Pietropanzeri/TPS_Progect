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
        string gifResult;
        public PopUpResultController(GameResult result, PopUpResult view)
        {
            switch (result) 
            {
                case GameResult.Vittoria:
                    GifResult = "vittoria.gif";
                    break;
                 case GameResult.Pareggio:
                    GifResult = "pareggio.gif";
                    break;
                case GameResult.Sconfitta:
                    GifResult = "sconfitta.gif";
                    break;
            }
            Close(view);
        }
        public async Task Close(PopUpResult view)
        {
            await Task.Delay(2000);
            await view.CloseAsync();
        }
    }

}
