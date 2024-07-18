using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.CustomerComplaintReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem.Dtos;

namespace TsiErp.Business.Entities.CustomerComplaintReport.Services
{
    public interface ICustomerComplaintReportsAppService : ICrudAppService<SelectCustomerComplaintReportsDto, ListCustomerComplaintReportsDto, CreateCustomerComplaintReportsDto, UpdateCustomerComplaintReportsDto, ListCustomerComplaintReportsParameterDto>
    {
        Task<IDataResult<SelectCustomerComplaintReportsDto>> GetWithUnsuitabilityItemDescriptionAsync(string description);
    }
}
