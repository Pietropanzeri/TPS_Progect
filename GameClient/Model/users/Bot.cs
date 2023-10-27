using System;
using System.Threading.Tasks;
using GameClient.Model;
using WebSocketSharp;
using Cell = GameClient.Model.Cell;

namespace GameClient.model;

public class Bot : Utente
{
    private Random _random = new Random();
    
    public Cell Mossa(Game game)
    {
        while (true)
        {
            int pos = _random.Next(0, 9);

            Cell cella = game.GameField[pos];
            if (cella.Content.IsNullOrEmpty()) 
                return cella;
        }
    }
}