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
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Entities.PackageFiche.Services;
using TsiErp.Business.Entities.ShippingManagement.PalletRecord.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackageFiche.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingList;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecord;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecord.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecordLine;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecordLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PalletRecords.Page;

namespace TsiErp.Business.Entities.PalletRecord.Services
{
    [ServiceRegistration(typeof(IPalletRecordsAppService), DependencyInjectionType.Scoped)]
    public class PalletRecordsAppService : ApplicationService<PalletRecordsResource>, IPalletRecordsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly IPackageFichesAppService _PackageFichesAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public PalletRecordsAppService(IStringLocalizer<PalletRecordsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, IPackageFichesAppService packageFichesAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _PackageFichesAppService = packageFichesAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }




        [ValidationAspect(typeof(CreatePalletRecordsValidator), Priority = 1)]
        public async Task<IDataResult<SelectPalletRecordsDto>> CreateAsync(CreatePalletRecordsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.PalletRecords).Select("Code").Where(new { Code = input.Code }, "");
            var list = queryFactory.ControlList<PalletRecords>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            int state = 0;

            if (input.SelectPalletRecordLines.Where(t => t.LineApproval == true).ToList().Count == input.SelectPalletRecordLines.Count)
            {
                state = 3;
            }
            else if (input.SelectPalletRecordLines.Where(t => t.LineApproval == true).ToList().Count < input.SelectPalletRecordLines.Count)
            {
                state = 1;
            }


            var query = queryFactory.Query().From(Tables.PalletRecords).Insert(new CreatePalletRecordsDto
            {

                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Height_ = input.Height_,
                Lenght_ = input.Lenght_,
                MaxPackageNumber = input.MaxPackageNumber,
                PalletRecordsPrintTicketEnum = input.PalletRecordsPrintTicketEnum,
                PalletRecordsStateEnum = state,
                PalletRecordsTicketStateEnum = 1,
                Name = input.Name,
                PackageType = input.PackageType,
                PackingListID = input.PackingListID.GetValueOrDefault(),
                PalletPackageNumber = input.PalletPackageNumber,
                PlannedLoadingTime = input.PlannedLoadingTime,
                Width_ = input.Width_,
                Code = input.Code,
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
            });

