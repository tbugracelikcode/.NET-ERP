using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;


namespace TsiErp.DataAccess.Services.Login
{
    public static class LoginedUserService
    {
        public static Guid UserId { get; set; }

        public static string UserName { get; set; }
        public static Guid VersionTableId { get; set; }

        public static bool StockManagementShowAmountsChildMenu { get; set; } = true;
        public static bool ProductionManagementShowAmountsChildMenu { get; set; } = true;
        public static bool PurchaseManagementShowAmountsChildMenu { get; set; } = true;
        public static bool SalesManagementShowAmountsChildMenu { get; set; } = true;
        public static bool ShipmentShowAmountsChildMenu { get; set; } = true;
        public static bool MachWorkforceManagementShowAmountsChildMenu { get; set; } = true;

        public static int PurchaseOrderExchangeRateType { get; set; } = 0;
        public static int PurchaseRequestExchangeRateType { get; set; } = 0;
    }
}
