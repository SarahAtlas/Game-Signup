using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classwork___April_16.data
{
    public class EmailSignupsManager
    {
        public string _connectionString;
        public EmailSignupsManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddEmailSignup(EmailSignup ns)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText =
                    "INSERT INTO EmailSignups " +
                    "VALUES (@email, @firstName, @lastName)";
                cmd.Parameters.AddWithValue("@email", ns.Email);
                cmd.Parameters.AddWithValue("@firstName", ns.FirstName);
                cmd.Parameters.AddWithValue("@lastName", ns.LastName);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<EmailSignup> GetEmailSignups()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                List<EmailSignup> result = new List<EmailSignup>();
                cmd.CommandText = "SELECT * FROM EmailSignups";
                connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    EmailSignup es = new EmailSignup
                    {
                        Id = (int)reader["Id"],
                        Email = (string)reader["Email"],
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"]
                    };
                    result.Add(es);
                }
                return result;
            }
        }
    }
}
