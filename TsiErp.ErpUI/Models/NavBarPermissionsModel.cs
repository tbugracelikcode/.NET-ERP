namespace TsiErp.ErpUI.Models
{
    public static class NavBarPermissionsModel
    {
        public static bool StockManagementParentMenu { get; set; } = true;
        public static bool StockManagementMainRecordsMenu { get; set; } = true;
        public static bool ProductGroupsChildMenu { get; set; } = true;
        public static bool UnitSetsChildMenu { get; set; } = true;
        public static bool WarehousesChildMenu { get; set; } = true;
        public static bool ProductsChildMenu { get; set; } = true;
        public static bool TechnicalDrawingsChildMenu { get; set; } = true;
        public static bool ProductRefNumbersChildMenu { get; set; } = true;
        public static bool StockManagementTransactionsMenu { get; set; } = true;
        public static bool StockFichesChildMenu { get; set; } = true;
        public static bool StockManagementReportsMenu { get; set; } = true;
        public static bool ListofMaterialsChildMenu { get; set; } = true;
        public static bool MaterialWarehouseStatusChildMenu { get; set; } = true;
        public static bool InventoryListChildMenu { get; set; } = true;
        public static bool ByProdGrpPerfAnalysisChildMenu { get; set; } = true;
        public static bool ByProdGrpScrapAnalysisChildMenu { get; set; } = true;
    }
}
