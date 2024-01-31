using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TSI.QueryBuilder.DatabaseSchemes
{
    public class DatabaseModel
    {
        private readonly IDbConnection _connection;

        public DatabaseModel(IDbConnection connection)
        {
            _connection = connection;
        }

        public Server ConnectToServer()
        {
            if (_connection != null)
            {
                SqlConnection sqlConnection = new SqlConnection("Server=94.73.145.4;Database=u0364806_TSIERP;UID=u0364806_TSIERP;PWD=u=xfJ@i-7H5-VN23;MultipleActiveResultSets=True;TrustServerCertificate=True;");

                if (sqlConnection.State == ConnectionState.Closed)
                    sqlConnection.Open();

                ServerConnection serverConnection = new ServerConnection(sqlConnection);

                return new Server(serverConnection);
            }

            return null;
        }

        public Table CreateTable(string tableName)
        {
            Table table = null;
         
            var server = ConnectToServer();

            if (server != null)
            {
                Database database = server.Databases[_connection.Database];

                if (database != null)
                {
                    table = new Table(database, tableName);
                }
            }
            return table;
        }

        public bool DropTable(string tableName)
        {
            bool acceptChanges = false;

            var server = ConnectToServer();

            if (server != null)
            {
                Database database = server.Databases[_connection.Database];

                if (database != null)
                {
                    server.ConnectionContext.BeginTransaction();

                    try
                    {
                        Table table = database.Tables[tableName];

                        table.DropIfExists();

                        server.ConnectionContext.CommitTransaction();
                        acceptChanges = true;
                    }
                    catch (Exception)
                    {
                        server.ConnectionContext.RollBackTransaction();
                        acceptChanges = false;
                    }
                }
            }

            return acceptChanges;
        }

        public bool AddColumn(string tableName, Column[] columns)
        {
            bool acceptChanges = false;

            var server = ConnectToServer();

            if (server != null)
            {
                Database database = server.Databases[_connection.Database];

                if (database != null)
                {
                    server.ConnectionContext.BeginTransaction();

                    try
                    {
                        Table table = database.Tables[tableName];

                        foreach (Column item in columns)
                        {
                            table.Columns.Add(item);
                        }

                        table.Alter();

                        server.ConnectionContext.CommitTransaction();

                        acceptChanges = true;
                    }
                    catch (Exception)
                    {
                        server.ConnectionContext.RollBackTransaction();
                        acceptChanges = false;
                    }
                }
            }

            return acceptChanges;
        }

        public bool DropColumn(string tableName, Column[] columns)
        {
            bool acceptChanges = false;

            var server = ConnectToServer();

            if (server != null)
            {
                Database database = server.Databases[_connection.Database];

                if (database != null)
                {
                    server.ConnectionContext.BeginTransaction();

                    try
                    {
                        Table table = database.Tables[tableName];

                        foreach (Column item in columns)
                        {
                            table.Columns[item.Name].DropIfExists();
                        }

                        table.Alter();

                        server.ConnectionContext.CommitTransaction();

                        acceptChanges = true;
                    }
                    catch (Exception)
                    {
                        server.ConnectionContext.RollBackTransaction();
                        acceptChanges = false;
                    }
                }
            }

            return acceptChanges;
        }

        public bool AlterColumn(string tableName, Column[] columns)
        {
            bool acceptChanges = false;

            var server = ConnectToServer();

            if (server != null)
            {
                Database database = server.Databases[_connection.Database];

                if (database != null)
                {
                    server.ConnectionContext.BeginTransaction();

                    try
                    {
                        Table table = database.Tables[tableName];

                        foreach (Column item in columns)
                        {
                            Column column = table.Columns[item.Name];
                            column = item;
                            column.Alter();
                        }

                        server.ConnectionContext.CommitTransaction();

                        acceptChanges = true;
                    }
                    catch (Exception)
                    {
                        server.ConnectionContext.RollBackTransaction();
                        acceptChanges = false;
                    }
                }
            }

            return acceptChanges;
        }

        public bool CreateIndex(string tableName, string indexName, Column column, IndexKeyType indexKeyType)
        {
            bool acceptChanges = false;

            var server = ConnectToServer();

            if (server != null)
            {
                Database database = server.Databases[_connection.Database];

                if (database != null)
                {
                    try
                    {
                        Table table = database.Tables[tableName];

                        Index index = new Index(table, indexName);
                        index.IndexKeyType = indexKeyType;

                        IndexedColumn indexedColumn = new IndexedColumn(index, column.Name);
                        index.IndexedColumns.Add(indexedColumn);
                        index.Create();
                        server.ConnectionContext.CommitTransaction();
                        acceptChanges = true;
                    }
                    catch (Exception)
                    {
                        server.ConnectionContext.RollBackTransaction();
                        acceptChanges = false;
                    }
                }
            }
            return acceptChanges;
        }
    }
}