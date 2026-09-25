using Microsoft.Data.SqlClient;

namespace Proyecto3.AccesoDatos;

/// Funcion que centraliza la creación de las conexiones SQL, ninguna capa superior conoce la cadena de conexión.
public sealed class SqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = string.IsNullOrWhiteSpace(connectionString)
            ? throw new ArgumentException("La cadena de conexión es obligatoria.", nameof(connectionString))
            : connectionString;
    }

    public SqlConnection Create() => new(_connectionString);
}
