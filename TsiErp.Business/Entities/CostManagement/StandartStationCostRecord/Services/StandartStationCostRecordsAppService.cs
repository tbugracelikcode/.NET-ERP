using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.CostManagement.StandartStationCostRecord.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.CostManagement.StandartStationCostRecord;
using TsiErp.Entities.Entities.CostManagement.StandartStationCostRecord.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.StandartStationCostRecords.Page;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TSI.QueryBuilder.Models;

namespace TsiErp.Business.Entities.CostManagement.StandartStationCostRecord.Services
{
    [ServiceRegistration(typeof(IStandartStationCostRecordsAppService), DependencyInjectionType.Scoped)]
    public class StandartStationCostRecordsAppService : ApplicationService<StandartStationCostRecordsResource>, IStandartStationCostRecordsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public StandartStationCostRecordsAppService(IStringLocalizer<StandartStationCostRecordsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }

        [ValidationAspect(typeof(CreateStandartStationCostRecordsValidator), Priority = 1)]
        public async Task<IDataResult<SelectStandartStationCostRecordsDto>> CreateAsync(CreateStandartStationCostRecordsDto input)
        {

            var listQuery = queryFactory.Query().From(Tables.StandartStationCostRecords).Select("Code").Where(new { Code = input.Code }, "");

            var list = queryFactory.ControlList<StandartStationCostRecords>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.StandartStationCostRecords).Insert(new CreateStandartStationCostRecordsDto
            {
                Code = input.Code,
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                StationCost = input.StationCost,
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                StationID = input.StationID.GetValueOrDefault(),
                StartDate = now.Date,
                EndDate = input.EndDate,
                isStandart = input.isStandart,


            });

            var StandartStationCostRecords = queryFactory.Insert<SelectStandartStationCostRecordsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("StandartStationCostRecordsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.StandartStationCostRecords, LogType.Insert, addedEntityId);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectStandartStationCostRecordsDto>(StandartStationCostRecords);
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.StandartStationCostRecords).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var standartStationCostRecords = queryFactory.Update<SelectStandartStationCostRecordsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Users, LogType.Delete, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStandartStationCostRecordsDto>(standartStationCostRecords);
        }

        public async Task<IDataResult<SelectStandartStationCostRecordsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.StandartStationCostRecords).Select<StandartStationCostRecords>(null)
                        .Join<Stations>
                        (
                            p => new { StationID = p.Id, StationCode = p.Code, StationName = p.Name },
                            nameof(StandartStationCostRecords.StationID),
                            nameof(Stations.Id),
                            JoinType.Left
                        )
                        .Join<Currencies>
                        (
                            p => new { CurrencyID = p.Id, CurrencyCode = p.Code},
                            nameof(StandartStationCostRecords.CurrencyID),
                            nameof(Currencies.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, Tables.StandartStationCostRecords);

            var standartStationCostRecords = queryFactory.Get<SelectStandartStationCostRecordsDto>(query);

            LogsAppService.InsertLogToDatabase(standartStationCostRecords, standartStationCostRecords, LoginedUserService.UserId, Tables.StandartStationCostRecords, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStandartStationCostRecordsDto>(standartStationCostRecords);
        }

        public async Task<IDataResult<IList<ListStandartStationCostRecordsDto>>> GetListAsync(ListStandartStationCostRecordsParameterDto input)
        {
            var query = queryFactory
               .Query()
               .From(Tables.StandartStationCostRecords).Select<StandartStationCostRecords>(a=> new {a.Code, a.StationCost, a.EndDate, a.StartDate, a.Id, a.isStandart})
                        .Join<Stations>
                        (
                            p => new { StationID = p.Id, StationCode = p.Code, StationName = p.Name },
                            nameof(StandartStationCostRecords.StationID),
                            nameof(Stations.Id),
                            JoinType.Left
                        )
                        .Join<Currencies>
                        (
                            p => new { CurrencyID = p.Id, CurrencyCode = p.Code},
                            nameof(StandartStationCostRecords.CurrencyID),
                            nameof(Currencies.Id),
                            JoinType.Left
                        ).Where(null, Tables.StandartStationCostRecords);

            var standartStationCostRecords = queryFactory.GetList<ListStandartStationCostRecordsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListStandartStationCostRecordsDto>>(standartStationCostRecords);
        }

        [ValidationAspect(typeof(UpdateStandartStationCostRecordsValidator), Priority = 1)]
        public async Task<IDataResult<SelectStandartStationCostRecordsDto>> UpdateAsync(UpdateStandartStationCostRecordsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.StandartStationCostRecords).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<StandartStationCostRecords>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.StandartStationCostRecords).Select("*").Where(new { Code = input.Code }, "");
            var list = queryFactory.GetList<StandartStationCostRecords>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.StandartStationCostRecords).Update(new UpdateStandartStationCostRecordsDto
            {
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
                Code = input.Code,
                StartDate = input.StartDate,
                EndDate = input.EndDate,
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                StationID = input.StationID.GetValueOrDefault(),
                StationCost = input.StationCost,
                isStandart = input.isStandart,
            }).Where(new { Id = input.Id }, "");

            var standartStationCostRecords = queryFactory.Update<SelectStandartStationCostRecordsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, standartStationCostRecords, LoginedUserService.UserId, Tables.StandartStationCostRecords, LogType.Update, entity.Id);
  
            await Task.CompletedTask;
            return new SuccessDataResult<SelectStandartStationCostRecordsDto>(standartStationCostRecords);

        }

        public async Task<IDataResult<SelectStandartStationCostRecordsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.StandartStationCostRecords).Select("*").Where(new { Id = id }, "");
            var entity = queryFactory.Get<StandartStationCostRecords>(entityQuery);

            var query = queryFactory.Query().From(Tables.StandartStationCostRecords).Update(new UpdateStandartStationCostRecordsDto
            {
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Id = id,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                 Code = entity.Code,
                  StationID = entity.StationID.GetValueOrDefault(),
                   CurrencyID = entity.CurrencyID.GetValueOrDefault(),
                    StationCost = entity.StationCost,
                    EndDate = entity.EndDate,
                    StartDate = entity.StartDate,
                isStandart = entity.isStandart,
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var standartStationCostRecords = queryFactory.Update<SelectStandartStationCostRecordsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStandartStationCostRecordsDto>(standartStationCostRecords);


        }
    }
}
