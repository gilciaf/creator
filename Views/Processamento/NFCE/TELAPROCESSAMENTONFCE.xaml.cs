using System;
using MahApps.Metro.Controls;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Media;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using DFe.Utils;
using FontAwesome5;
using FontAwesome5.Extensions;

namespace nfecreator
{
    /// <summary>
    /// Lógica interna para TELAPROCESSAMENTONFCE.xaml
    /// </summary>
    public partial class TELAPROCESSAMENTONFCE : MetroWindow
    {
        VendaNFCe vendanfce;
        Vendanfcea vendanfcea;
        string linkinformacao = "";
        int nrvenda;
        private ConfiguracaoApp _configuracoes;

        public TELAPROCESSAMENTONFCE(string param00 = "")
        {
            try
            {
                InitializeComponent();
                AtualizarInformacao();


                if (!string.IsNullOrEmpty(param00))
                {
                    NumeroTextBox.Text = param00.Trim();
                    Button_Click(null, null);
                }
                else
                {
                    Thread thread = new Thread(Certificado);
                    thread.Start();
                } 

            }
            catch (Exception ex)
            {
                CongelarTela(false);
                Funcoes.Crashe(ex, "ERRO - Code:TELAPROCESSAMENTONFCE");
            }
        }


        /// <summary>
        /// Update
        /// </summary>
        ///
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
                        statusLabel.Text = "Nova versão disponivel! Atualize no depois que terminar sua nota. " + versao.Notas;
                    }), DispatcherPriority.ContextIdle, null);

                }
                else
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
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
            if (updateButton.Visibility == Visibility.Visible)
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
        /// NFC-e
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
                Funcoes.Crashe(ex, "ERRO - Code:CERTIFICADONATELA");
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
                        statusLabel.Text = informacao;
                        if (linkimagem.Trim() != "") ImagedeFundo.Source = new BitmapImage(new Uri(linkimagem));
                    }), DispatcherPriority.ContextIdle, null);
                }
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "", false);
            }
        }

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
                //BtnEmLote.IsEnabled = flag;
                StatusSefazBtn.IsEnabled = flag;
                ConfiguracaoBtn.IsEnabled = flag;
            }
            catch (Exception ex)
            {
                CongelarTela(false);
                Funcoes.Crashe(ex, "ERRO - Code:BOTOES");
            }
        }

        private void CongelarTela(bool flag, string texto = "Carregando...")
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
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
                Funcoes.Analitico("TELAPROCESSAMENTONFCE INFORMACAO");
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

        public void Autorizar(string status)
        {
            try
            {
                Autorizar_Click(null, null);
                Button_Click(null, null);
                if (status == "show")
                {
                    Show();
                    Activate();
                }
                else
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        FuncoesNFCe autorizarNFCe = new FuncoesNFCe();
                        autorizarNFCe.ImprimirDireto(vendanfce, vendanfcea);
                    }), DispatcherPriority.ContextIdle, null);
                    this.Close();
                }


            }
            catch (Exception ex)
            {
                CongelarTela(false);
                Funcoes.Crashe(ex, "ERRO - Code:Autorizar");
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
                Button_Click(null, null);
                if (vendanfcea.Statusnfe == "CANCELADO")
                {
                    this.Close();
                }


            }
            catch (Exception ex)
            {
                CongelarTela(false);

            }
        }
        public void Inutilizar(string justificativa)
        {
            try
            {
                JustificativaTextBox.Text = justificativa;
                JustificativaTextBox.Visibility = Visibility.Visible;
                JustificativaTextBlock.Visibility = Visibility.Visible;
                Inutilizar_Click(null, null);
                Button_Click(null, null);
                if (vendanfcea.Statusnfe == "INUTILIZADO")
                {
                    this.Close();
                }
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
                    tela.Activate();
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
                nrvenda = Convert.ToInt32(NumeroTextBox.Text.Trim());
                CongelarTela(true, "Autorizando nota. Aguarde...");
                await Task.Run(() =>
                {
                    vendanfce = new VendaNFCe(nrvenda);
                    vendanfcea = new Vendanfcea(nrvenda);

                    string retorno = "";
                    Dispatcher.Invoke(new Action(() =>
                    {
                        FuncoesNFCe autorizarNFCe = new FuncoesNFCe();
                        retorno = autorizarNFCe.Autoriza(vendanfce, vendanfcea);
                    }), DispatcherPriority.ContextIdle, null);

                    Button_Click(null, null);

                    if (retorno != "" && retorno != null)
                    {
                        CongelarTela(true, "Obtendo retorno da SEFAZ.");
                        Dispatcher.Invoke(new Action(() =>
                        {
                            telaretorno.CarregarNFCE(vendanfce.Nrvenda, retorno, "RetornoNFeAutorizacao");
                            telaretorno.Visibility = Visibility.Visible;
                        }), DispatcherPriority.ContextIdle, null);
                    }
                });
                CongelarTela(false);

            }
            catch (Exception ex)
            {
                CongelarTela(false);
                Funcoes.Crashe(ex, "ERRO - Code:Autorizar");
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
                    vendanfce = new VendaNFCe(nrvenda);
                    vendanfcea = new Vendanfcea(nrvenda);

                    string retorno = "";
                    Dispatcher.Invoke(new Action(() =>
                    {
                        FuncoesNFCe fnfce = new FuncoesNFCe();
                        retorno = fnfce.Verificar(vendanfce, vendanfcea);
                    }), DispatcherPriority.ContextIdle, null);

                    Button_Click(null, null);

                    if (retorno != "" && retorno != null)
                    {
                        CongelarTela(true, "Obtendo retorno da SEFAZ.");
                        Dispatcher.Invoke(new Action(() =>
                        {
                            telaretorno.CarregarNFCE(vendanfce.Nrvenda, retorno, "RetornoNfeConsultaProtocolo");
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

                string justificativa = "";
                if (JustificativaTextBox.Text.Replace(" ", "") == "")
                    justificativa = Funcoes.InpuBox(this, "Inutilizar NFe", "Justificativa").Trim();
                else
                    justificativa = JustificativaTextBox.Text.Trim();

                if (!string.IsNullOrEmpty(justificativa))
                {
                    if (justificativa.Length > 15)
                    {
                        CongelarTela(true, "Inutilizando nota. Aguarde...");
                        nrvenda = Convert.ToInt32(NumeroTextBox.Text.Trim());
                        await Task.Run(() =>
                        {
                            vendanfce = new VendaNFCe(nrvenda);
                            vendanfcea = new Vendanfcea(nrvenda);

                            string retorno = "";
                            Dispatcher.Invoke(new Action(() =>
                            {
                                FuncoesNFCe autorizarNFCe = new FuncoesNFCe();
                                retorno = autorizarNFCe.Inutilizar(vendanfce, vendanfcea, justificativa);
                            }), DispatcherPriority.ContextIdle, null);

                            Button_Click(null, null);

                            if (retorno != "" && retorno != null)
                            {
                                CongelarTela(true, "Obtendo retorno da SEFAZ.");
                                Dispatcher.Invoke(new Action(() =>
                                {
                                    telaretorno.CarregarNFCE(vendanfce.Nrvenda, retorno, "RetornoNfeInutilizacao");
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
                            vendanfce = new VendaNFCe(nrvenda);
                            vendanfcea = new Vendanfcea(nrvenda);

                            string retorno = "";
                            Dispatcher.Invoke(new Action(() =>
                            {
                                FuncoesNFCe autorizarNFCe = new FuncoesNFCe();
                                retorno = autorizarNFCe.Cancelar(vendanfce, vendanfcea, justificativa);

                            }), DispatcherPriority.ContextIdle, null);

                            Button_Click(null, null);

                            if (retorno != "" && retorno != null)
                            {
                                CongelarTela(true, "Obtendo retorno da SEFAZ.");
                                Dispatcher.Invoke(new Action(() =>
                                {
                                    telaretorno.CarregarNFCE(vendanfce.Nrvenda, retorno, "RetornoRecepcaoEvento");
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    nrvenda = Convert.ToInt32(NumeroTextBox.Text.Trim());
                    vendanfce = new VendaNFCe(nrvenda);
                    vendanfcea = new Vendanfcea(nrvenda);

                    if (vendanfce.Nrvenda != vendanfcea.Nrvenda)
                    {

                        if (vendanfcea.Nrvenda == 0)
                        {
                            vendanfcea.Nrvenda = vendanfce.Nrvenda;
                            vendanfcea.Statusnfe = "PENDENTE";
                            vendanfcea.Insert();
                            vendanfcea = new Vendanfcea(nrvenda);
                        }
                    }

                    serieTextBox.Text = vendanfce.Serie.ToString();
                    nomeTextBox.Text = vendanfce.Nomecliente;
                    valorTextBox.Text = vendanfce.Vnf.ToString();

                    chaveTextBox.Text = vendanfcea.Chave;
                    statusTextBox.Text = vendanfcea.Statusnfe;
                }), DispatcherPriority.ContextIdle, null);
            }
            catch (Exception ex)
            {
                CongelarTela(false);
                Funcoes.Crashe(ex, "ERRO - Code:ClickNFCe");
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
                Funcoes.Crashe(ex, "ERRO - Code:NumeroNFCE");
            }
        }

        private void BtnStatusSefaz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var flag = true;
                int contador = 0;
                Dispatcher.Invoke(new Action(() => { statusLabel.Text = " "; }), DispatcherPriority.ContextIdle, null);
                Dispatcher.Invoke(new Action(() => { BtnStatusNFeIcon.Foreground = new SolidColorBrush(Colors.Transparent); }), DispatcherPriority.ContextIdle, null);

                int valor = FuncoesNFCe.StatusdeServicos(false);
                if (valor == 0)
                {
                    Dispatcher.Invoke(new Action(() => { BtnStatusNFeIcon.Foreground = new SolidColorBrush(Colors.Yellow); }), DispatcherPriority.ContextIdle, null);
                    Dispatcher.Invoke(new Action(() => { statusLabel.Text = "Sefaz: a consulta ao servidor da SEFAZ retornou negativa."; }), DispatcherPriority.ContextIdle, null);
                    Dispatcher.Invoke(new Action(() =>
                    {
                        var circulo = EFontAwesomeIconsExtensions.CreateImageSource(EFontAwesomeIcon.Solid_Circle, System.Windows.Media.Brushes.Yellow, 10);
                        TaskbarItemInfo.Overlay = circulo;
                    }), DispatcherPriority.ContextIdle, null);
                    do
                    {
                        Thread.Sleep(5000);
                        valor = FuncoesNFCe.StatusdeServicos(true);
                        contador++;
                        if (contador > 60)
                        {
                            Dispatcher.Invoke(new Action(() => { statusLabel.Text = "Sefaz: o servidor da SEFAZ falta Serviço"; }), DispatcherPriority.ContextIdle, null);
                            Dispatcher.Invoke(new Action(() => { BtnStatusNFeIcon.Foreground = new SolidColorBrush(Colors.Red); }), DispatcherPriority.ContextIdle, null);
                            Dispatcher.Invoke(new Action(() =>
                            {
                                var circulo = EFontAwesomeIconsExtensions.CreateImageSource(EFontAwesomeIcon.Solid_Circle, System.Windows.Media.Brushes.Red, 10);
                                TaskbarItemInfo.Overlay = circulo;
                            }), DispatcherPriority.ContextIdle, null);
                        }
                        Dispatcher.Invoke(new Action(() => { flag = Convert.ToBoolean(Application.Current.Windows.OfType<TELAPARAMETROS>().Count() > 0); }), DispatcherPriority.ContextIdle, null);
                        Dispatcher.Invoke(new Action(() =>
                        {
                            if (Application.Current.Windows.OfType<TELAPROCESSAMENTONFCE>().Count() > 0) flag = false;
                        }), DispatcherPriority.ContextIdle, null);

                    } while (valor == 0 && flag);
                }
                if (valor == 107)
                {
                    Dispatcher.Invoke(new Action(() => { BtnStatusNFeIcon.Foreground = new SolidColorBrush(Colors.Green); }), DispatcherPriority.ContextIdle, null);
                    Dispatcher.Invoke(new Action(() => { statusLabel.Text = "Sefaz: o servidor da SEFAZ se encontra normal."; }), DispatcherPriority.ContextIdle, null);
                    Dispatcher.Invoke(new Action(() =>
                    {

                        var circulo = EFontAwesomeIconsExtensions.CreateImageSource(EFontAwesomeIcon.Solid_Circle, System.Windows.Media.Brushes.Green, 10);
                        TaskbarItemInfo.Overlay = circulo;
                    }), DispatcherPriority.ContextIdle, null);
                }
                NumeroTextBox.Focus();

            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ATENÇÃO!!!");
            }

        }

    }
}
