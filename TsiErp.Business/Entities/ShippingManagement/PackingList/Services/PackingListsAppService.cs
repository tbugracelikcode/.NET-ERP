using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.CurrentAccountCard.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.ShippingManagement.PackingList.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine;
using TsiErp.Entities.Entities.ShippingManagement.PackingList;
using TsiErp.Entities.Entities.ShippingManagement.PackingList.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingList.ReportDtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletCubageLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletLine;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletPackageLine;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletPackageLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecord;
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.ProductGroup;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PackingLists.Page;

namespace TsiErp.Business.Entities.PackingList.Services
{
    [ServiceRegistration(typeof(IPackingListsAppService), DependencyInjectionType.Scoped)]
    public class PackingListsAppService : ApplicationService<PackingListsResource>, IPackingListsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly ICurrentAccountCardsAppService _CurrentAccountCardsAppService;
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public PackingListsAppService(IStringLocalizer<PackingListsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, ICurrentAccountCardsAppService currentAccountCardsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _CurrentAccountCardsAppService = currentAccountCardsAppService;
        }

        [ValidationAspect(typeof(CreatePackingListsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPackingListsDto>> CreateAsync(CreatePackingListsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.PackingLists).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.ControlList<PackingLists>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.PackingLists).Insert(new CreatePackingListsDto
            {

                BillDate = input.BillDate,
                PaymentDate = input.PaymentDate,
                LoadingDate = input.LoadingDate,
                DeliveryDate = input.DeliveryDate,
                SalesType = input.SalesType,
                PackingListState = input.PackingListState,
                TIRType = input.TIRType,
                TransmitterID = input.TransmitterID.GetValueOrDefault(),
                BankID = input.BankID.GetValueOrDefault(),
                BillNo = input.BillNo,
                Code2 = input.Code2,
                CustomsOfficial = input.CustomsOfficial,
                Description_ = input.Description_,
                DriverNameSurname = input.DriverNameSurname,
                DriverPhone = input.DriverPhone,
                LoadingHour = input.LoadingHour,
                OrderNo = input.OrderNo,
                RecieverID = input.RecieverID.GetValueOrDefault(),
                ShippingAddressID = input.ShippingAddressID.GetValueOrDefault(),
                ShippingCompany = input.ShippingCompany,
                VehiclePlateNumber2 = input.VehiclePlateNumber2,
                VehiclePlateNumber1 = input.VehiclePlateNumber1,
                ShippingOfficial = input.ShippingOfficial,
                Code = input.Code,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
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

            foreach (var item in input.SelectPackingListPalletCubageLines)
            {
                var queryLine = queryFactory.Query().From(Tables.PackingListPalletCubageLines).Insert(new CreatePackingListPalletCubageLinesDto
                {
                    Cubage = item.Cubage,
                    NumberofPallet = item.NumberofPallet,
                    Height_ = item.Height_,
                    Load_ = item.Load_,
                    Width_ = item.Width_,
                    PackingListID = addedEntityId,
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
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            foreach (var item in input.SelectPackingListPalletLines)
            {
                var queryLine = queryFactory.Query().From(Tables.PackingListPalletLines).Insert(new CreatePackingListPalletLinesDto
                {

                    FirstPackageNo = item.FirstPackageNo,
                    LastPackageNo = item.LastPackageNo,
                    NumberofPackage = item.NumberofPackage,
                    PalletID = item.PalletID,
                    PackingListID = addedEntityId,
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
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            foreach (var item in input.SelectPackingListPalletPackageLines)
            {
                var queryLine = queryFactory.Query().From(Tables.PackingListPalletPackageLines).Insert(new CreatePackingListPalletPackageLinesDto
                {
                    CustomerID = item.CustomerID.GetValueOrDefault(),
                    ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                    PackageFicheID = item.PackageFicheID,
                    SalesOrderID = item.SalesOrderID.GetValueOrDefault(),
                    SalesOrderLineID = item.SalesOrderLineID.GetValueOrDefault(),
                    OnePackageGrossKG = item.OnePackageGrossKG,
                    OnePackageNetKG = item.OnePackageNetKG,
                    PackageContent = item.PackageContent,
                    PackageNo = item.PackageNo,
                    PackageType = item.PackageType,
                    ProductID = item.ProductID.GetValueOrDefault(),
                    TotalAmount = item.TotalAmount,
                    TotalGrossKG = item.TotalGrossKG,
                    TotalNetKG = item.TotalNetKG,
                    NumberofPackage = item.NumberofPackage,
                    PackingListID = addedEntityId,
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
                    ProductGroupID = item.ProductGroupID
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var PackingList = queryFactory.Insert<SelectPackingListsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("PackingListsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PackingLists, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPackingListsDto>(PackingList);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("PackingListID", new List<string>
            {
                Tables.PalletRecords
            });


            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var deleteQuery = queryFactory.Query().From(Tables.PackingLists).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.PackingListPalletCubageLines).Delete(LoginedUserService.UserId).Where(new { PackingListID = id }, false, false, "");
                var line2DeleteQuery = queryFactory.Query().From(Tables.PackingListPalletLines).Delete(LoginedUserService.UserId).Where(new { PackingListID = id }, false, false, "");
                var line3DeleteQuery = queryFactory.Query().From(Tables.PackingListPalletPackageLines).Delete(LoginedUserService.UserId).Where(new { PackingListID = id }, false, false, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;
                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + line2DeleteQuery.Sql + " where " + line2DeleteQuery.WhereSentence;
                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + line3DeleteQuery.Sql + " where " + line3DeleteQuery.WhereSentence;

                var PackingList = queryFactory.Update<SelectPackingListsDto>(deleteQuery, "Id", true);


                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PackingLists, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectPackingListsDto>(PackingList);
            }
        }

        #region Satır Delete Methodları

        public async Task<IResult> DeleteLineCubageAsync(Guid id)
        {
            var queryLine = queryFactory.Query().From(Tables.PackingListPalletCubageLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
            var PackingListLines = queryFactory.Update<SelectPackingListPalletCubageLinesDto>(queryLine, "Id", true);
            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PackingListPalletCubageLines, LogType.Delete, id);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectPackingListPalletCubageLinesDto>(PackingListLines);

        }

        public async Task<IResult> DeleteLinePalletAsync(Guid id)
        {
            var queryLine = queryFactory.Query().From(Tables.PackingListPalletLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
            var PackingListLines = queryFactory.Update<SelectPackingListPalletLinesDto>(queryLine, "Id", true);
            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PackingListPalletLines, LogType.Delete, id);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectPackingListPalletLinesDto>(PackingListLines);

        }

        public async Task<IResult> DeleteLinePalletPackageAsync(Guid id)
        {
            var queryLine = queryFactory.Query().From(Tables.PackingListPalletPackageLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
            var PackingListLines = queryFactory.Update<SelectPackingListPalletPackageLinesDto>(queryLine, "Id", true);
            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PackingListPalletPackageLines, LogType.Delete, id);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectPackingListPalletPackageLinesDto>(PackingListLines);

        }

        #endregion

        public async Task<IDataResult<SelectPackingListsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.PackingLists)
                   .Select<PackingLists>(null)
                    .Join<CurrentAccountCards>
                    (
                        pr => new { TransmitterCode = pr.Code, TransmitterID = pr.Id, TransmitterName = pr.Name, TransmitterSupplierNo = pr.SupplierNo, TransmitterEORINo = pr.EORINr, TransmitterPaymentTermDay = pr.PaymentTermDay },
                        nameof(PackingLists.TransmitterID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        pr => new { RecieverCode = pr.Code, RecieverID = pr.Id, RecieverName = pr.Name, RecieverCustomerCode = pr.CustomerCode },
                        nameof(PackingLists.RecieverID),
                        nameof(CurrentAccountCards.Id),
                         "Sent",
                        JoinType.Left
                    )
                     .Join<ShippingAdresses>
                    (
                        pr => new { ShippingAddressAddress = pr.Adress1, ShippingAddressID = pr.Id },
                        nameof(PackingLists.ShippingAddressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                      .Join<BankAccounts>
                    (
                        pr => new { BankID = pr.Id, BankName = pr.Name },
                        nameof(PackingLists.BankID),
                        nameof(BankAccounts.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, false, false, Tables.PackingLists);

            var packingLists = queryFactory.Get<SelectPackingListsDto>(query);

            #region Kübaj Satırları

            var queryCubageLines = queryFactory
                   .Query()
                   .From(Tables.PackingListPalletCubageLines)
                   .Select("*")
                    .Where(new { PackingListID = id }, false, false, "");

            var PackingListCubageLine = queryFactory.GetList<SelectPackingListPalletCubageLinesDto>(queryCubageLines).ToList();

            packingLists.SelectPackingListPalletCubageLines = PackingListCubageLine;

            #endregion

            #region Palet Satırları

            var queryPalletLines = queryFactory
                   .Query()
                   .From(Tables.PackingListPalletLines)
                   .Select<PackingListPalletLines>(null)
                    .Join<PalletRecords>
                    (
                        pr => new { PalletName = pr.Name, PalletID = pr.Id },
                        nameof(PackingListPalletLines.PalletID),
                        nameof(PalletRecords.Id),
                        JoinType.Left
                    )
                    .Where(new { PackingListID = id }, false, false, Tables.PackingListPalletLines);

            var PackingListPalletLine = queryFactory.GetList<SelectPackingListPalletLinesDto>(queryPalletLines).ToList();

            packingLists.SelectPackingListPalletLines = PackingListPalletLine;

            #endregion

            #region Palet Paket Satırları

            var queryPalletPackageLines = queryFactory
                   .Query()
                   .From(Tables.PackingListPalletPackageLines)
                   .Select<PackingListPalletPackageLines>(null)
                    .Join<Products>
                    (
                        pr => new { ProductName = pr.Name, ProductID = pr.Id, ProductCode = pr.Code, ProductEnglishDefinition = pr.EnglishDefinition },
                        nameof(PackingListPalletPackageLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<ProductGroups>
                    (
                        pr => new { ProductGroupID = pr.Id, ProductGroupName = pr.Name },
                        nameof(PackingListPalletPackageLines.ProductGroupID),
                        nameof(ProductGroups.Id),
                        JoinType.Left
                    )
                    .Join<SalesOrderLines>
                    (
                        pr => new { TransactionExchangeUnitPrice = pr.TransactionExchangeUnitPrice },
                        nameof(PackingListPalletPackageLines.SalesOrderLineID),
                        nameof(SalesOrderLines.Id),
                        JoinType.Left
                    )
                    .Join<SalesOrders>
                    (
                        pr => new { SalesOrderFicheNo = pr.FicheNo },
                        nameof(PackingListPalletPackageLines.SalesOrderID),
                        nameof(SalesOrders.Id),
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        pr => new { CustomerID = pr.Id, CustomerCode = pr.CustomerCode },
                        nameof(PackingListPalletPackageLines.CustomerID),
                        nameof(CurrentAccountCards.Id),
                         "PalletPackageCustomer",
                        JoinType.Left
                    )
                    .Where(new { PackingListID = id }, false, false, Tables.PackingListPalletPackageLines);

            var PackingListPalletPackageLine = queryFactory.GetList<SelectPackingListPalletPackageLinesDto>(queryPalletPackageLines).ToList();

            packingLists.SelectPackingListPalletPackageLines = PackingListPalletPackageLine;

            #endregion

            LogsAppService.InsertLogToDatabase(packingLists, packingLists, LoginedUserService.UserId, Tables.PackingLists, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPackingListsDto>(packingLists);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPackingListsDto>>> GetListAsync(ListPackingListsParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.PackingLists)
                   .Select<PackingLists>(null)
                    .Join<CurrentAccountCards>
                    (
                        pr => new { TransmitterCode = pr.Code, TransmitterID = pr.Id, TransmitterName = pr.Name, TransmitterSupplierNo = pr.SupplierNo, TransmitterEORINo = pr.EORINr, TransmitterPaymentTermDay = pr.PaymentTermDay },
                        nameof(PackingLists.TransmitterID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        pr => new { RecieverCode = pr.Code, RecieverID = pr.Id, RecieverName = pr.Name, RecieverCustomerCode = pr.CustomerCode },
                        nameof(PackingLists.RecieverID),
                        nameof(CurrentAccountCards.Id),
                         "Sent",
                        JoinType.Left
                    )
                       .Join<BankAccounts>
                    (
                        pr => new { BankID = pr.Id, BankName = pr.Name },
                        nameof(PackingLists.BankID),
                        nameof(BankAccounts.Id),
                        JoinType.Left
                    )
                     .Join<ShippingAdresses>
                    (
                        pr => new { ShippingAddressAddress = pr.Adress1, ShippingAddressID = pr.Id },
                        nameof(PackingLists.ShippingAddressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(null, false, false, Tables.PackingLists);

            var packingLists = queryFactory.GetList<ListPackingListsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListPackingListsDto>>(packingLists);

        }

        public async Task<IDataResult<IList<SelectPackingListPalletPackageLinesDto>>> GetLinePalletPackageListAsync()
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.PackingListPalletPackageLines)
                   .Select<PackingListPalletPackageLines>(null)
                    .Join<Products>
                    (
                        pr => new { ProductName = pr.Name, ProductID = pr.Id, ProductCode = pr.Code },
                        nameof(PackingListPalletPackageLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        pr => new { CustomerID = pr.Id, CustomerCode = pr.CustomerCode },
                        nameof(PackingListPalletPackageLines.CustomerID),
                        nameof(CurrentAccountCards.Id),
                         "PalletPackageCustomer",
                        JoinType.Left
                    )
                    .Where(null, false, false, Tables.PackingListPalletPackageLines);

            var packingListPalletPackageLines = queryFactory.GetList<SelectPackingListPalletPackageLinesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<SelectPackingListPalletPackageLinesDto>>(packingListPalletPackageLines);

        }

        [ValidationAspect(typeof(UpdatePackingListsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPackingListsDto>> UpdateAsync(UpdatePackingListsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.PackingLists)
                   .Select<PackingLists>(null)
                    .Join<CurrentAccountCards>
                    (
                        pr => new { TransmitterCode = pr.Code, TransmitterID = pr.Id, TransmitterName = pr.Name, TransmitterSupplierNo = pr.SupplierNo, TransmitterEORINo = pr.EORINr },
                        nameof(PackingLists.TransmitterID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        pr => new { RecieverCode = pr.Code, RecieverID = pr.Id, RecieverName = pr.Name, RecieverCustomerCode = pr.CustomerCode },
                        nameof(PackingLists.RecieverID),
                        nameof(CurrentAccountCards.Id),
                         "Sent",
                        JoinType.Left
                    )
                       .Join<BankAccounts>
                    (
                        pr => new { BankID = pr.Id, BankName = pr.Name },
                        nameof(PackingLists.BankID),
                        nameof(BankAccounts.Id),
                        JoinType.Left
                    )
                     .Join<ShippingAdresses>
                    (
                        pr => new { ShippingAddressAddress = pr.Adress1, ShippingAddressID = pr.Id },
                        nameof(PackingLists.ShippingAddressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, false, false, Tables.PackingLists);

            var entity = queryFactory.Get<SelectPackingListsDto>(entityQuery);

            #region Satır Get İşlemleri

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PackingListPalletCubageLines)
                  .Select("*")
                    .Where(new { PackingListID = input.Id }, false, false, "");

            var PackingListLine = queryFactory.GetList<SelectPackingListPalletCubageLinesDto>(queryLines).ToList();

            entity.SelectPackingListPalletCubageLines = PackingListLine;

            var queryLines2 = queryFactory
                  .Query()
                 .From(Tables.PackingListPalletLines)
                   .Select<PackingListPalletLines>(null)
                    .Join<PalletRecords>
                    (
                        pr => new { PalletName = pr.Name, PalletID = pr.Id },
                        nameof(PackingListPalletLines.PalletID),
                        nameof(PalletRecords.Id),
                        JoinType.Left
                    )
                    .Where(new { PackingListID = input.Id }, false, false, Tables.PackingListPalletLines);

            var PackingListLine2 = queryFactory.GetList<SelectPackingListPalletLinesDto>(queryLines2).ToList();

            entity.SelectPackingListPalletLines = PackingListLine2;

            var queryLines3 = queryFactory
                .Query()
               .From(Tables.PackingListPalletPackageLines)
                   .Select<PackingListPalletPackageLines>(null)
                    .Join<Products>
                    (
                        pr => new { ProductName = pr.Name, ProductID = pr.Id, ProductCode = pr.Code },
                        nameof(PackingListPalletPackageLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        pr => new { CustomerID = pr.Id, CustomerCode = pr.CustomerCode },
                        nameof(PackingListPalletPackageLines.CustomerID),
                        nameof(CurrentAccountCards.Id),
                         "PalletPackageCustomer",
                        JoinType.Left
                    )
                    .Where(new { PackingListID = input.Id }, false, false, Tables.PackingListPalletPackageLines);

            var PackingListLine3 = queryFactory.GetList<SelectPackingListPalletPackageLinesDto>(queryLines3).ToList();

            entity.SelectPackingListPalletPackageLines = PackingListLine3;

            #endregion

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.PackingLists)
                   .Select<PackingLists>(null)
                     .Join<CurrentAccountCards>
                    (
                        pr => new { TransmitterCode = pr.Code, TransmitterID = pr.Id, TransmitterName = pr.Name, TransmitterSupplierNo = pr.SupplierNo, TransmitterEORINo = pr.EORINr },
                        nameof(PackingLists.TransmitterID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        pr => new { RecieverCode = pr.Code, RecieverID = pr.Id, RecieverName = pr.Name, RecieverCustomerCode = pr.CustomerCode },
                        nameof(PackingLists.RecieverID),
                        nameof(CurrentAccountCards.Id),
                         "Sent",
                        JoinType.Left
                    )
                       .Join<BankAccounts>
                    (
                        pr => new { BankID = pr.Id, BankName = pr.Name },
                        nameof(PackingLists.BankID),
                        nameof(BankAccounts.Id),
                        JoinType.Left
                    )
                     .Join<ShippingAdresses>
                    (
                        pr => new { ShippingAddressAddress = pr.Adress1, ShippingAddressID = pr.Id },
                        nameof(PackingLists.ShippingAddressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                            .Where(new { Code = input.Code }, false, false, Tables.PackingLists);

            var list = queryFactory.GetList<ListPackingListsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            var query = queryFactory.Query().From(Tables.PackingLists).Update(new UpdatePackingListsDto
            {
                BillDate = input.BillDate,
                PaymentDate = input.PaymentDate,
                LoadingDate = input.LoadingDate,
                DeliveryDate = input.DeliveryDate,
                SalesType = input.SalesType,
                PackingListState = input.PackingListState,
                TIRType = input.TIRType,
                TransmitterID = input.TransmitterID.GetValueOrDefault(),
                BankID = input.BankID.GetValueOrDefault(),
                BillNo = input.BillNo,
                Code2 = input.Code2,
                CustomsOfficial = input.CustomsOfficial,
                Description_ = input.Description_,
                DriverNameSurname = input.DriverNameSurname,
                DriverPhone = input.DriverPhone,
                LoadingHour = input.LoadingHour,
                OrderNo = input.OrderNo,
                RecieverID = input.RecieverID.GetValueOrDefault(),
                ShippingAddressID = input.ShippingAddressID.GetValueOrDefault(),
                ShippingCompany = input.ShippingCompany,
                VehiclePlateNumber2 = input.VehiclePlateNumber2,
                VehiclePlateNumber1 = input.VehiclePlateNumber1,
                ShippingOfficial = input.ShippingOfficial,
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id }, false, false, "");

            #region Satır Insert - Update İşlemleri

            foreach (var item in input.SelectPackingListPalletCubageLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PackingListPalletCubageLines).Insert(new CreatePackingListPalletCubageLinesDto
                    {
                        PackingListID = input.Id,
                        Cubage = item.Cubage,
                        Height_ = item.Height_,
                        NumberofPallet = item.NumberofPallet,
                        Load_ = item.Load_,
                        Width_ = item.Width_,
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
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.PackingListPalletCubageLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                    var line = queryFactory.Get<SelectPackingListPalletCubageLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PackingListPalletCubageLines).Update(new UpdatePackingListPalletCubageLinesDto
                        {
                            PackingListID = input.Id,
                            Cubage = item.Cubage,
                            NumberofPallet = item.NumberofPallet,
                            Height_ = item.Height_,
                            Load_ = item.Load_,
                            Width_ = item.Width_,
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
                        }).Where(new { Id = line.Id }, false, false, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            foreach (var item in input.SelectPackingListPalletLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PackingListPalletLines).Insert(new CreatePackingListPalletLinesDto
                    {
                        PackingListID = input.Id,
                        FirstPackageNo = item.LastPackageNo,
                        LastPackageNo = item.LastPackageNo,
                        NumberofPackage = item.NumberofPackage,
                        PalletID = item.PalletID.GetValueOrDefault(),
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
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.PackingListPalletLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                    var line = queryFactory.Get<SelectPackingListPalletLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PackingListPalletLines).Update(new UpdatePackingListPalletLinesDto
                        {
                            PackingListID = input.Id,
                            FirstPackageNo = item.LastPackageNo,
                            LastPackageNo = item.LastPackageNo,
                            NumberofPackage = item.NumberofPackage,
                            PalletID = item.PalletID.GetValueOrDefault(),
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
                        }).Where(new { Id = line.Id }, false, false, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            foreach (var item in input.SelectPackingListPalletPackageLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PackingListPalletPackageLines).Insert(new CreatePackingListPalletPackageLinesDto
                    {
                        PackingListID = input.Id,
                        ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                        SalesOrderID = item.SalesOrderID.GetValueOrDefault(),
                        PackageFicheID = item.PackageFicheID,
                        SalesOrderLineID = item.SalesOrderLineID.GetValueOrDefault(),
                        NumberofPackage = item.NumberofPackage,
                        CustomerID = item.CustomerID.GetValueOrDefault(),
                        OnePackageGrossKG = item.OnePackageGrossKG,
                        OnePackageNetKG = item.OnePackageNetKG,
                        PackageContent = item.PackageContent,
                        PackageNo = item.PackageNo,
                        PackageType = item.PackageType,
                        ProductID = item.ProductID.GetValueOrDefault(),
                        TotalAmount = item.TotalAmount,
                        TotalGrossKG = item.TotalGrossKG,
                        TotalNetKG = item.TotalNetKG,
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
                        ProductGroupID = item.ProductGroupID
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.PackingListPalletPackageLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                    var line = queryFactory.Get<SelectPackingListPalletPackageLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PackingListPalletPackageLines).Update(new UpdatePackingListPalletPackageLinesDto
                        {
                            PackingListID = input.Id,
                            ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                            SalesOrderID = item.SalesOrderID.GetValueOrDefault(),
                            PackageFicheID = item.PackageFicheID,
                            SalesOrderLineID = item.SalesOrderLineID.GetValueOrDefault(),
                            NumberofPackage = item.NumberofPackage,
                            CustomerID = item.CustomerID.GetValueOrDefault(),
                            OnePackageGrossKG = item.OnePackageGrossKG,
                            OnePackageNetKG = item.OnePackageNetKG,
                            PackageContent = item.PackageContent,
                            PackageNo = item.PackageNo,
                            PackageType = item.PackageType,
                            ProductID = item.ProductID.GetValueOrDefault(),
                            TotalAmount = item.TotalAmount,
                            TotalGrossKG = item.TotalGrossKG,
                            TotalNetKG = item.TotalNetKG,
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
                            ProductGroupID = item.ProductGroupID
                        }).Where(new { Id = line.Id }, false, false, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            #endregion

            var billOfMaterial = queryFactory.Update<SelectPackingListsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PackingLists, LogType.Update, billOfMaterial.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPackingListsDto>(billOfMaterial);

        }

        public async Task<IDataResult<SelectPackingListsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.PackingLists).Select("*").Where(new { Id = id }, false, false, "");

            var entity = queryFactory.Get<PackingLists>(entityQuery);

            var query = queryFactory.Query().From(Tables.PackingLists).Update(new UpdatePackingListsDto
            {
                BillDate = entity.BillDate,
                PaymentDate = entity.PaymentDate,
                LoadingDate = entity.LoadingDate,
                DeliveryDate = entity.DeliveryDate,
                SalesType = (int)entity.SalesType,
                PackingListState = (int)entity.PackingListState,
                TIRType = (int)entity.TIRType,
                TransmitterID = entity.TransmitterID,
                BankID = entity.BankID,
                BillNo = entity.BillNo,
                Code2 = entity.Code2,
                CustomsOfficial = entity.CustomsOfficial,
                Description_ = entity.Description_,
                DriverNameSurname = entity.DriverNameSurname,
                DriverPhone = entity.DriverPhone,
                LoadingHour = entity.LoadingHour,
                OrderNo = entity.OrderNo,
                RecieverID = entity.RecieverID,
                ShippingAddressID = entity.ShippingAddressID,
                ShippingCompany = entity.ShippingCompany,
                VehiclePlateNumber2 = entity.VehiclePlateNumber2,
                VehiclePlateNumber1 = entity.VehiclePlateNumber1,
                ShippingOfficial = entity.ShippingOfficial,
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

            var PackingListsDto = queryFactory.Update<SelectPackingListsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectPackingListsDto>(PackingListsDto);


        }

        public async Task<IDataResult<IList<SelectPackingListPalletPackageLinesDto>>> GetPackingListLineByPackageId(Guid PackageFicheId)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.PackingListPalletPackageLines)
                   .Select<PackingListPalletPackageLines>(null)
                    .Join<Products>
                    (
                        pr => new { ProductName = pr.Name, ProductID = pr.Id, ProductCode = pr.Code },
                        nameof(PackingListPalletPackageLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        pr => new { CustomerID = pr.Id, CustomerCode = pr.CustomerCode },
                        nameof(PackingListPalletPackageLines.CustomerID),
                        nameof(CurrentAccountCards.Id),
                         "PalletPackageCustomer",
                        JoinType.Left
                    )
                    .Where(new { PackageFicheID = PackageFicheId }, false, false, Tables.PackingListPalletPackageLines);

            var packingListPalletPackageLines = queryFactory.GetList<SelectPackingListPalletPackageLinesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<SelectPackingListPalletPackageLinesDto>>(packingListPalletPackageLines);
        }

        public async Task<List<CommercialInvoiceDto>> GetCommercialInvoiceReportDataSource(SelectPackingListsDto packingList)
        {
            List<CommercialInvoiceDto> reportSource = new List<CommercialInvoiceDto>();


            Guid sentCompanyId = packingList.TransmitterID.GetValueOrDefault();
            var sentCurrentAccountCard = (await _CurrentAccountCardsAppService.GetAsync(sentCompanyId)).Data;
            int odemeVadeGun = sentCurrentAccountCard.PaymentTermDay;

            Guid recieverCompanyId = packingList.RecieverID.GetValueOrDefault();
            var recieverCurrentAccountCard = (await _CurrentAccountCardsAppService.GetAsync(recieverCompanyId)).Data;
            string shippingAddress = recieverCurrentAccountCard.ShippingAddress;
            int gonderilenOdemeVadeGun = recieverCurrentAccountCard.PaymentTermDay;

            string satisSekli = packingList.SalesTypeName;
            if (satisSekli == null)
            {
                satisSekli = "";
            }

            for (int i = 0; i < packingList.SelectPackingListPalletPackageLines.Count; i++)
            {
                var line = packingList.SelectPackingListPalletPackageLines[i];
                Guid siparisId = line.SalesOrderID.GetValueOrDefault();


                string sipNo = line.SalesOrderFicheNo;
                string stokkodu = line.ProductCode;
                string varyantKodu = "049";
                string cekiStokAciklama = line.ProductEnglishDefinition;

                CommercialInvoiceDto c = new CommercialInvoiceDto();

                c.SiparisId = siparisId;
                c.CekiListesiNo = packingList.Code;
                c.CariUnvan = sentCurrentAccountCard.Name ;
                c.Adres1 = sentCurrentAccountCard.Address1;
                c.Adres2 = sentCurrentAccountCard.Address2;
                c.Tel1 = sentCurrentAccountCard.Tel1;
                c.Faks = sentCurrentAccountCard.Fax;
                c.EoriNr = sentCurrentAccountCard.EORINr;
                c.FaturaNo = packingList.BillNo ;
                c.FaturaTarhi = packingList.BillDate.Value;
                c.Adet = line.TotalAmount;
                //c.StokAciklamasi = cekiStokAciklama;
                c.BirimFiyat = line.TransactionExchangeUnitPrice ;
                c.ToplamTutar = c.Adet * c.BirimFiyat;
                c.BagliSiparisNo = line.SalesOrderFicheNo;
                c.StokKodu = stokkodu;
                c.OdemeTarihi = c.FaturaTarhi.AddDays(odemeVadeGun);
                c.TeslimTarihi = packingList.DeliveryDate.GetValueOrDefault();
                c.NakliyeFirmasi = sentCurrentAccountCard.ShippingCompany;
                c.VaryantKodu = varyantKodu;
                c.SatisSekli = satisSekli;

                if (c.SatisSekli == "EX-WORKS ISTANBUL (€)")
                {
                    c.SevkiyatAdresi = shippingAddress;
                }

                if (!reportSource.Any(t => t.StokKodu == stokkodu && t.VaryantKodu == varyantKodu && t.SiparisId == siparisId))
                {
                    reportSource.Add(c);
                }
                else
                {
                    var satir = reportSource.Find(t => t.StokKodu == stokkodu && t.VaryantKodu == varyantKodu && t.SiparisId == siparisId);

                    satir.Adet += c.Adet;
                    satir.ToplamTutar = satir.Adet * c.BirimFiyat;

                }
            }


            await Task.CompletedTask;
            return reportSource;
        }

        public string YaziyaCevir(decimal tutar)
        {
            string yazi = "";

            string[] alanlar = tutar.ToString().Split(',');

            Int64 tamKisim = 0;
            Int16 ondalik = 0;
            tamKisim = Convert.ToInt64(alanlar[0]); // tam kısmını aldım
            try
            {
                ondalik = Convert.ToInt16(alanlar[1].Substring(0, 2)); // ondalık kısmın ilk2 2 hanesini aldım
            }
            catch { }

            string[] birlik = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
            string[] Onluk = { "", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
            string[] Yuzluk = { "", "OneHundredAnd", "TwoHundredand", "ThreeHundredAnd", "FourHundredAnd", "FiveHundredAnd", "SixHundredAnd", "SevenHundredAnd", "EigthHundredAnd", "NineHundredAnd" };

            string tamKisimYazi = tamKisim.ToString();
            // 12 hane yaptık
            while (tamKisimYazi.Length < 15)
            {
                tamKisimYazi = "0" + tamKisimYazi;
            }

            string trilyonlar = tamKisimYazi.Substring(0, 3);

            if (Convert.ToInt16(trilyonlar) > 0)
            {
                // trilyonlar hanesi var..
                yazi = yazi + Yuzluk[Convert.ToInt16(trilyonlar.Substring(0, 1))].ToString();
                yazi = yazi + Onluk[Convert.ToInt16(trilyonlar.Substring(1, 1))].ToString();
                yazi = yazi + birlik[Convert.ToInt16(trilyonlar.Substring(2, 1))].ToString();
                yazi = yazi + "Trillion";
            }

            string milyarlar = tamKisimYazi.Substring(3, 3);

            if (Convert.ToInt16(milyarlar) > 0)
            {
                // milyar hanesi var..

                yazi = yazi + Yuzluk[Convert.ToInt16(milyarlar.Substring(0, 1))].ToString();
                yazi = yazi + Onluk[Convert.ToInt16(milyarlar.Substring(1, 1))].ToString();
                yazi = yazi + birlik[Convert.ToInt16(milyarlar.Substring(2, 1))].ToString();
                yazi = yazi + "Billion";
            }

            string milyonlar = tamKisimYazi.Substring(6, 3);

            if (Convert.ToInt16(milyonlar) > 0)
            {
                // milyonlar hanesi var..
                yazi = yazi + Yuzluk[Convert.ToInt16(milyonlar.Substring(0, 1))].ToString();
                yazi = yazi + Onluk[Convert.ToInt16(milyonlar.Substring(1, 1))].ToString();
                yazi = yazi + birlik[Convert.ToInt16(milyonlar.Substring(2, 1))].ToString();
                yazi = yazi + "Million";
            }

            string binler = tamKisimYazi.Substring(9, 3);

            if (Convert.ToInt16(binler) > 0)
            {
                // binler hanesi var..
                if (Convert.ToInt16(binler) > 1)
                {
                    // 1 den büüyk değil 1 e eşit ise sadece bin yazacağı için burası atlandı
                    yazi = yazi + Yuzluk[Convert.ToInt16(binler.Substring(0, 1))].ToString();
                    yazi = yazi + Onluk[Convert.ToInt16(binler.Substring(1, 1))].ToString();
                    yazi = yazi + birlik[Convert.ToInt16(binler.Substring(2, 1))].ToString();
                }
                yazi = yazi + "Thousand";
            }

            string birler = tamKisimYazi.Substring(12, 3);

            if (Convert.ToInt16(birler) > 0)
            {
                // birler hanesi var..
                yazi = yazi + Yuzluk[Convert.ToInt16(birler.Substring(0, 1))].ToString();
                yazi = yazi + Onluk[Convert.ToInt16(birler.Substring(1, 1))].ToString();
                yazi = yazi + birlik[Convert.ToInt16(birler.Substring(2, 1))].ToString();
            }

            yazi = yazi + " EURO AND ";

            // ondalık işlemleri

            if (ondalik > 0 && ondalik < 10)
            {
                // odalık hanesi var..
                //yazi = yazi + Onluk[Convert.ToInt16(ondalik.ToString().Substring(0, 1))].ToString();
                yazi = yazi + birlik[Convert.ToInt16(ondalik.ToString().Substring(1, 1))].ToString();
                yazi = yazi + " CENT";
            }
            else
            {
                // odalık hanesi var..
                yazi = yazi + Onluk[Convert.ToInt16(ondalik.ToString().Substring(0, 1))].ToString();
                yazi = yazi + birlik[Convert.ToInt16(ondalik.ToString().Substring(1, 1))].ToString();
                yazi = yazi + " CENT";
            }

            return yazi;
        }

        public string SayiOku(int tutar)
        {
            int sayi, birler, onlar, yuzler, binler, onbinler, yuzBinler;
            sayi = tutar;
            birler = sayi % 10;
            onlar = (sayi / 10) % 10;
            yuzler = (sayi / 100) % 10;
            binler = (sayi / 1000) % 10;
            onbinler = (sayi / 10000) % 10;
            yuzBinler = (sayi / 100000) % 10;

            string[] birlik = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
            string[] Onluk = { "", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
            string[] Yuzluk = { "", "OneHundredAnd", "TwoHundredand", "ThreeHundredAnd", "FourHundredAnd", "FiveHundredAnd", "SixHundredAnd", "SevenHundredAnd", "EigthHundredAnd", "NineHundredAnd" };
            string[] binlik = { "", "Thousand", "TwoThousand", "ThreeThousand", "FourThousand", "FiveThousand", "SixThousand", "SevenThousand", "EightThousand", "NineThousand" };
            string[] onbinlik = { "", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
            string[] yuzBinlik = { "", "OneHundredAnd", "TwoHundredAnd", "ThreeHundredAnd", "FourHundredAnd", "FiveHundredAnd", "SixHundredAnd", "SevenHundredAnd", "EigthHundredAnd", "NineHundredAnd" };

            return yuzBinlik[yuzBinler] + onbinlik[onbinler] + binlik[binler] + Yuzluk[yuzler] + Onluk[onlar] + birlik[birler];
        }

        public string SayiyiOku(string sayi)
        {
            string[] tlVeKurus = sayi.Split(',');
            string tl = tlVeKurus[0], kurus = tlVeKurus[1];
            sayi = SayiOku(int.Parse(tl)) + " EURO AND " + SayiOku(int.Parse(kurus.Substring(0, 2))) + " CENT";
            return sayi;
        }

        public string NumberToWords(double doubleNumber)
        {
            var beforeFloatingPoint = (int)Math.Floor(doubleNumber);
            var beforeFloatingPointWord = NumberToWords(beforeFloatingPoint) + " EURO";

            var afterFloatingPointWord = SmallNumberToWord((double)((doubleNumber - beforeFloatingPoint) * 100), "") + " CENT";

            return beforeFloatingPointWord + " AND " + afterFloatingPointWord;
        }

        public string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            var words = "";

            if (number / 1000000000 > 0)
            {
                words += NumberToWords(number / 1000000000) + " Billion ";
                number %= 1000000000;
            }

            if (number / 1000000 > 0)
            {
                words += NumberToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if (number / 1000 > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if (number / 100 > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            words = SmallNumberToWord(number, words);

            return words;
        }

        public string SmallNumberToWord(double number, string words)
        {
            //number = Math.Ceiling(number);
            number = Convert.ToDouble(number.ToString("0.##"));
            if (number <= 0) return words;
            if (words != "")
                words += "";

            var unitsMap = new[] { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            if (number < 20)
                words += unitsMap[(int)number];
            else
            {
                words += tensMap[(int)number / 10];
                if ((number % 10) > 0)
                    words += unitsMap[(int)number % 10];
            }
            return words;
        }

        public string NumberToWordsTR(decimal tutar)
        {
            string sTutar = tutar.ToString("F2").Replace('.', ',');
            string lira = sTutar.Substring(0, sTutar.IndexOf(','));
            string kurus = sTutar.Substring(sTutar.IndexOf(',') + 1, 2);
            string yazi = "";
            string[] birler = { "", "BİR", "İKİ", "Üç", "DÖRT", "BEŞ", "ALTI", "YEDİ", "SEKİZ", "DOKUZ" };
            string[] onlar = { "", "ON", "YİRMİ", "OTUZ", "KIRK", "ELLİ", "ALTMIŞ", "YETMİŞ", "SEKSEN", "DOKSAN" };
            string[] binler = { "KATRİLYON", "TRİLYON", "MİLYAR", "MİLYON", "BİN", "" };
            int grupSayisi = 6;
            lira = lira.PadLeft(grupSayisi * 3, '0');
            string grupDegeri;
            for (int i = 0; i < grupSayisi * 3; i += 3)
            {
                grupDegeri = "";
                if (lira.Substring(i, 1) != "0")
                    grupDegeri += birler[Convert.ToInt32(lira.Substring(i, 1))] + "YÜZ";
                if (grupDegeri == "BİRYÜZ")
                    grupDegeri = "YÜZ";
                grupDegeri += onlar[Convert.ToInt32(lira.Substring(i + 1, 1))];
                grupDegeri += birler[Convert.ToInt32(lira.Substring(i + 2, 1))];
                if (grupDegeri != "")
                    grupDegeri += binler[i / 3];
                if (grupDegeri == "BİRBİN")
                    grupDegeri = "BİN";
                yazi += grupDegeri;
            }
            if (yazi != "")
                yazi += " EURO ";
            int yaziUzunlugu = yazi.Length;
            if (kurus.Substring(0, 1) != "0")
                yazi += onlar[Convert.ToInt32(kurus.Substring(0, 1))];
            if (kurus.Substring(1, 1) != "0")
                yazi += birler[Convert.ToInt32(kurus.Substring(1, 1))];
            if (yazi.Length > yaziUzunlugu)
                yazi += " CENT";
            else
                yazi += "SIFIR CENT";
            return yazi;
        }
    }
}
