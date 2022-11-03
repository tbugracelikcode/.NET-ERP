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
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Department;
using TsiErp.Business.Entities.Department.Validations;
using TsiErp.Entities.Entities.Department.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.Department;
using TsiErp.Business.Entities.Department.BusinessRules;

namespace TsiErp.Business.Entities.Department.Services
{
    [ServiceRegistration(typeof(IDepartmentsAppService), DependencyInjectionType.Scoped)]
    public class DepartmentsAppService : ApplicationService, IDepartmentsAppService
    {
        private readonly IDepartmentsRepository _repository;

        DepartmentManager _manager { get; set; } = new DepartmentManager();

        public DepartmentsAppService(IDepartmentsRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateDepartmentsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectDepartmentsDto>> CreateAsync(CreateDepartmentsDto input)
        {
            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateDepartmentsDto, Departments>(input);

            var addedEntity = await _repository.InsertAsync(entity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectDepartmentsDto>(ObjectMapper.Map<Departments, SelectDepartmentsDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _manager.DeleteControl(_repository, id);
            await _repository.DeleteAsync(id);
            await _repository.SaveChanges();
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectDepartmentsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.Employees,t=>t.EquipmentRecords);
            var mappedEntity = ObjectMapper.Map<Departments, SelectDepartmentsDto>(entity);
            return new SuccessDataResult<SelectDepartmentsDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListDepartmentsDto>>> GetListAsync(ListDepartmentsParameterDto input)
        {
            var list = await _repository.GetListAsync(t=>t.IsActive==input.IsActive, t => t.Employees, t => t.EquipmentRecords);

            var mappedEntity = ObjectMapper.Map<List<Departments>, List<ListDepartmentsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListDepartmentsDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateDepartmentsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectDepartmentsDto>> UpdateAsync(UpdateDepartmentsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateDepartmentsDto, Departments>(input);

            await _repository.UpdateAsync(mappedEntity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectDepartmentsDto>(ObjectMapper.Map<Departments, SelectDepartmentsDto>(mappedEntity));
        }
    }
}
