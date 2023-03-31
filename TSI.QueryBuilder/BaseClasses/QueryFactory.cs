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

        public int CommandTimeOut { get; set; } = 60;


        public QueryFactory(IDbConnection connection)
        {
            Connection = connection;
        }

        public Query Query()
        {
            return new Query();
        }


        public IEnumerable<T>? GetList<T>(Query query)
        {
            var command = Connection.CreateCommand();

            command.CommandTimeout = CommandTimeOut;

            if (command != null)
            {
                command.CommandText = query.Sql;

                query.SqlResult = command.ExecuteReader().DataReaderMapToList<T>();

                return query.SqlResult as IEnumerable<T>;
            }
            else
            {
                return null;
            }
        }



        public int? Create(Query query, string returnIdCaption)
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


        public IEnumerable<T>? GetList<T>(Query query, bool toJsonObject)
        {
            var command = Connection.CreateCommand();

            command.CommandTimeout = CommandTimeOut;

            if (command != null)
            {
                command.CommandText = query.Sql;

                query.SqlResult = command.ExecuteReader().DataReaderMapToList<T>();

                if (toJsonObject)
                {
                    query.JsonData = JsonConvert.SerializeObject(query.SqlResult, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                }

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

        public string ToJsonObject(object sqlResult)
        {
            return JsonConvert.SerializeObject(sqlResult, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }
    }
}
