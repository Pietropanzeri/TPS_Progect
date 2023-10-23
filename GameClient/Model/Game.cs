using CommunityToolkit.Mvvm.ComponentModel;
using GameClient.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Model
{
    public partial class Game :ObservableObject
    {
        public ObservableCollection<Cella> Campo { get; set; } = new ObservableCollection<Cella>();
        public Utente[] Players { get; set; }
        
        public Game(Utente[] players)
        {
            this.Players = players;
            
            Campo.Add(new Cella() { Positon = 1 });
            Campo.Add(new Cella() { Positon = 2 });
            Campo.Add(new Cella() { Positon = 3 });
            Campo.Add(new Cella() { Positon = 4 });
            Campo.Add(new Cella() { Positon = 5 });
            Campo.Add(new Cella() { Positon = 6 });
            Campo.Add(new Cella() { Positon = 7 });
            Campo.Add(new Cella() { Positon = 8 });
            Campo.Add(new Cella() { Positon = 9 });
        }

        public bool CheckWin()
        {
            return true;
        }
        
    }
}
