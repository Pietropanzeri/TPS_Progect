using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
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

        [ObservableProperty]
        string skin_x;
        [ObservableProperty]
        string skin_o;

        public ImpostazioniController(MainPageController page)
        {
            punti = page.CurrentPlayer.Points;
            name = page.CurrentPlayer.UserName;
            Skin_o = "skin_o.png";
            Skin_x = "skin_x.png";


        }
    }
}
