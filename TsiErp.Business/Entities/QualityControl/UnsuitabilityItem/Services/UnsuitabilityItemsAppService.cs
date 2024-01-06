using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.QualityControl.UnsuitabilityItem.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.UnsuitabilityItems.Page;

namespace TsiErp.Business.Entities.QualityControl.UnsuitabilityItem.Services
{
    [ServiceRegistration(typeof(IUnsuitabilityItemsAppService), DependencyInjectionType.Scoped)]
    public class UnsuitabilityItemsAppService : ApplicationService<UnsuitabilityItemsResource>, IUnsuitabilityItemsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }

        public UnsuitabilityItemsAppService(IStringLocalizer<UnsuitabilityItemsResource> l, IFicheNumbersAppService ficheNumbersAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
        }

        [ValidationAspect(typeof(CreateUnsuitabilityItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnsuitabilityItemsDto>> CreateAsync(CreateUnsuitabilityItemsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.UnsuitabilityItems).Select("*").Where(new { Code = input.Code }, false, false, "");

            var list = queryFactory.ControlList<UnsuitabilityItems>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.UnsuitabilityItems).Insert(new CreateUnsuitabilityItemsDto
            {
                Code = input.Code,
                CreationTime = DateTime.Now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Description_ = input.Description_,
                Id = GuidGenerator.CreateGuid(),
                IsActive = true,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                Name = input.Name,
                Detectability = input.Detectability,
                ExtraCost = input.ExtraCost,
                IntensityCoefficient = input.IntensityCoefficient,
                IntensityRange = input.IntensityRange,
                LifeThreatening = input.LifeThreatening,
                LossOfPrestige = input.LossOfPrestige,
                ProductLifeShortening = input.ProductLifeShortening,
                ToBeUsedAs = input.ToBeUsedAs,
                UnsuitabilityTypesItemsId = input.UnsuitabilityTypesItemsId,
                StationGroupId = input.StationGroupId
            });

            var unsuitabilityItem = queryFactory.Insert<SelectUnsuitabilityItemsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("UnsItemsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.UnsuitabilityItems, LogType.Insert, unsuitabilityItem.Id);

            return new SuccessDataResult<SelectUnsuitabilityItemsDto>(unsuitabilityItem);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("UnsuitabilityItemID", new List<string>
            {
                Tables.PFMEAs,
                Tables.UnsuitabilityItemSPCLines
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var query = queryFactory.Query().From(Tables.UnsuitabilityItems).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                var unsuitabilityItem = queryFactory.Update<SelectUnsuitabilityItemsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.UnsuitabilityItems, LogType.Delete, id);

                return new SuccessDataResult<SelectUnsuitabilityItemsDto>(unsuitabilityItem);
            }
        }

        public async Task<IDataResult<SelectUnsuitabilityItemsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.UnsuitabilityItems).Select<UnsuitabilityItems>(p => new { p.Id, p.Code, p.Name, p.IsActive, p.DataOpenStatus, p.DataOpenStatusUserId, p.Description_, p.LifeThreatening, p.LossOfPrestige, p.StationGroupId, p.ExtraCost, p.ProductLifeShortening, p.Detectability, p.ToBeUsedAs, p.IntensityRange, p.IntensityCoefficient })
                        .Join<UnsuitabilityTypesItems>
                        (
                            b => new { UnsuitabilityTypesItemsName = b.Name, UnsuitabilityTypesItemsId = b.Id },
                            nameof(UnsuitabilityItems.UnsuitabilityTypesItemsId),
                            nameof(UnsuitabilityTypesItems.Id),
                            JoinType.Left
                        )
                         .Join<StationGroups>
                        (
                            sg => new { StationGroupName = sg.Name, StationGroupId = sg.Id },
                            nameof(UnsuitabilityItems.StationGroupId),
                            nameof(StationGroups.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, true, true, Tables.UnsuitabilityItems);

            var unsuitabilityItem = queryFactory.Get<SelectUnsuitabilityItemsDto>(query);

            LogsAppService.InsertLogToDatabase(unsuitabilityItem, unsuitabilityItem, LoginedUserService.UserId, Tables.UnsuitabilityItems, LogType.Get, id);

            return new SuccessDataResult<SelectUnsuitabilityItemsDto>(unsuitabilityItem);


        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListUnsuitabilityItemsDto>>> GetListAsync(ListUnsuitabilityItemsParameterDto input)
        {
            var query = queryFactory
                    .Query().From(Tables.UnsuitabilityItems).Select<UnsuitabilityItems>(p => new { p.Id, p.Code, p.Name, p.IsActive, p.IntensityRange, p.IntensityCoefficient })
                        .Join<UnsuitabilityTypesItems>
                        (
                            b => new { UnsuitabilityTypesItemsName = b.Name },
                            nameof(UnsuitabilityItems.UnsuitabilityTypesItemsId),
                            nameof(UnsuitabilityTypesItems.Id),
                            JoinType.Left
                        )
                         .Join<StationGroups>
                        (
                            sg => new { StationGroupName = sg.Name },
                            nameof(UnsuitabilityItems.StationGroupId),
                            nameof(StationGroups.Id),
                            JoinType.Left
                        )
                        .Where(null, true, true, Tables.UnsuitabilityItems);

            var unsuitabilityItems = queryFactory.GetList<ListUnsuitabilityItemsDto>(query).ToList();


            return new SuccessDataResult<IList<ListUnsuitabilityItemsDto>>(unsuitabilityItems);

        }

        [ValidationAspect(typeof(UpdateUnsuitabilityItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnsuitabilityItemsDto>> UpdateAsync(UpdateUnsuitabilityItemsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.UnsuitabilityItems).Select("*").Where(new { Id = input.Id }, true, true, "");
            var entity = queryFactory.Get<UnsuitabilityItems>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.UnsuitabilityItems).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.GetList<UnsuitabilityItems>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.UnsuitabilityItems).Update(new UpdateUnsuitabilityItemsDto
            {
                Code = input.Code,
                Description_ = input.Description_,
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
                LastModifierId = LoginedUserService.UserId,
                Detectability = input.Detectability,
                ExtraCost = input.ExtraCost,
                IntensityCoefficient = input.IntensityCoefficient,
                IntensityRange = input.IntensityRange,
                LifeThreatening = input.LifeThreatening,
                LossOfPrestige = input.LossOfPrestige,
                ProductLifeShortening = input.ProductLifeShortening,
                ToBeUsedAs = input.ToBeUsedAs,
                UnsuitabilityTypesItemsId = input.UnsuitabilityTypesItemsId,
                StationGroupId = input.StationGroupId,
            }).Where(new { Id = input.Id }, true, true, "");

            var unsuitabilityItem = queryFactory.Update<SelectUnsuitabilityItemsDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, unsuitabilityItem, LoginedUserService.UserId, Tables.UnsuitabilityItems, LogType.Update, entity.Id);


            return new SuccessDataResult<SelectUnsuitabilityItemsDto>(unsuitabilityItem);

        }

        public async Task<IDataResult<SelectUnsuitabilityItemsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.UnsuitabilityItems).Select("*").Where(new { Id = id }, true, true, "");
            var entity = queryFactory.Get<UnsuitabilityItems>(entityQuery);

            var query = queryFactory.Query().From(Tables.UnsuitabilityItems).Update(new UpdateUnsuitabilityItemsDto
            {
                Code = entity.Code,
                Description_ = entity.Description_,
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
                DataOpenStatusUserId = userId,
                Detectability = entity.Detectability,
                ExtraCost = entity.ExtraCost,
                IntensityCoefficient = entity.IntensityCoefficient,
                IntensityRange = entity.IntensityRange,
                LifeThreatening = entity.LifeThreatening,
                LossOfPrestige = entity.LossOfPrestige,
                ProductLifeShortening = entity.ProductLifeShortening,
                ToBeUsedAs = entity.ToBeUsedAs,
                UnsuitabilityTypesItemsId = entity.UnsuitabilityTypesItemsId,
                StationGroupId = entity.StationGroupId,
            }).Where(new { Id = id }, true, true, "");

            var unsuitabilityItem = queryFactory.Update<SelectUnsuitabilityItemsDto>(query, "Id", true);

            return new SuccessDataResult<SelectUnsuitabilityItemsDto>(unsuitabilityItem);


        }
    }
}
