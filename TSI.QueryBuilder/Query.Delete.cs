using System;
using System.Collections.Generic;
using System.Text;

namespace TSI.QueryBuilder
{
    public partial class Query
    {
        public Query HardDelete()
        {
            string deleteQuery = " delete from " + TableName;
            Sql = deleteQuery;
            return this;
        }
    }
}
