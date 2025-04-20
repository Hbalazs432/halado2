using crypto_s4buby.Entities;
using crypto_s4buby.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<CryptoDbContext>(options => options.UseSqlServer("Server=(localdb)\\Localdb1;Database=CryptoDb_S4BUBY;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"));
//builder.Services.AddDbContext<CryptoDbContext>(options => options.UseSqlServer("Server=(local);Database=CryptoDb_S4BUBY;Trusted_Connection=True;TrustServerCertificate=True"));

builder.Services.AddScoped<UnitOfWork>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
