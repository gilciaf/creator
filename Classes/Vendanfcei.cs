using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nfecreator
{
    class VendaNFCeI
    {

        int id_vendanfcei;
        int id_vendanfce;
        string codigoproduto;
        string codigob;
        string descricao;
        string infadprod;
        decimal quantidade;
        decimal valor;
        decimal desconto;
        decimal vl_total;
        int indtot;
        decimal comissao_i;
        decimal comissao_g;
        decimal comissao_v;
        int nritem;
        string ncm;
        string cfop;
        string unidade;
        string utrib;
        decimal qtrib;
        decimal vuntrib;
        decimal vtottrib;
        decimal aicms;
        decimal aipi;
        decimal vicms;
        decimal vipi;
        decimal vipidevol;
        decimal bcicms;
        string cst;
        decimal pesol;
        string cstipi;
        string cstcofins;
        decimal vcofins;
        string cstpis;
        int stmodalidade;
        decimal mva;
        decimal strdbcst;
        decimal stbcicmsst;
        decimal stalicms;
        decimal stvalicmsst;
        decimal cofbc;
        decimal cofaliq;
        int cofflag;
        decimal cofbcst;
        decimal cofaliqst;
        decimal cofvalst;
        decimal frete;
        decimal vseg;
        decimal voutro;
        decimal pisbc;
        decimal pisaliq;
        decimal pisval;
        int pisflag;
        decimal pisbcst;
        decimal pisaliqst;
        decimal pisvalst;
        int origem;
        string stcodant;
        int i_flag;
        decimal i_vlbcalculo;
        decimal i_daduaneira;
        decimal i_vliof;
        decimal i_vlii;

        string nfci;

        string i_nrdi;
        DateTime i_dtregistro;
        string i_exportador;
        string i_ufdesemb;
        string i_localdesemb;
        DateTime i_dtdesemb;
        string i_adicao;
        string i_fabestra;
        decimal i_vldescadic;
        string i_nritemdi;
        decimal vbcstdest;
        decimal vicmsstdest;

        decimal vBCSTRet;
        decimal vICMSSubstituto;
        decimal vICMSSTRet;

        string codigoint;
        DateTime datae;
        decimal vlcusto;
        decimal pesob;
        string codigoanp;
        string descanp;
        string codif;
        decimal qtfattempa;
        string ufcombus;
        decimal bccombus;
        decimal alicombus;
        decimal valoricombus;
        string nrpedido;
        int i_tpviatransp;
        decimal i_afrmm;
        int i_tpintermedio;
        string i_cnpj;
        string i_ufterceiro;
        string nritemped;
        decimal vbcitem;
        string cest;
        decimal vbcufdest;
        decimal vbcfcpufdest;
        decimal pfcpufdest;
        decimal picmsufdest;
        decimal picmsinter;
        decimal picmsinterpa;
        decimal vfcpufdest;
        decimal vicmsufdest;
        decimal vicmsufremet;
        decimal vFCP;
        decimal vBCFCP;
        decimal pFCP;
        decimal vFCPST;
        decimal vBCFCPST;
        decimal pFCPST;
        decimal vFCPSTRet;
        decimal vBCFCPSTRet;
        decimal pFCPSTRet;
        decimal pST;

        int cenqipi;

        decimal perglp;
        decimal pergnat;
        decimal pergnat_i;
        decimal vlpartida;
        string indescala;
        string cnpjfab;
        string cBenef;
        string nomefab;

        decimal pCredSn;
        decimal vCredIcmssn;
        decimal pRedBCEfet;
        decimal vBCEfet;
        decimal pICMSEfet;
        decimal vICMSEfet;
        
        // REFORMA TRIBUTÁRIA 
        string cst_is;
        string cclass_trib_is;
        string v_bc_is;
        string p_is_espec;
        string p_is;
        
        string cst_ibscbs;
        string cclass_trib_ibscbs;
        
        string v_bc_ibscbs;
        string p_ibs_uf;
        string p_dif_uf_ibs;
        string v_dif_uf_ibs;
        string v_dev_trib_uf_ibs;
        string p_red_aliq_uf_ibs;
        string p_red_aliq_efet_uf_ibs;
        string v_ibs_uf;
        
        string p_ibs_mun;
        string p_dif_mun;
        string v_dif_mun;
        string v_dev_trib_mun;
        string p_red_aliq_efet_mun;
        string v_ibs_mun;
        
        string p_cbs;
        
        string p_dif_uf_cbs;
        string v_dev_trib_cbs;
        string p_red_aliq_cbs;
        string v_red_aliq_cbs;
        
        string v_cbs;
        
        string cst_reg;
        string cclass_trib_reg;
        string p_aliq_efet_reg_ibs_uf;
        string v_trib_reg_ibs_uf;
        string v_trib_reg_ibs_mun;
        string p_aliq_efet_reg_cbs;
        string v_trib_reg_cbs;
        
        
        public int Id_vendanfcei { get => id_vendanfcei; set => id_vendanfcei = value; }
        public int Id_vendanfce { get => id_vendanfce; set => id_vendanfce = value; }
        public string Codigoproduto { get => codigoproduto; set => codigoproduto = value; }
        public string Codigob { get => codigob; set => codigob = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public decimal Quantidade { get => quantidade; set => quantidade = value; }
        public decimal Valor { get => valor; set => valor = value; }
        public decimal Desconto { get => desconto; set => desconto = value; }
        public decimal Vl_total { get => vl_total; set => vl_total = value; }
        public decimal Comissao_i { get => comissao_i; set => comissao_i = value; }
        public decimal Comissao_g { get => comissao_g; set => comissao_g = value; }
        public decimal Comissao_v { get => comissao_v; set => comissao_v = value; }
        public int Nritem { get => nritem; set => nritem = value; }
        public string Ncm { get => ncm; set => ncm = value; }
        public string Cfop { get => cfop; set => cfop = value; }
        public string Unidade { get => unidade; set => unidade = value; }
        public string Utrib { get => utrib; set => utrib = value; }
        public decimal Qtrib { get => qtrib; set => qtrib = value; }
        public decimal Vuntrib { get => vuntrib; set => vuntrib = value; }
        public decimal Vtottrib { get => vtottrib; set => vtottrib = value; }
        public decimal Aicms { get => aicms; set => aicms = value; }
        public decimal Aipi { get => aipi; set => aipi = value; }
        public decimal Vicms { get => vicms; set => vicms = value; }
        public decimal Vipi { get => vipi; set => vipi = value; }
        public decimal Vipidevol { get => vipidevol; set => vipidevol = value; }
        public decimal Bcicms { get => bcicms; set => bcicms = value; }
        public string Cst { get => cst; set => cst = value; }
        public decimal Pesol { get => pesol; set => pesol = value; }
        public string Cstipi { get => cstipi; set => cstipi = value; }
        public string Cstcofins { get => cstcofins; set => cstcofins = value; }
        public decimal Vcofins { get => vcofins; set => vcofins = value; }
        public string Cstpis { get => cstpis; set => cstpis = value; }
        public int Stmodalidade { get => stmodalidade; set => stmodalidade = value; }
        public decimal Mva { get => mva; set => mva = value; }
        public decimal Strdbcst { get => strdbcst; set => strdbcst = value; }
        public decimal Stbcicmsst { get => stbcicmsst; set => stbcicmsst = value; }
        public decimal Stalicms { get => stalicms; set => stalicms = value; }
        public decimal Stvalicmsst { get => stvalicmsst; set => stvalicmsst = value; }
        public decimal Cofbc { get => cofbc; set => cofbc = value; }
        public decimal Cofaliq { get => cofaliq; set => cofaliq = value; }
        public int Cofflag { get => cofflag; set => cofflag = value; }
        public decimal Cofbcst { get => cofbcst; set => cofbcst = value; }
        public decimal Cofaliqst { get => cofaliqst; set => cofaliqst = value; }
        public decimal Cofvalst { get => cofvalst; set => cofvalst = value; }
        public decimal Frete { get => frete; set => frete = value; }
        public decimal Pisbc { get => pisbc; set => pisbc = value; }
        public decimal Pisaliq { get => pisaliq; set => pisaliq = value; }
        public decimal Pisval { get => pisval; set => pisval = value; }
        public int Pisflag { get => pisflag; set => pisflag = value; }
        public decimal Pisbcst { get => pisbcst; set => pisbcst = value; }
        public decimal Pisaliqst { get => pisaliqst; set => pisaliqst = value; }
        public decimal Pisvalst { get => pisvalst; set => pisvalst = value; }
        public int Origem { get => origem; set => origem = value; }
        public string Stcodant { get => stcodant; set => stcodant = value; }
        public int I_flag { get => i_flag; set => i_flag = value; }
        public decimal I_vlbcalculo { get => i_vlbcalculo; set => i_vlbcalculo = value; }
        public decimal I_daduaneira { get => i_daduaneira; set => i_daduaneira = value; }
        public decimal I_vliof { get => i_vliof; set => i_vliof = value; }
        public decimal I_vlii { get => i_vlii; set => i_vlii = value; }
        public string I_nrdi { get => i_nrdi; set => i_nrdi = value; }
        public DateTime I_dtregistro { get => i_dtregistro; set => i_dtregistro = value; }
        public string I_exportador { get => i_exportador; set => i_exportador = value; }
        public string I_ufdesemb { get => i_ufdesemb; set => i_ufdesemb = value; }
        public string I_localdesemb { get => i_localdesemb; set => i_localdesemb = value; }
        public DateTime I_dtdesemb { get => i_dtdesemb; set => i_dtdesemb = value; }
        public string I_adicao { get => i_adicao; set => i_adicao = value; }
        public string I_fabestra { get => i_fabestra; set => i_fabestra = value; }
        public decimal I_vldescadic { get => i_vldescadic; set => i_vldescadic = value; }
        public string I_nritemdi { get => i_nritemdi; set => i_nritemdi = value; }

        public string Codigoint { get => codigoint; set => codigoint = value; }
        public DateTime Datae { get => datae; set => datae = value; }
        public decimal Vlcusto { get => vlcusto; set => vlcusto = value; }
        public decimal Pesob { get => pesob; set => pesob = value; }
        public string Codigoanp { get => codigoanp; set => codigoanp = value; }
        public string Codif { get => codif; set => codif = value; }
        public decimal Qtfattempa { get => qtfattempa; set => qtfattempa = value; }
        public string Ufcombus { get => ufcombus; set => ufcombus = value; }
        public decimal Bccombus { get => bccombus; set => bccombus = value; }
        public decimal Alicombus { get => alicombus; set => alicombus = value; }
        public decimal Valoricombus { get => valoricombus; set => valoricombus = value; }
        public string Nrpedido { get => nrpedido; set => nrpedido = value; }
        public int I_tpviatransp { get => i_tpviatransp; set => i_tpviatransp = value; }
        public decimal I_afrmm { get => i_afrmm; set => i_afrmm = value; }
        public int I_tpintermedio { get => i_tpintermedio; set => i_tpintermedio = value; }
        public string I_cnpj { get => i_cnpj; set => i_cnpj = value; }
        public string I_ufterceiro { get => i_ufterceiro; set => i_ufterceiro = value; }
        public string Nritemped { get => nritemped; set => nritemped = value; }
        public decimal Vbcitem { get => vbcitem; set => vbcitem = value; }
        public string Cest { get => cest; set => cest = value; }
        public decimal Vbcufdest { get => vbcufdest; set => vbcufdest = value; }
        public decimal Pfcpufdest { get => pfcpufdest; set => pfcpufdest = value; }
        public decimal Picmsufdest { get => picmsufdest; set => picmsufdest = value; }
        public decimal Picmsinter { get => picmsinter; set => picmsinter = value; }
        public decimal Picmsinterpa { get => picmsinterpa; set => picmsinterpa = value; }
        public decimal Vfcpufdest { get => vfcpufdest; set => vfcpufdest = value; }
        public decimal Vicmsufdest { get => vicmsufdest; set => vicmsufdest = value; }
        public decimal Vicmsufremet { get => vicmsufremet; set => vicmsufremet = value; }
        public int Cenqipi { get => cenqipi; set => cenqipi = value; }
        public decimal VFCP { get => vFCP; set => vFCP = value; }
        public decimal VBCFCP { get => vBCFCP; set => vBCFCP = value; }
        public decimal PFCP { get => pFCP; set => pFCP = value; }
        public decimal VFCPST { get => vFCPST; set => vFCPST = value; }
        public decimal VBCFCPST { get => vBCFCPST; set => vBCFCPST = value; }
        public decimal PFCPST { get => pFCPST; set => pFCPST = value; }
        public decimal VFCPSTRet { get => vFCPSTRet; set => vFCPSTRet = value; }
        public decimal VBCFCPSTRet { get => vBCFCPSTRet; set => vBCFCPSTRet = value; }
        public decimal PFCPSTRet { get => pFCPSTRet; set => pFCPSTRet = value; }
        public decimal PST { get => pST; set => pST = value; }

        public decimal Perglp { get => perglp; set => perglp = value; }
        public decimal Pergnat { get => pergnat; set => pergnat = value; }
        public decimal Pergnat_i { get => pergnat_i; set => pergnat_i = value; }
        public decimal Vlpartida { get => vlpartida; set => vlpartida = value; }
        public string Indescala { get => indescala; set => indescala = value; }
        public string Cnpjfab { get => cnpjfab; set => cnpjfab = value; }
        public string Nomefab { get => nomefab; set => nomefab = value; }
        public string Infadprod { get => infadprod; set => infadprod = value; }
        public decimal PCredSn { get => pCredSn; set => pCredSn = value; }
        public decimal PRedBCEfet { get => pRedBCEfet; set => pRedBCEfet = value; }
        public decimal VBCEfet { get => vBCEfet; set => vBCEfet = value; }
        public decimal PICMSEfet { get => pICMSEfet; set => pICMSEfet = value; }
        public decimal VICMSEfet { get => vICMSEfet; set => vICMSEfet = value; }
        public decimal VCredIcmssn { get => vCredIcmssn; set => vCredIcmssn = value; }
        public decimal Vseg { get => vseg; set => vseg = value; }
        public string Descanp { get => descanp; set => descanp = value; }
        public decimal Voutro { get => voutro; set => voutro = value; }
        public decimal Vbcstdest { get => vbcstdest; set => vbcstdest = value; }
        public decimal Vicmsstdest { get => vicmsstdest; set => vicmsstdest = value; }
        public string Nfci { get => nfci; set => nfci = value; }
        public decimal VBCSTRet { get => vBCSTRet; set => vBCSTRet = value; }
        public decimal VICMSSubstituto { get => vICMSSubstituto; set => vICMSSubstituto = value; }
        public decimal VICMSSTRet { get => vICMSSTRet; set => vICMSSTRet = value; }
        public string CBenef { get => cBenef; set => cBenef = value; }
        public int Indtot { get => indtot; set => indtot = value; }
        public decimal Vbcfcpufdest { get => vbcfcpufdest; set => vbcfcpufdest = value; }

        public string CstIs
        {
            get => cst_is;
            set => cst_is = value;
        }

        public string CclassTribIs
        {
            get => cclass_trib_is;
            set => cclass_trib_is = value;
        }

        public string VBcIs
        {
            get => v_bc_is;
            set => v_bc_is = value;
        }

        public string PIsEspec
        {
            get => p_is_espec;
            set => p_is_espec = value;
        }

        public string PIs
        {
            get => p_is;
            set => p_is = value;
        }

        public string CstIbscbs
        {
            get => cst_ibscbs;
            set => cst_ibscbs = value;
        }

        public string CclassTribIbscbs
        {
            get => cclass_trib_ibscbs;
            set => cclass_trib_ibscbs = value;
        }

        public string VBcIbscbs
        {
            get => v_bc_ibscbs;
            set => v_bc_ibscbs = value;
        }

        public string PIbsUf
        {
            get => p_ibs_uf;
            set => p_ibs_uf = value;
        }

        public string PDifUfIbs
        {
            get => p_dif_uf_ibs;
            set => p_dif_uf_ibs = value;
        }

        public string VDifUfIbs
        {
            get => v_dif_uf_ibs;
            set => v_dif_uf_ibs = value;
        }

        public string VDevTribUfIbs
        {
            get => v_dev_trib_uf_ibs;
            set => v_dev_trib_uf_ibs = value;
        }

        public string PRedAliqUfIbs
        {
            get => p_red_aliq_uf_ibs;
            set => p_red_aliq_uf_ibs = value;
        }

        public string PRedAliqEfetUfIbs
        {
            get => p_red_aliq_efet_uf_ibs;
            set => p_red_aliq_efet_uf_ibs = value;
        }

        public string VIbsUf
        {
            get => v_ibs_uf;
            set => v_ibs_uf = value;
        }

        public string PIbsMun
        {
            get => p_ibs_mun;
            set => p_ibs_mun = value;
        }

        public string PDifMun
        {
            get => p_dif_mun;
            set => p_dif_mun = value;
        }

        public string VDifMun
        {
            get => v_dif_mun;
            set => v_dif_mun = value;
        }

        public string VDevTribMun
        {
            get => v_dev_trib_mun;
            set => v_dev_trib_mun = value;
        }

        public string PRedAliqEfetMun
        {
            get => p_red_aliq_efet_mun;
            set => p_red_aliq_efet_mun = value;
        }

        public string VIbsMun
        {
            get => v_ibs_mun;
            set => v_ibs_mun = value;
        }

        public string PCbs
        {
            get => p_cbs;
            set => p_cbs = value;
        }

        public string PDifUfCbs
        {
            get => p_dif_uf_cbs;
            set => p_dif_uf_cbs = value;
        }

        public string VDevTribCbs
        {
            get => v_dev_trib_cbs;
            set => v_dev_trib_cbs = value;
        }

        public string PRedAliqCbs
        {
            get => p_red_aliq_cbs;
            set => p_red_aliq_cbs = value;
        }

        public string VRedAliqCbs
        {
            get => v_red_aliq_cbs;
            set => v_red_aliq_cbs = value;
        }

        public string VCbs
        {
            get => v_cbs;
            set => v_cbs = value;
        }

        public string CstReg
        {
            get => cst_reg;
            set => cst_reg = value;
        }

        public string CclassTribReg
        {
            get => cclass_trib_reg;
            set => cclass_trib_reg = value;
        }

        public string PAliqEfetRegIbsUf
        {
            get => p_aliq_efet_reg_ibs_uf;
            set => p_aliq_efet_reg_ibs_uf = value;
        }

        public string VTribRegIbsUf
        {
            get => v_trib_reg_ibs_uf;
            set => v_trib_reg_ibs_uf = value;
        }

        public string VTribRegIbsMun
        {
            get => v_trib_reg_ibs_mun;
            set => v_trib_reg_ibs_mun = value;
        }

        public string PAliqEfetRegCbs
        {
            get => p_aliq_efet_reg_cbs;
            set => p_aliq_efet_reg_cbs = value;
        }

        public string VTribRegCbs
        {
            get => v_trib_reg_cbs;
            set => v_trib_reg_cbs = value;
        }

        public VendaNFCeI() { }

        public List<VendaNFCeI> GetTodas(int nrvenda)
        {
            List<VendaNFCeI> lista = new List<VendaNFCeI>();
            VendaNFCeI ivenda;
            try
            {
                DbfBase ebase = new DbfBase();
                string instrucao = @"SELECT nrvenda, codigob, descricao, unidade, quantidade, Cast(vunitario As NUMERIC(11,5)) as vunitario, " +
                "vtotal, vtotalsd, desconto, " +
                "aicms,	aipi, vicms, vipi, bcicms, cfop, cst, ncm, cstipi, cstcofins, vcofins, cstpis, " +
                "mva, stredbcst, stbcicmsst, stalicms, cofbc, cofaliq, cofflag, cofbcst, cofaliqst, cofvalst, frete, pisbc, " +
                "pisaliq, pisval, pisflag, pisbcst, pisaliqst, pisvalst, nritem, origem, stcodant,  " +
                "valbcstret, valstret, codigoint, odesp, datae, vlcusto, " +
                "codigoanp, codif, qtfattempa, ufcombus, bccombus, aliqcombus, valoricomb, vmimpostos, " +
                
                "issflag, issvbc, issaliq, isslista, isscodmu, issvalor, isscdtri, " +
                "tipol, cstipi, vipi, cdpresta, vcompresta, descanp, " +
                "perglp, pergnat, pergnat_i, vlpartida, vicmssubstituto " +
                "from " + ebase.Path + @"\NFCEI.dbf WHERE nrvenda = " + nrvenda + " ";
                OleDbCommand cmd = new OleDbCommand(instrucao, ebase.Conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);

                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                ebase.Close();

                foreach (DataRow row in dt.Rows)
                {
                    var desconto = Convert.ToDecimal(row["desconto"].ToString().Trim());

                    ivenda = new VendaNFCeI()
                    {

                        id_vendanfce = Convert.ToInt32(row["nrvenda"].ToString().Trim()),
                        codigob = row["codigob"].ToString().Trim(),

                        descricao = row["descricao"].ToString().Trim(),
                        quantidade = Convert.ToDecimal(row["quantidade"].ToString().Trim()),
                        valor = Convert.ToDecimal(row["vunitario"].ToString().Trim()),
                        desconto = Convert.ToDecimal(row["desconto"].ToString().Trim()),
                        vl_total = Convert.ToDecimal(row["vtotal"].ToString().Trim()),
                        indtot = 1,
                        nritem = Convert.ToInt32(row["nritem"].ToString().Trim()),
                        ncm = row["ncm"].ToString().Trim(),
                        cfop = row["cfop"].ToString().Trim(),
                        unidade = row["unidade"].ToString().Trim(),
                        utrib = row["unidade"].ToString().Trim(),
                        qtrib = Convert.ToDecimal(row["quantidade"].ToString().Trim()),
                        vuntrib = Convert.ToDecimal(row["vunitario"].ToString().Trim()),

                        aicms = Convert.ToDecimal(row["aicms"].ToString().Trim()),
                        aipi = Convert.ToDecimal(row["aipi"].ToString().Trim()),
                        vicms = Convert.ToDecimal(row["vicms"].ToString().Trim()),

                        bcicms = Convert.ToDecimal(row["bcicms"].ToString().Trim()),
                        
                        cst = row["cst"].ToString().Trim(),
                        
                        cstipi = row["cstipi"].ToString().Trim(),
                        cstcofins = row["cstcofins"].ToString().Trim(),
                        vcofins = Convert.ToDecimal(row["vcofins"].ToString().Trim()),
                        cstpis = row["cstpis"].ToString().Trim(),
                        
                        mva = Convert.ToDecimal(row["mva"].ToString().Trim()),
                        
                        stbcicmsst = Convert.ToDecimal(row["stbcicmsst"].ToString().Trim()),
                        stalicms = Convert.ToDecimal(row["stalicms"].ToString().Trim()),
                        
                        cofbc = Convert.ToDecimal(row["cofbc"].ToString().Trim()),
                        cofaliq = Convert.ToDecimal(row["cofaliq"].ToString().Trim()),
                        cofflag = Convert.ToInt32(row["cofflag"].ToString().Trim()),
                        cofbcst = Convert.ToDecimal(row["cofbcst"].ToString().Trim()),
                        cofaliqst = Convert.ToDecimal(row["cofaliqst"].ToString().Trim()),
                        cofvalst = Convert.ToDecimal(row["cofvalst"].ToString().Trim()),
                        
                        frete = Convert.ToDecimal(row["frete"].ToString().Trim()),
                        voutro = Convert.ToDecimal(row["odesp"].ToString().Trim()),                        
                        origem = Convert.ToInt32(row["origem"].ToString().Trim()),
                        stcodant = row["stcodant"].ToString().Trim(),

                        vBCSTRet = Convert.ToDecimal(row["valbcstret"].ToString().Trim()),
                        vICMSSubstituto = Convert.ToDecimal(row["vicmssubstituto"].ToString().Trim()),
                        vICMSSTRet = Convert.ToDecimal(row["valstret"].ToString().Trim()),

                        codigoint = row["codigoint"].ToString().Trim(),
                        datae = Convert.ToDateTime(row["datae"].ToString().Trim()),
                        vlcusto = Convert.ToDecimal(row["vlcusto"].ToString().Trim()),
                                                
                    };

                    if(ivenda.bcicms > 0)
                        ivenda.vbcitem = (ivenda.Valor * ivenda.Quantidade) * (ivenda.bcicms / 100);

                    if (row["vmimpostos"].ToString() != null)
                        if(row["vmimpostos"].ToString().Trim() != "")
                            ivenda.vtottrib = Convert.ToDecimal(row["vmimpostos"].ToString().Trim());


                    ivenda.codigoanp = row["codigoanp"].ToString().Trim();
                    ivenda.codif = row["codif"].ToString().Trim();
                    ivenda.qtfattempa = Convert.ToDecimal(row["qtfattempa"].ToString().Trim());
                    ivenda.ufcombus = row["ufcombus"].ToString().Trim();
                    ivenda.bccombus = Convert.ToDecimal(row["bccombus"].ToString().Trim());
                    ivenda.alicombus = Convert.ToDecimal(row["aliqcombus"].ToString().Trim());
                    ivenda.valoricombus = Convert.ToDecimal(row["valoricomb"].ToString().Trim());
                   
                    ivenda.pisflag = Convert.ToInt32(row["issflag"].ToString().Trim());
                    ivenda.pisbc = Convert.ToDecimal(row["issvbc"].ToString().Trim());
                    ivenda.pisaliq = Convert.ToDecimal(row["issaliq"].ToString().Trim());
                    ivenda.pisval = Convert.ToDecimal(row["issvalor"].ToString().Trim());

                    ivenda.cstipi = row["cstipi"].ToString().Trim();
                    ivenda.vipi = Convert.ToDecimal(row["vipi"].ToString().Trim());
                                        
                    ivenda.perglp = Convert.ToDecimal(row["perglp"].ToString().Trim());
                    ivenda.pergnat = Convert.ToDecimal(row["pergnat"].ToString().Trim());
                    ivenda.pergnat_i = Convert.ToDecimal(row["pergnat_i"].ToString().Trim());
                    
                    ivenda.vlpartida = Convert.ToDecimal(row["vlpartida"].ToString().Trim());
                    ivenda.vICMSSubstituto = Convert.ToDecimal(row["vicmssubstituto"].ToString().Trim());
                    //ivenda.cBenef = row["cbenef"].ToString().Trim();

                    try
                    {
                        string instrucao2 = @"SELECT cbenef " +
                        "from " + ebase.Path + @"\NFCEI.dbf WHERE nrvenda = " + nrvenda + " ";
                        OleDbCommand cmd2 = new OleDbCommand(instrucao2, ebase.Conn);
                        OleDbDataAdapter da2 = new OleDbDataAdapter(cmd);
                        
                        DataSet ds2 = new DataSet();
                        da2.Fill(ds2);
                        DataTable dt2 = ds2.Tables[0];
                        foreach (DataRow row2 in dt2.Rows)
                        {
                            ivenda.cBenef = row2["cbenef"].ToString().Trim();
                        }

                    }
                    catch (Exception exe){
                    }
                    


                    lista.Add(ivenda);
                }

                return lista;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cannot open file"))
                {
                    Funcoes.Crashe(ex, "TABELA ABERTA NO CIAF", true);
                }
                else
                    Funcoes.Crashe(ex, "TABELA ABERTA NO CIAF", false);
                return lista;
            }

        }

        public void UpdateVmimposto(string vmimposto)
        {
            try
            {
                DbfBase ebase = new DbfBase();
                String instrucao = @"Update " + ebase.Path + @"\NFCEI.dbf SET vmimpostos = " + vmimposto.Replace(",",".") +
                 " where nrvenda = " + id_vendanfce + " AND codigob = '" + codigob +"' AND ncm = '" + ncm + "' ";

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
