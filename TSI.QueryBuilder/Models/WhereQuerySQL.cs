using System;
using System.Collections.Generic;
using System.Text;

namespace TSI.QueryBuilder.Models
{
    public class WhereQuerySQL
    {
        public string Sql { get; set; }

        public Dictionary<string, object> ParameterList { get; set; }

        public WhereQuerySQL()
        {
            ParameterList = new Dictionary<string, object>();
        }
    }
}
