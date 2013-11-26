using System.ComponentModel.DataAnnotations;

namespace Commerce.Application.Billing
{
    public class StateTax
    {
        [Key]
        public string Abbreviation { get; set; }
        public string Name { get; set; }
        public decimal TaxRate { get; set; }
    }
}