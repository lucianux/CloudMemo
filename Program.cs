using Azure.Data.AppConfiguration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Conectamos la App con el servicio de Azure para LEER
string connectionString = builder.Configuration.GetConnectionString("AzureAppConfig");

builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(connectionString)
	   // Esto hace que si cambias el valor en el portal, la app se entere
	   .ConfigureRefresh(refresh => refresh.Register("MensajePersistente", refreshAll: true));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 2. Endpoint para LEER el string
app.MapGet("/leer", (IConfiguration config) => 
{
    var valor = config["MensajePersistente"] ?? "No encontrado";
    return Results.Ok(new { mensaje = valor });
});

// 3. Endpoint para GUARDAR (Actualizar) el string
app.MapPost("/guardar", async (string nuevoTexto) => 
{
    var client = new ConfigurationClient(connectionString);
    
    // Actualizamos el valor en Azure
    await client.SetConfigurationSettingAsync("MensajePersistente", nuevoTexto);
    
    return Results.Ok("Valor actualizado en Azure App Configuration.");
});

app.Run();
