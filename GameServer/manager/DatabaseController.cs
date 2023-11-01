using GameClient.Model;
using GameServer.model;
using GameServer.utils;

namespace GameServer.manager;

using MySql.Data.MySqlClient; 

public class DatabaseController
{
    private static readonly string CREATE_USER_TABLE = """
                                                CREATE TABLE IF NOT EXISTS User (
                                                    id int PRIMARY KEY AUTO_INCREMENT,
                                                    username varchar(16) NOT NULL,
                                                    points int NOT NULL DEFAULT 0
                                                );
                                                """;
    private static readonly string CREATE_GAME_TABLE = """
                                                   CREATE TABLE IF NOT EXISTS Game (
                                                       id int PRIMARY KEY AUTO_INCREMENT,
                                                       player1 int NOT NULL,
                                                       player2 int NOT NULL,
                                                       winner int NOT NULL,
                                                       startTime datetime NOT NULL,
                                                       endTime datetime NOT NULL,
                                                       FOREIGN KEY (player1) REFERENCES User(id) ON DELETE CASCADE,
                                                       FOREIGN KEY (player2) REFERENCES User(id) ON DELETE CASCADE
                                                   );
                                                   """;
    
    private static readonly string CONNECTION_STRING = @"server=localhost;userid=root;database=tictactoe";

    private static readonly string PLAYER_EXIST = "SELECT COUNT(*) FROM User WHERE username = @Username";
    private static readonly string CREATE_PLAYER = "INSERT INTO User (username, points) VALUES (@Username, 0)";
    private static readonly string GET_PLAYER = "SELECT * FROM User WHERE username = @Username";
    private static readonly string UPDATE_POINTS = "UPDATE User SET points = @Points WHERE id = @Id";
    private static readonly string GET_PLAYER_TOP = "SELECT * FROM User ORDER BY points DESC LIMIT 100";
    private static readonly string ADD_GAME = @"
                                            INSERT INTO Game (player1, player2, winner, startTime, endTime)
                                            VALUES (@player1Id, @player2Id, @winner, @startTime, @endTime);
                                            ";

    private static readonly string GET_GAME = @"
                                        SELECT 
                                            u1.username AS player1,
                                            u2.username AS player2,
                                            CASE
                                                WHEN g.winner = -1 THEN 'Pareggio'
                                                ELSE u3.username
                                            END AS winner,
                                            g.startTime AS start_time,
                                            g.endTime AS end_time
                                        FROM Game AS g
                                        LEFT JOIN User AS u1 ON g.player1 = u1.id
                                        LEFT JOIN User AS u2 ON g.player2 = u2.id
                                        LEFT JOIN User AS u3 ON g.winner = u3.id
                                        ORDER BY g.startTime DESC
                                        LIMIT 20;
                                        ";
    
    
    private GameController _gameController;

    public DatabaseController(GameController gameController)
    {
        _gameController = gameController;
        Init();
    }

    private void Init()
    {
        ExecuteQuery(conn =>
        {
            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = conn;
                command.CommandText = CREATE_USER_TABLE;

                command.ExecuteNonQuery();
                
                return false;
            }
            catch (Exception ex)
            {
                MessageHelper.Send("Errore nella creazione della tabella user: " + ex.Message, ConsoleColor.Red);
                
                return false;
            }
        }).Wait();
        
