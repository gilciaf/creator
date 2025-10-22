using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace nfecreator
{
    /// <summary>
    /// Lógica interna para InputBoxWindow.xaml
    /// </summary>
    public partial class InputBoxWindow : Window
    {
        public InputBoxWindow()
        {
            InitializeComponent();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            TxtValor.Text = null;
            Close();
            /*throw new Exception("");*/
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TxtValor_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            e.Handled = true;
            BtnOk.Focus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TxtValor.Focus();
        }
    }
}