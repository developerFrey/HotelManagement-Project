using System.ComponentModel.DataAnnotations;

namespace Proyecto2ApiRESTful.Models
{
    public class Cliente
    {
        [Required(ErrorMessage = "La identificación es obligatoria.")]
        public string Identificacion { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Debe tener entre 3 y 50 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Solo letras y números.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [StringLength(75, MinimumLength = 3)]
        [RegularExpression(@"^[\p{L}\p{M}0-9\s'\-]+$")]
        public string PrimerApellido { get; set; }

        [Required(ErrorMessage = "El segundo apellido es obligatorio.")]
        [StringLength(75, MinimumLength = 3)]
        [RegularExpression(@"^[\p{L}\p{M}0-9\s'\-]+$")]
        public string SegundoApellido { get; set; }

        [Required(ErrorMessage = "La fecha  de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "Seleccione el tipo de identificación.")]
        public string TipoIdentificacion { get; set; }
    }
}