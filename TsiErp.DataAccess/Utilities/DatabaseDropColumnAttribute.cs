using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.DataAccess.Utilities
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DatabaseDropColumnAttribute : Attribute
    {

        public string TableName { get; set; }

        public string ColumnName { get; set; }
    }
}
