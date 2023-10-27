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
        public ImpostazioniController(MainPageController page)
        {
            punti = page.CurrentPlayer.Points;
        }
    }
}
