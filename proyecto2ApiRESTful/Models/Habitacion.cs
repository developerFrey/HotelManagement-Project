using System.ComponentModel.DataAnnotations;

namespace Proyecto2ApiRESTful.Models
{
    public class Habitacion
    {
        [Required]
        [Range(1, 500, ErrorMessage = "El número debe estar entre 1 y 500")]
        public int Numero { get; set; }

        [Required]
        public string Tipo { get; set; }

        [Required]
        [Range(50, 800, ErrorMessage = "La tarifa debe estar entre 50 y 800")]
        public decimal Tarifa { get; set; }

        [Required]
        public bool TV { get; set; }

        [StringLength(500, ErrorMessage = "Máximo 500 caracteres")]
        public string Mantenimiento { get; set; }
    }
}