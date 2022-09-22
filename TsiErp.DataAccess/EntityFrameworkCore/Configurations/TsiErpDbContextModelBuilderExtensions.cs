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
using TsiErp.Entities.Entities.Station;
using TsiErp.Entities.Entities.StationGroup;
using TsiErp.Entities.Entities.UnitSet;
using TsiErp.Entities.Entities.Vsm;
using TsiErp.Entities.Entities.Currency;
using TsiErp.Entities.Entities.PaymentPlan;
using TsiErp.Entities.Entities.WareHouse;
using TsiErp.Entities.Entities.OperationUnsuitabilityItem;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityItem;

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

                

                b.HasOne(x=>x.TsiMenus).WithMany(x=>x.TsiRolePermissions).HasForeignKey(x=>x.MenuId);
                b.HasOne(x=>x.TsiRoles).WithMany(x=>x.TsiRolePermissions).HasForeignKey(x=>x.RoleId);
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
                b.Property(t => t.X).HasColumnType("decimal(18, 6)");
                b.Property(t => t.Y).HasColumnType("decimal(18, 6)");
                b.Property(t => t.AreaCovered).HasColumnType("decimal(18, 6)");
                b.Property(t => t.UsageArea).HasColumnType("decimal(18, 6)");
                b.Property(t => t.Amortization).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.MachineCost).HasColumnType("decimal(18, 6)");
                b.Property(t => t.Shift).HasColumnType(SqlDbType.Int.ToString());
                b.Property(t => t.ShiftWorkingTime).HasColumnType("decimal(18, 6)");
                b.Property(t => t.PowerFactor).HasColumnType("decimal(18, 6)");
                b.Property(t => t.WorkSafetyInstruction).HasColumnType("varbinary(MAX)");
                b.Property(t => t.UsageInstruction).HasColumnType("varbinary(MAX)");
                b.Property(t => t.IsActive).HasColumnType(SqlDbType.Bit.ToString());

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
                b.Property(t => t.EquipmentID).IsRequired().HasColumnType(SqlDbType.UniqueIdentifier.ToString());
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
                b.Property(t => t.DelayMaturityDifference).HasColumnType("decimal(18, 6)");
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


    }
}
