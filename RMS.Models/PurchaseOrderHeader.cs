using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.Models
{
    /// <summary>
    /// Description of Inquiry Process.
    /// </summary>
    public class PurchaseOrderHeader
    {
        [Key]
        public int Id { get; set; }

        public int ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser? ApplicationUser { get; set; }

        public int StoreId { get; set; }
        [ForeignKey("StoreId")]
        [ValidateNever]
        public Store? Store { get; set; }

        public string RequestNo { get; set; } = Guid.NewGuid().ToString();
        public string? ReferenceNo { get; set; }
        public DateOnly RefernceDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        public List<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
        public double Budget { get; set; }
        public DateOnly ExpectedDelivery { get; set; }
        public string? OrderStatus { get; set; }
        //public bool Fulfilled { get; set; } = false;


    }
}
