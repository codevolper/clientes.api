using Clientes.API.Application.CasosUso;
using Clientes.API.Infrastructure.Auth;
using Clientes.API.Infrastructure.Persistencia;
using Clientes.API.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient("AuthApi", client =>
{
    var baseUrl = builder.Configuration["AuthApi:BaseUrl"] ?? "https://localhost:5001";
    client.BaseAddress = new Uri(baseUrl);
});


builder.Services.AddScoped<IValidadorTokenRemoto, ValidadorTokenRemoto>();
builder.Services.AddScoped<Clientes.API.API.Filters.ValidarTokenAttribute>();

// Add controllers
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();

// Serviços de aplicação e infraestrutura
builder.Services.AddSingleton<InMemoryDatabase>();
builder.Services.AddSingleton<Clientes.API.Domain.Interfaces.IClienteRepositorio, ClientesRepositorio>();
builder.Services.AddScoped<CasoDeUsoCadastrarCliente>();
builder.Services.AddScoped<CasoDeUsoListarClientes>();
builder.Services.AddMemoryCache();

// Register cache service implementation before building the app
builder.Services.AddSingleton<Clientes.API.Infrastructure.Cache.ICacheService, Clientes.API.Infrastructure.Cache.MemoriaCacheService>();

builder.Services.AddSwaggerGen(s =>
{

    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    s.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Clientes.API v1"); c.RoutePrefix = string.Empty; });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
