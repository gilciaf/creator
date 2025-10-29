using NFe.Classes.Informacoes.Pagamento;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using DFe.Classes.Flags;
using DFe.Utils;
using NFe.Classes;
using NFe.Classes.Informacoes;
using NFe.Classes.Informacoes.Cobranca;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Detalhe;
using NFe.Classes.Informacoes.Detalhe.Tributacao;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.Tipos;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using NFe.Classes.Informacoes.Total;
using NFe.Classes.Informacoes.Transporte;
using NFe.Classes.Servicos.Tipos;
using NFe.Servicos;
using NFe.Utils;
using NFe.Utils.Excecoes;
using NFe.Utils.NFe;
using System.Reflection;
using NFe.Classes.Informacoes.Observacoes;
using NFe.Servicos.Retorno;
using NFe.Utils.Email;
using NFe.Classes.Servicos.Consulta;
using NFe.Utils.Consulta;
using Shared.NFe.Classes.Informacoes.InfRespTec;
using Shared.NFe.Utils.InfRespTec;
using NFe.Classes.Informacoes.Detalhe.ProdEspecifico;
using NFe.Utils.Tributacao.Federal;
using NFe.Classes.Servicos.Inutilizacao;
using NFe.Classes.Informacoes.Detalhe.DeclaracaoImportacao;
using DFe.Classes.Entidades;
using NFe.Utils.Tributacao.Estadual;
using System.Reflection.Emit;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Compartilhado;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Compartilhado.InformacoesIbsCbs;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Compartilhado.InformacoesIbsCbs.InformacoesCbs;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Compartilhado.InformacoesIbsCbs.InformacoesIbs;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Compartilhado.Tipos;
using NFe.Classes.Informacoes.Total.IbsCbs;
using NFe.Classes.Informacoes.Total.IbsCbs.Cbs;
using NFe.Classes.Informacoes.Total.IbsCbs.Monofasica;

namespace nfecreator
{
    class FuncoesNFe
    {
        private const string ArquivoConfiguracao = @"\configuracao.xml";
        private static ConfiguracaoApp _configuracoes;
        private static readonly string _path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        
        private CRT crtcliente;
        private ServicosNFe servicoNFe;
        private VendaNFe vendanfe;
        private Vendanfea vendanfea;

