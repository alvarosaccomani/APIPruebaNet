using Microsoft.Extensions.Configuration;
using APIPruebaNet.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuro los servicios
builder.Services.AddScoped<EquipoService>();

// Add services to the container.

builder.Services.AddControllers();

// Agregar política CORS
builder.Services.AddCors(options => 
{
    options.AddPolicy("PermitirTodo", builder => 
    { 
        builder.WithOrigins("https://localhost:7185") // Agregar el origen de tu aplicación Blazor 
                .AllowAnyHeader()
                .AllowAnyMethod();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Usar la política CORS
app.UseCors("PermitirTodo");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
