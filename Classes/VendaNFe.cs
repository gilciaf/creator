using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nfecreator
{
    class VendaNFe
    {
        int id_vendanfe = 0;
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


        string mod;
        string nECF;
        string nCOO;

        string ru_nota;
        string ru_serie;
        string ru_datae;
        string ru_cpfcnpj;
        string ru_ieprod;
        string ru_ufprod;
        
        // REFORMA TRIBUTÁRIA
        decimal vtot_is;
        
        decimal vtot_bc_ibscbs;
        
        string vtot_uf_dif;
        string vtot_uf_dev_trib;
        string vtot_uf_ibs;
        
        string vtot_mun_dif;
        string vtot_mun_dev_trib;
        string vtot_mun_ibs;
        
        decimal vtot_dif_cbs;
        decimal vtot_dev_trib_cbs;
        string vtot_cbs;
        decimal vtot_cred_pres;
        string vtot_cred_pres_cond_sus;
        
        decimal vtot_ibs_mono;
        decimal vtot_cbs_mono;
        decimal vtot_ibs_mono_reten;
        decimal vtot_cbs_mono_reten;
        decimal vtot_ibs_mono_ret;
        decimal vtot_cbs_mono_ret;

        public int Id_vendanfe { get => id_vendanfe; set => id_vendanfe = value; }
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

        public decimal VtotIs
        {
            get => vtot_is;
            set => vtot_is = value;
        }

        public decimal VtotBcIbscbs
        {
            get => vtot_bc_ibscbs;
            set => vtot_bc_ibscbs = value;
        }

        public string VtotUfDif
        {
            get => vtot_uf_dif;
            set => vtot_uf_dif = value;
        }

        public string VtotUfDevTrib
        {
            get => vtot_uf_dev_trib;
            set => vtot_uf_dev_trib = value;
        }

        public string VtotUfIbs
        {
            get => vtot_uf_ibs;
            set => vtot_uf_ibs = value;
        }

        public string VtotMunDif
        {
            get => vtot_mun_dif;
            set => vtot_mun_dif = value;
        }

        public string VtotMunDevTrib
        {
            get => vtot_mun_dev_trib;
            set => vtot_mun_dev_trib = value;
        }

        public string VtotMunIbs
        {
            get => vtot_mun_ibs;
            set => vtot_mun_ibs = value;
        }

        public Decimal VtotDevTribCbs
        {
            get => vtot_dev_trib_cbs;
            set => vtot_dev_trib_cbs = value;
        }

        public Decimal VtotDifCbs
        {
            get => vtot_dif_cbs;
            set => vtot_dif_cbs = value;
        }

        public string VtotCbs
        {
            get => vtot_cbs;
            set => vtot_cbs = value;
        }

        public decimal VtotCredPres
        {
            get => vtot_cred_pres;
            set => vtot_cred_pres = value;
        }

        public string VtotCredPresCondSus
        {
            get => vtot_cred_pres_cond_sus;
            set => vtot_cred_pres_cond_sus = value;
        }

        public decimal VtotIbsMono
        {
            get => vtot_ibs_mono;
            set => vtot_ibs_mono = value;
        }

        public decimal VtotCbsMono
        {
            get => vtot_cbs_mono;
            set => vtot_cbs_mono = value;
        }

        public decimal VtotIbsMonoReten
        {
            get => vtot_ibs_mono_reten;
            set => vtot_ibs_mono_reten = value;
        }

        public decimal VtotCbsMonoReten
        {
            get => vtot_cbs_mono_reten;
            set => vtot_cbs_mono_reten = value;
        }

        public decimal VtotIbsMonoRet
        {
            get => vtot_ibs_mono_ret;
            set => vtot_ibs_mono_ret = value;
        }

        public decimal VtotCbsMonoRet
        {
            get => vtot_cbs_mono_ret;
            set => vtot_cbs_mono_ret = value;
        }

        public string ColorStatus
        {
            get => status == "PENDENTE" ? "#0088cc" :
                   status == "APROVADO" ? "#47a447" :
                   status == "INUTILIZADO" ? "#ed9c28" :
                   status == "CANCELADO" ? "#d2322d" :
                   status == "DENEGADO" ? "#777777" :
                   status == "REJEITADO" ? "#5bc0de" : "#0088cc";
        }
        public string Mod { get => mod; set => mod = value; }
        public string NECF { get => nECF; set => nECF = value; }
        public string NCOO { get => nCOO; set => nCOO = value; }
        public string Ru_nota { get => ru_nota; set => ru_nota = value; }
        public string Ru_serie { get => ru_serie; set => ru_serie = value; }
        public string Ru_datae { get => ru_datae; set => ru_datae = value; }
        public string Ru_cpfcnpj { get => ru_cpfcnpj; set => ru_cpfcnpj = value; }
        public string Ru_ieprod { get => ru_ieprod; set => ru_ieprod = value; }
        public string Ru_ufprod { get => ru_ufprod; set => ru_ufprod = value; }

        public VendaNFe()
        { }

        public VendaNFe(int id)
        {
            try 
            {
                DbfBase ebase = new DbfBase();
                string instrucao = @"SELECT * from " + ebase.Path + @"\VENDANFE.dbf WHERE nrvenda = "+ id + " ";
                
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
                    if (row["serie"].ToString().Trim() != "")
                        serie = Convert.ToInt32(row["serie"].ToString().Trim());
                    else
                        Funcoes.Mensagem("Nota sem série, favor ajustar essa informação nos Parametros.", "SÉRIE DA NOTA", System.Windows.MessageBoxButton.OK);
                    dhEmi = Convert.ToDateTime(row["datae"].ToString().Trim());
                    dhEmi = Convert.ToDateTime(dhEmi.ToString("dd/MM/yyyy ") + row["horaensai"].ToString().Trim());

                    dhSaiEnt = Convert.ToDateTime(row["datas"].ToString().Trim());
                    dhSaiEnt = Convert.ToDateTime(dhSaiEnt.ToString("dd/MM/yyyy ") + row["horaensai"].ToString().Trim());
                    
                    cfop = row["cfop"].ToString().Trim();
                    natop = row["dcfop"].ToString().Trim();

                    // = Convert.ToInt32(row["nmvendedor"].ToString().Trim());
                    id_cliente = row["codcliente"].ToString().Trim();
                    nomecliente =  row["nmcliente"].ToString().Trim();
                   // Console.WriteLine(nomecliente + " - nome do cliente");
                    endereco = row["endereco"].ToString().Trim();
                    numero = row["numero"].ToString().Trim();
                    complemento = row["complemento"].ToString().Trim();
                    bairro = row["bairro"].ToString().Trim();
                    cidade = row["cidade"].ToString().Trim();
                    uf = row["uf"].ToString().Trim();
                    pais = row["pais"].ToString().Trim();

                    cmun = Convert.ToInt32(row["codcidade"].ToString().Trim());
                    //coduf = Convert.ToInt32(row["coduf"].ToString().Trim());
                    if(row["codpais"].ToString().Trim()!="")
                    cpais = Convert.ToInt32(row["codpais"].ToString().Trim());

                    cnpj_cpf = Funcoes.Deixarnumero(row["cnpj"].ToString().Trim());
                    ie_rg = Funcoes.Deixarnumero(row["iestadual"].ToString().Trim());

                    fone = row["fone"].ToString().Trim().Replace("(", "").Replace(")", "");
                    fone_entrega = "";
                    fone_retirada = "";


                    cep = row["cep"].ToString().Trim();

                    vbc = Convert.ToDecimal(row["bcalicms"].ToString().Trim());
                    vicms = Convert.ToDecimal(row["valoricms"].ToString().Trim());

                    email = "";
                     
                   
                    ModFrete = Convert.ToInt32( row["freteconta"].ToString().Trim() );

                    vfrete = Convert.ToDecimal(row["frete"].ToString().Trim());
                    vseg = Convert.ToDecimal(row["seguro"].ToString().Trim());
                    vdesc = Convert.ToDecimal(row["desconto"].ToString().Trim());
                    voutro =  Convert.ToDecimal(row["acessorias"].ToString().Trim());

                    vipi = Convert.ToDecimal(row["valoripi"].ToString().Trim());
                    vprod = Convert.ToDecimal(row["valorprodutos"].ToString().Trim());
                    vnf = Convert.ToDecimal(row["valornota"].ToString().Trim());

                    nome_transp = row["transnome"].ToString().Trim();
                    //freteconta
                    rntc_transp = row["codigoantt"].ToString().Trim();
                    placa_transp = row["placav"].ToString().Trim();
                    uf_transp = row["transuf"].ToString().Trim();

                    
                    cnpjcpf_transp = row["cnpjcpfmo"].ToString().Trim();
                    ender_transp = row["transende"].ToString().Trim();
                    mun_transp = row["transcidade"].ToString().Trim();
                    ufveiculo_transp = row["ufv"].ToString().Trim();
                    ie_transp = row["transiestadual"].ToString().Trim();

                    qVol_transp = Convert.ToInt32(Convert.ToDecimal(row["quantidade"].ToString().Trim()));
                    esp_transp = row["especie"].ToString().Trim();
                    marca_transp = row["marca"].ToString().Trim();
                    nvol_transp = row["numeracao"].ToString().Trim();
                    pesol_transp = Convert.ToDecimal(row["pesol"].ToString().Trim());
                    pesob_transp = Convert.ToDecimal(row["pesob"].ToString().Trim());

                    obs = row["obsnf"].ToString().Trim();
                    status = row["statusnfe"].ToString().Trim();

                    //aventrada
                    //qtparcela
                    //vlparcela
                    //comissaov comissao_total = reader.GetDecimal("comissao_total");

                    vpis = Convert.ToDecimal(row["valpis"].ToString().Trim());                    
                    vconfins = Convert.ToDecimal(row["valcofins"].ToString().Trim());

                    cnpj_cpf_entrega = row["lentrega"].ToString().Trim();
                    cnpj_cpf_retirada = row["lretirada"].ToString().Trim();
                    //lentrega
                    //lretirada

                    vii = Convert.ToDecimal(row["valorii"].ToString().Trim());

                    vbcst = Convert.ToDecimal(row["bcalicmssub"].ToString().Trim());
                    vst = Convert.ToDecimal(row["valicmssub"].ToString().Trim());
                    //vst = Convert.ToDecimal(row["valstret"].ToString().Trim());
                    //valicmssub



                    uf_embarq = row["ufembarq"].ToString().Trim();
                    lc_embarq = row["lcembarq"].ToString().Trim();

                    //formapgauto
                    //dformapgauto
                    //vmimpostos
                    //pmimpostos
                   
                    nrpedido = row["nrpedido"].ToString().Trim();
                    //razaos
                    indfinal = Convert.ToInt32(row["consumidorf"].ToString().Trim());
                    //coprese

                    indpres = Convert.ToInt32(row["coprese"].ToString().Trim());
                    idop = Convert.ToInt32(row["idope"].ToString().Trim());

                  


                    if (row["horacon"].ToString().Replace(" ", "").Replace(":", "").Trim() == "") datacontingencia = Convert.ToDateTime(row["datacon"].ToString().Trim());
                    else datacontingencia = Convert.ToDateTime(row["datacon"].ToString().Replace("00:00:00","").Trim() + " " + row["horacon"].ToString().Trim());
                    datacontingencia = Convert.ToDateTime(datacontingencia.ToString("dd/MM/yyyy HH:mm"));

                    if(row["tpemissao"].ToString().Trim() != "")
                    tpEmis = Convert.ToInt32(row["tpemissao"].ToString().Trim());
                    finalidade = Convert.ToInt32(row["finalidade"].ToString().Trim());
                    danfe = Convert.ToInt32(row["danfe"].ToString().Trim());
                    justifica = row["justifica"].ToString().Trim();

                    chavedev = row["chavedev"].ToString().Trim();                    
                    nrnfedev = row["nrnfedev"].ToString().Trim();

                    vfcpufdest = Convert.ToDecimal(row["vfcpufdest"].ToString().Trim());
                    vicmsufdest = Convert.ToDecimal(row["vicmsufdest"].ToString().Trim());
                    vicmsufremet = Convert.ToDecimal(row["vicmsufremet"].ToString().Trim());

                    //refmod
                    //refnecf
                    //refcoo


                    //ru_nota
                    ru_nota = row["ru_nota"].ToString().Trim();
                    if (!string.IsNullOrEmpty(ru_nota))
                    {
                        //ru_serie
                        ru_serie = row["ru_serie"].ToString().Trim();
                        //ru_datae
                        ru_datae = row["ru_datae"].ToString().Trim();
                        //ru_cpfcnpj
                        ru_cpfcnpj = row["ru_cpfcnpj"].ToString().Trim();
                        //ru_ieprod
                        ru_ieprod = row["ru_ieprod"].ToString().Trim();
                        //ru_ufprod
                        ru_ufprod = row["ru_ufprod"].ToString().Trim();
                    }
                  

                    //formapg
                    //meiopag
                    //recebido

                    vtroco = Convert.ToDecimal(row["troco"].ToString().Trim());

                    //formaciaf
                    //formadecia
                    //formaqpcia

                    //cdconveio
                    //nmconveio

                    vipidevol = Convert.ToDecimal(row["vipidevol"].ToString().Trim());
                    vfcp = Convert.ToDecimal(row["vfcp"].ToString().Trim());
                    vfcpst = Convert.ToDecimal(row["vfcpst"].ToString().Trim());
                    vfcpstret = Convert.ToDecimal(row["vfcpstret"].ToString().Trim());

                    infadfisco = row["obsfisco"].ToString().Trim();

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

                    mod = row["refmod"].ToString().Trim();
                    nECF = row["refnecf"].ToString().Trim();
                    nCOO = row["refcoo"].ToString().Trim();
                    
                    // REFORMA TRIBUTÁRIA
                    vtot_is = Convert.ToDecimal(row["vtot_is"].ToString().Trim());
                    vtot_bc_ibscbs = Convert.ToDecimal(row["vtot_bc_ibscbs"].ToString().Trim());
                    vtot_uf_dif = row["vtot_uf_dif"].ToString().Trim();
                    vtot_uf_dev_trib = row["vtot_uf_dev_trib"].ToString().Trim();
                    vtot_uf_ibs = row["vtot_uf_ibs"].ToString().Trim();
                    vtot_mun_dif = row["vtot_mun_dif"].ToString().Trim();
                    vtot_mun_dev_trib = row["vtot_mun_dev_trib"].ToString().Trim();
                    vtot_mun_ibs = row["vtot_mun_ibs"].ToString().Trim();
                    vtot_dif_cbs = Convert.ToDecimal(row["vtot_dif_cbs"].ToString().Trim());
                    vtot_dev_trib_cbs = Convert.ToDecimal(row["vtot_dev_trib_cbs"].ToString().Trim());
                    vtot_cbs = row["vtot_cbs"].ToString().Trim();
                    vtot_cred_pres = Convert.ToDecimal(row["vtot_cred_pres"].ToString().Trim());
                    vtot_cred_pres_cond_sus = row["vtot_cred_pres_cond_sus"].ToString().Trim();
                    vtot_ibs_mono = Convert.ToDecimal(row["vtot_ibs_mono"].ToString().Trim());
                    vtot_cbs_mono = Convert.ToDecimal(row["vtot_cbs_mono"].ToString().Trim());
                    vtot_ibs_mono_reten = Convert.ToDecimal(row["vtot_ibs_mono_reten"].ToString().Trim());
                    vtot_cbs_mono_reten = Convert.ToDecimal(row["vtot_cbs_mono_reten"].ToString().Trim());
                    vtot_ibs_mono_ret = Convert.ToDecimal(row["vtot_ibs_mono_ret"].ToString().Trim());
                    vtot_cbs_mono_ret = Convert.ToDecimal(row["vtot_cbs_mono_ret"].ToString().Trim());

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

        public void UpdateVipidevol()
        {
            try
            {
                DbfBase ebase = new DbfBase();
                String instrucao = @"Update " + ebase.Path + @"\VENDANFE.dbf SET vipidevol = " + Vipidevol.ToString().Replace(",", ".") +
                 " where nrvenda = " + nrvenda + " ";

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
                    Funcoes.Crashe(ex, "ERRO AO SALVAR IMPOSTO", true);

            }

        }

        public void UpdateICMS()
        {
            try
            {
                DbfBase ebase = new DbfBase();
                String instrucao = @"Update " + ebase.Path + @"\VENDANFE.dbf SET valoricms = " + Vicms.ToString().Replace(",", ".") +
                 " where nrvenda = " + nrvenda + " ";

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
                    Funcoes.Crashe(ex, "ERRO AO SALVAR IMPOSTO", true);

            }

        }

        public void UpdateValorNF()
        {
            try
            {
                DbfBase ebase = new DbfBase();
                String instrucao = @"Update " + ebase.Path + @"\VENDANFE.dbf SET valornota = " + Vnf.ToString().Replace(",", ".") +
                 " where nrvenda = " + nrvenda + " ";

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
                    Funcoes.Crashe(ex, "ERRO AO SALVAR IMPOSTO", true);

            }

        }



    }

}
