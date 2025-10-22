using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nfecreator
{
    class VendaSatcea
    {
        int nrvenda;
        string statusnfe;
        string ambiente;
        string chave;
        string xml;
        string danfe;
        string nprotocolo;
        string merro;

        public int Nrvenda { get => nrvenda; set => nrvenda = value; }
        public string Statusnfe { get => statusnfe; set => statusnfe = value; }
        public string Ambiente { get => ambiente; set => ambiente = value; }
        public string Chave { get => chave; set => chave = value; }
        public string Xml { get => xml; set => xml = value; }
        public string Danfe { get => danfe; set => danfe = value; }
        public string Nprotocolo { get => nprotocolo; set => nprotocolo = value; }

        public string ColorStatus
        {
            get => statusnfe == "PENDENTE" ? "#0088cc" :
                   statusnfe == "APROVADO" ? "#47a447" :
                   statusnfe == "INUTILIZADO" ? "#ed9c28" :
                   statusnfe == "CANCELADO" ? "#d2322d" :
                   statusnfe == "DENEGADO" ? "#777777" :
                   statusnfe == "REJEITADO" ? "#5bc0de" : "#0088cc";
        }
        public string Merro { get => merro; set => merro = value; }

        public VendaSatcea()
        { }

        public VendaSatcea(int id)
        {
            try
            {
                DbfBase ebase = new DbfBase();
                string instrucao = @"SELECT * from " + ebase.Path + @"\SATCEA.dbf WHERE nrvenda = " + id + " ";

                OleDbCommand cmd = new OleDbCommand(instrucao, ebase.Conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);

                DataSet ds = new DataSet();

                da.Fill(ds);

                DataTable dt = ds.Tables[0];

                foreach (DataRow row in dt.Rows)
                {
                    nrvenda = id;
                    statusnfe = row["statusnfe"].ToString().Trim();
                    ambiente = row["ambiente"].ToString().Trim();
                    chave = row["chave"].ToString().Trim();
                    xml = row["xml"].ToString().Trim();
                    danfe = row["danfe"].ToString().Trim();
                    nprotocolo = row["nprotocolo"].ToString().Trim();
                    merro = row["merro"].ToString().Trim();
                }

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


        public void Update()
        {
            try
            {
                if (merro == null) merro = "";

                DbfBase ebase = new DbfBase();
                String instrucao = @"Update " + ebase.Path + @"\SATCEA.dbf SET statusnfe = '" + Statusnfe + "', " +
                    "ambiente = '" + Ambiente + "', " +
                    "chave = '" + chave + "', " +
                    "xml = '" + xml + "', " +
                    "danfe = '" + danfe + "', " +
                    "merro = '" + merro.Trim() + "', " +
                    "nprotocolo = '" + nprotocolo + "' " +
                    "where nrvenda = " + nrvenda;

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
                    Funcoes.Crashe(ex, "ERRO - NFCEA", true);

            }

        }

        public void Insert()
        {
            try
            {
                DbfBase ebase = new DbfBase();
                String instrucao = @"Insert into " + ebase.Path + @"\SATCEA.dbf  (nrvenda, statusnfe, ambiente, chave, xml, danfe, merro, nprotocolo )  VALUES (" + nrvenda + @", '" + statusnfe + @"', '', '', '', '', '', ''  )";
                
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
