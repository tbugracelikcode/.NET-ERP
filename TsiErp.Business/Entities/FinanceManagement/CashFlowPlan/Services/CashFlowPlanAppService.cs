using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.FinanceManagement.CashFlowPlan.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CashFlowPlan;
using TsiErp.Entities.Entities.FinanceManagement.CashFlowPlan.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PurchaseManagementParameter.Page;

namespace TsiErp.Business.Entities.CashFlowPlan.Services
{
    [ServiceRegistration(typeof(ICashFlowPlansAppService), DependencyInjectionType.Scoped)]
    public class CashFlowPlansAppService : ApplicationService<PurchaseManagementParametersResource>, ICashFlowPlansAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public CashFlowPlansAppService(IStringLocalizer<PurchaseManagementParametersResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }

        public async Task<IDataResult<SelectCashFlowPlansDto>> CreateAsync(CreateCashFlowPlansDto input)
        {
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.CashFlowPlans).Insert(new CreateCashFlowPlansDto
            {
                Id = GuidGenerator.CreateGuid(),
                Date_ = input.Date_,
                Amount_ = input.Amount_,
                CashFlowPlansBalanceType = input.CashFlowPlansBalanceType,
                CashFlowPlansTransactionType = input.CashFlowPlansTransactionType,
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountID = input.CurrentAccountID.GetValueOrDefault(),
                ExpenseAmount_ = input.ExpenseAmount_,
                RecieverBankAccountID = input.RecieverBankAccountID.GetValueOrDefault(),
                TransactionDescription = input.TransactionDescription,
                TransmitterBankAccountID = input.TransmitterBankAccountID.GetValueOrDefault()
            });


            var CashFlowPlans = queryFactory.Insert<SelectCashFlowPlansDto>(query, "Id", true);

            await Task.CompletedTask;

            return new SuccessDataResult<SelectCashFlowPlansDto>(CashFlowPlans);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {

            var query = queryFactory.Query().From(Tables.CashFlowPlans).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var CashFlowPlans = queryFactory.Update<SelectCashFlowPlansDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.CashFlowPlans, LogType.Delete, id);



            await Task.CompletedTask;
            return new SuccessDataResult<SelectCashFlowPlansDto>(CashFlowPlans);

        }

        public async Task<IDataResult<SelectCashFlowPlansDto>> GetAsync(Guid id)
        {

            var query = queryFactory.Query().From(Tables.CashFlowPlans).Select("*").Where(
            new
            {
                Id = id
            }, "").UseIsDelete(false);
            var CashFlowPlan = queryFactory.Get<SelectCashFlowPlansDto>(query);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCashFlowPlansDto>(CashFlowPlan);

        }

        public async Task<IDataResult<IList<ListCashFlowPlansDto>>> GetListAsync(ListCashFlowPlansParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.CashFlowPlans).Select<CashFlowPlans>(null).Where(null, "").UseIsDelete(false);
            var cashFlowPlans = queryFactory.GetList<ListCashFlowPlansDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListCashFlowPlansDto>>(cashFlowPlans);

        }


        public async Task<IDataResult<SelectCashFlowPlansDto>> UpdateAsync(UpdateCashFlowPlansDto input)
        {
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.CashFlowPlans).Update(new UpdateCashFlowPlansDto
            {
                Id = input.Id,
                Date_ = input.Date_,
                TransmitterBankAccountID = input.TransmitterBankAccountID.GetValueOrDefault(),
                TransactionDescription = input.TransactionDescription,
                RecieverBankAccountID = input.RecieverBankAccountID.GetValueOrDefault(),
                ExpenseAmount_ = input.ExpenseAmount_,
                CurrentAccountID = input.CurrentAccountID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CashFlowPlansTransactionType = input.CashFlowPlansTransactionType,
                CashFlowPlansBalanceType = input.CashFlowPlansBalanceType,
                Amount_ = input.Amount_,
            }).Where(new { Id = input.Id }, "").UseIsDelete(false);

            var CashFlowPlans = queryFactory.Update<SelectCashFlowPlansDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCashFlowPlansDto>(CashFlowPlans);

        }

        public Task<IDataResult<SelectCashFlowPlansDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
