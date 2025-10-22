using DFe.Classes.Entidades;
using DFe.Classes.Flags;
using DFe.Utils;
using FontAwesome5;
using FontAwesome5.Extensions;
using MahApps.Metro.Controls;
using MosaicoSolutions.ViaCep;
using MosaicoSolutions.ViaCep.Modelos;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using NFe.Utils;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace nfecreator
{
    /// <summary>
    /// Lógica interna para TELAPARAMETROS.xaml
    /// </summary>
    public partial class TELAPARAMETROS : MetroWindow
    {
        private const string ArquivoConfiguracao = @"\configuracao.xml";
        private ConfiguracaoApp _configuracoes;
        private readonly string _path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private string cnpj;

        public TELAPARAMETROS()
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; }; // não precisa de cadeia de certificado digital

                InitializeComponent();

                Parametros parametros = App.Parametro;
                DataContext = parametros;
                cnpj = parametros.Cnpj;

                PreencheListaImpressoras();
                PreencheListaTipoEmissao();

                ApenasLeituraCampos(true);

                Ativarbotoes(false);

            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ATENÇÃO!!! TELAPARAMETROS", true);
            }
        }

        private void PreencheListaTipoEmissao()
        {
            try
            {
                List<ComboData> ListData = new List<ComboData>
                {
                    new ComboData { Id = 1, Value = "1 - Normal "},
                    new ComboData { Id = 2, Value = "2 - FSIA " },
                    new ComboData { Id = 3, Value = "3 - SCAN " },
                    new ComboData { Id = 4, Value = "4 - EPEC " },
                    new ComboData { Id = 5, Value = "5 - FSDA " },
                    new ComboData { Id = 6, Value = "6 - SVCAN " },
                    new ComboData { Id = 7, Value = "7 - SVCRS " },
                    //new ComboData { Id = 9, Value = "9 - OffLine " },
                };

                CBTipodeemissao.ItemsSource = ListData;
                CBTipodeemissao.DisplayMemberPath = "Value";
                CBTipodeemissao.SelectedValuePath = "Id";

                CBTipodeemissao.SelectedValue = "1";

            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ATENÇÃO!!!");
            }
        }

        private void PreencheListaImpressoras()
        {
            try
            {

                List<ComboData> ListData = new List<ComboData>
                {
                    new ComboData { Id = 0, Value = " " },
                };

                int i = 0;
                foreach (string impressora in PrinterSettings.InstalledPrinters)
                {
                    i++;
                    ComboData impressor = new ComboData { Id = i, Value = impressora };
                    ListData.Add(impressor);

                    //Console.WriteLine(impressora);
                }

                impressoranfceComboBox.ItemsSource = ListData;
                impressoranfceComboBox.DisplayMemberPath = "Value";
                impressoranfceComboBox.SelectedValuePath = "Value";

                impressoranfceComboBox.SelectedValue = " ";

            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "", false);
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Title += " - Versão: " + Assembly.GetExecutingAssembly().GetName().Version.ToString();

                Parametros parametros = App.Parametro;
                DataContext = parametros;

                CarregarConfiguracao();

                PreencherPropedades();
                CarregarValidadeModulo();

                CbxPadrao_Checked(null, null);
                if (TxtDiretorioSchema.Text.Replace(" ", "") == "")
                    TxtDiretorioSchema.Text = _path + @"\Schemas\";

                Hversoes versao = new Hversoes(Assembly.GetExecutingAssembly().GetName().Version.ToString());
                if (versao.ExisteAtualizacao())
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        updateButton.Visibility = Visibility.Visible;
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

        private void CarregarValidadeModulo()
        {
            try
            {
                MySQLSITE sqlsite = new MySQLSITE();
                if (TxtEmitCnpj.Text.Replace(" ", "") != "")
                {
                    string validademodulo = sqlsite.GetValidadeModulo(TxtEmitCnpj.Text.Replace(" ", "").Replace("-", "").Replace("/", "").Replace(".", ""));
                    if (validademodulo != "0")
                    {
                        DateTime datavalidade = Convert.ToDateTime(validademodulo);
                        if (datavalidade >= DateTime.Today)
                        {
                            RbtAmbProducao.Visibility = Visibility.Visible;
                            // BtnZerar.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            //BtnZerar.Visibility = Visibility.Collapsed;
                            RbtAmbProducao.Visibility = Visibility.Collapsed;
                        }
                    }
                }
                sqlsite.Closer();
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


                _configuracoes = !File.Exists(_path + ArquivoConfiguracao)
                    ? new ConfiguracaoApp()
                    : FuncoesXml.ArquivoXmlParaClasse<ConfiguracaoApp>(_path + ArquivoConfiguracao);
                if (_configuracoes.CfgServico.TimeOut == 0)
                    _configuracoes.CfgServico.TimeOut = 13000; //mínimo

                if (!File.Exists(_path + ArquivoConfiguracao))
                {

                    DbfBase ebase = new DbfBase();
                    _configuracoes.ConfiguracaoDanfeNfe.SalvarServidor = ebase.Servidor;
                    CbxSalvarServidor.IsChecked = ebase.Servidor;
                    ebase.Close();
                    BtnEditar_Click(null, null);
                }

                #region Carrega a logo no controle logoEmitente

                if (_configuracoes.ConfiguracaoDanfeNfe.Logomarca != null && _configuracoes.ConfiguracaoDanfeNfe.Logomarca.Length > 0)
                    using (var stream = new MemoryStream(_configuracoes.ConfiguracaoDanfeNfe.Logomarca))
                    {
                        LogoEmitente.Source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    }
                if (_configuracoes.ConfiguracaoDanfeNfce.Logomarca != null && _configuracoes.ConfiguracaoDanfeNfce.Logomarca.Length > 0)
                    using (var stream = new MemoryStream(_configuracoes.ConfiguracaoDanfeNfce.Logomarca))
                    {
                        LogoEmitenteNFCe.Source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    }

                #endregion

                //aqui a mudança do CE
                if (App.Parametro.Uf == "CE")
                    ConfiguracaoUrls.FactoryUrl = Shared.NFe.Utils.Enderecos.NovasUrlsCeara.FactoryUrlCearaMudanca.CriaFactoryUrl();
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ATENÇÃO !!! CARREGAR CONFIGURAÇÃO", true);
            }
        }

        public void PreencherPropedades()
        {
            try
            {
                CarregarConfiguracao();
                Parametros parametro = new Parametros(1);
                DataContext = parametro;

                TxtEmitFantasia.Text = parametro.Nome;
                _configuracoes.Emitente.xFant = parametro.Nome;

                TxtEmitRazao.Text = parametro.Razaosocial;
                _configuracoes.Emitente.xNome = parametro.Razaosocial;

                cnpj = parametro.Cnpj;
                TxtEmitCnpj.Text = parametro.Cnpj;
                _configuracoes.Emitente.CNPJ = parametro.Cnpj;


                TxtEmitIe.Text = parametro.Ie;
                _configuracoes.Emitente.IE = parametro.Ie;


                CBTipodeemissao.SelectedValue = (int)_configuracoes.CfgServico.tpEmis;


                if (iestTextBox.Text == "")
                    iestTextBox.Text = _configuracoes.Emitente.IEST;

                if (cnaefTextBox.Text == "")
                    cnaefTextBox.Text = _configuracoes.Emitente.CNAE;

                if (inscricaomunicipalTextBox.Text == "")
                    inscricaomunicipalTextBox.Text = _configuracoes.Emitente.IM;

                //INFORMACOES NF-e
                TxtDiretorioSchema.Text = _configuracoes.CfgServico.DiretorioSchemas;
                if (TxtDiretorioSchema.Text == "")
                    TxtDiretorioSchema.Text = _path + @"\Schemas\";

                TxtDiretorioXml.Text = _configuracoes.CfgServico.DiretorioSalvarXml;

                CbxSalvarXml.IsChecked = _configuracoes.CfgServico.SalvarXmlServicos;
                CbxSalvarServidor.IsChecked = _configuracoes.ConfiguracaoDanfeNfe.SalvarServidor;

                if (TipoAmbiente.Homologacao == _configuracoes.CfgServico.tpAmb)
                {
                    RbtAmbHomologacao.IsChecked = true;
                    RbtAmbProducao.IsChecked = false;
                }
                else
                {
                    RbtAmbHomologacao.IsChecked = false;
                    RbtAmbProducao.IsChecked = true;
                }



                CmbUfDestino.Text = _configuracoes.CfgServico.cUF.ToString();

                if (_configuracoes.CfgServico.Certificado.TipoCertificado == TipoCertificado.A1Arquivo) CertificadoA1emArquivo.IsChecked = true;
                if (_configuracoes.CfgServico.Certificado.TipoCertificado == TipoCertificado.A3) CertificadoA3.IsChecked = true;
                if (_configuracoes.CfgServico.Certificado.ManterDadosEmCache == true)
                    CbxManterDadosCert.IsChecked = true;



                if (CertificadoA1emArquivo.IsChecked == true)
                {
                    GrupoA1.Visibility = Visibility.Visible;
                    GrupoA3.Visibility = Visibility.Collapsed;
                }
                else if (CertificadoA3.IsChecked == true)
                {
                    GrupoA1.Visibility = Visibility.Collapsed;
                    GrupoA3.Visibility = Visibility.Visible;
                }


                TxtCertificado.Text = _configuracoes.CfgServico.Certificado.Serial;
                TxtArquivoCertificado.Text = _configuracoes.CfgServico.Certificado.Arquivo;
                CmbUfDestino.SelectedValue = _configuracoes.CfgServico.cUF;

                CmbCrt.Text = _configuracoes.Emitente.CRT.CrtParaString();
                
                TxtEmitCep.Text = parametro.Cep;
                TxtEmitLogradouro.Text = parametro.Endereco;
                TxtEmitNumero.Text = parametro.Complemento;
                TxtEmitComplemento.Text = parametro.Comple2;
                TxtEmitBairro.Text = parametro.Bairro;
                TxtEmitCidade.Text = parametro.Cidade;
                CmbEmitUf.Text = parametro.Uf;
                TxtEmitCodCidade.Text = parametro.Codcidade;
                CBCalculatrib.Text = parametro.Calculatrib;

                TxtEmitEmail.Text = parametro.Email;

                TxtEmitFone.Text = parametro.Fone_res;
                TxtEmitCelular.Text = parametro.Fone2;


                TxtEmitResponsavel.Text = parametro.Responsavel;
                //CbDezporcento.Text = parametro.Dezporcento;

                TxtSerie.Text = parametro.Serie.ToString();
                //if (parametro.Ambiente == "HOMOLOGAÇÃO")
                //{
                //    RbtAmbHomologacao.IsChecked = true;
                //    _configuracoes.CfgServico.tpAmb = TipoAmbiente.Homologacao;
                //}
                //else
                //{
                //    RbtAmbProducao.IsChecked = true;
                //    _configuracoes.CfgServico.tpAmb = TipoAmbiente.Producao;
                //}

                impressaoemLoteComboBox.Text = _configuracoes.ConfiguracaoDanfeNfe.ImpressaoLote;


                if (_configuracoes.ConfiguracaoDanfeNfe.Ipidevol)
                    ipidevolComboBox.Text = "SIM";
                else
                    ipidevolComboBox.Text = "NÃO";

                if (_configuracoes.ConfiguracaoDanfeNfe.CodAut)
                    codigoautorizacaoComboBox.Text = "SIM";
                else
                    codigoautorizacaoComboBox.Text = "NÃO";

                if (_configuracoes.ConfiguracaoDanfeNfe.InfAdProd)
                    infadprodComboBox.Text = "SIM";
                else
                    infadprodComboBox.Text = "NÃO";

                if (_configuracoes.ConfiguracaoDanfeNfe.Vicmsdeson)
                    vicmsdesonComboBox.Text = "SIM";
                else
                    vicmsdesonComboBox.Text = "NÃO";

                //NFC-e
                CidtokenTextBox.Text = _configuracoes.ConfiguracaoCsc.CIdToken;
                CscTextBox.Text = _configuracoes.ConfiguracaoCsc.Csc;

                EsquerdaTextBox.Text = _configuracoes.ConfiguracaoDanfeNfce.MargemEsquerda.ToString();
                DireitaTextBox.Text = _configuracoes.ConfiguracaoDanfeNfce.MargemDireita.ToString();

                diretonfceComboBox.Text = _configuracoes.ConfiguracaoDanfeNfce.Direto;
                impressoranfceComboBox.SelectedValue = _configuracoes.ConfiguracaoDanfeNfce.Impressora;


                TxtSerienfce.Text = parametro.Serienfce.ToString();

                _configuracoes.CfgServico.ModeloDocumento = ModeloDocumento.NFe;

                _configuracoes.CfgServico.VersaoLayout = VersaoServico.Versao400;

                _configuracoes.CfgServico.DefineVersaoServicosAutomaticamente = false;

                _configuracoes.CfgServico.SalvarXmlServicos = false;

                _configuracoes.CfgServico.DefineVersaoServicosAutomaticamente = true;


                _configuracoes.CfgServico.VersaoNfceAministracaoCSC = VersaoServico.Versao400;
                _configuracoes.CfgServico.VersaoNfeDownloadNF = VersaoServico.Versao310;
                _configuracoes.CfgServico.VersaoNfeConsultaDest = VersaoServico.Versao400;
                //_configuracoes.CfgServico.VersaoNfeConsultaCadastro = VersaoServico.ve400;
                _configuracoes.CfgServico.VersaoNFeDistribuicaoDFe = VersaoServico.Versao100;

                _configuracoes.CfgServico.VersaoNFeRetAutorizacao = VersaoServico.Versao400;
                _configuracoes.CfgServico.VersaoNFeAutorizacao = VersaoServico.Versao400;
                _configuracoes.CfgServico.VersaoNfeStatusServico = VersaoServico.Versao400;
                _configuracoes.CfgServico.VersaoNfeConsultaProtocolo = VersaoServico.Versao400;
                _configuracoes.CfgServico.VersaoNfeInutilizacao = VersaoServico.Versao400;
                _configuracoes.CfgServico.VersaoNfeRetRecepcao = VersaoServico.Versao400;
                _configuracoes.CfgServico.VersaoNfeRecepcao = VersaoServico.Versao400;

                _configuracoes.CfgServico.VersaoRecepcaoEventoManifestacaoDestinatario = VersaoServico.Versao400;

                _configuracoes.CfgServico.VersaoRecepcaoEventoEpec = VersaoServico.Versao400;
                _configuracoes.CfgServico.VersaoRecepcaoEventoCceCancelamento = VersaoServico.Versao400;

                _configuracoes.ConfiguracaoDanfeNfce.VersaoQrCode = VersaoQrCode.QrCodeVersao2;
                _configuracoes.ConfiguracaoDanfeNfce.NfceLayoutQrCode = NfceLayoutQrCode.Abaixo;
                _configuracoes.ConfiguracaoDanfeNfce.DetalheVendaNormal = NfceDetalheVendaNormal.DuasLinhas;
                _configuracoes.ConfiguracaoDanfeNfce.DetalheVendaContigencia = NfceDetalheVendaContigencia.DuasLinhas;

                _configuracoes.ConfiguracaoDanfeNfce.ImprimeDescontoItem = true;

                //if (Tls12.IsChecked == true)
                //    _configuracoes.CfgServico.ProtocoloDeSeguranca = SecurityProtocolType.Tls12;
                //if (Tls11.IsChecked == true)
                //    _configuracoes.CfgServico.ProtocoloDeSeguranca = SecurityProtocolType.Tls11;
                //if (Tls.IsChecked == true)
                //    _configuracoes.CfgServico.ProtocoloDeSeguranca = SecurityProtocolType.Tls;
                //if (ssl3.IsChecked == true)
                //_configuracoes.CfgServico.ProtocoloDeSeguranca = SecurityProtocolType.Ssl3;

                if (_configuracoes.CfgServico.ProtocoloDeSeguranca == SecurityProtocolType.Tls12)
                    Tls12.IsChecked = true;

                if (_configuracoes.CfgServico.ProtocoloDeSeguranca == SecurityProtocolType.Tls11)
                    Tls11.IsChecked = true;

                //if (_configuracoes.CfgServico.ProtocoloDeSeguranca == SecurityProtocolType.Tls)
                //    Tls.IsChecked = true;

                //if (_configuracoes.CfgServico.ProtocoloDeSeguranca == SecurityProtocolType.Ssl3)
                //    Ssl3.IsChecked = true;

                TimeOutTextBox.Text = _configuracoes.CfgServico.TimeOut.ToString();

                //DataContext = _configuracoes;

                //Email


                if ("smtp.dominio.com" == _configuracoes.ConfiguracaoEmail.ServidorSmtp)
                {
                    ConfEmailComboBox.Text = "CIAF";
                    App.Parametro.Confemail = "CIAF";
                    parametro.Confemail = "CIAF";
                }
                else
                {
                    ConfEmailComboBox.Text = "EMPRESA";
                    App.Parametro.Confemail = "EMPRESA";
                    parametro.Confemail = "EMPRESA";
                }


                EnvioAutComboBox.Text = _configuracoes.ConfiguracaoDanfeNfe.EmailAut;
                EmailCopia.Text = _configuracoes.ConfiguracaoDanfeNfe.EmailCopia;


                TxtEmitEmail.Text = _configuracoes.ConfiguracaoEmail.Email;

                ServidorSmtp.Text = _configuracoes.ConfiguracaoEmail.ServidorSmtp;
                TxtPorta.Text = _configuracoes.ConfiguracaoEmail.Porta.ToString();
                UsuarioEmail.Text = _configuracoes.ConfiguracaoEmail.Email;

                if (_configuracoes.ConfiguracaoEmail.Ssl) SSLCb.IsChecked = true;
                else SSLCb.IsChecked = false;

                if (_configuracoes.ConfiguracaoEmail.Assincrono) AssincronoCb.IsChecked = true;
                else _configuracoes.ConfiguracaoEmail.Assincrono = false;

                if (_configuracoes.ConfiguracaoEmail.MensagemEmHtml) MsgHTML.IsChecked = true;
                else MsgHTML.IsChecked = false;

                AssuntoEmail.Text = _configuracoes.ConfiguracaoEmail.Assunto;
                MensagemEmail.Text = _configuracoes.ConfiguracaoEmail.Mensagem;
                TimeoutEmail.Text = _configuracoes.ConfiguracaoEmail.Timeout.ToString();

                SenhaEmail.Password = _configuracoes.ConfiguracaoEmail.Senha;
                TxtSenhaCertificado.Password = _configuracoes.CfgServico.Certificado.Senha;


                //Contabilidade
                cnpjcontabilidadeTextBox.Text = parametro.Cnpjcontabilidade;
                Funcoes.RetornarMascaraCpfCnpj(cnpjcontabilidadeTextBox);
                TxtCertificado.Text = _configuracoes.CfgServico.Certificado.Serial;

            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ATENÇÃO!!!");
            }

        }

        private void BtnCertificado_Click(object sender, RoutedEventArgs e)
        {
            CarregaDadosCertificado();
        }

        private void CarregaDadosCertificado()
        {
            try
            {
                var cert = CertificadoDigitalUtils.ListareObterDoRepositorio();
                _configuracoes.CfgServico.Certificado.Serial = cert.SerialNumber;
                TxtCertificado.Text = _configuracoes.CfgServico.Certificado.Serial;
                TxtValidade.Text = " - Validade: " + cert.GetExpirationDateString();
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ATENÇÃO!!!");
            }
        }

        private void BtnArquivoCertificado_Click(object sender, RoutedEventArgs e)
        {

            if (_configuracoes.CfgServico.Certificado.TipoCertificado == TipoCertificado.A1ByteArray)
            {
                var caminhoArquivo = Funcoes.BuscarArquivoCertificado();
                if (!string.IsNullOrWhiteSpace(caminhoArquivo))
                {
                    _configuracoes.CfgServico.Certificado.ArrayBytesArquivo = File.ReadAllBytes(caminhoArquivo);
                    TxtCertificado.Text = _configuracoes.CfgServico.Certificado.Serial;
                    _configuracoes.CfgServico.Certificado.Arquivo = null;

                }
                TxtArquivoCertificado.Text = caminhoArquivo;
            }
            else if (_configuracoes.CfgServico.Certificado.TipoCertificado == TipoCertificado.A1Arquivo)
            {
                _configuracoes.CfgServico.Certificado.Arquivo = Funcoes.BuscarArquivoCertificado();
                TxtCertificado.Text = _configuracoes.CfgServico.Certificado.Serial;
                TxtArquivoCertificado.Text = _configuracoes.CfgServico.Certificado.Arquivo;
            }

        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            SalvarConfiguracao();
            Dispatcher.Invoke(new Action(() => { TaskbarItemInfo.Overlay = null; }), DispatcherPriority.ContextIdle, null);
        }

        private Parametros CriarParametro()
        {
            try
            {
                Parametros parametro = (Parametros)DataContext;
                parametro.Nome = TxtEmitFantasia.Text;

                parametro.Razaosocial = TxtEmitRazao.Text;
                parametro.Tipoe = " ";

                parametro.Endereco = TxtEmitLogradouro.Text;

                if (string.IsNullOrEmpty(TxtEmitNumero.Text.Trim()))
                    throw new ArgumentException("Numero do endereço está vazio.");

                parametro.Complemento = TxtEmitNumero.Text;
                parametro.Comple2 = TxtEmitComplemento.Text;
                parametro.Bairro = TxtEmitBairro.Text;
                parametro.Cidade = TxtEmitCidade.Text;
                parametro.Codcidade = TxtEmitCodCidade.Text;
                parametro.Uf = CmbEmitUf.Text;
                //Coduf = TbCoduf.Text;
                parametro.Pais = "BRASIL";
                //Codpais = TbCodpais.Text;
                parametro.Cep = TxtEmitCep.Text;

                parametro.Fone_res = TxtEmitFone.Text;
                parametro.Fone2 = TxtEmitCelular.Text;


                parametro.Ie = TxtEmitIe.Text;

                parametro.Crt = CmbCrt.Text;
                // Simples = CmbSimples.Text;

                parametro.Calculatrib = CBCalculatrib.Text;

                parametro.Email = TxtEmitEmail.Text;
                parametro.Responsavel = TxtEmitResponsavel.Text;
                //Dezporcento = CbDezporcento.Text;


                if (TxtSerie.Text != "")
                    parametro.Serie = Convert.ToInt32(TxtSerie.Text);

                if (cnpj == "99999999000191")
                {
                    parametro.Cnpj = TxtEmitCnpj.Text;
                    cnpj = parametro.Cnpj;
                }
                else
                {
                    if (cnpj != TxtEmitCnpj.Text)
                    {
                        MessageBox.Show("Para alteração de CNPJ favor entrar em contato com SUPORTE! www.ciaf.com.br ");
                        TxtEmitCnpj.Text = cnpj;
                    }
                }


                if (RbtAmbHomologacao.IsChecked == true)
                {
                    parametro.Ambiente = "HOMOLOGAÇÃO";
                    _configuracoes.CfgServico.tpAmb = TipoAmbiente.Homologacao;
                }
                else
                {
                    parametro.Ambiente = "PRODUÇÃO";
                    _configuracoes.CfgServico.tpAmb = TipoAmbiente.Producao;
                }

                if (pCredSNTextBox.Text != "")
                    parametro.PCredsn = Convert.ToDecimal(pCredSNTextBox.Text);
                else
                    parametro.PCredsn = 0;

                //NFC-e
                parametro.Cidtoken = CidtokenTextBox.Text;
                parametro.Csc = CscTextBox.Text;

                _configuracoes.ConfiguracaoDanfeNfce.Direto = diretonfceComboBox.Text;
                _configuracoes.ConfiguracaoDanfeNfce.Impressora = impressoranfceComboBox.Text;


                if (TxtSerienfce.Text != "")
                    parametro.Serienfce = Convert.ToInt32(TxtSerienfce.Text);

                //Certificado
                parametro.Cnpjcontabilidade = cnpjcontabilidadeTextBox.Text.Replace(" ", "").Replace(".", "").Replace("-", "").Replace(@"\", "").Replace(@"/", "");
                // parametro.Stonecode = stonecodeTextBox.Text.Replace(" ", "").Replace(".", "").Replace("-", "").Replace(@"\", "").Replace(@"/", "");
                return parametro;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Numero do endereço está vazio."))
                    Funcoes.Mensagem("Numero do endereço está vazio.", "AVISO - PARAMETROS", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                //else if (ex.Message.Contains("Digite apenas um e-mail no cadastro deste cliente."))
                //    MinhaNotificacao.NotificarErro("Erro no Envio de EMAIL", ex.Message);
                else
                    Funcoes.Crashe(ex, "ERRO DA E-MAIL", false);
                return null;
            }


        }

        private void SalvarConfiguracao()
        {
            try
            {
                Parametros parametro = CriarParametro();
                if (parametro == null)
                    throw new ArgumentException("Parametro null");
                App.Parametro = parametro;
                //parametro.AtualizarParametro();

                _configuracoes.Emitente.CNPJ = Funcoes.Deixarnumero(TxtEmitCnpj.Text);
                _configuracoes.Emitente.IE = Funcoes.Deixarnumero(TxtEmitIe.Text);
                _configuracoes.Emitente.xNome = TxtEmitRazao.Text;
                _configuracoes.Emitente.xFant = TxtEmitFantasia.Text;
                _configuracoes.EnderecoEmitente.CEP = TxtEmitCep.Text;
                _configuracoes.EnderecoEmitente.xLgr = TxtEmitLogradouro.Text;
                _configuracoes.EnderecoEmitente.nro = TxtEmitNumero.Text;
                _configuracoes.EnderecoEmitente.xCpl = TxtEmitComplemento.Text;
                _configuracoes.EnderecoEmitente.xBairro = TxtEmitBairro.Text;
                _configuracoes.EnderecoEmitente.UF = Funcoes.EstadoString(CmbEmitUf.Text);
                _configuracoes.EnderecoEmitente.xMun = TxtEmitCidade.Text;
                _configuracoes.EnderecoEmitente.cPais = 1058;
                _configuracoes.EnderecoEmitente.xPais = "BRASIL";

                if (Funcoes.Deixarnumero(TxtEmitCodCidade.Text) != "")
                    _configuracoes.EnderecoEmitente.cMun = Convert.ToInt64(Funcoes.Deixarnumero(TxtEmitCodCidade.Text));

                if (TxtEmitFone.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "") != "")
                    if (Funcoes.Deixarnumero(TxtEmitFone.Text) != "")
                        _configuracoes.EnderecoEmitente.fone = Convert.ToInt64(Funcoes.Deixarnumero(TxtEmitFone.Text));

                _configuracoes.Emitente.IEST = iestTextBox.Text;
                _configuracoes.Emitente.IM = inscricaomunicipalTextBox.Text;
                _configuracoes.Emitente.CNAE = cnaefTextBox.Text;

                _configuracoes.CfgServico.DiretorioSchemas = TxtDiretorioSchema.Text;
                if (CbxSalvarXml.IsChecked == true)
                    _configuracoes.CfgServico.SalvarXmlServicos = true;
                else
                    _configuracoes.CfgServico.SalvarXmlServicos = false;


                if (CbxSalvarServidor.IsChecked == true)
                    _configuracoes.ConfiguracaoDanfeNfe.SalvarServidor = true;
                else
                    _configuracoes.ConfiguracaoDanfeNfe.SalvarServidor = false;

                _configuracoes.CfgServico.DiretorioSalvarXml = TxtDiretorioXml.Text;

                parametro.Crt = CmbCrt.Text;
                if (parametro.Crt == "Simples Nacional")
                    _configuracoes.Emitente.CRT = CRT.SimplesNacional;
                else if (parametro.Crt == "Simples Nacional - Excesso de sublimite de Receita Bruto")
                    _configuracoes.Emitente.CRT = CRT.SimplesNacionalExcessoSublimite;
                else if (parametro.Crt == "Regime Normal")
                    _configuracoes.Emitente.CRT = CRT.RegimeNormal;
                else if (App.Parametro.Crt == "Simples Nacional MEI")
                    this._configuracoes.Emitente.CRT = CRT.SimplesNacionalMei;


                Estado ufEmpresa = Estado.MG;
                switch (CmbUfDestino.Text)
                {
                    case "AC":
                        ufEmpresa = Estado.AC;
                        break;
                    case "AL":
                        ufEmpresa = Estado.AL;
                        break;
                    case "AM":
                        ufEmpresa = Estado.AM;
                        break;
                    case "AN":
                        ufEmpresa = Estado.AN;
                        break;
                    case "AP":
                        ufEmpresa = Estado.AP;
                        break;
                    case "BA":
                        ufEmpresa = Estado.BA;
                        break;
                    case "CE":
                        ufEmpresa = Estado.CE;
                        break;
                    case "DF":
                        ufEmpresa = Estado.DF;
                        break;
                    case "ES":
                        ufEmpresa = Estado.ES;
                        break;
                    case "EX":
                        ufEmpresa = Estado.EX;
                        break;
                    case "GO":
                        ufEmpresa = Estado.GO;
                        break;
                    case "MA":
                        ufEmpresa = Estado.MA;
                        break;
                    case "MG":
                        ufEmpresa = Estado.MG;
                        break;
                    case "MS":
                        ufEmpresa = Estado.MS;
                        break;
                    case "MT":
                        ufEmpresa = Estado.MT;
                        break;
                    case "PA":
                        ufEmpresa = Estado.PA;
                        break;
                    case "PB":
                        ufEmpresa = Estado.PB;
                        break;
                    case "PE":
                        ufEmpresa = Estado.PE;
                        break;
                    case "PI":
                        ufEmpresa = Estado.PI;
                        break;
                    case "PR":
                        ufEmpresa = Estado.PR;
                        break;
                    case "RJ":
                        ufEmpresa = Estado.RJ;
                        break;
                    case "RN":
                        ufEmpresa = Estado.RN;
                        break;
                    case "RO":
                        ufEmpresa = Estado.RO;
                        break;
                    case "RR":
                        ufEmpresa = Estado.RR;
                        break;
                    case "RS":
                        ufEmpresa = Estado.RS;
                        break;
                    case "SC":
                        ufEmpresa = Estado.SC;
                        break;
                    case "SE":
                        ufEmpresa = Estado.SE;
                        break;
                    case "SP":
                        ufEmpresa = Estado.SP;
                        break;
                    case "TO":
                        ufEmpresa = Estado.TO;
                        break;
                    default:
                        ufEmpresa = Estado.PE;
                        break;
                }
                _configuracoes.CfgServico.cUF = ufEmpresa;
                if (CertificadoA1emArquivo.IsChecked == true) _configuracoes.CfgServico.Certificado.TipoCertificado = TipoCertificado.A1Arquivo;
                if (CertificadoA3.IsChecked == true) _configuracoes.CfgServico.Certificado.TipoCertificado = TipoCertificado.A3;
                _configuracoes.CfgServico.Certificado.Serial = TxtCertificado.Text;
                _configuracoes.CfgServico.Certificado.Arquivo = TxtArquivoCertificado.Text;
                if (CbxManterDadosCert.IsChecked == true)
                {
                    _configuracoes.CfgServico.Certificado.ManterDadosEmCache = true;
                    _configuracoes.CfgServico.Certificado.CacheId = "1";
                    // ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                }
                else _configuracoes.CfgServico.Certificado.ManterDadosEmCache = false;

                _configuracoes.ConfiguracaoDanfeNfe.ImpressaoLote = impressaoemLoteComboBox.Text;


                if (ipidevolComboBox.Text == "SIM")
                    _configuracoes.ConfiguracaoDanfeNfe.Ipidevol = true;
                else
                    _configuracoes.ConfiguracaoDanfeNfe.Ipidevol = false;

                if (codigoautorizacaoComboBox.Text == "SIM")
                    _configuracoes.ConfiguracaoDanfeNfe.CodAut = true;
                else
                    _configuracoes.ConfiguracaoDanfeNfe.CodAut = false;


                if (infadprodComboBox.Text == "SIM")
                    _configuracoes.ConfiguracaoDanfeNfe.InfAdProd = true;
                else
                    _configuracoes.ConfiguracaoDanfeNfe.InfAdProd = false;

                if (vicmsdesonComboBox.Text == "SIM")
                    _configuracoes.ConfiguracaoDanfeNfe.Vicmsdeson = true;
                else
                    _configuracoes.ConfiguracaoDanfeNfe.Vicmsdeson = false;



                //NFCe
                _configuracoes.ConfiguracaoCsc.CIdToken = CidtokenTextBox.Text;
                _configuracoes.ConfiguracaoCsc.Csc = CscTextBox.Text;

                if (Funcoes.Deixarnumero(EsquerdaTextBox.Text) != "")
                    _configuracoes.ConfiguracaoDanfeNfce.MargemEsquerda = Convert.ToSingle(EsquerdaTextBox.Text);
                if (Funcoes.Deixarnumero(DireitaTextBox.Text) != "")
                    _configuracoes.ConfiguracaoDanfeNfce.MargemDireita = Convert.ToSingle(DireitaTextBox.Text);

                //CERTIFICADO
                if (Tls12.IsChecked == true)
                    _configuracoes.CfgServico.ProtocoloDeSeguranca = SecurityProtocolType.Tls12;
                if (Tls11.IsChecked == true)
                    _configuracoes.CfgServico.ProtocoloDeSeguranca = SecurityProtocolType.SystemDefault;
                //if (Tls.IsChecked == true)
                //    _configuracoes.CfgServico.ProtocoloDeSeguranca = SecurityProtocolType.Tls;
                //if (Ssl3.IsChecked == true)
                //    _configuracoes.CfgServico.ProtocoloDeSeguranca = SecurityProtocolType.Ssl3;

                _configuracoes.CfgServico.tpEmis = (TipoEmissao)((int)CBTipodeemissao.SelectedValue);

                _configuracoes.CfgServico.TimeOut = Convert.ToInt32(TimeOutTextBox.Text);
                 
                //Email
                _configuracoes.ConfiguracaoDanfeNfe.EmailCopia = EmailCopia.Text;
                _configuracoes.ConfiguracaoDanfeNfe.EmailAut = EnvioAutComboBox.Text;
                _configuracoes.ConfiguracaoEmail.ServidorSmtp = ServidorSmtp.Text;
                if (Funcoes.Deixarnumero(TxtPorta.Text) != "")
                    _configuracoes.ConfiguracaoEmail.Porta = Convert.ToInt32(Funcoes.Deixarnumero(TxtPorta.Text));

                UsuarioEmail.Text = TxtEmitEmail.Text;
                _configuracoes.ConfiguracaoEmail.Email = UsuarioEmail.Text;

                if (SSLCb.IsChecked == true) _configuracoes.ConfiguracaoEmail.Ssl = true;
                else _configuracoes.ConfiguracaoEmail.Ssl = false;

                if (AssincronoCb.IsChecked == true) _configuracoes.ConfiguracaoEmail.Assincrono = true;
                else _configuracoes.ConfiguracaoEmail.Assincrono = false;

                if (MsgHTML.IsChecked == true) _configuracoes.ConfiguracaoEmail.MensagemEmHtml = true;
                else _configuracoes.ConfiguracaoEmail.MensagemEmHtml = false;

                _configuracoes.ConfiguracaoEmail.Assunto = AssuntoEmail.Text;
                _configuracoes.ConfiguracaoEmail.Mensagem = MensagemEmail.Text;

                if (Funcoes.Deixarnumero(TimeoutEmail.Text) != "")
                    _configuracoes.ConfiguracaoEmail.Timeout = Convert.ToInt32(Funcoes.Deixarnumero(TimeoutEmail.Text));


                //DbfBase ebase = new DbfBase();
                //ebase.UpdateBairro(parametro.Email);

                _configuracoes.ConfiguracaoEmail.Senha = SenhaEmail.Password;
                _configuracoes.CfgServico.Certificado.Senha = TxtSenhaCertificado.Password;
                // _configuracoes.ConfiguracaoDanfeNfe.Logomarca = 
                _configuracoes.SalvarParaAqruivo(_path + ArquivoConfiguracao);
                Ativarbotoes(false);
                ApenasLeituraCampos(true);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Parametro null"))
                    Console.WriteLine("Parametro null");
                // Funcoes.Crashe(ex, "ATENÇÃO!!! SalvarConfiguracao");
                else
                    Funcoes.Crashe(ex, "ATENÇÃO!!! SalvarConfiguracao");
            }
        }

        private void BtnDiretorioSchema_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dlg = new System.Windows.Forms.FolderBrowserDialog();
                dlg.ShowDialog();
                TxtDiretorioSchema.Text = dlg.SelectedPath;
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                    Funcoes.Mensagem(ex.Message, "Atenção", MessageBoxButton.OK);
            }
        }

        private void BtnDesfazer_Click(object sender, RoutedEventArgs e)
        {
            Ativarbotoes(false);
            ApenasLeituraCampos(true);
            PreencherPropedades();
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            Ativarbotoes(true);
            ApenasLeituraCampos(false);
        }

        private void ApenasLeituraCampos(Boolean flag)
        {
            if (cnpj == "99999999000191")
            {
                TxtEmitCnpj.IsReadOnly = flag;
                TxtEmitFantasia.IsReadOnly = flag;
            }
            else
            {
                TxtEmitCnpj.IsReadOnly = true;
                TxtEmitFantasia.IsReadOnly = true;
            }

            TxtEmitRazao.IsReadOnly = true;

            TxtEmitIe.IsReadOnly = true;

            iestTextBox.IsReadOnly = true;//flag;
            cnaefTextBox.IsReadOnly = true;// flag;
            inscricaomunicipalTextBox.IsReadOnly = true;// flag;

            CmbCrt.IsEnabled = !flag;
            CmbCrt.IsReadOnly = flag;

            TxtEmitCep.IsReadOnly = true; // flag;
            TxtEmitLogradouro.IsReadOnly = true;
            TxtEmitNumero.IsReadOnly = true;
            TxtEmitComplemento.IsReadOnly = true;
            TxtEmitBairro.IsReadOnly = true;
            TxtEmitCidade.IsReadOnly = true;
            CmbEmitUf.IsReadOnly = true; // flag;
            CmbEmitUf.IsEnabled = false; // !flag;
            TxtEmitCodCidade.IsReadOnly = true; // flag;


            TxtEmitEmail.IsReadOnly = flag;
            TxtEmitResponsavel.IsReadOnly = true;// flag;


            TxtEmitFone.IsReadOnly = true;  //  = flag;
            TxtEmitCelular.IsReadOnly = flag;

            TxtEmitCelular.Visibility = Visibility.Collapsed;
            CelularTextBlock.Visibility = Visibility.Collapsed;


            //TbFone2.Text = parametro.Fone2;

            //Informações fiscais
            GpbAmbiente.IsEnabled = !flag;

            TxtSerie.IsReadOnly = true;// flag;

            SequenciaNFeTextBox.IsReadOnly = flag;
            SequenciaTextBlock.Visibility = Visibility.Hidden;
            SequenciaNFeTextBox.Visibility = Visibility.Hidden;


            CBCalculatrib.IsEnabled = false;// !flag;
            CmbUfDestino.IsEnabled = !flag;
            CBTipodeemissao.IsEnabled = !flag;

            pCredSNTextBox.IsReadOnly = true; // flag;
            //CmbSimples.IsEnabled = !flag;

            TxtDiretorioSchema.IsReadOnly = flag;
            BtnDiretorioSchema.IsEnabled = !flag;
            CbxSalvarXml.IsEnabled = !flag;
            TxtDiretorioXml.IsReadOnly = flag;
            BtnDiretorioXml.IsEnabled = !flag;
            CbxSalvarServidor.IsEnabled = !flag;
            impressaoemLoteComboBox.IsEnabled = !flag;
            ipidevolComboBox.IsEnabled = !flag;
            codigoautorizacaoComboBox.IsEnabled = !flag;
            infadprodComboBox.IsEnabled = !flag;
            vicmsdesonComboBox.IsEnabled = !flag;

            //NFC-e
            CidtokenTextBox.IsReadOnly = flag;
            CscTextBox.IsReadOnly = flag;
            TxtSerienfce.IsReadOnly = flag;
            SequenciaNFCeTextBox.IsReadOnly = flag;
            EsquerdaTextBox.IsReadOnly = flag;
            DireitaTextBox.IsReadOnly = flag;
            diretonfceComboBox.IsEnabled = !flag;
            impressoranfceComboBox.IsEnabled = !flag;

            BtnLogoNFCe.IsEnabled = !flag;
            BtnRemoveLogoNFCe.IsEnabled = !flag;

            //Certificado
            CertificadoGroupBox.IsEnabled = !flag;
            TxtCertificado.IsReadOnly = flag;
            BtnCertificado.IsEnabled = !flag;
            TxtArquivoCertificado.IsReadOnly = flag;
            BtnArquivoCertificado.IsEnabled = !flag;
            TxtSenhaCertificado.IsEnabled = !flag;
            CbxManterDadosCert.IsEnabled = !flag;

            cnpjcontabilidadeTextBox.IsReadOnly = true; // flag;
            //stonecodeTextBox.IsReadOnly = flag;
            cnpjcredenciadoraTextBox.IsReadOnly = true; // flag;

            Tls12.IsEnabled = !flag;
            Tls11.IsEnabled = !flag;
            // Tls.IsEnabled = !flag;
            // Ssl3.IsEnabled = !flag;

            TimeOutTextBox.IsReadOnly = flag;

            //E-mail
            ConfEmailComboBox.IsEnabled = !flag;
            EnvioAutComboBox.IsEnabled = !flag;
            EmailCopia.IsReadOnly = flag;

            ServidorSmtp.IsReadOnly = flag;
            TxtPorta.IsReadOnly = flag;
            UsuarioEmail.IsReadOnly = true;
            SenhaEmail.IsEnabled = !flag;
            AssuntoEmail.IsReadOnly = flag;
            MensagemEmail.IsReadOnly = flag;
            TimeoutEmail.IsReadOnly = flag;

            SSLCb.IsEnabled = !flag;

            BtnLogo.IsEnabled = !flag;
            BtnRemoveLogo.IsEnabled = !flag;
            //CbxPadrao.IsEnabled = !flag;


            //Parametros
            //beta
            //betaComboBox.IsEnabled = false;
            //betaComboBox.IsReadOnly = true;
            ////financeiro
            //financeiroComboBox.IsEnabled = !flag;
            //financeiroComboBox.IsEnabled = false;

            ////Estoque 
            //estoqueComboBox.IsEnabled = false;

        }

        private void Ativarbotoes(Boolean flag)
        {
            BtnEditar.IsEnabled = !flag;
            BtnGravar.IsEnabled = flag;
            BtnDesfazer.IsEnabled = flag;

            BtnCerXml.IsEnabled = flag;

            BtnTTeste.IsEnabled = flag;
            BtnTTeste.IsEnabled = !flag;
            //BtnZerarNFCe.IsEnabled = flag;
        }

        private void BtnLogoNFe_Click(object sender, RoutedEventArgs e)
        {
            var arquivo = Funcoes.BuscarImagem();
            if (string.IsNullOrEmpty(arquivo)) return;
            var imagem = System.Drawing.Image.FromFile(arquivo);
            LogoEmitente.Source = new BitmapImage(new Uri(arquivo));


            // _configuracoes.ConfiguracaoDanfeNfe.Logomarca = new byte[0];
            using (var stream = new MemoryStream())
            {
                imagem.Save(stream, ImageFormat.Png);
                stream.Close();
                _configuracoes.ConfiguracaoDanfeNfe.Logomarca = stream.ToArray();
            }
        }

        private void BtnLogoNFCe_Click(object sender, RoutedEventArgs e)
        {
            var arquivo = Funcoes.BuscarImagem();
            if (string.IsNullOrEmpty(arquivo)) return;
            var imagem = System.Drawing.Image.FromFile(arquivo);
            LogoEmitenteNFCe.Source = new BitmapImage(new Uri(arquivo));


            //_configuracoes.ConfiguracaoDanfeNfe.Logomarca = new byte[0];
            using (var stream = new MemoryStream())
            {
                imagem.Save(stream, ImageFormat.Png);
                stream.Close();
                _configuracoes.ConfiguracaoDanfeNfce.Logomarca = stream.ToArray();
            }
        }

        private void BtnRemoveLogoNFe_Click(object sender, RoutedEventArgs e)
        {
            LogoEmitente.Source = null;
            _configuracoes.ConfiguracaoDanfeNfe.Logomarca = null;
        }

        private void BtnRemoveLogoNFCe_Click(object sender, RoutedEventArgs e)
        {
            LogoEmitente.Source = null;
            _configuracoes.ConfiguracaoDanfeNfe.Logomarca = null;
        }

        private void TxtEmitCep_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (e.Key == Key.Enter || e.Key == Key.Tab)
                BuscarCEP(txt.Text);
            _ = e.OriginalSource as UIElement;
            if (e.Key == Key.Enter)
            {
                Funcoes.KeyDown(e, this);
            }
        }

        private void BuscarCEP(string CEP)
        {
            try
            {
                Cep cep = CEP.Replace("/", "").Replace(".", "").Replace("-", "").Replace(" ", "");
                var viaCepService = ViaCepService.Default();
                var endereco = viaCepService.ObterEndereco(cep); // viaCep.ObterEndereco("01001000");

                //TbNumero.Text = endereco.Complemento;

                TxtEmitBairro.Text = endereco.Bairro.ToUpper();
                TxtEmitLogradouro.Text = endereco.Logradouro.ToUpper();

                TxtEmitCidade.Text = endereco.Localidade.ToUpper();
                CmbEmitUf.Text = endereco.UF;
                //TxPais.Text = "BRASIL";

                TxtEmitCodCidade.Text = endereco.IBGE;

                CmbUfDestino.SelectedItem = endereco.UF;
                //TxtEmitCodCidade.SelectedText = endereco.IBGE.Substring(0, 2);
                //codpaisTextBox.Text = "1058";

                TxtEmitNumero.Focus();
                TxtEmitNumero.SelectAll();

                //complementoTextBox.Select();
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ATENÇÃO!!!");

            }
        }

        private void BtnCerXml_Click(object sender, RoutedEventArgs e)
        {
            if (BtnGravar.IsEnabled)
                BuscarCEP(TxtEmitCep.Text);
        }

        private void CbxPadrao_Checked(object sender, RoutedEventArgs e)
        {
            //bool myBool = CbxPadrao.IsChecked ?? false;
            //if (myBool)
            //{
            //    //Falta mais
            //    TxtDiretorioSchema.Text = _path + @"\Schemas\";
            //    //TxtDiretorioXml.Text = _path + @"\NFE\";
            //    this.UpdateLayout();
            //}
            //else
            //{
            //    TxtDiretorioSchema.Text = @"";
            //    TxtDiretorioXml.Text = @"";
            //}
        }

        private void BtnStatusNFe_Click(object sender, RoutedEventArgs e)
        {


            if (BtnEditar.IsEnabled)
            {
                Dispatcher.Invoke(new Action(() => { TaskbarItemInfo.Overlay = null; }), DispatcherPriority.ContextIdle, null);
                Thread thread = new Thread(UpdateStatusNFe);
                thread.Start();
            }

        }

        private void UpdateStatusNFe()
        {
            try
            {
                var flag = true;
                int contador = 0;
                Dispatcher.Invoke(new Action(() => { statusLabel.Content = " "; }), DispatcherPriority.ContextIdle, null);
                Dispatcher.Invoke(new Action(() => { BtnStatusNFeIcon.Foreground = new SolidColorBrush(Colors.Transparent); }), DispatcherPriority.ContextIdle, null);


                int valor = FuncoesNFe.StatusdeServicos(false);
                if (valor == 0)
                {
                    Dispatcher.Invoke(new Action(() => { BtnStatusNFeIcon.Foreground = new SolidColorBrush(Colors.Yellow); }), DispatcherPriority.ContextIdle, null);
                    Dispatcher.Invoke(new Action(() => { statusLabel.Content = "Amarelo: a consulta retornou negativa"; }), DispatcherPriority.ContextIdle, null);
                    Dispatcher.Invoke(new Action(() =>
                    {
                        var circulo = EFontAwesomeIconsExtensions.CreateImageSource(EFontAwesomeIcon.Solid_Circle, Brushes.Yellow, 10);
                        TaskbarItemInfo.Overlay = circulo;
                    }), DispatcherPriority.ContextIdle, null);

                    Thread.Sleep(5000);
                    valor = FuncoesNFe.StatusdeServicos(true);
                    contador++;
                    if (contador > 60)
                    {
                        Dispatcher.Invoke(new Action(() => { statusLabel.Content = "Vermelho: respostas negativas seguidas."; }), DispatcherPriority.ContextIdle, null);
                        Dispatcher.Invoke(new Action(() => { BtnStatusNFeIcon.Foreground = new SolidColorBrush(Colors.Red); }), DispatcherPriority.ContextIdle, null);
                        Dispatcher.Invoke(new Action(() =>
                        {
                            var circulo = EFontAwesomeIconsExtensions.CreateImageSource(EFontAwesomeIcon.Solid_Circle, Brushes.Red, 10);
                            TaskbarItemInfo.Overlay = circulo;
                        }), DispatcherPriority.ContextIdle, null);
                    }
                    Dispatcher.Invoke(new Action(() => { flag = Convert.ToBoolean(Application.Current.Windows.OfType<TELAPARAMETROS>().Count() > 0); }), DispatcherPriority.ContextIdle, null);
                    Dispatcher.Invoke(new Action(() =>
                    {
                        if (BtnGravar.IsEnabled) flag = false;
                    }), DispatcherPriority.ContextIdle, null);


                }
                if (valor == 107)
                {
                    Dispatcher.Invoke(new Action(() => { BtnStatusNFeIcon.Foreground = new SolidColorBrush(Colors.Green); }), DispatcherPriority.ContextIdle, null);
                    Dispatcher.Invoke(new Action(() => { statusLabel.Content = "Verde: a consulta retornou positiva."; }), DispatcherPriority.ContextIdle, null);

                    Dispatcher.Invoke(new Action(() =>
                    {
                        var ciclo = EFontAwesomeIconsExtensions.CreateImageSource(EFontAwesomeIcon.Solid_Circle, Brushes.Green, 10);
                        TaskbarItemInfo.Overlay = ciclo;
                    }), DispatcherPriority.ContextIdle, null);
                }

            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ATENÇÃO!!!");
            }
        }


        private void TxtEmitCnpj_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Funcoes.KeyDown(e, this);
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {

            Funcoes.KeyDown(e, this);
        }

        private void BtnDiretorioXml_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dlg = new System.Windows.Forms.FolderBrowserDialog();
                dlg.ShowDialog();
                TxtDiretorioXml.Text = dlg.SelectedPath;
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                    Funcoes.Mensagem(ex.Message, "Atenção", MessageBoxButton.OK);
            }
        }

        private void CertificadoA1emArquivo_Checked(object sender, RoutedEventArgs e)
        {
            if (BtnGravar != null)
                if (BtnGravar.IsEnabled)
                {
                    BtnSalvar_Click(null, null);
                    BtnEditar_Click(null, null);
                    TxtArquivoCertificado.IsEnabled = true;
                    TxtSenhaCertificado.IsEnabled = true;
                    TxtCertificado.IsEnabled = false;
                    if (CertificadoA1emArquivo.IsChecked == true)
                    {
                        GrupoA1.Visibility = Visibility.Visible;
                        GrupoA3.Visibility = Visibility.Collapsed;
                    }

                }
        }
        private void CertificadoA3_Checked(object sender, RoutedEventArgs e)
        {
            if (BtnGravar != null)
                if (BtnGravar.IsEnabled)
                {

                    BtnSalvar_Click(null, null);
                    BtnEditar_Click(null, null);

                    TxtArquivoCertificado.IsEnabled = false;
                    TxtSenhaCertificado.IsEnabled = false;
                    TxtCertificado.IsEnabled = true;

                    if (CertificadoA3.IsChecked == true)
                    {
                        GrupoA1.Visibility = Visibility.Collapsed;
                        GrupoA3.Visibility = Visibility.Visible;
                    }
                }
        }

        private void CmbEmitUf_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BtnGravar != null)
                if (BtnGravar.IsEnabled)
                    CmbUfDestino.Text = CmbEmitUf.Text;
        }

        private void BtnProcessaNFe_Click(object sender, RoutedEventArgs e)
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
                this.Close();
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Hversoes versao = new Hversoes(App.Parametro.Versao);
            versao = new Hversoes(versao.Ultimaversao());
            if (MessageBox.Show("Nova versão disponivel! Atualize já! " +
                        "\n " + versao.Notas +
                       "\n\nDeseja atualizar o seu Módulo Fiscal agora?", "Atualização do Módulo Fiscal!!!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                ///Hversoes versao = new Hversoes(App.Parametro.Versao);
                //versao = new Hversoes(versao.Ultimaversao());
                string _path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\dll\";

                //statusLabel.Text = "Aguarde enquanto está atualizando.";

                TELADEATUALIZACAO tela = new TELADEATUALIZACAO();
                tela.Show();
                tela.DownloadFile(versao.Url, _path + versao.Filename, "DOWNLOAD DE ATUALIZAÇÃO");

            }
        }


        private void BtnProcessaCCe_Click(object sender, RoutedEventArgs e)
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
                this.Close();
            }
        }

        private void BtnProcessaNFCe_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Windows.OfType<TELAPROCESSAMENTONFCE>().Count() > 0)
            {
                Application.Current.Windows.OfType<TELAPROCESSAMENTONFCE>().First().WindowState = WindowState.Normal;
                Application.Current.Windows.OfType<TELAPROCESSAMENTONFCE>().First().Activate();
            }
            else
            {
                TELAPROCESSAMENTONFCE tela = new TELAPROCESSAMENTONFCE();
                tela.Show();
                this.Close();
            }
        }



        private void BtnProcessaSATCe_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Windows.OfType<TELAPROCESSAMENTOSAT>().Count() > 0)
            {
                Application.Current.Windows.OfType<TELAPROCESSAMENTOSAT>().First().WindowState = WindowState.Normal;
                Application.Current.Windows.OfType<TELAPROCESSAMENTOSAT>().First().Activate();
            }
            else
            {
                TELAPROCESSAMENTOSAT tela = new TELAPROCESSAMENTOSAT();
                tela.Show();
                this.Close();
            }
        }

        private void TestarEMAILButton_Clik(object sender, RoutedEventArgs e)
        {
            if (ConfEmailComboBox.Text == "CIAF")
            {
                MandarEmail.NovoEmailTesteCiaf();
            }
            else
                MandarEmail.NovoEmailTesteEmpresa();
            EmailCopia.Focus();
        }

        private void CbxSalvarServidor_Checked(object sender, RoutedEventArgs e)
        {
            if (BtnGravar.IsEnabled)
                Funcoes.Mensagem(" Por gentileza, ao selecionar esse item vai salvar uma COPIA na pasta do servidor colocado no CONFIG." + Environment.NewLine + Environment.NewLine +
                                  "Lembresse que tem que ter as pasta AUTORIADA - INUTILIZADA - CANCELADA da NF-e e NFC-e.", " AVISO IMPORTANTE ", MessageBoxButton.OK);
        }

        private void BtnTextoPadrao_Click(object sender, RoutedEventArgs e)
        {
            AssuntoEmail.Text = "NF: #NumerodaNota# -  DOCUMENTO DIGITAL";
            MensagemEmail.Text = @"<html><head><style type=""text/css"">html{font-family:Helvetica;}</style></head>" +
            "<body><h3>Notificação de emissão de Nota Fiscal Eletronica</h3> " +
            "<h3> Prezado Cliente,</h3> \n" +
             "<p>Segue anexo a DOCUMENTO DIGITAL (xml e pdf). <br> \n" +
            @"Consulte a autencidade de sua NFe acessando <a href=""#Link#"">Consulta Nota Fiscal.</a> " +
            "\n </p> \n" +
            "<p>Nota: #NumerodaNota# <br> \n" +
            "Série: #SeriedaNota# <br> \n" +
            "Chave de acesso: #ChavedaNota# <br> \n" +
            "Data de Emissão: #DatadaNota# <br> \n" +
            "</p> \n" +
            "<p>Emitente: #RazaoSocialEmitente# <br> \n" +
            "CNPJ:  #CNPJEmitente# \n" +
            "</p> \n" +
            "<p>Destinatário: #RazaoSocialDestinatario# <br> \n" +
            "CNPJ/CPF:  #CNPJDestinatario# \n" +
            "</p> \n" +
            "<p>Emitindo pelo Sistema CIAF - www.ciaf.com.br </p></body></html> \n";

        }

        private void ConfEmailComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (ConfEmailComboBox.Text.Trim() == "CIAF")
            {
                ServidorSmtp.Text = "smtp.dominio.com";
            }
            Funcoes.KeyDown(e, this);

        }
    }
}
