using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Department.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Departments.Page;

namespace TsiErp.Business.Entities.Department.Services
{
    [ServiceRegistration(typeof(IDepartmentsAppService), DependencyInjectionType.Scoped)]
    public class DepartmentsAppService : ApplicationService<DepartmentsResource>, IDepartmentsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public DepartmentsAppService(IStringLocalizer<DepartmentsResource> l) : base(l)
        {
        }


        [ValidationAspect(typeof(CreateDepartmentsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectDepartmentsDto>> CreateAsync(CreateDepartmentsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.Departments).Select("*").Where(new { Code = input.Code }, false, false, "");

                var list = queryFactory.ControlList<Departments>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();

                var query = queryFactory.Query().From(Tables.Departments).Insert(new CreateDepartmentsDto
                {
                    Code = input.Code,
                    Name = input.Name,
                    IsActive = true,
                    Id = addedEntityId,
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    IsDeleted = false
                });


                var departments = queryFactory.Insert<SelectDepartmentsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Departments, LogType.Insert, addedEntityId);


                return new SuccessDataResult<SelectDepartmentsDto>(departments);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                #region Delete Control


                #endregion

                var query = queryFactory.Query().From(Tables.Departments).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                var departments = queryFactory.Update<SelectDepartmentsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Departments, LogType.Delete, id);

                return new SuccessDataResult<SelectDepartmentsDto>(departments);
            }
        }


        public async Task<IDataResult<SelectDepartmentsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.Departments).Select("*").Where(
                new
                {
                    Id = id
                }, true, true, "");
                var department = queryFactory.Get<SelectDepartmentsDto>(query);


                LogsAppService.InsertLogToDatabase(department, department, LoginedUserService.UserId, Tables.Departments, LogType.Get, id);

                return new SuccessDataResult<SelectDepartmentsDto>(department);

            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListDepartmentsDto>>> GetListAsync(ListDepartmentsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.Departments).Select("*").Where(null, true, true, "");
                var departments = queryFactory.GetList<ListDepartmentsDto>(query).ToList();
                return new SuccessDataResult<IList<ListDepartmentsDto>>(departments);
            }
        }


        [ValidationAspect(typeof(UpdateDepartmentsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectDepartmentsDto>> UpdateAsync(UpdateDepartmentsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.Departments).Select("*").Where(new { Id = input.Id }, true, true, "");
                var entity = queryFactory.Get<Departments>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.Departments).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.GetList<Departments>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.Departments).Update(new UpdateDepartmentsDto
                {
                    Code = input.Code,
                    Name = input.Name,
                    Id = input.Id,
                    IsActive = input.IsActive,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = input.Id }, true, true, "");

                var departments = queryFactory.Update<SelectDepartmentsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, departments, LoginedUserService.UserId, Tables.Departments, LogType.Update, entity.Id);

                return new SuccessDataResult<SelectDepartmentsDto>(departments);
            }
        }

        public async Task<IDataResult<SelectDepartmentsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.Departments).Select("*").Where(new { Id = id }, true, true, "");

                var entity = queryFactory.Get<Departments>(entityQuery);

                var query = queryFactory.Query().From(Tables.Departments).Update(new UpdateDepartmentsDto
                {
                    Code = entity.Code,
                    Name = entity.Name,
                    IsActive = entity.IsActive,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                    LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                    Id = id,
                    DataOpenStatus = lockRow,
                    DataOpenStatusUserId = userId

                }).Where(new { Id = id }, true, true, "");

                var departments = queryFactory.Update<SelectDepartmentsDto>(query, "Id", true);
                return new SuccessDataResult<SelectDepartmentsDto>(departments);

            }

        }
    }
}
