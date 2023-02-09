using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.EntityFrameworkCore.EntityframeworkCore;
using Tsi.EntityFrameworkCore.Extensions;
using TsiErp.DataAccess.EntityFrameworkCore.Configurations;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.CalibrationRecord;
using TsiErp.Entities.Entities.CalibrationVerification;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem;
using TsiErp.Entities.Entities.Department;
using TsiErp.Entities.Entities.Employee;
using TsiErp.Entities.Entities.EquipmentRecord;
using TsiErp.Entities.Entities.Logging;
using TsiErp.Entities.Entities.Period;
using TsiErp.Entities.Entities.SalesProposition;
using TsiErp.Entities.Entities.SalesPropositionLine;
using TsiErp.Entities.Entities.Station;
using TsiErp.Entities.Entities.StationGroup;
using TsiErp.Entities.Entities.UnitSet;
using TsiErp.Entities.Entities.Vsm;
using TsiErp.Entities.Entities.Currency;
using TsiErp.Entities.Entities.PaymentPlan;
using TsiErp.Entities.Entities.WareHouse;
using TsiErp.Entities.Entities.OperationUnsuitabilityItem;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityItem;
using TsiErp.Entities.Entities.ExchangeRate;
using TsiErp.Entities.Entities.CurrentAccountCard;
using TsiErp.Entities.Entities.ProductGroup;
using TsiErp.Entities.Entities.Product;
using TsiErp.Entities.Entities.CustomerComplaintItem;
using TsiErp.Entities.Entities.ProductionOrderChangeItem;
using TsiErp.Entities.Entities.PurchasingUnsuitabilityItem;
using TsiErp.Entities.Entities.ShippingAdress;
using TsiErp.Entities.Entities.TemplateOperation;
using TsiErp.Entities.Entities.TemplateOperationLine;
using TsiErp.Entities.Entities.Route;
using TsiErp.Entities.Entities.RouteLine;
using TsiErp.Entities.Entities.Calendar;
using TsiErp.Entities.Entities.CalendarLine;
using TsiErp.Entities.Entities.Shift;
using TsiErp.Entities.Entities.ShiftLine;
using TsiErp.Entities.Entities.SalesOrderLine;
using TsiErp.Entities.Entities.SalesOrder;
using TsiErp.Entities.Entities.ProductsOperation;
using TsiErp.Entities.Entities.ProductsOperationLine;
using TsiErp.Entities.Entities.BillsofMaterial;
using TsiErp.Entities.Entities.BillsofMaterialLine;
using TsiErp.Entities.Entities.CalendarDay;
using TsiErp.Entities.Entities.ProductionOrder;
using TsiErp.Entities.Entities.WorkOrder;
using TsiErp.Entities.Entities.PurchaseOrder;
using TsiErp.Entities.Entities.PurchaseOrderLine;
using TsiErp.Entities.Entities.PurchaseRequest;
using TsiErp.Entities.Entities.PurchaseRequestLine;
using TsiErp.Entities.Entities.Forecast;
using TsiErp.Entities.Entities.ForecastLine;
using TsiErp.Entities.Entities.PurchaseUnsuitabilityReport;
using TsiErp.Entities.Entities.OperationUnsuitabilityReport;
using TsiErp.Entities.Entities.ProductionTracking;
using TsiErp.Entities.Entities.ContractProductionTracking;
using TsiErp.Entities.Entities.ProductionTrackingHaltLine;
using TsiErp.Entities.Entities.HaltReason;
using TsiErp.Entities.Entities.Menu;
using TsiErp.Entities.Entities.SalesPrice;
using TsiErp.Entities.Entities.SalesPriceLine;
using TsiErp.Entities.Entities.PurchasePrice;
using TsiErp.Entities.Entities.PurchasePriceLine;
using TsiErp.Entities.Entities.UserGroup;
using TsiErp.Entities.Entities.User;
using TsiErp.Entities.Entities.StationInventory;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityReport;
using TsiErp.Entities.Entities.MaintenancePeriod;
using TsiErp.Entities.Entities.MaintenanceInstruction;
using TsiErp.Entities.Entities.MaintenanceInstructionLine;
using TsiErp.Entities.Entities.PlannedMaintenance;
using TsiErp.Entities.Entities.PlannedMaintenanceLine;
using TsiErp.Entities.Entities.UnplannedMaintenance;
using TsiErp.Entities.Entities.UnplannedMaintenanceLine;
using TsiErp.Entities.Entities.ByDateStockMovement;
using TsiErp.Entities.Entities.GrandTotalStockMovement;
using TsiErp.Entities.Entities.TechnicalDrawing;
using TsiErp.Entities.Entities.ProductReferanceNumber;

