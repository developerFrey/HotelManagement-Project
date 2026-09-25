using System.Data.Common;
using Proyecto3.AccesoDatos.Models;
using Proyecto3.AccesoDatos.Repositories;

namespace Proyecto2ApiRESTful.Services;

public sealed class HabitacionService
{
    private readonly HotelRepository _repositorio;
    public HabitacionService(HotelRepository repositorio) => _repositorio = repositorio;
    public List<Habitacion> ObtenerTodas() => _repositorio.ObtenerHabitaciones();
    public Habitacion? ObtenerPorNumero(int numero) => _repositorio.ObtenerHabitacion(numero);
    public bool Agregar(Habitacion habitacion) => EjecutarSeguro(() => _repositorio.CrearHabitacion(habitacion));
    public bool Actualizar(int numero, Habitacion habitacion) => EjecutarSeguro(() => _repositorio.ActualizarHabitacion(numero, habitacion));
    public bool Eliminar(int numero) => EjecutarSeguro(() => _repositorio.EliminarHabitacion(numero));
    private static bool EjecutarSeguro(Func<bool> operacion) { try { return operacion(); } catch (DbException) { return false; } }
}
