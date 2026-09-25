using System.ComponentModel.DataAnnotations;
using Proyecto3.AccesoDatos.Validation;

namespace Proyecto3.AccesoDatos.Models;

// Entidades persistentes. Las validaciones de entrada permanecen en MVC y API.
public class Cliente : IValidatableObject
{
    public string Identificacion { get; set; } = string.Empty;
    public string TipoIdentificacion { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string PrimerApellido { get; set; } = string.Empty;
    public string SegundoApellido { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!ReglasValidacion.IdentificacionValida(TipoIdentificacion, Identificacion, true)) yield return new ValidationResult("La identificación debe ser cédula (1-1111-1111), DIMEX de 12 dígitos o pasaporte alfanumérico de 1 a 50 caracteres.", [nameof(Identificacion)]);
        if (!ReglasValidacion.TextoAlfanumerico(Nombre, 3, 50)) yield return new ValidationResult("El nombre debe ser alfanumérico y tener entre 3 y 50 caracteres.", [nameof(Nombre)]);
        if (!ReglasValidacion.TextoAlfanumerico(PrimerApellido, 3, 75)) yield return new ValidationResult("El primer apellido debe ser alfanumérico y tener entre 3 y 75 caracteres.", [nameof(PrimerApellido)]);
        if (!ReglasValidacion.TextoAlfanumerico(SegundoApellido, 3, 75)) yield return new ValidationResult("El segundo apellido debe ser alfanumérico y tener entre 3 y 75 caracteres.", [nameof(SegundoApellido)]);
        if (!ReglasValidacion.FechaNacimientoValida(FechaNacimiento)) yield return new ValidationResult("La fecha de nacimiento debe estar entre el 01/01/1800 y ayer.", [nameof(FechaNacimiento)]);
    }
}

public class Empleado : IValidatableObject
{
    public string Identificacion { get; set; } = string.Empty;
    public string TipoIdentificacion { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
    public decimal Salario { get; set; }
    public DateTime FechaIngreso { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public string Ubicacion { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!ReglasValidacion.IdentificacionValida(TipoIdentificacion, Identificacion, false)) yield return new ValidationResult("La identificación debe ser cédula (1-1111-1111) o DIMEX de 12 dígitos.", [nameof(Identificacion)]);
        if (!ReglasValidacion.TextoAlfanumerico(Nombre, 3, 50)) yield return new ValidationResult("El nombre debe ser alfanumérico y tener entre 3 y 50 caracteres.", [nameof(Nombre)]);
        if (!ReglasValidacion.TextoAlfanumerico(Apellidos, 3, 150)) yield return new ValidationResult("Los apellidos deben ser alfanuméricos y tener entre 3 y 150 caracteres.", [nameof(Apellidos)]);
        if (!ReglasValidacion.FechaNacimientoValida(FechaNacimiento)) yield return new ValidationResult("La fecha de nacimiento debe estar entre el 01/01/1800 y ayer.", [nameof(FechaNacimiento)]);
        if (FechaIngreso == default) yield return new ValidationResult("La fecha de ingreso es obligatoria.", [nameof(FechaIngreso)]);
        
        // La empresa puede registrar contrataciones futuras, solo se compara con el nacimiento.

        else if (FechaIngreso.Date <= FechaNacimiento.Date) yield return new ValidationResult("La fecha de ingreso debe ser posterior a la fecha de nacimiento.", [nameof(FechaIngreso)]);
        if (!ReglasValidacion.CategoriaValida(Categoria)) yield return new ValidationResult("Seleccione una categoría válida.", [nameof(Categoria)]);
        if (string.IsNullOrWhiteSpace(Ubicacion)) yield return new ValidationResult("Seleccione provincia, cantón y distrito.", [nameof(Ubicacion)]);
        if (!ReglasValidacion.TextoAlfanumerico(Direccion, 1, 150)) yield return new ValidationResult("La dirección debe ser alfanumérica y tener entre 1 y 150 caracteres.", [nameof(Direccion)]);
    }
}

public class Habitacion : IValidatableObject
{
    public int Numero { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public decimal Tarifa { get; set; }
    public bool TV { get; set; }
    public string Mantenimiento { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Numero is < 1 or > 500) yield return new ValidationResult("El número de habitación debe estar entre 1 y 500.", [nameof(Numero)]);
        if (!ReglasValidacion.TiposHabitacion.Contains(Tipo)) yield return new ValidationResult("Seleccione un tipo de habitación válido.", [nameof(Tipo)]);
        if (Tarifa is < 50 or > 800) yield return new ValidationResult("La tarifa debe estar entre 50 y 800 dólares.", [nameof(Tarifa)]);
        if (!ReglasValidacion.TextoAlfanumericoOpcional(Mantenimiento, 500)) yield return new ValidationResult("Los pendientes de mantenimiento deben ser texto alfanumérico de hasta 500 caracteres.", [nameof(Mantenimiento)]);
    }
}

public class Reservacion : IValidatableObject
{
    public string CodigoReservacion { get; set; } = string.Empty;
    public string ClienteId { get; set; } = string.Empty;
    public int? HabitacionNumero { get; set; }
    public DateTime FechaReservacion { get; set; }
    public DateTime? FechaIngreso { get; set; }
    public DateTime? FechaSalida { get; set; }
    public decimal TarifaReservacion { get; set; }
    public string? SolicitudesEspeciales { get; set; }
    public decimal PorcentajeDescuento { get; set; }
    public decimal MontoTotal { get; set; }
    public int? CantidadPersonas { get; set; }
    public string Estado { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(ClienteId)) yield return new ValidationResult("Debe seleccionar un cliente.", [nameof(ClienteId)]);
        if (HabitacionNumero is null) yield return new ValidationResult("Debe seleccionar una habitación.", [nameof(HabitacionNumero)]);
        if (FechaIngreso is null || FechaSalida is null || FechaSalida <= FechaIngreso) yield return new ValidationResult("La fecha de salida debe ser posterior a la fecha de ingreso.", [nameof(FechaSalida)]);
        if (CantidadPersonas is < 1 or > 10) yield return new ValidationResult("La cantidad de personas debe estar entre 1 y 10.", [nameof(CantidadPersonas)]);
        if (PorcentajeDescuento is < 0 or > 100) yield return new ValidationResult("El descuento debe estar entre 0 y 100.", [nameof(PorcentajeDescuento)]);
        if (!ReglasValidacion.EstadosReservacion.Contains(Estado)) yield return new ValidationResult("Seleccione un estado válido: Reservada, En Proceso, Cancelada o Finalizada.", [nameof(Estado)]);
    }
}
