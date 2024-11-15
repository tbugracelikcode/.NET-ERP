using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.FinanceManagement.BankBalance.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount;
using TsiErp.Entities.Entities.FinanceManagement.BankBalance;
using TsiErp.Entities.Entities.FinanceManagement.BankBalance.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.CashFlowPlans.Page;

namespace TsiErp.Business.Entities.BankBalance.Services
{
    [ServiceRegistration(typeof(IBankBalancesAppService), DependencyInjectionType.Scoped)]
    public class BankBalancesAppService : ApplicationService<CashFlowPlansResource>, IBankBalancesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public BankBalancesAppService(IStringLocalizer<CashFlowPlansResource> l, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }



        public async Task<IDataResult<SelectBankBalancesDto>> CreateAsync(CreateBankBalancesDto input)
        {

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.BankBalances).Insert(new CreateBankBalancesDto
            {
                Id = addedEntityId,
                Amount_ = input.Amount_,
                BankAccountID = input.BankAccountID.GetValueOrDefault(),
                Date_ = input.Date_,
                MonthYear = input.MonthYear,
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


            var cashFlow = queryFactory.Insert<SelectBankBalancesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.BankBalances, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectBankBalancesDto>(cashFlow);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {

            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.BankBalances).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var BankBalances = queryFactory.Update<SelectBankBalancesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.BankBalances, LogType.Delete, id);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectBankBalancesDto>(BankBalances);
        }

        public async Task<IDataResult<SelectBankBalancesDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.BankBalances)
                    .Select<BankBalances>(null)
                    .Join<BankAccounts>
                    (
                        p => new { BankAccountID = p.Id, BankAccountName = p.Name },
                        nameof(BankBalances.BankAccountID),
                        nameof(BankAccounts.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, Tables.BankBalances);

            var bankBalances = queryFactory.Get<SelectBankBalancesDto>(query);



            LogsAppService.InsertLogToDatabase(bankBalances, bankBalances, LoginedUserService.UserId, Tables.BankBalances, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectBankBalancesDto>(bankBalances);

        }

        public async Task<IDataResult<IList<ListBankBalancesDto>>> GetListAsync(ListBankBalancesParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.BankBalances)
                   .Select<BankBalances>(null)
                    .Join<BankAccounts>
                    (
                        p => new { BankAccountID = p.Id, BankAccountName = p.Name },
                        nameof(BankBalances.BankAccountID),
                        nameof(BankAccounts.Id),
                        JoinType.Left
                    )
                    .Where(null, Tables.BankBalances);

            var bankBalances = queryFactory.GetList<ListBankBalancesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListBankBalancesDto>>(bankBalances);

        }

        public async Task<IDataResult<IList<SelectBankBalancesDto>>> GetListbyDateAsync(DateTime Date, Guid BankId)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.BankBalances)
                   .Select<BankBalances>(null)
                    .Join<BankAccounts>
                    (
                        p => new { BankAccountID = p.Id, BankAccountName = p.Name },
                        nameof(BankBalances.BankAccountID),
                        nameof(BankAccounts.Id),
                        JoinType.Left
                    )
                    .Where(new { BankAccountID = BankId }, Tables.BankBalances);

            var bankBalances = queryFactory.GetList<SelectBankBalancesDto>(query).ToList();

            bankBalances = bankBalances.Where(t => t.Date_ >= Date && t.Date_.Year == Date.Year).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<SelectBankBalancesDto>>(bankBalances);

        }

        public async Task<IDataResult<SelectBankBalancesDto>> UpdateAsync(UpdateBankBalancesDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.BankBalances)
                    .Select<BankBalances>(null)
                    .Join<BankAccounts>
                    (
                        p => new { BankAccountID = p.Id, BankAccountName = p.Name },
                        nameof(BankBalances.BankAccountID),
                        nameof(BankAccounts.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.BankBalances);

            var entity = queryFactory.Get<SelectBankBalancesDto>(entityQuery);

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.BankBalances).Update(new UpdateBankBalancesDto
            {
                Id = input.Id,
                BankAccountID = input.BankAccountID.GetValueOrDefault(),
                Date_ = input.Date_,
                Amount_ = input.Amount_,
                CreationTime = entity.CreationTime,
                MonthYear = input.MonthYear,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id }, "");


            var bankBalance = queryFactory.Update<SelectBankBalancesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.BankBalances, LogType.Update, bankBalance.Id);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectBankBalancesDto>(bankBalance);

        }

        public async Task<IDataResult<SelectBankBalancesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.BankBalances).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<BankBalances>(entityQuery);

            var query = queryFactory.Query().From(Tables.BankBalances).Update(new UpdateBankBalancesDto
            {
                Id = entity.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                MonthYear = entity.MonthYear,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Amount_ = entity.Amount_,
                Date_ = entity.Date_,
                BankAccountID = entity.BankAccountID,
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var BankBalancesDto = queryFactory.Update<SelectBankBalancesDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectBankBalancesDto>(BankBalancesDto);


        }
    }
}
