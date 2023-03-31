using System;
using System.Collections.Generic;
using System.Text;

namespace TSI.QueryBuilder
{
    public partial class Query
    {
        public Query Take(int limit)
        {
            //Method = "select";

            string replacingvalue = " top " + limit.ToString() + " ";

            //Sql = Sql.Replace("*", replacingvalue);


            Sql = Sql.Insert(6, replacingvalue).Remove(6 + replacingvalue.Length, 1);

            return this;
        }
    }
}
