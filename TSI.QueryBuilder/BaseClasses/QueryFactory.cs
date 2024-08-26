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
                                if (!string.IsNullOrEmpty(query.WhereSentence))
                                {
                                    if (query.WhereSentence.Contains(QueryConstants.QueryWhereParamsConstant))
                                    {
                                        query.WhereSentence = isDeleted + " and " + query.WhereSentence;
                                    }
                                    else
                                    {
                                        query.WhereSentence = query.WhereSentence + " and " + isDeleted;
                                    }

                                    query.Sql = query.Sql + " where " + query.WhereSentence;
                                }
                                else
                                {
                                    query.Sql = query.Sql + " where " + query.WhereSentence;
                                }
                            }
                            else
                            {
                                query.Sql = query.Sql + " where " + query.WhereSentence;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(query.WhereSentence))
                    {
                        if (query.WhereSentence.Contains(QueryConstants.QueryWhereParamsConstant))
                        {
                            command.Parameters.Clear();

                            var parameters = query.Sql.Split(QueryConstants.QueryWhereParamsConstant).LastOrDefault().Split(',');

                            foreach (var item in parameters)
                            {
                                var parameter = command.CreateParameter();
                                parameter.ParameterName = item.Split('=').FirstOrDefault();
                                parameter.Value = item.Split('=').LastOrDefault();

                                command.Parameters.Add(parameter);
                            }
                        }
                    }


                    string sql = "";

                    if (!string.IsNullOrEmpty(query.WhereSentence))
                    {
                        if (query.WhereSentence.Contains(QueryConstants.QueryWhereParamsConstant))
                        {
                            sql = query.Sql.Split(QueryConstants.QueryWhereParamsConstant).FirstOrDefault();
                        }
                        else
                        {
                            sql = query.Sql;
                        }
                    }
                    else
                    {
                        sql = query.Sql;
                    }

                    command.CommandText = sql;

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
                                if (!string.IsNullOrEmpty(query.WhereSentence))
                                {
                                    if (query.WhereSentence.Contains(QueryConstants.QueryWhereParamsConstant))
                                    {
                                        query.WhereSentence = isDeleted + " and " + query.WhereSentence;
                                    }
                                    else
                                    {
                                        query.WhereSentence = query.WhereSentence + " and " + isDeleted;
                                    }

                                    query.Sql = query.Sql + " where " + query.WhereSentence;
                                }
                                else
                                {
                                    query.Sql = query.Sql + " where " + query.WhereSentence;
                                }
                            }
                            else
                            {
                                query.Sql = query.Sql + " where " + query.WhereSentence;
                            }
                        }
                    }


                    if (!string.IsNullOrEmpty(query.WhereSentence))
                    {
                        if (query.WhereSentence.Contains(QueryConstants.QueryWhereParamsConstant))
                        {
                            command.Parameters.Clear();

                            var parameters = query.Sql.Split(QueryConstants.QueryWhereParamsConstant).LastOrDefault().Split(',');

                            foreach (var item in parameters)
                            {
                                var parameter = command.CreateParameter();
                                parameter.ParameterName = item.Split('=').FirstOrDefault();
                                parameter.Value = item.Split('=').LastOrDefault();

                                command.Parameters.Add(parameter);
                            }
                        }
                    }


                    string sql = "";

                    if (!string.IsNullOrEmpty(query.WhereSentence))
                    {
                        if (query.WhereSentence.Contains(QueryConstants.QueryWhereParamsConstant))
                        {
                            sql = query.Sql.Split(QueryConstants.QueryWhereParamsConstant).FirstOrDefault();
                        }
                        else
                        {
                            sql = query.Sql;
                        }
                    }
                    else
                    {
                        sql = query.Sql;
                    }

                    command.CommandText = sql;

                    query.SqlResult = command.ExecuteReader().DataReaderMapToList<T>();

                    query.JsonData = query.SqlResult != null ? JsonConvert.SerializeObject(query.SqlResult, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) : "";

                    Connection.Close();
                    Connection.Dispose();
                    GC.Collect();

                    return query.SqlResult as IEnumerable<T>;
                }
                else
                {
                    Connection.Close();
                    Connection.Dispose();
                    GC.Collect();

                    return null;
                }
            }
            catch (Exception exp)
            {
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
                    query.Sql = query.Sql + " where " + query.WhereSentence;

                    string sql = "";

                    if (!string.IsNullOrEmpty(query.WhereSentence))
                    {
                        if (query.WhereSentence.Contains(QueryConstants.QueryWhereParamsConstant))
                        {
                            sql = query.Sql.Split(QueryConstants.QueryWhereParamsConstant).FirstOrDefault();
                        }
                        else
                        {
                            sql = query.Sql;
                        }
                    }
                    else
                    {
                        sql = query.Sql;
                    }



                    if (!string.IsNullOrEmpty(query.WhereSentence))
                    {
                        if (query.WhereSentence.Contains(QueryConstants.QueryWhereParamsConstant))
                        {

                            command.Parameters.Clear();

                            var parameters = query.Sql.Split(QueryConstants.QueryWhereParamsConstant).LastOrDefault().Split(',');

                            foreach (var item in parameters)
                            {
                                var parameter = command.CreateParameter();
                                parameter.ParameterName = item.Split('=').FirstOrDefault();
                                parameter.Value = item.Split('=').LastOrDefault();

                                command.Parameters.Add(parameter);
                            }
                        }
                    }

                    command.CommandText = sql;

                    query.SqlResult = command.ExecuteReader().DataReaderMapToList<T>();

                    query.JsonData = query.SqlResult != null ? JsonConvert.SerializeObject(query.SqlResult, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) : "";

                    Connection.Close();
                    Connection.Dispose();
                    GC.Collect();

                    return query.SqlResult as IEnumerable<T>;
                }
                else
                {
                    Connection.Close();
                    Connection.Dispose();
                    GC.Collect();

                    return null;
                }
            }
            catch (Exception exp)
            {
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
                string[] insertQueries = query.Sql.Split(QueryConstants.QueryConstant);

                Guid _id = Guid.Empty;

                for (int i = 0; i < insertQueries.Length; i++)
                {
                    var lineQuery = insertQueries[i];

                    var commandLine = Connection.CreateCommand();
                    commandLine.CommandTimeout = CommandTimeOut;

                    string sql = "";
                    sql = lineQuery.Split(QueryConstants.QueryParamsConstant).FirstOrDefault();
                    sql = sql.Replace("values", "output INSERTED." + returnIdCaption + " values");

                    if (sql.StartsWith("insert"))
                    {
                        commandLine.Parameters.Clear();

                        string[] parameters = lineQuery.Split(QueryConstants.QueryParamsConstant).LastOrDefault().Split(',');

                        foreach (var param in parameters)
                        {
                            var parameter = commandLine.CreateParameter();
                            parameter.ParameterName = param.Split('=').FirstOrDefault();
                            parameter.Value = param.Split('=').LastOrDefault();

                            commandLine.Parameters.Add(parameter);
                        }
                    }

                    if (sql.StartsWith("update"))
                    {
                        string where = lineQuery.Split("where").LastOrDefault().Split(QueryConstants.QueryWhereParamsConstant).FirstOrDefault();
                        sql = lineQuery.Split(QueryConstants.QueryParamsConstant).FirstOrDefault();
                        sql = sql + " where " + where;

                        commandLine.Parameters.Clear();

                        string[] parameters = lineQuery.Split(QueryConstants.QueryParamsConstant).LastOrDefault().Split(',');

                        foreach (var param in parameters)
                        {
                            var parameter = commandLine.CreateParameter();

                            if (param.Contains("where"))
                            {
                                parameter.ParameterName = param.Split("where").FirstOrDefault().Split('=').FirstOrDefault();
                                parameter.Value = param.Split("where").FirstOrDefault().Split('=').LastOrDefault();
                            }
                            else
                            {
                                parameter.ParameterName = param.Split('=').FirstOrDefault();
                                parameter.Value = param.Split('=').LastOrDefault();
                            }

                            commandLine.Parameters.Add(parameter);
                        }

                        if (!string.IsNullOrEmpty(where))
                        {

                            var whereParameters = lineQuery.Split("where").LastOrDefault().Split(QueryConstants.QueryWhereParamsConstant).LastOrDefault().Split(',');

                            foreach (var item in whereParameters)
                            {
                                var parameter = commandLine.CreateParameter();
                                parameter.ParameterName = item.Split('=').FirstOrDefault();
                                parameter.Value = item.Split('=').LastOrDefault();

                                commandLine.Parameters.Add(parameter);
                            }

                        }

                        sql = sql.Replace("where", "output INSERTED." + returnIdCaption + " where");
                    }



                    commandLine.CommandText = sql;

                    commandLine.Transaction = transaction;

                    if (i == 0)
                    {
                        _id = (Guid)commandLine.ExecuteScalar();
                    }
                    else
                    {
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
                return returnValue;
            }
            catch (Exception exp)
            {

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
                string[] updateQueries = query.Sql.Split(QueryConstants.QueryConstant);

                Guid _id = Guid.Empty;

                for (int i = 0; i < updateQueries.Length; i++)
                {
                    var lineQuery = updateQueries[i];

                    var commandLine = Connection.CreateCommand();
                    commandLine.CommandTimeout = CommandTimeOut;

                    string sql = "";


                    if (i == 0)
                    {
                        if (!string.IsNullOrEmpty(query.WhereSentence))
                        {
                            if (query.WhereSentence.Contains(QueryConstants.QueryWhereParamsConstant))
                            {
                                sql = lineQuery.Split(QueryConstants.QueryParamsConstant).FirstOrDefault();
                                sql = sql + " where " + query.WhereSentence;
                                sql = sql.Split(QueryConstants.QueryWhereParamsConstant).FirstOrDefault();
                            }
                            else
                            {
                                sql = lineQuery.Split(QueryConstants.QueryParamsConstant).FirstOrDefault();
                                sql = sql + " where " + query.WhereSentence;
                            }
                        }
                        else
                        {
                            sql = lineQuery.Split(QueryConstants.QueryParamsConstant).FirstOrDefault();
                            sql = sql + " where " + query.WhereSentence;
                        }

                        commandLine.Parameters.Clear();

                        string[] parameters = lineQuery.Split(QueryConstants.QueryParamsConstant).LastOrDefault().Split(',');

                        foreach (var param in parameters)
                        {
                            var parameter = commandLine.CreateParameter();
                            parameter.ParameterName = param.Split('=').FirstOrDefault();
                            parameter.Value = param.Split('=').LastOrDefault();

                            commandLine.Parameters.Add(parameter);
                        }

                        if (!string.IsNullOrEmpty(query.WhereSentence))
                        {
                            if (query.WhereSentence.Contains(QueryConstants.QueryWhereParamsConstant))
                            {
                                var whereParameters = query.WhereSentence.Split(QueryConstants.QueryWhereParamsConstant).LastOrDefault().Split(',');

                                foreach (var item in whereParameters)
                                {
                                    var parameter = commandLine.CreateParameter();
                                    parameter.ParameterName = item.Split('=').FirstOrDefault();
                                    parameter.Value = item.Split('=').LastOrDefault();

                                    commandLine.Parameters.Add(parameter);
                                }
                            }
                        }

                        sql = sql.Replace("where", "output INSERTED." + returnIdCaption + " where");

                    }
                    else
                    {
                        if (lineQuery.StartsWith("insert"))
                        {
                            sql = lineQuery.Split(QueryConstants.QueryParamsConstant).FirstOrDefault();
                            sql = sql.Replace("values", "output INSERTED." + returnIdCaption + " values");

                            commandLine.Parameters.Clear();

                            string[] parameters = lineQuery.Split(QueryConstants.QueryParamsConstant).LastOrDefault().Split(',');

                            foreach (var param in parameters)
                            {
                                var parameter = commandLine.CreateParameter();
                                parameter.ParameterName = param.Split('=').FirstOrDefault();
                                parameter.Value = param.Split('=').LastOrDefault();

                                commandLine.Parameters.Add(parameter);
                            }
                        }
                        else
                        {
                            string where = lineQuery.Split("where").LastOrDefault().Split(QueryConstants.QueryWhereParamsConstant).FirstOrDefault();
                            sql = lineQuery.Split(QueryConstants.QueryParamsConstant).FirstOrDefault();
                            sql = sql + " where " + where;

                            commandLine.Parameters.Clear();

                            string[] parameters = lineQuery.Split(QueryConstants.QueryParamsConstant).LastOrDefault().Split(',');

                            foreach (var param in parameters)
                            {
                                var parameter = commandLine.CreateParameter();

                                if (param.Contains("where"))
                                {
                                    parameter.ParameterName = param.Split("where").FirstOrDefault().Split('=').FirstOrDefault();
                                    parameter.Value = param.Split("where").FirstOrDefault().Split('=').LastOrDefault();
                                }
                                else
                                {
                                    parameter.ParameterName = param.Split('=').FirstOrDefault();
                                    parameter.Value = param.Split('=').LastOrDefault();
                                }

                                commandLine.Parameters.Add(parameter);
                            }

                            if (!string.IsNullOrEmpty(where))
                            {

                                var whereParameters = lineQuery.Split("where").LastOrDefault().Split(QueryConstants.QueryWhereParamsConstant).LastOrDefault().Split(',');

                                foreach (var item in whereParameters)
                                {
                                    var parameter = commandLine.CreateParameter();
                                    parameter.ParameterName = item.Split('=').FirstOrDefault();
                                    parameter.Value = item.Split('=').LastOrDefault();

                                    commandLine.Parameters.Add(parameter);
                                }

                            }

                            sql = sql.Replace("where", "output INSERTED." + returnIdCaption + " where");
                        }
                    }

                    commandLine.CommandText = sql;

                    commandLine.Transaction = transaction;

                    if (i == 0)
                    {
                        _id = (Guid)commandLine.ExecuteScalar();
                    }
                    else
                    {
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
            catch (Exception exp)
            {
                transaction.Rollback();

                Connection.Close();
                Connection.Dispose();
                GC.Collect();
                var error = ErrorException.ThrowException(exp);
                return default(T);
            }

            return returnValue;
        }

        public bool Delete(Query query, bool useTransaction = true)
        {
            ConnectToDatabase();
            IDbTransaction transaction = Connection.BeginTransaction();
            try
            {
                var command = Connection.CreateCommand();

                command.CommandTimeout = CommandTimeOut;

                if (command != null)
                {
                    string sql = query.Sql + " where " + query.WhereSentence;

                    if (!string.IsNullOrEmpty(query.WhereSentence))
                    {
                        if (query.WhereSentence.Contains(QueryConstants.QueryWhereParamsConstant))
                        {
                            command.Parameters.Clear();

                            var parameters = sql.Split(QueryConstants.QueryWhereParamsConstant).LastOrDefault().Split(',');

                            foreach (var item in parameters)
                            {
                                var parameter = command.CreateParameter();
                                parameter.ParameterName = item.Split('=').FirstOrDefault();
                                parameter.Value = item.Split('=').LastOrDefault();

                                command.Parameters.Add(parameter);
                            }
                        }
                    }



                    if (!string.IsNullOrEmpty(query.WhereSentence))
                    {
                        if (query.WhereSentence.Contains(QueryConstants.QueryWhereParamsConstant))
                        {
                            sql = sql.Split(QueryConstants.QueryWhereParamsConstant).FirstOrDefault();
                        }
                        else
                        {
                            sql = query.Sql;
                        }
                    }
                    else
                    {
                        sql = query.Sql;
                    }



                    command.CommandText = sql;
                    command.Transaction = transaction;

                    query.SqlResult = command.ExecuteReader();
                    transaction.Commit();

                    Connection.Close();
                    Connection.Dispose();
                    GC.Collect();
                    return true;
                }
                else
                {
                    transaction.Rollback();
                    Connection.Close();
                    Connection.Dispose();
                    GC.Collect();
                    return false;
                }
            }
            catch (Exception exp)
            {
                transaction.Rollback();
                Connection.Close();
                Connection.Dispose();
                GC.Collect();
                var error = ErrorException.ThrowException(exp);
                return false;
            }
        }

    }
}
