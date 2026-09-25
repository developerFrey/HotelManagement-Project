
using System.ComponentModel.DataAnnotations;

using Proyecto1_FelipeBrenes.Validation;

namespace Proyecto1_FelipeBrenes.Models
{

    public class Empleado : IValidatableObject
    {

        [Required(ErrorMessage = "Debe selccionar un tipo de identificación.")]
        public string TipoIdentificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La identificación es obligatoria.")]
        public string Identificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, MinimumLength = 3)]
        [RegularExpression(@"^[\p{L}\p{M}0-9\s'\-]+$")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        [StringLength(75, MinimumLength = 3)]
        [RegularExpression(@"^[\p{L}\p{M}0-9\s'\-]+$")]
        public string Apellidos { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimientos es obligatoria")]
        [DataType(DataType.Date)]
        // Nullable para permitir que [Required] informe una fecha omitida claramente.
        public DateTime? FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El salario es obligatorio.")]
        [Range(0, 5000000)]
        public decimal Salario { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una fecha de ingreso.")]
        [DataType(DataType.Date)]
        public DateTime? FechaIngreso { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una categoria.")]
        public string Categoria { get; set; } = string.Empty;

        
        [Required(ErrorMessage = "Debe seleccionar una ubicación")]
        public string Ubicacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe indicar una dirección")]
        [StringLength(150, MinimumLength = 1)]
        public string Direccion { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ReglasValidacion.IdentificacionValida(TipoIdentificacion, Identificacion, false)) yield return new ValidationResult("La identificación debe ser cédula (1-1111-1111) o DIMEX de 12 dígitos.", [nameof(Identificacion)]);
            if (!ReglasValidacion.TextoAlfanumerico(Nombre, 3, 50)) yield return new ValidationResult("El nombre debe ser alfanumérico y tener entre 3 y 50 caracteres.", [nameof(Nombre)]);
            if (!ReglasValidacion.TextoAlfanumerico(Apellidos, 3, 150)) yield return new ValidationResult("Los apellidos deben ser alfanuméricos y tener entre 3 y 150 caracteres.", [nameof(Apellidos)]);
            if (FechaNacimiento is null || !ReglasValidacion.FechaNacimientoValida(FechaNacimiento.Value)) yield return new ValidationResult("La fecha de nacimiento debe estar entre el 01/01/1800 y ayer.", [nameof(FechaNacimiento)]);
            if (FechaIngreso is null) yield return new ValidationResult("La fecha de ingreso es obligatoria.", [nameof(FechaIngreso)]);
            // Se permiten contrataciones futuras; solo es inválida una fecha igual o anterior al nacimiento.
            else if (FechaNacimiento is not null && FechaIngreso.Value.Date <= FechaNacimiento.Value.Date) yield return new ValidationResult("La fecha de ingreso debe ser posterior a la fecha de nacimiento.", [nameof(FechaIngreso)]);
            if (!ReglasValidacion.CategoriaValida(Categoria)) yield return new ValidationResult("Seleccione una categoría válida.", [nameof(Categoria)]);
            if (string.IsNullOrWhiteSpace(Ubicacion)) yield return new ValidationResult("Seleccione provincia, cantón y distrito.", [nameof(Ubicacion)]);
            if (!ReglasValidacion.TextoAlfanumerico(Direccion, 1, 150)) yield return new ValidationResult("La dirección debe ser alfanumérica y tener entre 1 y 150 caracteres.", [nameof(Direccion)]);
        }
    }
} 
