using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nfecreator
{
    class Receber
    {
        string doct;
        int vendanfe;
        int vendanfep;
        DateTime vencto;


        public Receber() { }

        public Receber(int id) {
            try
            {
                DbfBase ebase = new DbfBase();
                string instrucao = @"SELECT * from " + ebase.Path + @"\RECEBER.dbf WHERE nrvenda = " + id + " ";

                OleDbCommand cmd = new OleDbCommand(instrucao, ebase.Conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);

                DataSet ds = new DataSet();

                da.Fill(ds);

                DataTable dt = ds.Tables[0];

                foreach (DataRow row in dt.Rows)
                {
                    doct = row["doct"].ToString().Trim();
                    vendanfep = Convert.ToInt32(row["vendanfep"].ToString());
                    vencto = (DateTime)row["vencto"];
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
                    Funcoes.Crashe(ex, "TABELA ABERTA NO CIAF", false);
            }

        }


        public static DateTime Verificardata(int nrvenda, int numeroop)
        {
            try
            {
                DbfBase ebase = new DbfBase();

                string instrucao = @"SELECT vencto from " + ebase.Path + @"\RECEBER.dbf where nrvendanfe = " + nrvenda + " AND  vendanfep = " + numeroop + " ";

                OleDbCommand cmd = new OleDbCommand(instrucao, ebase.Conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                string label1 = ds.Tables[0].Rows[0][0].ToString();

                return Convert.ToDateTime(label1);
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "TABELA ABERTA NO CIAF", false);
                // Console.WriteLine(e.ToString());
                //DialogResult result1 = MessageBox.Show("Erro ao adicionar no banco do CIAF informe o suporte. Informe o erro: " + error, "Mensagem do Sistema", MessageBoxButtons.OK);

                return Convert.ToDateTime("30/12/1899");
            }
        }


    }
}
