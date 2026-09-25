using System.ComponentModel.DataAnnotations;

using Proyecto1_FelipeBrenes.Validation;

namespace Proyecto1_FelipeBrenes.Models
{
    public class Cliente : IValidatableObject
    {
        [Required(ErrorMessage = "La identificación es obligatoria.")]
        public string Identificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Debe tener entre 3 y 50 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Solo letras y números.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [StringLength(75, MinimumLength = 3)]
        [RegularExpression(@"^[\p{L}\p{M}0-9\s'\-]+$")]
        public string PrimerApellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El segundo apellido es obligatorio.")]
        [StringLength(75, MinimumLength = 3)]
        [RegularExpression(@"^[\p{L}\p{M}0-9\s'\-]+$")]
        public string SegundoApellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        // Nullable para que [Required] muestre su mensaje en lugar del mensaje técnico del binder.
        public DateTime? FechaNacimiento { get; set; }

        [Required(ErrorMessage = "Seleccione el tipo de identificación.")]
        public string TipoIdentificacion { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ReglasValidacion.IdentificacionValida(TipoIdentificacion, Identificacion, true)) yield return new ValidationResult("La identificación debe ser cédula (1-1111-1111), DIMEX de 12 dígitos o pasaporte alfanumérico de 1 a 50 caracteres.", [nameof(Identificacion)]);
            if (!ReglasValidacion.TextoAlfanumerico(Nombre, 3, 50)) yield return new ValidationResult("El nombre debe ser alfanumérico y tener entre 3 y 50 caracteres.", [nameof(Nombre)]);
            if (!ReglasValidacion.TextoAlfanumerico(PrimerApellido, 3, 75)) yield return new ValidationResult("El primer apellido debe ser alfanumérico y tener entre 3 y 75 caracteres.", [nameof(PrimerApellido)]);
            if (!ReglasValidacion.TextoAlfanumerico(SegundoApellido, 3, 75)) yield return new ValidationResult("El segundo apellido debe ser alfanumérico y tener entre 3 y 75 caracteres.", [nameof(SegundoApellido)]);
            if (FechaNacimiento is null || !ReglasValidacion.FechaNacimientoValida(FechaNacimiento.Value)) yield return new ValidationResult("La fecha de nacimiento debe estar entre el 01/01/1800 y ayer.", [nameof(FechaNacimiento)]);
        }
    }
}
