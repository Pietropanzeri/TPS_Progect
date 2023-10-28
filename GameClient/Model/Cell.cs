using System.ComponentModel;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace GameClient.Model
{
    public partial class Cell : ObservableObject
    {
        public int Position { get; set; }

        [ObservableProperty]
        public string skin;

        [ObservableProperty]
        public string content;

        [JsonConstructor]
        public Cell(int position, string content)
        {
            this.Position = position;
            this.Content = content;
            SetSkin();
        }
        public Cell() { }

        public string SetSkin()
        {
            switch (Content)
            {
                case "X":
                    Skin = "ics.png";
                    break;
                case "O":
                    Skin = "cerchio.png";
                    break;
            }

            return null;
        }
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.PropertyName == nameof(Content))
            {
                SetSkin();
            }
        }


    }
}
