using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Entities.StockManagement.ProductCostCost.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.ProductCost;
using TsiErp.Entities.Entities.StockManagement.ProductCost.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ProductCosts.Page;

namespace TsiErp.Business.Entities.ProductCost.Services
{
    [ServiceRegistration(typeof(IProductCostsAppService), DependencyInjectionType.Scoped)]
    public class ProductCostsAppService : ApplicationService<ProductCostsResource>, IProductCostsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public ProductCostsAppService(IStringLocalizer<ProductCostsResource> l, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }

        [ValidationAspect(typeof(CreateProductCostsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductCostsDto>> CreateAsync(CreateProductCostsDto input)
        {

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.ProductCosts).Insert(new CreateProductCostsDto
            {
                Id = addedEntityId,
                EndDate = input.EndDate,
                BillCost = input.BillCost,
                Overheads = input.Overheads,
                ProductionCost = input.ProductionCost,
                UnsuitabilityCost = input.UnsuitabilityCost,
                ProductID = input.ProductID.GetValueOrDefault(),
                StartDate = input.StartDate,
                UnitCost = input.UnitCost
            });

            var costList = (await GetListByProductIdAsync(input.ProductID.GetValueOrDefault())).Data.ToList();

            foreach (var cost in costList)
            {
                UpdateProductCostsDto costsDto = new UpdateProductCostsDto
                {
                    Id = cost.Id,
                    EndDate = cost.EndDate,
                    ProductID = cost.ProductID.GetValueOrDefault(),
                    StartDate = cost.StartDate,
                    UnitCost = cost.UnitCost
                };

                var updatedCostLine = (await UpdateAsync(costsDto)).Data;
            }


            var ProductCosts = queryFactory.Insert<SelectProductCostsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ProductCosts, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ProductCostsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
                                RecordNumber = string.Empty,
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
                            RecordNumber = string.Empty,
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

            return new SuccessDataResult<SelectProductCostsDto>(ProductCosts);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.ProductCosts).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

            var ProductCosts = queryFactory.Update<SelectProductCostsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ProductCosts, LogType.Delete, id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ProductCostsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                                RecordNumber = string.Empty,
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
                            RecordNumber = string.Empty,
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
            return new SuccessDataResult<SelectProductCostsDto>(ProductCosts);

        }

        public async Task<IDataResult<SelectProductCostsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.ProductCosts).Select<ProductCosts>(null)
                        .Join<Products>
                        (
                            u => new { ProductCode = u.Code, ProductID = u.Id, ProductName = u.Name },
                            nameof(ProductCosts.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id },  Tables.ProductCosts);

            var ProductCost = queryFactory.Get<SelectProductCostsDto>(query);

            LogsAppService.InsertLogToDatabase(ProductCost, ProductCost, LoginedUserService.UserId, Tables.ProductCosts, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductCostsDto>(ProductCost);


        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductCostsDto>>> GetListAsync(ListProductCostsParameterDto input)
        {
            var query = queryFactory
               .Query()
               .From(Tables.ProductCosts).Select<ProductCosts>(null)
                        .Join<Products>
                        (
                            u => new { ProductCode = u.Code, ProductID = u.Id, ProductName = u.Name },
                            nameof(ProductCosts.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        ).Where(null,  Tables.ProductCosts);

            var productCosts = queryFactory.GetList<ListProductCostsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListProductCostsDto>>(productCosts);

        }

        public async Task<IDataResult<IList<ListProductCostsDto>>> GetListByProductIdAsync(Guid productId)
        {
            var query = queryFactory
               .Query()
               .From(Tables.ProductCosts).Select<ProductCosts>(null)
                        .Join<Products>
                        (
                            u => new { ProductCode = u.Code, ProductID = u.Id, ProductName = u.Name },
                            nameof(ProductCosts.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        ).Where(new { ProductID = productId }, Tables.ProductCosts);

            var productCosts = queryFactory.GetList<ListProductCostsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListProductCostsDto>>(productCosts);
        }

        [ValidationAspect(typeof(UpdateProductCostsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductCostsDto>> UpdateAsync(UpdateProductCostsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.ProductCosts).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<ProductCosts>(entityQuery);

            var query = queryFactory.Query().From(Tables.ProductCosts).Update(new UpdateProductCostsDto
            {
                Id = input.Id,
                EndDate = input.EndDate,
                BillCost = input.BillCost,
                Overheads = input.Overheads,
                ProductionCost = input.ProductionCost,
                UnsuitabilityCost = input.UnsuitabilityCost,
                StartDate = input.StartDate,
                UnitCost = input.UnitCost,
                ProductID = input.ProductID.GetValueOrDefault(),
            }).Where(new { Id = input.Id }, "");

            var ProductCosts = queryFactory.Update<SelectProductCostsDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, ProductCosts, LoginedUserService.UserId, Tables.ProductCosts, LogType.Update, entity.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ProductCostsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
                                RecordNumber = string.Empty,
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
                            RecordNumber = string.Empty,
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

            return new SuccessDataResult<SelectProductCostsDto>(ProductCosts);

        }

        

        #region Unused Methods

        public Task<IDataResult<SelectProductCostsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }




        #endregion
    }
}
