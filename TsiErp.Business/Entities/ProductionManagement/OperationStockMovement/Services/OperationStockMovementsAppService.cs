using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.ProductionManagement.OperationStockMovement.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.OperationStockMovement.Page;

namespace TsiErp.Business.Entities.ProductionManagement.OperationStockMovement.Services
{
    [ServiceRegistration(typeof(IOperationStockMovementsAppService), DependencyInjectionType.Scoped)]
    public class OperationStockMovementsAppService : ApplicationService<OperationStockMovementsResources>, IOperationStockMovementsAppService
    {

        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public OperationStockMovementsAppService(IStringLocalizer<OperationStockMovementsResources> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }




        public async Task<IDataResult<SelectOperationStockMovementsDto>> CreateAsync(CreateOperationStockMovementsDto input)
        {
            if(input.Id == Guid.Empty)
            {
                Guid addedEntityId = GuidGenerator.CreateGuid();
                input.Id = addedEntityId;
            }
            var query = queryFactory.Query().From(Tables.OperationStockMovements).Insert(input).UseIsDelete(false);

            var insertedEntity = queryFactory.Insert<SelectOperationStockMovementsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.OperationStockMovements, LogType.Insert, input.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectOperationStockMovementsDto>(insertedEntity);

        }

        public async Task<IDataResult<SelectOperationStockMovementsDto>> UpdateAsync(UpdateOperationStockMovementsDto input)
        {
            var query = queryFactory.Query().From(Tables.OperationStockMovements).Insert(input).UseIsDelete(false);

            var updatedEntity = queryFactory.Insert<SelectOperationStockMovementsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.OperationStockMovements, LogType.Update, input.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectOperationStockMovementsDto>(updatedEntity);

        }

        public async Task<IDataResult<SelectOperationStockMovementsDto>> GetByProductionOrderIdAsync(Guid productionOrderId, Guid productOperationId)
        {
            var query = queryFactory.Query().From(Tables.OperationStockMovements).Select("*").Where(new { ProductionorderID = productionOrderId, OperationID = productOperationId }, "").UseIsDelete(false);

            var entity = queryFactory.Get<SelectOperationStockMovementsDto>(query);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectOperationStockMovementsDto>(entity);

        }

        public async Task<IDataResult<IList<ListOperationStockMovementsDto>>> GetListAsync(ListOperationStockMovementsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.OperationStockMovements).Select("*").Where(null, "");
            var entity = queryFactory.GetList<ListOperationStockMovementsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListOperationStockMovementsDto>>(entity);

        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectOperationStockMovementsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectOperationStockMovementsDto>> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
