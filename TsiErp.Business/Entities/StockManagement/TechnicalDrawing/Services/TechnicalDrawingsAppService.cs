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
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Entities.TechnicalDrawing.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.TechnicalDrawings.Page;

namespace TsiErp.Business.Entities.TechnicalDrawing.Services
{
    [ServiceRegistration(typeof(ITechnicalDrawingsAppService), DependencyInjectionType.Scoped)]
    public class TechnicalDrawingsAppService : ApplicationService<TechnicalDrawingsResource>, ITechnicalDrawingsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public TechnicalDrawingsAppService(IStringLocalizer<TechnicalDrawingsResource> l, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }

        [ValidationAspect(typeof(CreateTechnicalDrawingsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectTechnicalDrawingsDto>> CreateAsync(CreateTechnicalDrawingsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.TechnicalDrawings).Select("RevisionNo").Where(new { RevisionNo = input.RevisionNo },  "");

            var list = queryFactory.ControlList<TechnicalDrawings>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.TechnicalDrawings).Insert(new CreateTechnicalDrawingsDto
            {
                CustomerApproval = input.CustomerApproval,
                RevisionNo = input.RevisionNo,
                RevisionDate = input.RevisionDate,
                ProductID = input.ProductID.GetValueOrDefault(),
                CustomerCurrentAccountCardID = input.CustomerCurrentAccountCardID.GetValueOrDefault(),
                Drawer = input.Drawer,
                DrawingDomain = input.DrawingDomain,
                DrawingFilePath = input.DrawingFilePath,
                DrawingNo = input.DrawingNo,
                IsApproved = input.IsApproved,
                SampleApproval = input.SampleApproval,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Description_ = input.Description_,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
            });

            var technicalDrawings = queryFactory.Insert<SelectTechnicalDrawingsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.TechnicalDrawings, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["TechnicalDrawingsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
                                RecordNumber = input.RevisionNo,
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
                            RecordNumber = input.RevisionNo,
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
            return new SuccessDataResult<SelectTechnicalDrawingsDto>(technicalDrawings);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("TechnicalDrawingID", new List<string>
            {
                Tables.Report8Ds
            });


            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var entity = (await GetAsync(id)).Data;
                var query = queryFactory.Query().From(Tables.TechnicalDrawings).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

                var technicalDrawings = queryFactory.Update<SelectTechnicalDrawingsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.TechnicalDrawings, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["TechnicalDrawingsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                                    RecordNumber = entity.RevisionNo,
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
                                RecordNumber = entity.RevisionNo,
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
                return new SuccessDataResult<SelectTechnicalDrawingsDto>(technicalDrawings);
            }
        }


        public async Task<IDataResult<SelectTechnicalDrawingsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.TechnicalDrawings).Select<TechnicalDrawings>(null)
                        .Join<Products>
                        (
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                            nameof(TechnicalDrawings.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                          .Join<CurrentAccountCards>
                        (
                            p => new { CustomerCurrentAccountCardID = p.Id, CustomerCode = p.CustomerCode },
                            nameof(TechnicalDrawings.CustomerCurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id },  Tables.TechnicalDrawings);

            var technicalDrawing = queryFactory.Get<SelectTechnicalDrawingsDto>(query);

            LogsAppService.InsertLogToDatabase(technicalDrawing, technicalDrawing, LoginedUserService.UserId, Tables.TechnicalDrawings, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectTechnicalDrawingsDto>(technicalDrawing);


        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListTechnicalDrawingsDto>>> GetListAsync(ListTechnicalDrawingsParameterDto input)
        {
            var query = queryFactory
               .Query()
               .From(Tables.TechnicalDrawings).Select<TechnicalDrawings>(s => new { s.RevisionNo, s.RevisionDate, s.Drawer, s.Id })
                        .Join<Products>
                        (
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                            nameof(TechnicalDrawings.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                           .Join<CurrentAccountCards>
                        (
                            p => new { CustomerCode = p.CustomerCode },
                            nameof(TechnicalDrawings.CustomerCurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        ).Where(null,  Tables.TechnicalDrawings);

            var technicalDrawings = queryFactory.GetList<ListTechnicalDrawingsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListTechnicalDrawingsDto>>(technicalDrawings);

        }


        public async Task<IDataResult<IList<SelectTechnicalDrawingsDto>>> GetSelectListAsync(Guid productId)
        {
            var query = queryFactory
               .Query()
               .From(Tables.TechnicalDrawings).Select<TechnicalDrawings>(s => new { s.RevisionNo, s.RevisionDate, s.Drawer, s.Id })
                        .Join<Products>
                        (
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                            nameof(TechnicalDrawings.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                          .Join<CurrentAccountCards>
                        (
                            p => new { CustomerCurrentAccountCardID = p.Id, CustomerCode = p.CustomerCode },
                            nameof(TechnicalDrawings.CustomerCurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        ).Where(new { ProductID = productId },  Tables.TechnicalDrawings);

            var technicalDrawings = queryFactory.GetList<SelectTechnicalDrawingsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<SelectTechnicalDrawingsDto>>(technicalDrawings);

        }


        [ValidationAspect(typeof(UpdateTechnicalDrawingsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectTechnicalDrawingsDto>> UpdateAsync(UpdateTechnicalDrawingsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.TechnicalDrawings).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<TechnicalDrawings>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.TechnicalDrawings).Select("*").Where(new { RevisionNo = input.RevisionNo }, "");
            var list = queryFactory.GetList<TechnicalDrawings>(listQuery).ToList();

            if (list.Count > 0 && entity.RevisionNo != input.RevisionNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.TechnicalDrawings).Update(new UpdateTechnicalDrawingsDto
            {
                CustomerApproval = input.CustomerApproval,
                RevisionNo = input.RevisionNo,
                RevisionDate = input.RevisionDate,
                CustomerCurrentAccountCardID = input.CustomerCurrentAccountCardID.GetValueOrDefault(),
                ProductID = input.ProductID.GetValueOrDefault(),
                Drawer = input.Drawer,
                DrawingDomain = input.DrawingDomain,
                DrawingFilePath = input.DrawingFilePath,
                DrawingNo = input.DrawingNo,
                IsApproved = input.IsApproved,
                SampleApproval = input.SampleApproval,
                Description_ = input.Description_,
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id }, "");

            var technicalDrawings = queryFactory.Update<SelectTechnicalDrawingsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, technicalDrawings, LoginedUserService.UserId, Tables.TechnicalDrawings, LogType.Update, entity.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["TechnicalDrawingsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
                                RecordNumber = input.RevisionNo,
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
                            RecordNumber = input.RevisionNo,
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
            return new SuccessDataResult<SelectTechnicalDrawingsDto>(technicalDrawings);

        }

        public async Task<IDataResult<SelectTechnicalDrawingsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.TechnicalDrawings).Select("*").Where(new { Id = id }, "");
            var entity = queryFactory.Get<TechnicalDrawings>(entityQuery);

            var query = queryFactory.Query().From(Tables.TechnicalDrawings).Update(new UpdateTechnicalDrawingsDto
            {
                CustomerApproval = entity.CustomerApproval,
                RevisionNo = entity.RevisionNo,
                RevisionDate = entity.RevisionDate,
                CustomerCurrentAccountCardID = entity.CustomerCurrentAccountCardID,
                ProductID = entity.ProductID,
                Drawer = entity.Drawer,
                DrawingDomain = entity.DrawingDomain,
                DrawingFilePath = entity.DrawingFilePath,
                DrawingNo = entity.DrawingNo,
                IsApproved = entity.IsApproved,
                SampleApproval = entity.SampleApproval,
                Description_ = entity.Description_,
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

            var technicalDrawings = queryFactory.Update<SelectTechnicalDrawingsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectTechnicalDrawingsDto>(technicalDrawings);


        }
    }
}
