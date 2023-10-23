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
        Random random = new Random();

        [ObservableProperty]
        bool side;

        public PopUpMonetaController(PopUpMoneta view)
        {
            side = random.Next(0, 2) == 0;
            Close(view);
        }

        public async Task Close(PopUpMoneta view)
        {
            await Task.Delay(3000);
            await view.CloseAsync(side);
        }
    }
}
