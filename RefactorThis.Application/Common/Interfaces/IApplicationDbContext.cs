using Microsoft.EntityFrameworkCore;
using RefactorThis.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace RefactorThis.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Invoice> Invoices { get; }
        DbSet<Payment> Payments { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
