using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using TsiErp.Entities.Entities.Product;
using TsiErp.Entities.Entities.ProductGroup;
using TsiErp.Entities.Entities.CustomerComplaintItem;
using TsiErp.Entities.Entities.ProductionOrderChangeItem;
using TsiErp.Entities.Entities.PurchasingUnsuitabilityItem;
using TsiErp.Entities.Entities.ShippingAdress;
using TsiErp.Entities.Entities.Route;
using TsiErp.Entities.Entities.RouteLine;
using TsiErp.Entities.Entities.Calendar;
using TsiErp.Entities.Entities.CalendarLine;
using TsiErp.Entities.Entities.Shift;
using TsiErp.Entities.Entities.ShiftLine;
using TsiErp.Entities.Entities.SalesOrder;
using TsiErp.Entities.Entities.SalesOrderLine;
using TsiErp.Entities.Entities.TemplateOperation;
using TsiErp.Entities.Entities.TemplateOperationLine;
using TsiErp.Entities.Entities.ProductsOperation;
using TsiErp.Entities.Entities.ProductsOperationLine;
using TsiErp.Entities.Entities.BillsofMaterial;
using TsiErp.Entities.Entities.BillsofMaterialLine;
using TsiErp.Entities.Entities.Forecast;
using TsiErp.Entities.Entities.ForecastLine;
using TsiErp.Entities.Entities.ProductionOrder;
using TsiErp.Entities.Entities.WorkOrder;
using TsiErp.Entities.Entities.CalendarDay;
using TsiErp.Entities.Entities.PurchaseOrder;
using TsiErp.Entities.Entities.PurchaseOrderLine;
using TsiErp.Entities.Entities.PurchaseRequest;
using TsiErp.Entities.Entities.PurchaseRequestLine;
using TsiErp.Entities.Entities.PurchaseUnsuitabilityReport;
using TsiErp.Entities.Entities.OperationUnsuitabilityReport;
using TsiErp.Entities.Entities.ProductionTracking;
using TsiErp.Entities.Entities.ProductionTrackingHaltLine;
using TsiErp.Entities.Entities.SalesPrice;
using TsiErp.Entities.Entities.SalesPriceLine;
using TsiErp.Entities.Entities.PurchasePrice;
using TsiErp.Entities.Entities.PurchasePriceLine;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityReport;
using TsiErp.Entities.Entities.HaltReason;
using TsiErp.Entities.Entities.UserGroup;
using TsiErp.Entities.Entities.StationInventory;
using TsiErp.Entities.Entities.Menu;
using TsiErp.Entities.Entities.ContractProductionTracking;
using TsiErp.Entities.Entities.User;
using TsiErp.Entities.Entities.MaintenancePeriod;
using TsiErp.Entities.Entities.MaintenanceInstruction;
using TsiErp.Entities.Entities.MaintenanceInstructionLine;
using TsiErp.Entities.Entities.PlannedMaintenance;
using TsiErp.Entities.Entities.PlannedMaintenanceLine;
using TsiErp.Entities.Entities.UnplannedMaintenance;
using TsiErp.Entities.Entities.UnplannedMaintenanceLine;
using TsiErp.Entities.Entities.GrandTotalStockMovement;
using TsiErp.Entities.Entities.ByDateStockMovement;
using TsiErp.Entities.Entities.TechnicalDrawing;
using TsiErp.Entities.Entities.ProductReferanceNumber;
using TsiErp.DataAccess.EntityFrameworkCore.Modeling;

namespace TsiErp.DataAccess.EntityFrameworkCore.Configurations
{
    public static class TsiErpDbContextModelBuilderExtensions
    {

