using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace nfecreator
{
    class DeOlhoNoImposto
    {
        [JsonProperty(PropertyName = "Codigo")]
        public int Codigo { get; set; }

        [JsonProperty(PropertyName = "UF")]
        public string UF { get; set; }

        [JsonProperty(PropertyName = "EX")]
        public string EX { get; set; }

        [JsonProperty(PropertyName = "Descricao")]
        public string Descricao { get; set; }

        [JsonProperty(PropertyName = "Nacional")]
        public decimal Nacional { get; set; }

        [JsonProperty(PropertyName = "Estadual")]
        public decimal Estadual { get; set; }

        [JsonProperty(PropertyName = "Importado")]
        public decimal Importado { get; set; }

        [JsonProperty(PropertyName = "Municipal")]
        public decimal Municipal { get; set; }

        [JsonProperty(PropertyName = "Tipo")]
        public string Tipo { get; set; }

        [JsonProperty(PropertyName = "VigenciaInicio")]
        public string VigenciaInicio { get; set; }

        [JsonProperty(PropertyName = "VigenciaFim")]
        public string VigenciaFim { get; set; }

        [JsonProperty(PropertyName = "Chave")]
        public string Chave { get; set; }

        [JsonProperty(PropertyName = "Versao")]
        public string Versao { get; set; }

        [JsonProperty(PropertyName = "Fonte")]
        public string Fonte { get; set; }

        [JsonProperty(PropertyName = "Valor")]
        public decimal Valor { get; set; }

        [JsonProperty(PropertyName = "ValorTributoNacional")]
        public decimal ValorTributoNacional { get; set; }

        [JsonProperty(PropertyName = "ValorTributoEstadual")]
        public decimal ValorTributoEstadual { get; set; }

        [JsonProperty(PropertyName = "ValorTributoImportado")]
        public decimal ValorTributoImportado { get; set; }

        [JsonProperty(PropertyName = "ValorTributoMunicipal")]
        public decimal ValorTributoMunicipal { get; set; }


        public DeOlhoNoImposto() { }

        public Decimal GetValorAproxTributos()
        {
            decimal vlaproxtrib = 0;
            try
            {
                //+ ValorTributoImportado
                vlaproxtrib = ValorTributoNacional + ValorTributoEstadual + ValorTributoMunicipal;
                return vlaproxtrib;
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "", false);
                return vlaproxtrib;
            }
        }

        public DeOlhoNoImposto Requisicao(string NCM, string valor, string uf)
        {
            DeOlhoNoImposto novo = new DeOlhoNoImposto();
            try
            {
                string token = "Lvl6gv_CyOrnRp1DtJiy5WmONpVG3SMY31lWuDlGnByriEbPBV39FUVrsnQ1Nhkn";

                string UrlRequisicao = @"https://apidoni.ibpt.org.br/api/v1/produtos?token=" + token;
                UrlRequisicao += @"&cnpj=03054280000109";
                UrlRequisicao = UrlRequisicao + @"&codigo=" + NCM;
                UrlRequisicao = UrlRequisicao + @"&uf=" + uf;
                UrlRequisicao += @"&ex=0&descricao=PIZZA";
                UrlRequisicao += @"&unidadeMedida=UN";
                UrlRequisicao = UrlRequisicao + @"&valor=" + valor.Replace(".", "").Replace(",", ".");
                UrlRequisicao += @"&gtin=12345678";



                const string _mediaType = "application/json";
                const string _charSet = "UTF-8";


                var req = (HttpWebRequest)WebRequest.Create(UrlRequisicao);
                req.Method = "GET";
                req.ContentType = _mediaType + ";charset=" + _charSet;
                // req.ContentLength = data.Length;
                req.Accept = _mediaType;
                req.Headers.Add(HttpRequestHeader.AcceptCharset, _charSet);

                //Credenciais de Acesso
                //String header = email + ":" + chaveSeguranca;
                //String headerBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(header));
                //req.Headers.Add(HttpRequestHeader.Authorization, "Basic " + headerBase64);
                //  req.GetRequestStream().Write(data, 0, data.Length);
                var response = (HttpWebResponse)req.GetResponse();
                var bodyResposta = response.GetResponseStream();
                using (var reader = new StreamReader(bodyResposta))
                {
                    var jsonretorno = reader.ReadToEnd().Replace("null", "0");
                    novo = JsonConvert.DeserializeObject<DeOlhoNoImposto>(jsonretorno);
                }
                return novo;
            }
            catch (Exception ex)
            {
                Funcoes.Crashe(ex, "", false);
                return novo;
            }
        }
    }
}
