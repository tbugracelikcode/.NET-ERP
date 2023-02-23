using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Employee.BusinessRules;
using TsiErp.Business.Entities.Employee.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Employee;
using TsiErp.Entities.Entities.Employee.Dtos;

namespace TsiErp.Business.Entities.Employee.Services
{
    [ServiceRegistration(typeof(IEmployeesAppService), DependencyInjectionType.Scoped)]
    public class EmployeesAppService : ApplicationService, IEmployeesAppService
    {
        EmployeeManager _manager { get; set; } = new EmployeeManager();

        [ValidationAspect(typeof(CreateEmployeesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEmployeesDto>> CreateAsync(CreateEmployeesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.EmployeesRepository, input.Code);

                var entity = ObjectMapper.Map<CreateEmployeesDto, Employees>(input);

                var addedEntity = await _uow.EmployeesRepository.InsertAsync(entity);
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "Employees", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectEmployeesDto>(ObjectMapper.Map<Employees, SelectEmployeesDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _uow.EmployeesRepository.DeleteAsync(id);
                var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "Employees", LogType.Delete, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }


        public async Task<IDataResult<SelectEmployeesDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.EmployeesRepository.GetAsync(t => t.Id == id, t => t.Departments);
                var mappedEntity = ObjectMapper.Map<Employees, SelectEmployeesDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "Employees", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessDataResult<SelectEmployeesDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListEmployeesDto>>> GetListAsync(ListEmployeesParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.EmployeesRepository.GetListAsync(t => t.IsActive == input.IsActive, t => t.Departments);

                var mappedEntity = ObjectMapper.Map<List<Employees>, List<ListEmployeesDto>>(list.ToList());

                return new SuccessDataResult<IList<ListEmployeesDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateEmployeesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEmployeesDto>> UpdateAsync(UpdateEmployeesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.EmployeesRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.EmployeesRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateEmployeesDto, Employees>(input);

                await _uow.EmployeesRepository.UpdateAsync(mappedEntity);
                var before = ObjectMapper.Map<Employees, UpdateEmployeesDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "Employees", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectEmployeesDto>(ObjectMapper.Map<Employees, SelectEmployeesDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectEmployeesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.EmployeesRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.EmployeesRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<Employees, SelectEmployeesDto>(updatedEntity);

                return new SuccessDataResult<SelectEmployeesDto>(mappedEntity);
            }
        }
    }
}
