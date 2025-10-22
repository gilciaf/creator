using System.Drawing;
using System.Drawing.Text;
using NFe.Utils;
using System.Runtime.InteropServices;
using nfecreator.Properties;

namespace nfecreator
{
    public static class Fonte
    {
        /// <summary>
        /// Obtém um objeto <see cref="FontFamily"/> a partir de um array de bytes. Útil para carregar uma fonte a partir de um arquivo de recurso
        /// </summary>
        /// <returns></returns>
        public static FontFamily CarregarDeByteArray(byte[] fonte, out PrivateFontCollection colecaoDeFonte)
        {
            var handle = GCHandle.Alloc(fonte, GCHandleType.Pinned);
            try
            {
                var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(fonte, 0);
                colecaoDeFonte = new PrivateFontCollection();
                colecaoDeFonte.AddMemoryFont(ptr, fonte.Length);
                return colecaoDeFonte.Families[0];
            }
            finally
            {
                handle.Free();
            }
        }
    }

    public enum NfceDetalheVendaNormal
    {
        NaoImprimir = 0,
        UmaLinha = 1,
        DuasLinhas = 2
    }

    public enum NfceDetalheVendaContigencia
    {
        UmaLinha = 1,
        DuasLinhas = 2
    }

    public enum NfceModoImpressao
    {
        //Imprime o conteúdo em múltiplas páginas
        MultiplasPaginas = 0,

        //Imprime o conteúdo em uma única página, mesmo que o tamanho da página exceda o tamanho pré-definido (A4)
        UnicaPagina = 1
    }

    /// <summary>
    /// Layout de impressão do DANFE:
    /// Abaixo - QRCode abaixo dos dados do cliente; Lateral - QRCode ao lado dos dados do cliente (usa menos papel)
    /// </summary>
    public enum NfceLayoutQrCode
    {
        Abaixo = 0,
        Lateral = 1
    }

    public class ConfiguracaoDanfeNfce : ConfiguracaoDanfe
    {
        public ConfiguracaoDanfeNfce(NfceDetalheVendaNormal detalheVendaNormal,
            NfceDetalheVendaContigencia detalheVendaContigencia, byte[] logomarca = null,
            bool imprimeDescontoItem = true, float margemEsquerda = 4.5F, float margemDireita = 4.5F,
            NfceModoImpressao modoImpressao = NfceModoImpressao.MultiplasPaginas,
            bool documentoCancelado = false, NfceLayoutQrCode nfceLayoutQrCode = NfceLayoutQrCode.Abaixo, VersaoQrCode versaoQrCode = VersaoQrCode.QrCodeVersao1)
        {
            DocumentoCancelado = documentoCancelado;
            DetalheVendaNormal = detalheVendaNormal;
            DetalheVendaContigencia = detalheVendaContigencia;
            Logomarca = logomarca;
            ImprimeDescontoItem = true;
            MargemEsquerda = margemEsquerda;
            MargemDireita = margemDireita;
            ModoImpressao = NfceModoImpressao.MultiplasPaginas;
            NfceLayoutQrCode = nfceLayoutQrCode;
            CarregarFontePadraoNfceNativa();
            VersaoQrCode = versaoQrCode;
            SegundaViaContingencia = true;
        }

        /// <summary>
        /// Construtor sem parâmetros para serialização
        /// </summary>
        private ConfiguracaoDanfeNfce()
        {
            DocumentoCancelado = false;
        }

        /// <summary>
        /// Modo de impressão do detalhe (produtos) para NFCes emitidos em ambiente Normal
        /// </summary>
        public NfceDetalheVendaNormal DetalheVendaNormal { get; set; }

        /// <summary>
        /// Modo de impressão do detalhe (produtos) para NFCes emitidos em ambiente de Homologação
        /// Nesse modo a informação do detalhe é obrigatória. Vide Manual de Padrões Padrões Técnicos do DANFE-NFC-e e QR Code, versão 3.2
        /// </summary>
        public NfceDetalheVendaContigencia DetalheVendaContigencia { get; set; }

        /// <summary>
        /// Determina se o desconto do item será impresso no DANTE, quando houver
        /// </summary>
        public bool ImprimeDescontoItem { get; set; }

        /// <summary>
        /// Margem esquerda de impressão em milímetros
        /// </summary>
        public float MargemEsquerda { get; set; }

        /// <summary>
        /// Margem direita de impressão em milímetros
        /// </summary>
        public float MargemDireita { get; set; }

        /// <summary>
        /// Determina o modo de impressão do DANFE da NFCe.
        /// 
        /// </summary>
        public NfceModoImpressao ModoImpressao { get; set; }

        /// <summary>
        /// Determina se o QRCode do Nfce será impresso ao lado ou abaixo dos dados do consumidor 
        /// </summary>
        public NfceLayoutQrCode NfceLayoutQrCode { get; set; }

        /// <summary>
        /// Versão do QRCode da NFCe. 1.0 ou 2.0
        /// </summary>
        public VersaoQrCode VersaoQrCode { get; set; }

        public string FontPadraoNfceNativa { get; set; }

        public string Direto { get; set; }

        public string Impressora { get; set; }

        public bool SegundaViaContingencia { get; set; }

        public FontFamily CarregarFontePadraoNfceNativa(string font = null)
        {
            if (font != null)
            {
                FontPadraoNfceNativa = font;
                return new FontFamily(font);
            }

            //todo dispose na coleção
            var openSans = Fonte.CarregarDeByteArray(Resources.OpenSans_CondBold, out PrivateFontCollection colecaoDeFontes);

            return openSans;
        }
    }
}
