using System;
using System.Collections.Generic;
using System.Text;

namespace TSI.QueryBuilder
{
    public partial class Query
    {
        public Query Take(string column, int limit)
        {
            Method = "select";

            if(column == "")
            {
                column = "*";
            }

            string replacingvalue = "top " + limit.ToString() + " " + column;

            Sql = Sql.Replace("*", replacingvalue);

            return this;
        }
    }
}
