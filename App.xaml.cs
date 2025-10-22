using System;
using System.Windows;
using System.Globalization;
using System.Diagnostics;
using System.Linq;
using System.Windows.Shell;

namespace nfecreator
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {
      //  private static MySQLBase mysqlb;
      //  private static Usuarios usuariologado = new Usuarios();
        private static Parametros parametro;
       

        public App()
        {

            //_clientApp = PublicClientApplicationBuilder.Create(ClientId)
            //    .WithAuthority(AzureCloudInstance.AzurePublic, Tenant)
            //    .Build();
            //TokenCacheHelper.EnableSerialization(_clientApp.UserTokenCache);

        }


        internal static void OnStartup()
        {
            if (IsAppAlreadyRunning())
            {
               // MessageBox.Show("O aplicativo já está em execução!", "Já está em execução", MessageBoxButton.OK, MessageBoxImage.Warning);
                Process.GetCurrentProcess().Kill();
            }          
            //AppCenter.Start("a0ec84e9-843e-4077-b6ab-c07884a64840",
            //                       typeof(Analytics), typeof(Crashes));
            //AppCenter.LogLevel = LogLevel.Verbose;

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (IsAppAlreadyRunning())
            {
                //MessageBox.Show("O aplicativo já está em execução!", "Já está em execução", MessageBoxButton.OK, MessageBoxImage.Warning);
                Process.GetCurrentProcess().Kill();
            }           

            base.OnStartup(e);
            // AppCenter.LogLevel = Microsoft.AppCenter.LogLevel.Verbose;
            //AppCenter.Start("a0ec84e9-843e-4077-b6ab-c07884a64840",
            //                   typeof(Analytics), typeof(Crashes));

            var countryCode = RegionInfo.CurrentRegion.TwoLetterISORegionName;
            //AppCenter.SetCountryCode(countryCode);

        }


        private static bool IsAppAlreadyRunning()
        {

            Process currentProcess = Process.GetCurrentProcess();

            if (Process.GetProcessesByName(currentProcess.ProcessName).Any(p => p.Id != currentProcess.Id && !p.HasExited))
            {
                return true;
            }

            return false;

        }

        //private static readonly string ClientId = "a0ec84e9-843e-4077-b6ab-c07884a64840";

        // Note: Tenant is important for the quickstart. We'd need to check with Andre/Portal if we
        // want to change to the AadAuthorityAudience.
        //private static readonly string Tenant = "common";

        //private static IPublicClientApplication _clientApp;

        //public static IPublicClientApplication PublicClientApp { get { return _clientApp; } }
        //internal static MySQLBase Mysqlb { get => mysqlb; set => mysqlb = value; }
        //internal static Usuarios Usuariologado { get => usuariologado; set => usuariologado = value; }
        internal static Parametros Parametro { get => parametro; set => parametro = value; }


    }
}
