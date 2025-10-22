using System.IO;
using FastReport;
using FastReport.Barcode;
using NFe.Classes;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using NFe.Utils;
using NFe.Utils.InformacoesSuplementares;

namespace nfecreator
{
    public class DanfeFrNfce : DanfeBase
    {
        /// <summary>
        /// Construtor da classe responsável pela impressão do DANFE da NFCe em Fast Reports
        /// </summary>
        /// <param name="proc">Objeto do tipo nfeProc</param>
        /// <param name="configuracaoDanfeNfce">Objeto do tipo ConfiguracaoDanfeNfce contendo as definições de impressão</param>
        /// <param name="cIdToken">Identificador do CSC – Código de Segurança do Contribuinte no Banco de Dados da SEFAZ</param>
        /// <param name="csc">Código de Segurança do Contribuinte(antigo Token)</param>
        /// <param name="arquivoRelatorio">Caminho e arquivo frx contendo as definições do relatório personalizado</param>
        public DanfeFrNfce(nfeProc proc, ConfiguracaoDanfeNfce configuracaoDanfeNfce, string cIdToken, string csc, string arquivoRelatorio = "")
        {
            #region Define as variáveis que serão usadas no relatório (dúvidas a respeito do fast reports consulte a documentação em https://www.fast-report.com/pt/product/fast-report-net/documentation/)

            Relatorio = new Report();
            Relatorio.RegisterData(new[] { proc }, "NFCe", 20);
            Relatorio.GetDataSource("NFCe").Enabled = true;
            if (string.IsNullOrEmpty(arquivoRelatorio))
                Relatorio.Load(new MemoryStream(Properties.Resources.NFCe));
            else
                Relatorio.Load(arquivoRelatorio);
            Relatorio.SetParameterValue("NfceDetalheVendaNormal", configuracaoDanfeNfce.DetalheVendaNormal);
            Relatorio.SetParameterValue("NfceDetalheVendaContigencia", configuracaoDanfeNfce.DetalheVendaContigencia);
            Relatorio.SetParameterValue("NfceImprimeDescontoItem", configuracaoDanfeNfce.ImprimeDescontoItem);
            Relatorio.SetParameterValue("NfceModoImpressao", configuracaoDanfeNfce.ModoImpressao);
            Relatorio.SetParameterValue("NfceCancelado", configuracaoDanfeNfce.DocumentoCancelado);
            Relatorio.SetParameterValue("NfceLayoutQrCode", configuracaoDanfeNfce.NfceLayoutQrCode);
            ((ReportPage)Relatorio.FindObject("PgNfce")).LeftMargin = configuracaoDanfeNfce.MargemEsquerda;
            ((ReportPage)Relatorio.FindObject("PgNfce")).RightMargin = configuracaoDanfeNfce.MargemDireita;
            ((PictureObject)Relatorio.FindObject("poEmitLogo")).Image = configuracaoDanfeNfce.ObterLogo();
            ((TextObject)Relatorio.FindObject("txtUrl")).Text = string.IsNullOrEmpty(proc.NFe.infNFeSupl.urlChave) ? proc.NFe.infNFeSupl.ObterUrlConsulta(proc.NFe, configuracaoDanfeNfce.VersaoQrCode) : proc.NFe.infNFeSupl.urlChave;
            ((BarcodeObject)Relatorio.FindObject("bcoQrCode")).Text = proc.NFe.infNFeSupl == null ? proc.NFe.infNFeSupl.ObterUrlQrCode(proc.NFe, configuracaoDanfeNfce.VersaoQrCode, cIdToken, csc) : proc.NFe.infNFeSupl.qrCode;
            ((BarcodeObject)Relatorio.FindObject("bcoQrCodeLateral")).Text = proc.NFe.infNFeSupl == null ? proc.NFe.infNFeSupl.ObterUrlQrCode(proc.NFe, configuracaoDanfeNfce.VersaoQrCode, cIdToken, csc) : proc.NFe.infNFeSupl.qrCode;



            //Segundo o Manual de Padrões Técnicos do DANFE - NFC - e e QR Code, versão 3.2, página 9, nos casos de emissão em contingência deve ser impresso uma segunda cópia como via do estabelecimento
            if (configuracaoDanfeNfce.SegundaViaContingencia)
                Relatorio.PrintSettings.Copies = (proc.NFe.infNFe.ide.tpEmis == TipoEmissao.teNormal | (proc.protNFe != null && proc.protNFe.infProt != null && NfeSituacao.Autorizada(proc.protNFe.infProt.cStat))
                /*Se a NFe for autorizada, mesmo que seja em contingência, imprime somente uma via*/ ) ? 1 : 2;

            #endregion
        }

        /// <summary>
        /// Construtor da classe responsável pela impressão do DANFE da NFCe em Fast Reports.
        /// Use esse construtor apenas para impressão em contingência, já que neste modo ainda não é possível obter o grupo protNFe 
        /// </summary>
        /// <param name="nfe">Objeto do tipo NFe</param>
        /// <param name="configuracaoDanfeNfce">Objeto do tipo ConfiguracaoDanfeNfce contendo as definições de impressão</param>
        /// <param name="cIdToken">Identificador do CSC – Código de Segurança do Contribuinte no Banco de Dados da SEFAZ</param>
        /// <param name="csc">Código de Segurança do Contribuinte(antigo Token)</param>
        public DanfeFrNfce(NFe.Classes.NFe nfe, ConfiguracaoDanfeNfce configuracaoDanfeNfce, string cIdToken, string csc) :
            this(new nfeProc() { NFe = nfe }, configuracaoDanfeNfce, cIdToken, csc, string.Empty)
        {

        }
    }
}