        ExecuteQuery(conn =>
        {
            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = conn;
                command.CommandText = CREATE_GAME_TABLE;

                command.ExecuteNonQuery();
                
                return false;
            }
            catch (Exception ex)
            {
                MessageHelper.Send("Errore nella creazione della tabella game: " + ex.Message, ConsoleColor.Red);
                
                return false;
            }
        }).Wait();

        //Task.WaitAll(userTable, gameTable);
    }
    
    public async Task<Player> LoadPlayer(string id, string username)
    {
        if (!await PlayerExists(username)) await CreateUser(username);
        return await GetPlayer(id, username);
    }

    public async Task<List<Player>> GetPlayerTop()
    {
        return await ExecuteQuery<List<Player>>(conn =>
        {
            List<Player> topPlayers = new List<Player>();
            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = conn;
                command.CommandText = GET_PLAYER_TOP;

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("id"));
                        string resultUsername = reader.GetString(reader.GetOrdinal("username"));
                        int points = reader.IsDBNull(reader.GetOrdinal("points"))
                            ? 0
                            : reader.GetInt32(reader.GetOrdinal("points"));

                        topPlayers.Add(
                            new Player { Id = id, UserName = resultUsername, Points = points, Symbol = "X" }
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.Send("Errore durante il retrive della top dell'utente: " + ex.Message, ConsoleColor.Red);
            }

            return topPlayers;
        });
    }
    public async Task<bool> UpdatePoints(int id, int points)
    {
        return await ExecuteQuery(connection =>
        {
            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = UPDATE_POINTS;
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Points", points);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                MessageHelper.Send("Errore durante l'aggiunta di un punto: " + ex.Message, ConsoleColor.Red);
            }

            return false;
        });
    }

    public async Task<bool> AddGame(Game game)
    {
        return await ExecuteQuery(connection =>
        {
            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = ADD_GAME;
                
                command.Parameters.AddWithValue("@player1Id", game.Players[0].Id);
                command.Parameters.AddWithValue("@player2Id", game.Players[1].Id);
                command.Parameters.AddWithValue("@winner", game.CheckDraw() ? -1 : game.CurrentUser.Id);
                
                command.Parameters.AddWithValue("@startTime", game.StartTime);
                command.Parameters.AddWithValue("@endTime", game.EndTime);
                
                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                MessageHelper.Send("Errore durante l'aggiunta di un game: " + ex.Message, ConsoleColor.Red);
                throw;
            }
            return false;
        });
    }

    public async Task<List<HistoryGame>> RetriveGame(Player player)
    {
        return await ExecuteQuery<List<HistoryGame>>(connection =>
        {
            List<HistoryGame> historyGames = new List<HistoryGame>();
            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = GET_GAME;

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string userName1 = reader.GetString(reader.GetOrdinal("player1"));
                        string userName2 = reader.GetString(reader.GetOrdinal("player2"));
                        string winner = reader.GetString(reader.GetOrdinal("winner"));
                        DateTime startTime = reader.GetDateTime(reader.GetOrdinal("start_time"));
                        DateTime endTime = reader.GetDateTime(reader.GetOrdinal("end_time"));

                        historyGames.Add(
                            new HistoryGame {Player1 = userName1, Player2 = userName2, Winner = winner, StartTime = startTime, EndTime = endTime}
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.Send("Errore durante il retrive dei game dell'utente: " + ex.Message, ConsoleColor.Red);
            }

            return historyGames;
        });
    }
    
    private async Task<Player> GetPlayer(string socketId, string username)
    {
        return await ExecuteQuery<Player>(conn =>
        {
            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = conn;
                command.CommandText = GET_PLAYER;
                command.Parameters.AddWithValue("@Username", username);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("id"));
                        string resultUsername = reader.GetString(reader.GetOrdinal("username"));
                        int points = reader.IsDBNull(reader.GetOrdinal("points")) ? 0 : reader.GetInt32(reader.GetOrdinal("points"));

                        return new Player { Id = id, SocketId = socketId, UserName = resultUsername, Points = points, Symbol = "X" };
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageHelper.Send("Errore durante il retrive dell'utente: " + ex.Message, ConsoleColor.Red);
                throw;
            }
        });
    }
    private async Task<bool> CreateUser(string username)
    {
        return await ExecuteQuery(conn =>
        {
            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = conn;
                command.CommandText = CREATE_PLAYER;
                command.Parameters.AddWithValue("@Username", username);
                
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                MessageHelper.Send("Errore nella creazione dell'utente: " + ex.Message, ConsoleColor.Red);
                return false;
            }
        });
    }
    private async Task<bool> PlayerExists(string username)
    {
        return await ExecuteQuery(conn =>
        {
            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = conn;
                command.CommandText = PLAYER_EXIST;
                command.Parameters.AddWithValue("@Username", username);
                
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
            catch (Exception ex)
            {
                MessageHelper.Send("Errore nell controllo se il player esiste: " + ex.Message, ConsoleColor.Red);
                return false;
            }
        });
    }

    private async Task<R> ExecuteQuery<R>(Func<MySqlConnection, R> queryFunc)
    {
        try
        {
            await using (MySqlConnection connection = new MySqlConnection(CONNECTION_STRING))
            {
                await connection.OpenAsync();
                R result = queryFunc.Invoke(connection);
                return result;
            }
        }
        catch (Exception ex)
        {
            MessageHelper.Send("Errore generale \nTipo: " + typeof(R) + "\nErrore: " + ex.Message, ConsoleColor.Red);
            return default;
        }
    }
    
}