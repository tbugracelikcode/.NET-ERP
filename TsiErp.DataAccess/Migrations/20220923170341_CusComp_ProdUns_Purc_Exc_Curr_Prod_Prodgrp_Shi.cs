using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class CusComp_ProdUns_Purc_Exc_Curr_Prod_Prodgrp_Shi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "ExchangeRates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "DateTime", nullable: false),
                    BuyingRate = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    SaleRate = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    EffectiveBuyingRate = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    EffectiveSaleRate = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
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
                    _Default = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
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
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    SupplyForm = table.Column<int>(type: "Int", nullable: false),
                    ProductSize = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    GTIP = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    SawWastage = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
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
                    CoatingWeight = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
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
                name: "SalesPropositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FicheNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_ = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    GrossAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalVatExcludedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalVatAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                name: "SalesPropositionLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalesPropositionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LineNr = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitSetID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LineAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LineTotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LineDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VATrate = table.Column<int>(type: "int", nullable: false),
                    VATamount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentPlanID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalesPropositionLineState = table.Column<int>(type: "int", nullable: false),
                    OrderConversionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WarehouseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                name: "IX_ExchangeRates_CurrencyID",
                table: "ExchangeRates",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_Date",
                table: "ExchangeRates",
                column: "Date");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerComplaintItems");

            migrationBuilder.DropTable(
                name: "ExchangeRates");

            migrationBuilder.DropTable(
                name: "ProductionOrderChangeItems");

            migrationBuilder.DropTable(
                name: "PurchasingUnsuitabilityItems");

            migrationBuilder.DropTable(
                name: "SalesPropositionLines");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "SalesPropositions");

            migrationBuilder.DropTable(
                name: "ProductGroups");

            migrationBuilder.DropTable(
                name: "ShippingAdresses");

            migrationBuilder.DropTable(
                name: "CurrentAccountCards");
        }
    }
}
