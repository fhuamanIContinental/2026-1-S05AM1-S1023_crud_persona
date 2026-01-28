using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// =====================
// Cadena de conexión
// =====================
var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

// Registrar MySqlConnection
builder.Services.AddScoped<MySqlConnection>(_ =>
    new MySqlConnection(connectionString)
);

// =====================
// Configuración de CORS
// =====================
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// =====================
// Servicios
// =====================
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// =====================
// Pipeline HTTP
// =====================
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// ⚠️ CORS debe ir antes de Authorization y MapControllers
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
