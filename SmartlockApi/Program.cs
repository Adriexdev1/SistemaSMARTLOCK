using SmartlockApi.Datos;
using SmartlockApi.Servicios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Repositorio de acceso a SMARTLOCK_BD
builder.Services.AddScoped<AccesoRepositorio>();
builder.Services.AddScoped<AuthRepositorio>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<UsuariosRepositorio>();
builder.Services.AddScoped<IUsuariosService, UsuariosService>();
builder.Services.AddScoped<IncidentesRepositorio>();
builder.Services.AddScoped<IIncidentesService, IncidentesService>();
builder.Services.AddScoped<EventosRepositorio>();
builder.Services.AddScoped<IEventosService, EventosService>();
builder.Services.AddScoped<HistorialRepositorio>();
builder.Services.AddScoped<IHistorialService, HistorialService>();
//el portal web, el Arduino/ESP8266 y el chatbot de WhatsApp
builder.Services.AddCors(options =>
{
    options.AddPolicy("SmartlockClientes", policy =>
    {
        policy.WithOrigins(
                builder.Configuration.GetSection("OrigenesPermitidos").Get<string[]>() ?? Array.Empty<string>())
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("SmartlockClientes");
app.MapControllers();

app.Run();
