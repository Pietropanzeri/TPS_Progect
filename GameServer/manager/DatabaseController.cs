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
                                                    point int NOT NULL DEFAULT 0
                                                );
                                                """;
    
    private static readonly string CONNECTION_STRING = @"server=localhost;userid=root;database=tictactoe";

    private static readonly string PLAYER_EXIST = "SELECT * FROM User WHERE username = @Username";
    private static readonly string CREATE_PLAYER = "INSERT INTO User (username, point) VALUES (@Username, 0)";
    private static readonly string GET_PLAYER = "SELECT * FROM User WHERE username = @Username";
    
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
                MessageUtils.Send("Errore nella creazione della tabella: " + ex.Message, ConsoleColor.Red);
                
                return false;
            }
        }).Wait();
    }
    
    public async Task<Player> LoadPlayer(string username)
    {
        if (!await PlayerExists(username)) await CreateUser(username);
        return await GetPlayer(username);
    }


    private async Task<Player> GetPlayer(string username)
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
                        int points = reader.IsDBNull(reader.GetOrdinal("point")) ? 0 : reader.GetInt32(reader.GetOrdinal("point"));

                        return new Player { Id = id, UserName = resultUsername, Points = points };
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageUtils.Send("Errore durante il retrive dell'utente: " + ex.Message, ConsoleColor.Red);
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
                MessageUtils.Send("Errore nella creazione dell'utente: " + ex.Message, ConsoleColor.Red);
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

                return command.ExecuteNonQuery() != 0;
            }
            catch (Exception ex)
            {
                MessageUtils.Send("Errore nell controllo se il player esiste: " + ex.Message, ConsoleColor.Red);
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
            MessageUtils.Send("Errore generale \nTipo: " + typeof(R) + "\nErrore: " + ex.Message, ConsoleColor.Red);
            return default;
        }
    }
    
}