            foreach (var item in input.SelectPalletRecordLines)
            {
                var queryLine = queryFactory.Query().From(Tables.PalletRecordLines).Insert(new CreatePalletRecordLinesDto
                {
                    PackageType = item.PackageType,
                    CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                    PackageFicheID = item.PackageFicheID.GetValueOrDefault(),
                    NumberofPackage = item.NumberofPackage,
                    SalesOrderID = item.SalesOrderID.GetValueOrDefault(),
                     PackageFicheLineID = item.PackageFicheLineID,
                    PackageContent = item.PackageContent,
                    ApprovedUnitPrice = 0,
                    LineApproval = item.LineApproval,
                    TotalAmount = item.TotalAmount,
                    TotalGrossKG = item.TotalGrossKG,
                    TotalNetKG = item.TotalNetKG,
                    PalletRecordID = addedEntityId,
                    CreationTime =now,
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
                    ProductID = item.ProductID.GetValueOrDefault(),
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var PalletRecord = queryFactory.Insert<SelectPalletRecordsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("PalletRecordsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PalletRecords, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PalletRecordsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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


            #region Paket Fişlerinin Çeki Listesi ID Kısımlarını Update İşlemleri

            var packageFicheIDList = input.SelectPalletRecordLines.Select(t => t.PackageFicheID).Distinct().ToList();

            if (packageFicheIDList.Count > 0 && packageFicheIDList != null)
            {
                foreach (var packageFicheID in packageFicheIDList)
                {
                    if (packageFicheID != null && packageFicheID != Guid.Empty)
                    {
                        var selectedPackageFiche = (await _PackageFichesAppService.GetAsync(packageFicheID.GetValueOrDefault())).Data;

                        if (selectedPackageFiche != null)
                        {
                            selectedPackageFiche.PackingListID = input.PackingListID;

                            var updatedPackageFicheEntity = ObjectMapper.Map<SelectPackageFichesDto, UpdatePackageFichesDto>(selectedPackageFiche);

                            await _PackageFichesAppService.UpdateAsync(updatedPackageFicheEntity);
                        }
                    }
                }
            }

            #endregion

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPalletRecordsDto>(PalletRecord);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("PalletID", new List<string>
            {
                Tables.PackingListPalletLines
            });


            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var entity = (await GetAsync(id)).Data;
                var query = queryFactory.Query().From(Tables.PalletRecords).Select("*").Where(new { Id = id },  "");

                var PalletRecords = queryFactory.Get<SelectPalletRecordsDto>(query);

                if (PalletRecords.Id != Guid.Empty && PalletRecords != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.PalletRecords).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.PalletRecordLines).Delete(LoginedUserService.UserId).Where(new { PalletRecordID = id },  "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var PalletRecord = queryFactory.Update<SelectPalletRecordsDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PalletRecords, LogType.Delete, id);
                    #region Notification

                    var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PalletRecordsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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

                    await Task.CompletedTask;
                    return new SuccessDataResult<SelectPalletRecordsDto>(PalletRecord);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.PalletRecordLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");
                    var PalletRecordLines = queryFactory.Update<SelectPalletRecordLinesDto>(queryLine, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PalletRecordLines, LogType.Delete, id);
                    await Task.CompletedTask;
                    return new SuccessDataResult<SelectPalletRecordLinesDto>(PalletRecordLines);
                }
            }
        }

        public async Task<IDataResult<SelectPalletRecordsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.PalletRecords)
                   .Select<PalletRecords>(null)
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CurrentAccountCardName = pr.Name, CurrentAccountCardCode = pr.Code, CurrentAccountCardID = pr.Id },
                        nameof(PalletRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<PackingLists>
                    (
                        pr => new { PackingListCode = pr.Code, PackingListID = pr.Id },
                        nameof(PalletRecords.PackingListID),
                        nameof(PackingLists.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id },  Tables.PalletRecords);

            var palletRecords = queryFactory.Get<SelectPalletRecordsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PalletRecordLines)
                   .Select<PalletRecordLines>(null)
                   .Join<CurrentAccountCards>
                    (
                        p => new { CurrentAccountCardID = p.Id, CustomerCode = p.CustomerCode },
                        nameof(PalletRecordLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        "CurrentAccountCardLine",
                        JoinType.Left
                    )
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PalletRecordLines.ProductID),
                        nameof(Products.Id),
                        "ProductLine",
                        JoinType.Left
                    )
                    .Where(new { PalletRecordID = id }, Tables.PalletRecordLines);

            var PalletRecordLine = queryFactory.GetList<SelectPalletRecordLinesDto>(queryLines).ToList();

            palletRecords.SelectPalletRecordLines = PalletRecordLine;

            LogsAppService.InsertLogToDatabase(palletRecords, palletRecords, LoginedUserService.UserId, Tables.PalletRecords, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPalletRecordsDto>(palletRecords);

        }

        public async Task<IDataResult<IList<ListPalletRecordsDto>>> GetListAsync(ListPalletRecordsParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.PalletRecords)
                   .Select<PalletRecords>(null)
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CurrentAccountCardName = pr.Name, CurrentAccountCardCode = pr.Code, CurrentAccountCardID = pr.Id },
                        nameof(PalletRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<PackingLists>
                    (
                        pr => new { PackingListCode = pr.Code, PackingListID = pr.Id },
                        nameof(PalletRecords.PackingListID),
                        nameof(PackingLists.Id),
                        JoinType.Left
                    )
                    .Where(null,  Tables.PalletRecords);

            var palletRecords = queryFactory.GetList<ListPalletRecordsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListPalletRecordsDto>>(palletRecords);

        }

        [ValidationAspect(typeof(UpdatePalletRecordsValidator), Priority = 1)]
        public async Task<IDataResult<SelectPalletRecordsDto>> UpdateAsync(UpdatePalletRecordsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.PalletRecords)
                   .Select<PalletRecords>(null)
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CurrentAccountCardName = pr.Name, CurrentAccountCardCode = pr.Code, CurrentAccountCardID = pr.Id },
                        nameof(PalletRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<PackingLists>
                    (
                        pr => new { PackingListCode = pr.Code, PackingListID = pr.Id },
                        nameof(PalletRecords.PackingListID),
                        nameof(PackingLists.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id },  Tables.PalletRecords);

            var entity = queryFactory.Get<SelectPalletRecordsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PalletRecordLines)
                   .Select<PalletRecordLines>(null)
                   .Join<CurrentAccountCards>
                    (
                        p => new { CurrentAccountCardID = p.Id, CustomerCode = p.CustomerCode },
                        nameof(PalletRecordLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        "CurrentAccountCardLine",
                        JoinType.Left
                    )
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PalletRecordLines.ProductID),
                        nameof(Products.Id),
                        "ProductLine",
                        JoinType.Left
                    )
                    .Where(new { PalletRecordID = input.Id },  Tables.PalletRecordLines);

            var PalletRecordLine = queryFactory.GetList<SelectPalletRecordLinesDto>(queryLines).ToList();

            entity.SelectPalletRecordLines = PalletRecordLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.PalletRecords)
                   .Select<PalletRecords>(null)
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CurrentAccountCardName = pr.Name, CurrentAccountCardCode = pr.Code, CurrentAccountCardID = pr.Id },
                        nameof(PalletRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                            .Where(new { Code = input.Code },  Tables.PalletRecords);

            var list = queryFactory.GetList<ListPalletRecordsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            int state = 0;

            if (input.SelectPalletRecordLines.Where(t => t.LineApproval == true).ToList().Count == input.SelectPalletRecordLines.Count)
            {
                state = 3;
            }
            else if (input.SelectPalletRecordLines.Where(t => t.LineApproval == true).ToList().Count < input.SelectPalletRecordLines.Count)
            {
                state = input.PalletRecordsStateEnum;
            }

            var query = queryFactory.Query().From(Tables.PalletRecords).Update(new UpdatePalletRecordsDto
            {
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Height_ = input.Height_,
                PalletRecordsTicketStateEnum = input.PalletRecordsTicketStateEnum,
                PalletRecordsStateEnum = state,
                PalletRecordsPrintTicketEnum = input.PalletRecordsPrintTicketEnum,
                Lenght_ = input.Lenght_,
                MaxPackageNumber = input.MaxPackageNumber,
                Name = input.Name,
                PackageType = input.PackageType,
                PackingListID = input.PackingListID.GetValueOrDefault(),
                PalletPackageNumber = input.PalletPackageNumber,
                PlannedLoadingTime = input.PlannedLoadingTime,
                Width_ = input.Width_,
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime =now,
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectPalletRecordLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PalletRecordLines).Insert(new CreatePalletRecordLinesDto
                    {
                        PackageType = item.PackageType,
                        CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                        PackageFicheID = item.PackageFicheID.GetValueOrDefault(),
                        NumberofPackage = item.NumberofPackage,
                        PackageContent = item.PackageContent,
                        SalesOrderID = item.SalesOrderID.GetValueOrDefault(),
                        TotalAmount = item.TotalAmount,
                        TotalGrossKG = item.TotalGrossKG,
                        LineApproval = item.LineApproval,
                        PackageFicheLineID = item.PackageFicheLineID,
                        ApprovedUnitPrice = item.ApprovedUnitPrice,
                        TotalNetKG = item.TotalNetKG,
                        PalletRecordID = input.Id,
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
                        ProductID = item.ProductID.GetValueOrDefault(),
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.PalletRecordLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectPalletRecordLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PalletRecordLines).Update(new UpdatePalletRecordLinesDto
                        {
                            PalletRecordID = input.Id,
                            PackageType = item.PackageType,
                            CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                            NumberofPackage = item.NumberofPackage,
                            PackageContent = item.PackageContent,
                            SalesOrderID = item.SalesOrderID.GetValueOrDefault(),
                            PackageFicheID = item.PackageFicheID.GetValueOrDefault(),
                            TotalAmount = item.TotalAmount,
                            TotalGrossKG = item.TotalGrossKG,
                            PackageFicheLineID = item.PackageFicheLineID,
                            ApprovedUnitPrice = item.ApprovedUnitPrice,
                            LineApproval = item.LineApproval,
                            TotalNetKG = item.TotalNetKG,
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
                            ProductID = item.ProductID.GetValueOrDefault(),
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var billOfMaterial = queryFactory.Update<SelectPalletRecordsDto>(query, "Id", true);

            #region Paket Fişlerinin Çeki Listesi ID Kısımlarını Update İşlemleri

            var packageFicheIDList = input.SelectPalletRecordLines.Select(t => t.PackageFicheID).Distinct().ToList();

            if (packageFicheIDList.Count > 0 && packageFicheIDList != null)
            {
                foreach (var packageFicheID in packageFicheIDList)
                {
                    if (packageFicheID != null && packageFicheID != Guid.Empty)
                    {
                        var selectedPackageFiche = (await _PackageFichesAppService.GetAsync(packageFicheID.GetValueOrDefault())).Data;

                        if (selectedPackageFiche != null)
                        {
                            selectedPackageFiche.PackingListID = input.PackingListID;

                            var updatedPackageFicheEntity = ObjectMapper.Map<SelectPackageFichesDto, UpdatePackageFichesDto>(selectedPackageFiche);

                            await _PackageFichesAppService.UpdateAsync(updatedPackageFicheEntity);
                        }
                    }
                }
            }

            #endregion

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PalletRecords, LogType.Update, billOfMaterial.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PalletRecordsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectPalletRecordsDto>(billOfMaterial);

        }

        public async Task<IDataResult<SelectPalletRecordsDto>> UpdatePreparingAsync(UpdatePalletRecordsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.PalletRecords)
                   .Select<PalletRecords>(null)
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CurrentAccountCardName = pr.Name, CurrentAccountCardCode = pr.Code, CurrentAccountCardID = pr.Id },
                        nameof(PalletRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<PackingLists>
                    (
                        pr => new { PackingListCode = pr.Code, PackingListID = pr.Id },
                        nameof(PalletRecords.PackingListID),
                        nameof(PackingLists.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.PalletRecords);

            var entity = queryFactory.Get<SelectPalletRecordsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PalletRecordLines)
                   .Select<PalletRecordLines>(null)
                   .Join<CurrentAccountCards>
                    (
                        p => new { CurrentAccountCardID = p.Id, CustomerCode = p.CustomerCode },
                        nameof(PalletRecordLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        "CurrentAccountCardLine",
                        JoinType.Left
                    )
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PalletRecordLines.ProductID),
                        nameof(Products.Id),
                        "ProductLine",
                        JoinType.Left
                    )
                    .Where(new { PalletRecordID = input.Id }, Tables.PalletRecordLines);

            var PalletRecordLine = queryFactory.GetList<SelectPalletRecordLinesDto>(queryLines).ToList();

            entity.SelectPalletRecordLines = PalletRecordLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.PalletRecords)
                   .Select<PalletRecords>(null)
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CurrentAccountCardName = pr.Name, CurrentAccountCardCode = pr.Code, CurrentAccountCardID = pr.Id },
                        nameof(PalletRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                            .Where(new { Code = input.Code }, Tables.PalletRecords);

            var list = queryFactory.GetList<ListPalletRecordsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            int state = 0;

            if (input.SelectPalletRecordLines.Where(t => t.LineApproval == true).ToList().Count == input.SelectPalletRecordLines.Count)
            {
                state = 3;
            }
            else if (input.SelectPalletRecordLines.Where(t => t.LineApproval == true).ToList().Count < input.SelectPalletRecordLines.Count)
            {
                state = input.PalletRecordsStateEnum;
            }

            var query = queryFactory.Query().From(Tables.PalletRecords).Update(new UpdatePalletRecordsDto
            {
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Height_ = input.Height_,
                PalletRecordsTicketStateEnum = input.PalletRecordsTicketStateEnum,
                PalletRecordsStateEnum = state,
                PalletRecordsPrintTicketEnum = input.PalletRecordsPrintTicketEnum,
                Lenght_ = input.Lenght_,
                MaxPackageNumber = input.MaxPackageNumber,
                Name = input.Name,
                PackageType = input.PackageType,
                PackingListID = input.PackingListID.GetValueOrDefault(),
                PalletPackageNumber = input.PalletPackageNumber,
                PlannedLoadingTime = input.PlannedLoadingTime,
                Width_ = input.Width_,
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectPalletRecordLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PalletRecordLines).Insert(new CreatePalletRecordLinesDto
                    {
                        PackageType = item.PackageType,
                        CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                        PackageFicheID = item.PackageFicheID.GetValueOrDefault(),
                        NumberofPackage = item.NumberofPackage,
                        PackageContent = item.PackageContent,
                        SalesOrderID = item.SalesOrderID.GetValueOrDefault(),
                        TotalAmount = item.TotalAmount,
                        TotalGrossKG = item.TotalGrossKG,
                        LineApproval = item.LineApproval,
                        PackageFicheLineID = item.PackageFicheLineID,
                        ApprovedUnitPrice = item.ApprovedUnitPrice,
                        TotalNetKG = item.TotalNetKG,
                        PalletRecordID = input.Id,
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
                        ProductID = item.ProductID.GetValueOrDefault(),
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.PalletRecordLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectPalletRecordLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PalletRecordLines).Update(new UpdatePalletRecordLinesDto
                        {
                            PalletRecordID = input.Id,
                            PackageType = item.PackageType,
                            CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                            NumberofPackage = item.NumberofPackage,
                            PackageContent = item.PackageContent,
                            SalesOrderID = item.SalesOrderID.GetValueOrDefault(),
                            PackageFicheID = item.PackageFicheID.GetValueOrDefault(),
                            TotalAmount = item.TotalAmount,
                            TotalGrossKG = item.TotalGrossKG,
                            ApprovedUnitPrice = item.ApprovedUnitPrice,
                            LineApproval = item.LineApproval,
                            TotalNetKG = item.TotalNetKG,
                            PackageFicheLineID = item.PackageFicheLineID,
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
                            ProductID = item.ProductID.GetValueOrDefault(),
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var billOfMaterial = queryFactory.Update<SelectPalletRecordsDto>(query, "Id", true);

            #region Paket Fişlerinin Çeki Listesi ID Kısımlarını Update İşlemleri

            var packageFicheIDList = input.SelectPalletRecordLines.Select(t => t.PackageFicheID).Distinct().ToList();

            if (packageFicheIDList.Count > 0 && packageFicheIDList != null)
            {
                foreach (var packageFicheID in packageFicheIDList)
                {
                    if (packageFicheID != null && packageFicheID != Guid.Empty)
                    {
                        var selectedPackageFiche = (await _PackageFichesAppService.GetAsync(packageFicheID.GetValueOrDefault())).Data;

                        if (selectedPackageFiche != null)
                        {
                            selectedPackageFiche.PackingListID = input.PackingListID;

                            var updatedPackageFicheEntity = ObjectMapper.Map<SelectPackageFichesDto, UpdatePackageFichesDto>(selectedPackageFiche);

                            await _PackageFichesAppService.UpdateAsync(updatedPackageFicheEntity);
                        }
                    }
                }
            }

            #endregion

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PalletRecords, LogType.Update, billOfMaterial.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["PalletRecordsChildMenu"],  L["PalletRecordsContextStatePreparing"])).Data.FirstOrDefault();

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
                                ContextMenuName_ = L["PalletRecordsContextStatePreparing"],
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
                            ContextMenuName_ = L["PalletRecordsContextStatePreparing"],
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
            return new SuccessDataResult<SelectPalletRecordsDto>(billOfMaterial);

        }

        public async Task<IDataResult<SelectPalletRecordsDto>> UpdateCompletedAsync(UpdatePalletRecordsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.PalletRecords)
                   .Select<PalletRecords>(null)
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CurrentAccountCardName = pr.Name, CurrentAccountCardCode = pr.Code, CurrentAccountCardID = pr.Id },
                        nameof(PalletRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<PackingLists>
                    (
                        pr => new { PackingListCode = pr.Code, PackingListID = pr.Id },
                        nameof(PalletRecords.PackingListID),
                        nameof(PackingLists.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.PalletRecords);

            var entity = queryFactory.Get<SelectPalletRecordsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PalletRecordLines)
                   .Select<PalletRecordLines>(null)
                   .Join<CurrentAccountCards>
                    (
                        p => new { CurrentAccountCardID = p.Id, CustomerCode = p.CustomerCode },
                        nameof(PalletRecordLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        "CurrentAccountCardLine",
                        JoinType.Left
                    )
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PalletRecordLines.ProductID),
                        nameof(Products.Id),
                        "ProductLine",
                        JoinType.Left
                    )
                    .Where(new { PalletRecordID = input.Id }, Tables.PalletRecordLines);

            var PalletRecordLine = queryFactory.GetList<SelectPalletRecordLinesDto>(queryLines).ToList();

            entity.SelectPalletRecordLines = PalletRecordLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.PalletRecords)
                   .Select<PalletRecords>(null)
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CurrentAccountCardName = pr.Name, CurrentAccountCardCode = pr.Code, CurrentAccountCardID = pr.Id },
                        nameof(PalletRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                            .Where(new { Code = input.Code }, Tables.PalletRecords);

            var list = queryFactory.GetList<ListPalletRecordsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            int state = 0;

            if (input.SelectPalletRecordLines.Where(t => t.LineApproval == true).ToList().Count == input.SelectPalletRecordLines.Count)
            {
                state = 3;
            }
            else if (input.SelectPalletRecordLines.Where(t => t.LineApproval == true).ToList().Count < input.SelectPalletRecordLines.Count)
            {
                state = input.PalletRecordsStateEnum;
            }

            var query = queryFactory.Query().From(Tables.PalletRecords).Update(new UpdatePalletRecordsDto
            {
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Height_ = input.Height_,
                PalletRecordsTicketStateEnum = input.PalletRecordsTicketStateEnum,
                PalletRecordsStateEnum = state,
                PalletRecordsPrintTicketEnum = input.PalletRecordsPrintTicketEnum,
                Lenght_ = input.Lenght_,
                MaxPackageNumber = input.MaxPackageNumber,
                Name = input.Name,
                PackageType = input.PackageType,
                PackingListID = input.PackingListID.GetValueOrDefault(),
                PalletPackageNumber = input.PalletPackageNumber,
                PlannedLoadingTime = input.PlannedLoadingTime,
                Width_ = input.Width_,
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectPalletRecordLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PalletRecordLines).Insert(new CreatePalletRecordLinesDto
                    {
                        PackageType = item.PackageType,
                        CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                        PackageFicheID = item.PackageFicheID.GetValueOrDefault(),
                        NumberofPackage = item.NumberofPackage,
                        PackageContent = item.PackageContent,
                        SalesOrderID = item.SalesOrderID.GetValueOrDefault(),
                        TotalAmount = item.TotalAmount,
                        TotalGrossKG = item.TotalGrossKG,
                        LineApproval = item.LineApproval,
                        ApprovedUnitPrice = item.ApprovedUnitPrice,
                        TotalNetKG = item.TotalNetKG,
                        PalletRecordID = input.Id,
                        CreationTime = now,
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        PackageFicheLineID = item.PackageFicheLineID,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        Id = GuidGenerator.CreateGuid(),
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                        LineNr = item.LineNr,
                        ProductID = item.ProductID.GetValueOrDefault(),
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.PalletRecordLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectPalletRecordLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PalletRecordLines).Update(new UpdatePalletRecordLinesDto
                        {
                            PalletRecordID = input.Id,
                            PackageType = item.PackageType,
                            CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                            NumberofPackage = item.NumberofPackage,
                            PackageFicheLineID = item.PackageFicheLineID,
                            PackageContent = item.PackageContent,
                            SalesOrderID = item.SalesOrderID.GetValueOrDefault(),
                            PackageFicheID = item.PackageFicheID.GetValueOrDefault(),
                            TotalAmount = item.TotalAmount,
                            TotalGrossKG = item.TotalGrossKG,
                            ApprovedUnitPrice = item.ApprovedUnitPrice,
                            LineApproval = item.LineApproval,
                            TotalNetKG = item.TotalNetKG,
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
                            ProductID = item.ProductID.GetValueOrDefault(),
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var billOfMaterial = queryFactory.Update<SelectPalletRecordsDto>(query, "Id", true);

            #region Paket Fişlerinin Çeki Listesi ID Kısımlarını Update İşlemleri

            var packageFicheIDList = input.SelectPalletRecordLines.Select(t => t.PackageFicheID).Distinct().ToList();

            if (packageFicheIDList.Count > 0 && packageFicheIDList != null)
            {
                foreach (var packageFicheID in packageFicheIDList)
                {
                    if (packageFicheID != null && packageFicheID != Guid.Empty)
                    {
                        var selectedPackageFiche = (await _PackageFichesAppService.GetAsync(packageFicheID.GetValueOrDefault())).Data;

                        if (selectedPackageFiche != null)
                        {
                            selectedPackageFiche.PackingListID = input.PackingListID;

                            var updatedPackageFicheEntity = ObjectMapper.Map<SelectPackageFichesDto, UpdatePackageFichesDto>(selectedPackageFiche);

                            await _PackageFichesAppService.UpdateAsync(updatedPackageFicheEntity);
                        }
                    }
                }
            }

            #endregion

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PalletRecords, LogType.Update, billOfMaterial.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["PalletRecordsChildMenu"], L["PalletRecordsContextStateCompleted"])).Data.FirstOrDefault();

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
                                ContextMenuName_ = L["PalletRecordsContextStateCompleted"],
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
                            ContextMenuName_ = L["PalletRecordsContextStateCompleted"],
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
            return new SuccessDataResult<SelectPalletRecordsDto>(billOfMaterial);

        }

        public async Task<IDataResult<SelectPalletRecordsDto>> UpdateApprovedAsync(UpdatePalletRecordsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.PalletRecords)
                   .Select<PalletRecords>(null)
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CurrentAccountCardName = pr.Name, CurrentAccountCardCode = pr.Code, CurrentAccountCardID = pr.Id },
                        nameof(PalletRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<PackingLists>
                    (
                        pr => new { PackingListCode = pr.Code, PackingListID = pr.Id },
                        nameof(PalletRecords.PackingListID),
                        nameof(PackingLists.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.PalletRecords);

            var entity = queryFactory.Get<SelectPalletRecordsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PalletRecordLines)
                   .Select<PalletRecordLines>(null)
                   .Join<CurrentAccountCards>
                    (
                        p => new { CurrentAccountCardID = p.Id, CustomerCode = p.CustomerCode },
                        nameof(PalletRecordLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        "CurrentAccountCardLine",
                        JoinType.Left
                    )
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PalletRecordLines.ProductID),
                        nameof(Products.Id),
                        "ProductLine",
                        JoinType.Left
                    )
                    .Where(new { PalletRecordID = input.Id }, Tables.PalletRecordLines);

            var PalletRecordLine = queryFactory.GetList<SelectPalletRecordLinesDto>(queryLines).ToList();

            entity.SelectPalletRecordLines = PalletRecordLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.PalletRecords)
                   .Select<PalletRecords>(null)
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CurrentAccountCardName = pr.Name, CurrentAccountCardCode = pr.Code, CurrentAccountCardID = pr.Id },
                        nameof(PalletRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                            .Where(new { Code = input.Code }, Tables.PalletRecords);

            var list = queryFactory.GetList<ListPalletRecordsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            int state = 0;

            if (input.SelectPalletRecordLines.Where(t => t.LineApproval == true).ToList().Count == input.SelectPalletRecordLines.Count)
            {
                state = 3;
            }
            else if (input.SelectPalletRecordLines.Where(t => t.LineApproval == true).ToList().Count < input.SelectPalletRecordLines.Count)
            {
                state = input.PalletRecordsStateEnum;
            }

            var query = queryFactory.Query().From(Tables.PalletRecords).Update(new UpdatePalletRecordsDto
            {
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Height_ = input.Height_,
                PalletRecordsTicketStateEnum = input.PalletRecordsTicketStateEnum,
                PalletRecordsStateEnum = state,
                PalletRecordsPrintTicketEnum = input.PalletRecordsPrintTicketEnum,
                Lenght_ = input.Lenght_,
                MaxPackageNumber = input.MaxPackageNumber,
                Name = input.Name,
                PackageType = input.PackageType,
                PackingListID = input.PackingListID.GetValueOrDefault(),
                PalletPackageNumber = input.PalletPackageNumber,
                PlannedLoadingTime = input.PlannedLoadingTime,
                Width_ = input.Width_,
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectPalletRecordLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PalletRecordLines).Insert(new CreatePalletRecordLinesDto
                    {
                        PackageType = item.PackageType,
                        CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                        PackageFicheID = item.PackageFicheID.GetValueOrDefault(),
                        NumberofPackage = item.NumberofPackage,
                        PackageContent = item.PackageContent,
                        SalesOrderID = item.SalesOrderID.GetValueOrDefault(),
                        TotalAmount = item.TotalAmount,
                        TotalGrossKG = item.TotalGrossKG,
                        PackageFicheLineID = item.PackageFicheLineID,
                        LineApproval = item.LineApproval,
                        ApprovedUnitPrice = item.ApprovedUnitPrice,
                        TotalNetKG = item.TotalNetKG,
                        PalletRecordID = input.Id,
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
                        ProductID = item.ProductID.GetValueOrDefault(),
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.PalletRecordLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectPalletRecordLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PalletRecordLines).Update(new UpdatePalletRecordLinesDto
                        {
                            PalletRecordID = input.Id,
                            PackageType = item.PackageType,
                            CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                            NumberofPackage = item.NumberofPackage,
                            PackageContent = item.PackageContent,
                            SalesOrderID = item.SalesOrderID.GetValueOrDefault(),
                            PackageFicheID = item.PackageFicheID.GetValueOrDefault(),
                            TotalAmount = item.TotalAmount,
                            TotalGrossKG = item.TotalGrossKG,
                            ApprovedUnitPrice = item.ApprovedUnitPrice,
                            LineApproval = item.LineApproval,
                            PackageFicheLineID = item.PackageFicheLineID,
                            TotalNetKG = item.TotalNetKG,
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
                            ProductID = item.ProductID.GetValueOrDefault(),
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var billOfMaterial = queryFactory.Update<SelectPalletRecordsDto>(query, "Id", true);

            #region Paket Fişlerinin Çeki Listesi ID Kısımlarını Update İşlemleri

            var packageFicheIDList = input.SelectPalletRecordLines.Select(t => t.PackageFicheID).Distinct().ToList();

            if (packageFicheIDList.Count > 0 && packageFicheIDList != null)
            {
                foreach (var packageFicheID in packageFicheIDList)
                {
                    if (packageFicheID != null && packageFicheID != Guid.Empty)
                    {
                        var selectedPackageFiche = (await _PackageFichesAppService.GetAsync(packageFicheID.GetValueOrDefault())).Data;

                        if (selectedPackageFiche != null)
                        {
                            selectedPackageFiche.PackingListID = input.PackingListID;

                            var updatedPackageFicheEntity = ObjectMapper.Map<SelectPackageFichesDto, UpdatePackageFichesDto>(selectedPackageFiche);

                            await _PackageFichesAppService.UpdateAsync(updatedPackageFicheEntity);
                        }
                    }
                }
            }

            #endregion

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PalletRecords, LogType.Update, billOfMaterial.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["PalletRecordsChildMenu"], L["PalletRecordsContextStateApproved"])).Data.FirstOrDefault();

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
                                ContextMenuName_ = L["PalletRecordsContextStateApproved"],
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
                            ContextMenuName_ = L["PalletRecordsContextStateApproved"],
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
            return new SuccessDataResult<SelectPalletRecordsDto>(billOfMaterial);

        }

        public async Task<IDataResult<SelectPalletRecordsDto>> UpdateTicketPendingAsync(UpdatePalletRecordsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.PalletRecords)
                   .Select<PalletRecords>(null)
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CurrentAccountCardName = pr.Name, CurrentAccountCardCode = pr.Code, CurrentAccountCardID = pr.Id },
                        nameof(PalletRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<PackingLists>
                    (
                        pr => new { PackingListCode = pr.Code, PackingListID = pr.Id },
                        nameof(PalletRecords.PackingListID),
                        nameof(PackingLists.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.PalletRecords);

            var entity = queryFactory.Get<SelectPalletRecordsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PalletRecordLines)
                   .Select<PalletRecordLines>(null)
                   .Join<CurrentAccountCards>
                    (
                        p => new { CurrentAccountCardID = p.Id, CustomerCode = p.CustomerCode },
                        nameof(PalletRecordLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        "CurrentAccountCardLine",
                        JoinType.Left
                    )
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PalletRecordLines.ProductID),
                        nameof(Products.Id),
                        "ProductLine",
                        JoinType.Left
                    )
                    .Where(new { PalletRecordID = input.Id }, Tables.PalletRecordLines);

            var PalletRecordLine = queryFactory.GetList<SelectPalletRecordLinesDto>(queryLines).ToList();

            entity.SelectPalletRecordLines = PalletRecordLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.PalletRecords)
                   .Select<PalletRecords>(null)
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CurrentAccountCardName = pr.Name, CurrentAccountCardCode = pr.Code, CurrentAccountCardID = pr.Id },
                        nameof(PalletRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                            .Where(new { Code = input.Code }, Tables.PalletRecords);

            var list = queryFactory.GetList<ListPalletRecordsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            int state = 0;

            if (input.SelectPalletRecordLines.Where(t => t.LineApproval == true).ToList().Count == input.SelectPalletRecordLines.Count)
            {
                state = 3;
            }
            else if (input.SelectPalletRecordLines.Where(t => t.LineApproval == true).ToList().Count < input.SelectPalletRecordLines.Count)
            {
                state = input.PalletRecordsStateEnum;
            }

            var query = queryFactory.Query().From(Tables.PalletRecords).Update(new UpdatePalletRecordsDto
            {
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Height_ = input.Height_,
                PalletRecordsTicketStateEnum = input.PalletRecordsTicketStateEnum,
                PalletRecordsStateEnum = state,
                PalletRecordsPrintTicketEnum = input.PalletRecordsPrintTicketEnum,
                Lenght_ = input.Lenght_,
                MaxPackageNumber = input.MaxPackageNumber,
                Name = input.Name,
                PackageType = input.PackageType,
                PackingListID = input.PackingListID.GetValueOrDefault(),
                PalletPackageNumber = input.PalletPackageNumber,
                PlannedLoadingTime = input.PlannedLoadingTime,
                Width_ = input.Width_,
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectPalletRecordLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PalletRecordLines).Insert(new CreatePalletRecordLinesDto
                    {
                        PackageType = item.PackageType,
                        CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                        PackageFicheID = item.PackageFicheID.GetValueOrDefault(),
                        NumberofPackage = item.NumberofPackage,
                        PackageContent = item.PackageContent,
                        PackageFicheLineID = item.PackageFicheLineID,
                        SalesOrderID = item.SalesOrderID.GetValueOrDefault(),
                        TotalAmount = item.TotalAmount,
                        TotalGrossKG = item.TotalGrossKG,
                        LineApproval = item.LineApproval,
                        ApprovedUnitPrice = item.ApprovedUnitPrice,
                        TotalNetKG = item.TotalNetKG,
                        PalletRecordID = input.Id,
                        CreationTime =now,
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
                        ProductID = item.ProductID.GetValueOrDefault(),
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.PalletRecordLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectPalletRecordLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PalletRecordLines).Update(new UpdatePalletRecordLinesDto
                        {
                            PalletRecordID = input.Id,
                            PackageType = item.PackageType,
                            CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                            NumberofPackage = item.NumberofPackage,
                            PackageContent = item.PackageContent,
                            SalesOrderID = item.SalesOrderID.GetValueOrDefault(),
                            PackageFicheID = item.PackageFicheID.GetValueOrDefault(),
                            TotalAmount = item.TotalAmount,
                            PackageFicheLineID = item.PackageFicheLineID,
                            TotalGrossKG = item.TotalGrossKG,
                            ApprovedUnitPrice = item.ApprovedUnitPrice,
                            LineApproval = item.LineApproval,
                            TotalNetKG = item.TotalNetKG,
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
                            ProductID = item.ProductID.GetValueOrDefault(),
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var billOfMaterial = queryFactory.Update<SelectPalletRecordsDto>(query, "Id", true);

            #region Paket Fişlerinin Çeki Listesi ID Kısımlarını Update İşlemleri

            var packageFicheIDList = input.SelectPalletRecordLines.Select(t => t.PackageFicheID).Distinct().ToList();

            if (packageFicheIDList.Count > 0 && packageFicheIDList != null)
            {
                foreach (var packageFicheID in packageFicheIDList)
                {
                    if (packageFicheID != null && packageFicheID != Guid.Empty)
                    {
                        var selectedPackageFiche = (await _PackageFichesAppService.GetAsync(packageFicheID.GetValueOrDefault())).Data;

                        if (selectedPackageFiche != null)
                        {
                            selectedPackageFiche.PackingListID = input.PackingListID;

                            var updatedPackageFicheEntity = ObjectMapper.Map<SelectPackageFichesDto, UpdatePackageFichesDto>(selectedPackageFiche);

                            await _PackageFichesAppService.UpdateAsync(updatedPackageFicheEntity);
                        }
                    }
                }
            }

            #endregion

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PalletRecords, LogType.Update, billOfMaterial.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["PalletRecordsChildMenu"],  L["PalletRecordsContextTicketStatePending"])).Data.FirstOrDefault();

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
                                ContextMenuName_ = L["PalletRecordsContextTicketStatePending"],
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
                            ContextMenuName_ = L["PalletRecordsContextTicketStatePending"],
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
            return new SuccessDataResult<SelectPalletRecordsDto>(billOfMaterial);

        }

        public async Task<IDataResult<SelectPalletRecordsDto>> UpdateTicketCompletedAsync(UpdatePalletRecordsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.PalletRecords)
                   .Select<PalletRecords>(null)
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CurrentAccountCardName = pr.Name, CurrentAccountCardCode = pr.Code, CurrentAccountCardID = pr.Id },
                        nameof(PalletRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<PackingLists>
                    (
                        pr => new { PackingListCode = pr.Code, PackingListID = pr.Id },
                        nameof(PalletRecords.PackingListID),
                        nameof(PackingLists.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.PalletRecords);

            var entity = queryFactory.Get<SelectPalletRecordsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PalletRecordLines)
                   .Select<PalletRecordLines>(null)
                   .Join<CurrentAccountCards>
                    (
                        p => new { CurrentAccountCardID = p.Id, CustomerCode = p.CustomerCode },
                        nameof(PalletRecordLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        "CurrentAccountCardLine",
                        JoinType.Left
                    )
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PalletRecordLines.ProductID),
                        nameof(Products.Id),
                        "ProductLine",
                        JoinType.Left
                    )
                    .Where(new { PalletRecordID = input.Id }, Tables.PalletRecordLines);

            var PalletRecordLine = queryFactory.GetList<SelectPalletRecordLinesDto>(queryLines).ToList();

            entity.SelectPalletRecordLines = PalletRecordLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.PalletRecords)
                   .Select<PalletRecords>(null)
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CurrentAccountCardName = pr.Name, CurrentAccountCardCode = pr.Code, CurrentAccountCardID = pr.Id },
                        nameof(PalletRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                            .Where(new { Code = input.Code }, Tables.PalletRecords);

            var list = queryFactory.GetList<ListPalletRecordsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            int state = 0;

            if (input.SelectPalletRecordLines.Where(t => t.LineApproval == true).ToList().Count == input.SelectPalletRecordLines.Count)
            {
                state = 3;
            }
            else if (input.SelectPalletRecordLines.Where(t => t.LineApproval == true).ToList().Count < input.SelectPalletRecordLines.Count)
            {
                state = input.PalletRecordsStateEnum;
            }

            var query = queryFactory.Query().From(Tables.PalletRecords).Update(new UpdatePalletRecordsDto
            {
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Height_ = input.Height_,
                PalletRecordsTicketStateEnum = input.PalletRecordsTicketStateEnum,
                PalletRecordsStateEnum = state,
                PalletRecordsPrintTicketEnum = input.PalletRecordsPrintTicketEnum,
                Lenght_ = input.Lenght_,
                MaxPackageNumber = input.MaxPackageNumber,
                Name = input.Name,
                PackageType = input.PackageType,
                PackingListID = input.PackingListID.GetValueOrDefault(),
                PalletPackageNumber = input.PalletPackageNumber,
                PlannedLoadingTime = input.PlannedLoadingTime,
                Width_ = input.Width_,
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime =now,
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectPalletRecordLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PalletRecordLines).Insert(new CreatePalletRecordLinesDto
                    {
                        PackageType = item.PackageType,
                        CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                        PackageFicheID = item.PackageFicheID.GetValueOrDefault(),
                        NumberofPackage = item.NumberofPackage,
                        PackageContent = item.PackageContent,
                        SalesOrderID = item.SalesOrderID.GetValueOrDefault(),
                        TotalAmount = item.TotalAmount,
                        TotalGrossKG = item.TotalGrossKG,
                        PackageFicheLineID = item.PackageFicheLineID,
                        LineApproval = item.LineApproval,
                        ApprovedUnitPrice = item.ApprovedUnitPrice,
                        TotalNetKG = item.TotalNetKG,
                        PalletRecordID = input.Id,
                        CreationTime =now,
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
                        ProductID = item.ProductID.GetValueOrDefault(),
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.PalletRecordLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectPalletRecordLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PalletRecordLines).Update(new UpdatePalletRecordLinesDto
                        {
                            PalletRecordID = input.Id,
                            PackageType = item.PackageType,
                            CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                            NumberofPackage = item.NumberofPackage,
                            PackageContent = item.PackageContent,
                            SalesOrderID = item.SalesOrderID.GetValueOrDefault(),
                            PackageFicheID = item.PackageFicheID.GetValueOrDefault(),
                            TotalAmount = item.TotalAmount,
                            TotalGrossKG = item.TotalGrossKG,
                            ApprovedUnitPrice = item.ApprovedUnitPrice,
                            LineApproval = item.LineApproval,
                            PackageFicheLineID = item.PackageFicheLineID,
                            TotalNetKG = item.TotalNetKG,
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
                            ProductID = item.ProductID.GetValueOrDefault(),
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var billOfMaterial = queryFactory.Update<SelectPalletRecordsDto>(query, "Id", true);

            #region Paket Fişlerinin Çeki Listesi ID Kısımlarını Update İşlemleri

            var packageFicheIDList = input.SelectPalletRecordLines.Select(t => t.PackageFicheID).Distinct().ToList();

            if (packageFicheIDList.Count > 0 && packageFicheIDList != null)
            {
                foreach (var packageFicheID in packageFicheIDList)
                {
                    if (packageFicheID != null && packageFicheID != Guid.Empty)
                    {
                        var selectedPackageFiche = (await _PackageFichesAppService.GetAsync(packageFicheID.GetValueOrDefault())).Data;

                        if (selectedPackageFiche != null)
                        {
                            selectedPackageFiche.PackingListID = input.PackingListID;

                            var updatedPackageFicheEntity = ObjectMapper.Map<SelectPackageFichesDto, UpdatePackageFichesDto>(selectedPackageFiche);

                            await _PackageFichesAppService.UpdateAsync(updatedPackageFicheEntity);
                        }
                    }
                }
            }

            #endregion

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PalletRecords, LogType.Update, billOfMaterial.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["PalletRecordsChildMenu"],  L["PalletRecordsContextTicketStateCompleted"])).Data.FirstOrDefault();

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
                                ContextMenuName_ = L["PalletRecordsContextTicketStateCompleted"],
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
                            ContextMenuName_ = L["PalletRecordsContextTicketStateCompleted"],
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
            return new SuccessDataResult<SelectPalletRecordsDto>(billOfMaterial);

        }

        public async Task<IDataResult<SelectPalletRecordsDto>> UpdatePalletDetailAsync(UpdatePalletRecordsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.PalletRecords)
                   .Select<PalletRecords>(null)
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CurrentAccountCardName = pr.Name, CurrentAccountCardCode = pr.Code, CurrentAccountCardID = pr.Id },
                        nameof(PalletRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<PackingLists>
                    (
                        pr => new { PackingListCode = pr.Code, PackingListID = pr.Id },
                        nameof(PalletRecords.PackingListID),
                        nameof(PackingLists.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.PalletRecords);

            var entity = queryFactory.Get<SelectPalletRecordsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PalletRecordLines)
                   .Select<PalletRecordLines>(null)
                   .Join<CurrentAccountCards>
                    (
                        p => new { CurrentAccountCardID = p.Id, CustomerCode = p.CustomerCode },
                        nameof(PalletRecordLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        "CurrentAccountCardLine",
                        JoinType.Left
                    )
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PalletRecordLines.ProductID),
                        nameof(Products.Id),
                        "ProductLine",
                        JoinType.Left
                    )
                    .Where(new { PalletRecordID = input.Id }, Tables.PalletRecordLines);

            var PalletRecordLine = queryFactory.GetList<SelectPalletRecordLinesDto>(queryLines).ToList();

            entity.SelectPalletRecordLines = PalletRecordLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.PalletRecords)
                   .Select<PalletRecords>(null)
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CurrentAccountCardName = pr.Name, CurrentAccountCardCode = pr.Code, CurrentAccountCardID = pr.Id },
                        nameof(PalletRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                            .Where(new { Code = input.Code }, Tables.PalletRecords);

            var list = queryFactory.GetList<ListPalletRecordsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            int state = 0;

            if (input.SelectPalletRecordLines.Where(t => t.LineApproval == true).ToList().Count == input.SelectPalletRecordLines.Count)
            {
                state = 3;
            }
            else if (input.SelectPalletRecordLines.Where(t => t.LineApproval == true).ToList().Count < input.SelectPalletRecordLines.Count)
            {
                state = input.PalletRecordsStateEnum;
            }

            var query = queryFactory.Query().From(Tables.PalletRecords).Update(new UpdatePalletRecordsDto
            {
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Height_ = input.Height_,
                PalletRecordsTicketStateEnum = input.PalletRecordsTicketStateEnum,
                PalletRecordsStateEnum = state,
                PalletRecordsPrintTicketEnum = input.PalletRecordsPrintTicketEnum,
                Lenght_ = input.Lenght_,
                MaxPackageNumber = input.MaxPackageNumber,
                Name = input.Name,
                PackageType = input.PackageType,
                PackingListID = input.PackingListID.GetValueOrDefault(),
                PalletPackageNumber = input.PalletPackageNumber,
                PlannedLoadingTime = input.PlannedLoadingTime,
                Width_ = input.Width_,
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime =now,
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectPalletRecordLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PalletRecordLines).Insert(new CreatePalletRecordLinesDto
                    {
                        PackageType = item.PackageType,
                        CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                        PackageFicheID = item.PackageFicheID.GetValueOrDefault(),
                        NumberofPackage = item.NumberofPackage,
                        PackageContent = item.PackageContent,
                        SalesOrderID = item.SalesOrderID.GetValueOrDefault(),
                        TotalAmount = item.TotalAmount,
                        TotalGrossKG = item.TotalGrossKG,
                        PackageFicheLineID = item.PackageFicheLineID,
                        LineApproval = item.LineApproval,
                        ApprovedUnitPrice = item.ApprovedUnitPrice,
                        TotalNetKG = item.TotalNetKG,
                        PalletRecordID = input.Id,
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
                        ProductID = item.ProductID.GetValueOrDefault(),
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.PalletRecordLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectPalletRecordLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PalletRecordLines).Update(new UpdatePalletRecordLinesDto
                        {
                            PalletRecordID = input.Id,
                            PackageType = item.PackageType,
                            CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                            NumberofPackage = item.NumberofPackage,
                            PackageContent = item.PackageContent,
                            SalesOrderID = item.SalesOrderID.GetValueOrDefault(),
                            PackageFicheID = item.PackageFicheID.GetValueOrDefault(),
                            TotalAmount = item.TotalAmount,
                            TotalGrossKG = item.TotalGrossKG,
                            ApprovedUnitPrice = item.ApprovedUnitPrice,
                            LineApproval = item.LineApproval,
                            PackageFicheLineID = item.PackageFicheLineID,
                            TotalNetKG = item.TotalNetKG,
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
                            ProductID = item.ProductID.GetValueOrDefault(),
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var billOfMaterial = queryFactory.Update<SelectPalletRecordsDto>(query, "Id", true);

            #region Paket Fişlerinin Çeki Listesi ID Kısımlarını Update İşlemleri

            var packageFicheIDList = input.SelectPalletRecordLines.Select(t => t.PackageFicheID).Distinct().ToList();

            if (packageFicheIDList.Count > 0 && packageFicheIDList != null)
            {
                foreach (var packageFicheID in packageFicheIDList)
                {
                    if (packageFicheID != null && packageFicheID != Guid.Empty)
                    {
                        var selectedPackageFiche = (await _PackageFichesAppService.GetAsync(packageFicheID.GetValueOrDefault())).Data;

                        if (selectedPackageFiche != null)
                        {
                            selectedPackageFiche.PackingListID = input.PackingListID;

                            var updatedPackageFicheEntity = ObjectMapper.Map<SelectPackageFichesDto, UpdatePackageFichesDto>(selectedPackageFiche);

                            await _PackageFichesAppService.UpdateAsync(updatedPackageFicheEntity);
                        }
                    }
                }
            }

            #endregion

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PalletRecords, LogType.Update, billOfMaterial.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["PalletRecordsChildMenu"],  L["PalletRecordsContextPalletDetail"])).Data.FirstOrDefault();

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
                                ContextMenuName_ = L["PalletRecordsContextPalletDetail"],
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
                            ContextMenuName_ = L["PalletRecordsContextPalletDetail"],
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
            return new SuccessDataResult<SelectPalletRecordsDto>(billOfMaterial);

        }

        public async Task<IDataResult<SelectPalletRecordsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.PalletRecords).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<PalletRecords>(entityQuery);

            var query = queryFactory.Query().From(Tables.PalletRecords).Update(new UpdatePalletRecordsDto
            {
                CurrentAccountCardID = entity.CurrentAccountCardID,
                Height_ = entity.Height_,
                Lenght_ = entity.Lenght_,
                MaxPackageNumber = entity.MaxPackageNumber,
                PalletRecordsPrintTicketEnum = (int)entity.PalletRecordsPrintTicketEnum,
                PalletRecordsStateEnum = (int)entity.PalletRecordsStateEnum,
                PalletRecordsTicketStateEnum = (int)entity.PalletRecordsTicketStateEnum,
                Name = entity.Name,
                PackageType = entity.PackageType,
                PackingListID = entity.PackingListID,
                PalletPackageNumber = entity.PalletPackageNumber,
                PlannedLoadingTime = entity.PlannedLoadingTime,
                Width_ = entity.Width_,
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
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id },  "");

            var PalletRecordsDto = queryFactory.Update<SelectPalletRecordsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectPalletRecordsDto>(PalletRecordsDto);


        }

        public async Task<List<SelectPalletRecordLinesDto>> GetPalletLines()
        {
            var query = queryFactory.Query().From(Tables.PalletRecordLines).Select("*").Where(null, Tables.PalletRecordLines);
            var lines = queryFactory.GetList<SelectPalletRecordLinesDto>(query).ToList();
            await Task.CompletedTask;
            return lines;
        }
    }
}
