using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.StockManagementParameter.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Entities.ProductionOrder.Services;
using TsiErp.Business.Entities.PurchaseOrder.Services;
using TsiErp.Business.Entities.StockFiche.Validations;
using TsiErp.Business.Entities.StockMovement;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequest;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.StockFiche;
using TsiErp.Entities.Entities.StockManagement.StockFiche.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.Enums;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.StockFiche;

namespace TsiErp.Business.Entities.StockFiche.Services
{
    [ServiceRegistration(typeof(IStockFichesAppService), DependencyInjectionType.Scoped)]
    public class StockFichesAppService : ApplicationService<StockFichesResource>, IStockFichesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;
        private readonly IProductionOrdersAppService _ProductionOrdersAppService;
        private readonly IPurchaseOrdersAppService _PurchaseOrdersAppService;

        private IStockManagementParametersAppService StockManagementParametersAppService { get; set; }

        public StockFichesAppService(IStringLocalizer<StockFichesResource> l, IGetSQLDateAppService getSQLDateAppService, IFicheNumbersAppService ficheNumbersAppService, IStockManagementParametersAppService stockManagementParametersAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService, IProductionOrdersAppService productionOrdersAppService, IPurchaseOrdersAppService purchaseOrdersAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            StockManagementParametersAppService = stockManagementParametersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
            _ProductionOrdersAppService = productionOrdersAppService;
            _PurchaseOrdersAppService = purchaseOrdersAppService;
        }



        [ValidationAspect(typeof(CreateStockFichesValidatorDto), Priority = 1)]
        public async Task<IDataResult<SelectStockFichesDto>> CreateAsync(CreateStockFichesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.StockFiches).Select("FicheNo").Where(new { FicheNo = input.FicheNo }, "");
            var list = queryFactory.ControlList<StockFiches>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            if (input.FicheType != 25)
            {
                switch (input.FicheType)
                {
                    case 12: input.InputOutputCode = 1; break;
                    case 11: input.InputOutputCode = 1; break;
                    case 13: input.InputOutputCode = 0; break;
                    case 50: input.InputOutputCode = 0; break;
                    case 51: input.InputOutputCode = 1; break;
                    case 55: input.InputOutputCode = 1; break;
                }
            }

            var stockParameterDataSource = (await StockManagementParametersAppService.GetStockManagementParametersAsync()).Data;

            bool autoCostParameter = stockParameterDataSource.AutoCostParameter;
            int costCalculateType = stockParameterDataSource.CostCalculationMethod;


            var query = queryFactory.Query().From(Tables.StockFiches).Insert(new CreateStockFichesDto
            {
                FicheNo = input.FicheNo,
                InputOutputCode = input.InputOutputCode,
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                PurchaseOrderID = input.PurchaseOrderID.GetValueOrDefault(),
                DeleterId = Guid.Empty,
                DeletionTime = null,
                ProductionDateReferance = input.ProductionDateReferance,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                TransactionExchangeCurrencyID = input.TransactionExchangeCurrencyID.GetValueOrDefault(),
                PurchaseRequestID = input.PurchaseRequestID.GetValueOrDefault(),
                BranchID = input.BranchID.GetValueOrDefault(),
                PurchaseInvoiceID = input.PurchaseInvoiceID.GetValueOrDefault(),
                SalesInvoiceID = input.SalesInvoiceID.GetValueOrDefault(),
                Date_ = input.Date_,
                Description_ = input.Description_,
                ExchangeRate = input.ExchangeRate,
                FicheType = input.FicheType,
                NetAmount = input.NetAmount,
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                SpecialCode = input.SpecialCode,
                Time_ = input.Time_,
                WarehouseID = input.WarehouseID.GetValueOrDefault()
            });

