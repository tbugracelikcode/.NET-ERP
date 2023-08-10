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

        public ProductReferanceNumbersAppService(IStringLocalizer<ProductReferanceNumbersResource> l) : base(l)
        {
        }


        [ValidationAspect(typeof(CreateProductReferanceNumbersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductReferanceNumbersDto>> CreateAsync(CreateProductReferanceNumbersDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.ProductReferanceNumbers).Select("*").Where(new { ReferanceNo = input.ReferanceNo }, false, false, "");

                var list = queryFactory.ControlList<ProductReferanceNumbers>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();

                var query = queryFactory.Query().From(Tables.ProductReferanceNumbers).Insert(new CreateProductReferanceNumbersDto
                {
                    ReferanceNo = input.ReferanceNo,
                    CurrentAccountCardID = input.CurrentAccountCardID,
                    ProductID = input.ProductID,
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Description_ = input.Description_,
                    Id = addedEntityId,
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                });

                var productReferanceNumbers = queryFactory.Insert<SelectProductReferanceNumbersDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ProductReferanceNumbers, LogType.Insert,addedEntityId);

                return new SuccessDataResult<SelectProductReferanceNumbersDto>(productReferanceNumbers);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.ProductReferanceNumbers).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var productReferanceNumbers = queryFactory.Update<SelectProductReferanceNumbersDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ProductReferanceNumbers, LogType.Delete, id);

                return new SuccessDataResult<SelectProductReferanceNumbersDto>(productReferanceNumbers);
            }

        }


        public async Task<IDataResult<SelectProductReferanceNumbersDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                        .Query().From(Tables.ProductReferanceNumbers).Select<ProductReferanceNumbers>(prn => new {prn.ProductID,prn.DataOpenStatus,prn.DataOpenStatusUserId,prn.Id,prn.CurrentAccountCardID,prn.Description_,prn.ReferanceNo})
                            .Join<Products>
                            (
                                p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                                nameof(ProductReferanceNumbers.ProductID),
                                nameof(Products.Id),
                                JoinType.Left
                            )
                            .Join<CurrentAccountCards>
                            (
                                ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode= ca.Code, CurrentAccountCardName = ca.Name },
                                nameof(ProductReferanceNumbers.CurrentAccountCardID),
                                nameof(CurrentAccountCards.Id),
                                JoinType.Left
                            )
                            .Where(new { Id = id }, false, false, Tables.ProductReferanceNumbers);

                var productReferanceNumber = queryFactory.Get<SelectProductReferanceNumbersDto>(query);

                LogsAppService.InsertLogToDatabase(productReferanceNumber, productReferanceNumber, LoginedUserService.UserId, Tables.ProductReferanceNumbers, LogType.Get, id);

                return new SuccessDataResult<SelectProductReferanceNumbersDto>(productReferanceNumber);

            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductReferanceNumbersDto>>> GetListAsync(ListProductReferanceNumbersParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory
                   .Query()
                   .From(Tables.ProductReferanceNumbers).Select<ProductReferanceNumbers>(prn => new { prn.ProductID, prn.DataOpenStatus, prn.DataOpenStatusUserId, prn.Id, prn.CurrentAccountCardID, prn.Description_, prn.ReferanceNo })
                            .Join<Products>
                            (
                                p => new { ProductCode = p.Code, ProductName = p.Name },
                                nameof(ProductReferanceNumbers.ProductID),
                                nameof(Products.Id),
                                JoinType.Left
                            )
                            .Join<CurrentAccountCards>
                            (
                                ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                                nameof(ProductReferanceNumbers.CurrentAccountCardID),
                                nameof(CurrentAccountCards.Id),
                                JoinType.Left
                            )
                   .Where(null, false, false, Tables.ProductReferanceNumbers);

                var productReferanceNumbers = queryFactory.GetList<ListProductReferanceNumbersDto>(query).ToList();

                return new SuccessDataResult<IList<ListProductReferanceNumbersDto>>(productReferanceNumbers);
            }
        }

        public async Task<IDataResult<IList<SelectProductReferanceNumbersDto>>> GetSelectListAsync(Guid productId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                        .Query().From(Tables.ProductReferanceNumbers).Select<ProductReferanceNumbers>(prn => new { prn.ProductID, prn.DataOpenStatus, prn.DataOpenStatusUserId, prn.Id, prn.CurrentAccountCardID, prn.Description_, prn.ReferanceNo })
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

                return new SuccessDataResult<IList<SelectProductReferanceNumbersDto>>(productReferanceNumber);

            }
        }


        [ValidationAspect(typeof(UpdateProductReferanceNumbersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductReferanceNumbersDto>> UpdateAsync(UpdateProductReferanceNumbersDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.ProductReferanceNumbers).Select("*").Where(new { Id = input.Id }, false, false, "");
                var entity = queryFactory.Get<ProductReferanceNumbers>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.ProductReferanceNumbers).Select("*").Where(new { ReferanceNo = input.ReferanceNo }, false, false, "");
                var list = queryFactory.GetList<ProductReferanceNumbers>(listQuery).ToList();

                if (list.Count > 0 && entity.ReferanceNo != input.ReferanceNo)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.ProductReferanceNumbers).Update(new UpdateProductReferanceNumbersDto
                {
                    ReferanceNo = input.ReferanceNo,
                    CurrentAccountCardID = input.CurrentAccountCardID,
                    ProductID = input.ProductID,
                    Description_ = input.Description_,
                    Id = input.Id,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = input.Id }, false, false, "");

                var productReferanceNumbers = queryFactory.Update<SelectProductReferanceNumbersDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, productReferanceNumbers, LoginedUserService.UserId, Tables.ProductReferanceNumbers, LogType.Update, entity.Id);

                return new SuccessDataResult<SelectProductReferanceNumbersDto>(productReferanceNumbers);
            }
        }

        public async Task<IDataResult<SelectProductReferanceNumbersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
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

                return new SuccessDataResult<SelectProductReferanceNumbersDto>(productReferanceNumbers);

            }
        }
    }
}
