using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RefactorThis.Application;
using RefactorThis.Application.Common.Interfaces;
using RefactorThis.Persistence.Repositories;

namespace RefactorThis.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddApplication();
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("TestDb"));
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            return services;
        }
    }
}
