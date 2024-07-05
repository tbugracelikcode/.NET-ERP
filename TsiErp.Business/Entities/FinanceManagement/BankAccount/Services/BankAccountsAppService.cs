using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.FinanceManagement.BankAccount.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.BankAccounts.Page;

namespace TsiErp.Business.Entities.BankAccount.Services
{
    [ServiceRegistration(typeof(IBankAccountsAppService), DependencyInjectionType.Scoped)]
    public class BankAccountsAppService : ApplicationService<BankAccountsResource>, IBankAccountsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public BankAccountsAppService(IStringLocalizer<BankAccountsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }

        [ValidationAspect(typeof(CreateBankAccountsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectBankAccountsDto>> CreateAsync(CreateBankAccountsDto input)
        {

            var listQuery = queryFactory.Query().From(Tables.BankAccounts).Select("*").Where(new { Code = input.Code }, false, false, "");

            var list = queryFactory.ControlList<BankAccounts>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.BankAccounts).Insert(new CreateBankAccountsDto
            {
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
                Name = input.Name,
                Address = input.Address,
                BankInstructionDescription = input.BankInstructionDescription,
                BankBranchName = input.BankBranchName,
                SWIFTCode = input.SWIFTCode,
                EuroAccountIBAN = input.EuroAccountIBAN,
                EuroAccountNo = input.EuroAccountNo,
                GBPAccountIBAN = input.GBPAccountIBAN,
                GBPAccountNo = input.GBPAccountNo,
                TLAccountIBAN = input.TLAccountIBAN,
                TLAccountNo = input.TLAccountNo,
                USDAccountIBAN = input.USDAccountIBAN,
                USDAccountNo = input.USDAccountNo,
            });

            var BankAccounts = queryFactory.Insert<SelectBankAccountsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("BankAccountsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.BankAccounts, LogType.Insert, addedEntityId);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectBankAccountsDto>(BankAccounts);


        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("BankAccountID", new List<string>
            {
                Tables.PackingLists
            });



            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var query = queryFactory.Query().From(Tables.BankAccounts).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var BankAccounts = queryFactory.Update<SelectBankAccountsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.BankAccounts, LogType.Delete, id);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectBankAccountsDto>(BankAccounts);
            }
        }


        public async Task<IDataResult<SelectBankAccountsDto>> GetAsync(Guid id)
        {

            var query = queryFactory.Query().From(Tables.BankAccounts).Select("*").Where(
            new
            {
                Id = id
            }, false, false, "");

            var BankAccount = queryFactory.Get<SelectBankAccountsDto>(query);

            LogsAppService.InsertLogToDatabase(BankAccount, BankAccount, LoginedUserService.UserId, Tables.BankAccounts, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectBankAccountsDto>(BankAccount);


        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListBankAccountsDto>>> GetListAsync(ListBankAccountsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.BankAccounts).Select("*").Where(null, false, false, "");

            var BankAccounts = queryFactory.GetList<ListBankAccountsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListBankAccountsDto>>(BankAccounts);

        }


        [ValidationAspect(typeof(UpdateBankAccountsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectBankAccountsDto>> UpdateAsync(UpdateBankAccountsDto input)
        {

            var entityQuery = queryFactory.Query().From(Tables.BankAccounts).Select("*").Where(new { Id = input.Id }, false, false, "");
            var entity = queryFactory.Get<BankAccounts>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.BankAccounts).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.GetList<BankAccounts>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.BankAccounts).Update(new UpdateBankAccountsDto
            {
                Code = input.Code,
                Name = input.Name,
                SWIFTCode = input.SWIFTCode,
                BankInstructionDescription = input.BankInstructionDescription,
                BankBranchName = input.BankBranchName,
                Address = input.Address,
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
                EuroAccountIBAN = input.EuroAccountIBAN,
                EuroAccountNo = input.EuroAccountNo,
                GBPAccountIBAN = input.GBPAccountIBAN,
                GBPAccountNo = input.GBPAccountNo,
                TLAccountIBAN = input.TLAccountIBAN,
                TLAccountNo = input.TLAccountNo,
                USDAccountIBAN = input.USDAccountIBAN,
                USDAccountNo = input.USDAccountNo,
            }).Where(new { Id = input.Id }, false, false, "");

            var BankAccounts = queryFactory.Update<SelectBankAccountsDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, BankAccounts, LoginedUserService.UserId, Tables.BankAccounts, LogType.Update, entity.Id);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectBankAccountsDto>(BankAccounts);

        }

        public async Task<IDataResult<SelectBankAccountsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {

            var entityQuery = queryFactory.Query().From(Tables.BankAccounts).Select("*").Where(new { Id = id }, false, false, "");
            var entity = queryFactory.Get<BankAccounts>(entityQuery);

            var query = queryFactory.Query().From(Tables.BankAccounts).Update(new UpdateBankAccountsDto
            {
                Code = entity.Code,
                Name = entity.Name,
                Address = entity.Address,
                BankInstructionDescription = entity.BankInstructionDescription,
                BankBranchName = entity.BankBranchName,
                EuroAccountIBAN = entity.EuroAccountIBAN,
                EuroAccountNo = entity.EuroAccountNo,
                GBPAccountIBAN = entity.GBPAccountIBAN,
                GBPAccountNo = entity.GBPAccountNo,
                TLAccountIBAN = entity.TLAccountIBAN,
                TLAccountNo = entity.TLAccountNo,
                USDAccountIBAN = entity.USDAccountIBAN,
                USDAccountNo = entity.USDAccountNo,
                SWIFTCode = entity.SWIFTCode,
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
            }).Where(new { Id = id }, false, false, "");

            var BankAccounts = queryFactory.Update<SelectBankAccountsDto>(query, "Id", true);

            await Task.CompletedTask;

            return new SuccessDataResult<SelectBankAccountsDto>(BankAccounts);


        }
    }
}
