using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using GameClient.model;
using GameClient.Model;

namespace GameClient.Controller;

public partial class ClassificaController : ObservableObject
{
    public ObservableCollection<TopPlayer> TopPlayers { get; set; } = new ();

    public ClassificaController(List<Player> topPlayers)
    {
        int num = 0;
        topPlayers.Select(player => {
            TopPlayer topp = new TopPlayer(player);
            topp.Position = ++num;
            return topp;
        }).ToList().ForEach(player => this.TopPlayers.Add(player));
    }
}