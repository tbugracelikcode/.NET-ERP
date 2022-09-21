using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsi.Blazor.Component.Core.Services
{
    public interface ICoreCommonService
    {
        public ComponentBase ActiveEditComponent { get; set; }
    }
}
