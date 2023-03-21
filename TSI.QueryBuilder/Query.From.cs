using System;
using System.Collections.Generic;
using System.Text;

namespace TSI.QueryBuilder
{
    public partial class Query
    {
        public Query From(string table)
        {
            Method = "select";

            TableName = table;

            Sql = Method + " * " + "from " + table;

            return this;
        }
    }
}
