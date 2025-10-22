using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nfecreator
{
    public enum ImprimirUnidQtdeValor
    {
        Comercial,
        Tributavel,
        Ambos
    }

    public class ConfiguracaoDanfeNfe : ConfiguracaoDanfe
    {
        public ConfiguracaoDanfeNfe(byte[] logomarca = null, bool duasLinhas = true, bool documentoCancelado = false, bool quebrarLinhasObservacao = true, bool tributos = false) : this()
        {
            Logomarca = logomarca;
            DuasLinhas = duasLinhas;
            DocumentoCancelado = documentoCancelado;
            QuebrarLinhasObservacao = quebrarLinhasObservacao;
            ExibirTotalTributos = tributos;
        }

        /// <summary>
        /// Construtor sem parâmetros para serialização
        /// </summary>
        private ConfiguracaoDanfeNfe()
        {
            Logomarca = null;
            DuasLinhas = true;
            DocumentoCancelado = false;
            QuebrarLinhasObservacao = true;
            ExibirResumoCanhoto = true;
            ResumoCanhoto = string.Empty;
            ChaveContingencia = string.Empty;
            ExibeCampoFatura = false;
            ImprimirISSQN = true;
            ImprimirDescPorc = false;
            ImprimirTotalLiquido = false;
            ImprimirUnidQtdeValor = ImprimirUnidQtdeValor.Comercial;
            ExibirTotalTributos = false;
            DecimaisValorUnitario = 2;
            DecimaisQuantidadeItem = 2;
        }

        public bool DuasLinhas { get; set; }

        public bool QuebrarLinhasObservacao { get; set; }

        public bool ExibeCampoFatura { get; set; }

        public bool ExibirResumoCanhoto { get; set; }

        public string ResumoCanhoto { get; set; }

        public string ChaveContingencia { get; set; }

        public bool ImprimirISSQN { get; set; }

        public bool ImprimirDescPorc { get; set; }

        public bool ImprimirTotalLiquido { get; set; }

        public ImprimirUnidQtdeValor ImprimirUnidQtdeValor { get; set; }

        public bool ExibirTotalTributos { get; set; }
        public int DecimaisValorUnitario { get; set; }
        public int DecimaisQuantidadeItem { get; set; }

        public string EmailAut { get; set; }

        public string ImpressaoLote { get; set; }

        public string EmailCopia { get; set; }

        public bool SalvarServidor { get; set; }

        public bool Ipidevol { get; set; }

        public bool CodAut { get; set; }

        public bool InfAdProd { get; set; }
        public bool Vicmsdeson { get; set; }


    }
}