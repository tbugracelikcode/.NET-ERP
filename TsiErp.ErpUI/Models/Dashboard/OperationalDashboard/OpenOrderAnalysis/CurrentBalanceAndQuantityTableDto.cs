namespace TsiErp.ErpUI.Models.Dashboard.OperationalDashboard.OpenOrderAnalysis
{
    public class CurrentBalanceAndQuantityTableDto
    {
        public Guid ProductGroupID { get; set; }

        public string ProductGroupName { get; set; }

        public int BalanceQuantity { get; set; }

        public int ProductionOrderCount { get; set; }
    }
}
