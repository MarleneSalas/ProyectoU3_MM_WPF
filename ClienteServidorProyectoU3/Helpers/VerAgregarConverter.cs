using ClienteServidorProyectoU3.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ClienteServidorProyectoU3.Helpers
{
    public class VerAgregarConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DepartamentoDTO? departamento = (DepartamentoDTO)value;
            if (departamento != null && departamento.Id == App.ActividadesViewModel.DepartamentoUser.Id)
            {
                return Visibility.Visible;
            }
            else
                return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
