using System;
using System.Collections.Generic;
using System.Text;
using TSI.QueryBuilder.BaseClasses;

namespace TSI.QueryBuilder
{
    public partial class Insert : BaseQueryBuilder<Insert>
    {
        public string TableName { get; set; }
        public string[] ColumnName { get; set; }
        public string[] InsertingValues { get; set; }
        public string Sql { get; set; }
        public string Method { get; set; }
    }
}
