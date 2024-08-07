using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.EntityContracts.StationGroup;
using TsiErp.Localizations.Resources.StationGroups.Page;

namespace TsiErp.Business.Entities.StationGroup.Services
{
    [ServiceRegistration(typeof(IStationGroupsAppService), DependencyInjectionType.Scoped)]
    public class StationGroupsAppService : ApplicationService<StationGroupsResource>, IStationGroupsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public StationGroupsAppService(IStringLocalizer<StationGroupsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }


        [ValidationAspect(typeof(CreateStationGroupsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStationGroupsDto>> CreateAsync(CreateStationGroupsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.StationGroups).Select("*").Where(new { Code = input.Code }, "");

            var list = queryFactory.ControlList<StationGroups>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.StationGroups).Insert(new CreateStationGroupsDto
            {
                Code = input.Code,
                TotalEmployees = input.TotalEmployees,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                Name = input.Name
            });

            var stationGroups = queryFactory.Insert<SelectStationGroupsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("StationGroupChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.StationGroups, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStationGroupsDto>(stationGroups);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("GroupID", new List<string>
            {
                Tables.Stations
            });

            DeleteControl.ControlList.Add("StationGroupID", new List<string>
            {
                Tables.OperationUnsuitabilityReports,
                Tables.WorkOrders
            });

            DeleteControl.ControlList.Add("StationGroupId", new List<string>
            {
                Tables.UnsuitabilityItems
            });

            DeleteControl.ControlList.Add("WorkCenterID", new List<string>
            {
                Tables.ContractQualityPlanLines,
                Tables.OperationalQualityPlanLines,
                Tables.OperationalSPCLines,
                Tables.PFMEAs,
                Tables.ProductsOperations,
                Tables.PurchaseQualityPlanLines,
                Tables.TemplateOperations,
                Tables.UnsuitabilityItemSPCLines
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var query = queryFactory.Query().From(Tables.StationGroups).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var stationGroups = queryFactory.Update<SelectStationGroupsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.StationGroups, LogType.Delete, id);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectStationGroupsDto>(stationGroups);
            }
        }

        public async Task<IDataResult<SelectStationGroupsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.StationGroups).Select("*").Where(
            new
            {
                Id = id
            }, "");
            var stationGroup = queryFactory.Get<SelectStationGroupsDto>(query);


            LogsAppService.InsertLogToDatabase(stationGroup, stationGroup, LoginedUserService.UserId, Tables.StationGroups, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStationGroupsDto>(stationGroup);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListStationGroupsDto>>> GetListAsync(ListStationGroupsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.StationGroups).Select("*").Where(null, "");
            var stationGroups = queryFactory.GetList<ListStationGroupsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListStationGroupsDto>>(stationGroups);
        }


        [ValidationAspect(typeof(UpdateStationGroupsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStationGroupsDto>> UpdateAsync(UpdateStationGroupsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.StationGroups).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<StationGroups>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.StationGroups).Select("*").Where(new { Code = input.Code }, "");
            var list = queryFactory.GetList<StationGroups>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.StationGroups).Update(new UpdateStationGroupsDto
            {
                Code = input.Code,
                TotalEmployees = input.TotalEmployees,
                Name = input.Name,
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
            }).Where(new { Id = input.Id }, "");

            var stationGroups = queryFactory.Update<SelectStationGroupsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, stationGroups, LoginedUserService.UserId, Tables.StationGroups, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStationGroupsDto>(stationGroups);
        }

        public async Task<IDataResult<SelectStationGroupsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.StationGroups).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<StationGroups>(entityQuery);

            var query = queryFactory.Query().From(Tables.StationGroups).Update(new UpdateStationGroupsDto
            {
                Code = entity.Code,
                TotalEmployees = entity.TotalEmployees,
                Name = entity.Name,
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

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var stationGroups = queryFactory.Update<SelectStationGroupsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectStationGroupsDto>(stationGroups);
        }
    }
}
