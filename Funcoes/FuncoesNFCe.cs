using NFe.Classes.Informacoes.Pagamento;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DFe.Classes.Entidades;
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
using NFe.Utils.Tributacao.Estadual;
using System.Reflection;
using NFe.Classes.Informacoes.Observacoes;
using NFe.Servicos.Retorno;
using NFe.Utils.Email;
using NFe.Classes.Servicos.Consulta;
using NFe.Classes.Servicos.ConsultaCadastro;
using NFe.Utils.Consulta;
using Shared.NFe.Classes.Informacoes.InfRespTec;
using Shared.NFe.Utils.InfRespTec;
using NFe.Classes.Informacoes.Detalhe.ProdEspecifico;
using NFe.Utils.Tributacao.Federal;
using NFe.Utils.InformacoesSuplementares;
using NFe.Classes.Servicos.Inutilizacao;
using NFe.Classes.Informacoes.Total.IbsCbs;
using NFe.Classes.Informacoes.Total.IbsCbs.Cbs;
using NFe.Classes.Informacoes.Total.IbsCbs.Ibs;
using NFe.Classes.Informacoes.Total.IbsCbs.Monofasica;
using System.Net;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Compartilhado;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Compartilhado.InformacoesIbsCbs;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Compartilhado.InformacoesIbsCbs.InformacoesCbs;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Compartilhado.InformacoesIbsCbs.InformacoesIbs;
using CST = NFe.Classes.Informacoes.Detalhe.Tributacao.Compartilhado.Tipos.CST;

namespace nfecreator
{
    class FuncoesNFCe
    {
        private const string ArquivoConfiguracao = @"\configuracao.xml";
        private static ConfiguracaoApp _configuracoes;
        private static readonly string _path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private CRT crtcliente;
        private ServicosNFe servicoNFe;
        private VendaNFCe vendanfce;
        private Vendanfcea vendanfcea;

