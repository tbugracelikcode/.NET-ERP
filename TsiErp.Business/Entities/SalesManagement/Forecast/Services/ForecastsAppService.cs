using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Forecast.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Period;
using TsiErp.Entities.Entities.SalesManagement.Forecast;
using TsiErp.Entities.Entities.SalesManagement.Forecast.Dtos;
using TsiErp.Entities.Entities.SalesManagement.ForecastLine;
using TsiErp.Entities.Entities.SalesManagement.ForecastLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Forecasts.Page;

namespace TsiErp.Business.Entities.Forecast.Services
{
    [ServiceRegistration(typeof(IForecastsAppService), DependencyInjectionType.Scoped)]
    public class ForecastsAppService : ApplicationService<ForecastsResource>, IForecastsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }

        public ForecastsAppService(IStringLocalizer<ForecastsResource> l, IFicheNumbersAppService ficheNumbersAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
        }

        [ValidationAspect(typeof(CreateForecastsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectForecastsDto>> CreateAsync(CreateForecastsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.Forecasts).Select("*").Where(new { Code = input.Code }, false, false, "");
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
                BranchID = input.BranchID,
                CreationDate_ = DateTime.Now,
                CurrentAccountCardID = input.CurrentAccountCardID,
                Description_ = input.Description_,
                LineNumber = input.LineNumber,
                PeriodID = input.PeriodID,
                ValidityEndDate = input.ValidityStartDate,
                ValidityStartDate = input.ValidityStartDate,
                Total = input.Total,
                Code = input.Code,
                CreationTime = DateTime.Now,
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
                    CreationTime = DateTime.Now,
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
                    ProductID = item.ProductID,
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var forecast = queryFactory.Insert<SelectForecastsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("ForecastsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Forecasts, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectForecastsDto>(forecast);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.Forecasts).Select("*").Where(new { Id = id }, false, false, "");

            var forecasts = queryFactory.Get<SelectForecastsDto>(query);

            if (forecasts.Id != Guid.Empty && forecasts != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.Forecasts).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.ForecastLines).Delete(LoginedUserService.UserId).Where(new { ForecastID = id }, false, false, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var forecast = queryFactory.Update<SelectForecastsDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Forecasts, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectForecastsDto>(forecast);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.ForecastLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
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
                   .Select<Forecasts>(f => new { f.ValidityStartDate, f.ValidityEndDate, f.Total, f.PeriodID, f.LineNumber, f.Id, f.Description_, f.DataOpenStatusUserId, f.DataOpenStatus, f.CurrentAccountCardID, f.Code, f.BranchID })
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
                    .Where(new { Id = id }, false, false, Tables.Forecasts);

            var forecasts = queryFactory.Get<SelectForecastsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.ForecastLines)
                   .Select<ForecastLines>(fl => new { fl.ProductID, fl.LineNr, fl.Id, fl.ForecastID, fl.DataOpenStatus, fl.DataOpenStatusUserId, fl.CustomerProductCode, fl.Amount })
                   .Join<Products>
                    (
                        pr => new { ProductID = pr.Id, ProductCode = pr.Code, ProductName = pr.Name },
                        nameof(ForecastLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Where(new { ForecastID = id }, false, false, Tables.ForecastLines);

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
                    .Where(null, false, false, Tables.Forecasts);

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
                    .Where(new { Id = input.Id }, false, false, Tables.Forecasts);

            var entity = queryFactory.Get<SelectForecastsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.ForecastLines)
                   .Select<ForecastLines>(fl => new { fl.ProductID, fl.LineNr, fl.Id, fl.ForecastID, fl.DataOpenStatus, fl.DataOpenStatusUserId, fl.CustomerProductCode, fl.Amount })
                  .Join<Products>
                    (
                        pr => new { ProductID = pr.Id, ProductCode = pr.Code, ProductName = pr.Name },
                        nameof(ForecastLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Where(new { ForecastID = input.Id }, false, false, Tables.ForecastLines);

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
                             .Where(new { Code = input.Code }, false, false, Tables.Forecasts);

            var list = queryFactory.GetList<ListForecastsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            var query = queryFactory.Query().From(Tables.Forecasts).Update(new UpdateForecastsDto
            {
                BranchID = input.BranchID,
                CreationDate_ = input.CreationDate_,
                CurrentAccountCardID = input.CurrentAccountCardID,
                Description_ = input.Description_,
                LineNumber = input.LineNumber,
                PeriodID = input.PeriodID,
                ValidityEndDate = input.ValidityStartDate,
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
                LastModificationTime = DateTime.Now,
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id }, false, false, "");

            foreach (var item in input.SelectForecastLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.ForecastLines).Insert(new CreateForecastLinesDto
                    {
                        Amount = item.Amount,
                        CustomerProductCode = item.CustomerProductCode,
                        ForecastID = input.Id,
                        CreationTime = DateTime.Now,
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
                        ProductID = item.ProductID,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.ForecastLines).Select("*").Where(new { Id = item.Id }, false, false, "");

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
                            LastModificationTime = DateTime.Now,
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                            ProductID = item.ProductID,
                        }).Where(new { Id = line.Id }, false, false, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var forecast = queryFactory.Update<SelectForecastsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.Forecasts, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectForecastsDto>(forecast);

        }

        public async Task<IDataResult<SelectForecastsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.Forecasts).Select("*").Where(new { Id = id }, false, false, "");

            var entity = queryFactory.Get<Forecasts>(entityQuery);

            var query = queryFactory.Query().From(Tables.Forecasts).Update(new UpdateForecastsDto
            {
                BranchID = entity.BranchID,
                CreationDate_ = entity.CreationDate_,
                CurrentAccountCardID = entity.CurrentAccountCardID,
                Description_ = entity.Description_,
                LineNumber = entity.LineNumber,
                PeriodID = entity.PeriodID,
                ValidityEndDate = entity.ValidityStartDate,
                ValidityStartDate = entity.ValidityStartDate,
                Total = entity.Total,
                Code = entity.Code,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = entity.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
            }).Where(new { Id = id }, false, false, "");

            var forecastsDto = queryFactory.Update<SelectForecastsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectForecastsDto>(forecastsDto);


        }
    }
}
