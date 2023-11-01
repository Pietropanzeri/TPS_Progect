using CommunityToolkit.Mvvm.ComponentModel;
using GameClient.model;

namespace GameClient.Model;

public partial class HistoryGame : ObservableObject
{
    [ObservableProperty]
    Player player1;

    [ObservableProperty]
    Player player2;

    [ObservableProperty]
    Player winner;



    [ObservableProperty]
    DateTime startTime;

    [ObservableProperty]
    DateTime endTime;
}