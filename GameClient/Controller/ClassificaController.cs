using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using GameClient.model;

namespace GameClient.Controller;

public partial class ClassificaController : ObservableObject
{
    public ObservableCollection<Player> TopPlayers { get; set; } = new ();

    public ClassificaController(List<Player> topPlayers)
    {
        topPlayers.ForEach(player => this.TopPlayers.Add(player));
    }
}