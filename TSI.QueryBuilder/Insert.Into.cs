using System;
using System.Collections.Generic;
using System.Text;

namespace TSI.QueryBuilder
{
    public partial class Insert
    {
        public Insert Into(string tableName, params string[] columns)
        {
            TableName = tableName;
            Method = "insert";

            if(columns.Length > 0)
            {
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

                Sql = Method + " into " + TableName + " (" + queryColumns + ")";
            }
            else
            {
                Sql = Method + " into " + TableName;
            }

            return this;
        }
    }
}
