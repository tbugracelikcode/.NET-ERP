using System;
using System.Collections.Generic;
using System.Text;

namespace TSI.QueryBuilder
{
    public partial class Query
    {
        public Query InnerJoin(string secondtable, string column)
        {
            //Method = "select";

            int point = Sql.IndexOf('*') + 6;

            int length = Sql.Length - 1;

            string firsttable = Sql.Substring(length, point);

            Sql = Sql + " inner join " + secondtable + " on " + firsttable + "." + column + " = " + secondtable + "." + column;

            return this;
        }
    }
}
