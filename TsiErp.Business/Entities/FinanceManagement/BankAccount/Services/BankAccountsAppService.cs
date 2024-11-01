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
using TsiErp.Business.Entities.FinanceManagement.BankAccount.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
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
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public BankAccountsAppService(IStringLocalizer<BankAccountsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }

        [ValidationAspect(typeof(CreateBankAccountsValidator), Priority = 1)]
        public async Task<IDataResult<SelectBankAccountsDto>> CreateAsync(CreateBankAccountsDto input)
        {

            var listQuery = queryFactory.Query().From(Tables.BankAccounts).Select("Code").Where(new { Code = input.Code }, "");

            var list = queryFactory.ControlList<BankAccounts>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();


            var query = queryFactory.Query().From(Tables.BankAccounts).Insert(new CreateBankAccountsDto
            {
                Code = input.Code,
                CreationTime = now,
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
                AccountIBAN = input.AccountIBAN,
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                AccountNo = input.AccountNo,
            });

            var BankAccounts = queryFactory.Insert<SelectBankAccountsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("BankAccountsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.BankAccounts, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["BankAccountsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectBankAccountsDto>(BankAccounts);


        }


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
                var entity = (await GetAsync(id)).Data;
                var query = queryFactory.Query().From(Tables.BankAccounts).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var BankAccounts = queryFactory.Update<SelectBankAccountsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.BankAccounts, LogType.Delete, id);

                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["BankAccountsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                return new SuccessDataResult<SelectBankAccountsDto>(BankAccounts);
            }
        }


        public async Task<IDataResult<SelectBankAccountsDto>> GetAsync(Guid id)
        {

            var query = queryFactory.Query().From(Tables.BankAccounts).Select<BankAccounts>(null)
                .Join<Currencies>
                    (
                        pr => new { CurrencyCode = pr.CurrencySymbol, CurrencyID = pr.Id },
                        nameof(BankAccounts.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                .Where( new {  Id = id }, Tables.BankAccounts);

            var BankAccount = queryFactory.Get<SelectBankAccountsDto>(query);

            LogsAppService.InsertLogToDatabase(BankAccount, BankAccount, LoginedUserService.UserId, Tables.BankAccounts, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectBankAccountsDto>(BankAccount);


        }


        public async Task<IDataResult<IList<ListBankAccountsDto>>> GetListAsync(ListBankAccountsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.BankAccounts).Select<BankAccounts>(null)
                .Join<Currencies>
                    (
                        pr => new { CurrencyCode = pr.CurrencySymbol, CurrencyID = pr.Id },
                        nameof(BankAccounts.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    ).Where(null, Tables.BankAccounts);

            var bankAccounts = queryFactory.GetList<ListBankAccountsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListBankAccountsDto>>(bankAccounts);

        }


        [ValidationAspect(typeof(UpdateBankAccountsValidator), Priority = 1)]
        public async Task<IDataResult<SelectBankAccountsDto>> UpdateAsync(UpdateBankAccountsDto input)
        {

            var entityQuery = queryFactory.Query().From(Tables.BankAccounts).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<BankAccounts>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.BankAccounts).Select("*").Where(new { Code = input.Code }, "");
            var list = queryFactory.GetList<BankAccounts>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

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
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
                AccountIBAN = input.AccountIBAN,
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                AccountNo = input.AccountNo,
            }).Where(new { Id = input.Id }, "");

            var BankAccounts = queryFactory.Update<SelectBankAccountsDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, BankAccounts, LoginedUserService.UserId, Tables.BankAccounts, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["BankAccountsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectBankAccountsDto>(BankAccounts);

        }

        public async Task<IDataResult<SelectBankAccountsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {

            var entityQuery = queryFactory.Query().From(Tables.BankAccounts).Select("*").Where(new { Id = id }, "");
            var entity = queryFactory.Get<BankAccounts>(entityQuery);

            var query = queryFactory.Query().From(Tables.BankAccounts).Update(new UpdateBankAccountsDto
            {
                Code = entity.Code,
                Name = entity.Name,
                Address = entity.Address,
                BankInstructionDescription = entity.BankInstructionDescription,
                BankBranchName = entity.BankBranchName,
                AccountIBAN = entity.AccountIBAN,
                CurrencyID = entity.CurrencyID,
                AccountNo = entity.AccountNo,
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
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var BankAccounts = queryFactory.Update<SelectBankAccountsDto>(query, "Id", true);

            await Task.CompletedTask;

            return new SuccessDataResult<SelectBankAccountsDto>(BankAccounts);


        }
    }
}
