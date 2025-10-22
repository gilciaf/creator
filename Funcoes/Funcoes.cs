using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DFe.Classes.Entidades;
using MahApps.Metro.Controls;
using Microsoft.Win32;

namespace nfecreator
{
    public static class Funcoes
    {

        public static void Mensagem(string mensagem, string titulo, MessageBoxButton botoes, MessageBoxImage imagem = MessageBoxImage.None)
        {
            MessageBox.Show(mensagem, titulo, botoes, imagem);
        }

        /// <summary>
        ///     Abre um dialógo para o usuário digitar algo
        /// </summary>
        public static string InpuBox(MetroWindow owner, string titulo, string descricao, string valorPadrao = "")
        {
            var inputBox = new InputBoxWindow
            {
                Title = titulo,
                TxtDescricao = { Text = descricao },
                TxtValor = { Text = valorPadrao },
                Owner = owner
            };
            inputBox.ShowDialog(); 
            return inputBox.TxtValor.Text;


        }

        /// <summary>
        ///     Abre o diálogo de busca de arquivo com o filtro configurado para arquivos do tipo ".xml"
        /// </summary>
        /// <returns></returns>
        public static string BuscarArquivoZIP()
        {
            return BuscarArquivo("Selecione o arquivo ZIP", ".ZIP", "Arquivo ZIP (.zip)|*.zip");
        }

        public static string BuscarArquivoSql()
        {
            return BuscarArquivo("Selecione o arquivo SQL", ".sql", "Arquivo SQL (.sql)|*.sql");
        }
        public static string BuscarArquivoXml()
        {
            return BuscarArquivo("Selecione o arquivo XML", ".xml", "Arquivo XML (.xml)|*.xml");
        }

        public static string BuscarArquivoPdf()
        {
            return BuscarArquivo("Selecione o arquivo Pdf", ".pdf", "Arquivo Pdf (.pdf)|*.pdf");
        }

        /// <summary>
        ///     Abre o diálogo de busca de arquivo com o filtro configurado para arquivos do tipo ".pfx ou todos os arquivos (*.*)"
        /// </summary>
        /// <returns></returns>
        public static string BuscarArquivoCertificado()
        {
            return BuscarArquivo("Selecione o arquivo de Certificado", ".pfx", "Arquivos PFX (*.pfx)|*.pfx| Aquivo P12 (*.p12)|*.p12|Todos os Arquivos (*.*)|*.*");
        }

        /// <summary>
        ///     Abre o diálogo de busca de arquivo com o filtro configurado para arquivos do tipo "PNG, Bitmap, JPEG, JPG e GIF"
        /// </summary>
        /// <returns></returns>
        public static string BuscarImagem()
        {
            return BuscarArquivo("Selecione uma imagem", ".png", "PNG (*.png)|*.png|Bitmap (*.bmp)|*.bmp|JPEG (*.jpeg)|*.jpeg|JPG (*.jpg)|*.jpg|GIF (*.gif)|*.gif");
        }

        /// <summary>
        ///     Abre o diálogo de busca de arquivo com com os dados passados no parâmetro
        /// </summary>
        public static string BuscarArquivo(string titulo, string extensaoPadrao, string filtro, string arquivoPadrao = null)
        {
            var dlg = new OpenFileDialog
            {
                Title = titulo,
                FileName = arquivoPadrao,
                DefaultExt = extensaoPadrao,
                Filter = filtro
            };
            dlg.ShowDialog();
            return dlg.FileName;
        }

       

        public static string Deixarnumero(string tirartudo)
        {
            string limpo = "";
            if (tirartudo != null)
            {
                var apenasDigitos = new Regex(@"[^\d]");
                limpo = apenasDigitos.Replace(tirartudo, "");
                limpo = limpo.Replace(".", "").Replace("-", "").Replace("/", "").Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
            }
            return limpo;
        }


        public static void KeyDown(KeyEventArgs e, Window form)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                var uie = e.OriginalSource as UIElement;
                e.Handled = true;
                uie.MoveFocus(
                new TraversalRequest(
                FocusNavigationDirection.Next));

                IInputElement focusedControl = FocusManager.GetFocusedElement(form);

