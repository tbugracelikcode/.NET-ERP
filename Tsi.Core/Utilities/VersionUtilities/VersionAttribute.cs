using System;
using System.Collections.Generic;
using System.Text;

namespace Tsi.Core.Utilities.VersionUtilities
{
    [AttributeUsage(AttributeTargets.Method)]
    public class VersionAttribute : Attribute
    {
        public string VersiyonNumber { get; set; }
    }
}