        private static void CarregarConfiguracao()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            try
            {
                _configuracoes = !File.Exists(path + ArquivoConfiguracao)
                    ? new ConfiguracaoApp()
                    : FuncoesXml.ArquivoXmlParaClasse<ConfiguracaoApp>(path + ArquivoConfiguracao);
                if (_configuracoes.CfgServico.TimeOut == 0)
                    _configuracoes.CfgServico.TimeOut = 6000; //mínimo

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

        public static int StatusdeServicos(Boolean readmessage = true)
        {
            try
            {
                int valor = 0;

                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                _configuracoes = !File.Exists(path + ArquivoConfiguracao)
                        ? new ConfiguracaoApp()
                        : FuncoesXml.ArquivoXmlParaClasse<ConfiguracaoApp>(path + ArquivoConfiguracao);
                if (_configuracoes.CfgServico.TimeOut == 0)
                    _configuracoes.CfgServico.TimeOut = 13000; //mínimo
                                                               //configuracoes.CfgServico.tpAmb = TipoAmbiente.Homologacao;

                _configuracoes.CfgServico.ModeloDocumento = ModeloDocumento.NFCe;
                using (var servicoNFe = new ServicosNFe(_configuracoes.CfgServico))
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

        public string Verificar(VendaNFCe vend, Vendanfcea vnfcea)
        {
            try
            {
                #region Consulta Recibo de lote
                vendanfce = vend;
                vendanfcea = vnfcea;

                var chave = vendanfcea.Chave.Replace("NFe", "");
                CarregarConfiguracao();
                ConfiguracaoApp configuracoes = _configuracoes;
                configuracoes.CfgServico.ModeloDocumento = ModeloDocumento.NFCe;

                if (string.IsNullOrEmpty(chave)) throw new Exception("A Chave deve ser informada!");

                ServicosNFe servicoNFe = new ServicosNFe(configuracoes.CfgServico);
                RetornoNfeConsultaProtocolo retornoRecibo = servicoNFe.NfeConsultaProtocolo(chave);

                string Motivo = retornoRecibo.Retorno.xMotivo ?? retornoRecibo.Retorno.protNFe.infProt.xMotivo;
                int valorStat = retornoRecibo.Retorno.cStat != 0 ? retornoRecibo.Retorno.cStat : retornoRecibo.Retorno.protNFe.infProt.cStat;
                string protocolo = retornoRecibo.Retorno.protNFe != null ? retornoRecibo.Retorno.protNFe.infProt.nProt : "";
                if (valorStat != 217 && valorStat != 226 && valorStat != 999 && valorStat != 526)
                    protocolo = retornoRecibo.Retorno.protNFe.infProt.nProt ?? "";

                if (valorStat == 100)
                {
                    vendanfcea.Chave = chave;
                    vendanfcea.Statusnfe = "APROVADO";
                    vendanfcea.Nprotocolo = protocolo;
                    vendanfcea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                    vendanfcea.Update();

                    NFe.Classes.NFe _nfe = GetNf(vendanfce.Nrvenda, ModeloDocumento.NFCe, configuracoes.CfgServico.VersaoNFeAutorizacao);
                    _nfe.Assina(); //não precisa validar aqui, pois o lote será validado em ServicosNFe.NFeAutorizacao

                    _nfe.infNFeSupl = new infNFeSupl();

                    _nfe.infNFeSupl.urlChave = _nfe.infNFeSupl.ObterUrlConsulta(_nfe, VersaoQrCode.QrCodeVersao1);

                    _nfe.infNFeSupl.qrCode = _nfe.infNFeSupl.ObterUrlQrCode(_nfe, _configuracoes.ConfiguracaoDanfeNfce.VersaoQrCode, _configuracoes.ConfiguracaoCsc.CIdToken, _configuracoes.ConfiguracaoCsc.Csc);

                    FuncoesFTP.GuardaXML(_nfe.ObterXmlString(), _path + @"\NFCe\Autorizada\", _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe");
                    var local = _path + @"\NFCe\Autorizada\" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml";

                    var proc = new nfeProc
                    {
                        NFe = new NFe.Classes.NFe().CarregarDeArquivoXml(local),
                        protNFe = retornoRecibo.Retorno.protNFe,
                        versao = retornoRecibo.Retorno.versao
                    };
                    FuncoesFTP.GuardaXML(proc.ObterXmlString(), _path + @"\NFCE\Autorizada\", vendanfcea.Chave.Replace("NFe", "") + "-procNFe");
                    string localxml = FuncoesFTP.SubirArquivo(local, _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml", "nfce");

                    Notasemitidas notaemitida = new Notasemitidas()
                    {
                        Chave = _nfe.infNFe.Id,
                        Modelo = (int)_nfe.infNFe.ide.mod,
                        Cnpj = _configuracoes.Emitente.CNPJ,
                        Data = DateTime.Now,
                        Valor = vendanfce.Vnf,
                        Localxml = localxml,
                    };
                    notaemitida.Insert();


                    GerarPdf(vend, vendanfcea);
                    if (_configuracoes.ConfiguracaoDanfeNfe.EmailAut == "SIM") Email(vendanfce, vendanfcea);
                }
                else if (valorStat == 101 ||
                    valorStat == 135 ||
                    valorStat == 151 ||
                    valorStat == 155)
                {
                    protocolo = retornoRecibo.Retorno.protNFe != null ? retornoRecibo.Retorno.procEventoNFe.First().retEvento.infEvento.nProt : protocolo;

                    vendanfcea.Statusnfe = "CANCELADO";
                    vendanfcea.Nprotocolo = protocolo;
                    vendanfcea.Danfe = chave.Replace("NFe", "") + "-procNFe.pdf";
                    vendanfcea.Xml = chave.Replace("NFe", "") + "-procNFe.xml";
                    vendanfcea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                    vendanfcea.Update();

                    Notasemitidas notaemitida = new Notasemitidas()
                    {
                        Chave = vendanfcea.Chave.Replace("NFe", ""),
                        Modelo = 55,
                        Cnpj = _configuracoes.Emitente.CNPJ,
                        Data = DateTime.Now,
                        Valor = vendanfce.Vnf,
                    };

                    notaemitida.Insert();

                    GerarPdf(vendanfce, vendanfcea);

                }
                else if (valorStat == 110 ||
                    valorStat == 301 ||
                    valorStat == 302 ||
                    valorStat == 303)
                {
                    vendanfcea.Statusnfe = "DENEGADO";
                    vendanfcea.Nprotocolo = protocolo;
                    vendanfcea.Danfe = chave.Replace("NFe", "") + "-procNFe.pdf";
                    vendanfcea.Xml = chave.Replace("NFe", "") + "-procNFe.xml";
                    vendanfcea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                    vendanfcea.Update();

                    Notasemitidas notaemitida = new Notasemitidas()
                    {
                        Chave = vendanfcea.Chave.Replace("NFe", ""),
                        Modelo = 55,
                        Cnpj = _configuracoes.Emitente.CNPJ,
                        Data = DateTime.Now,
                        Valor = vendanfce.Vnf,
                    };
                    notaemitida.Insert();

                    GerarPdf(vendanfce, vendanfcea);
                }
                else
                {
                    vendanfcea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                    vendanfcea.Statusnfe = "REJEITADO";
                    vendanfcea.Update();
                }
                #endregion

                return retornoRecibo.RetornoStr;
            }
            catch (ComunicacaoException ex)
            {
                Funcoes.Crashe(ex, "", false);
                Funcoes.Mensagem(ex.Message + Environment.NewLine +
                    " Tente novamente, se o erro continuar:  " + Environment.NewLine +
                    " Por gentileza, verificar o Disponibilidade da SEFAZ.", "RETORNO SEFAZ ", MessageBoxButton.OK);
                return null;
            }
            catch (ValidacaoSchemaException ex)
            {
                Funcoes.Crashe(ex, "", false);
                Funcoes.Mensagem(ex.Message, "Erro -  Cod:ValidacaoSchemaException", MessageBoxButton.OK);
                return null;
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ERRO DA SEFAZ");
                return null;
            }
        }

        public string ConsultaRecibo(VendaNFCe venda, Vendanfcea vnfcea, string recibo)
        {
            try
            {
                #region Consulta Recibo de lote
                CarregarConfiguracao();
                vendanfcea = vnfcea;

                if (string.IsNullOrEmpty(recibo)) throw new Exception("A recibo deve ser informada!");

                var servicoNFe = new ServicosNFe(_configuracoes.CfgServico);
                RetornoNFeRetAutorizacao retornoRecibo = servicoNFe.NFeRetAutorizacao(recibo);

                string Motivo = retornoRecibo.Retorno.xMotivo != null ? retornoRecibo.Retorno.protNFe.First().infProt.xMotivo : retornoRecibo.Retorno.xMotivo;
                int valorStat = retornoRecibo.Retorno.cStat != 0 ? retornoRecibo.Retorno.protNFe.First().infProt.cStat : retornoRecibo.Retorno.cStat;
                if (valorStat == 100)
                {
                    string protocolo = retornoRecibo.Retorno.protNFe.First().infProt.nProt;
                    if (vendanfcea.Statusnfe != "APROVADO")
                    {
                        vendanfcea.Statusnfe = "APROVADO";
                        vendanfcea.Nprotocolo = protocolo;
                        // vendanfcea.Chave = _nfe.infNFe.Id.Replace("NFe", "");
                        vendanfcea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                        vendanfcea.Update();

                        vendanfce = venda;

                        NFe.Classes.NFe _nfe = GetNf(venda.Nrvenda, _configuracoes.CfgServico.ModeloDocumento, _configuracoes.CfgServico.VersaoNFeAutorizacao);
                        _nfe.Assina(); //não precisa validar aqui, pois o lote será validado em ServicosNFe.NFeAutorizacao
                        FuncoesFTP.GuardaXML(_nfe.ObterXmlString(), _path + @"\NFCe\Autorizada\", _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe");
                        var local = _path + @"\NFCe\Autorizada\" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml";



                        var proc = new nfeProc
                        {
                            NFe = new NFe.Classes.NFe().CarregarDeArquivoXml(local),
                            protNFe = retornoRecibo.Retorno.protNFe.First(),
                            versao = retornoRecibo.Retorno.versao
                        };

                        FuncoesFTP.GuardaXML(proc.ObterXmlString(), _path + @"\NFCe\Autorizada\", _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe");

                        string localxml = FuncoesFTP.SubirArquivo(local, _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml", "nfce");

                        Notasemitidas notaemitida = new Notasemitidas()
                        {
                            Chave = _nfe.infNFe.Id,
                            Modelo = (int)_nfe.infNFe.ide.mod,
                            Cnpj = _configuracoes.Emitente.CNPJ,
                            Data = DateTime.Now,
                            Valor = vendanfce.Vnf,
                            Localxml = localxml,
                        };
                        notaemitida.Insert();


                        string localsave = _path + @"\NFCe\Autorizada\" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.pdf";
                        try
                        {
                            proc = new nfeProc().CarregarDeArquivoXml(local);
                        }
                        catch //Carregar NFe ainda não transmitida à sefaz, como uma pré-visualização.
                        {
                            proc = new nfeProc() { NFe = new NFe.Classes.NFe().CarregarDeArquivoXml(local), protNFe = new NFe.Classes.Protocolo.protNFe() };
                        }

                        bool parametrotributos = false;
                        if (App.Parametro.Calculatrib == "SIM") parametrotributos = true;

                        var danfe = new DanfeFrNfe(proc: proc,
                                           configuracaoDanfeNfe: new ConfiguracaoDanfeNfe()
                                           {
                                               Logomarca = _configuracoes.ConfiguracaoDanfeNfe.Logomarca,
                                               DuasLinhas = false,
                                               DocumentoCancelado = false,
                                               QuebrarLinhasObservacao = _configuracoes.ConfiguracaoDanfeNfe.QuebrarLinhasObservacao,
                                               ExibirResumoCanhoto = true,
                                               ResumoCanhoto = string.Empty,
                                               ChaveContingencia = string.Empty,
                                               ExibeCampoFatura = false,
                                               ImprimirISSQN = true,
                                               ImprimirDescPorc = true,
                                               ImprimirTotalLiquido = false,
                                               ImprimirUnidQtdeValor = ImprimirUnidQtdeValor.Comercial,
                                               ExibirTotalTributos = parametrotributos,
                                           },
                                           desenvolvedor: "CIAF SOLUÇÕES EM SOFTWARE - TESCHE E VASCONCELOS LTDA",
                                           arquivoRelatorio: string.Empty);
                        danfe.ExportarPdf(localsave);
                        if (_configuracoes.CfgServico.DiretorioSalvarXml.Replace(" ", "") != "" && _configuracoes.CfgServico.DiretorioSalvarXml != null)
                        {
                            try
                            {
                                danfe.ExportarPdf(_configuracoes.CfgServico.DiretorioSalvarXml + @"\" + _nfe.infNFe.Id + "-procNFe.pdf");
                            }
                            catch
                            { }
                        }
                        GerarPdf(venda, vendanfcea);

                        if (!string.IsNullOrEmpty(_configuracoes.ConfiguracaoDanfeNfe.EmailAut))
                            if (_configuracoes.ConfiguracaoDanfeNfe.EmailAut == "SIM")
                            {
                                try
                                {
                                    if (!string.IsNullOrEmpty(_nfe.infNFe.dest.email))
                                        Email(vendanfce, vendanfcea, _nfe.infNFe.dest.email);
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

                    if (venda.Status != "CANCELADO")
                    {
                        string protocolo = retornoRecibo.Retorno.protNFe.First().infProt.nProt;
                        vendanfcea.Statusnfe = "CANCELADO";
                        vendanfcea.Nprotocolo = protocolo;
                        vendanfcea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                        vendanfcea.Update();

                    }
                }
                else if (valorStat == 110 ||
                    valorStat == 301 ||
                    valorStat == 302 ||
                    valorStat == 303)
                {
                    string protocolo = retornoRecibo.Retorno.protNFe.First().infProt.nProt;
                    vendanfcea.Statusnfe = "DENEGADO";
                    vendanfcea.Nprotocolo = protocolo;
                    vendanfcea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                    vendanfcea.Update();

                    vendanfce = venda;
                    NFe.Classes.NFe _nfe = GetNf(venda.Nrvenda, _configuracoes.CfgServico.ModeloDocumento, _configuracoes.CfgServico.VersaoNFeAutorizacao);
                    _nfe.Assina(); //não precisa validar aqui, pois o lote será validado em ServicosNFe.NFeAutorizacao
                    FuncoesFTP.GuardaXML(_nfe.ObterXmlString(), _path + @"\NFCe\Autorizada\", _nfe.infNFe.Id.Replace("NFe", ""));
                    var local = _path + @"\NFCe\Autorizada\" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml";

                    var proc = new nfeProc
                    {
                        NFe = new NFe.Classes.NFe().CarregarDeArquivoXml(local),
                        protNFe = retornoRecibo.Retorno.protNFe.First(),
                        versao = retornoRecibo.Retorno.versao
                    };
                    FuncoesFTP.GuardaXML(proc.ObterXmlString(), _path + @"\NFCe\Autorizada\", _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe");
                    string localxml = FuncoesFTP.SubirArquivo(local, _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml", "nfce");

                    Notasemitidas notaemitida = new Notasemitidas()
                    {
                        Chave = _nfe.infNFe.Id,
                        Modelo = (int)_nfe.infNFe.ide.mod,
                        Cnpj = _configuracoes.Emitente.CNPJ,
                        Data = DateTime.Now,
                        Valor = vendanfce.Vnf,
                        Localxml = localxml,
                    };
                    notaemitida.Insert();

                }
                else
                {
                    vendanfcea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                    vendanfcea.Statusnfe = "REJEITADO";
                    vendanfcea.Update();

                }

                return retornoRecibo.RetornoStr;
                #endregion
            }
            catch (ComunicacaoException ex)
            {
                Funcoes.Mensagem(ex.Message + Environment.NewLine +
                    " Tente novamente, se o erro continuar:  " + Environment.NewLine +
                    " Por gentileza, verificar o Disponibilidade da SEFAZ ou entre em contato com suporte.", "RETORNO SEFAZ ", MessageBoxButton.OK);
                return null;
            }
            catch (ValidacaoSchemaException ex)
            {
                Funcoes.Mensagem(ex.Message, "ERRO DA SEFAZ - RECIBO", MessageBoxButton.OK);
                return null;
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ERRO DA SEFAZ - RECIBO");
                return null;
            }

        }

        public string Autoriza(VendaNFCe vendac, Vendanfcea vcna)
        {
            vendanfce = vendac;
            vendanfcea = vcna;
            NFe.Classes.NFe _nfe;
            try
            {
                //#region Cria e Envia NFe
                CarregarConfiguracao();

                _configuracoes.CfgServico.VersaoLayout = VersaoServico.Versao400;
                _configuracoes.CfgServico.ModeloDocumento = ModeloDocumento.NFCe;

                _nfe = GetNf(vendanfce.Nrvenda, ModeloDocumento.NFCe, _configuracoes.CfgServico.VersaoNFeAutorizacao);
                _nfe.Assina();

                _nfe.infNFeSupl = new infNFeSupl();
                _nfe.infNFeSupl.urlChave = _nfe.infNFeSupl.ObterUrlConsulta(_nfe, VersaoQrCode.QrCodeVersao2);
                _nfe.infNFeSupl.qrCode = _nfe.infNFeSupl.ObterUrlQrCode(_nfe, _configuracoes.ConfiguracaoDanfeNfce.VersaoQrCode, _configuracoes.ConfiguracaoCsc.CIdToken, _configuracoes.ConfiguracaoCsc.Csc);

                _nfe.Valida();

                if (App.Parametro.Uf == "MT" || App.Parametro.Uf == "MS")
                {
                    _nfe = new NFe.Classes.NFe().CarregarDeXmlString(Funcoes.RemoverAcentos(_nfe.ObterXmlString()));
                    _nfe.Assina();
                }

                vendanfcea.Chave = _nfe.infNFe.Id.Replace("NFe", "");
                //vecna.Qrcode = _nfe.infNFeSupl.qrCode;
                //vecna.Urlchave = _nfe.infNFeSupl.urlChave;
                vendanfcea.Update();

                FuncoesFTP.GuardaXML(_nfe.ObterXmlString(), _path + @"\NFCE\", "VERIFICAR" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe");
                //FuncoesFTP.SubirArquivo(_path + @"\NFCE\VERIFICAR" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml", "VERIFICAR-" + _nfe.infNFe.Id + "-procNFe.xml");
                //FuncoesFTP.DeleteArquivoLocal(_path + @"\NFCE\VERIFICAR" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml");

                _configuracoes.CfgServico.ModeloDocumento = ModeloDocumento.NFCe;
                ServicosNFe servicoNFe = new ServicosNFe(_configuracoes.CfgServico);

                RetornoNFeAutorizacao retornoEnvio = servicoNFe.NFeAutorizacao(1, IndicadorSincronizacao.Sincrono, new List<NFe.Classes.NFe> { _nfe }, false /*Envia a mensagem compactada para a SEFAZ*/);

                string Motivo = retornoEnvio.Retorno.protNFe != null ? retornoEnvio.Retorno.protNFe.infProt.xMotivo : retornoEnvio.Retorno.xMotivo;
                int valorStat = retornoEnvio.Retorno.protNFe != null ? retornoEnvio.Retorno.protNFe.infProt.cStat : retornoEnvio.Retorno.cStat;

                if (valorStat == 100)
                {
                    vendanfcea.Statusnfe = "APROVADO";
                    vendanfcea.Chave = _nfe.infNFe.Id.Replace("NFe", "");
                    vendanfcea.Nprotocolo = retornoEnvio.Retorno.protNFe.infProt.nProt;
                    vendanfcea.Danfe = _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.pdf";
                    vendanfcea.Xml = _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml";
                    vendanfcea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                    vendanfcea.Update();

                    vendanfce.Status = "APROVADO";
                    vendanfce.Update();

                    var proc = new nfeProc
                    {
                        NFe = _nfe,
                        protNFe = retornoEnvio.Retorno.protNFe,
                        versao = retornoEnvio.Retorno.versao
                    };

                    FuncoesFTP.GuardaXML(proc.ObterXmlString(), _path + @"\NFCE\Autorizada\", _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe");

                    string local = _path + @"\NFCE\Autorizada\" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml";
                    string localsave = _path + @"\NFCE\Autorizada\" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.pdf";
                    try
                    {
                        proc = new nfeProc().CarregarDeArquivoXml(local);
                    }
                    catch //Carregar NFe ainda não transmitida à sefaz, como uma pré-visualização.
                    {
                        proc = new nfeProc() { NFe = new NFe.Classes.NFe().CarregarDeArquivoXml(local), protNFe = new NFe.Classes.Protocolo.protNFe() };
                    }

                    GerarPdf(vendanfce, vendanfcea);
                    string localxml = FuncoesFTP.SubirArquivo(local, _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml", "nfce");

                    Notasemitidas notaemitida = new Notasemitidas()
                    {
                        Chave = _nfe.infNFe.Id,
                        Modelo = (int)_nfe.infNFe.ide.mod,
                        Cnpj = _configuracoes.Emitente.CNPJ,
                        Data = DateTime.Now,
                        Valor = vendanfce.Vnf,
                        Localxml = localxml,
                    };
                    notaemitida.Insert();

                    if (!string.IsNullOrEmpty(_configuracoes.ConfiguracaoDanfeNfe.EmailAut))
                        if (_configuracoes.ConfiguracaoDanfeNfe.EmailAut == "SIM")
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(_nfe.infNFe.dest.email))
                                    Email(vendanfce, vendanfcea, _nfe.infNFe.dest.email);
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
                    vendanfcea.Statusnfe = "DENEGADO";
                    vendanfcea.Chave = _nfe.infNFe.Id.Replace("NFe", "");
                    vendanfcea.Nprotocolo = retornoEnvio.Retorno.protNFe.infProt.nProt;
                    vendanfcea.Danfe = _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.pdf";
                    vendanfcea.Xml = _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml";
                    vendanfcea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                    vendanfcea.Update();


                    Notasemitidas notaemitida = new Notasemitidas()
                    {
                        Chave = _nfe.infNFe.Id.Replace("NFe", ""),
                        Modelo = (int)_nfe.infNFe.ide.mod,
                        Cnpj = _configuracoes.Emitente.CNPJ,
                        Data = DateTime.Now,
                        Valor = vendanfce.Vnf,
                    };
                    notaemitida.Insert();

                    var proc = new nfeProc
                    {
                        NFe = _nfe,
                        protNFe = retornoEnvio.Retorno.protNFe,
                        versao = retornoEnvio.Retorno.versao
                    };

                    FuncoesFTP.GuardaXML(proc.ObterXmlString(), _path + @"\NFCe\Autorizada\", _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe");
                    GerarPdf(vendanfce, vendanfcea);

                    string local = _path + @"\NFCE\Autorizada\" + vendanfcea.Chave + "-procNFe.xml";
                    string localsave = _path + @"\NFCE\Autorizada\" + vendanfcea.Chave + "-procNFe.pdf";

                    FuncoesFTP.SubirArquivo(local, _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml", "nfce");

                }
                else if (valorStat == 103)
                {
                    MinhaNotificacao.NotificarAviso("SEFAZ INFORMA", "Lote recebido com sucesso");
                    ConsultaRecibo(vendanfce, vendanfcea, retornoEnvio.Retorno.infRec.nRec);
                }
                else
                {
                    vendanfcea.Statusnfe = "REJEITADO";
                    //vendanfcea.Nprotocolo = retornoEnvio.Retorno.protNFe.infProt.nProt;
                    vendanfcea.Chave = _nfe.infNFe.Id.Replace("NFe", "");
                    vendanfcea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                    vendanfcea.Merro = Motivo;
                    vendanfcea.Update();

                    vendanfce.Status = "REJEITADO";
                    vendanfce.Update();

                }


                return retornoEnvio.RetornoStr;
            }
            catch (ComunicacaoException ex)
            {
                //Faça o tratamento de contingência OffLine aqui.
                if (MessageBox.Show(ex.Message.Trim() + Environment.NewLine +
                                      " Tente novamente, se o erro continuar:  " + Environment.NewLine +
                                      " Por gentileza, verificar o Disponibilidade da SEFAZ ou entre em contato com suporte. " + Environment.NewLine + Environment.NewLine +
                                      " Deseja emitir essa nota em contingência automaticamente? ", "SERVIDORES DA SEFAZ FORA DO AR!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {

                        string uf = _configuracoes.EnderecoEmitente.UF.ToString();
                        if (uf == "AC" || uf == "AL" || uf == "AP" || uf == "DF" || uf == "ES" || uf == "MG" || uf == "PA" || uf == "PB" || uf == "SP" ||
                           uf == "PI" || uf == "RJ" || uf == "RN" || uf == "RO" || uf == "RR" || uf == "RS" || uf == "SC" || uf == "SE" || uf == "TO")
                        {

                            vendanfce.TpEmis = 6;
                            vendanfce.Datacontingencia = DateTime.Now;
                            vendanfce.Justifica = "NOTA EMITIDA EM CONTIGENCIA POIS NAO OBTEVE RETORNO DA SEFAZ DO ESTADO";

                            _configuracoes.CfgServico.tpEmis = TipoEmissao.teSVCAN;
                            servicoNFe = new ServicosNFe(_configuracoes.CfgServico);
                        }
                        if (uf == "AM" || uf == "BA" || uf == "CE" || uf == "GO" || uf == "MA" || uf == "MS" || uf == "MT" || uf == "PE" || uf == "PR")
                        {
                            vendanfce.TpEmis = 7;
                            vendanfce.Datacontingencia = DateTime.Now;
                            vendanfce.Justifica = "NOTA EMITIDA EM CONTIGENCIA POIS NAO OBTEVE RETORNO DA SEFAZ DO ESTADO";

                            _configuracoes.CfgServico.tpEmis = TipoEmissao.teSVCRS;
                            servicoNFe = new ServicosNFe(_configuracoes.CfgServico);
                        }


                        _nfe = GetNf(vendanfce.Nrvenda, ModeloDocumento.NFe, _configuracoes.CfgServico.VersaoNFeAutorizacao);
                        _nfe.Assina();

                        RetornoNFeAutorizacao retornoEnvio = servicoNFe.NFeAutorizacao(1, IndicadorSincronizacao.Sincrono, new List<NFe.Classes.NFe> { _nfe }, false /*Envia a mensagem compactada para a SEFAZ*/);

                        string Motivo = retornoEnvio.Retorno.protNFe != null ? retornoEnvio.Retorno.protNFe.infProt.xMotivo : retornoEnvio.Retorno.xMotivo;
                        int valorStat = retornoEnvio.Retorno.protNFe != null ? retornoEnvio.Retorno.protNFe.infProt.cStat : retornoEnvio.Retorno.cStat;

                        FuncoesFTP.GuardaXML(_nfe.ObterXmlString(), _path + @"\NF_Autorizada\", _nfe.infNFe.Id + "-procNFe");

                        if (valorStat == 100)
                        {
                            vendanfcea.Chave = _nfe.infNFe.Id.Replace("NFe", "");
                            vendanfcea.Statusnfe = "APROVADO";
                            vendanfcea.Nprotocolo = retornoEnvio.Retorno.protNFe.infProt.nProt;
                            vendanfcea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                            vendanfcea.Update();

                            var proc = new nfeProc
                            {
                                NFe = _nfe,
                                protNFe = retornoEnvio.Retorno.protNFe,
                                versao = retornoEnvio.Retorno.versao
                            };

                            FuncoesFTP.GuardaXML(proc.ObterXmlString(), _path + @"\NF_Autorizada\", _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe");

                            string local = _path + @"\NF_Autorizada\" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml";
                            string localsave = _path + @"\NF_Autorizada\" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.pdf";
                            try
                            {
                                proc = new nfeProc().CarregarDeArquivoXml(local);
                            }
                            catch //Carregar NFe ainda não transmitida à sefaz, como uma pré-visualização.
                            {
                                proc = new nfeProc() { NFe = new NFe.Classes.NFe().CarregarDeArquivoXml(local), protNFe = new NFe.Classes.Protocolo.protNFe() };
                            }

                            GerarPdf(vendanfce, vendanfcea);

                            string localxml = FuncoesFTP.SubirArquivo(local, _nfe.infNFe.Id.Replace("NFe", "") + ".xml", "nfce");

                            Notasemitidas notaemitida = new Notasemitidas()
                            {
                                Chave = _nfe.infNFe.Id,
                                Modelo = (int)_nfe.infNFe.ide.mod,
                                Cnpj = _configuracoes.Emitente.CNPJ,
                                Data = DateTime.Now,
                                Valor = vendanfce.Vnf,
                                Localxml = localxml,
                            };
                            notaemitida.Insert();

                        }
                        else if (valorStat == 110 ||
                            valorStat == 301 ||
                            valorStat == 302 ||
                            valorStat == 303)
                        {
                            vendanfcea.Chave = _nfe.infNFe.Id.Replace("NFe", "");
                            vendanfcea.Statusnfe = "DENEGADO";
                            vendanfcea.Nprotocolo = retornoEnvio.Retorno.protNFe.infProt.nProt;
                            vendanfcea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                            vendanfcea.Update();


                            Notasemitidas notaemitida = new Notasemitidas()
                            {
                                Chave = _nfe.infNFe.Id,
                                Modelo = (int)_nfe.infNFe.ide.mod,
                                Cnpj = _configuracoes.Emitente.CNPJ,
                                Data = DateTime.Now,
                                Valor = vendanfce.Vnf,
                            };
                            notaemitida.Insert();

                            var proc = new nfeProc
                            {
                                NFe = _nfe,
                                protNFe = retornoEnvio.Retorno.protNFe,
                                versao = retornoEnvio.Retorno.versao
                            };

                            FuncoesFTP.GuardaXML(proc.ObterXmlString(), _path + @"\NFE\NF_Autorizada\", vendanfcea.Chave + "-procNFe");

                            ///Imprimir 
                            ///
                            GerarPdf(vendanfce, vendanfcea);

                            string local = _path + @"\NFE\NF_Autorizada\" + vendanfcea.Chave + ".xml";
                            string localsave = _path + @"\NFE\NF_Autorizada\NFe" + vendanfcea.Chave + ".pdf";
                            //nfeProc proc = null;

                            FuncoesFTP.SubirArquivo(local, _nfe.infNFe.Id.Replace("NFe", "") + ".xml", "nfce");

                        }
                        else if (valorStat == 103)
                        {
                            MinhaNotificacao.NotificarAviso("SEFAZ INFORMA", "Lote recebido com sucesso");
                            ConsultaRecibo(vendanfce, vendanfcea, retornoEnvio.Retorno.infRec.nRec);
                        }
                        else
                        {
                            vendanfcea.Chave = _nfe.infNFe.Id.Replace("NFe", "");
                            vendanfcea.Statusnfe = "REJEITADO";
                            vendanfcea.Ambiente = ((int)_configuracoes.CfgServico.tpAmb).ToString();
                            vendanfcea.Update();


                        }

                        return retornoEnvio.RetornoStr;
                    }
                    catch (Exception exx)
                    {
                        vendanfcea.Statusnfe = "REJEITADO";

                        Funcoes.Crashe(exx, "", false);
                        if (ex.InnerException != null)
                            MessageBox.Show(exx.Message + " Outros: " + ex.InnerException.Message, "Erro");
                        else if (!string.IsNullOrEmpty(ex.Message))
                            MessageBox.Show(exx.Message, "Erro");

                        return null;
                    }
                }
                else
                {
                    return null;
                }

            }
            catch (ValidacaoSchemaException ex)
            {
                vendanfcea.Statusnfe = "REJEITADO";

                Funcoes.Mensagem(ex.Message + Environment.NewLine +
                                    " Por gentileza, verificar as informações apresentada.", "RETORNO SEFAZ de XML ", MessageBoxButton.OK);
                Funcoes.Crashe(ex, "", false);

                return null;
            }
            catch (Exception ex)
            {
                vendanfcea.Statusnfe = "PENDENTE";

                Funcoes.Crashe(ex, "", false);
                if (ex.InnerException != null)
                    Funcoes.Mensagem(ex.Message + " Outros: " + ex.InnerException.Message +
                                   " Por gentileza, verificar as informações apresentada.", "Erro PROCESSAMENTO - NFC-e COD:1 ", MessageBoxButton.OK);
                else if (!string.IsNullOrEmpty(ex.Message))
                    Funcoes.Mensagem(ex.Message +
                                   " Por gentileza, verificar as informações apresentada.", "Erro PROCESSAMENTO - NFC-e COD:2 ", MessageBoxButton.OK);
                return null;
            }

        }

        public string Cancelar(VendaNFCe venda, Vendanfcea vnfcea, string justificativa)
        {
            try
            {
                #region Cancelar NFe
                vendanfcea = vnfcea;

                string protocolo = vendanfcea.Nprotocolo;

                CarregarConfiguracao();

                ConfiguracaoApp configuracoes = _configuracoes;
                configuracoes.CfgServico.ModeloDocumento = ModeloDocumento.NFCe;

                var servicoNFe = new ServicosNFe(_configuracoes.CfgServico);

                var chave = vendanfcea.Chave.Replace("NFe", "");

                if (vendanfcea.Nprotocolo.Trim() == "")
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
                    vendanfcea.Statusnfe = "CANCELADO";
                    vendanfcea.Nprotocolo = retornoCancelamento.Retorno.retEvento.First().infEvento.nProt;
                    vendanfcea.Update();

                    FuncoesFTP.GuardaXML(retornoCancelamento.ProcEventosNFe.First().ObterXmlString(), _path + @"\NFCe\Cancelada\", vendanfcea.Chave + "-nProt" + vendanfcea.Nprotocolo);
                    string local = _path + @"\NFCe\Cancelada\" + vendanfcea.Chave + "-nProt" + vendanfcea.Nprotocolo + ".xml";

                    FuncoesFTP.SubirArquivo(local, "NFe" + vendanfcea.Chave + "-nProt" + vendanfcea.Nprotocolo + ".xml", "nfce");
                    string localnfe = _path + @"\NFCe\Autorizada\" + vendanfcea.Chave + "-procNFe.xml";
                    string localcancelada = _path + @"\NFCe\Cancelada\" + vendanfcea.Chave + "-nProt" + vendanfcea.Nprotocolo + ".xml";
                    string localsave = _path + @"\NFCe\Cancelada\" + vendanfcea.Chave + "-nProt" + vendanfcea.Nprotocolo + ".pdf";

                    var proc = new nfeProc().CarregarDeArquivoXml(localnfe);
                    procEventoNFe procEvento = FuncoesXml.ArquivoXmlParaClasse<procEventoNFe>(localcancelada);

                    var danfe = new DanfeFrEvento(proc, procEvento,
                        new ConfiguracaoDanfeNfe(_configuracoes.ConfiguracaoDanfeNfe.Logomarca,
                        false,
                        false),
                        "CIAF - SOLUÇÕES EM SOFTWARE");

                    danfe.ExportarPdf(localsave);
                    FuncoesFTP.SubirArquivo(_path + @"\NFCe\Cancelada\" + vendanfcea.Chave + "-nProt" + vendanfcea.Nprotocolo + ".pdf", vendanfcea.Chave + " - nProt" + vendanfcea.Nprotocolo + ".pdf", "nfce");
                    if (_configuracoes.CfgServico.DiretorioSalvarXml.Replace(" ", "") != "" && _configuracoes.CfgServico.DiretorioSalvarXml != null)
                    {
                        try
                        {
                            danfe.ExportarPdf(_configuracoes.CfgServico.DiretorioSalvarXml + @"\NFe" + vendanfcea.Chave + "-nProt" + vendanfcea.Nprotocolo + ".pdf");
                        }
                        catch
                        { }
                    }
                }
                else
                {
                    vendanfcea.Merro = Motivo;
                    vendanfcea.Update();

                }
                return retornoCancelamento.RetornoStr;
                #endregion
            }
            catch (ComunicacaoException ex)
            {
                Funcoes.Mensagem(ex.Message + Environment.NewLine +
                    " Tente novamente, se o erro continuar: " + Environment.NewLine +
                    " Por gentileza, verificar o Disponibilidade da SEFAZ ou entre em contato com suporte.", "RETORNO SEFAZ ", MessageBoxButton.OK);
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

        public string Inutilizar(VendaNFCe vnfce, Vendanfcea vnfcea, string justificativa)
        {
            try
            {
                #region Inutiliza Numeração

                vendanfce = vnfce;
                vendanfcea = vnfcea;

                CarregarConfiguracao();
                ConfiguracaoApp configuracoes = _configuracoes;
                configuracoes.CfgServico.ModeloDocumento = ModeloDocumento.NFCe;

                string ano = DateTime.Now.ToString("yy");

                if (string.IsNullOrEmpty(justificativa)) throw new Exception("A Justificativa deve ser informada!");

                var servicoNFe = new ServicosNFe(_configuracoes.CfgServico);

                RetornoNfeInutilizacao retornoConsulta = servicoNFe.NfeInutilizacao(_configuracoes.Emitente.CNPJ, Convert.ToInt16(ano),
                    _configuracoes.CfgServico.ModeloDocumento, Convert.ToInt16(vendanfce.Serie), Convert.ToInt32(vendanfce.Nrvenda),
                    Convert.ToInt32(vendanfce.Nrvenda), justificativa);

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

                    vendanfcea.Statusnfe = "INUTILIZADO";
                    vendanfcea.Chave = pinutnf.inutNFe.infInut.Id;
                    vendanfcea.Nprotocolo = retornoConsulta.Retorno.infInut.nProt;
                    vendanfcea.Update();

                    FuncoesFTP.GuardaXML(FuncoesXml.ClasseParaXmlString<procInutNFe>(pinutnf), _path + @"\NFCe\Inutilizada\", "" + pinutnf.inutNFe.infInut.Id + "-nProt" + vendanfcea.Nprotocolo);
                    string local = _path + @"\NFCe\Inutilizada\" + pinutnf.inutNFe.infInut.Id + "-nProt" + vendanfcea.Nprotocolo + ".xml";
                    FuncoesFTP.SubirArquivo(local, pinutnf.inutNFe.infInut.Id + "-nProt" + vendanfcea.Nprotocolo + ".xml", "nfce");

                }
                else
                {
                    vendanfcea.Statusnfe = "REJEITADO";
                    vendanfcea.Merro = Motivo;
                    vendanfcea.Update();
                }

                return retornoConsulta.RetornoStr;
                #endregion
            }
            catch (ComunicacaoException ex)
            {
                Funcoes.Mensagem(ex.Message, "Erro", MessageBoxButton.OK);
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

        public void GerarPdf(VendaNFCe vnfce, Vendanfcea vnfcea)
        {
            try
            {
                vendanfce = vnfce;
                vendanfcea = vnfcea;

                CarregarConfiguracao();
                string local = _path + @"\NFCE\Autorizada\" + vendanfcea.Chave.Replace("NFe", "") + "-procNFe.xml";
                string localsave = _path + @"\NFCE\Autorizada\" + vendanfcea.Chave.Replace("NFe", "") + "-procNFe.pdf";

                nfeProc proc = null;
                try
                {
                    proc = new nfeProc().CarregarDeArquivoXml(local);
                }
                catch
                {
                    NFe.Classes.NFe _nfe;
                    _nfe = GetNf(vendanfcea.Nrvenda, ModeloDocumento.NFCe, _configuracoes.CfgServico.VersaoNFeAutorizacao);
                    //_nfe.Assina(); //não precisa validar aqui, pois o lote será validado em ServicosNFe.NFeAutorizacao
                    _nfe.infNFeSupl = new infNFeSupl();
                    _nfe.infNFeSupl.urlChave = _nfe.infNFeSupl.ObterUrlConsulta(_nfe, _configuracoes.ConfiguracaoDanfeNfce.VersaoQrCode);
                    _nfe.infNFeSupl.qrCode = _nfe.infNFeSupl.ObterUrlQrCode(_nfe, _configuracoes.ConfiguracaoDanfeNfce.VersaoQrCode, _configuracoes.ConfiguracaoCsc.CIdToken, _configuracoes.ConfiguracaoCsc.Csc);

                    //_nfe.Valida(); // não precisa validar aqui, pois o lote será validado em ServicosNFe.NFeAutorizacao
                    FuncoesFTP.GuardaXML(_nfe.ObterXmlString(), _path + @"\NFCE\", "VERIFICAR" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe");
                    local = _path + @"\NFCE\VERIFICAR" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml";
                    localsave = _path + @"\NFCE\" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.pdf";
                    proc = new nfeProc() { NFe = new NFe.Classes.NFe().CarregarDeArquivoXml(local), protNFe = new NFe.Classes.Protocolo.protNFe() };

                    if (vendanfcea.Nprotocolo.Replace(" ", "") != "")
                        proc.protNFe.infProt.nProt = vendanfcea.Nprotocolo;

                    vendanfcea.Chave = _nfe.infNFe.Id;
                    vendanfcea.Update();
                }

                var danfe = new DanfeFrNfce(proc: proc,
                    configuracaoDanfeNfce: _configuracoes.ConfiguracaoDanfeNfce,
                    cIdToken: _configuracoes.ConfiguracaoCsc.CIdToken,
                    csc: _configuracoes.ConfiguracaoCsc.Csc,
                    arquivoRelatorio: string.Empty);

                //danfe.ExibirDesign();
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
                        danfe.ExportarPdf(_configuracoes.CfgServico.DiretorioSalvarXml + @"\" + vendanfcea.Chave + "-procNFe.pdf");
                    }
                    catch
                    { }
                }

                if (_configuracoes.ConfiguracaoDanfeNfce.Direto == "SIM")
                    danfe.Imprimir(false, _configuracoes.ConfiguracaoDanfeNfce.Impressora);

            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                    Funcoes.Mensagem(ex.Message + "  Mais informações:" + ex.ToString(), "Erro ao imprimir.", MessageBoxButton.OK);
            }
        }

        public void ImprimirDireto(VendaNFCe vnfce, Vendanfcea vnfcea)
        {
            try
            {
                vendanfce = vnfce;
                vendanfcea = vnfcea;

                CarregarConfiguracao();
                string local = _path + @"\NFCE\Autorizada\" + vendanfcea.Chave.Replace("NFe", "") + "-procNFe.xml";
                string localsave = _path + @"\NFCE\Autorizada\" + vendanfcea.Chave.Replace("NFe", "") + "-procNFe.pdf";

                nfeProc proc = null;
                try
                {
                    proc = new nfeProc().CarregarDeArquivoXml(local);
                }
                catch
                {
                    NFe.Classes.NFe _nfe;
                    _nfe = GetNf(vendanfcea.Nrvenda, ModeloDocumento.NFCe, _configuracoes.CfgServico.VersaoNFeAutorizacao);
                    _nfe.Assina(); //não precisa validar aqui, pois o lote será validado em ServicosNFe.NFeAutorizacao
                    _nfe.infNFeSupl = new infNFeSupl();
                    _nfe.infNFeSupl.urlChave = _nfe.infNFeSupl.ObterUrlConsulta(_nfe, _configuracoes.ConfiguracaoDanfeNfce.VersaoQrCode);
                    _nfe.infNFeSupl.qrCode = _nfe.infNFeSupl.ObterUrlQrCode(_nfe, _configuracoes.ConfiguracaoDanfeNfce.VersaoQrCode, _configuracoes.ConfiguracaoCsc.CIdToken, _configuracoes.ConfiguracaoCsc.Csc);

                    _nfe.Valida(); // não precisa validar aqui, pois o lote será validado em ServicosNFe.NFeAutorizacao
                    FuncoesFTP.GuardaXML(_nfe.ObterXmlString(), _path + @"\NFCE\", "VERIFICAR" + _nfe.infNFe.Id.Replace("NFe", ""));
                    local = _path + @"\NFCE\VERIFICAR" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml";
                    localsave = _path + @"\NFCE\" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.pdf";
                    proc = new nfeProc() { NFe = new NFe.Classes.NFe().CarregarDeArquivoXml(local), protNFe = new NFe.Classes.Protocolo.protNFe() };

                    if (vendanfcea.Nprotocolo.Replace(" ", "") != "")
                        proc.protNFe.infProt.nProt = vendanfcea.Nprotocolo;

                    vendanfcea.Chave = _nfe.infNFe.Id;
                    vendanfcea.Update();
                }

                var danfe = new DanfeFrNfce(proc: proc,
                    configuracaoDanfeNfce: _configuracoes.ConfiguracaoDanfeNfce,
                    cIdToken: _configuracoes.ConfiguracaoCsc.CIdToken,
                    csc: _configuracoes.ConfiguracaoCsc.Csc,
                    arquivoRelatorio: string.Empty);

                //danfe.Visualizar();
                danfe.Imprimir();
                if (_configuracoes.CfgServico.DiretorioSalvarXml.Replace(" ", "") != "" && _configuracoes.CfgServico.DiretorioSalvarXml != null)
                {
                    try
                    {
                        danfe.ExportarPdf(_configuracoes.CfgServico.DiretorioSalvarXml + @"\" + vendanfcea.Chave + "-procNFe.pdf");
                    }
                    catch
                    { }
                }

                if (App.Parametro.Diretonfce == "SIM")
                    danfe.Imprimir(false, App.Parametro.Impressoranfce);

            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                    Funcoes.Mensagem(ex.Message + "  Mais informações:" + ex.ToString(), "Erro ao imprimir.", MessageBoxButton.OK);
            }
        }

        public static void Email(VendaNFCe vnfce, Vendanfcea vnfcea, string copia = "")
        {
            try
            {

                if (App.Parametro.Confemail == "CIAF" || _configuracoes.ConfiguracaoEmail.ServidorSmtp == "smtp.dominio.com")
                {
                    App.Parametro.Confemail = "CIAF";
                    App.Parametro.Email = _configuracoes.ConfiguracaoEmail.Email;

                    MandarEmail.NovoEmailNFCeCiaf(vnfce, vnfcea, copia);
                }
                else
                {
                    CarregarConfiguracao();
                    string arquivoXml = _path + @"\NFCE\Autorizada\" + vnfcea.Chave + "-procNFe.xml";
                    string arquivoPdf = _path + @"\NFCE\Autorizada\" + vnfcea.Chave + "-procNFe.pdf";

                    string mensagem = _configuracoes.ConfiguracaoEmail.Mensagem;
                    mensagem = mensagem.Replace("#NumerodaNota#", vnfcea.Nrvenda.ToString());
                    mensagem = mensagem.Replace("#SeriedaNota#", vnfce.Serie.ToString());
                    mensagem = mensagem.Replace("#ChavedaNota#", vnfcea.Chave.ToString());
                    mensagem = mensagem.Replace("#DatadaNota#", vnfce.DhEmi.ToString("dd/MM/yyyy"));
                    mensagem = mensagem.Replace("#RazaoSocialEmitente#", App.Parametro.Razaosocial);
                    mensagem = mensagem.Replace("#CNPJEmitente#", App.Parametro.Cnpj);
                    mensagem = mensagem.Replace("#RazaoSocialDestinatario#", vnfce.Nomecliente);
                    mensagem = mensagem.Replace("#CNPJDestinatario#", vnfce.Cnpj_cpf);

                    try
                    {
                        nfeProc nfe = new nfeProc().CarregarDeArquivoXml(_path + @"\NFCE\Autorizada\" + vnfcea.Chave.Replace("NFe", "") + "-procNFe.xml");
                        mensagem = mensagem.Replace("#Link#", nfe.NFe.infNFeSupl.qrCode);
                    }
                    catch (ArgumentException ex)
                    {
                        Funcoes.Crashe(ex, "ERRO - Code:Email QRCODE");
                    }

                    _configuracoes.ConfiguracaoEmail.Mensagem = mensagem;

                    _configuracoes.ConfiguracaoEmail.Assunto = _configuracoes.ConfiguracaoEmail.Assunto.Replace("#NumerodaNota#", vnfcea.Nrvenda.ToString());


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
                    emailBuilder.ErroAoEnviarEmail += erro => Funcoes.Mensagem(erro.Message, "Erro - Code:Email0", MessageBoxButton.OK, MessageBoxImage.Error);



                    emailBuilder.Enviar();
                }
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
                Funcoes.Mensagem(ex.Message, "Erro - Code:Email2", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ERRO - Code:Email3");
            }
        }
        private static void EventoDepoisDeEnviarEmail(object sender, EventArgs e)
        {
            Funcoes.Mensagem("E-mail encaminhado com sucesso.", "Evento", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void Imprimir(VendaNFCe vnfce, Vendanfcea vnfcea)
        {
            try
            {
                vendanfce = vnfce;
                vendanfcea = vnfcea;

                CarregarConfiguracao();
                string local = _path + @"\NFCE\Autorizada\" + vnfcea.Chave + "-procNFe.xml";
                string localsave = _path + @"\NFCE\Autorizada\" + vnfcea.Chave + "-procNFe.pdf";

                nfeProc proc = null;
                try
                {
                    proc = new nfeProc().CarregarDeArquivoXml(local);
                }
                catch
                {
                    NFe.Classes.NFe _nfe;
                    _nfe = GetNf(vnfcea.Nrvenda, ModeloDocumento.NFCe, _configuracoes.CfgServico.VersaoNFeAutorizacao);
                    _nfe.Assina(); //não precisa validar aqui, pois o lote será validado em ServicosNFe.NFeAutorizacao
                    _nfe.infNFeSupl = new infNFeSupl();
                    _nfe.infNFeSupl.urlChave = _nfe.infNFeSupl.ObterUrlConsulta(_nfe, _configuracoes.ConfiguracaoDanfeNfce.VersaoQrCode);
                    _nfe.infNFeSupl.qrCode = _nfe.infNFeSupl.ObterUrlQrCode(_nfe, _configuracoes.ConfiguracaoDanfeNfce.VersaoQrCode, _configuracoes.ConfiguracaoCsc.CIdToken, _configuracoes.ConfiguracaoCsc.Csc);

                    _nfe.Valida(); // não precisa validar aqui, pois o lote será validado em ServicosNFe.NFeAutorizacao
                    FuncoesFTP.GuardaXML(_nfe.ObterXmlString(), _path + @"\NFCE\", "VERIFICAR" + _nfe.infNFe.Id.Replace("NFe", ""));
                    local = _path + @"\NFCE\VERIFICAR" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.xml";
                    localsave = _path + @"\NFCE\" + _nfe.infNFe.Id.Replace("NFe", "") + "-procNFe.pdf";
                    proc = new nfeProc() { NFe = new NFe.Classes.NFe().CarregarDeArquivoXml(local), protNFe = new NFe.Classes.Protocolo.protNFe() };

                    if (vnfcea.Nprotocolo.Replace(" ", "") != "")
                        proc.protNFe.infProt.nProt = vnfcea.Nprotocolo;

                    vnfcea.Chave = _nfe.infNFe.Id;
                    vnfcea.Update();
                }

                var danfe = new DanfeFrNfce(proc: proc,
                    configuracaoDanfeNfce: _configuracoes.ConfiguracaoDanfeNfce,
                    cIdToken: _configuracoes.ConfiguracaoCsc.CIdToken,
                    csc: _configuracoes.ConfiguracaoCsc.Csc,
                    arquivoRelatorio: string.Empty);

                //danfe.Visualizar();
                danfe.Imprimir();

                //if (App.Parametro.Diretonfce == "SIM")
                //    danfe.Imprimir(false, App.Parametro.Impressoranfce);

            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                    Funcoes.Mensagem(ex.Message + "  Mais informações:" + ex.ToString(), "Erro ao imprimir.", MessageBoxButton.OK);
            }
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
                    transp = GetTransporte(),
                };

                if (vendanfce.Cnpj_cpf.Replace(" ", "") != "")
                    infNFe.dest = GetDestinatario(numero, versao, modelo);

                if ((vendanfce.Obs.Length > 1))
                {
                    infNFe.infAdic = new infAdic();
                    if (vendanfce.Obs.Length > 1) infNFe.infAdic.infCpl = vendanfce.Obs.Trim();
                    //if (vendanfce.Infadfisco.Length > 1) infNFe.infAdic.infAdFisco = vendanfce.Infadfisco.Trim();
                }

                VendaNFCeI ivenda = new VendaNFCeI();
                List<VendaNFCeI> lista = ivenda.GetTodas(vendanfce.Nrvenda);
                foreach (VendaNFCeI i in lista)
                {
                    infNFe.det.Add(GetDetalhe(i, infNFe.emit.CRT, modelo));
                }

                infNFe.total = GetTotal(infNFe.det);

                infNFe.pag = GetPagamento(numero, infNFe.total.ICMSTot, versao);

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
                    modFrete = (ModalidadeFrete)vendanfce.ModFrete
                };


                if (!string.IsNullOrWhiteSpace(vendanfce.Nome_transp))
                {
                    t.transporta = new transporta();
                    if (!string.IsNullOrWhiteSpace(vendanfce.Nome_transp))
                        t.transporta.xNome = vendanfce.Nome_transp;

                    if (!string.IsNullOrWhiteSpace(vendanfce.Ender_transp))
                        t.transporta.xEnder = vendanfce.Ender_transp;

                    if (!string.IsNullOrWhiteSpace(vendanfce.Mun_transp))
                        t.transporta.xMun = vendanfce.Mun_transp;

                    if (!string.IsNullOrWhiteSpace(vendanfce.Uf_transp))
                        t.transporta.UF = vendanfce.Uf_transp;

                    if (!string.IsNullOrWhiteSpace(vendanfce.Ie_transp))
                        t.transporta.IE = vendanfce.Ie_transp;

                    if (vendanfce.Cnpjcpf_transp.Length >= 14)
                        t.transporta.CNPJ = Funcoes.Deixarnumero(vendanfce.Cnpjcpf_transp.Replace(".", "").Replace(@"/", "").Replace("-", "").Trim());
                    else
                        t.transporta.CPF = Funcoes.Deixarnumero(vendanfce.Cnpjcpf_transp.Replace(".", "").Replace(@"/", "").Replace("-", "").Trim());
                }
                if (!string.IsNullOrWhiteSpace(vendanfce.Placa_transp))
                {
                    t.veicTransp = new veicTransp()
                    {
                        placa = vendanfce.Placa_transp,
                        RNTC = vendanfce.Rntc_transp,
                        UF = vendanfce.Ufveiculo_transp,
                    };
                }

                if (vendanfce.QVol_transp > 0 || vendanfce.Pesol_transp > 0 || vendanfce.Pesob_transp > 0)
                {
                    t.vol = new List<vol>();
                    var volu = new vol();

                    if (vendanfce.QVol_transp > 0)
                        volu.qVol = vendanfce.QVol_transp;

                    if (vendanfce.Nvol_transp.Replace(" ", "") != "")
                        volu.nVol = vendanfce.Nvol_transp;

                    if (vendanfce.Esp_transp.Replace(" ", "") != "")
                        volu.esp = vendanfce.Esp_transp;

                    if (vendanfce.Marca_transp.Replace(" ", "") != "")
                        volu.marca = vendanfce.Marca_transp;

                    if (vendanfce.Pesol_transp > 0)
                        volu.pesoL = vendanfce.Pesol_transp;

                    if (vendanfce.Pesob_transp > 0)
                        volu.pesoB = vendanfce.Pesob_transp;

                    t.vol.Add(volu);
                }

                return t;
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ERRO AO GERAR O XML | CODIGO: GetTransporte ");
                return null;
            }
        }

        protected virtual ide GetIdentificacao(int numero, ModeloDocumento modelo, VersaoServico versao)
        {
            try
            {
                if (Convert.ToInt32(vendanfce.Finalidade) > 3) vendanfce.Tpnf = 0;
                else vendanfce.Tpnf = 1;

                vendanfce.Serie = App.Parametro.Serienfce;
                _configuracoes.CfgServico.tpEmis = (TipoEmissao)App.Parametro.NfcetpEmis;


                var ide = new ide
                {
                    cUF = _configuracoes.EnderecoEmitente.UF,
                    natOp = "VENDA DE MERCADORIA ADQUIRIDA OU RECEBIDA DE TERCEIROS",
                    mod = modelo,
                    serie = vendanfce.Serie,
                    nNF = numero,
                    tpNF = (TipoNFe)vendanfce.Tpnf, //verificar se e entrada ou saida
                    idDest = DestinoOperacao.doInterna,
                    cMunFG = _configuracoes.EnderecoEmitente.cMun,
                    //tpEmis = _configuracoes.CfgServico.tpEmis,
                    tpEmis = (TipoEmissao)_configuracoes.CfgServico.tpEmis,
                    tpImp = (TipoImpressao)Convert.ToInt32(vendanfce.Danfe), //TIPO 
                    cNF = "03054280",  //"1234",
                    tpAmb = _configuracoes.CfgServico.tpAmb,

                    finNFe = (FinalidadeNFe)Convert.ToInt32(vendanfce.Finalidade),

                    indFinal = (ConsumidorFinal)vendanfce.Indfinal,
                    indPres = (PresencaComprador)vendanfce.Indpres,

                    verProc = "CIAF " + Assembly.GetExecutingAssembly().GetName().Version.ToString(),

                    dhEmi = DateTime.Now

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
                    ide.dhCont = DateTime.Now;
                    if (string.IsNullOrEmpty(vendanfce.Justifica))
                    {
                        var justificativa = Funcoes.InpuBox(Application.Current.Windows.OfType<TELAPROCESSAMENTONFCE>().First(), "Contingência NFe", "Justificativa");
                        if (string.IsNullOrEmpty(justificativa)) throw new Exception("A justificativa deve ser informada!");
                        ide.xJust = justificativa;
                        vendanfce.Justifica = justificativa;
                        //vendanfce.Update();
                    }
                    else
                    {
                        ide.xJust = vendanfce.Justifica;
                    }

                }

                if (ide.finNFe == FinalidadeNFe.fnComplementar || ide.finNFe == FinalidadeNFe.fnDevolucao || ide.finNFe == FinalidadeNFe.fnAjuste)
                {
                    List<NFref> lista = new List<NFref>();
                    if (vendanfce.Chavedev != "")
                    {

                        NFref nfref = new NFref();
                        nfref.refNFe = vendanfce.Chavedev;
                        lista.Add(nfref);


                    }
                    ide.NFref = lista;
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

                if (_configuracoes.Emitente.CNPJ.Length >= 14)
                    emit.CNPJ = _configuracoes.Emitente.CNPJ;
                else
                    emit.CPF = _configuracoes.Emitente.CPF;


                //var enderEmit = new enderEmit()
                //{
                //    xLgr = App.Parametro.Endereco,
                //    nro = App.Parametro.Complemento,
                //    xCpl = App.Parametro.Comple2,
                //    xBairro = App.Parametro.Bairro,
                //    cMun = Convert.ToInt32(App.Parametro.Codcidade),
                //    xMun = App.Parametro.Cidade,
                //    UF = Funcoes.EstadoString(App.Parametro.Uf),
                //    CEP = App.Parametro.Cep.Replace(".", "").Replace("-", ""),
                //    fone = Convert.ToInt64(App.Parametro.Fone_res.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")),
                //};

                //enderEmit.cPais = 1058;
                //enderEmit.xPais = "BRASIL";

                //emit.enderEmit = enderEmit;

                emit.enderEmit = _configuracoes.EnderecoEmitente;
                return emit;
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ERRO AO GERAR O XML | CODIGO: GetEmitente ");
                return null;
            }

        }


        protected virtual dest GetDestinatario(int numero, VersaoServico versao, ModeloDocumento modelo)
        {
            try
            {
                vendanfce.Cnpj_cpf = vendanfce.Cnpj_cpf.Replace(" ", "");
                var dest = new dest(versao);

                if (vendanfce.Cnpj_cpf.Length >= 14)
                {
                    Clientes cliente = new Clientes(vendanfce.Id_cliente);
                    dest.xNome = cliente.Razao;

                    dest.CNPJ = vendanfce.Cnpj_cpf.Replace(" ", "");
                    if (vendanfce.Email.Replace(" ", "") != "")
                        dest.email = vendanfce.Email.Replace(" ", "");

                    if (cliente.Rg != null)
                    {
                        if (cliente.Rg.Replace(".", "").Replace(" ", "") != "")
                        {
                            dest.IE = vendanfce.Ie_rg.Replace(".", "").Replace(".", "").Replace("-", "");
                        }
                    }

                }
                else if (vendanfce.Cnpj_cpf.Replace(" ", "") != "")
                {
                    dest.xNome = vendanfce.Nomecliente;
                    dest.CPF = vendanfce.Cnpj_cpf.Replace(" ", "");
                    if (vendanfce.Email.Replace(" ", "") != "")
                        dest.email = vendanfce.Email.Replace(" ", "");
                }
                else
                {
                    dest.xNome = vendanfce.Nomecliente;
                    dest.CPF = vendanfce.Cnpj_cpf.Replace(" ", "");
                    if (vendanfce.Email.Replace(" ", "") != "")
                        dest.email = vendanfce.Email.Replace(" ", "");
                }

                if (vendanfce.Id_cliente.Replace(" ", "") != "0")
                {
                    dest.enderDest = new enderDest
                    {
                        xLgr = vendanfce.Endereco,
                        nro = vendanfce.Numero,
                        xCpl = vendanfce.Complemento,
                        xBairro = vendanfce.Bairro,
                        cMun = vendanfce.Cmun,
                        xMun = vendanfce.Cidade,
                        UF = vendanfce.Uf,
                        CEP = vendanfce.Cep.Replace(".", "").Replace("-", ""),
                        cPais = vendanfce.Cpais,
                        xPais = vendanfce.Pais,
                    };
                }

                if (vendanfce.Fone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != "")
                    if (vendanfce.Fone != "0" && vendanfce.Fone != null && vendanfce.Fone != "")
                    {
                        dest.enderDest.fone = Convert.ToInt64(vendanfce.Fone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", ""));
                    }


                if (vendanfce.Contrib == 1)
                {
                    dest.indIEDest = indIEDest.ContribuinteICMS;
                    dest.IE = vendanfce.Ie_rg.Replace(".", "").Replace("-", "");
                }
                else if (vendanfce.Contrib == 2)
                {
                    dest.indIEDest = indIEDest.Isento;
                }
                else if (vendanfce.Contrib == 9)
                {
                    //dest.IE = "ISENTO";
                    dest.indIEDest = indIEDest.NaoContribuinte;
                }


                //if (_configuracoes.CfgServico. == "HOMOLOGAÇÃO")
                //{
                //    dest.xNome = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";
                //}

                dest.indIEDest = indIEDest.NaoContribuinte;
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

        protected virtual entrega GetEntrega()
        {

            entrega entregar = new entrega();
            try
            {
                if (vendanfce.Cnpj_cpf_entrega.Length >= 14)
                    entregar.CNPJ = vendanfce.Cnpj_cpf_entrega;
                else entregar.CPF = vendanfce.Cnpj_cpf_entrega;

                entregar.xNome = vendanfce.Xnome_entrega;
                entregar.CEP = long.Parse(vendanfce.Cep_entrega);
                entregar.xLgr = vendanfce.Xlgr_entrega;
                entregar.nro = vendanfce.Nro_entrega;

                if (vendanfce.Xcpl_entrega.Replace(" ", "") != "")
                    entregar.xCpl = vendanfce.Xcpl_entrega;
                entregar.xBairro = vendanfce.Xbairro_entrega;
                entregar.xMun = vendanfce.Xmun_entrega;
                entregar.cMun = vendanfce.Cmun;

                entregar.UF = vendanfce.Uf_entrega;
                entregar.cPais = vendanfce.Cpais_entrega;
                entregar.xPais = vendanfce.Xpais_entrega;

                if (vendanfce.Fone_entrega != "")
                    entregar.fone = vendanfce.Fone_entrega;

                if (vendanfce.Email_entrega != "")
                    entregar.email = vendanfce.Email_entrega;

                if (vendanfce.Ie_entrega != "" && vendanfce.Ie_entrega != "0")
                    entregar.IE = vendanfce.Ie_entrega;

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
                if (vendanfce.Cnpj_cpf_retirada.Length >= 14)
                    retiradar.CNPJ = vendanfce.Cnpj_cpf_retirada;
                else retiradar.CPF = vendanfce.Cnpj_cpf_retirada;

                retiradar.xNome = vendanfce.Xnome_retirada;
                retiradar.CEP = long.Parse(vendanfce.Cep_retirada);
                retiradar.xLgr = vendanfce.Xlgr_retirada;
                retiradar.nro = vendanfce.Nro_retirada;


                if (vendanfce.Xcpl_retirada.Replace(" ", "") != "")
                    retiradar.xCpl = vendanfce.Xcpl_retirada;

                retiradar.xBairro = vendanfce.Xbairro_retirada;
                retiradar.xMun = vendanfce.Xmun_retirada;
                retiradar.cMun = vendanfce.Cmun;

                retiradar.UF = vendanfce.Uf_retirada;
                retiradar.cPais = vendanfce.Cpais_retirada;
                retiradar.xPais = vendanfce.Xpais_retirada;

                if (vendanfce.Fone_retirada != "")
                    retiradar.fone = vendanfce.Fone_retirada;

                if (vendanfce.Email_retirada != "")
                    retiradar.email = vendanfce.Email_retirada;

                if (vendanfce.Ie_retirada != "")
                    retiradar.IE = vendanfce.Ie_retirada;

                return retiradar;

            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                    Funcoes.Mensagem(ex.Message, "Erro", MessageBoxButton.OK);
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
        protected virtual det GetDetalhe(VendaNFCeI ivenda, CRT crt, ModeloDocumento modelo)
        {

            try
            {
                var det = new det
                {
                    nItem = ivenda.Nritem,
                    prod = new prod
                    {
                        cProd = ivenda.Codigob,
                        //cEAN = ivenda.Codigob,
                        xProd = ivenda.Descricao,
                        NCM = ivenda.Ncm,
                        CFOP = Convert.ToInt32(ivenda.Cfop),
                        uCom = ivenda.Unidade, //"UN",
                        qCom = ivenda.Quantidade,
                        vUnCom = ivenda.Valor,
                        vProd = ivenda.Valor * ivenda.Quantidade,
                        vDesc = ivenda.Desconto,
                        //  cEANTrib = ivenda.Codigob,//"7770000000012",
                        uTrib = ivenda.Utrib, //"UNID",
                        qTrib = ivenda.Qtrib, //1,
                        vUnTrib = ivenda.Vuntrib,
                        indTot = IndicadorTotal.ValorDoItemCompoeTotalNF,
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
                        vTotTrib = ivenda.Vtottrib,
                    }
                };


                //Parametros  de Olho no Imposto
                if (App.Parametro.Calculatrib == "SIM")
                {
                    DeOlhoNoImposto olhoimposto = new DeOlhoNoImposto();

                    olhoimposto = olhoimposto.Requisicao(ivenda.Ncm, ivenda.Vl_total.ToString(), App.Parametro.Uf);

                    ivenda.Vtottrib = olhoimposto.GetValorAproxTributos();
                    ivenda.UpdateVmimposto(ivenda.Vtottrib.ToString());

                    det.imposto = new imposto
                    {
                        vTotTrib = ivenda.Vtottrib,
                    };
                }


                if (ivenda.Voutro > 0)
                {
                    det.prod.vOutro = ivenda.Voutro;
                }

                //CBenef
                if (ivenda.CBenef != null)
                    if (ivenda.CBenef.Replace(" ", "") != "") { det.prod.cBenef = ivenda.CBenef; }

                // PISGeral
                var pisgeral = new PISGeral()
                {
                    CST = (CSTPIS)Convert.ToInt32(ivenda.Cstpis),
                    pPIS = ivenda.Pisaliq,
                    vBC = ivenda.Pisbc,
                    vPIS = ivenda.Pisval
                };
                det.imposto.PIS = new PIS
                {
                    TipoPIS = pisgeral.ObterPISBasico(),
                };

                // COFINSGeral
                var cofinsGeral = new COFINSGeral()
                {
                    CST = (CSTCOFINS)Convert.ToUInt32(ivenda.Cstcofins),
                    pCOFINS = ivenda.Cofaliq,
                    vBC = ivenda.Cofbc,
                    vCOFINS = ivenda.Vcofins,
                };
                det.imposto.COFINS = new COFINS
                {
                    TipoCOFINS = cofinsGeral.ObterCOFINSBasico(),
                };

                ///ICMS
                det.imposto.ICMS = new ICMS
                {
                    TipoICMS = ObterIcmsBasic(ivenda, crtcliente),
                };

                if (ivenda.Codigoanp != "" && ivenda.Codigoanp != "0")
                {
                    comb comb = new comb()
                    {
                        cProdANP = ivenda.Codigoanp,
                        descANP = ivenda.Descanp,
                        UFCons = ivenda.Ufcombus,
                    };

                    if (ivenda.Codif != "0" && ivenda.Codif != "") comb.CODIF = ivenda.Codif;
                    if (ivenda.Perglp > 0) comb.pGLP = ivenda.Perglp;
                    if (ivenda.Pergnat > 0) comb.pGNn = ivenda.Pergnat;
                    if (ivenda.Pergnat_i > 0) comb.pGNi = ivenda.Pergnat_i;
                    if (ivenda.Vlpartida > 0) comb.vPart = ivenda.Vlpartida;
                    if (ivenda.Qtfattempa > 0) comb.qTemp = ivenda.Qtfattempa;


                    det.prod.ProdutoEspecifico = new List<ProdutoEspecifico>();
                    det.prod.ProdutoEspecifico.Add(comb);

                    det.imposto.ICMS = new ICMS
                    {
                        TipoICMS = ObterIcmsBasic(ivenda, crtcliente),
                    };

                    //det.imposto.ICMS = new ICMS
                    //{

                    //    TipoICMS = new ICMSST
                    //    {
                    //        orig = (OrigemMercadoria)ivenda.Origem,
                    //        CST = csticmse,
                    //        vBCSTRet = ivenda.VBCSTRet,
                    //        vICMSSTRet = ivenda.VICMSSTRet,
                    //        vBCSTDest = ivenda.Vbcstdest,
                    //        vICMSSTDest = ivenda.Vicmsstdest,
                    //    }
                    //};

                }


                if (_configuracoes.CfgServico.tpAmb.ToString() == "Homologacao" && ivenda.Nritem == 1)
                {
                    det.prod.xProd = "NOTA FISCAL EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";
                }

                if (ivenda.Nrpedido != null)
                    if (ivenda.Nrpedido.Replace(" ", "") != "")
                    {
                        det.prod.xPed = ivenda.Nrpedido;
                        det.prod.nItemPed = Convert.ToInt32(ivenda.Nritemped.Replace(" ", ""));
                    }

                Produtos produtos = new Produtos(ivenda.Codigob);
                //EAN VALIDO
                if (!string.IsNullOrWhiteSpace(produtos.Cest))
                {
                    if (ivenda.Cst == "10" ||
                        ivenda.Cst == "30" ||
                        ivenda.Cst == "60" ||
                        ivenda.Cst == "70" ||
                        ivenda.Cst == "90" ||
                        ivenda.Cst == "201" ||
                        ivenda.Cst == "202" ||
                        ivenda.Cst == "203" ||
                        ivenda.Cst == "500" ||
                        ivenda.Cst == "900")
                    {
                        ivenda.Cest = produtos.Cest;
                        det.prod.CEST = produtos.Cest;
                    }

                }


                if (produtos.Eanval == "SIM")
                {
                    det.prod.cEAN = ivenda.Codigob;
                    det.prod.cEANTrib = ivenda.Codigob;
                }
                else
                {
                    det.prod.cEAN = "SEM GTIN";
                    det.prod.cEANTrib = "SEM GTIN";
                }

                if (ivenda.Frete > 0)
                {
                    det.prod.vFrete = ivenda.Frete;
                }

                if (ivenda.Cest != "0" && ivenda.Cest != null && ivenda.Cest.Replace(" ", "") != "")
                {
                    det.prod.CEST = ivenda.Cest;
                }

                if (ivenda.Picmsufdest > 0)
                {
                    det.imposto.ICMSUFDest = new ICMSUFDest()
                    {
                        pFCPUFDest = ivenda.Pfcpufdest,
                        pICMSInter = ivenda.Picmsinter,
                        pICMSInterPart = ivenda.Picmsinterpa,
                        pICMSUFDest = ivenda.Picmsufdest,
                        vBCUFDest = ivenda.Vicmsufdest,
                        vFCPUFDest = ivenda.Vfcpufdest,
                        vICMSUFDest = ivenda.Vicmsufdest,
                        vICMSUFRemet = ivenda.Vicmsufremet,
                    };
                }

                if (modelo == ModeloDocumento.NFe) //NFCe não aceita grupo "IPI"
                    if (Convert.ToInt32(ivenda.Vipi) > 0)
                    {
                        det.imposto.IPI = new IPI()
                        {
                            cEnq = Convert.ToInt32(ivenda.Cenqipi), //999
                            TipoIPI = new IPITrib()
                            {
                                CST = (CSTIPI)Convert.ToInt32(ivenda.Cstipi),
                                pIPI = ivenda.Aipi,
                                vBC = ivenda.Vl_total,
                                vIPI = ivenda.Vipi, // 0.05m
                            }
                        };
                    }
                if (ivenda.Vipidevol > 0)
                {
                    det.impostoDevol = new impostoDevol()
                    {
                        IPI = new IPIDevolvido()
                        {
                            vIPIDevol = ivenda.Vipidevol
                        },
                        pDevol = ivenda.Aipi
                    };
                }
                
                det.imposto.IS = new IS()
                {
                    CSTIS = CSTIS.Is000,
                    cClassTribIS = "000000",
                    vBCIS = ivenda.VBcIs,
                    pIS = ivenda.PIs,
                    pISEspec = 2,
                    uTrib = ivenda.Utrib,
                    qTrib = ivenda.Qtrib,
                    vIS = (ivenda.VBcIs * ivenda.PIs) / 100
                };
                
                if (det.imposto == null)
                    det.imposto = new imposto();
                
                var cstIbs = CST.Cst000;
                switch ((ivenda.CstIbscbs ?? string.Empty).Trim())
                {
                    case "000":
                        cstIbs = CST.Cst000;
                        break;
                    case "200":
                        cstIbs = CST.Cst200;
                        break;
                    case "220":
                        cstIbs = CST.Cst220;
                        break;
                    case "221":
                        cstIbs = CST.Cst221;
                        break;
                    case "400":
                        cstIbs = CST.Cst400;
                        break;
                    case "410":
                        cstIbs = CST.Cst410;
                        break;
                    case "510":
                        cstIbs = CST.Cst510;
                        break;
                    case "550":
                        cstIbs = CST.Cst550;
                        break;
                    case "620":
                        cstIbs = CST.Cst620;
                        break;
                    case "800":
                        cstIbs = CST.Cst800;
                        break;
                    case "810":
                        cstIbs = CST.Cst810;
                        break;
                    case "820":
                        cstIbs = CST.Cst820;
                        break;
                    default:
                        // Mantém padrão Cst000 se código não mapeado
                        cstIbs = CST.Cst000;
                        break;
                }

                det.imposto.IBSCBS = BuildIbscbs(ivenda, cstIbs);
                //if (ivenda.Infadprod.Replace(" ", "") != "")
                //{
                //    det.infAdProd = ivenda.Infadprod;
                //}

                return det;
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ERRO - CODE: GETDETALHE  NO ITEM:" + ivenda.Nritem);
                return null;
            }

        }
        private IBSCBS BuildIbscbs(VendaNFCeI ivenda, CST cstIbs)
        {
            try
            {
                var cstCode = (ivenda.CstIbscbs ?? string.Empty).Trim();
                // Allowed CSTs to generate IBSCBS
                var allowed = new HashSet<string> { "000", "200", "220", "222", "510", "515", "550", "620", "810", "811" };
                if (!allowed.Contains(cstCode))
                {
                    return null;
                }

                bool includeDif = cstCode == "510" || cstCode == "515";
                bool includeRed = cstCode == "200" || cstCode == "515";
                bool includeMono = cstCode == "620";
                bool includeTrib = cstCode != "000";
                var ibs = new IBSCBS
                {
                    CST = cstIbs,
                    cClassTrib = ivenda.CclassTribIbscbs ?? "000000",
                    gIBSCBS = new gIBSCBS
                    {
                        vBC = ivenda.VBcIbscbs,
                        gIBSUF = new gIBSUF
                        {
                            pIBSUF = ivenda.PIbsUf,
                            gDif = includeDif ? new gDif { pDif = ivenda.PDifUfIbs, vDif = ivenda.VDifUfIbs } : null,
                            gDevTrib = new gDevTrib { vDevTrib = ivenda.VDevTribUfIbs },
                            gRed = includeRed ? new gRed { pRedAliq = ivenda.PRedAliqUfIbs, pAliqEfet = ivenda.PRedAliqEfetUfIbs } : null,
                            vIBSUF = ivenda.VIbsUf
                        },
                        gIBSMun = new gIBSMun
                        {
                            pIBSMun = ivenda.PIbsMun,
                            gDif = includeDif ? new gDif { pDif = ivenda.PDifMun, vDif = ivenda.VDifMun } : null,
                            gDevTrib = new gDevTrib { vDevTrib = ivenda.VDevTribMun },
                            gRed = includeRed ? new gRed { pRedAliq = ivenda.PRedAliqMun, pAliqEfet = ivenda.PRedAliqEfetMun } : null,
                            vIBSMun = ivenda.VIbsMun
                        },
                        
                       vIBS = ivenda.VIbsUf + ivenda.VIbsMun,
                        gCBS = new gCBS
                        {
                            pCBS = ivenda.PCbs,
                            gDif = includeDif ? new gDif { pDif = ivenda.PDifUfCbs, vDif = ivenda.VDifCbs } : null,
                            gDevTrib = new gDevTrib { vDevTrib = ivenda.VDevTribCbs },
                            gRed = includeRed ? new gRed { pRedAliq = ivenda.PRedAliqCbs, pAliqEfet = ivenda.VRedAliqCbs } : null,
                            vCBS = ivenda.VCbs
                        },
                        gTribRegular = includeTrib ? new gTribRegular
                        {
                            CSTReg = CST.Cst000,
                            cClassTribReg = "000000",
                            pAliqEfetRegIBSUF = ivenda.PAliqEfetRegIbsUf,
                            vTribRegIBSUF = ivenda.VTribRegIbsUf,
                            pAliqEfetRegIBSMun = ivenda.PAliqEfetRegIbsMun,
                            vTribRegIBSMun = ivenda.VTribRegIbsMun,
                            pAliqEfetRegCBS = ivenda.PAliqEfetRegCbs,
                            vTribRegCBS = ivenda.VTribRegCbs
                        } : null
                    },
                    gIBSCBSMono = includeMono ? new gIBSCBSMono { } : null
                };

                return ibs;
            }
            catch
            {
                return null;
            }
        }
        protected virtual ICMSBasico ObterIcmsBasic(VendaNFCeI ivenda, CRT crt)
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
                    UFST = vendanfce.Uf,
                    pST = ivenda.PST,
                    // motDesICMS = MotivoDesoneracaoIcms.MdiOutros,

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
                if (ivenda.Strdbcst > 0) icmsGeral.pRedBCST = ivenda.Strdbcst;
                if (ivenda.Strdbcst > 0) icmsGeral.pRedBCST = ivenda.Strdbcst;


                if (ivenda.PCredSn > 0) icmsGeral.pCredSN = ivenda.PCredSn;
                if (ivenda.VCredIcmssn > 0) icmsGeral.vCredICMSSN = ivenda.VCredIcmssn;


                //private decimal? _pRedBCEfet;
                //private decimal? _vBCEfet;
                //private decimal? _pICMSEfet;
                //private decimal? _vICMSEfet;

                var retorno = icmsGeral.ObterICMSBasico(crt);
                return retorno;
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "ERRO - CODE: ObterICMSBasico");
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
                vNF = vendanfce.Vnf,
                vDesc = produtos.Sum(p => p.prod.vDesc ?? 0),
                vTotTrib = produtos.Sum(p => p.imposto.vTotTrib ?? 0),
                vICMS = produtos.Sum(p => p.imposto.ICMS.TipoICMS.GetIcmsValue()),
                vBC = produtos.Sum(p => p.imposto.ICMS.TipoICMS.GetIcmsBcValue()),

            };

            icmsTot.vOutro = vendanfce.Voutro;
            icmsTot.vCOFINS = vendanfce.Vconfins;
            icmsTot.vSeg = vendanfce.Vseg;
            icmsTot.vFrete = vendanfce.Vfrete;

            icmsTot.vIPIDevol = vendanfce.Vipidevol;
            icmsTot.vIPI = vendanfce.Vipi;
            icmsTot.vII = vendanfce.Vii;

            icmsTot.vST = vendanfce.Vst;

            //icmsTot.vICMS = vendanfce.Vicms;
            //icmsTot.vBC = vendanfce.Vbc;
            icmsTot.vBCST = vendanfce.Vbcst;


            icmsTot.vICMSDeson = vendanfce.Vicmsdeson;
            icmsTot.vFCPUFDest = vendanfce.Vfcpufdest;
            icmsTot.vICMSUFDest = vendanfce.Vicmsufdest;
            icmsTot.vICMSUFRemet = vendanfce.Vicmsufremet;

            icmsTot.vFCP = vendanfce.Vfcp;
            icmsTot.vFCPST = vendanfce.Vfcpst;
            icmsTot.vFCPSTRet = vendanfce.Vfcpstret;

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

            var t = new total
            {
                ICMSTot = icmsTot

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
            string auten = "";
            var listapagamento = new List<pag>();
            try
            {
                var valorPagto = Valor.Arredondar(icmsTot.vProd / 2, 2);
                VendaNFCePG vendanfcepg = new VendaNFCePG();
                List<VendaNFCePG> lista = vendanfcepg.GetItensdePG(vendanfce.Nrvenda);
                pag pagamento = new pag
                {
                    detPag = new List<detPag>()
                };
                pagamento.vTroco = vendanfce.Vtroco;

                if (lista.Count > 0)
                    foreach (VendaNFCePG i in lista)
                    {
                        if ((FormaPagamento)i.Formapg_id == FormaPagamento.fpDinheiro)
                        {

                            detPag detPag = new detPag()
                            {
                                tPag = FormaPagamento.fpDinheiro,
                                vPag = i.Total_forma,
                            };

                            if (vendanfce.Indpres != 5)
                            {
                                detPag.indPag = IndicadorPagamentoDetalhePagamento.ipDetPgVista;
                            }

                            pagamento.detPag.Add(detPag);

                        }
                        else
                         if ((FormaPagamento)i.Formapg_id == FormaPagamento.fpCartaoCredito || (FormaPagamento)i.Formapg_id == FormaPagamento.fpCartaoDebito)
                        {
                            card tcard = new card();

                            Convenio conve = new Convenio(i.Conveniopg_id);


                            tcard.tpIntegra = TipoIntegracaoPagamento.TipNaoIntegrado;

                            tcard.tBand = (BandeiraCartao)Convert.ToInt32(i.Conveniopg_id);
                            tcard.CNPJ = Funcoes.Deixarnumero(conve.Cnpj);

                            auten = "";
                            if (_configuracoes.ConfiguracaoDanfeNfe.CodAut)
                                auten = Funcoes.InpuBox(null, "NFC-e - Número cAtu do seu POS", "Número de autorização da operação cartão de crédito e/ou débito", auten).Trim();

                            if (auten.Trim() == "") tcard.cAut = "0";
                            else tcard.cAut = auten.Trim();

                            detPag detPag = new detPag()
                            {
                                tPag = (FormaPagamento)i.Formapg_id,

                                vPag = i.Total_forma
                            };

                            detPag.card = tcard;



                            if (vendanfce.Indpres != 5)
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
                                xPag = i.Descricaodomeiodepagamento,
                                vPag = i.Total_forma
                            };

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

                            if (vendanfce.Indpres != 5)
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
                    MessageBox.Show(ex.Message, "Erro - Cod: GetPagamento");

                return listapagamento;
            }

        }

        protected virtual cobr GetCobranca()
        {
            var c = new cobr()
            {
                dup = new List<dup>()
            };

            try
            {
                VendaNFCePG vendanfcepg = new VendaNFCePG();
                List<VendaNFCePG> lista = vendanfcepg.GetItensdePG(vendanfce.Id_vendanfce);
                int contador = 0;
                decimal valorliq = 0;
                if (lista.Count > 0)
                    foreach (VendaNFCePG i in lista)
                    {
                        if ((FormaPagamento)i.Formapg_id == FormaPagamento.fpOutro || (FormaPagamento)i.Formapg_id == FormaPagamento.fpBoletoBancario)
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