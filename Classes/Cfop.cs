using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nfecreator
{
    class Cfop
    {
        string codigo;
        string descricao;
        string obs;
        string tipo;
             

        public Cfop(string cod)
        {
            try
            {
                DbfBase ebase = new DbfBase();
                string instrucao = @"SELECT * from " + ebase.Path + @"\CFOP.dbf WHERE codigo = '" + cod + "' ";

                OleDbCommand cmd = new OleDbCommand(instrucao, ebase.Conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);

                DataSet ds = new DataSet();

                da.Fill(ds);

                DataTable dt = ds.Tables[0];
                ebase.Close();

                foreach (DataRow row in dt.Rows)
                {
                    codigo = row["codigo"].ToString().Trim();
                    descricao = row["descricao"].ToString().Trim();
                    obs = row["obs"].ToString().Trim();
                    tipo = row["tipo"].ToString().Trim();
                }
              
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());             
            }

        }

        public string Codigo { get => codigo; set => codigo = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Obs { get => obs; set => obs = value; }
        public string Tipo { get => tipo; set => tipo = value; }
    }



}
