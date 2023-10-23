using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameClient.model;
using GameClient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Controller
{
    public partial class GameController : ObservableObject
    {
        Random random = new Random();

        [ObservableProperty]
        private Game game;

        [ObservableProperty]
        bool turno;

        [ObservableProperty]
        bool isBot;

        public GameController(bool bot, bool side)
        {
            //assegna side a player che va salvato nel programma
            turno = side;
            game = new Game();
        }
        public GameController(bool bot)
        {

            game = new Game();
        }

        [RelayCommand]
        public async Task Select(Cella cella)
        {
            if (cella.Content == null && Turno)
            {
                //cella.Content = Side;         //sistema quando hai il segno del player
            }
            Turno = !Turno;
            //bisogna ricordarsi di cambiare turno quando arriva messagio nel multilayer
            if (IsBot)
                await MossaBot();

        }

        public async Task MossaBot()
        {
            while (true)
            {
                int i = random.Next(0, 9);
                if (Game.Campo[i] == null)
                {
                    // metti segno opposto al player
                }
            }
            
        }
        
    }
}
