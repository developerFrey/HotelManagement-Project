
using System.ComponentModel.DataAnnotations;

namespace Proyecto2ApiRESTful.Models
{

    public class Empleado
    {

        [Required(ErrorMessage = "Debe selccionar un tipo de identificación.")]
        public string TipoIdentificacion { get; set; }

        [Required(ErrorMessage = "La identificación es obligatoria.")]
        public string Identificacion { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, MinimumLength = 3)]
        [RegularExpression(@"^[\p{L}\p{M}0-9\s'\-]+$")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        [StringLength(75, MinimumLength = 3)]
        [RegularExpression(@"^[\p{L}\p{M}0-9\s'\-]+$")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "La fecha de nacimientos es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El salario es obligatorio.")]
        [Range(0, 5000000)]
        public decimal Salario { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una fecha de ingreso.")]
        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una categoria.")]
        public string Categoria { get; set; }


        [Required(ErrorMessage = "Debe seleccionar una ubicación")]
        public string Ubicacion { get; set; }

        [Required(ErrorMessage = "Debe indicar una dirección")]
        [StringLength(150, MinimumLength = 1)]
        public string Direccion { get; set; }
    }
} 
