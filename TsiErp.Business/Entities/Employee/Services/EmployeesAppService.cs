using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Branch.Services;
using TsiErp.Business.Entities.Branch.Validations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Employee;
using TsiErp.Business.Entities.Employee.Validations;
using TsiErp.Entities.Entities.Employee.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.Employee;

namespace TsiErp.Business.Entities.Employee.Services
{
    [ServiceRegistration(typeof(IEmployeesAppService), DependencyInjectionType.Scoped)]
    public class EmployeesAppService : ApplicationService, IEmployeesAppService
    {
        private readonly IEmployeesRepository _repository;

        public EmployeesAppService(IEmployeesRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateEmployeesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEmployeesDto>> CreateAsync(CreateEmployeesDto input)
        {
            var entity = ObjectMapper.Map<CreateEmployeesDto, Employees>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            return new SuccessDataResult<SelectEmployeesDto>(ObjectMapper.Map<Employees, SelectEmployeesDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectEmployeesDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.Departments);
            var mappedEntity = ObjectMapper.Map<Employees, SelectEmployeesDto>(entity);
            return new SuccessDataResult<SelectEmployeesDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListEmployeesDto>>> GetListAsync(ListEmployeesParameterDto input)
        {
            var list = await _repository.GetListAsync(t => t.IsActive == input.IsActive, t => t.Departments);

            var mappedEntity = ObjectMapper.Map<List<Employees>, List<ListEmployeesDto>>(list.ToList());

            return new SuccessDataResult<IList<ListEmployeesDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateEmployeesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEmployeesDto>> UpdateAsync(UpdateEmployeesDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            var mappedEntity = ObjectMapper.Map<UpdateEmployeesDto, Employees>(input);

            await _repository.UpdateAsync(mappedEntity);

            return new SuccessDataResult<SelectEmployeesDto>(ObjectMapper.Map<Employees, SelectEmployeesDto>(mappedEntity));
        }
    }
}
