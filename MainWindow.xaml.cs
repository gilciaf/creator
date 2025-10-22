using ControlzEx.Theming;
using MahApps.Metro.Theming;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
namespace nfecreator
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        private string param00 = "";
        private string tipo = "";
        private string modo = "";
        private bool naotematualizacao = true;


        public MainWindow()
        {
            // System.Diagnostics.Process.Start(@"C:\Ciaf\nfecreator.exe", " /parametros");

            App.OnStartup();
            Funcoes.Analitico("TELA-INICIO");
            InitializeComponent();
            SetTheme();

            try {
                //var obj = Activator.CreateInstance(Type.GetTypeFromProgID("NFeIntegratorInterOp.NFeIntegrator"));

                string justificativa = "";

                App.Parametro = new Parametros(1);
                NomeTextBox.Text = App.Parametro.Razaosocial;
                VerificarBanco();
                VerificarAtualizacao();

                if (naotematualizacao)
                {
                    var args = Environment.GetCommandLineArgs();
                    foreach (string parametros in args)
                    {
                        //Modo                        
                        //Maiusculo
                        if (parametros.Contains("/IdKeySistema="))
                        {
                            //idkeysistema=b772a449-3ccb-441d-9bbe-9f83b39207a1
                            //NF-e
                            var mod = parametros.Replace("/IdKeySistema=", "");
                            if (mod == "b772a449-3ccb-441d-9bbe-9f83b39207a1")
                                modo = "processamentocce";
                            if (mod == "b54bea0e-6e3c-4cf9-9f22-27a964ea3951")
                                modo = "processamentonfe";
                            if (mod == "ee7ae119-db83-4560-862d-4ff00a2dc40e")
                                modo = "processamentonfe";
                            //NFC-e
                            if (mod == "b6e58ec3-2de3-4041-b817-0625dd014396")
                                modo = "processamentonfce";
                            if (mod == "8bb8966a-0f1a-4dfd-ba4d-74e46a67dc2a")
                            {
                                modo = "processamentonfce";
                                tipo = "cancelar";
                            }
                            if (mod == "77958185-3f28-4cfd-b6fc-7f1c4b3ddf8c")
                                modo = "processamentonfce";
                            if (mod == "6ef0dbfc-65be-487e-acf2-21cfe818c4dd")
                                modo = "processamentonfce";
                            //Sat
                            if (mod == "4c3a288e-3502-4663-9125-c6e2e9f7acf0")
                                modo = "processamentosatce";
                            if (mod == "3f8bbd33-9ddc-4911-bfd3-d73867a24086")
                                modo = "processamentosatce";

                        }
                        //Minusculo
                        if (parametros.Contains("/idkeysistema="))
                        {
                            //NF-e
                            var mod = parametros.Replace("/idkeysistema=", "");
                            if (mod == "b772a449-3ccb-441d-9bbe-9f83b39207a1")
                                modo = "processamentocce";
                            if (mod == "b54bea0e-6e3c-4cf9-9f22-27a964ea3951")
                                modo = "processamentonfe";
                            if (mod == "ee7ae119-db83-4560-862d-4ff00a2dc40e")
                                modo = "processamentonfe";
                            //NFC-e
                            if (mod == "b6e58ec3-2de3-4041-b817-0625dd014396")
                                modo = "processamentonfce";
                            if (mod == "8bb8966a-0f1a-4dfd-ba4d-74e46a67dc2a")
                                modo = "processamentonfce";
                            if (mod == "77958185-3f28-4cfd-b6fc-7f1c4b3ddf8c")
                                modo = "processamentonfce";
                            if (mod == "6ef0dbfc-65be-487e-acf2-21cfe818c4dd")
                                modo = "processamentonfce";
                            //Sat
                            if (mod == "4c3a288e-3502-4663-9125-c6e2e9f7acf0")
                                modo = "processamentosatce";
                            if (mod == "3f8bbd33-9ddc-4911-bfd3-d73867a24086")
                                modo = "processamentosatce";

                        }

                        if (parametros.Contains("/Param00="))
                        {
                            param00 = parametros.Replace("/Param00=", "");
                        }
                        if (parametros.Contains("/Show"))
                        {
                            tipo = parametros.Replace("/", "");
                        }
                        if (parametros.Contains("/parametros"))
                        {
                            tipo = parametros.Replace("/", "");
                        }
                        if (parametros.Contains("/Autorizar"))
                        {
                            tipo = parametros.Replace("/", "");
                        }
                        if (parametros.Contains("/Cancelar"))
                        {
                            tipo = parametros.Replace("/", "");
                        }
                        if (parametros.Contains("/Inutilizar"))
                        {
                            tipo = parametros.Replace("/", "");
                        }
                        if (parametros.Contains("/Justificativa"))
                        {
                            justificativa = parametros.Replace("/Justificativa=", "");
                        }
                        if (parametros.Contains("configenterprise.exe"))
                        {
                            tipo = "parametros";
                        }
                    }

                    //PARAMETROS
                    if (tipo == "parametros")
                    {
                        TELAPARAMETROS tela = new TELAPARAMETROS();
                        tela.Show();
                        tela.Activate();

                    }

                    //NF-e
                    if (modo == "processamentonfe" && tipo == "Autorizar")
                    {
                        TELAPROCESSAMENTONFE tela = new TELAPROCESSAMENTONFE(param00);
                        tela.Show();
                        tela.Activate();


                        System.Threading.Thread.Sleep(2 * 1000);
                        tela.Autorizar();
                    }
                    if (modo == "processamentonfe" && tipo == "Cancelar")
                    {
                        TELAPROCESSAMENTONFE tela = new TELAPROCESSAMENTONFE(param00);
                        tela.Show();
                        tela.Activate();


                        System.Threading.Thread.Sleep(2 * 1000);
                        tela.Cancelar(justificativa.Replace("_", " ").Replace("  ", "").Trim());
                    }
                    if (modo == "processamentonfe")
                    {
                        if (Application.Current.Windows.OfType<TELAPROCESSAMENTONFE>().Count() > 0)
                        {
                            Application.Current.Windows.OfType<TELAPROCESSAMENTONFE>().First().WindowState = WindowState.Normal;
                            Application.Current.Windows.OfType<TELAPROCESSAMENTONFE>().First().Activate();
                        }
                        else
                        {
                            TELAPROCESSAMENTONFE tela = new TELAPROCESSAMENTONFE();
                            tela.Show();
                            tela.Activate();
                            // 
                        }

                    }
                    if (modo == "processamentocce")
                    {
                        if (Application.Current.Windows.OfType<TELAPROCESSAMENTOCCE>().Count() > 0)
                        {
                            Application.Current.Windows.OfType<TELAPROCESSAMENTOCCE>().First().WindowState = WindowState.Normal;
                            Application.Current.Windows.OfType<TELAPROCESSAMENTOCCE>().First().Activate();
                        }
                        else
                        {
                            TELAPROCESSAMENTOCCE tela = new TELAPROCESSAMENTOCCE();
                            tela.Show();
                            tela.Activate();
                            // 
                        }

                    }

                    //NFC-e
                    if (modo == "processamentonfce")
                    {
                        if (Application.Current.Windows.OfType<TELAPROCESSAMENTONFCE>().Count() > 0)
                        {
                            Application.Current.Windows.OfType<TELAPROCESSAMENTONFCE>().First().WindowState = WindowState.Normal;
                            Application.Current.Windows.OfType<TELAPROCESSAMENTONFCE>().First().Activate();
                        }
                        else
                        {
                            TELAPROCESSAMENTONFCE tela = new TELAPROCESSAMENTONFCE(param00);
                            tela.Show();
                            tela.Activate();
                            // 
                            if (tipo == "Autorizar")
                            {
                                tela.Hide();
                                tela.Autorizar("noshow");
                            }
                            if (tipo == "Show")
                            {
                                // tela.Hide();
                                tela.Autorizar("show");
                            }

                            if (tipo == "Cancelar")
                            {
                                tela.Cancelar(justificativa.Replace("_", " ").Replace("  ", "").Trim());
                            }

                            if (tipo == "Inutilizar")
                            {
                                tela.Inutilizar(justificativa.Replace("_", " ").Replace("  ", "").Trim());
                            }
                        }

                    }

                    //SATC-e
                    if (modo == "processamentosatce")
                    {
                        if (Application.Current.Windows.OfType<TELAPROCESSAMENTOSAT>().Count() > 0)
                        {
                            Application.Current.Windows.OfType<TELAPROCESSAMENTOSAT>().First().WindowState = WindowState.Normal;
                            Application.Current.Windows.OfType<TELAPROCESSAMENTOSAT>().First().Activate();
                        }
                        else
                        {
                            TELAPROCESSAMENTOSAT tela = new TELAPROCESSAMENTOSAT(param00);
                            tela.Show();
                            tela.Activate();
                            // 
                            if (tipo == "Autorizar")
                            {
                                //tela.Hide();
                                tela.Autorizar("show");
                            }
                            if (tipo == "Show")
                            {
                                // tela.Hide();
                                tela.Autorizar("show");
                            }

                            if (tipo == "Cancelar")
                            {
                                tela.Cancelar(justificativa.Replace("_", " ").Replace("  ", "").Trim());
                            }

                            if (tipo == "Inutilizar")
                            {
                                tela.Inutilizar(justificativa.Replace("_", " ").Replace("  ", "").Trim());
                            }
                        }

                    }


                    if (tipo == "" && modo == "")
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
                            //  
                        }
                    }
                    this.Close();
                }
                

            }
            catch (Exception ex)
            {
               Funcoes.Crashe(ex, "ATENÇÃO!!!", true);
            }
        }


        public void VerificarBanco()
        {
            try
            {
                MySQLSITE sqlsite = new MySQLSITE();
                string cnpj = App.Parametro.Cnpj;
                if (cnpj.Replace(" ", "") != "")
                {
                    string validademodulo = sqlsite.GetValidadeModulo(cnpj.Replace(" ", "").Replace("-", "").Replace("/", "").Replace(".", ""));
                    if (validademodulo != "0")
                    {
                        DateTime datavalidade = Convert.ToDateTime(validademodulo);
                        if (datavalidade >= DateTime.Now.Date )
                        {
                            App.Parametro.Update();
                        }
                        else
                        {
                            Funcoes.Mensagem(datavalidade.ToString(), "ENTRE EM CONTATO SUPORTE", MessageBoxButton.OK);
                            this.Close();
                        }
                    }
                }
                sqlsite.Closer();

            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ATENÇÃO!!!", true);
            }
        }

        public void VerificarAtualizacao()
        {
            try
            {
                string numeroversao = Assembly.GetExecutingAssembly().GetName().Version.ToString();

                Hversoes versao = new Hversoes(numeroversao);

                if (versao.Habilitado == "NÃO")
                {

                    versao = new Hversoes(versao.Ultimaversao());
                    if (versao.Filename != "" && versao.Filename != null)
                    {
                        NomeTextBox.Text = "Aguarde... Atualização do módulo necessaria.";
                        naotematualizacao = false;

                        string _path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\dll\";

                        TELADEATUALIZACAO tela = new TELADEATUALIZACAO();
                        tela.Show();
                        tela.DownloadFile(versao.Url, _path + versao.Filename, "DOWNLOAD DE ATUALIZAÇÃO");

                    }
                }
                else
                {
                    if (versao.ExisteAtualizacao())
                    {
                        versao = new Hversoes(versao.Ultimaversao());
                        if (MessageBox.Show("Nova versão disponivel! Atualize já! " +
                        "\n " + versao.Notas +
                       "\n\nDeseja atualizar o seu Módulo Fiscal agora?", "Atualização!!!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            NomeTextBox.Text = "Aguarde... Atualização do módulo necessaria.";
                            naotematualizacao = false;

                            string _path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\dll\";

                            TELADEATUALIZACAO tela = new TELADEATUALIZACAO();
                            tela.Show();
                            tela.DownloadFile(versao.Url, _path + versao.Filename, "DOWNLOAD DE ATUALIZAÇÃO");

                        }


                    }
                }


                if (naotematualizacao)
                {
                    versao.HversoesSistema();
                    if (numeroversao != versao.Vinco)
                    {
                        NomeTextBox.Text = "Aguarde... ajuste do sistema CIAF.";
                        versao.AtualizarHversoesSistema(versao.Vinco);

                        if (MessageBox.Show("O módulo foi atualizado com informações importantes da SEFAZ. " + Environment.NewLine +
                                        " Por gentileza, realize o processamento novamente.", "NOVA VERSÃO " + numeroversao,
                                        MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            this.Close();
                        }
                    }
                }
               



                




            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "", false);
            }

        }

        private void SetTheme()
        {
            Theme theme = ThemeManager.Current.DetectTheme();
            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
            ThemeManager.Current.SyncTheme();

            theme = ThemeManager.Current.AddLibraryTheme(
                   new LibraryTheme(
                new Uri("pack://application:,,,/nfecreator;component/Styles/ThemeCiafAutomotivo.xaml"),
                MahAppsLibraryThemeProvider.DefaultInstance
                )
            );


            theme = ThemeManager.Current.AddLibraryTheme(
                   new LibraryTheme(
                new Uri("pack://application:,,,/nfecreator;component/Styles/ThemePetsystem.xaml"),
                MahAppsLibraryThemeProvider.DefaultInstance
                )
            );


            theme = ThemeManager.Current.AddLibraryTheme(
            new LibraryTheme(
                new Uri("pack://application:,,,/nfecreator;component/Styles/ThemeCiaf.xaml"),

                MahAppsLibraryThemeProvider.DefaultInstance
                )
            );

            DbfBase basee = new DbfBase();
            if (basee.Sistema == "ciafautomotivo") {
                theme = ThemeManager.Current.GetTheme("Light.CiafAutomotivo");
            }
            else if (basee.Sistema == "petsystem")
            {
                theme = ThemeManager.Current.GetTheme("Light.Petsystem");
            }
            else
                theme = ThemeManager.Current.GetTheme("Light.Ciaf");


            ThemeManager.Current.ChangeTheme(App.Current, theme);

        }


    }
}
