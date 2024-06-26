using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesWebApiTutorial.Models
{
    public class Items
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(11,2)")]
        public decimal Price { get; set; }
       
    }
}
