// using AssemblyService.Domain.Interfaces;
using AssemblyService.Infrastructure.Context;
using AssemblyService.Infrastructure.Interfaces;
// using AssemblyService.Infrastructure.Repositories;

namespace AssemblyService.Api.Configurations
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureApi(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            // services.AddScoped<IAssemblyProductRepository, AssemblyProductRepository>();
            // services.AddScoped<IAssemblyProductService, AssemblyProductService>();

            // services.AddAutoMapper(typeof(DomainToDtoMappingProfile));
        }
    }
}