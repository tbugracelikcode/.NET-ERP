using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TSI.QueryBuilder.BaseClasses;

namespace TSI.QueryBuilder
{
    public partial class Query : QueryFactory
    {
        //public string Method { get; set; }

        public string Sql { get; set; }

        public string TableName { get; set; }

        public object SqlResult { get; set; }

        public string JsonData { get; set; }

        public string WhereSentence { get; set; }
    }
}
