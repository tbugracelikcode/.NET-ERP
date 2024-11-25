using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.CompanyCheck.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount;
using TsiErp.Entities.Entities.FinanceManagement.CompanyCheck;
using TsiErp.Entities.Entities.FinanceManagement.CompanyCheck.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.CompanyChecks.Page;

namespace TsiErp.Business.Entities.CompanyCheck.Services
{
    [ServiceRegistration(typeof(ICompanyChecksAppService), DependencyInjectionType.Scoped)]
    public class CompanyChecksAppService : ApplicationService<CompanyChecksResource>, ICompanyChecksAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public CompanyChecksAppService(IStringLocalizer<CompanyChecksResource> l, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }

        [ValidationAspect(typeof(CreateCompanyChecksValidator), Priority = 1)]
        public async Task<IDataResult<SelectCompanyChecksDto>> CreateAsync(CreateCompanyChecksDto input)
        {


            Guid addedEntityId = GuidGenerator.CreateGuid();

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.CompanyChecks).Insert(new CreateCompanyChecksDto
            {

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
                Amount_ = input.Amount_,
                BankAccountID = input.BankAccountID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                CompanyChecksPaymentState = input.CompanyChecksPaymentState,
                CompanyChecksState = input.CompanyChecksState,
                DueDate = input.DueDate,
                SerialNo = input.SerialNo,
            });

            var CompanyChecks = queryFactory.Insert<SelectCompanyChecksDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.CompanyChecks, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["CompanyChecksChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
                                RecordNumber = input.SerialNo,
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
                            RecordNumber = input.SerialNo,
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
            return new SuccessDataResult<SelectCompanyChecksDto>(CompanyChecks);


        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.CompanyChecks).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var CompanyChecks = queryFactory.Update<SelectCompanyChecksDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.CompanyChecks, LogType.Delete, id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["CompanyChecksChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                                RecordNumber = entity.SerialNo,
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
                            RecordNumber = entity.SerialNo,
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
            return new SuccessDataResult<SelectCompanyChecksDto>(CompanyChecks);
        }


        public async Task<IDataResult<SelectCompanyChecksDto>> GetAsync(Guid id)
        {

            var query = queryFactory
                    .Query().From(Tables.CompanyChecks).Select<CompanyChecks>(null)
                        .Join<CurrentAccountCards>
                        (
                            c => new { CurrentAccountCardCode = c.Code, CurrentAccountCardID = c.Id, CurrentAccountCardName = c.Name },
                            nameof(CompanyChecks.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                         .Join<BankAccounts>
                        (
                            c => new { BankAccountID = c.Id, BankAccountName = c.Name },
                            nameof(CompanyChecks.BankAccountID),
                            nameof(BankAccounts.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, Tables.CompanyChecks);

            var CompanyCheck = queryFactory.Get<SelectCompanyChecksDto>(query);

            LogsAppService.InsertLogToDatabase(CompanyCheck, CompanyCheck, LoginedUserService.UserId, Tables.CompanyChecks, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCompanyChecksDto>(CompanyCheck);


        }

        public async Task<IDataResult<IList<ListCompanyChecksDto>>> GetListAsync(ListCompanyChecksParameterDto input)
        {
            var query = queryFactory
               .Query()
               .From(Tables.CompanyChecks)
               .Select<CompanyChecks>(null)
                    .Join<CurrentAccountCards>
                        (
                            c => new { CurrentAccountCardCode = c.Code, CurrentAccountCardID = c.Id, CurrentAccountCardName = c.Name },
                            nameof(CompanyChecks.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                         .Join<BankAccounts>
                        (
                            c => new { BankAccountID = c.Id, BankAccountName = c.Name },
                            nameof(CompanyChecks.BankAccountID),
                            nameof(BankAccounts.Id),
                            JoinType.Left
                        ).Where(null, Tables.CompanyChecks);

            var companyChecks = queryFactory.GetList<ListCompanyChecksDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListCompanyChecksDto>>(companyChecks);

        }


        [ValidationAspect(typeof(UpdateCompanyChecksValidator), Priority = 1)]
        public async Task<IDataResult<SelectCompanyChecksDto>> UpdateAsync(UpdateCompanyChecksDto input)
        {

            var entityQuery = queryFactory.Query().From(Tables.CompanyChecks).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<CompanyChecks>(entityQuery);



            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.CompanyChecks).Update(new UpdateCompanyChecksDto
            {

                Id = input.Id,
                Amount_ = input.Amount_,
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                SerialNo = input.SerialNo,
                DueDate = input.DueDate,
                CompanyChecksState = input.CompanyChecksState,
                BankAccountID = input.BankAccountID.GetValueOrDefault(),
                CompanyChecksPaymentState = input.CompanyChecksPaymentState,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id }, "");

            var CompanyChecks = queryFactory.Update<SelectCompanyChecksDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, CompanyChecks, LoginedUserService.UserId, Tables.CompanyChecks, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["CompanyChecksChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
                                RecordNumber = input.SerialNo,
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
                            RecordNumber = input.SerialNo,
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
            return new SuccessDataResult<SelectCompanyChecksDto>(CompanyChecks);

        }

        public async Task<IDataResult<SelectCompanyChecksDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {

            var entityQuery = queryFactory.Query().From(Tables.CompanyChecks).Select("*").Where(new { Id = id }, "");
            var entity = queryFactory.Get<CompanyChecks>(entityQuery);

            var query = queryFactory.Query().From(Tables.CompanyChecks).Update(new UpdateCompanyChecksDto
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
                SerialNo = entity.SerialNo,
                CompanyChecksPaymentState = (int)entity.CompanyChecksPaymentState,
                CompanyChecksState = (int)entity.CompanyChecksState,
                BankAccountID = entity.BankAccountID,
                DueDate = entity.DueDate,
                CurrentAccountCardID = entity.CurrentAccountCardID,
                Amount_ = entity.Amount_

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var CompanyChecks = queryFactory.Update<SelectCompanyChecksDto>(query, "Id", true);

            await Task.CompletedTask;

            return new SuccessDataResult<SelectCompanyChecksDto>(CompanyChecks);


        }
    }
}
