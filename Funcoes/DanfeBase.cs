using FastReport;
using FastReport.Export.Pdf;
using System;
using System.Windows;

namespace nfecreator
{
    public class DanfeBase : IDanfe
    {
        protected Report Relatorio;

        /// <summary>
        /// Abre a janela de visualização do DANFE da NFCe
        /// </summary>
        /// <param name="modal">Se true, exibe a visualização em Modal. O modo modal está disponível apenas para WinForms</param>
        public void Visualizar(bool modal = true)
        {
            try
            {
                Relatorio.Show(modal);
            }
            catch (Exception ex)
            {
                 if (!string.IsNullOrEmpty(ex.Message))
                    Funcoes.Mensagem(ex.Message, "Erro", MessageBoxButton.OK);

            }

        }

        /// <summary>
        ///  Abre a janela de visualização do design do DANFE da NFCe.
        /// Chame esse método se desja fazer alterações no design do DANFE em modo run-time
        /// </summary>
        /// <param name="modal">Se true, exibe a visualização em Modal. O modo modal está disponível apenas para WinForms</param>
        public void ExibirDesign(bool modal = false)
        {
            Relatorio.Design(modal);
        }

        /// <summary>
        /// Envia a impressão do DANFE da NFCe diretamente para a impressora
        /// </summary>
        /// <param name="exibirDialogo">Se true exibe o diálogo Imprimindo...</param>
        /// <param name="impressora">Passe a string com o nome da impressora para imprimir diretamente em determinada impressora. Caso contrário, a impressão será feita na impressora que estiver como padrão</param>
        public void Imprimir(bool exibirDialogo = false, string impressora = "")
        {
            Relatorio.Prepare();
            Relatorio.PrintSettings.ShowDialog = exibirDialogo;
            Relatorio.PrintSettings.Printer = impressora;
            Relatorio.Print();
        }

        /// <summary>
        /// Converte o DANFE para PDF e salva-o no caminho/arquivo indicado
        /// </summary>
        /// <param name="arquivo">Caminho/arquivo onde deve ser salvo o PDF do DANFE</param>
        public void ExportarPdf(string arquivo)
        {
            try
            {
                Relatorio.Prepare();
                Relatorio.Export(new PDFExport(), arquivo);
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                    Funcoes.Mensagem(ex.Message + "  Mais informações:" + ex.ToString(), "Erro ao imprimir.", MessageBoxButton.OK);
            }            
        }
    }

    public interface IDanfe
    {
        /// <summary>
        /// Envia a impressão do DANFE da NFCe diretamente para a impressora
        /// </summary>
        /// <param name="exibirDialogo">Se true exibe o diálogo Imprimindo...</param>
        /// <param name="impressora">Passe a string com o nome da impressora para imprimir diretamente em determinada impressora. Caso contrário, a impressão será feita na impressora que estiver como padrão</param>
        void Imprimir(bool exibirDialogo = true, string impressora = "");

        /// <summary>
        /// Converte o DANFE para PDF e salva-o no caminho/arquivo indicado
        /// </summary>
        /// <param name="arquivo">Caminho/arquivo onde deve ser salvo o PDF do DANFE</param>
        void ExportarPdf(string arquivo);
    }
}
