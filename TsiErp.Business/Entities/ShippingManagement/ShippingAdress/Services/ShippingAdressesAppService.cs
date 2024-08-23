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
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Entities.ShippingAdress.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress;
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress.Dtos;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ShippingAdresses.Page;

namespace TsiErp.Business.Entities.ShippingAdress.Services
{
    [ServiceRegistration(typeof(IShippingAdressesAppService), DependencyInjectionType.Scoped)]
    public class ShippingAdressesAppService : ApplicationService<ShippingAdressesResource>, IShippingAdressesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public ShippingAdressesAppService(IStringLocalizer<ShippingAdressesResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }



        [ValidationAspect(typeof(CreateShippingAdressesValidator), Priority = 1)]
        public async Task<IDataResult<SelectShippingAdressesDto>> CreateAsync(CreateShippingAdressesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.ShippingAdresses).Select("Code").Where(new { Code = input.Code },  "");

            var list = queryFactory.ControlList<ShippingAdresses>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.ShippingAdresses).Insert(new CreateShippingAdressesDto
            {
                Adress1 = input.Adress1,
                Adress2 = input.Adress2,
                City = input.City,
                Country = input.Country,
                CustomerCardID = input.CustomerCardID.GetValueOrDefault(),
                District = input.District,
                EMail = input.EMail,
                Fax = input.Fax,
                Phone = input.Phone,
                PostCode = input.PostCode,
                _Default = input._Default,
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
                Name = input.Name
            });

            var shippingAdresses = queryFactory.Insert<SelectShippingAdressesDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("ShippingAdressesChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ShippingAdresses, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ShippingAdressesChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectShippingAdressesDto>(shippingAdresses);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("ShippingAdressID", new List<string>
            {
                Tables.PurchaseOrders,
                Tables.SalesOrders,
                Tables.SalesPropositions
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var entity = (await GetAsync(id)).Data;
                var query = queryFactory.Query().From(Tables.ShippingAdresses).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var shippingAdresses = queryFactory.Update<SelectShippingAdressesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ShippingAdresses, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ShippingAdressesChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                return new SuccessDataResult<SelectShippingAdressesDto>(shippingAdresses);
            }
        }

        public async Task<IDataResult<SelectShippingAdressesDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.ShippingAdresses).Select<ShippingAdresses>(null)
                        .Join<CurrentAccountCards>
                        (
                            ca => new { CustomerCardID = ca.Id, CustomerCardCode = ca.Code, CustomerCardName = ca.Name },
                            nameof(ShippingAdresses.CustomerCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, Tables.ShippingAdresses);

            var shippingAdress = queryFactory.Get<SelectShippingAdressesDto>(query);

            LogsAppService.InsertLogToDatabase(shippingAdress, shippingAdress, LoginedUserService.UserId, Tables.ShippingAdresses, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectShippingAdressesDto>(shippingAdress);


        }

        public async Task<IDataResult<IList<ListShippingAdressesDto>>> GetListAsync(ListShippingAdressesParameterDto input)
        {

            var query = queryFactory
               .Query()
               .From(Tables.ShippingAdresses).Select<ShippingAdresses>(s => new { s.Code, s.Name, s.Country , s.City, s.District, s._Default, s.Id })
                        .Join<CurrentAccountCards>
                        (
                            ca => new { CustomerCardCode = ca.Code, CustomerCardName = ca.Name },
                            nameof(ShippingAdresses.CustomerCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        ).Where(null, Tables.ShippingAdresses);

            var shippingAdresses = queryFactory.GetList<ListShippingAdressesDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListShippingAdressesDto>>(shippingAdresses);


        }

        [ValidationAspect(typeof(UpdateShippingAdressesValidator), Priority = 1)]
        public async Task<IDataResult<SelectShippingAdressesDto>> UpdateAsync(UpdateShippingAdressesDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.ShippingAdresses).Select("*").Where(new { Id = input.Id },  "");
            var entity = queryFactory.Get<ShippingAdresses>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.ShippingAdresses).Select("*").Where(new { Code = input.Code },  "");
            var list = queryFactory.GetList<ShippingAdresses>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.ShippingAdresses).Update(new UpdateShippingAdressesDto
            {
                Adress1 = input.Adress1,
                Adress2 = input.Adress2,
                City = input.City,
                Country = input.Country,
                CustomerCardID = input.CustomerCardID.GetValueOrDefault(),
                District = input.District,
                EMail = input.EMail,
                Fax = input.Fax,
                Phone = input.Phone,
                PostCode = input.PostCode,
                _Default = input._Default,
                Code = input.Code,
                Name = input.Name,
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id },  "");

            var shippingAdresses = queryFactory.Update<SelectShippingAdressesDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, shippingAdresses, LoginedUserService.UserId, Tables.ShippingAdresses, LogType.Update, entity.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ShippingAdressesChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectShippingAdressesDto>(shippingAdresses);

        }

        public async Task<IDataResult<SelectShippingAdressesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.ShippingAdresses).Select("*").Where(new { Id = id }, "");
            var entity = queryFactory.Get<ShippingAdresses>(entityQuery);

            var query = queryFactory.Query().From(Tables.ShippingAdresses).Update(new UpdateShippingAdressesDto
            {
                Adress1 = entity.Adress1,
                Adress2 = entity.Adress2,
                City = entity.City,
                Country = entity.Country,
                CustomerCardID = entity.CustomerCardID.GetValueOrDefault(),
                District = entity.District,
                EMail = entity.EMail,
                Fax = entity.Fax,
                Phone = entity.Phone,
                PostCode = entity.PostCode,
                _Default = entity._Default,
                Code = entity.Code,
                Name = entity.Name,
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

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id },  "");

            var shippingAdresses = queryFactory.Update<SelectShippingAdressesDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectShippingAdressesDto>(shippingAdresses);

        }
    }
}
