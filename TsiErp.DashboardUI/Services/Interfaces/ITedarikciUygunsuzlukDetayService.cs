using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services.Interfaces
{
    public interface ITedarikciUygunsuzlukDetayService
    {
        Task<List<SupplierUnsuitabilityDetailedCustomer>> GetSupplierUnsuitabilityDetailedCustomerAnalysis(int errorID, DateTime startDate, DateTime endDate);

        Task<List<SupplierUnsuitabilityDetailedProduct>> GetSupplierUnsuitabilityDetailedProductAnalysis(int errorID, DateTime startDate, DateTime endDate);
    }
}
