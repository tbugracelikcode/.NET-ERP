using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Department.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Departments.Page;

namespace TsiErp.Business.Entities.Department.Services
{
    [ServiceRegistration(typeof(IDepartmentsAppService), DependencyInjectionType.Scoped)]
    public class DepartmentsAppService : ApplicationService<DepartmentsResource>, IDepartmentsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public DepartmentsAppService(IStringLocalizer<DepartmentsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }


        [ValidationAspect(typeof(CreateDepartmentsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectDepartmentsDto>> CreateAsync(CreateDepartmentsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.Departments).Select("*").Where(new { Code = input.Code }, false, false, "");

            var list = queryFactory.ControlList<Departments>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.Departments).Insert(new CreateDepartmentsDto
            {
                Code = input.Code,
                Name = input.Name,
                SeniorityID = input.SeniorityID.GetValueOrDefault(),
                IsActive = true,
                Id = addedEntityId,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
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

            await FicheNumbersAppService.UpdateFicheNumberAsync("DepartmentsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Departments, LogType.Insert, addedEntityId);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectDepartmentsDto>(departments);
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("DepartmentID", new List<string>
            {
                Tables.Employees,
                Tables.EmployeeScoringLines
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var query = queryFactory.Query().From(Tables.Departments).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                var departments = queryFactory.Update<SelectDepartmentsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Departments, LogType.Delete, id);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectDepartmentsDto>(departments);
            }
        }


        public async Task<IDataResult<SelectDepartmentsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.Departments).Select<Departments>(null)
                  .Join<EmployeeSeniorities>
                        (
                            d => new { SeniorityName = d.Name, SeniorityID = d.Id },
                            nameof(Departments.SeniorityID),
                            nameof(EmployeeSeniorities.Id),
                            JoinType.Left
                        )
                .Where(new { Id = id }, true, true, Tables.Departments);
            var department = queryFactory.Get<SelectDepartmentsDto>(query);


            LogsAppService.InsertLogToDatabase(department, department, LoginedUserService.UserId, Tables.Departments, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectDepartmentsDto>(department);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListDepartmentsDto>>> GetListAsync(ListDepartmentsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.Departments).Select<Departments>(null)
                 .Join<EmployeeSeniorities>
                        (
                            d => new { SeniorityName = d.Name, SeniorityID = d.Id },
                            nameof(Departments.SeniorityID),
                            nameof(EmployeeSeniorities.Id),
                            JoinType.Left
                        )
                .Where(null, true, true, Tables.Departments);
            var departments = queryFactory.GetList<ListDepartmentsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListDepartmentsDto>>(departments);
        }


        [ValidationAspect(typeof(UpdateDepartmentsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectDepartmentsDto>> UpdateAsync(UpdateDepartmentsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.Departments).Select("*").Where(new { Id = input.Id }, true, true, "");
            var entity = queryFactory.Get<Departments>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.Departments).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.GetList<Departments>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.Departments).Update(new UpdateDepartmentsDto
            {
                Code = input.Code,
                Name = input.Name,
                SeniorityID = input.SeniorityID.GetValueOrDefault(),
                Id = input.Id,
                IsActive = input.IsActive,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id }, true, true, "");

            var departments = queryFactory.Update<SelectDepartmentsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, departments, LoginedUserService.UserId, Tables.Departments, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectDepartmentsDto>(departments);
        }

        public async Task<IDataResult<SelectDepartmentsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.Departments).Select("*").Where(new { Id = id }, true, true, "");

            var entity = queryFactory.Get<Departments>(entityQuery);

            var query = queryFactory.Query().From(Tables.Departments).Update(new UpdateDepartmentsDto
            {
                Code = entity.Code,
                Name = entity.Name,
                SeniorityID = entity.SeniorityID,
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

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, true, true, "");

            var departments = queryFactory.Update<SelectDepartmentsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectDepartmentsDto>(departments);
        }
    }
}
