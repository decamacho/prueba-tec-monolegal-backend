using Application.Interfaces;
using Application.UseCases;
using Domain.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.Persistence;
using Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// configurar opciones de MongoDB (Mapea el JSON a la clase C#)
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// registrar el contexto de MongoDB como Singleton
// Singleton: porque la conexión a Mongo debe mantenerse viva y reutilizarse
builder.Services.AddSingleton<MongoDbContext>();

var sendGridApiKey = builder.Configuration["SendGrid:ApiKey"];
var fromEmail = builder.Configuration["SendGrid:FromEmail"];

bool configLocalSendGrid = !string.IsNullOrWhiteSpace(sendGridApiKey) && sendGridApiKey != "SG_API_KEY"
                           x&& !string.IsNullOrWhiteSpace(fromEmail) && fromEmail != "demoenvioemail@gmail.com";

// inyeccion de la dependencia correcta basada en el entorno
if (configLocalSendGrid)
{
    // MODO PRODUCCION / PRUEBA REAL: Se usa SendGrid
    builder.Services.AddScoped<IEmailService, SendGridEmailService>();
}
else
{
    // MODO EVALUACION / DESARROLLO LOCAL: Se usa Consola
    builder.Services.AddScoped<IEmailService, ConsoleEmailService>();
}

// registrar los repositorios (Scoped significa que se crea una instancia por cada petición HTTP)
builder.Services.AddScoped<IInvoiceRepository, MongoDbInvoiceRepository>();
builder.Services.AddScoped<IProcessInvoiceReminders, ProcessInvoiceRemindersUseCase>();

// configuracion estandar de la API
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() // O usa .WithOrigins("http://localhost:5173")
              .AllowAnyMethod() // Permite GET, POST, OPTIONS, etc.
              .AllowAnyHeader();
    });
});

// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(); 
// Swagger sirve para probar sin frontend

var app = builder.Build();

// middleware pipeline
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();