namespace TsiErp.DataAccess.EntityFrameworkCore
{
    public class TsiErpDbContext : DbContext
    {
        public IConfigurationRoot _configuration;

        public virtual bool _IsSoftDelete { get; set; }

        public virtual string BasePath { get; set; }

        public virtual string JsonFile { get; set; }

        public virtual string SoftDeleteSectionName { get; set; }

        public virtual string SoftDeleteKey { get; set; }

        public virtual string ConnectionStringKey { get; set; }

        public TsiErpDbContext()
        {
            this.BasePath = Directory.GetCurrentDirectory();
            this.JsonFile = "appsettings.json";
            this.SoftDeleteSectionName = "AppParams";
            this.SoftDeleteKey = "IsSoftDelete";
            this.ConnectionStringKey = "AppConnectionString";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(BasePath)
                .AddJsonFile(JsonFile)
                .Build();

            if (configuration != null)
            {
                _configuration = configuration;

                _IsSoftDelete = _configuration.GetSection(SoftDeleteSectionName)[SoftDeleteKey].ToString() == "true" ? true : false;
            }


            optionsBuilder.UseSqlServer(_configuration.GetConnectionString(ConnectionStringKey));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            if (_IsSoftDelete)
            {
                foreach (var entityType in builder.Model.GetEntityTypes())
                {
                    if (typeof(IFullEntityObject).IsAssignableFrom(entityType.ClrType))
                    {
                        entityType.AddSoftDeleteQueryFilter();
                    }
                }
            }

            builder.ConfigureBranches();
            builder.ConfigurePeriods();
            builder.ConfigureLogs();
            builder.ConfigureUnitSets();
            builder.ConfigureStationGroups();
            builder.ConfigureStations();
            builder.ConfigureEmployees();
            builder.ConfigureEquipmentRecords();
            builder.ConfigureDepartments();
            builder.ConfigureContractUnsuitabilityItems();
            builder.ConfigureCalibrationVerifications();
            builder.ConfigureCalibrationRecords();
            builder.ConfigureVsmSchemas();
            builder.ConfigureCurrencies();
            builder.ConfigurePaymentPlans();
            builder.ConfigureOperationUnsuitabilityItems();
            builder.ConfigureFinalControlUnsuitabilityItems();
            builder.ConfigureExchangeRates();
            builder.ConfigureCurrentAccountCards();
            builder.ConfigureProductCodes();
            builder.ConfigureProductGroups();
            builder.ConfigureProductionOrderChangeItems();
            builder.ConfigurePurchasingUnsuitabilityItems();
            builder.ConfigureCustomerComplaintItems();
            builder.ConfigureShippingAdresses();
            builder.ConfigureWarehouses();
            builder.ConfigureTemplateOperations();
            builder.ConfigureTemplateOperationLines();
            builder.ConfigureRoutes();
            builder.ConfigureRouteLines();
            builder.ConfigureSalesPropositions();
            builder.ConfigureSalesPropositionLines();
            builder.ConfigureCalendars();
            builder.ConfigureCalendarLines();
            builder.ConfigureCalendarDays();
            builder.ConfigureShifts();
            builder.ConfigureShiftLines();
            builder.ConfigureSalesOrders();
            builder.ConfigureSalesOrderLines();
            builder.ConfigureProductsOperations();
            builder.ConfigureProductsOperationLines();
            builder.ConfigureBillsofMaterials();
            builder.ConfigureBillsofMaterialLines();
            builder.ConfigureProductionOrders();
            builder.ConfigureWorkOrders();
            builder.ConfigurePurchaseOrders();
            builder.ConfigurePurchaseOrderLines();
            builder.ConfigurePurchaseRequestLines();
            builder.ConfigurePurchaseRequests();
            builder.ConfigurePurchaseUnsuitabilityReports();
            builder.ConfigureOperationUnsuitabilityReports();
            builder.ConfigureProductionTrackings();
            builder.ConfigureContractProductionTrackings();
            builder.ConfigureProductionTrackingHaltLines();
            builder.ConfigureForecasts();
            builder.ConfigureForecastLines();
            builder.ConfigureSalesPrices();
            builder.ConfigureSalesPriceLines();
            builder.ConfigurePurchasePrices();
            builder.ConfigurePurchasePriceLines();
            builder.ConfigureHaltReasons();
            builder.ConfigureUserGroups();
            builder.ConfigureUsers();
            builder.ConfigureStationInventories();
            builder.ConfigureFinalControlUnsuitabilityReports();
            builder.ConfigureMaintenancePeriods();
            builder.ConfigureMenus();
            builder.ConfigureMaintenanceInstructions();
            builder.ConfigureMaintenanceInstructionLines();
            builder.ConfigurePlannedMaintenanceLines();
            builder.ConfigurePlannedMaintenances();
            builder.ConfigureUnplannedMaintenanceLines();
            builder.ConfigureUnplannedMaintenances();
            builder.ConfigureByDateStockMovements();
            builder.ConfigureGrandTotalStockMovements();
            builder.ConfigureTechnicalDrawings();
            builder.ConfigureProductReferanceNumbers();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_IsSoftDelete)
            {
                ChangeTracker.SetAuditProperties();
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            if (_IsSoftDelete)
            {
                ChangeTracker.SetAuditProperties();
            }
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            if (_IsSoftDelete)
            {
                ChangeTracker.SetAuditProperties();
            }
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            if (_IsSoftDelete)
            {
                ChangeTracker.SetAuditProperties();
            }
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        #region Authentication DbSets

        #endregion


        #region DbSets
        public DbSet<Branches> Branches { get; set; }
        public DbSet<Periods> Periods { get; set; }
        public DbSet<Logs> Logs { get; set; }
        public DbSet<UnitSets> UnitSets { get; set; }
        public DbSet<StationGroups> StationGroups { get; set; }
        public DbSet<Stations> Stations { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<EquipmentRecords> EquipmentRecords { get; set; }
        public DbSet<ContractUnsuitabilityItems> ContractUnsuitabilityItems { get; set; }
        public DbSet<CalibrationVerifications> CalibrationVerifications { get; set; }
        public DbSet<CalibrationRecords> CalibrationRecords { get; set; }
        public DbSet<VsmSchemas> VsmSchemas { get; set; }
        public DbSet<Currencies> Currencies { get; set; }
        public DbSet<PaymentPlans> PaymentPlans { get; set; }
        public DbSet<OperationUnsuitabilityItems> OperationUnsuitabilityItems { get; set; }
        public DbSet<FinalControlUnsuitabilityItems> FinalControlUnsuitabilityItems { get; set; }
        public DbSet<ExchangeRates> ExchangeRates { get; set; }
        public DbSet<CurrentAccountCards> CurrentAccountCards { get; set; }
        public DbSet<ProductGroups> ProductGroups { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<ProductionOrderChangeItems> ProductionOrderChangeItems { get; set; }
        public DbSet<CustomerComplaintItems> CustomerComplaintItems { get; set; }
        public DbSet<PurchasingUnsuitabilityItems> PurchasingUnsuitabilityItems { get; set; }
        public DbSet<ShippingAdresses> ShippingAdresses { get; set; }
        public DbSet<Warehouses> Warehouses { get; set; }
        public DbSet<TemplateOperations> TemplateOperations { get; set; }
        public DbSet<TemplateOperationLines> TemplateOperationLines { get; set; }
        public DbSet<Routes> Routes { get; set; }
        public DbSet<RouteLines> RouteLines { get; set; }
        public DbSet<SalesPropositions> SalesPropositions { get; set; }
        public DbSet<SalesPropositionLines> SalesPropositionLines { get; set; }
        public DbSet<Calendars> Calendars { get; set; }
        public DbSet<CalendarLines> CalendarLines { get; set; }
        public DbSet<CalendarDays> CalendarDays { get; set; }
        public DbSet<Shifts> Shifts { get; set; }
        public DbSet<ShiftLines> ShiftLines { get; set; }
        public DbSet<SalesOrders> SalesOrders { get; set; }
        public DbSet<SalesOrderLines> SalesOrderLines { get; set; }
        public DbSet<ProductsOperations> ProductsOperations { get; set; }
        public DbSet<ProductsOperationLines> ProductsOperationLines { get; set; }
        public DbSet<BillsofMaterials> BillsofMaterials { get; set; }
        public DbSet<BillsofMaterialLines> BillsofMaterialLines { get; set; }
        public DbSet<ProductionOrders> ProductionOrders { get; set; }
        public DbSet<WorkOrders> WorkOrders { get; set; }
        public DbSet<PurchaseOrders> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderLines> PurchaseOrderLines { get; set; }
        public DbSet<Forecasts> Forecasts { get; set; }
        public DbSet<ForecastLines> ForecastLines { get; set; }
        public DbSet<PurchaseRequests> PurchaseRequests { get; set; }
        public DbSet<PurchaseRequestLines> PurchaseRequestLines { get; set; }
        public DbSet<PurchaseUnsuitabilityReports> PurchaseUnsuitabilityReports { get; set; }
        public DbSet<OperationUnsuitabilityReports> OperationUnsuitabilityReports { get; set; }
        public DbSet<ProductionTrackings> ProductionTrackings { get; set; }
        public DbSet<ContractProductionTrackings> ContractProductionTrackings { get; set; }
        public DbSet<ProductionTrackingHaltLines> ProductionTrackingHaltLines { get; set; }
        public DbSet<SalesPrices> SalesPrices { get; set; }
        public DbSet<SalesPriceLines> SalesPriceLines { get; set; }
        public DbSet<PurchasePrices> PurchasePrices { get; set; }
        public DbSet<PurchasePriceLines> PurchasePriceLines { get; set; }
        public DbSet<HaltReasons> HaltReasons { get; set; }
        public DbSet<UserGroups> UserGroups { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<FinalControlUnsuitabilityReports> FinalControlUnsuitabilityReports { get; set; }
        public DbSet<StationInventories> StationInventories { get; set; }
        public DbSet<Menus> Menus { get; set; }
        public DbSet<MaintenancePeriods> MaintenancePeriods { get; set; }
        public DbSet<MaintenanceInstructions> MaintenanceInstructions { get; set; }
        public DbSet<MaintenanceInstructionLines> MaintenanceInstructionLines { get; set; }
        public DbSet<PlannedMaintenances> PlannedMaintenances { get; set; }
        public DbSet<PlannedMaintenanceLines> PlannedMaintenanceLines { get; set; }
        public DbSet<UnplannedMaintenances> UnplannedMaintenances { get; set; }
        public DbSet<UnplannedMaintenanceLines> UnplannedMaintenanceLines { get; set; }
        public DbSet<ByDateStockMovements> ByDateStockMovements { get; set; }
        public DbSet<GrandTotalStockMovements> GrandTotalStockMovements { get; set; }
        public DbSet<TechnicalDrawings> TechnicalDrawings { get; set; }
        public DbSet<ProductReferanceNumbers> ProductReferanceNumbers { get; set; }

        #endregion
    }
}
