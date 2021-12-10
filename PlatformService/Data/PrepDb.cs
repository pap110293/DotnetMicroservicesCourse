using PlatformService.Models;

namespace PlatformService.Data;

public static class PrepDb
{
    public static void PrepPopulation(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();

        SeedData(serviceScope.ServiceProvider.GetService<AppDataContext>());
    }

    private static void SeedData(AppDataContext context)
    {
        if (!context.Platforms.Any())
        {
            context.Platforms.AddRange(new Platform[] {
                new Platform
                {
                    Name ="Dot net", 
                    Publisher ="Microsoft", 
                    Cost = "Free"
                },
                new Platform
                {
                    Name ="SQL server Express",
                    Publisher ="Microsoft",
                    Cost = "Free"
                },
                new Platform
                {
                    Name ="Kubernetes",
                    Publisher ="Unknow",
                    Cost = "Free"
                },
            });

            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> we already had it");
        }
    }
}
