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
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.ShippingManagement.PalletRecord.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder;
using TsiErp.Entities.Entities.ShippingManagement.PackingList;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecord;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecord.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecordLine;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecordLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
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

        public PalletRecordsAppService(IStringLocalizer<PalletRecordsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }

        [ValidationAspect(typeof(CreatePalletRecordsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPalletRecordsDto>> CreateAsync(CreatePalletRecordsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.PalletRecords).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.ControlList<PalletRecords>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

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

            foreach (var item in input.SelectPalletRecordLines)
            {
                var queryLine = queryFactory.Query().From(Tables.PalletRecordLines).Insert(new CreatePalletRecordLinesDto
                {
                    PackageType = item.PackageType,
                    CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                    PackageFicheID = item.PackageFicheID.GetValueOrDefault(),
                    NumberofPackage = item.NumberofPackage,
                    PackageContent = item.PackageContent,
                    LineApproval = item.LineApproval,
                    TotalAmount = item.TotalAmount,
                    TotalGrossKG = item.TotalGrossKG,
                    TotalNetKG = item.TotalNetKG,
                    PalletRecordID = addedEntityId,
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
                    ProductID = item.ProductID.GetValueOrDefault(),
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var PalletRecord = queryFactory.Insert<SelectPalletRecordsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("PalletRecordsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PalletRecords, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPalletRecordsDto>(PalletRecord);

        }

        [CacheRemoveAspect("Get")]
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
                var query = queryFactory.Query().From(Tables.PalletRecords).Select("*").Where(new { Id = id }, false, false, "");

                var PalletRecords = queryFactory.Get<SelectPalletRecordsDto>(query);

                if (PalletRecords.Id != Guid.Empty && PalletRecords != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.PalletRecords).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.PalletRecordLines).Delete(LoginedUserService.UserId).Where(new { PalletRecordID = id }, false, false, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var PalletRecord = queryFactory.Update<SelectPalletRecordsDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PalletRecords, LogType.Delete, id);
                    await Task.CompletedTask;
                    return new SuccessDataResult<SelectPalletRecordsDto>(PalletRecord);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.PalletRecordLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
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
                    .Where(new { Id = id }, false, false, Tables.PalletRecords);

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
                    .Where(new { PalletRecordID = id }, false, false, Tables.PalletRecordLines);

            var PalletRecordLine = queryFactory.GetList<SelectPalletRecordLinesDto>(queryLines).ToList();

            palletRecords.SelectPalletRecordLines = PalletRecordLine;

            LogsAppService.InsertLogToDatabase(palletRecords, palletRecords, LoginedUserService.UserId, Tables.PalletRecords, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPalletRecordsDto>(palletRecords);

        }

        [CacheAspect(duration: 60)]
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
                    .Where(null, false, false, Tables.PalletRecords);

            var palletRecords = queryFactory.GetList<ListPalletRecordsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListPalletRecordsDto>>(palletRecords);

        }

        [ValidationAspect(typeof(UpdatePalletRecordsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
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
                    .Where(new { Id = input.Id }, false, false, Tables.PalletRecords);

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
                    .Where(new { PalletRecordID = input.Id }, false, false, Tables.PalletRecordLines);

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
                            .Where(new { Code = input.Code }, false, false, Tables.PalletRecords);

            var list = queryFactory.GetList<ListPalletRecordsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

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
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id }, false, false, "");

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
                        TotalAmount = item.TotalAmount,
                        TotalGrossKG = item.TotalGrossKG,
                        LineApproval = item.LineApproval,
                        TotalNetKG = item.TotalNetKG,
                        PalletRecordID = input.Id,
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
                        ProductID = item.ProductID.GetValueOrDefault(),
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.PalletRecordLines).Select("*").Where(new { Id = item.Id }, false, false, "");

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
                            PackageFicheID = item.PackageFicheID.GetValueOrDefault(),
                            TotalAmount = item.TotalAmount,
                            TotalGrossKG = item.TotalGrossKG,
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
                            LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                            ProductID = item.ProductID.GetValueOrDefault(),
                        }).Where(new { Id = line.Id }, false, false, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var billOfMaterial = queryFactory.Update<SelectPalletRecordsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PalletRecords, LogType.Update, billOfMaterial.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPalletRecordsDto>(billOfMaterial);

        }

        public async Task<IDataResult<SelectPalletRecordsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.PalletRecords).Select("*").Where(new { Id = id }, false, false, "");

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
            }).Where(new { Id = id }, false, false, "");

            var PalletRecordsDto = queryFactory.Update<SelectPalletRecordsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectPalletRecordsDto>(PalletRecordsDto);


        }
    }
}
