using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Blazor.Component.Core.TsiComponents.Extensions;

namespace Tsi.Blazor.Component.Core.TsiComponents.Enums
{
    public record Color : Enumeration<Color>
    {


        public Color(string name) : base(name)
        {
        }

        private Color(Color parent, string name) : base(parent, name)
        {
        }

        public static implicit operator Color(string name)
        {
            return new Color(name);
        }

        public static readonly Color Default = new((string)null);

        public static readonly Color Primary = new("primary");

        public static readonly Color Secondary = new("secondary");

        public static readonly Color Success = new("success");

        public static readonly Color Danger = new("danger");

        public static readonly Color Warning = new("warning");

        public static readonly Color Info = new("info");

        public static readonly Color Light = new("light");

        public static readonly Color Dark = new("dark");

        public static readonly Color Link = new("link");
    }
}
