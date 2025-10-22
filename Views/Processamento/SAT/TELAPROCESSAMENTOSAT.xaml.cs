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

namespace nfecreator
{
    /// <summary>
    /// Lógica interna para TELAPROCESSAMENTONFCE.xaml
    /// </summary>
    public partial class TELAPROCESSAMENTOSAT : MetroWindow
    {
        VendaSatce venda;
        VendaSatcea vendaa;
        string linkinformacao = "";
        int nrvenda;
        private readonly string _path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public TELAPROCESSAMENTOSAT(string param00 = "")
        {
            InitializeComponent();
            Thread thread = new Thread(AtualizarInformacao);
            thread.Start();
            System.Threading.Thread.Sleep(2 * 1000);
            try
            {
                if (param00 != "")
                {
                    NumeroTextBox.Text = param00.Trim();
                    Button_Click(null, null);
                }
            }
            catch (Exception ex)
            {
               CongelarTela(false);
                Funcoes.Crashe(ex, "ERRO - Code:TELAPROCESSAMENTONFCE");
            }
        }

        private void AtualizarInformacao()
        {
            try
            {
                Parametrogeral geral = new Parametrogeral(3);
                if( geral.Validade >= DateTime.Now) { 
                    string informacao = geral.Informacao;
                    linkinformacao = geral.Linkinformacao;
                    string linkimagem = geral.Linkimagem;

                    Dispatcher.Invoke(new Action(() =>
                    {
                        StatusLabel.Text = informacao;
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
            BtnAutorizar.IsEnabled = flag;
            BtnVerificar.IsEnabled = flag;
            BtnCancelar.IsEnabled = flag;
            BtnInutulizar.IsEnabled = flag;
            //BtnEmLote.IsEnabled = flag;
            StatusSefazBtn.IsEnabled = flag;
            ConfiguracaoBtn.IsEnabled = flag;
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
                    Dispatcher.Invoke(new Action(() => {
                        Show();
                        Activate();
                        //FuncoesSATCE autorizarNFCe = new FuncoesSATCE();
                        //autorizarNFCe.ImprimirDireto(venda, vendaa);

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
                if (vendaa.Statusnfe == "CANCELADO")
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
                if (vendaa.Statusnfe == "INUTILIZADO")
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
                //nrvenda = Convert.ToInt32(NumeroTextBox.Text.Trim());
                //CongelarTela(true, "Autorizando nota. Aguarde...");
                //await Task.Run(() =>
                //{
                //    venda = new VendaSatce(nrvenda);
                //    vendaa = new VendaSatcea(nrvenda);

                //    if (vendaa.Statusnfe == "PENDENTE")
                //    {
                //        string retorno = "";
                //        Dispatcher.Invoke(new Action(() =>
                //            {

                //                FuncoesSATCE funcaosat = new FuncoesSATCE();
                //                retorno = funcaosat.Autoriza(venda, vendaa);

                //            }), DispatcherPriority.ContextIdle, null);

                //        Button_Click(null, null);

                //        if (retorno != "" && retorno != null)
                //        {
                //            CongelarTela(true, "Obtendo retorno da SEFAZ.");
                //            Dispatcher.Invoke(new Action(() =>
                //            {
                //                telaretorno.CarregarSAT(venda.Nrvenda, retorno, "RetornoNFeAutorizacao");
                //                telaretorno.Visibility = Visibility.Visible;
                //            }), DispatcherPriority.ContextIdle, null);
                //        }
                //    }
                //    else
                //        Funcoes.Mensagem("Coloque a venda como PENDENTE.", "STATUS DA VENDA", MessageBoxButton.OK, MessageBoxImage.Warning);
                //});
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
                    venda = new VendaSatce(nrvenda);
                    vendaa = new VendaSatcea(nrvenda);

                    string retorno = "";
                    Dispatcher.Invoke(new Action(() => {
                        //FuncoesSATCE fnfce = new FuncoesSATCE();
                        //retorno = fnfce.Verificar(venda, vendaa);
                    }), DispatcherPriority.ContextIdle, null);

                    Button_Click(null, null);

                if (retorno != "" && retorno != null)
                {
                        CongelarTela(true, "Obtendo retorno da SEFAZ.");
                        Dispatcher.Invoke(new Action(() => {
                            telaretorno.CarregarSAT(venda.Nrvenda, retorno, "RetornoNfeConsultaProtocolo");
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
                            venda = new VendaSatce(nrvenda);
                            vendaa = new VendaSatcea(nrvenda);

                            string retorno = "";
                            Dispatcher.Invoke(new Action(() => {

                                //FuncoesSATCE autorizarNFCe = new FuncoesSATCE();
                                //retorno = autorizarNFCe.Inutilizar(venda, vendaa, justificativa);

                            }), DispatcherPriority.ContextIdle, null);

                            Button_Click(null, null);

                            if (retorno != "" && retorno != null)
                            {
                                CongelarTela(true, "Obtendo retorno da SEFAZ.");
                                Dispatcher.Invoke(new Action(() => {
                                    telaretorno.CarregarSAT(venda.Nrvenda, retorno, "RetornoNfeInutilizacao");
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
                    justificativa = Funcoes.InpuBox(this, "Cancelar", "Justificativa").Trim();
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
                            venda = new VendaSatce(nrvenda);
                            vendaa = new VendaSatcea(nrvenda);

                            string retorno = "";
                            Dispatcher.Invoke(new Action(() => {

                                //FuncoesSATCE autorizarNFCe = new FuncoesSATCE();
                                //retorno = autorizarNFCe.Cancelar(venda, vendaa, justificativa);

                            }), DispatcherPriority.ContextIdle, null);

                            Button_Click(null, null);

                            if (retorno != "" && retorno != null)
                            {
                                CongelarTela(true, "Obtendo retorno da SEFAZ.");
                                Dispatcher.Invoke(new Action(() => {
                                    telaretorno.CarregarSAT(venda.Nrvenda, retorno, "RetornoRecepcaoEvento");
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
                Dispatcher.Invoke(new Action(() => {
                    nrvenda = Convert.ToInt32(NumeroTextBox.Text.Trim());
                    venda = new VendaSatce(nrvenda);
                    vendaa = new VendaSatcea(nrvenda);

                    if (venda.Nrvenda != vendaa.Nrvenda) {

                        if (vendaa.Nrvenda == 0)
                        {
                            vendaa.Nrvenda = venda.Nrvenda;
                            vendaa.Statusnfe = "PENDENTE";
                            vendaa.Insert();
                            vendaa = new VendaSatcea(nrvenda);
                        }
                    }

                    serieTextBox.Text = venda.Serie.ToString();
                    nomeTextBox.Text = venda.Nomecliente;
                    valorTextBox.Text = venda.Vnf.ToString();

                    chaveTextBox.Text = vendaa.Chave;
                    statusTextBox.Text = vendaa.Statusnfe;
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
                Dispatcher.Invoke(new Action(() => { StatusLabel.Text = " ";
                    nrvenda = Convert.ToInt32(NumeroTextBox.Text.Trim());
                    venda = new VendaSatce(nrvenda);
                    vendaa = new VendaSatcea(nrvenda);
                }), DispatcherPriority.ContextIdle, null);
                Dispatcher.Invoke(new Action(() => { BtnStatusNFeIcon.Foreground = new SolidColorBrush(Colors.White); }), DispatcherPriority.ContextIdle, null);
               
                if (!string.IsNullOrEmpty(venda.Status))
                {
                    //FuncoesSATCE funcaosat = new FuncoesSATCE();
                    //string retorno = funcaosat.Comunicacao(venda, vendaa);

                    //if(retorno =="Emitido com sucesso")
                    //{
                    //    Dispatcher.Invoke(new Action(() => {
                    //        StatusLabel.Text = "Comunicado com sucesso";
                    //        BtnStatusNFeIcon.Foreground = new SolidColorBrush(Colors.Green); 
                    //    }), DispatcherPriority.ContextIdle, null);
                    //}
                    //else
                    //{
                    //    Dispatcher.Invoke(new Action(() => {
                    //        StatusLabel.Text = retorno;
                    //        BtnStatusNFeIcon.Foreground = new SolidColorBrush(Colors.Red); 
                    //    }), DispatcherPriority.ContextIdle, null);
                    //}


                }

            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ATENÇÃO!!!");
            }

        }

    }
}
