using CommunityToolkit.Mvvm.ComponentModel;

namespace GameClient.model;

public partial class Utente : ObservableObject
{
    public int Id { get; set; }
    public string UserName { get; set; } = "Bot";
    public string Symbol { get; set; }

    public int Points { get; set; }
}