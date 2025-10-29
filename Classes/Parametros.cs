using System;
using System.Data;
using System.Data.OleDb;

namespace nfecreator
{
    class Parametros
    {
        string id;
        string razaosocial;
        string nome;
        string tipoe;
        string endereco;
        string complemento;
        string comple2;
        string bairro;
        string cidade;
        string codcidade;
        string uf;
        string coduf;
        string pais;
        string codpais;
        string cep;
        string fone_res;
        string fone2;
        string estado_c;
        string cnpj;
        string ie;
        string iest;
        string im;
        string cnae;
        string crt;
        string simples;
        string email;
        string responsavel;
        string dezporcento;
        int serie;
        int serienfce;
        string ambiente;
        string calculatrib;
        string cidtoken;
        string csc;
        string impressoranfce;
        string diretonfce;
        string versao;
        int hversao;
        string atualizar;
        string estoque;
        DateTime data;
        DateTime validade;
        string categoria;
        string cnpjcontabilidade;
        string stonecode;
        string cnpjcredenciadora;
        decimal pcredsn;
        string beta;
        string financeiro;
        String confemail;
        string ipi;

        //satce
        string cdativa;
        string cadllsat;
        string assina;
        string vdados;

        int nfcetpEmis;

        public string Id { get => id; set => id = value; }
        public string Razaosocial { get => razaosocial; set => razaosocial = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Tipoe { get => tipoe; set => tipoe = value; }
        public string Endereco { get => endereco; set => endereco = value; }
        public string Complemento { get => complemento; set => complemento = value; }
        public string Comple2 { get => comple2; set => comple2 = value; }
        public string Bairro { get => bairro; set => bairro = value; }
        public string Cidade { get => cidade; set => cidade = value; }
        public string Codcidade { get => codcidade; set => codcidade = value; }
        public string Uf { get => uf; set => uf = value; }
        public string Coduf { get => coduf; set => coduf = value; }
        public string Pais { get => pais; set => pais = value; }
        public string Codpais { get => codpais; set => codpais = value; }
        public string Cep { get => cep; set => cep = value; }
        public string Fone_res { get => fone_res; set => fone_res = value; }
        public string Fone2 { get => fone2; set => fone2 = value; }
        public string Estado_c { get => estado_c; set => estado_c = value; }
        public string Cnpj { get => cnpj; set => cnpj = value; }
        public string Ie { get => ie; set => ie = value; }
        public string Crt { get => crt; set => crt = value; }
        public string Simples { get => simples; set => simples = value; }
        public string Email { get => email; set => email = value; }
        public string Responsavel { get => responsavel; set => responsavel = value; }
        public string Dezporcento { get => dezporcento; set => dezporcento = value; }
        public int Serie { get => serie; set => serie = value; }
        public string Ambiente { get => ambiente; set => ambiente = value; }
        public string Calculatrib { get => calculatrib; set => calculatrib = value; }
        public string Cidtoken { get => cidtoken; set => cidtoken = value; }
        public string Csc { get => csc; set => csc = value; }
        public string Impressoranfce { get => impressoranfce; set => impressoranfce = value; }
        public string Diretonfce { get => diretonfce; set => diretonfce = value; }
        public string Versao { get => versao; set => versao = value; }
        public DateTime Data { get => data; set => data = value; }
        public DateTime Validade { get => validade; set => validade = value; }
        public int Serienfce { get => serienfce; set => serienfce = value; }
        public string Cnpjcontabilidade { get => cnpjcontabilidade; set => cnpjcontabilidade = value; }
        public string Categoria { get => categoria; set => categoria = value; }
        public string Stonecode { get => stonecode; set => stonecode = value; }
        public decimal PCredsn { get => pcredsn; set => pcredsn = value; }
        public string Beta { get => beta; set => beta = value; }
        public string Financeiro { get => financeiro; set => financeiro = value; }
        public string Iest { get => iest; set => iest = value; }
        public string Im { get => im; set => im = value; }
        public string Cnae { get => cnae; set => cnae = value; }
        public int Hversao { get => hversao; set => hversao = value; }
        public string Atualizar { get => atualizar; set => atualizar = value; }
        public string Estoque { get => estoque; set => estoque = value; }
        public string Cnpjcredenciadora { get => cnpjcredenciadora; set => cnpjcredenciadora = value; }
        public string Confemail { get => confemail; set => confemail = value; }
        public int NfcetpEmis { get => nfcetpEmis; set => nfcetpEmis = value; }
        public string Ipi { get => ipi; set => ipi = value; }

        //SAT
        public string Cdativa { get => cdativa; set => cdativa = value; }
        public string Cadllsat { get => cadllsat; set => cadllsat = value; }
        public string Assina { get => assina; set => assina = value; }
        public string Vdados { get => vdados; set => vdados = value; }

        public Parametros() { }

        public Parametros(int id)
        {
            
            try
            {
                DbfBase ebase = new DbfBase();
                string instrucao = @"SELECT * from " + ebase.Path + @"\PARAMETROS.dbf";
                string sql = instrucao;
                OleDbCommand cmd = new OleDbCommand(sql, ebase.Conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);

                DataSet ds = new DataSet();

                da.Fill(ds);

                DataTable dt = ds.Tables[0];


                int i = 1;
                foreach (DataRow row in dt.Rows)
                {
                    if (i == 1)
                    {
                        Cnpj = row["cnpj"].ToString().Trim();
                        Nome = row["medico"].ToString().Trim();
                        Razaosocial = row["razaosocial"].ToString().Trim();
                        //Tipoe = row["tipoe"].ToString();
                        Endereco = row["endereco"].ToString().Trim();
                        Complemento = row["numero"].ToString().Trim();
                        Comple2 = row["comple"].ToString().Trim();
                        Bairro = row["bairro"].ToString().Trim();
                        Cidade = row["cidade"].ToString().Trim();
                        Codcidade = row["codigomu"].ToString().Trim();
                        Uf = row["estado"].ToString().Trim();
                        Coduf = row["codigouf"].ToString().Trim();
                        Pais = row["pais"].ToString().Trim();
                        Codpais = row["codigopais"].ToString().Trim();
                        Cep = row["cep"].ToString().Trim();
                        Fone_res = row["fone"].ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "");
                        Fone2 = row["fax"].ToString().Trim();
                        //Email = row["email"].ToString();
                        //Estado_c = row["estado_c"].ToString();
                        Ie = row["iest"].ToString().Trim();
                        //Iest = row["iest"].ToString();
                        Im = row["imunicipal"].ToString().Trim();
                        Cnae = row["cnae"].ToString().Trim();

                        Crt = row["crt"].ToString();
                        Simples = row["simples"].ToString();
                        Responsavel = row["responsavel"].ToString().Trim();
                       // int mySerie = 3;
                        if (row["campo94"].ToString().Trim() != "")
                            serie = Convert.ToInt32(row["campo94"].ToString().Trim());
                       
                        if (row["nfceserie"].ToString().Trim() != "")
                            serienfce = Convert.ToInt32(row["nfceserie"].ToString().Trim());

                        //Dezporcento = row["dezporcento"].ToString();
                        //serie = Convert.ToInt32(row["serie"].ToString());
                        //serienfce = Convert.ToInt32(row["nfceserie"].ToString());
                        //ambiente = row["ambiente"].ToString();

                        calculatrib = row["nfe12741"].ToString().Trim();

                        //cidtoken = row["cidtoken"].ToString();
                        //csc = row["csc"].ToString();

                        //impressoranfce = row["impressoranfce"].ToString();
                        //diretonfce = row["diretonfce"].ToString();
                        //versao = row["versao"].ToString();

                        /////
                        //hversao = Convert.ToInt32(row["hversao"].ToString());
                        //atualizar = row["atualizar"].ToString();
                        //estoque = row["estoque"].ToString();

                        // data =   row.GetDateTime("data");
                        //data = Convert.ToDateTime(row["data"].ToString());

                        //// validade = row.GetDateTime("validade");
                        //validade = Convert.ToDateTime(row["validade"].ToString());
                        //stonecode = row["stonecode"].ToString();
                        //cnpjcredenciadora = row["cnpjcredenciadora"].ToString();


                        //cnpjcontabilidade = row["cnpjcontabilidade"].ToString().Replace(" ", "").Replace(".", "").Replace("-", "").Replace(@"\", "").Replace(@"/", "");
                        //categoria = row["categoria"].ToString();

                        //// pcredsn = row.GetDecimal("pcredsn");
                        
                        if (row["aliqcredito"].ToString().Trim() != "")
                            pcredsn = Convert.ToDecimal(row["aliqcredito"].ToString());
                        //confemail = row["confemail"].ToString();

                        //beta = row["beta"].ToString();
                        //financeiro = row["financeiro"].ToString();

                        if (row["nfcetipoemis"].ToString().Trim() != "")
                            nfcetpEmis = Convert.ToInt32(row["nfcetipoemis"].ToString().Trim());
                        else nfcetpEmis = 1;

                        if (row["usaipi"].ToString().Trim() != "")
                            ipi = row["usaipi"].ToString().Trim();
                        else ipi = "NÃO";


                        i++;
                    }
                    else {
                        Funcoes.Mensagem("PARAMETROS DUPLICADO", "TABELA PARAMETROS", System.Windows.MessageBoxButton.OK);
                    }
                }


                ebase = new DbfBase();
                instrucao = @"SELECT * from " + ebase.Path + @"\PARAMAUX.dbf";
                sql = instrucao;
                cmd = new OleDbCommand(sql, ebase.Conn);
                da = new OleDbDataAdapter(cmd);

                ds = new DataSet();

                da.Fill(ds);

                dt = ds.Tables[0];

                foreach (DataRow row in dt.Rows)
                {      
                    cnpjcontabilidade = row["contadord"].ToString().Replace(" ", "").Replace(".", "").Replace("-", "").Replace(@"\", "").Replace(@"/", "");
                   
                    //SATCE
                    cadllsat = row["cadllsat"].ToString().Replace(" ", "");
                    cdativa = row["cdativa"].ToString().Replace(" ", "");
                    assina = row["assina"].ToString().Replace(" ", "");
                    vdados = row["vdados"].ToString().Replace(" ", "");
                }



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

        public void Update()
        {
            try
            {
                DbfBase ebase = new DbfBase();
                String instrucao = @"Update " + ebase.Path + @"\PARAMAUX.dbf SET acessoi =  0 ";
                    //"ambiente = '" + Ambiente + "', " +
                    //"chave = '" + chave + "', " +
                    //"xml = '" + xml + "', " +
                    //"danfe = '" + danfe + "', " +
                    //"nprotocolo = '" + nprotocolo + "' " +
                    //"where nrvenda = " + nrvenda;

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
