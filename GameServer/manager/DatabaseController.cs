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
    
    private static readonly string CONNECTION_STRING = @"server=localhost;userid=root;database=TicTacToe";

    private static readonly string PLAYER_EXIST = "SELECT COUNT(*) FROM User WHERE username = @Username";
    private static readonly string CREATE_PLAYER = "INSERT INTO User (username, point) VALUES (@Username, 0)";
    private static readonly string GET_PLAYER = "SELECT * FROM User WHERE username = @Username";
    
    private GameController _gameController;
    private MySqlConnection connection;

    public DatabaseController(GameController gameController)
    {
        _gameController = gameController;
        connection = new MySqlConnection(CONNECTION_STRING);
        connection.Open();
        Init();
    }

    private void Init()
    {
        Task.Run(async () =>
        {
            try
            {
                //using (MySqlConnection connection = await GetConnection())
                //{
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = connection;
                    command.CommandText = CREATE_USER_TABLE;

                    command.ExecuteNonQuery();
                //}
            }
            catch (Exception ex)
            {
                MessageUtils.Send("Errore nella creazione della tabella: " + ex.Message, ConsoleColor.Red);
            }
        });
    }
    
    public async Task<Player> LoadPlayer(string username)
    {
        if (!await PlayerExists(username)) await CreateUser(username);
        return await GetPlayer(username);
    }


    private async Task<Player> GetPlayer(String username)
    {
        try
        {
            //using (MySqlConnection connection = await GetConnection())
            //{
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = GET_PLAYER;
                command.Parameters.AddWithValue("@Username", username);

                await using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("id"));
                        string resultUsername = reader.GetString(reader.GetOrdinal("username"));
                        int points = reader.GetInt32(reader.GetOrdinal("point"));

                        return new Player { Id = id, UserName = resultUsername, Points = points };
                    }

                    return null;
                }
            //}
        }
        catch (Exception ignore)
        {
            return null;
        }
    }
    private async Task<bool> CreateUser(string username)
    {
        try
        {
            //using (MySqlConnection connection = await GetConnection())
            //{
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = CREATE_PLAYER;
                command.Parameters.AddWithValue("@Username", username);

                await connection.OpenAsync();

                int rowsAffected = await command.ExecuteNonQueryAsync();

                return rowsAffected > 0;
            //}
        }
        catch (Exception ex)
        {
            MessageUtils.Send("Errore nella creazione dell'utente: " + ex.Message, ConsoleColor.Red);
            return false;
        }
    }

    private async Task<bool> PlayerExists(String username)
    {
        try
        {
            //using (MySqlConnection connection = await GetConnection())
            //{
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = PLAYER_EXIST;
                command.Parameters.AddWithValue("@Username", username);

                return command.ExecuteNonQuery() > 0;
            //}
        }
        catch (Exception ex)
        {
            MessageUtils.Send("Errore nell controllo se il player esiste: " + ex.Message, ConsoleColor.Red);
            return false;
        }
    }

    private async Task<MySqlConnection> GetConnection()
    {
        return connection;
    }
    
}