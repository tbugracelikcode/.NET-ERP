using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.PlanningManagement.MRP.Services;
using TsiErp.Business.Entities.PlanningManagement.MRP.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.PlanningManagement.MRP;
using TsiErp.Entities.Entities.PlanningManagement.MRP.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRPLine;
using TsiErp.Entities.Entities.PlanningManagement.MRPLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.MRPs.Page;

namespace TsiErp.Business.Entities.MRP.Services
{
    [ServiceRegistration(typeof(IMRPsAppService), DependencyInjectionType.Scoped)]
    public class MRPsAppService : ApplicationService<MRPsResource>, IMRPsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }

        public MRPsAppService(IStringLocalizer<MRPsResource> l, IFicheNumbersAppService ficheNumbersAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
        }

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        [ValidationAspect(typeof(CreateMRPsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMRPsDto>> CreateAsync(CreateMRPsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.MRPs).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.ControlList<MRPs>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();

                var query = queryFactory.Query().From(Tables.MRPs).Insert(new CreateMRPsDto
                {
                    Date_ = input.Date_,
                    Description_ = input.Description_,
                    State_ = input.State_,
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

                foreach (var item in input.SelectMRPLines)
                {
                    var queryLine = queryFactory.Query().From(Tables.MRPLines).Insert(new CreateMRPLinesDto
                    {
                        State_ = item.State_,
                        LineNr = item.LineNr,
                        Amount = item.Amount,
                        ProductID = item.ProductID,
                        RequirementAmount = item.RequirementAmount,
                        SalesOrderID = item.SalesOrderID,
                        SalesOrderLineID = item.SalesOrderLineID,
                        UnitSetID = item.UnitSetID,
                        MRPID = addedEntityId,
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
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }

                var MRP = queryFactory.Insert<SelectMRPsDto>(query, "Id", true);

                await FicheNumbersAppService.UpdateFicheNumberAsync("MRPChildMenu", input.Code);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.MRPs, LogType.Insert, addedEntityId);

                return new SuccessDataResult<SelectMRPsDto>(MRP);
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.MRPs).Select("*").Where(new { Id = id }, false, false, "");

                var MRPs = queryFactory.Get<SelectMRPsDto>(query);

                if (MRPs.Id != Guid.Empty && MRPs != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.MRPs).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.MRPLines).Delete(LoginedUserService.UserId).Where(new { BomID = id }, false, false, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var MRP = queryFactory.Update<SelectMRPsDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.MRPs, LogType.Delete, id);
                    return new SuccessDataResult<SelectMRPsDto>(MRP);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.MRPLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                    var MRPLines = queryFactory.Update<SelectMRPLinesDto>(queryLine, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.MRPLines, LogType.Delete, id);
                    return new SuccessDataResult<SelectMRPLinesDto>(MRPLines);
                }
            }
        }

        public async Task<IDataResult<SelectMRPsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.MRPs).Select("*").Where(
               new
               {
                   Id = id
               }, false, false, "");
                var MRP = queryFactory.Get<SelectMRPsDto>(query);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.MRPLines)
                       .Select<MRPLines>(null)
                       .Join<Products>
                        (
                            s => new { ProductName = s.Name, ProductID = s.Id, ProductCode = s.Code },
                            nameof(MRPLines.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                       .Join<UnitSets>
                        (
                            sh => new { UnitSetID = sh.Id, UnitSetCode = sh.Code },
                            nameof(MRPLines.UnitSetID),
                            nameof(UnitSets.Id),
                            JoinType.Left
                        )
                         .Join<SalesOrders>
                        (
                            sh => new { SalesOrderID = sh.Id, SalesOrderFicheNo = sh.FicheNo },
                            nameof(MRPLines.SalesOrderID),
                            nameof(SalesOrders.Id),
                            JoinType.Left
                        )
                          .Join<SalesOrderLines>
                        (
                            sh => new { SalesOrderLineID = sh.Id },
                            nameof(MRPLines.SalesOrderLineID),
                            nameof(SalesOrderLines.Id),
                            JoinType.Left
                        )
                        .Where(new { MRPID = id }, false, false, Tables.MRPLines);

                var MRPLine = queryFactory.GetList<SelectMRPLinesDto>(queryLines).ToList();

                MRP.SelectMRPLines = MRPLine;

                LogsAppService.InsertLogToDatabase(MRP, MRP, LoginedUserService.UserId, Tables.MRPs, LogType.Get, id);

                return new SuccessDataResult<SelectMRPsDto>(MRP);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListMRPsDto>>> GetListAsync(ListMRPsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.MRPs).Select("*").Where(null, false, false, "");
                var MRPs = queryFactory.GetList<ListMRPsDto>(query).ToList();
                return new SuccessDataResult<IList<ListMRPsDto>>(MRPs);


            }
        }

        [ValidationAspect(typeof(UpdateMRPsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMRPsDto>> UpdateAsync(UpdateMRPsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.MRPs).Select("*").Where(
              new
              {
                  Id = input.Id
              }, false, false, "");
                var entity = queryFactory.Get<SelectMRPsDto>(entityQuery);

                var queryLines = queryFactory
                     .Query()
                     .From(Tables.MRPLines)
                     .Select<MRPLines>(null)
                     .Join<Products>
                      (
                          s => new { ProductName = s.Name, ProductID = s.Id, ProductCode = s.Code },
                          nameof(MRPLines.ProductID),
                          nameof(Products.Id),
                          JoinType.Left
                      )
                     .Join<UnitSets>
                      (
                          sh => new { UnitSetID = sh.Id, UnitSetCode = sh.Code },
                          nameof(MRPLines.UnitSetID),
                          nameof(UnitSets.Id),
                          JoinType.Left
                      )
                       .Join<SalesOrders>
                      (
                          sh => new { SalesOrderID = sh.Id, SalesOrderFicheNo = sh.FicheNo },
                          nameof(MRPLines.SalesOrderID),
                          nameof(SalesOrders.Id),
                          JoinType.Left
                      )
                        .Join<SalesOrderLines>
                      (
                          sh => new { SalesOrderLineID = sh.Id },
                          nameof(MRPLines.SalesOrderLineID),
                          nameof(SalesOrderLines.Id),
                          JoinType.Left
                      )
                      .Where(new { MRPID = input.Id }, false, false, Tables.MRPLines);

                var MRPLine = queryFactory.GetList<SelectMRPLinesDto>(queryLines).ToList();

                entity.SelectMRPLines = MRPLine;

                #region Update Control
                var listQuery = queryFactory
                               .Query()
                               .From(Tables.MRPs).Select("*").Where(new { Code = input.Code }, false, false, Tables.MRPs);

                var list = queryFactory.GetList<ListMRPsDto>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }
                #endregion

                var query = queryFactory.Query().From(Tables.MRPs).Update(new UpdateMRPsDto
                {
                    Date_ = input.Date_,
                    Description_ = input.Description_,
                    State_ = input.State_,
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

                foreach (var item in input.SelectMRPLines)
                {
                    if (item.Id == Guid.Empty)
                    {
                        var queryLine = queryFactory.Query().From(Tables.MRPLines).Insert(new CreateMRPLinesDto
                        {
                            State_ = item.State_,
                            LineNr = item.LineNr,
                            Amount = item.Amount,
                            ProductID = item.ProductID,
                            RequirementAmount = item.RequirementAmount,
                            SalesOrderID = item.SalesOrderID,
                            SalesOrderLineID = item.SalesOrderLineID,
                            UnitSetID = item.UnitSetID,
                            MRPID = input.Id,
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
                        });

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                    }
                    else
                    {
                        var lineGetQuery = queryFactory.Query().From(Tables.MRPLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                        var line = queryFactory.Get<SelectMRPLinesDto>(lineGetQuery);

                        if (line != null)
                        {
                            var queryLine = queryFactory.Query().From(Tables.MRPLines).Update(new UpdateMRPLinesDto
                            {
                                State_ = item.State_,
                                LineNr = item.LineNr,
                                Amount = item.Amount,
                                ProductID = item.ProductID,
                                RequirementAmount = item.RequirementAmount,
                                SalesOrderID = item.SalesOrderID,
                                SalesOrderLineID = item.SalesOrderLineID,
                                UnitSetID = item.UnitSetID,
                                MRPID = input.Id,
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
                            }).Where(new { Id = line.Id }, false, false, "");

                            query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                        }
                    }
                }

                var MRP = queryFactory.Update<SelectMRPsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.MRPs, LogType.Update, input.Id);

                return new SuccessDataResult<SelectMRPsDto>(MRP);
            }
        }

        public async Task<IDataResult<SelectMRPsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.MRPs).Select("*").Where(new { Id = id }, false, false, "");

                var entity = queryFactory.Get<MRPs>(entityQuery);

                var query = queryFactory.Query().From(Tables.MRPs).Update(new UpdateMRPsDto
                {
                    State_ = entity.State_,
                    Date_ = entity.Date_,
                    Description_ = entity.Description_,
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

                var MRPsDto = queryFactory.Update<SelectMRPsDto>(query, "Id", true);
                return new SuccessDataResult<SelectMRPsDto>(MRPsDto);

            }
        }
    }
}
