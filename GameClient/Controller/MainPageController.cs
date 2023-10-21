using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.VisualBasic;

namespace GameClient.Controller
{
    public partial class MainPageController : ObservableObject
    {
        [RelayCommand]
        public async Task MessageTest()
        {
            await App.Current.MainPage.DisplayAlert("Funge", "1", "ok");
        }
    }
}
