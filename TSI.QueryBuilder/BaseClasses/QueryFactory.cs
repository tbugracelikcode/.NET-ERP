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

            if (command != null)
            {
                command.CommandText = query.Sql;
                return command.ExecuteReader().DataReaderMapToList<T>();
            }
            else
            {

                return null;
            }
        }
    }
}
