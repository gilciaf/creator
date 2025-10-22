using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Linq;

namespace nfecreator
{
    class MySQLSITE
    {
        //String connString;
        public MySqlConnection connection;
        public String erolog;
        MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();

        public string CriarStringConexao()
        {
            builder.Server = "www.ciaf.com.br";
            builder.Database = "tvsistem_site";
            builder.UserID = "tvsistem_site";
            builder.Password = ConfigurationManager.ConnectionStrings["tvsistem_ciafsite"].ConnectionString;
            builder.SslMode = new MySqlSslMode();
            return builder.ToString();
        }

        public MySQLSITE()
        {
            try
            {
                string StringConexaoMysql = CriarStringConexao();
                connection = new MySqlConnection(StringConexaoMysql);
                connection.Open();
            }
            catch (Exception erro)
            {
                var error = erro.ToString();
                if (erro is MySql.Data.MySqlClient.MySqlException)
                {
                    error = (((MySqlException)erro).Message);
                    //    MessageBox.Show(" Error:" + error, " Mysql ERRO! ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Funcoes.Crashe(erro, "", false);
                connection.Close();
                erolog = error;
            }

        }


        public string Executacomando(string instrucao)
        {
            string retorno = "0";
            try
            {
                var obj = connection.Query<string>(instrucao).ToList().FirstOrDefault();
                return obj;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Funcoes.Crashe(ex, "", false);
                return "0";
            }
        }

        public void Closer()
        {
            try
            {
                connection.Close();
                connection.CloseAsync();
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "", false);
            }
        }

        public void AdicionaEntrada(int codigo, string nome, string ip)
        {
            string sql = "INSERT INTO `entradas` (`id`, `codigocliente`, `nomecliente`, `data`, `ip`)" +
                                        " VALUES (NULL, @codigocliente, @nomecliente, @data, @ip)";

            try
            {
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                //cmd.Parameters.AddWithValue("@A", cliente.A"]);
                cmd.CommandTimeout = 1000;

                cmd.Parameters.AddWithValue("@codigocliente", codigo);
                cmd.Parameters.AddWithValue("@nomecliente", nome);
                cmd.Parameters.AddWithValue("@data", DateTime.Now);
                cmd.Parameters.AddWithValue("@ip", ip);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "", false);
            }
        }


        //Clientes
        public String GetValidadeModulo(string cnpj)
        {
            String instrucao = @"SELECT DATE_FORMAT(validademodulo, '%d/%m/%y') from clientes where cnpj_cpf = '" + cnpj + "' order by codigo desc ";
            return Executacomando(instrucao);
        }
        //Clientes
        public String GetValidade(string cnpj)
        {
            String instrucao = @"SELECT DATE_FORMAT(validade, '%d/%m/%y') from clientes where cnpj_cpf = '" + cnpj + "' order by codigo desc ";
            return Executacomando(instrucao);
        }
        //Clientes
        public String GetNomeFantasia(string cnpj)
        {
            String instrucao = @"SELECT nome from clientes where cnpj_cpf = '" + cnpj + "' order by codigo desc ";
            return Executacomando(instrucao);
        }



    }
}
