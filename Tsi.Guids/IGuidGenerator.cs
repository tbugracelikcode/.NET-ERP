using System;
using System.Collections.Generic;
using System.Text;
using Tsi.IoC.IoC.Autofac;

namespace Tsi.Guids
{
    public interface IGuidGenerator
    {
        Guid CreateGuid();
    }
}
