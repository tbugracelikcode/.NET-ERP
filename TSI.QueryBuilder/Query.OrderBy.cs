using System;
using System.Collections.Generic;
using System.Text;

namespace TSI.QueryBuilder
{
    public partial class Query
    {
        public Query OrderBy( string column)
        {
            Method = "select";

            string order = " order by ";

            Sql = Sql + order + column;

            return this;
        }
    }
}
