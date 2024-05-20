using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteServidorProyectoU3.Models
{
    public class DepartamentoDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = "";

        public string Username { get; set; } = "";

        public string Password { get; set; } = "";

        public int? IdSuperior { get; set; } = null;
    }
}
