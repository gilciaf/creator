using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nfecreator
{
    class VendaNFePG
    {
        int vendanfepg_id;
        int vendanfe_id;
        int formapg_id;
        string formadescricao;
        int condicaopg_id;
        string condicaodescricao;
        int conveniopg_id;
        string conveniodescricao;
        decimal total_forma;
        DateTime data;
        string autoriza;
        int nDup;
        string descricaodomeiodepagamento;

        public int Vendanfepg_id { get => vendanfepg_id; set => vendanfepg_id = value; }
        public int Vendanfe_id { get => vendanfe_id; set => vendanfe_id = value; }
        public int Formapg_id { get => formapg_id; set => formapg_id = value; }
        public string Formadescricao { get => formadescricao; set => formadescricao = value; }
        public int Condicaopg_id { get => condicaopg_id; set => condicaopg_id = value; }
        public string Condicaodescricao { get => condicaodescricao; set => condicaodescricao = value; }
        public int Conveniopg_id { get => conveniopg_id; set => conveniopg_id = value; }
        public string Conveniodescricao { get => conveniodescricao; set => conveniodescricao = value; }
        public decimal Total_forma { get => total_forma; set => total_forma = value; }
        public DateTime Data { get => data; set => data = value; }
        public string Autoriza { get => autoriza; set => autoriza = value; }
        public int NDup { get => nDup; set => nDup = value; }
        public string Descricaodomeiodepagamento { get => descricaodomeiodepagamento; set => descricaodomeiodepagamento = value; }


        public static bool VerificarDup(int nrvenda)
        {
            try
            {
                bool ret = false;
            DbfBase ebase = new DbfBase();
                //String instrucao = @"SELECT 1 from vendanfepg where (tipopg = '99' OR tipopg = '15' ) AND vendanfe_id = " + idvenda;
                string instrucao = @"SELECT 1 from " + ebase.Path + @"\NFEPG.dbf where (tipopg = '99' OR tipopg = '15' ) AND nrvenda = " + nrvenda + " ";

                OleDbCommand cmd = new OleDbCommand(instrucao, ebase.Conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                string label1 = "0";
                if (ds.Tables[0].Rows.Count > 0)
                 label1 = ds.Tables[0].Rows[0][0].ToString();
                if (label1 == "1") ret = true;
                return ret;
            }
            catch (Exception ex)
            {
              return false;
            }
        }



        public List<VendaNFePG> GetItensdePG(int nrvenda)
        {
            List<VendaNFePG> lista = new List<VendaNFePG>();
            VendaNFePG ivenda;
            try
            {
                DbfBase ebase = new DbfBase();
                string instrucao = @"SELECT * from " + ebase.Path + @"\NFEPG.dbf WHERE nrvenda = " + nrvenda + " ";

                OleDbCommand cmd = new OleDbCommand(instrucao, ebase.Conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);

                DataSet ds = new DataSet();

                da.Fill(ds);

                DataTable dt = ds.Tables[0];
                ebase.Close();

                foreach (DataRow row in dt.Rows)
                {

                     ivenda = new VendaNFePG()
                    {
                        //vendanfepg_id = (int)row["vendanfepg_id"],
                        vendanfe_id = Convert.ToInt32(row["nrvenda"].ToString().Trim()),
                        formapg_id = Convert.ToInt32(row["tipopg"].ToString().Trim()),
                        formadescricao = row["tipod"].ToString().Trim(),
                        condicaopg_id = Convert.ToInt32(row["meiopag"].ToString().Trim()),
                        condicaodescricao = row["bandeira"].ToString().Trim(),
                        
                        conveniodescricao = row["dconvenio"].ToString().Trim(),
                        total_forma = (decimal)row["valor"],

                         Data = (DateTime)row["vencto"],

                        //autoriza = row["autoriza"].ToString().Trim(),
                        nDup = Convert.ToInt32(row["numerop"].ToString().Trim()),
                    };


                    
                    if (ivenda.Data == Convert.ToDateTime("30/12/1899"))
                    {
                        ivenda.data = Receber.Verificardata(ivenda.vendanfe_id, ivenda.NDup);
                        if(ivenda.data != Convert.ToDateTime("30/12/1899"))
                        {
                            InsertData(ivenda.vendanfe_id, Convert.ToInt32(ivenda.nDup), ivenda.data.ToString("yyyy/MM/dd"));
                        }

                    }
                      


                    if (row["bandeira"].ToString().Trim() != "") ivenda.conveniopg_id = Convert.ToInt32(row["bandeira"].ToString().Trim());

                    lista.Add(ivenda);
                }
                return lista;
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "TABELA ABERTA NO CIAF", false);
                //Console.WriteLine(e.ToString());
                //DialogResult result1 = MessageBox.Show("Erro ao adicionar no banco do CIAF informe o suporte. Informe o erro: " + error, "Mensagem do Sistema", MessageBoxButtons.OK);
                return lista;
            }

        }






        public static void InsertData(int nrvenda, int numerop, string data)
        {
            try
            {
                DbfBase ebase = new DbfBase();
                String instrucao = @"Update " + ebase.Path + @"\NFEPG.dbf SET vencto = {^" + data + "} " +
                " where nrvenda = " + nrvenda + " AND numerop = " + numerop + " ";

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
