using TsiErp.DataAccess.EntityFrameworkCore.Repositories;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.BillsofMaterial;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.BillsofMaterialLine;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ByDateStockMovement;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.CalibrationRecord;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.CalibrationVerification;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ContractProductionTracking;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ContractUnsuitabilityItem;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Currency;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.CurrentAccountCard;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.CustomerComplaintItem;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Department;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Employee;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.EquipmentRecord;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ExchangeRate;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.FinalControlUnsuitabilityItem;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.FinalControlUnsuitabilityReport;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Forecast;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ForecastLine;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.GrandTotalStockMovement;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.HaltReason;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Logging;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.MaintenanceInstruction;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.MaintenanceInstructionLine;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.MaintenancePeriod;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Menu;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.OperationUnsuitabilityItem;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.OperationUnsuitabilityReport;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PaymentPlan;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Period;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PlannedMaintenance;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PlannedMaintenanceLine;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Product;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductGroup;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionOrder;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionOrderChangeItem;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionTracking;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionTrackingHaltLine;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductReferanceNumber;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductsOperation;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductsOperationLine;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseOrder;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseOrderLine;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchasePrice;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchasePriceLine;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseRequest;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseRequestLine;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseUnsuitabilityReport;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchasingUnsuitabilityItem;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Route;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.RouteLine;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesOrder;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesOrderLine;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesPrice;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesPriceLine;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesProposition;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesPropositionLine;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Shift;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ShiftLine;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ShippingAdress;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Station;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.StationGroup;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.StationInventory;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.TechnicalDrawing;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.TemplateOperation;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.TemplateOperationLine;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.UnitSet;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.UnplannedMaintenance;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.UnplannedMaintenanceLine;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.User;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.UserGroup;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Warehouse;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.WorkOrder;