        private static void CarregarConfiguracao()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Console.WriteLine(path + "  PATH --- ");
            try
            {
                _configuracoes = !File.Exists(path + ArquivoConfiguracao)
                    ? new ConfiguracaoApp()
                    : FuncoesXml.ArquivoXmlParaClasse<ConfiguracaoApp>(path + ArquivoConfiguracao);
                if (_configuracoes.CfgServico.TimeOut == 0)
                    _configuracoes.CfgServico.TimeOut = 3000; //mínimo

                //aqui a mudança do CE
                if (App.Parametro.Uf == "CE")
                    ConfiguracaoUrls.FactoryUrl = Shared.NFe.Utils.Enderecos.NovasUrlsCeara.FactoryUrlCearaMudanca.CriaFactoryUrl();

                #region Carrega a logo no controle logoEmitente

                if (_configuracoes.ConfiguracaoDanfeNfe.Logomarca != null && _configuracoes.ConfiguracaoDanfeNfe.Logomarca.Length > 0)
                {
                    // 

                }
                else
                {


                }


                #endregion
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex);
            }
        }

        public static int StatusdeServicos(Boolean readmessage = true)
        {
            try
            {
                int valor = 0;

                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                ConfiguracaoApp configuracoes;
                configuracoes = !File.Exists(path + ArquivoConfiguracao)
                        ? new ConfiguracaoApp()
                        : FuncoesXml.ArquivoXmlParaClasse<ConfiguracaoApp>(path + ArquivoConfiguracao);
                if (configuracoes.CfgServico.TimeOut == 0)
                    configuracoes.CfgServico.TimeOut = 3000; //mínimo
                //configuracoesc= TipoAmbiente.Homologacao;
                using (var servicoNFe = new ServicosNFe(configuracoes.CfgServico))
                {
                    RetornoNfeStatusServico retornoStatus = servicoNFe.NfeStatusServico();
                    int valorStat = retornoStatus.Retorno.cStat;
                    string Motivo = retornoStatus.Retorno.xMotivo ?? retornoStatus.RetornoCompletoStr;
                    if (readmessage)
                        MessageBox.Show("Codigo:" + valorStat.ToString() + " Motivo: " + Motivo, "Retorno da Sefaz!!!! ");
                    valor = valorStat;
                }
                return valor;
            }
            catch (ComunicacaoException comunicao)
            {
                if (readmessage)
                    Funcoes.Crashe(comunicao, "SERVIDORES DA SEFAZ FORA DO AR!");
                return 0;
            }
            catch (Exception ex)
            {
                if (readmessage)
                    Funcoes.Crashe(ex, "ERRO DA SEFAZ");
                return 0;
            }
        }

        public string Verificar(VendaNFe vnfe, Vendanfea vnfea)
        {
            try
            {
                #region Consulta Recibo de lote
                vendanfe = vnfe;
                vendanfea = vnfea;

                var chave = vendanfea.Chave.Replace("NFe", "");
                CarregarConfiguracao();

                if (string.IsNullOrEmpty(chave)) throw new Exception("A Chave deve ser informada!");

                ServicosNFe servicoNFe = new ServicosNFe(_configuracoes.CfgServico);
                RetornoNfeConsultaProtocolo retornoRecibo = servicoNFe.NfeConsultaProtocolo(chave);

                string Motivo = retornoRecibo.Retorno.xMotivo ?? retornoRecibo.Retorno.protNFe.infProt.xMotivo;
                int valorStat = retornoRecibo.Retorno.cStat != 0 ? retornoRecibo.Retorno.cStat : retornoRecibo.Retorno.protNFe.infProt.cStat;
                string protocolo = retornoRecibo.Retorno.protNFe != null ? retornoRecibo.Retorno.protNFe.infProt.nProt : "";

                if (valorStat == 100)
                {
                    vendanfea.Chave = chave;
                    vendanfea.Xml = chave + "-procNFe.pdf";
                    vendanfea.Danfe = chave + "-procNFe.pdf";
                    vendanfea.Statusnfe = "APROVADO";
                    vendanfea.Nprotocolo = protocolo;
                    vendanfea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                    vendanfea.Update();

                    NFe.Classes.NFe _nfe = GetNf(vendanfe.Nrvenda, _configuracoes.CfgServico.ModeloDocumento, _configuracoes.CfgServico.VersaoNFeAutorizacao);
                    _nfe.Assina(); //não precisa validar aqui, pois o lote será validado em ServicosNFe.NFeAutorizacao
                    
                    FuncoesFTP.GuardaXML(_nfe.ObterXmlString(), _path + @"\NFe_Autorizada\", _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe");
                    var local = _path + @"\NFe_Autorizada\" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml";

                    var proc = new nfeProc
                    {
                        NFe = new NFe.Classes.NFe().CarregarDeArquivoXml(local),
                        protNFe = retornoRecibo.Retorno.protNFe,
                        versao = retornoRecibo.Retorno.versao
                    };

                    FuncoesFTP.GuardaXML(proc.ObterXmlString(), _path + @"\NFe_Autorizada\", vendanfea.Chave.Replace("NFe", "") + "-procNFe");
                    string localxml = FuncoesFTP.SubirArquivo(local, _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml", "nfe");

                    Notasemitidas notaemitida = new Notasemitidas()
                    {
                        Chave = _nfe.infNFe.Id,
                        Modelo = (int)_nfe.infNFe.ide.mod,
                        Cnpj = _configuracoes.Emitente.CNPJ,
                        Data = DateTime.Now,
                        Valor = vendanfe.Vnf,
                        Localxml = localxml,
                    };
                    notaemitida.Insert();


                    GerarPdf(vendanfe, vendanfea);
                    if (_configuracoes.ConfiguracaoDanfeNfe.EmailAut == "SIM")
                    {
                        if (!string.IsNullOrEmpty(_nfe.infNFe.dest.email))
                            Email(vendanfe, vendanfea, _nfe.infNFe.dest.email);
                        else
                            MinhaNotificacao.NotificarErro("Nota sem e-mail de destinatario.", "E-MAIL");

                    }

                }
                else if (valorStat == 101 ||
                    valorStat == 135 ||
                    valorStat == 151 ||
                    valorStat == 155)
                {
                    protocolo = retornoRecibo.Retorno.protNFe != null ? retornoRecibo.Retorno.procEventoNFe.First().retEvento.infEvento.nProt : protocolo;

                    vendanfea.Statusnfe = "CANCELADO";
                    vendanfea.Nprotocolo = protocolo;
                    vendanfea.Danfe = chave.Replace("NFe", "") + "-procNFe.pdf";
                    vendanfea.Xml = chave.Replace("NFe", "") + "-procNFe.xml";
                    vendanfea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                    vendanfea.Update();

                    Notasemitidas notaemitida = new Notasemitidas()
                    {
                        Chave = vendanfea.Chave.Replace("NFe", ""),
                        Modelo = 55,
                        Cnpj = _configuracoes.Emitente.CNPJ,
                        Data = DateTime.Now,
                        Valor = vendanfe.Vnf,
                    };
                    notaemitida.Insert();
                    GerarPdf(vendanfe, vendanfea);


                }
                else if (valorStat == 110 ||
                    valorStat == 301 ||
                    valorStat == 302 ||
                    valorStat == 303)
                {
                    vendanfea.Statusnfe = "DENEGADO";
                    vendanfea.Nprotocolo = protocolo;
                    vendanfea.Danfe = chave.Replace("NFe", "") + "-procNFe.pdf";
                    vendanfea.Xml = chave.Replace("NFe", "") + "-procNFe.xml";
                    vendanfea.Update();

                    NFe.Classes.NFe _nfe = GetNf(vendanfe.Nrvenda, _configuracoes.CfgServico.ModeloDocumento, _configuracoes.CfgServico.VersaoNFeAutorizacao);
                    _nfe.Assina(); //não precisa validar aqui, pois o lote será validado em ServicosNFe.NFeAutorizacao
                    var local = _path + @"\NFe_Autorizada\" + chave.Replace("NFe", "") + "-procNFe.xml";
                    var proc = new nfeProc
                    {
                        NFe = _nfe,
                        protNFe = retornoRecibo.Retorno.protNFe,
                        versao = retornoRecibo.Retorno.versao
                    };
                    FuncoesFTP.GuardaXML(proc.ObterXmlString(), _path + @"\NFe_Autorizada\", chave.Replace("NFe", "") + "-procNFe");
                    string localxml = FuncoesFTP.SubirArquivo(local, chave.Replace("NFe", "") + "-procNFe.xml", "nfe");

                    Notasemitidas notaemitida = new Notasemitidas()
                    {
                        Chave = vendanfea.Chave.Replace("NFe", ""),
                        Modelo = 55,
                        Cnpj = _configuracoes.Emitente.CNPJ,
                        Data = DateTime.Now,
                        Valor = vendanfe.Vnf,
                        Localxml = localxml,
                    };
                    notaemitida.Insert();
                    GerarPdf(vendanfe, vendanfea);
                }
                else
                {
                    vendanfea.Statusnfe = "REJEITADO";
                    vendanfea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                    vendanfea.Update();
                }
                #endregion
                return retornoRecibo.RetornoStr;
            }
            catch (ComunicacaoException ex)
            {
                //Funcoes.Crashe(ex, "", false);
                Funcoes.Mensagem(ex.Message + Environment.NewLine + Environment.NewLine + Environment.NewLine +
                    " Por gentileza, " + Environment.NewLine +
                     "verifique a disponibilidade no portal www.nfe.fazenda.gov.br", "RETORNO COMUNICAÇÃO SEFAZ ", MessageBoxButton.OK);
                return null;
            }
            catch (ValidacaoSchemaException ex)
            {
                Funcoes.Crashe(ex, "", false);
                Funcoes.Mensagem(ex.Message, "Erro - Validação", MessageBoxButton.OK);
                return null;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("A Chave deve ser informada!"))
                {
                    Funcoes.Mensagem(ex.Message, "FALTANDO A CHAVE", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    Funcoes.Crashe(ex, "ERRO DA SEFAZ");
                }

                return null;
            }
        }

        public string ConsultaRecibo(VendaNFe vnfe, Vendanfea vnfea, string recibo)
        {
            try
            {
                #region Consulta Recibo de lote   

                vendanfe = vnfe;
                vendanfea = vnfea;

                if (string.IsNullOrEmpty(recibo)) throw new Exception("A recibo deve ser informada!");

                RetornoNFeRetAutorizacao retornoRecibo = servicoNFe.NFeRetAutorizacao(recibo);

                string Motivo = retornoRecibo.Retorno.xMotivo != null ? retornoRecibo.Retorno.protNFe.First().infProt.xMotivo : retornoRecibo.Retorno.xMotivo;
                int valorStat = retornoRecibo.Retorno.cStat != 0 ? retornoRecibo.Retorno.protNFe.First().infProt.cStat : retornoRecibo.Retorno.cStat;
                if (valorStat == 100)
                {
                    string protocolo = retornoRecibo.Retorno.protNFe.First().infProt.nProt;
                    if (vendanfea.Statusnfe != "APROVADO")
                    {

                        vendanfea.Statusnfe = "APROVADO";
                        vendanfea.Nprotocolo = protocolo;
                        vendanfea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                        vendanfea.Xml = vendanfea.Chave + "-procNFe.pdf";
                        vendanfea.Danfe = vendanfea.Chave + "-procNFe.pdf";
                        vendanfea.Update();

                        NFe.Classes.NFe _nfe = GetNf(vendanfe.Nrvenda, _configuracoes.CfgServico.ModeloDocumento, _configuracoes.CfgServico.VersaoNFeAutorizacao);
                        _nfe.Assina(); //não precisa validar aqui, pois o lote será validado em ServicosNFe.NFeAutorizacao
                        FuncoesFTP.GuardaXML(_nfe.ObterXmlString(), _path + @"\NFe_Autorizada\", _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe");
                        var local = _path + @"\NFe_Autorizada\" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml";

                        var proc = new nfeProc
                        {
                            NFe = new NFe.Classes.NFe().CarregarDeArquivoXml(local),
                            protNFe = retornoRecibo.Retorno.protNFe.First(),
                            versao = retornoRecibo.Retorno.versao
                        };

                        FuncoesFTP.GuardaXML(proc.ObterXmlString(), _path + @"\NFe_Autorizada\", _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe");
                        string localxml = FuncoesFTP.SubirArquivo(local, _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml", "nfe");
                        string localsave = _path + @"\NFe_Autorizada\" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.pdf";

                        GerarPdf(vendanfe, vendanfea);

                        Notasemitidas notaemitida = new Notasemitidas()
                        {
                            Chave = _nfe.infNFe.Id,
                            Modelo = (int)_nfe.infNFe.ide.mod,
                            Cnpj = _configuracoes.Emitente.CNPJ,
                            Data = DateTime.Now,
                            Valor = vendanfe.Vnf,
                            Localxml = localxml,
                        };
                        notaemitida.Insert();

                        if (!string.IsNullOrEmpty(_configuracoes.ConfiguracaoDanfeNfe.EmailAut))
                            if (_configuracoes.ConfiguracaoDanfeNfe.EmailAut == "SIM")
                            {
                                try
                                {
                                    if (!string.IsNullOrEmpty(_nfe.infNFe.dest.email))
                                        Email(vendanfe, vendanfea, _nfe.infNFe.dest.email);
                                    else
                                        MinhaNotificacao.NotificarErro("Nota sem e-mail de destinatario.", "E-MAIL");
                                }
                                catch
                                {

                                }
                            }
                    }

                }
                else if (valorStat == 101 ||
                    valorStat == 135 ||
                    valorStat == 151 ||
                    valorStat == 155)
                {

                    if (vendanfea.Statusnfe != "CANCELADO")
                    {
                        string protocolo = retornoRecibo.Retorno.protNFe.First().infProt.nProt;
                        vendanfea.Statusnfe = "CANCELADO";
                        vendanfea.Nprotocolo = protocolo;
                        vendanfea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                        vendanfea.Update();
                    }
                }
                else if (valorStat == 110 ||
                    valorStat == 301 ||
                    valorStat == 302 ||
                    valorStat == 303)
                {
                    string protocolo = retornoRecibo.Retorno.protNFe.First().infProt.nProt;
                    vendanfea.Statusnfe = "DENEGADO";
                    vendanfea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                    vendanfea.Nprotocolo = protocolo;
                    vendanfea.Update();

                    NFe.Classes.NFe _nfe = GetNf(vendanfe.Nrvenda, _configuracoes.CfgServico.ModeloDocumento, _configuracoes.CfgServico.VersaoNFeAutorizacao);
                    _nfe.Assina(); //não precisa validar aqui, pois o lote será validado em ServicosNFe.NFeAutorizacao
                    FuncoesFTP.GuardaXML(_nfe.ObterXmlString(), _path + @"\NFe_Autorizada\", _nfe.infNFe.Id.Replace("NFe", ""));
                    var local = _path + @"\NFe_Autorizada\" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml";

                    var proc = new nfeProc
                    {
                        NFe = new NFe.Classes.NFe().CarregarDeArquivoXml(local),
                        protNFe = retornoRecibo.Retorno.protNFe.First(),
                        versao = retornoRecibo.Retorno.versao
                    };
                    FuncoesFTP.GuardaXML(proc.ObterXmlString(), _path + @"\NFe_Autorizada\", vendanfea.Chave);
                    string localxml = FuncoesFTP.SubirArquivo(local, _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml", "nfe");

                    Notasemitidas notaemitida = new Notasemitidas()
                    {
                        Chave = _nfe.infNFe.Id,
                        Modelo = (int)_nfe.infNFe.ide.mod,
                        Cnpj = _configuracoes.Emitente.CNPJ,
                        Data = DateTime.Now,
                        Valor = vendanfe.Vnf,
                        Localxml = localxml,
                    };
                    notaemitida.Insert();
                }
                else
                {
                    vendanfea.Statusnfe = "REJEITADO";
                    vendanfea.Update();
                }
                return retornoRecibo.RetornoStr;
                #endregion
            }
            catch (ComunicacaoException ex)
            {
                Funcoes.Mensagem(ex.Message + Environment.NewLine +
                    " Tente novamente, se o erro continuar:  " + Environment.NewLine + Environment.NewLine +
                    " Por gentileza, " + Environment.NewLine +
                     "verifique a disponibilidade no portal www.nfe.fazenda.gov.br", "RETORNO SEFAZ ", MessageBoxButton.OK);
                Funcoes.Crashe(ex, "ERRO DA SEFAZ - RECIBO - CD:ComunicacaoException", false);
                return null;
            }
            catch (ValidacaoSchemaException ex)
            {
                Funcoes.Mensagem(ex.Message, "ERRO DA SEFAZ - RECIBO  - CD:ValidacaoSchemaException", MessageBoxButton.OK);
                Funcoes.Crashe(ex, "ERRO DA SEFAZ - RECIBO - CD:ValidacaoSchemaException", false);
                return null;
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ERRO DA SEFAZ - RECIBO - CD:Exception");
                return null;
            }

        }

        public string Autoriza(VendaNFe vnfe, Vendanfea vnfea)
        {
            vendanfe = vnfe;
            vendanfea = vnfea;

            string retorno = "";
            NFe.Classes.NFe _nfe;
            try
            {
                //#region Cria e Envia NFe
                CarregarConfiguracao();

                _nfe = GetNf(vendanfe.Nrvenda, ModeloDocumento.NFe, _configuracoes.CfgServico.VersaoNFeAutorizacao);
                _nfe.Assina();
                try
                {
                    var xmlPreview = _nfe.ObterXmlString();
                    var previewPath = System.IO.Path.Combine(_path, "NFe_Preview");
                    if (!Directory.Exists(previewPath)) Directory.CreateDirectory(previewPath);
                    var file = System.IO.Path.Combine(previewPath, _nfe.infNFe.Id.Replace("NFe", "") + "-preview.xml");
                    File.WriteAllText(file, xmlPreview);
                }
                catch
                {
                     /* apenas não interromper o fluxo se falhar o preview */
                     Console.WriteLine("Erro ao salvar o preview da NFe");
                }
                _nfe.Valida();

                servicoNFe = new ServicosNFe(_configuracoes.CfgServico);
                vendanfea.Chave = _nfe.infNFe.Id.Replace("NFe", "");
                vendanfe.Chavedeacesso = _nfe.infNFe.Id.Replace("NFe", "");
                
                if (_configuracoes.CfgServico.SalvarXmlServicos)
                    FuncoesFTP.GuardaXML(_nfe.ObterXmlString(), _path + @"\", "VERIFICAR-" + _nfe.infNFe.Id + "-procNFe");

                if (App.Parametro.Uf == "MT" || App.Parametro.Uf == "MS")
                {
                    _nfe = new NFe.Classes.NFe().CarregarDeXmlString(Funcoes.RemoverAcentos(_nfe.ObterXmlString()));
                    _nfe.Assina();
                }

                if (_configuracoes.CfgServico.SalvarXmlServicos)
                    FuncoesFTP.GuardaXML(_nfe.ObterXmlString(), _path + @"\", "VERIFICAR-" + _nfe.infNFe.Id + "-procNFe");

                RetornoNFeAutorizacao retornoEnvio;
               // if (App.Parametro.Uf == "BA")
               //     retornoEnvio = servicoNFe.NFeAutorizacao(1, IndicadorSincronizacao.Assincrono, new List<NFe.Classes.NFe> { _nfe }, false /*Envia a mensagem compactada para a SEFAZ*/);
               // else
                retornoEnvio = servicoNFe.NFeAutorizacao(1, IndicadorSincronizacao.Sincrono, new List<NFe.Classes.NFe> { _nfe }, false /*Envia a mensagem compactada para a SEFAZ*/);

                string Motivo = retornoEnvio.Retorno.protNFe != null ? retornoEnvio.Retorno.protNFe.infProt.xMotivo : retornoEnvio.Retorno.xMotivo;
                int valorStat = retornoEnvio.Retorno.protNFe != null ? retornoEnvio.Retorno.protNFe.infProt.cStat : retornoEnvio.Retorno.cStat;

                if (valorStat == 100)
                {
                    vendanfea.Statusnfe = "APROVADO";
                    vendanfea.Chave = _nfe.infNFe.Id.Replace("NFe", "");
                    vendanfea.Nprotocolo = retornoEnvio.Retorno.protNFe.infProt.nProt;
                    vendanfea.Danfe = _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.pdf";
                    vendanfea.Xml = _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml";
                    vendanfea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                    vendanfea.Update();
                    var proc = new nfeProc
                    {
                        NFe = _nfe,
                        protNFe = retornoEnvio.Retorno.protNFe,
                        versao = retornoEnvio.Retorno.versao
                    };

                    FuncoesFTP.GuardaXML(proc.ObterXmlString(), _path + @"\NFe_Autorizada\", _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe");

                    string local = _path + @"\NFe_Autorizada\" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml";
                    string localxml = FuncoesFTP.SubirArquivo(local, _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml", "nfe");

                    GerarPdf(vendanfe, vendanfea);

                    Notasemitidas notaemitida = new Notasemitidas()
                    {
                        Chave = _nfe.infNFe.Id,
                        Modelo = (int)_nfe.infNFe.ide.mod,
                        Cnpj = _configuracoes.Emitente.CNPJ,
                        Data = DateTime.Now,
                        Valor = vendanfe.Vnf,
                        Localxml = localxml,
                    };
                    notaemitida.Insert();

                    if (!string.IsNullOrEmpty(_configuracoes.ConfiguracaoDanfeNfe.EmailAut))
                        if (_configuracoes.ConfiguracaoDanfeNfe.EmailAut == "SIM")
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(_nfe.infNFe.dest.email))
                                    Email(vendanfe, vendanfea, _nfe.infNFe.dest.email);
                                else
                                    MinhaNotificacao.NotificarErro("Nota sem e-mail de destinatario.", "E-MAIL");
                            }
                            catch
                            {

                            }
                        }
                }
                else if (valorStat == 110 ||
                    valorStat == 301 ||
                    valorStat == 302 ||
                    valorStat == 303)
                {
                    vendanfea.Statusnfe = "DENEGADO";
                    vendanfea.Chave = _nfe.infNFe.Id.Replace("NFe", "");
                    vendanfea.Nprotocolo = retornoEnvio.Retorno.protNFe.infProt.nProt;
                    vendanfea.Danfe = _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.pdf";
                    vendanfea.Xml = _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml";
                    vendanfea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                    vendanfea.Update();



                    var proc = new nfeProc
                    {
                        NFe = _nfe,
                        protNFe = retornoEnvio.Retorno.protNFe,
                        versao = retornoEnvio.Retorno.versao
                    };

                    FuncoesFTP.GuardaXML(proc.ObterXmlString(), _path + @"\NF_Autorizada\", _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe");

                    string local = _path + @"\NF_Autorizada\" + vendanfea.Chave.Replace("NFe", "") + "-procNFe.xml";
                    string localsave = _path + @"\NF_Autorizada\" + vendanfea.Chave.Replace("NFe", "") + "-procNFe.pdf";

                    GerarPdf(vendanfe, vendanfea);

                    string localxml = FuncoesFTP.SubirArquivo(local, _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml", "nfe");

                    Notasemitidas notaemitida = new Notasemitidas()
                    {
                        Chave = _nfe.infNFe.Id.Replace("NFe", ""),
                        Modelo = (int)_nfe.infNFe.ide.mod,
                        Cnpj = _configuracoes.Emitente.CNPJ,
                        Data = DateTime.Now,
                        Valor = vendanfe.Vnf,
                        Localxml = localxml,
                    };
                    notaemitida.Insert();

                }
                else if (valorStat == 103 || valorStat == 104 || valorStat == 105)
                {
                    vendanfea.Statusnfe = "AGUARDANDO";
                    vendanfea.Chave = _nfe.infNFe.Id.Replace("NFe", "");
                    vendanfea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                    vendanfea.Update();
                    MinhaNotificacao.NotificarAviso("SEFAZ INFORMA", "Lote recebido com sucesso");

                    if (retornoEnvio.Retorno.infRec != null)
                        if (!string.IsNullOrWhiteSpace(retornoEnvio.Retorno.infRec.nRec))
                            retorno = ConsultaRecibo(vendanfe, vendanfea, retornoEnvio.Retorno.infRec.nRec);
                }
                else if (valorStat == 204)
                {
                    vendanfea.Statusnfe = "REJEITADO";
                    vendanfea.Chave = _nfe.infNFe.Id.Replace("NFe", "");
                    vendanfea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                    vendanfea.Update();
                    MinhaNotificacao.NotificarAviso("SEFAZ INFORMA", "Duplicidade de NF-e");

                    if (retornoEnvio.Retorno.infRec != null)
                        if (!string.IsNullOrWhiteSpace(retornoEnvio.Retorno.infRec.nRec))
                            retorno = ConsultaRecibo(vendanfe, vendanfea, retornoEnvio.Retorno.infRec.nRec);
                }
                else
                {
                    vendanfea.Statusnfe = "REJEITADO";
                    vendanfea.Chave = _nfe.infNFe.Id.Replace("NFe", "");
                    vendanfea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                    vendanfea.Update();
                }
                if (retorno == "") retorno = retornoEnvio.RetornoStr;
                return retorno;
            }
            catch (ComunicacaoException ex)
            {
                //Faça o tratamento de contingência OffLine aqui.
                //Funcoes.Crashe(ex, "", false);
                Funcoes.Mensagem(ex.Message + Environment.NewLine + Environment.NewLine + Environment.NewLine +
                     " Por gentileza, " + Environment.NewLine +
                     "verifique a disponibilidade no portal www.nfe.fazenda.gov.br", "RETORNO SEFAZ ", MessageBoxButton.OK);
                return null;
            }
            catch (ValidacaoSchemaException ex)
            {
                vendanfea.Statusnfe = "REJEITADO";
                vendanfea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                vendanfea.Update();

                Funcoes.Mensagem(ex.Message + Environment.NewLine +
                                    " Por gentileza, verificar as informações apresentada.", "RETORNO SEFAZ de XML ", MessageBoxButton.OK);
                Funcoes.Crashe(ex, "", false);

                return null;
            }
            catch (Exception ex)
            {
                vendanfea.Statusnfe = "PENDENTE";
                vendanfea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                vendanfea.Update();

                Funcoes.Crashe(ex, "", false);
                if (ex.InnerException != null)
                    MessageBox.Show(ex.Message + " Outros: " + ex.InnerException.Message, "Erro");
                else if (!string.IsNullOrEmpty(ex.Message))
                {
                    Funcoes.Crashe(ex, "", false);
                    if (ex.Message.Contains("A solicitação foi anulada: Não foi possível criar um canal seguro para SSL/TLS."))
                    {
                        MessageBox.Show("Por gentileza, verifica a conexão com seu CERTIFICADO. " + Environment.NewLine +
                            "Dica: pode ser que não esteja contra conectado ou esteja vencido. " + Environment.NewLine + Environment.NewLine +
                            ex.Message, "Erro do CERTIFICADO - CODE: SSL/TLS");
                    }
                    else
                        MessageBox.Show(ex.Message, "Erro - CODE: Autoriza");
                }
                return null;
            }
        }
        public string Cancelar(VendaNFe vnfe, Vendanfea vnfea, string justificativa)
        {
            try
            {
                #region Cancelar NFe
                vendanfe = vnfe;
                vendanfea = vnfea;

                string protocolo = vendanfea.Nprotocolo.Trim();

                CarregarConfiguracao();

                var servicoNFe = new ServicosNFe(_configuracoes.CfgServico);

                var chave = vendanfea.Chave.Replace("NFe", "");

                if (protocolo == "")
                {
                    RetornoNfeConsultaProtocolo retornoRecibo = servicoNFe.NfeConsultaProtocolo(chave);
                    protocolo = retornoRecibo.Retorno.protNFe.infProt.nProt;
                }


                if (string.IsNullOrEmpty(justificativa)) throw new Exception("A justificativa deve ser informada!");
                if (string.IsNullOrEmpty(protocolo)) throw new Exception("A protocolo deve ser informada!");

                var cpfcnpj = string.IsNullOrEmpty(_configuracoes.Emitente.CNPJ)
                    ? _configuracoes.Emitente.CPF
                    : _configuracoes.Emitente.CNPJ;

                RetornoRecepcaoEvento retornoCancelamento = servicoNFe.RecepcaoEventoCancelamento(Convert.ToInt32(1), Convert.ToInt16(1), protocolo, chave, justificativa, cpfcnpj);

                string Motivo = retornoCancelamento.Retorno.retEvento.First().infEvento.xMotivo ?? retornoCancelamento.Retorno.retEvento.First().infEvento.xMotivo;
                int valorStat = retornoCancelamento.Retorno.retEvento.First().infEvento == null ? retornoCancelamento.Retorno.cStat : retornoCancelamento.Retorno.retEvento.First().infEvento.cStat;


                if (valorStat == 100 || valorStat == 105 || valorStat == 135 || valorStat == 155)
                {
                    vendanfea.Statusnfe = "CANCELADO";
                    vendanfea.Nprotocolo = retornoCancelamento.Retorno.retEvento.First().infEvento.nProt;
                    vendanfea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                    vendanfea.Update();

                    FuncoesFTP.GuardaXML(retornoCancelamento.ProcEventosNFe.First().ObterXmlString(), _path + @"\NFe_Cancelada\", vendanfea.Chave + "-nProt" + vendanfea.Nprotocolo);
                    string local = _path + @"\NFe_Cancelada\" + vendanfea.Chave + "-nProt" + vendanfea.Nprotocolo + ".xml";

                    string localxml = FuncoesFTP.SubirArquivo(local, vendanfea.Chave + "-nProt" + vendanfea.Nprotocolo + ".xml", "nfe");
                    string localnfe = _path + @"\NFe_Autorizada\" + vendanfea.Chave + "-procNFe.xml";
                    string localcancelada = _path + @"\NFe_Cancelada\" + vendanfea.Chave + "-nProt" + vendanfea.Nprotocolo + ".xml";
                    string localsave = _path + @"\NFe_Cancelada\" + vendanfea.Chave + "-nProt" + vendanfea.Nprotocolo + ".pdf";

                    var proc = new nfeProc().CarregarDeArquivoXml(localnfe);
                    procEventoNFe procEvento = FuncoesXml.ArquivoXmlParaClasse<procEventoNFe>(localcancelada);

                    var danfe = new DanfeFrEvento(proc, procEvento,
                        new ConfiguracaoDanfeNfe(_configuracoes.ConfiguracaoDanfeNfe.Logomarca,
                        false,
                        false),
                        "CIAF SOLUÇÕES EM SOFTWARE");

                    danfe.ExportarPdf(localsave);


                }

                return retornoCancelamento.RetornoStr;
                #endregion
            }
            catch (ComunicacaoException ex)
            {
                Funcoes.Crashe(ex, "", false);
                Funcoes.Mensagem(ex.Message + Environment.NewLine + Environment.NewLine + Environment.NewLine +
                    " Por gentileza, " + Environment.NewLine +
                     "verifique a disponibilidade no portal www.nfe.fazenda.gov.br", "RETORNO SEFAZ ", MessageBoxButton.OK);

                return null;
            }
            catch (ValidacaoSchemaException ex)
            {
                Funcoes.Mensagem(ex.Message, "Erro ValidacaoSchemaException", MessageBoxButton.OK);
                return null;
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ERRO DA SEFAZ");
                return null;
            }
        }

        public string Inutilizar(VendaNFe vnfe, Vendanfea vnfea, string justificativa)
        {
            try
            {
                #region Inutiliza Numeração

                vendanfe = vnfe;
                vendanfea = vnfea;

                CarregarConfiguracao();

                string ano = DateTime.Now.ToString("yy");

                if (string.IsNullOrEmpty(justificativa)) throw new Exception("A Justificativa deve ser informada!");

                var servicoNFe = new ServicosNFe(_configuracoes.CfgServico);
                RetornoNfeInutilizacao retornoConsulta = servicoNFe.NfeInutilizacao(_configuracoes.Emitente.CNPJ, Convert.ToInt16(ano),
                    _configuracoes.CfgServico.ModeloDocumento, Convert.ToInt16(vendanfe.Serie), Convert.ToInt32(vendanfe.Nrvenda),
                    Convert.ToInt32(vendanfe.Nrvenda), justificativa);

                string Motivo = retornoConsulta.Retorno.infInut.xMotivo ?? retornoConsulta.Retorno.infInut.xMotivo;
                int valorStat = retornoConsulta.Retorno.infInut.xMotivo == null ? retornoConsulta.Retorno.infInut.cStat : retornoConsulta.Retorno.infInut.cStat;

                if (retornoConsulta.Retorno.infInut.cStat == 102)
                {
                    inutNFe inutNFe = FuncoesXml.XmlStringParaClasse<inutNFe>(retornoConsulta.EnvioStr);

                    procInutNFe pinutnf = new procInutNFe
                    {
                        versao = retornoConsulta.Retorno.versao,
                        inutNFe = inutNFe,
                        retInutNFe = retornoConsulta.Retorno
                    };

                    vendanfea.Statusnfe = "INUTILIZADO";
                    vendanfea.Chave = pinutnf.inutNFe.infInut.Id;
                    vendanfea.Nprotocolo = retornoConsulta.Retorno.infInut.nProt;
                    vendanfea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                    vendanfea.Update();

                    FuncoesFTP.GuardaXML(FuncoesXml.ClasseParaXmlString<procInutNFe>(pinutnf), _path + @"\NFe_Inutilizada\", "" + pinutnf.inutNFe.infInut.Id + "-nProt" + vendanfea.Nprotocolo);

                    string local = _path + @"\NFe_Inutilizada\" + pinutnf.inutNFe.infInut.Id + "-nProt" + vendanfea.Nprotocolo + ".xml";

                    FuncoesFTP.SubirArquivo(local, vendanfea.Nrvenda + "-nProt" + vendanfea.Nprotocolo + ".xml", "nfe");

                }
                else
                {
                    vendanfea.Statusnfe = "REJEITADO";
                    vendanfea.Update();
                }

                return retornoConsulta.RetornoStr;
                #endregion
            }
            catch (ComunicacaoException ex)
            {
                Funcoes.Crashe(ex, "ERRO DE COMUNICAÇÃO", false);
                Funcoes.Mensagem(ex.Message + Environment.NewLine + Environment.NewLine + Environment.NewLine +
                    " Por gentileza, " + Environment.NewLine +
                     "verifique a disponibilidade no portal www.nfe.fazenda.gov.br", "RETORNO COMUNICAÇÃO SEFAZ ", MessageBoxButton.OK);

                return null;
            }
            catch (ValidacaoSchemaException ex)
            {
                Funcoes.Mensagem(ex.Message, "Erro", MessageBoxButton.OK);
                return null;
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ERRO DA SEFAZ");
                return null;
            }
        }

        public void TransmitirCarta(Cartanfe carta)
        {
            try
            {
                #region Inutiliza Numeração
                CarregarConfiguracao();

                // if (string.IsNullOrEmpty(justificativa)) throw new Exception("A Justificativa deve ser informada!");

                var servicoNFe = new ServicosNFe(_configuracoes.CfgServico);
                var cpfcnpj = string.IsNullOrEmpty(_configuracoes.Emitente.CNPJ)
                    ? _configuracoes.Emitente.CPF
                    : _configuracoes.Emitente.CNPJ;


                if (App.Parametro.Uf == "MT")
                {
                    carta.Texto = Funcoes.RemoverAcentos(carta.Texto.Trim());
                }

                RetornoRecepcaoEvento retornoCartaCorrecao = servicoNFe.RecepcaoEventoCartaCorrecao(Convert.ToInt32(1),
                  Convert.ToInt16(carta.Nrseq), carta.Chavea, carta.Texto, carta.Cnpjcpf);

                string Motivo = retornoCartaCorrecao.Retorno.retEvento.First().infEvento.xMotivo ?? retornoCartaCorrecao.Retorno.retEvento.First().infEvento.xMotivo;
                int valorStat = retornoCartaCorrecao.Retorno.retEvento.First().infEvento == null ? retornoCartaCorrecao.Retorno.cStat : retornoCartaCorrecao.Retorno.retEvento.First().infEvento.cStat;

                if (retornoCartaCorrecao.Retorno.cStat == 102 || valorStat == 135)
                {

                    MessageBox.Show("Código:" + valorStat +
                       "\nMotivo: " + Motivo, "Retorno da Sefaz!!!! ", MessageBoxButton.OK, MessageBoxImage.Information);

                    carta.Status = "TRANSMITIDA";
                    carta.Protocolo = retornoCartaCorrecao.Retorno.retEvento.First().infEvento.nProt;
                    carta.Update();

                    FuncoesFTP.GuardaXML(retornoCartaCorrecao.ProcEventosNFe.First().ObterXmlString(), _path + @"\NFe_CCe\", "CCE" + carta.Nrcarta.ToString());
                    FuncoesFTP.GuardaXML(retornoCartaCorrecao.ProcEventosNFe.First().ObterXmlString(), _path + @"\NFe_CCe\", carta.Chavea + "-nProt" + carta.Protocolo);

                    string localnfe = _path + @"\NFe_Autorizada\" + carta.Chavea + "-procNFe.xml";
                    string localcarta = _path + @"\NFe_CCe\" + carta.Chavea + "-nProt" + carta.Protocolo + ".xml";
                    string localsave = _path + @"\NFe_CCe\" + carta.Chavea + "-nProt" + carta.Protocolo + ".pdf";

                    string local = _path + @"\NFe_CCe\" + carta.Chavea + "-nProt" + carta.Protocolo + ".xml";
                    FuncoesFTP.SubirArquivo(local, carta.Chavea + "-nProt" + carta.Protocolo + ".xml", "nfe");

                    var proc = new nfeProc().CarregarDeArquivoXml(localnfe);
                    var procEvento = FuncoesXml.ArquivoXmlParaClasse<procEventoNFe>(localcarta);

                    var danfe = new DanfeFrEvento(proc, procEvento,
                        new ConfiguracaoDanfeNfe(_configuracoes.ConfiguracaoDanfeNfe.Logomarca,
                         false,
                        false),
                        "CIAF - SOLUÇÕES EM SOFTWARE");

                    danfe.ExportarPdf(localsave);
                    if (_configuracoes.ConfiguracaoDanfeNfe.SalvarServidor)
                    {
                        FuncoesFTP.SaveNota(localcarta);
                        FuncoesFTP.SaveNota(localsave);
                    }


                    FuncoesFTP.SubirArquivo(_path + @"\NFe_CCe\" + carta.Chavea + "-nProt" + carta.Protocolo + ".pdf", carta.Chavea + "-nProt" + carta.Protocolo + ".xml", "nfe");
                    if (_configuracoes.CfgServico.DiretorioSalvarXml.Replace(" ", "") != "" && _configuracoes.CfgServico.DiretorioSalvarXml != null)
                    {
                        try
                        {
                            danfe.ExportarPdf(_configuracoes.CfgServico.DiretorioSalvarXml + @"\" + carta.Chavea + "-nProt" + carta.Protocolo + ".pdf");
                        }
                        catch
                        { }
                    }
                    System.Diagnostics.Process.Start(localsave);
                }
                else
                {
                    if (MessageBox.Show("Código: " + valorStat +
                      "\nMotivo: " + Motivo +
                      ". \n\nDeseja verificar o motivo na internet?", "Retorno da Sefaz!!!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        string url = @"http://www.google.com.br/search?hl=pt-BR&source=hp&q=" + valorStat + " " + Motivo;
                        System.Diagnostics.Process.Start(url);
                    }
                }


                #endregion
            }
            catch (ComunicacaoException ex)
            {
                Funcoes.Crashe(ex, "ERRO DE COMUNICAÇÃO", false);
                Funcoes.Mensagem(ex.Message + Environment.NewLine + Environment.NewLine + Environment.NewLine +
                    " Por gentileza, " + Environment.NewLine +
                     "verifique a disponibilidade no portal www.nfe.fazenda.gov.br", "RETORNO COMUNICAÇÃO SEFAZ ", MessageBoxButton.OK);

            }
            catch (ValidacaoSchemaException ex)
            {
                Funcoes.Mensagem(ex.Message, "Erro", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ERRO DA SEFAZ");
            }
        }

        public void VerificarCarta(Cartanfe carta)
        {
            try
            {
                CarregarConfiguracao();

                if (string.IsNullOrEmpty(carta.Chavea)) throw new Exception("A Chave deve ser informada!");

                var servicoNFe = new ServicosNFe(_configuracoes.CfgServico);
                RetornoNfeConsultaProtocolo retornoRecibo = servicoNFe.NfeConsultaProtocolo(carta.Chavea);

                procEventoNFe evento = retornoRecibo.Retorno.procEventoNFe.SingleOrDefault(p => p.evento.infEvento.chNFe == carta.Chavea
                                                                    && p.evento.infEvento.tpEvento == NFeTipoEvento.TeNfeCartaCorrecao
                                                                    && p.evento.infEvento.nSeqEvento == carta.Nrseq);

                if (evento != null)
                {
                    string Motivo = evento.retEvento.infEvento.xMotivo ?? retornoRecibo.Retorno.xMotivo;
                    int valorStat = evento.retEvento.infEvento.xMotivo != null ? evento.retEvento.infEvento.cStat : retornoRecibo.Retorno.cStat;

                    MessageBox.Show("Código:" + valorStat +
                       "\nMotivo: " + Motivo, "Retorno da Sefaz!!!! ", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (carta.Status != "TRANSMITIDA")
                    {
                        carta.Protocolo = evento.retEvento.infEvento.nProt;
                        carta.Update();
                        carta.Status = "TRANSMITIDA";
                        carta.Update();

                        FuncoesFTP.GuardaXML(evento.ObterXmlString(), _path + @"\NFe_CCe\", "CCE" + carta.Nrcarta.ToString());
                        FuncoesFTP.GuardaXML(evento.ObterXmlString(), _path + @"\NFe_CCe\", carta.Chavea + "-nProt" + carta.Protocolo);
                        FuncoesFTP.SubirArquivo(_path + @"\NFe_CCe\" + carta.Chavea + "-nProt" + carta.Protocolo + ".xml", carta.Chavea + "-nProt" + carta.Protocolo + ".xml", "nfe");

                        string localnfe = _path + @"\NFe_Autorizada\" + carta.Chavea + "-procNFe.xml";
                        string localcarta = _path + @"\NFe_CCe\" + carta.Chavea + "-nProt" + carta.Protocolo + ".xml";
                        string localsave = _path + @"\NFe_CCe\" + carta.Chavea + "-nProt" + carta.Protocolo + ".pdf";


                        var proc = new nfeProc().CarregarDeArquivoXml(localnfe);
                        var procEvento = FuncoesXml.ArquivoXmlParaClasse<procEventoNFe>(localcarta);



                        var danfe = new DanfeFrEvento(proc, procEvento,
                            new ConfiguracaoDanfeNfe(_configuracoes.ConfiguracaoDanfeNfe.Logomarca,
                             false,
                            false),
                            "CIAF - SOLUÇÕES EM SOFTWARE");
                        //danfe.Visualizar();
                        //danfe.Imprimir();
                        //danfe.ExibirDesign();
                        danfe.ExportarPdf(localsave);

                        if (_configuracoes.CfgServico.DiretorioSalvarXml.Replace(" ", "") != "" && _configuracoes.CfgServico.DiretorioSalvarXml != null)
                        {
                            try
                            {
                                danfe.ExportarPdf(_configuracoes.CfgServico.DiretorioSalvarXml + @"\" + carta.Chavea + "-nProt" + carta.Protocolo + ".pdf");
                            }
                            catch
                            { }
                        }
                        System.Diagnostics.Process.Start(localsave);
                    }

                }
                else
                {
                    MessageBox.Show("Carta não encontrado pelo NF-e Consulta Protocolo." +
                    "\nEm caso de dúvidas sobre sua NF-e, consulte no site da SEFAZ em seu estado. ", "Retorno da Sefaz!!!! ", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (ComunicacaoException ex)
            {
                Funcoes.Crashe(ex, "ERRO DE COMUNICAÇÃO", false);
                Funcoes.Mensagem(ex.Message + Environment.NewLine + Environment.NewLine + Environment.NewLine +
                  " Por gentileza, " + Environment.NewLine +
                   "verifique a disponibilidade no portal www.nfe.fazenda.gov.br", "RETORNO COMUNICAÇÃO SEFAZ ", MessageBoxButton.OK);

            }
            catch (ValidacaoSchemaException ex)
            {
                Funcoes.Crashe(ex, "", false);
                Funcoes.Mensagem(ex.Message, "Erro - Code: ValidacaoSchemaException", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ERRO DA SEFAZ");
            }
        }

        public void GerarPdf(VendaNFe vnfe, Vendanfea vnfea)
        {
            try
            {
                vendanfe = vnfe;
                vendanfea = vnfea;

                bool flagcancelada;
                if (vendanfea.Statusnfe == "CANCELADO") flagcancelada = true; else flagcancelada = false;
                CarregarConfiguracao();
                string local = _path + @"\NFe_Autorizada\" + vendanfea.Chave + "-procNFe.xml";
                string localsave = _path + @"\NFe_Autorizada\" + vendanfea.Chave + "-procNFe.pdf";
                string localcalcelada = _path + @"\NFe_Cancelada\" + vendanfea.Chave + "-nProt" + vendanfea.Nprotocolo + ".xml";
                //System.Diagnostics.Process.Start(local);

                nfeProc proc = null;
                try
                {
                    if (!flagcancelada)
                        proc = new nfeProc().CarregarDeArquivoXml(local);
                    else
                    {
                        proc = new nfeProc().CarregarDeArquivoXml(local);
                        procEventoNFe procEvento = FuncoesXml.ArquivoXmlParaClasse<procEventoNFe>(localcalcelada);
                        proc.protNFe.infProt.nProt = procEvento.retEvento.infEvento.nProt;
                        proc.protNFe.infProt.dhRecbto = procEvento.retEvento.infEvento.dhRegEvento;
                    }

                }
                catch
                {
                    var numero = vendanfe.Nrvenda;
                    NFe.Classes.NFe _nfe;
                    _nfe = GetNf(numero, _configuracoes.CfgServico.ModeloDocumento, _configuracoes.CfgServico.VersaoNFeAutorizacao);
                    /// _nfe.Assina(); //não precisa validar aqui, pois o lote será validado em ServicosNFe.NFeAutorizacao
                    var dadosChave = ChaveFiscal.ObterChave(_nfe.infNFe.ide.cUF, _nfe.infNFe.ide.dhEmi.LocalDateTime, _nfe.infNFe.emit.CNPJ, _nfe.infNFe.ide.mod, _nfe.infNFe.ide.serie, _nfe.infNFe.ide.nNF, (int)_nfe.infNFe.ide.tpEmis, int.Parse(_nfe.infNFe.ide.cNF));
                    _nfe.infNFe.Id = "NFe" + dadosChave.Chave;
                    FuncoesFTP.GuardaXML(_nfe.ObterXmlString(), _path + @"\NFe_Autorizada\", _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe");
                    local = _path + @"\NFe_Autorizada\" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml";
                    localsave = _path + @"\NFe_Autorizada\" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.pdf";
                    proc = new nfeProc() { NFe = new NFe.Classes.NFe().CarregarDeArquivoXml(local), protNFe = new NFe.Classes.Protocolo.protNFe() };
                    vendanfea.Chave = _nfe.infNFe.Id.Replace("NFe", "");

                }

                bool parametrotributos = false;
                if (App.Parametro.Calculatrib == "SIM") parametrotributos = true;

                var danfe = new DanfeFrNfe(proc: proc,
                                   configuracaoDanfeNfe: new ConfiguracaoDanfeNfe()
                                   {
                                       Logomarca = _configuracoes.ConfiguracaoDanfeNfe.Logomarca,
                                       DuasLinhas = true,
                                       DocumentoCancelado = flagcancelada,
                                       QuebrarLinhasObservacao = true,
                                       ExibirResumoCanhoto = true,
                                       ResumoCanhoto = string.Empty,
                                       ChaveContingencia = string.Empty,
                                       ExibeCampoFatura = false,
                                       ImprimirISSQN = true,
                                       ImprimirDescPorc = false,
                                       ImprimirTotalLiquido = false,
                                       ImprimirUnidQtdeValor = ImprimirUnidQtdeValor.Comercial,
                                       ExibirTotalTributos = parametrotributos,
                                   },
                                   desenvolvedor: "EMITIDA PELO SISTEMA CIAF",
                                   arquivoRelatorio: string.Empty);
                danfe.ExportarPdf(localsave);
                if (_configuracoes.ConfiguracaoDanfeNfe.SalvarServidor)
                {
                    FuncoesFTP.SaveNota(local);
                    FuncoesFTP.SaveNota(localsave);
                }


                if (_configuracoes.CfgServico.DiretorioSalvarXml.Replace(" ", "") != "" && _configuracoes.CfgServico.DiretorioSalvarXml != null)
                {
                    try
                    {
                        danfe.ExportarPdf(_configuracoes.CfgServico.DiretorioSalvarXml + @"\" + vendanfea.Chave + "-procNFe.pdf");
                    }
                    catch
                    { }
                }
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                    Funcoes.Mensagem(ex.ToString(), "Erro - COD:GerarPdf ", MessageBoxButton.OK);
            }
        }

        public void ImprimirDireto(VendaNFe vnfe, Vendanfea vnfea)
        {
            try
            {
                vendanfe = vnfe;
                vendanfea = vnfea;

                bool flagcancelada;
                if (vendanfea.Statusnfe == "CANCELADO") flagcancelada = true; else flagcancelada = false;

                CarregarConfiguracao();

                string local = _path + @"\NFe_Autorizada\" + vendanfea.Chave + "-procNFe.xml";
                string localsave = _path + @"\NFe_Autorizada\" + vendanfea.Chave + "-procNFe.pdf";
                string localcalcelada = _path + @"\NFe_Cancelada\" + vendanfea.Chave + "-nProt" + vendanfea.Nprotocolo + ".xml";

                nfeProc proc = null;
                try
                {
                    if (!flagcancelada)
                        proc = new nfeProc().CarregarDeArquivoXml(local);
                    else
                    {
                        proc = new nfeProc().CarregarDeArquivoXml(local);
                        procEventoNFe procEvento = FuncoesXml.ArquivoXmlParaClasse<procEventoNFe>(localcalcelada);
                        proc.protNFe.infProt.nProt = procEvento.retEvento.infEvento.nProt;
                        proc.protNFe.infProt.dhRecbto = procEvento.retEvento.infEvento.dhRegEvento;
                    }

                }
                catch
                {
                    var numero = vendanfe.Nrvenda;
                    NFe.Classes.NFe _nfe;
                    _nfe = GetNf(numero, _configuracoes.CfgServico.ModeloDocumento, _configuracoes.CfgServico.VersaoNFeAutorizacao);
                    /// _nfe.Assina(); //não precisa validar aqui, pois o lote será validado em ServicosNFe.NFeAutorizacao
                    var dadosChave = ChaveFiscal.ObterChave(_nfe.infNFe.ide.cUF, _nfe.infNFe.ide.dhEmi.LocalDateTime, _nfe.infNFe.emit.CNPJ, _nfe.infNFe.ide.mod, _nfe.infNFe.ide.serie, _nfe.infNFe.ide.nNF, (int)_nfe.infNFe.ide.tpEmis, int.Parse(_nfe.infNFe.ide.cNF));
                    _nfe.infNFe.Id = "NFe" + dadosChave.Chave;
                    FuncoesFTP.GuardaXML(_nfe.ObterXmlString(), _path + @"\NFe_Autorizada\", _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe");
                    local = _path + @"\NFe_Autorizada\" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml";
                    localsave = _path + @"\NFe_Autorizada\" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.pdf";
                    proc = new nfeProc() { NFe = new NFe.Classes.NFe().CarregarDeArquivoXml(local), protNFe = new NFe.Classes.Protocolo.protNFe() };
                    vendanfea.Chave = _nfe.infNFe.Id.Replace("NFe", "");

                }

                bool parametrotributos = false;
                if (App.Parametro.Calculatrib == "SIM") parametrotributos = true;
                var danfe = new DanfeFrNfe(proc: proc,
                                   configuracaoDanfeNfe: new ConfiguracaoDanfeNfe()
                                   {
                                       Logomarca = _configuracoes.ConfiguracaoDanfeNfe.Logomarca,
                                       DuasLinhas = true,
                                       DocumentoCancelado = flagcancelada,
                                       QuebrarLinhasObservacao = true,
                                       ExibirResumoCanhoto = true,
                                       ResumoCanhoto = string.Empty,
                                       ChaveContingencia = string.Empty,
                                       ExibeCampoFatura = false,
                                       ImprimirISSQN = true,
                                       ImprimirDescPorc = false,
                                       ImprimirTotalLiquido = false,
                                       ImprimirUnidQtdeValor = ImprimirUnidQtdeValor.Comercial,
                                       ExibirTotalTributos = parametrotributos,
                                   },
                                   desenvolvedor: "EMITIDA PELO SISTEMA CIAF",
                                   arquivoRelatorio: string.Empty);
                danfe.Imprimir();
                if (_configuracoes.CfgServico.DiretorioSalvarXml.Replace(" ", "") != "" && _configuracoes.CfgServico.DiretorioSalvarXml != null)
                {
                    try
                    {
                        danfe.ExportarPdf(_configuracoes.CfgServico.DiretorioSalvarXml + @"\" + vendanfea.Chave + "-procNFe.pdf");
                    }
                    catch
                    { }
                }
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                    Funcoes.Mensagem(ex.ToString(), "ERRO - COD:ImprimirDireto", MessageBoxButton.OK);
            }
        }

        public static void Email(VendaNFe vnfe, Vendanfea vnfea, string copia = "")
        {
            try
            {
                if (_configuracoes.ConfiguracaoEmail.ServidorSmtp == "smtp.dominio.com")
                {
                    App.Parametro.Confemail = "CIAF";
                    App.Parametro.Email = _configuracoes.ConfiguracaoEmail.Email;
                }
                if (App.Parametro.Confemail == "CIAF" || _configuracoes.ConfiguracaoEmail.ServidorSmtp == "")
                {

                    MandarEmail.NovoEmailNFeCiaf(vnfe, vnfea, copia);
                }
                else
                {
                    CarregarConfiguracao();
                    string arquivoXml = _path + @"\NFe_Autorizada\" + vnfea.Chave + "-procNFe.xml";

                    if (!File.Exists(arquivoXml))
                        throw new ArgumentException("Não foi possivel localizar o XML.");

                    string arquivoPdf = _path + @"\NFe_Autorizada\" + vnfea.Chave + "-procNFe.pdf";

                    if (!File.Exists(arquivoPdf))
                        throw new ArgumentException("Não foi possivel localizar o PDF.");

                    string mensagem = _configuracoes.ConfiguracaoEmail.Mensagem;
                    mensagem = mensagem.Replace("#NumerodaNota#", vnfea.Nrvenda.ToString());
                    mensagem = mensagem.Replace("#SeriedaNota#", vnfe.Serie.ToString());
                    mensagem = mensagem.Replace("#ChavedaNota#", vnfea.Chave.ToString());
                    mensagem = mensagem.Replace("#DatadaNota#", vnfe.DhEmi.ToString("dd/MM/yyyy"));
                    mensagem = mensagem.Replace("#RazaoSocialEmitente#", App.Parametro.Razaosocial);
                    mensagem = mensagem.Replace("#CNPJEmitente#", App.Parametro.Cnpj);
                    mensagem = mensagem.Replace("#RazaoSocialDestinatario#", vnfe.Nomecliente);
                    mensagem = mensagem.Replace("#CNPJDestinatario#", vnfe.Cnpj_cpf);
                    if (_configuracoes.CfgServico.tpAmb == TipoAmbiente.Homologacao)
                        mensagem = mensagem.Replace("#Link#", "https://hom.nfe.fazenda.gov.br/Portal/consultaResumo.aspx?tipoConsulta=resumo&nfe=" + vnfea.Chave);
                    else
                        mensagem = mensagem.Replace("#Link#", "https://www.nfe.fazenda.gov.br/portal/consultaRecaptcha.aspx?tipoConsulta=resumo&nfe=" + vnfea.Chave);

                    _configuracoes.ConfiguracaoEmail.Mensagem = mensagem;
                    _configuracoes.ConfiguracaoEmail.Assunto = _configuracoes.ConfiguracaoEmail.Assunto.Replace("#NumerodaNota#", vnfea.Nrvenda.ToString());

                    var emailBuilder = new EmailBuilder(_configuracoes.ConfiguracaoEmail)
                        .AdicionarAnexo(arquivoXml)
                        .AdicionarAnexo(arquivoPdf);

                    var emailDoDestinatario = copia.Trim();
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
                    emailBuilder.ErroAoEnviarEmail += erro => Funcoes.Mensagem(erro.Message, "Erro  - Code:Email0", MessageBoxButton.OK, MessageBoxImage.Error);
                    emailBuilder.Enviar();
                }
            }
            catch (ArgumentException ex)
            {
                if (ex.Message.Contains("O cliente desta nota está com e-mail vazio, verifique no cadastro de cliente."))
                    MinhaNotificacao.NotificarErro("Erro no Envio de EMAIL", ex.Message);
                else if (ex.Message.Contains("Digite apenas um e-mail no cadastro deste cliente."))
                    MinhaNotificacao.NotificarErro("Erro no Envio de EMAIL", ex.Message);
                else if (ex.Message.Contains("Não foi possivel localizar "))
                    MinhaNotificacao.NotificarErro("Erro no Envio de EMAIL", ex.Message);
                else
                    Funcoes.Crashe(ex, "ERRO DA E-MAIL", false);
            }
            catch (InvalidOperationException ex)
            {
                Funcoes.Mensagem(ex.Message, "Erro - Code:Email2", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                Funcoes.Mensagem(ex.Message, "Erro - Code:Email1", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private static void EventoDepoisDeEnviarEmail(object sender, EventArgs e)
        {
            MinhaNotificacao.NotificarEInfo("E-mail encaminhado com sucesso.", "E-MAIL");
        }




        /// <summary>
        /// Montagem da Nota NF-e
        /// </summary>
        /// <param name="numero"></param>
        /// <param name="modelo"></param>
        /// <param name="versao"></param>
        /// <returns></returns>
        protected virtual NFe.Classes.NFe GetNf(int numero, ModeloDocumento modelo, VersaoServico versao)
        {
            var nf = new NFe.Classes.NFe { infNFe = GetInf(numero, modelo, versao) };
            Infresptec infres = new Infresptec(_configuracoes.EnderecoEmitente.UF.ToString());
            if (infres.IdCSRT > 0)
            {
                nf.Assina();
                nf.infNFe.infRespTec = GetInfRespTec(infres, nf.infNFe.Id.Replace("NFe", ""));
            }
            return nf;
        }

        protected virtual infNFe GetInf(int numero, ModeloDocumento modelo, VersaoServico versao)
        {
            infNFe infNFe = null;
            try
            {
                infNFe = new infNFe
                {
                    versao = versao.VersaoServicoParaString(),
                    ide = GetIdentificacao(numero, modelo, versao),
                    emit = GetEmitente(),
                    dest = GetDestinatario(versao),
                    transp = GetTransporte(),
                };

                if (vendanfe.Uf_embarq != null)
                    if (vendanfe.Uf_embarq.Trim() != "")
                    {
                        infNFe.exporta = new exporta()
                        {
                            UFSaidaPais = vendanfe.Uf_embarq,
                            xLocExporta = vendanfe.Lc_embarq,
                            xLocDespacho = vendanfe.Lc_embarq,
                        };
                    }

                if (App.Parametro.Cnpjcontabilidade != null)
                    if (App.Parametro.Cnpjcontabilidade != "")
                    {
                        autXML autXMLa = new autXML();
                        string cnpjcontabilidade = App.Parametro.Cnpjcontabilidade;
                        cnpjcontabilidade = Funcoes.Deixarnumero(cnpjcontabilidade);
                        if (cnpjcontabilidade.Length == 14)
                        {
                            autXMLa.CNPJ = cnpjcontabilidade;
                        }
                        else if (cnpjcontabilidade.Length == 11)
                        {
                            autXMLa.CPF = cnpjcontabilidade;
                        }
                        else
                        {
                            Funcoes.Mensagem("Campo de CNPJ/CPF da contabilidade incorreto:" + cnpjcontabilidade, "INFORMAÇÃO", MessageBoxButton.OK);
                        }
                        List<autXML> listaautxml = new List<autXML>
                        {
                            autXMLa
                        };
                        infNFe.autXML = listaautxml;
                    }


                if (vendanfe.Cnpj_cpf_entrega != "" && vendanfe.Cnpj_cpf_retirada != "0")
                {
                    infNFe.entrega = GetEntrega();
                }

                if (vendanfe.Cnpj_cpf_retirada != "" && vendanfe.Cnpj_cpf_retirada != "0")
                {
                    infNFe.retirada = GetRetirada();
                }

                if ((vendanfe.Obs.Length > 1) || (vendanfe.Infadfisco.Length > 1))
                {
                    infNFe.infAdic = new infAdic();
                    if (vendanfe.Obs.Length > 1) infNFe.infAdic.infCpl = vendanfe.Obs.Replace("\n", "").Replace("\r", "").Trim();
                    if (vendanfe.Infadfisco.Length > 1) infNFe.infAdic.infAdFisco = vendanfe.Infadfisco.Trim();
                }

                VendaNFeI ivenda = new VendaNFeI();
                List<VendaNFeI> lista = ivenda.GetTodas(vendanfe.Nrvenda);
                foreach (VendaNFeI i in lista)
                {
                    infNFe.det.Add(GetDetalhe(i, infNFe.emit.CRT, modelo));
                }

                infNFe.total = GetTotal(infNFe.det);

                infNFe.pag = GetPagamento(numero, infNFe.total.ICMSTot, versao);

                if (VendaNFePG.VerificarDup(vendanfe.Nrvenda))
                    infNFe.cobr = GetCobranca(); //V3.00 e 4.00 Somente

                return infNFe;

            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ERRO AO GERAR O XML | CODIGO: GETINF ");
                return infNFe;
            }
        }

        protected virtual transp GetTransporte()
        {
            try
            {
                var t = new transp
                {
                    modFrete = (ModalidadeFrete)vendanfe.ModFrete
                };
                if (!string.IsNullOrWhiteSpace(vendanfe.Nome_transp))
                {
                    t.transporta = new transporta();
                    if (!string.IsNullOrWhiteSpace(vendanfe.Nome_transp))
                        t.transporta.xNome = vendanfe.Nome_transp;

                    if (!string.IsNullOrWhiteSpace(vendanfe.Ender_transp))
                        t.transporta.xEnder = vendanfe.Ender_transp;

                    if (!string.IsNullOrWhiteSpace(vendanfe.Mun_transp))
                        t.transporta.xMun = vendanfe.Mun_transp;

                    if (!string.IsNullOrWhiteSpace(vendanfe.Uf_transp))
                        t.transporta.UF = vendanfe.Uf_transp;

                    if (!string.IsNullOrWhiteSpace(vendanfe.Ie_transp))
                        t.transporta.IE = vendanfe.Ie_transp;

                    if (vendanfe.Cnpjcpf_transp.Length >= 14)
                        t.transporta.CNPJ = Funcoes.Deixarnumero(vendanfe.Cnpjcpf_transp.Replace(".", "").Replace(@"/", "").Replace("-", "").Trim());
                    else
                        t.transporta.CPF = Funcoes.Deixarnumero(vendanfe.Cnpjcpf_transp.Replace(".", "").Replace(@"/", "").Replace("-", "").Trim());
                }
                if (!string.IsNullOrWhiteSpace(vendanfe.Placa_transp))
                {
                    t.veicTransp = new veicTransp()
                    {
                        placa = vendanfe.Placa_transp,
                        RNTC = vendanfe.Rntc_transp,
                        UF = vendanfe.Ufveiculo_transp,
                    };
                }
                if (vendanfe.QVol_transp > 0 || vendanfe.Pesol_transp > 0 || vendanfe.Pesob_transp > 0)
                {
                    t.vol = new List<vol>();
                    var volu = new vol();

                    if (vendanfe.QVol_transp > 0)
                        volu.qVol = vendanfe.QVol_transp;

                    if (!string.IsNullOrEmpty(vendanfe.Nvol_transp.Replace(" ", "")))
                        volu.nVol = vendanfe.Nvol_transp;

                    if (!string.IsNullOrEmpty(vendanfe.Esp_transp.Replace(" ", "")))
                        volu.esp = vendanfe.Esp_transp;

                    if (!string.IsNullOrEmpty(vendanfe.Marca_transp.Replace(" ", "")))
                        volu.marca = vendanfe.Marca_transp;

                    if (vendanfe.Pesol_transp > 0)
                        volu.pesoL = vendanfe.Pesol_transp;

                    if (vendanfe.Pesob_transp > 0)
                        volu.pesoB = vendanfe.Pesob_transp;

                    t.vol.Add(volu);
                }

                return t;

            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ERRO AO GERAR O XML | CODIGO: GetTransporte");
                return null;
            }
        }

        protected virtual ide GetIdentificacao(int numero, ModeloDocumento modelo, VersaoServico versao)
        {
            try
            {
                Cfop cfope = new Cfop(vendanfe.Cfop);
                if (cfope.Tipo == "E") vendanfe.Tpnf = 0;
                else vendanfe.Tpnf = 1;

                var ide = new ide
                {
                    cUF = _configuracoes.EnderecoEmitente.UF,
                    natOp = vendanfe.Natop.Trim(),
                    mod = modelo,
                    serie = vendanfe.Serie,
                    nNF = numero,
                    tpNF = (TipoNFe)vendanfe.Tpnf, //verificar se e entrada ou saida
                    cMunFG = _configuracoes.EnderecoEmitente.cMun,
                    //tpEmis = _configuracoes.CfgServico.tpEmis,
                    tpEmis = (TipoEmissao)_configuracoes.CfgServico.tpEmis,
                    tpImp = (TipoImpressao)Convert.ToInt32(vendanfe.Danfe), //TIPO 
                    cNF = "03054280",  //"1234",
                    tpAmb = _configuracoes.CfgServico.tpAmb,
                    finNFe = (FinalidadeNFe)Convert.ToInt32(vendanfe.Finalidade),
                    idDest = (DestinoOperacao)Convert.ToInt32(vendanfe.Idop),
                    indFinal = (ConsumidorFinal)vendanfe.Indfinal,
                    indPres = (PresencaComprador)vendanfe.Indpres,
                    verProc = "CIAF " + Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                    dhEmi = vendanfe.DhEmi
                };

                if (ide.indPres == PresencaComprador.pcPresencial || ide.indPres == PresencaComprador.pcInternet ||
               ide.indPres == PresencaComprador.pcTeleatendimento || ide.indPres == PresencaComprador.pcEntregaDomicilio ||
               ide.indPres == PresencaComprador.pcOutros)
                {
                    ide.indIntermed = IndicadorIntermediador.iiSemIntermediador;
                }
                if (ide.tpEmis != TipoEmissao.teNormal)
                {
                    // tpEmis = TipoEmissao.teSCAN;
                    ide.dhCont = vendanfe.Datacontingencia;
                    if (string.IsNullOrEmpty(vendanfe.Justifica))
                    {
                        var justificativa = Funcoes.InpuBox(Application.Current.Windows.OfType<TELAPROCESSAMENTONFE>().First(), "Contingência NFe", "JUSTIFICATIVA", "Nota em contingência porque está com problemas técnicos.").Trim();

                        if (string.IsNullOrEmpty(justificativa)) throw new Exception("A justificativa deve ser informada!");
                        ide.xJust = justificativa;
                        vendanfe.Justifica = justificativa;
                        //vendanfe.Update();
                    }
                    else
                    {
                        ide.xJust = vendanfe.Justifica;
                    }
                }

                if (vendanfe.Chavedev.Replace(" ", "") != "")
                {

                    List<NFref> lista = new List<NFref>();
                    if (vendanfe.Chavedev != "")
                    {
                        if (vendanfe.Chavedev.Contains("NFe NÃO EMITIDA PELO CIAF, DIGITE MANUALMENTE CHAVE"))
                        {
                            Funcoes.Mensagem(@"Chave da NF-e Referenciada como ""NFe NÃO EMITIDA PELO CIAF, DIGITE MANUALMENTE CHAVE"" " + Environment.NewLine +
                                "Sua nota pode ser precessada sem essa informação.",
                                "AVISO DE CHAVE REFERENCIADA", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            NFref nfref = new NFref();
                            nfref.refNFe = vendanfe.Chavedev;
                            lista.Add(nfref);
                        }
                    }
                    ide.NFref = lista;
                }

                if (!string.IsNullOrWhiteSpace(vendanfe.Ru_nota))
                {
                    List<NFref> lista = new List<NFref>();
                    NFref nfref = new NFref();
                    nfref.refNFP = new refNFP();
                    nfref.refNFP.cUF = Funcoes.EstadoString(vendanfe.Ru_ufprod);
                    nfref.refNFP.nNF = Convert.ToInt32(vendanfe.Ru_nota);
                    nfref.refNFP.serie = Convert.ToInt32(vendanfe.Ru_serie);
                    DateTime ru_data = Convert.ToDateTime(vendanfe.Ru_datae);
                    nfref.refNFP.AAMM = ru_data.ToString("yyMM");
                    if (vendanfe.Ru_cpfcnpj.Length >= 14)
                    {
                        nfref.refNFP.CNPJ = vendanfe.Ru_cpfcnpj;
                    }
                    else
                        nfref.refNFP.CPF = vendanfe.Ru_cpfcnpj;
                    nfref.refNFP.IE = vendanfe.Ru_ieprod;
                    nfref.refNFP.mod = "04";
                    lista.Add(nfref);
                    ide.NFref = lista;
                }
                if (ide.finNFe == FinalidadeNFe.fnComplementar || ide.finNFe == FinalidadeNFe.fnDevolucao || ide.finNFe == FinalidadeNFe.fnAjuste)
                {
                    List<NFref> lista = new List<NFref>();
                    if (vendanfe.Chavedev != "")
                    {

                        if (vendanfe.Chavedev.Contains("NFe NÃO EMITIDA PELO CIAF, DIGITE MANUALMENTE CHAVE"))
                        {
                            Funcoes.Mensagem(@"Chave da NF-e Referenciada como ""NFe NÃO EMITIDA PELO CIAF, DIGITE MANUALMENTE CHAVE"" " + Environment.NewLine +
                                       "Sua nota pode ser precessada sem essa informação.",
                                       "AVISO DE CHAVE REFERENCIADA", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            NFref nfref = new NFref();
                            nfref.refNFe = vendanfe.Chavedev;
                            lista.Add(nfref);
                        }
                    }
                    ide.NFref = lista;
                }

                if (vendanfe.DhSaiEnt.Date > Convert.ToDateTime("01/01/2001"))
                    ide.dhSaiEnt = vendanfe.DhSaiEnt;

                if (vendanfe.Mod != null)
                    if (vendanfe.Mod == "2D" || vendanfe.Mod == "2C" || vendanfe.Mod == "2B")
                    {
                        if (string.IsNullOrEmpty(vendanfe.NECF))
                            throw new ArgumentException("O vendanfe.NECF está vazio.");

                        if (string.IsNullOrEmpty(vendanfe.NCOO))
                            throw new ArgumentException("O vendanfe.NCOO está vazio.");
                        try
                        {
                            List<NFref> lista = new List<NFref>();
                            NFref nfref = new NFref()
                            {
                                refECF = new refECF()
                                {
                                    mod = vendanfe.Mod,
                                    nECF = Convert.ToInt32(vendanfe.NECF),
                                    nCOO = Convert.ToInt32(vendanfe.NCOO)
                                }
                            };
                            lista.Add(nfref);
                            ide.NFref = lista;
                        }
                        catch (Exception exe)
                        {
                            Funcoes.Crashe(exe, "ERRO AO GERAR O XML | CODIGO: GetIdentificacao/NFref ");
                            return null;
                        }
                    }
                return ide;
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ERRO AO GERAR O XML | CODIGO: GetIdentificacao ");
                return null;
            }
        }
        protected virtual emit GetEmitente()
        {
            try
            {
                var emit = new emit()
                {
                    xNome = _configuracoes.Emitente.xNome,
                    xFant = _configuracoes.Emitente.xFant,
                    IE = _configuracoes.Emitente.IE,
                };

                if (_configuracoes.Emitente.IEST != "")
                {
                    emit.IEST = _configuracoes.Emitente.IEST;
                }
                if (_configuracoes.Emitente.IM != "")
                {
                    emit.IM = _configuracoes.Emitente.IM;
                }
                if (_configuracoes.Emitente.CNAE != "")
                {
                    emit.CNAE = _configuracoes.Emitente.CNAE;
                }
                crtcliente = _configuracoes.Emitente.CRT;
                emit.CRT = crtcliente;

                //if (App.Parametro.Cnpj.Length >= 14)
                //    emit.CNPJ = App.Parametro.Cnpj.Replace("/", "").Replace(".", "").Replace("-", "");
                //else
                //    emit.CPF = App.Parametro.Cnpj.Replace("/", "").Replace(".", "").Replace("-", "");
                if (_configuracoes.Emitente.CNPJ.Length >= 14)
                    emit.CNPJ = _configuracoes.Emitente.CNPJ;
                else
                    emit.CPF = _configuracoes.Emitente.CPF;
                emit.enderEmit = _configuracoes.EnderecoEmitente;
                return emit;
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ERRO AO GERAR O XML | CODIGO: GetEmitente ");
                return null;
            }
        }
        protected virtual dest GetDestinatario(VersaoServico versao)
        {
            try
            {
                Clientes cliente = new Clientes(vendanfe.Id_cliente.ToString());
                var dest = new dest(versao)
                {
                    xNome = vendanfe.Nomecliente,
                    enderDest = new enderDest
                    {
                        xLgr = vendanfe.Endereco.Trim(),
                        nro = vendanfe.Numero.Trim(),
                        xCpl = vendanfe.Complemento.Trim(),
                        xBairro = vendanfe.Bairro.Trim(),
                        cMun = vendanfe.Cmun,
                        xMun = vendanfe.Cidade.Trim(),
                        UF = vendanfe.Uf,
                        cPais = vendanfe.Cpais,
                        xPais = vendanfe.Pais,
                    }
                };

                if (!string.IsNullOrEmpty(vendanfe.Cep.Replace(".", "").Replace("-", "").Replace(" ", "")))
                    dest.enderDest.CEP = vendanfe.Cep.Replace(".", "").Replace("-", "").Replace(" ", "");
                if (vendanfe.Cpais == 1058)
                {
                    if (vendanfe.Cnpj_cpf.Length >= 14)
                    {
                        dest.CNPJ = vendanfe.Cnpj_cpf;
                        dest.xNome = cliente.Razao;
                    }
                    else
                    {
                        dest.CPF = vendanfe.Cnpj_cpf;
                        if (vendanfe.Ie_rg.Replace(".", "").Replace("-", "") != "")
                        {
                            if (vendanfe.Ie_rg.Trim() != "ISENTO")
                            {
                                dest.IE = vendanfe.Ie_rg.Replace(".", "").Replace("-", "").Trim();
                                dest.xNome = cliente.Razao;
                            }

                        }

                    }
                }
                else
                {
                    dest.idEstrangeiro = vendanfe.Cnpj_cpf;
                }

                if (vendanfe.Fone != "0" && vendanfe.Fone != null && vendanfe.Fone != "")
                {
                    if (Funcoes.Deixarnumero(vendanfe.Fone) != "")
                        dest.enderDest.fone = Convert.ToInt64(Funcoes.Deixarnumero(vendanfe.Fone));
                }

                if (cliente.Contrib == "SIM")
                {
                    dest.indIEDest = indIEDest.ContribuinteICMS;
                    dest.IE = vendanfe.Ie_rg.Replace(".", "").Replace("-", "");
                    dest.xNome = cliente.Razao;
                }
                else if (vendanfe.Contrib == 2)
                {
                    dest.indIEDest = indIEDest.Isento;
                }
                else if (cliente.Contrib == "NÃO")
                {
                    //dest.IE = "ISENTO";
                    dest.indIEDest = indIEDest.NaoContribuinte;
                }

                if (vendanfe.Im != "")
                    dest.IM = vendanfe.Im;

                if (vendanfe.Email != "")
                    dest.email = vendanfe.Email;
                else
                    if (cliente.Email != "")
                    dest.email = cliente.Email;

                if (_configuracoes.CfgServico.tpAmb == TipoAmbiente.Homologacao)
                    dest.xNome = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";
                return dest;
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ERRO AO GERAR O XML | CODIGO: GetDestinatario ");
                return null;
            }
        }

        protected virtual infRespTec GetInfRespTec(Infresptec infres, string chave)
        {
            try
            {
                infRespTec inf = new infRespTec
                {
                    CNPJ = infres.Cnpj,
                    xContato = infres.XContato,
                    email = infres.Email,
                    fone = Funcoes.Deixarnumero(infres.Fone),
                };
                if (infres.Csrt.Replace(" ", "") != "")
                {
                    inf.idCSRT = infres.IdCSRT;
                    inf.hashCSRT = GerarHashCSRT.HashCSRT(infres.Csrt, chave);
                }
                return inf;
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ERRO AO GERAR O XML | CODIGO: GetInfRespTec ");
                return null;
            }
        }
        protected virtual entrega GetEntrega()
        {

            entrega entregar = new entrega();
            try
            {
                if (vendanfe.Cnpj_cpf_entrega.Length >= 14)
                    entregar.CNPJ = vendanfe.Cnpj_cpf_entrega;
                else entregar.CPF = vendanfe.Cnpj_cpf_entrega;

                entregar.xNome = vendanfe.Xnome_entrega;
                entregar.CEP = long.Parse(vendanfe.Cep_entrega);
                entregar.xLgr = vendanfe.Xlgr_entrega;
                entregar.nro = vendanfe.Nro_entrega;

                if (vendanfe.Xcpl_entrega.Replace(" ", "") != "")
                    entregar.xCpl = vendanfe.Xcpl_entrega;
                entregar.xBairro = vendanfe.Xbairro_entrega;
                entregar.xMun = vendanfe.Xmun_entrega;
                entregar.cMun = vendanfe.Cmun;

                entregar.UF = vendanfe.Uf_entrega;
                entregar.cPais = vendanfe.Cpais_entrega;
                entregar.xPais = vendanfe.Xpais_entrega;

                if (vendanfe.Fone_entrega != "")
                    entregar.fone = vendanfe.Fone_entrega;

                if (vendanfe.Email_entrega != "")
                    entregar.email = vendanfe.Email_entrega;

                if (vendanfe.Ie_entrega != "" && vendanfe.Ie_entrega != "0")
                    entregar.IE = vendanfe.Ie_entrega;

                return entregar;
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ERRO - CODE: GETENTREGA");
                return entregar;
            }
        }

        protected virtual retirada GetRetirada()
        {
            retirada retiradar = new retirada();
            try
            {
                if (vendanfe.Cnpj_cpf_retirada.Length >= 14)
                    retiradar.CNPJ = vendanfe.Cnpj_cpf_retirada;
                else retiradar.CPF = vendanfe.Cnpj_cpf_retirada;

                retiradar.xNome = vendanfe.Xnome_retirada;
                retiradar.CEP = long.Parse(vendanfe.Cep_retirada);
                retiradar.xLgr = vendanfe.Xlgr_retirada;
                retiradar.nro = vendanfe.Nro_retirada;

                if (vendanfe.Xcpl_retirada.Replace(" ", "") != "")
                    retiradar.xCpl = vendanfe.Xcpl_retirada;

                retiradar.xBairro = vendanfe.Xbairro_retirada;
                retiradar.xMun = vendanfe.Xmun_retirada;
                retiradar.cMun = vendanfe.Cmun;

                retiradar.UF = vendanfe.Uf_retirada;
                retiradar.cPais = vendanfe.Cpais_retirada;
                retiradar.xPais = vendanfe.Xpais_retirada;

                if (vendanfe.Fone_retirada != "")
                    retiradar.fone = vendanfe.Fone_retirada;

                if (vendanfe.Email_retirada != "")
                    retiradar.email = vendanfe.Email_retirada;

                if (vendanfe.Ie_retirada != "")
                    retiradar.IE = vendanfe.Ie_retirada;

                return retiradar;
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                    Funcoes.Crashe(ex, "ERRO - CODE: GetRetirada");
                return retiradar;
            }
        }

        /// <summary>
        /// PRODUTOS
        /// </summary>
        /// <param name="versao"></param>
        /// <param name="produtos"></param>
        /// <returns></returns>
        /// 
        protected virtual det GetDetalhe(VendaNFeI ivendanfe, CRT crt, ModeloDocumento modelo)
        {
            det det = new det();
            try
            {
                
                det = new det
                {   
                    
                    nItem = ivendanfe.Nritem,
                    prod = new prod
                    {
                        cProd = ivendanfe.Codigob.Trim(),
                        //cEAN = ivendanfe.Codigob,
                        xProd = ivendanfe.Descricao.Trim(),
                        NCM = ivendanfe.Ncm,
                        CFOP = Convert.ToInt32(ivendanfe.Cfop),
                        uCom = ivendanfe.Unidade, //"UN",
                        qCom = ivendanfe.Quantidade,
                        vUnCom = ivendanfe.Valor,
                        vProd = ivendanfe.Valor * ivendanfe.Quantidade,
                        vDesc = ivendanfe.Desconto,
                        //  cEANTrib = ivendanfe.Codigob,//"7770000000012",
                        uTrib = ivendanfe.Utrib, //"UNID",
                        qTrib = ivendanfe.Qtrib, //1,
                        vUnTrib = ivendanfe.Vuntrib,
                        indTot = (IndicadorTotal)ivendanfe.Indtot,
                        //NVE = {"AA0001", "AB0002", "AC0002"},
                        //CEST = ?

                        //ProdutoEspecifico = new med
                        //{
                        //    tpArma = Tipomed.UsoPermitido,
                        //    nSerie = "123456",
                        //    nCano = "123456",
                        //    descr = "TESTE DE MED"
                        //}

                    },
                    imposto = new imposto
                    {
                        vTotTrib = ivendanfe.Vtottrib,
                    }
                    
                };

                //Parametros  de Olho no Imposto
                if (App.Parametro.Calculatrib == "SIM")
                {
                    DeOlhoNoImposto olhoimposto = new DeOlhoNoImposto();

                    olhoimposto = olhoimposto.Requisicao(ivendanfe.Ncm, ivendanfe.Vl_total.ToString(), App.Parametro.Uf);

                    ivendanfe.Vtottrib = olhoimposto.GetValorAproxTributos();
                    ivendanfe.UpdateVmimposto(ivendanfe.Vtottrib.ToString());

                    if (det.imposto == null)
                        det.imposto = new imposto();
                    else
                        det.imposto.vTotTrib = ivendanfe.Vtottrib;
                }

                if (!string.IsNullOrEmpty(ivendanfe.I_nrdi.Replace(" ", "")))
                {
                    det.prod.DI = new List<DI>()
                    {
                        new DI {
                                nDI = ivendanfe.I_nrdi,
                                dDI = ivendanfe.I_dtregistro,
                                xLocDesemb = ivendanfe.I_localdesemb,
                                UFDesemb = ivendanfe.I_ufdesemb,
                                dDesemb = ivendanfe.I_dtdesemb,
                                tpViaTransp = (TipoTransporteInternacional) ivendanfe.I_tpviatransp,
                                vAFRMM = ivendanfe.I_afrmm,
                                tpIntermedio = (TipoIntermediacao) ivendanfe.I_tpintermedio,
                                cExportador  = ivendanfe.I_exportador,
                                adi = new List<adi>()
                                {
                                    new adi()
                                    {
                                        nAdicao = Convert.ToInt32(ivendanfe.I_adicao),
                                        nSeqAdic =Convert.ToInt32(ivendanfe.I_nritemdi),
                                        cFabricante = ivendanfe.I_fabestra,
                                    }
                                 }
                        }
                    };
                    
                    det.imposto.II = new II()
                    {
                        vBC = ivendanfe.I_vlbcalculo,
                        vDespAdu = ivendanfe.I_daduaneira,
                        vII = ivendanfe.I_vlii,
                        vIOF = ivendanfe.I_vliof,
                    };
                    
                }
                if (ivendanfe.Voutro > 0)
                {
                    det.prod.vOutro = ivendanfe.Voutro;
                }
                // PISGeral
                var pisgeral = new PISGeral()
                {
                    CST = (CSTPIS)Convert.ToInt32(ivendanfe.Cstpis),
                    pPIS = ivendanfe.Pisaliq,
                    vBC = ivendanfe.Pisbc,
                    vPIS = ivendanfe.Pisval
                };
                det.imposto.PIS = new PIS
                {
                    TipoPIS = pisgeral.ObterPISBasico(),
                };

                // COFINSGeral
                var cofinsGeral = new COFINSGeral()
                {
                    CST = (CSTCOFINS)Convert.ToUInt32(ivendanfe.Cstcofins),
                    pCOFINS = ivendanfe.Cofaliq,
                    vBC = ivendanfe.Cofbc,
                    vCOFINS = ivendanfe.Vcofins,
                };
                det.imposto.COFINS = new COFINS
                {
                    TipoCOFINS = cofinsGeral.ObterCOFINSBasico(),
                };

                ///ICMS
                det.imposto.ICMS = new ICMS
                {
                    TipoICMS = ObterIcmsBasic(ivendanfe, crtcliente),
                };
                if (ivendanfe.Picmsufdest > 0)
                {
                    det.imposto.ICMSUFDest = new ICMSUFDest()
                    {
                        pFCPUFDest = ivendanfe.Pfcpufdest,
                        pICMSInter = ivendanfe.Picmsinter,
                        pICMSInterPart = ivendanfe.Picmsinterpa,
                        pICMSUFDest = ivendanfe.Picmsufdest,
                        vBCFCPUFDest = ivendanfe.Vbcfcpufdest,
                        vBCUFDest = ivendanfe.Vbcufdest,
                        vFCPUFDest = ivendanfe.Vfcpufdest,
                        vICMSUFDest = ivendanfe.Vicmsufdest,
                        vICMSUFRemet = ivendanfe.Vicmsufremet,
                    };
                }
                
                if (ivendanfe.Codigoanp != "" && ivendanfe.Codigoanp != "0")
                {
                    comb comb = new comb()
                    {
                        cProdANP = ivendanfe.Codigoanp,
                        descANP = ivendanfe.Descanp,
                        UFCons = ivendanfe.Ufcombus,
                    };

                    if (ivendanfe.Codif != "0" && ivendanfe.Codif != "") comb.CODIF = ivendanfe.Codif;
                    if (ivendanfe.Perglp > 0) comb.pGLP = ivendanfe.Perglp;
                    if (ivendanfe.Pergnat > 0) comb.pGNn = ivendanfe.Pergnat;
                    if (ivendanfe.Pergnat_i > 0) comb.pGNi = ivendanfe.Pergnat_i;
                    if (ivendanfe.Vlpartida > 0) comb.vPart = ivendanfe.Vlpartida;
                    if (ivendanfe.Qtfattempa > 0) comb.qTemp = ivendanfe.Qtfattempa;

                    det.prod.ProdutoEspecifico = new List<ProdutoEspecifico>();
                    det.prod.ProdutoEspecifico.Add(comb);

                    if (Convert.ToInt32(ivendanfe.Cst) == 60)
                    {
                        det.imposto.ICMS = new ICMS
                        {
                            TipoICMS = new ICMSST
                            {
                                orig = (OrigemMercadoria)ivendanfe.Origem,
                                CST = Csticms.Cst60,
                                vBCSTRet = ivendanfe.VBCSTRet,
                                vICMSSTRet = ivendanfe.VICMSSTRet,
                                vBCSTDest = ivendanfe.Vbcstdest,
                                vICMSSTDest = ivendanfe.Vicmsstdest,

                            }
                        };
                    }
                    else
                    {
                        det.imposto.ICMS = new ICMS
                        {
                            TipoICMS = ObterIcmsBasic(ivendanfe, crtcliente),
                        };
                    }

                }

                if (ivendanfe.Nrpedido.Replace(" ", "") != "" && ivendanfe.Nrpedido != null)
                {
                    det.prod.xPed = ivendanfe.Nrpedido;
                    if (ivendanfe.Nritemped.Replace(" ", "") != "")
                        det.prod.nItemPed = Convert.ToInt32(ivendanfe.Nritemped.Replace(" ", ""));
                }

                Produtos produto = new Produtos(ivendanfe.Codigob);
                //EAN VALIDO
                if (produto.Eanval == "SIM")
                {
                    det.prod.cEAN = ivendanfe.Codigob;
                    det.prod.cEANTrib = ivendanfe.Codigob;
                }
                else
                {
                    det.prod.cEAN = "SEM GTIN";
                    det.prod.cEANTrib = "SEM GTIN";
                }


                if (_configuracoes.ConfiguracaoDanfeNfe.InfAdProd)
                    det.infAdProd = produto.Obs;

                if (produto.CBenef != null)
                    if (produto.CBenef.Replace(" ", "") != "") { det.prod.cBenef = produto.CBenef; }

                if (ivendanfe.Frete > 0)
                {
                    det.prod.vFrete = ivendanfe.Frete;
                }


                if (ivendanfe.Cest != "0" && ivendanfe.Cest != null && ivendanfe.Cest.Replace(" ", "") != "")
                {
                    det.prod.CEST = ivendanfe.Cest;
                }

                if (App.Parametro.Ipi == "SIM")
                {

                    if (ivendanfe.Cenqipi != null && ivendanfe.Cenqipi != "")
                    {
                        var ipiGeral = new IPIGeral()
                        {
                            CST = RetornaIPI(Convert.ToInt32(ivendanfe.Cstipi)),
                            pIPI = ivendanfe.Aipi,
                            vBC = det.prod.vProd,
                            vIPI = ivendanfe.Vipi,

                        };

                        if (det.prod.vFrete != null)
                            ipiGeral.vBC += det.prod.vFrete;

                        if (det.prod.vOutro != null)
                            ipiGeral.vBC += det.prod.vOutro;

                        if (det.prod.vSeg != null)
                            ipiGeral.vBC += det.prod.vSeg;


                        det.imposto.IPI = new IPI()
                        {
                            cEnq = Convert.ToInt32(ivendanfe.Cenqipi),

                            TipoIPI = ipiGeral.ObterIPIBasico()
                        };


                    }
                }

                if (_configuracoes.ConfiguracaoDanfeNfe.Ipidevol || ivendanfe.Vipidevol > 0)
                {

                    if (ivendanfe.Vipidevol == 0)
                    {
                        string vipidevol = "0,00";
                        vipidevol = Funcoes.InpuBox(null, "Valor de IPI Devolução", "Valor do IPI Devolução no ITEM:" + ivendanfe.Nritem + " Descrição: " + ivendanfe.Descricao, vipidevol).Trim();
                        try
                        {
                            ivendanfe.Vipidevol = Convert.ToDecimal(vipidevol);
                            ivendanfe.UpdateVipidevol();
                        }
                        catch (Exception ex)
                        {
                            Funcoes.Crashe(ex, "ERRO - CODE: Vipidevol", true);
                        }
                    }

                    if (ivendanfe.Pdevol == 0)
                    {
                        string aipidevol = "0,00";
                        aipidevol = Funcoes.InpuBox(null, "Valor da Aliquota(%) Percentual de Mercadoria Devolvida", "Valor de Percentual de Mercadoria Devolvida no ITEM:" + ivendanfe.Nritem + " Descrição: " + ivendanfe.Descricao, aipidevol).Trim();
                        decimal aipidevold = 0;

                        try
                        {
                            aipidevold = Convert.ToDecimal(aipidevol);
                            ivendanfe.Pdevol = aipidevold;
                            ivendanfe.Updatepdevol();
                        }
                        catch (Exception ex)
                        {
                            Funcoes.Crashe(ex, "ERRO - CODE: aipidevold", true);
                        }
                    }


                    det.impostoDevol = new impostoDevol()
                    {
                        IPI = new IPIDevolvido()
                        {
                            vIPIDevol = ivendanfe.Vipidevol
                        },
                        pDevol = ivendanfe.Pdevol
                    };
                } 
                det.imposto.IS = new IS()
                    {
                        CSTIS = CSTIS.Is000,
                        cClassTribIS = "000000",
                        vBCIS = ivendanfe.VBcIs,
                        pIS = ivendanfe.PIs,
                        pISEspec = 2,
                        uTrib = ivendanfe.Utrib,
                        qTrib = ivendanfe.Qtrib,
                        vIS = ivendanfe.VIs
                    };

                // Garantir que o objeto imposto esteja instanciado
                if (det.imposto == null)
                    det.imposto = new imposto();

                // Instanciação segura de toda a árvore IBSCBS evitando NRE
                det.imposto.IBSCBS = new IBSCBS
                {
                    CST = CST.Cst000,
                    cClassTrib = "000001",
                    gIBSCBS = new gIBSCBS
                    {
                        vBC = ivendanfe.VBcIbscbs,
                        gIBSUF = new gIBSUF
                        {
                            pIBSUF = ivendanfe.PIbsUf,
                            gDif = new gDif
                            {
                                pDif = ivendanfe.PDifUfIbs,
                                vDif = ivendanfe.VDifUfIbs
                            },
                            gDevTrib = new gDevTrib
                            {
                                vDevTrib = ivendanfe.VDevTribUfIbs
                            },
                            gRed = new gRed
                            {
                                pRedAliq = ivendanfe.PRedAliqUfIbs,
                                pAliqEfet = ivendanfe.PRedAliqEfetUfIbs
                            },
                            vIBSUF = ivendanfe.VIbsUf
                        },
                        gIBSMun = new gIBSMun
                        {
                            pIBSMun = ivendanfe.PIbsMun,
                            gDif = new gDif
                            {
                                pDif = ivendanfe.PDifMun,
                                vDif = ivendanfe.VDifMun
                            },
                            gDevTrib = new gDevTrib
                            {
                                vDevTrib = ivendanfe.VDevTribMun
                            },
                            gRed = new gRed
                            {
                                pRedAliq = ivendanfe.PRedAliqMun,
                                pAliqEfet = ivendanfe.PRedAliqEfetMun
                            },
                            vIBSMun = ivendanfe.VIbsMun
                        },
                        vIBS = ivendanfe.VDifUfIbs + ivendanfe.VDifMun,
                        gCBS = new gCBS
                        {
                            pCBS = ivendanfe.PCbs,
                            gDif = new gDif
                            {
                                pDif = ivendanfe.PDifUfCbs,
                                vDif = ivendanfe.VDifCbs
                            },
                            gDevTrib = new gDevTrib
                            {
                                vDevTrib = ivendanfe.VDevTribCbs
                            },
                            gRed = new gRed
                            {
                                pRedAliq = ivendanfe.PRedAliqCbs,
                                pAliqEfet = ivendanfe.VRedAliqCbs
                            },
                            vCBS = ivendanfe.VCbs
                        },
                        gTribRegular = new gTribRegular
                        {
                            CSTReg = CST.Cst000,
                            cClassTribReg = "000000",
                            pAliqEfetRegIBSUF = ivendanfe.PAliqEfetRegIbsUf,
                            vTribRegIBSUF = ivendanfe.VTribRegIbsUf,
                            pAliqEfetRegIBSMun = ivendanfe.PAliqEfetRegIbsMun,
                            vTribRegIBSMun = ivendanfe.VTribRegIbsMun,
                            pAliqEfetRegCBS = ivendanfe.PAliqEfetRegCbs,
                            vTribRegCBS = ivendanfe.VTribRegCbs
                        }
                    }
                };
                 
                return det;
            }

            catch (Exception ex)
            {
                Funcoes.Mensagem(" Por gentileza, verificar as informações apresentada no cadastro do item da nota como CFOP, CST, ICMS entre outas informações." + Environment.NewLine +
                                 ex.Message, " ERRO - CODE: GETDETALHE de ITEM DA NOTA ", MessageBoxButton.OK);
                Funcoes.Crashe(ex, "ERRO - CODE: GETDETALHE", false);

                return det;
            }

        }
        protected virtual ICMSBasico ObterIcmsBasic(VendaNFeI ivenda, CRT crt)
        {
            try
            {
                Csticms cst = Csticms.Cst00;
                if (Convert.ToInt32(ivenda.Cst) == 00) cst = Csticms.Cst00;
                if (Convert.ToInt32(ivenda.Cst) == 10) cst = Csticms.Cst10;
                if (Convert.ToInt32(ivenda.Cst) == 11) cst = Csticms.CstPart10;
                if (Convert.ToInt32(ivenda.Cst) == 20) cst = Csticms.Cst20;
                if (Convert.ToInt32(ivenda.Cst) == 30) cst = Csticms.Cst30;
                if (Convert.ToInt32(ivenda.Cst) == 40) cst = Csticms.Cst40;
                if (Convert.ToInt32(ivenda.Cst) == 41) cst = Csticms.Cst41;
                if (Convert.ToInt32(ivenda.Cst) == 42) cst = Csticms.CstRep41;

                if (Convert.ToInt32(ivenda.Cst) == 50) cst = Csticms.Cst50;
                if (Convert.ToInt32(ivenda.Cst) == 51) cst = Csticms.Cst51;
                if (Convert.ToInt32(ivenda.Cst) == 60) cst = Csticms.Cst60;
                if (Convert.ToInt32(ivenda.Cst) == 70) cst = Csticms.Cst70;
                if (Convert.ToInt32(ivenda.Cst) == 90) cst = Csticms.Cst90;
                if (Convert.ToInt32(ivenda.Cst) == 91) cst = Csticms.CstPart90;


                var icmsGeral = new ICMSGeral
                {
                    orig = (OrigemMercadoria)ivenda.Origem,
                    CST = cst,
                    modBC = DeterminacaoBaseIcms.DbiValorOperacao,
                    vBC = ivenda.Vbcitem, // 1.1m,
                    pICMS = ivenda.Aicms, //18,
                    vICMS = ivenda.Vicms, //0.20m,
                    CSOSN = (Csosnicms)Convert.ToInt32(ivenda.Cst),
                    pCredSN = 0, //pCredSN - Alíquota aplicável de cálculo do crédito (Simples Nacional).
                    vCredICMSSN = 0, // Valor crédito do ICMS que pode ser aproveitado nos termos do art. 23 da LC 123 (Simples Nacional)
                    vBCSTRet = ivenda.VBCSTRet,
                    vICMSSTRet = ivenda.VICMSSTRet,
                    vBCSTDest = ivenda.Vbcstdest,
                    vICMSSTDest = ivenda.Vicmsstdest,
                    vICMSSubstituto = ivenda.VICMSSubstituto,
                    UFST = vendanfe.Uf,
                    pST = ivenda.PST,
                    vICMSST = ivenda.Stvalicmsst,
                    //motDesICMS = MotivoDesoneracaoIcms.MdiOutros,

                };

                if (ivenda.VBCFCP > 0) icmsGeral.pFCP = ivenda.PFCP;
                if (ivenda.VBCFCP > 0) icmsGeral.vBCFCP = ivenda.VBCFCP;
                if (ivenda.VFCP > 0) icmsGeral.vFCP = ivenda.VFCP;

                if (ivenda.PFCPST > 0)
                {
                    icmsGeral.pFCPST = ivenda.PFCPST;
                    icmsGeral.vBCFCPST = ivenda.VBCFCPST;
                    icmsGeral.vFCPST = ivenda.VFCPST;
                }

                if (ivenda.PFCPSTRet > 0)
                {
                    icmsGeral.pFCPSTRet = ivenda.PFCPSTRet;
                    icmsGeral.vBCFCPSTRet = ivenda.VBCFCPSTRet;
                    icmsGeral.vFCPSTRet = ivenda.VFCPSTRet;
                }
                if (ivenda.Stmodalidade > 0) icmsGeral.modBCST = (DeterminacaoBaseIcmsSt)ivenda.Stmodalidade;
                if (ivenda.Stalicms > 0) icmsGeral.pICMSST = ivenda.Stalicms;
                if (ivenda.Stalicms > 0) icmsGeral.motDesICMS = (MotivoDesoneracaoIcms)9;
                if (ivenda.Mva > 0) icmsGeral.pMVAST = ivenda.Mva;

                if (ivenda.Stbcicmsst > 0) icmsGeral.vBCST = ivenda.Stbcicmsst;
                if (ivenda.Stvalicmsst > 0) icmsGeral.vICMSST = ivenda.Stvalicmsst;
                if (ivenda.Strdbcst > 0) icmsGeral.pRedBCST = ivenda.Strdbcst;
                if (ivenda.Strdbcst > 0) icmsGeral.pRedBCST = ivenda.Strdbcst;


                if (ivenda.PCredSn > 0) icmsGeral.pCredSN = ivenda.PCredSn;
                if (ivenda.VCredIcmssn > 0) icmsGeral.vCredICMSSN = ivenda.VCredIcmssn;


                //private decimal? _pRedBCEfet;
                //private decimal? _vBCEfet;
                //private decimal? _pICMSEfet;
                //private decimal? _vICMSEfet;

                //icmsGeral.pDif = 0;
                //icmsGeral.vICMSDif = 0;


                if (cst == Csticms.Cst51)
                {
                    string valor = "";

                    try
                    {
                        valor = "0";
                        valor = Funcoes.InpuBox(null, "Percentual do diferimento (pDif) ", "Percentual do diferimento no ITEM:" + ivenda.Nritem + " Descrição: " + ivenda.Descricao, valor);
                        if (!string.IsNullOrEmpty(valor))
                            icmsGeral.pDif = Convert.ToDecimal(valor);
                    }
                    catch (Exception ex)
                    {
                        Funcoes.Crashe(ex, "ERRO - CODE: pDif", true);
                    }

                    try
                    {
                        valor = (icmsGeral.vBC * (icmsGeral.pICMS / 100)).ToString("N2");
                        valor = Funcoes.InpuBox(null, "Valor do ICMS da Operação (vICMSOp) ", "Valor do ICMS da Operação no ITEM:" + ivenda.Nritem + " Descrição: " + ivenda.Descricao, valor);
                        if (!string.IsNullOrEmpty(valor))
                            icmsGeral.vICMSOp = Convert.ToDecimal(valor);
                    }
                    catch (Exception ex)
                    {
                        Funcoes.Crashe(ex, "ERRO - CODE: vICMSOp", true);
                    }


                    try
                    {
                        valor = (Convert.ToDecimal(icmsGeral.vICMSOp * (icmsGeral.pDif / 100))).ToString("n2");
                        valor = Funcoes.InpuBox(null, "Valor do ICMS diferido (vICMSDif) ", "Valor do ICMS diferido no ITEM:" + ivenda.Nritem + " Descrição: " + ivenda.Descricao, valor);
                        if (!string.IsNullOrEmpty(valor))
                            icmsGeral.vICMSDif = Convert.ToDecimal(valor);
                    }
                    catch (Exception ex)
                    {
                        Funcoes.Crashe(ex, "ERRO - CODE: vICMSDif", true);
                    }

                    try
                    {
                        icmsGeral.vICMS = (decimal)(icmsGeral.vICMSOp - icmsGeral.vICMSDif);
                        ivenda.Vicms = icmsGeral.vICMS;
                        ivenda.UpdatepICMS();
                        List<VendaNFeI> lista = ivenda.GetTodas(vendanfe.Nrvenda);
                        this.vendanfe.Vicms = lista.Sum(p => p.Vicms);
                        this.vendanfe.UpdateICMS();
                    }
                    catch (Exception ex)
                    {
                        Funcoes.Crashe(ex, "ERRO - CODE: vICMS NO ICMS 51", true);
                    }




                }

                if ( _configuracoes.ConfiguracaoDanfeNfe.Vicmsdeson
                    || File.Exists("vicmsdeson.txt"))
                {
                    string valor = "";

                    try
                    {
                        valor = "";
                        valor = Funcoes.InpuBox(null, " Valor do ICMS desonerado ", " Valor do ICMS desonerado no ITEM:" + ivenda.Nritem + " Descrição: " + ivenda.Descricao, valor);
                        if (!string.IsNullOrEmpty(valor))
                        {
                            icmsGeral.vICMSDeson = Convert.ToDecimal(valor); 
                            this.vendanfe.Vicmsdeson += icmsGeral.vICMSDeson.GetValueOrDefault(0);
                        }
                            
                    }
                    catch (Exception ex)
                    {
                        Funcoes.Crashe(ex, "ERRO - CODE: motDesICMS", true);
                    }

                    try
                    { 
                        string opcoes = "\n1 – Táxi; \r\n"
                           //+ "2 – Deficiente Físico (Revogado); \r\n"
                           + "3 – Produtor Agropecuário;\r\n"
                           + "4 – Frotista/Locadora; \r\n"
                           + "5 – Diplomático/Consular; \r\n"
                           + "6 – Utilitários e Motocicletas da Amazônia Ocidental e Áreas de Livre Comércio (Resolução 714/88 e 790/94 – CONTRAN e suas alterações); \r\n"
                           + "7 – SUFRAMA; \r\n"
                           + "8 – Venda a Órgãos Públicos \r\n"
                           + "9 – Outros. \r\n"
                           + "10 – Deficiente Condutor (Convênio ICMS 38/12); \r\n"
                           + "11 – Deficiente Não Condutor (Convênio ICMS 38/12)\r\n" 
                           + "16 – Olimpíadas Rio 2016;\r\n"
                           + "90 – Solicitado pelo Fisco\r\n";

                        if (cst == Csticms.Cst10 || cst == Csticms.Cst20 || cst == Csticms.Cst70 || cst == Csticms.Cst90)
                            opcoes = "\n3 – Produtor Agropecuário;\r\n"                          
                           + "9 – Outros. \r\n"
                           + "12 – Órgão de fomento e desenvolvimento agropecuário\r\n";

                        if (cst == Csticms.Cst30)
                            opcoes = "\n6 – Utilitários e Motocicletas da Amazônia Ocidental e Áreas de Livre Comércio (Resolução 714/88 e 790/94 – CONTRAN e suas alterações); \r\n"
                             + "7 – SUFRAMA; \r\n"                             
                             + "9 – Outros. \r\n";

                        valor = "";
                        valor = Funcoes.InpuBox(null, "Motivo da desoneração do ICMS/ICMS ",
                            "Digite o número do Motivo da desoneração do ICMS/ICMS no ITEM:" + ivenda.Nritem + " Descrição: " + ivenda.Descricao +
                            opcoes,
                            valor);

                        if (!string.IsNullOrEmpty(valor))
                            icmsGeral.motDesICMS = (MotivoDesoneracaoIcms?)Convert.ToInt32(valor);
                    }
                    catch (Exception ex)
                    {
                        Funcoes.Crashe(ex, "ERRO - CODE: motDesICMS", true);
                    }



                }

                return icmsGeral.ObterICMSBasico(crt);
            }

            catch (Exception ex)
            {
                Funcoes.Mensagem(" Por gentileza, verificar as informações apresentada no cadastro do item da nota como CST/CSOSN e ICMS." + Environment.NewLine +
                                  ex.Message, " ERRO - CODE: ObterIcmsBasic ", MessageBoxButton.OK);

                Funcoes.Crashe(ex, "ERRO - CODE: ObterIcmsBasic", false);
                return null;
            }

        }


        /// <summary>
        /// PAGAMENTO
        /// </summary>
        /// <param name="versao"></param>
        /// <param name="produtos"></param>
        /// <returns></returns>

        protected virtual total GetTotal(List<det> produtos)
        {
            var icmsTot = new ICMSTot
            {
                vProd = produtos.Sum(p => p.prod.vProd),
                vNF = vendanfe.Vnf,
                vDesc = produtos.Sum(p => p.prod.vDesc ?? 0),
                vTotTrib = produtos.Sum(p => p.imposto.vTotTrib ?? 0),

            };

            icmsTot.vPIS = vendanfe.Vpis;

            icmsTot.vOutro = vendanfe.Voutro;
            icmsTot.vCOFINS = vendanfe.Vconfins;
            icmsTot.vSeg = vendanfe.Vseg;
            icmsTot.vFrete = vendanfe.Vfrete;

            icmsTot.vIPI = vendanfe.Vipi;
            icmsTot.vII = vendanfe.Vii;

            icmsTot.vST = vendanfe.Vst;

            icmsTot.vICMS = vendanfe.Vicms;
            icmsTot.vBC = vendanfe.Vbc;
            icmsTot.vBCST = vendanfe.Vbcst;

            icmsTot.vICMSDeson = vendanfe.Vicmsdeson;
            icmsTot.vFCPUFDest = vendanfe.Vfcpufdest;
            icmsTot.vICMSUFDest = vendanfe.Vicmsufdest;
            icmsTot.vICMSUFRemet = vendanfe.Vicmsufremet;

            icmsTot.vFCP = vendanfe.Vfcp;
            icmsTot.vFCPST = vendanfe.Vfcpst;
            icmsTot.vFCPSTRet = vendanfe.Vfcpstret;

            icmsTot.vIPIDevol = vendanfe.Vipidevol;

            decimal vipidevol = produtos.Sum(p => (p.impostoDevol == null ? 0 : p.impostoDevol.IPI.vIPIDevol));
            if (vipidevol != vendanfe.Vipidevol)
            {
                vendanfe.Vipidevol = vipidevol;
                vendanfe.UpdateVipidevol();

                icmsTot.vIPIDevol = vendanfe.Vipidevol;

                vendanfe.Vnf = icmsTot.vProd
                       - icmsTot.vDesc
                       - icmsTot.vICMSDeson.GetValueOrDefault()
                       + icmsTot.vST
                       + icmsTot.vFCPST.GetValueOrDefault()
                       + icmsTot.vFrete
                       + icmsTot.vSeg
                       + icmsTot.vOutro
                       + icmsTot.vII
                       + icmsTot.vIPI
                       + icmsTot.vIPIDevol.GetValueOrDefault();

                vendanfe.UpdateValorNF();
                icmsTot.vNF = vendanfe.Vnf;

            }
             
            //icmsTot.vNF =
            //  icmsTot.vProd
            //  - icmsTot.vDesc
            //  - icmsTot.vICMSDeson.GetValueOrDefault()
            //  + icmsTot.vST
            //  + icmsTot.vFCPST.GetValueOrDefault()
            //  + icmsTot.vFrete
            //  + icmsTot.vSeg
            //  + icmsTot.vOutro
            //  + icmsTot.vII
            //  + icmsTot.vIPI
            //  + icmsTot.vIPIDevol.GetValueOrDefault();


            if (icmsTot.vNF !=
                icmsTot.vProd
                - icmsTot.vDesc
                - icmsTot.vICMSDeson.GetValueOrDefault()
                + icmsTot.vST
                + icmsTot.vFCPST.GetValueOrDefault()
                + icmsTot.vFrete
                + icmsTot.vSeg
                + icmsTot.vOutro
                + icmsTot.vII
                + icmsTot.vIPI
                + icmsTot.vIPIDevol.GetValueOrDefault())
            {
                //.. Funcoes.Mensagem("  Regra de validação W16-10 que rege sobre o Total da NF-e ", "RETORNO SEFAZ ", MessageBoxButton.OK);
                MinhaNotificacao.NotificarErro("RETORNO SEFAZ ", "Regra de validação que rege sobre o Total da NF-e. ");
            }

            var isTot  = new ISTot();
            isTot.vIS = vendanfe.VtotIs;
            
            var ibscbsTot = new IBSCBSTot();
            ibscbsTot.vBCIBSCBS = vendanfe.VtotBcIbscbs;
            ibscbsTot.gCBS = new gCBSTotal();
            ibscbsTot.gCBS.vDif = vendanfe.VtotDifCbs;
            ibscbsTot.gCBS.vDevTrib = vendanfe.VtotDevTribCbs;
            ibscbsTot.gCBS.vCredPres = vendanfe.VtotCredPres;
            ibscbsTot.gCBS.vCredPresCondSus = vendanfe.VtotCredPres;
            ibscbsTot.gMono = new gMono();
            ibscbsTot.gMono.vIBSMono = vendanfe.VtotIbsMono;
            ibscbsTot.gMono.vCBSMono = vendanfe.VtotCbsMono;
            ibscbsTot.gMono.vIBSMonoReten = vendanfe.VtotIbsMono;
            ibscbsTot.gMono.vCBSMonoReten = vendanfe.VtotCbsMono;
            ibscbsTot.gMono.vIBSMonoRet = vendanfe.VtotIbsMonoRet;
            ibscbsTot.gMono.vCBSMonoRet = vendanfe.VtotCbsMonoRet;
            
            var t = new total
            {
                ICMSTot = icmsTot, 
                ISTot = isTot,
                IBSCBSTot = ibscbsTot
            };
            return t;
        }

        protected virtual vol GetVolume()
        {
            var v = new vol
            {
                esp = "teste de especia",
                lacres = new List<lacres> { new lacres { nLacre = "123456" } }
            };

            return v;
        }

        protected virtual List<pag> GetPagamento(int numero, ICMSTot icmsTot, VersaoServico versao)
        {
            var listapagamento = new List<pag>();
            try
            {
                var valorPagto = Valor.Arredondar(icmsTot.vProd / 2, 2);
                VendaNFePG vendanfepg = new VendaNFePG();
                List<VendaNFePG> lista = vendanfepg.GetItensdePG(vendanfe.Nrvenda);
                pag pagamento = new pag
                {
                    detPag = new List<detPag>()
                };
                pagamento.vTroco = vendanfe.Vtroco;

                if (lista.Count > 0)
                    foreach (VendaNFePG i in lista)
                    {
                        if ((FormaPagamento)i.Formapg_id == FormaPagamento.fpDinheiro)
                        {

                            detPag detPag = new detPag()
                            {
                                tPag = FormaPagamento.fpDinheiro,
                                vPag = i.Total_forma,
                            };

                            if (vendanfe.Indpres != 5)
                            {
                                detPag.indPag = IndicadorPagamentoDetalhePagamento.ipDetPgVista;
                            }

                            pagamento.detPag.Add(detPag);

                        }
                        else
                         if ((FormaPagamento)i.Formapg_id == FormaPagamento.fpCartaoCredito || (FormaPagamento)i.Formapg_id == FormaPagamento.fpCartaoDebito)
                        {
                            card tcard = new card();
                            if (i.Autoriza == "") i.Autoriza = "";
                            tcard.cAut = i.Autoriza; // 8 - nsu
                            tcard.tpIntegra = TipoIntegracaoPagamento.TipNaoIntegrado;

                            tcard.tBand = (BandeiraCartao)i.Conveniopg_id;

                            detPag detPag = new detPag()
                            {
                                tPag = (FormaPagamento)i.Formapg_id,

                                vPag = i.Total_forma
                            };

                            if (tcard.cAut != "")
                            {
                                detPag.card = tcard;
                            }


                            if (vendanfe.Indpres != 5)
                            {
                                detPag.indPag = IndicadorPagamentoDetalhePagamento.ipDetPgPrazo;
                            }
                            pagamento.detPag.Add(detPag);
                        }
                        else if ((FormaPagamento)i.Formapg_id == FormaPagamento.fpSemPagamento)
                        {
                            pagamento = new pag
                            {
                                detPag = new List<detPag>
                                {
                                        new detPag
                                        {
                                            tPag = FormaPagamento.fpSemPagamento,
                                        }
                                }
                            };
                        }
                        else if ((FormaPagamento)i.Formapg_id == FormaPagamento.fpOutro)
                        {

                            if (String.IsNullOrEmpty(i.Descricaodomeiodepagamento))
                                i.Descricaodomeiodepagamento = Funcoes.InpuBox(null, "Descrição do Meio de Pagamento", "Digite o a Descrição do Meio de Pagamento, pois colocou OUTROS:");

                            detPag detPag = new detPag()
                            {
                                tPag = (FormaPagamento)i.Formapg_id,
                                vPag = i.Total_forma
                            };

                            if (i.Descricaodomeiodepagamento.ToUpper().Contains("PIX"))
                            {
                                if (MessageBox.Show("Foi verificado que voce colocou que o meio de pagamento foi PIX. " +
                                ". \n\n Deseja alterar para esse meio de pagamento(TPpag)?",
                                "MEIO DE PAGAMENTO", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                                {
                                    detPag.tPag = FormaPagamento.fpPagamentoInstantaneoPIXEstatico;
                                }
                                else
                                    detPag.xPag = i.Descricaodomeiodepagamento;
                            }
                            else
                            {
                                detPag.xPag = i.Descricaodomeiodepagamento;

                            }




                            if (i.Condicaopg_id == 1)
                                detPag.indPag = IndicadorPagamentoDetalhePagamento.ipDetPgVista;
                            else
                                detPag.indPag = IndicadorPagamentoDetalhePagamento.ipDetPgPrazo;

                            pagamento.detPag.Add(detPag);
                        }
                        else
                        {
                            detPag detPag = new detPag()
                            {
                                tPag = (FormaPagamento)i.Formapg_id,
                                vPag = i.Total_forma
                            };

                            if (vendanfe.Indpres != 5)
                            {
                                detPag.indPag = IndicadorPagamentoDetalhePagamento.ipDetPgPrazo;
                            }
                            pagamento.detPag.Add(detPag);

                        }
                    }
                else
                {
                    pagamento = new pag
                    {
                        detPag = new List<detPag>
                            {
                                new detPag {
                                    tPag = FormaPagamento.fpSemPagamento,
                                }
                        }
                    };
                }
                listapagamento.Add(pagamento);
                return listapagamento;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    MessageBox.Show(ex.Message + " Outros: " + ex.InnerException.Message, "Erro");
                else if (!string.IsNullOrEmpty(ex.Message))
                    Funcoes.Crashe(ex, "ERRO - CODE: GetPagamento");

                return listapagamento;
            }

        }

        protected virtual cobr GetCobranca()
        {
            var c = new cobr()
            {
                dup = new List<dup>()
            };
            //var valorParcela = (icmsTot.vNF / 2).Arredondar(2);
            //var c = new cobr
            //{
            //    fat = new fat { nFat = "12345678910", vLiq = icmsTot.vNF, vOrig = icmsTot.vNF, vDesc = 0m },
            //    dup = new List<dup>
            //    {
            //        new dup {nDup = "001", dVenc = DateTime.Now.AddDays(30), vDup = valorParcela},
            //        new dup {nDup = "002", dVenc = DateTime.Now.AddDays(60), vDup = icmsTot.vNF - valorParcela}
            //    }
            //};

            try
            {
                VendaNFePG vendanfepg = new VendaNFePG();
                List<VendaNFePG> lista = vendanfepg.GetItensdePG(vendanfe.Nrvenda);
                int contador = 0;
                decimal valorliq = 0;
                if (lista.Count > 0)
                    foreach (VendaNFePG i in lista)
                    {
                        if ((FormaPagamento)i.Formapg_id == FormaPagamento.fpOutro
                            || (FormaPagamento)i.Formapg_id == FormaPagamento.fpDuplicataMercantil
                            || (FormaPagamento)i.Formapg_id == FormaPagamento.fpCheque
                            || (FormaPagamento)i.Formapg_id == FormaPagamento.fpBoletoBancario)
                        {
                            contador++;
                            var nDupe = new dup { nDup = contador.ToString("000"), dVenc = i.Data, vDup = i.Total_forma };
                            c.dup.Add(nDupe);
                            valorliq += i.Total_forma;
                        }
                    }
                if (contador > 0)
                    c.fat = new fat { nFat = "001", vLiq = valorliq, vOrig = valorliq, vDesc = 0m };
                return c;
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ERRO - CODE: GETCOBRANCA");
                return c;
            }
        }

        private CSTIPI RetornaIPI(int ipi)
        {
            if (ipi == 0) return CSTIPI.ipi00;
            if (ipi == 49) return CSTIPI.ipi49;
            if (ipi == 50) return CSTIPI.ipi50;
            if (ipi == 99) return CSTIPI.ipi99;
            if (ipi == 01) return CSTIPI.ipi01;
            if (ipi == 02) return CSTIPI.ipi02;
            if (ipi == 03) return CSTIPI.ipi03;
            if (ipi == 04) return CSTIPI.ipi04;
            if (ipi == 05) return CSTIPI.ipi05;

            if (ipi == 51) return CSTIPI.ipi51;
            if (ipi == 52) return CSTIPI.ipi52;
            if (ipi == 53) return CSTIPI.ipi53;
            if (ipi == 54) return CSTIPI.ipi54;
            if (ipi == 55) return CSTIPI.ipi55;
            return CSTIPI.ipi00;
        }

    }
}
