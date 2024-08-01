using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Department.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.EmployeeSeniorities.Page;

namespace TsiErp.Business.Entities.EmployeeSeniority.Services
{
    [ServiceRegistration(typeof(IEmployeeSenioritiesAppService), DependencyInjectionType.Scoped)]
    public class EmployeeSenioritiesAppService : ApplicationService<EmployeeSenioritiesResource>, IEmployeeSenioritiesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public EmployeeSenioritiesAppService(IStringLocalizer<EmployeeSenioritiesResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }


        [ValidationAspect(typeof(CreateEmployeeSenioritiesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEmployeeSenioritiesDto>> CreateAsync(CreateEmployeeSenioritiesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.EmployeeSeniorities).Select("*").Where(new { Code = input.Code }, false, false, "");

            var list = queryFactory.ControlList<EmployeeSeniorities>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.EmployeeSeniorities).Insert(new CreateEmployeeSenioritiesDto
            {
                Code = input.Code,
                Description_ = input.Description_,
                Name = input.Name,
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


            var EmployeeSeniorities = queryFactory.Insert<SelectEmployeeSenioritiesDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("EmployeeSenioritiesChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.EmployeeSeniorities, LogType.Insert, addedEntityId);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectEmployeeSenioritiesDto>(EmployeeSeniorities);
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();


            DeleteControl.ControlList.Add("SeniorityID", new List<string>
            {
                Tables.Departments,
                Tables.Employees,
                Tables.EmployeeAnnualSeniorityDifferences,
                Tables.EmployeeScoringLines,
                Tables.StartingSalaryLines,
                Tables.TaskScorings
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var query = queryFactory.Query().From(Tables.EmployeeSeniorities).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var EmployeeSeniorities = queryFactory.Update<SelectEmployeeSenioritiesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.EmployeeSeniorities, LogType.Delete, id);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectEmployeeSenioritiesDto>(EmployeeSeniorities);
            }
        }


        public async Task<IDataResult<SelectEmployeeSenioritiesDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.EmployeeSeniorities).Select("*").Where(
            new
            {
                Id = id
            }, false, false, "");
            var EmployeeSeniority = queryFactory.Get<SelectEmployeeSenioritiesDto>(query);


            LogsAppService.InsertLogToDatabase(EmployeeSeniority, EmployeeSeniority, LoginedUserService.UserId, Tables.EmployeeSeniorities, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectEmployeeSenioritiesDto>(EmployeeSeniority);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListEmployeeSenioritiesDto>>> GetListAsync(ListEmployeeSenioritiesParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.EmployeeSeniorities).Select("*").Where(null, false, false, "");
            var EmployeeSeniorities = queryFactory.GetList<ListEmployeeSenioritiesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListEmployeeSenioritiesDto>>(EmployeeSeniorities);
        }


        [ValidationAspect(typeof(UpdateEmployeeSenioritiesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEmployeeSenioritiesDto>> UpdateAsync(UpdateEmployeeSenioritiesDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.EmployeeSeniorities).Select("*").Where(new { Id = input.Id }, false, false, "");
            var entity = queryFactory.Get<EmployeeSeniorities>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.EmployeeSeniorities).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.GetList<EmployeeSeniorities>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.EmployeeSeniorities).Update(new UpdateEmployeeSenioritiesDto
            {
                Code = input.Code,
                Name = input.Name,
                Description_ = input.Description_,
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id }, false, false, "");

            var EmployeeSeniorities = queryFactory.Update<SelectEmployeeSenioritiesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, EmployeeSeniorities, LoginedUserService.UserId, Tables.EmployeeSeniorities, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectEmployeeSenioritiesDto>(EmployeeSeniorities);
        }

        public async Task<IDataResult<SelectEmployeeSenioritiesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.EmployeeSeniorities).Select("*").Where(new { Id = id }, false, false, "");

            var entity = queryFactory.Get<EmployeeSeniorities>(entityQuery);

            var query = queryFactory.Query().From(Tables.EmployeeSeniorities).Update(new UpdateEmployeeSenioritiesDto
            {
                Code = entity.Code,
                Name = entity.Name,
                Description_ = entity.Description_,
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

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, false, false, "");

            var EmployeeSeniorities = queryFactory.Update<SelectEmployeeSenioritiesDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectEmployeeSenioritiesDto>(EmployeeSeniorities);
        }
    }
}
