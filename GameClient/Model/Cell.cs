using System.ComponentModel;
using System.Net.Mime;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameClient.Controller;
using GameClient.Helpers;

namespace GameClient.Model
{
    public partial class Cell : ObservableObject
    {
        public int Position { get; set; }

        private ImpostazioniController impostazioni = ServiceHelper.GetService<ImpostazioniController>();

        [JsonIgnore]
        [ObservableProperty]
        public string skin;


        private string content;

        public string Content
        {
            get => content;
            set
            {
                content = value;
                SetSkin();
                OnPropertyChanged();
            }
        }

        [JsonConstructor]
        public Cell(int position, string content)
        {
            this.Position = position;
            this.Content = content;
        }
        public Cell() { }

        public string SetSkin()
        {
            switch (Content)
            {
                case "X":
                    Skin = impostazioni.Skin_x;
                    break;
                case "O":
                    Skin = impostazioni.Skin_o;
                    break;
            }

            return null;
        }
        


    }
}
