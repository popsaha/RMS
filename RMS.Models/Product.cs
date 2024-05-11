using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.Models
{
    /// <summary>
    /// Details of products
    /// </summary>
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Number { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? Group { get; set; }
        [Required]
        public string? UOM { get; set; }
        [Required]
        public string? Type { get; set; }
        [Display(Name = "Stock")]
        public int AmountInStock { get; set; }
        //public bool IsBelowStockThreshold { get; private set; }

        public int StoreId { get; set; } // Reference to the associated store
        [ForeignKey("StoreId")]
        public Store Store { get; set; } // Navigation property
    }
}
