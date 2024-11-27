using Microsoft.Extensions.Localization;
using SqlBulkTools;
using System.Data.SqlClient;
using System.Transactions;
using Tsi.Core.Entities.CreationEntites;
using Tsi.Core.Entities.DeleterEntities;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.FinanceManagement.BankBalanceCashFlow.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount;
using TsiErp.Entities.Entities.FinanceManagement.BankBalanceCashFlow;
using TsiErp.Entities.Entities.FinanceManagement.BankBalanceCashFlow.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.BankBalanceCashFlowLine;
using TsiErp.Entities.Entities.FinanceManagement.BankBalanceCashFlowLine.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.BankBalanceCashFlowLinesLine;
using TsiErp.Entities.Entities.FinanceManagement.BankBalanceCashFlowLinesLine.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.CashFlowPlans.Page;

namespace TsiErp.Business.Entities.BankBalanceCashFlow.Services
{
    [ServiceRegistration(typeof(IBankBalanceCashFlowsAppService), DependencyInjectionType.Scoped)]
    public class BankBalanceCashFlowsAppService : ApplicationService<CashFlowPlansResource>, IBankBalanceCashFlowsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public BankBalanceCashFlowsAppService(IStringLocalizer<CashFlowPlansResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }


        public async Task<IDataResult<SelectBankBalanceCashFlowsDto>> CreateAsync(CreateBankBalanceCashFlowsDto input)
        {
            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.BankBalanceCashFlows).Insert(new CreateBankBalanceCashFlowsDto
            {
                Id = addedEntityId,
                _Description = input._Description,
                Year_ = input.Year_,
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





            foreach (var item in input.SelectBankBalanceCashFlowLinesDto)
            {

                var queryLine = queryFactory.Query().From(Tables.BankBalanceCashFlowLines).Insert(new CreateBankBalanceCashFlowLinesDto
                {
                    BankBalanceCashFlowID = addedEntityId,
                    Date_ = item.Date_,
                    Id = item.Id,
                    LineNr = item.LineNr,
                    AmountAkbankEUR = item.AmountAkbankEUR,
                    AmountAkbankTL = item.AmountAkbankTL,
                    AmountIsBankEUR = item.AmountIsBankEUR,
                    AmountIsBankTL = item.AmountIsBankTL,
                    AmountAkbankEURColor = item.AmountAkbankEURColor,
                    AmountAkbankTLColor = item.AmountAkbankTLColor,
                    AmountIsBankEURColor = item.AmountIsBankEURColor,
                    AmountIsBankTLColor = item.AmountIsBankTLColor,
                    Date_Color = item.Date_Color,
                    MonthYearColor = item.MonthYearColor,
                    MonthYear = item.MonthYear,
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

                if (item.SelectBankBalanceCashFlowLinesLines != null && item.SelectBankBalanceCashFlowLinesLines.Count > 0)
                {
                    foreach (var itemline in item.SelectBankBalanceCashFlowLinesLines)
                    {
                        var queryLinesLine = queryFactory.Query().From(Tables.BankBalanceCashFlowLinesLines).Insert(new CreateBankBalanceCashFlowLinesLinesDto
                        {
                            BankBalanceCashFlowID = addedEntityId,
                            Date_ = itemline.Date_,
                            BankBalanceCashFlowLineID = item.Id,
                            Amount_ = itemline.Amount_,
                            BankAccountID = itemline.BankAccountID.GetValueOrDefault(),
                            CashFlowPlansBalanceType = (int)itemline.CashFlowPlansBalanceType,
                            CashFlowPlansTransactionType = (int)itemline.CashFlowPlansTransactionType,
                            LinkedBankBalanceCashFlowLinesLineID = itemline.LinkedBankBalanceCashFlowLinesLineID.GetValueOrDefault(),
                            RecurrentEndTime = itemline.RecurrentEndTime.GetValueOrDefault(),
                            CurrencyID = itemline.CurrencyID.GetValueOrDefault(),
                            CurrentAccountID = itemline.CurrentAccountID.GetValueOrDefault(),
                            ExchangeAmount_ = itemline.ExchangeAmount_,
                            TransactionDescription = itemline.TransactionDescription,
                            isRecurrent = itemline.isRecurrent,
                            Id = GuidGenerator.CreateGuid(),
                            LineNr = itemline.LineNr,
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

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLinesLine.Sql;
                    }
                }


            }

            var cashFlow = queryFactory.Insert<SelectBankBalanceCashFlowsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.BankBalanceCashFlows, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectBankBalanceCashFlowsDto>(cashFlow);
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var deleteQuery = queryFactory.Query().From(Tables.BankBalanceCashFlows).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var lineDeleteQuery = queryFactory.Query().From(Tables.BankBalanceCashFlowLines).Delete(LoginedUserService.UserId).Where(new { BankBalanceCashFlowID = id }, "");

            var lineslineDeleteQuery = queryFactory.Query().From(Tables.BankBalanceCashFlowLinesLines).Delete(LoginedUserService.UserId).Where(new { BankBalanceCashFlowID = id }, "");

            deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

            deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineslineDeleteQuery.Sql + " where " + lineslineDeleteQuery.WhereSentence;

            var bankBalanceCashFlow = queryFactory.Update<SelectBankBalanceCashFlowsDto>(deleteQuery, "Id", true);
            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.BankBalanceCashFlows, LogType.Delete, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectBankBalanceCashFlowsDto>(bankBalanceCashFlow);
        }

        public async Task<IResult> DeleteLinesLineAsync(Guid id)
        {
            var queryLinesLine = queryFactory
                  .Query()
                  .From(Tables.BankBalanceCashFlowLinesLines)
                  .Select<BankBalanceCashFlowLinesLines>(null)
                    .Join<CurrentAccountCards>
                    (
                        p => new { CurrentAccountID = p.Id, CurrentAccountName = p.Name },
                        nameof(BankBalanceCashFlowLinesLines.CurrentAccountID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Join<BankAccounts>
                    (
                        p => new { BankAccountID = p.Id, BankAccountName = p.Name },
                        nameof(BankBalanceCashFlowLinesLines.BankAccountID),
                        nameof(BankAccounts.Id),
                        "RecieverBank",
                        JoinType.Left
                    )
                   .Join<Currencies>
                    (
                        p => new { CurrencyID = p.Id, CurrencyCode = p.CurrencySymbol },
                        nameof(BankBalanceCashFlowLinesLines.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                   .Where(new { Id = id }, Tables.BankBalanceCashFlowLinesLines);

            var BankBalanceCashFlowLinesLine = queryFactory.Get<SelectBankBalanceCashFlowLinesLinesDto>(queryLinesLine);

            if (BankBalanceCashFlowLinesLine != null)
            {
                List<SelectBankBalanceCashFlowLinesDto> linesList = (await GetLineListbyDateAsync(BankBalanceCashFlowLinesLine.Date_)).Data.ToList();

                foreach (var line in linesList)
                {
                    switch (BankBalanceCashFlowLinesLine.BankAccountName)
                    {
                        case "AKBANK T A Ş TRY": line.AmountAkbankTL -= BankBalanceCashFlowLinesLine.Amount_; break;
                        case "AKBANK T A Ş EUR": line.AmountAkbankEUR -= BankBalanceCashFlowLinesLine.Amount_; break;
                        case "TÜRKİYE İŞ BANKASI A Ş TRY": line.AmountIsBankTL -= BankBalanceCashFlowLinesLine.Amount_; break;
                        case "TÜRKİYE İŞ BANKASI A Ş EUR": line.AmountIsBankEUR -= BankBalanceCashFlowLinesLine.Amount_; break;
                    }
                }

                var bulk = new BulkOperations();

                using (TransactionScope trans = new TransactionScope())
                {
                    using (SqlConnection connection = new SqlConnection(queryFactory.ConnectionString))
                    {
                        bulk.Setup<SelectBankBalanceCashFlowLinesDto>(x => x.ForCollection(linesList))
                            .WithTable(Tables.BankBalanceCashFlowLines)
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

            }

            var query = queryFactory.Query().From(Tables.BankBalanceCashFlowLinesLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var bankBalanceCashFlowLinesLine = queryFactory.Update<SelectBankBalanceCashFlowLinesLinesDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectBankBalanceCashFlowLinesLinesDto>(bankBalanceCashFlowLinesLine);
        }

        public async Task<IDataResult<SelectBankBalanceCashFlowsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.BankBalanceCashFlows)
                   .Select("*")
                    .Where(new { Id = id }, "");

            var BankBalanceCashFlows = queryFactory.Get<SelectBankBalanceCashFlowsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.BankBalanceCashFlowLines)
                   .Select("*")
                    .Where(new { BankBalanceCashFlowID = id }, "");

            var BankBalanceCashFlowLine = queryFactory.GetList<SelectBankBalanceCashFlowLinesDto>(queryLines).ToList();

            BankBalanceCashFlows.SelectBankBalanceCashFlowLinesDto = BankBalanceCashFlowLine;

            foreach (var line in BankBalanceCashFlows.SelectBankBalanceCashFlowLinesDto)
            {
                var queryLinesLine = queryFactory
                  .Query()
                  .From(Tables.BankBalanceCashFlowLinesLines)
                  .Select<BankBalanceCashFlowLinesLines>(null)
                    .Join<CurrentAccountCards>
                    (
                        p => new { CurrentAccountID = p.Id, CurrentAccountName = p.Name },
                        nameof(BankBalanceCashFlowLinesLines.CurrentAccountID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Join<BankAccounts>
                    (
                        p => new { BankAccountID = p.Id, BankAccountName = p.Name },
                        nameof(BankBalanceCashFlowLinesLines.BankAccountID),
                        nameof(BankAccounts.Id),
                        "RecieverBank",
                        JoinType.Left
                    )
                   .Join<Currencies>
                    (
                        p => new { CurrencyID = p.Id, CurrencyCode = p.CurrencySymbol },
                        nameof(BankBalanceCashFlowLinesLines.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                   .Where(new { BankBalanceCashFlowLineID = line.Id }, Tables.BankBalanceCashFlowLinesLines);

                var BankBalanceCashFlowLinesLine = queryFactory.GetList<SelectBankBalanceCashFlowLinesLinesDto>(queryLinesLine).ToList();

                line.SelectBankBalanceCashFlowLinesLines = BankBalanceCashFlowLinesLine;
            }

            LogsAppService.InsertLogToDatabase(BankBalanceCashFlows, BankBalanceCashFlows, LoginedUserService.UserId, Tables.BankBalanceCashFlows, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectBankBalanceCashFlowsDto>(BankBalanceCashFlows);

        }

        public async Task<IDataResult<IList<ListBankBalanceCashFlowsDto>>> GetListAsync(ListBankBalanceCashFlowsParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.BankBalanceCashFlows)
                   .Select("*")
                    .Where(null, "");

            var BankBalanceCashFlows = queryFactory.GetList<ListBankBalanceCashFlowsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListBankBalanceCashFlowsDto>>(BankBalanceCashFlows);

        }

        public async Task<IDataResult<IList<SelectBankBalanceCashFlowLinesDto>>> GetLineListbyDateAsync(DateTime date)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.BankBalanceCashFlowLines)
                   .Select("*")
                    .Where(null, "");

            var BankBalanceCashFlowLines = queryFactory.GetList<SelectBankBalanceCashFlowLinesDto>(query).ToList();

            BankBalanceCashFlowLines = BankBalanceCashFlowLines.Where(t => t.Date_ >= date && t.Date_.Year == date.Year).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<SelectBankBalanceCashFlowLinesDto>>(BankBalanceCashFlowLines);

        }

        public async Task<IDataResult<SelectBankBalanceCashFlowsDto>> UpdateAsync(UpdateBankBalanceCashFlowsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                    .From(Tables.BankBalanceCashFlows)
                   .Select("*")
                    .Where(new { Id = input.Id }, "");

            var entity = queryFactory.Get<SelectBankBalanceCashFlowsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                    .From(Tables.BankBalanceCashFlowLines)
                   .Select("*")
                    .Where(new { BankBalanceCashFlowID = input.Id }, "");

            var BankBalanceCashFlowLine = queryFactory.GetList<SelectBankBalanceCashFlowLinesDto>(queryLines).ToList();

            entity.SelectBankBalanceCashFlowLinesDto = BankBalanceCashFlowLine;

            foreach (var entityline in entity.SelectBankBalanceCashFlowLinesDto)
            {
                var queryLinesLine = queryFactory
                  .Query()
                  .From(Tables.BankBalanceCashFlowLinesLines)
                  .Select<BankBalanceCashFlowLinesLines>(null)
                    .Join<CurrentAccountCards>
                    (
                        p => new { CurrentAccountID = p.Id, CurrentAccountName = p.Name },
                        nameof(BankBalanceCashFlowLinesLines.CurrentAccountID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Join<BankAccounts>
                    (
                        p => new { BankAccountID = p.Id, BankAccountName = p.Name },
                        nameof(BankBalanceCashFlowLinesLines.BankAccountID),
                        nameof(BankAccounts.Id),
                        "RecieverBank",
                        JoinType.Left
                    )
                   .Join<Currencies>
                    (
                        p => new { CurrencyID = p.Id, CurrencyCode = p.CurrencySymbol },
                        nameof(BankBalanceCashFlowLinesLines.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                   .Where(new { BankBalanceCashFlowLineID = entityline.Id }, Tables.BankBalanceCashFlowLinesLines);

                var BankBalanceCashFlowLinesLine = queryFactory.GetList<SelectBankBalanceCashFlowLinesLinesDto>(queryLinesLine).ToList();

                entityline.SelectBankBalanceCashFlowLinesLines = BankBalanceCashFlowLinesLine;
            }

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.BankBalanceCashFlows).Update(new UpdateBankBalanceCashFlowsDto
            {
                Id = input.Id,
                Year_ = input.Year_,
                CreationTime = entity.CreationTime,
                _Description = input._Description,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectBankBalanceCashFlowLinesDto)
            {
                if (item.Id == Guid.Empty)
                {
                    Guid lineId = GuidGenerator.CreateGuid();

                    var queryLine = queryFactory.Query().From(Tables.BankBalanceCashFlowLines).Insert(new CreateBankBalanceCashFlowLinesDto
                    {
                        Id = lineId,
                        LineNr = item.LineNr,
                        BankBalanceCashFlowID = item.BankBalanceCashFlowID,
                        AmountAkbankEUR = item.AmountAkbankEUR,
                        AmountAkbankTL = item.AmountAkbankTL,
                        AmountIsBankEUR = item.AmountIsBankEUR,
                        AmountAkbankEURColor = item.AmountAkbankEURColor,
                        AmountAkbankTLColor = item.AmountAkbankTLColor,
                        AmountIsBankEURColor = item.AmountIsBankEURColor,
                        AmountIsBankTLColor = item.AmountIsBankTLColor,
                        Date_Color = item.Date_Color,
                        AmountIsBankTL = item.AmountIsBankTL,
                        MonthYear = item.MonthYear,
                        Date_ = item.Date_,
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

                    if (item.SelectBankBalanceCashFlowLinesLines != null && item.SelectBankBalanceCashFlowLinesLines.Count > 0)
                    {
                        foreach (var itemline in item.SelectBankBalanceCashFlowLinesLines)
                        {
                            if (itemline.Id == Guid.Empty)
                            {
                                var queryLinesLine = queryFactory.Query().From(Tables.BankBalanceCashFlowLinesLines).Insert(new CreateBankBalanceCashFlowLinesLinesDto
                                {

                                    Id = GuidGenerator.CreateGuid(),
                                    LineNr = item.LineNr,
                                    BankBalanceCashFlowID = item.BankBalanceCashFlowID,
                                    Amount_ = itemline.Amount_,
                                    BankAccountID = itemline.BankAccountID.GetValueOrDefault(),
                                    BankBalanceCashFlowLineID = lineId,
                                    CashFlowPlansBalanceType = (int)itemline.CashFlowPlansBalanceType,
                                    CashFlowPlansTransactionType = (int)itemline.CashFlowPlansTransactionType,
                                    LinkedBankBalanceCashFlowLinesLineID = itemline.LinkedBankBalanceCashFlowLinesLineID.GetValueOrDefault(),
                                    RecurrentEndTime = itemline.RecurrentEndTime.GetValueOrDefault(),
                                    CurrencyID = itemline.CurrencyID.GetValueOrDefault(),
                                    CurrentAccountID = itemline.CurrentAccountID.GetValueOrDefault(),
                                    ExchangeAmount_ = itemline.ExchangeAmount_,
                                    TransactionDescription = itemline.TransactionDescription,
                                    isRecurrent = itemline.isRecurrent,
                                    Date_ = item.Date_,
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

                                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLinesLine.Sql;
                            }
                            else
                            {
                                var linesLineGetQuery = queryFactory.Query().From(Tables.BankBalanceCashFlowLinesLines).Select("*").Where(new { Id = itemline.Id }, "");

                                var linesline = queryFactory.Get<SelectBankBalanceCashFlowLinesLinesDto>(linesLineGetQuery);

                                if (linesline != null)
                                {
                                    var queryLinesLine = queryFactory.Query().From(Tables.BankBalanceCashFlowLinesLines).Update(new UpdateBankBalanceCashFlowLinesLinesDto
                                    {
                                        Id = item.Id,
                                        LineNr = item.LineNr,
                                        Date_ = item.Date_,
                                        TransactionDescription = itemline.TransactionDescription,
                                        ExchangeAmount_ = itemline.ExchangeAmount_,
                                        CurrentAccountID = itemline.CurrentAccountID.GetValueOrDefault(),
                                        CurrencyID = itemline.CurrencyID.GetValueOrDefault(),
                                        CashFlowPlansTransactionType = (int)itemline.CashFlowPlansTransactionType,
                                        LinkedBankBalanceCashFlowLinesLineID = itemline.LinkedBankBalanceCashFlowLinesLineID.GetValueOrDefault(),
                                        RecurrentEndTime = itemline.RecurrentEndTime.GetValueOrDefault(),
                                        CashFlowPlansBalanceType = (int)itemline.CashFlowPlansBalanceType,
                                        BankBalanceCashFlowLineID = lineId,
                                        Amount_ = itemline.Amount_,
                                        BankAccountID = itemline.BankAccountID.GetValueOrDefault(),
                                        BankBalanceCashFlowID = item.BankBalanceCashFlowID,
                                        isRecurrent = itemline.isRecurrent,
                                        CreationTime = linesline.CreationTime,
                                        CreatorId = linesline.CreatorId,
                                        DataOpenStatus = false,
                                        DataOpenStatusUserId = Guid.Empty,
                                        DeleterId = linesline.DeleterId.GetValueOrDefault(),
                                        DeletionTime = linesline.DeletionTime.GetValueOrDefault(),
                                        IsDeleted = item.IsDeleted,
                                        LastModificationTime = now,
                                        LastModifierId = LoginedUserService.UserId,
                                    }).Where(new { Id = linesline.Id }, "");

                                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLinesLine.Sql + " where " + queryLinesLine.WhereSentence;
                                }
                            }
                        }
                    }


                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.BankBalanceCashFlowLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectBankBalanceCashFlowLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.BankBalanceCashFlowLines).Update(new UpdateBankBalanceCashFlowLinesDto
                        {
                            Id = item.Id,
                            LineNr = item.LineNr,
                            Date_ = item.Date_,
                            BankBalanceCashFlowID = item.BankBalanceCashFlowID,
                            MonthYear = item.MonthYear,
                            AmountIsBankTL = item.AmountIsBankTL,
                            AmountAkbankEURColor = item.AmountAkbankEURColor,
                            AmountAkbankTLColor = item.AmountAkbankTLColor,
                            AmountIsBankEURColor = item.AmountIsBankEURColor,
                            AmountIsBankTLColor = item.AmountIsBankTLColor,
                            Date_Color = item.Date_Color,
                            AmountIsBankEUR = item.AmountIsBankEUR,
                            AmountAkbankTL = item.AmountAkbankTL,
                            AmountAkbankEUR = item.AmountAkbankEUR,
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

                        if (item.SelectBankBalanceCashFlowLinesLines != null && item.SelectBankBalanceCashFlowLinesLines.Count > 0)
                        {
                            foreach (var itemline in item.SelectBankBalanceCashFlowLinesLines)
                            {
                                if (itemline.Id == Guid.Empty)
                                {
                                    var queryLinesLine = queryFactory.Query().From(Tables.BankBalanceCashFlowLinesLines).Insert(new CreateBankBalanceCashFlowLinesLinesDto
                                    {

                                        Id = GuidGenerator.CreateGuid(),
                                        LineNr = item.LineNr,
                                        BankBalanceCashFlowID = item.BankBalanceCashFlowID,
                                        Amount_ = itemline.Amount_,
                                        BankAccountID = itemline.BankAccountID.GetValueOrDefault(),
                                        BankBalanceCashFlowLineID = line.Id,
                                        CashFlowPlansBalanceType = (int)itemline.CashFlowPlansBalanceType,
                                        LinkedBankBalanceCashFlowLinesLineID = itemline.LinkedBankBalanceCashFlowLinesLineID.GetValueOrDefault(),
                                        RecurrentEndTime = itemline.RecurrentEndTime.GetValueOrDefault(),
                                        CashFlowPlansTransactionType = (int)itemline.CashFlowPlansTransactionType,
                                        CurrencyID = itemline.CurrencyID.GetValueOrDefault(),
                                        CurrentAccountID = itemline.CurrentAccountID.GetValueOrDefault(),
                                        ExchangeAmount_ = itemline.ExchangeAmount_,
                                        TransactionDescription = itemline.TransactionDescription,
                                        isRecurrent = itemline.isRecurrent,
                                        Date_ = item.Date_,
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

                                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLinesLine.Sql;
                                }
                                else
                                {
                                    var linesLineGetQuery = queryFactory.Query().From(Tables.BankBalanceCashFlowLinesLines).Select("*").Where(new { Id = itemline.Id }, "");

                                    var linesline = queryFactory.Get<SelectBankBalanceCashFlowLinesLinesDto>(linesLineGetQuery);

                                    if (linesline.Id != Guid.Empty)
                                    {
                                        var queryLinesLine = queryFactory.Query().From(Tables.BankBalanceCashFlowLinesLines).Update(new UpdateBankBalanceCashFlowLinesLinesDto
                                        {
                                            Id = itemline.Id,
                                            LineNr = itemline.LineNr,
                                            Date_ = itemline.Date_,
                                            TransactionDescription = itemline.TransactionDescription,
                                            ExchangeAmount_ = itemline.ExchangeAmount_,
                                            CurrentAccountID = itemline.CurrentAccountID.GetValueOrDefault(),
                                            CurrencyID = itemline.CurrencyID.GetValueOrDefault(),
                                            CashFlowPlansTransactionType = (int)itemline.CashFlowPlansTransactionType,
                                            LinkedBankBalanceCashFlowLinesLineID = itemline.LinkedBankBalanceCashFlowLinesLineID.GetValueOrDefault(),
                                            RecurrentEndTime = itemline.RecurrentEndTime.GetValueOrDefault(),
                                            CashFlowPlansBalanceType = (int)itemline.CashFlowPlansBalanceType,
                                            BankBalanceCashFlowLineID = line.Id,
                                            Amount_ = itemline.Amount_,
                                            BankAccountID = itemline.BankAccountID.GetValueOrDefault(),
                                            isRecurrent = itemline.isRecurrent,
                                            BankBalanceCashFlowID = item.BankBalanceCashFlowID,
                                            CreationTime = linesline.CreationTime,
                                            CreatorId = linesline.CreatorId,
                                            DataOpenStatus = false,
                                            DataOpenStatusUserId = Guid.Empty,
                                            DeleterId = linesline.DeleterId.GetValueOrDefault(),
                                            DeletionTime = linesline.DeletionTime.GetValueOrDefault(),
                                            IsDeleted = item.IsDeleted,
                                            LastModificationTime = now,
                                            LastModifierId = LoginedUserService.UserId,
                                        }).Where(new { Id = linesline.Id }, "");

                                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLinesLine.Sql + " where " + queryLinesLine.WhereSentence;
                                    }
                                    else
                                    {
                                        var queryLinesLine = queryFactory.Query().From(Tables.BankBalanceCashFlowLinesLines).Insert(new CreateBankBalanceCashFlowLinesLinesDto
                                        {

                                            Id = itemline.Id,
                                            LineNr = item.LineNr,
                                            BankBalanceCashFlowID = item.BankBalanceCashFlowID,
                                            Amount_ = itemline.Amount_,
                                            BankAccountID = itemline.BankAccountID.GetValueOrDefault(),
                                            BankBalanceCashFlowLineID = line.Id,
                                            CashFlowPlansBalanceType = (int)itemline.CashFlowPlansBalanceType,
                                            LinkedBankBalanceCashFlowLinesLineID = itemline.LinkedBankBalanceCashFlowLinesLineID.GetValueOrDefault(),
                                            RecurrentEndTime = itemline.RecurrentEndTime.GetValueOrDefault(),
                                            CashFlowPlansTransactionType = (int)itemline.CashFlowPlansTransactionType,
                                            CurrencyID = itemline.CurrencyID.GetValueOrDefault(),
                                            CurrentAccountID = itemline.CurrentAccountID.GetValueOrDefault(),
                                            ExchangeAmount_ = itemline.ExchangeAmount_,
                                            TransactionDescription = itemline.TransactionDescription,
                                            isRecurrent = itemline.isRecurrent,
                                            Date_ = item.Date_,
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

                                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLinesLine.Sql;
                                    }
                                }
                            }
                        }


                    }
                }


            }

            var bankBalanceCashFlow = queryFactory.Update<SelectBankBalanceCashFlowsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.BankBalanceCashFlows, LogType.Update, bankBalanceCashFlow.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectBankBalanceCashFlowsDto>(bankBalanceCashFlow);

        }

        public async Task<IDataResult<SelectBankBalanceCashFlowsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.BankBalanceCashFlows).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<BankBalanceCashFlows>(entityQuery);

            var query = queryFactory.Query().From(Tables.BankBalanceCashFlows).Update(new UpdateBankBalanceCashFlowsDto
            {
                Id = entity.Id,
                CreationTime = entity.CreationTime.Value,
                _Description = entity._Description,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Year_ = entity.Year_
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var BankBalanceCashFlowsDto = queryFactory.Update<SelectBankBalanceCashFlowsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectBankBalanceCashFlowsDto>(BankBalanceCashFlowsDto);


        }

        public Guid BankBalanceCashFlowLineGuidGenerate()
        {
            Guid lineGuid = GuidGenerator.CreateGuid();

            return lineGuid;
        }
    }
}
