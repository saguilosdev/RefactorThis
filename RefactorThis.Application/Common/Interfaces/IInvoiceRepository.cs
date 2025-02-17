using RefactorThis.Domain.Entities;
using System.Threading.Tasks;

namespace RefactorThis.Application.Common.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<Invoice> GetInvoice(int id);
        Task SaveInvoice(Invoice invoice);
    }
}
