using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Business.Exceptions
{
    public class DuplicateCodeException : Exception
    {
        public DuplicateCodeException(string code) : base(code)
        {
            Data["code"] = code;
        }

        public DuplicateCodeException WithData(string code, object value)
        {
            Data[code] = value;
            return this;
        }
    }
}
