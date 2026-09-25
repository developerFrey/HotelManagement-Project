
namespace Proyecto2ApiRESTful.Models
{
    public static class Datos
    {
        //Listas estáticas que guardan  en memoria los datos para la aplicación.
        public static List<Cliente> Clientes { get; } = new();

        public static List<Empleado> Empleados { get; } = new();

        public static List<Habitacion> Habitaciones { get; } = new();

        public static List<Reservacion> Reservaciones { get; } = new();

        //Datos por defecto que se cargan al iniciar la aplicación
        public static void Inicializar()
        {
            if (Clientes.Count > 0)
                return;

            // Cliente por defecto
            Clientes.Add(new Cliente
            {
                TipoIdentificacion = "Cedula",
                Identificacion = "1-1305-2611",
                Nombre = "Juan",
                PrimerApellido = "Pérez",
                SegundoApellido = "Rojas",
                FechaNacimiento = new DateTime(1995, 5, 10)
            });

            // Empleado por defecto
            Empleados.Add(new Empleado
            {
                TipoIdentificacion = "Cedula",
                Identificacion = "2-2032-0309",
                Nombre = "María",
                Apellidos = "González",
                FechaNacimiento = new DateTime(1990, 3, 15),
                Salario = 750000,
                FechaIngreso = DateTime.Today,
                Categoria = "Recepcionista",
                Ubicacion = "San José - Central - Catedral",
                Direccion = "100 metros norte del parque"
            });

            // Habitación por defecto
            Habitaciones.Add(new Habitacion
            {
                Numero = 101,
                Tipo = "Start Junior",
                Tarifa = 120,
                TV = true,
                Mantenimiento = "Falta limpiar Piso"
            });

            // La reservación por defecto la agregaremos después,
            Reservaciones.Add(new Reservacion
            {
                CodigoReservacion = "RES001",
                ClienteId = "1-1305-2611",
                HabitacionNumero = 101,
                FechaReservacion = DateTime.Today,
                FechaIngreso = DateTime.Today,
                FechaSalida = DateTime.Today.AddDays(3),
                CantidadPersonas = 2,
                SolicitudesEspeciales = "Cama extra",
                Estado = "Reservada"
            });
            // ya que depende del cliente y la habitación.
        }
    }
}
