using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Windows.Shell;
using FontAwesome5;
using System.Windows.Threading;
using FontAwesome5.Extensions;

namespace nfecreator
{
    /// <summary>
    /// Lógica interna para TELADEATUALIZACAO.xaml
    /// </summary>
    public partial class TELADEATUALIZACAO : MetroWindow
    {
        Stopwatch sw = new Stopwatch();
        WebClient webClient;

        public TELADEATUALIZACAO()
        {
            InitializeComponent();
            var circulo = EFontAwesomeIconsExtensions.CreateImageSource(EFontAwesomeIcon.Solid_Download, Brushes.Yellow);
            Dispatcher.Invoke(new Action(() => { TaskbarItemInfo.Overlay = circulo;
                TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
            }), DispatcherPriority.ContextIdle, null);
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            webClient.CancelAsync();
            Close();
            /*throw new Exception("");*/
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TxtValor_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            e.Handled = true;
            // BtnOk.Focus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // TxtValor.Focus();
        }

        public void DownloadFile(string urlAddress, string location, string title = "")
        {
            if (title != "")
                this.Title = title;
            TxturlAddress.Text = urlAddress;
            TxtLocalion.Text = location;

            using (webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

                // The variable that will be holding the url address (making sure it starts with http://)
                Uri URL = urlAddress.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ? new Uri(urlAddress) : new Uri("https://" + urlAddress);

                // Start the stopwatch which we will be using to calculate the download speed
                sw.Start();

                try
                {
                    // Start downloading the file
                    //  webClient.DownloadFileAsync(URL, location);
                    webClient.DownloadFileAsync(URL, location);

                }
                catch (Exception ex)
                {
                    Funcoes.Crashe(ex, "ATENÇÃO!!!");
                }
            }
        }

        // The event that will fire whenever the progress of the WebClient is changed
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Calculate download speed and output it to labelSpeed.
            TxtVelocidade.Text = string.Format("Velocidade: {0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));

            // Update the progressbar percentage only when the value is not the same.
            progressBar.Value = e.ProgressPercentage;
            TaskbarItemInfo.ProgressValue = (double)e.ProgressPercentage / 100;

            // Show the percentage on our label.
            TxtPerc.Text = e.ProgressPercentage.ToString() + "%";

            // Update the label with how much data have been downloaded so far and the total size of the file we are currently downloading
            TxtDownloaded.Text = string.Format("Total {0} MB's \n/ {1} MB's",
                (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
                (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));
        }

        // The event that will trigger when the WebClient is completed
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            // Reset the stopwatch.
            sw.Reset();
            string _path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (e.Cancelled == true)
            {
                TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Paused;
                //MessageBox.Show("Download has been canceled.");
                MinhaNotificacao.NotificarAviso("Download", "Cancelado");
                System.Threading.Thread.Sleep(2000);
                this.Close();
            }
            else
            {
                MinhaNotificacao.NotificarAviso("Download", "Completo");
                ProcessStartInfo startInfo = new ProcessStartInfo(TxtLocalion.Text)
                {
                    WindowStyle = ProcessWindowStyle.Normal,
                    Arguments = @"/VERYSILENT /FORCECLOSEAPPLICATIONS /RESTARTAPPLICATIONS /DIR=""" + _path + @""" "
                };
                Process.Start(startInfo);
                System.Threading.Thread.Sleep(2000);
                this.Close();               
            }
        }   

    }
}   