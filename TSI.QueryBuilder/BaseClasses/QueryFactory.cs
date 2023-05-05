using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using Tsi.Core.Utilities.Results;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.ExceptionHandler;
using TSI.QueryBuilder.Extensions;
using TSI.QueryBuilder.Models;

namespace TSI.QueryBuilder.BaseClasses
{
    public class QueryFactory
    {
        public IDbConnection Connection { get; set; }

        public int CommandTimeOut { get; set; } = 600;

        public bool _IsSoftDelete { get; set; }

        public string BasePath { get; set; } = Directory.GetCurrentDirectory();

        public string JsonFile { get; set; } = "appsettings.json";

        public string SoftDeleteSectionName { get; set; } = "AppParams";

        public string SoftDeleteKey { get; set; } = "IsSoftDelete";

        public string DeleterIdField { get; set; }
        public string DeletionTimeField { get; set; }
        public string IsDeletedField { get; set; }
        public string ConnectionString { get; set; }

        public QueryFactory()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(BasePath)
                .AddJsonFile(JsonFile)
                .Build();

            if (configuration != null)
            {
                _IsSoftDelete = configuration.GetSection(SoftDeleteSectionName)[SoftDeleteKey].ToString() == "true" ? true : false;
                DeleterIdField = configuration.GetSection(SoftDeleteSectionName)["DeleterIdField"].ToString();
                DeletionTimeField = configuration.GetSection(SoftDeleteSectionName)["DeletionTimeField"].ToString();
                IsDeletedField = configuration.GetSection(SoftDeleteSectionName)["IsDeletedField"].ToString();
                ConnectionString = configuration.GetSection("ConnectionStrings")["AppConnectionString"].ToString();
            }
        }

        public IDbConnection ConnectToDatabase()
        {
            Connection = new SqlConnection();
            Connection.ConnectionString = ConnectionString;
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
                return Connection;
            }

