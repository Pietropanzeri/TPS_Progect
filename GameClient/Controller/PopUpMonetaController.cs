using CommunityToolkit.Mvvm.ComponentModel;
using GameClient.View;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Controller
{
    public partial class PopUpMonetaController :ObservableObject
    {

        [ObservableProperty]
        string side;

        public PopUpMonetaController(PopUpMoneta view, bool side)
        {
            if (side)
                this.side = "croce.gif";
            else
                this.side = "cerchio.gif";
            Close(view);
        }

        public async Task Close(PopUpMoneta view)
        {
            await Task.Delay(5000);
            await view.CloseAsync();
        }
    }
}
