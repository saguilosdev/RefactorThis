using Microsoft.Extensions.DependencyInjection;
using RefactorThis.Application.Services;

namespace RefactorThis.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<InvoiceService>();
            return services;
        }
    }
}
