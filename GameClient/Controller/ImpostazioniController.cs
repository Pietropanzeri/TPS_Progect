﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameClient.model;
using GameClient.Model;
using System.Collections.ObjectModel;

namespace GameClient.Controller
{
    public enum Difficolta
    {
        Facile,
        Medio,
        Impossibile
    }
    public partial class ImpostazioniController : ObservableObject
    {

        [ObservableProperty]
        int punti;
        [ObservableProperty]
        string name;

        public ObservableCollection<HistoryGame> Games { get; set; } = new ObservableCollection<HistoryGame>();

        private bool diffFacile;

        public bool DiffFacile 
        {
            get => diffFacile;
            set 
            {
                diffFacile = value;
                SetDifficolta();
                OnPropertyChanged();
            } 
        }

        private bool diffMedio;

        public bool DiffMedio
        {
            get => diffMedio;
            set
            {
                diffMedio = value;
                SetDifficolta();
                OnPropertyChanged();
            }
        }

        private bool diffImpossibile;

        public bool DiffImpossibile
        {
            get => diffImpossibile;
            set
            {
                diffImpossibile = value;
                SetDifficolta();
                OnPropertyChanged();
            }
        }

        public Difficolta difficoltaBot;

        public Skin Skin { get; set; }

        public ObservableCollection<Skin> Skins { get; set; } = new ObservableCollection<Skin>();

        public static string AppDirectory = Path.Combine("/storage/emulated/0/Documents", "Tris");
        public static string ThemeDirectory = Path.Combine(AppDirectory, "Skins");

        public ImpostazioniController(MainPageController page)
        {
            punti = page.CurrentPlayer.Points;
            name = page.CurrentPlayer.UserName;
            difficoltaBot = Difficolta.Medio;
            DiffMedio = true;
            Skins.Add(new Skin() { O = "skin_o", X = "skin_x" });
            Skins.Add(new Skin() { O = "skin_verde", X = "skin_viola" });
            Skins.Add(new Skin() { O = "quadratoskin", X = "croceskin" });
            SkinManager();
            Games.Add(new HistoryGame { Player1 = new Player { UserName = "pippo" }, Player2 = new Player { UserName = "coca" }, Winner = new Player { UserName = "coca" }, EndTime = DateTime.Now, StartTime = DateTime.Now });
            Games.Add(new HistoryGame { Player1 = new Player { UserName = "pippo" }, Player2 = new Player { UserName = "coca" }, Winner = new Player { UserName = "coca" }, EndTime = DateTime.Now, StartTime = DateTime.Now });
            Games.Add(new HistoryGame { Player1 = new Player { UserName = "pippo" }, Player2 = new Player { UserName = "coca" }, Winner = new Player { UserName = "coca" }, EndTime = DateTime.Now, StartTime = DateTime.Now });
        }
        public void SkinManager()
        {
            DirectoryInfo skinDirectory = new DirectoryInfo(ThemeDirectory);
            if (!skinDirectory.Exists) skinDirectory.Create();
            foreach (DirectoryInfo directory in skinDirectory.GetDirectories())
            {
                 Skins.Add(new Skin(directory));
            }

            Skin = Skins[0];
        }

        [RelayCommand]
        public void SetSkin(Skin skin)
        {
            App.Current.MainPage.DisplayAlert("Skin impostata con successo" ,"","Ok");
            this.Skin = skin;
        }
        public void SetDifficolta()
        {
            if(DiffFacile)
                difficoltaBot = Difficolta.Facile;
            if (DiffMedio)
                difficoltaBot = Difficolta.Medio;
            if (diffImpossibile)
                difficoltaBot = Difficolta.Impossibile;
        }

    }
}
