using System.Drawing;
using System.IO;


namespace nfecreator
{
    public class ConfiguracaoDanfe
    {
        /// <summary>
        /// Logomarca do emitente a ser impressa no DANFE da NFCe
        /// </summary>
        public byte[] Logomarca { get; set; }

        /// <summary>
        /// Determina se deve ser impresso uma tarja "DOCUMENTO CANCELADO", indicando que o DANFE impresso refere-se ao DANFE de uma NFe cancelada
        /// </summary>
        public bool DocumentoCancelado { get; set; }

        /// <summary>
        /// Retorna um objeto do tipo <see cref="Image"/> a partir da logo armazenada na propriedade <see cref="Logomarca"/>
        /// </summary>
        /// <returns></returns>
        public Image ObterLogo()
        {
            if (Logomarca == null)
                return null;
            var ms = new MemoryStream(Logomarca);
            var image = Image.FromStream(ms);
            return image;
        }
    }
}
