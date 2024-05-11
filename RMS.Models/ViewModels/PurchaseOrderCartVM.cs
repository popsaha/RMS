namespace RMS.Models.ViewModels
{
    public class PurchaseOrderCartVM
    {
        public IEnumerable<PurchaseOrderCart> PurchaseOrderCartList { get; set; }
        public PurchaseOrderHeader PurchaseOrderHeader { get; set; }
    }
}
