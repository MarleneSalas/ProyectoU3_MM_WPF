using ClienteServidorProyectoU3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
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

namespace ClienteServidorProyectoU3.Views
{
    /// <summary>
    /// Interaction logic for ActividadesView.xaml
    /// </summary>
    public partial class ActividadesView : UserControl
    {
        public ActividadesView()
        {
            InitializeComponent();
            this.DataContext = App.ActividadesViewModel;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DepartamentoDTO? dep = ((ComboBox)sender).SelectedItem as DepartamentoDTO;
            if(App.ActividadesViewModel != null && dep!=null)
            {
                App.ActividadesViewModel.FiltrarCommand.Execute(dep.Id);

                //Vemos que onda con el boton
                if(dep.Id!=App.ActividadesViewModel.DepartamentoUser.Id)
                    RegistrarButton.Visibility = Visibility.Hidden;
                else
                    RegistrarButton.Visibility =Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var actActual = ((Button)sender).DataContext as ActividadDTO;
            if(actActual != null)
            {
                App.ActividadesViewModel.VerActividadCommand.Execute(actActual); 
            }
        }
    }
}
