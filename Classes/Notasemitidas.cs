using MySql.Data.MySqlClient;
using System;

namespace nfecreator
{
    class Notasemitidas
    {
        int id;
        string cnpj;
        int modelo;
        string chave;
        DateTime data;
        decimal valor;
        string localxml;


        public int Id { get => id; set => id = value; }
        public string Cnpj { get => cnpj; set => cnpj = value; }
        public int Modelo { get => modelo; set => modelo = value; }
        public string Chave { get => chave; set => chave = value; }
        public DateTime Data { get => data; set => data = value; }
        public decimal Valor { get => valor; set => valor = value; }
        public string Localxml { get => localxml; set => localxml = value; }

        public Notasemitidas() { }

        public Notasemitidas(int ide)
        {
            String sql = @"SELECT * " +
                "FROM  notasemitidas where id =" + ide +
                " ORDER BY id";

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
                            if (reader["fin_id"].ToString().Replace(" ", "") != "")
                            {
                                id = reader.GetInt32("fin_id");
                                cnpj = reader["cnpj"].ToString();
                                chave = reader["chave"].ToString();
                                data = reader.GetDateTime("data");
                                data = reader.GetDateTime("data");
                                valor = reader.GetDecimal("valor");
                                localxml = reader["localxml"].ToString();
                            }
                        }
                    }
                    reader.Close();    // Fecha o DataReader  
                }
                basemysql.Closer();


            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "", false);
                basemysql.Closer();
            }

        }

        public void Insert()
        {

            string sql = @"INSERT INTO notasemitidas (id, cnpj, modelo, chave, data, valor, localxml) " +
               "VALUES (null, @cnpj, @modelo, @chave, @data, @valor, @localxml)";
            try
            {
                MySQLSITE basemysql = new MySQLSITE();
                MySqlCommand cmd = basemysql.connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandTimeout = 1000;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@cnpj", cnpj);
                cmd.Parameters.AddWithValue("@modelo", modelo);
                cmd.Parameters.AddWithValue("@chave", chave.Replace("NFe", ""));
                cmd.Parameters.AddWithValue("@data", data);
                cmd.Parameters.AddWithValue("@valor", valor);
                cmd.Parameters.AddWithValue("@localxml", localxml);

                cmd.ExecuteNonQuery();

                //String instrucao = @"SELECT MAX(id) from nostasemitidas ";
                //string retorno = basemysql.Executacomando(instrucao);
                //id = Convert.ToInt32(retorno);

                //basemysql.Closer();
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "", false);
            }

        }

       

    }
}

