using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.FinanceManagement.EximBankReeskont.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount;
using TsiErp.Entities.Entities.FinanceManagement.EximBankReeskont;
using TsiErp.Entities.Entities.FinanceManagement.EximBankReeskont.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.EximBankReeskonts.Page;

namespace TsiErp.Business.Entities.EximBankReeskont.Services
{
    [ServiceRegistration(typeof(IEximBankReeskontsAppService), DependencyInjectionType.Scoped)]
    public class EximBankReeskontsAppService : ApplicationService<EximBankReeskontsResource>, IEximBankReeskontsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public EximBankReeskontsAppService(IStringLocalizer<EximBankReeskontsResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }


        public async Task<IDataResult<SelectEximBankReeskontsDto>> CreateAsync(CreateEximBankReeskontsDto input)
        {
            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.EximBankReeskonts).Insert(new CreateEximBankReeskontsDto
            {
                Id = addedEntityId,
                BankAccountID = input.BankAccountID.GetValueOrDefault(),
                CommissionAmount = input.CommissionAmount,
                Year_ = input.Date_.Year,
                Date_ = input.Date_,
                InterestAmount = input.InterestAmount,
                MainAmount = input.MainAmount,
                PaidAmount = input.PaidAmount,
                RemainingAmount = input.RemainingAmount,
                TotalAmount = input.TotalAmount,
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

            var cashFlow = queryFactory.Insert<SelectEximBankReeskontsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.EximBankReeskonts, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectEximBankReeskontsDto>(cashFlow);
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var deleteQuery = queryFactory.Query().From(Tables.EximBankReeskonts).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var EximBankReeskont = queryFactory.Update<SelectEximBankReeskontsDto>(deleteQuery, "Id", true);
            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.EximBankReeskonts, LogType.Delete, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectEximBankReeskontsDto>(EximBankReeskont);
        }


        public async Task<IDataResult<SelectEximBankReeskontsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.EximBankReeskonts)
                   .Select<EximBankReeskonts>(null)
                   .Join<BankAccounts>
                    (
                        p => new { BankAccountID = p.Id, BankAccountName = p.Name },
                        nameof(EximBankReeskonts.BankAccountID),
                        nameof(BankAccounts.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, Tables.EximBankReeskonts);

            var eximBankReeskonts = queryFactory.Get<SelectEximBankReeskontsDto>(query);

            LogsAppService.InsertLogToDatabase(eximBankReeskonts, eximBankReeskonts, LoginedUserService.UserId, Tables.EximBankReeskonts, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectEximBankReeskontsDto>(eximBankReeskonts);

        }

        public async Task<IDataResult<IList<ListEximBankReeskontsDto>>> GetListAsync(ListEximBankReeskontsParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.EximBankReeskonts)
                    .Select<EximBankReeskonts>(null)
                   .Join<BankAccounts>
                    (
                        p => new { BankAccountID = p.Id, BankAccountName = p.Name },
                        nameof(EximBankReeskonts.BankAccountID),
                        nameof(BankAccounts.Id),
                        JoinType.Left
                    )
                    .Where(null, Tables.EximBankReeskonts);

            var eximBankReeskonts = queryFactory.GetList<ListEximBankReeskontsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListEximBankReeskontsDto>>(eximBankReeskonts);

        }

        public async Task<IDataResult<SelectEximBankReeskontsDto>> UpdateAsync(UpdateEximBankReeskontsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                    .From(Tables.EximBankReeskonts)
                   .Select<EximBankReeskonts>(null)
                   .Join<BankAccounts>
                    (
                        p => new { BankAccountID = p.Id, BankAccountName = p.Name },
                        nameof(EximBankReeskonts.BankAccountID),
                        nameof(BankAccounts.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.EximBankReeskonts);

            var entity = queryFactory.Get<SelectEximBankReeskontsDto>(entityQuery);



            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.EximBankReeskonts).Update(new UpdateEximBankReeskontsDto
            {
                Id = input.Id,
                TotalAmount = input.TotalAmount,
                RemainingAmount = input.RemainingAmount,
                Year_ = input.Date_.Year,
                PaidAmount = input.PaidAmount,
                MainAmount = input.MainAmount,
                InterestAmount = input.InterestAmount,
                Date_ = input.Date_,
                CommissionAmount = input.CommissionAmount,
                BankAccountID = input.BankAccountID.GetValueOrDefault(),
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



            var EximBankReeskont = queryFactory.Update<SelectEximBankReeskontsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.EximBankReeskonts, LogType.Update, EximBankReeskont.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectEximBankReeskontsDto>(EximBankReeskont);

        }

        public async Task<IDataResult<SelectEximBankReeskontsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.EximBankReeskonts).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<EximBankReeskonts>(entityQuery);

            var query = queryFactory.Query().From(Tables.EximBankReeskonts).Update(new UpdateEximBankReeskontsDto
            {
                Id = entity.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                Year_ = entity.Date_.Year,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                BankAccountID = entity.BankAccountID,
                CommissionAmount = entity.CommissionAmount,
                Date_ = entity.Date_,
                InterestAmount = entity.InterestAmount,
                MainAmount = entity.MainAmount,
                PaidAmount = entity.PaidAmount,
                RemainingAmount = entity.RemainingAmount,
                TotalAmount = entity.TotalAmount,
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var EximBankReeskontsDto = queryFactory.Update<SelectEximBankReeskontsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectEximBankReeskontsDto>(EximBankReeskontsDto);


        }


    }
}
