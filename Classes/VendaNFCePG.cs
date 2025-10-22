using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nfecreator
{
    class VendaNFCePG
    {
        int vendanfepg_id;
        int vendanfe_id;
        int formapg_id;
        string formadescricao;
        int condicaopg_id;
        string condicaodescricao;
        string conveniopg_id;
        string conveniodescricao;
        decimal total_forma;
        DateTime data;
        string autoriza;
        string nDup;
        string descricaodomeiodepagamento;


        public int Vendanfepg_id { get => vendanfepg_id; set => vendanfepg_id = value; }
        public int Vendanfe_id { get => vendanfe_id; set => vendanfe_id = value; }
        public int Formapg_id { get => formapg_id; set => formapg_id = value; }
        public string Formadescricao { get => formadescricao; set => formadescricao = value; }
        public int Condicaopg_id { get => condicaopg_id; set => condicaopg_id = value; }
        public string Condicaodescricao { get => condicaodescricao; set => condicaodescricao = value; }
        public string Conveniopg_id { get => conveniopg_id; set => conveniopg_id = value; }
        public string Conveniodescricao { get => conveniodescricao; set => conveniodescricao = value; }
        public decimal Total_forma { get => total_forma; set => total_forma = value; }
        public DateTime Data { get => data; set => data = value; }
        public string Autoriza { get => autoriza; set => autoriza = value; }
        public string NDup { get => nDup; set => nDup = value; }
        public string Descricaodomeiodepagamento { get => descricaodomeiodepagamento; set => descricaodomeiodepagamento = value; }


        public static bool VerificarDup(int nrvenda)
        {
            try
            {
                bool ret = false;
                DbfBase ebase = new DbfBase();
                //String instrucao = @"SELECT 1 from vendanfepg where (formapg_id = 99 OR formapg_id = 15 ) AND vendanfe_id = " + idvenda;
                string instrucao = @"SELECT 1 from " + ebase.Path + @"\NFEPG.dbf where (formapg_id = 99 OR formapg_id = 15 ) AND nrvenda = " + nrvenda + " ";

                OleDbCommand cmd = new OleDbCommand(instrucao, ebase.Conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                string label1 = ds.Tables[0].Rows[0][0].ToString();
                if (label1 == "1") ret = true;
                return ret;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                //DialogResult result1 = MessageBox.Show("Erro ao adicionar no banco do CIAF informe o suporte. Informe o erro: " + error, "Mensagem do Sistema", MessageBoxButtons.OK);
                return false;
            }
        }



        public List<VendaNFCePG> GetItensdePG(int nrvenda)
        {
            List<VendaNFCePG> lista = new List<VendaNFCePG>();
            VendaNFCePG ivenda;
            try
            {
                DbfBase ebase = new DbfBase();
                string instrucao = @"SELECT * from " + ebase.Path + @"\NFCEPG.dbf WHERE nrvenda = " + nrvenda + " ";

                OleDbCommand cmd = new OleDbCommand(instrucao, ebase.Conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);

                DataSet ds = new DataSet();

                da.Fill(ds);

                DataTable dt = ds.Tables[0];
                ebase.Close();

                foreach (DataRow row in dt.Rows)
                {
                    ivenda = new VendaNFCePG()
                    {
                        //vendanfepg_id = (int)row["vendanfepg_id"],
                        vendanfe_id = Convert.ToInt32(row["nrvenda"].ToString().Trim()),
                        formapg_id = Convert.ToInt32(row["tipopg"].ToString().Trim()),
                        formadescricao = row["tipod"].ToString().Trim(),
                        condicaopg_id = Convert.ToInt32(row["meiopag"].ToString().Trim()),
                        condicaodescricao = row["bandeira"].ToString().Trim(),
                        conveniopg_id = row["bandeira"].ToString().Trim(),
                        conveniodescricao = row["dconvenio"].ToString().Trim(),
                        total_forma = (decimal)row["valor"],
                        //data = (DateTime)row["vencto"],
                        autoriza = row["autoriza"].ToString(),
                        nDup = row["numerop"].ToString().Trim(),
                    };

                  
                    lista.Add(ivenda);
                }
                return lista;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                //DialogResult result1 = MessageBox.Show("Erro ao adicionar no banco do CIAF informe o suporte. Informe o erro: " + error, "Mensagem do Sistema", MessageBoxButtons.OK);
                return lista;
            }

        }






    }



}
