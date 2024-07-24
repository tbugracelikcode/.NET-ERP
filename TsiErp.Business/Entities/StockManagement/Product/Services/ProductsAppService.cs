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
using TsiErp.Business.Entities.Product.Validations;
using TsiErp.Business.Entities.StockAddress.Services;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductGroup;
using TsiErp.Entities.Entities.StockManagement.ProductRelatedProductProperty.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockAddress.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Products.Page;

namespace TsiErp.Business.Entities.Product.Services
{
    [ServiceRegistration(typeof(IProductsAppService), DependencyInjectionType.Scoped)]
    public class ProductsAppService : ApplicationService<ProductsResource>, IProductsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        private readonly IStockAddressesAppService _stockAddressesAppService;

        public ProductsAppService(IStringLocalizer<ProductsResource> l, IStockAddressesAppService stockAddressesAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _stockAddressesAppService = stockAddressesAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }

        [ValidationAspect(typeof(CreateProductsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductsDto>> CreateAsync(CreateProductsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.Products).Select("*").Where(new { Code = input.Code }, false, false, "");

            var list = queryFactory.ControlList<Products>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
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
                UnitWeight = input.UnitWeight,
                ProductGrpID = input.ProductGrpID.GetValueOrDefault(),
                ProductSize = input.ProductSize,
                ProductType = input.ProductType,
                PurchaseVAT = input.PurchaseVAT,
                SaleVAT = input.SaleVAT,
                SawWastage = input.SawWastage,
                SupplyForm = input.SupplyForm,
                TechnicalConfirmation = input.TechnicalConfirmation,
                UnitSetID = input.UnitSetID.GetValueOrDefault(),
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
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
                Name = input.Name,
                RawMaterialType = input.RawMaterialType,
                ExternalRadius = input.ExternalRadius,
                InternalRadius = input.InternalRadius,
                RadiusValue = input.RadiusValue,
                Width_ = input.Width_,
                Tickness_ = input.Tickness_
                

            });

            foreach (var item in input.SelectProductRelatedProductProperties)
            {
                var queryLine = queryFactory.Query().From(Tables.ProductRelatedProductProperties).Insert(new CreateProductRelatedProductPropertiesDto
                {
                    CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = GuidGenerator.CreateGuid(),
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    LineNr = item.LineNr,
                    ProductGroupID = item.ProductGroupID,
                    ProductPropertyID = item.ProductPropertyID,
                    isPurchaseBreakdown = item.isPurchaseBreakdown,
                    IsQualityControlCriterion = item.IsQualityControlCriterion,
                    ProductID = addedEntityId,
                    PropertyName = item.PropertyName,
                    PropertyValue = item.PropertyValue,
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var products = queryFactory.Insert<SelectProductsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Products, LogType.Insert, addedEntityId);

            return new SuccessDataResult<SelectProductsDto>(products);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("FinishedProductID", new List<string>
            {
                Tables.BillsofMaterials,
                Tables.ProductionOrders
            });

            DeleteControl.ControlList.Add("ProductID", new List<string>
            {
                Tables.BillsofMaterialLines,
                Tables.ByDateStockMovements,
                Tables.StockAddresses,
                Tables.ContractProductionTrackings,
                Tables.ContractQualityPlanLines,
                Tables.ContractQualityPlans,
                Tables.ContractTrackingFiches,
                Tables.CustomerComplaintReports,
                Tables.FinalControlUnsuitabilityReports,
                Tables.ShipmentPlanningLines,
                Tables.FirstProductApprovals,
                Tables.ProductReceiptTransactions,
                Tables.ForecastLines,
                Tables.GrandTotalStockMovements,
                Tables.MaintenanceInstructionLines,
                Tables.MaintenanceMRPLines,
                Tables.MRPLines,
                Tables.OperationalQualityPlanLines,
                Tables.OperationalQualityPlans,
                Tables.OperationUnsuitabilityReports,
                Tables.PackageFicheLines,
                Tables.PackageFiches,
                Tables.PackingListPalletPackageLines,
                Tables.PalletRecordLines,
                Tables.PlannedMaintenanceLines,
                Tables.ProductReferanceNumbers,
                Tables.ProductsOperations,
                Tables.PurchaseOrderLines,
                Tables.PurchasePriceLines,
                Tables.PurchaseQualityPlanLines,
                Tables.PurchaseQualityPlans,
                Tables.PurchaseRequestLines,
                Tables.PurchaseUnsuitabilityReports,
                Tables.Report8Ds,
                Tables.RouteLines,
                Tables.Routes,
                Tables.SalesOrderLines,
                Tables.SalesPriceLines,
                Tables.SalesPropositionLines,
                Tables.StationInventories,
                Tables.StockFicheLines,
                Tables.TechnicalDrawings,Tables.UnplannedMaintenanceLines,
                Tables.WorkOrders
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {

                var query = queryFactory.Query().From(Tables.Products).Select("*").Where(new { Id = id }, true, true, "");

                var products = queryFactory.Get<SelectProductsDto>(query);

                if (products.Id != Guid.Empty && products != null)
                {

                    var deleteQuery = queryFactory.Query().From(Tables.Products).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.ProductRelatedProductProperties).Delete(LoginedUserService.UserId).Where(new { ProductID = id }, false, false, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var product = queryFactory.Update<SelectProductsDto>(deleteQuery, "Id", true);

                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Products, LogType.Delete, id);

                    var stockAddressList = (await _stockAddressesAppService.GetListAsync(new ListStockAddressesParameterDto())).Data.Where(t => t.ProductID == id).ToList();

                    if (stockAddressList != null && stockAddressList.Count > 0)
                    {
                        foreach (var stockAddress in stockAddressList)
                        {
                            await _stockAddressesAppService.DeleteAsync(stockAddress.Id);
                        }
                    }

                    await Task.CompletedTask;
                    return new SuccessDataResult<SelectProductsDto>(product);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.ProductRelatedProductProperties).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                    var ProductRelatedProductProperties = queryFactory.Update<SelectProductRelatedProductPropertiesDto>(queryLine, "Id", true);

                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ProductRelatedProductProperties, LogType.Delete, id);

                    await Task.CompletedTask;
                    return new SuccessDataResult<SelectProductRelatedProductPropertiesDto>(ProductRelatedProductProperties);
                }
            }
        }


        public async Task<IDataResult<SelectProductsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.Products).Select<Products>(null)
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

