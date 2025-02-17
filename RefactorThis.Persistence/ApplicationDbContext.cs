using Microsoft.EntityFrameworkCore;
using RefactorThis.Application.Common.Interfaces;
using RefactorThis.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace RefactorThis.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<Payment> Payments => Set<Payment>();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
