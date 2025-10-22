using DFe.Utils;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;

namespace nfecreator
{
    public static class FuncoesFTP
    {
        private static ConfiguracaoApp _configuracoes;
        private const string ArquivoConfiguracao = @"\configuracao.xml";
        private static string UsuarioFtp = @"ciaf@ciaf.com.br";
        private static string SenhaUsuarioFtp = ConfigurationManager.ConnectionStrings["tvsistem_ciafftp"].ConnectionString;
        private static string UrlFtpInicial = @"ftp://ftp.ciaf.com.br/public_html/app/empresa/" + Funcoes.Deixarnumero(App.Parametro.Cnpj) + @"/";
        private static readonly string _path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static string UrlRetorno = @"http://www.ciaf.com.br/app/empresa/" + Funcoes.Deixarnumero(App.Parametro.Cnpj) + @"/";

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

                #endregion
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "AVISO - CARREGAR CONFIGURAÇÃO");
            }
        }

        public static string ExistePasta(string local)
        {
            try
            {
                var request = (FtpWebRequest)WebRequest.Create(local + "index.php");
                request.Credentials = new NetworkCredential(UsuarioFtp, SenhaUsuarioFtp);
                request.Method = WebRequestMethods.Ftp.GetFileSize;

                try
                {
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    return "true";
                }
                catch (WebException ex)
                {
                    FtpWebResponse response = (FtpWebResponse)ex.Response;
                    if (response.StatusCode ==
                        FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        byte[] buffer = Encoding.ASCII.GetBytes(@"<?php header(""Location: https://ciaf.com.br/site/"");");

                        FtpWebRequest Request;
                        Stream RequestStream;

                        Request = (FtpWebRequest)FtpWebRequest.Create(local);
                        Request.Credentials = new NetworkCredential(UsuarioFtp, SenhaUsuarioFtp);
                        Request.UsePassive = true;
                        Request.UseBinary = true;
                        Request.KeepAlive = false;
                        Request.Method = WebRequestMethods.Ftp.MakeDirectory;
                        using (var resp = (FtpWebResponse)Request.GetResponse())
                        {
                            Console.WriteLine(resp.StatusCode);
                        }


                        Request = (FtpWebRequest)FtpWebRequest.Create(local + "/index.php");
                        Request.Credentials = new NetworkCredential(UsuarioFtp, SenhaUsuarioFtp);
                        Request.UsePassive = true;
                        Request.UseBinary = true;
                        Request.KeepAlive = false;
                        Request.Method = WebRequestMethods.Ftp.UploadFile;
                        RequestStream = Request.GetRequestStream();
                        RequestStream.Write(buffer, 0, buffer.Length);
                        RequestStream.Close();

                        return "false";
                    }
                    return "erro";
                }


            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "AVISO - COD: SubirArquivoT");
                return "Erro";
            }
        }


        public static void SaveNota(string local)
        {
            try
            {
                CarregarConfiguracao();
                if (!string.IsNullOrEmpty(local))
                    if (!local.Contains("VERIFICAR"))
                        if (_configuracoes.ConfiguracaoDanfeNfe.SalvarServidor)
                        {
                            DbfBase ebase = new DbfBase();
                            string novolocal = local.Replace(_path, ebase.Path.Replace(@"\DADOS", "").Replace(@"\dados", ""));

                            if (!System.IO.File.Exists(novolocal))
                                System.IO.File.Copy(local, novolocal);

                        }

            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "AVISO - COD:SaveNotaServidor");
            }

        }

        public static void DeleteArquivoLocal(string localNomeArquivo)
        {
            try
            {
                File.Delete(localNomeArquivo);
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "AVISO - ARQUIVOLOCAL");
            }
        }

        internal static void GuardaXML(string retornoXmlString, string pasta, string nome)
        {
            try
            { //-procNFe
                var stw = new StreamWriter(pasta + nome + ".xml");
                stw.WriteLine(retornoXmlString);
                stw.Close();

                CarregarConfiguracao();
                if (_configuracoes.CfgServico.DiretorioSalvarXml.Replace(" ", "") != "" && _configuracoes.CfgServico.DiretorioSalvarXml != null)
                {
                    stw = new StreamWriter(_configuracoes.CfgServico.DiretorioSalvarXml + @"\" + nome + ".xml");
                    stw.WriteLine(retornoXmlString);
                    stw.Close();
                }
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "AVISO -  GUARDA XML");
            }
        }


        public static string SubirArquivo(string local, string NomeArquivo, string tipo)
        {
            string retorno = "";
            var t = new Thread(() => retorno = SubirArquivoTipo(local, NomeArquivo, tipo));
            t.Start();
            t.Join();
            return retorno;
        }

        public static string SubirArquivoTipo(string local, string NomeArquivo, string tipo)
        {
            try
            {

                byte[] buffer = System.IO.File.ReadAllBytes(local);
                string UrlFtp = UrlFtpInicial + tipo + @"/";

                ExistePasta(UrlFtpInicial);
                ExistePasta(UrlFtp);

                var request = (FtpWebRequest)WebRequest.Create(UrlFtp + NomeArquivo);
                request.Credentials = new NetworkCredential(UsuarioFtp, SenhaUsuarioFtp);
                request.Method = WebRequestMethods.Ftp.GetFileSize;

                FtpWebRequest Request;
                Stream RequestStream;

                Request = (FtpWebRequest)FtpWebRequest.Create(UrlFtp + NomeArquivo.Replace(UrlFtp, ""));
                Request.Credentials = new NetworkCredential(UsuarioFtp, SenhaUsuarioFtp);
                Request.UsePassive = true;
                Request.UseBinary = true;
                Request.KeepAlive = false;
                Request.Method = WebRequestMethods.Ftp.UploadFile;
                RequestStream = Request.GetRequestStream();
                RequestStream.Write(buffer, 0, buffer.Length);
                RequestStream.Close();
                return UrlRetorno + tipo + @"/" + NomeArquivo;
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "AVISO - COD: SubirArquivoT");
                return "Erro";
            }
        }

    }
}