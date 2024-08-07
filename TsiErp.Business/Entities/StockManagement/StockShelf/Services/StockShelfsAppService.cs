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
using TsiErp.Business.Entities.StockManagement.StockShelf.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.StockManagement.StockShelf;
using TsiErp.Entities.Entities.StockManagement.StockShelf.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.StockShelfs.Page;

namespace TsiErp.Business.Entities.StockShelf.Services
{
    [ServiceRegistration(typeof(IStockShelfsAppService), DependencyInjectionType.Scoped)]
    public class StockShelfsAppService : ApplicationService<StockShelfsResource>, IStockShelfsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public StockShelfsAppService(IStringLocalizer<StockShelfsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }

        [ValidationAspect(typeof(CreateStockShelfsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStockShelfsDto>> CreateAsync(CreateStockShelfsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.StockShelfs).Select("*").Where(new { Code = input.Code },  "");

            var list = queryFactory.ControlList<StockShelfs>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.StockShelfs).Insert(new CreateStockShelfsDto
            {
                Code = input.Code,
                Name = input.Name,
                Description_ = input.Description_,
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


            var StockShelfs = queryFactory.Insert<SelectStockShelfsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("StockShelfsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.StockShelfs, LogType.Insert, addedEntityId);


            return new SuccessDataResult<SelectStockShelfsDto>(StockShelfs);


        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("StockShelfID", new List<string>
            {

                Tables.StockAddressLines
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {

                var query = queryFactory.Query().From(Tables.StockShelfs).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

                var StockShelfs = queryFactory.Update<SelectStockShelfsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.StockShelfs, LogType.Delete, id);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectStockShelfsDto>(StockShelfs);
            }

        }

        public async Task<IDataResult<SelectStockShelfsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.StockShelfs).Select("*").Where(
            new
            {
                Id = id
            }, "");
            var StockShelf = queryFactory.Get<SelectStockShelfsDto>(query);


            LogsAppService.InsertLogToDatabase(StockShelf, StockShelf, LoginedUserService.UserId, Tables.StockShelfs, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStockShelfsDto>(StockShelf);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListStockShelfsDto>>> GetListAsync(ListStockShelfsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.StockShelfs).Select("*").Where(null, "");
            var StockShelfs = queryFactory.GetList<ListStockShelfsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListStockShelfsDto>>(StockShelfs);

        }


        [ValidationAspect(typeof(UpdateStockShelfsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStockShelfsDto>> UpdateAsync(UpdateStockShelfsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.StockShelfs).Select("*").Where(new { Id = input.Id },  "");
            var entity = queryFactory.Get<StockShelfs>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.StockShelfs).Select("*").Where(new { Code = input.Code },  "");
            var list = queryFactory.GetList<StockShelfs>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.StockShelfs).Update(new UpdateStockShelfsDto
            {
                Code = input.Code,
                Name = input.Name,
                Id = input.Id,
                Description_ = input.Description_,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id },"");

            var StockShelfs = queryFactory.Update<SelectStockShelfsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, StockShelfs, LoginedUserService.UserId, Tables.StockShelfs, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStockShelfsDto>(StockShelfs);

        }

        public async Task<IDataResult<SelectStockShelfsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.StockShelfs).Select("*").Where(new { Id = id },  "");

            var entity = queryFactory.Get<StockShelfs>(entityQuery);

            var query = queryFactory.Query().From(Tables.StockShelfs).Update(new UpdateStockShelfsDto
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

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id },  "");

            var StockShelfs = queryFactory.Update<SelectStockShelfsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectStockShelfsDto>(StockShelfs);

        }
    }
}
