using System;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Reflection;

namespace nfecreator
{
    internal class DbfBase
    {
        OdbcConnection oConn = new OdbcConnection();
        OleDbConnection conn;
       
        string path = @"C:\Ciaf\dados";
        string database = "";
        string uid = "";
        string pwd = "";
        bool servidor = false;
        string sistema = "ciaf";

        public OdbcConnection OConn { get => oConn; set => oConn = value; }
        public OleDbConnection Conn { get => conn; set => conn = value; }
        public string Path { get => path; set => path = value; }
        public bool Servidor { get => servidor; set => servidor = value; }

        public string Sistema { get => sistema; set => sistema = value; }



        public DbfBase()
        {
            try
            {
                Lerarquivo();
                // Funcoes.Mensagem(Path,"", System.Windows.MessageBoxButton.OK);

                servidor = true;
                if (Path == @"\CIAF\DADOS") {
                    Path = @"C:\Ciaf\dados";
                    servidor = false;
                    sistema = "ciaf";
                }
                else if (Path == @"\PetSystem\DADOS")
                {
                    Path = @"C:\Petsystem\dados";
                    servidor = false;
                    sistema = "petsystem";
                }
                else if(Path == @"\CIAF-Automotivo\DADOS")
                {
                    Path = @"C:\CIAF-Automotivo\dados";
                    servidor = false;
                    sistema = "ciafautomotivo";
                }

                string cs = "Provider=VFPOLEDB;Data Source=" + Path + ";Extended Properties=dBase 5.0";

                conn = new OleDbConnection(cs);
                conn.Open();
            }
            catch (Exception ex)
            {
               Funcoes.Crashe(ex, "ATENÇÃO!!! DbfBase: " + Path , true);
            }

        }

        public void Close()
        {
            try
            {
                oConn.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                //Funcoes.Crashe(ex, "ATENÇÃO!!!! DBF", false);
            }

        }

        public String Executacomando(String instrucao)
        {
            try
            {
                string sql = instrucao;
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);

                DataSet ds = new DataSet();

                da.Fill(ds);

                string label1 = ds.Tables[0].Rows[0][0].ToString();

                return label1;
            }
            catch (Exception ex)
            {
                //Funcoes.Crashe(ex, "ATENÇÃO!!!! DBF", false);
                return "0";
            }

        }


        public void ExecutacomandoUpdate(String instrucao)
        {
            try
            {
                string sql = instrucao;
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);

                DataSet ds = new DataSet();

                da.Fill(ds);

            }
            catch (Exception ex)
            {
                //Funcoes.Crashe(ex, "ATENÇÃO!!!! DBF", false);
            }

        }



        public String GetNivelUsuario(string usuario, string senha)
        {
            String instrucao = @"SELECT nivel from " + path + @"\usuarios.dbf where nome ='" + usuario + "' and senha ='" + senha + "'";
            String retorno = Executacomando(instrucao).Replace("  ", "");
            return Executacomando(instrucao).Replace(" ", "");

        }

        //Parametros do Sistema 

        public String GetNomeFantasia()
        {
            String instrucao = @"SELECT medico from " + path + @"\PARAMETROS.dbf";
            return Executacomando(instrucao);

        }

        public String Getrazaosocial()
        {
            String instrucao = @"SELECT razaosocial from " + path + @"\PARAMETROS.dbf";
            return Executacomando(instrucao);

        }
        public String GetCNPJ()
        {
            String instrucao = @"SELECT CNPJ from " + path + @"\PARAMETROS.dbf";
            return Executacomando(instrucao);

        }

        public String GetBairro()
        {
            String instrucao = @"SELECT bairro from " + path + @"\PARAMETROS.dbf";
            return Executacomando(instrucao);

        }


        public void UpdateBairro(string texto)
        {
            String instrucao = @"Update " + path + @"\PARAMETROS.dbf SET bairro = '"+texto+"' ";
            ExecutacomandoUpdate(instrucao);
        }

        public void Lerarquivo()
        {
            try{
                string pasta = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string[] lines = System.IO.File.ReadAllLines(pasta + @"\CONFIG.FPW");

                // Display the file contents by using a foreach loop.
                //System.Console.WriteLine("Contents of WriteLines2.txt = ");
                foreach (string line in lines)
                {
                    // Use a tab to indent each line of the file.
                    // Console.WriteLine("\t" + line);
                    String[] t = line.Split('=');
                    if (t[0].ToUpper().Replace(" ","") == "DEFAULT")
                    {
                        Path = t[1].Replace(" ", "")+ @"\DADOS";
                    }
                    if (t[0].Replace(" ", "") == "DATABASE")
                    {
                        database = t[1].Replace(" ", "");
                    }
                    if (t[0].Replace(" ", "") == "UID")
                    {
                        uid = t[1].Replace(" ", "");
                    }
                    if (t[0].Replace(" ", "") == "PWD")
                    {
                        pwd = t[1].Replace(" ", "");
                    }

                }
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ERRO DO CONFIG: "+ ex.Message , true);
            }

        }

    }


}
 