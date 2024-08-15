using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Currency.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Currencies.Page;

namespace TsiErp.Business.Entities.Currency.Services
{
    [ServiceRegistration(typeof(ICurrenciesAppService), DependencyInjectionType.Scoped)]
    public class CurrenciesAppService : ApplicationService<CurrenciesResource>, ICurrenciesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public CurrenciesAppService(IStringLocalizer<CurrenciesResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }


        [ValidationAspect(typeof(CreateCurrenciesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCurrenciesDto>> CreateAsync(CreateCurrenciesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.Currencies).Select("Code").Where(new { Code = input.Code },  "");

            var list = queryFactory.ControlList<Currencies>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.Currencies).Insert(new CreateCurrenciesDto
            {
                Code = input.Code,
                Name = input.Name,
                IsLocalCurrency = input.IsLocalCurrency,
                Id = addedEntityId,
                CreationTime =now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                IsDeleted = false,
                CurrencySymbol = input.CurrencySymbol
            });


            var currencies = queryFactory.Insert<SelectCurrenciesDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("CurrenciesChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Currencies, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["CurrenciesChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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

            return new SuccessDataResult<SelectCurrenciesDto>(currencies);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("CurrencyID", new List<string>
            {
                Tables.CurrentAccountCards,
                Tables.ExchangeRates,
                Tables.PurchaseOrders,
                Tables.PurchasePriceLines,
                Tables.PurchasePrices,
                Tables.PurchaseRequests,
                Tables.SalesOrders,
                Tables.SalesPriceLines,
                Tables.SalesPrices,
                Tables.SalesPropositions,
                Tables.StockFiches
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var entity = (await GetAsync(id)).Data;
                var query = queryFactory.Query().From(Tables.Currencies).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var currencies = queryFactory.Update<SelectCurrenciesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Currencies, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["CurrenciesChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                return new SuccessDataResult<SelectCurrenciesDto>(currencies);
            }
        }


        public async Task<IDataResult<SelectCurrenciesDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.Currencies).Select("*").Where(
            new
            {
                Id = id
            }, "");
            var currency = queryFactory.Get<SelectCurrenciesDto>(query);


            LogsAppService.InsertLogToDatabase(currency, currency, LoginedUserService.UserId, Tables.Currencies, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCurrenciesDto>(currency);

        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListCurrenciesDto>>> GetListAsync(ListCurrenciesParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.Currencies).Select<Currencies>(s => new { s.Code, s.Name, s.IsLocalCurrency, s.CurrencySymbol, s.Id }).Where(null, "");
            var currencies = queryFactory.GetList<ListCurrenciesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListCurrenciesDto>>(currencies);

        }


        [ValidationAspect(typeof(UpdateCurrenciesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCurrenciesDto>> UpdateAsync(UpdateCurrenciesDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.Currencies).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<Currencies>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.Currencies).Select("*").Where(new { Code = input.Code }, "");
            var list = queryFactory.GetList<Currencies>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.Currencies).Update(new UpdateCurrenciesDto
            {
                Code = input.Code,
                Name = input.Name,
                IsLocalCurrency = input.IsLocalCurrency,
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
                CurrencySymbol = input.CurrencySymbol
            }).Where(new { Id = input.Id }, "");

            var currencies = queryFactory.Update<SelectCurrenciesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, currencies, LoginedUserService.UserId, Tables.Currencies, LogType.Update, entity.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["CurrenciesChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectCurrenciesDto>(currencies);

        }

        public async Task<IDataResult<SelectCurrenciesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.Currencies).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<Currencies>(entityQuery);

            var query = queryFactory.Query().From(Tables.Currencies).Update(new UpdateCurrenciesDto
            {
                Code = entity.Code,
                Name = entity.Name,
                IsLocalCurrency = entity.IsLocalCurrency,
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
                CurrencySymbol = entity.CurrencySymbol
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var currencies = queryFactory.Update<SelectCurrenciesDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectCurrenciesDto>(currencies);

        }
    }
}
