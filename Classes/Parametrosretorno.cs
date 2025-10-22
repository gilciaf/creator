using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nfecreator
{
    class Parametrosretorno
    {
        int id;
        string sendwhatsapp = "";

        public int Id { get => id; set => id = value; }
        public string Sendwhatsapp { get => sendwhatsapp; set => sendwhatsapp = value; }

        public Parametrosretorno(int id)
        {
            MySqlConnection connection = new MySqlConnection();
            String sql = @"SELECT * FROM parametrosretorno where id = " + id;
            try
            {
                var builder = new MySqlConnectionStringBuilder();
                    builder.Server = @"www.ciaf.com.br";
                    builder.UserID = @"tvsistem_fabio";
                    builder.Password = ConfigurationManager.ConnectionStrings["tvsistem_ciaf"].ConnectionString;
                    builder.Database = @"tvsistem_ciafcsharp";
                    builder.SslMode = new MySqlSslMode();
                    connection = new MySqlConnection(builder.ToString());
                    connection.Open();

                using (var command = new MySqlCommand(sql, connection))
                {
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader["id"].ToString().Replace(" ", "") != "")
                            {
                                id = reader.GetInt32("id");
                                sendwhatsapp = reader["sendwhatsapp"].ToString();
                               
                            }
                        }
                    }
                }
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                Console.WriteLine(e.ToString());
            }
        }


    }
}
