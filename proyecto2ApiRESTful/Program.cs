
using Proyecto2ApiRESTful.Services;
using Proyecto3.AccesoDatos;
using Proyecto3.AccesoDatos.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("HotelDb")
    ?? throw new InvalidOperationException("No se configuró la cadena de conexión HotelDb.");
builder.Services.AddSingleton(new SqlConnectionFactory(connectionString));
builder.Services.AddSingleton<HotelRepository>();

builder.Services.AddSingleton<ClienteService>();

builder.Services.AddSingleton<HabitacionService>();

builder.Services.AddSingleton<EmpleadoService>();

builder.Services.AddSingleton<ReservacionService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
