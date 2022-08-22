using System;
using System.Collections.Generic;
using System.Text;

namespace Tsi.Core.Modularity
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RelatedModulesAttribute : Attribute, IDependedTypesProvider
    {
        public Type[] DependedTypes { get; }

        public RelatedModulesAttribute(params Type[] dependedTypes)
        {
            DependedTypes = dependedTypes ?? new Type[0];
        }

        public virtual Type[] GetDependedTypes()
        {
            return DependedTypes;
        }
    }
}
