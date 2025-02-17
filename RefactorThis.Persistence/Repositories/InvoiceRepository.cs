using Microsoft.EntityFrameworkCore;
using RefactorThis.Application.Common.Interfaces;
using RefactorThis.Domain.Entities;
using System.Threading.Tasks;

namespace RefactorThis.Persistence.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly IApplicationDbContext _context;

        public InvoiceRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Invoice> GetInvoice(int id)
        {
            return await _context.Invoices.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task SaveInvoice(Invoice invoice)
        {
            var existingInvoice = await _context.Invoices.FirstOrDefaultAsync(i => i.Id == invoice.Id);
            if (existingInvoice != null)
            {
                existingInvoice.Payments = invoice.Payments;
                existingInvoice.TaxAmount = invoice.TaxAmount;
                existingInvoice.AmountPaid = invoice.AmountPaid;
                existingInvoice.Amount = invoice.Amount;
                await _context.SaveChangesAsync();
                return;
            }

            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
        }
    }
}