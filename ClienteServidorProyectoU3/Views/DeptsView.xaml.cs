using ClienteServidorProyectoU3.Models;
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

namespace ClienteServidorProyectoU3.Views.UserControls
{
    /// <summary>
    /// Lógica de interacción para DeptsView.xaml
    /// </summary>
    public partial class DeptsView : UserControl
    {
        public DeptsView()
        {
            InitializeComponent();
            this.DataContext = App.ActividadesViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var depActual = ((Button)sender).DataContext as DepartamentoDTO;
            if (depActual != null)
            {
                App.ActividadesViewModel.VerDetallesDepartamentoCommand.Execute(depActual);
            }
        }
    }
}
