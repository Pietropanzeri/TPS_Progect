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

        private ImpostazioniController impostazioniController = ServiceHelper.GetService<ImpostazioniController>();

        [JsonIgnore]
        [ObservableProperty]
        public ImageSource skin;


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
                    Skin = impostazioniController.Skin.X;
                    break;
                case "O":
                    Skin = impostazioniController.Skin.O;
                    break;
            }

            return null;
        }
        


    }
}
