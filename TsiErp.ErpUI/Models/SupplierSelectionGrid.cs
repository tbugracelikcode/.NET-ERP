namespace TsiErp.ErpUI.Models
{
    public class SupplierSelectionGrid
    {
        public string ProductCode { get; set; }

        public decimal UnitPrice { get; set; }

        public Guid? CurrentAccountID { get; set; }

        public string CurrentAccountName { get; set; }

        public Guid? CurrenyID { get; set; }

        public string CurrenyCode { get; set; }

        public int SupplyDate { get; set; }
    }
}
