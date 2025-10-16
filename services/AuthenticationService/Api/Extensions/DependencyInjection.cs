using Common.Interfaces;
using Infrastructure.Security;

namespace Api.Extensions;


internal static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IPasswordHasher, BCryptPasswordHasher>();
        services.AddTransient<ITokenHasher, HmacTokenHasher>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var hmacSecret = configuration.GetValue<string>("HmacSecret")
                ?? throw new InvalidOperationException("Hmac secret not found");
            return new HmacTokenHasher(hmacSecret);
        });
    
        return services;
    }
}
