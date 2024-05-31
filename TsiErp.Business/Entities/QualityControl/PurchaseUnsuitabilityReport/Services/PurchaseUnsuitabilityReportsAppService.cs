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
using TsiErp.Business.Entities.PurchaseUnsuitabilityReport.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder;
using TsiErp.Entities.Entities.QualityControl.PurchaseUnsuitabilityReport;
using TsiErp.Entities.Entities.QualityControl.PurchaseUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PurchaseUnsuitabilityReports.Page;

namespace TsiErp.Business.Entities.PurchaseUnsuitabilityReport.Services
{
    [ServiceRegistration(typeof(IPurchaseUnsuitabilityReportsAppService), DependencyInjectionType.Scoped)]
    public class PurchaseUnsuitabilityReportsAppService : ApplicationService<PurchaseUnsuitabilityReportsResource>, IPurchaseUnsuitabilityReportsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public PurchaseUnsuitabilityReportsAppService(IStringLocalizer<PurchaseUnsuitabilityReportsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }

        [ValidationAspect(typeof(CreatePurchaseUnsuitabilityReportsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseUnsuitabilityReportsDto>> CreateAsync(CreatePurchaseUnsuitabilityReportsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.PurchaseUnsuitabilityReports).Select("*").Where(new { FicheNo = input.FicheNo }, false, false, "");

            var list = queryFactory.ControlList<PurchaseUnsuitabilityReports>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.PurchaseUnsuitabilityReports).Insert(new CreatePurchaseUnsuitabilityReportsDto
            {
                FicheNo = input.FicheNo,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                Date_ = input.Date_,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                Action_ = input.Action_,
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Description_ = input.Description_,
                IsUnsuitabilityWorkOrder = input.IsUnsuitabilityWorkOrder,
                OrderID = input.OrderID.GetValueOrDefault(),
                PartyNo = input.PartyNo,
                ProductID = input.ProductID.GetValueOrDefault(),
                UnsuitableAmount = input.UnsuitableAmount,
                UnsuitabilityItemsID = input.UnsuitabilityItemsID.GetValueOrDefault()
            });


            var purchaseUnsuitabilityReport = queryFactory.Insert<SelectPurchaseUnsuitabilityReportsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("PurchUnsRecordsChildMenu", input.FicheNo);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PurchaseUnsuitabilityReports, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseUnsuitabilityReportsDto>(purchaseUnsuitabilityReport);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.PurchaseUnsuitabilityReports).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

            var purchaseUnsuitabilityReport = queryFactory.Update<SelectPurchaseUnsuitabilityReportsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PurchaseUnsuitabilityReports, LogType.Delete, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseUnsuitabilityReportsDto>(purchaseUnsuitabilityReport);

        }

