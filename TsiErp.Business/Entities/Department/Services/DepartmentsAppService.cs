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
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;

namespace TsiErp.Business.Entities.Department.Services
{
    [ServiceRegistration(typeof(IDepartmentsAppService), DependencyInjectionType.Scoped)]
    public class DepartmentsAppService : ApplicationService, IDepartmentsAppService
    {
        DepartmentManager _manager { get; set; } = new DepartmentManager();


        [ValidationAspect(typeof(CreateDepartmentsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectDepartmentsDto>> CreateAsync(CreateDepartmentsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.DepartmentsRepository, input.Code);

                var entity = ObjectMapper.Map<CreateDepartmentsDto, Departments>(input);

                var addedEntity = await _uow.DepartmentsRepository.InsertAsync(entity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectDepartmentsDto>(ObjectMapper.Map<Departments, SelectDepartmentsDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.DepartmentsRepository, id);
                await _uow.DepartmentsRepository.DeleteAsync(id);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }


        public async Task<IDataResult<SelectDepartmentsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.DepartmentsRepository.GetAsync(t => t.Id == id, t => t.Employees, t => t.EquipmentRecords);
                var mappedEntity = ObjectMapper.Map<Departments, SelectDepartmentsDto>(entity);
                return new SuccessDataResult<SelectDepartmentsDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListDepartmentsDto>>> GetListAsync(ListDepartmentsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.DepartmentsRepository.GetListAsync(t => t.IsActive == input.IsActive, t => t.Employees, t => t.EquipmentRecords);

                var mappedEntity = ObjectMapper.Map<List<Departments>, List<ListDepartmentsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListDepartmentsDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateDepartmentsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectDepartmentsDto>> UpdateAsync(UpdateDepartmentsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.DepartmentsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.DepartmentsRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateDepartmentsDto, Departments>(input);

                await _uow.DepartmentsRepository.UpdateAsync(mappedEntity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectDepartmentsDto>(ObjectMapper.Map<Departments, SelectDepartmentsDto>(mappedEntity));
            }
        }
    }
}
