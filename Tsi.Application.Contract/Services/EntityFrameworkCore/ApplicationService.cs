using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Utilities.Guids;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;

namespace Tsi.Application.Contract.Services.EntityFrameworkCore
{
    [ServiceRegistration(typeof(IApplicationService), DependencyInjectionType.Transient)]
    public class ApplicationService : IApplicationService
    {
        public IGuidGenerator GuidGenerator { get; set; } = new SequentialGuidGenerator();
    }
}
