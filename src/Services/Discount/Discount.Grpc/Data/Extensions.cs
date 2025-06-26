using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public static class Extensions
{
    public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DiscountContext>();
        // for some reason, it didn't wait for migration -> DB issues as a resulr
        // used sync call as a work around
        dbContext.Database.Migrate();

        return app;
    }
}