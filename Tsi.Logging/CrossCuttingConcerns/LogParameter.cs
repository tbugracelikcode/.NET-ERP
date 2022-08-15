using System;
using System.Collections.Generic;
using System.Text;

namespace Tsi.Logging.CrossCuttingConcerns
{
    public class LogParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public string Type { get; set; }
    }
}
