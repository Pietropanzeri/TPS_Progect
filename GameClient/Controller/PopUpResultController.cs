using CommunityToolkit.Mvvm.ComponentModel;
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
        Pareggio
    }
    public partial class PopUpResultController : ObservableObject
    {
        [ObservableProperty]
        string result;
        public PopUpResultController(GameResult result, string user)
        {
            switch (result) 
            {
                case GameResult.Vittoria:
                    this.result = "Ha vinto: " + user;
                    break;
                 case GameResult.Pareggio:
                    this.result = "Pareggio";
                    break;
            }
        }
    }
}
