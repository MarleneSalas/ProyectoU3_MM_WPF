using ClienteServidorProyectoU3.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClienteServidorProyectoU3.Services
{
    public class DepartamentoService
    {
        string url = App.Url;
        HttpClient client;

        public DepartamentoService()
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri(url)
            };
        }

        public async Task<IEnumerable<DepartamentoDTO>> GetDeptos(int id)
        {
            IEnumerable<DepartamentoDTO>? deptos = Enumerable.Empty<DepartamentoDTO>(); ;
            var request = await client.GetAsync("departamento/" + id);

            if (request.IsSuccessStatusCode)
            {
                var json = await request.Content.ReadAsStringAsync();
                deptos = JsonConvert.DeserializeObject<IEnumerable<DepartamentoDTO>>(json);
            }

            if (deptos == null)
                deptos = Enumerable.Empty<DepartamentoDTO>();

            return deptos;


        }

        public async Task<string> Post(DepartamentoDTO departamento)
        {
            var request = await client.PostAsJsonAsync("departamento", departamento);
            if (request.IsSuccessStatusCode)
                return "";
            else
            {
                var message = await request.Content.ReadAsStringAsync();
                return message;
            }
        }

        public async Task<string> Put(DepartamentoDTO departamento)
        {
            var request = await client.PutAsJsonAsync("departamento", departamento);
            if (request.IsSuccessStatusCode)
                return "";
            else
            {
                var message = await request.Content.ReadAsStringAsync();
                return message;
            }
        }

        public async Task<string> Delete(int idDepto)
        {
            var request = await client.DeleteAsync("departamento/" + idDepto);
            if (request.IsSuccessStatusCode)
            {
                return "";
            }
            else
            {
                var message = await request.Content.ReadAsStringAsync();
                return message;
            }
        }
    }
}
