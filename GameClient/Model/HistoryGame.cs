using CommunityToolkit.Mvvm.ComponentModel;
using GameClient.model;

namespace GameClient.Model;

public partial class HistoryGame : ObservableObject
{
    [ObservableProperty]
    String player1;

    [ObservableProperty]
    String player2;

    [ObservableProperty]
    String winner;



    [ObservableProperty]
    DateTime startTime;

    [ObservableProperty]
    DateTime endTime;
}