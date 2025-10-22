using DFe.Utils;
using Microsoft.Win32;
using NFe.Classes;
using NFe.Classes.Servicos.Consulta;
using NFe.Classes.Servicos.Evento;
using NFe.Classes.Servicos.Recepcao.Retorno;
using NFe.Utils.Consulta;
using NFe.Utils.Inutilizacao;
using NFe.Utils.Recepcao;
using QRCoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace nfecreator
{
    /// <summary>
    /// Lógica interna para TELARETORNO.xaml
    /// </summary>
    public partial class TELARETORNONFCE : UserControl
    {       
        VendaNFCe vendanfenfce;
        Vendanfcea vendanfenfcea;
        string linkinformacao = "";
        int ambiente = 1;

        private readonly string _path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public TELARETORNONFCE()
        {
            InitializeComponent();
            
            sendwhatsappWrapPanel.Visibility = Visibility.Collapsed;
            Thread thread = new Thread(AtualizarTela);
            thread.Start();
        }
        private void AtualizarTela()
        {
            Parametrosretorno pretorno = new Parametrosretorno(1);
            Dispatcher.Invoke(new Action(() => {
                if (pretorno.Sendwhatsapp == "SIM")
                {
                    sendwhatsappWrapPanel.Visibility = Visibility.Visible;
                }
            }), DispatcherPriority.ContextIdle, null);
            AtualizarInformacao();
        }


        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public void CarregarNFCE(int id_vendanfe, string retornoBasico, string tipo)
        {
            try
            {
                //nfetab.Visibility = Visibility.Collapsed;
                inftab.Visibility = Visibility.Visible;
                retornotab.Visibility = Visibility.Collapsed;
                xmltab.Visibility = Visibility.Collapsed;

                vendanfenfce = new VendaNFCe(id_vendanfe);
                vendanfenfcea = new Vendanfcea(id_vendanfe);

                string Motivo = "";
                int valorStat = 0;
                codigoTextBox.Text = "";
                MotivoTextBox.Text = "";
                protocoloTextBox.Text = "";
                if (vendanfenfce.Pais == "BRASIL")
                    telefoneTextBox.Text = "55" + Funcoes.Deixarnumero(vendanfenfce.Fone);
                else
                    telefoneTextBox.Text = Funcoes.Deixarnumero(vendanfenfce.Fone);

                statusTextBlock.Text = vendanfenfcea.Statusnfe;
                protocoloTextBox.Text = vendanfenfcea.Nprotocolo;
                var bc = new BrushConverter();
                StatusGrid.Background = (Brush)bc.ConvertFrom(vendanfenfcea.ColorStatus);

                if (vendanfenfcea.Statusnfe == "APROVADO")
                {
                    chaveacessoTextBox.Text = vendanfenfcea.Chave.Replace("NFe", "");
                    emailTextBox.Text = vendanfenfce.Email;
                    inftab.Visibility = Visibility.Visible;
                    TabControPrincipal.SelectedIndex = 0;
                    ImprimirCancelamento.Visibility = Visibility.Hidden;

                    #region QrCode
                    nfeProc nfe = new nfeProc().CarregarDeArquivoXml(_path + @"\NFCE\Autorizada\" + vendanfenfcea.Chave.Replace("NFe", "") + "-procNFe.xml");
                    if (!string.IsNullOrWhiteSpace(nfe.NFe.infNFeSupl.qrCode))
                    {
                        QRCodeGenerator qrGenerator = new QRCodeGenerator();
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(nfe.NFe.infNFeSupl.qrCode, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);
                        System.Drawing.Bitmap qrCodeImage = qrCode.GetGraphic(20);

                        qrcodeImage.Source = Imaging.CreateBitmapSourceFromHBitmap(qrCodeImage.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    }
                    #endregion QrCode

                }
                else if (vendanfenfcea.Statusnfe == "CANCELADO")
                {
                    chaveacessoTextBox.Text = vendanfenfcea.Chave.Replace("NFe", "");
                    emailTextBox.Text = vendanfenfce.Email;
                    inftab.Visibility = Visibility.Visible;
                    TabControPrincipal.SelectedIndex = 0;
                    ImprimirCancelamento.Visibility = Visibility.Visible;

                    WebXmlRetorno.Navigate(_path + @"\NFCE\Cancelada\" + vendanfenfcea.Chave.Replace("NFe", "") + "-nProt" + vendanfenfcea.Nprotocolo + ".xml");
                    xmltab.Visibility = Visibility.Visible;

                }
                else if (vendanfenfcea.Statusnfe == "INUTILIZADO")
                {
                    inftab.Visibility = Visibility.Collapsed;  
                    retornotab.Visibility = Visibility.Collapsed;
                    TabControPrincipal.SelectedIndex = 0;

                    WebXmlRetorno.Navigate(_path + @"\NFCE\Inutilizada\" + vendanfenfcea.Chave + "-nProt" + vendanfenfcea.Nprotocolo + ".xml");
                }
                else if (vendanfenfcea.Statusnfe == "DENEGADO")
                {
                    TabControPrincipal.SelectedIndex = 0;
                    inftab.Visibility = Visibility.Collapsed;
                    retornotab.Visibility = Visibility.Collapsed;

                    WebXmlRetorno.Navigate(_path + @"\NFCE\Autorizada\" + vendanfenfcea.Chave.Replace("NFe", "") + ".xml");
                    xmltab.Visibility = Visibility.Visible;

                }
                else 
                {
                    inftab.Visibility = Visibility.Collapsed;
                    TabControPrincipal.SelectedIndex = 1;
                    //CancelamentoPanel.Visibility = Visibility.Collapsed;
                }

                if (retornoBasico != null)
                {
                    xmltab.Visibility = Visibility.Visible;
                    retornotab.Visibility = Visibility.Visible;


                    var stw = new StreamWriter(_path + @"\NFCE\tmp.xml");
                    stw.WriteLine(retornoBasico);
                    stw.Close();
                    WebXmlRetorno.Navigate(_path + @"\NFCE\tmp.xml");

                    if (tipo == "RetornoNfeConsultaProtocolo")
                    {
                        var retConsulta = new retConsSitNFe().CarregarDeXmlString(retornoBasico);
                        Motivo = retConsulta.xMotivo;
                        valorStat = retConsulta.cStat;
                    }
                    else if (tipo == "RetornoNFeAutorizacao")
                    {
                        var retEnvio = new NFe.Classes.Servicos.Recepcao.retEnviNFe().CarregarDeXmlString(retornoBasico);
                        Motivo = retEnvio.protNFe != null ? retEnvio.protNFe.infProt.xMotivo : retEnvio.xMotivo;
                        valorStat = retEnvio.protNFe != null ? retEnvio.protNFe.infProt.cStat : retEnvio.cStat;
                        ambiente = retEnvio.protNFe != null ? (int)retEnvio.tpAmb : 1;

                    }
                    else
                    if (tipo == "RetornoNfeInutilizacao")
                    {
                        var retEnvio = new NFe.Classes.Servicos.Inutilizacao.retInutNFe().CarregarDeXmlString(retornoBasico);
                        Motivo = retEnvio.infInut.xMotivo;
                        valorStat = retEnvio.infInut.cStat;

                    }
                    else
                    if (tipo == "RetornoRecepcaoEvento")
                    {
                        var retornoCancelamento = FuncoesXml.XmlStringParaClasse<retEnvEvento>(retornoBasico);
                        Motivo = retornoCancelamento.retEvento.First().infEvento.xMotivo;
                        valorStat = retornoCancelamento.retEvento.First().infEvento.cStat;
                    }

                    codigoTextBox.Text = valorStat.ToString();
                    MotivoTextBox.Text = Motivo;
                }
            }
            catch (Exception ex)
            {
                xmltab.Visibility = Visibility.Visible;
                inftab.Visibility = Visibility.Collapsed;
                retornotab.Visibility = Visibility.Collapsed;
                TabControPrincipal.SelectedIndex = 2;
                Funcoes.Crashe(ex, "", false);
            }

        }

        private void GoogleButton_Click(object sender, RoutedEventArgs e)
        {
            Funcoes.Analitico("TELARETORNO VERIFICAR GOOGLE");

            string url = @"http://www.google.com.br/search?hl=pt-BR&source=hp&q=" + codigoTextBox.Text + " " + MotivoTextBox.Text;
            System.Diagnostics.Process.Start(url);
        }

        private void SuporeButton_Click(object sender, RoutedEventArgs e)
        {
            Funcoes.Analitico("TELARETORNO SUPORTE");
            System.Diagnostics.Process.Start("https://servidorseguro.mysuite.com.br/client/chatan.php?sl=tvs&h=");
        }
        
        private void Copiando()
        {
            Thread.Sleep(3000);
            Dispatcher.Invoke(new Action(() => {
                copiarButtonText.Text = " Copiar Chave ";

            }), DispatcherPriority.ContextIdle, null);
        }
    
        private void SefazButton_Click(object sender, RoutedEventArgs e)
        {
            Funcoes.Analitico("TELARETORNO SEFAZ NFCE");
            nfeProc nfe = new nfeProc().CarregarDeArquivoXml(_path + @"\NFCE\Autorizada\" + vendanfenfcea.Chave.Replace("NFe", "") + "-procNFe.xml");
            System.Diagnostics.Process.Start(nfe.NFe.infNFeSupl.qrCode);
        }

        private void SalvarXMLButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Funcoes.Analitico("TELARETORNO SALVA XML NFCE");
                var dlg = new SaveFileDialog
                {
                    Title = "Local para salvar o arquivo",
                    FileName = "NFCe" + vendanfenfcea.Chave.Replace("NFe", "") + "-procNFe.xml",
                    DefaultExt = ".xml",
                };
                dlg.ShowDialog();
                if (!String.IsNullOrEmpty(dlg.FileName))
                {
                    System.IO.File.Copy(_path + @"\NFCE\Autorizada\" + vendanfenfcea.Chave.Replace("NFe", "") + "-procNFe.xml", dlg.FileName);

                }
            }
            catch (Exception ex)
            {
               
                if (!string.IsNullOrEmpty(ex.Message))
                {
                    if (ex.Message.Contains("já existe"))
                    {
                        Funcoes.Mensagem(ex.Message, "TELA RETORNO -  SALVAR XML", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                        Funcoes.Crashe(ex, "TELA RETORNO -  SALVAR XML");
                }
             }
        }

        private void SalvarPDFButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Funcoes.Analitico("TELARETORNO SALVA XML NFCE");
                var dlg = new SaveFileDialog
                {
                    Title = "Local para salvar o arquivo",
                    FileName = "NFCe" + vendanfenfce.Chavedeacesso.Replace("NFe", "") + "-procNFe.pdf",
                    DefaultExt = ".pdf",
                };
                dlg.ShowDialog();
                if (!String.IsNullOrEmpty(dlg.FileName))
                {
                    System.IO.File.Copy(_path + @"\NFCE\Autorizada\" + vendanfenfcea.Chave.Replace("NFe", "") + "-procNFe.pdf", dlg.FileName);

                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("já existe"))
                {
                    Funcoes.Mensagem(ex.Message, "TELA RETORNO -  SALVAR PDF", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                    Funcoes.Crashe(ex, "TELA RETORNO -  SALVAR PDF");
            }
        }

        public void ImprimirButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Funcoes.Analitico("TELARETORNO IMPRIMI NFCE");
                FuncoesNFCe funcao = new FuncoesNFCe();
                funcao.ImprimirDireto(vendanfenfce, vendanfenfcea);
            }

            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ERRO IMPRIMIR DIRETO");

            }
        }

        private void ImprimirCancelamento_Click(object sender, RoutedEventArgs e)
        {
            Funcoes.Analitico("TELARETORNO IMPRIMI CANCELAMENTO NFCE");
            try
            {
                string localsave = _path + @"\NFCE\Cancelada\" + vendanfenfcea.Chave + "-nProt" + vendanfenfcea.Nprotocolo + ".pdf";
                System.Diagnostics.Process.Start(localsave);
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                    Funcoes.Crashe(ex, "TELA RETORNO -  IMPRIMIR");
            }
        }

        private void AbrirButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Funcoes.Analitico("TELARETORNO ABRI NFCE");
                System.Diagnostics.Process.Start(_path + @"\NFCE\Autorizada\" + vendanfenfcea.Chave + "-procNFe.pdf");

            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                    Funcoes.Crashe(ex, "TELA RETORNO -  ABRIR NFCE");
            }
        }

        private void CopiarButton_Click(object sender, RoutedEventArgs e)
        {
            Funcoes.Analitico("TELARETORNO COPIA CHAVE NFCE");
            Clipboard.SetText(chaveacessoTextBox.Text);
            copiarButtonText.Text = " Copiado ";

            Thread thread = new Thread(Copiando);
            thread.Start();
            chaveacessoTextBox.Focus();
        }

        private void EnviarEmailButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Funcoes.Analitico("TELARETORNO EMAIL NFCE");
                FuncoesNFCe.Email(vendanfenfce, vendanfenfcea, emailTextBox.Text);
                emailTextBox.Focus();

            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                    Funcoes.Crashe(ex, "TELA RETORNO -  ENVIAR E-MAIL");
            }
        }

        private void ButtonSair_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            System.Windows.Application.Current.Shutdown();
        }

        private void ButtonVoltaClick(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void ButtonSite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Funcoes.Analitico("TELAPROCESSAMENTONFE INFORMACAO");
                if (linkinformacao.Trim() != "")
                    System.Diagnostics.Process.Start(linkinformacao);
                else
                    System.Diagnostics.Process.Start("https://ciaf.com.br/noticias");
            }
            catch (Exception ex)
            {
               // CongelarTela(false);

            }
        }

        private void AtualizarInformacao()
        {
            try
            {
                Parametrogeral geral = new Parametrogeral(3);
                if (geral.Validade >= DateTime.Now)
                {
                    string informacao = geral.Informacao;
                    linkinformacao = geral.Linkinformacao;
                    string linkimagem = geral.Linkimagem;

                    Dispatcher.Invoke(new Action(() =>
                    {
                        //StatusLabel.Text = informacao;
                        if (linkimagem.Trim() != "") ImagedeFundo.Source = new BitmapImage(new Uri(linkimagem));
                    }), DispatcherPriority.ContextIdle, null);
                }

            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "", false);
            }


        }


        private void WhatsAppButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Funcoes.Analitico("TELARETORNO SEND WHATSAPP");
                //https://api.whatsapp.com/send?phone=553537150180&text=Nota%20com%20status%20aprovada%20verifica%20a%20vericidade%20no%20link%3A
                string linkfinal = @"https://api.whatsapp.com/send?phone=" + telefoneTextBox.Text;
                string mensagem = "&text=Sua Nota: " + vendanfenfce.Nrvenda + " foi Aprovada. Consulte a NF-e no portal com o link: ";
                mensagem = mensagem.Replace(" ", "%20");

                nfeProc nfe = new nfeProc().CarregarDeArquivoXml(_path + @"\NFCE\Autorizada\" + vendanfenfcea.Chave.Replace("NFe", "") + "-procNFe.xml");
                string  linkdanota = nfe.NFe.infNFeSupl.qrCode;

                linkdanota = HttpUtility.UrlEncode(linkdanota);

                //if (ambiente == 2)
                //    mensagem += @"https%3A%2F%2Fhom.nfe.fazenda.gov.br%2Fportal%2FconsultaRecaptcha.aspx%3FtipoConsulta%3Dcompleta%26tipoConteudo%3DXbSeqxE8pl8%3D%26nfe%3D" + vendanfenfce;
                //else
                //    mensagem += @"https%3A%2F%2Fwww.nfe.fazenda.gov.br%2Fportal%2FconsultaRecaptcha.aspx%3FtipoConsulta%3Dcompleta%26tipoConteudo%3DXbSeqxE8pl8%3D%26nfe%3D" + vendanfenfea.Chave;

                linkfinal += mensagem + linkdanota;
                System.Diagnostics.Process.Start(linkfinal);
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "", false);
            }
        }

        private void AbrirxmlButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Funcoes.Analitico("TELARETORNO ABRE XML NFE");
                System.Diagnostics.Process.Start(_path + @"\NFCE\Autorizada\" + vendanfenfcea.Chave + "-procNFe.xml");
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "", false);
            }
        }

        private void BtnUnidade_Click(object sender, RoutedEventArgs e)
        {
            if (
            MessageBox.Show("MANDAR MENSAGEM VIA WHATSAPP \n\n " +
            "1º Instale o WhatsApp em seu computador \n" +
            " https://web.whatsapp.com/ " +
            " \n" +
            "2º Instale o WhatsApp em seu computador   \n" +
            "3º Vincule o número que você vai mandar a mensagem.  \n " +
            "4º Se for Brasil vai ser 55 + DDD + Telefone ou Celular  \n " +
            "Obs: Nessa primeira versão ainda não anexamos o XML e PDF.   \n \n" +
            " Você gostou desse recurso? ", " Ajuda do sistema! WHATSAPP ", MessageBoxButton.YesNo, MessageBoxImage.Information)
             == MessageBoxResult.Yes
            )
                Funcoes.Analitico("TELARETORNO WHATSAPP SIM");
            else
                Funcoes.Analitico("TELARETORNO WHATSAPP NÃO");
        }


    }
}
