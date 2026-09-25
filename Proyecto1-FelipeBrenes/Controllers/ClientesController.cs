using Microsoft.AspNetCore.Mvc;
using Proyecto1_FelipeBrenes.Models;
using System.Linq;
using Proyecto1_FelipeBrenes.Services;

namespace Proyecto1.Controllers
{
    // Controlador MVC responsable de las vistas y acciones relacionadas con clientes.
    // Consume el ApiService para comunicarse con el ApiREST.
    public class ClientesController : Controller
    {

        // Servicio HTTP que realiza las llamadas a la API para operaciones sobre clientes.
        private readonly ApiService _api;

        // Inyección de dependencias del servicio API.
        public ClientesController(ApiService api)
        {
            _api = api;
        }
        // Función que devuelve el INDEX
        // GET /Clientes/Index
        public IActionResult Index()
        {
            return View();
        }


        // Función para crear un cliente
        // GET /Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        // POST /Clientes/Create
        // Funcion que valida el modelo y envía la petición POST a la API para crear el cliente.
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);

            var response = await _api.PostAsync(
                "api/clientes",
                cliente);

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensaje"] = "Cliente creado correctamente.";

                return RedirectToAction(nameof(Create));
            }

            var mensaje = await response.Content.ReadAsStringAsync();

            // Agrega el mensaje de error de la API al ModelState para visualizarlo en la vista.
            ModelState.AddModelError("", mensaje);

            return View(cliente);
        }


        // Función para leer la lista de clientes o buscar clientes por identificacion
        // GET /Clientes/Read
        public async Task<IActionResult> Read()
        {
            // Obtiene todos los clientes desde la API y los pasa a la vista.
            var clientes =
                await _api.GetAsync<List<Cliente>>("api/clientes");

            return View(clientes);
        }

        // Si la lista esta vacia , se muestra un mensaje de advertencia
        // GET /Clientes/Edit (muestra lista para editar)
        public async Task<IActionResult> Edit()
        {
            var clientes = await _api.GetAsync<List<Cliente>>("api/clientes");

            if (clientes == null || clientes.Count == 0)
            {
                ViewBag.Mensaje = "⚠️ No hay clientes para editar.";
            }

            return View(clientes);
        }

        [HttpPost]
        // POST /Clientes/Edit
        // Funcion que valida y envía la actualización del cliente mediante PUT a la API.
        public async Task<IActionResult> Edit(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                var lista = await _api.GetAsync<List<Cliente>>("api/clientes");
                return View(lista);
            }

            var response = await _api.PutAsync(
                $"api/clientes/{cliente.Identificacion}",
                cliente);

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensaje"] = "Cliente actualizado correctamente.";
            }
            else
            {
                TempData["Mensaje"] = await response.Content.ReadAsStringAsync();
            }

            return RedirectToAction(nameof(Edit));
        }


        // Función para eliminar un cliente, si el cliente tiene reservaciones asociadas, no se puede eliminar
        // GET /Clientes/Delete (muestra lista para eliminar)
        public async Task<IActionResult> Delete()
        {
            var clientes = await _api.GetAsync<List<Cliente>>("api/clientes");

            if (clientes == null || clientes.Count == 0)
            {
                ViewBag.Mensaje = "⚠️ No hay clientes para eliminar.";
            }

            return View(clientes);
        }

        [HttpPost]
        // POST /Clientes/Delete
        // Funcion que llama al endpoint DELETE de la API y muestra el resultado al usuario.
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _api.DeleteAsync($"api/clientes/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensaje"] = "Cliente eliminado correctamente.";
            }
            else
            {
                TempData["Mensaje"] = await response.Content.ReadAsStringAsync();
            }

            return RedirectToAction(nameof(Delete));
        }

        [HttpPost]
        public async Task<IActionResult> Buscar(string identificacion)
        {

            if (string.IsNullOrWhiteSpace(identificacion))
            {
                TempData["Mensaje"] =
                    "Debe ingresar una Identificacion.";

                var lista = await _api.GetAsync<List<Cliente>>("api/clientes") ?? [];
                return View("Read", lista);
            }
            // Obtiene un cliente por identificación desde la API.
            var cliente = await _api.GetByIdAsync<Cliente>(
                $"api/clientes/buscar/{identificacion}");

            // Prepara los resultados para mostrarlos de forma parcial en la vista Read.
            ViewBag.Resultados = cliente is null ? [] : new List<Cliente> { cliente };
            var clientes = await _api.GetAsync<List<Cliente>>("api/clientes") ?? [];
            return View("Read", clientes);
        }
    }
}
