using Microsoft.SqlServer.Management.Smo;
using System.Reflection;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using Tsi.Core.Utilities.VersionUtilities;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.DatabaseSchemes;
using TsiErp.DataAccess.Utilities;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ExchangeRate;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.FinanceManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.GeneralParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.MachineAndWorkforceManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.MaintenanceManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Period;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.NotificationTemplate;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.PlanningManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ProductionManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.PurchaseManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.QualityControlParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.SalesManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Shift;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ShiftLine;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ShippingManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.StockManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.User;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserGroup;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Version;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationInventory;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstruction;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstructionLine;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenancePeriod;
using TsiErp.Entities.Entities.MaintenanceManagement.PlannedMaintenance;
using TsiErp.Entities.Entities.MaintenanceManagement.PlannedMaintenanceLine;
using TsiErp.Entities.Entities.MaintenanceManagement.UnplannedMaintenance;
using TsiErp.Entities.Entities.MaintenanceManagement.UnplannedMaintenanceLine;
using TsiErp.Entities.Entities.Other.ByDateStockMovement;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement;
using TsiErp.Entities.Entities.Other.Logging;
using TsiErp.Entities.Entities.PlanningManagement.Calendar;
using TsiErp.Entities.Entities.PlanningManagement.CalendarDay;
using TsiErp.Entities.Entities.PlanningManagement.CalendarLine;
using TsiErp.Entities.Entities.PlanningManagement.MRP;
using TsiErp.Entities.Entities.PlanningManagement.MRPLine;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine;
using TsiErp.Entities.Entities.ProductionManagement.ContractProductionTracking;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperationLine;
using TsiErp.Entities.Entities.ProductionManagement.Route;
using TsiErp.Entities.Entities.ProductionManagement.RouteLine;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFiche;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheLine;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperation;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperationLine;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperationUnsuitabilityItem;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine;
using TsiErp.Entities.Entities.PurchaseManagement.PurchasePrice;
using TsiErp.Entities.Entities.PurchaseManagement.PurchasePriceLine;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequest;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequestLine;
using TsiErp.Entities.Entities.QualityControl.CalibrationRecord;
using TsiErp.Entities.Entities.QualityControl.CalibrationVerification;
using TsiErp.Entities.Entities.QualityControl.ControlCondition;
using TsiErp.Entities.Entities.QualityControl.ControlType;
using TsiErp.Entities.Entities.QualityControl.EquipmentRecord;
using TsiErp.Entities.Entities.QualityControl.FinalControlUnsuitabilityReport;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan;
using TsiErp.Entities.Entities.QualityControl.OperationUnsuitabilityReport;
using TsiErp.Entities.Entities.QualityControl.PurchaseUnsuitabilityReport;
using TsiErp.Entities.Entities.QualityControl.ContractUnsuitabilityReport;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlanLine;
using TsiErp.Entities.Entities.QualityControl.OperationPicture;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlanLine;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlanOperation;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlan;
using TsiErp.Entities.Entities.QualityControl.ContractOperationPicture;
using TsiErp.Entities.Entities.QualityControl.Report8D;
using TsiErp.Entities.Entities.QualityControl.FirstProductApproval;
using TsiErp.Entities.Entities.QualityControl.FirstProductApprovalLine;
using TsiErp.Entities.Entities.QualityControl.OperationalSPC;
using TsiErp.Entities.Entities.QualityControl.OperationalSPCLine;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPC;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPCLine;
using TsiErp.Entities.Entities.QualityControl.PFMEA;
using TsiErp.Entities.Entities.QualityControl.CustomerComplaintReport;
using TsiErp.Entities.Entities.SalesManagement.Forecast;
using TsiErp.Entities.Entities.SalesManagement.ForecastLine;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine;
using TsiErp.Entities.Entities.SalesManagement.SalesPrice;
using TsiErp.Entities.Entities.SalesManagement.SalesPriceLine;
using TsiErp.Entities.Entities.SalesManagement.SalesProposition;
using TsiErp.Entities.Entities.SalesManagement.SalesPropositionLine;
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.ProductGroup;
using TsiErp.Entities.Entities.StockManagement.ProductReferanceNumber;
using TsiErp.Entities.Entities.StockManagement.StockFiche;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.TableConstant;
using TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlan;
using TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlanLine;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.FicheNumber;
using TsiErp.Entities.Entities.ProductionManagement.OperationStockMovement;
using TsiErp.Entities.Entities.ShippingManagement.PackageFiche;
using TsiErp.Entities.Entities.ShippingManagement.PackageFicheLine;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheAmountEntryLine;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecordLine;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecord;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceMRP;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceMRPLine;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeAnnualSeniorityDifference;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EducationLevelScore;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeGeneralSkillRecord;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.TaskScoring;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.GeneralSkillRecordPriority;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StartingSalary;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StartingSalaryLine;
using TsiErp.Entities.Entities.ShippingManagement.PackingList;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletCubageLine;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletLine;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletPackageLine;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeScoring;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeScoringLine;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeOperation;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecord;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecordLine;
using TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanning;
using TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanningLine;
using TsiErp.Entities.Entities.StockManagement.ProductCost;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationOccupancy;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationOccupancyLine;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationOccupancyHistory;
using TsiErp.Entities.Entities.PlanningManagement.MRPII;
using TsiErp.Entities.Entities.PlanningManagement.MRPIILine;
using TsiErp.Entities.Entities.StockManagement.StockColumn;
using TsiErp.Entities.Entities.StockManagement.StockNumber;
using TsiErp.Entities.Entities.StockManagement.StockSection;
using TsiErp.Entities.Entities.StockManagement.StockShelf;
using TsiErp.Entities.Entities.StockManagement.StockAddressLine;
using TsiErp.Entities.Entities.StockManagement.StockAddress;
using TsiErp.Entities.Entities.StockManagement.ProductPropertyLine;
using TsiErp.Entities.Entities.StockManagement.ProductProperty;
using TsiErp.Entities.Entities.StockManagement.ProductRelatedProductProperty;
using TsiErp.Entities.Entities.StockManagement.ProductReceiptTransaction;
using TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApproval;
using TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApprovalLine;
using TsiErp.Entities.Entities.QualityControl.ProductionOrderChangeReport;
using TsiErp.Entities.Entities.TestManagement.Continent;
using TsiErp.Entities.Entities.TestManagement.ContinentLine;
using TsiErp.Entities.Entities.TestManagement.Sector;
using TsiErp.Entities.Entities.TestManagement.SectorLine;
using TsiErp.Entities.Entities.TestManagement.District;
using TsiErp.Entities.Entities.TestManagement.City;
using System.Data;
using Microsoft.SqlServer.Management.Common;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.NotificationTemplate;
using TsiErp.Entities.Entities.Other.Notification;

