using System.Net.Http.Json;

namespace Proyecto1_FelipeBrenes.Services
{
    // Servicio HTTP cliente usado por el proyecto 1 para consumir la API REST del proyecto 2.
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        // HttpClient inyectado por DI.
        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Funcion que obtiene un recurso por URL y lo deserializa al tipo T.
        public async Task<T?> GetByIdAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        }

        // GET
        // Funcion que obtiene y deserializa una colección o recurso al tipo solicitado.
        public async Task<T?> GetAsync<T>(string url)
        {
            return await _httpClient.GetFromJsonAsync<T>(url);
        }

        
        // POST
        // Funcion que envía un objeto serializado como JSON mediante POST y devuelve la respuesta HTTP.
        public async Task<HttpResponseMessage> PostAsync<T>(string url, T objeto)
        {
            return await _httpClient.PostAsJsonAsync(url, objeto);
        }

        
        // PUT
        // Funcion que envía un objeto serializado como JSON mediante PUT y devuelve la respuesta HTTP.
        public async Task<HttpResponseMessage> PutAsync<T>(string url, T objeto)
        {
            return await _httpClient.PutAsJsonAsync(url, objeto);
        }

        
        // DELETE
        // Funcion que ejecuta una petición DELETE a la URL indicada.
        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            return await _httpClient.DeleteAsync(url);
        }
    }
}