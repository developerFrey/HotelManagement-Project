using Microsoft.AspNetCore.Mvc;
using Proyecto1_FelipeBrenes.Models;
using Proyecto1_FelipeBrenes.Services;
using System.Linq;

namespace Proyecto1_FelipeBrenes.Controllers
{
    // Controlador MVC que maneja las vistas y acciones relacionadas con habitaciones.
    public class HabitacionesController : Controller
    {

        // Servicio para comunicarse con la API (realiza solicitudes HTTP).
        private readonly ApiService _api;

        // Inyección de dependencias: se recibe el servicio que encapsula llamadas a la API.
        public HabitacionesController(ApiService api)
        {
            _api = api;
        }
        // Función que devuelve la vista principal de habitaciones
        // GET /Habitaciones/Index
        public IActionResult Index()
        {
            return View();
        }

        // Función que devuelve la vista para crear una nueva habitación
        // GET /Habitaciones/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        // POST /Habitaciones/Create
        // Funcion que recibe el modelo `Habitacion` desde el formulario, valida los datos y hace POST a la API.
        public async Task<IActionResult> Create(Habitacion habitacion)
        {
            if (!ModelState.IsValid)
                return View(habitacion);

            var response =
                await _api.PostAsync(
                    "api/habitaciones",
                    habitacion);

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensaje"] =
                    "Habitación creada correctamente.";

                return RedirectToAction(nameof(Create));
            }

            // Si la API devuelve error, se añade al ModelState para mostrarlo en la vista.
            ModelState.AddModelError("",
                await response.Content.ReadAsStringAsync());

            return View(habitacion);
        }

        // Función que devuelve la vista para leer las habitaciones
        // GET /Habitaciones/Read
        public async Task<IActionResult> Read()
        {
            // Obtiene la lista completa desde la API y la pasa a la vista.
            var habitaciones =
                await _api.GetAsync<List<Habitacion>>
                ("api/habitaciones");

            return View(habitaciones);
        }

        // Función que devuelve la vista para editar las habitaciones
        // GET /Habitaciones/Edit
        public async Task<IActionResult> Edit()
        {
            var habitaciones =
                await _api.GetAsync<List<Habitacion>>
                ("api/habitaciones");

            if (habitaciones is null || habitaciones.Count == 0)
                ViewBag.Mensaje =
                    "⚠️ No hay habitaciones.";

            return View(habitaciones ?? []);
        }

        [HttpPost]
        // POST /Habitaciones/Edit
        // Funcion que actualiza los datos de la habitación usando PUT hacia la API.
        public async Task<IActionResult> Edit(Habitacion habitacion)
        {
            if (!ModelState.IsValid)
            {
                var lista =
                    await _api.GetAsync<List<Habitacion>>
                    ("api/habitaciones");

                return View(lista);
            }

            var response =
                await _api.PutAsync(
                $"api/habitaciones/{habitacion.Numero!.Value}",
                    habitacion);

            TempData["Mensaje"] =
                response.IsSuccessStatusCode
                ? "Habitación actualizada correctamente."
                : await response.Content.ReadAsStringAsync();

            return RedirectToAction(nameof(Edit));
        }

        // Función que devuelve la vista para eliminar una habitación
        // GET /Habitaciones/Delete
        public async Task<IActionResult> Delete()
        {
            var habitaciones =
                await _api.GetAsync<List<Habitacion>>
                ("api/habitaciones");

            if (habitaciones is null || habitaciones.Count == 0)
                ViewBag.Mensaje =
                    "⚠️ No hay habitaciones.";

            return View(habitaciones ?? []);
        }

        [HttpPost]
        // POST /Habitaciones/Delete
        // Funcion que realiza la eliminación mediante DELETE a la API.
        public async Task<IActionResult> Delete(int numero)
        {
            var response =
                await _api.DeleteAsync(
                    $"api/habitaciones/{numero}");

            TempData["Mensaje"] =
                response.IsSuccessStatusCode
                ? "Habitación eliminada correctamente."
                : await response.Content.ReadAsStringAsync();

            return RedirectToAction(nameof(Delete));
        }

        [HttpPost]
        // POST /Habitaciones/Buscar
        // Valida la entrada del usuario, solicita la habitación por id a la API y prepara los resultados.
        public async Task<IActionResult> Buscar(string numero)
        {
            // Se valida que el usuario haya digitado algo
            if (string.IsNullOrWhiteSpace(numero))
            {
                TempData["Mensaje"] = "Debe ingresar un número de habitación para buscar.";

                var habitaciones = await _api.GetAsync<List<Habitacion>>("api/habitaciones") ?? [];
                return View("Read", habitaciones);
            }

            // Validar que sea un número
            if (!int.TryParse(numero, out int numeroHabitacion))
            {
                TempData["Mensaje"] = "El número de habitación debe ser un valor numérico.";

                var habitaciones = await _api.GetAsync<List<Habitacion>>("api/habitaciones") ?? [];
                return View("Read", habitaciones);
            }

            // Buscar la habitación
            var habitacion = await _api.GetByIdAsync<Habitacion>(
                $"api/habitaciones/buscar/{numeroHabitacion}");

            ViewBag.Resultados = habitacion is null ? [] : new List<Habitacion> { habitacion };
            var lista = await _api.GetAsync<List<Habitacion>>("api/habitaciones") ?? [];
            return View("Read", lista);
        }
    }
}
