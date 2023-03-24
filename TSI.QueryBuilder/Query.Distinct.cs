using System;
using System.Collections.Generic;
using System.Text;

namespace TSI.QueryBuilder
{
    public partial class Query
    {
        public Query Distinct(string column)
        {
            Method = "select";

            if (column != "")
            {
                string replacingvalue = " distinct " + column;

                Sql = Sql.Replace("*", replacingvalue);
            }

           

            return this;
        }
    }
}
