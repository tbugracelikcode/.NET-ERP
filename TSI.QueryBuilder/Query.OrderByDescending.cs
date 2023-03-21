using System;
using System.Collections.Generic;
using System.Text;

namespace TSI.QueryBuilder
{
    public partial class Query
    {
        public Query OrderByDescending(string column)
        {
            Method = "select";

            string order = " order by ";

            string descending = " desc";

            Sql = Sql + order + column + descending;

            return this;
        }
    }
}
