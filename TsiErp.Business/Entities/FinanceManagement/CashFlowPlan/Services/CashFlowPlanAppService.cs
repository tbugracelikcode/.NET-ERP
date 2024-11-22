using Microsoft.Extensions.Localization;
using SqlBulkTools;
using System.Data.SqlClient;
using System.Transactions;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.BankAccount.Services;
using TsiErp.Business.Entities.FinanceManagement.BankBalance.Services;
using TsiErp.Business.Entities.FinanceManagement.CashFlowPlan.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.BankBalance.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.CashFlowPlan;
using TsiErp.Entities.Entities.FinanceManagement.CashFlowPlan.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.CashFlowPlanLine;
using TsiErp.Entities.Entities.FinanceManagement.CashFlowPlanLine.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.CashFlowPlans.Page;

namespace TsiErp.Business.Entities.CashFlowPlan.Services
{
    [ServiceRegistration(typeof(ICashFlowPlansAppService), DependencyInjectionType.Scoped)]
    public class CashFlowPlansAppService : ApplicationService<CashFlowPlansResource>, ICashFlowPlansAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;
        private readonly IBankBalancesAppService _BankBalancesAppService;

        public CashFlowPlansAppService(IStringLocalizer<CashFlowPlansResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService, IBankBalancesAppService bankBalancesAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
            _BankBalancesAppService = bankBalancesAppService;
        }



