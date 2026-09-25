using System.ComponentModel.DataAnnotations;

using Proyecto1_FelipeBrenes.Validation;

namespace Proyecto1_FelipeBrenes.Models
{
    public class Habitacion : IValidatableObject
    {
        [Required(ErrorMessage = "El número de habitación es obligatorio.")]
        [Range(1, 500, ErrorMessage = "El número debe estar entre 1 y 500")]
        // Nullable: permite que [Required] muestre su mensaje si el usuario deja el campo vacío.
        public int? Numero { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un tipo de habitación.")]
        public string Tipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe digitar una tarifa.")]
        [Range(50, 800, ErrorMessage = "La tarifa debe estar entre 50 y 800")]
        // Nullable: evita el mensaje técnico "The value is invalid" para una tarifa omitida.
        public decimal? Tarifa { get; set; }

        [Required(ErrorMessage = "Debe seleccionar si existe una TV.")]
        public bool TV { get; set; }

        [StringLength(500, ErrorMessage = "Máximo 500 caracteres")]
        public string Mantenimiento { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (!ReglasValidacion.TiposHabitacion.Contains(Tipo)) yield return new ValidationResult("Seleccione un tipo de habitación válido.", [nameof(Tipo)]);
            if (!ReglasValidacion.TextoAlfanumericoOpcional(Mantenimiento, 500)) yield return new ValidationResult("Los pendientes de mantenimiento deben ser texto alfanumérico de hasta 500 caracteres.", [nameof(Mantenimiento)]);
        }
    }
}
