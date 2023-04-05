using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TSI.QueryBuilder.Extensions;
using TSI.QueryBuilder.Models;

namespace TSI.QueryBuilder.BaseClasses
{
    public class QueryFactory
    {
        public IDbConnection Connection { get; set; }

        public int CommandTimeOut { get; set; } = 600;

        public QueryFactory(IDbConnection connection)
        {
            Connection = connection;
        }

        public Query Query()
        {
            return new Query();
        }

        public T Get<T>(Query query)
        {
            var command = Connection.CreateCommand();

            command.CommandTimeout = CommandTimeOut;

            if (command != null)
            {
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

        public IEnumerable<T>? GetList<T>(Query query)
        {
            var command = Connection.CreateCommand();

            command.CommandTimeout = CommandTimeOut;

            if (command != null)
            {
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

        public IEnumerable<T>? GetArray<T>(Query query)
        {
            var command = Connection.CreateCommand();

            command.CommandTimeout = CommandTimeOut;

            if (command != null)
            {
                command.CommandText = query.Sql;

                query.SqlResult = command.ExecuteReader().DataReaderMapToArray<T>();

                return query.SqlResult as IEnumerable<T>;
            }
            else
            {

                return null;
            }
        }

        public int? Insert(Query query, string returnIdCaption)
        {
            var command = Connection.CreateCommand();

            command.CommandTimeout = CommandTimeOut;

            if (command != null)
            {
                query.Sql = query.Sql.Replace("values", "output INSERTED." + returnIdCaption + " values");
                command.CommandText = query.Sql;
                int _id = (int)command.ExecuteScalar();
                return _id;
            }
            else
            {
                return null;
            }
        }

        public T Insert<T>(Query query,string returnIdCaption)
        {
            var command = Connection.CreateCommand();

            command.CommandTimeout = CommandTimeOut;

            if (command != null)
            {
                query.Sql = query.Sql.Replace("values", "output INSERTED." + returnIdCaption + " values");

                command.CommandText = query.Sql;

                int _id = (int)command.ExecuteScalar();

                var resultSql = query.From(query.TableName).Select().Where(returnIdCaption, _id.ToString());

                var result = Get<T>(resultSql);

                return (T)result;
            }
            else
            {
                return default(T);
            }
        }
    }
}
