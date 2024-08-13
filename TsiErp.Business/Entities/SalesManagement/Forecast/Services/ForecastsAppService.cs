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
using TsiErp.Business.Entities.Forecast.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Period;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.SalesManagement.Forecast;
using TsiErp.Entities.Entities.SalesManagement.Forecast.Dtos;
using TsiErp.Entities.Entities.SalesManagement.ForecastLine;
using TsiErp.Entities.Entities.SalesManagement.ForecastLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Forecasts.Page;

namespace TsiErp.Business.Entities.Forecast.Services
{
    [ServiceRegistration(typeof(IForecastsAppService), DependencyInjectionType.Scoped)]
    public class ForecastsAppService : ApplicationService<ForecastsResource>, IForecastsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public ForecastsAppService(IStringLocalizer<ForecastsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }

        [ValidationAspect(typeof(CreateForecastsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectForecastsDto>> CreateAsync(CreateForecastsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.Forecasts).Select("Code").Where(new { Code = input.Code },  "");
            var list = queryFactory.ControlList<Forecasts>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.Forecasts).Insert(new CreateForecastsDto
            {
                BranchID = input.BranchID.GetValueOrDefault(),
                CreationDate_ = _GetSQLDateAppService.GetDateFromSQL(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Description_ = input.Description_,
                LineNumber = input.LineNumber,
                PeriodID = input.PeriodID.GetValueOrDefault(),
                ValidityEndDate = input.ValidityEndDate,
                ValidityStartDate = input.ValidityStartDate,
                Total = input.Total,
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
            });

            foreach (var item in input.SelectForecastLines)
            {
                var queryLine = queryFactory.Query().From(Tables.ForecastLines).Insert(new CreateForecastLinesDto
                {
                    Amount = item.Amount,
                    CustomerProductCode = item.CustomerProductCode,
                    ForecastID = addedEntityId,
                    CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = GuidGenerator.CreateGuid(),
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    LineNr = item.LineNr,
                    ProductID = item.ProductID.GetValueOrDefault(),
                    StartDate = item.StartDate,
                    EndDate = item.EndDate
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var forecast = queryFactory.Insert<SelectForecastsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("ForecastsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Forecasts, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ForecastsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectForecastsDto>(forecast);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.Forecasts).Select("*").Where(new { Id = id }, "");

            var forecasts = queryFactory.Get<SelectForecastsDto>(query);

            if (forecasts.Id != Guid.Empty && forecasts != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.Forecasts).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.ForecastLines).Delete(LoginedUserService.UserId).Where(new { ForecastID = id }, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var forecast = queryFactory.Update<SelectForecastsDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Forecasts, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ForecastsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                return new SuccessDataResult<SelectForecastsDto>(forecast);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.ForecastLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");
                var forecastLines = queryFactory.Update<SelectForecastLinesDto>(queryLine, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ForecastLines, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectForecastLinesDto>(forecastLines);
            }

        }

        public async Task<IDataResult<SelectForecastsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.Forecasts)
                   .Select<Forecasts>(null)
                   .Join<Periods>
                    (
                        p => new { PeriodID = p.Id, PeriodCode = p.Code, PeriodName = p.Name },
                        nameof(Forecasts.PeriodID),
                        nameof(Periods.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(Forecasts.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(Forecasts.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id },  Tables.Forecasts);

            var forecasts = queryFactory.Get<SelectForecastsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.ForecastLines)
                   .Select<ForecastLines>(null)
                   .Join<Products>
                    (
                        pr => new { ProductID = pr.Id, ProductCode = pr.Code, ProductName = pr.Name },
                        nameof(ForecastLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Where(new { ForecastID = id },  Tables.ForecastLines);

            var forecastLine = queryFactory.GetList<SelectForecastLinesDto>(queryLines).ToList();

            forecasts.SelectForecastLines = forecastLine;

            LogsAppService.InsertLogToDatabase(forecasts, forecasts, LoginedUserService.UserId, Tables.Forecasts, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectForecastsDto>(forecasts);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListForecastsDto>>> GetListAsync(ListForecastsParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.Forecasts)
                   .Select<Forecasts>(s => new { s.Code, s.ValidityEndDate, s.ValidityStartDate, s.Description_, s.Total })
                    .Join<Periods>
                    (
                        p => new { PeriodCode = p.Code, PeriodName = p.Name },
                        nameof(Forecasts.PeriodID),
                        nameof(Periods.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchName = b.Name },
                        nameof(Forecasts.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(Forecasts.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(null, Tables.Forecasts);

            var forecasts = queryFactory.GetList<ListForecastsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListForecastsDto>>(forecasts);

        }

        [ValidationAspect(typeof(UpdateForecastsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectForecastsDto>> UpdateAsync(UpdateForecastsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.Forecasts)
                   .Select<Forecasts>(null)
                   .Join<Periods>
                    (
                        p => new { PeriodID = p.Id, PeriodCode = p.Code, PeriodName = p.Name },
                        nameof(Forecasts.PeriodID),
                        nameof(Periods.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(Forecasts.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(Forecasts.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id },  Tables.Forecasts);

            var entity = queryFactory.Get<SelectForecastsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.ForecastLines)
                   .Select<ForecastLines>(null)
                  .Join<Products>
                    (
                        pr => new { ProductID = pr.Id, ProductCode = pr.Code, ProductName = pr.Name },
                        nameof(ForecastLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Where(new { ForecastID = input.Id }, Tables.ForecastLines);

            var forecastLine = queryFactory.GetList<SelectForecastLinesDto>(queryLines).ToList();

            entity.SelectForecastLines = forecastLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.Forecasts)
                           .Select<Forecasts>(f => new { f.ValidityStartDate, f.ValidityEndDate, f.Total, f.PeriodID, f.LineNumber, f.Id, f.Description_, f.DataOpenStatusUserId, f.DataOpenStatus, f.CurrentAccountCardID, f.Code, f.BranchID })
                           .Join<Periods>
                             (
                                 p => new { PeriodCode = p.Code, PeriodName = p.Name },
                                 nameof(Forecasts.PeriodID),
                                 nameof(Periods.Id),
                                 JoinType.Left
                             )
                             .Join<Branches>
                             (
                                 b => new { BranchCode = b.Code, BranchName = b.Name },
                                 nameof(Forecasts.BranchID),
                                 nameof(Branches.Id),
                                 JoinType.Left
                             )
                              .Join<CurrentAccountCards>
                             (
                                 ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                                 nameof(Forecasts.CurrentAccountCardID),
                                 nameof(CurrentAccountCards.Id),
                                 JoinType.Left
                             )
                             .Where(new { Code = input.Code },  Tables.Forecasts);

            var list = queryFactory.GetList<ListForecastsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            var query = queryFactory.Query().From(Tables.Forecasts).Update(new UpdateForecastsDto
            {
                BranchID = input.BranchID.GetValueOrDefault(),
                CreationDate_ = entity.CreationDate_,
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Description_ = input.Description_,
                LineNumber = input.LineNumber,
                PeriodID = input.PeriodID.GetValueOrDefault(),
                ValidityEndDate = input.ValidityEndDate,
                ValidityStartDate = input.ValidityStartDate,
                Total = input.Total,
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id },  "");

            foreach (var item in input.SelectForecastLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.ForecastLines).Insert(new CreateForecastLinesDto
                    {
                        Amount = item.Amount,
                        CustomerProductCode = item.CustomerProductCode,
                        ForecastID = input.Id,
                        CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        Id = GuidGenerator.CreateGuid(),
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                        LineNr = item.LineNr,
                        ProductID = item.ProductID.GetValueOrDefault(),
                        EndDate = item.EndDate,
                        StartDate = item.StartDate
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.ForecastLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectForecastLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.ForecastLines).Update(new UpdateForecastLinesDto
                        {
                            Amount = item.Amount,
                            CustomerProductCode = item.CustomerProductCode,
                            ForecastID = input.Id,
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                            ProductID = item.ProductID.GetValueOrDefault(),
                            EndDate = item.EndDate,
                            StartDate = item.StartDate
                        }).Where(new { Id = line.Id },  "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var forecast = queryFactory.Update<SelectForecastsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.Forecasts, LogType.Update, entity.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ForecastsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectForecastsDto>(forecast);

        }

        public async Task<IDataResult<SelectForecastsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.Forecasts).Select("Id").Where(new { Id = id }, "");

            var entity = queryFactory.Get<Forecasts>(entityQuery);

            var query = queryFactory.Query().From(Tables.Forecasts).Update(new UpdateForecastsDto
            {
                BranchID = entity.BranchID,
                CreationDate_ = entity.CreationDate_,
                CurrentAccountCardID = entity.CurrentAccountCardID,
                Description_ = entity.Description_,
                LineNumber = entity.LineNumber,
                PeriodID = entity.PeriodID,
                ValidityEndDate = entity.ValidityEndDate,
                ValidityStartDate = entity.ValidityStartDate,
                Total = entity.Total,
                Code = entity.Code,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.GetValueOrDefault(),
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = entity.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault()
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id },  "");

            var forecastsDto = queryFactory.Update<SelectForecastsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectForecastsDto>(forecastsDto);


        }
    }
}
