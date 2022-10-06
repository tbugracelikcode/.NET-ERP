using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class databaseCreated : Migration
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
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    ProductionPoolID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
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
                    table.PrimaryKey("PK_Operations", x => x.Id);
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
                name: "TsiMenus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    MenuName = table.Column<string>(type: "NVarChar(250)", maxLength: 250, nullable: false),
                    ParentMenutId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TsiMenus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TsiRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleName = table.Column<string>(type: "NVarChar(250)", maxLength: 250, nullable: false),
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
                    table.PrimaryKey("PK_TsiRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TsiUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "NVarChar(250)", maxLength: 250, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(250)", maxLength: 250, nullable: false),
                    Surname = table.Column<string>(type: "NVarChar(250)", maxLength: 250, nullable: false),
                    Email = table.Column<string>(type: "NVarChar(250)", maxLength: 250, nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "Bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "NVarChar(95)", maxLength: 95, nullable: false),
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
                    table.PrimaryKey("PK_TsiUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TsiUserRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
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
                    table.PrimaryKey("PK_TsiUserRoles", x => x.Id);
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
                name: "TsiRolePermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    MenuId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
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
                    table.PrimaryKey("PK_TsiRolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TsiRolePermissions_TsiMenus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "TsiMenus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TsiRolePermissions_TsiRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "TsiRoles",
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
                name: "OperationLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    OperationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    StationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Priority = table.Column<int>(type: "Int", nullable: false),
                    ProcessQuantity = table.Column<int>(type: "Int", nullable: false),
                    AdjustmentAndControlTime = table.Column<int>(type: "Int", nullable: false),
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
                    table.PrimaryKey("PK_OperationLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationLines_Operations_OperationID",
                        column: x => x.OperationID,
                        principalTable: "Operations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OperationLines_Stations_StationID",
                        column: x => x.StationID,
                        principalTable: "Stations",
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
                name: "SalesPropositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FicheNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_ = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Description_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropositionRevisionNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RevisionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevisionTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesPropositionState = table.Column<int>(type: "int", nullable: false),
                    LinkedSalesPropositionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentPlanID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentAccountCardID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GrossAmount = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalVatExcludedAmount = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalVatAmount = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalDiscountAmount = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ValidityDate_ = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShippingAdressID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BranchesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WarehousesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrenciesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentAccountCardsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShippingAdressesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                        name: "FK_SalesPropositions_Branches_BranchesId",
                        column: x => x.BranchesId,
                        principalTable: "Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesPropositions_Currencies_CurrenciesId",
                        column: x => x.CurrenciesId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesPropositions_CurrentAccountCards_CurrentAccountCardsId",
                        column: x => x.CurrentAccountCardsId,
                        principalTable: "CurrentAccountCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesPropositions_PaymentPlans_PaymentPlanID",
                        column: x => x.PaymentPlanID,
                        principalTable: "PaymentPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesPropositions_ShippingAdresses_ShippingAdressesId",
                        column: x => x.ShippingAdressesId,
                        principalTable: "ShippingAdresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesPropositions_Warehouses_WarehousesId",
                        column: x => x.WarehousesId,
                        principalTable: "Warehouses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RouteLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    RouteID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    OperationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductionPoolID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductionPoolDescription = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    AdjustmentAndControlTime = table.Column<int>(type: "Int", nullable: false),
                    OperationTime = table.Column<int>(type: "Int", nullable: false),
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
                        name: "FK_RouteLines_Operations_OperationID",
                        column: x => x.OperationID,
                        principalTable: "Operations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RouteLines_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RouteLines_Routes_RouteID",
                        column: x => x.RouteID,
                        principalTable: "Routes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SalesPropositionLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalesPropositionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LineNr = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitSetID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    DiscountRate = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    LineAmount = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    LineTotalAmount = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    LineDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VATrate = table.Column<int>(type: "int", nullable: false),
                    VATamount = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PaymentPlanID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalesPropositionLineState = table.Column<int>(type: "int", nullable: false),
                    OrderConversionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WarehouseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ProductsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UnitSetsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BranchesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PaymentPlansId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WarehousesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SalesPropositionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                        name: "FK_SalesPropositionLines_Branches_BranchesId",
                        column: x => x.BranchesId,
                        principalTable: "Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesPropositionLines_PaymentPlans_PaymentPlansId",
                        column: x => x.PaymentPlansId,
                        principalTable: "PaymentPlans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesPropositionLines_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesPropositionLines_SalesPropositions_SalesPropositionsId",
                        column: x => x.SalesPropositionsId,
                        principalTable: "SalesPropositions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesPropositionLines_UnitSets_UnitSetsId",
                        column: x => x.UnitSetsId,
                        principalTable: "UnitSets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesPropositionLines_Warehouses_WarehousesId",
                        column: x => x.WarehousesId,
                        principalTable: "Warehouses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Branches_Code",
                table: "Branches",
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
                name: "IX_OperationLines_Code",
                table: "OperationLines",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_OperationLines_OperationID",
                table: "OperationLines",
                column: "OperationID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationLines_StationID",
                table: "OperationLines",
                column: "StationID");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_Code",
                table: "Operations",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_OperationUnsuitabilityItems_Code",
                table: "OperationUnsuitabilityItems",
                column: "Code");

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
                name: "IX_PurchasingUnsuitabilityItems_Code",
                table: "PurchasingUnsuitabilityItems",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_RouteLines_Code",
                table: "RouteLines",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_RouteLines_OperationID",
                table: "RouteLines",
                column: "OperationID");

            migrationBuilder.CreateIndex(
                name: "IX_RouteLines_ProductID",
                table: "RouteLines",
                column: "ProductID");

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
                column: "ProductID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositionLines_BranchesId",
                table: "SalesPropositionLines",
                column: "BranchesId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositionLines_PaymentPlansId",
                table: "SalesPropositionLines",
                column: "PaymentPlansId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositionLines_ProductsId",
                table: "SalesPropositionLines",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositionLines_SalesPropositionsId",
                table: "SalesPropositionLines",
                column: "SalesPropositionsId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositionLines_UnitSetsId",
                table: "SalesPropositionLines",
                column: "UnitSetsId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositionLines_WarehousesId",
                table: "SalesPropositionLines",
                column: "WarehousesId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositions_BranchesId",
                table: "SalesPropositions",
                column: "BranchesId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositions_CurrenciesId",
                table: "SalesPropositions",
                column: "CurrenciesId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositions_CurrentAccountCardsId",
                table: "SalesPropositions",
                column: "CurrentAccountCardsId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositions_PaymentPlanID",
                table: "SalesPropositions",
                column: "PaymentPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositions_ShippingAdressesId",
                table: "SalesPropositions",
                column: "ShippingAdressesId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositions_WarehousesId",
                table: "SalesPropositions",
                column: "WarehousesId");

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
                name: "IX_TsiRolePermissions_MenuId",
                table: "TsiRolePermissions",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_TsiRolePermissions_RoleId",
                table: "TsiRolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitSets_Code",
                table: "UnitSets",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_Code",
                table: "Warehouses",
                column: "Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalibrationRecords");

            migrationBuilder.DropTable(
                name: "CalibrationVerifications");

            migrationBuilder.DropTable(
                name: "ContractUnsuitabilityItems");

            migrationBuilder.DropTable(
                name: "CustomerComplaintItems");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "ExchangeRates");

            migrationBuilder.DropTable(
                name: "FinalControlUnsuitabilityItems");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "OperationLines");

            migrationBuilder.DropTable(
                name: "OperationUnsuitabilityItems");

            migrationBuilder.DropTable(
                name: "Periods");

            migrationBuilder.DropTable(
                name: "ProductionOrderChangeItems");

            migrationBuilder.DropTable(
                name: "PurchasingUnsuitabilityItems");

            migrationBuilder.DropTable(
                name: "RouteLines");

            migrationBuilder.DropTable(
                name: "SalesPropositionLines");

            migrationBuilder.DropTable(
                name: "TsiRolePermissions");

            migrationBuilder.DropTable(
                name: "TsiUser");

            migrationBuilder.DropTable(
                name: "TsiUserRoles");

            migrationBuilder.DropTable(
                name: "VsmSchemas");

            migrationBuilder.DropTable(
                name: "EquipmentRecords");

            migrationBuilder.DropTable(
                name: "Stations");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "SalesPropositions");

            migrationBuilder.DropTable(
                name: "TsiMenus");

            migrationBuilder.DropTable(
                name: "TsiRoles");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "StationGroups");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "PaymentPlans");

            migrationBuilder.DropTable(
                name: "ShippingAdresses");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "ProductGroups");

            migrationBuilder.DropTable(
                name: "UnitSets");

            migrationBuilder.DropTable(
                name: "CurrentAccountCards");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
