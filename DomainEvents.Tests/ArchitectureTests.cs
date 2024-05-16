using System.Reflection;
using DomainEvents.Infrastructure.Interfaces;
using DomainEvents.UseCases.Categories.NotificationHandlers;
using MediatR;
using NetArchTest.Rules;

namespace DomainEvents.Tests;

public class ArchitectureTests
{
    [Fact]
    public void NotificationHandlersShouldNotDependOnDbContext()
    {
        var notificationHandlers = Types.InAssembly(Assembly.GetAssembly(typeof(ProductDeletedNotificationHandler)))
            .That().ImplementInterface(typeof(INotificationHandler<>))
            .GetTypes()
            .ToArray();

        var dependOnDbContext = notificationHandlers
            .Where(x => x.GetConstructors().Single()
                .GetParameters()
                .Any(p => p.ParameterType == typeof(IDbContext)))
            .ToArray();

        Assert.Empty(dependOnDbContext);
    }
}