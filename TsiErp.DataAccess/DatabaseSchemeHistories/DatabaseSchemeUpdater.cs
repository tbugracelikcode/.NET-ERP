using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using Tsi.Core.Utilities.VersionUtilities;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.DatabaseSchemes;
using TsiErp.DataAccess.Utilities;
using TsiErp.Entities.Entities.StockFiche;
using TsiErp.Entities.Entities.StockFicheLine;
using TsiErp.Entities.Entities.Version;
using TsiErp.Entities.TableConstant;

namespace TsiErp.DataAccess.DatabaseSchemeHistories
{
    public class DatabaseSchemeUpdater
    {
        [Version(VersiyonNumber = "1.23.2")]
        public bool Update()
        {
            //var queryFactory = new QueryFactory();

            //queryFactory.ConnectToDatabase();

            //DatabaseModel model = new DatabaseModel(queryFactory.Connection);

            //#region Stock Fiche Table Created
            //Table stockFichesTable = model.CreateTable(Tables.StockFiches);

            //if (stockFichesTable != null)
            //{
            //    var properties = (typeof(StockFiches)).GetProperties();

            //    foreach (var property in properties)
            //    {
            //        var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
            //        var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
            //        var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
            //        var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
            //        var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
            //        var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

            //        Column column = new Column(stockFichesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
            //        column.Nullable = required;

            //        if (isPrimaryKey)
            //        {
            //            Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(stockFichesTable, "PK_" + stockFichesTable.Name);
            //            pkIndex.IsClustered = true;
            //            pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
            //            pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
            //            stockFichesTable.Indexes.Add(pkIndex);
            //        }

            //        stockFichesTable.Columns.Add(column);
            //    }

                //stockFichesTable.Create();
            //}
            //#endregion

            //#region Stock Fiche Line Table Created
            //Table stockFicheLinesTable = model.CreateTable(Tables.StockFicheLines);

            //if (stockFicheLinesTable != null)
            //{
            //    var properties = (typeof(StockFicheLines)).GetProperties();

            //    foreach (var property in properties)
            //    {
            //        var dbType = property.GetCustomAttribute<SqlColumnTypeAttribute>().SqlDbType;
            //        var required = property.GetCustomAttribute<SqlColumnTypeAttribute>().Nullable;
            //        var maxLength = property.GetCustomAttribute<SqlColumnTypeAttribute>().MaxLength;
            //        var scale = property.GetCustomAttribute<SqlColumnTypeAttribute>().Scale;
            //        var precision = property.GetCustomAttribute<SqlColumnTypeAttribute>().Precision;
            //        var isPrimaryKey = property.GetCustomAttribute<SqlColumnTypeAttribute>().IsPrimaryKey;

            //        Column column = new Column(stockFicheLinesTable, property.Name, SqlColumnDataTypeFactory.ConvertToDataType(dbType, maxLength, scale, precision));
            //        column.Nullable = required;

            //        if (isPrimaryKey)
            //        {
            //            Microsoft.SqlServer.Management.Smo.Index pkIndex = new Microsoft.SqlServer.Management.Smo.Index(stockFicheLinesTable, "PK_" + stockFicheLinesTable.Name);
            //            pkIndex.IsClustered = true;
            //            pkIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
            //            pkIndex.IndexedColumns.Add(new IndexedColumn(pkIndex, property.Name));
            //            stockFicheLinesTable.Indexes.Add(pkIndex);
            //        }

            //        stockFicheLinesTable.Columns.Add(column);
            //    }

            //    //stockFicheLinesTable.Create();
            //}
            //#endregion

            return true;
        }
    }
}
