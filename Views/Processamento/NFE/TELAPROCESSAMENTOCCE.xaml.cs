using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;

namespace nfecreator
{
    /// <summary>
    /// Lógica interna para TELAPROCESSAMENTONFE.xaml
    /// </summary>
    public partial class TELAPROCESSAMENTOCCE : MetroWindow
    {  
        int nrvenda;
        Cartanfe cce;
        string linkinformacao = "";
        private readonly string _path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public TELAPROCESSAMENTOCCE(string param00 = "")
        {
            InitializeComponent();
            Thread thread = new Thread(AtualizarInformacao);
            thread.Start();
            if (param00 != "")
            {
                NumeroTextBox.Text = param00;
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

        private void ButtonSite_Click(object sender, RoutedEventArgs e)
        {
            Funcoes.Analitico("TELAPROCESSAMENTONFE INFORMACAO");
            if (linkinformacao.Trim() != "")
                System.Diagnostics.Process.Start(linkinformacao);
            else
                System.Diagnostics.Process.Start("https://ciaf.com.br/noticias");
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            ImagedeFundo.Source = new BitmapImage(new Uri(@"/Pics/CREATORBANNER.png", UriKind.Relative));
        }

        public void Autorizar()
        {
            Button_Click(null, null);
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                nrvenda = Convert.ToInt32(NumeroTextBox.Text.Trim());
            cce = new Cartanfe(nrvenda);

            nrnfeTextBox.Text = cce.Nrnfe.ToString();
            nrseqTextBox.Text = cce.Nrseq.ToString();
            chaveTextBox.Text = cce.Chavea.ToString();
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(new Action(() => { CarregamentoColo.Visibility = Visibility.Collapsed; }), DispatcherPriority.ContextIdle, null);

            }




        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
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


        private void Autorizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dispatcher.Invoke(new Action(() => { CarregamentoColo.Visibility = Visibility.Visible; }), DispatcherPriority.ContextIdle, null);

                nrvenda = Convert.ToInt32(NumeroTextBox.Text.Trim());
                FuncoesNFe autorizarNFe = new FuncoesNFe();
               // string retorno = autorizarNFe.TransmitirCarta(cce);

                autorizarNFe.TransmitirCarta(cce);
                //if (retorno != "" && retorno != null)
                //{
                //   // telaretorno.Carregar(cce.Nrvenda, retorno, "RetornoNFeAutorizacao");
                //    telaretorno.Visibility = Visibility.Visible;
                //}
                Dispatcher.Invoke(new Action(() => { CarregamentoColo.Visibility = Visibility.Collapsed; }), DispatcherPriority.ContextIdle, null);

            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(new Action(() => { CarregamentoColo.Visibility = Visibility.Collapsed; }), DispatcherPriority.ContextIdle, null);

            }
        }

        private void Verificar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dispatcher.Invoke(new Action(() => { CarregamentoColo.Visibility = Visibility.Visible; }), DispatcherPriority.ContextIdle, null);

                nrvenda = Convert.ToInt32(NumeroTextBox.Text.Trim());
                FuncoesNFe autorizarNFe = new FuncoesNFe();
                // string retorno = autorizarNFe.TransmitirCarta(cce);

                autorizarNFe.VerificarCarta(cce);

            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(new Action(() => { CarregamentoColo.Visibility = Visibility.Collapsed; }), DispatcherPriority.ContextIdle, null);

            }
        }



        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Close();
            System.Windows.Application.Current.Shutdown();
        }

        private void NumeroTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(null, null);
                Funcoes.KeyDown(e, this);
            }
        }
    }
}
