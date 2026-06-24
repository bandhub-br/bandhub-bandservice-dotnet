using BandHub.BandService.Features.Bands.CreateBand;
using BandHub.BandService.Features.Bands.Domain;
using BandHub.BandService.Features.Bands.GetBands;
using BandHub.BandService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BandHub.BandService.Common;

public static class DependencyInjection
{
        public static IServiceCollection AddBandService(this IServiceCollection services, IConfiguration configuration)
        {
        services.AddDbContext<BandDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IBandRepository, BandRepository>();

        services.AddScoped<CreateBandHandler>();
        services.AddScoped<GetBandsHandler>();

        return services;
    }
}
