using System;
using System.Collections.Generic;
using System.Text;
using TSI.QueryBuilder.Models;

namespace TSI.QueryBuilder.Helpers
{
    public static class WhereHelper
    {
        public static List<WhereQuerySQL> WhereQueries;

        public static void WhereQueryList(WhereQuerySQL querySQL)
        {
            if (WhereQueries == null)
            {
                WhereQueries = new List<WhereQuerySQL>();
            }

            WhereQueries.Add(querySQL);
        }
    }
}
