using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class _20092022_DatabaseUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    KWA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GroupID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    X = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Y = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    AreaCovered = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    UsageArea = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Amortization = table.Column<int>(type: "Int", nullable: false),
                    MachineCost = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Shift = table.Column<int>(type: "Int", nullable: false),
                    ShiftWorkingTime = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    PowerFactor = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    WorkSafetyInstruction = table.Column<byte[]>(type: "varbinary(MAX)", nullable: true),
                    UsageInstruction = table.Column<byte[]>(type: "varbinary(MAX)", nullable: true),
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
                    table.PrimaryKey("PK_Stations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stations_StationGroups_GroupID",
                        column: x => x.GroupID,
                        principalTable: "StationGroups",
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
                name: "IX_UnitSets_Code",
                table: "UnitSets",
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
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Stations");

            migrationBuilder.DropTable(
                name: "UnitSets");

            migrationBuilder.DropTable(
                name: "EquipmentRecords");

            migrationBuilder.DropTable(
                name: "StationGroups");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