        public async Task<IDataResult<SelectCashFlowPlansDto>> CreateAsync(CreateCashFlowPlansDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.CashFlowPlans).Select("Code").Where(new { Code = input.Code }, "");
            var list = queryFactory.ControlList<CashFlowPlans>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.CashFlowPlans).Insert(new CreateCashFlowPlansDto
            {
                Code = input.Code,
                Id = addedEntityId,
                Description_ = input.Description_,
                EndDate = input.EndDate,
                IsActive = input.IsActive,
                StartDate = input.StartDate,
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
            });

            foreach (var item in input.SelectCashFlowPlanLines)
            {
                var queryLine = queryFactory.Query().From(Tables.CashFlowPlanLines).Insert(new CreateCashFlowPlanLinesDto
                {
                    Amount_ = item.Amount_,
                    CashFlowPlanID = addedEntityId,
                    CashFlowPlansBalanceType = (int)item.CashFlowPlansBalanceType,
                    CashFlowPlansTransactionType = (int)item.CashFlowPlansTransactionType,
                    CurrencyID = item.CurrencyID.GetValueOrDefault(),
                    CurrentAccountID = item.CurrentAccountID.GetValueOrDefault(),
                    Date_ = item.Date_,
                    Id = GuidGenerator.CreateGuid(),
                    LineNr = item.LineNr,
                    BankAccountID = item.BankAccountID.GetValueOrDefault(),
                    ExchangeAmount_ = item.ExchangeAmount_,
                    TransactionDescription = item.TransactionDescription,
                    CreationTime = now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var cashFlow = queryFactory.Insert<SelectCashFlowPlansDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("CashFlowPlansChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.CashFlowPlans, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["CashFlowPlansChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectCashFlowPlansDto>(cashFlow);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {

            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.CashFlowPlans).Select("*").Where(new { Id = id }, "");

            var CashFlowPlans = queryFactory.Get<SelectCashFlowPlansDto>(query);

            if (CashFlowPlans.Id != Guid.Empty && CashFlowPlans != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.CashFlowPlans).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.CashFlowPlanLines).Delete(LoginedUserService.UserId).Where(new { CashFlowPlanID = id }, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var billOfMaterial = queryFactory.Update<SelectCashFlowPlansDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.CashFlowPlans, LogType.Delete, id);

                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["CashFlowPlansChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                return new SuccessDataResult<SelectCashFlowPlansDto>(billOfMaterial);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.CashFlowPlanLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");
                var billOfMaterialLines = queryFactory.Update<SelectCashFlowPlanLinesDto>(queryLine, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.CashFlowPlanLines, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectCashFlowPlanLinesDto>(billOfMaterialLines);
            }

        }

        public async Task<IDataResult<SelectCashFlowPlansDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.CashFlowPlans)
                   .Select("*")
                    .Where(new { Id = id }, "");

            var CashFlowPlans = queryFactory.Get<SelectCashFlowPlansDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.CashFlowPlanLines)
                   .Select<CashFlowPlanLines>(null)
                   .Join<CurrentAccountCards>
                    (
                        p => new { CurrentAccountID = p.Id, CurrentAccountName = p.Name },
                        nameof(CashFlowPlanLines.CurrentAccountID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Join<BankAccounts>
                    (
                        p => new { BankAccountID = p.Id, BankAccountName = p.Name },
                        nameof(CashFlowPlanLines.BankAccountID),
                        nameof(BankAccounts.Id),
                        JoinType.Left
                    )
                   .Join<Currencies>
                    (
                        p => new { CurrencyID = p.Id, CurrencyCode = p.CurrencySymbol },
                        nameof(CashFlowPlanLines.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Where(new { CashFlowPlanID = id }, Tables.CashFlowPlanLines);

            var CashFlowPlanLine = queryFactory.GetList<SelectCashFlowPlanLinesDto>(queryLines).ToList();

            CashFlowPlans.SelectCashFlowPlanLines = CashFlowPlanLine;

            LogsAppService.InsertLogToDatabase(CashFlowPlans, CashFlowPlans, LoginedUserService.UserId, Tables.CashFlowPlans, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCashFlowPlansDto>(CashFlowPlans);

        }

        public async Task<IDataResult<IList<ListCashFlowPlansDto>>> GetListAsync(ListCashFlowPlansParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.CashFlowPlans)
                   .Select("*")
                    .Where(null, "");

            var CashFlowPlans = queryFactory.GetList<ListCashFlowPlansDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListCashFlowPlansDto>>(CashFlowPlans);

        }

        public async Task<IDataResult<SelectCashFlowPlansDto>> UpdateAsync(UpdateCashFlowPlansDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.CashFlowPlans)
                   .Select("*")
                    .Where(new { Id = input.Id }, "");

            var entity = queryFactory.Get<SelectCashFlowPlansDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.CashFlowPlanLines)
                   .Select<CashFlowPlanLines>(null)
                   .Join<CurrentAccountCards>
                    (
                        p => new { CurrentAccountID = p.Id, CurrentAccountName = p.Name },
                        nameof(CashFlowPlanLines.CurrentAccountID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Join<BankAccounts>
                    (
                        p => new { BankAccountID = p.Id, BankAccountName = p.Name },
                        nameof(CashFlowPlanLines.BankAccountID),
                        nameof(BankAccounts.Id),
                        "RecieverBank",
                        JoinType.Left
                    )
                   .Join<Currencies>
                    (
                        p => new { CurrencyID = p.Id, CurrencyCode = p.CurrencySymbol },
                        nameof(CashFlowPlanLines.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Where(new { CashFlowPlanID = input.Id }, Tables.CashFlowPlanLines);

            var CashFlowPlanLine = queryFactory.GetList<SelectCashFlowPlanLinesDto>(queryLines).ToList();

            entity.SelectCashFlowPlanLines = CashFlowPlanLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.CashFlowPlans)
                           .Select("*")
                            .Where(new { Code = input.Code }, "").UseIsDelete(false);

            var list = queryFactory.GetList<ListCashFlowPlansDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion


            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.CashFlowPlans).Update(new UpdateCashFlowPlansDto
            {
                Code = input.Code,
                Id = input.Id,
                Description_ = input.Description_,
                EndDate = input.EndDate,
                IsActive = input.IsActive,
                StartDate = input.StartDate,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectCashFlowPlanLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.CashFlowPlanLines).Insert(new CreateCashFlowPlanLinesDto
                    {

                        Id = GuidGenerator.CreateGuid(),
                        LineNr = item.LineNr,
                        Amount_ = item.Amount_,
                        CashFlowPlanID = item.CashFlowPlanID,
                        CashFlowPlansBalanceType = (int)item.CashFlowPlansBalanceType,
                        CashFlowPlansTransactionType = (int)item.CashFlowPlansTransactionType,
                        CurrencyID = item.CurrencyID.GetValueOrDefault(),
                        CurrentAccountID = item.CurrentAccountID.GetValueOrDefault(),
                        Date_ = item.Date_,
                        BankAccountID = item.BankAccountID.GetValueOrDefault(),
                        ExchangeAmount_ = item.ExchangeAmount_,
                        TransactionDescription = item.TransactionDescription,
                        CreationTime = now,
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.CashFlowPlanLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectCashFlowPlanLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.CashFlowPlanLines).Update(new UpdateCashFlowPlanLinesDto
                        {
                            Id = item.Id,
                            LineNr = item.LineNr,
                            BankAccountID = item.BankAccountID.GetValueOrDefault(),
                            TransactionDescription = item.TransactionDescription,
                            Date_ = item.Date_,
                            CurrentAccountID = item.CurrentAccountID.GetValueOrDefault(),
                            CurrencyID = item.CurrencyID.GetValueOrDefault(),
                            CashFlowPlansTransactionType = (int)item.CashFlowPlansTransactionType,
                            Amount_ = item.Amount_,
                            CashFlowPlanID = item.CashFlowPlanID,
                            CashFlowPlansBalanceType = (int)item.CashFlowPlansBalanceType,
                            ExchangeAmount_ = item.ExchangeAmount_,
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            IsDeleted = item.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId,
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var billOfMaterial = queryFactory.Update<SelectCashFlowPlansDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.CashFlowPlans, LogType.Update, billOfMaterial.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["CashFlowPlansChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectCashFlowPlansDto>(billOfMaterial);

        }

        public async Task<IDataResult<SelectCashFlowPlansDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.CashFlowPlans).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<CashFlowPlans>(entityQuery);

            var query = queryFactory.Query().From(Tables.CashFlowPlans).Update(new UpdateCashFlowPlansDto
            {
                Code = entity.Code,
                Id = entity.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Description_ = entity.Description_,
                EndDate = entity.EndDate,
                IsActive = entity.IsActive,
                StartDate = entity.StartDate,
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var CashFlowPlansDto = queryFactory.Update<SelectCashFlowPlansDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectCashFlowPlansDto>(CashFlowPlansDto);


        }

        #region Banka Bakiyeleriyle Paralel Çalışan Nakit Akış Satır Metotları

        public async Task<IDataResult<SelectCashFlowPlanLinesDto>> GetLineAsync(Guid id)
        {

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.CashFlowPlanLines)
                   .Select<CashFlowPlanLines>(null)
                   .Join<CurrentAccountCards>
                    (
                        p => new { CurrentAccountID = p.Id, CurrentAccountName = p.Name },
                        nameof(CashFlowPlanLines.CurrentAccountID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Join<BankAccounts>
                    (
                        p => new { BankAccountID = p.Id, BankAccountName = p.Name },
                        nameof(CashFlowPlanLines.BankAccountID),
                        nameof(BankAccounts.Id),
                        JoinType.Left
                    )
                   .Join<Currencies>
                    (
                        p => new { CurrencyID = p.Id, CurrencyCode = p.CurrencySymbol },
                        nameof(CashFlowPlanLines.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, Tables.CashFlowPlanLines);

            var CashFlowPlanLine = queryFactory.Get<SelectCashFlowPlanLinesDto>(queryLines);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCashFlowPlanLinesDto>(CashFlowPlanLine);

        }

        public async Task<IDataResult<IList<SelectCashFlowPlanLinesDto>>> GetLineListAsync(DateTime date, Guid currenctId, Guid bankAccountId)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.CashFlowPlanLines)
                   .Select<CashFlowPlanLines>(null)
                   .Join<CurrentAccountCards>
                    (
                        p => new { CurrentAccountID = p.Id, CurrentAccountName = p.Name },
                        nameof(CashFlowPlanLines.CurrentAccountID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Join<BankAccounts>
                    (
                        p => new { BankAccountID = p.Id, BankAccountName = p.Name },
                        nameof(CashFlowPlanLines.BankAccountID),
                        nameof(BankAccounts.Id),
                        JoinType.Left
                    )
                   .Join<Currencies>
                    (
                        p => new { CurrencyID = p.Id, CurrencyCode = p.CurrencySymbol },
                        nameof(CashFlowPlanLines.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Where(new { Date_ = date, CurrencyID = currenctId, BankAccountID = bankAccountId }, Tables.CashFlowPlanLines);

            var cashFlowPlanLines = queryFactory.GetList<SelectCashFlowPlanLinesDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<SelectCashFlowPlanLinesDto>>(cashFlowPlanLines);

        }

        public async Task<IDataResult<SelectCashFlowPlanLinesDto>> CreateUpdateLineAsync(SelectCashFlowPlanLinesDto input)
        {

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            SelectCashFlowPlanLinesDto line = new SelectCashFlowPlanLinesDto();

            SelectCashFlowPlanLinesDto cashFlowLine = new SelectCashFlowPlanLinesDto();

            if (input.Id == Guid.Empty) // Create
            {

                var queryLine = queryFactory.Query().From(Tables.CashFlowPlanLines).Insert(new CreateCashFlowPlanLinesDto
                {

                    Id = GuidGenerator.CreateGuid(),
                    LineNr = input.LineNr,
                    Amount_ = input.Amount_,
                    CashFlowPlanID = Guid.Empty,
                    CashFlowPlansBalanceType = (int)input.CashFlowPlansBalanceType,
                    CashFlowPlansTransactionType = (int)input.CashFlowPlansTransactionType,
                    CurrencyID = input.CurrencyID.GetValueOrDefault(),
                    CurrentAccountID = input.CurrentAccountID.GetValueOrDefault(),
                    Date_ = input.Date_,
                    BankAccountID = input.BankAccountID.GetValueOrDefault(),
                    ExchangeAmount_ = input.ExchangeAmount_,
                    TransactionDescription = input.TransactionDescription,
                    CreationTime = now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                });

                cashFlowLine = queryFactory.Insert<SelectCashFlowPlanLinesDto>(queryLine, "Id", true);

            }
            else // Update
            {
                var lineGetQuery = queryFactory.Query().From(Tables.CashFlowPlanLines).Select("*").Where(new { Id = input.Id }, "");

                line = queryFactory.Get<SelectCashFlowPlanLinesDto>(lineGetQuery);

                if (line != null && line.Id != Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.CashFlowPlanLines).Update(new UpdateCashFlowPlanLinesDto
                    {
                        Id = input.Id,
                        LineNr = input.LineNr,
                        BankAccountID = input.BankAccountID.GetValueOrDefault(),
                        TransactionDescription = input.TransactionDescription,
                        Date_ = input.Date_,
                        CurrentAccountID = input.CurrentAccountID.GetValueOrDefault(),
                        CurrencyID = input.CurrencyID.GetValueOrDefault(),
                        CashFlowPlansTransactionType = (int)input.CashFlowPlansTransactionType,
                        Amount_ = input.Amount_,
                        CashFlowPlanID = input.CashFlowPlanID,
                        CashFlowPlansBalanceType = (int)input.CashFlowPlansBalanceType,
                        ExchangeAmount_ = input.ExchangeAmount_,
                        CreationTime = line.CreationTime,
                        CreatorId = line.CreatorId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = line.DeleterId.GetValueOrDefault(),
                        DeletionTime = line.DeletionTime.GetValueOrDefault(),
                        IsDeleted = input.IsDeleted,
                        LastModificationTime = now,
                        LastModifierId = LoginedUserService.UserId,
                    }).Where(new { Id = line.Id }, "");

                    cashFlowLine = queryFactory.Update<SelectCashFlowPlanLinesDto>(queryLine, "Id", true);

                }
            }

            List<SelectBankBalancesDto> bankBalanceList = (await _BankBalancesAppService.GetListbyDateAsync(input.Date_)).Data.ToList();

            #region Bank Balance Bulk Update

            foreach (SelectBankBalancesDto bankBalance in bankBalanceList)
            {
                if (input.CashFlowPlansBalanceType == TsiErp.Entities.Enums.CashFlowPlansBalanceTypeEnum.GelenOdeme)
                {
                    if (input.Id == Guid.Empty)
                    {
                        switch (input.BankAccountName)
                        {
                            case "AKBANK T A Ş TRY": bankBalance.AmountAkbankTL += input.Amount_; break;
                            case "AKBANK T A Ş EUR": bankBalance.AmountAkbankEUR += input.Amount_; break;
                            case "TÜRKİYE İŞ BANKASI A Ş TRY": bankBalance.AmountIsBankTL += input.Amount_; break;
                            case "TÜRKİYE İŞ BANKASI A Ş EUR": bankBalance.AmountIsBankEUR += input.Amount_; break;
                        }
                        
                    }
                    else
                    {
                        switch (input.BankAccountName)
                        {
                            case "AKBANK T A Ş TRY": bankBalance.AmountAkbankTL += (line.Amount_ - input.Amount_); break;
                            case "AKBANK T A Ş EUR": bankBalance.AmountAkbankEUR += (line.Amount_ - input.Amount_); break;
                            case "TÜRKİYE İŞ BANKASI A Ş TRY": bankBalance.AmountIsBankTL += (line.Amount_ - input.Amount_); break;
                            case "TÜRKİYE İŞ BANKASI A Ş EUR": bankBalance.AmountIsBankEUR += (line.Amount_ - input.Amount_); break;
                        }
                    }
                }
                else
                {
                    if (input.Id == Guid.Empty)
                    {
                        switch (input.BankAccountName)
                        {
                            case "AKBANK T A Ş TRY": bankBalance.AmountAkbankTL -= input.Amount_; break;
                            case "AKBANK T A Ş EUR": bankBalance.AmountAkbankEUR -= input.Amount_; break;
                            case "TÜRKİYE İŞ BANKASI A Ş TRY": bankBalance.AmountIsBankTL -= input.Amount_; break;
                            case "TÜRKİYE İŞ BANKASI A Ş EUR": bankBalance.AmountIsBankEUR -= input.Amount_; break;
                        }
                    }
                    else
                    {
                        switch (input.BankAccountName)
                        {
                            case "AKBANK T A Ş TRY": bankBalance.AmountAkbankTL -= (line.Amount_ - input.Amount_); break;
                            case "AKBANK T A Ş EUR": bankBalance.AmountAkbankEUR -= (line.Amount_ - input.Amount_); break;
                            case "TÜRKİYE İŞ BANKASI A Ş TRY": bankBalance.AmountIsBankTL -= (line.Amount_ - input.Amount_); break;
                            case "TÜRKİYE İŞ BANKASI A Ş EUR": bankBalance.AmountIsBankEUR -= (line.Amount_ - input.Amount_); break;
                        }
                    }
                }
            }

            var bulk = new BulkOperations();

            using (TransactionScope trans = new TransactionScope())
            {
                using (SqlConnection connection = new SqlConnection(queryFactory.ConnectionString))
                {
                    bulk.Setup<SelectBankBalancesDto>(x => x.ForCollection(bankBalanceList))
                        .WithTable(Tables.BankBalances)
                        .AddColumn(x => x.Id)
                        .AddColumn(x => x.AmountAkbankTL)
                        .AddColumn(x => x.AmountAkbankEUR)
                        .AddColumn(x => x.AmountIsBankTL)
                        .AddColumn(x => x.AmountIsBankEUR)
                        .AddColumn(x => x.Date_)
                        .AddColumn(x => x.DataOpenStatus)
                        .AddColumn(x => x.DataOpenStatusUserId)
                        .AddColumn(x => x.IsDeleted)
                        .AddColumn(x => x.DeletionTime)
                        .AddColumn(x => x.DeleterId)
                        .AddColumn(x => x.LastModifierId)
                        .AddColumn(x => x.LastModificationTime)
                        .AddColumn(x => x.CreationTime)
                        .AddColumn(x => x.CreatorId)
                        .BulkUpdate()
                        .MatchTargetOn(x => x.Id);

                    bulk.CommitTransaction(connection);
                }

                trans.Complete();
            }

            #endregion

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCashFlowPlanLinesDto>(cashFlowLine);


        }

        #endregion
    }
}
