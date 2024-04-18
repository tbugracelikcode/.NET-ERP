using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.ProductReferanceNumber.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.ProductReferanceNumber;
using TsiErp.Entities.Entities.StockManagement.ProductReferanceNumber.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ProductReferanceNumbers.Page;

namespace TsiErp.Business.Entities.ProductReferanceNumber.Services
{
    [ServiceRegistration(typeof(IProductReferanceNumbersAppService), DependencyInjectionType.Scoped)]
    public class ProductReferanceNumbersAppService : ApplicationService<ProductReferanceNumbersResource>, IProductReferanceNumbersAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public ProductReferanceNumbersAppService(IStringLocalizer<ProductReferanceNumbersResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }


        [ValidationAspect(typeof(CreateProductReferanceNumbersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductReferanceNumbersDto>> CreateAsync(CreateProductReferanceNumbersDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.ProductReferanceNumbers).Select("*").Where(new { ReferanceNo = input.ReferanceNo }, false, false, "");

            var list = queryFactory.ControlList<ProductReferanceNumbers>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.ProductReferanceNumbers).Insert(new CreateProductReferanceNumbersDto
            {
                ReferanceNo = input.ReferanceNo,
                CurrentAccountCardID = input.CurrentAccountCardID,
                ProductID = input.ProductID,
                CustomerBarcodeNo = input.CustomerBarcodeNo,
                CustomerReferanceNo = input.CustomerReferanceNo,
                MinOrderAmount = input.MinOrderAmount,
                OrderReferanceNo = input.OrderReferanceNo,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Description_ = input.Description_,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty
            });

            var productReferanceNumbers = queryFactory.Insert<SelectProductReferanceNumbersDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ProductReferanceNumbers, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductReferanceNumbersDto>(productReferanceNumbers);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.ProductReferanceNumbers).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

            var productReferanceNumbers = queryFactory.Update<SelectProductReferanceNumbersDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ProductReferanceNumbers, LogType.Delete, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductReferanceNumbersDto>(productReferanceNumbers);

        }

       


