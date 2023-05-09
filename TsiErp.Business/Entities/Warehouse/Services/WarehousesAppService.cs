using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.Warehouses.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Warehouse.BusinessRules;
using TsiErp.Business.Entities.Warehouse.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.WareHouse;
using TsiErp.Entities.Entities.WareHouse.Dtos;
using Microsoft.Extensions.Localization;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Entities.TableConstant;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;

namespace TsiErp.Business.Entities.Warehouse.Services
{
    [ServiceRegistration(typeof(IWarehousesAppService), DependencyInjectionType.Scoped)]
    public class WarehousesAppService : ApplicationService<WarehousesResource>, IWarehousesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public WarehousesAppService(IStringLocalizer<WarehousesResource> l) : base(l)
        {
        }

        [ValidationAspect(typeof(CreateWarehousesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectWarehousesDto>> CreateAsync(CreateWarehousesDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.Warehouses).Select("*").Where(new { Code = input.Code }, false, false, "");

                var list = queryFactory.ControlList<Warehouses>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion


                var query = queryFactory.Query().From(Tables.Warehouses).Insert(new CreateWarehousesDto
                {
                    Code = input.Code,
                    Name = input.Name,
                    IsActive = true,
                    Id = GuidGenerator.CreateGuid(),
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

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Warehouses, LogType.Insert, warehouses.Id);


                return new SuccessDataResult<SelectWarehousesDto>(warehouses);
            }

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.Warehouses).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                var warehouses = queryFactory.Update<SelectWarehousesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Warehouses, LogType.Delete, id);

                return new SuccessDataResult<SelectWarehousesDto>(warehouses);
            }

        }

        public async Task<IDataResult<SelectWarehousesDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
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

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListWarehousesDto>>> GetListAsync(ListWarehousesParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.Warehouses).Select("*").Where(null, true, true, "");
                var warehouses = queryFactory.GetList<ListWarehousesDto>(query).ToList();
                return new SuccessDataResult<IList<ListWarehousesDto>>(warehouses);
            }

        }


        [ValidationAspect(typeof(UpdateWarehousesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectWarehousesDto>> UpdateAsync(UpdateWarehousesDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.Warehouses).Select("*").Where(new { Id = input.Id }, true, true, "");
                var entity = queryFactory.Get<Warehouses>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.Warehouses).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.GetList<Warehouses>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
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
                    DeleterId = entity.DeleterId.Value,
                    DeletionTime = entity.DeletionTime.Value,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = input.Id }, true, true, "");

                var warehouses = queryFactory.Update<SelectWarehousesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, warehouses, LoginedUserService.UserId, Tables.Warehouses, LogType.Update, entity.Id);

                return new SuccessDataResult<SelectWarehousesDto>(warehouses);
            }

        }

        public async Task<IDataResult<SelectWarehousesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
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
}
