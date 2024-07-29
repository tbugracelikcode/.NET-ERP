using System;
using System.Collections.Generic;
using System.Text;

namespace TSI.QueryBuilder.Models
{
    public class InsertQuerySQL
    {
        public string Sql { get; set; }

        public Dictionary<string, object> ParameterList { get; set; }

        public InsertQuerySQL()
        {
            ParameterList = new Dictionary<string, object>();
        }
    }
}
