using System.Data.Common;
using Proyecto3.AccesoDatos.Models;
using Proyecto3.AccesoDatos.Repositories;

namespace Proyecto2ApiRESTful.Services;

/// Validaciones y reglas del hotel antes de persistir con la reservación.
public sealed class ReservacionService
{
    private static readonly string[] EstadosValidos = ["Reservada", "En Proceso", "Cancelada", "Finalizada"];
    private readonly HotelRepository _repositorio;
    public ReservacionService(HotelRepository repositorio) => _repositorio = repositorio;
    public List<Reservacion> ObtenerTodas() => _repositorio.ObtenerReservaciones();
    public List<Reservacion> ObtenerSemanaEntrante() => _repositorio.ObtenerReservacionesSemanaEntrante();
    public Reservacion? ObtenerPorCodigo(string codigo) => _repositorio.ObtenerReservacion(codigo);
    public bool Agregar(Reservacion reservacion)
    {
        if (!EsValida(reservacion) || _repositorio.ObtenerCliente(reservacion.ClienteId) is null || _repositorio.ObtenerHabitacion(reservacion.HabitacionNumero!.Value) is null || _repositorio.ExisteTraslape(reservacion.HabitacionNumero.Value, reservacion.FechaIngreso!.Value, reservacion.FechaSalida!.Value)) return false;
        reservacion.CodigoReservacion = _repositorio.SiguienteCodigoReservacion(); reservacion.FechaReservacion = DateTime.Today; CalcularMontos(reservacion);
        return EjecutarSeguro(() => _repositorio.CrearReservacion(reservacion));
    }
    public bool Actualizar(string codigo, Reservacion reservacion)
    {
        var existente = ObtenerPorCodigo(codigo);
        if (existente is null || !EsValida(reservacion) || _repositorio.ObtenerCliente(reservacion.ClienteId) is null || _repositorio.ObtenerHabitacion(reservacion.HabitacionNumero!.Value) is null || _repositorio.ExisteTraslape(reservacion.HabitacionNumero.Value, reservacion.FechaIngreso!.Value, reservacion.FechaSalida!.Value, codigo)) return false;
        reservacion.CodigoReservacion = codigo; reservacion.FechaReservacion = existente.FechaReservacion; CalcularMontos(reservacion);
        return EjecutarSeguro(() => _repositorio.ActualizarReservacion(codigo, reservacion));
    }
    public bool Eliminar(string codigo) => EjecutarSeguro(() => _repositorio.EliminarReservacion(codigo));
    private bool EsValida(Reservacion r) => r.HabitacionNumero.HasValue && r.FechaIngreso.HasValue && r.FechaSalida.HasValue && r.CantidadPersonas is >= 1 and <= 10 && r.FechaSalida > r.FechaIngreso && r.PorcentajeDescuento is >= 0 and <= 100 && EstadosValidos.Contains(r.Estado);
    private void CalcularMontos(Reservacion r) { var noches = (r.FechaSalida!.Value - r.FechaIngreso!.Value).Days; r.TarifaReservacion = _repositorio.ObtenerHabitacion(r.HabitacionNumero!.Value)!.Tarifa * noches; var subtotal = r.TarifaReservacion * (1 - r.PorcentajeDescuento / 100); r.MontoTotal = subtotal * 1.13m; }
    private static bool EjecutarSeguro(Func<bool> operacion) { try { return operacion(); } catch (DbException) { return false; } }
}
