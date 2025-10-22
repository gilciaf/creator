using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nfecreator
{
    class VendaNFCe
    {
        int id_vendanfce = 0;
        int nrvenda;
        DateTime dhEmi;
        DateTime dhSaiEnt;
        int indfinal;
        int indpres;
        int tpnf;
        int tpEmis;
        int idop;
        int contrib;
        string id_cliente;
        string nomecliente;
        int id_vendedor;
        string obs;
        string infadfisco;
        string endereco;
        string numero;
        string complemento;
        string bairro;
        int cmun;
        string cidade;
        string uf;
        int cpais;
        string pais;
        string cep;
        string fone;
        string email;
        string cnpj_cpf;
        string ie_rg;
        string im;
        decimal vl_entrega;
        decimal comissao_total;
        string status;
        string natop;
        string cfop;
        int serie;
        decimal vbc;
        decimal vicms;
        decimal vicmsdeson;
        decimal vfcpufdest;
        decimal vicmsufdest;
        decimal vicmsufremet;
        decimal vfcp;
        decimal vbcst;
        decimal vst;
        decimal vfcpst;
        decimal vfcpstret;
        decimal vprod;
        decimal vfrete;
        decimal vseg;
        decimal vdesc;
        decimal vii;
        decimal vipi;
        decimal vipidevol;
        decimal vpis;
        decimal vconfins;
        decimal voutro;
        decimal vnf;
        decimal vtroco;
        decimal vtotaltrib;
        int modFrete;
        string nome_transp;
        string cnpjcpf_transp;
        string ie_transp;
        string uf_transp;
        string ender_transp;
        string mun_transp;
        string rntc_transp;
        string placa_transp;
        string ufveiculo_transp;
        int qVol_transp;
        string esp_transp;
        string marca_transp;
        string nvol_transp;
        decimal pesol_transp;
        decimal pesob_transp;
        string nlacre_transp;
        string nrpedido;
        string modelodocumento;
        string nrordemequipamento;
        string nrdocemitido;
        string uf_embarq;
        string lc_embarq;
        int finalidade;
        int danfe;
        string justifica;
        string chavedev;
        string nrnfedev;
        DateTime datacontingencia;
        string chavedeacesso;
        string protocolo;

        string cnpj_cpf_entrega;
        string ie_entrega;
        string xnome_entrega;
        string xlgr_entrega;
        string nro_entrega;
        string xcpl_entrega;
        string xbairro_entrega;
        int cmun_entrega;
        string xmun_entrega;
        string uf_entrega;
        string cep_entrega;
        int cpais_entrega;
        string xpais_entrega;
        string fone_entrega;
        string email_entrega;
        string cnpj_cpf_retirada;
        string ie_retirada;
        string xnome_retirada;
        string xlgr_retirada;
        string nro_retirada;
        string xcpl_retirada;
        string xbairro_retirada;
        int cmun_retirada;
        string xmun_retirada;
        string uf_retirada;
        string cep_retirada;
        int cpais_retirada;
        string xpais_retirada;
        string fone_retirada;
        string email_retirada;



        public int Id_vendanfce { get => id_vendanfce; set => id_vendanfce = value; }
        public int Nrvenda { get => nrvenda; set => nrvenda = value; }
        public DateTime DhEmi { get => dhEmi; set => dhEmi = value; }
        public DateTime DhSaiEnt { get => dhSaiEnt; set => dhSaiEnt = value; }
        public int Indfinal { get => indfinal; set => indfinal = value; }
        public int Indpres { get => indpres; set => indpres = value; }
        public int Idop { get => idop; set => idop = value; }
        public string Id_cliente { get => id_cliente; set => id_cliente = value; }
        public string Nomecliente { get => nomecliente; set => nomecliente = value; }
        public int Id_vendedor { get => id_vendedor; set => id_vendedor = value; }
        public string Obs { get => obs; set => obs = value; }
        public string Endereco { get => endereco; set => endereco = value; }
        public string Numero { get => numero; set => numero = value; }
        public string Complemento { get => complemento; set => complemento = value; }
        public string Bairro { get => bairro; set => bairro = value; }
        public int Cmun { get => cmun; set => cmun = value; }
        public string Cidade { get => cidade; set => cidade = value; }
        public string Uf { get => uf; set => uf = value; }
        public int Cpais { get => cpais; set => cpais = value; }
        public string Pais { get => pais; set => pais = value; }
        public string Cep { get => cep; set => cep = value; }
        public string Email { get => email; set => email = value; }
        public string Cnpj_cpf { get => cnpj_cpf; set => cnpj_cpf = value; }
        public string Ie_rg { get => ie_rg; set => ie_rg = value; }
        public decimal Vl_entrega { get => vl_entrega; set => vl_entrega = value; }
        public decimal Comissao_total { get => comissao_total; set => comissao_total = value; }
        public string Status { get => status; set => status = value; }
        public string Natop { get => natop; set => natop = value; }
        public string Cfop { get => cfop; set => cfop = value; }
        public int Serie { get => serie; set => serie = value; }
        public decimal Vbc { get => vbc; set => vbc = value; }
        public decimal Vicms { get => vicms; set => vicms = value; }
        public decimal Vicmsdeson { get => vicmsdeson; set => vicmsdeson = value; }
        public decimal Vfcpufdest { get => vfcpufdest; set => vfcpufdest = value; }
        public decimal Vicmsufdest { get => vicmsufdest; set => vicmsufdest = value; }
        public decimal Vicmsufremet { get => vicmsufremet; set => vicmsufremet = value; }
        public decimal Vfcp { get => vfcp; set => vfcp = value; }
        public decimal Vbcst { get => vbcst; set => vbcst = value; }
        public decimal Vst { get => vst; set => vst = value; }
        public decimal Vfcpst { get => vfcpst; set => vfcpst = value; }
        public decimal Vfcpstret { get => vfcpstret; set => vfcpstret = value; }
        public decimal Vprod { get => vprod; set => vprod = value; }
        public decimal Vfrete { get => vfrete; set => vfrete = value; }
        public decimal Vseg { get => vseg; set => vseg = value; }
        public decimal Vdesc { get => vdesc; set => vdesc = value; }
        public decimal Vii { get => vii; set => vii = value; }
        public decimal Vipi { get => vipi; set => vipi = value; }
        public decimal Vipidevol { get => vipidevol; set => vipidevol = value; }
        public decimal Vpis { get => vpis; set => vpis = value; }
        public decimal Vconfins { get => vconfins; set => vconfins = value; }
        public decimal Voutro { get => voutro; set => voutro = value; }
        public decimal Vnf { get => vnf; set => vnf = value; }
        public decimal Vtroco { get => vtroco; set => vtroco = value; }
        public decimal Vtotaltrib { get => vtotaltrib; set => vtotaltrib = value; }
        public int ModFrete { get => modFrete; set => modFrete = value; }
        public string Nome_transp { get => nome_transp; set => nome_transp = value; }
        public string Cnpjcpf_transp { get => cnpjcpf_transp; set => cnpjcpf_transp = value; }
        public string Ie_transp { get => ie_transp; set => ie_transp = value; }
        public string Uf_transp { get => uf_transp; set => uf_transp = value; }
        public string Ender_transp { get => ender_transp; set => ender_transp = value; }
        public string Mun_transp { get => mun_transp; set => mun_transp = value; }
        public string Rntc_transp { get => rntc_transp; set => rntc_transp = value; }
        public string Placa_transp { get => placa_transp; set => placa_transp = value; }
        public string Ufveiculo_transp { get => ufveiculo_transp; set => ufveiculo_transp = value; }
        public int QVol_transp { get => qVol_transp; set => qVol_transp = value; }
        public string Esp_transp { get => esp_transp; set => esp_transp = value; }
        public string Marca_transp { get => marca_transp; set => marca_transp = value; }
        public string Nvol_transp { get => nvol_transp; set => nvol_transp = value; }
        public decimal Pesol_transp { get => pesol_transp; set => pesol_transp = value; }
        public decimal Pesob_transp { get => pesob_transp; set => pesob_transp = value; }
        public string Nlacre_transp { get => nlacre_transp; set => nlacre_transp = value; }
        public string Nrpedido { get => nrpedido; set => nrpedido = value; }
        public string Modelodocumento { get => modelodocumento; set => modelodocumento = value; }
        public string Nrordemequipamento { get => nrordemequipamento; set => nrordemequipamento = value; }
        public string Nrdocemitido { get => nrdocemitido; set => nrdocemitido = value; }
        public string Uf_embarq { get => uf_embarq; set => uf_embarq = value; }
        public string Lc_embarq { get => lc_embarq; set => lc_embarq = value; }
        public int Finalidade { get => finalidade; set => finalidade = value; }
        public int Danfe { get => danfe; set => danfe = value; }
        public string Justifica { get => justifica; set => justifica = value; }
        public string Chavedev { get => chavedev; set => chavedev = value; }
        public string Nrnfedev { get => nrnfedev; set => nrnfedev = value; }
        public DateTime Datacontingencia { get => datacontingencia; set => datacontingencia = value; }
        public string Chavedeacesso { get => chavedeacesso; set => chavedeacesso = value; }
        public int Contrib { get => contrib; set => contrib = value; }
        public string Protocolo { get => protocolo; set => protocolo = value; }
        public string Infadfisco { get => infadfisco; set => infadfisco = value; }
        public int Tpnf { get => tpnf; set => tpnf = value; }
        public int TpEmis { get => tpEmis; set => tpEmis = value; }
        public string Fone { get => fone; set => fone = value; }

        public string Cnpj_cpf_entrega { get => cnpj_cpf_entrega; set => cnpj_cpf_entrega = value; }
        public string Ie_entrega { get => ie_entrega; set => ie_entrega = value; }
        public string Xnome_entrega { get => xnome_entrega; set => xnome_entrega = value; }
        public string Xlgr_entrega { get => xlgr_entrega; set => xlgr_entrega = value; }
        public string Nro_entrega { get => nro_entrega; set => nro_entrega = value; }
        public string Xcpl_entrega { get => xcpl_entrega; set => xcpl_entrega = value; }
        public string Xbairro_entrega { get => xbairro_entrega; set => xbairro_entrega = value; }
        public int Cmun_entrega { get => cmun_entrega; set => cmun_entrega = value; }
        public string Xmun_entrega { get => xmun_entrega; set => xmun_entrega = value; }
        public string Uf_entrega { get => uf_entrega; set => uf_entrega = value; }
        public string Cep_entrega { get => cep_entrega; set => cep_entrega = value; }
        public int Cpais_entrega { get => cpais_entrega; set => cpais_entrega = value; }
        public string Xpais_entrega { get => xpais_entrega; set => xpais_entrega = value; }
        public string Fone_entrega { get => fone_entrega; set => fone_entrega = value; }
        public string Email_entrega { get => email_entrega; set => email_entrega = value; }
        public string Cnpj_cpf_retirada { get => cnpj_cpf_retirada; set => cnpj_cpf_retirada = value; }
        public string Ie_retirada { get => ie_retirada; set => ie_retirada = value; }
        public string Xnome_retirada { get => xnome_retirada; set => xnome_retirada = value; }
        public string Xlgr_retirada { get => xlgr_retirada; set => xlgr_retirada = value; }
        public string Nro_retirada { get => nro_retirada; set => nro_retirada = value; }
        public string Xcpl_retirada { get => xcpl_retirada; set => xcpl_retirada = value; }
        public string Xbairro_retirada { get => xbairro_retirada; set => xbairro_retirada = value; }
        public int Cmun_retirada { get => cmun_retirada; set => cmun_retirada = value; }
        public string Xmun_retirada { get => xmun_retirada; set => xmun_retirada = value; }
        public string Uf_retirada { get => uf_retirada; set => uf_retirada = value; }
        public string Cep_retirada { get => cep_retirada; set => cep_retirada = value; }
        public int Cpais_retirada { get => cpais_retirada; set => cpais_retirada = value; }
        public string Xpais_retirada { get => xpais_retirada; set => xpais_retirada = value; }
        public string Fone_retirada { get => fone_retirada; set => fone_retirada = value; }
        public string Email_retirada { get => email_retirada; set => email_retirada = value; }
        public string IM1 { get => im; set => im = value; }
        public string Im { get => im; set => im = value; }


        public string ColorStatus
        {
            get => status == "PENDENTE" ? "#0088cc" :
                   status == "APROVADO" ? "#47a447" :
                   status == "INUTILIZADO" ? "#ed9c28" :
                   status == "CANCELADO" ? "#d2322d" :
                   status == "DENEGADO" ? "#777777" :
                   status == "REJEITADO" ? "#5bc0de" : "#0088cc";
        }

        public VendaNFCe()
        { }

        public VendaNFCe(int id)
        {
            try
            {
                DbfBase ebase = new DbfBase();
                string instrucao = @"SELECT * from " + ebase.Path + @"\NFCE.dbf WHERE nrvenda = " + id + " ";

                OleDbCommand cmd = new OleDbCommand(instrucao, ebase.Conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);

                DataSet ds = new DataSet();

                da.Fill(ds);

                DataTable dt = ds.Tables[0];

                ebase.Close();

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    nrvenda = id;
                    serie = Convert.ToInt32(row["serie"].ToString().Trim());
                    dhEmi = Convert.ToDateTime(row["datae"].ToString().Trim());
                    dhEmi = Convert.ToDateTime(dhEmi.ToString("dd/MM/yyyy ") + row["horaensai"].ToString().Trim());

                    dhSaiEnt = Convert.ToDateTime(row["datas"].ToString().Trim());
                    dhSaiEnt = Convert.ToDateTime(dhSaiEnt.ToString("dd/MM/yyyy ") + row["horaensai"].ToString().Trim());

                    cfop = row["cfop"].ToString().Trim();
                    natop = row["dcfop"].ToString().Trim();

                    // = Convert.ToInt32(row["nmvendedor"].ToString().Trim());
                    id_cliente = row["codcliente"].ToString().Trim();
                    nomecliente = row["nmcliente"].ToString().Trim();

                    endereco = row["endereco"].ToString().Trim();
                    numero = row["numero"].ToString().Trim();
                    complemento = row["complement"].ToString().Trim();
                    bairro = row["bairro"].ToString().Trim();
                    cidade = row["cidade"].ToString().Trim();
                    uf = row["uf"].ToString().Trim();
                    pais = row["pais"].ToString().Trim();

                    if (row["codcidade"].ToString().Trim() != "")
                        cmun = Convert.ToInt32(row["codcidade"].ToString().Trim());
                   

                    if (row["codpais"].ToString().Trim() != "")
                        cpais = Convert.ToInt32(row["codpais"].ToString().Trim());

                    if (row["cnpj"].ToString().Trim() != "")
                        cnpj_cpf = Funcoes.Deixarnumero(row["cnpj"].ToString().Trim());
                    else
                        cnpj_cpf = "";
                    //ie_rg = Funcoes.Deixarnumero(row["iestadual"].ToString().Trim());

                    fone = row["fone"].ToString().Trim();
                    cep = row["cep"].ToString().Trim();

                    vbc = Convert.ToDecimal(row["bcalicms"].ToString().Trim());
                    vicms = Convert.ToDecimal(row["valoricms"].ToString().Trim());

                    email = "";
                    //bcalicmssub
                    //valicmssub

                    //if (row["frete"].ToString().Trim() != "")
                    //    vfrete = Convert.ToDecimal(row["frete"].ToString().Trim());
                    vseg = Convert.ToDecimal(row["seguro"].ToString().Trim());
                    vdesc = Convert.ToDecimal(row["desconto"].ToString().Trim());
                    //voutro = Convert.ToDecimal(row["acessorias"].ToString().Trim());

                    //vipi = Convert.ToDecimal(row["valoripi"].ToString().Trim());
                    //vprod = Convert.ToDecimal(row["valorprodutos"].ToString().Trim());
                    vnf = Convert.ToDecimal(row["valornota"].ToString().Trim());

                    //nome_transp = row["transnome"].ToString().Trim();
                    //freteconta
                    //rntc_transp = row["codigoantt"].ToString().Trim();
                    //placa_transp = row["placav"].ToString().Trim();
                    //uf_transp = row["ufv"].ToString().Trim();


                    //cnpjcpf_transp = row["cnpjcpfmo"].ToString().Trim();
                    //ender_transp = row["transende"].ToString().Trim();
                    //mun_transp = row["transcidade"].ToString().Trim();
                    //ufveiculo_transp = row["transuf"].ToString().Trim();
                    //ie_transp = row["transiestadual"].ToString().Trim();

                    qVol_transp = 0;
                    //esp_transp = row["especie"].ToString().Trim();
                    //marca_transp = row["marca"].ToString().Trim();
                    //nvol_transp = row["numeracao"].ToString().Trim();
                    //pesol_transp = Convert.ToDecimal(row["pesob"].ToString().Trim());
                    //pesob_transp = Convert.ToDecimal(row["pesol"].ToString().Trim());

                    obs = row["obsnf"].ToString().Trim();
                    status = row["statusnfe"].ToString().Trim();

                    //aventrada
                    //qtparcela
                    //vlparcela
                    //comissaov comissao_total = reader.GetDecimal("comissao_total");

                   // vpis = Convert.ToDecimal(row["valpis"].ToString().Trim());
                   // vconfins = Convert.ToDecimal(row["valcofins"].ToString().Trim());

                    cnpj_cpf_entrega = row["lentrega"].ToString().Trim();
                    cnpj_cpf_retirada = row["lretirada"].ToString().Trim();
                    //lentrega
                    //lretirada

                   // vii = Convert.ToDecimal(row["valorii"].ToString().Trim());
                    vbcst = Convert.ToDecimal(row["valbcstret"].ToString().Trim());
                    vst = Convert.ToDecimal(row["valstret"].ToString().Trim());

                    // uf_embarq = row["ufembarq"].ToString().Trim();
                    // lc_embarq = row["lcembarq"].ToString().Trim();

                    //formapgauto
                    //dformapgauto
                    //vmimpostos
                    //pmimpostos

                    //nrpedido = row["nrpedido"].ToString().Trim();
                    //razaos
                    //indfinal = Convert.ToInt32(row["consumidorf"].ToString().Trim());
                    Indpres = Convert.ToInt32(row["coprese"].ToString().Trim());

                    // idop = Convert.ToInt32(row["idope"].ToString().Trim());


                    datacontingencia = Convert.ToDateTime(row["datacon"].ToString().Trim() + row["horacon"].ToString().Trim());
                    datacontingencia = Convert.ToDateTime(datacontingencia.ToString("dd/MM/yyyy ") + row["horacon"].ToString().Trim());

                    if (row["tipoemis"].ToString().Trim() != "")
                        tpEmis = Convert.ToInt32(row["tipoemis"].ToString().Trim());
                    finalidade = Convert.ToInt32(row["finalid"].ToString().Trim());

                    if (row["danfe"].ToString().Trim() != "")
                        danfe = Convert.ToInt32(row["danfe"].ToString().Trim());
                    else danfe = 4;

                  justifica = row["juscancel"].ToString().Trim();

                    //chavedev = row["chavedev"].ToString().Trim();
                    //nrnfedev = row["nrnfedev"].ToString().Trim();

                    //vfcpufdest = Convert.ToDecimal(row["vfcpufdest"].ToString().Trim());
                    //vicmsufdest = Convert.ToDecimal(row["vicmsufdest"].ToString().Trim());
                    // vicmsufremet = Convert.ToDecimal(row["vicmsufremet"].ToString().Trim());

                    //refmod
                    //refnecf
                    //refcoo
                    //ru_nota
                    //ru_serie
                    //ru_datae
                    //ru_cpfcnpj
                    //ru_ieprod
                    //ru_ufprod

                    //formapg
                    //meiopag
                    //recebido

                    vtroco = Convert.ToDecimal(row["troco"].ToString().Trim());

                    //formaciaf
                    //formadecia
                    //formaqpcia

                    //cdconveio
                    //nmconveio

                    //vipidevol = Convert.ToDecimal(row["vipidevol"].ToString().Trim());
                    //vfcp = Convert.ToDecimal(row["vfcp"].ToString().Trim());
                    //vfcpst = Convert.ToDecimal(row["vfcpst"].ToString().Trim());
                    //vfcpstret = Convert.ToDecimal(row["vfcpstret"].ToString().Trim());

                    //infadfisco = row["obsfisco"].ToString().Trim();

                    // vicmssubstituto = Convert.ToDecimal(row["vicmssubstituto"].ToString().Trim());



                    //                indpres = reader.GetInt32("indpres");
                    //                tpnf = reader.GetInt32("tpnf");
                    //                contrib = reader.GetInt32("contrib");
                    //                
                    //                email = reader["email"].ToString();             
                    //                im = reader["im"].ToString();
                    //                serie = reader.GetInt32("serie");
                    //                                   //                
                    //                vicmsdeson = reader.GetDecimal("vicmsdeson");
                    //                vicmsufdest = reader.GetDecimal("vicmsufdest");
                    //                vicmsufremet = reader.GetDecimal("vicmsufremet");
                    //               
                    //                vbcst = reader.GetDecimal("vbcst");
                    //                vst = reader.GetDecimal("vst");
                    //               
                    //                modFrete = reader.GetInt32("modFrete");
                    //                rntc_transp = reader["rntc_transp"].ToString();
                    //                nlacre_transp = reader["nlacre_transp"].ToString();
                    //                
                    //                modelodocumento = reader["modelodocumento"].ToString();
                    //                nrordemequipamento = reader["nrordemequipamento"].ToString();
                    //                nrdocemitido = reader["nrdocemitido"].ToString();

                    //                datacontingencia = reader.GetDateTime("datacontingencia");


                    //                chavedeacesso = reader["chavedeacesso"].ToString();
                    //                protocolo = reader["protocolo"].ToString();
                    modFrete = 9;
                    Indfinal = 1;
                    
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


        //public void Insert()
        //{
        //    string sql = @"INSERT INTO vendanfe (id_vendanfe ,  nrvenda ,  dhEmi, status, dhSaiEnt, datacontingencia, id_vendedor) VALUES (NULL, @nrvenda,  @dhEmi, 'NOVO', @dhSaiEnt, @datacontingencia, 1)";
        //    try
        //    {
        //        MySQLBase basemysql = new MySQLBase();
        //        MySqlCommand cmd = basemysql.connection.CreateCommand();
        //        cmd.CommandText = sql;
        //        cmd.CommandTimeout = 1000;

        //        cmd.Parameters.AddWithValue("@nrvenda", nrvenda);
        //        cmd.Parameters.AddWithValue("@dhEmi", DateTime.Now);
        //        cmd.Parameters.AddWithValue("@dhSaiEnt", DateTime.Now);
        //        cmd.Parameters.AddWithValue("@datacontingencia", DateTime.Now);
        //        cmd.ExecuteNonQuery();
        //        basemysql.Closer();
        //    }
        //    catch (Exception ex)
        //    {
        //        Funcoes.Crashe(ex, "", false);
        //    }
        //}

        //public void Update()
        //{
        //    string sql = @"UPDATE vendanfe SET nrvenda = @nrvenda,  dhEmi  = @dhEmi,  dhSaiEnt = @dhSaiEnt, indfinal = @indfinal, indpres = @indpres, tpnf = @tpnf, tpEmis = @tpEmis, idop=@idop, contrib = @contrib, id_cliente = @id_cliente, " +
        //      "nomecliente = @nomecliente, id_vendedor = @id_vendedor, " +
        //      "obs  = @obs, infadfisco  = @infadfisco, endereco  = @endereco, numero = @numero,  " +
        //      "complemento  = @complemento, bairro  = @bairro, cmun  = @cmun, cidade  = @cidade, uf  = @uf, cpais  = @cpais, pais  = @pais, " +
        //      "cep  = @cep, fone = @fone, email = @email, cnpj_cpf  = @cnpj_cpf, ie_rg  = @ie_rg, im = @im, vl_entrega = @vl_entrega, " +
        //      "comissao_total  = @comissao_total, status  = @status, natop  = @natop, cfop  = @cfop, serie  = @serie, " +
        //      "vbc  = @vbc, vicms  = @vicms, vicmsdeson  = @vicmsdeson, vfcpufdest  = @vfcpufdest, vicmsufdest = @vicmsufdest,  " +
        //      "vicmsufremet  = @vicmsufremet, vfcp  = @vfcp, vbcst  = @vbcst, vst  = @vst, vfcpst  = @vfcpst, vfcpstret  = @vfcpstret, vprod  = @vprod, " +
        //      "vfrete  = @vfrete, vseg  = @vseg, vdesc  = @vdesc, vii  = @vii, vipi  = @vipi, vipidevol  = @vipidevol, " +
        //      "vpis  = @vpis, vconfins  = @vconfins, voutro = @voutro, vnf = @vnf, vtroco  = @vtroco, vtotaltrib = @vtotaltrib, modFrete = @modFrete,  " +
        //      "nome_transp = @nome_transp, cnpjcpf_transp = @cnpjcpf_transp, ie_transp = @ie_transp, uf_transp = @uf_transp, ender_transp = @ender_transp,  " +
        //      "mun_transp = @mun_transp, rntc_transp = @rntc_transp, placa_transp = @placa_transp, ufveiculo_transp = @ufveiculo_transp, qVol_transp = @qVol_transp, " +
        //      "esp_transp = @esp_transp, marca_transp = @marca_transp, nvol_transp = @nvol_transp, pesol_transp = @pesol_transp, " +
        //      "pesob_transp = @pesob_transp, nlacre_transp = @nlacre_transp, nrpedido = @nrpedido, modelodocumento = @modelodocumento, " +
        //      "nrordemequipamento = @nrordemequipamento, nrdocemitido = @nrdocemitido, uf_embarq = @uf_embarq, lc_embarq = @lc_embarq, finalidade = @finalidade, " +
        //      "danfe = @danfe, justifica = @justifica, chavedev = @chavedev, nrnfedev = @nrnfedev, datacontingencia = @datacontingencia, chavedeacesso = @chavedeacesso, protocolo = @protocolo, " +

        //      "cnpj_cpf_entrega = @cnpj_cpf_entrega, ie_entrega = @ie_entrega, xnome_entrega = @xnome_entrega, xlgr_entrega = @xlgr_entrega, nro_entrega = @nro_entrega, " +
        //      "xcpl_entrega = @xcpl_entrega, xbairro_entrega = @xbairro_entrega, cmun_entrega = @cmun_entrega, xmun_entrega = @xmun_entrega, uf_entrega = @uf_entrega, cep_entrega = @cep_entrega, cpais_entrega = @cpais_entrega, " +
        //      "xpais_entrega = @xpais_entrega, fone_entrega = @fone_entrega, email_entrega = @email_entrega, cnpj_cpf_retirada = @cnpj_cpf_retirada, " +
        //      "ie_retirada = @ie_retirada, xnome_retirada = @xnome_retirada, xlgr_retirada = @xlgr_retirada, nro_retirada = @nro_retirada, " +
        //      "xcpl_retirada = @xcpl_retirada,  xbairro_retirada = @xbairro_retirada, cmun_retirada = @cmun_retirada, xmun_retirada = @xmun_retirada, uf_retirada = @uf_retirada, cep_retirada = @cep_retirada, " +
        //      "cpais_retirada = @cpais_retirada, xpais_retirada = @xpais_retirada, fone_retirada = @fone_retirada, email_retirada = @email_retirada " +

        //      "where id_vendanfe = @id_vendanfe ";
        //    try
        //    {
        //        MySQLBase basemysql = new MySQLBase();
        //        MySqlCommand cmd = basemysql.connection.CreateCommand();
        //        cmd.CommandText = sql;
        //        cmd.CommandTimeout = 1000;
        //        cmd.Parameters.AddWithValue("@id_vendanfe", id_vendanfe);
        //        cmd.Parameters.AddWithValue("@nrvenda", nrvenda);
        //        cmd.Parameters.AddWithValue("@dhEmi", dhEmi);
        //        cmd.Parameters.AddWithValue("@dhSaiEnt", dhSaiEnt);
        //        cmd.Parameters.AddWithValue("@indfinal", indfinal);
        //        cmd.Parameters.AddWithValue("@indpres", indpres);
        //        cmd.Parameters.AddWithValue("@tpnf", tpnf);
        //        cmd.Parameters.AddWithValue("@tpEmis", tpEmis);
        //        cmd.Parameters.AddWithValue("@idop", idop);
        //        cmd.Parameters.AddWithValue("@contrib", contrib);
        //        cmd.Parameters.AddWithValue("@id_cliente", id_cliente);
        //        cmd.Parameters.AddWithValue("@nomecliente", nomecliente.Trim());
        //        cmd.Parameters.AddWithValue("@id_vendedor", id_vendedor);
        //        cmd.Parameters.AddWithValue("@obs", obs);
        //        cmd.Parameters.AddWithValue("@infadfisco", infadfisco);
        //        cmd.Parameters.AddWithValue("@endereco", endereco.Trim());
        //        cmd.Parameters.AddWithValue("@numero", numero);
        //        cmd.Parameters.AddWithValue("@complemento", complemento);
        //        cmd.Parameters.AddWithValue("@bairro", bairro.Trim());
        //        cmd.Parameters.AddWithValue("@cmun", cmun);
        //        cmd.Parameters.AddWithValue("@cidade", cidade.Trim());
        //        cmd.Parameters.AddWithValue("@uf", uf);
        //        cmd.Parameters.AddWithValue("@cpais", cpais);
        //        cmd.Parameters.AddWithValue("@pais", pais);
        //        cmd.Parameters.AddWithValue("@cep", cep);
        //        cmd.Parameters.AddWithValue("@fone", fone);
        //        cmd.Parameters.AddWithValue("@email", email);
        //        cmd.Parameters.AddWithValue("@cnpj_cpf", cnpj_cpf);
        //        cmd.Parameters.AddWithValue("@ie_rg", ie_rg);
        //        cmd.Parameters.AddWithValue("@im", im);
        //        cmd.Parameters.AddWithValue("@vl_entrega", vl_entrega);
        //        cmd.Parameters.AddWithValue("@comissao_total", comissao_total);
        //        cmd.Parameters.AddWithValue("@status", status);
        //        cmd.Parameters.AddWithValue("@natop", natop);
        //        cmd.Parameters.AddWithValue("@cfop", cfop);
        //        cmd.Parameters.AddWithValue("@serie", serie);
        //        cmd.Parameters.AddWithValue("@vbc", vbc);
        //        cmd.Parameters.AddWithValue("@vicms", vicms);
        //        cmd.Parameters.AddWithValue("@vicmsdeson", vicmsdeson);
        //        cmd.Parameters.AddWithValue("@vfcpufdest", vfcpufdest);
        //        cmd.Parameters.AddWithValue("@vicmsufdest", vicmsufdest);
        //        cmd.Parameters.AddWithValue("@vicmsufremet", vicmsufremet);
        //        cmd.Parameters.AddWithValue("@vfcp", vfcp);
        //        cmd.Parameters.AddWithValue("@vbcst", vbcst);
        //        cmd.Parameters.AddWithValue("@vst", vst);
        //        cmd.Parameters.AddWithValue("@vfcpst", vfcpst);
        //        cmd.Parameters.AddWithValue("@vfcpstret", vfcpstret);
        //        cmd.Parameters.AddWithValue("@vprod", vprod);
        //        cmd.Parameters.AddWithValue("@vfrete", vfrete);
        //        cmd.Parameters.AddWithValue("@vseg", vseg);
        //        cmd.Parameters.AddWithValue("@vdesc", vdesc);
        //        cmd.Parameters.AddWithValue("@vii", vii);
        //        cmd.Parameters.AddWithValue("@vipi", vipi);
        //        cmd.Parameters.AddWithValue("@vipidevol", vipidevol);
        //        cmd.Parameters.AddWithValue("@vpis", vpis);
        //        cmd.Parameters.AddWithValue("@vconfins", vconfins);
        //        cmd.Parameters.AddWithValue("@voutro", voutro);
        //        cmd.Parameters.AddWithValue("@vnf", vnf);
        //        cmd.Parameters.AddWithValue("@vtroco", vtroco);
        //        cmd.Parameters.AddWithValue("@vtotaltrib", vtotaltrib);
        //        cmd.Parameters.AddWithValue("@modFrete", modFrete);
        //        cmd.Parameters.AddWithValue("@nome_transp", nome_transp);
        //        cmd.Parameters.AddWithValue("@cnpjcpf_transp", Funcoes.Deixarnumero(cnpjcpf_transp));
        //        cmd.Parameters.AddWithValue("@ie_transp", Funcoes.Deixarnumero(ie_transp));
        //        cmd.Parameters.AddWithValue("@uf_transp", uf_transp);
        //        cmd.Parameters.AddWithValue("@ender_transp", ender_transp.Trim());
        //        cmd.Parameters.AddWithValue("@mun_transp", mun_transp.Trim());
        //        cmd.Parameters.AddWithValue("@rntc_transp", rntc_transp);
        //        cmd.Parameters.AddWithValue("@placa_transp", placa_transp);
        //        cmd.Parameters.AddWithValue("@ufveiculo_transp", ufveiculo_transp);
        //        cmd.Parameters.AddWithValue("@qVol_transp", qVol_transp);
        //        cmd.Parameters.AddWithValue("@esp_transp", esp_transp);
        //        cmd.Parameters.AddWithValue("@marca_transp", marca_transp);
        //        cmd.Parameters.AddWithValue("@nvol_transp", nvol_transp);
        //        cmd.Parameters.AddWithValue("@pesol_transp", pesol_transp);
        //        cmd.Parameters.AddWithValue("@pesob_transp", pesob_transp);
        //        cmd.Parameters.AddWithValue("@nlacre_transp", nlacre_transp);
        //        cmd.Parameters.AddWithValue("@nrpedido", nrpedido);
        //        cmd.Parameters.AddWithValue("@modelodocumento", modelodocumento);
        //        cmd.Parameters.AddWithValue("@nrordemequipamento", nrordemequipamento);
        //        cmd.Parameters.AddWithValue("@nrdocemitido", nrdocemitido);
        //        cmd.Parameters.AddWithValue("@uf_embarq", uf_embarq);
        //        cmd.Parameters.AddWithValue("@lc_embarq", lc_embarq);
        //        cmd.Parameters.AddWithValue("@finalidade", finalidade);
        //        cmd.Parameters.AddWithValue("@danfe", danfe);
        //        cmd.Parameters.AddWithValue("@justifica", justifica);
        //        cmd.Parameters.AddWithValue("@chavedev", chavedev);
        //        cmd.Parameters.AddWithValue("@nrnfedev", nrnfedev);
        //        cmd.Parameters.AddWithValue("@datacontingencia", datacontingencia);
        //        cmd.Parameters.AddWithValue("@chavedeacesso", chavedeacesso);
        //        cmd.Parameters.AddWithValue("@protocolo", protocolo);

        //        cmd.Parameters.AddWithValue("cnpj_cpf_entrega", Funcoes.Deixarnumero(cnpj_cpf_entrega));
        //        cmd.Parameters.AddWithValue("ie_entrega", Funcoes.Deixarnumero(ie_entrega));
        //        cmd.Parameters.AddWithValue("xnome_entrega", xnome_entrega);
        //        cmd.Parameters.AddWithValue("xlgr_entrega", xlgr_entrega);
        //        cmd.Parameters.AddWithValue("nro_entrega", nro_entrega);
        //        cmd.Parameters.AddWithValue("xcpl_entrega", xcpl_entrega);
        //        cmd.Parameters.AddWithValue("xbairro_entrega", xbairro_entrega);
        //        cmd.Parameters.AddWithValue("cmun_entrega", cmun_entrega);
        //        cmd.Parameters.AddWithValue("xmun_entrega", xmun_entrega);
        //        cmd.Parameters.AddWithValue("uf_entrega", uf_entrega);
        //        cmd.Parameters.AddWithValue("cep_entrega", Funcoes.Deixarnumero(cep_entrega));
        //        cmd.Parameters.AddWithValue("cpais_entrega", cpais_entrega);
        //        cmd.Parameters.AddWithValue("xpais_entrega", xpais_entrega);
        //        cmd.Parameters.AddWithValue("fone_entrega", Funcoes.Deixarnumero(fone_entrega));
        //        cmd.Parameters.AddWithValue("email_entrega", email_entrega);
        //        cmd.Parameters.AddWithValue("cnpj_cpf_retirada", Funcoes.Deixarnumero(cnpj_cpf_retirada));
        //        cmd.Parameters.AddWithValue("ie_retirada", Funcoes.Deixarnumero(ie_retirada));
        //        cmd.Parameters.AddWithValue("xnome_retirada", xnome_retirada);
        //        cmd.Parameters.AddWithValue("xlgr_retirada", xlgr_retirada);
        //        cmd.Parameters.AddWithValue("nro_retirada", nro_retirada);
        //        cmd.Parameters.AddWithValue("xcpl_retirada", xcpl_retirada);
        //        cmd.Parameters.AddWithValue("xbairro_retirada", xbairro_retirada);
        //        cmd.Parameters.AddWithValue("cmun_retirada", cmun_retirada);
        //        cmd.Parameters.AddWithValue("xmun_retirada", xmun_retirada);
        //        cmd.Parameters.AddWithValue("uf_retirada", uf_retirada);
        //        cmd.Parameters.AddWithValue("cep_retirada", Funcoes.Deixarnumero(cep_retirada));
        //        cmd.Parameters.AddWithValue("cpais_retirada", cpais_retirada);
        //        cmd.Parameters.AddWithValue("xpais_retirada", xpais_retirada);
        //        cmd.Parameters.AddWithValue("fone_retirada", Funcoes.Deixarnumero(fone_retirada));
        //        cmd.Parameters.AddWithValue("email_retirada", email_retirada);


        //        cmd.ExecuteNonQuery();
        //        basemysql.Closer();
        //    }
        //    catch (Exception ex)
        //    {
        //        Funcoes.Crashe(ex, "", false);
        //    }
        //}

        //public void Delete()
        //{
        //    string sql = @"DELETE FROM vendanfe WHERE id_venda = @id_venda ";
        //    try
        //    {
        //        MySQLBase basemysql = new MySQLBase();
        //        MySqlCommand cmd = basemysql.connection.CreateCommand();
        //        cmd.CommandText = sql;
        //        cmd.CommandTimeout = 1000;
        //        cmd.Parameters.AddWithValue("@id_vendanfe", id_vendanfe);
        //        cmd.ExecuteNonQuery();
        //        basemysql.Closer();
        //    }
        //    catch (Exception ex)
        //    {
        //        Funcoes.Crashe(ex, "", false);
        //    }
        //    id_vendanfe = 0;
        //}


        //private List<VendaNFe> GetConsulta(string sql)
        //{
        //    List<VendaNFe> lista = new List<VendaNFe>();
        //    VendaNFe venda;

        //    try
        //    {
        //        MySQLBase basemysql = new MySQLBase();
        //        using (var command = new MySqlCommand(sql, basemysql.connection))
        //        {
        //            MySqlDataReader reader = command.ExecuteReader();
        //            if (reader.HasRows)
        //            {
        //                DataTable dt = new DataTable();
        //                dt.Load(reader);
        //                reader.Close();

        //                foreach (DataRow row in dt.Rows)
        //                {
        //                    venda = new VendaNFe()
        //                    {
        //                        id_vendanfe = (int)row["id_vendanfe"],
        //                        nrvenda = (int)row["nrvenda"],
        //                        dhEmi = (DateTime)row["dhEmi"],
        //                        dhSaiEnt = (DateTime)row["dhSaiEnt"],
        //                        indfinal = (int)row["indfinal"],
        //                        indpres = (int)row["indpres"],
        //                        tpnf = (int)row["tpnf"],
        //                        tpEmis = (int)row["tpEmis"],
        //                        idop = (int)row["idop"],
        //                        contrib = (int)row["contrib"],
        //                        id_cliente = (int)row["id_cliente"],
        //                        nomecliente = row["nomecliente"].ToString(),
        //                        id_vendedor = (int)row["id_vendedor"],
        //                        obs = row["obs"].ToString(),
        //                        infadfisco = row["infadfisco"].ToString(),
        //                        endereco = row["endereco"].ToString(),
        //                        numero = row["numero"].ToString(),
        //                        complemento = row["complemento"].ToString(),
        //                        bairro = row["bairro"].ToString(),
        //                        cmun = (int)row["cmun"],
        //                        cidade = row["cidade"].ToString(),
        //                        uf = row["uf"].ToString(),
        //                        cpais = (int)row["cpais"],
        //                        pais = row["pais"].ToString(),
        //                        cep = row["cep"].ToString(),
        //                        fone = row["fone"].ToString(),
        //                        email = row["email"].ToString(),
        //                        cnpj_cpf = row["cnpj_cpf"].ToString(),
        //                        ie_rg = row["ie_rg"].ToString(),
        //                        im = row["im"].ToString(),
        //                        vl_entrega = (decimal)row["vl_entrega"],
        //                        comissao_total = (decimal)row["comissao_total"],
        //                        status = row["status"].ToString(),
        //                        natop = row["natop"].ToString(),
        //                        cfop = row["cfop"].ToString(),
        //                        serie = (int)row["serie"],
        //                        vbc = (decimal)row["vbc"],
        //                        vicms = (decimal)row["vicms"],
        //                        vicmsdeson = (decimal)row["vicmsdeson"],
        //                        vfcpufdest = (decimal)row["vfcpufdest"],
        //                        vicmsufdest = (decimal)row["vicmsufdest"],
        //                        vicmsufremet = (decimal)row["vicmsufremet"],
        //                        vfcp = (decimal)row["vfcp"],
        //                        vbcst = (decimal)row["vbcst"],
        //                        vst = (decimal)row["vst"],
        //                        vfcpst = (decimal)row["vfcpst"],
        //                        vfcpstret = (decimal)row["vfcpstret"],
        //                        vprod = (decimal)row["vprod"],
        //                        vfrete = (decimal)row["vfrete"],
        //                        vseg = (decimal)row["vseg"],
        //                        vdesc = (decimal)row["vdesc"],
        //                        vii = (decimal)row["vii"],
        //                        vipi = (decimal)row["vipi"],
        //                        vipidevol = (decimal)row["vipidevol"],
        //                        vpis = (decimal)row["vpis"],
        //                        vconfins = (decimal)row["vconfins"],
        //                        voutro = (decimal)row["voutro"],
        //                        vnf = (decimal)row["vnf"],
        //                        vtroco = (decimal)row["vtroco"],
        //                        vtotaltrib = (decimal)row["vtotaltrib"],
        //                        modFrete = (int)row["modFrete"],
        //                        nome_transp = row["nome_transp"].ToString(),
        //                        cnpjcpf_transp = row["cnpjcpf_transp"].ToString(),
        //                        ie_transp = row["ie_transp"].ToString(),
        //                        uf_transp = row["uf_transp"].ToString(),
        //                        ender_transp = row["ender_transp"].ToString(),
        //                        mun_transp = row["mun_transp"].ToString(),
        //                        rntc_transp = row["rntc_transp"].ToString(),
        //                        placa_transp = row["placa_transp"].ToString(),
        //                        ufveiculo_transp = row["ufveiculo_transp"].ToString(),
        //                        qVol_transp = (int)row["qVol_transp"],
        //                        esp_transp = row["esp_transp"].ToString(),
        //                        marca_transp = row["marca_transp"].ToString(),
        //                        nvol_transp = row["nvol_transp"].ToString(),
        //                        pesol_transp = (decimal)row["pesol_transp"],
        //                        pesob_transp = (decimal)row["pesob_transp"],
        //                        nlacre_transp = row["nlacre_transp"].ToString(),
        //                        nrpedido = row["nrpedido"].ToString(),
        //                        modelodocumento = row["modelodocumento"].ToString(),
        //                        nrordemequipamento = row["nrordemequipamento"].ToString(),
        //                        nrdocemitido = row["nrdocemitido"].ToString(),
        //                        uf_embarq = row["uf_embarq"].ToString(),
        //                        lc_embarq = row["lc_embarq"].ToString(),
        //                        finalidade = (int)row["finalidade"],
        //                        danfe = (int)row["danfe"],
        //                        justifica = row["justifica"].ToString(),
        //                        chavedev = row["chavedev"].ToString(),
        //                        nrnfedev = row["nrnfedev"].ToString(),
        //                        datacontingencia = (DateTime)row["datacontingencia"],
        //                        chavedeacesso = row["chavedeacesso"].ToString(),
        //                        protocolo = row["protocolo"].ToString(),

        //                        cnpj_cpf_entrega = row["cnpj_cpf_entrega"].ToString(),
        //                        ie_entrega = row["ie_entrega"].ToString(),
        //                        xnome_entrega = row["xnome_entrega"].ToString(),
        //                        xlgr_entrega = row["xlgr_entrega"].ToString(),
        //                        nro_entrega = row["nro_entrega"].ToString(),
        //                        xcpl_entrega = row["xcpl_entrega"].ToString(),
        //                        xbairro_entrega = row["xbairro_entrega"].ToString(),
        //                        cmun_entrega = (int)row["cmun_entrega"],
        //                        xmun_entrega = row["xmun_entrega"].ToString(),
        //                        uf_entrega = row["uf_entrega"].ToString(),
        //                        cep_entrega = row["cep_entrega"].ToString(),
        //                        cpais_entrega = (int)row["cpais_entrega"],
        //                        xpais_entrega = row["xpais_entrega"].ToString(),
        //                        fone_entrega = row["fone_entrega"].ToString(),
        //                        email_entrega = row["email_entrega"].ToString(),
        //                        cnpj_cpf_retirada = row["cnpj_cpf_retirada"].ToString(),
        //                        ie_retirada = row["ie_retirada"].ToString(),
        //                        xnome_retirada = row["xnome_retirada"].ToString(),
        //                        xlgr_retirada = row["xlgr_retirada"].ToString(),
        //                        nro_retirada = row["nro_retirada"].ToString(),
        //                        xcpl_retirada = row["xcpl_retirada"].ToString(),
        //                        xbairro_retirada = row["xbairro_retirada"].ToString(),
        //                        cmun_retirada = (int)row["cmun_retirada"],
        //                        xmun_retirada = row["xmun_retirada"].ToString(),
        //                        uf_retirada = row["uf_retirada"].ToString(),
        //                        cep_retirada = row["cep_retirada"].ToString(),
        //                        cpais_retirada = (int)row["cpais_retirada"],
        //                        xpais_retirada = row["xpais_retirada"].ToString(),
        //                        fone_retirada = row["fone_retirada"].ToString(),
        //                        email_retirada = row["email_retirada"].ToString(),

        //                    };
        //                    lista.Add(venda);

        //                }
        //            }
        //            reader.Close();    // Fecha o DataReader  
        //        }
        //        basemysql.Closer();

        //        return lista;
        //    }
        //    catch (Exception ex)
        //    {
        //        Funcoes.Crashe(ex, "", false);
        //        return lista;
        //    }

        //}

        //public List<VendaNFe> GetTodasVendas()
        //{
        //    String sql = @"SELECT * FROM vendanfe where 1 order by id_vendanfe asc";
        //    return GetConsulta(sql);
        //}

        //public List<VendaNFe> GetUsuarioNotasPorPeriodo(DateTime inicial, DateTime final, string sts = "", string groupby = "")
        //{
        //    String sql = @"SELECT * FROM vendanfe where ";
        //    if (sts != "") sql = sql + " status = '" + sts + "' AND ";
        //    sql = sql + " dhEmi BETWEEN '" + inicial.ToString("yyyy-MM-dd") + "' AND '" + final.ToString("yyyy-MM-dd") + "' ";
        //    if (groupby != "")
        //        sql = sql + "GROUP BY " + groupby;
        //    return GetConsulta(sql);
        //}

        //public List<VendaNFe> GetRelatorioVendas(DateTime inicial, DateTime final, string sts = "")
        //{
        //    String sql = @"SELECT * FROM vendanfe where ";
        //    if (sts != "") sql = sql + " status = '" + sts + "' AND ";
        //    sql = sql + " dhEmi BETWEEN '" + inicial.ToString("yyyy-MM-dd") + "' " +
        //        "AND '" + final.ToString("yyyy-MM-dd") + "' " +
        //        "order by id_vendanfe asc";
        //    return GetConsulta(sql);
        //}

        //public List<VendaNFe> GetBuscaNFE(string busca)
        //{
        //    String sql = @"SELECT * FROM vendanfe where nomecliente LIKE '%" + busca.Replace("'", @"\'") + "%' " +
        //                     " OR  nrvenda LIKE '%" + busca.Replace("'", @"\'") + "%' ";
        //    //  " OR  nomecliente   LIKE '% " + busca.Replace("'", @"\'") + "%' ";
        //    return GetConsulta(sql);
        //}

        //public int GetUltimaVenda()
        //{
        //    string instrucao = @"SELECT MAX(id_vendanfe) from vendanfe ";
        //    MySQLBase mysql = new MySQLBase();
        //    string retorno = mysql.Executacomando(instrucao);
        //    if (retorno == "" || retorno == null) retorno = "0";
        //    mysql.Closer();

        //    return Convert.ToInt32(retorno);
        //}

        //public String GetUltimoNumerodeVenda()
        //{
        //    String instrucao = @"SELECT MAX(nrvenda) from vendanfe ";
        //    MySQLBase mysql = new MySQLBase();
        //    string retorno = mysql.Executacomando(instrucao);
        //    mysql.Closer();
        //    if (retorno == "" || retorno == null) retorno = "0";
        //    return retorno;
        //}


        //public String ExisteVenda(string numerovenda)
        //{
        //    String instrucao = @"SELECT id_vendanfe from vendanfe where nrvenda = " + numerovenda;
        //    MySQLBase mysql = new MySQLBase();
        //    string retorno = mysql.Executacomando(instrucao);
        //    mysql.Closer();
        //    return retorno;
        //}

        //public int NovaVenda(int numerovenda)
        //{
        //    nrvenda = numerovenda;
        //    Insert();
        //    GetVendaPorNrVenda(numerovenda);
        //    return id_vendanfe;
        //}
        public void Update()
        {
            try
            {
                DbfBase ebase = new DbfBase();
                String instrucao = @"Update " + ebase.Path + @"\NFCE.dbf SET statusnfe = '" + Status + "' " +                   
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
                    Funcoes.Crashe(ex, "TABELA ABERTA NO CIAF", false);

            }

        }

    }

}
