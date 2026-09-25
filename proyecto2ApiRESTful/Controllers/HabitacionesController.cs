using Microsoft.AspNetCore.Mvc;
using Proyecto3.AccesoDatos.Models;
using Proyecto2ApiRESTful.Services;

namespace Proyecto2ApiRESTful.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // Controlador API que expone endpoints para gestionar habitaciones (CRUD).
    public class HabitacionesController : ControllerBase
    {
        // Servicio que contiene la lógica de negocio y acceso a datos para habitaciones.
        private readonly HabitacionService _habitacionService;

        public HabitacionesController(HabitacionService habitacionService)
        {
            _habitacionService = habitacionService;
        }

        // GET - Funcion para obtener todas las habitaciones
        
        [HttpGet]
        public ActionResult<List<Habitacion>> Get()
        {
            return Ok(_habitacionService.ObtenerTodas());
        }

        // GET POR NUMERO - Funcion para obtener una habitación por su número
        
        [HttpGet("{numero}")]
        public ActionResult<Habitacion> Get(int numero)
        {
            var habitacion =
                _habitacionService.ObtenerPorNumero(numero);

            if (habitacion == null)
                return NotFound("Habitación no encontrada.");

            return Ok(habitacion);
        }

        
        // POST - Funcion para crear una nueva habitación
        
        [HttpPost]
        public IActionResult Post(Habitacion habitacion)
        {
            bool agregado =
                _habitacionService.Agregar(habitacion);

            if (!agregado)
                return BadRequest("Ya existe esa habitación.");

            return Ok(habitacion);
        }

        
        // PUT - Funcion para actualizar una habitación existente
        
        [HttpPut("{numero}")]
        public IActionResult Put(int numero,
            Habitacion habitacion)
        {
            bool actualizado =
                _habitacionService.Actualizar(numero,
                habitacion);

            if (!actualizado)
                return NotFound();

            return Ok();
        }

        
        // DELETE - Funcion para eliminar una habitación (si no tiene reservaciones)
        
        [HttpDelete("{numero}")]
        public IActionResult Delete(int numero)
        {
            bool eliminado =
                _habitacionService.Eliminar(numero);

            if (!eliminado)
                return BadRequest(
                    "La habitación posee reservaciones.");

            return Ok();
        }

        
        // BUSCAR - Funcion para buscar una habitación por número.
        
        [HttpGet("buscar/{numero}")]
        public IActionResult Buscar(int numero)
        {
            var habitacion =
                _habitacionService.ObtenerPorNumero(numero);

            if (habitacion == null)
                return NotFound();

            return Ok(habitacion);
        }
    }
}
