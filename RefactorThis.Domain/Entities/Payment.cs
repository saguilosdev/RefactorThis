using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RefactorThis.Domain.Entities
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public string Reference { get; set; }
    }
}
