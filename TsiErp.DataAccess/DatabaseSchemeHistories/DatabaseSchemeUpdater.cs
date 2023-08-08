using Microsoft.SqlServer.Management.Smo;
using System.Reflection;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using Tsi.Core.Utilities.VersionUtilities;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.DatabaseSchemes;
using TsiErp.DataAccess.Utilities;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
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
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine;
using TsiErp.Entities.Entities.ProductionManagement.ContractOfProductsOperation;
using TsiErp.Entities.Entities.ProductionManagement.ContractProductionTracking;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTrackingHaltLine;
using TsiErp.Entities.Entities.ProductionManagement.ProductOperationQualtityPlan;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperationLine;
using TsiErp.Entities.Entities.ProductionManagement.Route;
using TsiErp.Entities.Entities.ProductionManagement.RouteLine;
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
using TsiErp.Entities.Entities.QualityControl.ContractUnsuitabilityItem;
using TsiErp.Entities.Entities.QualityControl.ControlCondition;
using TsiErp.Entities.Entities.QualityControl.ControlType;
using TsiErp.Entities.Entities.QualityControl.CustomerComplaintItem;
using TsiErp.Entities.Entities.QualityControl.EquipmentRecord;
using TsiErp.Entities.Entities.QualityControl.FinalControlUnsuitabilityItem;
using TsiErp.Entities.Entities.QualityControl.FinalControlUnsuitabilityReport;
using TsiErp.Entities.Entities.QualityControl.OperationUnsuitabilityItem;
using TsiErp.Entities.Entities.QualityControl.OperationUnsuitabilityReport;
using TsiErp.Entities.Entities.QualityControl.ProductionOrderChangeItem;
using TsiErp.Entities.Entities.QualityControl.PurchaseUnsuitabilityReport;
using TsiErp.Entities.Entities.QualityControl.PurchasingUnsuitabilityItem;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem;
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

            #region Logs Table Created
            Table LogsTable = model.CreateTable(Tables.Logs);

            if (LogsTable != null)
            {
                var properties = (typeof(Logs)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(LogsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(LogsTable, "PK_" + LogsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        LogsTable.Indexes.Add(pkIndex);
                    }

                    LogsTable.Columns.Add(column);
                }

                LogsTable.Create();
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

            #region ProductionTrackingHaltLines Table Created
            Table ProductionTrackingHaltLinesTable = model.CreateTable(Tables.ProductionTrackingHaltLines);

            if (ProductionTrackingHaltLinesTable != null)
            {
                var properties = (typeof(ProductionTrackingHaltLines)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ProductionTrackingHaltLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ProductionTrackingHaltLinesTable, "PK_" + ProductionTrackingHaltLinesTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ProductionTrackingHaltLinesTable.Indexes.Add(pkIndex);
                    }

                    ProductionTrackingHaltLinesTable.Columns.Add(column);
                }

                ProductionTrackingHaltLinesTable.Create();
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

            #region ProductOperationQualityPlans Table Created
            Table ProductOperationQualityPlansTable = model.CreateTable(Tables.ProductOperationQualityPlans);

            if (ProductOperationQualityPlansTable != null)
            {
                var properties = (typeof(ProductOperationQualityPlans)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ProductOperationQualityPlansTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ProductOperationQualityPlansTable, "PK_" + ProductOperationQualityPlansTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ProductOperationQualityPlansTable.Indexes.Add(pkIndex);
                    }

                    ProductOperationQualityPlansTable.Columns.Add(column);
                }

                ProductOperationQualityPlansTable.Create();
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

            #region ContractUnsuitabilityItems Table Created
            Table ContractUnsuitabilityItemsTable = model.CreateTable(Tables.ContractUnsuitabilityItems);

            if (ContractUnsuitabilityItemsTable != null)
            {
                var properties = (typeof(ContractUnsuitabilityItems)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ContractUnsuitabilityItemsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ContractUnsuitabilityItemsTable, "PK_" + ContractUnsuitabilityItemsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ContractUnsuitabilityItemsTable.Indexes.Add(pkIndex);
                    }

                    ContractUnsuitabilityItemsTable.Columns.Add(column);
                }

                ContractUnsuitabilityItemsTable.Create();
            }
            #endregion

            #region CustomerComplaintItems Table Created
            Table CustomerComplaintItemsTable = model.CreateTable(Tables.CustomerComplaintItems);

            if (CustomerComplaintItemsTable != null)
            {
                var properties = (typeof(CustomerComplaintItems)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(CustomerComplaintItemsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(CustomerComplaintItemsTable, "PK_" + CustomerComplaintItemsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        CustomerComplaintItemsTable.Indexes.Add(pkIndex);
                    }

                    CustomerComplaintItemsTable.Columns.Add(column);
                }

                CustomerComplaintItemsTable.Create();
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

            #region FinalControlUnsuitabilityItems Table Created
            Table FinalControlUnsuitabilityItemsTable = model.CreateTable(Tables.FinalControlUnsuitabilityItems);

            if (FinalControlUnsuitabilityItemsTable != null)
            {
                var properties = (typeof(FinalControlUnsuitabilityItems)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(FinalControlUnsuitabilityItemsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(FinalControlUnsuitabilityItemsTable, "PK_" + FinalControlUnsuitabilityItemsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        FinalControlUnsuitabilityItemsTable.Indexes.Add(pkIndex);
                    }

                    FinalControlUnsuitabilityItemsTable.Columns.Add(column);
                }

                FinalControlUnsuitabilityItemsTable.Create();
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

            #region OperationUnsuitabilityItems Table Created
            Table OperationUnsuitabilityItemsTable = model.CreateTable(Tables.OperationUnsuitabilityItems);

            if (OperationUnsuitabilityItemsTable != null)
            {
                var properties = (typeof(OperationUnsuitabilityItems)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(OperationUnsuitabilityItemsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(OperationUnsuitabilityItemsTable, "PK_" + OperationUnsuitabilityItemsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        OperationUnsuitabilityItemsTable.Indexes.Add(pkIndex);
                    }

                    OperationUnsuitabilityItemsTable.Columns.Add(column);
                }

                OperationUnsuitabilityItemsTable.Create();
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

            #region ProductionOrderChangeItems Table Created
            Table ProductionOrderChangeItemsTable = model.CreateTable(Tables.ProductionOrderChangeItems);

            if (ProductionOrderChangeItemsTable != null)
            {
                var properties = (typeof(ProductionOrderChangeItems)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ProductionOrderChangeItemsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ProductionOrderChangeItemsTable, "PK_" + ProductionOrderChangeItemsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ProductionOrderChangeItemsTable.Indexes.Add(pkIndex);
                    }

                    ProductionOrderChangeItemsTable.Columns.Add(column);
                }

                ProductionOrderChangeItemsTable.Create();
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

            #region PurchasingUnsuitabilityItems Table Created
            Table PurchasingUnsuitabilityItemsTable = model.CreateTable(Tables.PurchasingUnsuitabilityItems);

            if (PurchasingUnsuitabilityItemsTable != null)
            {
                var properties = (typeof(PurchasingUnsuitabilityItems)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(PurchasingUnsuitabilityItemsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(PurchasingUnsuitabilityItemsTable, "PK_" + PurchasingUnsuitabilityItemsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        PurchasingUnsuitabilityItemsTable.Indexes.Add(pkIndex);
                    }

                    PurchasingUnsuitabilityItemsTable.Columns.Add(column);
                }

                PurchasingUnsuitabilityItemsTable.Create();
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

            #region ContractOfProductsOperations Table Created
            Table ContractOfProductsOperationsTable = model.CreateTable(Tables.ContractOfProductsOperations);

            if (ContractOfProductsOperationsTable != null)
            {
                var properties = (typeof(ContractOfProductsOperations)).GetProperties();

                foreach (var property in properties)
                {
                    var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
                    var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
                    var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
                    var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
                    var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
                    var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

                    Column column = new Column(ContractOfProductsOperationsTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
                    column.Nullable = required;

                    if (isPrimaryKey)
                    {
                        Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(ContractOfProductsOperationsTable, "PK_" + ContractOfProductsOperationsTable.Name);
                        pkIndex.IsClustered = true;
                        pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
                        pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
                        ContractOfProductsOperationsTable.Indexes.Add(pkIndex);
                    }

                    ContractOfProductsOperationsTable.Columns.Add(column);
                }

                ContractOfProductsOperationsTable.Create();
            }
            #endregion

            return true;
        }
    }
}
