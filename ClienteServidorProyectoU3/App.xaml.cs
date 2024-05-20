using ClienteServidorProyectoU3.Services;
using ClienteServidorProyectoU3.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;

namespace ClienteServidorProyectoU3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static string Url = "https://test2.sistemas19.com/api/";
        //public static string Url = "https://localhost:44349/api/";
        public static DepartamentoService departamentoService = new();
        public static ActividadService actividadService = new();
        public static MainViewModel MainViewModel { get; set; } = new MainViewModel();
        public static ActividadesViewModel ActividadesViewModel { get; set; } = new();

        public static void MostrarError(string message)
        {
            MessageBox.Show(message, "Error",MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

}
