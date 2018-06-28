using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classwork___April_16.data
{
    public class PlayerManager
    {
        public string _connectionString;
        public PlayerManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddPlayer(Player player)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Players " +
                                    "VALUES(@firstName, @lastName, @email, @eventId)";
                cmd.Parameters.AddWithValue("@firstName", player.FirstName);
                cmd.Parameters.AddWithValue("@lastName", player.LastName);
                cmd.Parameters.AddWithValue("@email", player.Email);
                cmd.Parameters.AddWithValue("@eventId", player.EventId);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<Player> GetEventPlayers(int eventId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                List<Player> players = new List<Player>();
                cmd.CommandText = "SELECT * FROM Players " +
                                  "WHERE EventId = @eventId";
                cmd.Parameters.AddWithValue("@eventId", eventId);
                connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Player player = new Player
                    {
                        Id = (int)reader["Id"],
                        Email = (string)reader["Email"],
                        EventId = (int)reader["EventId"],
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"]
                    };
                    players.Add(player);
                }

                return players;
            }
        }
    }
}
