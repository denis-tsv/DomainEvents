using DomainEvents.Infrastructure.Interfaces;
using DomainEvents.Infrastructure.MsSql;
using DomainEvents.UseCases.Categories;
using DomainEvents.UseCases.Products.Commands;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<IDbContext, AppDbContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DomainEvents")));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<DeleteProductCommand>());
builder.Services.AddScoped<CategoryService>();

builder.Services.AddScoped<IExternalService, ExternalService>();

var app = builder.Build();

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

public class ExternalService : IExternalService
{
    public Task LongOperationAsync(CancellationToken token) => Task.CompletedTask;
}