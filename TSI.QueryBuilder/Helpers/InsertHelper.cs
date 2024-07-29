using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using TSI.QueryBuilder.Models;

namespace TSI.QueryBuilder.Helpers
{
    public static class InsertHelper
    {
        public static List<InsertQuerySQL> InsertQueris;

        public static void InsertQueryList(InsertQuerySQL querySQL)
        {
            if (InsertQueris == null)
            {
                InsertQueris = new List<InsertQuerySQL>();
            }

            InsertQueris.Add(querySQL);
        }
    }
}
