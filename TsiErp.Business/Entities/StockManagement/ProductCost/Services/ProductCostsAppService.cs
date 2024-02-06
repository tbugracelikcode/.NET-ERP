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
using TsiErp.Business.Entities.StockManagement.ProductCostCost.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.ProductCost;
using TsiErp.Entities.Entities.StockManagement.ProductCost.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ProductCosts.Page;

namespace TsiErp.Business.Entities.ProductCost.Services
{
    [ServiceRegistration(typeof(IProductCostsAppService), DependencyInjectionType.Scoped)]
    public class ProductCostsAppService : ApplicationService<ProductCostsResource>, IProductCostsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }

        public ProductCostsAppService(IStringLocalizer<ProductCostsResource> l, IFicheNumbersAppService ficheNumbersAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
        }

        [ValidationAspect(typeof(CreateProductCostsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductCostsDto>> CreateAsync(CreateProductCostsDto input)
        {

            Guid addedEntityId = GuidGenerator.CreateGuid();


            var query = queryFactory.Query().From(Tables.ProductCosts).Insert(new CreateProductCostsDto
            {
                Id = addedEntityId,
                EndDate = input.EndDate,
                IsValid = input.IsValid,
                ProductID = input.ProductID,
                StartDate = input.StartDate,
                UnitCost = input.UnitCost,
            });

            var ProductCosts = queryFactory.Insert<SelectProductCostsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ProductCosts, LogType.Insert, addedEntityId);

            return new SuccessDataResult<SelectProductCostsDto>(ProductCosts);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.ProductCosts).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

            var ProductCosts = queryFactory.Update<SelectProductCostsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ProductCosts, LogType.Delete, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductCostsDto>(ProductCosts);

        }


        public async Task<IDataResult<SelectProductCostsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.ProductCosts).Select<ProductCosts>(null)
                        .Join<Products>
                        (
                            u => new { ProductCode = u.Code, ProductID = u.Id, ProductName = u.Name },
                            nameof(ProductCosts.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, false, false, Tables.ProductCosts);

            var ProductCost = queryFactory.Get<SelectProductCostsDto>(query);

            LogsAppService.InsertLogToDatabase(ProductCost, ProductCost, LoginedUserService.UserId, Tables.ProductCosts, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductCostsDto>(ProductCost);


        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductCostsDto>>> GetListAsync(ListProductCostsParameterDto input)
        {
            var query = queryFactory
               .Query()
               .From(Tables.ProductCosts).Select<ProductCosts>(null)
                        .Join<Products>
                        (
                            u => new { ProductCode = u.Code, ProductID = u.Id, ProductName = u.Name },
                            nameof(ProductCosts.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        ).Where(null, false, false, Tables.ProductCosts);

            var productCosts = queryFactory.GetList<ListProductCostsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListProductCostsDto>>(productCosts);

        }


        [ValidationAspect(typeof(UpdateProductCostsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductCostsDto>> UpdateAsync(UpdateProductCostsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.ProductCosts).Select("*").Where(new { Id = input.Id }, false, false, "");
            var entity = queryFactory.Get<ProductCosts>(entityQuery);



            var query = queryFactory.Query().From(Tables.ProductCosts).Update(new UpdateProductCostsDto
            {
                Id = input.Id,
                EndDate = input.EndDate,
                IsValid = input.IsValid,
                StartDate = input.StartDate,
                UnitCost = input.UnitCost,
                ProductID = input.ProductID,
            }).Where(new { Id = input.Id }, false, false, "");

            var ProductCosts = queryFactory.Update<SelectProductCostsDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, ProductCosts, LoginedUserService.UserId, Tables.ProductCosts, LogType.Update, entity.Id);
            await Task.CompletedTask;

            return new SuccessDataResult<SelectProductCostsDto>(ProductCosts);

        }

        #region Unused Methods

        public Task<IDataResult<SelectProductCostsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