namespace TsiErp.DataAccess.DatabaseSchemeHistories
{
    public class DatabaseSchemeUpdater
    {
        [Version(VersiyonNumber = "1.23.4")]
        public bool Update()
        {
            var queryFactory = new QueryFactory();

            queryFactory.ConnectToDatabase();

            DatabaseModel model = new DatabaseModel(queryFactory.Connection);

            #region Current Account Cards Table Created
            Table CurrentAccountCardsTable = model.CreateTable(Tables.CurrentAccountCards);

            if (CurrentAccountCardsTable != null)
            {
                var properties = (typeof(CurrentAccountCards)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(CurrentAccountCardsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(CurrentAccountCardsTable, "PK_" + CurrentAccountCardsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        CurrentAccountCardsTable.Indexes.Add(pkIndex);
                    }

                    CurrentAccountCardsTable.Columns.Add(column);
                }

                CurrentAccountCardsTable.Create();
            }
            #endregion

            #region PaymentPlans Table Created
            Table PaymentPlansTable = model.CreateTable(Tables.PaymentPlans);

            if (PaymentPlansTable != null)
            {
                var properties = (typeof(PaymentPlans)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PaymentPlansTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PaymentPlansTable, "PK_" + PaymentPlansTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PaymentPlansTable.Indexes.Add(pkIndex);
                    }

                    PaymentPlansTable.Columns.Add(column);
                }

                PaymentPlansTable.Create();
            }
            #endregion

            #region Branches Table Created
            Table BranchesTable = model.CreateTable(Tables.Branches);

            if (BranchesTable != null)
            {
                var properties = (typeof(Branches)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(BranchesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(BranchesTable, "PK_" + BranchesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        BranchesTable.Indexes.Add(pkIndex);
                    }

                    BranchesTable.Columns.Add(column);
                }

                BranchesTable.Create();
            }
            #endregion

            #region Currencies Table Created
            Table CurrenciesTable = model.CreateTable(Tables.Currencies);

            if (CurrenciesTable != null)
            {
                var properties = (typeof(Currencies)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(CurrenciesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(CurrenciesTable, "PK_" + CurrenciesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        CurrenciesTable.Indexes.Add(pkIndex);
                    }

                    CurrenciesTable.Columns.Add(column);
                }

                CurrenciesTable.Create();
            }
            #endregion

            #region ExchangeRates Table Created
            Table ExchangeRatesTable = model.CreateTable(Tables.ExchangeRates);

            if (ExchangeRatesTable != null)
            {
                var properties = (typeof(ExchangeRates)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ExchangeRatesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ExchangeRatesTable, "PK_" + ExchangeRatesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ExchangeRatesTable.Indexes.Add(pkIndex);
                    }

                    ExchangeRatesTable.Columns.Add(column);
                }

                ExchangeRatesTable.Create();
            }
            #endregion

            #region Menus Table Created
            Table MenusTable = model.CreateTable(Tables.Menus);

            if (MenusTable != null)
            {
                var properties = (typeof(Menus)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(MenusTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(MenusTable, "PK_" + MenusTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        MenusTable.Indexes.Add(pkIndex);
                    }

                    MenusTable.Columns.Add(column);
                }

                MenusTable.Create();
            }
            #endregion

            #region Fiche Numbers Table Created
            Table FicheNumbersTable = model.CreateTable(Tables.FicheNumbers);

            if (FicheNumbersTable != null)
            {
                var properties = (typeof(FicheNumbers)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(FicheNumbersTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(FicheNumbersTable, "PK_" + FicheNumbersTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        FicheNumbersTable.Indexes.Add(pkIndex);
                    }

                    FicheNumbersTable.Columns.Add(column);
                }

                FicheNumbersTable.Create();
            }
            #endregion

            #region Periods Table Created
            Table PeriodsTable = model.CreateTable(Tables.Periods);

            if (PeriodsTable != null)
            {
                var properties = (typeof(Periods)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PeriodsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PeriodsTable, "PK_" + PeriodsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PeriodsTable.Indexes.Add(pkIndex);
                    }

                    PeriodsTable.Columns.Add(column);
                }

                PeriodsTable.Create();
            }
            #endregion

            #region Shifts Table Created
            Table ShiftsTable = model.CreateTable(Tables.Shifts);

            if (ShiftsTable != null)
            {
                var properties = (typeof(Shifts)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ShiftsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ShiftsTable, "PK_" + ShiftsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ShiftsTable.Indexes.Add(pkIndex);
                    }

                    ShiftsTable.Columns.Add(column);
                }

                ShiftsTable.Create();
            }
            #endregion

            #region Shift Lines Table Created
            Table ShiftLinesTable = model.CreateTable(Tables.ShiftLines);

            if (ShiftLinesTable != null)
            {
                var properties = (typeof(ShiftLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ShiftLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ShiftLinesTable, "PK_" + ShiftLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ShiftLinesTable.Indexes.Add(pkIndex);
                    }

                    ShiftLinesTable.Columns.Add(column);
                }

                ShiftLinesTable.Create();
            }
            #endregion

            #region Users Table Created
            Table UsersTable = model.CreateTable(Tables.Users);

            if (UsersTable != null)
            {
                var properties = (typeof(Users)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(UsersTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(UsersTable, "PK_" + UsersTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        UsersTable.Indexes.Add(pkIndex);
                    }

                    UsersTable.Columns.Add(column);
                }

                UsersTable.Create();
            }
            #endregion

            #region User Groups Table Created
            Table UserGroupsTable = model.CreateTable(Tables.UserGroups);

            if (UserGroupsTable != null)
            {
                var properties = (typeof(UserGroups)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(UserGroupsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(UserGroupsTable, "PK_" + UserGroupsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        UserGroupsTable.Indexes.Add(pkIndex);
                    }

                    UserGroupsTable.Columns.Add(column);
                }

                UserGroupsTable.Create();
            }
            #endregion

            #region Prog Versions Table Created
            Table ProgVersionsTable = model.CreateTable(Tables.ProgVersions);

            if (ProgVersionsTable != null)
            {
                var properties = (typeof(ProgVersions)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ProgVersionsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ProgVersionsTable, "PK_" + ProgVersionsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ProgVersionsTable.Indexes.Add(pkIndex);
                    }

                    ProgVersionsTable.Columns.Add(column);
                }

                ProgVersionsTable.Create();
            }
            #endregion

            #region Departments Table Created
            Table DepartmentsTable = model.CreateTable(Tables.Departments);

            if (DepartmentsTable != null)
            {
                var properties = (typeof(Departments)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(DepartmentsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(DepartmentsTable, "PK_" + DepartmentsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        DepartmentsTable.Indexes.Add(pkIndex);
                    }

                    DepartmentsTable.Columns.Add(column);
                }

                DepartmentsTable.Create();
            }
            #endregion

            #region Employees Table Created
            Table EmployeesTable = model.CreateTable(Tables.Employees);

            if (EmployeesTable != null)
            {
                var properties = (typeof(Employees)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(EmployeesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(EmployeesTable, "PK_" + EmployeesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        EmployeesTable.Indexes.Add(pkIndex);
                    }

                    EmployeesTable.Columns.Add(column);
                }

                EmployeesTable.Create();
            }
            #endregion

            #region Stations Table Created
            Table StationsTable = model.CreateTable(Tables.Stations);

            if (StationsTable != null)
            {
                var properties = (typeof(Stations)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(StationsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(StationsTable, "PK_" + StationsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        StationsTable.Indexes.Add(pkIndex);
                    }

                    StationsTable.Columns.Add(column);
                }

                StationsTable.Create();
            }
            #endregion

            #region StationGroups Table Created
            Table StationGroupsTable = model.CreateTable(Tables.StationGroups);

            if (StationGroupsTable != null)
            {
                var properties = (typeof(StationGroups)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(StationGroupsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(StationGroupsTable, "PK_" + StationGroupsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        StationGroupsTable.Indexes.Add(pkIndex);
                    }

                    StationGroupsTable.Columns.Add(column);
                }

                StationGroupsTable.Create();
            }
            #endregion

            #region StationInventories Table Created
            Table StationInventoriesTable = model.CreateTable(Tables.StationInventories);

            if (StationInventoriesTable != null)
            {
                var properties = (typeof(StationInventories)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(StationInventoriesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(StationInventoriesTable, "PK_" + StationInventoriesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        StationInventoriesTable.Indexes.Add(pkIndex);
                    }

                    StationInventoriesTable.Columns.Add(column);
                }

                StationInventoriesTable.Create();
            }
            #endregion

            #region MaintenanceInstructions Table Created
            Table MaintenanceInstructionsTable = model.CreateTable(Tables.MaintenanceInstructions);

            if (MaintenanceInstructionsTable != null)
            {
                var properties = (typeof(MaintenanceInstructions)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(MaintenanceInstructionsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(MaintenanceInstructionsTable, "PK_" + MaintenanceInstructionsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        MaintenanceInstructionsTable.Indexes.Add(pkIndex);
                    }

                    MaintenanceInstructionsTable.Columns.Add(column);
                }

                MaintenanceInstructionsTable.Create();
            }
            #endregion

            #region MaintenanceInstructionLines Table Created
            Table MaintenanceInstructionLinesTable = model.CreateTable(Tables.MaintenanceInstructionLines);

            if (MaintenanceInstructionLinesTable != null)
            {
                var properties = (typeof(MaintenanceInstructionLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(MaintenanceInstructionLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(MaintenanceInstructionLinesTable, "PK_" + MaintenanceInstructionLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        MaintenanceInstructionLinesTable.Indexes.Add(pkIndex);
                    }

                    MaintenanceInstructionLinesTable.Columns.Add(column);
                }

                MaintenanceInstructionLinesTable.Create();
            }
            #endregion

            #region MaintenancePeriods Table Created
            Table MaintenancePeriodsTable = model.CreateTable(Tables.MaintenancePeriods);

            if (MaintenancePeriodsTable != null)
            {
                var properties = (typeof(MaintenancePeriods)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(MaintenancePeriodsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(MaintenancePeriodsTable, "PK_" + MaintenancePeriodsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        MaintenancePeriodsTable.Indexes.Add(pkIndex);
                    }

                    MaintenancePeriodsTable.Columns.Add(column);
                }

                MaintenancePeriodsTable.Create();
            }
            #endregion

            #region PlannedMaintenances Table Created
            Table PlannedMaintenancesTable = model.CreateTable(Tables.PlannedMaintenances);

            if (PlannedMaintenancesTable != null)
            {
                var properties = (typeof(PlannedMaintenances)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PlannedMaintenancesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PlannedMaintenancesTable, "PK_" + PlannedMaintenancesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PlannedMaintenancesTable.Indexes.Add(pkIndex);
                    }

                    PlannedMaintenancesTable.Columns.Add(column);
                }

                PlannedMaintenancesTable.Create();
            }
            #endregion

            #region PlannedMaintenanceLines Table Created
            Table PlannedMaintenanceLinesTable = model.CreateTable(Tables.PlannedMaintenanceLines);

            if (PlannedMaintenanceLinesTable != null)
            {
                var properties = (typeof(PlannedMaintenanceLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PlannedMaintenanceLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PlannedMaintenanceLinesTable, "PK_" + PlannedMaintenanceLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PlannedMaintenanceLinesTable.Indexes.Add(pkIndex);
                    }

                    PlannedMaintenanceLinesTable.Columns.Add(column);
                }

                PlannedMaintenanceLinesTable.Create();
            }
            #endregion

            #region UnplannedMaintenances Table Created
            Table UnplannedMaintenancesTable = model.CreateTable(Tables.UnplannedMaintenances);

            if (UnplannedMaintenancesTable != null)
            {
                var properties = (typeof(UnplannedMaintenances)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(UnplannedMaintenancesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(UnplannedMaintenancesTable, "PK_" + UnplannedMaintenancesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        UnplannedMaintenancesTable.Indexes.Add(pkIndex);
                    }

                    UnplannedMaintenancesTable.Columns.Add(column);
                }

                UnplannedMaintenancesTable.Create();
            }
            #endregion

            #region UnplannedMaintenanceLines Table Created
            Table UnplannedMaintenanceLinesTable = model.CreateTable(Tables.UnplannedMaintenanceLines);

            if (UnplannedMaintenanceLinesTable != null)
            {
                var properties = (typeof(UnplannedMaintenanceLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(UnplannedMaintenanceLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(UnplannedMaintenanceLinesTable, "PK_" + UnplannedMaintenanceLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        UnplannedMaintenanceLinesTable.Indexes.Add(pkIndex);
                    }

                    UnplannedMaintenanceLinesTable.Columns.Add(column);
                }

                UnplannedMaintenanceLinesTable.Create();
            }
            #endregion

            #region ByDateStockMovements Table Created
            Table ByDateStockMovementsTable = model.CreateTable(Tables.ByDateStockMovements);

            if (ByDateStockMovementsTable != null)
            {
                var properties = (typeof(ByDateStockMovements)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ByDateStockMovementsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ByDateStockMovementsTable, "PK_" + ByDateStockMovementsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ByDateStockMovementsTable.Indexes.Add(pkIndex);
                    }

                    ByDateStockMovementsTable.Columns.Add(column);
                }

                ByDateStockMovementsTable.Create();
            }
            #endregion

            #region GrandTotalStockMovements Table Created
            Table GrandTotalStockMovementsTable = model.CreateTable(Tables.GrandTotalStockMovements);

            if (GrandTotalStockMovementsTable != null)
            {
                var properties = (typeof(GrandTotalStockMovements)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(GrandTotalStockMovementsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(GrandTotalStockMovementsTable, "PK_" + GrandTotalStockMovementsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        GrandTotalStockMovementsTable.Indexes.Add(pkIndex);
                    }

                    GrandTotalStockMovementsTable.Columns.Add(column);
                }

                GrandTotalStockMovementsTable.Create();
            }
            #endregion

            #region Calendars Table Created
            Table CalendarsTable = model.CreateTable(Tables.Calendars);

            if (CalendarsTable != null)
            {
                var properties = (typeof(Calendars)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(CalendarsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(CalendarsTable, "PK_" + CalendarsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        CalendarsTable.Indexes.Add(pkIndex);
                    }

                    CalendarsTable.Columns.Add(column);
                }

                CalendarsTable.Create();
            }
            #endregion

            #region CalendarDays Table Created
            Table CalendarDaysTable = model.CreateTable(Tables.CalendarDays);

            if (CalendarDaysTable != null)
            {
                var properties = (typeof(CalendarDays)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(CalendarDaysTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(CalendarDaysTable, "PK_" + CalendarDaysTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        CalendarDaysTable.Indexes.Add(pkIndex);
                    }

                    CalendarDaysTable.Columns.Add(column);
                }

                CalendarDaysTable.Create();
            }
            #endregion

            #region CalendarLines Table Created
            Table CalendarLinesTable = model.CreateTable(Tables.CalendarLines);

            if (CalendarLinesTable != null)
            {
                var properties = (typeof(CalendarLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(CalendarLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(CalendarLinesTable, "PK_" + CalendarLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        CalendarLinesTable.Indexes.Add(pkIndex);
                    }

                    CalendarLinesTable.Columns.Add(column);
                }

                CalendarLinesTable.Create();
            }
            #endregion

            #region BillsofMaterials Table Created
            Table BillsofMaterialsTable = model.CreateTable(Tables.BillsofMaterials);

            if (BillsofMaterialsTable != null)
            {
                var properties = (typeof(BillsofMaterials)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(BillsofMaterialsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(BillsofMaterialsTable, "PK_" + BillsofMaterialsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        BillsofMaterialsTable.Indexes.Add(pkIndex);
                    }

                    BillsofMaterialsTable.Columns.Add(column);
                }

                BillsofMaterialsTable.Create();
            }
            #endregion

            #region BillsofMaterialLines Table Created
            Table BillsofMaterialLinesTable = model.CreateTable(Tables.BillsofMaterialLines);

            if (BillsofMaterialLinesTable != null)
            {
                var properties = (typeof(BillsofMaterialLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(BillsofMaterialLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(BillsofMaterialLinesTable, "PK_" + BillsofMaterialLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        BillsofMaterialLinesTable.Indexes.Add(pkIndex);
                    }

                    BillsofMaterialLinesTable.Columns.Add(column);
                }

                BillsofMaterialLinesTable.Create();
            }
            #endregion

            #region ContractProductionTrackings Table Created
            Table ContractProductionTrackingsTable = model.CreateTable(Tables.ContractProductionTrackings);

            if (ContractProductionTrackingsTable != null)
            {
                var properties = (typeof(ContractProductionTrackings)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ContractProductionTrackingsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ContractProductionTrackingsTable, "PK_" + ContractProductionTrackingsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ContractProductionTrackingsTable.Indexes.Add(pkIndex);
                    }

                    ContractProductionTrackingsTable.Columns.Add(column);
                }

                ContractProductionTrackingsTable.Create();
            }
            #endregion

            #region HaltReasons Table Created
            Table HaltReasonsTable = model.CreateTable(Tables.HaltReasons);

            if (HaltReasonsTable != null)
            {
                var properties = (typeof(HaltReasons)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(HaltReasonsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(HaltReasonsTable, "PK_" + HaltReasonsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        HaltReasonsTable.Indexes.Add(pkIndex);
                    }

                    HaltReasonsTable.Columns.Add(column);
                }

                HaltReasonsTable.Create();
            }
            #endregion

            #region ProductionOrders Table Created
            Table ProductionOrdersTable = model.CreateTable(Tables.ProductionOrders);

            if (ProductionOrdersTable != null)
            {
                var properties = (typeof(ProductionOrders)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ProductionOrdersTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ProductionOrdersTable, "PK_" + ProductionOrdersTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ProductionOrdersTable.Indexes.Add(pkIndex);
                    }

                    ProductionOrdersTable.Columns.Add(column);
                }

                ProductionOrdersTable.Create();
            }
            #endregion

            #region ProductionTrackings Table Created
            Table ProductionTrackingsTable = model.CreateTable(Tables.ProductionTrackings);

            if (ProductionTrackingsTable != null)
            {
                var properties = (typeof(ProductionTrackings)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ProductionTrackingsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ProductionTrackingsTable, "PK_" + ProductionTrackingsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ProductionTrackingsTable.Indexes.Add(pkIndex);
                    }

                    ProductionTrackingsTable.Columns.Add(column);
                }

                ProductionTrackingsTable.Create();
            }
            #endregion

            #region ProductsOperations Table Created
            Table ProductsOperationsTable = model.CreateTable(Tables.ProductsOperations);

            if (ProductsOperationsTable != null)
            {
                var properties = (typeof(ProductsOperations)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ProductsOperationsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ProductsOperationsTable, "PK_" + ProductsOperationsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ProductsOperationsTable.Indexes.Add(pkIndex);
                    }

                    ProductsOperationsTable.Columns.Add(column);
                }

                ProductsOperationsTable.Create();
            }
            #endregion

            #region ProductsOperationLines Table Created
            Table ProductsOperationLinesTable = model.CreateTable(Tables.ProductsOperationLines);

            if (ProductsOperationLinesTable != null)
            {
                var properties = (typeof(ProductsOperationLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ProductsOperationLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ProductsOperationLinesTable, "PK_" + ProductsOperationLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ProductsOperationLinesTable.Indexes.Add(pkIndex);
                    }

                    ProductsOperationLinesTable.Columns.Add(column);
                }

                ProductsOperationLinesTable.Create();
            }
            #endregion

            #region Routes Table Created
            Table RoutesTable = model.CreateTable(Tables.Routes);

            if (RoutesTable != null)
            {
                var properties = (typeof(Routes)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(RoutesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(RoutesTable, "PK_" + RoutesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        RoutesTable.Indexes.Add(pkIndex);
                    }

                    RoutesTable.Columns.Add(column);
                }

                RoutesTable.Create();
            }
            #endregion

            #region RouteLines Table Created
            Table RouteLinesTable = model.CreateTable(Tables.RouteLines);

            if (RouteLinesTable != null)
            {
                var properties = (typeof(RouteLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(RouteLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(RouteLinesTable, "PK_" + RouteLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        RouteLinesTable.Indexes.Add(pkIndex);
                    }

                    RouteLinesTable.Columns.Add(column);
                }

                RouteLinesTable.Create();
            }
            #endregion

            #region TemplateOperations Table Created
            Table TemplateOperationsTable = model.CreateTable(Tables.TemplateOperations);

            if (TemplateOperationsTable != null)
            {
                var properties = (typeof(TemplateOperations)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(TemplateOperationsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(TemplateOperationsTable, "PK_" + TemplateOperationsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        TemplateOperationsTable.Indexes.Add(pkIndex);
                    }

                    TemplateOperationsTable.Columns.Add(column);
                }

                TemplateOperationsTable.Create();
            }
            #endregion

            #region TemplateOperationLines Table Created
            Table TemplateOperationLinesTable = model.CreateTable(Tables.TemplateOperationLines);

            if (TemplateOperationLinesTable != null)
            {
                var properties = (typeof(TemplateOperationLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(TemplateOperationLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(TemplateOperationLinesTable, "PK_" + TemplateOperationLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        TemplateOperationLinesTable.Indexes.Add(pkIndex);
                    }

                    TemplateOperationLinesTable.Columns.Add(column);
                }

                TemplateOperationLinesTable.Create();
            }
            #endregion

            #region TemplateOperationUnsuitabilityItems Table Created
            Table TemplateOperationUnsuitabilityItemsTable = model.CreateTable(Tables.TemplateOperationUnsuitabilityItems);

            if (TemplateOperationUnsuitabilityItemsTable != null)
            {
                var properties = (typeof(TemplateOperationUnsuitabilityItems)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(TemplateOperationUnsuitabilityItemsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(TemplateOperationUnsuitabilityItemsTable, "PK_" + TemplateOperationUnsuitabilityItemsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        TemplateOperationUnsuitabilityItemsTable.Indexes.Add(pkIndex);
                    }

                    TemplateOperationUnsuitabilityItemsTable.Columns.Add(column);
                }

                TemplateOperationUnsuitabilityItemsTable.Create();
            }
            #endregion

            #region WorkOrders Table Created
            Table WorkOrdersTable = model.CreateTable(Tables.WorkOrders);

            if (WorkOrdersTable != null)
            {
                var properties = (typeof(WorkOrders)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(WorkOrdersTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(WorkOrdersTable, "PK_" + WorkOrdersTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        WorkOrdersTable.Indexes.Add(pkIndex);
                    }

                    WorkOrdersTable.Columns.Add(column);
                }

                WorkOrdersTable.Create();
            }
            #endregion

            #region PurchaseOrders Table Created
            Table PurchaseOrdersTable = model.CreateTable(Tables.PurchaseOrders);

            if (PurchaseOrdersTable != null)
            {
                var properties = (typeof(PurchaseOrders)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PurchaseOrdersTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PurchaseOrdersTable, "PK_" + PurchaseOrdersTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PurchaseOrdersTable.Indexes.Add(pkIndex);
                    }

                    PurchaseOrdersTable.Columns.Add(column);
                }

                PurchaseOrdersTable.Create();
            }
            #endregion

            #region PurchaseOrderLines Table Created
            Table PurchaseOrderLinesTable = model.CreateTable(Tables.PurchaseOrderLines);

            if (PurchaseOrderLinesTable != null)
            {
                var properties = (typeof(PurchaseOrderLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PurchaseOrderLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PurchaseOrderLinesTable, "PK_" + PurchaseOrderLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PurchaseOrderLinesTable.Indexes.Add(pkIndex);
                    }

                    PurchaseOrderLinesTable.Columns.Add(column);
                }

                PurchaseOrderLinesTable.Create();
            }
            #endregion

            #region PurchasePrices Table Created
            Table PurchasePricesTable = model.CreateTable(Tables.PurchasePrices);

            if (PurchasePricesTable != null)
            {
                var properties = (typeof(PurchasePrices)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PurchasePricesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PurchasePricesTable, "PK_" + PurchasePricesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PurchasePricesTable.Indexes.Add(pkIndex);
                    }

                    PurchasePricesTable.Columns.Add(column);
                }

                PurchasePricesTable.Create();
            }
            #endregion

            #region PurchasePriceLines Table Created
            Table PurchasePriceLinesTable = model.CreateTable(Tables.PurchasePriceLines);

            if (PurchasePriceLinesTable != null)
            {
                var properties = (typeof(PurchasePriceLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PurchasePriceLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PurchasePriceLinesTable, "PK_" + PurchasePriceLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PurchasePriceLinesTable.Indexes.Add(pkIndex);
                    }

                    PurchasePriceLinesTable.Columns.Add(column);
                }

                PurchasePriceLinesTable.Create();
            }
            #endregion

            #region PurchaseRequests Table Created
            Table PurchaseRequestsTable = model.CreateTable(Tables.PurchaseRequests);

            if (PurchaseRequestsTable != null)
            {
                var properties = (typeof(PurchaseRequests)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PurchaseRequestsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PurchaseRequestsTable, "PK_" + PurchaseRequestsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PurchaseRequestsTable.Indexes.Add(pkIndex);
                    }

                    PurchaseRequestsTable.Columns.Add(column);
                }

                PurchaseRequestsTable.Create();
            }
            #endregion

            #region PurchaseRequestLines Table Created
            Table PurchaseRequestLinesTable = model.CreateTable(Tables.PurchaseRequestLines);

            if (PurchaseRequestLinesTable != null)
            {
                var properties = (typeof(PurchaseRequestLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PurchaseRequestLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PurchaseRequestLinesTable, "PK_" + PurchaseRequestLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PurchaseRequestLinesTable.Indexes.Add(pkIndex);
                    }

                    PurchaseRequestLinesTable.Columns.Add(column);
                }

                PurchaseRequestLinesTable.Create();
            }
            #endregion

            #region CalibrationRecords Table Created
            Table CalibrationRecordsTable = model.CreateTable(Tables.CalibrationRecords);

            if (CalibrationRecordsTable != null)
            {
                var properties = (typeof(CalibrationRecords)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(CalibrationRecordsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(CalibrationRecordsTable, "PK_" + CalibrationRecordsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        CalibrationRecordsTable.Indexes.Add(pkIndex);
                    }

                    CalibrationRecordsTable.Columns.Add(column);
                }

                CalibrationRecordsTable.Create();
            }
            #endregion

            #region CalibrationVerifications Table Created
            Table CalibrationVerificationsTable = model.CreateTable(Tables.CalibrationVerifications);

            if (CalibrationVerificationsTable != null)
            {
                var properties = (typeof(CalibrationVerifications)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(CalibrationVerificationsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(CalibrationVerificationsTable, "PK_" + CalibrationVerificationsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        CalibrationVerificationsTable.Indexes.Add(pkIndex);
                    }

                    CalibrationVerificationsTable.Columns.Add(column);
                }

                CalibrationVerificationsTable.Create();
            }
            #endregion

            #region EquipmentRecords Table Created
            Table EquipmentRecordsTable = model.CreateTable(Tables.EquipmentRecords);

            if (EquipmentRecordsTable != null)
            {
                var properties = (typeof(EquipmentRecords)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(EquipmentRecordsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(EquipmentRecordsTable, "PK_" + EquipmentRecordsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        EquipmentRecordsTable.Indexes.Add(pkIndex);
                    }

                    EquipmentRecordsTable.Columns.Add(column);
                }

                EquipmentRecordsTable.Create();
            }
            #endregion

            #region FinalControlUnsuitabilityReports Table Created
            Table FinalControlUnsuitabilityReportsTable = model.CreateTable(Tables.FinalControlUnsuitabilityReports);

            if (FinalControlUnsuitabilityReportsTable != null)
            {
                var properties = (typeof(FinalControlUnsuitabilityReports)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(FinalControlUnsuitabilityReportsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(FinalControlUnsuitabilityReportsTable, "PK_" + FinalControlUnsuitabilityReportsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        FinalControlUnsuitabilityReportsTable.Indexes.Add(pkIndex);
                    }

                    FinalControlUnsuitabilityReportsTable.Columns.Add(column);
                }

                FinalControlUnsuitabilityReportsTable.Create();
            }
            #endregion

            #region OperationUnsuitabilityReports Table Created
            Table OperationUnsuitabilityReportsTable = model.CreateTable(Tables.OperationUnsuitabilityReports);

            if (OperationUnsuitabilityReportsTable != null)
            {
                var properties = (typeof(OperationUnsuitabilityReports)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(OperationUnsuitabilityReportsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(OperationUnsuitabilityReportsTable, "PK_" + OperationUnsuitabilityReportsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        OperationUnsuitabilityReportsTable.Indexes.Add(pkIndex);
                    }

                    OperationUnsuitabilityReportsTable.Columns.Add(column);
                }

                OperationUnsuitabilityReportsTable.Create();
            }
            #endregion

            #region PurchaseUnsuitabilityReports Table Created
            Table PurchaseUnsuitabilityReportsTable = model.CreateTable(Tables.PurchaseUnsuitabilityReports);

            if (PurchaseUnsuitabilityReportsTable != null)
            {
                var properties = (typeof(PurchaseUnsuitabilityReports)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PurchaseUnsuitabilityReportsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PurchaseUnsuitabilityReportsTable, "PK_" + PurchaseUnsuitabilityReportsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PurchaseUnsuitabilityReportsTable.Indexes.Add(pkIndex);
                    }

                    PurchaseUnsuitabilityReportsTable.Columns.Add(column);
                }

                PurchaseUnsuitabilityReportsTable.Create();
            }
            #endregion

            #region UnsuitabilityTypesItems Table Created
            Table UnsuitabilityTypesItemsTable = model.CreateTable(Tables.UnsuitabilityTypesItems);

            if (UnsuitabilityTypesItemsTable != null)
            {
                var properties = (typeof(UnsuitabilityTypesItems)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(UnsuitabilityTypesItemsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(UnsuitabilityTypesItemsTable, "PK_" + UnsuitabilityTypesItemsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        UnsuitabilityTypesItemsTable.Indexes.Add(pkIndex);
                    }

                    UnsuitabilityTypesItemsTable.Columns.Add(column);
                }

                UnsuitabilityTypesItemsTable.Create();
            }
            #endregion

            #region UnsuitabilityItems Table Created
            Table UnsuitabilityItemsTable = model.CreateTable(Tables.UnsuitabilityItems);

            if (UnsuitabilityItemsTable != null)
            {
                var properties = (typeof(UnsuitabilityItems)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(UnsuitabilityItemsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(UnsuitabilityItemsTable, "PK_" + UnsuitabilityItemsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        UnsuitabilityItemsTable.Indexes.Add(pkIndex);
                    }

                    UnsuitabilityItemsTable.Columns.Add(column);
                }

                UnsuitabilityItemsTable.Create();
            }
            #endregion

            #region ControlTypes Table Created
            Table ControlTypesTable = model.CreateTable(Tables.ControlTypes);

            if (ControlTypesTable != null)
            {
                var properties = (typeof(ControlTypes)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ControlTypesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ControlTypesTable, "PK_" + ControlTypesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ControlTypesTable.Indexes.Add(pkIndex);
                    }

                    ControlTypesTable.Columns.Add(column);
                }

                ControlTypesTable.Create();
            }
            #endregion

            #region ControlConditions Table Created
            Table ControlConditionsTable = model.CreateTable(Tables.ControlConditions);

            if (ControlConditionsTable != null)
            {
                var properties = (typeof(ControlConditions)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ControlConditionsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ControlConditionsTable, "PK_" + ControlConditionsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ControlConditionsTable.Indexes.Add(pkIndex);
                    }

                    ControlConditionsTable.Columns.Add(column);
                }

                ControlConditionsTable.Create();
            }
            #endregion

            #region Forecasts Table Created
            Table ForecastsTable = model.CreateTable(Tables.Forecasts);

            if (ForecastsTable != null)
            {
                var properties = (typeof(Forecasts)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ForecastsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ForecastsTable, "PK_" + ForecastsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ForecastsTable.Indexes.Add(pkIndex);
                    }

                    ForecastsTable.Columns.Add(column);
                }

                ForecastsTable.Create();
            }
            #endregion

            #region ForecastLines Table Created
            Table ForecastLinesTable = model.CreateTable(Tables.ForecastLines);

            if (ForecastLinesTable != null)
            {
                var properties = (typeof(ForecastLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ForecastLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ForecastLinesTable, "PK_" + ForecastLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ForecastLinesTable.Indexes.Add(pkIndex);
                    }

                    ForecastLinesTable.Columns.Add(column);
                }

                ForecastLinesTable.Create();
            }
            #endregion

            #region SalesOrders Table Created
            Table SalesOrdersTable = model.CreateTable(Tables.SalesOrders);

            if (SalesOrdersTable != null)
            {
                var properties = (typeof(SalesOrders)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(SalesOrdersTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(SalesOrdersTable, "PK_" + SalesOrdersTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        SalesOrdersTable.Indexes.Add(pkIndex);
                    }

                    SalesOrdersTable.Columns.Add(column);
                }

                SalesOrdersTable.Create();
            }
            #endregion

            #region SalesOrderLines Table Created
            Table SalesOrderLinesTable = model.CreateTable(Tables.SalesOrderLines);

            if (SalesOrderLinesTable != null)
            {
                var properties = (typeof(SalesOrderLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(SalesOrderLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(SalesOrderLinesTable, "PK_" + SalesOrderLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        SalesOrderLinesTable.Indexes.Add(pkIndex);
                    }

                    SalesOrderLinesTable.Columns.Add(column);
                }

                SalesOrderLinesTable.Create();
            }
            #endregion

            #region SalesPrices Table Created
            Table SalesPricesTable = model.CreateTable(Tables.SalesPrices);

            if (SalesPricesTable != null)
            {
                var properties = (typeof(SalesPrices)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(SalesPricesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(SalesPricesTable, "PK_" + SalesPricesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        SalesPricesTable.Indexes.Add(pkIndex);
                    }

                    SalesPricesTable.Columns.Add(column);
                }

                SalesPricesTable.Create();
            }
            #endregion

            #region SalesPriceLines Table Created
            Table SalesPriceLinesTable = model.CreateTable(Tables.SalesPriceLines);

            if (SalesPriceLinesTable != null)
            {
                var properties = (typeof(SalesPriceLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(SalesPriceLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(SalesPriceLinesTable, "PK_" + SalesPriceLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        SalesPriceLinesTable.Indexes.Add(pkIndex);
                    }

                    SalesPriceLinesTable.Columns.Add(column);
                }

                SalesPriceLinesTable.Create();
            }
            #endregion

            #region SalesPropositions Table Created
            Table SalesPropositionsTable = model.CreateTable(Tables.SalesPropositions);

            if (SalesPropositionsTable != null)
            {
                var properties = (typeof(SalesPropositions)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(SalesPropositionsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(SalesPropositionsTable, "PK_" + SalesPropositionsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        SalesPropositionsTable.Indexes.Add(pkIndex);
                    }

                    SalesPropositionsTable.Columns.Add(column);
                }

                SalesPropositionsTable.Create();
            }
            #endregion

            #region SalesPropositionLines Table Created
            Table SalesPropositionLinesTable = model.CreateTable(Tables.SalesPropositionLines);

            if (SalesPropositionLinesTable != null)
            {
                var properties = (typeof(SalesPropositionLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(SalesPropositionLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(SalesPropositionLinesTable, "PK_" + SalesPropositionLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        SalesPropositionLinesTable.Indexes.Add(pkIndex);
                    }

                    SalesPropositionLinesTable.Columns.Add(column);
                }

                SalesPropositionLinesTable.Create();
            }
            #endregion

            #region ShippingAdresses Table Created
            Table ShippingAdressesTable = model.CreateTable(Tables.ShippingAdresses);

            if (ShippingAdressesTable != null)
            {
                var properties = (typeof(ShippingAdresses)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ShippingAdressesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ShippingAdressesTable, "PK_" + ShippingAdressesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ShippingAdressesTable.Indexes.Add(pkIndex);
                    }

                    ShippingAdressesTable.Columns.Add(column);
                }

                ShippingAdressesTable.Create();
            }
            #endregion

            #region Products Table Created
            Table ProductsTable = model.CreateTable(Tables.Products);

            if (ProductsTable != null)
            {
                var properties = (typeof(Products)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ProductsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ProductsTable, "PK_" + ProductsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ProductsTable.Indexes.Add(pkIndex);
                    }

                    ProductsTable.Columns.Add(column);
                }

                ProductsTable.Create();
            }
            #endregion

            #region ProductGroups Table Created
            Table ProductGroupsTable = model.CreateTable(Tables.ProductGroups);

            if (ProductGroupsTable != null)
            {
                var properties = (typeof(ProductGroups)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ProductGroupsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ProductGroupsTable, "PK_" + ProductGroupsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ProductGroupsTable.Indexes.Add(pkIndex);
                    }

                    ProductGroupsTable.Columns.Add(column);
                }

                ProductGroupsTable.Create();
            }
            #endregion

            #region ProductReferanceNumbers Table Created
            Table ProductReferanceNumbersTable = model.CreateTable(Tables.ProductReferanceNumbers);

            if (ProductReferanceNumbersTable != null)
            {
                var properties = (typeof(ProductReferanceNumbers)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ProductReferanceNumbersTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ProductReferanceNumbersTable, "PK_" + ProductReferanceNumbersTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ProductReferanceNumbersTable.Indexes.Add(pkIndex);
                    }

                    ProductReferanceNumbersTable.Columns.Add(column);
                }

                ProductReferanceNumbersTable.Create();
            }
            #endregion

            #region Stock Fiche Table Created
            Table stockFichesTable = model.CreateTable(Tables.StockFiches);

            if (stockFichesTable != null)
            {
                var properties = (typeof(StockFiches)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(stockFichesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(stockFichesTable, "PK_" + stockFichesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        stockFichesTable.Indexes.Add(pkIndex);
                    }

                    stockFichesTable.Columns.Add(column);
                }

                stockFichesTable.Create();
            }
            #endregion

            #region Stock Fiche Line Table Created
            Table stockFicheLinesTable = model.CreateTable(Tables.StockFicheLines);

            if (stockFicheLinesTable != null)
            {
                var properties = (typeof(StockFicheLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(stockFicheLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(stockFicheLinesTable, "PK_" + stockFicheLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        stockFicheLinesTable.Indexes.Add(pkIndex);
                    }

                    stockFicheLinesTable.Columns.Add(column);
                }

                stockFicheLinesTable.Create();
            }
            #endregion

            #region TechnicalDrawings Table Created
            Table TechnicalDrawingsTable = model.CreateTable(Tables.TechnicalDrawings);

            if (TechnicalDrawingsTable != null)
            {
                var properties = (typeof(TechnicalDrawings)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(TechnicalDrawingsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(TechnicalDrawingsTable, "PK_" + TechnicalDrawingsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        TechnicalDrawingsTable.Indexes.Add(pkIndex);
                    }

                    TechnicalDrawingsTable.Columns.Add(column);
                }

                TechnicalDrawingsTable.Create();
            }
            #endregion

            #region UnitSets Table Created
            Table UnitSetsTable = model.CreateTable(Tables.UnitSets);

            if (UnitSetsTable != null)
            {
                var properties = (typeof(UnitSets)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(UnitSetsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(UnitSetsTable, "PK_" + UnitSetsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        UnitSetsTable.Indexes.Add(pkIndex);
                    }

                    UnitSetsTable.Columns.Add(column);
                }

                UnitSetsTable.Create();
            }
            #endregion

            #region Warehouses Table Created
            Table WarehousesTable = model.CreateTable(Tables.Warehouses);

            if (WarehousesTable != null)
            {
                var properties = (typeof(Warehouses)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(WarehousesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(WarehousesTable, "PK_" + WarehousesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        WarehousesTable.Indexes.Add(pkIndex);
                    }

                    WarehousesTable.Columns.Add(column);
                }

                WarehousesTable.Create();
            }
            #endregion

            #region FinanceManagementParameters Table Created
            Table FinanceManagementParametersTable = model.CreateTable(Tables.FinanceManagementParameters);

            if (FinanceManagementParametersTable != null)
            {
                var properties = (typeof(FinanceManagementParameters)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(FinanceManagementParametersTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(FinanceManagementParametersTable, "PK_" + FinanceManagementParametersTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        FinanceManagementParametersTable.Indexes.Add(pkIndex);
                    }

                    FinanceManagementParametersTable.Columns.Add(column);
                }

                FinanceManagementParametersTable.Create();
            }
            #endregion

            #region GeneralParameters Table Created
            Table GeneralParametersTable = model.CreateTable(Tables.GeneralParameters);

            if (GeneralParametersTable != null)
            {
                var properties = (typeof(GeneralParameters)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(GeneralParametersTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(GeneralParametersTable, "PK_" + GeneralParametersTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        GeneralParametersTable.Indexes.Add(pkIndex);
                    }

                    GeneralParametersTable.Columns.Add(column);
                }

                GeneralParametersTable.Create();
            }
            #endregion

            #region MachineAndWorkforceManagementParameters Table Created
            Table MachineAndWorkforceManagementParametersTable = model.CreateTable(Tables.MachineAndWorkforceManagementParameters);

            if (MachineAndWorkforceManagementParametersTable != null)
            {
                var properties = (typeof(MachineAndWorkforceManagementParameters)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(MachineAndWorkforceManagementParametersTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(MachineAndWorkforceManagementParametersTable, "PK_" + MachineAndWorkforceManagementParametersTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        MachineAndWorkforceManagementParametersTable.Indexes.Add(pkIndex);
                    }

                    MachineAndWorkforceManagementParametersTable.Columns.Add(column);
                }

                MachineAndWorkforceManagementParametersTable.Create();
            }
            #endregion

            #region MaintenanceManagementParameters Table Created
            Table MaintenanceManagementParametersTable = model.CreateTable(Tables.MaintenanceManagementParameters);

            if (MaintenanceManagementParametersTable != null)
            {
                var properties = (typeof(MaintenanceManagementParameters)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(MaintenanceManagementParametersTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(MaintenanceManagementParametersTable, "PK_" + MaintenanceManagementParametersTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        MaintenanceManagementParametersTable.Indexes.Add(pkIndex);
                    }

                    MaintenanceManagementParametersTable.Columns.Add(column);
                }

                MaintenanceManagementParametersTable.Create();
            }
            #endregion

            #region PlanningManagementParameters Table Created
            Table PlanningManagementParametersTable = model.CreateTable(Tables.PlanningManagementParameters);

            if (PlanningManagementParametersTable != null)
            {
                var properties = (typeof(PlanningManagementParameters)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PlanningManagementParametersTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PlanningManagementParametersTable, "PK_" + PlanningManagementParametersTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PlanningManagementParametersTable.Indexes.Add(pkIndex);
                    }

                    PlanningManagementParametersTable.Columns.Add(column);
                }

                PlanningManagementParametersTable.Create();
            }
            #endregion

            #region ProductionManagementParameters Table Created
            Table ProductionManagementParametersTable = model.CreateTable(Tables.ProductionManagementParameters);

            if (ProductionManagementParametersTable != null)
            {
                var properties = (typeof(ProductionManagementParameters)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ProductionManagementParametersTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ProductionManagementParametersTable, "PK_" + ProductionManagementParametersTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ProductionManagementParametersTable.Indexes.Add(pkIndex);
                    }

                    ProductionManagementParametersTable.Columns.Add(column);
                }

                ProductionManagementParametersTable.Create();
            }
            #endregion

            #region PurchaseManagementParameters Table Created
            Table PurchaseManagementParametersTable = model.CreateTable(Tables.PurchaseManagementParameters);

            if (PurchaseManagementParametersTable != null)
            {
                var properties = (typeof(PurchaseManagementParameters)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PurchaseManagementParametersTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PurchaseManagementParametersTable, "PK_" + PurchaseManagementParametersTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PurchaseManagementParametersTable.Indexes.Add(pkIndex);
                    }

                    PurchaseManagementParametersTable.Columns.Add(column);
                }

                PurchaseManagementParametersTable.Create();
            }
            #endregion

            #region QualityControlParameters Table Created
            Table QualityControlParametersTable = model.CreateTable(Tables.QualityControlParameters);

            if (QualityControlParametersTable != null)
            {
                var properties = (typeof(QualityControlParameters)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(QualityControlParametersTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(QualityControlParametersTable, "PK_" + QualityControlParametersTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        QualityControlParametersTable.Indexes.Add(pkIndex);
                    }

                    QualityControlParametersTable.Columns.Add(column);
                }

                QualityControlParametersTable.Create();
            }
            #endregion

            #region SalesManagementParameters Table Created
            Table SalesManagementParametersTable = model.CreateTable(Tables.SalesManagementParameters);

            if (SalesManagementParametersTable != null)
            {
                var properties = (typeof(SalesManagementParameters)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(SalesManagementParametersTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(SalesManagementParametersTable, "PK_" + SalesManagementParametersTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        SalesManagementParametersTable.Indexes.Add(pkIndex);
                    }

                    SalesManagementParametersTable.Columns.Add(column);
                }

                SalesManagementParametersTable.Create();
            }
            #endregion

            #region ShippingManagementParameters Table Created
            Table ShippingManagementParametersTable = model.CreateTable(Tables.ShippingManagementParameters);

            if (ShippingManagementParametersTable != null)
            {
                var properties = (typeof(ShippingManagementParameters)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ShippingManagementParametersTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ShippingManagementParametersTable, "PK_" + ShippingManagementParametersTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ShippingManagementParametersTable.Indexes.Add(pkIndex);
                    }

                    ShippingManagementParametersTable.Columns.Add(column);
                }

                ShippingManagementParametersTable.Create();
            }
            #endregion

            #region StockManagementParameters Table Created
            Table StockManagementParametersTable = model.CreateTable(Tables.StockManagementParameters);

            if (StockManagementParametersTable != null)
            {
                var properties = (typeof(StockManagementParameters)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(StockManagementParametersTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(StockManagementParametersTable, "PK_" + StockManagementParametersTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        StockManagementParametersTable.Indexes.Add(pkIndex);
                    }

                    StockManagementParametersTable.Columns.Add(column);
                }

                StockManagementParametersTable.Create();
            }
            #endregion

            #region OperationalQualityPlans Table Created
            Table OperationalQualityPlansTable = model.CreateTable(Tables.OperationalQualityPlans);

            if (OperationalQualityPlansTable != null)
            {
                var properties = (typeof(OperationalQualityPlans)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(OperationalQualityPlansTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(OperationalQualityPlansTable, "PK_" + OperationalQualityPlansTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        OperationalQualityPlansTable.Indexes.Add(pkIndex);
                    }

                    OperationalQualityPlansTable.Columns.Add(column);
                }

                OperationalQualityPlansTable.Create();
            }
            #endregion

            #region UserPermissions Table Created
            Table UserPermissionsTable = model.CreateTable(Tables.UserPermissions);

            if (UserPermissionsTable != null)
            {
                var properties = (typeof(UserPermissions)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(UserPermissionsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(UserPermissionsTable, "PK_" + UserPermissionsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        UserPermissionsTable.Indexes.Add(pkIndex);
                    }

                    UserPermissionsTable.Columns.Add(column);
                }

                UserPermissionsTable.Create();
            }
            #endregion

            #region OperationalQualityPlanLines Table Created
            Table OperationalQualityPlanLinesTable = model.CreateTable(Tables.OperationalQualityPlanLines);

            if (OperationalQualityPlanLinesTable != null)
            {
                var properties = (typeof(OperationalQualityPlanLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(OperationalQualityPlanLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(OperationalQualityPlanLinesTable, "PK_" + OperationalQualityPlanLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        OperationalQualityPlanLinesTable.Indexes.Add(pkIndex);
                    }

                    OperationalQualityPlanLinesTable.Columns.Add(column);
                }

                OperationalQualityPlanLinesTable.Create();
            }
            #endregion

            #region OperationPictures Table Created
            Table OperationPicturesTable = model.CreateTable(Tables.OperationPictures);

            if (OperationPicturesTable != null)
            {
                var properties = (typeof(OperationPictures)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(OperationPicturesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(OperationPicturesTable, "PK_" + OperationPicturesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        OperationPicturesTable.Indexes.Add(pkIndex);
                    }

                    OperationPicturesTable.Columns.Add(column);
                }

                OperationPicturesTable.Create();
            }
            #endregion

            #region ContractOperationPictures Table Created
            Table ContractOperationPicturesTable = model.CreateTable(Tables.ContractOperationPictures);

            if (ContractOperationPicturesTable != null)
            {
                var properties = (typeof(ContractOperationPictures)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ContractOperationPicturesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ContractOperationPicturesTable, "PK_" + ContractOperationPicturesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ContractOperationPicturesTable.Indexes.Add(pkIndex);
                    }

                    ContractOperationPicturesTable.Columns.Add(column);
                }

                ContractOperationPicturesTable.Create();
            }
            #endregion

            #region ContractQualityPlans Table Created
            Table ContractQualityPlansTable = model.CreateTable(Tables.ContractQualityPlans);

            if (ContractQualityPlansTable != null)
            {
                var properties = (typeof(ContractQualityPlans)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ContractQualityPlansTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ContractQualityPlansTable, "PK_" + ContractQualityPlansTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ContractQualityPlansTable.Indexes.Add(pkIndex);
                    }

                    ContractQualityPlansTable.Columns.Add(column);
                }

                ContractQualityPlansTable.Create();
            }
            #endregion

            #region ContractQualityPlanLines Table Created
            Table ContractQualityPlanLinesTable = model.CreateTable(Tables.ContractQualityPlanLines);

            if (ContractQualityPlanLinesTable != null)
            {
                var properties = (typeof(ContractQualityPlanLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ContractQualityPlanLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ContractQualityPlanLinesTable, "PK_" + ContractQualityPlanLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ContractQualityPlanLinesTable.Indexes.Add(pkIndex);
                    }

                    ContractQualityPlanLinesTable.Columns.Add(column);
                }

                ContractQualityPlanLinesTable.Create();
            }
            #endregion

            #region ContractQualityPlanOperations Table Created
            Table ContractQualityPlanOperationsTable = model.CreateTable(Tables.ContractQualityPlanOperations);

            if (ContractQualityPlanOperationsTable != null)
            {
                var properties = (typeof(ContractQualityPlanOperations)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ContractQualityPlanOperationsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ContractQualityPlanOperationsTable, "PK_" + ContractQualityPlanOperationsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ContractQualityPlanOperationsTable.Indexes.Add(pkIndex);
                    }

                    ContractQualityPlanOperationsTable.Columns.Add(column);
                }

                ContractQualityPlanOperationsTable.Create();
            }
            #endregion

            #region PurchaseQualityPlans Table Created
            Table PurchaseQualityPlansTable = model.CreateTable(Tables.PurchaseQualityPlans);

            if (PurchaseQualityPlansTable != null)
            {
                var properties = (typeof(PurchaseQualityPlans)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PurchaseQualityPlansTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PurchaseQualityPlansTable, "PK_" + PurchaseQualityPlansTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PurchaseQualityPlansTable.Indexes.Add(pkIndex);
                    }

                    PurchaseQualityPlansTable.Columns.Add(column);
                }

                PurchaseQualityPlansTable.Create();
            }
            #endregion

            #region PurchaseQualityPlanLines Table Created
            Table PurchaseQualityPlanLinesTable = model.CreateTable(Tables.PurchaseQualityPlanLines);

            if (PurchaseQualityPlanLinesTable != null)
            {
                var properties = (typeof(PurchaseQualityPlanLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PurchaseQualityPlanLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PurchaseQualityPlanLinesTable, "PK_" + PurchaseQualityPlanLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PurchaseQualityPlanLinesTable.Indexes.Add(pkIndex);
                    }

                    PurchaseQualityPlanLinesTable.Columns.Add(column);
                }

                PurchaseQualityPlanLinesTable.Create();
            }
            #endregion

            #region ContractTrackingFiches Table Created
            Table ContractTrackingFichesTable = model.CreateTable(Tables.ContractTrackingFiches);

            if (ContractTrackingFichesTable != null)
            {
                var properties = (typeof(ContractTrackingFiches)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ContractTrackingFichesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ContractTrackingFichesTable, "PK_" + ContractTrackingFichesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ContractTrackingFichesTable.Indexes.Add(pkIndex);
                    }

                    ContractTrackingFichesTable.Columns.Add(column);
                }

                ContractTrackingFichesTable.Create();
            }
            #endregion

            #region ContractTrackingFicheLines Table Created
            Table ContractTrackingFicheLinesTable = model.CreateTable(Tables.ContractTrackingFicheLines);

            if (ContractTrackingFicheLinesTable != null)
            {
                var properties = (typeof(ContractTrackingFicheLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ContractTrackingFicheLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ContractTrackingFicheLinesTable, "PK_" + ContractTrackingFicheLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ContractTrackingFicheLinesTable.Indexes.Add(pkIndex);
                    }

                    ContractTrackingFicheLinesTable.Columns.Add(column);
                }

                ContractTrackingFicheLinesTable.Create();
            }
            #endregion

            #region ContractTrackingFicheAmountEntryLines Table Created
            Table ContractTrackingFicheAmountEntryLinesTable = model.CreateTable(Tables.ContractTrackingFicheAmountEntryLines);

            if (ContractTrackingFicheAmountEntryLinesTable != null)
            {
                var properties = (typeof(ContractTrackingFicheAmountEntryLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ContractTrackingFicheAmountEntryLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ContractTrackingFicheAmountEntryLinesTable, "PK_" + ContractTrackingFicheAmountEntryLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ContractTrackingFicheAmountEntryLinesTable.Indexes.Add(pkIndex);
                    }

                    ContractTrackingFicheAmountEntryLinesTable.Columns.Add(column);
                }

                ContractTrackingFicheAmountEntryLinesTable.Create();
            }
            #endregion

            #region ContractUnsuitabilityReports Table Created
            Table ContractUnsuitabilityReportsTable = model.CreateTable(Tables.ContractUnsuitabilityReports);

            if (ContractUnsuitabilityReportsTable != null)
            {
                var properties = (typeof(ContractUnsuitabilityReports)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ContractUnsuitabilityReportsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ContractUnsuitabilityReportsTable, "PK_" + ContractUnsuitabilityReportsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ContractUnsuitabilityReportsTable.Indexes.Add(pkIndex);
                    }

                    ContractUnsuitabilityReportsTable.Columns.Add(column);
                }

                ContractUnsuitabilityReportsTable.Create();
            }
            #endregion

            #region UnsuitabilityItemSPCs Table Created
            Table UnsuitabilityItemSPCsTable = model.CreateTable(Tables.UnsuitabilityItemSPCs);

            if (UnsuitabilityItemSPCsTable != null)
            {
                var properties = (typeof(UnsuitabilityItemSPCs)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(UnsuitabilityItemSPCsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(UnsuitabilityItemSPCsTable, "PK_" + UnsuitabilityItemSPCsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        UnsuitabilityItemSPCsTable.Indexes.Add(pkIndex);
                    }

                    UnsuitabilityItemSPCsTable.Columns.Add(column);
                }

                UnsuitabilityItemSPCsTable.Create();
            }
            #endregion

            #region UnsuitabilityItemSPCLines Table Created
            Table UnsuitabilityItemSPCLinesTable = model.CreateTable(Tables.UnsuitabilityItemSPCLines);

            if (UnsuitabilityItemSPCLinesTable != null)
            {
                var properties = (typeof(UnsuitabilityItemSPCLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(UnsuitabilityItemSPCLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(UnsuitabilityItemSPCLinesTable, "PK_" + UnsuitabilityItemSPCLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        UnsuitabilityItemSPCLinesTable.Indexes.Add(pkIndex);
                    }

                    UnsuitabilityItemSPCLinesTable.Columns.Add(column);
                }

                UnsuitabilityItemSPCLinesTable.Create();
            }
            #endregion

            #region OperationalSPCs Table Created
            Table OperationalSPCsTable = model.CreateTable(Tables.OperationalSPCs);

            if (OperationalSPCsTable != null)
            {
                var properties = (typeof(OperationalSPCs)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(OperationalSPCsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(OperationalSPCsTable, "PK_" + OperationalSPCsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        OperationalSPCsTable.Indexes.Add(pkIndex);
                    }

                    OperationalSPCsTable.Columns.Add(column);
                }

                OperationalSPCsTable.Create();
            }
            #endregion

            #region OperationalSPCLines Table Created
            Table OperationalSPCLinesTable = model.CreateTable(Tables.OperationalSPCLines);

            if (OperationalSPCLinesTable != null)
            {
                var properties = (typeof(OperationalSPCLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(OperationalSPCLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(OperationalSPCLinesTable, "PK_" + OperationalSPCLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        OperationalSPCLinesTable.Indexes.Add(pkIndex);
                    }

                    OperationalSPCLinesTable.Columns.Add(column);
                }

                OperationalSPCLinesTable.Create();
            }
            #endregion

            #region PFMEAs Table Created
            Table PFMEAsTable = model.CreateTable(Tables.PFMEAs);

            if (PFMEAsTable != null)
            {
                var properties = (typeof(PFMEAs)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PFMEAsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PFMEAsTable, "PK_" + PFMEAsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PFMEAsTable.Indexes.Add(pkIndex);
                    }

                    PFMEAsTable.Columns.Add(column);
                }

                PFMEAsTable.Create();
            }
            #endregion

            #region MRPs Table Created
            Table MRPsTable = model.CreateTable(Tables.MRPs);

            if (MRPsTable != null)
            {
                var properties = (typeof(MRPs)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(MRPsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(MRPsTable, "PK_" + MRPsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        MRPsTable.Indexes.Add(pkIndex);
                    }

                    MRPsTable.Columns.Add(column);
                }

                MRPsTable.Create();
            }
            #endregion

            #region MRPLines Table Created
            Table MRPLinesTable = model.CreateTable(Tables.MRPLines);

            if (MRPLinesTable != null)
            {
                var properties = (typeof(MRPLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(MRPLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(MRPLinesTable, "PK_" + MRPLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        MRPLinesTable.Indexes.Add(pkIndex);
                    }

                    MRPLinesTable.Columns.Add(column);
                }

                MRPLinesTable.Create();
            }
            #endregion

            #region OperationStockMovement Table Created
            Table OperationStockMovementsTable = model.CreateTable(Tables.OperationStockMovements);

            if (OperationStockMovementsTable != null)
            {
                var properties = (typeof(OperationStockMovements)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(OperationStockMovementsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(OperationStockMovementsTable, "PK_" + OperationStockMovementsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        OperationStockMovementsTable.Indexes.Add(pkIndex);
                    }

                    OperationStockMovementsTable.Columns.Add(column);
                }

                OperationStockMovementsTable.Create();
            }
            #endregion

            #region Report8Ds Table Created
            Table Report8DsTable = model.CreateTable(Tables.Report8Ds);

            if (Report8DsTable != null)
            {
                var properties = (typeof(Report8Ds)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(Report8DsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(Report8DsTable, "PK_" + Report8DsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        Report8DsTable.Indexes.Add(pkIndex);
                    }

                    Report8DsTable.Columns.Add(column);
                }

                Report8DsTable.Create();
            }
            #endregion

            #region CustomerComplaintReports Table Created
            Table CustomerComplaintReportsTable = model.CreateTable(Tables.CustomerComplaintReports);

            if (CustomerComplaintReportsTable != null)
            {
                var properties = (typeof(CustomerComplaintReports)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(CustomerComplaintReportsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(CustomerComplaintReportsTable, "PK_" + CustomerComplaintReportsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        CustomerComplaintReportsTable.Indexes.Add(pkIndex);
                    }

                    CustomerComplaintReportsTable.Columns.Add(column);
                }

                CustomerComplaintReportsTable.Create();
            }
            #endregion

            #region FirstProductApprovals Table Created
            Table FirstProductApprovalsTable = model.CreateTable(Tables.FirstProductApprovals);

            if (FirstProductApprovalsTable != null)
            {
                var properties = (typeof(FirstProductApprovals)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(FirstProductApprovalsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(FirstProductApprovalsTable, "PK_" + FirstProductApprovalsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        FirstProductApprovalsTable.Indexes.Add(pkIndex);
                    }

                    FirstProductApprovalsTable.Columns.Add(column);
                }

                FirstProductApprovalsTable.Create();
            }
            #endregion

            #region FirstProductApprovalLines Table Created
            Table FirstProductApprovalLinesTable = model.CreateTable(Tables.FirstProductApprovalLines);

            if (FirstProductApprovalLinesTable != null)
            {
                var properties = (typeof(FirstProductApprovalLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(FirstProductApprovalLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(FirstProductApprovalLinesTable, "PK_" + FirstProductApprovalLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        FirstProductApprovalLinesTable.Indexes.Add(pkIndex);
                    }

                    FirstProductApprovalLinesTable.Columns.Add(column);
                }

                FirstProductApprovalLinesTable.Create();
            }
            #endregion

            #region PackageFiches Table Created
            Table PackageFichesTable = model.CreateTable(Tables.PackageFiches);

            if (PackageFichesTable != null)
            {
                var properties = (typeof(PackageFiches)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PackageFichesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PackageFichesTable, "PK_" + PackageFichesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PackageFichesTable.Indexes.Add(pkIndex);
                    }

                    PackageFichesTable.Columns.Add(column);
                }

                PackageFichesTable.Create();
            }
            #endregion

            #region PackageFicheLines Table Created
            Table PackageFicheLinesTable = model.CreateTable(Tables.PackageFicheLines);

            if (PackageFicheLinesTable != null)
            {
                var properties = (typeof(PackageFicheLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PackageFicheLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PackageFicheLinesTable, "PK_" + PackageFicheLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PackageFicheLinesTable.Indexes.Add(pkIndex);
                    }

                    PackageFicheLinesTable.Columns.Add(column);
                }

                PackageFicheLinesTable.Create();
            }
            #endregion

            #region PalletRecords Table Created
            Table PalletRecordsTable = model.CreateTable(Tables.PalletRecords);

            if (PalletRecordsTable != null)
            {
                var properties = (typeof(PalletRecords)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PalletRecordsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PalletRecordsTable, "PK_" + PalletRecordsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PalletRecordsTable.Indexes.Add(pkIndex);
                    }

                    PalletRecordsTable.Columns.Add(column);
                }

                PalletRecordsTable.Create();
            }
            #endregion

            #region PalletRecordLines Table Created
            Table PalletRecordLinesTable = model.CreateTable(Tables.PalletRecordLines);

            if (PalletRecordLinesTable != null)
            {
                var properties = (typeof(PalletRecordLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PalletRecordLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PalletRecordLinesTable, "PK_" + PalletRecordLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PalletRecordLinesTable.Indexes.Add(pkIndex);
                    }

                    PalletRecordLinesTable.Columns.Add(column);
                }

                PalletRecordLinesTable.Create();
            }
            #endregion

            #region MaintenanceMRPs Table Created
            Table MaintenanceMRPsTable = model.CreateTable(Tables.MaintenanceMRPs);

            if (MaintenanceMRPsTable != null)
            {
                var properties = (typeof(MaintenanceMRPs)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(MaintenanceMRPsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(MaintenanceMRPsTable, "PK_" + MaintenanceMRPsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        MaintenanceMRPsTable.Indexes.Add(pkIndex);
                    }

                    MaintenanceMRPsTable.Columns.Add(column);
                }

                MaintenanceMRPsTable.Create();
            }
            #endregion

            #region MaintenanceMRPLines Table Created
            Table MaintenanceMRPLinesTable = model.CreateTable(Tables.MaintenanceMRPLines);

            if (MaintenanceMRPLinesTable != null)
            {
                var properties = (typeof(MaintenanceMRPLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(MaintenanceMRPLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(MaintenanceMRPLinesTable, "PK_" + MaintenanceMRPLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        MaintenanceMRPLinesTable.Indexes.Add(pkIndex);
                    }

                    MaintenanceMRPLinesTable.Columns.Add(column);
                }

                MaintenanceMRPLinesTable.Create();
            }
            #endregion

            #region EmployeeSeniorities Table Created
            Table EmployeeSenioritiesTable = model.CreateTable(Tables.EmployeeSeniorities);

            if (EmployeeSenioritiesTable != null)
            {
                var properties = (typeof(EmployeeSeniorities)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(EmployeeSenioritiesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(EmployeeSenioritiesTable, "PK_" + EmployeeSenioritiesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        EmployeeSenioritiesTable.Indexes.Add(pkIndex);
                    }

                    EmployeeSenioritiesTable.Columns.Add(column);
                }

                EmployeeSenioritiesTable.Create();
            }
            #endregion

            #region EmployeeAnnualSeniorityDifferences Table Created
            Table EmployeeAnnualSeniorityDifferencesTable = model.CreateTable(Tables.EmployeeAnnualSeniorityDifferences);

            if (EmployeeAnnualSeniorityDifferencesTable != null)
            {
                var properties = (typeof(EmployeeAnnualSeniorityDifferences)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(EmployeeAnnualSeniorityDifferencesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(EmployeeAnnualSeniorityDifferencesTable, "PK_" + EmployeeAnnualSeniorityDifferencesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        EmployeeAnnualSeniorityDifferencesTable.Indexes.Add(pkIndex);
                    }

                    EmployeeAnnualSeniorityDifferencesTable.Columns.Add(column);
                }

                EmployeeAnnualSeniorityDifferencesTable.Create();
            }
            #endregion

            #region EducationLevelScores Table Created
            Table EducationLevelScoresTable = model.CreateTable(Tables.EducationLevelScores);

            if (EducationLevelScoresTable != null)
            {
                var properties = (typeof(EducationLevelScores)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(EducationLevelScoresTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(EducationLevelScoresTable, "PK_" + EducationLevelScoresTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        EducationLevelScoresTable.Indexes.Add(pkIndex);
                    }

                    EducationLevelScoresTable.Columns.Add(column);
                }

                EducationLevelScoresTable.Create();
            }
            #endregion

            #region EmployeeGeneralSkillRecords Table Created
            Table EmployeeGeneralSkillRecordsTable = model.CreateTable(Tables.EmployeeGeneralSkillRecords);

            if (EmployeeGeneralSkillRecordsTable != null)
            {
                var properties = (typeof(EmployeeGeneralSkillRecords)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(EmployeeGeneralSkillRecordsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(EmployeeGeneralSkillRecordsTable, "PK_" + EmployeeGeneralSkillRecordsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        EmployeeGeneralSkillRecordsTable.Indexes.Add(pkIndex);
                    }

                    EmployeeGeneralSkillRecordsTable.Columns.Add(column);
                }

                EmployeeGeneralSkillRecordsTable.Create();
            }
            #endregion

            #region TaskScorings Table Created
            Table TaskScoringsTable = model.CreateTable(Tables.TaskScorings);

            if (TaskScoringsTable != null)
            {
                var properties = (typeof(TaskScorings)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(TaskScoringsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(TaskScoringsTable, "PK_" + TaskScoringsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        TaskScoringsTable.Indexes.Add(pkIndex);
                    }

                    TaskScoringsTable.Columns.Add(column);
                }

                TaskScoringsTable.Create();
            }
            #endregion

            #region GeneralSkillRecordPriorities Table Created
            Table GeneralSkillRecordPrioritiesTable = model.CreateTable(Tables.GeneralSkillRecordPriorities);

            if (GeneralSkillRecordPrioritiesTable != null)
            {
                var properties = (typeof(GeneralSkillRecordPriorities)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(GeneralSkillRecordPrioritiesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(GeneralSkillRecordPrioritiesTable, "PK_" + GeneralSkillRecordPrioritiesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        GeneralSkillRecordPrioritiesTable.Indexes.Add(pkIndex);
                    }

                    GeneralSkillRecordPrioritiesTable.Columns.Add(column);
                }

                GeneralSkillRecordPrioritiesTable.Create();
            }
            #endregion

            #region StartingSalaries Table Created
            Table StartingSalariesTable = model.CreateTable(Tables.StartingSalaries);

            if (StartingSalariesTable != null)
            {
                var properties = (typeof(StartingSalaries)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(StartingSalariesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(StartingSalariesTable, "PK_" + StartingSalariesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        StartingSalariesTable.Indexes.Add(pkIndex);
                    }

                    StartingSalariesTable.Columns.Add(column);
                }

                StartingSalariesTable.Create();
            }
            #endregion

            #region StartingSalaryLines Table Created
            Table StartingSalaryLinesTable = model.CreateTable(Tables.StartingSalaryLines);

            if (StartingSalaryLinesTable != null)
            {
                var properties = (typeof(StartingSalaryLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(StartingSalaryLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(StartingSalaryLinesTable, "PK_" + StartingSalaryLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        StartingSalaryLinesTable.Indexes.Add(pkIndex);
                    }

                    StartingSalaryLinesTable.Columns.Add(column);
                }

                StartingSalaryLinesTable.Create();
            }
            #endregion

            #region PackingLists Table Created
            Table PackingListsTable = model.CreateTable(Tables.PackingLists);

            if (PackingListsTable != null)
            {
                var properties = (typeof(PackingLists)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PackingListsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PackingListsTable, "PK_" + PackingListsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PackingListsTable.Indexes.Add(pkIndex);
                    }

                    PackingListsTable.Columns.Add(column);
                }

                PackingListsTable.Create();
            }
            #endregion

            #region PackingListPalletCubageLines Table Created
            Table PackingListPalletCubageLinesTable = model.CreateTable(Tables.PackingListPalletCubageLines);

            if (PackingListPalletCubageLinesTable != null)
            {
                var properties = (typeof(PackingListPalletCubageLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PackingListPalletCubageLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PackingListPalletCubageLinesTable, "PK_" + PackingListPalletCubageLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PackingListPalletCubageLinesTable.Indexes.Add(pkIndex);
                    }

                    PackingListPalletCubageLinesTable.Columns.Add(column);
                }

                PackingListPalletCubageLinesTable.Create();
            }
            #endregion

            #region PackingListPalletLines Table Created
            Table PackingListPalletLinesTable = model.CreateTable(Tables.PackingListPalletLines);

            if (PackingListPalletLinesTable != null)
            {
                var properties = (typeof(PackingListPalletLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PackingListPalletLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PackingListPalletLinesTable, "PK_" + PackingListPalletLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PackingListPalletLinesTable.Indexes.Add(pkIndex);
                    }

                    PackingListPalletLinesTable.Columns.Add(column);
                }

                PackingListPalletLinesTable.Create();
            }
            #endregion

            #region PackingListPalletPackageLines Table Created
            Table PackingListPalletPackageLinesTable = model.CreateTable(Tables.PackingListPalletPackageLines);

            if (PackingListPalletPackageLinesTable != null)
            {
                var properties = (typeof(PackingListPalletPackageLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PackingListPalletPackageLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PackingListPalletPackageLinesTable, "PK_" + PackingListPalletPackageLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PackingListPalletPackageLinesTable.Indexes.Add(pkIndex);
                    }

                    PackingListPalletPackageLinesTable.Columns.Add(column);
                }

                PackingListPalletPackageLinesTable.Create();
            }
            #endregion

            #region EmployeeScorings Table Created
            Table EmployeeScoringsTable = model.CreateTable(Tables.EmployeeScorings);

            if (EmployeeScoringsTable != null)
            {
                var properties = (typeof(EmployeeScorings)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(EmployeeScoringsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(EmployeeScoringsTable, "PK_" + EmployeeScoringsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        EmployeeScoringsTable.Indexes.Add(pkIndex);
                    }

                    EmployeeScoringsTable.Columns.Add(column);
                }

                EmployeeScoringsTable.Create();
            }
            #endregion

            #region EmployeeScoringLines Table Created
            Table EmployeeScoringLinesTable = model.CreateTable(Tables.EmployeeScoringLines);

            if (EmployeeScoringLinesTable != null)
            {
                var properties = (typeof(EmployeeScoringLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(EmployeeScoringLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(EmployeeScoringLinesTable, "PK_" + EmployeeScoringLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        EmployeeScoringLinesTable.Indexes.Add(pkIndex);
                    }

                    EmployeeScoringLinesTable.Columns.Add(column);
                }

                EmployeeScoringLinesTable.Create();
            }
            #endregion

            #region EmployeeOperations Table Created
            Table EmployeeOperationsTable = model.CreateTable(Tables.EmployeeOperations);

            if (EmployeeOperationsTable != null)
            {
                var properties = (typeof(EmployeeOperations)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(EmployeeOperationsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(EmployeeOperationsTable, "PK_" + EmployeeOperationsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        EmployeeOperationsTable.Indexes.Add(pkIndex);
                    }

                    EmployeeOperationsTable.Columns.Add(column);
                }

                EmployeeOperationsTable.Create();
            }
            #endregion

            #region Operation Adjustment Table Created
            Table OperationAdjustmentsTable = model.CreateTable(Tables.OperationAdjustments);

            if (OperationAdjustmentsTable != null)
            {
                var properties = (typeof(PackingListPalletPackageLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(OperationAdjustmentsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(OperationAdjustmentsTable, "PK_" + OperationAdjustmentsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        OperationAdjustmentsTable.Indexes.Add(pkIndex);
                    }

                    OperationAdjustmentsTable.Columns.Add(column);
                }

                OperationAdjustmentsTable.Create();
            }
            #endregion

            #region OrderAcceptanceRecords Table Created
            Table OrderAcceptanceRecordsTable = model.CreateTable(Tables.OrderAcceptanceRecords);

            if (OrderAcceptanceRecordsTable != null)
            {
                var properties = (typeof(OrderAcceptanceRecords)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(OrderAcceptanceRecordsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(OrderAcceptanceRecordsTable, "PK_" + OrderAcceptanceRecordsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        OrderAcceptanceRecordsTable.Indexes.Add(pkIndex);
                    }

                    OrderAcceptanceRecordsTable.Columns.Add(column);
                }

                OrderAcceptanceRecordsTable.Create();
            }
            #endregion

            #region OrderAcceptanceRecordLines Table Created
            Table OrderAcceptanceRecordLinesTable = model.CreateTable(Tables.OrderAcceptanceRecordLines);

            if (OrderAcceptanceRecordLinesTable != null)
            {
                var properties = (typeof(OrderAcceptanceRecordLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(OrderAcceptanceRecordLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(OrderAcceptanceRecordLinesTable, "PK_" + OrderAcceptanceRecordLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        OrderAcceptanceRecordLinesTable.Indexes.Add(pkIndex);
                    }

                    OrderAcceptanceRecordLinesTable.Columns.Add(column);
                }

                OrderAcceptanceRecordLinesTable.Create();
            }
            #endregion

            #region ShipmentPlannings Table Created
            Table ShipmentPlanningsTable = model.CreateTable(Tables.ShipmentPlannings);

            if (ShipmentPlanningsTable != null)
            {
                var properties = (typeof(ShipmentPlannings)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ShipmentPlanningsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ShipmentPlanningsTable, "PK_" + ShipmentPlanningsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ShipmentPlanningsTable.Indexes.Add(pkIndex);
                    }

                    ShipmentPlanningsTable.Columns.Add(column);
                }

                ShipmentPlanningsTable.Create();
            }
            #endregion

            #region ShipmentPlanningLines Table Created
            Table ShipmentPlanningLinesTable = model.CreateTable(Tables.ShipmentPlanningLines);

            if (ShipmentPlanningLinesTable != null)
            {
                var properties = (typeof(ShipmentPlanningLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ShipmentPlanningLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ShipmentPlanningLinesTable, "PK_" + ShipmentPlanningLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ShipmentPlanningLinesTable.Indexes.Add(pkIndex);
                    }

                    ShipmentPlanningLinesTable.Columns.Add(column);
                }

                ShipmentPlanningLinesTable.Create();
            }
            #endregion

            #region ProductCosts Table Created
            Table ProductCostsTable = model.CreateTable(Tables.ProductCosts);

            if (ProductCostsTable != null)
            {
                var properties = (typeof(ProductCosts)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ProductCostsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ProductCostsTable, "PK_" + ProductCostsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ProductCostsTable.Indexes.Add(pkIndex);
                    }

                    ProductCostsTable.Columns.Add(column);
                }

                ProductCostsTable.Create();
            }
            #endregion

            #region StationOccupancies Table Created
            Table StationOccupanciesTable = model.CreateTable(Tables.StationOccupancies);

            if (StationOccupanciesTable != null)
            {
                var properties = (typeof(StationOccupancies)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(StationOccupanciesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(StationOccupanciesTable, "PK_" + StationOccupanciesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        StationOccupanciesTable.Indexes.Add(pkIndex);
                    }

                    StationOccupanciesTable.Columns.Add(column);
                }

                StationOccupanciesTable.Create();
            }
            #endregion

            #region StationOccupancyLines Table Created
            Table StationOccupancyLinesTable = model.CreateTable(Tables.StationOccupancyLines);

            if (StationOccupancyLinesTable != null)
            {
                var properties = (typeof(StationOccupancyLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(StationOccupancyLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(StationOccupancyLinesTable, "PK_" + StationOccupancyLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        StationOccupancyLinesTable.Indexes.Add(pkIndex);
                    }

                    StationOccupancyLinesTable.Columns.Add(column);
                }

                StationOccupancyLinesTable.Create();
            }
            #endregion

            #region MRPIIs Table Created
            Table MRPIIsTable = model.CreateTable(Tables.MRPIIs);

            if (MRPIIsTable != null)
            {
                var properties = (typeof(MRPIIs)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(MRPIIsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(MRPIIsTable, "PK_" + MRPIIsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        MRPIIsTable.Indexes.Add(pkIndex);
                    }

                    MRPIIsTable.Columns.Add(column);
                }

                MRPIIsTable.Create();
            }
            #endregion

            #region MRPIILines Table Created
            Table MRPIILinesTable = model.CreateTable(Tables.MRPIILines);

            if (MRPIILinesTable != null)
            {
                var properties = (typeof(MRPIILines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(MRPIILinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(MRPIILinesTable, "PK_" + MRPIILinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        MRPIILinesTable.Indexes.Add(pkIndex);
                    }

                    MRPIILinesTable.Columns.Add(column);
                }

                MRPIILinesTable.Create();
            }
            #endregion

            #region StockColumns Table Created
            Table StockColumnsTable = model.CreateTable(Tables.StockColumns);

            if (StockColumnsTable != null)
            {
                var properties = (typeof(StockColumns)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(StockColumnsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(StockColumnsTable, "PK_" + StockColumnsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        StockColumnsTable.Indexes.Add(pkIndex);
                    }

                    StockColumnsTable.Columns.Add(column);
                }

                StockColumnsTable.Create();
            }
            #endregion

            #region StockNumbers Table Created
            Table StockNumbersTable = model.CreateTable(Tables.StockNumbers);

            if (StockNumbersTable != null)
            {
                var properties = (typeof(StockNumbers)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(StockNumbersTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(StockNumbersTable, "PK_" + StockNumbersTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        StockNumbersTable.Indexes.Add(pkIndex);
                    }

                    StockNumbersTable.Columns.Add(column);
                }

                StockNumbersTable.Create();
            }
            #endregion

            #region StockSections Table Created
            Table StockSectionsTable = model.CreateTable(Tables.StockSections);

            if (StockSectionsTable != null)
            {
                var properties = (typeof(StockSections)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(StockSectionsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(StockSectionsTable, "PK_" + StockSectionsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        StockSectionsTable.Indexes.Add(pkIndex);
                    }

                    StockSectionsTable.Columns.Add(column);
                }

                StockSectionsTable.Create();
            }
            #endregion

            #region StockShelfs Table Created
            Table StockShelfsTable = model.CreateTable(Tables.StockShelfs);

            if (StockShelfsTable != null)
            {
                var properties = (typeof(StockShelfs)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(StockShelfsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(StockShelfsTable, "PK_" + StockShelfsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        StockShelfsTable.Indexes.Add(pkIndex);
                    }

                    StockShelfsTable.Columns.Add(column);
                }

                StockShelfsTable.Create();
            }
            #endregion

            #region StockAddresses Table Created
            Table StockAddressesTable = model.CreateTable(Tables.StockAddresses);

            if (StockAddressesTable != null)
            {
                var properties = (typeof(StockAddresses)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(StockAddressesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(StockAddressesTable, "PK_" + StockAddressesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        StockAddressesTable.Indexes.Add(pkIndex);
                    }

                    StockAddressesTable.Columns.Add(column);
                }

                StockAddressesTable.Create();
            }
            #endregion

            #region StockAddressLines Table Created
            Table StockAddressLinesTable = model.CreateTable(Tables.StockAddressLines);

            if (StockAddressLinesTable != null)
            {
                var properties = (typeof(StockAddressLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(StockAddressLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(StockAddressLinesTable, "PK_" + StockAddressLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        StockAddressLinesTable.Indexes.Add(pkIndex);
                    }

                    StockAddressLinesTable.Columns.Add(column);
                }

                StockAddressLinesTable.Create();
            }
            #endregion

            #region ProductProperties Table Created
            Table ProductPropertiesTable = model.CreateTable(Tables.ProductProperties);

            if (ProductPropertiesTable != null)
            {
                var properties = (typeof(ProductProperties)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ProductPropertiesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ProductPropertiesTable, "PK_" + ProductPropertiesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ProductPropertiesTable.Indexes.Add(pkIndex);
                    }

                    ProductPropertiesTable.Columns.Add(column);
                }

                ProductPropertiesTable.Create();
            }
            #endregion

            #region ProductPropertyLines Table Created
            Table ProductPropertyLinesTable = model.CreateTable(Tables.ProductPropertyLines);

            if (ProductPropertyLinesTable != null)
            {
                var properties = (typeof(ProductPropertyLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ProductPropertyLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ProductPropertyLinesTable, "PK_" + ProductPropertyLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ProductPropertyLinesTable.Indexes.Add(pkIndex);
                    }

                    ProductPropertyLinesTable.Columns.Add(column);
                }

                ProductPropertyLinesTable.Create();
            }
            #endregion

            #region ProductRelatedProductProperties Table Created
            Table ProductRelatedProductPropertiesTable = model.CreateTable(Tables.ProductRelatedProductProperties);

            if (ProductRelatedProductPropertiesTable != null)
            {
                var properties = (typeof(ProductRelatedProductProperties)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ProductRelatedProductPropertiesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ProductRelatedProductPropertiesTable, "PK_" + ProductRelatedProductPropertiesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ProductRelatedProductPropertiesTable.Indexes.Add(pkIndex);
                    }

                    ProductRelatedProductPropertiesTable.Columns.Add(column);
                }

                ProductRelatedProductPropertiesTable.Create();
            }
            #endregion

            #region ProductReceiptTransactions Table Created
            Table ProductReceiptTransactionsTable = model.CreateTable(Tables.ProductReceiptTransactions);

            if (ProductReceiptTransactionsTable != null)
            {
                var properties = (typeof(ProductReceiptTransactions)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ProductReceiptTransactionsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ProductReceiptTransactionsTable, "PK_" + ProductReceiptTransactionsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ProductReceiptTransactionsTable.Indexes.Add(pkIndex);
                    }

                    ProductReceiptTransactionsTable.Columns.Add(column);
                }

                ProductReceiptTransactionsTable.Create();
            }
            #endregion

            #region PurchaseOrdersAwaitingApprovals Table Created
            Table PurchaseOrdersAwaitingApprovalsTable = model.CreateTable(Tables.PurchaseOrdersAwaitingApprovals);

            if (PurchaseOrdersAwaitingApprovalsTable != null)
            {
                var properties = (typeof(PurchaseOrdersAwaitingApprovals)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PurchaseOrdersAwaitingApprovalsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PurchaseOrdersAwaitingApprovalsTable, "PK_" + PurchaseOrdersAwaitingApprovalsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PurchaseOrdersAwaitingApprovalsTable.Indexes.Add(pkIndex);
                    }

                    PurchaseOrdersAwaitingApprovalsTable.Columns.Add(column);
                }

                PurchaseOrdersAwaitingApprovalsTable.Create();
            }
            #endregion

            #region PurchaseOrdersAwaitingApprovalLines Table Created
            Table PurchaseOrdersAwaitingApprovalLinesTable = model.CreateTable(Tables.PurchaseOrdersAwaitingApprovalLines);

            if (PurchaseOrdersAwaitingApprovalLinesTable != null)
            {
                var properties = (typeof(PurchaseOrdersAwaitingApprovalLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PurchaseOrdersAwaitingApprovalLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PurchaseOrdersAwaitingApprovalLinesTable, "PK_" + PurchaseOrdersAwaitingApprovalLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PurchaseOrdersAwaitingApprovalLinesTable.Indexes.Add(pkIndex);
                    }

                    PurchaseOrdersAwaitingApprovalLinesTable.Columns.Add(column);
                }

                PurchaseOrdersAwaitingApprovalLinesTable.Create();
            }
            #endregion

            #region BankAccounts Table Created
            Table BankAccountsTable = model.CreateTable(Tables.BankAccounts);

            if (BankAccountsTable != null)
            {
                var properties = (typeof(BankAccounts)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(BankAccountsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(BankAccountsTable, "PK_" + BankAccountsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        BankAccountsTable.Indexes.Add(pkIndex);
                    }

                    BankAccountsTable.Columns.Add(column);
                }

                BankAccountsTable.Create();
            }
            #endregion

            #region ProductionOrderChangeReports Table Created
            Table ProductionOrderChangeReportsTable = model.CreateTable(Tables.ProductionOrderChangeReports);

            if (ProductionOrderChangeReportsTable != null)
            {
                var properties = (typeof(ProductionOrderChangeReports)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ProductionOrderChangeReportsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ProductionOrderChangeReportsTable, "PK_" + ProductionOrderChangeReportsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ProductionOrderChangeReportsTable.Indexes.Add(pkIndex);
                    }

                    ProductionOrderChangeReportsTable.Columns.Add(column);
                }

                ProductionOrderChangeReportsTable.Create();
            }
            #endregion

            #region NotificationTemplates Table Created
            Table NotificationTemplates = model.CreateTable(Tables.NotificationTemplates);

            if (NotificationTemplates != null)
            {
                var properties = (typeof(NotificationTemplates)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(NotificationTemplates, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(NotificationTemplates, "PK_" + NotificationTemplates.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        NotificationTemplates.Indexes.Add(pkIndex);
                    }

                    NotificationTemplates.Columns.Add(column);
                }

                NotificationTemplates.Create();
            }
            #endregion

            #region Notifications Table Created
            Table Notifications = model.CreateTable(Tables.Notifications);

            if (Notifications != null)
            {
                var properties = (typeof(Notifications)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(Notifications, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(Notifications, "PK_" + Notifications.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        Notifications.Indexes.Add(pkIndex);
                    }

                    Notifications.Columns.Add(column);
                }

                Notifications.Create();
            }
            #endregion


            //TEST BÖLGESİ

            #region Continents Table Created
            Table ContinentsTable = model.CreateTable(Tables.Continents);

            if (ContinentsTable != null)
            {
                var properties = (typeof(Continents)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ContinentsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ContinentsTable, "PK_" + ContinentsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ContinentsTable.Indexes.Add(pkIndex);
                    }

                    ContinentsTable.Columns.Add(column);
                }

                ContinentsTable.Create();
            }
            #endregion

            #region ContinentLines Table Created
            Table ContinentLinesTable = model.CreateTable(Tables.ContinentLines);

            if (ContinentLinesTable != null)
            {
                var properties = (typeof(ContinentLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ContinentLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ContinentLinesTable, "PK_" + ContinentLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ContinentLinesTable.Indexes.Add(pkIndex);
                    }

                    ContinentLinesTable.Columns.Add(column);
                }

                ContinentLinesTable.Create();
            }
            #endregion

            #region Sectors Table Created
            Table SectorsTable = model.CreateTable(Tables.Sectors);

            if (SectorsTable != null)
            {
                var properties = (typeof(Sectors)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(SectorsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(SectorsTable, "PK_" + SectorsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        SectorsTable.Indexes.Add(pkIndex);
                    }

                    SectorsTable.Columns.Add(column);
                }

                SectorsTable.Create();
            }
            #endregion

            #region SectorLines Table Created
            Table SectorLinesTable = model.CreateTable(Tables.SectorLines);

            if (SectorLinesTable != null)
            {
                var properties = (typeof(SectorLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(SectorLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(SectorLinesTable, "PK_" + SectorLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        SectorLinesTable.Indexes.Add(pkIndex);
                    }

                    SectorLinesTable.Columns.Add(column);
                }

                SectorLinesTable.Create();
            }
            #endregion

            #region Cities Table Created
            Table CitiesTable = model.CreateTable(Tables.Cities);

            if (CitiesTable != null)
            {
                var properties = (typeof(Cities)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(CitiesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(CitiesTable, "PK_" + CitiesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        CitiesTable.Indexes.Add(pkIndex);
                    }

                    CitiesTable.Columns.Add(column);
                }

                CitiesTable.Create();
            }
            #endregion

            #region CityLines Table Created
            Table CityLinesTable = model.CreateTable(Tables.CityLines);

            if (CityLinesTable != null)
            {
                var properties = (typeof(CityLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(CityLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(CityLinesTable, "PK_" + CityLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        CityLinesTable.Indexes.Add(pkIndex);
                    }

                    CityLinesTable.Columns.Add(column);
                }

                CityLinesTable.Create();
            }
            #endregion


            //---------------------

            return true;
        }


        public void DbAlterTable()
        {
            var queryFactory = new QueryFactory();

            queryFactory.ConnectToDatabase();

            DatabaseModel model = new DatabaseModel(queryFactory.Connection);

            Server server = model.ConnectToServer();

            Database database = server.Databases[queryFactory.Connection.Database];

            var addAttr = (typeof(DatabaseAlterTable)).GetProperties().Where(t => t.IsDefined(typeof(DatabaseAddColumnAttribute), false)).Count();

            var dropAttr = (typeof(DatabaseAlterTable)).GetProperties().Where(t => t.IsDefined(typeof(DatabaseDropColumnAttribute), false)).Count();

            #region Add Column

            if (addAttr > 0)
            {
                var groupedAddList = (typeof(DatabaseAlterTable)).GetProperties().GroupBy(t => t.GetCustomAttribute<DatabaseAddColumnAttribute>().TableName).ToList();

                foreach (var grouped in groupedAddList)
                {
                    string tableName = grouped.Key;

                    foreach (var item in grouped.ToList())
                    {
                        var dbType = item.GetCustomAttribute<DatabaseAddColumnAttribute>().SqlDbType;
                        var required = item.GetCustomAttribute<DatabaseAddColumnAttribute>().Nullable;
                        var maxLength = item.GetCustomAttribute<DatabaseAddColumnAttribute>().MaxLength;
                        var scale = item.GetCustomAttribute<DatabaseAddColumnAttribute>().Scale;
                        var precision = item.GetCustomAttribute<DatabaseAddColumnAttribute>().Precision;
                        var isPrimaryKey = item.GetCustomAttribute<DatabaseAddColumnAttribute>().IsPrimaryKey;
                        var default_ = item.GetCustomAttribute<DatabaseAddColumnAttribute>().Default_;

                        Table table = database.Tables[tableName];

                        Column column = new Column(table, item.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));

                        column.Nullable = required;

                        var defaultConstraint = column.AddDefaultConstraint("DF_" + tableName + "_" + item.Name + "_");
                        defaultConstraint.Text = default_;
                        column.Default = default_;

                        if (!table.Columns.Contains(column.Name))
                        {
                            table.Columns.Add(column);

                            table.Alter();
                        }

                    }
                }
            }
            #endregion

            #region Drop Column

            if (dropAttr > 0)
            {
                var groupedDropList = (typeof(DatabaseAlterTable)).GetProperties().GroupBy(t => t.GetCustomAttribute<DatabaseDropColumnAttribute>().TableName).ToList();

                foreach (var grouped in groupedDropList)
                {
                    string tableName = grouped.Key;

                    foreach (var item in grouped.ToList())
                    {

                        var columnName = item.GetCustomAttribute<DatabaseDropColumnAttribute>().ColumnName;

                        Table table = database.Tables[tableName];

                        table.Columns[columnName].DropIfExists();

                        table.Alter();
                    }
                }
            }
            #endregion
        }
    }
}
