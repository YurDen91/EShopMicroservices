using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("Database connection string is not configured.");
        }
        
        //services.adddbcontext<OrderingContext>(options =>
        //    options.UseNpgsql(connectionString));
        
        // Register infrastructure services here
        // Example: services.AddScoped<IOrderRepository, OrderRepository>();

        // You can also use configuration to set up database connections, etc.
        // Example: services.AddDbContext<OrderingContext>(options =>
        //     options.UseSqlServer(configuration.GetConnectionString("OrderingDatabase")));

        return services;
    }
}