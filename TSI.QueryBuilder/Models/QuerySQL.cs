using System;
using System.Collections.Generic;
using System.Text;

namespace TSI.QueryBuilder.Models
{
    public class QuerySQL
    {
        public string Sql { get; set; }

        public Dictionary<string, object> ParameterList { get; set; }

        public QuerySQL()
        {
            ParameterList = new Dictionary<string, object>();
        }
    }
}
