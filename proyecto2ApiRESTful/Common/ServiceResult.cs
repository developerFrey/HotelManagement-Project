namespace proyecto2ApiRESTful.Common
{
    namespace Proyecto2ApiRESTful.Common
    {
        public class ServiceResult<T>
        {

            // Indica si la operación fue exitosa (true) o fallida (false).
            public bool Success { get; set; }

            // Mensaje informativo o de error relacionado con la operación.
            // Inicializado por defecto a cadena vacía para evitar nulls.
            public string Message { get; set; } = string.Empty;

            // Datos devueltos por la operación, que pueden ser null si no aplica o en caso de error.
            public T? Data { get; set; }
        }
    }
}
