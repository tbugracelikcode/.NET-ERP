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
using TsiErp.Business.Entities.Product.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductGroup;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Products.Page;

namespace TsiErp.Business.Entities.Product.Services
{
    [ServiceRegistration(typeof(IProductsAppService), DependencyInjectionType.Scoped)]
    public class ProductsAppService : ApplicationService<ProductsResource>, IProductsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }

        public ProductsAppService(IStringLocalizer<ProductsResource> l, IFicheNumbersAppService ficheNumbersAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
        }

        [ValidationAspect(typeof(CreateProductsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductsDto>> CreateAsync(CreateProductsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.Products).Select("*").Where(new { Code = input.Code }, false, false, "");

                var list = queryFactory.ControlList<Products>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();


                var query = queryFactory.Query().From(Tables.Products).Insert(new CreateProductsDto
                {
                    Code = input.Code,
                    CoatingWeight = input.CoatingWeight,
                    Confirmation = input.Confirmation,
                    EnglishDefinition = input.EnglishDefinition,
                    ExportCatNo = input.ExportCatNo,
                    FeatureSetID = input.FeatureSetID,
                    GTIP = input.GTIP,
                    ManufacturerCode = input.ManufacturerCode,
                    OemRefNo = input.OemRefNo,
                    OemRefNo2 = input.OemRefNo2,
                    OemRefNo3 = input.OemRefNo3,
                    PlannedWastage = input.PlannedWastage,
                    ProductDescription = input.ProductDescription,
                    ProductGrpID = input.ProductGrpID,
                    ProductSize = input.ProductSize,
                    ProductType = input.ProductType,
                    PurchaseVAT = input.PurchaseVAT,
                    SaleVAT = input.SaleVAT,
                    SawWastage = input.SawWastage,
                    SupplyForm = input.SupplyForm,
                    TechnicalConfirmation = input.TechnicalConfirmation,
                    UnitSetID = input.UnitSetID,
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = addedEntityId,
                    IsActive = true,
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    Name = input.Name
                });

                var products = queryFactory.Insert<SelectProductsDto>(query, "Id", true);

                await FicheNumbersAppService.UpdateFicheNumberAsync("ProductsChildMenu", input.Code);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Products, LogType.Insert, addedEntityId);

                return new SuccessDataResult<SelectProductsDto>(products);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.Products).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                var products = queryFactory.Update<SelectProductsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Products, LogType.Delete, id);

                return new SuccessDataResult<SelectProductsDto>(products);
            }
        }


        public async Task<IDataResult<SelectProductsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                        .Query().From(Tables.Products).Select<Products>(p => new { p.Id, p.Code, p.Name, p.IsActive, p.DataOpenStatus, p.DataOpenStatusUserId, p.UnitSetID,p.CoatingWeight,p.Confirmation,p.EnglishDefinition,p.ExportCatNo,p.FeatureSetID,p.GTIP,p.ManufacturerCode,p.OemRefNo,p.OemRefNo2,p.OemRefNo3,p.TechnicalConfirmation,p.SupplyForm,p.SawWastage,p.SaleVAT,p.PurchaseVAT,p.ProductType,p.ProductSize,p.ProductGrpID,p.ProductDescription,p.PlannedWastage })
                            .Join<UnitSets>
                            (
                                u => new { UnitSet = u.Code, UnitSetID = u.Id },
                                nameof(Products.UnitSetID),
                                nameof(UnitSets.Id),
                                JoinType.Left
                            )
                            .Join<ProductGroups>
                            (
                                pg => new { ProductGrp = pg.Name, ProductGrpID = pg.Id },
                                nameof(Products.ProductGrpID),
                                nameof(ProductGroups.Id),
                                JoinType.Left
                            )
                            .Where(new { Id = id }, true, true, Tables.Products);

                var product = queryFactory.Get<SelectProductsDto>(query);

                LogsAppService.InsertLogToDatabase(product, product, LoginedUserService.UserId, Tables.Products, LogType.Get, id);

                return new SuccessDataResult<SelectProductsDto>(product);

            }
        }

        public async Task<IDataResult<IList<SelectGrandTotalStockMovementsDto>>> GetStockAmountAsync(Guid productid)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                        .Query().From(Tables.GrandTotalStockMovements).Select("*")
                            .Join<Products>
                            (
                                u => new { ProductCode = u.Code, ProductID = u.Id , ProductName = u.Name},
                                nameof(GrandTotalStockMovements.ProductID),
                                nameof(Products.Id),
                                JoinType.Left
                            )
                            .Join<Branches>
                            (
                                pg => new { BranchCode = pg.Code, BranchID = pg.Id, BranchName = pg.Name },
                                nameof(GrandTotalStockMovements.BranchID),
                                nameof(Branches.Id),
                                JoinType.Left
                            )
                             .Join<Warehouses>
                            (
                                pg => new { WarehouseCode = pg.Code, WarehouseID = pg.Id },
                                nameof(GrandTotalStockMovements.WarehouseID),
                                nameof(Warehouses.Id),
                                JoinType.Left
                            )
                            .Where(new { ProductID = productid }, false, false, Tables.GrandTotalStockMovements);

                var grandTotalStock = queryFactory.GetList<SelectGrandTotalStockMovementsDto>(query).ToList();

                return new SuccessDataResult<IList<SelectGrandTotalStockMovementsDto>>(grandTotalStock);

            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductsDto>>> GetListAsync(ListProductsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory
                   .Query()
                   .From(Tables.Products)
                  .Select<Products>(p => new { p.Id, p.Code, p.Name, p.IsActive, p.DataOpenStatus, p.DataOpenStatusUserId, p.UnitSetID, p.CoatingWeight, p.Confirmation, p.EnglishDefinition, p.ExportCatNo, p.FeatureSetID, p.GTIP, p.ManufacturerCode, p.OemRefNo, p.OemRefNo2, p.OemRefNo3, p.TechnicalConfirmation, p.SupplyForm, p.SawWastage, p.SaleVAT, p.PurchaseVAT, p.ProductType, p.ProductSize, p.ProductGrpID, p.ProductDescription, p.PlannedWastage })
                       .Join<UnitSets>
                       (
                            u => new { UnitSetCode = u.Code },
                                nameof(Products.UnitSetID),
                                nameof(UnitSets.Id),
                           JoinType.Left
                       )
                       .Join<ProductGroups>
                       (
                            pg => new { ProductGrp = pg.Name },
                                nameof(Products.ProductGrpID),
                                nameof(ProductGroups.Id),
                           JoinType.Left
                        ).Where(null, true, true, Tables.Products);

                var products = queryFactory.GetList<ListProductsDto>(query).ToList();

                foreach ( var product in products )
                {
                    var grandTotal = (await GetStockAmountAsync(product.Id)).Data.ToList();
                    product.AmountOfStock = grandTotal.Sum(t => t.Amount);
                }

                return new SuccessDataResult<IList<ListProductsDto>>(products);
            }

        }


        [ValidationAspect(typeof(UpdateProductsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductsDto>> UpdateAsync(UpdateProductsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.Products).Select("*").Where(new { Id = input.Id }, true, true, "");
                var entity = queryFactory.Get<Products>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.Products).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.GetList<Products>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.Products).Update(new UpdateProductsDto
                {
                    Code = input.Code,
                    Name = input.Name,
                    Id = input.Id,
                    IsActive = input.IsActive,
                    CoatingWeight = input.CoatingWeight,
                    Confirmation = input.Confirmation,
                    EnglishDefinition = input.EnglishDefinition,
                    ExportCatNo = input.ExportCatNo,
                    FeatureSetID = input.FeatureSetID,
                    GTIP = input.GTIP,
                    ManufacturerCode = input.ManufacturerCode,
                    OemRefNo = input.OemRefNo,
                    OemRefNo2 = input.OemRefNo2,
                    OemRefNo3 = input.OemRefNo3,
                    PlannedWastage = input.PlannedWastage,
                    ProductDescription = input.ProductDescription,
                    ProductGrpID = input.ProductGrpID,
                    ProductSize = input.ProductSize,
                    ProductType = input.ProductType,
                    PurchaseVAT = input.PurchaseVAT,
                    SaleVAT = input.SaleVAT,
                    SawWastage = input.SawWastage,
                    SupplyForm = input.SupplyForm,
                    TechnicalConfirmation = input.TechnicalConfirmation,
                    UnitSetID = input.UnitSetID,
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

                var products = queryFactory.Update<SelectProductsDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, products, LoginedUserService.UserId, Tables.Products, LogType.Update, entity.Id);


                return new SuccessDataResult<SelectProductsDto>(products);
            }
        }

        public async Task<IDataResult<SelectProductsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.Products).Select("*").Where(new { Id = id }, true, true, "");
                var entity = queryFactory.Get<Products>(entityQuery);

                var query = queryFactory.Query().From(Tables.Products).Update(new UpdateProductsDto
                {
                    Code = entity.Code,
                    Name = entity.Name,
                    IsActive = entity.IsActive,
                    CoatingWeight = entity.CoatingWeight,
                    Confirmation = entity.Confirmation,
                    EnglishDefinition = entity.EnglishDefinition,
                    ExportCatNo = entity.ExportCatNo,
                    FeatureSetID = entity.FeatureSetID,
                    GTIP = entity.GTIP,
                    ManufacturerCode = entity.ManufacturerCode,
                    OemRefNo = entity.OemRefNo,
                    OemRefNo2 = entity.OemRefNo2,
                    OemRefNo3 = entity.OemRefNo3,
                    PlannedWastage = entity.PlannedWastage,
                    ProductDescription = entity.ProductDescription,
                    ProductGrpID = entity.ProductGrpID,
                    ProductSize = entity.ProductSize,
                    ProductType = (int)entity.ProductType,
                    PurchaseVAT = entity.PurchaseVAT,
                    SaleVAT = entity.SaleVAT,
                    SawWastage = entity.SawWastage,
                    SupplyForm = (int)entity.SupplyForm,
                    TechnicalConfirmation = entity.TechnicalConfirmation,
                    UnitSetID = entity.UnitSetID,
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

                }).Where(new { Id = id }, true, true, "");

                var products = queryFactory.Update<SelectProductsDto>(query, "Id", true);

                return new SuccessDataResult<SelectProductsDto>(products);

            }
        }
    }
}