                if (focusedControl is TextBox textBox)
                {
                    textBox.Focus();
                    textBox.SelectAll();
                }



            }
        }

        public static void RetornarMascaraCpfCnpj(object sender)
        {
            try
            {
                TextBox txt = (TextBox)sender;
                int posicao = txt.SelectionStart;
                string texto = txt.Text.Replace(".", "").Replace("-", "").Replace("/", "");
                if (texto.Length == 11)
                {
                    long CPF = Convert.ToInt64(texto);
                    string CPFFormatado = String.Format(@"{0:000\.000\.000\-00}", CPF);
                    txt.Text = CPFFormatado;
                    posicao += 3;
                }
                if (texto.Length == 13)
                {
                    long CPF = Convert.ToInt64(texto);
                    string CPFFormatado = String.Format(@"{0:00\.000\.000\/0000\-0}", CPF);
                    txt.Text = CPFFormatado;
                    posicao += 4;
                }

                if (texto.Length == 14)
                {
                    long CPF = Convert.ToInt64(texto);
                    string CPFFormatado = String.Format(@"{0:00\.000\.000\/0000\-00}", CPF);
                    txt.Text = CPFFormatado;
                    posicao += 4;
                }
                txt.Select(posicao, 0);
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "AVISO MASCARA");
            }
        }

        public static void RetornarMascaraData(object sender)
        {
            try
            {
                DatePicker txt = (DatePicker)sender;

                string texto = txt.Text.Replace(".", "").Replace("-", "").Replace("/", "");
                if (texto.Length == 4)
                {
                    long data = Convert.ToInt64(texto);
                    string dataFormatado = String.Format(@"{0:00/00}", data);
                    txt.Text = dataFormatado;


                }
                if (texto.Length == 6)
                {
                    long data = Convert.ToInt64(texto);
                    string dataFormatado = String.Format(@"{0:00/00/00}", data);
                    txt.Text = dataFormatado;

                }

                if (texto.Length == 8)
                {
                    long data = Convert.ToInt64(texto);
                    string dataFormatado = String.Format(@"{0:00/00/0000}", data);
                    txt.Text = dataFormatado;



                }
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "AVISO MASCARA");
            }
        }

        public static void RetornarMascaraHora(object sender)
        {
            try
            {
                TextBox txt = (TextBox)sender;
                string texto = txt.Text.Replace(":", "").Replace(".", "").Replace("-", "").Replace("/", "");
                if (texto.Length == 4)
                {
                    long data = Convert.ToInt64(texto);
                    string dataFormatado = String.Format(@"{0:00:00}", data);
                    txt.Text = dataFormatado;


                }
                if (texto.Length == 5)
                {
                    long data = Convert.ToInt64(texto);
                    string dataFormatado = String.Format(@"{0:00:00}", data);
                    txt.Text = dataFormatado;

                }


            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                    Funcoes.Mensagem(string.Format("{0} \n\n {1}",
                        ex.Message, ex.InnerException), "Atenção!",
                        MessageBoxButton.OK);
            }
        }

        public static void RetornarMascaraDinheiro(object sender)
        {
            TextBox txt = (TextBox)sender;
            try
            {
                if (txt.Text.Trim() == string.Empty)
                {
                    txt.Text = "0";
                }
                string texto = txt.Text.Replace("-", "").Replace("/", "").Replace(".", "");
                texto = txt.Text.Replace(" ", "").Replace("R$", "");

                // texto = new string(texto.Where(char.IsDigit).ToArray());
                if (texto == "") texto = "0";

                decimal numeros = Convert.ToDecimal(texto);

                string CPFFormatado = String.Format(@"{0:0\,00}", numeros);

                CPFFormatado = numeros.ToString("#0.00##");

                txt.Text = CPFFormatado;

            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "AVISO MASCARA", false);
                MessageBox.Show("Verifique o campo - " + txt.Name + " - Valor informado invalido! Error:" + ex.Message, " Valor invalido para o sistema! ");
            }

        }

        public static void RetornarMascaraTelefone(object sender)
        {
            try
            {
                TextBox txt = (TextBox)sender;
                int posicao = txt.SelectionStart;


                string texto = txt.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "").Replace("+", "");
                if (texto.Length == 8)
                {
                    long TEL = Convert.ToInt64(texto);
                    string TELFormatado = String.Format(@"{0:0000\-0000}", TEL);
                    texto = txt.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
                    txt.Text = TELFormatado;
                    posicao += 1;
                }
                else if (texto.Length == 9)
                {
                    long TEL = Convert.ToInt64(texto);
                    string TELFormatado = String.Format(@"{0:0 0000\-0000}", TEL);
                    texto = txt.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
                    txt.Text = TELFormatado;
                    posicao += 2;

                }
                else if (texto.Length == 10)
                {
                    long TEL = Convert.ToInt64(texto);
                    string TELFormatado = String.Format(@"{0:\(00\) 0000\-0000}", TEL);
                    texto = txt.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
                    txt.Text = TELFormatado;
                    posicao += 4;

                }
                else if (texto.Length == 11)
                {
                    long TEL = Convert.ToInt64(texto);
                    string TELFormatado = String.Format(@"{0:\(00\) 0 0000\-0000}", TEL);
                    texto = txt.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
                    txt.Text = TELFormatado;
                    posicao += 5;
                }
                else if (texto.Length == 12)
                {
                    long TEL = Convert.ToInt64(texto);
                    string TELFormatado = String.Format(@"{0:+00 \(00\) 0000\-0000}", TEL);
                    texto = txt.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
                    txt.Text = TELFormatado;
                    posicao += 5;
                }
                else if (texto.Length == 13)
                {
                    long TEL = Convert.ToInt64(texto);
                    string TELFormatado = String.Format(@"{0:+00 \(00\) 0 0000\-0000}", TEL);
                    texto = txt.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
                    txt.Text = TELFormatado;
                    posicao += 5;
                }
                else
                {
                    //   MessageBox.Show("Formato de TELEFONE invalido", "Verifique o CEP! ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                txt.Select(posicao, 0);
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "AVISO MASCARA TELEFONE");
            }
        }

        public static void RetornaMascaraCEP(object sender)
        {
            try
            {
                TextBox txt = (TextBox)sender;
                int posicao = txt.SelectionStart;
                string texto = txt.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "").Replace("+", "");
                if (texto.Length == 8)
                {
                    long TEL = Convert.ToInt64(texto);
                    string TELFormatado = String.Format(@"{0:00000\-000}", TEL);
                    txt.Text = TELFormatado;
                    posicao += 1;
                }
                else
                {
                    Mensagem("O Cep não estava em um formato válido. Um dos seguintes formatos eram esperados: 00000000 ou 00000-000", "Verifique o CEP! ", MessageBoxButton.OK);
                }

                txt.Select(posicao, 0);
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "AVISO MASCARA CEP");
            }
        }

        public static void RetornarMascaraPlaca(object sender)
        {
            try
            {
                TextBox txt = (TextBox)sender;
                int posicao = txt.SelectionStart;


                string texto = txt.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
                if (texto.Length == 7)
                {
                    txt.Text = texto;
                    posicao += 1;
                }
                else if (texto.Length > 7)
                {
                    txt.Text = texto.Substring(0, 6);
                    posicao += 1;
                }

                else
                {
                    //   MessageBox.Show("Formato de TELEFONE invalido", "Verifique o CEP! ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                txt.Select(posicao, 0);
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "AVISO MASCARA");
            }
        }

        public static void RetornaVirgulaCampo(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ",")
            {
                if (!((TextBox)sender).Text.Contains(","))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;

        }


        public static DateTime GetDataInternet()
        {
            DateTime dateTime = DateTime.MinValue;
            try
            {
                MySQLSITE sqlsite = new MySQLSITE();
                string dataehora = sqlsite.Executacomando("SELECT NOW()");
                dateTime = DateTime.ParseExact(dataehora, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                return dateTime;
            }
            catch (Exception)
            {
                try
                {
                    TcpClient client = new TcpClient("time.nist.gov", 13);
                    using (var streamReader = new StreamReader(client.GetStream()))
                    {
                        var response = streamReader.ReadToEnd();
                        var utcDateTimeString = response.Substring(7, 17);
                        dateTime = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                    }

                    return dateTime;
                }
                catch (Exception ex)
                {
                    Funcoes.Crashe(ex, "", false);
                    return DateTime.Now;
                }
            }
        }

        public static Estado EstadoString(string uf)
        {
            Estado ufEmpresa;
            switch (uf)
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
            return ufEmpresa;
        }

        public static void Analitico(string texto = "ANALITICO")
        {
            //Analytics.TrackEvent(texto);
        }

        public static void Crashe(Exception exception, string title = "ERRO", bool message = true)
        {
            try
            {
                //AppCenter.LogLevel = LogLevel.Verbose;
                //Crashes.TrackError(exception);
                //Analytics.TrackEvent("Error");
                if (message)
                {
                    if (exception.InnerException != null)
                        MessageBox.Show(exception.Message + " Detalhes: " + exception.InnerException.Message, title);
                    else if (!string.IsNullOrEmpty(exception.Message))
                        MessageBox.Show(exception.Message, title);

                }
                else
                    Console.WriteLine(exception.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static string RemoverAcentos(this string valor)
        {
            if (string.IsNullOrEmpty(valor))
                return valor;

            valor = Regex.Replace(valor, "[áàâãª]", "a");
            valor = Regex.Replace(valor, "[ÁÀÂÃÄ]", "A");
            valor = Regex.Replace(valor, "[éèêë]", "e");
            valor = Regex.Replace(valor, "[ÉÈÊË]", "E");
            valor = Regex.Replace(valor, "[íìîï]", "i");
            valor = Regex.Replace(valor, "[ÍÌÎÏ]", "I");
            valor = Regex.Replace(valor, "[óòôõöº]", "o");
            valor = Regex.Replace(valor, "[ÓÒÔÕÖ]", "O");
            valor = Regex.Replace(valor, "[úùûü]", "u");
            valor = Regex.Replace(valor, "[ÚÙÛÜ]", "U");
            valor = Regex.Replace(valor, "[Ç]", "C");
            valor = Regex.Replace(valor, "[ç]", "c");

            return valor;
        }

        public static string GetComputer_InternetIP()
        {
            try
            {
                // check IP using DynDNS's service
                WebRequest request = WebRequest.Create("http://checkip.dyndns.org");
                WebResponse response = request.GetResponse();
                StreamReader stream = new StreamReader(response.GetResponseStream());

                // IMPORTANT: set Proxy to null, to drastically INCREASE the speed of request
                // request.Proxy = null;

                // read complete response
                string ipAddress = stream.ReadToEnd();

                // replace everything and keep only IP
                return ipAddress.
                    Replace("\r\n", "").
                    Replace("<html><head><title>Current IP Check</title></head><body>Current IP Address: ", string.Empty).
                    Replace("</body></html>", string.Empty);
            }
            catch (Exception ex)
            {
                Crashe(ex, "", false);
                return "0";
            }
        }

    }
}