            foreach (var item in input.SelectStockFicheLines)
            {
                decimal productCost = 0;

                if (item.FicheType == StockFicheTypeEnum.FireFisi || item.FicheType == StockFicheTypeEnum.SarfFisi || item.FicheType == StockFicheTypeEnum.StokCikisFisi)
                {
                    if (autoCostParameter)
                    {
                        if (costCalculateType == 1)
                        {
                            var outputList = (await GetOutputList(item.ProductID.GetValueOrDefault())).ToList();
                            outputList.Add(item);
                            productCost = (await CalculateProductFIFOCostAsync(item.ProductID.GetValueOrDefault(), outputList));
                        }

                        if (costCalculateType == 2)
                        {
                            var outputList = (await GetOutputList(item.ProductID.GetValueOrDefault())).ToList();
                            outputList.Add(item);
                            productCost = (await CalculateProductLIFOCostAsync(item.ProductID.GetValueOrDefault(), outputList));
                        }
                    }
                }

                var queryLine = queryFactory.Query().From(Tables.StockFicheLines).Insert(new CreateStockFicheLinesDto
                {
                    StockFicheID = addedEntityId,
                    CreationTime = now,
                    CreatorId = LoginedUserService.UserId,
                    PurchaseOrderID = item.PurchaseOrderID.GetValueOrDefault(),
                    PurchaseOrderLineID = item.PurchaseOrderLineID.GetValueOrDefault(),
                    MRPID = item.MRPID.GetValueOrDefault(),
                    MRPLineID = item.MRPID.GetValueOrDefault(),
                    PurchaseInvoiceID = input.PurchaseInvoiceID.GetValueOrDefault(),
                    PurchaseInvoiceLineID = item.PurchaseInvoiceLineID.GetValueOrDefault(),
                    SalesInvoiceID = input.SalesInvoiceID.GetValueOrDefault(),
                    SalesInvoiceLineID = item.SalesInvoiceLineID.GetValueOrDefault(),
                    DataOpenStatus = false,
                    ProductionDateReferance = item.ProductionDateReferance,
                    TransactionExchangeLineAmount = item.TransactionExchangeLineAmount,
                    TransactionExchangeUnitPrice = item.TransactionExchangeUnitPrice,
                    InputOutputCode = input.InputOutputCode,
                    PartyNo = item.PartyNo,
                    ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = GuidGenerator.CreateGuid(),
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    LineNr = item.LineNr,
                    ProductID = item.ProductID.GetValueOrDefault(),
                    Quantity = item.Quantity,
                    UnitSetID = item.UnitSetID.GetValueOrDefault(),
                    FicheType = (int)item.FicheType,
                    LineAmount = item.LineAmount,
                    LineDescription = item.LineDescription,
                    UnitPrice = item.UnitPrice,
                    Date_ = input.Date_,
                    UnitOutputCost = productCost
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }



            var stockFiche = queryFactory.Insert<SelectStockFichesDto>(query, "Id", true);


            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["StockFichesChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

            #region Stock Movement Service

            switch (input.FicheType)
            {
                case 11:
                    StockMovementsService.InsertTotalWastages(input);

                    #region Notification


                    if (notTemplate != null && notTemplate.Id != Guid.Empty)
                    {
                        if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                        {
                            if (notTemplate.TargetUsersId.Contains("*Not*"))
                            {
                                string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                                foreach (string user in usersNot)
                                {
                                    CreateNotificationsDto createInput = new CreateNotificationsDto
                                    {
                                        ContextMenuName_ = L["StockFicheContextAddWastege"],
                                        IsViewed = false,

                                        ModuleName_ = notTemplate.ModuleName_,
                                        ProcessName_ = notTemplate.ProcessName_,
                                        RecordNumber = input.FicheNo,
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
                                    ContextMenuName_ = L["StockFicheContextAddWastege"],
                                    IsViewed = false,

                                    ModuleName_ = notTemplate.ModuleName_,
                                    ProcessName_ = notTemplate.ProcessName_,
                                    RecordNumber = input.FicheNo,
                                    NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                    UserId = new Guid(notTemplate.TargetUsersId),
                                    ViewDate = null,
                                };

                                await _NotificationsAppService.CreateAsync(createInput);
                            }
                        }

                    }

                    #endregion

                    ; break;

                case 12:
                    StockMovementsService.InsertTotalConsumptions(input);
                    #region Notification


                    if (notTemplate != null && notTemplate.Id != Guid.Empty)
                    {
                        if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                        {
                            if (notTemplate.TargetUsersId.Contains("*Not*"))
                            {
                                string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                                foreach (string user in usersNot)
                                {
                                    CreateNotificationsDto createInput = new CreateNotificationsDto
                                    {
                                        ContextMenuName_ = L["StockFicheContextAddConsume"],
                                        IsViewed = false,

                                        ModuleName_ = notTemplate.ModuleName_,
                                        ProcessName_ = notTemplate.ProcessName_,
                                        RecordNumber = input.FicheNo,
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
                                    ContextMenuName_ = L["StockFicheContextAddConsume"],
                                    IsViewed = false,

                                    ModuleName_ = notTemplate.ModuleName_,
                                    ProcessName_ = notTemplate.ProcessName_,
                                    RecordNumber = input.FicheNo,
                                    NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                    UserId = new Guid(notTemplate.TargetUsersId),
                                    ViewDate = null,
                                };

                                await _NotificationsAppService.CreateAsync(createInput);
                            }
                        }

                    }

                    #endregion
                    break;

                case 13:
                    StockMovementsService.InsertTotalProductions(input);
                    #region Notification


                    if (notTemplate != null && notTemplate.Id != Guid.Empty)
                    {
                        if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                        {
                            if (notTemplate.TargetUsersId.Contains("*Not*"))
                            {
                                string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                                foreach (string user in usersNot)
                                {
                                    CreateNotificationsDto createInput = new CreateNotificationsDto
                                    {
                                        ContextMenuName_ = L["StockFicheContextAddProductionIncome"],
                                        IsViewed = false,

                                        ModuleName_ = notTemplate.ModuleName_,
                                        ProcessName_ = notTemplate.ProcessName_,
                                        RecordNumber = input.FicheNo,
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
                                    ContextMenuName_ = L["StockFicheContextAddProductionIncome"],
                                    IsViewed = false,

                                    ModuleName_ = notTemplate.ModuleName_,
                                    ProcessName_ = notTemplate.ProcessName_,
                                    RecordNumber = input.FicheNo,
                                    NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                    UserId = new Guid(notTemplate.TargetUsersId),
                                    ViewDate = null,
                                };

                                await _NotificationsAppService.CreateAsync(createInput);
                            }
                        }

                    }

                    #endregion
                    break;

                case 50:
                    StockMovementsService.InsertTotalGoods(input);
                    #region Notification


                    if (notTemplate != null && notTemplate.Id != Guid.Empty)
                    {
                        if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                        {
                            if (notTemplate.TargetUsersId.Contains("*Not*"))
                            {
                                string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                                foreach (string user in usersNot)
                                {
                                    CreateNotificationsDto createInput = new CreateNotificationsDto
                                    {
                                        ContextMenuName_ = L["StockFicheContextAddStockIncome"],
                                        IsViewed = false,

                                        ModuleName_ = notTemplate.ModuleName_,
                                        ProcessName_ = notTemplate.ProcessName_,
                                        RecordNumber = input.FicheNo,
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
                                    ContextMenuName_ = L["StockFicheContextAddStockIncome"],
                                    IsViewed = false,

                                    ModuleName_ = notTemplate.ModuleName_,
                                    ProcessName_ = notTemplate.ProcessName_,
                                    RecordNumber = input.FicheNo,
                                    NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                    UserId = new Guid(notTemplate.TargetUsersId),
                                    ViewDate = null,
                                };

                                await _NotificationsAppService.CreateAsync(createInput);
                            }
                        }

                    }

                    #endregion
                    break;

                case 51:
                    StockMovementsService.InsertTotalGoodIssues(input);
                    #region Notification


                    if (notTemplate != null && notTemplate.Id != Guid.Empty)
                    {
                        if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                        {
                            if (notTemplate.TargetUsersId.Contains("*Not*"))
                            {
                                string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                                foreach (string user in usersNot)
                                {
                                    CreateNotificationsDto createInput = new CreateNotificationsDto
                                    {
                                        ContextMenuName_ = L["StockFicheContextAddStockOutput"],
                                        IsViewed = false,

                                        ModuleName_ = notTemplate.ModuleName_,
                                        ProcessName_ = notTemplate.ProcessName_,
                                        RecordNumber = input.FicheNo,
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
                                    ContextMenuName_ = L["StockFicheContextAddStockOutput"],
                                    IsViewed = false,

                                    ModuleName_ = notTemplate.ModuleName_,
                                    ProcessName_ = notTemplate.ProcessName_,
                                    RecordNumber = input.FicheNo,
                                    NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                    UserId = new Guid(notTemplate.TargetUsersId),
                                    ViewDate = null,
                                };

                                await _NotificationsAppService.CreateAsync(createInput);
                            }
                        }

                    }

                    #endregion
                    break;

                case 55:
                    StockMovementsService.InsertTotalReserveds(input);
                    #region Notification


                    if (notTemplate != null && notTemplate.Id != Guid.Empty)
                    {
                        if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                        {
                            if (notTemplate.TargetUsersId.Contains("*Not*"))
                            {
                                string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                                foreach (string user in usersNot)
                                {
                                    CreateNotificationsDto createInput = new CreateNotificationsDto
                                    {
                                        ContextMenuName_ = L["StockFicheContextAddReserved"],
                                        IsViewed = false,

                                        ModuleName_ = notTemplate.ModuleName_,
                                        ProcessName_ = notTemplate.ProcessName_,
                                        RecordNumber = input.FicheNo,
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
                                    ContextMenuName_ = L["StockFicheContextAddReserved"],
                                    IsViewed = false,

                                    ModuleName_ = notTemplate.ModuleName_,
                                    ProcessName_ = notTemplate.ProcessName_,
                                    RecordNumber = input.FicheNo,
                                    NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                    UserId = new Guid(notTemplate.TargetUsersId),
                                    ViewDate = null,
                                };

                                await _NotificationsAppService.CreateAsync(createInput);
                            }
                        }

                    }

                    #endregion

                    break;

                case 25:
                    StockMovementsService.InsertTotalWarehouseShippings(input);
                    #region Notification


                    if (notTemplate != null && notTemplate.Id != Guid.Empty)
                    {
                        if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                        {
                            if (notTemplate.TargetUsersId.Contains("*Not*"))
                            {
                                string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                                foreach (string user in usersNot)
                                {
                                    CreateNotificationsDto createInput = new CreateNotificationsDto
                                    {
                                        ContextMenuName_ = L["StockFicheContextAddWarehouse"],
                                        IsViewed = false,

                                        ModuleName_ = notTemplate.ModuleName_,
                                        ProcessName_ = notTemplate.ProcessName_,
                                        RecordNumber = input.FicheNo,
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
                                    ContextMenuName_ = L["StockFicheContextAddWarehouse"],
                                    IsViewed = false,

                                    ModuleName_ = notTemplate.ModuleName_,
                                    ProcessName_ = notTemplate.ProcessName_,
                                    RecordNumber = input.FicheNo,
                                    NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                    UserId = new Guid(notTemplate.TargetUsersId),
                                    ViewDate = null,
                                };

                                await _NotificationsAppService.CreateAsync(createInput);
                            }
                        }

                    }

                    #endregion
                    break;
            }

            #endregion



            await FicheNumbersAppService.UpdateFicheNumberAsync("StockFichesChildMenu", input.FicheNo);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.StockFiches, LogType.Insert, addedEntityId);


            return new SuccessDataResult<SelectStockFichesDto>(stockFiche);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.StockFiches).Select("*").Where(new { Id = id }, "");

            var StockFiches = queryFactory.Get<SelectStockFichesDto>(query);

            if (StockFiches.Id != Guid.Empty && StockFiches != null)
            {
                #region Stock Movement Service

                switch (StockFiches.FicheType)
                {
                    case StockFicheTypeEnum.FireFisi: StockMovementsService.DeleteTotalWastages(StockFiches); ; break;
                    case StockFicheTypeEnum.SarfFisi: StockMovementsService.DeleteTotalConsumptions(StockFiches); ; break;
                    case StockFicheTypeEnum.UretimdenGirisFisi: StockMovementsService.DeleteTotalProductions(StockFiches); ; break;
                    case StockFicheTypeEnum.StokGirisFisi: StockMovementsService.DeleteTotalGoods(StockFiches); ; break;
                    case StockFicheTypeEnum.StokCikisFisi: StockMovementsService.DeleteTotalGoodIssues(StockFiches); ; break;
                    case StockFicheTypeEnum.DepoSevkFisi: StockMovementsService.DeleteTotalWarehouseShippings(StockFiches); break;
                    case StockFicheTypeEnum.StokRezerveFisi: StockMovementsService.DeleteTotalReserveds(StockFiches); break;
                }

                #endregion

                var deleteQuery = queryFactory.Query().From(Tables.StockFiches).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.StockFicheLines).Delete(LoginedUserService.UserId).Where(new { StockFicheID = id }, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var stockFiche = queryFactory.Update<SelectStockFichesDto>(deleteQuery, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.StockFiches, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["StockFichesChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

                if (notTemplate != null && notTemplate.Id != Guid.Empty)
                {
                    if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                    {
                        if (notTemplate.TargetUsersId.Contains("*Not*"))
                        {
                            string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                            foreach (string user in usersNot)
                            {
                                CreateNotificationsDto createInput = new CreateNotificationsDto
                                {
                                    ContextMenuName_ = notTemplate.ContextMenuName_,
                                    IsViewed = false,

                                    ModuleName_ = notTemplate.ModuleName_,
                                    ProcessName_ = notTemplate.ProcessName_,
                                    RecordNumber = entity.FicheNo,
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

                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = entity.FicheNo,
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
                return new SuccessDataResult<SelectStockFichesDto>(stockFiche);
            }
            else
            {
                var queryLineGet = queryFactory.Query().From(Tables.StockFicheLines).Select("*").Where(new { Id = id }, "");

                var stockFichesLineGet = queryFactory.Get<SelectStockFicheLinesDto>(queryLineGet);

                var queryDeleteEntity = queryFactory.Query().From(Tables.StockFiches).Select("*").Where(new { Id = stockFichesLineGet.StockFicheID }, "");

                var StockFichesDeleteEntity = queryFactory.Get<SelectStockFichesDto>(queryDeleteEntity);

                #region Stock Movement Service

                switch (StockFichesDeleteEntity.FicheType)
                {
                    case StockFicheTypeEnum.FireFisi: StockMovementsService.DeleteTotalWastageLines(stockFichesLineGet); ; break;
                    case StockFicheTypeEnum.SarfFisi: StockMovementsService.DeleteTotalConsumptionLines(stockFichesLineGet); ; break;
                    case StockFicheTypeEnum.UretimdenGirisFisi: StockMovementsService.DeleteTotalProductionLines(stockFichesLineGet); ; break;
                    case StockFicheTypeEnum.StokGirisFisi: StockMovementsService.DeleteTotalGoodLines(stockFichesLineGet); ; break;
                    case StockFicheTypeEnum.StokCikisFisi: StockMovementsService.DeleteTotalGoodIssueLines(stockFichesLineGet); ; break;
                    case StockFicheTypeEnum.DepoSevkFisi: StockMovementsService.DeleteTotalWarehouseShippingLines(stockFichesLineGet); break;
                    case StockFicheTypeEnum.StokRezerveFisi: StockMovementsService.DeleteTotalReservedLines(stockFichesLineGet); break;
                }

                #endregion

                var queryLine = queryFactory.Query().From(Tables.StockFicheLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var stockFicheLines = queryFactory.Update<SelectStockFicheLinesDto>(queryLine, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.StockFicheLines, LogType.Delete, id);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectStockFicheLinesDto>(stockFicheLines);
            }

        }

        public async Task<IDataResult<SelectStockFichesDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.StockFiches)
                   .Select<StockFiches>(null)
                   .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchID = b.Id },
                        nameof(StockFiches.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<PurchaseOrders>
                    (
                        b => new { PurchaseOrderFicheNo = b.FicheNo, PurchaseOrderID = b.Id },
                        nameof(StockFiches.PurchaseOrderID),
                        nameof(PurchaseOrders.Id),
                        JoinType.Left
                    )
                      .Join<PurchaseRequests>
                    (
                        b => new { PurchaseRequestFicheNo = b.FicheNo, PurchaseRequestID = b.Id },
                        nameof(StockFiches.PurchaseRequestID),
                        nameof(PurchaseRequests.Id),
                        JoinType.Left
                    )
                   .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code, WarehouseID = w.Id },
                        nameof(StockFiches.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        w => new { CurrencyCode = w.Code, CurrencyID = w.Id },
                        nameof(StockFiches.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(StockFiches.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                     .Join<ProductionOrders>
                    (
                        w => new { ProductionOrderCode = w.FicheNo, ProductionOrderID = w.Id },
                        nameof(StockFiches.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, Tables.StockFiches);

            var stockFiches = queryFactory.Get<SelectStockFichesDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.StockFicheLines)
                   .Select<StockFicheLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductCode = p.Code, ProductName = p.Name },
                        nameof(StockFicheLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                     .Join<PurchaseOrders>
                    (
                        b => new { PurchaseOrderFicheNo = b.FicheNo, PurchaseOrderID = b.Id },
                        nameof(StockFicheLines.PurchaseOrderID),
                        nameof(PurchaseOrders.Id),
                        "PurchaseOrderLine",
                        JoinType.Left
                    )
                     .Join<PurchaseOrderLines>
                    (
                        b => new { PurchaseOrderLineID = b.Id },
                        nameof(StockFicheLines.PurchaseOrderLineID),
                        nameof(PurchaseOrderLines.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(StockFicheLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                    .Join<ProductionOrders>
                    (
                        u => new { ProductionOrderID = u.Id, ProductionOrderFicheNo = u.FicheNo },
                        nameof(StockFicheLines.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Where(new { StockFicheID = id }, Tables.StockFicheLines);

            var stockFicheLine = queryFactory.GetList<SelectStockFicheLinesDto>(queryLines).ToList();

            stockFiches.SelectStockFicheLines = stockFicheLine;

            LogsAppService.InsertLogToDatabase(stockFiches, stockFiches, LoginedUserService.UserId, Tables.StockFiches, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStockFichesDto>(stockFiches);

        }

        public async Task<IDataResult<IList<ListStockFichesDto>>> GetListAsync(ListStockFichesParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.StockFiches)
                   .Select<StockFiches>(s => new { s.FicheNo, s.Date_, s.Description_, s.FicheType, s.NetAmount, s.Id })
                    .Join<PurchaseOrders>
                    (
                        b => new { PurchaseOrderFicheNo = b.FicheNo, PurchaseOrderID = b.Id },
                        nameof(StockFiches.PurchaseOrderID),
                        nameof(PurchaseOrders.Id),
                        JoinType.Left
                    )
                   .Join<Branches>
                    (
                        b => new { BranchCode = b.Code },
                        nameof(StockFiches.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                       .Join<PurchaseRequests>
                    (
                        b => new { PurchaseRequestFicheNo = b.FicheNo, PurchaseRequestID = b.Id },
                        nameof(StockFiches.PurchaseRequestID),
                        nameof(PurchaseRequests.Id),
                        JoinType.Left
                    )
                   .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code },
                        nameof(StockFiches.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        w => new { CurrencyCode = w.Code, CurrencyID = w.Id },
                        nameof(StockFiches.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(StockFiches.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                    .Where(null, Tables.StockFiches);

            var stockFichesDto = queryFactory.GetList<ListStockFichesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListStockFichesDto>>(stockFichesDto);

        }

        public async Task<IDataResult<IList<ListStockFichesDto>>> GetListbyPurchaseOrderAsync(Guid purchaseOrderID)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.StockFiches)
                   .Select<StockFiches>(null)
                    .Join<PurchaseOrders>
                    (
                        b => new { PurchaseOrderFicheNo = b.FicheNo, PurchaseOrderID = b.Id },
                        nameof(StockFiches.PurchaseOrderID),
                        nameof(PurchaseOrders.Id),
                        JoinType.Left
                    )
                   .Join<Branches>
                    (
                        b => new { BranchCode = b.Code },
                        nameof(StockFiches.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                       .Join<PurchaseRequests>
                    (
                        b => new { PurchaseRequestFicheNo = b.FicheNo, PurchaseRequestID = b.Id },
                        nameof(StockFiches.PurchaseRequestID),
                        nameof(PurchaseRequests.Id),
                        JoinType.Left
                    )
                   .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code },
                        nameof(StockFiches.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        w => new { CurrencyCode = w.Code, CurrencyID = w.Id },
                        nameof(StockFiches.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(StockFiches.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                    .Where(new { PurchaseOrderID = purchaseOrderID }, Tables.StockFiches);

            var stockFichesDto = queryFactory.GetList<ListStockFichesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListStockFichesDto>>(stockFichesDto);

        }

        public async Task<IDataResult<IList<ListStockFicheLinesDto>>> GetLineConsumeListbyProductIDAsync(Guid productID)
        {
            var queryLines = queryFactory
                   .Query()
                   .From(Tables.StockFicheLines)
                   .Select<StockFicheLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductCode = p.Code, ProductName = p.Name },
                        nameof(StockFicheLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                     .Join<PurchaseOrders>
                    (
                        b => new { PurchaseOrderFicheNo = b.FicheNo, PurchaseOrderID = b.Id },
                        nameof(StockFicheLines.PurchaseOrderID),
                        nameof(PurchaseOrders.Id),
                        "PurchaseOrderLine",
                        JoinType.Left
                    )
                     .Join<PurchaseOrderLines>
                    (
                        b => new { PurchaseOrderLineID = b.Id },
                        nameof(StockFicheLines.PurchaseOrderLineID),
                        nameof(PurchaseOrderLines.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetCode = u.Code },
                        nameof(StockFicheLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    ).Join<ProductionOrders>
                    (
                        u => new { ProductionOrderID = u.Id, ProductionOrderFicheNo = u.FicheNo },
                        nameof(StockFicheLines.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Where(new { ProductID = productID, FicheType = 12 }, Tables.StockFicheLines);

            var stockFicheLine = queryFactory.GetList<ListStockFicheLinesDto>(queryLines).ToList();


            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListStockFicheLinesDto>>(stockFicheLine);

        }

        public async Task<IDataResult<IList<ListStockFicheLinesDto>>> GetLineWastageListbyProductIDAsync(Guid productID)
        {
            var queryLines = queryFactory
                   .Query()
                   .From(Tables.StockFicheLines)
                   .Select<StockFicheLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductCode = p.Code, ProductName = p.Name },
                        nameof(StockFicheLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                     .Join<PurchaseOrders>
                    (
                        b => new { PurchaseOrderFicheNo = b.FicheNo, PurchaseOrderID = b.Id },
                        nameof(StockFicheLines.PurchaseOrderID),
                        nameof(PurchaseOrders.Id),
                        "PurchaseOrderLine",
                        JoinType.Left
                    )
                     .Join<PurchaseOrderLines>
                    (
                        b => new { PurchaseOrderLineID = b.Id },
                        nameof(StockFicheLines.PurchaseOrderLineID),
                        nameof(PurchaseOrderLines.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetCode = u.Code },
                        nameof(StockFicheLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    ).Join<ProductionOrders>
                    (
                        u => new { ProductionOrderID = u.Id, ProductionOrderFicheNo = u.FicheNo },
                        nameof(StockFicheLines.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Where(new { ProductID = productID, FicheType = 11 }, Tables.StockFicheLines);

            var stockFicheLine = queryFactory.GetList<ListStockFicheLinesDto>(queryLines).ToList();


            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListStockFicheLinesDto>>(stockFicheLine);

        }

        public async Task<IDataResult<IList<ListStockFichesDto>>> GetListbyProductionOrderAsync(Guid productionOrderID)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.StockFiches)
                   .Select<StockFiches>(null)
                    .Join<PurchaseOrders>
                    (
                        b => new { PurchaseOrderFicheNo = b.FicheNo, PurchaseOrderID = b.Id },
                        nameof(StockFiches.PurchaseOrderID),
                        nameof(PurchaseOrders.Id),
                        JoinType.Left
                    )
                       .Join<PurchaseRequests>
                    (
                        b => new { PurchaseRequestFicheNo = b.FicheNo, PurchaseRequestID = b.Id },
                        nameof(StockFiches.PurchaseRequestID),
                        nameof(PurchaseRequests.Id),
                        JoinType.Left
                    )
                   .Join<Branches>
                    (
                        b => new { BranchCode = b.Code },
                        nameof(StockFiches.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                   .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code },
                        nameof(StockFiches.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        w => new { CurrencyCode = w.Code, CurrencyID = w.Id },
                        nameof(StockFiches.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(StockFiches.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                    .Where(new { ProductionOrderID = productionOrderID }, Tables.StockFiches);

            var stockFichesDto = queryFactory.GetList<ListStockFichesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListStockFichesDto>>(stockFichesDto);

        }

        [ValidationAspect(typeof(UpdateStockFichesValidatorDto), Priority = 1)]
        public async Task<IDataResult<SelectStockFichesDto>> UpdateAsync(UpdateStockFichesDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.StockFiches)
                   .Select<StockFiches>(null)
                    .Join<PurchaseOrders>
                    (
                        b => new { PurchaseOrderFicheNo = b.FicheNo, PurchaseOrderID = b.Id },
                        nameof(StockFiches.PurchaseOrderID),
                        nameof(PurchaseOrders.Id),
                        JoinType.Left
                    )
                       .Join<PurchaseRequests>
                    (
                        b => new { PurchaseRequestFicheNo = b.FicheNo, PurchaseRequestID = b.Id },
                        nameof(StockFiches.PurchaseRequestID),
                        nameof(PurchaseRequests.Id),
                        JoinType.Left
                    )
                   .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchID = b.Id },
                        nameof(StockFiches.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                   .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code, WarehouseID = w.Id },
                        nameof(StockFiches.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        w => new { CurrencyCode = w.Code, CurrencyID = w.Id },
                        nameof(StockFiches.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(StockFiches.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.StockFiches);

            var entity = queryFactory.Get<SelectStockFichesDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.StockFicheLines)
                   .Select<StockFicheLines>(null)
                     .Join<PurchaseOrders>
                    (
                        b => new { PurchaseOrderFicheNo = b.FicheNo, PurchaseOrderID = b.Id },
                        nameof(StockFicheLines.PurchaseOrderID),
                        nameof(PurchaseOrders.Id),
                        "PurchaseOrderLine",
                        JoinType.Left
                    )
                     .Join<PurchaseOrderLines>
                    (
                        b => new { PurchaseOrderLineID = b.Id },
                        nameof(StockFicheLines.PurchaseOrderLineID),
                        nameof(PurchaseOrderLines.Id),
                        JoinType.Left
                    )
                   .Join<Products>
                    (
                        p => new { ProductCode = p.Code, ProductName = p.Name },
                        nameof(StockFicheLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(StockFicheLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    ).Join<ProductionOrders>
                    (
                        u => new { ProductionOrderID = u.Id, ProductionOrderFicheNo = u.FicheNo },
                        nameof(StockFicheLines.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Where(new { StockFicheID = input.Id }, Tables.StockFicheLines);

            var stockFicheLine = queryFactory.GetList<SelectStockFicheLinesDto>(queryLines).ToList();

            entity.SelectStockFicheLines = stockFicheLine;

            #region Update Control
            var listQuery = queryFactory.Query().From(Tables.StockFiches).Select("*").Where(new { FicheNo = input.FicheNo, FicheType = input.FicheType }, "");

            var list = queryFactory.GetList<StockFiches>(listQuery).ToList();

            if (list.Count > 0 && entity.FicheNo != input.FicheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            if (input.FicheType != 25)
            {
                switch (input.FicheType)
                {
                    case 12: input.InputOutputCode = 1; break;
                    case 11: input.InputOutputCode = 1; break;
                    case 13: input.InputOutputCode = 0; break;
                    case 50: input.InputOutputCode = 0; break;
                    case 51: input.InputOutputCode = 1; break;
                    case 55: input.InputOutputCode = 1; break;
                }
            }


            var query = queryFactory.Query().From(Tables.StockFiches).Update(new UpdateStockFichesDto
            {
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                InputOutputCode = input.InputOutputCode,
                PurchaseOrderID = input.PurchaseOrderID.GetValueOrDefault(),
                TransactionExchangeCurrencyID = input.TransactionExchangeCurrencyID.GetValueOrDefault(),
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                PurchaseRequestID = input.PurchaseRequestID.GetValueOrDefault(),
                ProductionDateReferance = input.ProductionDateReferance,
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                SalesInvoiceID = input.SalesInvoiceID.GetValueOrDefault(),
                PurchaseInvoiceID = input.PurchaseInvoiceID.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
                BranchID = input.BranchID,
                CurrencyID = input.CurrencyID,
                Date_ = input.Date_,
                Description_ = input.Description_,
                ExchangeRate = input.ExchangeRate,
                FicheNo = input.FicheNo,
                FicheType = input.FicheType,
                ProductionOrderID = input.ProductionOrderID,
                SpecialCode = input.SpecialCode,
                Time_ = input.Time_,
                WarehouseID = input.WarehouseID

            }).Where(new { Id = input.Id }, "");

            var stockParameterDataSource = (await StockManagementParametersAppService.GetStockManagementParametersAsync()).Data;

            bool autoCostParameter = stockParameterDataSource.AutoCostParameter;
            int costCalculateType = stockParameterDataSource.CostCalculationMethod;

            foreach (var item in input.SelectStockFicheLines)
            {
                if (item.Id == Guid.Empty)
                {

                    decimal productCost = 0;

                    if (item.FicheType == StockFicheTypeEnum.FireFisi || item.FicheType == StockFicheTypeEnum.SarfFisi || item.FicheType == StockFicheTypeEnum.StokCikisFisi || item.FicheType == StockFicheTypeEnum.StokRezerveFisi)
                    {
                        if (autoCostParameter)
                        {
                            if (costCalculateType == 1)
                            {
                                var outputList = (await GetOutputList(item.ProductID.GetValueOrDefault())).ToList();
                                outputList.Add(item);
                                productCost = (await CalculateProductFIFOCostAsync(item.ProductID.GetValueOrDefault(), outputList));
                            }

                            if (costCalculateType == 2)
                            {
                                var outputList = (await GetOutputList(item.ProductID.GetValueOrDefault())).ToList();
                                outputList.Add(item);
                                productCost = (await CalculateProductLIFOCostAsync(item.ProductID.GetValueOrDefault(), outputList));
                            }
                        }
                    }

                    var queryLine = queryFactory.Query().From(Tables.StockFicheLines).Insert(new CreateStockFicheLinesDto
                    {
                        CreationTime = now,
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        PurchaseOrderID = item.PurchaseOrderID.GetValueOrDefault(),
                        PurchaseOrderLineID = item.PurchaseOrderLineID.GetValueOrDefault(),
                        TransactionExchangeUnitPrice = item.TransactionExchangeUnitPrice,
                        TransactionExchangeLineAmount = item.TransactionExchangeLineAmount,
                        MRPID = item.MRPID.GetValueOrDefault(),
                        InputOutputCode = input.InputOutputCode,
                        PartyNo = item.PartyNo,
                        ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                        PurchaseInvoiceID = item.PurchaseInvoiceID.GetValueOrDefault(),
                        SalesInvoiceID = item.SalesInvoiceID.GetValueOrDefault(),
                        SalesInvoiceLineID = item.SalesInvoiceLineID.GetValueOrDefault(),
                        PurchaseInvoiceLineID = item.SalesInvoiceLineID.GetValueOrDefault(),
                        MRPLineID = item.MRPID.GetValueOrDefault(),
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        Id = GuidGenerator.CreateGuid(),
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                        LineNr = item.LineNr,
                        ProductionDateReferance = item.ProductionDateReferance,
                        ProductID = item.ProductID.GetValueOrDefault(),
                        Quantity = item.Quantity,
                        UnitSetID = item.UnitSetID.GetValueOrDefault(),
                        FicheType = (int)item.FicheType,
                        LineAmount = item.LineAmount,
                        LineDescription = item.LineDescription,
                        StockFicheID = input.Id,
                        UnitPrice = item.UnitPrice,
                        Date_ = input.Date_,
                        UnitOutputCost = productCost
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {

                    decimal productCost = 0;

                    if (item.FicheType == StockFicheTypeEnum.FireFisi || item.FicheType == StockFicheTypeEnum.SarfFisi || item.FicheType == StockFicheTypeEnum.StokCikisFisi || item.FicheType == StockFicheTypeEnum.StokRezerveFisi)
                    {
                        if (autoCostParameter)
                        {
                            if (costCalculateType == 1)
                            {
                                var outputList = (await GetOutputList(item.ProductID.GetValueOrDefault())).ToList();
                                outputList.Add(item);
                                productCost = (await CalculateProductFIFOCostAsync(item.ProductID.GetValueOrDefault(), outputList));
                            }

                            if (costCalculateType == 2)
                            {
                                var outputList = (await GetOutputList(item.ProductID.GetValueOrDefault())).ToList();
                                outputList.Add(item);
                                productCost = (await CalculateProductLIFOCostAsync(item.ProductID.GetValueOrDefault(), outputList));
                            }
                        }
                    }

                    var lineGetQuery = queryFactory.Query().From(Tables.StockFicheLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectStockFicheLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.StockFicheLines).Update(new UpdateStockFicheLinesDto
                        {
                            StockFicheID = input.Id,
                            CreationTime = line.CreationTime,
                            PurchaseOrderID = item.PurchaseOrderID.GetValueOrDefault(),
                            PurchaseOrderLineID = item.PurchaseOrderLineID.GetValueOrDefault(),
                            MRPID = item.MRPID.GetValueOrDefault(),
                            MRPLineID = item.MRPID.GetValueOrDefault(),
                            TransactionExchangeUnitPrice = item.TransactionExchangeUnitPrice,
                            TransactionExchangeLineAmount = item.TransactionExchangeLineAmount,
                            CreatorId = line.CreatorId,
                            InputOutputCode = input.InputOutputCode,
                            PartyNo = item.PartyNo,
                            ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            PurchaseInvoiceID = item.PurchaseInvoiceID.GetValueOrDefault(),
                            SalesInvoiceID = item.SalesInvoiceID.GetValueOrDefault(),
                            SalesInvoiceLineID = item.SalesInvoiceLineID.GetValueOrDefault(),
                            PurchaseInvoiceLineID = item.SalesInvoiceLineID.GetValueOrDefault(),
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            Id = item.Id,
                            ProductionDateReferance = item.ProductionDateReferance,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                            ProductID = item.ProductID.GetValueOrDefault(),
                            Quantity = item.Quantity,
                            UnitSetID = item.UnitSetID.GetValueOrDefault(),
                            FicheType = (int)item.FicheType,
                            LineAmount = item.LineAmount,
                            LineDescription = item.LineDescription,
                            UnitPrice = item.UnitPrice,
                            Date_ = input.Date_,
                            UnitOutputCost = productCost
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var stockFiche = queryFactory.Update<SelectStockFichesDto>(query, "Id", true);

            #region Stock Movement Service

            switch (input.FicheType)
            {
                case 11: StockMovementsService.UpdateTotalWastages(entity, input); break;
                case 12: StockMovementsService.UpdateTotalConsumptions(entity, input); break;
                case 13: StockMovementsService.UpdateTotalProductions(entity, input); break;
                case 50: StockMovementsService.UpdateTotalGoods(entity, input); break;
                case 51: StockMovementsService.UpdateTotalGoodIssues(entity, input); break;
                case 25: StockMovementsService.UpdateTotalWarehouseShippings(entity, input); break;
                case 55: StockMovementsService.UpdateTotalReserveds(entity, input); break;
            }

            #endregion

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.StockFiches, LogType.Update, entity.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["StockFichesChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains("*Not*"))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,

                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = input.FicheNo,
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

                            ModuleName_ = notTemplate.ModuleName_,
                            ProcessName_ = notTemplate.ProcessName_,
                            RecordNumber = input.FicheNo,
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
            return new SuccessDataResult<SelectStockFichesDto>(stockFiche);

        }

        public async Task<IDataResult<SelectStockFichesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.StockFiches).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<StockFiches>(entityQuery);

            var query = queryFactory.Query().From(Tables.StockFiches).Update(new UpdateStockFichesDto
            {
                FicheNo = entity.FicheNo,
                CreationTime = entity.CreationTime.Value,
                TransactionExchangeCurrencyID = entity.TransactionExchangeCurrencyID,
                PurchaseRequestID = entity.PurchaseRequestID,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                PurchaseInvoiceID = entity.PurchaseInvoiceID,
                SalesInvoiceID = entity.SalesInvoiceID,
                ProductionDateReferance = entity.ProductionDateReferance,
                PurchaseOrderID = entity.PurchaseOrderID,
                Id = entity.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                BranchID = entity.BranchID,
                CurrencyID = entity.CurrencyID,
                Date_ = entity.Date_,
                Description_ = entity.Description_,
                InputOutputCode = entity.InputOutputCode,
                ExchangeRate = entity.ExchangeRate,
                FicheType = (int)entity.FicheType,
                ProductionOrderID = entity.ProductionOrderID,
                SpecialCode = entity.SpecialCode,
                Time_ = entity.Time_,
                WarehouseID = entity.WarehouseID

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var stockFichesDto = queryFactory.Update<SelectStockFichesDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectStockFichesDto>(stockFichesDto);

        }

        public async Task<List<SelectStockFicheLinesDto>> GetInputList(Guid productId, DateTime? startDate = null, DateTime? endDate = null)
        {
            string resultQuery = "SELECT * FROM " + Tables.StockFicheLines;

            string where = " ProductID='" + productId + "' and FicheType In (" + 13 + "," + 50 + ")";

            if (startDate.HasValue && endDate.HasValue)
            {
                where = where + " and (Date_>='" + startDate + "' and '" + endDate + "'<=Date_) ";
            }

            Query query = new Query();
            query.Sql = resultQuery;
            query.WhereSentence = where;
            query.UseIsDeleteInQuery = false;
            var stockFicheLine = queryFactory.GetList<SelectStockFicheLinesDto>(query).ToList();
            await Task.CompletedTask;

            return stockFicheLine;
        }

        public async Task<List<SelectStockFicheLinesDto>> GetOutputList(Guid productId, DateTime? startDate = null, DateTime? endDate = null)
        {
            string resultQuery = "SELECT * FROM " + Tables.StockFicheLines;

            string where = " ProductID='" + productId + "' and FicheType In (" + 11 + "," + 12 + "," + 51 + ")";

            if (startDate.HasValue && endDate.HasValue)
            {
                where = where + " and (Date_>='" + startDate + "' and '" + endDate + "'<=Date_) ";
            }

            Query query = new Query();
            query.Sql = resultQuery;
            query.WhereSentence = where;
            query.UseIsDeleteInQuery = false;
            var stockFicheLine = queryFactory.GetList<SelectStockFicheLinesDto>(query).ToList();
            await Task.CompletedTask;

            return stockFicheLine;
        }

        public async Task<decimal> CalculateProductFIFOCostAsync(Guid productId, List<SelectStockFicheLinesDto> outputList)
        {
            decimal productCost = 0;
            var inputList = (await GetInputList(productId)).OrderBy(t => t.Date_).ToList();
            //var outputList = (await GetOutputList(productId)).OrderBy(t => t.Date_).ToList();

            decimal inputBalance = 0m;

            for (int c = 0; c < outputList.Count; c++)
            {
                decimal output = outputList[c].Quantity;
                decimal outputSum = output;

                productCost = 0;

                for (int g = 0; g < inputList.Count; g++)
                {
                    decimal input = inputBalance;

                    if (input == 0)
                    {
                        input = inputList[g].Quantity;
                    }

                    if (input == 0)
                    {
                        inputBalance = 0;
                        inputList.RemoveAt(g);
                        g--;
                        continue;
                    }

                    if (input >= output)
                    {
                        decimal unitprice = inputList[g].UnitPrice;
                        productCost = (productCost + (output * unitprice));
                        productCost = productCost / outputSum;
                        input = input - output;
                        inputBalance = input;


                        break;
                    }
                    else
                    {
                        decimal unitprice = inputList[g].UnitPrice;
                        output = output - input;
                        productCost = (productCost + (input * unitprice));
                        inputBalance = 0;

                        inputList.RemoveAt(g);
                        g--;
                    }
                }
            }

            return productCost;
        }

        public async Task<decimal> CalculateProductLIFOCostAsync(Guid productId, List<SelectStockFicheLinesDto> outputList)
        {
            decimal productCost = 0;
            var inputList = (await GetInputList(productId)).OrderByDescending(t => t.Date_).ToList();
            //var outputList = (await GetOutputList(productId)).OrderBy(t => t.Date_).ToList();

            decimal inputBalance = 0m;

            for (int c = 0; c < outputList.Count; c++)
            {
                decimal output = outputList[c].Quantity;
                decimal outputSum = output;

                productCost = 0;

                for (int g = 0; g < inputList.Count; g++)
                {
                    decimal input = inputBalance;

                    if (input == 0)
                    {
                        input = inputList[g].Quantity;
                    }

                    if (input == 0)
                    {
                        inputBalance = 0;
                        inputList.RemoveAt(g);
                        g--;
                        continue;
                    }

                    if (input >= output)
                    {
                        decimal unitprice = inputList[g].UnitPrice;
                        productCost = (productCost + (output * unitprice));
                        productCost = productCost / outputSum;
                        input = input - output;
                        inputBalance = input;


                        break;
                    }
                    else
                    {
                        decimal unitprice = inputList[g].UnitPrice;
                        output = output - input;
                        productCost = (productCost + (input * unitprice));
                        inputBalance = 0;

                        inputList.RemoveAt(g);
                        g--;
                    }
                }
            }

            return productCost;
        }

        public async Task<List<SelectStockFicheLinesDto>> GetbyProductionOrderAsync(Guid ProductionOrderID)
        {
            var IdQuery = queryFactory.Query().From(Tables.StockFiches).Select<StockFiches>(null).Where(new { ProductionOrderID = ProductionOrderID }, "");
            var stockFicheIds = queryFactory.GetList<SelectStockFichesDto>(IdQuery).ToList();

            string ficheIds = "";

            for (int i = 0; i < stockFicheIds.Count; i++)
            {
                if (i == 0)
                {
                    ficheIds = "'" + stockFicheIds[i].Id.ToString() + "'";
                }
                else
                {
                    ficheIds = ficheIds + "," + "'" + stockFicheIds[i].Id.ToString() + "'";
                }
            }

            string where = " StockFicheID In (" + ficheIds + ")";

            string resultQuery = "SELECT * FROM " + Tables.StockFicheLines;
            Query query = new Query();
            query.Sql = resultQuery;
            query.WhereSentence = where;
            query.UseIsDeleteInQuery = false;

            var stockFicheLine = queryFactory.GetList<SelectStockFicheLinesDto>(query).ToList();
            await Task.CompletedTask;

            return stockFicheLine;


        }

        public async Task<List<SelectStockFicheLinesDto>> GetbyProductionOrderDateReferenceAsync(string DateReference, Guid ProductionOrderID)
        {
            var IdQuery = queryFactory.Query().From(Tables.StockFiches).Select<StockFiches>(null).Where(new { ProductionDateReferance = DateReference }, "").Where(new { ProductionOrderID = ProductionOrderID }, "");
            var stockFicheIds = queryFactory.GetList<SelectStockFichesDto>(IdQuery).ToList();

            string ficheIds = "";

            for (int i = 0; i < stockFicheIds.Count; i++)
            {
                if (i == 0)
                {
                    ficheIds = "'" + stockFicheIds[i].Id.ToString() + "'";
                }
                else
                {
                    ficheIds = ficheIds + "," + "'" + stockFicheIds[i].Id.ToString() + "'";
                }
            }

            string where = " StockFicheID In (" + ficheIds + ")";

            string resultQuery = "SELECT * FROM " + Tables.StockFicheLines;
            Query query = new Query();
            query.Sql = resultQuery;
            query.WhereSentence = where;
            query.UseIsDeleteInQuery = false;

            var stockFicheLine = queryFactory.GetList<SelectStockFicheLinesDto>(query).ToList();
            await Task.CompletedTask;

            return stockFicheLine;
        }

        public async Task<IDataResult<IList<ProductMovementsDto>>> GetProductMovementsByProductIDAsync(Guid ProductID)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.StockFicheLines)
                   .Select<StockFicheLines>(s => new
                   {
                       s.StockFicheID,
                       s.Quantity,
                       s.UnitPrice,
                       s.LineAmount,
                       s.ProductID,
                       s.Date_,
                       s.InputOutputCode,
                       s.UnitSetID,
                       s.PartyNo,
                       s.ProductionDateReferance,
                       s.PurchaseOrderID,
                       s.ProductionOrderID
                   })

                    .Join<StockFiches>
                    (
                        b => new { StockFicheNr = b.FicheNo, StockFicheType = b.FicheType },
                        nameof(StockFicheLines.StockFicheID),
                        nameof(StockFiches.Id),
                        "StockFiches",
                        JoinType.Left
                    )

                    .Join<Products>
                    (
                        b => new { ProductCode = b.Code, ProductName = b.Name },
                        nameof(StockFicheLines.ProductID),
                        nameof(Products.Id),
                        "Products",
                        JoinType.Left
                    )

                    .Join<UnitSets>
                    (
                        b => new { UnitSetName = b.Name },
                        nameof(StockFicheLines.UnitSetID),
                        nameof(UnitSets.Id),
                        "UnitSets",
                        JoinType.Left
                    )
                    .Where(new { ProductID = ProductID }, Tables.StockFicheLines);

            var result = queryFactory.GetList<ProductMovementsDto>(query).ToList();

            foreach (var item in result)
            {
                #region Stok Fiş Türü Adı
                switch (item.StockFicheType)
                {
                    case StockFicheTypeEnum.FireFisi:
                        item.StockFicheTypeName = L["EnumWastage"];
                        break;
                    case StockFicheTypeEnum.SarfFisi:
                        item.StockFicheTypeName = L["EnumConsume"];
                        break;
                    case StockFicheTypeEnum.UretimdenGirisFisi:
                        item.StockFicheTypeName = L["EnumProductionIncome"];
                        break;
                    case StockFicheTypeEnum.DepoSevkFisi:
                        item.StockFicheTypeName = L["EnumWarehouse"];
                        break;
                    case StockFicheTypeEnum.StokGirisFisi:
                        item.StockFicheTypeName = L["EnumStockIncome"];
                        break;
                    case StockFicheTypeEnum.StokCikisFisi:
                        item.StockFicheTypeName = L["EnumStockOutput"];
                        break;
                    case StockFicheTypeEnum.StokRezerveFisi:
                        item.StockFicheTypeName = L["EnumReserved"];
                        break;
                    default:
                        break;
                }
                #endregion

                #region Giriş Çıkış Kodu Açıklaması
                if (item.InputOutputCode == 0) // Giriş
                {
                    item.InputOutputCodeDescripton = L["InputTransaction"];
                }
                else
                {
                    item.InputOutputCodeDescripton = L["OutputTransaction"];
                }
                #endregion

                if (item.ProductionOrderID != null && item.ProductionOrderID != Guid.Empty)
                {
                    item.LinkedModuleName = L["LinkedModuleProductionManagement"];
                    item.LinkedModuleFicheNumber = (await _ProductionOrdersAppService.GetAsync(item.ProductionOrderID.GetValueOrDefault())).Data.FicheNo;
                    item.CurrentAccountCardName = "";
                }

                if (item.PurchaseOrderID != null && item.PurchaseOrderID != Guid.Empty)
                {
                    var purchaseOrder = (await _PurchaseOrdersAppService.GetAsync(item.PurchaseOrderID.GetValueOrDefault())).Data;

                    if (purchaseOrder.Id != Guid.Empty)
                    {
                        item.LinkedModuleName = L["LinkedModulePurchaseManagement"];
                        item.LinkedModuleFicheNumber = purchaseOrder.FicheNo;
                        item.CurrentAccountCardName = purchaseOrder.CurrentAccountCardName;
                    }
                }
            }

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ProductMovementsDto>>(result);
        }
    }
}
