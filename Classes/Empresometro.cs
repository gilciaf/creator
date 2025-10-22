using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace nfecreator.Classes
{
    class Empresometro
    {
        Clientes cliente;

        public Empresometro() { }

        public Empresometro(string CNPJ)
        {

            try
            {
                string UrlRequisicao = @"https://www.receitaws.com.br/v1/cnpj/" + CNPJ;

                const string _mediaType = "application/json";
                const string _charSet = "UTF-8";

                var req = (HttpWebRequest)WebRequest.Create(UrlRequisicao);
                req.Method = "GET";
                req.ContentType = _mediaType + ";charset=" + _charSet;
                req.Accept = _mediaType;
                req.Headers.Add(HttpRequestHeader.AcceptCharset, _charSet);

                var response = (HttpWebResponse)req.GetResponse();
                var bodyResposta = response.GetResponseStream();
                using (var reader = new StreamReader(bodyResposta))
                {
                    var jsonretorno = reader.ReadToEnd();
                    cliente = JsonConvert.DeserializeObject<Clientes>(jsonretorno);

                }

                if (cliente.Nome != null)
                    MessageBox.Show("Nome do cliente encontrado: " + cliente.Nome.ToString());
                /// string result = System.Text.Encoding.UTF8.GetString(response);
                else
                    MessageBox.Show("CNPJ não encontrado");
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "AVISO - EMPRESOMETRO");
            }
        }

        internal Clientes Cliente { get => cliente; set => cliente = value; }
    }
}