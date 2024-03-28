using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.PRACTICAL_WORK_7
{
    public class Logger
    {
        private string connectionString; // Строка подключения к вашей базе данных

        public Logger()
        {
            
        }

        public void Log(string logLevel, string message)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-5ECDJ4N\\SQLEXPRESS;Initial Catalog=DB_CateringEstablishment;Integrated Security=True;"))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("INSERT INTO Log (Timestamp, LogLevel, Message) " +
                    "VALUES (@Timestamp, @LogLevel, @Message)", connection))
                {
                    cmd.Parameters.AddWithValue("@Timestamp", DateTime.Now);
                    cmd.Parameters.AddWithValue("@LogLevel", logLevel);
                    cmd.Parameters.AddWithValue("@Message", message);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
