using System;
using System.Collections.Generic;
using System.Text;

namespace TSI.QueryBuilder
{
    public partial class Query
    {
        public Query Count(string ColumnName)
        {
            Sql = "select count(" + ColumnName + ") from " + TableName;

            IsMapQuery = false;

            return this;
        }
    }
}
