using ClienteServidorProyectoU3.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClienteServidorProyectoU3.ViewModels
{
    public enum Vista
    {
        Login,
        Act,
        DetAct,
        RegAct,
        Dept,
        DetDept,
        RegDept
    }
    public class MainViewModel : INotifyPropertyChanged
    {

        private string vistaActual = "";

        public string VistaActual
        {
            get { return vistaActual; }
            set { vistaActual = value; Actualizar(nameof(VistaActual)); }
        }

        public MainViewModel()
        {
            CambiarVista(Vista.Login);
        }

        public void CambiarVista(Vista vista)
        {
            VistaActual =vista.ToString();
            Actualizar(nameof(VistaActual));
        }

        public void Actualizar(string nombre = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
