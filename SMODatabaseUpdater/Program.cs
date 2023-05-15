using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

SqlConnection sqlConnection = new SqlConnection("Server=94.73.145.4;Database=u0364806_TSIERP;UID=u0364806_TSIERP;PWD=u=xfJ@i-7H5-VN23;MultipleActiveResultSets=True;TrustServerCertificate=True;");
ServerConnection serverConnection = new ServerConnection(sqlConnection);
Server server = new Server(serverConnection);
Database workDatabase = server.Databases[sqlConnection.Database];

#region Table Create



//Table table = new Table(workDatabase, "SmoTest");

//Column idColumn = new Column(table, "Id", DataType.Int);
//idColumn.Identity = true;
//table.Columns.Add(idColumn);

//Column nameColumn = new Column(table, "Name", DataType.NVarChar(50));
//nameColumn.Nullable = false;
//table.Columns.Add(nameColumn);

//table.Create();
#endregion

#region Primary Key Create
//Microsoft.SqlServer.Management.Smo.Index primaryIndex = new Microsoft.SqlServer.Management.Smo.Index(table, "PK_SmoTest");
//primaryIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;

//IndexedColumn indexedColumn = new IndexedColumn(primaryIndex, "Id");
//primaryIndex.IndexedColumns.Add(indexedColumn);
//primaryIndex.Create();
#endregion

#region Table Alter

//Table alterTable = workDatabase.Tables["SmoTest"];

//Column surnameColumn = new Column(alterTable, "Surname", DataType.NVarChar(50));
//surnameColumn.Nullable = false;
//alterTable.Columns.Add(surnameColumn);

//alterTable.Alter();

#endregion

#region Column Alter

//Table alterTable = workDatabase.Tables["SmoTest"];
//Column clm = alterTable.Columns["Surname"];
//clm.DataType = DataType.Int;

//clm.Alter();

#endregion

#region Create Index

#endregion