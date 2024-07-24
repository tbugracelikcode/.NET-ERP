using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.DataAccess.Utilities
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DatabaseAddColumnAttribute : Attribute
    {
        public SqlDataType SqlDbType { get; set; }

        public bool Nullable { get; set; } = false;

        public int MaxLength { get; set; } = 0;

        public int Scale { get; set; } = 0;

        public int Precision { get; set; } = 0;

        public bool IsPrimaryKey { get; set; } = false;

        public string TableName { get; set; }

        public string Default_ { get; set; }
    }
}
