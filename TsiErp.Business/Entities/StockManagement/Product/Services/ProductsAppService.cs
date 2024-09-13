using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Entities.Product.Validations;
using TsiErp.Business.Entities.StockAddress.Services;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductGroup;
using TsiErp.Entities.Entities.StockManagement.ProductRelatedProductProperty;
using TsiErp.Entities.Entities.StockManagement.ProductRelatedProductProperty.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockAddress.Dtos;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing;
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
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public ProductsAppService(IStringLocalizer<ProductsResource> l, IStockAddressesAppService stockAddressesAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            _stockAddressesAppService = stockAddressesAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }




        [ValidationAspect(typeof(CreateProductsValidator), Priority = 1)]
        public async Task<IDataResult<SelectProductsDto>> CreateAsync(CreateProductsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.Products).Select("Code").Where(new { Code = input.Code }, "");

            var list = queryFactory.ControlList<Products>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();


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
                CriticalStockQuantity = input.CriticalStockQuantity,
                isStandart = input.isStandart,
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
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
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
                    CreationTime = now,
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
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ProductsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains(","))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split(',');

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                Message_ = notTemplate.Message_,
                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = input.Code,
                                NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                UserId = new Guid(user),
                                ViewDate = null,
                            };

                            await _NotificationsAppService.CreateAsync(createInput);
                        }
                    }
                    else
                    {
                        CreateNotificationsDto createInput = new CreateNotificationsDto
                        {
                            ContextMenuName_ = notTemplate.ContextMenuName_,
                            IsViewed = false,
                            Message_ = notTemplate.Message_,
                            ModuleName_ = notTemplate.ModuleName_,
                            ProcessName_ = notTemplate.ProcessName_,
                            RecordNumber = input.Code,
                            NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                            UserId = new Guid(notTemplate.TargetUsersId),
                            ViewDate = null,
                        };

                        await _NotificationsAppService.CreateAsync(createInput);
                    }
                }

            }

            #endregion

            return new SuccessDataResult<SelectProductsDto>(products);

        }

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
                Tables.StockAddressLines,
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
                var entity = (await GetAsync(id)).Data;

                var query = queryFactory.Query().From(Tables.Products).Select("*").Where(new { Id = id }, "");

                var products = queryFactory.Get<SelectProductsDto>(query);

                if (products.Id != Guid.Empty && products != null)
                {

                    var deleteQuery = queryFactory.Query().From(Tables.Products).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.ProductRelatedProductProperties).Delete(LoginedUserService.UserId).Where(new { ProductID = id }, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var product = queryFactory.Update<SelectProductsDto>(deleteQuery, "Id", true);

                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Products, LogType.Delete, id);
                    #region Notification

                    var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ProductsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

                    if (notTemplate != null && notTemplate.Id != Guid.Empty)
                    {
                        if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                        {
                            if (notTemplate.TargetUsersId.Contains(","))
                            {
                                string[] usersNot = notTemplate.TargetUsersId.Split(',');

                                foreach (string user in usersNot)
                                {
                                    CreateNotificationsDto createInput = new CreateNotificationsDto
                                    {
                                        ContextMenuName_ = notTemplate.ContextMenuName_,
                                        IsViewed = false,
                                        Message_ = notTemplate.Message_,
                                        ModuleName_ = notTemplate.ModuleName_,
                                        ProcessName_ = notTemplate.ProcessName_,
                                        RecordNumber = entity.Code,
                                        NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                        UserId = new Guid(user),
                                        ViewDate = null,
                                    };

                                    await _NotificationsAppService.CreateAsync(createInput);
                                }
                            }
                            else
                            {
                                CreateNotificationsDto createInput = new CreateNotificationsDto
                                {
                                    ContextMenuName_ = notTemplate.ContextMenuName_,
                                    IsViewed = false,
                                    Message_ = notTemplate.Message_,
                                    ModuleName_ = notTemplate.ModuleName_,
                                    ProcessName_ = notTemplate.ProcessName_,
                                    RecordNumber = entity.Code,
                                    NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                    UserId = new Guid(notTemplate.TargetUsersId),
                                    ViewDate = null,
                                };

                                await _NotificationsAppService.CreateAsync(createInput);
                            }
                        }

                    }

                    #endregion

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
                    var queryLine = queryFactory.Query().From(Tables.ProductRelatedProductProperties).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

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
                        .Where(new { Id = id }, Tables.Products);

            var product = queryFactory.Get<SelectProductsDto>(query);

            var queryLines = queryFactory
                  .Query()
                  .From(Tables.ProductRelatedProductProperties)
                  .Select("*").Where(
           new
           {
               ProductID = id
           }, "");

            var ProductRelatedProductProperties = queryFactory.GetList<SelectProductRelatedProductPropertiesDto>(queryLines).ToList();

            product.SelectProductRelatedProductProperties = ProductRelatedProductProperties;

            LogsAppService.InsertLogToDatabase(product, product, LoginedUserService.UserId, Tables.Products, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductsDto>(product);


        }

        public async Task<IDataResult<IList<ListProductsDto>>> GetListAsync(ListProductsParameterDto input)
        {
            var query = queryFactory
               .Query()
               .From(Tables.Products)
              .Select<Products, GrandTotalStockMovements>
              (
                  s => new { s.Code, s.Name, s.SupplyForm, s.ProductType, s.Id, s.UnitSetID, s.SaleVAT, s.PurchaseVAT, s.isStandart,s.ProductGrpID,s.ProductSize,s.CriticalStockQuantity }
                , t => new { t.Amount, t.TotalReserved, t.TotalPurchaseOrder }
                , Tables.GrandTotalStockMovements
                , true
                , nameof(GrandTotalStockMovements.ProductID) + "=" + Tables.Products + "." + nameof(Products.Id))
                   .Join<UnitSets>
                   (
                        u => new { UnitSetCode = u.Code, UnitSetID = u.Id },
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
                    ).Where(null, Tables.Products);

            var products = queryFactory.GetList<ListProductsDto>(query).ToList();

            if (products.Count > 0 && products != null)
            {
                foreach (var product in products)
                {
                    int index = products.IndexOf(product);
                    products[index].TotalAvailableStock = product.Amount - product.TotalReserved;

                    if (products[index].TotalAvailableStock < 0)
                    {
                        products[index].TotalAvailableStock = 0;
                    }

                }
            }



            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListProductsDto>>(products);

        }

        public async Task<IDataResult<SelectProductsDto>> GetbyProductIDAsync(Guid productId)
        {
            var query = queryFactory
                    .Query().From(Tables.Products).Select<Products>(s=> new {s.Id, s.Code, s.Name, s.isStandart})
                        .Join<ProductGroups>
                        (
                            pg => new { ProductGrp = pg.Name, ProductGrpID = pg.Id },
                            nameof(Products.ProductGrpID),
                            nameof(ProductGroups.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = productId, isStandart = true}, " ");

            var products = queryFactory.Get<SelectProductsDto>(query);

            LogsAppService.InsertLogToDatabase(products, products, LoginedUserService.UserId, Tables.Products, LogType.Get, productId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductsDto>(products);

        }

        [ValidationAspect(typeof(UpdateProductsValidator), Priority = 1)]
        public async Task<IDataResult<SelectProductsDto>> UpdateAsync(UpdateProductsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.Products).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<Products>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.Products).Select("*").Where(new { Code = input.Code }, "");
            var list = queryFactory.GetList<Products>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.Products).Update(new UpdateProductsDto
            {
                Code = input.Code,
                Name = input.Name,
                Id = input.Id,
                CoatingWeight = input.CoatingWeight,
                Confirmation = input.Confirmation,
                EnglishDefinition = input.EnglishDefinition,
                isStandart = input.isStandart,
                CriticalStockQuantity = input.CriticalStockQuantity,
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
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId

            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectProductRelatedProductProperties)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.ProductRelatedProductProperties).Insert(new CreateProductRelatedProductPropertiesDto
                    {
                        CreationTime = now,
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
                    var lineGetQuery = queryFactory.Query().From(Tables.ProductRelatedProductProperties).Select("*").Where(new { Id = item.Id }, "");

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
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                            ProductGroupID = item.ProductGroupID,
                            ProductPropertyID = item.ProductPropertyID,
                            IsQualityControlCriterion = item.IsQualityControlCriterion,
                            isPurchaseBreakdown = item.isPurchaseBreakdown,
                            ProductID = item.ProductID,
                            PropertyName = item.PropertyName,
                            PropertyValue = item.PropertyValue,
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var products = queryFactory.Update<SelectProductsDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, products, LoginedUserService.UserId, Tables.Products, LogType.Update, entity.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ProductsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains(","))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split(',');

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                Message_ = notTemplate.Message_,
                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = input.Code,
                                NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                UserId = new Guid(user),
                                ViewDate = null,
                            };

                            await _NotificationsAppService.CreateAsync(createInput);
                        }
                    }
                    else
                    {
                        CreateNotificationsDto createInput = new CreateNotificationsDto
                        {
                            ContextMenuName_ = notTemplate.ContextMenuName_,
                            IsViewed = false,
                            Message_ = notTemplate.Message_,
                            ModuleName_ = notTemplate.ModuleName_,
                            ProcessName_ = notTemplate.ProcessName_,
                            RecordNumber = input.Code,
                            NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                            UserId = new Guid(notTemplate.TargetUsersId),
                            ViewDate = null,
                        };

                        await _NotificationsAppService.CreateAsync(createInput);
                    }
                }

            }

            #endregion

            await Task.CompletedTask;

            return new SuccessDataResult<SelectProductsDto>(products);

        }

        public async Task<IDataResult<SelectProductsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.Products).Select("*").Where(new { Id = id }, "");
            var entity = queryFactory.Get<Products>(entityQuery);

            var query = queryFactory.Query().From(Tables.Products).Update(new UpdateProductsDto
            {
                Code = entity.Code,
                Name = entity.Name,
                CoatingWeight = entity.CoatingWeight,
                Confirmation = entity.Confirmation,
                EnglishDefinition = entity.EnglishDefinition,
                CriticalStockQuantity = entity.CriticalStockQuantity,
                isStandart = entity.isStandart,
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


            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var products = queryFactory.Update<SelectProductsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductsDto>(products);


        }

        public async Task<IResult> DeleteProductRelatedPropertiesAsync(Guid productId, Guid productGroupId)
        {
            var deleteQuery = queryFactory.Query().From(Tables.ProductRelatedProductProperties).Delete(LoginedUserService.UserId).Where(new { ProductID = productId, ProductGroupID = productGroupId }, "");

            var product = queryFactory.Update<SelectProductsDto>(deleteQuery, "Id", true);

            await Task.CompletedTask;

            return new SuccessDataResult<SelectProductsDto>(product);
        }

        public async Task<IDataResult<IList<ListProductRelatedProductPropertiesDto>>> GetProductRelatedPropertiesAsync(Guid productId, Guid productGroupId)
        {
            var query = queryFactory.Query().From(Tables.ProductRelatedProductProperties).Select<ProductRelatedProductProperties>(null).Where(new { ProductID = productId, ProductGroupID = productGroupId }, "");

            var properties = queryFactory.GetList<ListProductRelatedProductPropertiesDto>(query).ToList();

            await Task.CompletedTask;

            return new SuccessDataResult<IList<ListProductRelatedProductPropertiesDto>>(properties);
        }
    }
}
