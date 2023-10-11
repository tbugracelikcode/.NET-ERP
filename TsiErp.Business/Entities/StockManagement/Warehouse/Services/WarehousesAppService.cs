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
using TsiErp.Business.Entities.Warehouse.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Warehouses.Page;

namespace TsiErp.Business.Entities.Warehouse.Services
{
    [ServiceRegistration(typeof(IWarehousesAppService), DependencyInjectionType.Scoped)]
    public class WarehousesAppService : ApplicationService<WarehousesResource>, IWarehousesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }

        public WarehousesAppService(IStringLocalizer<WarehousesResource> l, IFicheNumbersAppService ficheNumbersAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
        }

        [ValidationAspect(typeof(CreateWarehousesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectWarehousesDto>> CreateAsync(CreateWarehousesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.Warehouses).Select("*").Where(new { Code = input.Code }, false, false, "");

            var list = queryFactory.ControlList<Warehouses>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.Warehouses).Insert(new CreateWarehousesDto
            {
                Code = input.Code,
                Name = input.Name,
                IsActive = true,
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


            var warehouses = queryFactory.Insert<SelectWarehousesDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("WarehousesChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Warehouses, LogType.Insert, addedEntityId);


            return new SuccessDataResult<SelectWarehousesDto>(warehouses);


        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.Warehouses).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

            var warehouses = queryFactory.Update<SelectWarehousesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Warehouses, LogType.Delete, id);

            return new SuccessDataResult<SelectWarehousesDto>(warehouses);

        }

        public async Task<IDataResult<SelectWarehousesDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.Warehouses).Select("*").Where(
            new
            {
                Id = id
            }, true, true, "");
            var warehouse = queryFactory.Get<SelectWarehousesDto>(query);


            LogsAppService.InsertLogToDatabase(warehouse, warehouse, LoginedUserService.UserId, Tables.Warehouses, LogType.Get, id);

            return new SuccessDataResult<SelectWarehousesDto>(warehouse);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListWarehousesDto>>> GetListAsync(ListWarehousesParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.Warehouses).Select("*").Where(null, true, true, "");
            var warehouses = queryFactory.GetList<ListWarehousesDto>(query).ToList();
            return new SuccessDataResult<IList<ListWarehousesDto>>(warehouses);

        }


        [ValidationAspect(typeof(UpdateWarehousesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectWarehousesDto>> UpdateAsync(UpdateWarehousesDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.Warehouses).Select("*").Where(new { Id = input.Id }, true, true, "");
            var entity = queryFactory.Get<Warehouses>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.Warehouses).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.GetList<Warehouses>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.Warehouses).Update(new UpdateWarehousesDto
            {
                Code = input.Code,
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
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id }, true, true, "");

            var warehouses = queryFactory.Update<SelectWarehousesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, warehouses, LoginedUserService.UserId, Tables.Warehouses, LogType.Update, entity.Id);

            return new SuccessDataResult<SelectWarehousesDto>(warehouses);

        }

        public async Task<IDataResult<SelectWarehousesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.Warehouses).Select("*").Where(new { Id = id }, true, true, "");

            var entity = queryFactory.Get<Warehouses>(entityQuery);

            var query = queryFactory.Query().From(Tables.Warehouses).Update(new UpdateWarehousesDto
            {
                Code = entity.Code,
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
                DataOpenStatusUserId = userId

            }).Where(new { Id = id }, true, true, "");

            var warehouses = queryFactory.Update<SelectWarehousesDto>(query, "Id", true);
            return new SuccessDataResult<SelectWarehousesDto>(warehouses);

        }
    }
}
