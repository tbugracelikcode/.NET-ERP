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
using TsiErp.Business.Entities.CurrentAccountCard.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.CurrentAccountCards.Page;

namespace TsiErp.Business.Entities.CurrentAccountCard.Services
{
    [ServiceRegistration(typeof(ICurrentAccountCardsAppService), DependencyInjectionType.Scoped)]
    public class CurrentAccountCardsAppService : ApplicationService<CurrentAccountCardsResource>, ICurrentAccountCardsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public CurrentAccountCardsAppService(IStringLocalizer<CurrentAccountCardsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }

        [ValidationAspect(typeof(CreateCurrentAccountCardsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCurrentAccountCardsDto>> CreateAsync(CreateCurrentAccountCardsDto input)
        {

            var listQuery = queryFactory.Query().From(Tables.CurrentAccountCards).Select("Code").Where(new { Code = input.Code },  "");

            var list = queryFactory.ControlList<CurrentAccountCards>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.CurrentAccountCards).Insert(new CreateCurrentAccountCardsDto
            {
                Code = input.Code,
                Address1 = input.Address1,
                CustomerCode = input.CustomerCode,
                ShippingCompany = input.ShippingCompany,
                Address2 = input.Address2,
                City = input.City,
                CoatingCustomer = input.CoatingCustomer,
                ContractSupplier = input.ContractSupplier,
                PaymentTermDay = input.PaymentTermDay,
                EORINr = input.EORINr,
                Country = input.Country,
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                District = input.District,
                Email = input.Email,
                Fax = input.Fax,
                IDnumber = input.IDnumber,
                BigPackageKG = input.BigPackageKG,
                SmallPackageKG = input.SmallPackageKG,
                PlusPercentage = input.PlusPercentage,
                PostCode = input.PostCode,
                PrivateCode1 = input.PrivateCode1,
                IsSoftwareCompanyInformation = input.IsSoftwareCompanyInformation,
                PrivateCode2 = input.PrivateCode2,
                PrivateCode3 = input.PrivateCode3,
                PrivateCode4 = input.PrivateCode4,
                PrivateCode5 = input.PrivateCode5,
                Responsible = input.Responsible,
                SaleContract = input.SaleContract,
                ShippingAddress = input.ShippingAddress,
                SoleProprietorship = input.SoleProprietorship,
                Supplier = input.Supplier,
                SupplierNo = input.SupplierNo,
                TaxAdministration = input.TaxAdministration,
                TaxNumber = input.TaxNumber,
                Tel1 = input.Tel1,
                Tel2 = input.Tel2,
                Type_ = input.Type_,
                Web = input.Web,
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
                ContractDailyWorkingCapacity = input.ContractDailyWorkingCapacity,
                NumberOfStations = input.NumberOfStations
            });

            var currentAccountCards = queryFactory.Insert<SelectCurrentAccountCardsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("CurrentAccountsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.CurrentAccountCards, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["CurrentAccountsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectCurrentAccountCardsDto>(currentAccountCards);


        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("CustomerCurrentAccountCardID", new List<string>
            {
                Tables.TechnicalDrawings
            });

            DeleteControl.ControlList.Add("CurrentAccountID", new List<string>
            {
                Tables.PackageFiches,
                Tables.ProductionOrders
            });

            DeleteControl.ControlList.Add("CurrentAccountCardID", new List<string>
            {
                Tables.BillsofMaterials,
                Tables.ContractOfProductsOperations,
                Tables.ContractProductionTrackings,
                Tables.ContractTrackingFiches,
                Tables.ContractUnsuitabilityReports,
                Tables.Forecasts,
                Tables.PalletRecordLines,
                Tables.PalletRecords,
                Tables.ProductionTrackings,
                Tables.ProductReferanceNumbers,
                Tables.PurchaseOrders,
                Tables.PurchasePriceLines,
                Tables.PurchasePrices,
                Tables.PurchaseRequests,
                Tables.PurchaseUnsuitabilityReports,
                Tables.SalesOrders,
                Tables.SalesPriceLines,
                Tables.SalesPrices,
                Tables.SalesPropositions,
                Tables.OrderAcceptanceRecords,
                Tables.WorkOrders
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var entity = (await GetAsync(id)).Data;
                var query = queryFactory.Query().From(Tables.CurrentAccountCards).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var currentAccountCards = queryFactory.Update<SelectCurrentAccountCardsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.CurrentAccountCards, LogType.Delete, id);

                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["CurrentAccountsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                return new SuccessDataResult<SelectCurrentAccountCardsDto>(currentAccountCards);
            }
        }


        public async Task<IDataResult<SelectCurrentAccountCardsDto>> GetAsync(Guid id)
        {

            var query = queryFactory
                    .Query().From(Tables.CurrentAccountCards).Select<CurrentAccountCards>(null)
                        .Join<Currencies>
                        (
                            c => new { Currency = c.Code, CurrencyID = c.Id },
                            nameof(CurrentAccountCards.CurrencyID),
                            nameof(Currencies.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, Tables.CurrentAccountCards);

            var currentAccountCard = queryFactory.Get<SelectCurrentAccountCardsDto>(query);

            LogsAppService.InsertLogToDatabase(currentAccountCard, currentAccountCard, LoginedUserService.UserId, Tables.CurrentAccountCards, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCurrentAccountCardsDto>(currentAccountCard);


        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListCurrentAccountCardsDto>>> GetListAsync(ListCurrentAccountCardsParameterDto input)
        {
            var query = queryFactory
               .Query()
               .From(Tables.CurrentAccountCards)
               .Select<CurrentAccountCards>(s => new { s.Name, s.CustomerCode, s.Responsible, s.Email, s.TaxNumber, s.IsSoftwareCompanyInformation, s.Tel1, s.Id })
                   .Join<Currencies>
                   (
                       c => new { Currency = c.Code, CurrencyID = c.Id },
                       nameof(CurrentAccountCards.CurrencyID),
                       nameof(Currencies.Id),
                       JoinType.Left
                   ).Where(null, Tables.CurrentAccountCards);

            var currentAccountCards = queryFactory.GetList<ListCurrentAccountCardsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListCurrentAccountCardsDto>>(currentAccountCards);

        }


        [ValidationAspect(typeof(UpdateCurrentAccountCardsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCurrentAccountCardsDto>> UpdateAsync(UpdateCurrentAccountCardsDto input)
        {

            var entityQuery = queryFactory.Query().From(Tables.CurrentAccountCards).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<CurrentAccountCards>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.CurrentAccountCards).Select("*").Where(new { Code = input.Code }, "");
            var list = queryFactory.GetList<CurrentAccountCards>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.CurrentAccountCards).Update(new UpdateCurrentAccountCardsDto
            {
                Code = input.Code,
                PlusPercentage = input.PlusPercentage,
                PostCode = input.PostCode,
                Address1 = input.Address1,
                PrivateCode1 = input.PrivateCode1,
                ShippingCompany = input.ShippingCompany,
                EORINr = input.EORINr,
                PrivateCode2 = input.PrivateCode2,
                Address2 = input.Address2,
                City = input.City,
                CoatingCustomer = input.CoatingCustomer,
                ContractSupplier = input.ContractSupplier,
                BigPackageKG = input.BigPackageKG,
                SmallPackageKG = input.SmallPackageKG,
                PaymentTermDay = input.PaymentTermDay,
                CustomerCode = input.CustomerCode,
                Country = input.Country,
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                District = input.District,
                Email = input.Email,
                Fax = input.Fax,
                IDnumber = input.IDnumber,
                IsSoftwareCompanyInformation = input.IsSoftwareCompanyInformation,
                PrivateCode3 = input.PrivateCode3,
                PrivateCode4 = input.PrivateCode4,
                PrivateCode5 = input.PrivateCode5,
                Responsible = input.Responsible,
                SaleContract = input.SaleContract,
                ShippingAddress = input.ShippingAddress,
                SoleProprietorship = input.SoleProprietorship,
                Supplier = input.Supplier,
                SupplierNo = input.SupplierNo,
                TaxAdministration = input.TaxAdministration,
                TaxNumber = input.TaxNumber,
                Tel1 = input.Tel1,
                Tel2 = input.Tel2,
                Type_ = input.Type_,
                Web = input.Web,
                Name = input.Name,
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
                ContractDailyWorkingCapacity = input.ContractDailyWorkingCapacity,
                NumberOfStations = input.NumberOfStations
            }).Where(new { Id = input.Id }, "");

            var currentAccountCards = queryFactory.Update<SelectCurrentAccountCardsDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, currentAccountCards, LoginedUserService.UserId, Tables.CurrentAccountCards, LogType.Update, entity.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["CurrentAccountsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectCurrentAccountCardsDto>(currentAccountCards);

        }

        public async Task<IDataResult<SelectCurrentAccountCardsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {

            var entityQuery = queryFactory.Query().From(Tables.CurrentAccountCards).Select("Id").Where(new { Id = id }, "");
            var entity = queryFactory.Get<CurrentAccountCards>(entityQuery);

            var query = queryFactory.Query().From(Tables.CurrentAccountCards).Update(new UpdateCurrentAccountCardsDto
            {
                Code = entity.Code,
                CustomerCode = entity.CustomerCode,
                ShippingCompany = entity.ShippingCompany,
                Name = entity.Name,
                PaymentTermDay = entity.PaymentTermDay,
                PlusPercentage = entity.PlusPercentage,
                PostCode = entity.PostCode,
                Address1 = entity.Address1,
                PrivateCode1 = entity.PrivateCode1,
                PrivateCode2 = entity.PrivateCode2,
                EORINr = entity.EORINr,
                Address2 = entity.Address2,
                City = entity.City,
                CoatingCustomer = entity.CoatingCustomer,
                ContractSupplier = entity.ContractSupplier,
                BigPackageKG = entity.BigPackageKG,
                SmallPackageKG = entity.SmallPackageKG,
                Country = entity.Country,
                CurrencyID = entity.CurrencyID,
                District = entity.District,
                Email = entity.Email,
                Fax = entity.Fax,
                IDnumber = entity.IDnumber,
                PrivateCode3 = entity.PrivateCode3,
                PrivateCode4 = entity.PrivateCode4,
                IsSoftwareCompanyInformation = entity.IsSoftwareCompanyInformation,
                PrivateCode5 = entity.PrivateCode5,
                Responsible = entity.Responsible,
                SaleContract = entity.SaleContract,
                ShippingAddress = entity.ShippingAddress,
                SoleProprietorship = entity.SoleProprietorship,
                Supplier = entity.Supplier,
                SupplierNo = entity.SupplierNo,
                TaxAdministration = entity.TaxAdministration,
                TaxNumber = entity.TaxNumber,
                Tel1 = entity.Tel1,
                Tel2 = entity.Tel2,
                Type_ = entity.Type_,
                Web = entity.Web,
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
                NumberOfStations = entity.NumberOfStations,
                ContractDailyWorkingCapacity = entity.ContractDailyWorkingCapacity
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id },  "");

            var currentAccountCards = queryFactory.Update<SelectCurrentAccountCardsDto>(query, "Id", true);

            await Task.CompletedTask;

            return new SuccessDataResult<SelectCurrentAccountCardsDto>(currentAccountCards);


        }
    }
}
