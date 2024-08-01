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
using TsiErp.Business.Entities.QualityControl.UnsuitabilityTypesItem.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.UnsuitabilityTypesItem.Page;

namespace TsiErp.Business.Entities.QualityControl.UnsuitabilityTypesItem.Services
{
    [ServiceRegistration(typeof(IUnsuitabilityTypesItemsAppService), DependencyInjectionType.Scoped)]
    public class UnsuitabilityTypesItemsAppService : ApplicationService<UnsuitabilityTypesItemResources>, IUnsuitabilityTypesItemsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }

        public UnsuitabilityTypesItemsAppService(IStringLocalizer<UnsuitabilityTypesItemResources> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }

        [ValidationAspect(typeof(CreateUnsuitabilityTypesItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnsuitabilityTypesItemsDto>> CreateAsync(CreateUnsuitabilityTypesItemsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Select("*").Where(new { Code = input.Code }, false, false, "");

            var list = queryFactory.ControlList<UnsuitabilityTypesItems>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion


            var query = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Insert(new CreateUnsuitabilityTypesItemsDto
            {
                Code = input.Code,
                Description_ = input.Description_,
                Name = input.Name,
                IsActive = true,
                Id = GuidGenerator.CreateGuid(),
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                IsDeleted = false,
                UnsuitabilityTypesDescription = input.UnsuitabilityTypesDescription
            });


            var unsuitabilityTypesItems = queryFactory.Insert<SelectUnsuitabilityTypesItemsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("UnsTypesItemsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.UnsuitabilityTypesItems, LogType.Insert, unsuitabilityTypesItems.Id);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectUnsuitabilityTypesItemsDto>(unsuitabilityTypesItems);

        }

        [ValidationAspect(typeof(UpdateUnsuitabilityTypesItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnsuitabilityTypesItemsDto>> UpdateAsync(UpdateUnsuitabilityTypesItemsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Select("*").Where(new { Id = input.Id }, true, true, "");
            var entity = queryFactory.Get<UnsuitabilityTypesItems>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.GetList<UnsuitabilityTypesItems>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Update(new UpdateUnsuitabilityTypesItemsDto
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
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId,
                UnsuitabilityTypesDescription = input.UnsuitabilityTypesDescription
            }).Where(new { Id = input.Id }, true, true, "");

            var unsuitabilityTypesItems = queryFactory.Update<SelectUnsuitabilityTypesItemsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, unsuitabilityTypesItems, LoginedUserService.UserId, Tables.UnsuitabilityTypesItems, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectUnsuitabilityTypesItemsDto>(unsuitabilityTypesItems);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("UnsuitabilityTypeID", new List<string>
            {
                Tables.UnsuitabilityItemSPCLines
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var query = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                var unsuitabilityTypesItems = queryFactory.Update<SelectUnsuitabilityTypesItemsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.UnsuitabilityTypesItems, LogType.Delete, id);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectUnsuitabilityTypesItemsDto>(unsuitabilityTypesItems);
            }
        }

        public async Task<IDataResult<SelectUnsuitabilityTypesItemsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Select("*").Where(
            new
            {
                Id = id
            }, true, true, "");
            var unsuitabilityTypesItems = queryFactory.Get<SelectUnsuitabilityTypesItemsDto>(query);


            LogsAppService.InsertLogToDatabase(unsuitabilityTypesItems, unsuitabilityTypesItems, LoginedUserService.UserId, Tables.UnsuitabilityTypesItems, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectUnsuitabilityTypesItemsDto>(unsuitabilityTypesItems);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListUnsuitabilityTypesItemsDto>>> GetListAsync(ListUnsuitabilityTypesItemsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Select("*").Where(null, true, true, "");
            var unsuitabilityTypesItems = queryFactory.GetList<ListUnsuitabilityTypesItemsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListUnsuitabilityTypesItemsDto>>(unsuitabilityTypesItems);

        }

        public async Task<IDataResult<SelectUnsuitabilityTypesItemsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Select("*").Where(new { Id = id }, true, true, "");

            var entity = queryFactory.Get<UnsuitabilityTypesItems>(entityQuery);

            var query = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Update(new UpdateUnsuitabilityTypesItemsDto
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
                UnsuitabilityTypesDescription = entity.UnsuitabilityTypesDescription
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, true, true, "");

            var unsuitabilityTypesItems = queryFactory.Update<SelectUnsuitabilityTypesItemsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectUnsuitabilityTypesItemsDto>(unsuitabilityTypesItems);

        }

        public async Task<IDataResult<SelectUnsuitabilityTypesItemsDto>> GetWithUnsuitabilityItemDescriptionAsync(string description)
        {
            var query = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Select("*").Where(
            new
            {
                UnsuitabilityTypesDescription = description
            }, true, true, "");
            var unsuitabilityTypesItems = queryFactory.Get<SelectUnsuitabilityTypesItemsDto>(query);


            LogsAppService.InsertLogToDatabase(unsuitabilityTypesItems, unsuitabilityTypesItems, LoginedUserService.UserId, Tables.UnsuitabilityTypesItems, LogType.Get, unsuitabilityTypesItems.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectUnsuitabilityTypesItemsDto>(unsuitabilityTypesItems);

        }
    }
}
