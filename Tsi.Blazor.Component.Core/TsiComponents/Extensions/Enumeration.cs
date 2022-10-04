using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsi.Blazor.Component.Core.TsiComponents.Extensions
{
    public record Enumeration<T>
       where T : Enumeration<T>
    {

        string name;

        string cachedName;

        public Enumeration(string name)
        {
            Name = name;
            ParentEnumeration = default;
        }

        protected Enumeration(T parentEnumeration, string name)
        {
            this.Name = name;
            this.ParentEnumeration = parentEnumeration;
        }

        private string BuildName()
        {
            var sb = new StringBuilder();

            if (ParentEnumeration != null)
                sb.Append(ParentEnumeration.Name).Append(' ');

            sb.Append(name);

            return sb.ToString();
        }


        public string Name
        {
            get
            {
                if (cachedName == null)
                    cachedName = BuildName();

                return cachedName;
            }
            private set
            {
                if (name == value)
                    return;

                name = value;
                cachedName = null;
            }
        }

        public T ParentEnumeration { get; private set; }

    }
}
