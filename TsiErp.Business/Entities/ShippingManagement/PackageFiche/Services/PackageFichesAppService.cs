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
using TsiErp.Business.Entities.ShippingManagement.PackageFiche.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder;
using TsiErp.Entities.Entities.ShippingManagement.PackageFiche;
using TsiErp.Entities.Entities.ShippingManagement.PackageFiche.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackageFicheLine;
using TsiErp.Entities.Entities.ShippingManagement.PackageFicheLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PackageFiches.Page;

namespace TsiErp.Business.Entities.PackageFiche.Services
{
    [ServiceRegistration(typeof(IPackageFichesAppService), DependencyInjectionType.Scoped)]
    public class PackageFichesAppService : ApplicationService<PackageFichesResource>, IPackageFichesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }

        public PackageFichesAppService(IStringLocalizer<PackageFichesResource> l, IFicheNumbersAppService ficheNumbersAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
        }

        [ValidationAspect(typeof(CreatePackageFichesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPackageFichesDto>> CreateAsync(CreatePackageFichesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.PackageFiches).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.ControlList<PackageFiches>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.PackageFiches).Insert(new CreatePackageFichesDto
            {
                CurrentAccountID = input.CurrentAccountID,
                ProductID = input.ProductID,
                SalesOrderID = input.SalesOrderID,
                Date_ = input.Date_,
                NumberofPackage = input.NumberofPackage,
                PackageContent = input.PackageContent,
                PackageType = input.PackageType,
                PalletNumber = input.PalletNumber,
                ProductPalletOrder = input.ProductPalletOrder,
                UnitWeight = input.UnitWeight,
                Code = input.Code,
                CreationTime = DateTime.Now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
            });

            foreach (var item in input.SelectPackageFicheLines)
            {
                var queryLine = queryFactory.Query().From(Tables.PackageFicheLines).Insert(new CreatePackageFicheLinesDto
                {
                    PackingDate = item.PackingDate,
                    ProductionOrderID = item.ProductionOrderID,
                    PackageFicheID = addedEntityId,
                    CreationTime = DateTime.Now,
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
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var packageFiche = queryFactory.Insert<SelectPackageFichesDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("PackageFichesChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PackageFiches, LogType.Insert, addedEntityId);

            return new SuccessDataResult<SelectPackageFichesDto>(packageFiche);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.PackageFiches).Select("*").Where(new { Id = id }, false, false, "");

            var PackageFiches = queryFactory.Get<SelectPackageFichesDto>(query);

            if (PackageFiches.Id != Guid.Empty && PackageFiches != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.PackageFiches).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.PackageFicheLines).Delete(LoginedUserService.UserId).Where(new { PackageFicheID = id }, false, false, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var packageFiche = queryFactory.Update<SelectPackageFichesDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PackageFiches, LogType.Delete, id);
                return new SuccessDataResult<SelectPackageFichesDto>(packageFiche);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.PackageFicheLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                var packageFicheLines = queryFactory.Update<SelectPackageFicheLinesDto>(queryLine, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PackageFicheLines, LogType.Delete, id);
                return new SuccessDataResult<SelectPackageFicheLinesDto>(packageFicheLines);
            }

        }

        public async Task<IDataResult<SelectPackageFichesDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.PackageFiches)
                   .Select<PackageFiches>(null)
                   .Join<Products>
                    (
                        pr => new { ProductCode = pr.Code, ProducName = pr.Name, ProductID = pr.Id, ProductUnitWeight = pr.UnitWeight },
                        nameof(PackageFiches.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                     .Join<SalesOrders>
                    (
                        pr => new { SalesOrderFicheNo = pr.FicheNo, SalesOrderCustomerOrderNo = pr.CustomerOrderNr, SalesOrderID = pr.Id },
                        nameof(PackageFiches.SalesOrderID),
                        nameof(SalesOrders.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CustomerCode = pr.CustomerCode, CurrentAccountID = pr.Id },
                        nameof(PackageFiches.CurrentAccountID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, false, false, Tables.PackageFiches);

            var packageFiches = queryFactory.Get<SelectPackageFichesDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PackageFicheLines)
                   .Select<PackageFicheLines>(null)
                   .Join<ProductionOrders>
                    (
                        p => new { ProductionOrderID = p.Id, ProductionOrderFicheNo = p.FicheNo },
                        nameof(PackageFicheLines.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PackageFicheLines.ProductID),
                        nameof(Products.Id),
                        "ProductLine",
                        JoinType.Left
                    )
                    .Where(new { PackageFicheID = id }, false, false, Tables.PackageFicheLines);

            var PackageFicheLine = queryFactory.GetList<SelectPackageFicheLinesDto>(queryLines).ToList();

            packageFiches.SelectPackageFicheLines = PackageFicheLine;

            LogsAppService.InsertLogToDatabase(packageFiches, packageFiches, LoginedUserService.UserId, Tables.PackageFiches, LogType.Get, id);

            return new SuccessDataResult<SelectPackageFichesDto>(packageFiches);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPackageFichesDto>>> GetListAsync(ListPackageFichesParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.PackageFiches)
                   .Select<PackageFiches>(null)
                   .Join<Products>
                    (
                        pr => new { ProductCode = pr.Code, ProducName = pr.Name, ProductID = pr.Id, ProductUnitWeight = pr.UnitWeight },
                        nameof(PackageFiches.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                     .Join<SalesOrders>
                    (
                        pr => new { SalesOrderFicheNo = pr.FicheNo, SalesOrderCustomerOrderNo = pr.CustomerOrderNr, SalesOrderID = pr.Id },
                        nameof(PackageFiches.SalesOrderID),
                        nameof(SalesOrders.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CustomerCode = pr.CustomerCode, CurrentAccountID = pr.Id },
                        nameof(PackageFiches.CurrentAccountID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(null, false, false, Tables.PackageFiches);

            var packageFiches = queryFactory.GetList<ListPackageFichesDto>(query).ToList();
            return new SuccessDataResult<IList<ListPackageFichesDto>>(packageFiches);

        }

        [ValidationAspect(typeof(UpdatePackageFichesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPackageFichesDto>> UpdateAsync(UpdatePackageFichesDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.PackageFiches)
                   .Select<PackageFiches>(null)
                   .Join<Products>
                    (
                        pr => new { ProductCode = pr.Code, ProducName = pr.Name, ProductID = pr.Id, ProductUnitWeight = pr.UnitWeight },
                        nameof(PackageFiches.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                     .Join<SalesOrders>
                    (
                        pr => new { SalesOrderFicheNo = pr.FicheNo, SalesOrderCustomerOrderNo = pr.CustomerOrderNr, SalesOrderID = pr.Id },
                        nameof(PackageFiches.SalesOrderID),
                        nameof(SalesOrders.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CustomerCode = pr.CustomerCode, CurrentAccountID = pr.Id },
                        nameof(PackageFiches.CurrentAccountID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, false, false, Tables.PackageFiches);

            var entity = queryFactory.Get<SelectPackageFichesDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PackageFicheLines)
                   .Select<PackageFicheLines>(null)
                   .Join<ProductionOrders>
                    (
                        p => new { ProductionOrderID = p.Id, ProductionOrderFicheNo = p.FicheNo },
                        nameof(PackageFicheLines.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PackageFicheLines.ProductID),
                        nameof(Products.Id),
                        "ProductLine",
                        JoinType.Left
                    )
                    .Where(new { PackageFicheID = input.Id }, false, false, Tables.PackageFicheLines);

            var PackageFicheLine = queryFactory.GetList<SelectPackageFicheLinesDto>(queryLines).ToList();

            entity.SelectPackageFicheLines = PackageFicheLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.PackageFiches)
                   .Select<PackageFiches>(null)
                   .Join<Products>
                    (
                        pr => new { ProductCode = pr.Code, ProducName = pr.Name, ProductID = pr.Id, ProductUnitWeight = pr.UnitWeight },
                        nameof(PackageFiches.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                     .Join<SalesOrders>
                    (
                        pr => new { SalesOrderFicheNo = pr.FicheNo, SalesOrderCustomerOrderNo = pr.CustomerOrderNr, SalesOrderID = pr.Id },
                        nameof(PackageFiches.SalesOrderID),
                        nameof(SalesOrders.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CustomerCode = pr.CustomerCode, CurrentAccountID = pr.Id },
                        nameof(PackageFiches.CurrentAccountID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                            .Where(new { Code = input.Code }, false, false, Tables.PackageFiches);

            var list = queryFactory.GetList<ListPackageFichesDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            var query = queryFactory.Query().From(Tables.PackageFiches).Update(new UpdatePackageFichesDto
            {
                CurrentAccountID = input.CurrentAccountID,
                ProductID = input.ProductID,
                SalesOrderID = input.SalesOrderID,
                Date_ = input.Date_,
                NumberofPackage = input.NumberofPackage,
                PackageContent = input.PackageContent,
                PackageType = input.PackageType,
                PalletNumber = input.PalletNumber,
                ProductPalletOrder = input.ProductPalletOrder,
                UnitWeight = input.UnitWeight,
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = DateTime.Now,
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id }, false, false, "");

            foreach (var item in input.SelectPackageFicheLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PackageFicheLines).Insert(new CreatePackageFicheLinesDto
                    {
                        PackageFicheID = input.Id,
                        PackingDate = item.PackingDate,
                        ProductionOrderID = item.ProductionOrderID,
                        Status_ = item.Status_,
                        CreationTime = DateTime.Now,
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
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.PackageFicheLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                    var line = queryFactory.Get<SelectPackageFicheLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PackageFicheLines).Update(new UpdatePackageFicheLinesDto
                        {
                            PackageFicheID = input.Id,
                            PackingDate = item.PackingDate,
                            ProductionOrderID = item.ProductionOrderID,
                            Status_ = item.Status_,
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime = DateTime.Now,
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                            ProductID = item.ProductID,
                            Quantity = item.Quantity,
                        }).Where(new { Id = line.Id }, false, false, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var billOfMaterial = queryFactory.Update<SelectPackageFichesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PackageFiches, LogType.Update, billOfMaterial.Id);

            return new SuccessDataResult<SelectPackageFichesDto>(billOfMaterial);

        }

        public async Task<IDataResult<SelectPackageFichesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.PackageFiches).Select("*").Where(new { Id = id }, false, false, "");

            var entity = queryFactory.Get<PackageFiches>(entityQuery);

            var query = queryFactory.Query().From(Tables.PackageFiches).Update(new UpdatePackageFichesDto
            {
                CurrentAccountID = entity.CurrentAccountID,
                ProductID = entity.ProductID,
                SalesOrderID = entity.SalesOrderID,
                Date_ = entity.Date_,
                NumberofPackage = entity.NumberofPackage,
                PackageContent = entity.PackageContent,
                PackageType = entity.PackageType,
                PalletNumber = entity.PalletNumber,
                ProductPalletOrder = entity.ProductPalletOrder,
                UnitWeight = entity.UnitWeight,
                Code = entity.Code,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = entity.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
            }).Where(new { Id = id }, false, false, "");

            var PackageFichesDto = queryFactory.Update<SelectPackageFichesDto>(query, "Id", true);
            return new SuccessDataResult<SelectPackageFichesDto>(PackageFichesDto);


        }
    }
}
