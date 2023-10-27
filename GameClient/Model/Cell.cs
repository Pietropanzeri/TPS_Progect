using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GameClient.Model
{
    public partial class Cell : ObservableObject
    {
        public int Position { get; set; }

        [ObservableProperty]
        private string content;

        [JsonConstructor]
        public Cell(int position, string content)
        {
            this.Position = position;
            this.Content = content;
        }
        
        public Cell() {}
    }
}
