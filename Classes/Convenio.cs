using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nfecreator
{
    class Convenio
    {
        string codigo;
        string nome;
        string cnpj;

        public string Codigo { get => codigo; set => codigo = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Cnpj { get => cnpj; set => cnpj = value; }

        public Convenio(string cod)
        {
            try
            {
                DbfBase ebase = new DbfBase();
                string instrucao = @"SELECT * from " + ebase.Path + @"\CONVENIO.dbf WHERE tipor = 'N' AND codigo = '" + cod + "' ";

                OleDbCommand cmd = new OleDbCommand(instrucao, ebase.Conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);

                DataSet ds = new DataSet();

                da.Fill(ds);

                DataTable dt = ds.Tables[0];
                ebase.Close();

                foreach (DataRow row in dt.Rows)
                {
                    codigo = row["codigo"].ToString().Trim();
                    Nome = row["nome"].ToString().Trim();
                    Cnpj = row["cnpj"].ToString().Trim();
                }
              
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());             
            }

        }






    }



}
