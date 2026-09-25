using Microsoft.AspNetCore.Mvc;
using Proyecto1_FelipeBrenes.Models;
using Proyecto1_FelipeBrenes.Services;
using System.Linq;
using System.Text.RegularExpressions;

namespace Proyecto1_FelipeBrenes.Controllers
{
    // Controlador MVC para la gestión de empleados: CRUD desde vistas que consumen la API.
    public class EmpleadosController : Controller

    {

        // Servicio que realiza las llamadas HTTP a la API.
        private readonly ApiService _api;

        public EmpleadosController(ApiService api)
        {
            _api = api;
        }
        // Función que devuelve el INDEX
        // GET /Empleados/Index
        public IActionResult Index()
        {
            return View();
        }

        // Función para crear un empleado
        // GET /Empleados/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        // POST /Empleados/Create
        // Funcion para validar el modelo y crea el empleado a través de la API.
        public async Task<IActionResult> Create(Empleado empleado)
        {
            if (!ModelState.IsValid)
                return View(empleado);

            var response = await _api.PostAsync(
                "api/empleados",
                empleado);

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensaje"] =
                    "Empleado creado correctamente.";

                return RedirectToAction(nameof(Create));
            }

            var mensaje =
                await response.Content.ReadAsStringAsync();

            ModelState.AddModelError("", mensaje);

            return View(empleado);
        }

        // Función para leer los empleados o buscar un empleado por su identificación
        // GET /Empleados/Read
        public async Task<IActionResult> Read()
        {
            var empleados = await _api.GetAsync<List<Empleado>>("api/empleados");

            return View(empleados);
        }

        // Función para editar un empleado
        // GET /Empleados/Edit
        public async Task<IActionResult> Edit()
        {
            var empleados =
                await _api.GetAsync<List<Empleado>>("api/empleados");

            if (empleados == null || empleados.Count == 0)
            {
                ViewBag.Mensaje = "⚠️ No hay empleados para editar.";
            }

            return View(empleados);
        }

        [HttpPost]
        // POST /Empleados/Edit
        // Actualiza el empleado mediante PUT a la API y muestra resultado en TempData.
        public async Task<IActionResult> Edit(Empleado empleado)
        {
            if (!ModelState.IsValid)
            {
                var lista =
                    await _api.GetAsync<List<Empleado>>("api/empleados");

                return View(lista);
            }

            var response = await _api.PutAsync(
                $"api/empleados/{empleado.Identificacion}",
                empleado);

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensaje"] =
                    "Empleado actualizado correctamente.";
            }
            else
            {
                TempData["Mensaje"] =
                    await response.Content.ReadAsStringAsync();
            }

            return RedirectToAction(nameof(Edit));
        }

        // Función para eliminar un empleado
        // GET /Empleados/Delete
        public async Task<IActionResult> Delete()
        {
            var empleados =
                await _api.GetAsync<List<Empleado>>("api/empleados");

            if (empleados == null || empleados.Count == 0)
            {
                ViewBag.Mensaje = "⚠️ No hay empleados para eliminar.";
            }

            return View(empleados);
        }

        [HttpPost]
        // POST /Empleados/Delete
        // Funcion para llamar al endpoint DELETE de la API y notifica el resultado.
        public async Task<IActionResult> Delete(string id)
        {
            var response =
                await _api.DeleteAsync($"api/empleados/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensaje"] =
                    "Empleado eliminado correctamente.";
            }
            else
            {
                TempData["Mensaje"] =
                    await response.Content.ReadAsStringAsync();
            }

            return RedirectToAction(nameof(Delete));
        }

        [HttpPost]
        // POST /Empleados/Buscar
        // Valida la entrada, obtiene empleado por identificación y prepara resultados para la vista Read.
        public async Task<IActionResult> Buscar(string identificacion)
        {
            if (string.IsNullOrWhiteSpace(identificacion))
            {
                TempData["Mensaje"] = "Debe ingresar una identificación para buscar.";

                var empleados = await _api.GetAsync<List<Empleado>>("api/empleados") ?? [];
                return View("Read", empleados);
            }

            var empleado = await _api.GetByIdAsync<Empleado>(
                $"api/empleados/buscar/{identificacion}");

            ViewBag.Resultados = empleado is null ? [] : new List<Empleado> { empleado };
            var lista = await _api.GetAsync<List<Empleado>>("api/empleados") ?? [];
            return View("Read", lista);
        }
    }
}