            return null;
        }

        public bool DisconnectToDatabase()
        {
            if (Connection.State == ConnectionState.Open)
            {
                Connection.Close();
                Connection.Dispose();
                return true;
            }

            return false;
        }

        public Query Query()
        {
            return new Query();
        }

        public T Get<T>(Query query)
        {
            try
            {
                var command = Connection.CreateCommand();

                command.CommandTimeout = CommandTimeOut;

                if (command != null)
                {
                    if (_IsSoftDelete)
                    {
                        string isDeleted = IsDeletedField + "=" + "'" + "0" + "'";

                        if (!string.IsNullOrEmpty(query.JoinSeperator))
                        {
                            isDeleted = query.JoinSeperator + "." + isDeleted;
                        }

                        if (!string.IsNullOrEmpty(query.TablesJoinKeywords))
                        {
                            query.Sql = "select " + query.Columns + " from " + query.TableName + " as " + query.TableName + " " + query.TablesJoinKeywords;
                        }

                        if (string.IsNullOrEmpty(query.WhereSentence))
                        {
                            query.Sql = query.Sql + " where " + isDeleted;
                        }
                        else
                        {
                            query.WhereSentence = query.WhereSentence + " and " + isDeleted;
                            query.Sql = query.Sql + " where " + query.WhereSentence;
                        }
                    }

                    command.CommandText = query.Sql;

                    query.SqlResult = command.ExecuteReader().DataReaderMapToGet<T>();

                    query.JsonData = query.SqlResult != null ? JsonConvert.SerializeObject(query.SqlResult, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) : "";

                    return (T)query.SqlResult;
                }
                else
                {
                    return default(T);
                }
            }
            catch (Exception exp)
            {
                var error = ErrorException.ThrowException(exp);
                return default(T);
            }
        }

        public IEnumerable<T>? GetList<T>(Query query)
        {
            try
            {
                var command = Connection.CreateCommand();

                command.CommandTimeout = CommandTimeOut;

                if (command != null)
                {
                    if (_IsSoftDelete)
                    {
                        string isDeleted = IsDeletedField + "=" + "'" + "0" + "'";

                        if (!string.IsNullOrEmpty(query.JoinSeperator))
                        {
                            isDeleted = query.JoinSeperator + "." + isDeleted;
                        }

                        if (!string.IsNullOrEmpty(query.TablesJoinKeywords))
                        {
                            query.Sql = "select " + query.Columns + " from " + query.TableName + " as " + query.TableName + " " + query.TablesJoinKeywords;
                        }

                        if (string.IsNullOrEmpty(query.WhereSentence))
                        {
                            query.Sql = query.Sql + " where " + isDeleted;
                        }
                        else
                        {
                            query.WhereSentence = query.WhereSentence + " and " + isDeleted;
                            query.Sql = query.Sql + " where " + query.WhereSentence;
                        }
                    }


                    command.CommandText = query.Sql;

                    query.SqlResult = command.ExecuteReader().DataReaderMapToList<T>();

                    query.JsonData = query.SqlResult != null ? JsonConvert.SerializeObject(query.SqlResult, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) : "";

                    return query.SqlResult as IEnumerable<T>;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exp)
            {
                var error = ErrorException.ThrowException(exp);
                return null;
            }
        }

        public IEnumerable<T>? ControlList<T>(Query query)
        {
            try
            {
                var command = Connection.CreateCommand();

                command.CommandTimeout = CommandTimeOut;

                if (command != null)
                {

                    query.Sql = query.Sql + " where " + query.WhereSentence;

                    command.CommandText = query.Sql;

                    query.SqlResult = command.ExecuteReader().DataReaderMapToList<T>();

                    query.JsonData = query.SqlResult != null ? JsonConvert.SerializeObject(query.SqlResult, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) : "";

                    return query.SqlResult as IEnumerable<T>;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exp)
            {
                var error = ErrorException.ThrowException(exp);
                return null;
            }
        }

        public IEnumerable<T>? GetArray<T>(Query query)
        {
            try
            {
                var command = Connection.CreateCommand();

                command.CommandTimeout = CommandTimeOut;

                if (command != null)
                {
                    if (_IsSoftDelete)
                    {
                        string isDeleted = IsDeletedField + "=" + "'" + "0" + "'";

                        if (!string.IsNullOrEmpty(query.JoinSeperator))
                        {
                            isDeleted = query.JoinSeperator + "." + isDeleted;
                        }

                        if (string.IsNullOrEmpty(query.WhereSentence))
                        {
                            query.Sql = query.Sql + " where " + isDeleted;
                        }
                        else
                        {
                            query.WhereSentence = query.WhereSentence + " and " + isDeleted;
                            query.Sql = query.Sql + " where " + query.WhereSentence;
                        }
                    }


                    command.CommandText = query.Sql;

                    query.SqlResult = command.ExecuteReader().DataReaderMapToArray<T>();

                    return query.SqlResult as IEnumerable<T>;
                }
                else
                {

                    return null;
                }
            }
            catch (Exception exp)
            {
                var error = ErrorException.ThrowException(exp);
                return null;
            }
        }

        public T Insert<T>(Query query, string returnIdCaption, bool useTransaction = true)
        {
            IDbTransaction transaction = Connection.BeginTransaction();

            T returnValue = (T)Activator.CreateInstance(typeof(T));

            try
            {
                string[] insertQueries = query.Sql.Split(QueryConstants.QueryConstant);

                if (insertQueries.Length == 1)
                {
                    var command = Connection.CreateCommand();

                    command.CommandTimeout = CommandTimeOut;

                    if (command != null)
                    {
                        query.Sql = query.Sql.Replace("values", "output INSERTED." + returnIdCaption + " values");

                        command.CommandText = query.Sql;
                        command.Transaction = transaction;

                        Guid _id = (Guid)command.ExecuteScalar();

                        transaction.Commit();

                        var resultSql = query.From(query.TableName).Select().Where(returnIdCaption, _id.ToString(), query.JoinSeperator);

                        var result = Get<T>(resultSql);

                        query.SqlResult = result;

                        query.JsonData = query.SqlResult != null ? JsonConvert.SerializeObject(query.SqlResult, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) : "";
                        returnValue = (T)query.SqlResult;
                    }
                    else
                    {
                        transaction.Rollback();
                        return default(T);
                    }
                }
                else
                {
                    var command = Connection.CreateCommand();

                    command.CommandTimeout = CommandTimeOut;

                    if (command != null)
                    {
                        query.Sql = insertQueries[0].Replace("values", "output INSERTED." + returnIdCaption + " values");

                        command.CommandText = query.Sql;
                        command.Transaction = transaction;

                        Guid _id = (Guid)command.ExecuteScalar();

                        if (_id != Guid.Empty)
                        {
                            for (int i = 0; i < insertQueries.Length; i++)
                            {
                                if (i == 0)
                                {
                                    continue;
                                }

                                var commandLine = Connection.CreateCommand();

                                if (commandLine != null)
                                {
                                    commandLine.CommandTimeout = CommandTimeOut;
                                    query.Sql = insertQueries[i].Replace("values", "output INSERTED." + returnIdCaption + " values");

                                    commandLine.CommandText = query.Sql;
                                    commandLine.Transaction = transaction;
                                    commandLine.ExecuteScalar();
                                }
                            }

                            transaction.Commit();

                            var resultSql = query.From(query.TableName).Select().Where(returnIdCaption, _id.ToString(), query.JoinSeperator);

                            var result = Get<T>(resultSql);

                            query.SqlResult = result;

                            query.JsonData = query.SqlResult != null ? JsonConvert.SerializeObject(query.SqlResult, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) : "";

                            returnValue = (T)query.SqlResult;
                        }
                    }
                }

                return returnValue;
            }
            catch (Exception exp)
            {
                transaction.Rollback();
                var error = ErrorException.ThrowException(exp);
                return default(T);
            }
        }

        public T Update<T>(Query query, string returnIdCaption, bool useTransaction = true)
        {
            IDbTransaction transaction = Connection.BeginTransaction();

            T returnValue = (T)Activator.CreateInstance(typeof(T));

            try
            {
                string[] insertQueries = query.Sql.Split(QueryConstants.QueryConstant);

                if (insertQueries.Length == 1)
                {
                    var command = Connection.CreateCommand();

                    command.CommandTimeout = CommandTimeOut;

                    if (command != null)
                    {

                        query.Sql = query.Sql + " where " + query.WhereSentence;
                        query.Sql = query.Sql.Replace("where", "output INSERTED." + returnIdCaption + " where");

                        command.CommandText = query.Sql;
                        command.Transaction = transaction;

                        Guid _id = (Guid)command.ExecuteScalar();

                        transaction.Commit();

                        var resultSql = query.From(query.TableName).Select().Where(returnIdCaption, _id.ToString(), query.JoinSeperator);

                        var result = Get<T>(resultSql);

                        query.SqlResult = result;

                        query.JsonData = query.SqlResult != null ? JsonConvert.SerializeObject(query.SqlResult, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) : "";
                        returnValue = (T)query.SqlResult;
                    }
                    else
                    {
                        transaction.Rollback();
                        return default(T);
                    }
                }
                else
                {
                    var command = Connection.CreateCommand();

                    command.CommandTimeout = CommandTimeOut;

                    if (command != null)
                    {
                        query.Sql = insertQueries[0] + " where " + query.WhereSentence;
                        query.Sql = query.Sql.Replace("where", "output INSERTED." + returnIdCaption + " where");

                        command.CommandText = query.Sql;
                        command.Transaction = transaction;

                        Guid _id = (Guid)command.ExecuteScalar();

                        if (_id != Guid.Empty)
                        {
                            for (int i = 0; i < insertQueries.Length; i++)
                            {
                                if (i == 0)
                                {
                                    continue;
                                }

                                var commandLine = Connection.CreateCommand();

                                if (commandLine != null)
                                {
                                    commandLine.CommandTimeout = CommandTimeOut;
                                    string lineQuery = insertQueries[i];

                                    if (insertQueries[i].StartsWith("insert"))
                                    {
                                        lineQuery = insertQueries[i].Replace("values", "output INSERTED." + returnIdCaption + " values");
                                    }
                                    else
                                    {
                                        lineQuery = insertQueries[i].Replace("where", "output INSERTED." + returnIdCaption + " where");
                                    }

                                    commandLine.CommandText = lineQuery;
                                    commandLine.Transaction = transaction;
                                    commandLine.ExecuteScalar();
                                }
                            }

                            transaction.Commit();

                            var resultSql = query.From(query.TableName).Select().Where(returnIdCaption, _id.ToString(), query.JoinSeperator);

                            var result = Get<T>(resultSql);

                            query.SqlResult = result;

                            query.JsonData = query.SqlResult != null ? JsonConvert.SerializeObject(query.SqlResult, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) : "";

                            returnValue = (T)query.SqlResult;
                        }
                    }
                }

                return returnValue;
            }
            catch (Exception exp)
            {
                transaction.Rollback();
                var error = ErrorException.ThrowException(exp);
                return default(T);
            }
        }

        public bool Delete(Query query)
        {
            try
            {
                var command = Connection.CreateCommand();

                command.CommandTimeout = CommandTimeOut;

                if (command != null)
                {
                    command.CommandText = query.Sql + " " + query.WhereSentence;

                    query.SqlResult = command.ExecuteReader();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exp)
            {
                var error = ErrorException.ThrowException(exp);
                return false;
            }
        }

        public bool Delete(Query query, bool useTransaction = true)
        {
            IDbTransaction transaction = Connection.BeginTransaction();
            try
            {
                var command = Connection.CreateCommand();

                command.CommandTimeout = CommandTimeOut;

                if (command != null)
                {
                    command.CommandText = query.Sql + " " + query.WhereSentence;
                    command.Transaction = transaction;

                    query.SqlResult = command.ExecuteReader();
                    transaction.Commit();

                    return true;
                }
                else
                {
                    transaction.Rollback();
                    return false;
                }
            }
            catch (Exception exp)
            {
                transaction.Rollback();
                var error = ErrorException.ThrowException(exp);
                return false;
            }
        }

    }
}
