using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using Tsi.Core.Utilities.Results;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.ExceptionHandler;
using TSI.QueryBuilder.Extensions;
using TSI.QueryBuilder.Helpers;
using TSI.QueryBuilder.Models;

namespace TSI.QueryBuilder.BaseClasses
{
    public class QueryFactory
    {
        public IDbConnection Connection { get; set; }

        public int CommandTimeOut { get; set; } = 900;

        public bool _IsSoftDelete { get; set; }

        public string BasePath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

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
            if (Connection == null)
                Connection = new SqlConnection();


            if (Connection.State == ConnectionState.Closed)
            {
                Connection.ConnectionString = ConnectionString;
                Connection.Open();
                return Connection;
            }

            return Connection;
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
                ConnectToDatabase();


                var command = Connection.CreateCommand();

                command.CommandTimeout = CommandTimeOut;

                var queries = WhereHelper.WhereQueries;

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
                            if (query.UseIsDeleteInQuery)
                            {
                                query.Sql = query.Sql + " where " + isDeleted;
                            }
                        }
                        else
                        {
                            if (query.UseIsDeleteInQuery)
                            {
                                query.WhereSentence = query.WhereSentence + " and " + isDeleted;
                                query.Sql = query.Sql + " where " + query.WhereSentence;
                            }
                            else
                            {
                                query.WhereSentence = query.WhereSentence;
                                query.Sql = query.Sql + " where " + query.WhereSentence;
                            }
                        }
                    }

                    command.CommandText = query.Sql;

                    if (queries.Count > 0)
                    {
                        foreach (var item in queries[0].ParameterList)
                        {
                            var parameter = command.CreateParameter();
                            parameter.ParameterName = item.Key;
                            parameter.Value = item.Value ?? DBNull.Value;
                            command.Parameters.Add(parameter);
                        }
                    }

                    if (query.IsMapQuery)
                    {
                        query.SqlResult = command.ExecuteReader().DataReaderMapToGet<T>();
                    }
                    else
                    {
                        query.SqlResult = command.ExecuteScalar();
                    }


                    query.JsonData = query.SqlResult != null ? JsonConvert.SerializeObject(query.SqlResult, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) : "";

                    WhereHelper.WhereQueries.Clear();
                    Connection.Close();
                    Connection.Dispose();
                    GC.Collect();
                    return (T)query.SqlResult;
                }
                else
                {
                    WhereHelper.WhereQueries.Clear();
                    Connection.Close();
                    Connection.Dispose();
                    GC.Collect();

                    return default(T);
                }
            }
            catch (Exception exp)
            {
                WhereHelper.WhereQueries.Clear();
                Connection.Close();
                Connection.Dispose();
                GC.Collect();

                var error = ErrorException.ThrowException(exp);
                return default(T);
            }
        }

        public T GetSQLDate<T>(Query query)
        {
            try
            {
                ConnectToDatabase();


                var command = Connection.CreateCommand();

                command.CommandTimeout = CommandTimeOut;

                if (command != null)
                {
                    command.CommandText = query.Sql;

                    if (query.IsMapQuery)
                    {
                        query.SqlResult = command.ExecuteReader().DataReaderMapToGet<T>();
                    }
                    else
                    {
                        query.SqlResult = command.ExecuteScalar();
                    }


                    query.JsonData = query.SqlResult != null ? JsonConvert.SerializeObject(query.SqlResult, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) : "";

                    Connection.Close();
                    Connection.Dispose();
                    GC.Collect();

                    return (T)query.SqlResult;
                }
                else
                {
                    Connection.Close();
                    Connection.Dispose();
                    GC.Collect();

                    return default(T);
                }
            }
            catch (Exception exp)
            {
                Connection.Close();
                Connection.Dispose();
                GC.Collect();

                var error = ErrorException.ThrowException(exp);
                return default(T);
            }
        }

        public IEnumerable<T> GetList<T>(Query query)
        {
            try
            {
                ConnectToDatabase();

                var command = Connection.CreateCommand();

                command.CommandTimeout = CommandTimeOut;

                var queries = WhereHelper.WhereQueries;

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
                            if (query.UseIsDeleteInQuery)
                            {
                                query.Sql = query.Sql + " where " + isDeleted;
                            }
                        }
                        else
                        {
                            if (query.UseIsDeleteInQuery)
                            {
                                query.WhereSentence = query.WhereSentence + " and " + isDeleted;
                                query.Sql = query.Sql + " where " + query.WhereSentence;
                            }
                            else
                            {
                                query.WhereSentence = query.WhereSentence;
                                query.Sql = query.Sql + " where " + query.WhereSentence;
                            }
                        }
                    }


                    command.CommandText = query.Sql;

                    if (queries.Count > 0)
                    {
                        foreach (var item in queries[0].ParameterList)
                        {
                            var parameter = command.CreateParameter();
                            parameter.ParameterName = item.Key;
                            parameter.Value = item.Value ?? DBNull.Value;
                            if (!command.Parameters.Contains(parameter.ParameterName))
                            {
                                command.Parameters.Add(parameter);
                            }
                        }
                    }

                    query.SqlResult = command.ExecuteReader().DataReaderMapToList<T>();

                    query.JsonData = query.SqlResult != null ? JsonConvert.SerializeObject(query.SqlResult, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) : "";

                    WhereHelper.WhereQueries.Clear();
                    Connection.Close();
                    Connection.Dispose();
                    GC.Collect();

                    return query.SqlResult as IEnumerable<T>;
                }
                else
                {
                    WhereHelper.WhereQueries.Clear();
                    Connection.Close();
                    Connection.Dispose();
                    GC.Collect();

                    return null;
                }
            }
            catch (Exception exp)
            {
                WhereHelper.WhereQueries.Clear();
                Connection.Close();
                Connection.Dispose();
                GC.Collect();

                var error = ErrorException.ThrowException(exp);
                return null;
            }
        }

        public IEnumerable<T> ControlList<T>(Query query)
        {
            try
            {
                ConnectToDatabase();

                var command = Connection.CreateCommand();

                command.CommandTimeout = CommandTimeOut;

                if (command != null)
                {

                    var whereQueries = WhereHelper.WhereQueries;

                    query.Sql = query.Sql + " where " + query.WhereSentence;

                    command.CommandText = query.Sql;

                    if(whereQueries.Count > 0)
                    {
                        foreach (var item in whereQueries[0].ParameterList)
                        {
                            var parameter = command.CreateParameter();
                            parameter.ParameterName = item.Key;
                            parameter.Value = item.Value ?? DBNull.Value;
                            if (!command.Parameters.Contains(parameter.ParameterName))
                            {
                                command.Parameters.Add(parameter);
                            }
                        }
                    }

                    query.SqlResult = command.ExecuteReader().DataReaderMapToList<T>();

                    query.JsonData = query.SqlResult != null ? JsonConvert.SerializeObject(query.SqlResult, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) : "";

                    WhereHelper.WhereQueries.Clear();
                    Connection.Close();
                    Connection.Dispose();
                    GC.Collect();

                    return query.SqlResult as IEnumerable<T>;
                }
                else
                {
                    WhereHelper.WhereQueries.Clear();
                    Connection.Close();
                    Connection.Dispose();
                    GC.Collect();

                    return null;
                }
            }
            catch (Exception exp)
            {
                WhereHelper.WhereQueries.Clear();
                Connection.Close();
                Connection.Dispose();
                GC.Collect();

                var error = ErrorException.ThrowException(exp);
                return null;
            }
        }

        public T Insert<T>(Query query, string returnIdCaption, bool useTransaction = true)
        {

            ConnectToDatabase();

            IDbTransaction transaction = Connection.BeginTransaction();

            T returnValue = (T)Activator.CreateInstance(typeof(T));

            try
            {
                //string[] insertQueries = query.Sql.Split(QueryConstants.QueryConstant);

                var insertQueries = InsertHelper.InsertQueris;

                if (insertQueries.Count == 1)
                {
                    var command = Connection.CreateCommand();

                    command.CommandTimeout = CommandTimeOut;

                    if (command != null)
                    {
                        query.Sql = query.Sql.Replace("values", "output INSERTED." + returnIdCaption + " values");

                        command.CommandText = query.Sql;
                        command.Transaction = transaction;

                        command.Parameters.Clear();

                        foreach (var item in insertQueries[0].ParameterList)
                        {
                            var parameter = command.CreateParameter();
                            parameter.ParameterName = item.Key;
                            parameter.Value = item.Value ?? DBNull.Value;
                            if (!command.Parameters.Contains(parameter.ParameterName))
                            {
                                command.Parameters.Add(parameter);
                            }
                        }

                        Guid _id = (Guid)command.ExecuteScalar();

                        transaction.Commit();

                        var resultSql = query.UseIsDeleteInQuery
                               ? query.From(query.TableName).Select().Where(returnIdCaption, _id.ToString(), query.JoinSeperator)
                               : query.From(query.TableName).Select().Where(returnIdCaption, _id.ToString(), query.JoinSeperator).UseIsDelete(false);

                        var result = Get<T>(resultSql);

                        query.SqlResult = result;

                        query.JsonData = query.SqlResult != null ? JsonConvert.SerializeObject(query.SqlResult, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) : "";
                        returnValue = (T)query.SqlResult;

                        Connection.Close();
                        Connection.Dispose();
                        GC.Collect();
                    }
                    else
                    {
                        InsertHelper.InsertQueris.Clear();
                        transaction.Rollback();

                        Connection.Close();
                        Connection.Dispose();
                        GC.Collect();
                        return default(T);
                    }
                }
                else
                {
                    var command = Connection.CreateCommand();

                    command.CommandTimeout = CommandTimeOut;

                    if (command != null)
                    {
                        query.Sql = insertQueries[0].Sql.Replace("values", "output INSERTED." + returnIdCaption + " values");

                        command.CommandText = query.Sql;
                        command.Transaction = transaction;

                        command.Parameters.Clear();

                        foreach (var item in insertQueries[0].ParameterList)
                        {
                            var parameter = command.CreateParameter();
                            parameter.ParameterName = item.Key;
                            parameter.Value = item.Value ?? DBNull.Value;
                            if (!command.Parameters.Contains(parameter.ParameterName))
                            {
                                command.Parameters.Add(parameter);
                            }
                        }

                        Guid _id = (Guid)command.ExecuteScalar();

                        if (_id != Guid.Empty)
                        {
                            for (int i = 0; i < insertQueries.Count; i++)
                            {
                                if (i == 0)
                                {
                                    continue;
                                }

                                var commandLine = Connection.CreateCommand();

                                if (commandLine != null)
                                {
                                    commandLine.CommandTimeout = CommandTimeOut;
                                    query.Sql = insertQueries[i].Sql.Replace("values", "output INSERTED." + returnIdCaption + " values");

                                    commandLine.CommandText = query.Sql;
                                    commandLine.Transaction = transaction;

                                    commandLine.Parameters.Clear();

                                    foreach (var item in insertQueries[i].ParameterList)
                                    {
                                        var parameter = commandLine.CreateParameter();
                                        parameter.ParameterName = item.Key;
                                        parameter.Value = item.Value ?? DBNull.Value;
                                        if (!commandLine.Parameters.Contains(parameter.ParameterName))
                                        {
                                            commandLine.Parameters.Add(parameter);
                                        }
                                    }

                                    commandLine.ExecuteScalar();
                                }
                            }

                            transaction.Commit();

                            var resultSql = query.UseIsDeleteInQuery
                                ? query.From(query.TableName).Select().Where(returnIdCaption, _id.ToString(), query.JoinSeperator)
                                : query.From(query.TableName).Select().Where(returnIdCaption, _id.ToString(), query.JoinSeperator).UseIsDelete(false);

                            var result = Get<T>(resultSql);

                            query.SqlResult = result;

                            query.JsonData = query.SqlResult != null ? JsonConvert.SerializeObject(query.SqlResult, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) : "";

                            returnValue = (T)query.SqlResult;

                            Connection.Close();
                            Connection.Dispose();
                            GC.Collect();
                        }
                    }
                }

                InsertHelper.InsertQueris.Clear();

                return returnValue;
            }
            catch (Exception exp)
            {

                InsertHelper.InsertQueris.Clear();
                transaction.Rollback();

                Connection.Close();
                Connection.Dispose();
                GC.Collect();
                var error = ErrorException.ThrowException(exp);
                return default(T);
            }
        }

        public T Update<T>(Query query, string returnIdCaption, bool useTransaction = true)
        {
            ConnectToDatabase();

            IDbTransaction transaction = Connection.BeginTransaction();

            T returnValue = (T)Activator.CreateInstance(typeof(T));

            try
            {
                //string[] insertQueries = query.Sql.Split(QueryConstants.QueryConstant);

                var updateQueries = UpdateHelper.UpdateQueris;

                var whereQueries = WhereHelper.WhereQueries;


                if (updateQueries.Count == 1)
                {
                    var command = Connection.CreateCommand();

                    command.CommandTimeout = CommandTimeOut;

                    if (command != null)
                    {

                        query.Sql = query.Sql + " where " + query.WhereSentence;
                        query.Sql = query.Sql.Replace("where", "output INSERTED." + returnIdCaption + " where");

                        command.CommandText = query.Sql;
                        command.Transaction = transaction;

                        command.Parameters.Clear();

                        //var parameterList = updateQueries[0].ParameterList.Except(whereQueries[0].ParameterList).ToList();



                        if (updateQueries.Count > 0)
                        {
                            foreach (var item in updateQueries[0].ParameterList)
                            {
                                var parameter = command.CreateParameter();
                                parameter.ParameterName = item.Key;
                                parameter.Value = item.Value ?? DBNull.Value;

                                if(!command.Parameters.Contains(parameter.ParameterName))
                                {
                                    command.Parameters.Add(parameter);
                                }
                            }
                        }

                        if (whereQueries.Count > 0)
                        {
                            foreach (var item in whereQueries[0].ParameterList)
                            {
                                var parameter = command.CreateParameter();
                                parameter.ParameterName = item.Key;
                                parameter.Value = item.Value ?? DBNull.Value;

                                if (!command.Parameters.Contains(parameter.ParameterName))
                                {
                                    command.Parameters.Add(parameter);
                                }
                            }
                        }

                        Guid _id = (Guid)command.ExecuteScalar();

                        transaction.Commit();

                        var resultSql = query.UseIsDeleteInQuery
                                ? query.From(query.TableName).Select().Where(returnIdCaption, _id.ToString(), query.JoinSeperator)
                                : query.From(query.TableName).Select().Where(returnIdCaption, _id.ToString(), query.JoinSeperator).UseIsDelete(false);

                        var result = Get<T>(resultSql);

                        query.SqlResult = result;

                        query.JsonData = query.SqlResult != null ? JsonConvert.SerializeObject(query.SqlResult, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) : "";
                        returnValue = (T)query.SqlResult;

                        Connection.Close();
                        Connection.Dispose();
                        GC.Collect();
                    }
                    else
                    {
                        UpdateHelper.UpdateQueris.Clear();
                        WhereHelper.WhereQueries.Clear();
                        transaction.Rollback();

                        Connection.Close();
                        Connection.Dispose();
                        GC.Collect();
                        return default(T);
                    }
                }
                else
                {
                    var command = Connection.CreateCommand();

                    command.CommandTimeout = CommandTimeOut;

                    if (command != null)
                    {
                        query.Sql = updateQueries[0] + " where " + query.WhereSentence;
                        query.Sql = query.Sql.Replace("where", "output INSERTED." + returnIdCaption + " where");

                        command.CommandText = query.Sql;
                        command.Transaction = transaction;

                        Guid _id = (Guid)command.ExecuteScalar();

                        if (_id != Guid.Empty)
                        {
                            for (int i = 0; i < updateQueries.Count; i++)
                            {
                                if (i == 0)
                                {
                                    continue;
                                }

                                var commandLine = Connection.CreateCommand();

                                if (commandLine != null)
                                {
                                    commandLine.CommandTimeout = CommandTimeOut;
                                    string lineQuery = updateQueries[i].Sql;

                                    if (updateQueries[i].Sql.StartsWith("insert"))
                                    {
                                        lineQuery = updateQueries[i].Sql.Replace("values", "output INSERTED." + returnIdCaption + " values");
                                    }
                                    else
                                    {
                                        lineQuery = updateQueries[i].Sql.Replace("where", "output INSERTED." + returnIdCaption + " where");
                                    }

                                    commandLine.CommandText = lineQuery;
                                    commandLine.Transaction = transaction;

                                    commandLine.Parameters.Clear();

                                    if(updateQueries.Count>0)
                                    {
                                        foreach (var item in updateQueries[i].ParameterList)
                                        {
                                            var parameter = commandLine.CreateParameter();
                                            parameter.ParameterName = item.Key;
                                            parameter.Value = item.Value ?? DBNull.Value;

                                            if (!commandLine.Parameters.Contains(parameter.ParameterName))
                                            {
                                                commandLine.Parameters.Add(parameter);
                                            }
                                        }
                                    }

                                    if (whereQueries.Count > 0)
                                    {
                                        foreach (var item in whereQueries[i].ParameterList)
                                        {
                                            var parameter = commandLine.CreateParameter();
                                            parameter.ParameterName = item.Key;
                                            parameter.Value = item.Value ?? DBNull.Value;

                                            if (!commandLine.Parameters.Contains(parameter.ParameterName))
                                            {
                                                commandLine.Parameters.Add(parameter);
                                            }
                                        }
                                    }


                                    commandLine.ExecuteScalar();
                                }
                            }

                            transaction.Commit();

                            var resultSql = query.UseIsDeleteInQuery
                               ? query.From(query.TableName).Select().Where(returnIdCaption, _id.ToString(), query.JoinSeperator)
                               : query.From(query.TableName).Select().Where(returnIdCaption, _id.ToString(), query.JoinSeperator).UseIsDelete(false);

                            var result = Get<T>(resultSql);

                            query.SqlResult = result;

                            query.JsonData = query.SqlResult != null ? JsonConvert.SerializeObject(query.SqlResult, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) : "";

                            returnValue = (T)query.SqlResult;

                            Connection.Close();
                            Connection.Dispose();
                            GC.Collect();
                        }
                    }
                }

                UpdateHelper.UpdateQueris.Clear();
                WhereHelper.WhereQueries.Clear();
                return returnValue;
            }
            catch (Exception exp)
            {
                UpdateHelper.UpdateQueris.Clear();
                WhereHelper.WhereQueries.Clear();
                transaction.Rollback();

                Connection.Close();
                Connection.Dispose();
                GC.Collect();
                var error = ErrorException.ThrowException(exp);
                return default(T);
            }
        }
    }
}
