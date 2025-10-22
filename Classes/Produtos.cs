using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nfecreator
{
    class Produtos
    {
        int id_produto = 0;
        String codigog = "0";
        String codigo = "0";
        String codigob = "0";
        String descricao = " ";
        String unidade = "UN";
        String marca = " ";
        decimal vl_custo = 0;
        decimal vl_venda = 0;
        decimal plucro = 0;
        decimal comissao = 0;
        decimal estoque = 0;
        decimal estoquem = 0;
        String situacaot = "0";
        String aliquotaicms = "99";
        String ipi = "999";
        String obs = " ";
        DateTime data = DateTime.Now;
        String cf = "0";
        decimal peso;
        String cst = "99";
        String ncm = "";
        String predubicms = "100";
        String cstipi = "0";
        String cstconfins = "99";
        String cstpis = "0";
        String subtrib = " ";
        decimal mva = 0;
        String adi = " ";
        String origem = " ";
        decimal pesol;
        String status = " ";
        String cfopd = "0";
        String cfopf = "0";
        String cest = "0";
        String fcp = "NÃO";
        String cenqipi = "0";
        String eanval = "0";


        decimal IBPTEstadual = 0;
        decimal IBPTNacional = 0;

        string indescala;
        string cnpjfab;
        string cBenef;
        string nomefab;

        string codigoanp;
        string descanp;
        string codif;
        decimal qtfattempa = 0;
        string ufcombus;
        decimal bccombus = 0;
        decimal alicombus = 0;
        decimal valoricombus = 0;


        public int Id_produto { get => id_produto; set => id_produto = value; }
        public string Codigog { get => codigog; set => codigog = value; }
        public string Codigo { get => codigo; set => codigo = value; }
        public string Codigob { get => codigob; set => codigob = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Unidade { get => unidade; set => unidade = value; }
        public string Marca { get => marca; set => marca = value; }
        public decimal Vl_custo { get => vl_custo; set => vl_custo = value; }
        public decimal Vl_venda { get => vl_venda; set => vl_venda = value; }
        public decimal Comissao { get => comissao; set => comissao = value; }
        public decimal Estoque { get => estoque; set => estoque = value; }
        public decimal Estoquem { get => estoquem; set => estoquem = value; }
        public string Situacaot { get => situacaot; set => situacaot = value; }
        public string Aliquotaicms { get => aliquotaicms; set => aliquotaicms = value; }
        public string Ipi { get => ipi; set => ipi = value; }
        public string Obs { get => obs; set => obs = value; }
        public DateTime Data { get => data; set => data = value; }
        public string Cf { get => cf; set => cf = value; }
        public decimal Peso { get => peso; set => peso = value; }
        public string Cst { get => cst; set => cst = value; }
        public string Ncm { get => ncm; set => ncm = value; }
        public string Predubicms { get => predubicms; set => predubicms = value; }
        public string Cstipi { get => cstipi; set => cstipi = value; }
        public string Cstconfins { get => cstconfins; set => cstconfins = value; }
        public string Cstpis { get => cstpis; set => cstpis = value; }
        public string Subtrib { get => subtrib; set => subtrib = value; }
        public Decimal Mva { get => mva; set => mva = value; }
        public string Adi { get => adi; set => adi = value; }
        public string Origem { get => origem; set => origem = value; }
        public decimal Pesol { get => pesol; set => pesol = value; }
        public string Status { get => status; set => status = value; }
        public string Cfopd { get => cfopd; set => cfopd = value; }
        public string Cfopf { get => cfopf; set => cfopf = value; }
        public string Cest { get => cest; set => cest = value; }
        public string Fcp { get => fcp; set => fcp = value; }
        public string Cenqipi { get => cenqipi; set => cenqipi = value; }
        public string Eanval { get => eanval; set => eanval = value; }
        public string Indescala { get => indescala; set => indescala = value; }
        public string Cnpjfab { get => cnpjfab; set => cnpjfab = value; }
        public string CBenef { get => cBenef; set => cBenef = value; }
        public string Nomefab { get => nomefab; set => nomefab = value; }
        public string Codigoanp { get => codigoanp; set => codigoanp = value; }
        public string Descanp { get => descanp; set => descanp = value; }
        public string Codif { get => codif; set => codif = value; }
        public decimal Qtfattempa { get => qtfattempa; set => qtfattempa = value; }
        public string Ufcombus { get => ufcombus; set => ufcombus = value; }
        public decimal Bccombus { get => bccombus; set => bccombus = value; }
        public decimal Alicombus { get => alicombus; set => alicombus = value; }
        public decimal Valoricombus { get => valoricombus; set => valoricombus = value; }
        public decimal IBPTEstadual1 { get => IBPTEstadual; set => IBPTEstadual = value; }
        public decimal IBPTNacional1 { get => IBPTNacional; set => IBPTNacional = value; }
        public decimal Plucro { get => plucro; set => plucro = value; }

        public Produtos() { }

        public Produtos(string codigobarra)
        {
           
            try
            {
                DbfBase ebase = new DbfBase();
                string instrucao = @"SELECT codigob, eanval, cest, obs from " + ebase.Path + @"\PRODUTOS.dbf WHERE codigob = '" + codigobarra + "' ";

                OleDbCommand cmd = new OleDbCommand(instrucao, ebase.Conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);

                DataSet ds = new DataSet();

                da.Fill(ds);

                DataTable dt = ds.Tables[0];
                ebase.Close();

                foreach (DataRow row in dt.Rows)
                {
                   // id_produto = Convert.ToInt32(row["codigob"].ToString().Trim());
                    codigob = row["codigob"].ToString().Trim();
                    eanval = row["eanval"].ToString().Trim();
                    cest = row["cest"].ToString().Trim();
                    obs = row["obs"].ToString().Trim();
                }

                ebase = new DbfBase();
                try
                {
                    instrucao = @"SELECT cbenef from " + ebase.Path + @"\PRODUTOI.dbf WHERE codigob = '" + codigobarra + "' ";

                    cmd = new OleDbCommand(instrucao, ebase.Conn);
                    da = new OleDbDataAdapter(cmd);

                    ds = new DataSet();

                    da.Fill(ds);

                    dt = ds.Tables[0];
                    ebase.Close();

                    foreach (DataRow row in dt.Rows)
                    {
                        CBenef = row["cbenef"].ToString().Trim();
                    }
                }
                catch (Exception ex)
                {
                  
                }



            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "", false);

            }

        }

        

    }
}
