using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.MachineAndWorkforceManagement.EducationLevelScore.Services;
using TsiErp.Business.Entities.MachineAndWorkforceManagement.EducationLevelScore.Validations;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EducationLevelScore;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EducationLevelScore.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.EducationLevelScores.Page;

namespace TsiErp.Business.Entities.EducationLevelScore.Services
{
    [ServiceRegistration(typeof(IEducationLevelScoresAppService), DependencyInjectionType.Scoped)]
    public class EducationLevelScoresAppService : ApplicationService<EducationLevelScoresResource>, IEducationLevelScoresAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public EducationLevelScoresAppService(IStringLocalizer<EducationLevelScoresResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }


        [ValidationAspect(typeof(CreateEducationLevelScoresValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEducationLevelScoresDto>> CreateAsync(CreateEducationLevelScoresDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.EducationLevelScores).Select("*").Where(new { Code = input.Code }, false, false, "");

            var list = queryFactory.ControlList<EducationLevelScores>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.EducationLevelScores).Insert(new CreateEducationLevelScoresDto
            {
                Code = input.Code,
                Description_ = input.Description_,
                Score = input.Score,
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


            var EducationLevelScores = queryFactory.Insert<SelectEducationLevelScoresDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("EducationLevelScoresChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.EducationLevelScores, LogType.Insert, addedEntityId);

            await Task.CompletedTask;

            return new SuccessDataResult<SelectEducationLevelScoresDto>(EducationLevelScores);
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("EducationLevelID", new List<string>
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
                var query = queryFactory.Query().From(Tables.EducationLevelScores).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var EducationLevelScores = queryFactory.Update<SelectEducationLevelScoresDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.EducationLevelScores, LogType.Delete, id);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectEducationLevelScoresDto>(EducationLevelScores);
            }
        }


        public async Task<IDataResult<SelectEducationLevelScoresDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.EducationLevelScores).Select("*").Where(
            new
            {
                Id = id
            }, false, false, "");
            var EducationLevelScore = queryFactory.Get<SelectEducationLevelScoresDto>(query);


            LogsAppService.InsertLogToDatabase(EducationLevelScore, EducationLevelScore, LoginedUserService.UserId, Tables.EducationLevelScores, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectEducationLevelScoresDto>(EducationLevelScore);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListEducationLevelScoresDto>>> GetListAsync(ListEducationLevelScoresParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.EducationLevelScores).Select("*").Where(null, false, false, "");
            var EducationLevelScores = queryFactory.GetList<ListEducationLevelScoresDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListEducationLevelScoresDto>>(EducationLevelScores);
        }


        [ValidationAspect(typeof(UpdateEducationLevelScoresValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEducationLevelScoresDto>> UpdateAsync(UpdateEducationLevelScoresDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.EducationLevelScores).Select("*").Where(new { Id = input.Id }, false, false, "");
            var entity = queryFactory.Get<EducationLevelScores>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.EducationLevelScores).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.GetList<EducationLevelScores>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.EducationLevelScores).Update(new UpdateEducationLevelScoresDto
            {
                Code = input.Code,
                Name = input.Name,
                Score = input.Score,
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

            var EducationLevelScores = queryFactory.Update<SelectEducationLevelScoresDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, EducationLevelScores, LoginedUserService.UserId, Tables.EducationLevelScores, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectEducationLevelScoresDto>(EducationLevelScores);
        }

        public async Task<IDataResult<SelectEducationLevelScoresDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.EducationLevelScores).Select("*").Where(new { Id = id }, false, false, "");

            var entity = queryFactory.Get<EducationLevelScores>(entityQuery);

            var query = queryFactory.Query().From(Tables.EducationLevelScores).Update(new UpdateEducationLevelScoresDto
            {
                Code = entity.Code,
                Name = entity.Name,
                Score = entity.Score,
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

            var EducationLevelScores = queryFactory.Update<SelectEducationLevelScoresDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectEducationLevelScoresDto>(EducationLevelScores);
        }
    }
}
