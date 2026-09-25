using Microsoft.AspNetCore.Mvc;
using Proyecto3.AccesoDatos.Models;
using Proyecto2ApiRESTful.Services;

namespace Proyecto2ApiRESTful.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // Controlador API para administrar reservaciones: endpoints CRUD y búsqueda.
    public class ReservacionesController : ControllerBase
    {
        // Servicio con la lógica para manejar reservaciones y persistencia.
        private readonly ReservacionService _reservacionService;

        public ReservacionesController(ReservacionService reservacionService)
        {
            _reservacionService = reservacionService;
        }

        
        // GET - Funcion para obtener todas las reservaciones
        
        [HttpGet]
        public ActionResult<List<Reservacion>> Get()
        {
            return Ok(_reservacionService.ObtenerTodas());
        }

        // GET: api/reservaciones/reporte-semana-entrante
        // Semana entrante: lunes a domingo de la semana calendario posterior a la actual.
        [HttpGet("reporte-semana-entrante")]
        public ActionResult<List<Reservacion>> ReporteSemanaEntrante()
        {
            return Ok(_reservacionService.ObtenerSemanaEntrante());
        }

        
        // GET - Funcion para obtener una reservacion por código
        
        [HttpGet("{codigo}")]
        public ActionResult<Reservacion> Get(string codigo)
        {
            var reservacion = _reservacionService.ObtenerPorCodigo(codigo);

            if (reservacion == null)
            {
                return NotFound("Reservación no encontrada.");
            }

            return Ok(reservacion);
        }

        // POST - Funcion para crear una nueva reservación
        
        [HttpPost]
        public IActionResult Post([FromBody] Reservacion reservacion)
        {
            bool agregado = _reservacionService.Agregar(reservacion);

            if (!agregado)
            {
                return BadRequest("No fue posible registrar la reservación.");
            }

            return Ok(reservacion);
        }

        
        // PUT - Funcion para actualizar una reservación existente
        
        [HttpPut("{codigo}")]
        public IActionResult Put(string codigo,
                                 [FromBody] Reservacion reservacion)
        {
            bool actualizado =
                _reservacionService.Actualizar(codigo, reservacion);

            if (!actualizado)
            {
                return BadRequest("No fue posible actualizar la reservación.");
            }

            return Ok("Reservación actualizada correctamente.");
        }

        
        // DELETE - Funcion para eliminar una reservación por código
        
        [HttpDelete("{codigo}")]
        public IActionResult Delete(string codigo)
        {
            bool eliminado =
                _reservacionService.Eliminar(codigo);

            if (!eliminado)
            {
                return NotFound("Reservación no encontrada.");
            }

            return Ok("Reservación eliminada correctamente.");
        }

        
        // BUSCAR - Funcion para buscar una reservación por código
       
        [HttpGet("buscar/{codigo}")]
        public IActionResult Buscar(string codigo)
        {
            var reservacion =
                _reservacionService.ObtenerPorCodigo(codigo);

            if (reservacion == null)
            {
                return NotFound("Reservación no encontrada.");
            }

            return Ok(reservacion);
        }
    }
}
