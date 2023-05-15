using System;
using System.Collections.Generic;
using System.Text;

namespace TSI.QueryBuilder.MappingAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NoDatabaseActionAttribute : Attribute
    {
    }
}
