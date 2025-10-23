using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nfecreator
{
    class Cartanfe
    {
        int cartanfe_id;
        int nrcarta;
        int nrseq;
        int nrnfe;
        int serie;
        string texto;
        string estado;
        int codigouf;
        string cnpjcpf;
        string chavea;
        string protocolo;
        DateTime data;
        string status;

        public Cartanfe() { }

        public Cartanfe(int carta)
        {
            //String sql = @"SELECT * FROM cartanfe WHERE cartanfe_id = " + id + " ";
            try
            {
                DbfBase ebase = new DbfBase();
                string instrucao = @"SELECT * from " + ebase.Path + @"\CARTANFE.dbf WHERE nrcarta = " + carta + " ";

                OleDbCommand cmd = new OleDbCommand(instrucao, ebase.Conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);

                DataSet ds = new DataSet();

                da.Fill(ds);

                DataTable dt = ds.Tables[0];
                ebase.Close();

                foreach (DataRow row in dt.Rows)
                {
                    nrcarta = Convert.ToInt32(row["nrcarta"].ToString().Trim());

                    cnpjcpf = row["cnpjcpf"].ToString().Trim();
                    if(row["codigouf"].ToString().Trim() != "")
                    codigouf = Convert.ToInt32(row["codigouf"].ToString().Trim());
                    chavea = row["chavenfe"].ToString().Trim();
                    nrseq = Convert.ToInt32(row["nrseq"].ToString().Trim());
                    nrnfe = Convert.ToInt32(row["nrnfe"].ToString().Trim());
                    
                    texto = row["texto"].ToString().Trim();
                    serie = Convert.ToInt32(row["serie"].ToString().Trim());
                    //chavea = row["chavea"].ToString().Trim();
                    Console.WriteLine(serie + " - serie");
                    protocolo = row["protocolo"].ToString().Trim();
                    

                    //eanval = row["eanval"].ToString().Trim();

                }
            }

            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "", false);
            }

        }

        public int Cartanfe_id { get => cartanfe_id; set => cartanfe_id = value; }
        public int Nrcarta { get => nrcarta; set => nrcarta = value; }
        public int Nrseq { get => nrseq; set => nrseq = value; }
        public int Nrnfe { get => nrnfe; set => nrnfe = value; }
        public int Serie { get => serie; set => serie = value; }
        public string Texto { get => texto; set => texto = value; }
        public string Estado { get => estado; set => estado = value; }
        public int Codigouf { get => codigouf; set => codigouf = value; }
        public string Cnpjcpf { get => cnpjcpf; set => cnpjcpf = value; }
        public string Chavea { get => chavea; set => chavea = value; }
        public string Protocolo { get => protocolo; set => protocolo = value; }
        public DateTime Data { get => data; set => data = value; }
        public string Status { get => status; set => status = value; }

        public void Update()
        {
            try
            {
                DbfBase ebase = new DbfBase();
                String instrucao = @"Update " + ebase.Path + @"\CARTANFE.dbf SET " +
                    //"ambiente = '" + Ambiente + "', " +
                    //"chave = '" + chave + "', " +
                    //"xml = '" + xml + "', " +
                    //"danfe = '" + danfe + "', " +
                    "protocolo = '" + protocolo + "' " +
                    "where nrcarta = " + nrcarta;

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
