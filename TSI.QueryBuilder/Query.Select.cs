using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace TSI.QueryBuilder
{
    public partial class Query
    {
        public Query Select(params string[] columns)
        {
            //Method = "select";

            string queryColumns = "";

            for (int i = 0; i < columns.Length; i++)
            {
                if (i == 0)
                {
                    queryColumns = columns[i];
                }
                else
                {
                    queryColumns = queryColumns + "," + columns[i];
                }
            }

            Sql = "select" + " * "+ "from " + TableName;

            var insertPoint = Sql.IndexOf('*');

            Sql = Sql.Insert(insertPoint, queryColumns).Remove(insertPoint + queryColumns.Length, 1);

            return this;
        }

        public Query Select()
        {
            //Method = "select";

            Sql = "select" + " * " + "from " + TableName;

            return this;
        }
    }
}
