using System;
using System.Collections.Generic;
using System.Text;

namespace TSI.QueryBuilder
{
    public partial class Query
    {
        public Query Sum(string ColumnName)
        {
            Sql = "select sum(" + ColumnName + ") from " + TableName;

            IsSumQuery = true;

            return this;
        }
    }
}