            var queryLines = queryFactory
                  .Query()
                  .From(Tables.ProductRelatedProductProperties)
                  .Select("*").Where(
           new
           {
               ProductID = id
           }, false, false, "");

            var ProductRelatedProductProperties = queryFactory.GetList<SelectProductRelatedProductPropertiesDto>(queryLines).ToList();

            product.SelectProductRelatedProductProperties = ProductRelatedProductProperties;

            LogsAppService.InsertLogToDatabase(product, product, LoginedUserService.UserId, Tables.Products, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductsDto>(product);


        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductsDto>>> GetListAsync(ListProductsParameterDto input)
        {
            var query = queryFactory
               .Query()
               .From(Tables.Products)
              .Select<Products, GrandTotalStockMovements>
              (
                  null
                , t => new { t.Amount, t.TotalReserved, t.TotalPurchaseOrder }
                , Tables.GrandTotalStockMovements
                , true
                , nameof(GrandTotalStockMovements.ProductID) + "=" + Tables.Products + "." + nameof(Products.Id))
                   .Join<UnitSets>
                   (
                        u => new { UnitSetCode = u.Code },
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
                    ).Where(null, true, true, Tables.Products);

            var products = queryFactory.GetList<ListProductsDto>(query).ToList();

            if (products.Count > 0 && products != null)
            {
                foreach (var product in products)
                {
                    int index = products.IndexOf(product);
                    products[index].TotalAvailableStock = product.Amount - product.TotalReserved;

                    if(products[index].TotalAvailableStock < 0)
                    {
                        products[index].TotalAvailableStock = 0;
                    }
                    
                }
            }



            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListProductsDto>>(products);

        }


        [ValidationAspect(typeof(UpdateProductsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductsDto>> UpdateAsync(UpdateProductsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.Products).Select("*").Where(new { Id = input.Id }, true, true, "");
            var entity = queryFactory.Get<Products>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.Products).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.GetList<Products>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
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
                ProductGrpID = input.ProductGrpID.GetValueOrDefault(),
                UnitWeight = input.UnitWeight,
                ProductSize = input.ProductSize,
                ProductType = input.ProductType,
                PurchaseVAT = input.PurchaseVAT,
                SaleVAT = input.SaleVAT,
                SawWastage = input.SawWastage,
                SupplyForm = input.SupplyForm,
                ExternalRadius = input.ExternalRadius,
                RawMaterialType = input.RawMaterialType,
                InternalRadius = input.InternalRadius,
                Tickness_ = input.Tickness_,
                RadiusValue = input.RadiusValue,
                Width_ = input.Width_,
                TechnicalConfirmation = input.TechnicalConfirmation,
                UnitSetID = input.UnitSetID.GetValueOrDefault(),
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId

            }).Where(new { Id = input.Id }, true, true, "");

            foreach (var item in input.SelectProductRelatedProductProperties)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.ProductRelatedProductProperties).Insert(new CreateProductRelatedProductPropertiesDto
                    {
                        CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        Id = GuidGenerator.CreateGuid(),
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                        LineNr = item.LineNr,
                        ProductGroupID = item.ProductGroupID,
                        ProductPropertyID = item.ProductPropertyID,
                        PropertyValue = item.PropertyValue,
                        PropertyName = item.PropertyName,
                        ProductID = item.ProductID,
                        isPurchaseBreakdown = item.isPurchaseBreakdown,
                        IsQualityControlCriterion = item.IsQualityControlCriterion,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.ProductRelatedProductProperties).Select("*").Where(new { Id = item.Id }, false, false, "");

                    var line = queryFactory.Get<SelectProductRelatedProductPropertiesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.ProductRelatedProductProperties).Update(new UpdateProductRelatedProductPropertiesDto
                        {
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                            ProductGroupID = item.ProductGroupID,
                            ProductPropertyID = item.ProductPropertyID,
                            IsQualityControlCriterion = item.IsQualityControlCriterion,
                            isPurchaseBreakdown = item.isPurchaseBreakdown,
                            ProductID = item.ProductID,
                            PropertyName = item.PropertyName,
                            PropertyValue = item.PropertyValue,
                        }).Where(new { Id = line.Id }, false, false, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var products = queryFactory.Update<SelectProductsDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, products, LoginedUserService.UserId, Tables.Products, LogType.Update, entity.Id);
            await Task.CompletedTask;

            return new SuccessDataResult<SelectProductsDto>(products);

        }

        public async Task<IDataResult<SelectProductsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
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
                UnitWeight = entity.UnitWeight,
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
                RawMaterialType = (int)entity.RawMaterialType,
                InternalRadius = entity.InternalRadius,
                RadiusValue = entity.RadiusValue,
                Width_ = entity.Width_,
                ExternalRadius = entity.ExternalRadius,
                Tickness_ = entity.Tickness_


            }).Where(new { Id = id }, true, true, "");

            var products = queryFactory.Update<SelectProductsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductsDto>(products);


        }
    }
}
