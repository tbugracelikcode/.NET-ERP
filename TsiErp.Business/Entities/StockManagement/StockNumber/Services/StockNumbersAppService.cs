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
using TsiErp.Business.Entities.StockManagement.StockNumber.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.StockManagement.StockNumber;
using TsiErp.Entities.Entities.StockManagement.StockNumber.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.StockNumbers.Page;

namespace TsiErp.Business.Entities.StockNumber.Services
{
    [ServiceRegistration(typeof(IStockNumbersAppService), DependencyInjectionType.Scoped)]
    public class StockNumbersAppService : ApplicationService<StockNumbersResource>, IStockNumbersAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public StockNumbersAppService(IStringLocalizer<StockNumbersResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }

        [ValidationAspect(typeof(CreateStockNumbersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStockNumbersDto>> CreateAsync(CreateStockNumbersDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.StockNumbers).Select("*").Where(new { Code = input.Code }, false, false, "");

            var list = queryFactory.ControlList<StockNumbers>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.StockNumbers).Insert(new CreateStockNumbersDto
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


            var StockNumbers = queryFactory.Insert<SelectStockNumbersDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("StockNumbersChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.StockNumbers, LogType.Insert, addedEntityId);


            return new SuccessDataResult<SelectStockNumbersDto>(StockNumbers);


        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("StockNumberID", new List<string>
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
                var query = queryFactory.Query().From(Tables.StockNumbers).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var StockNumbers = queryFactory.Update<SelectStockNumbersDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.StockNumbers, LogType.Delete, id);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectStockNumbersDto>(StockNumbers);
            }

        }

        public async Task<IDataResult<SelectStockNumbersDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.StockNumbers).Select("*").Where(
            new
            {
                Id = id
            }, false, false, "");
            var StockNumber = queryFactory.Get<SelectStockNumbersDto>(query);


            LogsAppService.InsertLogToDatabase(StockNumber, StockNumber, LoginedUserService.UserId, Tables.StockNumbers, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStockNumbersDto>(StockNumber);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListStockNumbersDto>>> GetListAsync(ListStockNumbersParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.StockNumbers).Select("*").Where(null, false, false, "");
            var StockNumbers = queryFactory.GetList<ListStockNumbersDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListStockNumbersDto>>(StockNumbers);

        }


        [ValidationAspect(typeof(UpdateStockNumbersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStockNumbersDto>> UpdateAsync(UpdateStockNumbersDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.StockNumbers).Select("*").Where(new { Id = input.Id }, false, false, "");
            var entity = queryFactory.Get<StockNumbers>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.StockNumbers).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.GetList<StockNumbers>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.StockNumbers).Update(new UpdateStockNumbersDto
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
            }).Where(new { Id = input.Id }, false, false, "");

            var StockNumbers = queryFactory.Update<SelectStockNumbersDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, StockNumbers, LoginedUserService.UserId, Tables.StockNumbers, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStockNumbersDto>(StockNumbers);

        }

        public async Task<IDataResult<SelectStockNumbersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.StockNumbers).Select("*").Where(new { Id = id }, false, false, "");

            var entity = queryFactory.Get<StockNumbers>(entityQuery);

            var query = queryFactory.Query().From(Tables.StockNumbers).Update(new UpdateStockNumbersDto
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

            var StockNumbers = queryFactory.Update<SelectStockNumbersDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectStockNumbersDto>(StockNumbers);

        }
    }
}
