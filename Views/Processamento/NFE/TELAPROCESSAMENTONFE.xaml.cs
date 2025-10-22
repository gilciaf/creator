using DFe.Utils;
using FontAwesome5;
using FontAwesome5.Extensions;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace nfecreator
{
    /// <summary>
    /// Lógica interna para TELAPROCESSAMENTONFE.xaml
    /// </summary>
    public partial class TELAPROCESSAMENTONFE : MetroWindow
    {
       VendaNFe vendanfe;
       Vendanfea vendanfea;
       int nrvenda;
       string linkinformacao = "";
       private ConfiguracaoApp _configuracoes;

        private void CarregarConfiguracao()
        {
            try
            {
                string _path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string ArquivoConfiguracao = @"\configuracao.xml";

                _configuracoes = !File.Exists(_path + ArquivoConfiguracao)
                    ? new ConfiguracaoApp()
                    : FuncoesXml.ArquivoXmlParaClasse<ConfiguracaoApp>(_path + ArquivoConfiguracao);              
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "",  false);
            }
        }

        public TELAPROCESSAMENTONFE(string param00 = "")
        {
            try
            {
                
                InitializeComponent();
                updateButton.Visibility = Visibility.Collapsed;
                AtualizarInformacao();
                
                if (!string.IsNullOrEmpty(param00))
                {
                    NumeroTextBox.Text = param00.Trim();
                    Button_Click(null, null);
                }else
                {
                    Thread thread = new Thread(Certificado);
                    thread.Start();
                }

            }
            catch (Exception ex)
            {
                CongelarTela(false);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                Hversoes versao = new Hversoes(Assembly.GetExecutingAssembly().GetName().Version.ToString());
                if (versao.ExisteAtualizacao())
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        updateButton.Visibility = Visibility.Visible;
                        statusLabel.Text += "Nova versão disponivel! Atualize no depois que terminar sua nota. " + versao.Notas;
                    }), DispatcherPriority.ContextIdle, null);

                } 
                else
                {
                    Dispatcher.Invoke(new Action(() => {
                        updateButton.Visibility = Visibility.Collapsed;
                    }), DispatcherPriority.ContextIdle, null);

                }

            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ATENÇÃO!!! Window_Loaded", true);
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Hversoes versao = new Hversoes(App.Parametro.Versao);
            versao = new Hversoes(versao.Ultimaversao());
            if (MessageBox.Show("Nova versão disponivel! Atualize já! " +
                        "\n " + versao.Notas +
                        "\n\nDeseja atualizar o seu Módulo Fiscal agora?", "Atualização!!!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                ///Hversoes versao = new Hversoes(App.Parametro.Versao);
                //versao = new Hversoes(versao.Ultimaversao());
                string _path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\dll\";
               
                statusLabel.Text = "Aguarde enquanto está atualizando.";
                TELADEATUALIZACAO tela = new TELADEATUALIZACAO();
                tela.Show();
                tela.DownloadFile(versao.Url, _path + versao.Filename, "DOWNLOAD DE ATUALIZAÇÃO");

            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(updateButton.Visibility == Visibility.Visible)
            {
                Hversoes versao = new Hversoes(App.Parametro.Versao);
                versao = new Hversoes(versao.Ultimaversao());
                if (MessageBox.Show("Nova versão disponivel! Atualize já! " +
                            "\n " + versao.Notas +
                           "\n\nDeseja atualizar o seu sistema para nova versão?", "Atualização!!!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                    ///Hversoes versao = new Hversoes(App.Parametro.Versao);
                    //versao = new Hversoes(versao.Ultimaversao());
                    string _path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\dll\";

                    TELADEATUALIZACAO tela = new TELADEATUALIZACAO();
                    tela.Show();
                    tela.DownloadFile(versao.Url, _path + versao.Filename, "DOWNLOAD DE ATUALIZAÇÃO");

                }
            }

        }





        /// <summary>
        /// NF-e
        /// </summary>


        private void Certificado()
        {
            try
            {
                CarregarConfiguracao();
                int valor = FuncoesNFe.StatusdeServicos(false);
                if (_configuracoes.CfgServico.Certificado.TipoCertificado == TipoCertificado.A1Arquivo)
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        BtnStatusSefaz_Click(null, null);
                    }), DispatcherPriority.ContextIdle, null);

                }
            }
            catch (Exception ex)
            {
                CongelarTela(false);

            }
        }

        private void AtualizarInformacao()
        {
            try
            {
                Parametrogeral geral = new Parametrogeral(2);
                if (geral.Validade >= DateTime.Now)
                {
                    string informacao = geral.Informacao;
                    linkinformacao = geral.Linkinformacao;
                    string linkimagem = geral.Linkimagem;

                    Dispatcher.Invoke(new Action(() =>
                    {
                        statusLabel.Text += informacao;
                        if (linkimagem.Trim() != "") ImagedeFundo.Source = new BitmapImage(new Uri(linkimagem));
                    }), DispatcherPriority.ContextIdle, null);
                }                
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "", false);
            }


        }

        private void Botoes(bool flag)
        {
            try 
            { 
                BtnAutorizar.IsEnabled = flag;
                BtnVerificar.IsEnabled = flag;
                BtnCancelar.IsEnabled = flag;
                BtnInutulizar.IsEnabled = flag;
                BtnEmLote.IsEnabled = flag;
                StatusSefazBtn.IsEnabled = flag;
                ConfiguracaoBtn.IsEnabled = flag;
            }
            catch (Exception ex)
            {
                CongelarTela(false);

            }
        }

        private void CongelarTela(bool flag, string texto = "Carregando...")
        {
            try
            {
                Dispatcher.Invoke(new Action(() => {
                    if (flag)
                    {
                        Space.Visibility = Visibility.Collapsed;
                        Spinner.Visibility = Visibility.Visible;
                        CarregandoTextBlock.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        Space.Visibility = Visibility.Visible;
                        Spinner.Visibility = Visibility.Collapsed;
                        CarregandoTextBlock.Visibility = Visibility.Collapsed;
                    }
                       

                    Botoes(!flag);
                    NumeroTextBox.IsReadOnly = flag;

                    CarregandoTextBlock.Text = texto;

                }), DispatcherPriority.ContextIdle, null);
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "", false);
            }
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
                CongelarTela(false);

            }
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            try
            {
                ImagedeFundo.Source = new BitmapImage(new Uri(@"/Pics/CREATORBANNER.png", UriKind.Relative));
            }
            catch (Exception ex)
            {
                CongelarTela(false);

            }
        }

        public void Autorizar()
        {
           try
            {
                Autorizar_Click(null, null);
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex);
                CongelarTela(false);
            }
        }

        public void Cancelar(string justificativa)
        {
            try
            {
                JustificativaTextBox.Text = justificativa;
                JustificativaTextBox.Visibility = Visibility.Visible;
                JustificativaTextBlock.Visibility = Visibility.Visible;
                Cancelar_Click(null, null);
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex);
                CongelarTela(false);
            }
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dispatcher.Invoke(new Action(() => {
                    nrvenda = Convert.ToInt32(NumeroTextBox.Text.Trim());
                    vendanfe = new VendaNFe(nrvenda);
                    vendanfea = new Vendanfea(nrvenda);

                    if (vendanfe.Nrvenda != vendanfea.Nrvenda)
                    {

                    if (vendanfea.Nrvenda == 0)
                    {
                        vendanfea.Nrvenda = vendanfe.Nrvenda;
                        vendanfea.Statusnfe = "PENDENTE";
                        vendanfea.Insert();
                        vendanfea = new Vendanfea(nrvenda);
                    }
                    }

                    serieTextBox.Text = vendanfe.Serie.ToString();
                    nomeTextBox.Text = vendanfe.Nomecliente;
                    valorTextBox.Text = vendanfe.Vnf.ToString();
                    chaveTextBox.Text = vendanfea.Chave;
                    statusTextBox.Text = vendanfea.Statusnfe;
                }), DispatcherPriority.ContextIdle, null);
            }
            catch (Exception ex)
            {
                CongelarTela(false);

            }
        }

        private void Parametros_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Application.Current.Windows.OfType<TELAPARAMETROS>().Count() > 0)
            {
                Application.Current.Windows.OfType<TELAPARAMETROS>().First().WindowState = WindowState.Normal;
                Application.Current.Windows.OfType<TELAPARAMETROS>().First().Activate();
            }
            else
            {

                TELAPARAMETROS tela = new TELAPARAMETROS();
                tela.Show();
                this.Close();
            }
            }
            catch (Exception ex)
            {
                CongelarTela(false);
            }
        }

        private async void Autorizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CongelarTela(true);
                string numer = NumeroTextBox.Text.Trim();
                CongelarTela(true, "Autorizando nota. Aguarde...");
                await Task.Run(() =>
                {
                    nrvenda = Convert.ToInt32(numer);
                    vendanfe = new VendaNFe(nrvenda);
                    vendanfea = new Vendanfea(nrvenda);

                    string retorno = "";
                    Dispatcher.Invoke(new Action(() => {
                        FuncoesNFe autorizarNFe = new FuncoesNFe();
                        retorno = autorizarNFe.Autoriza(vendanfe, vendanfea);
                    }), DispatcherPriority.ContextIdle, null);

                    Button_Click(null, null);

                    if (retorno != "" && retorno != null)
                    {
                        CongelarTela(true,  "Obtendo retorno da SEFAZ.");
                        Dispatcher.Invoke(new Action(() => {
                            telaretorno.Carregar(vendanfe.Nrvenda, retorno, "RetornoNFeAutorizacao");
                            telaretorno.Visibility = Visibility.Visible;
                        }), DispatcherPriority.ContextIdle, null);
                    }
                });

                CongelarTela(false);

            }
            catch (Exception ex)
            {
                CongelarTela(false);
            }
        }

        private async void Verificar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CongelarTela(true);
                nrvenda = Convert.ToInt32(NumeroTextBox.Text.Trim());
                CongelarTela(true, "Verificando nota. Aguarde...");
                await Task.Run(() =>
                {
                    vendanfe = new VendaNFe(nrvenda);
                    vendanfea = new Vendanfea(nrvenda);

                    string retorno = "";
                    Dispatcher.Invoke(new Action(() => {
                        FuncoesNFe autorizarNFe = new FuncoesNFe();
                        retorno = autorizarNFe.Verificar(vendanfe, vendanfea);
                    }), DispatcherPriority.ContextIdle, null);

                    Button_Click(null, null);

                    if (retorno != "" && retorno != null)
                    {
                        CongelarTela(true, "Obtendo retorno da SEFAZ.");
                        Dispatcher.Invoke(new Action(() => {
                            telaretorno.Carregar(vendanfe.Nrvenda, retorno, "RetornoNfeConsultaProtocolo");
                            telaretorno.Visibility = Visibility.Visible;
                        }), DispatcherPriority.ContextIdle, null);
                    }
                });

                CongelarTela(false);

            }
            catch (Exception ex)
            {
                CongelarTela(false);

            }
        }
               
        private async void Inutilizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CongelarTela(true);

                var justificativa = Funcoes.InpuBox(this, "Inutilizar NFe", "Justificativa").Trim();
                if (!string.IsNullOrEmpty(justificativa))
                {
                    if (justificativa.Length > 15)
                    {
                        CongelarTela(true, "Inutilizando nota. Aguarde...");
                        nrvenda = Convert.ToInt32(NumeroTextBox.Text.Trim());
                        await Task.Run(() =>
                        {
                                    vendanfe = new VendaNFe(nrvenda);
                                    vendanfea = new Vendanfea(nrvenda);

                                    string retorno = "";
                                    Dispatcher.Invoke(new Action(() => {
                                        FuncoesNFe autorizarNFe = new FuncoesNFe();
                                        retorno = autorizarNFe.Inutilizar(vendanfe, vendanfea, justificativa);
                                    }), DispatcherPriority.ContextIdle, null);

                                    Button_Click(null, null);

                                    if (retorno != "" && retorno != null)
                                    {
                                        CongelarTela(true,"Obtendo retorno da SEFAZ.");
                                        Dispatcher.Invoke(new Action(() => {
                                            telaretorno.Carregar(vendanfe.Nrvenda, retorno, "RetornoNfeInutilizacao");
                                            telaretorno.Visibility = Visibility.Visible;
                                        }), DispatcherPriority.ContextIdle, null);
                                     }
                        });                    
                    }
                    else
                    {
                        Funcoes.Mensagem("Informe no mínimo 15 caracteres na justificativa!", " CANCELA NOTA FISCAL ", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                CongelarTela(false);

            }
            catch (Exception ex)
            {
                CongelarTela(false);

            }
        }

        private async void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CongelarTela(true);

                string justificativa = "";
                if (JustificativaTextBox.Text.Replace(" ", "") == "")
                    justificativa = Funcoes.InpuBox(this, "Cancelar NFe", "Justificativa").Trim();
                else 
                    justificativa = JustificativaTextBox.Text.Trim();

                if (!string.IsNullOrEmpty(justificativa))
                {
                    if (justificativa.Length > 15)
                    {

                        nrvenda = Convert.ToInt32(NumeroTextBox.Text.Trim());
                        CongelarTela(true, "Cancelando nota. Aguarde...");
                        await Task.Run(() =>
                        {
                            vendanfe = new VendaNFe(nrvenda);
                            vendanfea = new Vendanfea(nrvenda);

                            string retorno = "";
                            Dispatcher.Invoke(new Action(() => {
                                FuncoesNFe autorizarNFe = new FuncoesNFe();
                                retorno = autorizarNFe.Cancelar(vendanfe, vendanfea, justificativa);
                            }), DispatcherPriority.ContextIdle, null);

                            Button_Click(null, null);

                            if (retorno != "" && retorno != null)
                            {
                                CongelarTela(true,"Obtendo retorno da SEFAZ.");
                                    Dispatcher.Invoke(new Action(() => {
                                        telaretorno.Carregar(vendanfe.Nrvenda, retorno, "RetornoRecepcaoEvento");
                                        telaretorno.Visibility = Visibility.Visible;
                                    }), DispatcherPriority.ContextIdle, null);
                                }
                            });
                    }
                    else
                    {
                        Funcoes.Mensagem("Informe no mínimo 15 caracteres na justificativa!", " CANCELA NOTA FISCAL ", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                CongelarTela(false);

            }
            catch (Exception ex)
            {
                CongelarTela(false);

            }

        }

        private async void BtnEmLote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CarregarConfiguracao();
                var stringnincial = Funcoes.InpuBox(this, "EM LOTE - Numero Inicial", "Numero Inicial:", "0").Trim();

                var stringnfinal = Funcoes.InpuBox(this, "EM LOTE - Numero Final", "Numero Final:", "0").Trim();

                int ninicial = Convert.ToInt32(stringnincial);
                int nfinal = Convert.ToInt32(stringnfinal);

                CongelarTela(true, "Iniciando a autorização em lote.");
                for (int i = ninicial; i <= nfinal; i++)
                {
                    NumeroTextBox.Text = i.ToString();
                    Button_Click(null, null);
                    CongelarTela(true, "Autorizando nota "+ i.ToString() +". Aguarde...");
                    try
                    {

                        nrvenda = Convert.ToInt32(NumeroTextBox.Text.Trim());
                        vendanfe = new VendaNFe(nrvenda);
                        vendanfea = new Vendanfea(nrvenda);
                        await Task.Run(() =>
                        {
                            string retorno = "";
                            Dispatcher.Invoke(new Action(() => {
                                FuncoesNFe autorizarNFe = new FuncoesNFe();
                                retorno = autorizarNFe.Autoriza(vendanfe, vendanfea);

                                vendanfea = new Vendanfea(nrvenda);
                                if (vendanfea.Statusnfe == "APROVADO")
                                    if (_configuracoes.ConfiguracaoDanfeNfe.ImpressaoLote == "SIM")
                                        autorizarNFe.ImprimirDireto(vendanfe, vendanfea);

                            }), DispatcherPriority.ContextIdle, null);
                            
                            Button_Click(null, null);

                           

                            CongelarTela(true, "Aguardando processamento da próxima nota.");
                            System.Threading.Thread.Sleep(4 * 1000);
                                  

                        });
                       
                    }
                    catch (Exception ex)
                    {
                        
                    }


                }
                CongelarTela(false);



            }
            catch (Exception ex)
            {
                CongelarTela(false);
            }

        }

        private void Sair_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            System.Windows.Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                CongelarTela(false);

            }
        }

        private void NumeroTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
            {
                Button_Click(null, null);
                Funcoes.KeyDown(e, this);
            }
            }
            catch (Exception ex)
            {
                CongelarTela(false);
            }
        }

        private void BtnStatusSefaz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var flag = true;
                int contador = 0;
                var circulo = EFontAwesomeIconsExtensions.CreateImageSource(EFontAwesomeIcon.Solid_Circle, System.Windows.Media.Brushes.Yellow, 10);
               
                Dispatcher.Invoke(new Action(() => { statusLabel.Text = " "; }), DispatcherPriority.ContextIdle, null);
                Dispatcher.Invoke(new Action(() => { BtnStatusNFeIcon.Foreground = new SolidColorBrush(Colors.Transparent); }), DispatcherPriority.ContextIdle, null);
                Dispatcher.Invoke(new Action(() => { TaskbarItemInfo.Overlay = circulo; }), DispatcherPriority.ContextIdle, null);

                
                int valor = FuncoesNFe.StatusdeServicos(false);
                if (valor == 0)
                {

                    Dispatcher.Invoke(new Action(() => { BtnStatusNFeIcon.Foreground = new SolidColorBrush(Colors.Yellow); }), DispatcherPriority.ContextIdle, null);
                    Dispatcher.Invoke(new Action(() => { statusLabel.Text = " Sefaz: a consulta ao servidor da SEFAZ retornou negativa."; }), DispatcherPriority.ContextIdle, null);
                    do
                    {
                        Thread.Sleep(5000);
                        valor = FuncoesNFe.StatusdeServicos(true);
                        contador++;
                        if (contador > 60)
                        {
                            Dispatcher.Invoke(new Action(() => { statusLabel.Text = " Sefaz : o servidor da SEFAZ falta Serviço"; }), DispatcherPriority.ContextIdle, null);
                            Dispatcher.Invoke(new Action(() => { BtnStatusNFeIcon.Foreground = new SolidColorBrush(Colors.Red); }), DispatcherPriority.ContextIdle, null);

                            circulo = EFontAwesomeIconsExtensions.CreateImageSource(EFontAwesomeIcon.Solid_Circle, System.Windows.Media.Brushes.Red, 10);
                            Dispatcher.Invoke(new Action(() => { TaskbarItemInfo.Overlay = circulo; }), DispatcherPriority.ContextIdle, null);
                        }
                        Dispatcher.Invoke(new Action(() => { flag = Convert.ToBoolean(Application.Current.Windows.OfType<TELAPARAMETROS>().Count() > 0); }), DispatcherPriority.ContextIdle, null);
                        Dispatcher.Invoke(new Action(() => {
                            if(Application.Current.Windows.OfType<TELAPROCESSAMENTONFE>().Count() > 0) flag = false;
                        }), DispatcherPriority.ContextIdle, null);

                    } while (valor == 0 && flag);
                }
                if (valor == 107)
                {
                    Dispatcher.Invoke(new Action(() => { BtnStatusNFeIcon.Foreground = new SolidColorBrush(Colors.Green); }), DispatcherPriority.ContextIdle, null);
                    Dispatcher.Invoke(new Action(() => { statusLabel.Text = "Sefaz: o servidor da SEFAZ se encontra normal."; }), DispatcherPriority.ContextIdle, null);
                    circulo = EFontAwesomeIconsExtensions.CreateImageSource(EFontAwesomeIcon.Solid_Circle, System.Windows.Media.Brushes.Green, 10);
                    Dispatcher.Invoke(new Action(() => { TaskbarItemInfo.Overlay = circulo; }), DispatcherPriority.ContextIdle, null);
                }
                NumeroTextBox.Focus();

            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ATENÇÃO!!!");

            }

        }

        private void NumeroTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                Button_Click(null, null);
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ATENÇÃO!!!");

            }
        }

        
    }
}
