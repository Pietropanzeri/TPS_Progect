using CommunityToolkit.Mvvm.ComponentModel;
using GameClient.model;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using CommunityToolkit.Maui.Core.Extensions;
using GameClient.Helpers;
using WebSocketSharp;
using GameClient.Controller;

namespace GameClient.Model
{
    public partial class Game :ObservableObject
    {
        public string Id { get; set; }
        
        public ObservableCollection<Cell> GameField { get; set; } = new ();
        public Utente[] Players { get; set; }
        
        public int[] GamePoints { get; set; }
        
        [JsonIgnore]
        [ObservableProperty] 
        public bool side;

        [JsonIgnore]
        public Utente CurrentUser { get; set; }
        
        [JsonIgnore]
        [ObservableProperty]
        ImageSource winImage;
        
        [JsonIgnore]
        public List<int[]> WinPossibilities { get; set; } = new List<int[]>();
        
        [JsonIgnore]
        public List<string> WinImages { get; set; } = new List<string>();

        public bool IsOnline { get; set; }

        [JsonConstructor]
        public Game(string id, Player[] players, int[] gamePoints, List<Cell> gameField, bool side) : this(players.ToArray(), side)
        {
            Id = id;
            GamePoints = gamePoints;
            GameField = gameField.ToObservableCollection();
            IsOnline = true;
        }
        
        public Game(Utente[] players, bool startSide)
        {
            Players = players;
            Side = startSide;
            WinImage = "vuoto.png";

            if (Side)
            {
                CurrentUser = Players[0];
                Players[0].Symbol = "X";
                Players[1].Symbol = "O";
            }
            else
            {
                CurrentUser = Players[1];
                Players[0].Symbol = "O";
                Players[1].Symbol = "X";
            }
            
            if (GameField.Count == 0) InitializeField();

            WinPossibilities.Add(new[] { 0, 1, 2 });
            WinPossibilities.Add(new[] { 3, 4, 5 });
            WinPossibilities.Add(new[] { 6, 7, 8 });
            WinPossibilities.Add(new[] { 0, 3, 6 });
            WinPossibilities.Add(new[] { 1, 4, 7 });
            WinPossibilities.Add(new[] { 2, 5, 8 });
            WinPossibilities.Add(new[] { 0, 4, 8 });
            WinPossibilities.Add(new[] { 2, 4, 6 });

            WinImages.Add("uno.png");
            WinImages.Add("due.png");
            WinImages.Add("tre.png");
            WinImages.Add("quattro.png");
            WinImages.Add("cinque.png");
            WinImages.Add("sei.png");
            WinImages.Add("sette.png");
            WinImages.Add("otto.png");
        }

        private void InitializeField()
        {
            GameField.Add(new Cell() { Position = 0 });
            GameField.Add(new Cell() { Position = 1 });
            GameField.Add(new Cell() { Position = 2 });
            GameField.Add(new Cell() { Position = 3 });
            GameField.Add(new Cell() { Position = 4 });
            GameField.Add(new Cell() { Position = 5 });
            GameField.Add(new Cell() { Position = 6 });
            GameField.Add(new Cell() { Position = 7 });
            GameField.Add(new Cell() { Position = 8 });
        }

        public static Game CreateBotGame(Player player , bool startSide)
        {
            return new Game(new Utente[]
            {
                player,
                new Bot() { Id = 'O' }
            }, startSide);
        }

        public (GameResult,string) CheckWin(MainPageController _mainPage, string symbol)
        {
            var playerIndex = GameField.Where(c => c.Content == symbol).Select(c =>  c.Position).ToList();
            int n = 0;
            for (int i = 0; i < WinPossibilities.Count; i++)
            {
                foreach (var index in playerIndex)
                {
                    if (WinPossibilities[i].Contains(index))
                    {
                        n++;
                    }
                    if (n >= 3)
                    {
                        (GameResult, string) risultato = (_mainPage.CurrentPlayer.Id == CurrentUser.Id ? GameResult.Vittoria : GameResult.Sconfitta, WinImages[i]);
                        return risultato;
                    }
                }
                n = 0;
            }
            n = 0;
            foreach (var item in GameField)
            {
                if (item.Content.IsNullOrEmpty())
                    n++;
            }
            return(GameResult.Ongoing, null);

        }

        public Game ResetGame()
        {
            return new Game(Players, RandomHelper.RandomBool());
        }
        public static Game FromGameTest(GameSerializer gameSerializer)
        {
            return new Game(gameSerializer.Id, gameSerializer.Players, gameSerializer.GamePoints, gameSerializer.GameField, gameSerializer.Side);
        }
        
    }
}
