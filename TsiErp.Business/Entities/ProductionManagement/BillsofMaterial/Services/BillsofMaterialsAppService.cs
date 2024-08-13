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
using TsiErp.Business.Entities.BillsofMaterial.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.BillsofMaterials.Page;

namespace TsiErp.Business.Entities.BillsofMaterial.Services
{
    [ServiceRegistration(typeof(IBillsofMaterialsAppService), DependencyInjectionType.Scoped)]
    public class BillsofMaterialsAppService : ApplicationService<BillsofMaterialsResource>, IBillsofMaterialsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public BillsofMaterialsAppService(IStringLocalizer<BillsofMaterialsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }

        [ValidationAspect(typeof(CreateBillsofMaterialsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectBillsofMaterialsDto>> CreateAsync(CreateBillsofMaterialsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.BillsofMaterials).Select("Code").Where(new { Code = input.Code }, "");
            var list = queryFactory.ControlList<BillsofMaterials>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.BillsofMaterials).Insert(new CreateBillsofMaterialsDto
            {
                Code = input.Code,

                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                ProductType = input.ProductType,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                Name = input.Name,
                FinishedProductID = input.FinishedProductID.GetValueOrDefault(),
                _Description = input._Description
            });

            foreach (var item in input.SelectBillsofMaterialLines)
            {
                var queryLine = queryFactory.Query().From(Tables.BillsofMaterialLines).Insert(new CreateBillsofMaterialLinesDto
                {
                    BoMID = addedEntityId,
                    CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    FinishedProductID = item.FinishedProductID.GetValueOrDefault(),
                    Id = GuidGenerator.CreateGuid(),
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    LineNr = item.LineNr,
                    MaterialType = (int)item.MaterialType,
                    ProductID = item.ProductID.GetValueOrDefault(),
                    Quantity = item.Quantity,
                    Size = item.Size,
                    SupplyForm = (int)item.SupplyForm,
                    UnitSetID = item.UnitSetID.GetValueOrDefault(),
                    _Description = item._Description
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var billOfMaterial = queryFactory.Insert<SelectBillsofMaterialsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("BOMChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.BillsofMaterials, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["BOMChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectBillsofMaterialsDto>(billOfMaterial);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("BOMID", new List<string>
            {
                Tables.ProductionOrders
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var entity = (await GetAsync(id)).Data;
                var query = queryFactory.Query().From(Tables.BillsofMaterials).Select("*").Where(new { Id = id }, "");

                var billsOfMaterials = queryFactory.Get<SelectBillsofMaterialsDto>(query);

                if (billsOfMaterials.Id != Guid.Empty && billsOfMaterials != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.BillsofMaterials).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.BillsofMaterialLines).Delete(LoginedUserService.UserId).Where(new { BomID = id }, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var billOfMaterial = queryFactory.Update<SelectBillsofMaterialsDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.BillsofMaterials, LogType.Delete, id);
                    #region Notification

                    var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["BOMChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                    return new SuccessDataResult<SelectBillsofMaterialsDto>(billOfMaterial);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.BillsofMaterialLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");
                    var billOfMaterialLines = queryFactory.Update<SelectBillsofMaterialLinesDto>(queryLine, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.BillsofMaterialLines, LogType.Delete, id);
                    await Task.CompletedTask;
                    return new SuccessDataResult<SelectBillsofMaterialLinesDto>(billOfMaterialLines);
                }
            }
        }

        public async Task<IDataResult<SelectBillsofMaterialsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.BillsofMaterials)
                   .Select<BillsofMaterials>(null)
                   .Join<Products>
                    (
                        pr => new { FinishedProductCode = pr.Code, FinishedProducName = pr.Name, FinishedProductID = pr.Id, ProductType = pr.ProductType },
                        nameof(BillsofMaterials.FinishedProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CustomerCode = pr.CustomerCode, CurrentAccountCardID = pr.Id },
                        nameof(BillsofMaterials.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, Tables.BillsofMaterials);

            var billsOfMaterials = queryFactory.Get<SelectBillsofMaterialsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.BillsofMaterialLines)
                   .Select<BillsofMaterialLines>(null)
                   .Join<Products>
                    (
                        p => new { FinishedProductCode = p.Code },
                        nameof(BillsofMaterialLines.FinishedProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name, SupplyForm = p.SupplyForm },
                        nameof(BillsofMaterialLines.ProductID),
                        nameof(Products.Id),
                        "ProductLine",
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(BillsofMaterialLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                    .Where(new { BoMID = id }, Tables.BillsofMaterialLines);

            var billsOfMaterialLine = queryFactory.GetList<SelectBillsofMaterialLinesDto>(queryLines).ToList();

            billsOfMaterials.SelectBillsofMaterialLines = billsOfMaterialLine;

            LogsAppService.InsertLogToDatabase(billsOfMaterials, billsOfMaterials, LoginedUserService.UserId, Tables.BillsofMaterials, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectBillsofMaterialsDto>(billsOfMaterials);

        }

        public async Task<IDataResult<SelectBillsofMaterialsDto>> GetbyCurrentAccountIDAsync(Guid currentAccountID, Guid finishedProductId)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.BillsofMaterials)
                   .Select<BillsofMaterials>(s => new {s.Code,s.Name,s._Description})
                   .Join<Products>
                    (
                        pr => new { FinishedProductCode = pr.Code, FinishedProducName = pr.Name, FinishedProductID = pr.Id, ProductType = pr.ProductType },
                        nameof(BillsofMaterials.FinishedProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CustomerCode = pr.CustomerCode, CurrentAccountCardID = pr.Id },
                        nameof(BillsofMaterials.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { CurrentAccountCardID = currentAccountID, FinishedProductID = finishedProductId },Tables.BillsofMaterials);

            var billsOfMaterials = queryFactory.Get<SelectBillsofMaterialsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.BillsofMaterialLines)
                   .Select<BillsofMaterialLines>(null)
                   .Join<Products>
                    (
                        p => new { FinishedProductCode = p.Code },
                        nameof(BillsofMaterialLines.FinishedProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name, SupplyForm = p.SupplyForm },
                        nameof(BillsofMaterialLines.ProductID),
                        nameof(Products.Id),
                        "ProductLine",
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(BillsofMaterialLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                    .Where(new { BoMID = billsOfMaterials.Id }, Tables.BillsofMaterialLines);

            var billsOfMaterialLine = queryFactory.GetList<SelectBillsofMaterialLinesDto>(queryLines).ToList();

            billsOfMaterials.SelectBillsofMaterialLines = billsOfMaterialLine;

            LogsAppService.InsertLogToDatabase(billsOfMaterials, billsOfMaterials, LoginedUserService.UserId, Tables.BillsofMaterials, LogType.Get, billsOfMaterials.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectBillsofMaterialsDto>(billsOfMaterials);

        }

        public async Task<IDataResult<SelectBillsofMaterialsDto>> GetbyProductIDAsync(Guid finishedProductId)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.BillsofMaterials)
                   .Select<BillsofMaterials>(null)
                   .Join<Products>
                    (
                        pr => new { FinishedProductCode = pr.Code, FinishedProducName = pr.Name, FinishedProductID = pr.Id, ProductType = pr.ProductType },
                        nameof(BillsofMaterials.FinishedProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CustomerCode = pr.CustomerCode, CurrentAccountCardID = pr.Id },
                        nameof(BillsofMaterials.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { FinishedProductID = finishedProductId }, Tables.BillsofMaterials);

            var billsOfMaterials = queryFactory.Get<SelectBillsofMaterialsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.BillsofMaterialLines)
                   .Select<BillsofMaterialLines>(null)
                   .Join<Products>
                    (
                        p => new { FinishedProductCode = p.Code },
                        nameof(BillsofMaterialLines.FinishedProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name, SupplyForm = p.SupplyForm },
                        nameof(BillsofMaterialLines.ProductID),
                        nameof(Products.Id),
                        "ProductLine",
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(BillsofMaterialLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                    .Where(new { BoMID = billsOfMaterials.Id }, Tables.BillsofMaterialLines);

            var billsOfMaterialLine = queryFactory.GetList<SelectBillsofMaterialLinesDto>(queryLines).ToList();

            billsOfMaterials.SelectBillsofMaterialLines = billsOfMaterialLine;

            LogsAppService.InsertLogToDatabase(billsOfMaterials, billsOfMaterials, LoginedUserService.UserId, Tables.BillsofMaterials, LogType.Get, billsOfMaterials.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectBillsofMaterialsDto>(billsOfMaterials);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListBillsofMaterialsDto>>> GetListAsync(ListBillsofMaterialsParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.BillsofMaterials)
                   .Select<BillsofMaterials>(s => new { s.Code, s.Name, s._Description })
                   .Join<Products>
                    (
                        pr => new { FinishedProductCode = pr.Code, FinishedProducName = pr.Name, FinishedProductID = pr.Id, ProductType = pr.ProductType },
                        nameof(BillsofMaterials.FinishedProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CustomerCode = pr.CustomerCode },
                        nameof(BillsofMaterials.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(null, Tables.BillsofMaterials);

            var billsOfMaterials = queryFactory.GetList<ListBillsofMaterialsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListBillsofMaterialsDto>>(billsOfMaterials);

        }

        public async Task<IDataResult<SelectBillsofMaterialsDto>> GetListbyProductIDAsync(Guid finishedProductId)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.BillsofMaterials)
                   .Select<BillsofMaterials>(null)
                   .Join<Products>
                    (
                        pr => new { FinishedProductCode = pr.Code, FinishedProducName = pr.Name, FinishedProductID = pr.Id, ProductType = pr.ProductType },
                        nameof(BillsofMaterials.FinishedProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CustomerCode = pr.CustomerCode },
                        nameof(BillsofMaterials.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { FinishedProductID = finishedProductId }, Tables.BillsofMaterials);

            var billsOfMaterials = queryFactory.GetList<SelectBillsofMaterialsDto>(query).ToList();

            var billsOfMaterial = billsOfMaterials.FirstOrDefault();

            var queryLines = queryFactory
                  .Query()
                  .From(Tables.BillsofMaterialLines)
                  .Select<BillsofMaterialLines>(null)
                  .Join<Products>
                   (
                       p => new { FinishedProductCode = p.Code },
                       nameof(BillsofMaterialLines.FinishedProductID),
                       nameof(Products.Id),
                       JoinType.Left
                   )
                  .Join<Products>
                   (
                       p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name, SupplyForm = p.SupplyForm },
                       nameof(BillsofMaterialLines.ProductID),
                       nameof(Products.Id),
                       "ProductLine",
                       JoinType.Left
                   )
                  .Join<UnitSets>
                   (
                       u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                       nameof(BillsofMaterialLines.UnitSetID),
                       nameof(UnitSets.Id),
                       JoinType.Left
                   )
                   .Where(new { BoMID = billsOfMaterial.Id }, Tables.BillsofMaterialLines);

            var billsOfMaterialLine = queryFactory.GetList<SelectBillsofMaterialLinesDto>(queryLines).ToList();

            billsOfMaterial.SelectBillsofMaterialLines = billsOfMaterialLine;


            await Task.CompletedTask;
            return new SuccessDataResult<SelectBillsofMaterialsDto>(billsOfMaterial);

        }

        [ValidationAspect(typeof(UpdateBillsofMaterialsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectBillsofMaterialsDto>> UpdateAsync(UpdateBillsofMaterialsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.BillsofMaterials)
                   .Select<BillsofMaterials>(null)
                   .Join<Products>
                    (
                        pr => new { FinishedProductCode = pr.Code, FinishedProducName = pr.Name, FinishedProductID = pr.Id, ProductType = pr.ProductType },
                        nameof(BillsofMaterials.FinishedProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CustomerCode = pr.CustomerCode, CurrentAccountCardID = pr.Id },
                        nameof(BillsofMaterials.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id },  Tables.BillsofMaterials);

            var entity = queryFactory.Get<SelectBillsofMaterialsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.BillsofMaterialLines)
                   .Select<BillsofMaterialLines>(null)
                   .Join<Products>
                    (
                        p => new { FinishedProductCode = p.Code },
                        nameof(BillsofMaterialLines.FinishedProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name, SupplyForm = p.SupplyForm },
                        nameof(BillsofMaterialLines.ProductID),
                        nameof(Products.Id),
                        "ProductLine",
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(BillsofMaterialLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                    .Where(new { BoMID = input.Id }, Tables.BillsofMaterialLines);

            var billsOfMaterialLine = queryFactory.GetList<SelectBillsofMaterialLinesDto>(queryLines).ToList();

            entity.SelectBillsofMaterialLines = billsOfMaterialLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.BillsofMaterials)
                           .Select<BillsofMaterials>(null)
                           .Join<Products>
                            (
                                pr => new { FinishedProductCode = pr.Code, FinishedProducName = pr.Name, FinishedProductID = pr.Id, ProductType = pr.ProductType },
                                nameof(BillsofMaterials.FinishedProductID),
                                nameof(Products.Id),
                                JoinType.Left
                            )
                            .Join<CurrentAccountCards>
                    (
                        pr => new { CustomerCode = pr.CustomerCode },
                        nameof(BillsofMaterials.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                            .Where(new { Code = input.Code },  Tables.BillsofMaterials);

            var list = queryFactory.GetList<ListBillsofMaterialsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            var query = queryFactory.Query().From(Tables.BillsofMaterials).Update(new UpdateBillsofMaterialsDto
            {
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                ProductType = input.ProductType,
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId,
                Name = input.Name,
                FinishedProductID = input.FinishedProductID.GetValueOrDefault(),
                _Description = input._Description,
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault()
            }).Where(new { Id = input.Id },"");

            foreach (var item in input.SelectBillsofMaterialLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.BillsofMaterialLines).Insert(new CreateBillsofMaterialLinesDto
                    {
                        BoMID = input.Id,
                        CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        FinishedProductID = item.FinishedProductID.GetValueOrDefault(),
                        Id = GuidGenerator.CreateGuid(),
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                        LineNr = item.LineNr,
                        MaterialType = (int)item.MaterialType,
                        SupplyForm = (int)item.SupplyForm,
                        ProductID = item.ProductID.GetValueOrDefault(),
                        Quantity = item.Quantity,
                        Size = item.Size,
                        UnitSetID = item.UnitSetID.GetValueOrDefault(),
                        _Description = item._Description
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.BillsofMaterialLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectBillsofMaterialLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.BillsofMaterialLines).Update(new UpdateBillsofMaterialLinesDto
                        {
                            BoMID = input.Id,
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            FinishedProductID = item.FinishedProductID.GetValueOrDefault(),
                            Id = item.Id,
                            SupplyForm = (int)item.SupplyForm,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                            MaterialType = (int)item.MaterialType,
                            ProductID = item.ProductID.GetValueOrDefault(),
                            Quantity = item.Quantity,
                            Size = item.Size,
                            UnitSetID = item.UnitSetID.GetValueOrDefault(),
                            _Description = item._Description
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var billOfMaterial = queryFactory.Update<SelectBillsofMaterialsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.BillsofMaterials, LogType.Update, billOfMaterial.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["BOMChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectBillsofMaterialsDto>(billOfMaterial);

        }

        public async Task<IDataResult<SelectBillsofMaterialsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.BillsofMaterials).Select("Id").Where(new { Id = id },  "");

            var entity = queryFactory.Get<BillsofMaterials>(entityQuery);

            var query = queryFactory.Query().From(Tables.BillsofMaterials).Update(new UpdateBillsofMaterialsDto
            {
                Code = entity.Code,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                ProductType = (int)entity.ProductType,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = entity.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Name = entity.Name,
                FinishedProductID = entity.FinishedProductID,
                _Description = entity._Description,
                CurrentAccountCardID = entity.CurrentAccountCardID
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id },  "");

            var billsofMaterialsDto = queryFactory.Update<SelectBillsofMaterialsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectBillsofMaterialsDto>(billsofMaterialsDto);


        }
    }
}