        public static void ConfigureBranches(this ModelBuilder builder)
        {
            builder.Entity<Branches>(b =>
            {
                b.ToTable("Branches");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                //b.HasQueryFilter(x => !x.IsDeleted);

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.Description_).HasColumnType("nvarchar(MAX)");
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());

                b.HasIndex(x => x.Code);

            });
        }

        public static void ConfigureTechnicalDrawings(this ModelBuilder builder)
        {
            builder.Entity<TechnicalDrawings>(b =>
            {
                b.ToTable("TechnicalDrawings");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                //b.HasQueryFilter(x => !x.IsDeleted);

                b.Property(t => t.RevisionDate).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.RevisionNo).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.Drawer).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.DrawingNo).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.Description_).HasColumnType("nvarchar(MAX)");
                b.Property(t => t.DrawingDomain).HasColumnType("nvarchar(MAX)");
                b.Property(t => t.DrawingFilePath).HasColumnType("nvarchar(MAX)");
                b.Property(t => t.IsApproved).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.CustomerApproval).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.SampleApproval).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.ProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());

                b.HasIndex(x => x.RevisionNo);
                b.HasIndex(x => x.ProductID);


            });
        }

        public static void ConfigureProductReferanceNumbers(this ModelBuilder builder)
        {
            builder.Entity<ProductReferanceNumbers>(b =>
            {
                b.ToTable("ProductReferanceNumbers");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                //b.HasQueryFilter(x => !x.IsDeleted);

                b.Property(t => t.ReferanceNo).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.Description_).HasColumnType("nvarchar(MAX)");
                b.Property(t => t.ProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.CurrentAccountCardID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());

                b.HasIndex(x => x.ReferanceNo);
                b.HasIndex(x => x.ProductID);
                b.HasIndex(x => x.CurrentAccountCardID);

            });
        }

        public static void ConfigureMaintenancePeriods(this ModelBuilder builder)
        {
            builder.Entity<MaintenancePeriods>(b =>
            {
                b.ToTable("MaintenancePeriods");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                //b.HasQueryFilter(x => !x.IsDeleted);

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.Description_).HasColumnType("nvarchar(MAX)");
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsDaily).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.PeriodTime).HasColumnType(SqlDbType.Decimal.ToString());

                b.HasIndex(x => x.Code);

            });
        }

        public static void ConfigurePeriods(this ModelBuilder builder)
        {
            builder.Entity<Periods>(b =>
            {
                b.ToTable("Periods");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.Description_).HasColumnType("nvarchar(MAX)");
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.BranchID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());

                b.HasIndex(x => x.Code);

            });
        }

        public static void ConfigureLogs(this ModelBuilder builder)
        {
            builder.Entity<Logs>(b =>
            {
                b.ToTable("Logs");


                b.Property(t => t.Id).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Date_).IsRequired().HasColumnType(SqlDbType.DateTime.ToString()).HasMaxLength(200);
                b.Property(t => t.MethodName_).IsRequired().HasColumnType("nvarchar(MAX)");
                b.Property(t => t.BeforeValues).IsRequired().HasColumnType("sql_variant").HasMaxLength(5000);
                b.Property(t => t.AfterValues).IsRequired().HasColumnType("sql_variant").HasMaxLength(5000);
                b.Property(t => t.DiffValues).IsRequired().HasColumnType("sql_variant").HasMaxLength(5000);
                b.Property(t => t.LogLevel_).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.UserId).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.RecordId).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());

            });
        }

        public static void ConfigureUnitSets(this ModelBuilder builder)
        {
            builder.Entity<UnitSets>(b =>
            {
                b.ToTable("UnitSets");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());

                b.HasIndex(x => x.Code);


            });
        }

        public static void ConfigureStationGroups(this ModelBuilder builder)
        {
            builder.Entity<StationGroups>(b =>
            {
                b.ToTable("StationGroups");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.TotalEmployees).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());

                b.HasIndex(x => x.Code);
            });
        }

        public static void ConfigureStations(this ModelBuilder builder)
        {
            builder.Entity<Stations>(b =>
            {
                b.ToTable("Stations");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.Brand).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.Model).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.Capacity).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(85);
                b.Property(t => t.KWA).HasColumnType("decimal(18, 2)");
                b.Property(t => t.GroupID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.X).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.Y).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.AreaCovered).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.UsageArea).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.Amortization).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.MachineCost).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.Shift).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.ShiftWorkingTime).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.PowerFactor).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.WorkSafetyInstruction).HasColumnType("varbinary(MAX)");
                b.Property(t => t.UsageInstruction).HasColumnType("varbinary(MAX)");
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsContract).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsFixtures).HasColumnType(SqlDbType.Bit.ToString());

                b.HasIndex(x => x.Code);
            });
        }

        public static void ConfigureEmployees(this ModelBuilder builder)
        {
            builder.Entity<Employees>(b =>
            {
                b.ToTable("Employees");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.Surname).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(100);
                b.Property(t => t.DepartmentID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.IDnumber).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(11);
                b.Property(t => t.Birthday).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.BloodType).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.Address).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.District).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(100);
                b.Property(t => t.City).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(100);
                b.Property(t => t.HomePhone).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(100);
                b.Property(t => t.CellPhone).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(100);
                b.Property(t => t.Email).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.Image).HasColumnType("varbinary(max)");
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());

                b.HasIndex(x => x.Code);
            });
        }

        public static void ConfigureEquipmentRecords(this ModelBuilder builder)
        {
            builder.Entity<EquipmentRecords>(b =>
            {
                b.ToTable("EquipmentRecords");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();


                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.MeasuringRange).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(150);
                b.Property(t => t.MeasuringAccuracy).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.Department).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Responsible).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(150);
                b.Property(t => t.EquipmentSerialNo).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(250);
                b.Property(t => t.RecordDate).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.Cancel).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.CancellationDate).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.CancellationReason).HasColumnType("nvarchar(MAX)");

                b.HasIndex(x => x.Code);


            });
        }

        public static void ConfigureDepartments(this ModelBuilder builder)
        {
            builder.Entity<Departments>(b =>
            {
                b.ToTable("Departments");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());

                b.HasIndex(x => x.Code);
            });
        }

        public static void ConfigureContractUnsuitabilityItems(this ModelBuilder builder)
        {
            builder.Entity<ContractUnsuitabilityItems>(b =>
            {
                b.ToTable("ContractUnsuitabilityItems");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.Image).HasColumnType("varbinary(max)");
                b.Property(t => t.Detection).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.Severity).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.StaProductivityAnalysis).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.PerProductivityAnalysis).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());


                b.HasIndex(x => x.Code);
            });
        }

        public static void ConfigureCalibrationVerifications(this ModelBuilder builder)
        {
            builder.Entity<CalibrationVerifications>(b =>
            {
                b.ToTable("CalibrationVerifications");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();



                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.ReceiptNo).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.EquipmentID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Date).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.NextControl).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.InfinitiveCertificateNo).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.Result).HasColumnType("nvarchar(MAX)");

                b.HasIndex(x => x.Code);

            });
        }

        public static void ConfigureCalibrationRecords(this ModelBuilder builder)
        {
            builder.Entity<CalibrationRecords>(b =>
            {
                b.ToTable("CalibrationRecords");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();



                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.ReceiptNo).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.EquipmentID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Date).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.NextControl).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.InfinitiveCertificateNo).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.Result).HasColumnType("nvarchar(MAX)");

                b.HasIndex(x => x.Code);

            });
        }

        public static void ConfigureVsmSchemas(this ModelBuilder builder)
        {
            builder.Entity<VsmSchemas>(b =>
            {
                
                b.ToTable("VsmSchemas");
                b.ConfigureByConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(250);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(250);
                b.Property(t => t.VSMSchema).IsRequired().HasColumnType("sql_variant").HasMaxLength(25000000);
                b.Property(t => t.Name).IsRequired().HasColumnType("nvarchar(max)");

            });
        }

        public static void ConfigureSalesPropositions(this ModelBuilder builder)
        {
            builder.Entity<SalesPropositions>(b =>
            {
                b.ToTable("SalesPropositions");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.FicheNo).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Description_).HasColumnType("nvarchar(max)");
                b.Property(t => t.Date_).IsRequired().HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.Time_).HasColumnType("time(7)");
                b.Property(t => t.ExchangeRate).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.SpecialCode).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(201);
                b.Property(t => t.PropositionRevisionNo).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.RevisionDate).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.RevisionTime).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(8);
                b.Property(t => t.SalesPropositionState).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.CurrencyID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.WarehouseID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.LinkedSalesPropositionID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.PaymentPlanID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.BranchID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.CurrentAccountCardID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.GrossAmount).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalVatExcludedAmount).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalVatAmount).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalDiscountAmount).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.NetAmount).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.ValidityDate_).IsRequired().HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.ShippingAdressID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());



                b.HasIndex(x => x.FicheNo); 
                b.HasIndex(x => x.CurrentAccountCardID);
                b.HasIndex(x => x.BranchID);
                b.HasIndex(x => x.WarehouseID);
            });
        }

        public static void ConfigureSalesPropositionLines(this ModelBuilder builder)
        {
            builder.Entity<SalesPropositionLines>(b =>
            {
                b.ToTable("SalesPropositionLines");
                b.ConfigureByConvention();

                b.Property(t => t.SalesPropositionID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.LineNr).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.ProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.UnitSetID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Quantity).HasColumnType(SqlDbType.Decimal.ToString()).HasPrecision(18,6);
                b.Property(t => t.UnitPrice).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.DiscountRate).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.DiscountAmount).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.LineAmount).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.LineTotalAmount).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.LineDescription).HasColumnType("nvarchar(MAX)");
                b.Property(t => t.VATrate).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.VATamount).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.PaymentPlanID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ExchangeRate).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.SalesPropositionLineState).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.OrderConversionDate).HasColumnType(SqlDbType.DateTime.ToString());
                b.HasIndex(x => x.SalesPropositionID);
                b.HasIndex(x => x.ProductID);
            });
        }

        public static void ConfigureExchangeRates(this ModelBuilder builder)
        {
            builder.Entity<ExchangeRates>(b =>
            {
                b.ToTable("ExchangeRates");
                b.ConfigureByConvention();

                b.Property(t => t.CurrencyID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Date).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.BuyingRate).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.SaleRate).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.EffectiveBuyingRate).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.EffectiveSaleRate).HasColumnType(SqlDbType.Decimal.ToString());

                b.HasIndex(x => x.CurrencyID);
                b.HasIndex(x => x.Date);


            });
        }

        public static void ConfigureCurrentAccountCards(this ModelBuilder builder)
        {
            builder.Entity<CurrentAccountCards>(b =>
            {
                b.ToTable("CurrentAccountCards");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();



                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.SupplierNo).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.ShippingAddress).HasColumnType("nvarchar(MAX)");
                b.Property(t => t.Type).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.Address1).HasColumnType("nvarchar(MAX)");
                b.Property(t => t.Address2).HasColumnType("nvarchar(MAX)");
                b.Property(t => t.District).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.City).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.Country).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.PostCode).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.Tel1).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.Tel2).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.Fax).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.Responsible).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.Email).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.Web).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.PrivateCode1).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.PrivateCode2).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.PrivateCode3).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.PrivateCode4).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.PrivateCode5).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.SoleProprietorship).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IDnumber).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(11);
                b.Property(t => t.TaxAdministration).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(75);
                b.Property(t => t.TaxNumber).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(10);
                b.Property(t => t.CoatingCustomer).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.SaleContract).HasColumnType("nvarchar(MAX)");
                b.Property(t => t.PlusPercentage).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.CurrencyID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Supplier).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.ContractSupplier).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());

                b.HasIndex(x => x.Code);

            });
        }

        public static void ConfigureProductCodes(this ModelBuilder builder)
        {
            builder.Entity<Products>(b =>
            {
                b.ToTable("Products");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();



                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.SupplyForm).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.ProductSize).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.GTIP).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.SawWastage).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.Confirmation).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.TechnicalConfirmation).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.ProductType).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.ProductDescription).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(500);
                b.Property(t => t.ProductGrpID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ManufacturerCode).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.SaleVAT).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.PurchaseVAT).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.UnitSetID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.FeatureSetID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.EnglishDefinition).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(201);
                b.Property(t => t.ExportCatNo).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.OemRefNo).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.OemRefNo2).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.OemRefNo3).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.PlannedWastage).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.CoatingWeight).HasColumnType(SqlDbType.Decimal.ToString());


                b.HasIndex(x => x.Code);
            });
        }

        public static void ConfigureProductGroups(this ModelBuilder builder)
        {
            builder.Entity<ProductGroups>(b =>
            {
                b.ToTable("ProductGroups");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();



                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());

                b.HasIndex(x => x.Code);

            });
        }

        public static void ConfigureCustomerComplaintItems(this ModelBuilder builder)
        {
            builder.Entity<CustomerComplaintItems>(b =>
            {
                b.ToTable("CustomerComplaintItems");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.Image).HasColumnType("varbinary(max)");
                b.Property(t => t.Detection).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.Severity).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.StaProductivityAnalysis).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.PerProductivityAnalysis).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());


                b.HasIndex(x => x.Code);
            });
        }

        public static void ConfigureProductionOrderChangeItems(this ModelBuilder builder)
        {
            builder.Entity<ProductionOrderChangeItems>(b =>
            {
                b.ToTable("ProductionOrderChangeItems");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.Image).HasColumnType("varbinary(max)");
                b.Property(t => t.Detection).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.Severity).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.StaProductivityAnalysis).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.PerProductivityAnalysis).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());


                b.HasIndex(x => x.Code);
            });
        }

        public static void ConfigurePurchasingUnsuitabilityItems(this ModelBuilder builder)
        {
            builder.Entity<PurchasingUnsuitabilityItems>(b =>
            {
                b.ToTable("PurchasingUnsuitabilityItems");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.Image).HasColumnType("varbinary(max)");
                b.Property(t => t.Detection).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.Severity).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.StaProductivityAnalysis).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.PerProductivityAnalysis).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());


                b.HasIndex(x => x.Code);
            });
        }

        public static void ConfigureShippingAdresses(this ModelBuilder builder)
        {
            builder.Entity<ShippingAdresses>(b =>
            {
                b.ToTable("ShippingAdresses");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();



                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.CustomerCardID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Adress1).IsRequired().HasColumnType("nvarchar(MAX)");
                b.Property(t => t.Adress2).HasColumnType("nvarchar(MAX)");
                b.Property(t => t.District).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.City).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.Country).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.Phone).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(100);
                b.Property(t => t.EMail).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.Fax).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(100);
                b.Property(t => t.PostCode).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t._Default).HasColumnType(SqlDbType.Decimal.ToString());

                b.HasIndex(x => x.Code);

            });
        }

        public static void ConfigureCurrencies(this ModelBuilder builder)
        {
            builder.Entity<Currencies>(b =>
            {
                b.ToTable("Currencies");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());

                b.HasIndex(x => x.Code);


            });
        }

        public static void ConfigurePaymentPlans(this ModelBuilder builder)
        {
            builder.Entity<PaymentPlans>(b =>
            {
                b.ToTable("PaymentPlans");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.Days_).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.DelayMaturityDifference).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());

                b.HasIndex(x => x.Code);
            });
        }

        public static void ConfigureOperationUnsuitabilityItems(this ModelBuilder builder)
        {
            builder.Entity<OperationUnsuitabilityItems>(b =>
            {
                b.ToTable("OperationUnsuitabilityItems");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.Image).HasColumnType("varbinary(max)");
                b.Property(t => t.Detection).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.Severity).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.StaProductivityAnalysis).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.PerProductivityAnalysis).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());


                b.HasIndex(x => x.Code);
            });
        }

        public static void ConfigureFinalControlUnsuitabilityItems(this ModelBuilder builder)
        {
            builder.Entity<FinalControlUnsuitabilityItems>(b =>
            {
                b.ToTable("FinalControlUnsuitabilityItems");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.Image).HasColumnType("varbinary(max)");
                b.Property(t => t.Detection).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.Severity).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.StaProductivityAnalysis).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.PerProductivityAnalysis).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());


                b.HasIndex(x => x.Code);
            });
        }

        public static void ConfigureWarehouses(this ModelBuilder builder)
        {
            builder.Entity<Warehouses>(b =>
            {
                b.ToTable("Warehouses");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());

                b.HasIndex(x => x.Code);

            });
        }

        public static void ConfigureTemplateOperations(this ModelBuilder builder)
        {
            builder.Entity<TemplateOperations>(b =>
            {
                b.ToTable("TemplateOperations");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                //b.HasQueryFilter(x => !x.IsDeleted);

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.WorkCenterID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());

                b.HasIndex(x => x.Code);

            });
        }

        public static void ConfigureRoutes(this ModelBuilder builder)
        {
            builder.Entity<Routes>(b =>
            {
                b.ToTable("Routes");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                //b.HasQueryFilter(x => !x.IsDeleted);

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.ProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductionStart).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.Approval).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.TechnicalApproval).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());

                b.HasIndex(x => x.Code);
            });
        }

        public static void ConfigureTemplateOperationLines(this ModelBuilder builder)
        {
            builder.Entity<TemplateOperationLines>(b =>
            {
                b.ToTable("TemplateOperationLines");
                b.ConfigureByConvention();

                b.Property(t => t.StationID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.TemplateOperationID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Priority).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.ProcessQuantity).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.AdjustmentAndControlTime).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.OperationTime).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.LineNr).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.Alternative).HasColumnType(SqlDbType.Bit.ToString());

                b.HasIndex(x => x.TemplateOperationID);
            });
        }

        public static void ConfigureRouteLines(this ModelBuilder builder)
        {
            builder.Entity<RouteLines>(b =>
            {
                b.ToTable("RouteLines");
                b.ConfigureByConvention();

               
                b.Property(t => t.RouteID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductsOperationID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductionPoolID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductionPoolDescription).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.AdjustmentAndControlTime).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.OperationTime).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.Priority).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.LineNr).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.OperationPicture).HasColumnType("varbinary(max)");


                b.HasIndex(x => x.RouteID);
                b.HasIndex(x => x.ProductID);
                b.HasIndex(x => x.ProductsOperationID);
            });
        }

        public static void ConfigureCalendars(this ModelBuilder builder)
        {
            builder.Entity<Calendars>(b =>
            {
                b.ToTable("Calendars");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t._Description).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.Year).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.IsPlanned).IsRequired().HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.TotalDays).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.AvailableDays).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.OfficialHolidayDays).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());


                b.HasIndex(x => x.Code);

            });
        }

        public static void ConfigureCalendarLines(this ModelBuilder builder)
        {
            builder.Entity<CalendarLines>(b =>
            {
                b.ToTable("CalendarLines");
                b.ConfigureByConvention();

                b.Property(t => t.AvailableTime).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.PlannedHaltTimes).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.CalendarID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ShiftID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ShiftOverTime).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.ShiftTime).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.StationID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());

                b.HasIndex(x => x.StationID);
                b.HasIndex(x => x.CalendarID);
                b.HasIndex(x => x.ShiftID);


            });
        }

        public static void ConfigureCalendarDays(this ModelBuilder builder)
        {
            builder.Entity<CalendarDays>(b =>
            {
                b.ToTable("CalendarDays");
                b.ConfigureByConvention();

                b.Property(t => t.Date_).IsRequired().HasColumnType(SqlDbType.Date.ToString());
                b.Property(t => t.CalendarID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.CalendarDayStateEnum).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.ColorCode).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);

                b.HasIndex(x => x.CalendarID);


            });
        }

        public static void ConfigureShifts(this ModelBuilder builder)
        {
            builder.Entity<Shifts>(b =>
            {
                b.ToTable("Shifts");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                //b.HasQueryFilter(x => !x.IsDeleted);

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.TotalWorkTime).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalBreakTime).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.NetWorkTime).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.Overtime).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.ShiftOrder).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());

                b.HasIndex(x => x.Code);

            });
        }

        public static void ConfigureShiftLines(this ModelBuilder builder)
        {
            builder.Entity<ShiftLines>(b =>
            {
                b.ToTable("ShiftLines");
                b.ConfigureByConvention();

                b.Property(t => t.ShiftID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.StartHour).IsRequired().HasColumnType("time(7)");
                b.Property(t => t.EndHour).IsRequired().HasColumnType("time(7)");
                b.Property(t => t.Coefficient).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.Type).IsRequired().HasColumnType(SqlDbType.Int.ToString());


                b.HasIndex(x => x.ShiftID);

            });
        }

        public static void ConfigureSalesOrders(this ModelBuilder builder)
        {
            builder.Entity<SalesOrders>(b =>
            {
                b.ToTable("SalesOrders");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.FicheNo).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Description_).HasColumnType("nvarchar(max)");
                b.Property(t => t.Date_).IsRequired().HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.Time_).HasColumnType("time(7)");
                b.Property(t => t.ExchangeRate).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.SpecialCode).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(201);
                b.Property(t => t.LinkedSalesPropositionID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.SalesOrderState).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.CurrencyID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.WarehouseID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.LinkedSalesPropositionID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.PaymentPlanID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.BranchID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.CurrentAccountCardID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.GrossAmount).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalVatExcludedAmount).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalVatAmount).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalDiscountAmount).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.NetAmount).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.WorkOrderCreationDate).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.ShippingAdressID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());

                b.HasIndex(x => x.FicheNo);
                b.HasIndex(x => x.CurrentAccountCardID);
                b.HasIndex(x => x.BranchID);
                b.HasIndex(x => x.WarehouseID);
            });
        }

        public static void ConfigureSalesOrderLines(this ModelBuilder builder)
        {
            builder.Entity<SalesOrderLines>(b =>
            {
                b.ToTable("SalesOrderLines");
                b.ConfigureByConvention();

                b.Property(t => t.SalesOrderID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.LineNr).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.ProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.UnitSetID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.LikedPropositionLineID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Quantity).HasColumnType(SqlDbType.Decimal.ToString()).HasPrecision(18, 6);
                b.Property(t => t.UnitPrice).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.DiscountRate).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.DiscountAmount).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.LineAmount).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.LineTotalAmount).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.LineDescription).HasColumnType("nvarchar(MAX)");
                b.Property(t => t.VATrate).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.VATamount).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.PaymentPlanID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ExchangeRate).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.SalesOrderLineStateEnum).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.WorkOrderCreationDate).HasColumnType(SqlDbType.DateTime.ToString());
                b.HasIndex(x => x.SalesOrderID);
                b.HasIndex(x => x.ProductID);
            });
        }

        public static void ConfigurePurchaseOrders(this ModelBuilder builder)
        {
            builder.Entity<PurchaseOrders>(b =>
            {
                b.ToTable("PurchaseOrders");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.FicheNo).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Description_).HasColumnType("nvarchar(max)");
                b.Property(t => t.Date_).IsRequired().HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.Time_).HasColumnType("time(7)");
                b.Property(t => t.ExchangeRate).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.SpecialCode).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(201);
                b.Property(t => t.LinkedPurchaseRequestID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.PurchaseOrderState).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.CurrencyID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.WarehouseID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.PaymentPlanID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.BranchID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.CurrentAccountCardID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.GrossAmount).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalVatExcludedAmount).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalVatAmount).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalDiscountAmount).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.NetAmount).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.WorkOrderCreationDate).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.ShippingAdressID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductionOrderID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());

                b.HasIndex(x => x.FicheNo);
                b.HasIndex(x => x.CurrentAccountCardID);
                b.HasIndex(x => x.BranchID);
                b.HasIndex(x => x.WarehouseID);
            });
        }

        public static void ConfigurePurchaseOrderLines(this ModelBuilder builder)
        {
            builder.Entity<PurchaseOrderLines>(b =>
            {
                b.ToTable("PurchaseOrderLines");
                b.ConfigureByConvention();

                b.Property(t => t.PurchaseOrderID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.LineNr).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.ProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.UnitSetID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.LinkedPurchaseRequestID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.LikedPurchaseRequestLineID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Quantity).HasColumnType(SqlDbType.Decimal.ToString()).HasPrecision(18, 6);
                b.Property(t => t.UnitPrice).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.DiscountRate).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.DiscountAmount).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.LineAmount).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.LineTotalAmount).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.LineDescription).HasColumnType("nvarchar(MAX)");
                b.Property(t => t.VATrate).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.VATamount).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.PaymentPlanID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductionOrderID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ExchangeRate).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.PurchaseOrderLineStateEnum).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.WorkOrderCreationDate).HasColumnType(SqlDbType.DateTime.ToString());

                b.HasIndex(x => x.PurchaseOrderID);
                b.HasIndex(x => x.ProductID);
            });
        }

        public static void ConfigureProductsOperations(this ModelBuilder builder)
        {
            builder.Entity<ProductsOperations>(b =>
            {
                b.ToTable("ProductsOperations");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.ProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.TemplateOperationID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.WorkCenterID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());

                b.HasIndex(x => x.Code);

            });
        }

        public static void ConfigureProductsOperationLines(this ModelBuilder builder)
        {
            builder.Entity<ProductsOperationLines>(b =>
            {
                b.ToTable("ProductsOperationLines");
                b.ConfigureByConvention();

                b.Property(t => t.StationID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductsOperationID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Priority).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.ProcessQuantity).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.AdjustmentAndControlTime).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.OperationTime).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.LineNr).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.Alternative).HasColumnType(SqlDbType.Bit.ToString());

                b.HasIndex(x => x.ProductsOperationID);
            });
        }

        public static void ConfigureBillsofMaterials(this ModelBuilder builder)
        {
            builder.Entity<BillsofMaterials>(b =>
            {
                b.ToTable("BillsofMaterials");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                //b.HasQueryFilter(x => !x.IsDeleted);

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t._Description).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.FinishedProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());

                b.HasIndex(x => x.Code);

            });
        }

        public static void ConfigureBillsofMaterialLines(this ModelBuilder builder)
        {
            builder.Entity<BillsofMaterialLines>(b =>
            {
                b.ToTable("BillsofMaterialLines");
                b.ConfigureByConvention();

                b.Property(t => t.BoMID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.FinishedProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.UnitSetID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.MaterialType).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.LineNr).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.ProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Quantity).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.Size).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t._Description).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);


                b.HasIndex(x => x.BoMID);
                b.HasIndex(x => x.ProductID);
                b.HasIndex(x => x.UnitSetID);
            });
        }

        public static void ConfigureProductionOrders(this ModelBuilder builder)
        {
            builder.Entity<ProductionOrders>(b =>
            {
                b.ToTable("ProductionOrders");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                //b.HasQueryFilter(x => !x.IsDeleted);

                b.Property(t => t.FicheNo).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Date_).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.Cancel_).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.ProductionOrderState).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.StartDate).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.EndDate).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.PlannedQuantity).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.ProducedQuantity).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.Description_).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.CustomerOrderNo).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.OrderID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.OrderLineID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.FinishedProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.LinkedProductID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.UnitSetID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.BOMID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.RouteID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductTreeID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductTreeLineID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.PropositionID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.PropositionLineID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.CurrentAccountID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.LinkedProductionOrderID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());

                b.HasIndex(x => x.FicheNo);
                b.HasIndex(x => x.OrderID);
                b.HasIndex(x => x.FinishedProductID);
                b.HasIndex(x => x.BOMID);
                b.HasIndex(x => x.CurrentAccountID);

            });
        }

        public static void ConfigureWorkOrders(this ModelBuilder builder)
        {
            builder.Entity<WorkOrders>(b =>
            {
                b.ToTable("WorkOrders");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                //b.HasQueryFilter(x => !x.IsDeleted);

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.WorkOrderNo).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.IsCancel).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.WorkOrderState).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.AdjustmentAndControlTime).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.OperationTime).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.PlannedQuantity).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.ProducedQuantity).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.OccuredStartDate).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.OccuredFinishDate).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.LineNr).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.LinkedWorkOrderID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductionOrderID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.PropositionID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.RouteID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductsOperationID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.StationID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.StationGroupID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.CurrentAccountCardID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                

                b.HasIndex(x => x.Code);
                b.HasIndex(x => x.ProductionOrderID);
                b.HasIndex(x => x.ProductID);
                b.HasIndex(x => x.ProductsOperationID);
                b.HasIndex(x => x.CurrentAccountCardID);

            });
        }

        public static void ConfigurePurchaseRequests(this ModelBuilder builder)
        {
            builder.Entity<PurchaseRequests>(b =>
            {
                b.ToTable("PurchaseRequests");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.FicheNo).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Description_).HasColumnType("nvarchar(max)");
                b.Property(t => t.Date_).IsRequired().HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.Time_).HasColumnType("time(7)");
                b.Property(t => t.ExchangeRate).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.SpecialCode).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(201);
                b.Property(t => t.PropositionRevisionNo).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.RevisionDate).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.RevisionTime).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(8);
                b.Property(t => t.PurchaseRequestState).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.CurrencyID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.WarehouseID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.LinkedPurchaseRequestID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductionOrderID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.PaymentPlanID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.BranchID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.CurrentAccountCardID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.GrossAmount).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalVatExcludedAmount).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalVatAmount).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalDiscountAmount).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.NetAmount).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.ValidityDate_).IsRequired().HasColumnType(SqlDbType.DateTime.ToString());

                b.HasIndex(x => x.FicheNo);
                b.HasIndex(x => x.CurrentAccountCardID);
                b.HasIndex(x => x.BranchID);
                b.HasIndex(x => x.WarehouseID);
            });
        }

        public static void ConfigurePurchaseRequestLines(this ModelBuilder builder)
        {
            builder.Entity<PurchaseRequestLines>(b =>
            {
                b.ToTable("PurchaseRequestLines");
                b.ConfigureByConvention();

                b.Property(t => t.PurchaseRequestID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.LineNr).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.ProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.UnitSetID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Quantity).HasColumnType(SqlDbType.Decimal.ToString()).HasPrecision(18, 6);
                b.Property(t => t.UnitPrice).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.DiscountRate).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.DiscountAmount).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.LineAmount).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.LineTotalAmount).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.LineDescription).HasColumnType("nvarchar(MAX)");
                b.Property(t => t.VATrate).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.VATamount).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.PaymentPlanID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductionOrderID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ExchangeRate).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.PurchaseRequestLineState).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.OrderConversionDate).HasColumnType(SqlDbType.DateTime.ToString());

                b.HasIndex(x => x.PurchaseRequestID);
                b.HasIndex(x => x.ProductID);
            });
        }
        public static void ConfigureProductionTrackings(this ModelBuilder builder)
        {
            builder.Entity<ProductionTrackings>(b =>
            {
                b.ToTable("ProductionTrackings");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.WorkOrderID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProducedQuantity).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.OperationTime).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.OperationStartDate).IsRequired().HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.OperationStartTime).IsRequired().HasColumnType("time(7)");
                b.Property(t => t.OperationEndDate).IsRequired().HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.OperationEndTime).IsRequired().HasColumnType("time(7)");
                b.Property(t => t.HaltTime).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.AdjustmentTime).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.PlannedQuantity).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.IsFinished).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.StationID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.EmployeeID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ShiftID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());

                b.HasIndex(x => x.WorkOrderID);
                b.HasIndex(x => x.StationID);
                b.HasIndex(x => x.EmployeeID);
                b.HasIndex(x => x.ShiftID);
            });
        }
        public static void ConfigureContractProductionTrackings(this ModelBuilder builder)
        {
            builder.Entity<ContractProductionTrackings>(b =>
            {
                b.ToTable("ContractProductionTrackings");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.WorkOrderID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProducedQuantity).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.OperationTime).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.OperationStartDate).IsRequired().HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.OperationStartTime).HasColumnType("time(7)");
                b.Property(t => t.OperationEndDate).IsRequired().HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.OperationEndTime).HasColumnType("time(7)");
                b.Property(t => t.PlannedQuantity).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.IsFinished).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.StationID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.EmployeeID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ShiftID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.CurrentAccountCardID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());

                b.HasIndex(x => x.WorkOrderID);
                b.HasIndex(x => x.ProductID);
                b.HasIndex(x => x.StationID);
                b.HasIndex(x => x.EmployeeID);
                b.HasIndex(x => x.ShiftID);
                b.HasIndex(x => x.CurrentAccountCardID);
            });
        }

        public static void ConfigureProductionTrackingHaltLines(this ModelBuilder builder)
        {
            builder.Entity<ProductionTrackingHaltLines>(b =>
            {
                b.ToTable("ProductionTrackingHaltLines");
                b.ConfigureByConvention();

                b.Property(t => t.ProductionTrackingID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.HaltID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.HaltTime).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());

                b.HasIndex(x => x.ProductionTrackingID);
                b.HasIndex(x => x.HaltID);
            });
        }

        public static void ConfigureHaltReasons(this ModelBuilder builder)
        {
            builder.Entity<HaltReasons>(b =>
            {
                b.ToTable("HaltReasons");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.IsPlanned).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsMachine).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsOperator).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsManagement).HasColumnType(SqlDbType.Bit.ToString());

            });
        }

        public static void ConfigurePurchaseUnsuitabilityReports(this ModelBuilder builder)
        {
            builder.Entity<PurchaseUnsuitabilityReports>(b =>
            {
                b.ToTable("PurchaseUnsuitabilityReports");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.FicheNo).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Description_).HasColumnType("nvarchar(max)");
                b.Property(t => t.Date_).IsRequired().HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.OrderID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.PartyNo).HasColumnType("nvarchar(max)");
                b.Property(t => t.CurrentAccountCardID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.IsUnsuitabilityWorkOrder).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsReject).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsToBeUsedAs).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsContactSupplier).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsCorrection).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.UnsuitableAmount).HasColumnType(SqlDbType.Decimal.ToString());


                b.HasIndex(x => x.FicheNo);
                b.HasIndex(x => x.CurrentAccountCardID);
                b.HasIndex(x => x.OrderID);
                b.HasIndex(x => x.ProductID);
            });
        }

        public static void ConfigureOperationUnsuitabilityReports(this ModelBuilder builder)
        {
            builder.Entity<OperationUnsuitabilityReports>(b =>
            {
                b.ToTable("OperationUnsuitabilityReports");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.FicheNo).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Description_).HasColumnType("nvarchar(max)");
                b.Property(t => t.Date_).IsRequired().HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.IsUnsuitabilityWorkOrder).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsScrap).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.ControlFormDeclaration).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.IsToBeUsedAs).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsCorrection).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.WorkOrderID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.StationID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.StationGroupID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.EmployeeID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductionOrderID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.OperationID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());


                b.HasIndex(x => x.FicheNo);
                b.HasIndex(x => x.OperationID);
                b.HasIndex(x => x.WorkOrderID);
                b.HasIndex(x => x.ProductionOrderID);
                b.HasIndex(x => x.ProductID);
            });
        }

        public static void ConfigureForecasts(this ModelBuilder builder)
        {
            builder.Entity<Forecasts>(b =>
            {
                b.ToTable("Forecasts");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Description_).HasColumnType("nvarchar(max)");
                b.Property(t => t.CreationDate_).IsRequired().HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.ValidityStartDate).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.ValidityEndDate).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.CurrentAccountCardID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.BranchID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.PeriodID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Total).HasColumnType(SqlDbType.Decimal.ToString()).HasPrecision(18, 6);
                b.Property(t => t.LineNumber).HasColumnType(SqlDbType.Int.ToString());
                
                b.HasIndex(x => x.Code);
                b.HasIndex(x => x.CurrentAccountCardID);
                b.HasIndex(x => x.BranchID);
                b.HasIndex(x => x.PeriodID);
            });
        }

        public static void ConfigureForecastLines(this ModelBuilder builder)
        {
            builder.Entity<ForecastLines>(b =>
            {
                b.ToTable("ForecastLines");
                b.ConfigureByConvention();

                b.Property(t => t.ForecastID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.LineNr).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.Amount).HasColumnType(SqlDbType.Decimal.ToString()).HasPrecision(18, 6);
                b.Property(t => t.CustomerProductCode).HasColumnType("nvarchar(MAX)");

                b.HasIndex(x => x.ForecastID);
                b.HasIndex(x => x.ProductID);
            });
        }

        public static void ConfigureSalesPrices(this ModelBuilder builder)
        {
            builder.Entity<SalesPrices>(b =>
            {
                b.ToTable("SalesPrices");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.StartDate).IsRequired().HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.EndDate).IsRequired().HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.CurrencyID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.BranchID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.WarehouseID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.CurrentAccountCardID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsApproved).HasColumnType(SqlDbType.Bit.ToString());

                b.HasIndex(x => x.Code);
                b.HasIndex(x => x.CurrencyID);
                b.HasIndex(x => x.CurrentAccountCardID);
                b.HasIndex(x => x.BranchID);
                b.HasIndex(x => x.WarehouseID);
            });
        }

        public static void ConfigureSalesPriceLines(this ModelBuilder builder)
        {
            builder.Entity<SalesPriceLines>(b =>
            {
                b.ToTable("SalesPriceLines");
                b.ConfigureByConvention();

                b.Property(t => t.SalesPriceID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.CurrencyID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.CurrentAccountCardID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Linenr).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.Price).HasColumnType(SqlDbType.Decimal.ToString()).HasPrecision(18, 6);
                b.Property(t => t.StartDate).IsRequired().HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.EndDate).IsRequired().HasColumnType(SqlDbType.DateTime.ToString());

                b.HasIndex(x => x.SalesPriceID);
                b.HasIndex(x => x.ProductID);
                b.HasIndex(x => x.CurrencyID);
                b.HasIndex(x => x.CurrentAccountCardID);
            });
        }

        public static void ConfigurePurchasePrices(this ModelBuilder builder)
        {
            builder.Entity<PurchasePrices>(b =>
            {
                b.ToTable("PurchasePrices");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.StartDate).IsRequired().HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.EndDate).IsRequired().HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.CurrencyID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.BranchID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.WarehouseID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.CurrentAccountCardID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsApproved).HasColumnType(SqlDbType.Bit.ToString());


                b.HasIndex(x => x.Code);
                b.HasIndex(x => x.CurrencyID);
                b.HasIndex(x => x.CurrentAccountCardID);
                b.HasIndex(x => x.BranchID);
                b.HasIndex(x => x.WarehouseID);
            });
        }

        public static void ConfigurePurchasePriceLines(this ModelBuilder builder)
        {
            builder.Entity<PurchasePriceLines>(b =>
            {
                b.ToTable("PurchasePriceLines");
                b.ConfigureByConvention();

                b.Property(t => t.PurchasePriceID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.CurrencyID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.CurrentAccountCardID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Linenr).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.Price).HasColumnType(SqlDbType.Decimal.ToString()).HasPrecision(18, 6);
                b.Property(t => t.StartDate).IsRequired().HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.EndDate).IsRequired().HasColumnType(SqlDbType.DateTime.ToString());

                b.HasIndex(x => x.PurchasePriceID);
                b.HasIndex(x => x.ProductID);
                b.HasIndex(x => x.CurrencyID);
                b.HasIndex(x => x.CurrentAccountCardID);
            });
        }

        public static void ConfigureUserGroups(this ModelBuilder builder)
        {
            builder.Entity<UserGroups>(b =>
            {
                b.ToTable("UserGroups");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(300);
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());

                b.HasIndex(x => x.Code);
            });
        }

        public static void ConfigureUsers(this ModelBuilder builder)
        {
            builder.Entity<Users>(b =>
            {
                b.ToTable("Users");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.UserName).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(300);
                b.Property(t => t.NameSurname).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(300);
                b.Property(t => t.Email).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(300);
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsEmailApproved).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.Password).HasColumnType("nvarchar(MAX)");
                b.Property(t => t.GroupID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());

                b.HasIndex(x => x.Code);
            });
        }

        public static void ConfigureFinalControlUnsuitabilityReports(this ModelBuilder builder)
        {
            builder.Entity<FinalControlUnsuitabilityReports>(b =>
            {
                b.ToTable("FinalControlUnsuitabilityReports");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.FicheNo).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Description_).HasColumnType("nvarchar(max)");
                b.Property(t => t.Date_).IsRequired().HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.EmployeeID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.PartyNo).HasColumnType("nvarchar(max)");
                b.Property(t => t.IsToBeUsedAs).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsCorrection).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsScrap).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.ControlFormDeclaration).HasColumnType(SqlDbType.Decimal.ToString());


                b.HasIndex(x => x.FicheNo);
                b.HasIndex(x => x.EmployeeID);
                b.HasIndex(x => x.ProductID);
            });
        }

        public static void ConfigureStationInventories(this ModelBuilder builder)
        {
            builder.Entity<StationInventories>(b =>
            {
                b.ToTable("StationInventories");
                b.ConfigureByConvention();

                b.Property(t => t.StationID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Amount).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.Description_).HasColumnType("nvarchar(max)");

                b.HasIndex(x => x.StationID);
                b.HasIndex(x => x.ProductID);


            });
        }

        public static void ConfigureMaintenanceInstructions(this ModelBuilder builder)
        {
            builder.Entity<MaintenanceInstructions>(b =>
            {
                b.ToTable("MaintenanceInstructions");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.InstructionName).IsRequired().HasColumnType("nvarchar(max)");
                b.Property(t => t.StationID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.PeriodID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.PeriodTime).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.PlannedMaintenanceTime).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.Note_).HasColumnType("nvarchar(max)");


                b.HasIndex(x => x.Code);
                b.HasIndex(x => x.StationID);
                b.HasIndex(x => x.PeriodID);
            });
        }

        public static void ConfigureMaintenanceInstructionLines(this ModelBuilder builder)
        {
            builder.Entity<MaintenanceInstructionLines>(b =>
            {
                b.ToTable("MaintenanceInstructionLines");
                b.ConfigureByConvention();

                b.Property(t => t.InstructionID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.LineNr).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.ProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.UnitSetID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Amount).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.InstructionDescription).HasColumnType("nvarchar(MAX)");

                b.HasIndex(x => x.InstructionID);
                b.HasIndex(x => x.ProductID);
                b.HasIndex(x => x.UnitSetID);
            });
        }

        public static void ConfigurePlannedMaintenances(this ModelBuilder builder)
        {
            builder.Entity<PlannedMaintenances>(b =>
            {
                b.ToTable("PlannedMaintenances");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.RegistrationNo).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.StationID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.PeriodID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Status).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.Caregiver).HasColumnType("nvarchar(max)");
                b.Property(t => t.NumberofCaregivers).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.RemainingTime).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.PlannedTime).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.PeriodTime).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.OccuredTime).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.StartDate).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.PlannedDate).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.CompletionDate).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.Note_).HasColumnType("nvarchar(max)");

                b.HasIndex(x => x.RegistrationNo);
                b.HasIndex(x => x.StationID);
                b.HasIndex(x => x.PeriodID);
            });
        }

        public static void ConfigurePlannedMaintenanceLines(this ModelBuilder builder)
        {
            builder.Entity<PlannedMaintenanceLines>(b =>
            {
                b.ToTable("PlannedMaintenanceLines");
                b.ConfigureByConvention();

                b.Property(t => t.PlannedMaintenanceID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.LineNr).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.ProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.UnitSetID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Amount).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.InstructionDescription).HasColumnType("nvarchar(MAX)");
                b.Property(t => t.MaintenanceNote).HasColumnType("nvarchar(MAX)");

                b.HasIndex(x => x.PlannedMaintenanceID);
                b.HasIndex(x => x.ProductID);
                b.HasIndex(x => x.UnitSetID);
            });
        }

        public static void ConfigureUnplannedMaintenances(this ModelBuilder builder)
        {
            builder.Entity<UnplannedMaintenances>(b =>
            {
                b.ToTable("UnplannedMaintenances");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.RegistrationNo).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.StationID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.PeriodID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Status).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.Caregiver).HasColumnType("nvarchar(max)");
                b.Property(t => t.NumberofCaregivers).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.RemainingTime).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.UnplannedTime).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.PeriodTime).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.OccuredTime).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.StartDate).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.UnplannedDate).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.CompletionDate).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.Note_).HasColumnType("nvarchar(max)");

                b.HasIndex(x => x.RegistrationNo);
                b.HasIndex(x => x.StationID);
                b.HasIndex(x => x.PeriodID);
            });
        }

        public static void ConfigureUnplannedMaintenanceLines(this ModelBuilder builder)
        {
            builder.Entity<UnplannedMaintenanceLines>(b =>
            {
                b.ToTable("UnplannedMaintenanceLines");
                b.ConfigureByConvention();

                b.Property(t => t.UnplannedMaintenanceID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.LineNr).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.ProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.UnitSetID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Amount).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.InstructionDescription).HasColumnType("nvarchar(MAX)");
                b.Property(t => t.MaintenanceNote).HasColumnType("nvarchar(MAX)");

                b.HasIndex(x => x.UnplannedMaintenanceID);
                b.HasIndex(x => x.ProductID);
                b.HasIndex(x => x.UnitSetID);
            });
        }

        public static void ConfigureByDateStockMovements(this ModelBuilder builder)
        {
            builder.Entity<ByDateStockMovements>(b =>
            {
                b.ToTable("ByDateStockMovements");
                b.ConfigureByConvention();

                b.Property(t => t.Date_).IsRequired().HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.TotalPurchaseRequest).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalPurchaseOrder).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalSalesProposition).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalSalesOrder).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalConsumption).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalWastage).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalProduction).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalGoodsReceipt).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalGoodsIssue).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.ProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.BranchID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.WarehouseID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());

                b.HasIndex(x => x.ProductID);
                b.HasIndex(x => x.BranchID);
                b.HasIndex(x => x.WarehouseID);
            });
        }

        public static void ConfigureGrandTotalStockMovements(this ModelBuilder builder)
        {
            builder.Entity<GrandTotalStockMovements>(b =>
            {
                b.ToTable("GrandTotalStockMovements");
                b.ConfigureByConvention();

                b.Property(t => t.TotalReserved).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalPurchaseRequest).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalPurchaseOrder).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalSalesProposition).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalSalesOrder).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalConsumption).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalWastage).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalProduction).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalGoodsReceipt).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.TotalGoodsIssue).HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.ProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.BranchID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.WarehouseID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());

                b.HasIndex(x => x.ProductID);
                b.HasIndex(x => x.BranchID);
                b.HasIndex(x => x.WarehouseID);
            });
        }

        public static void ConfigureMenus(this ModelBuilder builder)
        {
            builder.Entity<Menus>(b =>
            {
                b.ToTable("Menus");
                b.ConfigureByConvention();
                b.ConfigureByConcurrencyConvention();

                b.Property(t => t.MenuName).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200); ;
                b.Property(t => t.ParentMenuId).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                
            });
        }



    }
}
