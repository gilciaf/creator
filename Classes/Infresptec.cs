using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nfecreator
{
    class Infresptec
    {
        int id;
        string cnpj;
        string xContato;
        string email;
        string fone;
        int idCSRT;
        string csrt;

        public int Id { get => id; set => id = value; }
        public string Cnpj { get => cnpj; set => cnpj = value; }
        public string XContato { get => xContato; set => xContato = value; }
        public string Email { get => email; set => email = value; }
        public string Fone { get => fone; set => fone = value; }
        public int IdCSRT { get => idCSRT; set => idCSRT = value; }
        public string Csrt { get => csrt; set => csrt = value; }

        public Infresptec() { }

        public Infresptec(int ide)
        {
            String sql = @"SELECT * " +
                "FROM  infresptec " +
                "where id = " + ide;

            MySQLSITE basemysql = new MySQLSITE();
            try
            {

                using (var command = new MySqlCommand(sql, basemysql.connection))
                {
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader["id"].ToString().Replace(" ", "") != "")
                            {
                                id = reader.GetInt32("id");
                                cnpj = reader["cnpj"].ToString();
                                xContato = reader["xContato"].ToString();
                                email = reader["email"].ToString();
                                fone = reader["fone"].ToString();
                                idCSRT = reader.GetInt32("idCSRT");
                                csrt = reader["csrt"].ToString();

                            }
                        }
                    }
                    reader.Close();    // Fecha o DataReader  
                }
                basemysql.Closer();

            }
            catch (Exception ex)
            {
                basemysql.Closer();
                Funcoes.Crashe(ex, "", false);
            }

        }

        public Infresptec(string uf)
        {
            String sql = @"SELECT * " +
                "FROM  infresptec " +
                "where xuf = '" + uf + "' ";

            MySQLSITE basemysql = new MySQLSITE();
            try
            {

                using (var command = new MySqlCommand(sql, basemysql.connection))
                {
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader["id"].ToString().Replace(" ", "") != "")
                            {
                                id = reader.GetInt32("id");
                                cnpj = reader["cnpj"].ToString();
                                xContato = reader["xContato"].ToString();
                                email = reader["email"].ToString();
                                fone = reader["fone"].ToString();
                                idCSRT = reader.GetInt32("idCSRT");
                                csrt = reader["csrt"].ToString();

                            }
                        }
                    }
                    reader.Close();    // Fecha o DataReader  
                }
                basemysql.Closer();

            }
            catch (Exception ex)
            {
                basemysql.Closer();
                Funcoes.Crashe(ex, "", false);
            }

        }

        public void Update()
        {
            string sql = @"Update infresptec set cnpj = @cnpj, xContato = @xContato, email = @email, fone = @fone, idCSRT = @idCSRT, csrt = @csrt " +
                " WHERE id = @id";
            try
            {
                MySQLSITE basemysql = new MySQLSITE();
                MySqlCommand cmd = basemysql.connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandTimeout = 1000;

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@cnpj", cnpj);
                cmd.Parameters.AddWithValue("@xContato", xContato);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@fone", fone);
                cmd.Parameters.AddWithValue("@idCSRT", idCSRT);
                cmd.Parameters.AddWithValue("@csrt", csrt);

                cmd.ExecuteNonQuery();
                basemysql.Closer();
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "", false);
            }

        }


    }
}