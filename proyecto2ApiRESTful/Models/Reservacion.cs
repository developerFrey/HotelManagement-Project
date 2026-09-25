using System.ComponentModel.DataAnnotations;

namespace Proyecto2ApiRESTful.Models
{
    public class Reservacion
    {
        // Autogenerado por el sistema
        public string CodigoReservacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar un cliente.")]
        public string ClienteId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una habitación.")]
        public int? HabitacionNumero { get; set; }

        // Autogenerada
        public DateTime FechaReservacion { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una fecha de ingreso.")]
        public DateTime? FechaIngreso { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una fecha de salida.")]
        public DateTime? FechaSalida { get; set; }

        // Calculada automáticamente
        public decimal TarifaReservacion { get; set; }

        [StringLength(300,
            ErrorMessage = "Las solicitudes especiales no pueden superar los 300 caracteres.")]
        public string? SolicitudesEspeciales { get; set; }

        [Range(0, 100,
            ErrorMessage = "El descuento debe estar entre 0 y 100%.")]
        public decimal PorcentajeDescuento { get; set; }

        // Calculado automáticamente
        public decimal MontoTotal { get; set; }

        [Required(ErrorMessage = "Debe indicar la cantidad de personas.")]
        [Range(1, 10,
            ErrorMessage = "La cantidad de personas debe estar entre 1 y 10.")]
        public int? CantidadPersonas { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un estado.")]
        public string Estado { get; set; }
    }
}