        public async Task<IDataResult<SelectPurchaseUnsuitabilityReportsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.PurchaseUnsuitabilityReports).Select<PurchaseUnsuitabilityReports>(null)
                .Join<PurchaseOrders>
                (
                   d => new { OrderFicheNo = d.FicheNo }, nameof(PurchaseUnsuitabilityReports.OrderID), nameof(PurchaseOrders.Id), JoinType.Left
                )
                .Join<Products>
                (
                   d => new { ProductCode = d.Code, ProductName = d.Name }, nameof(PurchaseUnsuitabilityReports.ProductID), nameof(Products.Id), JoinType.Left
                )
                .Join<CurrentAccountCards>
                (
                   d => new { CurrentAccountCardCode = d.Code, CurrentAccountCardName = d.Name }, nameof(PurchaseUnsuitabilityReports.CurrentAccountCardID), nameof(CurrentAccountCards.Id), JoinType.Left
                )
                .Join<UnsuitabilityItems>
                (
                   d => new { UnsuitabilityItemsName = d.Name }, nameof(PurchaseUnsuitabilityReports.UnsuitabilityItemsID), nameof(UnsuitabilityItems.Id), JoinType.Left
                )
                .Where(null, false, false, Tables.PurchaseUnsuitabilityReports);

            var purchaseUnsuitabilityReport = queryFactory.Get<SelectPurchaseUnsuitabilityReportsDto>(query);

            LogsAppService.InsertLogToDatabase(purchaseUnsuitabilityReport, purchaseUnsuitabilityReport, LoginedUserService.UserId, Tables.PurchaseUnsuitabilityReports, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseUnsuitabilityReportsDto>(purchaseUnsuitabilityReport);


        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPurchaseUnsuitabilityReportsDto>>> GetListAsync(ListPurchaseUnsuitabilityReportsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.PurchaseUnsuitabilityReports).Select<PurchaseUnsuitabilityReports>(r => new { r.Id, r.FicheNo, r.PartyNo, r.Date_, r.Description_, r.UnsuitableAmount, r.Action_ })
                .Join<PurchaseOrders>
                (
                   d => new { OrderFicheNo = d.FicheNo }, nameof(PurchaseUnsuitabilityReports.OrderID), nameof(PurchaseOrders.Id), JoinType.Left
                )
                .Join<Products>
                (
                   d => new { ProductCode = d.Code, ProductName = d.Name }, nameof(PurchaseUnsuitabilityReports.ProductID), nameof(Products.Id), JoinType.Left
                )
                .Join<CurrentAccountCards>
                (
                   d => new { CurrentAccountCardCode = d.Code, CurrentAccountCardName = d.Name }, nameof(PurchaseUnsuitabilityReports.CurrentAccountCardID), nameof(CurrentAccountCards.Id), JoinType.Left
                )
                .Join<UnsuitabilityItems>
                (
                   d => new { UnsuitabilityItemsName = d.Name }, nameof(PurchaseUnsuitabilityReports.UnsuitabilityItemsID), nameof(UnsuitabilityItems.Id), JoinType.Left
                )
                .Where(null, false, false, Tables.PurchaseUnsuitabilityReports);

            var purchaseUnsuitabilityReports = queryFactory.GetList<ListPurchaseUnsuitabilityReportsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListPurchaseUnsuitabilityReportsDto>>(purchaseUnsuitabilityReports);


        }


        [ValidationAspect(typeof(UpdatePurchaseUnsuitabilityReportsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseUnsuitabilityReportsDto>> UpdateAsync(UpdatePurchaseUnsuitabilityReportsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.PurchaseUnsuitabilityReports).Select("*").Where(new { Id = input.Id }, false, false, "");
            var entity = queryFactory.Get<PurchaseUnsuitabilityReports>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.PurchaseUnsuitabilityReports).Select("*").Where(new { FicheNo = input.FicheNo }, false, false, "");
            var list = queryFactory.GetList<PurchaseUnsuitabilityReports>(listQuery).ToList();

            if (list.Count > 0 && entity.FicheNo != input.FicheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.PurchaseUnsuitabilityReports).Update(new UpdatePurchaseUnsuitabilityReportsDto
            {
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId,
                Action_ = input.Action_,
                FicheNo = input.FicheNo,
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Date_ = input.Date_,
                Description_ = input.Description_,
                IsUnsuitabilityWorkOrder = input.IsUnsuitabilityWorkOrder,
                OrderID = input.OrderID.GetValueOrDefault(),
                PartyNo = input.PartyNo,
                ProductID = input.ProductID.GetValueOrDefault(),
                UnsuitableAmount = input.UnsuitableAmount,
                UnsuitabilityItemsID = input.UnsuitabilityItemsID.GetValueOrDefault()
            }).Where(new { Id = input.Id }, false, false, "");

            var purchaseUnsuitabilityReport = queryFactory.Update<SelectPurchaseUnsuitabilityReportsDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, purchaseUnsuitabilityReport, LoginedUserService.UserId, Tables.PurchaseUnsuitabilityReports, LogType.Update, entity.Id);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseUnsuitabilityReportsDto>(purchaseUnsuitabilityReport);

        }

        public async Task<IDataResult<SelectPurchaseUnsuitabilityReportsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.PurchaseUnsuitabilityReports).Select("*").Where(new { Id = id }, false, false, "");
            var entity = queryFactory.Get<PurchaseUnsuitabilityReports>(entityQuery);

            var query = queryFactory.Query().From(Tables.PurchaseUnsuitabilityReports).Update(new UpdatePurchaseUnsuitabilityReportsDto
            {
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
                Action_ = entity.Action_,
                UnsuitableAmount = entity.UnsuitableAmount,
                ProductID = entity.ProductID,
                PartyNo = entity.PartyNo,
                OrderID = entity.OrderID,
                IsUnsuitabilityWorkOrder = entity.IsUnsuitabilityWorkOrder,
                FicheNo = entity.FicheNo,
                Description_ = entity.Description_,
                Date_ = entity.Date_,
                CurrentAccountCardID = entity.CurrentAccountCardID,
                UnsuitabilityItemsID = entity.UnsuitabilityItemsID
            }).Where(new { Id = id }, false, false, "");

            var purchaseUnsuitabilityReport = queryFactory.Update<SelectPurchaseUnsuitabilityReportsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseUnsuitabilityReportsDto>(purchaseUnsuitabilityReport);


        }
    }
}
