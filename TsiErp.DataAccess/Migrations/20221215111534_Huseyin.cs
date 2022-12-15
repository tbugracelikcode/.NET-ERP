using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class Huseyin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    Description_ = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Calendars",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    _Description = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: true),
                    IsPlanned = table.Column<bool>(type: "Bit", nullable: false),
                    Year = table.Column<int>(type: "Int", nullable: false),
                    TotalDays = table.Column<decimal>(type: "Decimal", nullable: false),
                    OfficialHolidayDays = table.Column<decimal>(type: "Decimal", nullable: false),
                    AvailableDays = table.Column<decimal>(type: "Decimal", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContractUnsuitabilityItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Detection = table.Column<int>(type: "Int", nullable: false),
                    Severity = table.Column<int>(type: "Int", nullable: false),
                    StaProductivityAnalysis = table.Column<bool>(type: "Bit", nullable: false),
                    PerProductivityAnalysis = table.Column<bool>(type: "Bit", nullable: false),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractUnsuitabilityItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerComplaintItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Detection = table.Column<int>(type: "Int", nullable: false),
                    Severity = table.Column<int>(type: "Int", nullable: false),
                    StaProductivityAnalysis = table.Column<bool>(type: "Bit", nullable: false),
                    PerProductivityAnalysis = table.Column<bool>(type: "Bit", nullable: false),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerComplaintItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinalControlUnsuitabilityItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Detection = table.Column<int>(type: "Int", nullable: false),
                    Severity = table.Column<int>(type: "Int", nullable: false),
                    StaProductivityAnalysis = table.Column<bool>(type: "Bit", nullable: false),
                    PerProductivityAnalysis = table.Column<bool>(type: "Bit", nullable: false),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalControlUnsuitabilityItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Date_ = table.Column<DateTime>(type: "DateTime", maxLength: 200, nullable: false),
                    MethodName_ = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    BeforeValues = table.Column<object>(type: "sql_variant", maxLength: 5000, nullable: false),
                    AfterValues = table.Column<object>(type: "sql_variant", maxLength: 5000, nullable: false),
                    LogLevel_ = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MenuName = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    ParentMenuId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperationUnsuitabilityItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Detection = table.Column<int>(type: "Int", nullable: false),
                    Severity = table.Column<int>(type: "Int", nullable: false),
                    StaProductivityAnalysis = table.Column<bool>(type: "Bit", nullable: false),
                    PerProductivityAnalysis = table.Column<bool>(type: "Bit", nullable: false),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationUnsuitabilityItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    Days_ = table.Column<int>(type: "Int", nullable: false),
                    DelayMaturityDifference = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductionOrderChangeItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Detection = table.Column<int>(type: "Int", nullable: false),
                    Severity = table.Column<int>(type: "Int", nullable: false),
                    StaProductivityAnalysis = table.Column<bool>(type: "Bit", nullable: false),
                    PerProductivityAnalysis = table.Column<bool>(type: "Bit", nullable: false),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionOrderChangeItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchasingUnsuitabilityItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Detection = table.Column<int>(type: "Int", nullable: false),
                    Severity = table.Column<int>(type: "Int", nullable: false),
                    StaProductivityAnalysis = table.Column<bool>(type: "Bit", nullable: false),
                    PerProductivityAnalysis = table.Column<bool>(type: "Bit", nullable: false),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasingUnsuitabilityItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    TotalWorkTime = table.Column<decimal>(type: "Decimal", nullable: false),
                    TotalBreakTime = table.Column<decimal>(type: "Decimal", nullable: false),
                    NetWorkTime = table.Column<decimal>(type: "Decimal", nullable: false),
                    Overtime = table.Column<decimal>(type: "Decimal", nullable: false),
                    ShiftOrder = table.Column<int>(type: "Int", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StationGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    TotalEmployees = table.Column<int>(type: "Int", nullable: false),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemplateOperations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    WorkCenterID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateOperations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnitSets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitSets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VsmSchemas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(250)", maxLength: 250, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", maxLength: 250, nullable: false),
                    Description_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VSMSchema = table.Column<object>(type: "sql_variant", maxLength: 25000000, nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VsmSchemas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Periods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    Description_ = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    BranchID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Periods_Branches_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CalendarDays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CalendarID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Date_ = table.Column<DateTime>(type: "Date", nullable: false),
                    CalendarDayStateEnum = table.Column<int>(type: "Int", nullable: false),
                    ColorCode = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalendarDays_Calendars_CalendarID",
                        column: x => x.CalendarID,
                        principalTable: "Calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CurrentAccountCards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    SupplierNo = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    ShippingAddress = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    Type = table.Column<int>(type: "Int", nullable: false),
                    Address1 = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    District = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    Country = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    PostCode = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    Tel1 = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    Tel2 = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    Fax = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    Responsible = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    Web = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    PrivateCode1 = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    PrivateCode2 = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    PrivateCode3 = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    PrivateCode4 = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    PrivateCode5 = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    SoleProprietorship = table.Column<bool>(type: "Bit", nullable: false),
                    IDnumber = table.Column<string>(type: "NVarChar(11)", maxLength: 11, nullable: true),
                    TaxAdministration = table.Column<string>(type: "NVarChar(75)", maxLength: 75, nullable: false),
                    TaxNumber = table.Column<string>(type: "NVarChar(10)", maxLength: 10, nullable: false),
                    CoatingCustomer = table.Column<bool>(type: "Bit", nullable: false),
                    SaleContract = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    PlusPercentage = table.Column<int>(type: "Int", nullable: false),
                    CurrencyID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Supplier = table.Column<bool>(type: "Bit", nullable: false),
                    ContractSupplier = table.Column<bool>(type: "Bit", nullable: false),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentAccountCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrentAccountCards_Currencies_CurrencyID",
                        column: x => x.CurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExchangeRates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "DateTime", nullable: false),
                    BuyingRate = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    SaleRate = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EffectiveBuyingRate = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EffectiveSaleRate = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExchangeRates_Currencies_CurrencyID",
                        column: x => x.CurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    Surname = table.Column<string>(type: "NVarChar(100)", maxLength: 100, nullable: false),
                    DepartmentID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    IDnumber = table.Column<string>(type: "NVarChar(11)", maxLength: 11, nullable: false),
                    Birthday = table.Column<DateTime>(type: "DateTime", nullable: true),
                    BloodType = table.Column<int>(type: "Int", nullable: false),
                    Address = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: true),
                    District = table.Column<string>(type: "NVarChar(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "NVarChar(100)", maxLength: 100, nullable: true),
                    HomePhone = table.Column<string>(type: "NVarChar(100)", maxLength: 100, nullable: true),
                    CellPhone = table.Column<string>(type: "NVarChar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: true),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EquipmentRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    MeasuringRange = table.Column<string>(type: "NVarChar(150)", maxLength: 150, nullable: true),
                    MeasuringAccuracy = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    Department = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Responsible = table.Column<string>(type: "NVarChar(150)", maxLength: 150, nullable: true),
                    Frequency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EquipmentSerialNo = table.Column<string>(type: "NVarChar(250)", maxLength: 250, nullable: true),
                    RecordDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Cancel = table.Column<bool>(type: "Bit", nullable: false),
                    CancellationDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    CancellationReason = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentRecords_Departments_Department",
                        column: x => x.Department,
                        principalTable: "Departments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShiftLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShiftID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    LineNr = table.Column<int>(type: "int", nullable: false),
                    StartHour = table.Column<TimeSpan>(type: "time(7)", nullable: false),
                    EndHour = table.Column<TimeSpan>(type: "time(7)", nullable: false),
                    Type = table.Column<int>(type: "Int", nullable: false),
                    Coefficient = table.Column<decimal>(type: "Decimal", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShiftLines_Shifts_ShiftID",
                        column: x => x.ShiftID,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    Brand = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    Model = table.Column<int>(type: "Int", nullable: false),
                    Capacity = table.Column<string>(type: "NVarChar(85)", maxLength: 85, nullable: true),
                    KWA = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    GroupID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    X = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Y = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    AreaCovered = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    UsageArea = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Amortization = table.Column<int>(type: "Int", nullable: false),
                    MachineCost = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Shift = table.Column<int>(type: "Int", nullable: false),
                    ShiftWorkingTime = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PowerFactor = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WorkSafetyInstruction = table.Column<byte[]>(type: "varbinary(MAX)", nullable: true),
                    UsageInstruction = table.Column<byte[]>(type: "varbinary(MAX)", nullable: true),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    IsFixtures = table.Column<bool>(type: "Bit", nullable: false),
                    IsContract = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stations_StationGroups_GroupID",
                        column: x => x.GroupID,
                        principalTable: "StationGroups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    SupplyForm = table.Column<int>(type: "Int", nullable: false),
                    ProductSize = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    GTIP = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    SawWastage = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Confirmation = table.Column<bool>(type: "Bit", nullable: false),
                    TechnicalConfirmation = table.Column<bool>(type: "Bit", nullable: false),
                    ProductType = table.Column<int>(type: "Int", nullable: false),
                    ProductDescription = table.Column<string>(type: "NVarChar(500)", maxLength: 500, nullable: true),
                    ProductGrpID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ManufacturerCode = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    SaleVAT = table.Column<int>(type: "Int", nullable: false),
                    PurchaseVAT = table.Column<int>(type: "Int", nullable: false),
                    UnitSetID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    FeatureSetID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    EnglishDefinition = table.Column<string>(type: "NVarChar(201)", maxLength: 201, nullable: true),
                    ExportCatNo = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    OemRefNo = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    OemRefNo2 = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    OemRefNo3 = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    PlannedWastage = table.Column<int>(type: "Int", nullable: false),
                    CoatingWeight = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_ProductGroups_ProductGrpID",
                        column: x => x.ProductGrpID,
                        principalTable: "ProductGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_UnitSets_UnitSetID",
                        column: x => x.UnitSetID,
                        principalTable: "UnitSets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShippingAdresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    CustomerCardID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Adress1 = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    Adress2 = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    District = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: false),
                    City = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: false),
                    PostCode = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "NVarChar(100)", maxLength: 100, nullable: false),
                    EMail = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: true),
                    Fax = table.Column<string>(type: "NVarChar(100)", maxLength: 100, nullable: true),
                    _Default = table.Column<decimal>(type: "Decimal", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingAdresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShippingAdresses_CurrentAccountCards_CustomerCardID",
                        column: x => x.CustomerCardID,
                        principalTable: "CurrentAccountCards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CalibrationRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    ReceiptNo = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    EquipmentID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "DateTime", nullable: false),
                    NextControl = table.Column<DateTime>(type: "DateTime", nullable: false),
                    InfinitiveCertificateNo = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    Result = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalibrationRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalibrationRecords_EquipmentRecords_EquipmentID",
                        column: x => x.EquipmentID,
                        principalTable: "EquipmentRecords",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CalibrationVerifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    ReceiptNo = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    EquipmentID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "DateTime", nullable: false),
                    NextControl = table.Column<DateTime>(type: "DateTime", nullable: false),
                    InfinitiveCertificateNo = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    Result = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalibrationVerifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalibrationVerifications_EquipmentRecords_EquipmentID",
                        column: x => x.EquipmentID,
                        principalTable: "EquipmentRecords",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CalendarLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShiftOverTime = table.Column<decimal>(type: "Decimal", nullable: false),
                    ShiftTime = table.Column<decimal>(type: "Decimal", nullable: false),
                    PlannedHaltTimes = table.Column<decimal>(type: "Decimal", nullable: false),
                    AvailableTime = table.Column<decimal>(type: "Decimal", nullable: false),
                    ShiftID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CalendarID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    StationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalendarLines_Calendars_CalendarID",
                        column: x => x.CalendarID,
                        principalTable: "Calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalendarLines_Shifts_ShiftID",
                        column: x => x.ShiftID,
                        principalTable: "Shifts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CalendarLines_Stations_StationID",
                        column: x => x.StationID,
                        principalTable: "Stations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TemplateOperationLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateOperationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    StationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Priority = table.Column<int>(type: "Int", nullable: false),
                    ProcessQuantity = table.Column<int>(type: "Int", nullable: false),
                    AdjustmentAndControlTime = table.Column<int>(type: "Int", nullable: false),
                    OperationTime = table.Column<decimal>(type: "Decimal", nullable: false),
                    LineNr = table.Column<int>(type: "Int", nullable: false),
                    Alternative = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateOperationLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateOperationLines_Stations_StationID",
                        column: x => x.StationID,
                        principalTable: "Stations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TemplateOperationLines_TemplateOperations_TemplateOperationID",
                        column: x => x.TemplateOperationID,
                        principalTable: "TemplateOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillsofMaterials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    FinishedProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    RouteID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    _Description = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillsofMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillsofMaterials_Products_FinishedProductID",
                        column: x => x.FinishedProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductsOperations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    WorkCenterID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    TemplateOperationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsOperations_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductionStart = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    Approval = table.Column<bool>(type: "Bit", nullable: false),
                    TechnicalApproval = table.Column<bool>(type: "Bit", nullable: false),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Routes_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FicheNo = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Date_ = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Time_ = table.Column<string>(type: "NVarChar(8)", maxLength: 8, nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Description_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialCode = table.Column<string>(type: "NVarChar(201)", maxLength: 201, nullable: true),
                    PurchaseOrderState = table.Column<int>(type: "Int", nullable: false),
                    LinkedPurchaseRequestID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CurrencyID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    PaymentPlanID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    BranchID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    WarehouseID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CurrentAccountCardID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductionOrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    GrossAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalVatExcludedAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalVatAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalDiscountAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    NetAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WorkOrderCreationDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    ShippingAdressID = table.Column<Guid>(type: "UniqueIdentifier", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Branches_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Currencies_CurrencyID",
                        column: x => x.CurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_CurrentAccountCards_CurrentAccountCardID",
                        column: x => x.CurrentAccountCardID,
                        principalTable: "CurrentAccountCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_PaymentPlans_PaymentPlanID",
                        column: x => x.PaymentPlanID,
                        principalTable: "PaymentPlans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_ShippingAdresses_ShippingAdressID",
                        column: x => x.ShippingAdressID,
                        principalTable: "ShippingAdresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FicheNo = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Date_ = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Time_ = table.Column<string>(type: "NVarChar(8)", maxLength: 8, nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Description_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialCode = table.Column<string>(type: "NVarChar(201)", maxLength: 201, nullable: true),
                    PropositionRevisionNo = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    RevisionDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    RevisionTime = table.Column<string>(type: "NVarChar(8)", maxLength: 8, nullable: true),
                    PurchaseRequestState = table.Column<int>(type: "Int", nullable: false),
                    LinkedPurchaseRequestID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CurrencyID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    PaymentPlanID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    BranchID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    WarehouseID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CurrentAccountCardID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    GrossAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalVatExcludedAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalVatAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalDiscountAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    NetAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ValidityDate_ = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ShippingAdressID = table.Column<Guid>(type: "UniqueIdentifier", nullable: true),
                    ProductionOrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseRequests_Branches_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseRequests_Currencies_CurrencyID",
                        column: x => x.CurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseRequests_CurrentAccountCards_CurrentAccountCardID",
                        column: x => x.CurrentAccountCardID,
                        principalTable: "CurrentAccountCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseRequests_PaymentPlans_PaymentPlanID",
                        column: x => x.PaymentPlanID,
                        principalTable: "PaymentPlans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseRequests_ShippingAdresses_ShippingAdressID",
                        column: x => x.ShippingAdressID,
                        principalTable: "ShippingAdresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseRequests_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SalesOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FicheNo = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Date_ = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Time_ = table.Column<string>(type: "NVarChar(8)", maxLength: 8, nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Description_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialCode = table.Column<string>(type: "NVarChar(201)", maxLength: 201, nullable: true),
                    SalesOrderState = table.Column<int>(type: "Int", nullable: false),
                    LinkedSalesPropositionID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CurrencyID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    PaymentPlanID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    BranchID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    WarehouseID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CurrentAccountCardID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    GrossAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalVatExcludedAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalVatAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalDiscountAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    NetAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WorkOrderCreationDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    ShippingAdressID = table.Column<Guid>(type: "UniqueIdentifier", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesOrders_Branches_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesOrders_Currencies_CurrencyID",
                        column: x => x.CurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesOrders_CurrentAccountCards_CurrentAccountCardID",
                        column: x => x.CurrentAccountCardID,
                        principalTable: "CurrentAccountCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesOrders_PaymentPlans_PaymentPlanID",
                        column: x => x.PaymentPlanID,
                        principalTable: "PaymentPlans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesOrders_ShippingAdresses_ShippingAdressID",
                        column: x => x.ShippingAdressID,
                        principalTable: "ShippingAdresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesOrders_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SalesPropositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FicheNo = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Date_ = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Time_ = table.Column<string>(type: "NVarChar(8)", maxLength: 8, nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Description_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialCode = table.Column<string>(type: "NVarChar(201)", maxLength: 201, nullable: true),
                    PropositionRevisionNo = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    RevisionDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    RevisionTime = table.Column<string>(type: "NVarChar(8)", maxLength: 8, nullable: true),
                    SalesPropositionState = table.Column<int>(type: "Int", nullable: false),
                    LinkedSalesPropositionID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CurrencyID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    PaymentPlanID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    BranchID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    WarehouseID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CurrentAccountCardID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    GrossAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalVatExcludedAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalVatAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalDiscountAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    NetAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ValidityDate_ = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ShippingAdressID = table.Column<Guid>(type: "UniqueIdentifier", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesPropositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesPropositions_Branches_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesPropositions_Currencies_CurrencyID",
                        column: x => x.CurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesPropositions_CurrentAccountCards_CurrentAccountCardID",
                        column: x => x.CurrentAccountCardID,
                        principalTable: "CurrentAccountCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesPropositions_PaymentPlans_PaymentPlanID",
                        column: x => x.PaymentPlanID,
                        principalTable: "PaymentPlans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesPropositions_ShippingAdresses_ShippingAdressID",
                        column: x => x.ShippingAdressID,
                        principalTable: "ShippingAdresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesPropositions_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BillsofMaterialLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoMID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    FinishedProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    RouteID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    MaterialType = table.Column<int>(type: "Int", nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Quantity = table.Column<decimal>(type: "Decimal", nullable: false),
                    UnitSetID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    _Description = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: true),
                    LineNr = table.Column<int>(type: "Int", nullable: false),
                    Size = table.Column<decimal>(type: "Decimal", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillsofMaterialLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillsofMaterialLines_BillsofMaterials_BoMID",
                        column: x => x.BoMID,
                        principalTable: "BillsofMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillsofMaterialLines_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BillsofMaterialLines_UnitSets_UnitSetID",
                        column: x => x.UnitSetID,
                        principalTable: "UnitSets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductsOperationLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductsOperationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    StationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Priority = table.Column<int>(type: "Int", nullable: false),
                    ProcessQuantity = table.Column<int>(type: "Int", nullable: false),
                    AdjustmentAndControlTime = table.Column<int>(type: "Int", nullable: false),
                    OperationTime = table.Column<decimal>(type: "Decimal", nullable: false),
                    LineNr = table.Column<int>(type: "Int", nullable: false),
                    Alternative = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsOperationLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsOperationLines_ProductsOperations_ProductsOperationID",
                        column: x => x.ProductsOperationID,
                        principalTable: "ProductsOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsOperationLines_Stations_StationID",
                        column: x => x.StationID,
                        principalTable: "Stations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RouteLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RouteID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductsOperationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductionPoolID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductionPoolDescription = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: true),
                    AdjustmentAndControlTime = table.Column<int>(type: "Int", nullable: false),
                    OperationTime = table.Column<decimal>(type: "Decimal", nullable: false),
                    Priority = table.Column<int>(type: "Int", nullable: false),
                    LineNr = table.Column<int>(type: "Int", nullable: false),
                    OperationPicture = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteLines_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RouteLines_ProductsOperations_ProductsOperationID",
                        column: x => x.ProductsOperationID,
                        principalTable: "ProductsOperations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RouteLines_Routes_RouteID",
                        column: x => x.RouteID,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseOrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    LineNr = table.Column<int>(type: "Int", nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    UnitSetID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductionOrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    LikedPurchaseRequestLineID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    LinkedPurchaseRequestID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Quantity = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    DiscountRate = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    LineAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    LineTotalAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    LineDescription = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    VATrate = table.Column<int>(type: "Int", nullable: false),
                    VATamount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PaymentPlanID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    PurchaseOrderLineStateEnum = table.Column<int>(type: "Int", nullable: false),
                    WorkOrderCreationDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLines_PaymentPlans_PaymentPlanID",
                        column: x => x.PaymentPlanID,
                        principalTable: "PaymentPlans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLines_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLines_PurchaseOrders_PurchaseOrderID",
                        column: x => x.PurchaseOrderID,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLines_UnitSets_UnitSetID",
                        column: x => x.UnitSetID,
                        principalTable: "UnitSets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseUnsuitabilityReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FicheNo = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    PartyNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_ = table.Column<DateTime>(type: "DateTime", nullable: false),
                    UnsuitableAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Description_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUnsuitabilityWorkOrder = table.Column<bool>(type: "Bit", nullable: false),
                    IsReject = table.Column<bool>(type: "Bit", nullable: false),
                    IsCorrection = table.Column<bool>(type: "Bit", nullable: false),
                    IsToBeUsedAs = table.Column<bool>(type: "Bit", nullable: false),
                    IsContactSupplier = table.Column<bool>(type: "Bit", nullable: false),
                    OrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CurrentAccountCardID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseUnsuitabilityReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseUnsuitabilityReports_CurrentAccountCards_CurrentAccountCardID",
                        column: x => x.CurrentAccountCardID,
                        principalTable: "CurrentAccountCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseUnsuitabilityReports_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseUnsuitabilityReports_PurchaseOrders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseRequestLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseRequestID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    LineNr = table.Column<int>(type: "Int", nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    UnitSetID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Quantity = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    DiscountRate = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    LineAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    LineTotalAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    LineDescription = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    VATrate = table.Column<int>(type: "Int", nullable: false),
                    VATamount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PaymentPlanID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductionOrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    PurchaseRequestLineState = table.Column<int>(type: "Int", nullable: false),
                    OrderConversionDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRequestLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseRequestLines_PaymentPlans_PaymentPlanID",
                        column: x => x.PaymentPlanID,
                        principalTable: "PaymentPlans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseRequestLines_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseRequestLines_PurchaseRequests_PurchaseRequestID",
                        column: x => x.PurchaseRequestID,
                        principalTable: "PurchaseRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseRequestLines_UnitSets_UnitSetID",
                        column: x => x.UnitSetID,
                        principalTable: "UnitSets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SalesPropositionLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalesPropositionID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    LineNr = table.Column<int>(type: "Int", nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    UnitSetID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Quantity = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    DiscountRate = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    LineAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    LineTotalAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    LineDescription = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    VATrate = table.Column<int>(type: "Int", nullable: false),
                    VATamount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PaymentPlanID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    SalesPropositionLineState = table.Column<int>(type: "Int", nullable: false),
                    OrderConversionDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    PurchaseRequestsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesPropositionLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesPropositionLines_PaymentPlans_PaymentPlanID",
                        column: x => x.PaymentPlanID,
                        principalTable: "PaymentPlans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesPropositionLines_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesPropositionLines_PurchaseRequests_PurchaseRequestsId",
                        column: x => x.PurchaseRequestsId,
                        principalTable: "PurchaseRequests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesPropositionLines_SalesPropositions_SalesPropositionID",
                        column: x => x.SalesPropositionID,
                        principalTable: "SalesPropositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesPropositionLines_UnitSets_UnitSetID",
                        column: x => x.UnitSetID,
                        principalTable: "UnitSets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SalesOrderLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalesOrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    LineNr = table.Column<int>(type: "Int", nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    UnitSetID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    LikedPropositionLineID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Quantity = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    DiscountRate = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    LineAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    LineTotalAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    LineDescription = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    VATrate = table.Column<int>(type: "Int", nullable: false),
                    VATamount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PaymentPlanID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    SalesOrderLineStateEnum = table.Column<int>(type: "Int", nullable: false),
                    WorkOrderCreationDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    SalesPropositionLinesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrderLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesOrderLines_PaymentPlans_PaymentPlanID",
                        column: x => x.PaymentPlanID,
                        principalTable: "PaymentPlans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesOrderLines_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesOrderLines_SalesOrders_SalesOrderID",
                        column: x => x.SalesOrderID,
                        principalTable: "SalesOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesOrderLines_SalesPropositionLines_SalesPropositionLinesId",
                        column: x => x.SalesPropositionLinesId,
                        principalTable: "SalesPropositionLines",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesOrderLines_UnitSets_UnitSetID",
                        column: x => x.UnitSetID,
                        principalTable: "UnitSets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductionOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FicheNo = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Date_ = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Cancel_ = table.Column<bool>(type: "Bit", nullable: false),
                    ProductionOrderState = table.Column<int>(type: "Int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    PlannedQuantity = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ProducedQuantity = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Description_ = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: true),
                    CustomerOrderNo = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: true),
                    OrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    OrderLineID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    FinishedProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    LinkedProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    UnitSetID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    BOMID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    RouteID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductTreeID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductTreeLineID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    PropositionID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    PropositionLineID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CurrentAccountID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    LinkedProductionOrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionOrders_BillsofMaterials_BOMID",
                        column: x => x.BOMID,
                        principalTable: "BillsofMaterials",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionOrders_CurrentAccountCards_CurrentAccountID",
                        column: x => x.CurrentAccountID,
                        principalTable: "CurrentAccountCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionOrders_Products_FinishedProductID",
                        column: x => x.FinishedProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionOrders_Routes_RouteID",
                        column: x => x.RouteID,
                        principalTable: "Routes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionOrders_SalesOrderLines_OrderLineID",
                        column: x => x.OrderLineID,
                        principalTable: "SalesOrderLines",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionOrders_SalesOrders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "SalesOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionOrders_SalesPropositionLines_PropositionLineID",
                        column: x => x.PropositionLineID,
                        principalTable: "SalesPropositionLines",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionOrders_SalesPropositions_PropositionID",
                        column: x => x.PropositionID,
                        principalTable: "SalesPropositions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionOrders_UnitSets_UnitSetID",
                        column: x => x.UnitSetID,
                        principalTable: "UnitSets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    WorkOrderNo = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: true),
                    IsCancel = table.Column<bool>(type: "Bit", nullable: false),
                    WorkOrderState = table.Column<int>(type: "Int", nullable: false),
                    AdjustmentAndControlTime = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    OperationTime = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    OccuredStartDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    OccuredFinishDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    PlannedQuantity = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ProducedQuantity = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    LineNr = table.Column<int>(type: "Int", nullable: false),
                    LinkedWorkOrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: true),
                    ProductionOrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    PropositionID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    RouteID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductsOperationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    StationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    StationGroupID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CurrentAccountCardID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOrders_CurrentAccountCards_CurrentAccountCardID",
                        column: x => x.CurrentAccountCardID,
                        principalTable: "CurrentAccountCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkOrders_ProductionOrders_ProductionOrderID",
                        column: x => x.ProductionOrderID,
                        principalTable: "ProductionOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkOrders_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkOrders_ProductsOperations_ProductsOperationID",
                        column: x => x.ProductsOperationID,
                        principalTable: "ProductsOperations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkOrders_Routes_RouteID",
                        column: x => x.RouteID,
                        principalTable: "Routes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkOrders_SalesPropositions_PropositionID",
                        column: x => x.PropositionID,
                        principalTable: "SalesPropositions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkOrders_StationGroups_StationGroupID",
                        column: x => x.StationGroupID,
                        principalTable: "StationGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkOrders_Stations_StationID",
                        column: x => x.StationID,
                        principalTable: "Stations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OperationUnsuitabilityReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FicheNo = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Date_ = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Description_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUnsuitabilityWorkOrder = table.Column<bool>(type: "Bit", nullable: false),
                    ControlFormDeclaration = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    IsScrap = table.Column<bool>(type: "Bit", nullable: false),
                    IsCorrection = table.Column<bool>(type: "Bit", nullable: false),
                    IsToBeUsedAs = table.Column<bool>(type: "Bit", nullable: false),
                    WorkOrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    StationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    StationGroupID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    EmployeeID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductionOrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    OperationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationUnsuitabilityReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationUnsuitabilityReports_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OperationUnsuitabilityReports_ProductionOrders_ProductionOrderID",
                        column: x => x.ProductionOrderID,
                        principalTable: "ProductionOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OperationUnsuitabilityReports_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OperationUnsuitabilityReports_ProductsOperations_OperationID",
                        column: x => x.OperationID,
                        principalTable: "ProductsOperations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OperationUnsuitabilityReports_StationGroups_StationGroupID",
                        column: x => x.StationGroupID,
                        principalTable: "StationGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OperationUnsuitabilityReports_Stations_StationID",
                        column: x => x.StationID,
                        principalTable: "Stations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OperationUnsuitabilityReports_WorkOrders_WorkOrderID",
                        column: x => x.WorkOrderID,
                        principalTable: "WorkOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BillsofMaterialLines_BoMID",
                table: "BillsofMaterialLines",
                column: "BoMID");

            migrationBuilder.CreateIndex(
                name: "IX_BillsofMaterialLines_ProductID",
                table: "BillsofMaterialLines",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_BillsofMaterialLines_UnitSetID",
                table: "BillsofMaterialLines",
                column: "UnitSetID");

            migrationBuilder.CreateIndex(
                name: "IX_BillsofMaterials_Code",
                table: "BillsofMaterials",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_BillsofMaterials_FinishedProductID",
                table: "BillsofMaterials",
                column: "FinishedProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_Code",
                table: "Branches",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarDays_CalendarID",
                table: "CalendarDays",
                column: "CalendarID");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarLines_CalendarID",
                table: "CalendarLines",
                column: "CalendarID");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarLines_ShiftID",
                table: "CalendarLines",
                column: "ShiftID");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarLines_StationID",
                table: "CalendarLines",
                column: "StationID");

            migrationBuilder.CreateIndex(
                name: "IX_Calendars_Code",
                table: "Calendars",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationRecords_Code",
                table: "CalibrationRecords",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationRecords_EquipmentID",
                table: "CalibrationRecords",
                column: "EquipmentID");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationVerifications_Code",
                table: "CalibrationVerifications",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationVerifications_EquipmentID",
                table: "CalibrationVerifications",
                column: "EquipmentID");

            migrationBuilder.CreateIndex(
                name: "IX_ContractUnsuitabilityItems_Code",
                table: "ContractUnsuitabilityItems",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Code",
                table: "Currencies",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentAccountCards_Code",
                table: "CurrentAccountCards",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentAccountCards_CurrencyID",
                table: "CurrentAccountCards",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerComplaintItems_Code",
                table: "CustomerComplaintItems",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Code",
                table: "Departments",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Code",
                table: "Employees",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentID",
                table: "Employees",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentRecords_Code",
                table: "EquipmentRecords",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentRecords_Department",
                table: "EquipmentRecords",
                column: "Department");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_CurrencyID",
                table: "ExchangeRates",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_Date",
                table: "ExchangeRates",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_FinalControlUnsuitabilityItems_Code",
                table: "FinalControlUnsuitabilityItems",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_OperationUnsuitabilityItems_Code",
                table: "OperationUnsuitabilityItems",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_OperationUnsuitabilityReports_EmployeeID",
                table: "OperationUnsuitabilityReports",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationUnsuitabilityReports_FicheNo",
                table: "OperationUnsuitabilityReports",
                column: "FicheNo");

            migrationBuilder.CreateIndex(
                name: "IX_OperationUnsuitabilityReports_OperationID",
                table: "OperationUnsuitabilityReports",
                column: "OperationID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationUnsuitabilityReports_ProductID",
                table: "OperationUnsuitabilityReports",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationUnsuitabilityReports_ProductionOrderID",
                table: "OperationUnsuitabilityReports",
                column: "ProductionOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationUnsuitabilityReports_StationGroupID",
                table: "OperationUnsuitabilityReports",
                column: "StationGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationUnsuitabilityReports_StationID",
                table: "OperationUnsuitabilityReports",
                column: "StationID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationUnsuitabilityReports_WorkOrderID",
                table: "OperationUnsuitabilityReports",
                column: "WorkOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentPlans_Code",
                table: "PaymentPlans",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Periods_BranchID",
                table: "Periods",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_Periods_Code",
                table: "Periods",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_ProductGroups_Code",
                table: "ProductGroups",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrderChangeItems_Code",
                table: "ProductionOrderChangeItems",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_BOMID",
                table: "ProductionOrders",
                column: "BOMID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_CurrentAccountID",
                table: "ProductionOrders",
                column: "CurrentAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_FicheNo",
                table: "ProductionOrders",
                column: "FicheNo");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_FinishedProductID",
                table: "ProductionOrders",
                column: "FinishedProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_OrderID",
                table: "ProductionOrders",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_OrderLineID",
                table: "ProductionOrders",
                column: "OrderLineID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_PropositionID",
                table: "ProductionOrders",
                column: "PropositionID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_PropositionLineID",
                table: "ProductionOrders",
                column: "PropositionLineID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_RouteID",
                table: "ProductionOrders",
                column: "RouteID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_UnitSetID",
                table: "ProductionOrders",
                column: "UnitSetID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Code",
                table: "Products",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductGrpID",
                table: "Products",
                column: "ProductGrpID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UnitSetID",
                table: "Products",
                column: "UnitSetID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsOperationLines_ProductsOperationID",
                table: "ProductsOperationLines",
                column: "ProductsOperationID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsOperationLines_StationID",
                table: "ProductsOperationLines",
                column: "StationID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsOperations_Code",
                table: "ProductsOperations",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsOperations_ProductID",
                table: "ProductsOperations",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLines_PaymentPlanID",
                table: "PurchaseOrderLines",
                column: "PaymentPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLines_ProductID",
                table: "PurchaseOrderLines",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLines_PurchaseOrderID",
                table: "PurchaseOrderLines",
                column: "PurchaseOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLines_UnitSetID",
                table: "PurchaseOrderLines",
                column: "UnitSetID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_BranchID",
                table: "PurchaseOrders",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_CurrencyID",
                table: "PurchaseOrders",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_CurrentAccountCardID",
                table: "PurchaseOrders",
                column: "CurrentAccountCardID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_FicheNo",
                table: "PurchaseOrders",
                column: "FicheNo");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PaymentPlanID",
                table: "PurchaseOrders",
                column: "PaymentPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_ShippingAdressID",
                table: "PurchaseOrders",
                column: "ShippingAdressID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_WarehouseID",
                table: "PurchaseOrders",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestLines_PaymentPlanID",
                table: "PurchaseRequestLines",
                column: "PaymentPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestLines_ProductID",
                table: "PurchaseRequestLines",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestLines_PurchaseRequestID",
                table: "PurchaseRequestLines",
                column: "PurchaseRequestID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestLines_UnitSetID",
                table: "PurchaseRequestLines",
                column: "UnitSetID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_BranchID",
                table: "PurchaseRequests",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_CurrencyID",
                table: "PurchaseRequests",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_CurrentAccountCardID",
                table: "PurchaseRequests",
                column: "CurrentAccountCardID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_FicheNo",
                table: "PurchaseRequests",
                column: "FicheNo");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_PaymentPlanID",
                table: "PurchaseRequests",
                column: "PaymentPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_ShippingAdressID",
                table: "PurchaseRequests",
                column: "ShippingAdressID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_WarehouseID",
                table: "PurchaseRequests",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseUnsuitabilityReports_CurrentAccountCardID",
                table: "PurchaseUnsuitabilityReports",
                column: "CurrentAccountCardID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseUnsuitabilityReports_FicheNo",
                table: "PurchaseUnsuitabilityReports",
                column: "FicheNo");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseUnsuitabilityReports_OrderID",
                table: "PurchaseUnsuitabilityReports",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseUnsuitabilityReports_ProductID",
                table: "PurchaseUnsuitabilityReports",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasingUnsuitabilityItems_Code",
                table: "PurchasingUnsuitabilityItems",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_RouteLines_ProductID",
                table: "RouteLines",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_RouteLines_ProductsOperationID",
                table: "RouteLines",
                column: "ProductsOperationID");

            migrationBuilder.CreateIndex(
                name: "IX_RouteLines_RouteID",
                table: "RouteLines",
                column: "RouteID");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_Code",
                table: "Routes",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_ProductID",
                table: "Routes",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderLines_PaymentPlanID",
                table: "SalesOrderLines",
                column: "PaymentPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderLines_ProductID",
                table: "SalesOrderLines",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderLines_SalesOrderID",
                table: "SalesOrderLines",
                column: "SalesOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderLines_SalesPropositionLinesId",
                table: "SalesOrderLines",
                column: "SalesPropositionLinesId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderLines_UnitSetID",
                table: "SalesOrderLines",
                column: "UnitSetID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_BranchID",
                table: "SalesOrders",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_CurrencyID",
                table: "SalesOrders",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_CurrentAccountCardID",
                table: "SalesOrders",
                column: "CurrentAccountCardID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_FicheNo",
                table: "SalesOrders",
                column: "FicheNo");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_PaymentPlanID",
                table: "SalesOrders",
                column: "PaymentPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_ShippingAdressID",
                table: "SalesOrders",
                column: "ShippingAdressID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_WarehouseID",
                table: "SalesOrders",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositionLines_PaymentPlanID",
                table: "SalesPropositionLines",
                column: "PaymentPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositionLines_ProductID",
                table: "SalesPropositionLines",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositionLines_PurchaseRequestsId",
                table: "SalesPropositionLines",
                column: "PurchaseRequestsId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositionLines_SalesPropositionID",
                table: "SalesPropositionLines",
                column: "SalesPropositionID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositionLines_UnitSetID",
                table: "SalesPropositionLines",
                column: "UnitSetID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositions_BranchID",
                table: "SalesPropositions",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositions_CurrencyID",
                table: "SalesPropositions",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositions_CurrentAccountCardID",
                table: "SalesPropositions",
                column: "CurrentAccountCardID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositions_FicheNo",
                table: "SalesPropositions",
                column: "FicheNo");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositions_PaymentPlanID",
                table: "SalesPropositions",
                column: "PaymentPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositions_ShippingAdressID",
                table: "SalesPropositions",
                column: "ShippingAdressID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositions_WarehouseID",
                table: "SalesPropositions",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftLines_ShiftID",
                table: "ShiftLines",
                column: "ShiftID");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_Code",
                table: "Shifts",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingAdresses_Code",
                table: "ShippingAdresses",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingAdresses_CustomerCardID",
                table: "ShippingAdresses",
                column: "CustomerCardID");

            migrationBuilder.CreateIndex(
                name: "IX_StationGroups_Code",
                table: "StationGroups",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_Code",
                table: "Stations",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_GroupID",
                table: "Stations",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateOperationLines_StationID",
                table: "TemplateOperationLines",
                column: "StationID");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateOperationLines_TemplateOperationID",
                table: "TemplateOperationLines",
                column: "TemplateOperationID");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateOperations_Code",
                table: "TemplateOperations",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_UnitSets_Code",
                table: "UnitSets",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_Code",
                table: "Warehouses",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_Code",
                table: "WorkOrders",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_CurrentAccountCardID",
                table: "WorkOrders",
                column: "CurrentAccountCardID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_ProductID",
                table: "WorkOrders",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_ProductionOrderID",
                table: "WorkOrders",
                column: "ProductionOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_ProductsOperationID",
                table: "WorkOrders",
                column: "ProductsOperationID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_PropositionID",
                table: "WorkOrders",
                column: "PropositionID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_RouteID",
                table: "WorkOrders",
                column: "RouteID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_StationGroupID",
                table: "WorkOrders",
                column: "StationGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_StationID",
                table: "WorkOrders",
                column: "StationID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillsofMaterialLines");

            migrationBuilder.DropTable(
                name: "CalendarDays");

            migrationBuilder.DropTable(
                name: "CalendarLines");

            migrationBuilder.DropTable(
                name: "CalibrationRecords");

            migrationBuilder.DropTable(
                name: "CalibrationVerifications");

            migrationBuilder.DropTable(
                name: "ContractUnsuitabilityItems");

            migrationBuilder.DropTable(
                name: "CustomerComplaintItems");

            migrationBuilder.DropTable(
                name: "ExchangeRates");

            migrationBuilder.DropTable(
                name: "FinalControlUnsuitabilityItems");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "OperationUnsuitabilityItems");

            migrationBuilder.DropTable(
                name: "OperationUnsuitabilityReports");

            migrationBuilder.DropTable(
                name: "Periods");

            migrationBuilder.DropTable(
                name: "ProductionOrderChangeItems");

            migrationBuilder.DropTable(
                name: "ProductsOperationLines");

            migrationBuilder.DropTable(
                name: "PurchaseOrderLines");

            migrationBuilder.DropTable(
                name: "PurchaseRequestLines");

            migrationBuilder.DropTable(
                name: "PurchaseUnsuitabilityReports");

            migrationBuilder.DropTable(
                name: "PurchasingUnsuitabilityItems");

            migrationBuilder.DropTable(
                name: "RouteLines");

            migrationBuilder.DropTable(
                name: "ShiftLines");

            migrationBuilder.DropTable(
                name: "TemplateOperationLines");

            migrationBuilder.DropTable(
                name: "VsmSchemas");

            migrationBuilder.DropTable(
                name: "Calendars");

            migrationBuilder.DropTable(
                name: "EquipmentRecords");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "WorkOrders");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "TemplateOperations");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "ProductionOrders");

            migrationBuilder.DropTable(
                name: "ProductsOperations");

            migrationBuilder.DropTable(
                name: "Stations");

            migrationBuilder.DropTable(
                name: "BillsofMaterials");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "SalesOrderLines");

            migrationBuilder.DropTable(
                name: "StationGroups");

            migrationBuilder.DropTable(
                name: "SalesOrders");

            migrationBuilder.DropTable(
                name: "SalesPropositionLines");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "PurchaseRequests");

            migrationBuilder.DropTable(
                name: "SalesPropositions");

            migrationBuilder.DropTable(
                name: "ProductGroups");

            migrationBuilder.DropTable(
                name: "UnitSets");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "PaymentPlans");

            migrationBuilder.DropTable(
                name: "ShippingAdresses");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "CurrentAccountCards");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
