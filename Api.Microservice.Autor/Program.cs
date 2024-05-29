//Marshall

using Api.Microservice.Autor.Aplicacion;
using Api.Microservice.Autor.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Configuration;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//agregando los builder
builder.Services.AddDbContext<ContextoAutor>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

//Agregamos media TR como servicio
builder.Services.AddMediatR(typeof(Nuevo.Manejador).Assembly);
builder.Services.AddAutoMapper(typeof(Consulta.Manejador));
var app = builder.Build();

// Configure the HTTP request pipeline. //
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
