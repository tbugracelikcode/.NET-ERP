
using System;
using Tsi.Core.Utilities.Guids;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Logging.Services;

namespace TsiErp.Business.BusinessCoreServices
{
    [ServiceRegistration(typeof(IApplicationService), DependencyInjectionType.Transient)]
    public class ApplicationService : IApplicationService
    {
        public IGuidGenerator GuidGenerator { get; set; } = new SequentialGuidGenerator();

    }
}
