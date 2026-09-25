using System.Data.Common;
using Proyecto3.AccesoDatos.Models;
using Proyecto3.AccesoDatos.Repositories;

namespace Proyecto2ApiRESTful.Services;

public sealed class EmpleadoService
{
    private readonly HotelRepository _repositorio;
    public EmpleadoService(HotelRepository repositorio) => _repositorio = repositorio;
    public List<Empleado> ObtenerTodos() => _repositorio.ObtenerEmpleados();
    public Empleado? ObtenerPorId(string id) => _repositorio.ObtenerEmpleado(id);
    public bool Agregar(Empleado empleado) => EjecutarSeguro(() => _repositorio.CrearEmpleado(empleado));
    public bool Actualizar(string id, Empleado empleado) => EjecutarSeguro(() => _repositorio.ActualizarEmpleado(id, empleado));
    public bool Eliminar(string id) => EjecutarSeguro(() => _repositorio.EliminarEmpleado(id));
    private static bool EjecutarSeguro(Func<bool> operacion) { try { return operacion(); } catch (DbException) { return false; } }
}
