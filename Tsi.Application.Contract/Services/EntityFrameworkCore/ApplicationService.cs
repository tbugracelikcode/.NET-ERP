using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Guids;
using Tsi.IoC.Tsi.DependencyResolvers;

namespace Tsi.Application.Contract.Services.EntityFrameworkCore
{
    [ServiceRegistration(typeof(IApplicationService), DependencyInjectionType.Transient)]
    public class ApplicationService : IApplicationService
    {
        public IGuidGenerator GuidGenerator { get; set; } = new SequentialGuidGenerator();
    }
}
