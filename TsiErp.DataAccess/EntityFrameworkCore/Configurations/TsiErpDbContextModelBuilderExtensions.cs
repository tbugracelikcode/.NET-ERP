using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Authentication.Entities.Menus;
using Tsi.Authentication.Entities.RolePermissions;
using Tsi.Authentication.Entities.Roles;
using Tsi.Authentication.Entities.UserRoles;
using Tsi.Authentication.Entities.Users;
using Tsi.EntityFrameworkCore.Modeling;
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
using TsiErp.Entities.Entities.Operation;
using TsiErp.Entities.Entities.Route;
using TsiErp.Entities.Entities.OperationLine;
using TsiErp.Entities.Entities.RouteLine;
using TsiErp.Entities.Entities.Calendar;
using TsiErp.Entities.Entities.CalendarLine;
using TsiErp.Entities.Entities.Shift;
using TsiErp.Entities.Entities.ShiftLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Configurations
{
    public static class TsiErpDbContextModelBuilderExtensions
    {
        public static void ConfigureUsers(this ModelBuilder builder)
        {
            builder.Entity<TsiUser>(b =>
            {
                b.ToTable("TsiUser");
                b.ConfigureByConvention();

                b.Property(t => t.UserName).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(250);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(250);
                b.Property(t => t.Surname).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(250);
                b.Property(t => t.Email).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(250);
                b.Property(t => t.EmailConfirmed).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.PasswordHash).IsRequired().HasColumnType("nvarchar(max)");
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.PhoneNumber).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(95);

            });
        }

        public static void ConfigureUserRoles(this ModelBuilder builder)
        {
            builder.Entity<TsiUserRoles>(b =>
            {
                b.ToTable("TsiUserRoles");
                b.ConfigureByConvention();

                b.Property(t => t.UserId).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.RoleId).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
            });
        }

        public static void ConfigureRolePermissions(this ModelBuilder builder)
        {
            builder.Entity<TsiRolePermissions>(b =>
            {
                b.ToTable("TsiRolePermissions");
                b.ConfigureByConvention();

                b.Property(t => t.RoleId).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.MenuId).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());

                

                b.HasOne(x=>x.TsiMenus).WithMany(x=>x.TsiRolePermissions).HasForeignKey(x=>x.MenuId).OnDelete(DeleteBehavior.NoAction);
                b.HasOne(x=>x.TsiRoles).WithMany(x=>x.TsiRolePermissions).HasForeignKey(x=>x.RoleId).OnDelete(DeleteBehavior.NoAction);
            });
        }

        public static void ConfigureRoles(this ModelBuilder builder)
        {
            builder.Entity<TsiRoles>(b =>
            {
                b.ToTable("TsiRoles");
                b.ConfigureByConvention();

                b.Property(t => t.RoleName).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(250);

            });
        }

        public static void ConfigureMenus(this ModelBuilder builder)
        {
            builder.Entity<TsiMenus>(b =>
            {
                b.ToTable("TsiMenus");

                b.Property(t => t.Id).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ParentMenutId).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.MenuName).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(250);
            });
        }

        public static void ConfigureBranches(this ModelBuilder builder)
        {
            builder.Entity<Branches>(b =>
            {
                b.ToTable("Branches");
                b.ConfigureByConvention();

                //b.HasQueryFilter(x => !x.IsDeleted);

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.Description_).HasColumnType("nvarchar(MAX)");
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());

                b.HasIndex(x => x.Code);

            });
        }

        public static void ConfigurePeriods(this ModelBuilder builder)
        {
            builder.Entity<Periods>(b =>
            {
                b.ToTable("Periods");
                b.ConfigureByConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.Description_).HasColumnType("nvarchar(MAX)");
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.BranchID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());

                b.HasIndex(x => x.Code);

                b.HasOne(x => x.Branches).WithMany(x => x.Periods).HasForeignKey(x => x.BranchID).OnDelete(DeleteBehavior.NoAction);

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
                b.Property(t => t.LogLevel_).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.UserId).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());

            });
        }

        public static void ConfigureUnitSets(this ModelBuilder builder)
        {
            builder.Entity<UnitSets>(b =>
            {
                b.ToTable("UnitSets");
                b.ConfigureByConvention();

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

                b.HasOne(x => x.StationGroups).WithMany(x => x.Stations).HasForeignKey(x => x.GroupID).OnDelete(DeleteBehavior.NoAction);
            });
        }

        public static void ConfigureEmployees(this ModelBuilder builder)
        {
            builder.Entity<Employees>(b =>
            {
                b.ToTable("Employees");
                b.ConfigureByConvention();

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

                b.HasOne(x => x.Departments).WithMany(x => x.Employees).HasForeignKey(x => x.DepartmentID).OnDelete(DeleteBehavior.NoAction);
            });
        }

        public static void ConfigureEquipmentRecords(this ModelBuilder builder)
        {
            builder.Entity<EquipmentRecords>(b =>
            {
                b.ToTable("EquipmentRecords");
                b.ConfigureByConvention();


                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.MeasuringRange).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(150);
                b.Property(t => t.MeasuringAccuracy).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.Department).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Responsible).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(150);
                b.Property(t => t.EquipmentSerialNo).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(250);
                b.Property(t => t.RecordDate).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.Cancel).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.CancellationDate).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.CancellationReason).HasColumnType("nvarchar(MAX)");

                b.HasIndex(x => x.Code);

                b.HasOne(x => x.Departments).WithMany(x => x.EquipmentRecords).HasForeignKey(x => x.Department).OnDelete(DeleteBehavior.NoAction);


            });
        }

        public static void ConfigureDepartments(this ModelBuilder builder)
        {
            builder.Entity<Departments>(b =>
            {
                b.ToTable("Departments");
                b.ConfigureByConvention();

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



                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.ReceiptNo).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.EquipmentID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Date).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.NextControl).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.InfinitiveCertificateNo).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.Result).HasColumnType("nvarchar(MAX)");

                b.HasIndex(x => x.Code);

                b.HasOne(x => x.EquipmentRecords).WithMany(x => x.CalibrationVerifications).HasForeignKey(x => x.EquipmentID).OnDelete(DeleteBehavior.NoAction);

            });
        }

        public static void ConfigureCalibrationRecords(this ModelBuilder builder)
        {
            builder.Entity<CalibrationRecords>(b =>
            {
                b.ToTable("CalibrationRecords");
                b.ConfigureByConvention();



                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.ReceiptNo).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.EquipmentID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Date).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.NextControl).HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.InfinitiveCertificateNo).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(50);
                b.Property(t => t.Result).HasColumnType("nvarchar(MAX)");

                b.HasIndex(x => x.Code);

                b.HasOne(x => x.EquipmentRecords).WithMany(x => x.CalibrationRecords).HasForeignKey(x => x.EquipmentID).OnDelete(DeleteBehavior.NoAction);

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

                b.Property(t => t.FicheNo).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Description_).HasColumnType("nvarchar(max)");
                b.Property(t => t.Date_).IsRequired().HasColumnType(SqlDbType.DateTime.ToString());
                b.Property(t => t.Time_).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(8);
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

                b.HasOne(x => x.CurrentAccountCards).WithMany(x => x.SalesPropositions).HasForeignKey(x => x.CurrentAccountCardID).OnDelete(DeleteBehavior.NoAction);
                b.HasOne(x => x.Currencies).WithMany(x => x.SalesPropositions).HasForeignKey(x => x.CurrencyID).OnDelete(DeleteBehavior.NoAction);
                b.HasOne(x => x.Warehouses).WithMany(x => x.SalesPropositions).HasForeignKey(x => x.WarehouseID).OnDelete(DeleteBehavior.NoAction);
                b.HasOne(x => x.PaymentPlan).WithMany(x => x.SalesPropositions).HasForeignKey(x => x.PaymentPlanID).OnDelete(DeleteBehavior.NoAction);
                b.HasOne(x => x.Branches).WithMany(x => x.SalesPropositions).HasForeignKey(x => x.BranchID).OnDelete(DeleteBehavior.NoAction);
                b.HasOne(x => x.ShippingAdresses).WithMany(x => x.SalesPropositions).HasForeignKey(x => x.ShippingAdressID).OnDelete(DeleteBehavior.NoAction);
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
                b.Property(t => t.BranchID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
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
                b.Property(t => t.WarehouseID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.HasIndex(x => x.SalesPropositionID);
                b.HasIndex(x => x.ProductID);
                b.HasIndex(x => x.BranchID);
                b.HasIndex(x => x.WarehouseID);

                b.HasOne(x => x.Products).WithMany(x => x.SalesPropositionLines).HasForeignKey(x => x.ProductID).OnDelete(DeleteBehavior.NoAction);
                b.HasOne(x => x.SalesPropositions).WithMany(x => x.SalesPropositionLines).HasForeignKey(x => x.SalesPropositionID).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.UnitSets).WithMany(x => x.SalesPropositionLines).HasForeignKey(x => x.UnitSetID).OnDelete(DeleteBehavior.NoAction);
                b.HasOne(x => x.Warehouses).WithMany(x => x.SalesPropositionLines).HasForeignKey(x => x.WarehouseID).OnDelete(DeleteBehavior.NoAction);
                b.HasOne(x => x.PaymentPlans).WithMany(x => x.SalesPropositionLines).HasForeignKey(x => x.PaymentPlanID).OnDelete(DeleteBehavior.NoAction);
                b.HasOne(x => x.Branches).WithMany(x => x.SalesPropositionLines).HasForeignKey(x => x.BranchID).OnDelete(DeleteBehavior.NoAction);
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

                b.HasOne(x => x.Currencies).WithMany(x => x.ExchangeRates).HasForeignKey(x => x.CurrencyID).OnDelete(DeleteBehavior.NoAction);

            });
        }

        public static void ConfigureCurrentAccountCards(this ModelBuilder builder)
        {
            builder.Entity<CurrentAccountCards>(b =>
            {
                b.ToTable("CurrentAccountCards");
                b.ConfigureByConvention();



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

                b.HasOne(x => x.Currencies).WithMany(x => x.CurrentAccountCards).HasForeignKey(x => x.CurrencyID).OnDelete(DeleteBehavior.NoAction);

            });
        }

        public static void ConfigureProductCodes(this ModelBuilder builder)
        {
            builder.Entity<Products>(b =>
            {
                b.ToTable("Products");
                b.ConfigureByConvention();



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


                b.HasOne(x => x.UnitSets).WithMany(x => x.Products).HasForeignKey(x => x.UnitSetID).OnDelete(DeleteBehavior.NoAction);
                b.HasOne(x => x.ProductGroups).WithMany(x => x.Products).HasForeignKey(x => x.ProductGrpID).OnDelete(DeleteBehavior.NoAction);
            });
        }

        public static void ConfigureProductGroups(this ModelBuilder builder)
        {
            builder.Entity<ProductGroups>(b =>
            {
                b.ToTable("ProductGroups");
                b.ConfigureByConvention();



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

                b.HasOne(x => x.CurrentAccountCards).WithMany(x => x.ShippingAdresses).HasForeignKey(x => x.CustomerCardID).OnDelete(DeleteBehavior.NoAction);

            });
        }

        public static void ConfigureCurrencies(this ModelBuilder builder)
        {
            builder.Entity<Currencies>(b =>
            {
                b.ToTable("Currencies");
                b.ConfigureByConvention();

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

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());

                b.HasIndex(x => x.Code);

            });
        }

        public static void ConfigureOperations(this ModelBuilder builder)
        {
            builder.Entity<Operations>(b =>
            {
                b.ToTable("Operations");
                b.ConfigureByConvention();

                //b.HasQueryFilter(x => !x.IsDeleted);

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.ProductionPoolID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
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

                //b.HasQueryFilter(x => !x.IsDeleted);

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.ProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductionStart).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.Approval).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.TechnicalApproval).HasColumnType(SqlDbType.Bit.ToString());
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());

                b.HasIndex(x => x.Code);

                b.HasOne(x => x.Products).WithOne(x => x.Routes).HasForeignKey<Routes>(x=>x.ProductID).OnDelete(DeleteBehavior.NoAction);

            });
        }

        public static void ConfigureOperationLines(this ModelBuilder builder)
        {
            builder.Entity<OperationLines>(b =>
            {
                b.ToTable("OperationLines");
                b.ConfigureByConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.StationID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.OperationID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.Priority).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.ProcessQuantity).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.AdjustmentAndControlTime).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.LineNr).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.Alternative).HasColumnType(SqlDbType.Bit.ToString());

                b.HasIndex(x => x.Code);

                b.HasOne(x => x.Operations).WithMany(x => x.OperationLines).HasForeignKey(x => x.OperationID).OnDelete(DeleteBehavior.NoAction);
                b.HasOne(x => x.Stations).WithMany(x => x.OperationLines).HasForeignKey(x => x.StationID).OnDelete(DeleteBehavior.NoAction);
            });
        }

        public static void ConfigureRouteLines(this ModelBuilder builder)
        {
            builder.Entity<RouteLines>(b =>
            {
                b.ToTable("RouteLines");
                b.ConfigureByConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.RouteID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.OperationID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductionPoolID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ProductionPoolDescription).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.AdjustmentAndControlTime).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.OperationTime).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.Priority).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.LineNr).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.OperationPicture).HasColumnType("varbinary(max)");


                b.HasIndex(x => x.Code);

                b.HasOne(x => x.Routes).WithMany(x => x.RouteLines).HasForeignKey(x => x.RouteID).OnDelete(DeleteBehavior.NoAction);
                b.HasOne(x => x.Operations).WithMany(x => x.RouteLines).HasForeignKey(x => x.OperationID).OnDelete(DeleteBehavior.NoAction);
                b.HasOne(x => x.Products).WithMany(x => x.RouteLines).HasForeignKey(x => x.ProductID).OnDelete(DeleteBehavior.NoAction);
            });
        }
        public static void ConfigureCalendars(this ModelBuilder builder)
        {
            builder.Entity<Calendars>(b =>
            {
                b.ToTable("Calendars");
                b.ConfigureByConvention();

                b.Property(t => t.Code).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(17);
                b.Property(t => t.Name).IsRequired().HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t._Description).HasColumnType(SqlDbType.NVarChar.ToString()).HasMaxLength(200);
                b.Property(t => t.Year).IsRequired().HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.IsPlanned).IsRequired().HasColumnType(SqlDbType.Bit.ToString());


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
                b.Property(t => t.CalendarID).HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ShiftID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
                b.Property(t => t.ShiftOverTime).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.ShiftTime).IsRequired().HasColumnType(SqlDbType.Decimal.ToString());
                b.Property(t => t.StationID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());

                b.HasIndex(x => x.StationID);
                b.HasIndex(x => x.CalendarID);
                b.HasIndex(x => x.ShiftID);

                b.HasOne(x => x.Stations).WithMany(x => x.CalendarLines).HasForeignKey(x => x.StationID).OnDelete(DeleteBehavior.NoAction);
                b.HasOne(x => x.Shifts).WithMany(x => x.CalendarLines).HasForeignKey(x => x.ShiftID).OnDelete(DeleteBehavior.NoAction);
                b.HasOne(x => x.Calendars).WithMany(x => x.CalendarLines).HasForeignKey(x => x.CalendarID).OnDelete(DeleteBehavior.Cascade);


            });
        }

        public static void ConfigureShifts(this ModelBuilder builder)
        {
            builder.Entity<Shifts>(b =>
            {
                b.ToTable("Shifts");
                b.ConfigureByConvention();

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

                b.HasOne(x => x.Shifts).WithMany(x => x.ShiftLines).HasForeignKey(x => x.ShiftID).OnDelete(DeleteBehavior.NoAction);
            });
        }


    }
}
