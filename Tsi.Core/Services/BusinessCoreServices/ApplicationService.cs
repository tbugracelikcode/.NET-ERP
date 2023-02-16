
using System;
using Tsi.Core.Utilities.Guids;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;

namespace Tsi.Core.Services.BusinessCoreServices
{
    [ServiceRegistration(typeof(IApplicationService), DependencyInjectionType.Transient)]
    public class ApplicationService : IApplicationService
    {
        public IGuidGenerator GuidGenerator { get; set; } = new SequentialGuidGenerator();
    }
}
