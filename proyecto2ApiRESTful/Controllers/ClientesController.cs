using Microsoft.AspNetCore.Mvc;
using Proyecto3.AccesoDatos.Models;
using Proyecto2ApiRESTful.Services;

namespace Proyecto2ApiRESTful.Controllers
{
    // Controlador de la API que expone los endpoints para gestionar clientes en el (CRUD).
    [ApiController] // Indica que este controlador responde a solicitudes de API (validación automática, binding, etc.).
    [Route("api/[controller]")] // Ruta base: /api/clientes
    public class ClientesController : ControllerBase
    {
        // Servicio que encapsula la lógica de negocio y acceso a datos para clientes.
        private readonly ClienteService _clienteService;

        // Constructor: inyección de dependencias del servicio de clientes.
        public ClientesController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // GET: api/clientes
        // Devuelve todos los clientes

        [HttpGet]
        // Funcion que devuelve una lista completa de clientes en formato 200 OK.
        public ActionResult<List<Cliente>> Get()
        {
            return Ok(_clienteService.ObtenerTodos());
        }

        // GET: api/clientes/1-1111-1111
        // Funcion que devuelve solamente un cliente
        [HttpGet("{id}")]
        // Obtiene un cliente por su identificación. Retorna 404 si no existe.
        public ActionResult<Cliente> Get(string id)
        {
            var cliente = _clienteService.ObtenerPorId(id);

            if (cliente == null)
                return NotFound("Cliente no encontrado.");

            return Ok(cliente);
        }

        // POST: api/clientes
        // Recibe un JSON
        [HttpPost]
        // Funcion que crea un nuevo cliente a partir del cuerpo de la petición.
        // Valida duplicados mediante el servicio y responde 400 si ya existe.
        public IActionResult Post([FromBody] Cliente cliente)
        {
            // Distingue un duplicado real de cualquier otra falla de persistencia.
            if (_clienteService.ObtenerPorId(cliente.Identificacion) is not null)
                return Conflict("Ya existe un cliente con esa identificación.");

            bool agregado = _clienteService.Agregar(cliente);

            if (!agregado)
            {
                return BadRequest("No fue posible crear el cliente. Revise los datos ingresados.");
            }

            return Ok(cliente);
        }

        // PUT: api/clientes/1-1111-1111
        // Funcion que actualiza un cliente existente
        [HttpPut("{id}")]
        // Actualiza los datos de un cliente identificado por `id`. Devuelve 404 si no existe.
        public IActionResult Put(string id, [FromBody] Cliente cliente)
        {
            bool actualizado = _clienteService.Actualizar(id, cliente);

            if (!actualizado)
                return NotFound("Cliente no encontrado.");

            return Ok("Cliente actualizado correctamente.");
        }

        // DELETE: api/clientes/1-1111-1111
        // Funcion que elimina un cliente existente
        [HttpDelete("{id}")]
        // Elimina un cliente si no tiene reservaciones. Responde 404 si no existe o 400 si no puede eliminarse.
        public ActionResult Delete(string id)
        {
            var cliente = _clienteService.ObtenerPorId(id);

            if (cliente == null)
                return NotFound("Cliente no encontrado.");

            bool eliminado = _clienteService.Eliminar(id);

            if (!eliminado)
                return BadRequest("No se puede eliminar porque el cliente posee reservaciones.");

            return Ok("Cliente eliminado correctamente.");
        }

        // GET: api/clientes/buscar/1-1111-1111
        [HttpGet("buscar/{identificacion}")]
        // Endpoint alternativo para buscar un cliente por identificación (misma lógica que Get por id).
        public ActionResult<Cliente> Buscar(string identificacion)
        {
            var cliente = _clienteService.ObtenerPorId(identificacion);

            if (cliente == null)
                return NotFound("Cliente no encontrado.");

            return Ok(cliente);
        }
    }
}
