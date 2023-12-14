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
using TsiErp.Business.Entities.ShippingManagement.PackingList.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFiche.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheAmountEntryLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder;
using TsiErp.Entities.Entities.ShippingManagement.PackingList;
using TsiErp.Entities.Entities.ShippingManagement.PackingList.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletCubageLine;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletCubageLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletLine;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletPackageLine;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletPackageLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecord;
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PackingLists.Page;

namespace TsiErp.Business.Entities.PackingList.Services
{
    [ServiceRegistration(typeof(IPackingListsAppService), DependencyInjectionType.Scoped)]
    public class PackingListsAppService : ApplicationService<PackingListsResource>, IPackingListsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }

        public PackingListsAppService(IStringLocalizer<PackingListsResource> l, IFicheNumbersAppService ficheNumbersAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
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
                TransmitterID = input.TransmitterID,
                BankID = input.BankID.GetValueOrDefault(),
                BillNo = input.BillNo,
                Code2 = input.Code2,
                CustomsOfficial = input.CustomsOfficial,
                Description_ = input.Description_,
                DriverNameSurname = input.DriverNameSurname,
                DriverPhone = input.DriverPhone,
                LoadingHour = input.LoadingHour,
                OrderNo = input.OrderNo,
                RecieverID = input.RecieverID,
                ShippingAddressID = input.ShippingAddressID.GetValueOrDefault(),
                ShippingCompany = input.ShippingCompany,
                VehiclePlateNumber2 = input.VehiclePlateNumber2,
                VehiclePlateNumber1 = input.VehiclePlateNumber1,
                ShippingOfficial = input.ShippingOfficial,
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
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            foreach (var item in input.SelectPackingListPalletPackageLines)
            {
                var queryLine = queryFactory.Query().From(Tables.PackingListPalletPackageLines).Insert(new CreatePackingListPalletPackageLinesDto
                {
                    CustomerID = item.CustomerID,
                    OnePackageGrossKG = item.OnePackageGrossKG,
                    OnePackageNetKG = item.OnePackageNetKG,
                    PackageContent = item.PackageContent,
                    PackageNo = item.PackageNo,
                    PackageType = item.PackageType,
                    ProductID = item.ProductID,
                    TotalAmount = item.TotalAmount,
                    TotalGrossKG = item.TotalGrossKG,
                    TotalNetKG = item.TotalNetKG,
                    NumberofPackage = item.NumberofPackage,
                    PackingListID = addedEntityId,
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
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var PackingList = queryFactory.Insert<SelectPackingListsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("PackingListsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PackingLists, LogType.Insert, addedEntityId);

            return new SuccessDataResult<SelectPackingListsDto>(PackingList);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.PackingLists).Select("*").Where(new { Id = id }, false, false, "");

            var PackingLists = queryFactory.Get<SelectPackingListsDto>(query);


            var deleteQuery = queryFactory.Query().From(Tables.PackingLists).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

            var lineDeleteQuery = queryFactory.Query().From(Tables.PackingListPalletCubageLines).Delete(LoginedUserService.UserId).Where(new { PackingListID = id }, false, false, "");
            var line2DeleteQuery = queryFactory.Query().From(Tables.PackingListPalletLines).Delete(LoginedUserService.UserId).Where(new { PackingListID = id }, false, false, "");
            var line3DeleteQuery = queryFactory.Query().From(Tables.PackingListPalletPackageLines).Delete(LoginedUserService.UserId).Where(new { PackingListID = id }, false, false, "");

            deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

            var PackingList = queryFactory.Update<SelectPackingListsDto>(deleteQuery, "Id", true);
            var SelectPackingListsDtoLine = queryFactory.Update<SelectPackingListPalletCubageLinesDto>(lineDeleteQuery, "Id", true);
            var SelectPackingListsDtoLine2 = queryFactory.Update<SelectPackingListPalletLinesDto>(line2DeleteQuery, "Id", true);
            var SelectPackingListsDtoLine3 = queryFactory.Update<SelectPackingListPalletPackageLinesDto>(line3DeleteQuery, "Id", true);
            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PackingLists, LogType.Delete, id);
            return new SuccessDataResult<SelectPackingListsDto>(PackingList);

        }

        #region Satır Delete Methodları

        public async Task<IResult> DeleteLineCubageAsync(Guid id)
        {
            var queryLine = queryFactory.Query().From(Tables.PackingListPalletCubageLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
            var PackingListLines = queryFactory.Update<SelectPackingListPalletCubageLinesDto>(queryLine, "Id", true);
            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PackingListPalletCubageLines, LogType.Delete, id);
            return new SuccessDataResult<SelectPackingListPalletCubageLinesDto>(PackingListLines);

        }

        public async Task<IResult> DeleteLinePalletAsync(Guid id)
        {
            var queryLine = queryFactory.Query().From(Tables.PackingListPalletLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
            var PackingListLines = queryFactory.Update<SelectPackingListPalletLinesDto>(queryLine, "Id", true);
            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PackingListPalletLines, LogType.Delete, id);
            return new SuccessDataResult<SelectPackingListPalletLinesDto>(PackingListLines);

        }

        public async Task<IResult> DeleteLinePalletPackageAsync(Guid id)
        {
            var queryLine = queryFactory.Query().From(Tables.PackingListPalletPackageLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
            var PackingListLines = queryFactory.Update<SelectPackingListPalletPackageLinesDto>(queryLine, "Id", true);
            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PackingListPalletPackageLines, LogType.Delete, id);
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
                     .Join<ShippingAdresses>
                    (
                        pr => new { ShippingAddressAddress = pr.Adress1, ShippingAddressID = pr.Id },
                        nameof(PackingLists.ShippingAddressID),
                        nameof(ShippingAdresses.Id),
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
                    .Where(new { PackingListID = id }, false, false, Tables.PackingListPalletPackageLines);

            var PackingListPalletPackageLine = queryFactory.GetList<SelectPackingListPalletPackageLinesDto>(queryPalletPackageLines).ToList();

            packingLists.SelectPackingListPalletPackageLines = PackingListPalletPackageLine;

            #endregion

            LogsAppService.InsertLogToDatabase(packingLists, packingLists, LoginedUserService.UserId, Tables.PackingLists, LogType.Get, id);

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
                     .Join<ShippingAdresses>
                    (
                        pr => new { ShippingAddressAddress = pr.Adress1, ShippingAddressID = pr.Id },
                        nameof(PackingLists.ShippingAddressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(null, false, false, Tables.PackingLists);

            var packingLists = queryFactory.GetList<ListPackingListsDto>(query).ToList();
            return new SuccessDataResult<IList<ListPackingListsDto>>(packingLists);

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

            var PackingListLine2 = queryFactory.GetList<SelectPackingListPalletLinesDto>(queryLines).ToList();

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
                TransmitterID = input.TransmitterID,
                BankID = input.BankID,
                BillNo = input.BillNo,
                Code2 = input.Code2,
                CustomsOfficial = input.CustomsOfficial,
                Description_ = input.Description_,
                DriverNameSurname = input.DriverNameSurname,
                DriverPhone = input.DriverPhone,
                LoadingHour = input.LoadingHour,
                OrderNo = input.OrderNo,
                RecieverID = input.RecieverID,
                ShippingAddressID = input.ShippingAddressID,
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
                LastModificationTime = DateTime.Now,
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
                            LastModificationTime = DateTime.Now,
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
                        PalletID = item.PalletID,
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
                            PalletID = item.PalletID,
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
                        NumberofPackage = item.NumberofPackage,
                        CustomerID = item.CustomerID,
                        OnePackageGrossKG = item.OnePackageGrossKG,
                        OnePackageNetKG = item.OnePackageNetKG,
                        PackageContent = item.PackageContent,
                        PackageNo = item.PackageNo,
                        PackageType = item.PackageType,
                        ProductID = item.ProductID,
                        TotalAmount = item.TotalAmount,
                        TotalGrossKG = item.TotalGrossKG,
                        TotalNetKG = item.TotalNetKG,
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
                            NumberofPackage = item.NumberofPackage,
                            CustomerID = item.CustomerID,
                            OnePackageGrossKG = item.OnePackageGrossKG,
                            OnePackageNetKG = item.OnePackageNetKG,
                            PackageContent = item.PackageContent,
                            PackageNo = item.PackageNo,
                            PackageType = item.PackageType,
                            ProductID = item.ProductID,
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
                            LastModificationTime = DateTime.Now,
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                        }).Where(new { Id = line.Id }, false, false, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            #endregion

            var billOfMaterial = queryFactory.Update<SelectPackingListsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PackingLists, LogType.Update, billOfMaterial.Id);

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
            return new SuccessDataResult<SelectPackingListsDto>(PackingListsDto);


        }
    }
}
