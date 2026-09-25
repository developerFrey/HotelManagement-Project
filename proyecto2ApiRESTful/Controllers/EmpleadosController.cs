using Microsoft.AspNetCore.Mvc;
using Proyecto3.AccesoDatos.Models;
using Proyecto2ApiRESTful.Services;

namespace Proyecto2ApiRESTful.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // Controlador API que expone operaciones CRUD para empleados.
    public class EmpleadosController : ControllerBase
    {
        // Servicio con la lógica y almacenamiento de empleados.
        private readonly EmpleadoService _empleadoService;

        public EmpleadosController(EmpleadoService empleadoService)
        {
            _empleadoService = empleadoService;
        }

        // GET api/empleados - Para obtener todos los empleados
        [HttpGet]
        public ActionResult<List<Empleado>> Get()
        {
            return Ok(_empleadoService.ObtenerTodos());
        }

        // GET api/empleados/{id} - Funcion para obtener un empleado por identificación
        [HttpGet("{id}")]
        public ActionResult<Empleado> Get(string id)
        {
            var empleado = _empleadoService.ObtenerPorId(id);

            if (empleado == null)
                return NotFound("Empleado no encontrado.");

            return Ok(empleado);
        }

        // POST api/empleados - Funcion para crear un empleado y validar duplicados
        [HttpPost]
        public IActionResult Post([FromBody] Empleado empleado)
        {
            bool agregado = _empleadoService.Agregar(empleado);

            if (!agregado)
                return BadRequest("Ya existe un empleado con esa identificación.");

            return Ok(empleado);
        }

        // PUT api/empleados/{id} - Funcion para actualizar un empleado existente
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Empleado empleado)
        {
            bool actualizado = _empleadoService.Actualizar(id, empleado);

            if (!actualizado)
                return NotFound("Empleado no encontrado.");

            return Ok("Empleado actualizado correctamente.");
        }

        // DELETE api/empleados/{id} - Funcion para eliminar un empleado por identificación
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            bool eliminado = _empleadoService.Eliminar(id);

            if (!eliminado)
                return NotFound("Empleado no encontrado.");

            return Ok("Empleado eliminado correctamente.");
        }

        // GET api/empleados/buscar/{identificacion} - Funcion para buscar empleados por identificación
        [HttpGet("buscar/{identificacion}")]
        public IActionResult Buscar(string identificacion)
        {
            var empleado = _empleadoService.ObtenerPorId(identificacion);

            if (empleado == null)
                return NotFound("Empleado no encontrado.");

            return Ok(empleado);
        }
    }
}
