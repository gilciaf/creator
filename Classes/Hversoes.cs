using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Reflection;
using DFe.Utils;
using MySql.Data.MySqlClient;

namespace nfecreator
{
    class Hversoes
    {
        int id;
        string versao;
        string url;
        string filename;
        string habilitado;
        string notas;

        string vinco;
        string versaosistema;


        private object dt;

        public int Id { get => id; set => id = value; }
        public string Versao { get => versao; set => versao = value; }
        public string Url { get => url; set => url = value; }
        public string Filename { get => filename; set => filename = value; }
        public string Habilitado { get => habilitado; set => habilitado = value; }
        public string Vinco { get => vinco; set => vinco = value; }
        public string Versaosistema { get => versaosistema; set => versaosistema = value; }
        public string Notas { get => notas; set => notas = value; }

        public Hversoes() { }

        public Hversoes(int ide)
        {
            String sql = @"SELECT * " +
                "FROM  Creatorhversoes " +
                "where id = " + ide;
            MySQLSITE mysql = new MySQLSITE();
            try
            {

                using (var command = new MySqlCommand(sql, mysql.connection))
                {
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader["id"].ToString().Replace(" ", "") != "")
                            {
                                id = reader.GetInt32("id");
                                versao = reader["versao"].ToString();
                                url = reader["url"].ToString();
                                filename = reader["filename"].ToString();
                                habilitado = reader["habilitado"].ToString();
                                notas = reader["notas"].ToString();

                            }
                        }
                    }
                    reader.Close();    // Fecha o DataReader  
                }
                mysql.Closer();

            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "", false);
                mysql.Closer();
            }

        }

        public Hversoes(string versa)
        {
            String sql = @"SELECT * " +
                "FROM  Creatorhversoes " +
                "where versao = '" + versa + "'";

            MySQLSITE mysql = new MySQLSITE();
            try
            {

                using (var command = new MySqlCommand(sql, mysql.connection))
                {
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader["id"].ToString().Replace(" ", "") != "")
                            {
                                id = reader.GetInt32("id");
                                versao = reader["versao"].ToString();
                                url = reader["url"].ToString();
                                filename = reader["filename"].ToString();
                                habilitado = reader["habilitado"].ToString();
                            }
                        }
                    }
                    reader.Close();    // Fecha o DataReader  
                }
                mysql.Closer();

            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "", false);
                mysql.Closer();
            }

        }

        public int Ultimaversao()
        {

            String instrucao = @"SELECT MAX(id) from Creatorhversoes where id > " + id;
            MySQLSITE mysql = new MySQLSITE();
            int retorno = Convert.ToInt32(mysql.Executacomando(instrucao));
            mysql.Closer();
            //if (retorno == null) retorno = 0;
            return retorno;
        }

        public bool ExisteAtualizacao()
        {
            bool flag;
            String instrucao = @"SELECT MAX(id) from Creatorhversoes where id > " + id;
            MySQLSITE mysql = new MySQLSITE();
            int retorno = Convert.ToInt32(mysql.Executacomando(instrucao));
            mysql.Closer();


            if (retorno == 0) flag = false;
            else if (retorno > id)
            {
                Hversoes hver = new Hversoes(retorno);
                if (hver.url.Replace(" ", "") != "") flag = true;
                else flag = false;
            }
            else flag = false;

            return flag;
        }

        public void HversoesSistema()
        {
            try 
            { 
                DbfBase ebase = new DbfBase();
                string instrucao = @"SELECT * from " + ebase.Path + @"\HVERSOES.dbf";
                string sql = instrucao;
                OleDbCommand cmd = new OleDbCommand(sql, ebase.Conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataSet ds = new DataSet();
                ds = new DataSet();
                da.Fill(ds);
           
                DataTable dt = ds.Tables[0];
                foreach (DataRow row in dt.Rows)
                {
                    versaosistema = row["vinco"].ToString().Trim();
                    vinco = row["vinco"].ToString().Trim();
                }
                ebase.Close();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cannot open file"))
                {
                    Funcoes.Crashe(ex, "TABELA ABERTA NO CIAF", true);
                }
                else
                    Funcoes.Crashe(ex, "TABELA ABERTA NO CIAF", true);
            }

        }

        public void AtualizarHversoesSistema(string versaovinco)
        {
            if (versaovinco == "E1610" || versaovinco == "")
            {
                string _path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string ArquivoConfiguracao = @"\configuracao.xml";
                ConfiguracaoApp _configuracoes;
                if (File.Exists(_path + ArquivoConfiguracao)) {
                    _configuracoes = FuncoesXml.ArquivoXmlParaClasse<ConfiguracaoApp>(_path + ArquivoConfiguracao);

                    DbfBase ebase = new DbfBase();
                    if (ebase.Servidor)
                    {
                        _configuracoes.ConfiguracaoDanfeNfe.SalvarServidor = ebase.Servidor;                        
                    }
                    else
                    {
                        _configuracoes.ConfiguracaoDanfeNfe.SalvarServidor = ebase.Servidor;
                    }
                    ebase.Close();
                    _configuracoes.SalvarParaAqruivo(_path + ArquivoConfiguracao);
                }
                   
            }
            UpdateHversao();
        }

        public void UpdateHversao()
        {
            try
            {
                string numeroversao = Assembly.GetExecutingAssembly().GetName().Version.ToString();

                DbfBase ebase = new DbfBase();
                String instrucao = @"Update " + ebase.Path + @"\HVERSOES.dbf SET vinco = '" + numeroversao + @"' ";
              

                OleDbCommand cmd = new OleDbCommand(instrucao, ebase.Conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);

                DataSet ds = new DataSet();

                da.Fill(ds);

                ebase.Close();

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cannot open file"))
                {
                    Funcoes.Crashe(ex, "TABELA ABERTA NO CIAF", true);
                }
                else
                    Funcoes.Crashe(ex, "TABELA ABERTA NO CIAF", false);

            }
        }

    }


}