namespace TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TsiErpDbContext _dbContext;

        public UnitOfWork()
        {
            _dbContext = new TsiErpDbContext();
        }

        #region Dispose Method
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }

            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region SaveChanges Method
        public async Task<int> SaveChanges()
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    int returnValue = await _dbContext.SaveChangesAsync();
                    dbContextTransaction.Commit();
                    return returnValue;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    return 0;
                }
            }
        }
        #endregion

        #region Repositories
        private EFCoreBranchesRepository _branchRepository;
        public EFCoreBranchesRepository BranchRepository => _branchRepository ?? (_branchRepository = new EFCoreBranchesRepository(_dbContext));


        private EFCoreBillsofMaterialLinesRepository _billsofMaterialLinesRepository;
        public EFCoreBillsofMaterialLinesRepository BillsofMaterialLinesRepository => _billsofMaterialLinesRepository ?? (_billsofMaterialLinesRepository = new EFCoreBillsofMaterialLinesRepository(_dbContext));


        private EFCoreBillsofMaterialsRepository _billsofMaterialRepository;
        public EFCoreBillsofMaterialsRepository BillsofMaterialsRepository => _billsofMaterialRepository ?? (_billsofMaterialRepository = new EFCoreBillsofMaterialsRepository(_dbContext));


        private EFCoreByDateStockMovementsRepository _byDateStockMovementsRepository;
        public EFCoreByDateStockMovementsRepository ByDateStockMovementsRepository => _byDateStockMovementsRepository ?? (_byDateStockMovementsRepository = new EFCoreByDateStockMovementsRepository(_dbContext));


        private EFCoreCalibrationRecordsRepository _calibrationRecordsRepository;
        public EFCoreCalibrationRecordsRepository CalibrationRecordsRepository => _calibrationRecordsRepository ?? (_calibrationRecordsRepository = new EFCoreCalibrationRecordsRepository(_dbContext));


        private EFCoreCalibrationVerificationsRepository _calibrationVerificationsRepository;
        public EFCoreCalibrationVerificationsRepository CalibrationVerificationsRepository => _calibrationVerificationsRepository ?? (_calibrationVerificationsRepository = new EFCoreCalibrationVerificationsRepository(_dbContext));


        private EFCoreContractProductionTrackingsRepository _contractProductionTrackingsRepository;
        public EFCoreContractProductionTrackingsRepository ContractProductionTrackingsRepository => _contractProductionTrackingsRepository ?? (_contractProductionTrackingsRepository = new EFCoreContractProductionTrackingsRepository(_dbContext));


        private EFCoreContractUnsuitabilityItemsRepository _contractUnsuitabilityItemsRepository;
        public EFCoreContractUnsuitabilityItemsRepository ContractUnsuitabilityItemsRepository => _contractUnsuitabilityItemsRepository ?? (_contractUnsuitabilityItemsRepository = new EFCoreContractUnsuitabilityItemsRepository(_dbContext));


        private EFCoreCurrenciesRepository _currenciesRepository;
        public EFCoreCurrenciesRepository CurrenciesRepository => _currenciesRepository ?? (_currenciesRepository = new EFCoreCurrenciesRepository(_dbContext));


        private EFCoreCurrentAccountCardsRepository _currentAccountCardsRepository;
        public EFCoreCurrentAccountCardsRepository CurrentAccountCardsRepository => _currentAccountCardsRepository ?? (_currentAccountCardsRepository = new EFCoreCurrentAccountCardsRepository(_dbContext));


        private EFCoreCustomerComplaintItemsRepository _customerComplaintItemsRepository;
        public EFCoreCustomerComplaintItemsRepository CustomerComplaintItemsRepository => _customerComplaintItemsRepository ?? (_customerComplaintItemsRepository = new EFCoreCustomerComplaintItemsRepository(_dbContext));


        private EFCoreDepartmentsRepository _departmentsRepository;
        public EFCoreDepartmentsRepository DepartmentsRepository => _departmentsRepository ?? (_departmentsRepository = new EFCoreDepartmentsRepository(_dbContext));


        private EFCoreEmployeesRepository _employeesRepository;
        public EFCoreEmployeesRepository EmployeesRepository => _employeesRepository ?? (_employeesRepository = new EFCoreEmployeesRepository(_dbContext));


        private EFCoreEquipmentRecordsRepository _equipmentRecordsRepository;
        public EFCoreEquipmentRecordsRepository EquipmentRecordsRepository => _equipmentRecordsRepository ?? (_equipmentRecordsRepository = new EFCoreEquipmentRecordsRepository(_dbContext));


        private EFCoreExchangeRatesRepository _exchangeRatesRepository;
        public EFCoreExchangeRatesRepository ExchangeRatesRepository => _exchangeRatesRepository ?? (_exchangeRatesRepository = new EFCoreExchangeRatesRepository(_dbContext));


        private EFCoreFinalControlUnsuitabilityItemsRepository _finalControlUnsuitabilityItemsRepository;
        public EFCoreFinalControlUnsuitabilityItemsRepository FinalControlUnsuitabilityItemsRepository => _finalControlUnsuitabilityItemsRepository ?? (_finalControlUnsuitabilityItemsRepository = new EFCoreFinalControlUnsuitabilityItemsRepository(_dbContext));


        private EFCoreFinalControlUnsuitabilityReportsRepository _finalControlUnsuitabilityReportsRepository;
        public EFCoreFinalControlUnsuitabilityReportsRepository FinalControlUnsuitabilityReportsRepository => _finalControlUnsuitabilityReportsRepository ?? (_finalControlUnsuitabilityReportsRepository = new EFCoreFinalControlUnsuitabilityReportsRepository(_dbContext));


        private EFCoreForecastsRepository _forecastsRepository;
        public EFCoreForecastsRepository ForecastsRepository => _forecastsRepository ?? (_forecastsRepository = new EFCoreForecastsRepository(_dbContext));


        private EFCoreForecastLinesRepository _forecastLinesRepository;
        public EFCoreForecastLinesRepository ForecastLinesRepository => _forecastLinesRepository ?? (_forecastLinesRepository = new EFCoreForecastLinesRepository(_dbContext));


        private EFCoreGrandTotalStockMovementsRepository _grandTotalStockMovementsRepository;
        public EFCoreGrandTotalStockMovementsRepository GrandTotalStockMovementsRepository => _grandTotalStockMovementsRepository ?? (_grandTotalStockMovementsRepository = new EFCoreGrandTotalStockMovementsRepository(_dbContext));


        private EFCoreHaltReasonsRepository _haltReasonsRepository;
        public EFCoreHaltReasonsRepository HaltReasonsRepository => _haltReasonsRepository ?? (_haltReasonsRepository = new EFCoreHaltReasonsRepository(_dbContext));


        private EfCoreLogsRepository _logsRepository;
        public EfCoreLogsRepository LogsRepository => _logsRepository ?? (_logsRepository = new EfCoreLogsRepository(_dbContext));


        private EFCoreMaintenanceInstructionsRepository _maintenanceInstructionsRepository;
        public EFCoreMaintenanceInstructionsRepository MaintenanceInstructionsRepository => _maintenanceInstructionsRepository ?? (_maintenanceInstructionsRepository = new EFCoreMaintenanceInstructionsRepository(_dbContext));


        private EFCoreMaintenanceInstructionLinesRepository _maintenanceInstructionLinesRepository;
        public EFCoreMaintenanceInstructionLinesRepository MaintenanceInstructionLinesRepository => _maintenanceInstructionLinesRepository ?? (_maintenanceInstructionLinesRepository = new EFCoreMaintenanceInstructionLinesRepository(_dbContext));


        private EFCoreMaintenancePeriodsRepository _maintenancePeriodsRepository;
        public EFCoreMaintenancePeriodsRepository MaintenancePeriodsRepository => _maintenancePeriodsRepository ?? (_maintenancePeriodsRepository = new EFCoreMaintenancePeriodsRepository(_dbContext));


        private EFCoreOperationUnsuitabilityItemsRepository _operationUnsuitabilityItemsRepository;
        public EFCoreOperationUnsuitabilityItemsRepository OperationUnsuitabilityItemsRepository => _operationUnsuitabilityItemsRepository ?? (_operationUnsuitabilityItemsRepository = new EFCoreOperationUnsuitabilityItemsRepository(_dbContext));


        private EFCoreOperationUnsuitabilityReportsRepository _operationUnsuitabilityReportsRepository;
        public EFCoreOperationUnsuitabilityReportsRepository OperationUnsuitabilityReportsRepository => _operationUnsuitabilityReportsRepository ?? (_operationUnsuitabilityReportsRepository = new EFCoreOperationUnsuitabilityReportsRepository(_dbContext));


        private EFCorePaymentPlansRepository _paymentPlansRepository;
        public EFCorePaymentPlansRepository PaymentPlansRepository => _paymentPlansRepository ?? (_paymentPlansRepository = new EFCorePaymentPlansRepository(_dbContext));


        private EFCorePeriodsRepository _periodsRepository;
        public EFCorePeriodsRepository PeriodsRepository => _periodsRepository ?? (_periodsRepository = new EFCorePeriodsRepository(_dbContext));


        private EFCorePlannedMaintenancesRepository _plannedMaintenancesRepository;
        public EFCorePlannedMaintenancesRepository PlannedMaintenancesRepository => _plannedMaintenancesRepository ?? (_plannedMaintenancesRepository = new EFCorePlannedMaintenancesRepository(_dbContext));


        private EFCorePlannedMaintenanceLinesRepository _plannedMaintenanceLinesRepository;
        public EFCorePlannedMaintenanceLinesRepository PlannedMaintenanceLinesRepository => _plannedMaintenanceLinesRepository ?? (_plannedMaintenanceLinesRepository = new EFCorePlannedMaintenanceLinesRepository(_dbContext));


        private EFCoreProductsRepository _productsRepository;
        public EFCoreProductsRepository ProductsRepository => _productsRepository ?? (_productsRepository = new EFCoreProductsRepository(_dbContext));


        private EFCoreProductGroupsRepository _productGroupsRepository;
        public EFCoreProductGroupsRepository ProductGroupsRepository => _productGroupsRepository ?? (_productGroupsRepository = new EFCoreProductGroupsRepository(_dbContext));


        private EFCoreProductionOrdersRepository _productionOrdersRepository;
        public EFCoreProductionOrdersRepository ProductionOrdersRepository => _productionOrdersRepository ?? (_productionOrdersRepository = new EFCoreProductionOrdersRepository(_dbContext));


        private EFCoreProductionOrderChangeItemsRepository _productionOrderChangeItemsRepository;
        public EFCoreProductionOrderChangeItemsRepository ProductionOrderChangeItemsRepository => _productionOrderChangeItemsRepository ?? (_productionOrderChangeItemsRepository = new EFCoreProductionOrderChangeItemsRepository(_dbContext));


        private EFCoreProductionTrackingsRepository _productionTrackingsRepository;
        public EFCoreProductionTrackingsRepository ProductionTrackingsRepository => _productionTrackingsRepository ?? (_productionTrackingsRepository = new EFCoreProductionTrackingsRepository(_dbContext));


        private EFCoreProductionTrackingHaltLinesRepository _productionTrackingHaltLinesRepository;
        public EFCoreProductionTrackingHaltLinesRepository ProductionTrackingHaltLinesRepository => _productionTrackingHaltLinesRepository ?? (_productionTrackingHaltLinesRepository = new EFCoreProductionTrackingHaltLinesRepository(_dbContext));


        private EFCoreProductReferanceNumbersRepository _productReferanceNumbersRepository;
        public EFCoreProductReferanceNumbersRepository ProductReferanceNumbersRepository => _productReferanceNumbersRepository ?? (_productReferanceNumbersRepository = new EFCoreProductReferanceNumbersRepository(_dbContext));


        private EFCoreProductsOperationsRepository _productsOperationsRepository;
        public EFCoreProductsOperationsRepository ProductsOperationsRepository => _productsOperationsRepository ?? (_productsOperationsRepository = new EFCoreProductsOperationsRepository(_dbContext));


        private EFCoreProductsOperationLinesRepository _productsOperationLinesRepository;
        public EFCoreProductsOperationLinesRepository ProductsOperationLinesRepository => _productsOperationLinesRepository ?? (_productsOperationLinesRepository = new EFCoreProductsOperationLinesRepository(_dbContext));


        private EFCorePurchaseOrdersRepository _purchaseOrdersRepository;
        public EFCorePurchaseOrdersRepository PurchaseOrdersRepository => _purchaseOrdersRepository ?? (_purchaseOrdersRepository = new EFCorePurchaseOrdersRepository(_dbContext));


        private EFCorePurchaseOrderLinesRepository _purchaseOrderLinesRepository;
        public EFCorePurchaseOrderLinesRepository PurchaseOrderLinesRepository => _purchaseOrderLinesRepository ?? (_purchaseOrderLinesRepository = new EFCorePurchaseOrderLinesRepository(_dbContext));


        private EFCorePurchasePricesRepository _purchasePricesRepository;
        public EFCorePurchasePricesRepository PurchasePricesRepository => _purchasePricesRepository ?? (_purchasePricesRepository = new EFCorePurchasePricesRepository(_dbContext));


        private EFCorePurchasePriceLinesRepository _purchasePriceLinesRepository;
        public EFCorePurchasePriceLinesRepository PurchasePriceLinesRepository => _purchasePriceLinesRepository ?? (_purchasePriceLinesRepository = new EFCorePurchasePriceLinesRepository(_dbContext));


        private EFCorePurchaseRequestsRepository _purchaseRequestsRepository;
        public EFCorePurchaseRequestsRepository PurchaseRequestsRepository => _purchaseRequestsRepository ?? (_purchaseRequestsRepository = new EFCorePurchaseRequestsRepository(_dbContext));


        private EFCorePurchaseRequestLinesRepository _purchaseRequestLinesRepository;
        public EFCorePurchaseRequestLinesRepository PurchaseRequestLinesRepository => _purchaseRequestLinesRepository ?? (_purchaseRequestLinesRepository = new EFCorePurchaseRequestLinesRepository(_dbContext));


        private EFCorePurchaseUnsuitabilityReportsRepository _purchaseUnsuitabilityReportsRepository;
        public EFCorePurchaseUnsuitabilityReportsRepository PurchaseUnsuitabilityReportsRepository => _purchaseUnsuitabilityReportsRepository ?? (_purchaseUnsuitabilityReportsRepository = new EFCorePurchaseUnsuitabilityReportsRepository(_dbContext));


        private EFCorePurchasingUnsuitabilityItemsRepository _purchasingUnsuitabilityItemsRepository;
        public EFCorePurchasingUnsuitabilityItemsRepository PurchasingUnsuitabilityItemsRepository => _purchasingUnsuitabilityItemsRepository ?? (_purchasingUnsuitabilityItemsRepository = new EFCorePurchasingUnsuitabilityItemsRepository(_dbContext));


        private EFCoreRoutesRepository _routesRepository;
        public EFCoreRoutesRepository RoutesRepository => _routesRepository ?? (_routesRepository = new EFCoreRoutesRepository(_dbContext));


        private EFCoreRouteLinesRepository _routeLinesRepository;
        public EFCoreRouteLinesRepository RouteLinesRepository => _routeLinesRepository ?? (_routeLinesRepository = new EFCoreRouteLinesRepository(_dbContext));


        private EFCoreSalesOrdersRepository _salesOrdersRepository;
        public EFCoreSalesOrdersRepository SalesOrdersRepository => _salesOrdersRepository ?? (_salesOrdersRepository = new EFCoreSalesOrdersRepository(_dbContext));


        private EFCoreSalesOrderLinesRepository _salesOrderLinesRepository;
        public EFCoreSalesOrderLinesRepository SalesOrderLinesRepository => _salesOrderLinesRepository ?? (_salesOrderLinesRepository = new EFCoreSalesOrderLinesRepository(_dbContext));


        private EFCoreSalesPricesRepository _salesPricesRepository;
        public EFCoreSalesPricesRepository SalesPricesRepository => _salesPricesRepository ?? (_salesPricesRepository = new EFCoreSalesPricesRepository(_dbContext));


        private EFCoreSalesPriceLinesRepository _salesPriceLinesRepository;
        public EFCoreSalesPriceLinesRepository SalesPriceLinesRepository => _salesPriceLinesRepository ?? (_salesPriceLinesRepository = new EFCoreSalesPriceLinesRepository(_dbContext));


        private EFCoreSalesPropositionsRepository _salesPropositionsRepository;
        public EFCoreSalesPropositionsRepository SalesPropositionsRepository => _salesPropositionsRepository ?? (_salesPropositionsRepository = new EFCoreSalesPropositionsRepository(_dbContext));


        private EFCoreSalesPropositionLinesRepository _salesPropositionLinesRepository;
        public EFCoreSalesPropositionLinesRepository SalesPropositionLinesRepository => _salesPropositionLinesRepository ?? (_salesPropositionLinesRepository = new EFCoreSalesPropositionLinesRepository(_dbContext));


        private EFCoreShiftsRepository _shiftsRepository;
        public EFCoreShiftsRepository ShiftsRepository => _shiftsRepository ?? (_shiftsRepository = new EFCoreShiftsRepository(_dbContext));


        private EFCoreShiftLinesRepository _shiftLinesRepository;
        public EFCoreShiftLinesRepository ShiftLinesRepository => _shiftLinesRepository ?? (_shiftLinesRepository = new EFCoreShiftLinesRepository(_dbContext));


        private EFCoreShippingAdressesRepository _shippingAdressesRepository;
        public EFCoreShippingAdressesRepository ShippingAdressesRepository => _shippingAdressesRepository ?? (_shippingAdressesRepository = new EFCoreShippingAdressesRepository(_dbContext));


        private EFCoreStationsRepository _stationsRepository;
        public EFCoreStationsRepository StationsRepository => _stationsRepository ?? (_stationsRepository = new EFCoreStationsRepository(_dbContext));


        private EFCoreStationGroupsRepository _stationGroupsRepository;
        public EFCoreStationGroupsRepository StationGroupsRepository => _stationGroupsRepository ?? (_stationGroupsRepository = new EFCoreStationGroupsRepository(_dbContext));


        private EFCoreStationInventoriesRepository _stationInventoriesRepository;
        public EFCoreStationInventoriesRepository StationInventoriesRepository => _stationInventoriesRepository ?? (_stationInventoriesRepository = new EFCoreStationInventoriesRepository(_dbContext));


        private EFCoreTechnicalDrawingsRepository _technicalDrawingsRepository;
        public EFCoreTechnicalDrawingsRepository TechnicalDrawingsRepository => _technicalDrawingsRepository ?? (_technicalDrawingsRepository = new EFCoreTechnicalDrawingsRepository(_dbContext));


        private EFCoreTemplateOperationsRepository _templateOperationsRepository;
        public EFCoreTemplateOperationsRepository TemplateOperationsRepository => _templateOperationsRepository ?? (_templateOperationsRepository = new EFCoreTemplateOperationsRepository(_dbContext));


        private EFCoreTemplateOperationLinesRepository _templateOperationLinesRepository;
        public EFCoreTemplateOperationLinesRepository TemplateOperationLinesRepository => _templateOperationLinesRepository ?? (_templateOperationLinesRepository = new EFCoreTemplateOperationLinesRepository(_dbContext));


        private EFCoreUnitSetsRepository _unitSetsRepository;
        public EFCoreUnitSetsRepository UnitSetsRepository => _unitSetsRepository ?? (_unitSetsRepository = new EFCoreUnitSetsRepository(_dbContext));


        private EFCoreUnplannedMaintenancesRepository _unplannedMaintenancesRepository;
        public EFCoreUnplannedMaintenancesRepository UnplannedMaintenancesRepository => _unplannedMaintenancesRepository ?? (_unplannedMaintenancesRepository = new EFCoreUnplannedMaintenancesRepository(_dbContext));


        private EFCoreUnplannedMaintenanceLinesRepository _unplannedMaintenanceLinesRepository;
        public EFCoreUnplannedMaintenanceLinesRepository UnplannedMaintenanceLinesRepository => _unplannedMaintenanceLinesRepository ?? (_unplannedMaintenanceLinesRepository = new EFCoreUnplannedMaintenanceLinesRepository(_dbContext));


        private EFCoreUsersRepository _usersRepository;
        public EFCoreUsersRepository UsersRepository => _usersRepository ?? (_usersRepository = new EFCoreUsersRepository(_dbContext));


        private EFCoreUserGroupsRepository _userGroupsRepository;
        public EFCoreUserGroupsRepository UserGroupsRepository => _userGroupsRepository ?? (_userGroupsRepository = new EFCoreUserGroupsRepository(_dbContext));


        private EFCoreWarehousesRepository _warehousesRepository;
        public EFCoreWarehousesRepository WarehousesRepository => _warehousesRepository ?? (_warehousesRepository = new EFCoreWarehousesRepository(_dbContext));


        private EFCoreWorkOrdersRepository _workOrdersRepository;
        public EFCoreWorkOrdersRepository WorkOrdersRepository => _workOrdersRepository ?? (_workOrdersRepository = new EFCoreWorkOrdersRepository(_dbContext));


        private EFCoreMenusRepository _menusRepository;
        public EFCoreMenusRepository MenusRepository => _menusRepository ?? (_menusRepository = new EFCoreMenusRepository(_dbContext));
        #endregion

    }
}
