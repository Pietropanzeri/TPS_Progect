using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using GameClient.model;

namespace GameClient.model
{
    public partial class TopPlayer : Player
    {
        [ObservableProperty]
        private int position;

        public TopPlayer(Player player)
        {
            Id = player.Id;
            UserName = player.UserName;
            Symbol = player.Symbol;
            Points = player.Points;
            SocketId = player.SocketId;
        }
    }
}
