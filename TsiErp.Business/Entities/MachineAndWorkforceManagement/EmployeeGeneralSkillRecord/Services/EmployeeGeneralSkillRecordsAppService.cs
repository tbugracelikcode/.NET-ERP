using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Department.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.MachineAndWorkforceManagement.EmployeeGeneralSkillRecord.Services;
using TsiErp.Business.Entities.MachineAndWorkforceManagement.EmployeeGeneralSkillRecord.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeGeneralSkillRecord;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeGeneralSkillRecord.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.EmployeeGeneralSkillRecords.Page;

namespace TsiErp.Business.Entities.EmployeeGeneralSkillRecord.Services
{
    [ServiceRegistration(typeof(IEmployeeGeneralSkillRecordsAppService), DependencyInjectionType.Scoped)]
    public class EmployeeGeneralSkillRecordsAppService : ApplicationService<EmployeeGeneralSkillRecordsResource>, IEmployeeGeneralSkillRecordsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }

        public EmployeeGeneralSkillRecordsAppService(IStringLocalizer<EmployeeGeneralSkillRecordsResource> l, IFicheNumbersAppService ficheNumbersAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
        }


        [ValidationAspect(typeof(CreateEmployeeGeneralSkillRecordsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEmployeeGeneralSkillRecordsDto>> CreateAsync(CreateEmployeeGeneralSkillRecordsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.EmployeeGeneralSkillRecords).Select("*").Where(new { Code = input.Code }, false, false, "");

            var list = queryFactory.ControlList<EmployeeGeneralSkillRecords>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.EmployeeGeneralSkillRecords).Insert(new CreateEmployeeGeneralSkillRecordsDto
            {
                Code = input.Code,
                Description_ = input.Description_,
                Name = input.Name,
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


            var EmployeeGeneralSkillRecords = queryFactory.Insert<SelectEmployeeGeneralSkillRecordsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("EmployeeGeneralSkillRecordsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.EmployeeGeneralSkillRecords, LogType.Insert, addedEntityId);


            return new SuccessDataResult<SelectEmployeeGeneralSkillRecordsDto>(EmployeeGeneralSkillRecords);
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("GeneralSkillID", new List<string>
            {
                Tables.GeneralSkillRecordPriorities
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var query = queryFactory.Query().From(Tables.EmployeeGeneralSkillRecords).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var EmployeeGeneralSkillRecords = queryFactory.Update<SelectEmployeeGeneralSkillRecordsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.EmployeeGeneralSkillRecords, LogType.Delete, id);

                return new SuccessDataResult<SelectEmployeeGeneralSkillRecordsDto>(EmployeeGeneralSkillRecords);
            }
        }


        public async Task<IDataResult<SelectEmployeeGeneralSkillRecordsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.EmployeeGeneralSkillRecords).Select("*").Where(
            new
            {
                Id = id
            }, false, false, "");
            var EmployeeGeneralSkillRecord = queryFactory.Get<SelectEmployeeGeneralSkillRecordsDto>(query);


            LogsAppService.InsertLogToDatabase(EmployeeGeneralSkillRecord, EmployeeGeneralSkillRecord, LoginedUserService.UserId, Tables.EmployeeGeneralSkillRecords, LogType.Get, id);

            return new SuccessDataResult<SelectEmployeeGeneralSkillRecordsDto>(EmployeeGeneralSkillRecord);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListEmployeeGeneralSkillRecordsDto>>> GetListAsync(ListEmployeeGeneralSkillRecordsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.EmployeeGeneralSkillRecords).Select("*").Where(null, false, false, "");
            var EmployeeGeneralSkillRecords = queryFactory.GetList<ListEmployeeGeneralSkillRecordsDto>(query).ToList();
            return new SuccessDataResult<IList<ListEmployeeGeneralSkillRecordsDto>>(EmployeeGeneralSkillRecords);
        }


        [ValidationAspect(typeof(UpdateEmployeeGeneralSkillRecordsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEmployeeGeneralSkillRecordsDto>> UpdateAsync(UpdateEmployeeGeneralSkillRecordsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.EmployeeGeneralSkillRecords).Select("*").Where(new { Id = input.Id }, false, false, "");
            var entity = queryFactory.Get<EmployeeGeneralSkillRecords>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.EmployeeGeneralSkillRecords).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.GetList<EmployeeGeneralSkillRecords>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.EmployeeGeneralSkillRecords).Update(new UpdateEmployeeGeneralSkillRecordsDto
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
                LastModificationTime = DateTime.Now,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id }, false, false, "");

            var EmployeeGeneralSkillRecords = queryFactory.Update<SelectEmployeeGeneralSkillRecordsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, EmployeeGeneralSkillRecords, LoginedUserService.UserId, Tables.EmployeeGeneralSkillRecords, LogType.Update, entity.Id);

            return new SuccessDataResult<SelectEmployeeGeneralSkillRecordsDto>(EmployeeGeneralSkillRecords);
        }

        public async Task<IDataResult<SelectEmployeeGeneralSkillRecordsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.EmployeeGeneralSkillRecords).Select("*").Where(new { Id = id }, false, false, "");

            var entity = queryFactory.Get<EmployeeGeneralSkillRecords>(entityQuery);

            var query = queryFactory.Query().From(Tables.EmployeeGeneralSkillRecords).Update(new UpdateEmployeeGeneralSkillRecordsDto
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

            }).Where(new { Id = id }, false, false, "");

            var EmployeeGeneralSkillRecords = queryFactory.Update<SelectEmployeeGeneralSkillRecordsDto>(query, "Id", true);
            return new SuccessDataResult<SelectEmployeeGeneralSkillRecordsDto>(EmployeeGeneralSkillRecords);
        }
    }
}
