using System;
using System.Threading.Tasks;
using GameClient.Model;
using WebSocketSharp;

namespace GameClient.model;

public class Bot : Utente
{
    private Random _random = new Random();
    
    public Cella Mossa(Game game)
    {
        while (true)
        {
            int pos = _random.Next(0, 9);

            Cella cella = game.Campo[pos];
            if (cella.Content.IsNullOrEmpty()) return cella;
        }
    }
}