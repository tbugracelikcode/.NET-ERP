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
using TsiErp.Business.Entities.ProductionManagement.ContractTrackingFiche.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFiche;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFiche.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheLine;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlan;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ContractTrackingFiches.Page;

namespace TsiErp.Business.Entities.ContractTrackingFiche.Services
{
    [ServiceRegistration(typeof(IContractTrackingFichesAppService), DependencyInjectionType.Scoped)]

    public class ContractTrackingFichesAppService : ApplicationService<ContractTrackingFichesResource>, IContractTrackingFichesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }

        public ContractTrackingFichesAppService(IStringLocalizer<ContractTrackingFichesResource> l, IFicheNumbersAppService ficheNumbersAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
        }

        [ValidationAspect(typeof(CreateContractTrackingFichesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectContractTrackingFichesDto>> CreateAsync(CreateContractTrackingFichesDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.ContractTrackingFiches).Select("*").Where(new { FicheNr = input.FicheNr }, false, false, "");
                var list = queryFactory.ControlList<ContractTrackingFiches>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();

                var query = queryFactory.Query().From(Tables.ContractTrackingFiches).Insert(new CreateContractTrackingFichesDto
                {
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = addedEntityId,
                    Amount_ = input.Amount_,
                    FicheNr = input.FicheNr,
                    ContractQualityPlanID = input.ContractQualityPlanID,
                    CurrentAccountCardID = input.CurrentAccountCardID,
                    ProductionOrderID = input.ProductionOrderID,
                    Description_ = input.Description_,
                    EstimatedDate_ = input.EstimatedDate_,
                    FicheDate_ = input.FicheDate_,
                    OccuredAmount_ = input.OccuredAmount_,
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty
                });

                foreach (var item in input.SelectContractTrackingFicheLines)
                {
                    var queryLine = queryFactory.Query().From(Tables.ContractTrackingFicheLines).Insert(new CreateContractTrackingFicheLinesDto
                    {
                        StationID = item.StationID,
                        ContractTrackingFicheID = addedEntityId,
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
                        IsSent = item.IsSent,
                        OperationID = item.OperationID,
                        WorkOrderID = item.WorkOrderID,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }

                var ContractTrackingFiche = queryFactory.Insert<SelectContractTrackingFichesDto>(query, "Id", true);

                await FicheNumbersAppService.UpdateFicheNumberAsync("ContractTrackingFichesChildMenu", input.FicheNr);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ContractTrackingFiches, LogType.Insert, addedEntityId);

                return new SuccessDataResult<SelectContractTrackingFichesDto>(ContractTrackingFiche);
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.ContractTrackingFiches).Select("*").Where(new { Id = id }, false, false, "");

                var ContractTrackingFiches = queryFactory.Get<SelectContractTrackingFichesDto>(query);

                if (ContractTrackingFiches.Id != Guid.Empty && ContractTrackingFiches != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.ContractTrackingFiches).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.ContractTrackingFicheLines).Delete(LoginedUserService.UserId).Where(new { ContractTrackingFicheID = id }, false, false, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var ContractTrackingFiche = queryFactory.Update<SelectContractTrackingFichesDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ContractTrackingFiches, LogType.Delete, id);
                    return new SuccessDataResult<SelectContractTrackingFichesDto>(ContractTrackingFiche);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.ContractTrackingFicheLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                    var ContractTrackingFicheLines = queryFactory.Update<SelectContractTrackingFicheLinesDto>(queryLine, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ContractTrackingFicheLines, LogType.Delete, id);
                    return new SuccessDataResult<SelectContractTrackingFicheLinesDto>(ContractTrackingFicheLines);
                }
            }
        }

        public async Task<IDataResult<SelectContractTrackingFichesDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                       .From(Tables.ContractTrackingFiches)
                       .Select("*")
                       .Join<ProductionOrders>
                        (
                            p => new { ProductionOrderID = p.Id, ProductionOrderNr = p.FicheNo },
                            nameof(ContractTrackingFiches.ProductionOrderID),
                            nameof(ProductionOrders.Id),
                            JoinType.Left
                        )
                        .Join<CurrentAccountCards>
                        (
                            to => new { CurrentAccountCardID = to.Id, CurrentAccountCardCode = to.Code, CurrentAccountCardName = to.Name, CustomerCode = to.CustomerCode },
                            nameof(ContractTrackingFiches.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Join<ContractQualityPlans>
                        (
                            to => new { ContractQualityPlanID = to.Id, ContractQualityPlanDocumentNumber = to.DocumentNumber, ContractQualityPlanDescription = to.Description_ },
                            nameof(ContractTrackingFiches.ContractQualityPlanID),
                            nameof(ContractQualityPlans.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, false, false, Tables.ContractTrackingFiches);

                var contractTrackingFiches = queryFactory.Get<SelectContractTrackingFichesDto>(query);

                #region Contract Tracking Fiche Lines
                var queryLines = queryFactory
                               .Query()
                               .From(Tables.ContractTrackingFicheLines)
                               .Select("*")
                               .Join<ProductsOperations>
                                (
                                    s => new { OperationID = s.Id, OperationCode = s.Code, OperationName = s.Name },
                                    nameof(ContractTrackingFicheLines.OperationID),
                                    nameof(ProductsOperations.Id),
                                    JoinType.Left
                                )
                                .Join<Stations>
                                (
                                    s => new { StationID = s.Id, StationCode = s.Code, StationName = s.Name },
                                    nameof(ContractTrackingFicheLines.StationID),
                                    nameof(Stations.Id),
                                    JoinType.Left
                                )
                                 .Join<WorkOrders>
                                (
                                    s => new { WorkOrderID = s.Id, WorkOrderNr = s.Code },
                                    nameof(ContractTrackingFicheLines.WorkOrderID),
                                    nameof(WorkOrders.Id),
                                    JoinType.Left
                                )
                                .Where(new { ContractTrackingFicheID = id }, false, false, Tables.ContractTrackingFicheLines);

                var ContractTrackingFicheLine = queryFactory.GetList<SelectContractTrackingFicheLinesDto>(queryLines).ToList();

                contractTrackingFiches.SelectContractTrackingFicheLines = ContractTrackingFicheLine;
                #endregion

                LogsAppService.InsertLogToDatabase(contractTrackingFiches, contractTrackingFiches, LoginedUserService.UserId, Tables.ContractTrackingFiches, LogType.Get, id);

                return new SuccessDataResult<SelectContractTrackingFichesDto>(contractTrackingFiches);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListContractTrackingFichesDto>>> GetListAsync(ListContractTrackingFichesParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                       .From(Tables.ContractTrackingFiches)
                       .Select("*")
                      .Join<ProductionOrders>
                        (
                            p => new { ProductionOrderNr = p.FicheNo },
                            nameof(ContractTrackingFiches.ProductionOrderID),
                            nameof(ProductionOrders.Id),
                            JoinType.Left
                        )
                        .Join<CurrentAccountCards>
                        (
                            to => new { CurrentAccountCardCode = to.Code, CurrentAccountCardName = to.Name, CustomerCode = to.CustomerCode },
                            nameof(ContractTrackingFiches.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Join<ContractQualityPlans>
                        (
                            to => new { ContractQualityPlanDocumentNumber = to.DocumentNumber, ContractQualityPlanDescription = to.Description_ },
                            nameof(ContractTrackingFiches.ContractQualityPlanID),
                            nameof(ContractQualityPlans.Id),
                            JoinType.Left)
                        .Where(null, false, false, Tables.ContractTrackingFiches);

                var contractTrackingFiches = queryFactory.GetList<ListContractTrackingFichesDto>(query).ToList();
                return new SuccessDataResult<IList<ListContractTrackingFichesDto>>(contractTrackingFiches);
            }
        }

        [ValidationAspect(typeof(UpdateContractTrackingFichesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectContractTrackingFichesDto>> UpdateAsync(UpdateContractTrackingFichesDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory
                       .Query()
                        .From(Tables.ContractTrackingFiches)
                       .Select("*")
                           .Join<ProductionOrders>
                        (
                            p => new { ProductionOrderID = p.Id, ProductionOrderNr = p.FicheNo },
                            nameof(ContractTrackingFiches.ProductionOrderID),
                            nameof(ProductionOrders.Id),
                            JoinType.Left
                        )
                        .Join<CurrentAccountCards>
                        (
                            to => new { CurrentAccountCardID = to.Id, CurrentAccountCardCode = to.Code, CurrentAccountCardName = to.Name, CustomerCode = to.CustomerCode },
                            nameof(ContractTrackingFiches.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Join<ContractQualityPlans>
                        (
                            to => new { ContractQualityPlanID = to.Id, ContractQualityPlanDocumentNumber = to.DocumentNumber, ContractQualityPlanDescription = to.Description_ },
                            nameof(ContractTrackingFiches.ContractQualityPlanID),
                            nameof(ContractQualityPlans.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = input.Id }, false, false, Tables.ContractTrackingFiches);

                var entity = queryFactory.Get<SelectContractTrackingFichesDto>(entityQuery);

                #region Contract Tracking Fiche Lines
                var queryLines = queryFactory
                               .Query()
                               .From(Tables.ContractTrackingFicheLines)
                                .Select("*")
                               .Join<ProductsOperations>
                                (
                                    s => new { OperationID = s.Id, OperationCode = s.Code, OperationName = s.Name },
                                    nameof(ContractTrackingFicheLines.OperationID),
                                    nameof(ProductsOperations.Id),
                                    JoinType.Left
                                )
                                .Join<Stations>
                                (
                                    s => new { StationID = s.Id, StationCode = s.Code, StationName = s.Name },
                                    nameof(ContractTrackingFicheLines.StationID),
                                    nameof(Stations.Id),
                                    JoinType.Left
                                )
                                 .Join<WorkOrders>
                                (
                                    s => new { WorkOrderID = s.Id, WorkOrderNr = s.Code },
                                    nameof(ContractTrackingFicheLines.WorkOrderID),
                                    nameof(WorkOrders.Id),
                                    JoinType.Left
                                )
                                .Where(new { ContractTrackingFicheID = input.Id }, false, false, Tables.ContractTrackingFicheLines);

                var ContractTrackingFicheLine = queryFactory.GetList<SelectContractTrackingFicheLinesDto>(queryLines).ToList();

                entity.SelectContractTrackingFicheLines = ContractTrackingFicheLine;
                #endregion

                #region Update Control
                var listQuery = queryFactory
                               .Query()
                                .From(Tables.ContractTrackingFiches)
                         .Select("*")
                      .Join<ProductionOrders>
                        (
                            p => new { ProductionOrderNr = p.FicheNo },
                            nameof(ContractTrackingFiches.ProductionOrderID),
                            nameof(ProductionOrders.Id),
                            JoinType.Left
                        )
                        .Join<CurrentAccountCards>
                        (
                            to => new { CurrentAccountCardCode = to.Code, CurrentAccountCardName = to.Name, CustomerCode = to.CustomerCode },
                            nameof(ContractTrackingFiches.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Join<ContractQualityPlans>
                        (
                            to => new { ContractQualityPlanDocumentNumber = to.DocumentNumber, ContractQualityPlanDescription = to.Description_ },
                            nameof(ContractTrackingFiches.ContractQualityPlanID),
                            nameof(ContractQualityPlans.Id),
                            JoinType.Left)
                                .Where(new { FicheNr = input.FicheNr }, false, false, Tables.ContractTrackingFiches);

                var list = queryFactory.GetList<ListContractTrackingFichesDto>(listQuery).ToList();

                if (list.Count > 0 && entity.FicheNr != input.FicheNr)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }
                #endregion

                var query = queryFactory.Query().From(Tables.ContractTrackingFiches).Update(new UpdateContractTrackingFichesDto
                {
                    Amount_ = input.Amount_,
                    FicheNr = input.FicheNr,
                    ContractQualityPlanID = input.ContractQualityPlanID,
                    CurrentAccountCardID = input.CurrentAccountCardID,
                    Description_ = input.Description_,
                    EstimatedDate_ = input.EstimatedDate_,
                    FicheDate_ = input.FicheDate_,
                    OccuredAmount_ = input.OccuredAmount_,
                    ProductionOrderID = input.ProductionOrderID,
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
                }).Where(new { Id = input.Id }, true, true, "");

                #region Contract Tracking Fiche Lines
                foreach (var item in input.SelectContractTrackingFicheLines)
                {
                    if (item.Id == Guid.Empty)
                    {
                        var queryLine = queryFactory.Query().From(Tables.ContractTrackingFicheLines).Insert(new CreateContractTrackingFicheLinesDto
                        {
                            IsSent = item.IsSent,
                            OperationID = item.OperationID,
                            WorkOrderID = item.WorkOrderID,
                            StationID = item.StationID,
                            ContractTrackingFicheID = input.Id,
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
                        });

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                    }
                    else
                    {
                        var lineGetQuery = queryFactory.Query().From(Tables.ContractTrackingFicheLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                        var line = queryFactory.Get<SelectContractTrackingFicheLinesDto>(lineGetQuery);

                        if (line != null)
                        {
                            var queryLine = queryFactory.Query().From(Tables.ContractTrackingFicheLines).Update(new UpdateContractTrackingFicheLinesDto
                            {
                                IsSent = item.IsSent,
                                OperationID = item.OperationID,
                                WorkOrderID = item.WorkOrderID,
                                StationID = item.StationID,
                                ContractTrackingFicheID = input.Id,
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
                            }).Where(new { Id = line.Id }, false, false, "");

                            query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                        }
                    }
                }
                #endregion


                var ContractTrackingFiche = queryFactory.Update<SelectContractTrackingFichesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.ContractTrackingFiches, LogType.Update, entity.Id);

                return new SuccessDataResult<SelectContractTrackingFichesDto>(ContractTrackingFiche);
            }
        }

        public async Task<IDataResult<SelectContractTrackingFichesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.ContractTrackingFiches).Select("*").Where(new { Id = id }, true, true, "");

                var entity = queryFactory.Get<ContractTrackingFiches>(entityQuery);

                var query = queryFactory.Query().From(Tables.ContractTrackingFiches).Update(new UpdateContractTrackingFichesDto
                {
                    Amount_ = entity.Amount_,
                    FicheNr = entity.FicheNr,
                    ContractQualityPlanID = entity.ContractQualityPlanID,
                    CurrentAccountCardID = entity.CurrentAccountCardID,
                    Description_ = entity.Description_,
                    EstimatedDate_ = entity.EstimatedDate_,
                    FicheDate_ = entity.FicheDate_,
                    OccuredAmount_ = entity.OccuredAmount_,
                    ProductionOrderID = entity.ProductionOrderID,
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
                }).Where(new { Id = id }, true, true, "");

                var ContractTrackingFichesDto = queryFactory.Update<SelectContractTrackingFichesDto>(query, "Id", true);
                return new SuccessDataResult<SelectContractTrackingFichesDto>(ContractTrackingFichesDto);

            }
        }
    }
}
