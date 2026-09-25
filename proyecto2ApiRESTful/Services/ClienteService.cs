using System.Data.Common;
using Proyecto3.AccesoDatos.Models;
using Proyecto3.AccesoDatos.Repositories;

namespace Proyecto2ApiRESTful.Services;

/// Lógica de negocio de los clientes, la persistencia es responsabilidad del repositorio.
public sealed class ClienteService
{
    private readonly HotelRepository _repositorio;
    public ClienteService(HotelRepository repositorio) => _repositorio = repositorio;
    public List<Cliente> ObtenerTodos() => _repositorio.ObtenerClientes();
    public Cliente? ObtenerPorId(string id) => _repositorio.ObtenerCliente(id);
    public bool Agregar(Cliente cliente) => EjecutarSeguro(() => _repositorio.CrearCliente(cliente));
    public bool Actualizar(string id, Cliente cliente) => EjecutarSeguro(() => _repositorio.ActualizarCliente(id, cliente));
    public bool Eliminar(string id) => EjecutarSeguro(() => _repositorio.EliminarCliente(id));
    private static bool EjecutarSeguro(Func<bool> operacion) { try { return operacion(); } catch (DbException) { return false; } }
}
