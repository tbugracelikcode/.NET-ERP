using System;
using System.Collections.Generic;
using System.Text;

namespace TSI.QueryBuilder
{
    public partial class Insert
    {
        public Insert Values(params string[] values)
        {
            if(values.Length>0)
            {
                string queryvalues = "";

                for (int i = 0; i < values.Length; i++)
                {
                    if (i == 0)
                    {
                        queryvalues =  "'" + values[i] + "'";
                    }
                    else
                    {
                        queryvalues = queryvalues + "," + "'" + values[i] + "'";
                    }
                }

                Sql = Sql + " values (" + queryvalues + ")";
            }

            return this;
        }
    }
}
