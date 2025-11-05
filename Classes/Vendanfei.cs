using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nfecreator
{
    class VendaNFeI
    {

        int id_vendanfei;
        int id_vendanfe;
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
        decimal pdevol;
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

        string cenqipi;

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
        decimal vICMSDeson;
        
        // REFORMA TRIBUTÁRIA 
        string cst_is;
        string cclass_trib_is;
        decimal v_bc_is;
        decimal p_is;
        string p_is_espec;
        decimal v_is;
        
        string cst_ibscbs;
        string cclass_trib_ibscbs;
        
        decimal v_bc_ibscbs;
        decimal p_ibs_uf;
        decimal p_dif_uf_ibs;
        decimal v_dif_uf_ibs;
        decimal v_dev_trib_uf_ibs;
        decimal p_red_aliq_uf_ibs;
        decimal p_red_aliq_efet_uf_ibs;
        decimal v_ibs_uf;
        
        decimal p_ibs_mun;
        decimal p_dif_mun_ibs;
        decimal v_dif_mun_ibs;
        decimal v_dev_trib_mun_ibs;
        decimal p_red_aliq_mun_ibs;
        decimal p_red_aliq_efet_mun_ibs;
        decimal v_ibs_mun;
        
        decimal p_cbs;
        decimal v_dif_cbs;
        decimal p_dif_cbs;
        decimal v_dev_trib_cbs;
        decimal p_red_aliq_cbs;
        decimal v_red_aliq_cbs;
        decimal p_aliq_efet_cbs;
        
        decimal v_cbs;
        
        decimal cst_reg;
        decimal cclass_trib_reg;
        decimal p_aliq_efet_reg_ibs_uf;
        decimal v_trib_reg_ibs_uf;
        decimal p_aliq_efet_reg_ibs_mun;
        decimal v_trib_reg_ibs_mun;
        decimal p_aliq_efet_reg_cbs;
        decimal v_trib_reg_cbs;

        public int Id_vendanfei { get => id_vendanfei; set => id_vendanfei = value; }
        public int Id_vendanfe { get => id_vendanfe; set => id_vendanfe = value; }
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
        public string Cenqipi { get => cenqipi; set => cenqipi = value; }
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
        public decimal Pdevol { get => pdevol; set => pdevol = value; }
        public decimal VICMSDeson { get => vICMSDeson; set => vICMSDeson = value; }

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

        public decimal VBcIs
        {
            get => v_bc_is;
            set => v_bc_is = value;
        }

        public string PIsEspec
        {
            get => p_is_espec;
            set => p_is_espec = value;
        }

        public decimal VIs
        {
            get => v_is;
            set => v_is = value;
        }

        public decimal PIs
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

        public decimal VBcIbscbs
        {
            get => v_bc_ibscbs;
            set => v_bc_ibscbs = value;
        }

        public decimal PIbsUf
        {
            get => p_ibs_uf;
            set => p_ibs_uf = value;
        }

        public decimal PDifUfIbs
        {
            get => p_dif_uf_ibs;
            set => p_dif_uf_ibs = value;
        }

        public decimal VDifUfIbs
        {
            get => v_dif_uf_ibs;
            set => v_dif_uf_ibs = value;
        }

        public decimal VDevTribUfIbs
        {
            get => v_dev_trib_uf_ibs;
            set => v_dev_trib_uf_ibs = value;
        }

        public decimal PRedAliqUfIbs
        {
            get => p_red_aliq_uf_ibs;
            set => p_red_aliq_uf_ibs = value;
        }

        public decimal PRedAliqEfetUfIbs
        {
            get => p_red_aliq_efet_uf_ibs;
            set => p_red_aliq_efet_uf_ibs = value;
        }

        public decimal VIbsUf
        {
            get => v_ibs_uf;
            set => v_ibs_uf = value;
        }

        public decimal PIbsMun
        {
            get => p_ibs_mun;
            set => p_ibs_mun = value;
        }

        public decimal PDifMun
        {
            get => p_dif_mun_ibs;
            set => p_dif_mun_ibs = value;
        }

        public decimal VDifMun
        {
            get => v_dif_mun_ibs;
            set => v_dif_mun_ibs = value;
        }

        public decimal VDevTribMun
        {
            get => v_dev_trib_mun_ibs;
            set => v_dev_trib_mun_ibs = value;
        }
        public decimal PRedAliqMun
        {
            get => p_red_aliq_mun_ibs;
            set => p_red_aliq_mun_ibs = value;
        }
        public decimal PRedAliqEfetMun
        {
            get => p_red_aliq_efet_mun_ibs;
            set => p_red_aliq_efet_mun_ibs = value;
        }

        public decimal VIbsMun
        {
            get => v_ibs_mun;
            set => v_ibs_mun = value;
        }

        public decimal PCbs
        {
            get => p_cbs;
            set => p_cbs = value;
        }

        public decimal PDifUfCbs
        {
            get => p_dif_cbs;
            set => p_dif_cbs = value;
        }
        
        public decimal VDifCbs
        {
            get => v_dif_cbs;
            set => v_dif_cbs = value;
        }
        
        public decimal VDevTribCbs
        {
            get => v_dev_trib_cbs;
            set => v_dev_trib_cbs = value;
        }

        public decimal PRedAliqCbs
        {
            get => p_red_aliq_cbs;
            set => p_red_aliq_cbs = value;
        }

        public decimal VRedAliqCbs
        {
            get => v_red_aliq_cbs;
            set => v_red_aliq_cbs = value;
        }

        public decimal VCbs
        {
            get => v_cbs;
            set => v_cbs = value;
        }

        public decimal CstReg
        {
            get => cst_reg;
            set => cst_reg = value;
        }

        public decimal CclassTribReg
        {
            get => cclass_trib_reg;
            set => cclass_trib_reg = value;
        }

        public decimal PAliqEfetRegIbsUf
        {
            get => p_aliq_efet_reg_ibs_uf;
            set => p_aliq_efet_reg_ibs_uf = value;
        }

        public decimal VTribRegIbsUf
        {
            get => v_trib_reg_ibs_uf;
            set => v_trib_reg_ibs_uf = value;
        }

        public decimal PAliqEfetRegIbsMun
        {
            get => p_aliq_efet_reg_ibs_mun;
            set => p_aliq_efet_reg_ibs_mun = value;
        }

        public decimal VTribRegIbsMun
        {
            get => v_trib_reg_ibs_mun;
            set => v_trib_reg_ibs_mun = value;
        }

        public decimal PAliqEfetRegCbs
        {
            get => p_aliq_efet_reg_cbs;
            set => p_aliq_efet_reg_cbs = value;
        }

        public decimal VTribRegCbs
        {
            get => v_trib_reg_cbs;
            set => v_trib_reg_cbs = value;
        }

        public VendaNFeI() { }

        public List<VendaNFeI> GetTodas(int nrvenda)
        {
            List<VendaNFeI> lista = new List<VendaNFeI>();
            VendaNFeI ivenda;
            try
            {
                DbfBase ebase = new DbfBase();
                string instrucao = @"SELECT nrvenda, codigob, descricao, unidade, quantidade, Cast(vunitario As NUMERIC(11,5)) as vunitario, " +
                "vtotal, vtotalsd, desconto, " +
                "aicms,	aipi, vicms, vipi, bcicms, cfop, cst, ncm, peso, cstipi, cstcofins, vcofins, cstpis, stmodalidade, " +
                "mva, stredbcst, stbcicmsst, stalicms, stvalicmsst, cofbc, cofaliq, cofflag, cofbcst, cofaliqst, cofvalst, frete, pisbc, " +
                "pisaliq, pisval, pisflag, pisbcst, pisaliqst, pisvalst, nritem, origem, stcodant,  i_flag, i_vlbcalculo, i_daduaneira, i_vliof, " +
                "i_vlii, i_nrdi, i_dtregistro, i_exportador, i_ufdesemb, i_localdesemb, i_dtdesemba, i_adicao, i_fabestra, i_vldescadic, i_nritemdi, " +
                "valbcstret, valstret, codigoint, odesp, datae, vlcusto, pesol, " +
                "codigoanp, codif, qtfattempa, ufcombus, bccombus, aliqcombus, " +
                "valoricombus, vmimpostos, nrpedido, i_tpviatransp, i_afrim, i_tpintermedio, i_cnpjordem, i_ufterceiro, nritemped, vbcitem, cest, " +
                "vbcufdest, pfpufdest, picmsufdest, picmsinter, picmsinterpart, vfcpufdest, vicmsufdest, vicmsufremet, " +
                "cenqipi, untrib, qttrib, Cast(vltrib As NUMERIC(11,5)) as vltrib,  " +
                "perglp, pergnat, pergnat_i, vlpartida, cdbefiuf, iescalar, cnpjfab, nomefab, vipidevol, pproddevol, vbcfcp, pfcp, vfcp, vbcfcpst, " +
                "pfcpst, vfcpst, vbcfcpstret, pfcpstret, vfcpstret, vbcfcpufdest, pst, predbcefet, vbcefet, picmsefet, " +
                
                "vicmsefet, descanp, flagstcomb, vbcstretcomb, vicmsstretcomb, ref, lote, vencto, cor, tamanho, registro, vicmssubstituto, " +
                " cst_is, cclass_trib_is, cclass_trib_ibscbs, v_bc_is, p_is, p_is_espec,v_is, cst_ibscbs, v_bc_ibscbs, p_ibs_uf, p_dif_uf_ibs, " +
                "v_dif_uf_ibs, v_dev_trib_uf_ibs, p_red_aliq_uf_ibs, p_red_aliq_uf_ibs, v_ibs_uf, p_red_aliq_efet_uf_ibs, p_ibs_mun, p_dif_mun_ibs, v_dif_mun_ibs, " +
                "v_dev_trib_mun_ibs, p_red_aliq_mun_ibs, p_red_aliq_efet_mun_ibs, v_ibs_mun, p_cbs, p_dif_cbs, v_dif_cbs, v_dev_trib_cbs, p_red_aliq_cbs, v_red_aliq_cbs, " +
                "v_cbs, cst_reg, cclass_trib_reg, p_aliq_efet_reg_ibs_uf, v_trib_reg_ibs_uf, p_aliq_efet_reg_ibs_mun, v_trib_reg_ibs_mun, p_aliq_efet_reg_cbs, v_trib_reg_cbs " + 
                "from " + ebase.Path + @"\vendanfei.dbf WHERE nrvenda = " + nrvenda + " ";
                OleDbCommand cmd = new OleDbCommand(instrucao, ebase.Conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);

                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                ebase.Close();

                foreach (DataRow row in dt.Rows)
                {
                   var desconto = Convert.ToDecimal(row["desconto"].ToString().Trim());

                    ivenda = new VendaNFeI()
                    {
                        // id_vendanfei = Convert.ToInt32(row["nrvenda"].ToString().Trim()),
                        id_vendanfe = Convert.ToInt32(row["nrvenda"].ToString().Trim()),
                        // codigoproduto = row["codigoproduto"].ToString().Trim(),
                        codigob = row["codigob"].ToString().Trim(),

                        descricao = row["descricao"].ToString().Replace("\u0002","").Trim(),
                        // infadprod = row["infadprod"].ToString().Trim(),
                        quantidade = Convert.ToDecimal(row["quantidade"].ToString().Trim()),
                        valor = Convert.ToDecimal(row["vunitario"].ToString().Trim()),
                        desconto = Convert.ToDecimal(row["desconto"].ToString().Trim()),
                        vl_total = Convert.ToDecimal(row["vtotal"].ToString().Trim()),
                        indtot = 1,
                        //  indtot = (int)row["indtot"],
                        //comissao_i = Convert.ToDecimal(row["comissao_i"].ToString().Trim()),
                        //comissao_g = Convert.ToDecimal(row["comissao_g"].ToString().Trim()),
                        //comissao_v = Convert.ToDecimal(row["comissao_v"].ToString().Trim()),

                        nritem = Convert.ToInt32(row["nritem"].ToString().Trim()),
                        ncm = row["ncm"].ToString().Trim(),
                        cfop = row["cfop"].ToString().Trim(),
                        unidade = row["unidade"].ToString().Trim(),
                        utrib = row["untrib"].ToString().Trim(),
                        qtrib = Convert.ToDecimal(row["qttrib"].ToString().Trim()),
                        vuntrib = Convert.ToDecimal(row["vltrib"].ToString().Trim()),
                        vtottrib = Convert.ToDecimal(row["vmimpostos"].ToString().Trim()),
                        aicms = Convert.ToDecimal(row["aicms"].ToString().Trim()),
                        aipi = Convert.ToDecimal(row["aipi"].ToString().Trim()),
                        vicms = Convert.ToDecimal(row["vicms"].ToString().Trim()),
                        vipi = Convert.ToDecimal(row["vipi"].ToString().Trim()),
                        vipidevol = Convert.ToDecimal(row["vipidevol"].ToString().Trim()),
                        pdevol = Convert.ToDecimal(row["pproddevol"].ToString().Trim()),
                        bcicms = Convert.ToDecimal(row["bcicms"].ToString().Trim()),
                        cst = row["cst"].ToString().Trim(),
                        pesol = Convert.ToDecimal(row["pesol"].ToString().Trim()),
                        cstipi = row["cstipi"].ToString().Trim(),
                        cstcofins = row["cstcofins"].ToString().Trim(),
                        vcofins = Convert.ToDecimal(row["vcofins"].ToString().Trim()),
                        cstpis = row["cstpis"].ToString().Trim(),


                        stmodalidade = Convert.ToInt32(row["stmodalidade"].ToString().Trim()),
                        mva = Convert.ToDecimal(row["mva"].ToString().Trim()),
                        //strdbcst = Convert.ToDecimal(row["strdbcst"].ToString().Trim()),
                        stbcicmsst = Convert.ToDecimal(row["stbcicmsst"].ToString().Trim()),
                        stalicms = Convert.ToDecimal(row["stalicms"].ToString().Trim()),
                        stvalicmsst = Convert.ToDecimal(row["stvalicmsst"].ToString().Trim()),
                        cofbc = Convert.ToDecimal(row["cofbc"].ToString().Trim()),
                        cofaliq = Convert.ToDecimal(row["cofaliq"].ToString().Trim()),
                        cofflag = Convert.ToInt32(row["cofflag"].ToString().Trim()),
                        cofbcst = Convert.ToDecimal(row["cofbcst"].ToString().Trim()),
                        cofaliqst = Convert.ToDecimal(row["cofaliqst"].ToString().Trim()),
                        cofvalst = Convert.ToDecimal(row["cofvalst"].ToString().Trim()),
                        frete = Convert.ToDecimal(row["frete"].ToString().Trim()),
                        //  vseg = Convert.ToDecimal(row["vseg"].ToString().Trim()),
                        voutro = Convert.ToDecimal(row["odesp"].ToString().Trim()),
                        pisbc = Convert.ToDecimal(row["pisbc"].ToString().Trim()),
                        pisaliq = Convert.ToDecimal(row["pisaliq"].ToString().Trim()),
                        pisval = Convert.ToDecimal(row["pisval"].ToString().Trim()),
                        pisflag = Convert.ToInt32(row["pisflag"].ToString().Trim()),
                        pisbcst = Convert.ToDecimal(row["pisbcst"].ToString().Trim()),
                        pisaliqst = Convert.ToDecimal(row["pisaliqst"].ToString().Trim()),
                        pisvalst = Convert.ToDecimal(row["pisvalst"].ToString().Trim()),
                        origem = Convert.ToInt32(row["origem"].ToString().Trim()),


                        stcodant = row["stcodant"].ToString().Trim(),
                        i_flag = Convert.ToInt32(row["i_flag"].ToString().Trim()),
                        i_vlbcalculo = Convert.ToDecimal(row["i_vlbcalculo"].ToString().Trim()),
                        i_daduaneira = Convert.ToDecimal(row["i_daduaneira"].ToString().Trim()),
                        i_vliof = Convert.ToDecimal(row["i_vliof"].ToString().Trim()),
                        i_vlii = Convert.ToDecimal(row["i_vlii"].ToString().Trim()),
                        //  nfci = row["nfci"].ToString().Trim(),
                        i_nrdi = row["i_nrdi"].ToString().Trim(),
                        i_dtregistro = Convert.ToDateTime(row["i_dtregistro"].ToString().Trim()),
                        i_exportador = row["i_exportador"].ToString().Trim(),
                        i_ufdesemb = row["i_ufdesemb"].ToString().Trim(),
                        i_localdesemb = row["i_localdesemb"].ToString().Trim(),
                        i_dtdesemb = Convert.ToDateTime(row["i_dtdesemba"].ToString().Trim()),
                        i_adicao = row["i_adicao"].ToString().Trim(),
                        i_fabestra = row["i_fabestra"].ToString().Trim(),
                        i_vldescadic = Convert.ToDecimal(row["i_vldescadic"].ToString().Trim()),
                        i_nritemdi = row["i_nritemdi"].ToString().Trim(),


                        vBCSTRet = Convert.ToDecimal(row["valbcstret"].ToString().Trim()),
                        vICMSSubstituto = Convert.ToDecimal(row["vicmssubstituto"].ToString().Trim()),
                        vICMSSTRet = Convert.ToDecimal(row["valstret"].ToString().Trim()),
                        // vbcstdest = Convert.ToDecimal(row["vbcstdest"].ToString().Trim()),
                        // vicmsstdest = Convert.ToDecimal(row["vicmsstdest"].ToString().Trim()),

                        codigoint = row["codigoint"].ToString().Trim(),
                        datae = Convert.ToDateTime(row["datae"].ToString().Trim()),
                        vlcusto = Convert.ToDecimal(row["vlcusto"].ToString().Trim()),
                        pesob = Convert.ToDecimal(row["peso"].ToString().Trim()),
                        codigoanp = row["codigoanp"].ToString().Trim(),
                        descanp = row["descanp"].ToString().Trim(),
                        codif = row["codif"].ToString().Trim(),
                        
                        
                        // IBSCBS
                        cst_is = row["cst_is"].ToString().Trim(),
                        cclass_trib_is = row["cclass_trib_is"].ToString().Trim(),
                        v_bc_is = Convert.ToDecimal(row["v_bc_is"].ToString().Trim()),
                        p_is =Convert.ToDecimal(row["p_is"].ToString().Trim()),
                        p_is_espec = row["p_is_espec"].ToString().Trim(),
                        v_is = Convert.ToDecimal(row["v_is"].ToString().Trim()),
                        cst_ibscbs = row["cst_ibscbs"].ToString().Trim(),
                        cclass_trib_ibscbs = row["cclass_trib_ibscbs"].ToString().Trim(),
                        v_bc_ibscbs = Convert.ToDecimal(row["v_bc_ibscbs"].ToString().Trim()),
                        p_ibs_uf = Convert.ToDecimal(row["p_ibs_uf"].ToString().Trim()),
                        p_dif_uf_ibs = Convert.ToDecimal(row["p_dif_uf_ibs"].ToString().Trim()),
                        v_dif_uf_ibs = Convert.ToDecimal(row["v_dif_uf_ibs"].ToString().Trim()),
                        v_dev_trib_uf_ibs = Convert.ToDecimal(row["v_dev_trib_uf_ibs"].ToString().Trim()),
                        p_red_aliq_uf_ibs = Convert.ToDecimal(row["p_red_aliq_uf_ibs"].ToString().Trim()),
                        p_red_aliq_efet_uf_ibs = Convert.ToDecimal(row["p_red_aliq_efet_uf_ibs"].ToString().Trim()),
                        v_ibs_uf = Convert.ToDecimal(row["v_ibs_uf"].ToString().Trim()),
                        p_ibs_mun = Convert.ToDecimal(row["p_ibs_mun"].ToString().Trim()),
                        p_dif_mun_ibs = Convert.ToDecimal(row["p_dif_mun_ibs"].ToString().Trim()),
                        v_dif_mun_ibs = Convert.ToDecimal(row["v_dif_mun_ibs"].ToString().Trim()),
                        v_dev_trib_mun_ibs = Convert.ToDecimal(row["v_dev_trib_mun_ibs"].ToString().Trim()),
                        p_red_aliq_mun_ibs =Convert.ToDecimal( row["p_red_aliq_mun_ibs"].ToString().Trim()),
                        p_red_aliq_efet_mun_ibs = Convert.ToDecimal(row["p_red_aliq_efet_mun_ibs"].ToString().Trim()),
                        v_ibs_mun = Convert.ToDecimal(row["v_ibs_mun"].ToString().Trim()),
                        
                        p_cbs = Convert.ToDecimal(row["p_cbs"].ToString().Trim()),
                        p_dif_cbs = Convert.ToDecimal(row["p_dif_cbs"].ToString().Trim()),
                        v_dif_cbs = Convert.ToDecimal(row["v_dif_cbs"].ToString().Trim()),
                        v_dev_trib_cbs = Convert.ToDecimal(row["v_dev_trib_cbs"].ToString().Trim()),
                        p_red_aliq_cbs = Convert.ToDecimal(row["p_red_aliq_cbs"].ToString().Trim()),
                        v_red_aliq_cbs = Convert.ToDecimal(row["v_red_aliq_cbs"].ToString().Trim()),
                        
                        v_cbs = Convert.ToDecimal(row["v_cbs"].ToString().Trim()),
                        
                        cst_reg = Convert.ToDecimal(row["cst_reg"].ToString().Trim()),
                        cclass_trib_reg = Convert.ToDecimal(row["cclass_trib_reg"].ToString().Trim()),
                        p_aliq_efet_reg_ibs_uf = Convert.ToDecimal(row["p_aliq_efet_reg_ibs_uf"].ToString().Trim()),
                        v_trib_reg_ibs_uf = Convert.ToDecimal(row["v_trib_reg_ibs_uf"].ToString().Trim()),
                        p_aliq_efet_reg_ibs_mun = Convert.ToDecimal(row["p_aliq_efet_reg_ibs_mun"].ToString().Trim()),
                        v_trib_reg_ibs_mun = Convert.ToDecimal(row["v_trib_reg_ibs_mun"].ToString().Trim()),
                        p_aliq_efet_reg_cbs = Convert.ToDecimal(row["p_aliq_efet_reg_cbs"].ToString().Trim()),
                        v_trib_reg_cbs = Convert.ToDecimal(row["v_trib_reg_cbs"].ToString().Trim())
                        
                    };

                    ivenda.qtfattempa = Convert.ToDecimal(row["qtfattempa"].ToString().Trim());
                    ivenda.ufcombus = row["ufcombus"].ToString().Trim();
                    ivenda.bccombus = Convert.ToDecimal(row["bccombus"].ToString().Trim());
                    ivenda.alicombus = Convert.ToDecimal(row["aliqcombus"].ToString().Trim());
                    ivenda.valoricombus = Convert.ToDecimal(row["valoricombus"].ToString().Trim());
                    ivenda.nrpedido = row["nrpedido"].ToString().Trim();

                    if(row["i_tpviatransp"].ToString().Trim() != "")
                    ivenda.i_tpviatransp = Convert.ToInt32(row["i_tpviatransp"].ToString().Trim());
                    ivenda.i_afrmm = Convert.ToDecimal(row["i_afrim"].ToString().Trim());

                    if (row["i_tpintermedio"].ToString().Trim() != "")
                        ivenda.i_tpintermedio = Convert.ToInt32(row["i_tpintermedio"].ToString().Trim());
                    ivenda.i_cnpj = row["i_cnpjordem"].ToString().Trim();


                    ivenda.i_ufterceiro = row["i_ufterceiro"].ToString().Trim();
                    ivenda.nritemped = row["nritemped"].ToString().Trim();
                    ivenda.vbcitem = Convert.ToDecimal(row["vbcitem"].ToString().Trim());
                    ivenda.cest = row["cest"].ToString().Trim();
                    ivenda.vbcufdest = Convert.ToDecimal(row["vbcufdest"].ToString().Trim());
                    //vbcfcpufdest = Convert.ToDecimal(row["vbcfcpufdest"];
                    ivenda.pfcpufdest = Convert.ToDecimal(row["pfpufdest"].ToString().Trim());
                    ivenda.picmsufdest = Convert.ToDecimal(row["picmsufdest"].ToString().Trim());
                    ivenda.picmsinter = Convert.ToDecimal(row["picmsinter"].ToString().Trim());
                    ivenda.picmsinterpa = Convert.ToDecimal(row["picmsinterpart"].ToString().Trim());
                    ivenda.vfcpufdest = Convert.ToDecimal(row["vfcpufdest"].ToString().Trim());

                    ivenda.vicmsufdest = Convert.ToDecimal(row["vicmsufdest"].ToString().Trim());
                    ivenda.vicmsufremet = Convert.ToDecimal(row["vicmsufremet"].ToString().Trim());
                    ivenda.vFCP = Convert.ToDecimal(row["vFCP"].ToString().Trim());
                    ivenda.vBCFCP = Convert.ToDecimal(row["vBCFCP"].ToString().Trim());
                    ivenda.pFCP = Convert.ToDecimal(row["pFCP"].ToString().Trim());
                    ivenda.vFCPST = Convert.ToDecimal(row["vFCPST"].ToString().Trim());
                    ivenda.vBCFCPST = Convert.ToDecimal(row["vBCFCPST"].ToString().Trim());
                    ivenda.pFCPST = Convert.ToDecimal(row["pFCPST"].ToString().Trim());
                    ivenda.vFCPSTRet = Convert.ToDecimal(row["vFCPSTRet"].ToString().Trim());
                    ivenda.vBCFCPSTRet = Convert.ToDecimal(row["pfcpstret"].ToString().Trim());
                    ivenda.pFCPSTRet = Convert.ToDecimal(row["pFCPSTRet"].ToString().Trim());
                    ivenda.pST = Convert.ToDecimal(row["pst"].ToString().Trim());

                 
                    ivenda.cenqipi = row["cenqipi"].ToString().Trim();

                    ivenda.perglp = Convert.ToDecimal(row["perglp"].ToString().Trim());
                    ivenda.pergnat = Convert.ToDecimal(row["pergnat"].ToString().Trim());
                    ivenda.pergnat_i = Convert.ToDecimal(row["pergnat_i"].ToString().Trim());
                    ivenda.vlpartida = Convert.ToDecimal(row["vlpartida"].ToString().Trim());
                    // indescala = row["indescala"].ToString().Trim();
                    ivenda.cnpjfab = row["cnpjfab"].ToString().Trim();
                    //ivenda.cBenef = row["cbenef"].ToString().Trim();
                    ivenda.nomefab = row["nomefab"].ToString().Trim();

                    // pCredSn = Convert.ToDecimal(row["pCredSn"].ToString().Trim());
                    //  vCredIcmssn = Convert.ToDecimal(row["vCredIcmssn"].ToString().Trim())
                    ivenda.pRedBCEfet = Convert.ToDecimal(row["pRedBCEfet"].ToString().Trim());
                    ivenda.vBCEfet = Convert.ToDecimal(row["vBCEfet"].ToString().Trim());
                    ivenda.pICMSEfet = Convert.ToDecimal(row["pICMSEfet"].ToString().Trim());
                    ivenda.vICMSEfet = Convert.ToDecimal(row["vICMSEfet"].ToString().Trim());


                    if(App.Parametro.PCredsn > 0)
                    {
                        ivenda.PCredSn = App.Parametro.PCredsn;
                        decimal vprod = (ivenda.Valor * ivenda.Quantidade); ;
                        ivenda.VCredIcmssn = (vprod * ivenda.PCredSn) / 100;

                    }
                    lista.Add(ivenda);
                }

                return lista;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cannot open file"))
                {
                    Funcoes.Crashe(ex, "TABELA ABERTA NO CIAF - vendanfei", true);
                }
                else
                    Funcoes.Crashe(ex, "TABELA ABERTA NO CIAF - vendanfei", false);
                return lista;
            }

        }


        public void UpdateVmimposto(string vmimposto)
        {
            try
            {
                DbfBase ebase = new DbfBase();
                String instrucao = @"Update " + ebase.Path + @"\vendanfei.dbf SET vmimpostos = " + vmimposto.Replace(",", ".") +
                 " where nrvenda = " + id_vendanfe + " AND codigob = '" + codigob + "' AND ncm = '" + ncm + "' ";

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

        public void UpdateVipidevol()
        {
            try
            {
                DbfBase ebase = new DbfBase();
                String instrucao = @"Update " + ebase.Path + @"\vendanfei.dbf SET vipidevol = " + Vipidevol.ToString().Replace(",", ".") +
                 " where nrvenda = " + id_vendanfe + " AND codigob = '" + codigob + "' AND ncm = '" + ncm + "' ";

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

        public void Updatepdevol()
        {
            try
            {
                DbfBase ebase = new DbfBase();
                String instrucao = @"Update " + ebase.Path + @"\vendanfei.dbf SET pproddevol = " + pdevol.ToString().Replace(",", ".") +
                 " where nrvenda = " + id_vendanfe + " AND codigob = '" + codigob + "' AND ncm = '" + ncm + "' ";

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

        public void UpdatepICMS()
        {
            try
            {
                DbfBase ebase = new DbfBase();
                String instrucao = @"Update " + ebase.Path + @"\vendanfei.dbf SET vicms = " + vicms.ToString().Replace(",", ".") +
                 " where nrvenda = " + id_vendanfe + " AND codigob = '" + codigob + "' AND ncm = '" + ncm + "' ";

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
