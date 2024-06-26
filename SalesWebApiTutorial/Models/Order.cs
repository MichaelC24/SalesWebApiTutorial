using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalesWebApiTutorial.Models
{
    public class Order
    {

        public int Id { get; set; }
        public int CustomerId { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Date { get; set; } = DateTime.MinValue;

        [StringLength(80)]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(11,2)")]
        public decimal Total { get; set; } 

        // Allowed inputs in the below property NEW, SHIFT
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;

        public virtual Customer? Customer { get; set; }
    }
}
