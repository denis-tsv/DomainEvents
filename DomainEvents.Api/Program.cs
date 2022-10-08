using DomainEvents.Infrastructure.Interfaces;
using DomainEvents.Infrastructure.MsSql;
using DomainEvents.UseCases;
using DomainEvents.UseCases.AccountGroups;
using DomainEvents.UseCases.Accounts.Commands.DeleteAccount;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(DeleteAccountCommand));
builder.Services.AddDbContext<IDbContext, AppDbContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DomainEvents")));
builder.Services.AddScoped<AccountGroupService>();
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionalPipelineBehavior<,>));

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