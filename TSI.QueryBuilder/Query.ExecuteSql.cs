using System;
using System.Collections.Generic;
using System.Text;

namespace TSI.QueryBuilder
{
    public partial class Query
    {
        public Query ExecuteSql(string command, params string[] columns)
        {
            Method = "select";


            Sql = command;

            if (columns.Length != 0)
            {

                for (int i = 0; i < columns.Length; i++)
                {
                    string replacingvalue = "{" + i.ToString() + "}";

                    string newvalue = columns[i];

                    Sql = Sql.Replace(replacingvalue, newvalue);

                }

            }

            return this;
        }
    }
}
