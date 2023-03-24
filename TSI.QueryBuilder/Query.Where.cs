using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TSI.QueryBuilder
{
    public partial class Query
    {
        public Query Where(object constraints)
        {
            var dictionary = new Dictionary<string, object>();

            foreach (var item in constraints.GetType().GetRuntimeProperties())
            {
                dictionary.Add(item.Name, item.GetValue(constraints));
            }

            return Where(dictionary);
        }

        public Query Where(IEnumerable<KeyValuePair<string, object>> values)
        {
            var query = (Query)this;


            foreach (var tuple in values)
            {

            }

            return query;
        }

        public Query Where(string column, object value)
        {
            return Where(column, "=", value);
        }

        public Query Where(string column, string op, object value)
        {
            Method = "select";

            string tableName = TableName;

            if (!string.IsNullOrEmpty(tableName))
            {
                int insertPoint = Sql.LastIndexOf(tableName);
                Sql = Sql.Insert(insertPoint, " where " + column + " " + op + " " + value);
            }

            return this;
        }
    }
}
