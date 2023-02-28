
using Microsoft.Extensions.Localization;
using System;
using Tsi.Core.Utilities.Guids;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Logging.Services;

namespace TsiErp.Business.BusinessCoreServices
{
    //[ServiceRegistration(typeof(IApplicationService), DependencyInjectionType.Transient)]
    public class ApplicationService<TResource>
    {
        public IGuidGenerator GuidGenerator { get; set; } = new SequentialGuidGenerator();

        public IStringLocalizer<TResource> L;

        public ApplicationService(IStringLocalizer<TResource> l)
        {
            L = l;
        }
    }
}
