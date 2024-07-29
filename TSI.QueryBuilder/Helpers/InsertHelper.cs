using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using TSI.QueryBuilder.Models;

namespace TSI.QueryBuilder.Helpers
{
    public static class InsertHelper
    {
        public static List<QuerySQL> InsertQueris;

        public static void InsertQueryList(QuerySQL querySQL)
        {
            if (InsertQueris == null)
            {
                InsertQueris = new List<QuerySQL>();
            }

            InsertQueris.Add(querySQL);
        }
    }
}
