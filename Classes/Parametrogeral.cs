using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nfecreator
{
    class Parametrogeral
    {
        int id;
        string informacao = "";
        string linkinformacao = "";
        string linkimagem = "";
        DateTime validade;

        public int Id { get => id; set => id = value; }
        public string Informacao { get => informacao; set => informacao = value; }
        public string Linkinformacao { get => linkinformacao; set => linkinformacao = value; }
        public DateTime Validade { get => validade; set => validade = value; }
        public string Linkimagem { get => linkimagem; set => linkimagem = value; }

        public Parametrogeral(int id)
        {
            MySqlConnection connection = new MySqlConnection();
            String sql = @"SELECT * FROM parametrogeral where id = " + id;
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
                                informacao = reader["informacao"].ToString();
                                linkinformacao = reader["linkinformacao"].ToString();
                                linkimagem = reader["linkimagem"].ToString();
                                validade =Convert.ToDateTime(reader["validade"].ToString());
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
