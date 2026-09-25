using System.Text.RegularExpressions;

namespace Proyecto1_FelipeBrenes.Validation;

internal static class ReglasValidacion
{
    public static readonly string[] Categorias = ["Mesero", "Salonero", "Lavaplatos", "Recepcionista", "Administrador", "Mantenimiento", "Cocinero", "Chef", "Limpieza", "Cocina", "Seguridad", "Atención al Cliente", "Guía Turístico", "Encargado de Reservaciones"];
    public static readonly string[] TiposHabitacion = ["Start Junior", "Start Vista al Mar", "Master Start"];
    public static readonly string[] EstadosReservacion = ["Reservada", "En Proceso", "Cancelada", "Finalizada"];

    // Compara sin distinguir mayúsculas para aceptar categorías guardadas por versiones anteriores.
    /// Comprueba una categoría sin distinguir mayúsculas, para que los espacios no causen una falla por capitalización.
    public static bool CategoriaValida(string? categoria) => Categorias.Contains(categoria ?? string.Empty, StringComparer.OrdinalIgnoreCase);
    
    /// Se Valida texto visible del usuario: letras, números y espacios, dentro del intervalo recibido.
    public static bool TextoAlfanumerico(string? texto, int minimo, int maximo) => !string.IsNullOrWhiteSpace(texto) && texto.Length >= minimo && texto.Length <= maximo && Regex.IsMatch(texto, @"^[\p{L}\p{M}0-9\s]+$");
    
    /// Variante para campos opcionales, como pendientes de mantenimiento.
    public static bool TextoAlfanumericoOpcional(string? texto, int maximo) => string.IsNullOrEmpty(texto) || (texto.Length <= maximo && Regex.IsMatch(texto, @"^[\p{L}\p{M}0-9\s]+$"));
    
    /// Se aplica el formato de identificación que corresponde al tipo seleccionado.
    public static bool IdentificacionValida(string? tipo, string? identificacion, bool permitePasaporte) => tipo switch
    {
        "Cedula" => !string.IsNullOrWhiteSpace(identificacion) && Regex.IsMatch(identificacion, @"^\d-\d{4}-\d{4}$"),
        "Dimex" => !string.IsNullOrWhiteSpace(identificacion) && Regex.IsMatch(identificacion, @"^\d{12}$"),
        "Pasaporte" when permitePasaporte => !string.IsNullOrWhiteSpace(identificacion) && Regex.IsMatch(identificacion, @"^[A-Za-z0-9]{1,50}$"),
        _ => false
    };
    
    /// Se evitan fechas de nacimiento imposibles, como por ejemplo, antes de 1800, hoy o futuras.
    public static bool FechaNacimientoValida(DateTime fecha) => fecha.Date >= new DateTime(1800, 1, 1) && fecha.Date < DateTime.Today;
}
