using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Model
{
    public partial class Cella : ObservableObject
    {
        public int Positon { get; set; }

        [ObservableProperty]
        public string content;
    }
}
