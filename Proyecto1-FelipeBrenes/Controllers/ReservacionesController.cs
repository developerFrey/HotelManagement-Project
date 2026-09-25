using Microsoft.AspNetCore.Mvc;
using Proyecto1_FelipeBrenes.Models;
using Proyecto1_FelipeBrenes.Services;

namespace Proyecto1.Controllers
{
    // Controlador MVC para gestionar reservaciones: vistas de creación, lectura, edición y eliminación.
    public class ReservacionesController : Controller
    {
        // Servicio HTTP que comunica con la API backend.
        private readonly ApiService _api;

        public ReservacionesController(ApiService api)
        {
            _api = api;
        }

        
        // Cargar clientes y habitaciones para los combos
        // Método auxiliar que obtiene listas necesarias para los formularios.
        private async Task CargarCombosReservacion()
        {
            ViewBag.Clientes =
                await _api.GetAsync<List<Cliente>>("api/clientes");

            ViewBag.Habitaciones =
                await _api.GetAsync<List<Habitacion>>("api/habitaciones");
        }

        
        // INDEX
        // Muestra la página principal de reservaciones.
        public IActionResult Index()
        {
            return View();
        }

        
        // CREATE (GET)
        // Muestra el formulario para crear una reservación y carga los combos.
        public async Task<IActionResult> Create()
        {
            await CargarCombosReservacion();

            return View();
        }

        
        // CREATE (POST)
        // Funcion que valida el modelo y envía la reservación a la API, muestra errores si los hay.
        [HttpPost]
        public async Task<IActionResult> Create(Reservacion r)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombosReservacion();
                return View(r);
            }

            var response =
                await _api.PostAsync("api/reservaciones", r);

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensaje"] =
                    "Reservación creada correctamente.";

                return RedirectToAction(nameof(Create));
            }

            var mensaje =
                await response.Content.ReadAsStringAsync();

            ModelState.AddModelError("", mensaje);

            await CargarCombosReservacion();

            return View(r);
        }

        
        // READ
        // Funcion que obtiene todas las reservaciones y las muestra en la vista.
        public async Task<IActionResult> Read()
        {
            var reservaciones =
                await _api.GetAsync<List<Reservacion>>
                ("api/reservaciones");

            return View(reservaciones);
        }

        // La API calcula la semana entrante como lunes a domingo posteriores a la semana actual.
        public async Task<IActionResult> ReporteSemanal()
        {
            var reservaciones = await _api.GetAsync<List<Reservacion>>(
                "api/reservaciones/reporte-semana-entrante") ?? [];
            return View(reservaciones);
        }

        
        [HttpPost]
        public async Task<IActionResult> Buscar(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                TempData["Mensaje"] =
                    "Debe ingresar un código de reservación.";

                var lista = await _api.GetAsync<List<Reservacion>>("api/reservaciones") ?? [];
                return View("Read", lista);
            }

            var reservacion =
                await _api.GetByIdAsync<Reservacion>(
                    $"api/reservaciones/buscar/{codigo}");

            ViewBag.Resultados = reservacion is null ? [] : new List<Reservacion> { reservacion };
            var reservaciones = await _api.GetAsync<List<Reservacion>>("api/reservaciones") ?? [];
            return View("Read", reservaciones);
        }
        
        // EDIT (GET)
        // Funcion que muestra la lista de reservaciones que se pueden editar y carga combos.
        public async Task<IActionResult> Edit()
        {
            var reservaciones =
                await _api.GetAsync<List<Reservacion>>("api/reservaciones");

            if (reservaciones == null || reservaciones.Count == 0)
            {
                ViewBag.Mensaje =
                    "⚠️ No hay reservaciones para editar.";
            }

            await CargarCombosReservacion();

            return View(reservaciones);
        }

        
        // EDIT (POST)
        // funcion que valida y actualiza la reservación mediante PUT a la API.
        [HttpPost]
        public async Task<IActionResult> Edit(Reservacion r)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombosReservacion();

                var lista =
                    await _api.GetAsync<List<Reservacion>>("api/reservaciones");

                return View(lista);
            }

            var response =
                await _api.PutAsync(
                    $"api/reservaciones/{r.CodigoReservacion}",
                    r);

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensaje"] =
                    "Reservación actualizada correctamente.";
            }
            else
            {
                TempData["Mensaje"] =
                    await response.Content.ReadAsStringAsync();
            }

            return RedirectToAction(nameof(Edit));
        }

        
        // DELETE (GET)
        // Funcion que muestra las reservaciones disponibles para eliminar.
        public async Task<IActionResult> Delete()
        {
            var reservaciones =
                await _api.GetAsync<List<Reservacion>>("api/reservaciones");

            if (reservaciones == null || reservaciones.Count == 0)
            {
                ViewBag.Mensaje =
                    "⚠️ No hay reservaciones para eliminar.";
            }

            return View(reservaciones);
        }

        // DELETE (POST)
        // Elimina una reservación mediante la API.
        [HttpPost]
        public async Task<IActionResult> Delete(string CodigoReservacion)
        {
            var response =
                await _api.DeleteAsync($"api/reservaciones/{CodigoReservacion}");

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensaje"] =
                    "Reservación eliminada correctamente.";
            }
            else
            {
                TempData["Mensaje"] =
                    await response.Content.ReadAsStringAsync();
            }

            return RedirectToAction(nameof(Delete));
        }
    }
}
