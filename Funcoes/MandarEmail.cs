using System;
using System.Windows;
using System.IO;
using static nfecreator.EmailBuilderCiaf;
using System.Reflection;
using DFe.Utils;
using NFe.Utils.NFe;
using NFe.Classes;

namespace nfecreator
{
    class MandarEmail
    {
        readonly static string email = "nfe@ciaf.com.br";
        readonly static string senha = "Ci@f2035#";
        static ConfiguracaoApp _configuracoes;
        private const string ArquivoConfiguracao = @"\configuracao.xml";

        public static void NovoEmailNFeCiaf(VendaNFe vnfe,  Vendanfea vnfea, string destinatario)
        {
            try
            {
                CarregarConfiguracao();
                string path = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\Pics\EmailCiaf.htm";
                if (!File.Exists(path))
                {
                    string createText = "NF: " + vnfe.Nrvenda.ToString() + " - DOCUMENTO DIGITAL" + Environment.NewLine;
                    File.WriteAllText(path, createText);
                }

                string appendText = " " + Environment.NewLine;
                File.AppendAllText(path, appendText);

                // Open the file to read from.
                string readText = File.ReadAllText(path);
                Console.WriteLine(readText);

                readText = readText.Replace("#NumerodaNota#", vnfea.Nrvenda.ToString());
                readText = readText.Replace("#SeriedaNota#", vnfe.Serie.ToString());
                readText = readText.Replace("#ChavedaNota#", vnfea.Chave.ToString());
                readText = readText.Replace("#DatadaNota#", vnfe.DhEmi.ToString("dd/MM/yyyy"));
                readText = readText.Replace("#RazaoSocialEmitente#", App.Parametro.Razaosocial);
                readText = readText.Replace("#CNPJEmitente#", App.Parametro.Cnpj);
                readText = readText.Replace("#RazaoSocialDestinatario#", vnfe.Nomecliente);
                readText = readText.Replace("#CNPJDestinatario#", vnfe.Cnpj_cpf);

                readText = readText.Replace("date('Y')", DateTime.Now.ToString("dd/MM/yyyy - HH:mm"));

                string assunto = "NF: " + vnfe.Nrvenda.ToString() + " - DOCUMENTO DIGITAL ";
                string mensagem = readText;
                string servidorSmtp = "mail.ciaf.com.br";
                int porta = 26;
                string _path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\NFe_Autorizada\";
                ConfiguracaoEmailCiaf configuracao = new ConfiguracaoEmailCiaf(email, senha, assunto, mensagem, servidorSmtp, porta);

                var emailBuilder = new EmailBuilder(configuracao)
                    .AdicionarReply(App.Parametro.Email)
                    .AdicionarAnexo(_path + vnfea.Chave + "-procNFe.pdf")
                    .AdicionarAnexo(_path + vnfea.Chave + "-procNFe.xml");

                var emailDoDestinatario = destinatario;
                if (string.IsNullOrEmpty(emailDoDestinatario))
                {
                    if (!string.IsNullOrEmpty(_configuracoes.ConfiguracaoDanfeNfe.EmailCopia.Trim()))
                        emailDoDestinatario = _configuracoes.ConfiguracaoDanfeNfe.EmailCopia.Trim();
                    else
                        throw new ArgumentException("O cliente desta nota está com e-mail vazio, verifique no cadastro de cliente.");
                }

                foreach (var address in emailDoDestinatario.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    emailBuilder.AdicionarDestinatario(address.Trim());
                }


                if (emailDoDestinatario != _configuracoes.ConfiguracaoDanfeNfe.EmailCopia.Trim())
                    if (!string.IsNullOrEmpty(_configuracoes.ConfiguracaoDanfeNfe.EmailCopia.Trim()))
                        foreach (var address in _configuracoes.ConfiguracaoDanfeNfe.EmailCopia.Trim().Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            emailBuilder.AdicionarCopia(address.Trim());
                        }

                emailBuilder.DepoisDeEnviarEmail += EventoDepoisDeEnviarEmail;
                emailBuilder.ErroAoEnviarEmail += erro => System.Windows.MessageBox.Show(erro.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                emailBuilder.Enviar();
            }
            catch (ArgumentException ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "ERRO E-MAIL CIAF", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "ERRO E-MAIL CIAF", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                    System.Windows.MessageBox.Show(ex.Message, "ERRO E-MAIL CIAF", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

        public static void NovoEmailNFCeCiaf(VendaNFCe vnfce, Vendanfcea vnfcea, string destinatario)
        {
            try
            {
                CarregarConfiguracao();
                string path = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\Pics\EmailCiafNFCE.htm";

                if (!File.Exists(path))
                {
                    string createText = "DOCUMENTO DIGITAL" + Environment.NewLine;
                    File.WriteAllText(path, createText);
                }

                string appendText = " " + Environment.NewLine;
                File.AppendAllText(path, appendText);

                // Open the file to read from.
                string readText = File.ReadAllText(path);
                Console.WriteLine(readText);
                readText = readText.Replace("#NumerodaNota#", vnfcea.Nrvenda.ToString());
                readText = readText.Replace("#SeriedaNota#", vnfce.Serie.ToString());
                readText = readText.Replace("#ChavedaNota#", vnfcea.Chave.ToString());
                readText = readText.Replace("#DatadaNota#", vnfce.DhEmi.ToString("dd/MM/yyyy"));
                readText = readText.Replace("#RazaoSocialEmitente#", App.Parametro.Razaosocial);
                readText = readText.Replace("#CNPJEmitente#", App.Parametro.Cnpj);
                readText = readText.Replace("#RazaoSocialDestinatario#", vnfce.Nomecliente);
                readText = readText.Replace("#CNPJDestinatario#", vnfce.Cnpj_cpf);

                readText = readText.Replace("date('Y')", DateTime.Now.ToString("dd/MM/yyyy - HH:mm"));


                string assunto = "DOCUMENTO DIGITAL - Emitido pelo Sistema CIAF *** NÃO RESPONDA ESTA MENSAGEM ***";
                string mensagem = readText;
                string servidorSmtp = "mail.ciaf.com.br";
                int porta = 26;

                string _path = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\NFCe\";

                if (vnfcea.Statusnfe == "AUTORIZADO")
                {
                    _path += @"Autorizada\";
                }
                else if (vnfcea.Statusnfe == "CANCELADO")
                {
                    _path += @"Cancelada\";
                }
                else if (vnfcea.Statusnfe == "INULTILIZADO")
                {
                    _path += @"Inutilizada\";
                }
                else
                {
                    _path += @"Autorizada\";
                }


                NFe.Classes.NFe _nfe = new NFe.Classes.NFe().CarregarDeArquivoXml(_path + vnfcea.Chave + "-procNFe.xml");
                readText = readText.Replace(@"http://nfce.encat.org/", _nfe.infNFeSupl.qrCode.Replace(@"<![CDATA[","").Replace(@"]]>", ""));
                mensagem = readText;

                ConfiguracaoEmailCiaf configuracao = new ConfiguracaoEmailCiaf(email, senha, assunto, mensagem, servidorSmtp, porta);

                if (string.IsNullOrEmpty(destinatario))
                    throw new ArgumentException("O cliente desta nota está com e-mail vazio, verifique no cadastro de cliente.");


                var emailBuilder = new EmailBuilder(configuracao)
                    .AdicionarReply(App.Parametro.Email)
                    .AdicionarAnexo(_path + vnfcea.Chave + "-procNFe.pdf")
                    .AdicionarAnexo(_path + vnfcea.Chave + "-procNFe.xml");


                var emailDoDestinatario = destinatario;
                if (string.IsNullOrEmpty(emailDoDestinatario))
                {
                    if (!string.IsNullOrEmpty(_configuracoes.ConfiguracaoDanfeNfe.EmailCopia.Trim()))
                        emailDoDestinatario = _configuracoes.ConfiguracaoDanfeNfe.EmailCopia.Trim();
                    else
                        throw new ArgumentException("O cliente desta nota está com e-mail vazio, verifique no cadastro de cliente.");
                }

                foreach (var address in emailDoDestinatario.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    emailBuilder.AdicionarDestinatario(address.Trim());
                }


                if (emailDoDestinatario != _configuracoes.ConfiguracaoDanfeNfe.EmailCopia.Trim())
                    if (!string.IsNullOrEmpty(_configuracoes.ConfiguracaoDanfeNfe.EmailCopia.Trim()))
                        foreach (var address in _configuracoes.ConfiguracaoDanfeNfe.EmailCopia.Trim().Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            emailBuilder.AdicionarCopia(address.Trim());
                        }

                emailBuilder.DepoisDeEnviarEmail += EventoDepoisDeEnviarEmail;
                emailBuilder.ErroAoEnviarEmail += erro => System.Windows.MessageBox.Show(erro.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                emailBuilder.Enviar();
            }
            catch (ArgumentException ex)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                {
                    if (ex.Message.Contains("O cliente desta nota está com e-mail vazio, verifique no cadastro de cliente."))
                    {
                        MinhaNotificacao.NotificarErro("E-MAIL", ex.Message);
                    }
                    else
                    Funcoes.Crashe(ex, "ERRO", false);
                }                
            }
            catch (InvalidOperationException ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "ERRO E-MAIL CIAF - COD:1", MessageBoxButton.OK, MessageBoxImage.Error);
                Funcoes.Crashe(ex, "ERRO", false);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "ERRO E-MAIL CIAF - COD:2", MessageBoxButton.OK, MessageBoxImage.Error);
                Funcoes.Crashe(ex, "ERRO", false);
            }

        }


        public static void NovoEmailTesteCiaf()
        {
            try
            {

                string path = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\Pics\EmailCiafNFCE.htm";

                if (!File.Exists(path))
                {
                    string createText = "DOCUMENTO DIGITAL" + Environment.NewLine;
                    File.WriteAllText(path, createText);
                }

                string appendText = " " + Environment.NewLine;
                File.AppendAllText(path, appendText);

                // Open the file to read from.
                string readText = File.ReadAllText(path);
                Console.WriteLine(readText);


                // readText = readText.Replace("http://www.nfe.fazenda.gov.br/portal/disponibilidade.aspx", venda.Qrcode);

                readText = readText.Replace("YYY", App.Parametro.Razaosocial);

                readText = readText.Replace("XXX", "CHAVE");

                readText = readText.Replace("date('Y')", DateTime.Now.ToString("dd/MM/yyyy - HH:mm"));


                string assunto = "DOCUMENTO DIGITAL - Emitido pelo Sistema CIAF *** NÃO RESPONDA ESTA MENSAGEM ***";
                string mensagem = readText;
                string servidorSmtp = "mail.ciaf.com.br";
                int porta = 26;

                string _path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\NFCE\NFe";

                ConfiguracaoEmailCiaf configuracao = new ConfiguracaoEmailCiaf(email, senha, assunto, mensagem, servidorSmtp, porta);

                var emailBuilder = new EmailBuilder(configuracao)
                    .AdicionarReply(App.Parametro.Email)
                    .AdicionarDestinatario(App.Parametro.Email)
                    .AdicionarCopia("suporte@ciaf.com.br");

                if (!string.IsNullOrEmpty(_configuracoes.ConfiguracaoDanfeNfe.EmailCopia.Trim()))
                    emailBuilder.AdicionarCopia(_configuracoes.ConfiguracaoDanfeNfe.EmailCopia.Trim());

                emailBuilder.DepoisDeEnviarEmail += EventoDepoisDeEnviarEmail;
                emailBuilder.ErroAoEnviarEmail += erro => System.Windows.MessageBox.Show(erro.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                emailBuilder.Enviar();
            }
            catch (ArgumentException ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "ERRO E-MAIL CIAF", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "ERRO E-MAIL CIAF", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                    System.Windows.MessageBox.Show(ex.Message, "ERRO E-MAIL CIAF", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

        public static void NovoEmailTesteEmpresa()
        {
            try
            {


                CarregarConfiguracao();
                //_configuracoes.ConfiguracaoEmail.MensagemEmHtml = false;
                _configuracoes.ConfiguracaoEmail.Nome = "Teste";
                var emailBuilder = new NFe.Utils.Email.EmailBuilder(_configuracoes.ConfiguracaoEmail)
                       .AdicionarDestinatario(App.Parametro.Email);
                
                emailBuilder.AdicionarCopia("suporte@ciaf.com.br");

                emailBuilder.DepoisDeEnviarEmail += EventoDepoisDeEnviarEmail;
                emailBuilder.ErroAoEnviarEmail += EventoErroEmail;

                if (!string.IsNullOrEmpty(_configuracoes.ConfiguracaoDanfeNfe.EmailCopia.Trim()))
                    emailBuilder.AdicionarCopia(_configuracoes.ConfiguracaoDanfeNfe.EmailCopia.Trim());

                emailBuilder.Enviar();




            }
            catch (ArgumentException ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "ERRO E-MAIL CIAF", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "ERRO E-MAIL CIAF", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                    System.Windows.MessageBox.Show(ex.Message, "ERRO E-MAIL CIAF", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

        private static void CarregarConfiguracao()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            try
            {
                _configuracoes = !File.Exists(path + ArquivoConfiguracao)
                    ? new ConfiguracaoApp()
                    : FuncoesXml.ArquivoXmlParaClasse<ConfiguracaoApp>(path + ArquivoConfiguracao);
                if (_configuracoes.CfgServico.TimeOut == 0)
                    _configuracoes.CfgServico.TimeOut = 3000; //mínimo

                #region Carrega a logo no controle logoEmitente

                //if (_configuracoes.ConfiguracaoDanfeNfce.Logomarca != null && _configuracoes.ConfiguracaoDanfeNfce.Logomarca.Length > 0)
                //    using (var stream = new MemoryStream(_configuracoes.ConfiguracaoDanfeNfce.Logomarca))
                //    {
                //        LogoEmitente.Source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                //    }

                #endregion
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex);
            }
        }

        private static void EventoDepoisDeEnviarEmail(object sender, EventArgs e)
        {
            MinhaNotificacao.NotificarEInfo("E-mail encaminhado com sucesso.", "E-MAIL");
        }


        private static void EventoErroEmail(Exception erro)
        {
            Funcoes.Mensagem(erro.Message, "Erro do E-mail", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }
}
