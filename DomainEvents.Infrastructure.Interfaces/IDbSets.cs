using DomainEvents.Entities;
using Microsoft.EntityFrameworkCore;

namespace DomainEvents.Infrastructure.Interfaces;

public interface IDbSets
{
    DbSet<Product> Products { get; }

    DbSet<Category> Categories { get; }

    DbSet<ProductCategory> ProductCategories { get; }

    DbSet<Message> Messages { get; }
}