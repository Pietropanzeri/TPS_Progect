using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameClient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Controller
{
    public partial class ImpostazioniController : ObservableObject
    {

        [ObservableProperty]
        int punti;
        [ObservableProperty]
        string name;
        
        public Skin Skin { get; set; }

        public ObservableCollection<Skin> Skins { get; set; } = new ObservableCollection<Skin>();

        public static string AppDirectory = Path.Combine("/storage/emulated/0/Documents", "Tris");
        public static string ThemeDirectory = Path.Combine(AppDirectory, "Skins");

        public ImpostazioniController(MainPageController page)
        {
            punti = page.CurrentPlayer.Points;
            name = page.CurrentPlayer.UserName;
            Skins.Add(new Skin() { O = "skin_o", X = "skin_x" });
            SkinManager();
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
            this.Skin = skin;
        }

    }
}
