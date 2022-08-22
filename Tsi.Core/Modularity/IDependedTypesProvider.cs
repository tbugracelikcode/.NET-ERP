using System;
using System.Collections.Generic;
using System.Text;

namespace Tsi.Core.Modularity
{
    public interface IDependedTypesProvider
    {
        Type[] GetDependedTypes();
    }
}
