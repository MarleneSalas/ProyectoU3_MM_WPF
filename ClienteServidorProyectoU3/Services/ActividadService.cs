using ClienteServidorProyectoU3.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClienteServidorProyectoU3.Services
{
    public class ActividadService
    {
        string url = App.Url;
        HttpClient client;

        public ActividadService()
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri(url)
            };
        }

        public async Task<MisActividadesYSubordinados> GetActividadesYSubordinadas(int idDep)
        {
            MisActividadesYSubordinados? misActividadesYSubordinados = new();

            var request =  await client.GetAsync("actividad/departamento/"+idDep);


            if (request.IsSuccessStatusCode)
            {
                var json = await request.Content.ReadAsStringAsync();
                misActividadesYSubordinados = JsonConvert.DeserializeObject<MisActividadesYSubordinados>(json);
            }

            if(misActividadesYSubordinados == null)
            {
                misActividadesYSubordinados = new();
            }

            return misActividadesYSubordinados;
        }

        public async Task<string> Post(ActividadDTO actividad)
        {
            var request = await client.PostAsJsonAsync("actividad", actividad);
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

        public async Task<string> Put(ActividadDTO actividad)
        {
            var request = await client.PutAsJsonAsync("actividad", actividad);
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

        public async Task<string> Delete(int idActividad)
        {
            var request = await client.DeleteAsync("actividad/" + idActividad);
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
