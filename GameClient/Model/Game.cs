using CommunityToolkit.Mvvm.ComponentModel;
using GameClient.model;
using System.Collections.ObjectModel;
using WebSocketSharp;

namespace GameClient.Model
{
    public partial class Game :ObservableObject
    {
        public ObservableCollection<Cella> Campo { get; set; } = new ObservableCollection<Cella>();
        public Utente[] Players { get; set; }
        public List<int[]> WinPossibilities { get; set; } = new List<int[]>();
        
        public Game(Utente[] players)
        {
            this.Players = players;

            Campo.Add(new Cella() { Positon = 0 });
            Campo.Add(new Cella() { Positon = 1 });
            Campo.Add(new Cella() { Positon = 2 });
            Campo.Add(new Cella() { Positon = 3 });
            Campo.Add(new Cella() { Positon = 4 });
            Campo.Add(new Cella() { Positon = 5 });
            Campo.Add(new Cella() { Positon = 6 });
            Campo.Add(new Cella() { Positon = 7 });
            Campo.Add(new Cella() { Positon = 8 });


            WinPossibilities.Add(new[] { 0, 1, 2 });
            WinPossibilities.Add(new[] { 3, 4, 5 });
            WinPossibilities.Add(new[] { 6, 7, 8 });
            WinPossibilities.Add(new[] { 0, 3, 6 });
            WinPossibilities.Add(new[] { 1, 4, 7 });
            WinPossibilities.Add(new[] { 2, 5, 8 });
            WinPossibilities.Add(new[] { 0, 4, 8 });
            WinPossibilities.Add(new[] { 2, 4, 6 });
        }

        public bool CheckWin(string symbol)
        {
            var playerIndex = Campo.Where(c => c.Content == symbol).Select(c =>  c.Positon).ToList();
            int n = 0;
            foreach (var item in WinPossibilities)
            {
                foreach (var index in playerIndex)
                {
                    if (item.Contains(index))
                    {
                        n++;
                    }
                    if(n >= 3)
                        return true;
                }
                n = 0;
            }
            return false;
        }
        public bool CheckDraw()
        {
            int n = 0;
            foreach (var item in Campo)
            {
                if (item.Content.IsNullOrEmpty())
                    n++;
            }
            return (n == 0);
        }
        
    }
}
