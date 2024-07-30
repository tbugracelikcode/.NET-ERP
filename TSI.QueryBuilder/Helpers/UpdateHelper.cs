using System;
using System.Collections.Generic;
using System.Text;
using TSI.QueryBuilder.Models;

namespace TSI.QueryBuilder.Helpers
{
    public static class UpdateHelper
    {
        public static List<UpdateQuerySQL> UpdateQueris;

        public static void UpdateQueryList(UpdateQuerySQL querySQL)
        {
            if (UpdateQueris == null)
            {
                UpdateQueris = new List<UpdateQuerySQL>();
            }

            UpdateQueris.Add(querySQL);
        }
    }
}
