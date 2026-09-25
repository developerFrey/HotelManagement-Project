using Microsoft.Data.SqlClient;
using Proyecto3.AccesoDatos.Models;

namespace Proyecto3.AccesoDatos.Repositories;

/// Único punto de acceso a SQL Server para las cuatro entidades del hotel.
/// Las consultas usan parámetros para evitar inyección SQL y mejorar el rendimiento u la seguridad.

public sealed class HotelRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public HotelRepository(SqlConnectionFactory connectionFactory) => _connectionFactory = connectionFactory;

    public List<Cliente> ObtenerClientes() => ConsultarClientes("SELECT Identificacion, TipoIdentificacion, Nombre, PrimerApellido, SegundoApellido, FechaNacimiento FROM Clientes ORDER BY Nombre, PrimerApellido");
    public Cliente? ObtenerCliente(string id) => ConsultarClientes("SELECT Identificacion, TipoIdentificacion, Nombre, PrimerApellido, SegundoApellido, FechaNacimiento FROM Clientes WHERE Identificacion = @id", ("@id", id)).SingleOrDefault();
    public bool CrearCliente(Cliente c) => Ejecutar("INSERT INTO Clientes (Identificacion, TipoIdentificacion, Nombre, PrimerApellido, SegundoApellido, FechaNacimiento) VALUES (@id,@tipo,@nombre,@paterno,@materno,@fecha)", ("@id", c.Identificacion), ("@tipo", c.TipoIdentificacion), ("@nombre", c.Nombre), ("@paterno", c.PrimerApellido), ("@materno", c.SegundoApellido), ("@fecha", c.FechaNacimiento.Date)) == 1;
    public bool ActualizarCliente(string id, Cliente c) => Ejecutar("UPDATE Clientes SET TipoIdentificacion=@tipo, Nombre=@nombre, PrimerApellido=@paterno, SegundoApellido=@materno, FechaNacimiento=@fecha WHERE Identificacion=@id", ("@id", id), ("@tipo", c.TipoIdentificacion), ("@nombre", c.Nombre), ("@paterno", c.PrimerApellido), ("@materno", c.SegundoApellido), ("@fecha", c.FechaNacimiento.Date)) == 1;
    public bool EliminarCliente(string id) => Ejecutar("DELETE FROM Clientes WHERE Identificacion=@id", ("@id", id)) == 1;

    public List<Empleado> ObtenerEmpleados() => ConsultarEmpleados("SELECT * FROM Empleados ORDER BY Nombre, Apellidos");
    public Empleado? ObtenerEmpleado(string id) => ConsultarEmpleados("SELECT * FROM Empleados WHERE Identificacion=@id", ("@id", id)).SingleOrDefault();
    public bool CrearEmpleado(Empleado e) => Ejecutar("INSERT INTO Empleados VALUES (@id,@tipo,@nombre,@apellidos,@nacimiento,@salario,@ingreso,@categoria,@ubicacion,@direccion)", ("@id", e.Identificacion), ("@tipo", e.TipoIdentificacion), ("@nombre", e.Nombre), ("@apellidos", e.Apellidos), ("@nacimiento", e.FechaNacimiento.Date), ("@salario", e.Salario), ("@ingreso", e.FechaIngreso.Date), ("@categoria", e.Categoria), ("@ubicacion", e.Ubicacion), ("@direccion", e.Direccion)) == 1;
    public bool ActualizarEmpleado(string id, Empleado e) => Ejecutar("UPDATE Empleados SET TipoIdentificacion=@tipo, Nombre=@nombre, Apellidos=@apellidos, FechaNacimiento=@nacimiento, Salario=@salario, FechaIngreso=@ingreso, Categoria=@categoria, Ubicacion=@ubicacion, Direccion=@direccion WHERE Identificacion=@id", ("@id", id), ("@tipo", e.TipoIdentificacion), ("@nombre", e.Nombre), ("@apellidos", e.Apellidos), ("@nacimiento", e.FechaNacimiento.Date), ("@salario", e.Salario), ("@ingreso", e.FechaIngreso.Date), ("@categoria", e.Categoria), ("@ubicacion", e.Ubicacion), ("@direccion", e.Direccion)) == 1;
    public bool EliminarEmpleado(string id) => Ejecutar("DELETE FROM Empleados WHERE Identificacion=@id", ("@id", id)) == 1;

    public List<Habitacion> ObtenerHabitaciones() => ConsultarHabitaciones("SELECT Numero, Tipo, Tarifa, TieneTV, Mantenimiento FROM Habitaciones ORDER BY Numero");
    public Habitacion? ObtenerHabitacion(int numero) => ConsultarHabitaciones("SELECT Numero, Tipo, Tarifa, TieneTV, Mantenimiento FROM Habitaciones WHERE Numero=@numero", ("@numero", numero)).SingleOrDefault();
    public bool CrearHabitacion(Habitacion h) => Ejecutar("INSERT INTO Habitaciones (Numero,Tipo,Tarifa,TieneTV,Mantenimiento) VALUES (@numero,@tipo,@tarifa,@tv,@mantenimiento)", ("@numero", h.Numero), ("@tipo", h.Tipo), ("@tarifa", h.Tarifa), ("@tv", h.TV), ("@mantenimiento", h.Mantenimiento)) == 1;
    public bool ActualizarHabitacion(int numero, Habitacion h) => Ejecutar("UPDATE Habitaciones SET Tipo=@tipo,Tarifa=@tarifa,TieneTV=@tv,Mantenimiento=@mantenimiento WHERE Numero=@numero", ("@numero", numero), ("@tipo", h.Tipo), ("@tarifa", h.Tarifa), ("@tv", h.TV), ("@mantenimiento", h.Mantenimiento)) == 1;
    public bool EliminarHabitacion(int numero) => Ejecutar("DELETE FROM Habitaciones WHERE Numero=@numero", ("@numero", numero)) == 1;

    public List<Reservacion> ObtenerReservaciones() => ConsultarReservaciones("SELECT * FROM Reservaciones ORDER BY FechaIngreso");
    
    /// Función que obtiene las reservaciones de la semana entrante, de lunes a domingo de la semana calendario posterior a la actual,
    /// sin importar el día en que se ejecute la consulta.
    
    public List<Reservacion> ObtenerReservacionesSemanaEntrante()
    {
        var hoy = DateTime.Today;
        var diasHastaLunes = ((int)DayOfWeek.Monday - (int)hoy.DayOfWeek + 7) % 7;
        if (diasHastaLunes == 0) diasHastaLunes = 7;
        var inicio = hoy.AddDays(diasHastaLunes);
        var finExclusivo = inicio.AddDays(7);
        return ConsultarReservaciones("SELECT * FROM Reservaciones WHERE FechaIngreso >= @inicio AND FechaIngreso < @fin ORDER BY MontoTotal DESC", ("@inicio", inicio), ("@fin", finExclusivo));
    }
    public Reservacion? ObtenerReservacion(string codigo) => ConsultarReservaciones("SELECT * FROM Reservaciones WHERE CodigoReservacion=@codigo", ("@codigo", codigo)).SingleOrDefault();
    public bool ExisteTraslape(int habitacion, DateTime ingreso, DateTime salida, string? excluirCodigo = null) => EjecutarEscalar<int>("SELECT COUNT(1) FROM Reservaciones WHERE HabitacionNumero=@habitacion AND Estado <> N'Cancelada' AND FechaIngreso < @salida AND FechaSalida > @ingreso AND (@excluir IS NULL OR CodigoReservacion <> @excluir)", ("@habitacion", habitacion), ("@ingreso", ingreso.Date), ("@salida", salida.Date), ("@excluir", (object?)excluirCodigo ?? DBNull.Value)) > 0;
    public string SiguienteCodigoReservacion() => $"RES{EjecutarEscalar<int>("SELECT COUNT(1) + 1 FROM Reservaciones") :000}";
    public bool CrearReservacion(Reservacion r) => Ejecutar("INSERT INTO Reservaciones (CodigoReservacion,ClienteId,HabitacionNumero,FechaReservacion,FechaIngreso,FechaSalida,TarifaReservacion,SolicitudesEspeciales,PorcentajeDescuento,MontoTotal,CantidadPersonas,Estado) VALUES (@codigo,@cliente,@habitacion,@fechaReserva,@ingreso,@salida,@tarifa,@solicitudes,@descuento,@monto,@personas,@estado)", ParametrosReservacion(r)) == 1;
    public bool ActualizarReservacion(string codigo, Reservacion r) => Ejecutar("UPDATE Reservaciones SET ClienteId=@cliente,HabitacionNumero=@habitacion,FechaIngreso=@ingreso,FechaSalida=@salida,TarifaReservacion=@tarifa,SolicitudesEspeciales=@solicitudes,PorcentajeDescuento=@descuento,MontoTotal=@monto,CantidadPersonas=@personas,Estado=@estado WHERE CodigoReservacion=@codigo", ParametrosReservacion(r, codigo)) == 1;
    public bool EliminarReservacion(string codigo) => Ejecutar("DELETE FROM Reservaciones WHERE CodigoReservacion=@codigo", ("@codigo", codigo)) == 1;

    private static (string, object?)[] ParametrosReservacion(Reservacion r, string? codigo = null) => new[] { ("@codigo", (object?)(codigo ?? r.CodigoReservacion)), ("@cliente", (object?)r.ClienteId), ("@habitacion", (object?)r.HabitacionNumero), ("@fechaReserva", (object?)r.FechaReservacion.Date), ("@ingreso", (object?)r.FechaIngreso?.Date), ("@salida", (object?)r.FechaSalida?.Date), ("@tarifa", (object?)r.TarifaReservacion), ("@solicitudes", (object?)r.SolicitudesEspeciales ?? DBNull.Value), ("@descuento", (object?)r.PorcentajeDescuento), ("@monto", (object?)r.MontoTotal), ("@personas", (object?)r.CantidadPersonas), ("@estado", (object?)r.Estado) };

    private List<Cliente> ConsultarClientes(string sql, params (string, object?)[] parametros) => Consultar(sql, r => new Cliente { Identificacion = r.GetString(0), TipoIdentificacion = r.GetString(1), Nombre = r.GetString(2), PrimerApellido = r.GetString(3), SegundoApellido = r.GetString(4), FechaNacimiento = r.GetDateTime(5) }, parametros);
    private List<Empleado> ConsultarEmpleados(string sql, params (string, object?)[] parametros) => Consultar(sql, r => new Empleado { Identificacion = r.GetString(r.GetOrdinal("Identificacion")), TipoIdentificacion = r.GetString(r.GetOrdinal("TipoIdentificacion")), Nombre = r.GetString(r.GetOrdinal("Nombre")), Apellidos = r.GetString(r.GetOrdinal("Apellidos")), FechaNacimiento = r.GetDateTime(r.GetOrdinal("FechaNacimiento")), Salario = r.GetDecimal(r.GetOrdinal("Salario")), FechaIngreso = r.GetDateTime(r.GetOrdinal("FechaIngreso")), Categoria = r.GetString(r.GetOrdinal("Categoria")), Ubicacion = r.GetString(r.GetOrdinal("Ubicacion")), Direccion = r.GetString(r.GetOrdinal("Direccion")) }, parametros);
    private List<Habitacion> ConsultarHabitaciones(string sql, params (string, object?)[] parametros) => Consultar(sql, r => new Habitacion { Numero = r.GetInt32(0), Tipo = r.GetString(1), Tarifa = r.GetDecimal(2), TV = r.GetBoolean(3), Mantenimiento = r.GetString(4) }, parametros);
    private List<Reservacion> ConsultarReservaciones(string sql, params (string, object?)[] parametros) => Consultar(sql, r => new Reservacion { CodigoReservacion = r.GetString(r.GetOrdinal("CodigoReservacion")), ClienteId = r.GetString(r.GetOrdinal("ClienteId")), HabitacionNumero = r.GetInt32(r.GetOrdinal("HabitacionNumero")), FechaReservacion = r.GetDateTime(r.GetOrdinal("FechaReservacion")), FechaIngreso = r.GetDateTime(r.GetOrdinal("FechaIngreso")), FechaSalida = r.GetDateTime(r.GetOrdinal("FechaSalida")), TarifaReservacion = r.GetDecimal(r.GetOrdinal("TarifaReservacion")), SolicitudesEspeciales = r.IsDBNull(r.GetOrdinal("SolicitudesEspeciales")) ? null : r.GetString(r.GetOrdinal("SolicitudesEspeciales")), PorcentajeDescuento = r.GetDecimal(r.GetOrdinal("PorcentajeDescuento")), MontoTotal = r.GetDecimal(r.GetOrdinal("MontoTotal")), CantidadPersonas = r.GetInt32(r.GetOrdinal("CantidadPersonas")), Estado = r.GetString(r.GetOrdinal("Estado")) }, parametros);

    private List<T> Consultar<T>(string sql, Func<SqlDataReader, T> mapear, params (string, object?)[] parametros)
    {
        using var conexion = _connectionFactory.Create(); using var comando = CrearComando(conexion, sql, parametros); conexion.Open(); using var lector = comando.ExecuteReader(); var resultados = new List<T>(); while (lector.Read()) resultados.Add(mapear(lector)); return resultados;
    }
    private int Ejecutar(string sql, params (string, object?)[] parametros) { using var conexion = _connectionFactory.Create(); using var comando = CrearComando(conexion, sql, parametros); conexion.Open(); return comando.ExecuteNonQuery(); }
    private T EjecutarEscalar<T>(string sql, params (string, object?)[] parametros) { using var conexion = _connectionFactory.Create(); using var comando = CrearComando(conexion, sql, parametros); conexion.Open(); return (T)Convert.ChangeType(comando.ExecuteScalar()!, typeof(T)); }
    private static SqlCommand CrearComando(SqlConnection conexion, string sql, params (string, object?)[] parametros) { var comando = new SqlCommand(sql, conexion); foreach (var (nombre, valor) in parametros) comando.Parameters.AddWithValue(nombre, valor ?? DBNull.Value); return comando; }
}
