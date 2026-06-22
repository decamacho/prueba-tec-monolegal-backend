using Application.Interfaces;
using Application.UseCases;
using Domain.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// configurar opciones de MongoDB (Mapea el JSON a la clase C#)
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// registrar el contexto de MongoDB como Singleton
// Singleton: porque la conexión a Mongo debe mantenerse viva y reutilizarse
builder.Services.AddSingleton<MongoDbContext>();

// registrar los repositorios (Scoped significa que se crea una instancia por cada petición HTTP)
builder.Services.AddScoped<IInvoiceRepository, MongoDbInvoiceRepository>();
builder.Services.AddScoped<IProcessInvoiceReminders, ProcessInvoiceRemindersUseCase>();
builder.Services.AddScoped<IEmailService, Infrastructure.Services.LoggerEmailService>();

// configuracion estandar de la API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Swagger sirve para probar sin frontend

var app = builder.Build();

// 5. Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();