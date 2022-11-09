using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Authentication.Entities.Menus;
using Tsi.Authentication.Entities.RolePermissions;
using Tsi.Authentication.Entities.Roles;
using Tsi.Authentication.Entities.UserRoles;
using Tsi.Authentication.Entities.Users;
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
            builder.ConfigureUsers();
            builder.ConfigureUserRoles();
            builder.ConfigureRolePermissions();
            builder.ConfigureRoles();
            builder.ConfigureMenus();
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
            builder.ConfigureShifts();
            builder.ConfigureShiftLines();
            builder.ConfigureSalesOrders();
            builder.ConfigureSalesOrderLines();
            builder.ConfigureProductsOperations();
            builder.ConfigureProductsOperationLines();
            builder.ConfigureBillsofMaterials();
            builder.ConfigureBillsofMaterialLines();
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

        public DbSet<TsiUser> Users { get; set; }
        public DbSet<TsiUserRoles> UserRoles { get; set; }
        public DbSet<TsiRoles> Roles { get; set; }
        public DbSet<TsiMenus> Menus { get; set; }
        public DbSet<TsiRolePermissions> RolePermissions { get; set; }

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
        public DbSet<Shifts> Shifts { get; set; }
        public DbSet<ShiftLines> ShiftLines { get; set; }
        public DbSet<SalesOrders> SalesOrders { get; set; }
        public DbSet<SalesOrderLines> SalesOrderLines { get; set; }
        public DbSet<ProductsOperations> ProductsOperations { get; set; }
        public DbSet<ProductsOperationLines> ProductsOperationLines { get; set; }
        public DbSet<BillsofMaterials> BillsofMaterials { get; set; }
        public DbSet<BillsofMaterialLines> BillsofMaterialLines { get; set; }

        #endregion
    }
}
