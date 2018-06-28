using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classwork___April_16.data
{
    public class EventManager
    {
        public string _connectionString;
        public EventManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddEvent(Event e)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Events " +
                                    "VALUES(@date, @maxPlayers)";
                cmd.Parameters.AddWithValue("@date", e.Date);
                cmd.Parameters.AddWithValue("@maxPlayers", e.MaxPlayers);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public Event GetUpcomingEvent()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT TOP 1 * FROM Events " +
                                   "WHERE Date > @date " + 
                                   "ORDER BY Date";
                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                connection.Open();
                var reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                    return null;
                }
                return new Event
                {
                    Date = (DateTime)reader["Date"],
                    Id = (int)reader["Id"],
                    MaxPlayers = (int)reader["MaxPlayers"]
                };
            };
        }

        public Event GetEventById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM Events " +
                                   "WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                Event e = new Event
                {
                    Id = (int)reader["Id"],
                    Date = (DateTime)reader["Date"],
                    MaxPlayers = (int)reader["MaxPlayers"]
                };
                return e;
            };
        }

        public IEnumerable<Event> GetEvents()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM Events ORDER BY Date DESC";
                connection.Open();
                List<Event> events = new List<Event>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    events.Add(new Event
                    {
                        Id = (int)reader["Id"],
                        Date = (DateTime)reader["Date"],
                        MaxPlayers = (int)reader["MaxPlayers"]
                    });
                }
                return events;
            }

        }

        public EventStatus GetEventStatus(Event e)
        {
            if (e == null || e.Date < DateTime.Today)
            {
                return EventStatus.Past;
            }

            int playerCount = 0;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(*) FROM Players " +
                                  "WHERE EventId = @eventId";
                cmd.Parameters.AddWithValue("@eventId", e.Id);
                connection.Open();
                playerCount = (int)cmd.ExecuteScalar();
            }

            if (playerCount < e.MaxPlayers)
            {
                return EventStatus.Open;
            }

            return EventStatus.Full;
        }

        public IEnumerable<EventPlayerCount> GetEventsWithCount()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                List<EventPlayerCount> result = new List<EventPlayerCount>();
                cmd.CommandText = "GetEventPlayerCount";
                cmd.CommandType = CommandType.StoredProcedure;
                connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Event e = new Event
                    {
                        Id = (int)reader["Id"],
                        Date = (DateTime)reader["Date"],
                        MaxPlayers = (int)reader["MaxPlayers"]
                    };
                    result.Add(new EventPlayerCount
                    {
                        Event = e,
                        PlayerCount = (int)reader["PlayerCount"]
                    });
                }
                return result;
            }


        }

    }
}
