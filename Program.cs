using Azure.Data.AppConfiguration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// We connect the app to the Azure service to READ
string connectionString = builder.Configuration.GetConnectionString("AzureAppConfig");

builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(connectionString)
       // This means that if you change the value on the Portal, the app will know.
       .ConfigureRefresh(refresh => refresh.Register("MensajePersistente", refreshAll: true)
          //.SetCacheExpiration(TimeSpan.FromSeconds(5));
          .SetRefreshInterval(TimeSpan.FromSeconds(5)));
});

builder.Services.AddAzureAppConfiguration();

var app = builder.Build();

app.UseAzureAppConfiguration();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Root endpoint
    app.MapGet("/", () => "Cloud Memo API up");

// Endpoint to READ the string
app.MapGet("/leer", (IConfiguration config) => 
{
    var valor = config["MensajePersistente"] ?? "No encontrado";
    return Results.Ok(new { mensaje = valor });
});

// Endpoint to SAVE (Update) the string
app.MapPost("/guardar", async (string nuevoTexto) => 
{
    var client = new ConfigurationClient(connectionString);
    
    // We updated the value in Azure.
    await client.SetConfigurationSettingAsync("MensajePersistente", nuevoTexto);
    
    return Results.Ok("Valor actualizado en Azure App Configuration.");
});

app.Run();