        public async Task<IDataResult<SelectProductReferanceNumbersDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.ProductReferanceNumbers).Select<ProductReferanceNumbers>(null)
                        .Join<Products>
                        (
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                            nameof(ProductReferanceNumbers.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                        .Join<CurrentAccountCards>
                        (
                            ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CustomerCode = ca.CustomerCode },
                            nameof(ProductReferanceNumbers.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, false, false, Tables.ProductReferanceNumbers);

            var productReferanceNumber = queryFactory.Get<SelectProductReferanceNumbersDto>(query);

            LogsAppService.InsertLogToDatabase(productReferanceNumber, productReferanceNumber, LoginedUserService.UserId, Tables.ProductReferanceNumbers, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductReferanceNumbersDto>(productReferanceNumber);

        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductReferanceNumbersDto>>> GetListAsync(ListProductReferanceNumbersParameterDto input)
        {
            var query = queryFactory
               .Query()
               .From(Tables.ProductReferanceNumbers).Select<ProductReferanceNumbers>(null)
                        .Join<Products>
                        (
                            p => new { ProductCode = p.Code, ProductName = p.Name, ProductID = p.Id },
                            nameof(ProductReferanceNumbers.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                        .Join<CurrentAccountCards>
                        (
                            ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CustomerCode = ca.CustomerCode },
                            nameof(ProductReferanceNumbers.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
               .Where(null, false, false, Tables.ProductReferanceNumbers);

            var productReferanceNumbers = queryFactory.GetList<ListProductReferanceNumbersDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListProductReferanceNumbersDto>>(productReferanceNumbers);

        }

        public async Task<IDataResult<IList<SelectProductReferanceNumbersDto>>> GetSelectListAsync(Guid productId)
        {
            var query = queryFactory
                    .Query().From(Tables.ProductReferanceNumbers).Select<ProductReferanceNumbers>(null)
                        .Join<Products>
                        (
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                            nameof(ProductReferanceNumbers.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                        .Join<CurrentAccountCards>
                        (
                            ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                            nameof(ProductReferanceNumbers.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Where(new { ProductID = productId }, false, false, Tables.ProductReferanceNumbers);

            var productReferanceNumber = queryFactory.GetList<SelectProductReferanceNumbersDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<SelectProductReferanceNumbersDto>>(productReferanceNumber);

        }


        [ValidationAspect(typeof(UpdateProductReferanceNumbersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductReferanceNumbersDto>> UpdateAsync(UpdateProductReferanceNumbersDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.ProductReferanceNumbers).Select("*").Where(new { Id = input.Id }, false, false, "");
            var entity = queryFactory.Get<ProductReferanceNumbers>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.ProductReferanceNumbers).Select("*").Where(new { ReferanceNo = input.ReferanceNo }, false, false, "");
            var list = queryFactory.GetList<ProductReferanceNumbers>(listQuery).ToList();

            if (list.Count > 0 && entity.ReferanceNo != input.ReferanceNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.ProductReferanceNumbers).Update(new UpdateProductReferanceNumbersDto
            {
                ReferanceNo = input.ReferanceNo,
                CurrentAccountCardID = input.CurrentAccountCardID,
                ProductID = input.ProductID,
                Description_ = input.Description_,
                CustomerBarcodeNo = input.CustomerBarcodeNo,
                CustomerReferanceNo = input.CustomerReferanceNo,
                MinOrderAmount = input.MinOrderAmount,
                OrderReferanceNo = input.OrderReferanceNo,
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
            }).Where(new { Id = input.Id }, false, false, "");

            var productReferanceNumbers = queryFactory.Update<SelectProductReferanceNumbersDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, productReferanceNumbers, LoginedUserService.UserId, Tables.ProductReferanceNumbers, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductReferanceNumbersDto>(productReferanceNumbers);

        }

        public async Task<IDataResult<SelectProductReferanceNumbersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.ProductReferanceNumbers).Select("*").Where(new { Id = id }, false, false, "");
            var entity = queryFactory.Get<ProductReferanceNumbers>(entityQuery);

            var query = queryFactory.Query().From(Tables.ProductReferanceNumbers).Update(new UpdateProductReferanceNumbersDto
            {
                ReferanceNo = entity.ReferanceNo,
                CurrentAccountCardID = entity.CurrentAccountCardID,
                ProductID = entity.ProductID,
                Description_ = entity.Description_,
                CreationTime = entity.CreationTime.Value,
                CustomerBarcodeNo = entity.CustomerBarcodeNo,
                CustomerReferanceNo = entity.CustomerReferanceNo,
                MinOrderAmount = entity.MinOrderAmount,
                OrderReferanceNo = entity.OrderReferanceNo,
                CreatorId = entity.CreatorId.Value,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Id = id,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,

            }).Where(new { Id = id }, false, false, "");

            var productReferanceNumbers = queryFactory.Update<SelectProductReferanceNumbersDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductReferanceNumbersDto>(productReferanceNumbers);


        }

        public string GetLastSupplierReferanceNumber(Guid ProductID, Guid CurrentAccountID)
        {
            var query = queryFactory
                   .Query().From(Tables.ProductReferanceNumbers).Select<ProductReferanceNumbers>(null)
                       .Join<Products>
                       (
                           p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                           nameof(ProductReferanceNumbers.ProductID),
                           nameof(Products.Id),
                           JoinType.Left
                       )
                       .Join<CurrentAccountCards>
                       (
                           ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CustomerCode = ca.CustomerCode },
                           nameof(ProductReferanceNumbers.CurrentAccountCardID),
                           nameof(CurrentAccountCards.Id),
                           JoinType.Left
                       )
                       .Where(new { ProductID = ProductID , CurrentAccountCardID = CurrentAccountID }, false, false, Tables.ProductReferanceNumbers);

            var productReferanceNumber = queryFactory.GetList<SelectProductReferanceNumbersDto>(query).ToList().LastOrDefault();

            return productReferanceNumber.CustomerReferanceNo;
        }
    }
}
