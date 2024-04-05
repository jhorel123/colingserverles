using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Coling.Shared;
using Coling.Vista.Modelos;
using Newtonsoft.Json;



namespace Coling.Vista.Servicios.Curriculum
{
    public class InstitucionService : IInstitucionService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:7110";

        public InstitucionService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        public async Task<List<Institucion>> ListaInstituciones(string token)
        {
            var endPoint = "api/ListarInstitucion";
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(endPoint);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<Institucion>>();
        }

        public async Task<bool> InsertarInstitucion(Institucion institucion, string token)
        {
            var endPoint = "api/InsertarInstitucion";
            string jsonBody = JsonConvert.SerializeObject(institucion);

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endPoint, content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EditarInstitucion(Institucion institucion, string token)
        {
            var endPoint = $"api/EditarInstitucion/{institucion.RowKey}";
            string jsonBody = JsonConvert.SerializeObject(institucion);

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(endPoint, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al editar la institución: {ex.Message}");
                return false;
            }
        }

        public async Task<Institucion> ObtenerInstitucionPorRowKey(string rowKey, string token)
        {
            var endPoint = $"api/ListarInstitucionById/{rowKey}";

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync(endPoint);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Institucion>();
                }
                else
                {
                    Console.WriteLine($"Error al obtener la institución. Código de estado: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener la institución: {ex.Message}");
                return null;
            }
        }
        public async Task<bool> BorrarInstitucion(string partitionKey, string rowKey, string token)
        {
            try
            {
                string endPoint = $"{BaseUrl}/api/BorrarInstitucion/{partitionKey}/{rowKey}";
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage respuesta = await _httpClient.DeleteAsync(endPoint);

                if (respuesta.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    // Manejo de errores, por ejemplo:
                    Console.WriteLine($"Error al borrar la institución. Código de estado: {respuesta.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones, por ejemplo:
                Console.WriteLine($"Error al borrar la institución: {ex.Message}");
                return false;
            }
        }

    }
}
