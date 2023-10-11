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
using TsiErp.Business.Entities.UnplannedMaintenance.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenancePeriod;
using TsiErp.Entities.Entities.MaintenanceManagement.UnplannedMaintenance;
using TsiErp.Entities.Entities.MaintenanceManagement.UnplannedMaintenance.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.UnplannedMaintenanceLine;
using TsiErp.Entities.Entities.MaintenanceManagement.UnplannedMaintenanceLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.UnplannedMaintenances.Page;

namespace TsiErp.Business.Entities.UnplannedMaintenance.Services
{
    [ServiceRegistration(typeof(IUnplannedMaintenancesAppService), DependencyInjectionType.Scoped)]
    public class UnplannedMaintenancesAppService : ApplicationService<UnplannedMaintenancesResource>, IUnplannedMaintenancesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }

        public UnplannedMaintenancesAppService(IStringLocalizer<UnplannedMaintenancesResource> l, IFicheNumbersAppService ficheNumbersAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
        }

        [ValidationAspect(typeof(CreateUnplannedMaintenanceValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnplannedMaintenancesDto>> CreateAsync(CreateUnplannedMaintenancesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.UnplannedMaintenances).Select("*").Where(new { RegistrationNo = input.RegistrationNo }, false, false, "");
            var list = queryFactory.ControlList<UnplannedMaintenances>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.UnplannedMaintenances).Insert(new CreateUnplannedMaintenancesDto
            {
                RegistrationNo = input.RegistrationNo,
                StationID = input.StationID,
                CompletionDate = input.CompletionDate,
                Caregiver = input.Caregiver,
                Note_ = input.Note_,
                NumberofCaregivers = input.NumberofCaregivers,
                OccuredTime = input.OccuredTime,
                PeriodID = input.PeriodID,
                PeriodTime = input.PeriodTime,
                UnplannedDate = input.UnplannedDate,
                UnplannedTime = input.UnplannedTime,
                RemainingTime = input.RemainingTime,
                StartDate = input.StartDate,
                Status = input.Status,
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

            foreach (var item in input.SelectUnplannedMaintenanceLines)
            {
                var queryLine = queryFactory.Query().From(Tables.UnplannedMaintenanceLines).Insert(new CreateUnplannedMaintenanceLinesDto
                {
                    Amount = item.Amount,
                    InstructionDescription = item.InstructionDescription,
                    MaintenanceNote = item.MaintenanceNote,
                    UnplannedMaintenanceID = addedEntityId,
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
                    UnitSetID = item.UnitSetID,
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var maintenance = queryFactory.Insert<SelectUnplannedMaintenancesDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("UnplannedMainChildMenu", input.RegistrationNo);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.UnplannedMaintenances, LogType.Insert, maintenance.Id);

            return new SuccessDataResult<SelectUnplannedMaintenancesDto>(maintenance);
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.UnplannedMaintenances).Select("*").Where(new { Id = id }, false, false, "");

            var maintenances = queryFactory.Get<SelectUnplannedMaintenancesDto>(query);

            if (maintenances.Id != Guid.Empty && maintenances != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.UnplannedMaintenances).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.UnplannedMaintenanceLines).Delete(LoginedUserService.UserId).Where(new { UnplannedMaintenanceID = id }, false, false, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var maintenance = queryFactory.Update<SelectUnplannedMaintenancesDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.UnplannedMaintenances, LogType.Delete, id);
                return new SuccessDataResult<SelectUnplannedMaintenancesDto>(maintenance);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.UnplannedMaintenanceLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                var maintenanceLines = queryFactory.Update<SelectUnplannedMaintenanceLinesDto>(queryLine, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.UnplannedMaintenanceLines, LogType.Delete, id);
                return new SuccessDataResult<SelectUnplannedMaintenanceLinesDto>(maintenanceLines);
            }
        }

        public async Task<IDataResult<SelectUnplannedMaintenancesDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.UnplannedMaintenances)
                   .Select<UnplannedMaintenances>(pm => new { pm.Status, pm.StationID, pm.StartDate, pm.RemainingTime, pm.RegistrationNo, pm.UnplannedDate, pm.UnplannedTime, pm.PeriodTime, pm.PeriodID, pm.OccuredTime, pm.NumberofCaregivers, pm.Note_, pm.Id, pm.DataOpenStatusUserId, pm.DataOpenStatus, pm.CompletionDate, pm.Caregiver })
                   .Join<Stations>
                    (
                        s => new { StationID = s.Id, StationCode = s.Code },
                        nameof(UnplannedMaintenances.StationID),
                        nameof(Stations.Id),
                        JoinType.Left
                    )
                    .Join<MaintenancePeriods>
                    (
                        mp => new { PeriodID = mp.Id, PeriodName = mp.Name },
                        nameof(UnplannedMaintenances.PeriodID),
                        nameof(MaintenancePeriods.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, false, false, Tables.UnplannedMaintenances);

            var maintenances = queryFactory.Get<SelectUnplannedMaintenancesDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.UnplannedMaintenanceLines)
                   .Select<UnplannedMaintenanceLines>(pml => new { pml.UnitSetID, pml.ProductID, pml.UnplannedMaintenanceID, pml.MaintenanceNote, pml.LineNr, pml.InstructionDescription, pml.Id, pml.DataOpenStatusUserId, pml.DataOpenStatus, pml.Amount })
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(UnplannedMaintenanceLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(UnplannedMaintenanceLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                    .Where(new { UnplannedMaintenanceID = id }, false, false, Tables.UnplannedMaintenanceLines);

            var maintenanceLine = queryFactory.GetList<SelectUnplannedMaintenanceLinesDto>(queryLines).ToList();

            maintenances.SelectUnplannedMaintenanceLines = maintenanceLine;

            LogsAppService.InsertLogToDatabase(maintenances, maintenances, LoginedUserService.UserId, Tables.UnplannedMaintenances, LogType.Get, id);

            return new SuccessDataResult<SelectUnplannedMaintenancesDto>(maintenances);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListUnplannedMaintenancesDto>>> GetListAsync(ListUnplannedMaintenancesParameterDto input)
        {
            var query = queryFactory
                   .Query()
                 .From(Tables.UnplannedMaintenances)
                   .Select<UnplannedMaintenances>(pm => new { pm.Status, pm.StationID, pm.StartDate, pm.RemainingTime, pm.RegistrationNo, pm.UnplannedDate, pm.UnplannedTime, pm.PeriodTime, pm.PeriodID, pm.OccuredTime, pm.NumberofCaregivers, pm.Note_, pm.Id, pm.DataOpenStatusUserId, pm.DataOpenStatus, pm.CompletionDate, pm.Caregiver })
                   .Join<Stations>
                    (
                        s => new { StationCode = s.Code },
                        nameof(UnplannedMaintenances.StationID),
                        nameof(Stations.Id),
                        JoinType.Left
                    )
                    .Join<MaintenancePeriods>
                    (
                        mp => new { PeriodName = mp.Name },
                        nameof(UnplannedMaintenances.PeriodID),
                        nameof(MaintenancePeriods.Id),
                        JoinType.Left
                    )
                    .Where(null, false, false, Tables.UnplannedMaintenances);

            var maintenances = queryFactory.GetList<ListUnplannedMaintenancesDto>(query).ToList();
            return new SuccessDataResult<IList<ListUnplannedMaintenancesDto>>(maintenances);
        }

        [ValidationAspect(typeof(UpdateUnplannedMaintenanceValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnplannedMaintenancesDto>> UpdateAsync(UpdateUnplannedMaintenancesDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                  .From(Tables.UnplannedMaintenances)
                   .Select("*")
                   .Join<Stations>
                    (
                        s => new { StationID = s.Id, StationCode = s.Code },
                        nameof(UnplannedMaintenances.StationID),
                        nameof(Stations.Id),
                        JoinType.Left
                    )
                    .Join<MaintenancePeriods>
                    (
                        mp => new { PeriodID = mp.Id, PeriodName = mp.Name },
                        nameof(UnplannedMaintenances.PeriodID),
                        nameof(MaintenancePeriods.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, false, false, Tables.UnplannedMaintenances);

            var entity = queryFactory.Get<SelectUnplannedMaintenancesDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                    .From(Tables.UnplannedMaintenanceLines)
                   .Select<UnplannedMaintenanceLines>(pml => new { pml.UnitSetID, pml.ProductID, pml.UnplannedMaintenanceID, pml.MaintenanceNote, pml.LineNr, pml.InstructionDescription, pml.Id, pml.DataOpenStatusUserId, pml.DataOpenStatus, pml.Amount })
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(UnplannedMaintenanceLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(UnplannedMaintenanceLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                    .Where(new { UnplannedMaintenanceID = input.Id }, false, false, Tables.UnplannedMaintenanceLines);

            var maintenanceLine = queryFactory.GetList<SelectUnplannedMaintenanceLinesDto>(queryLines).ToList();

            entity.SelectUnplannedMaintenanceLines = maintenanceLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                            .From(Tables.UnplannedMaintenances)
                   .Select<UnplannedMaintenances>(pm => new { pm.Status, pm.StationID, pm.StartDate, pm.RemainingTime, pm.RegistrationNo, pm.UnplannedDate, pm.UnplannedTime, pm.PeriodTime, pm.PeriodID, pm.OccuredTime, pm.NumberofCaregivers, pm.Note_, pm.Id, pm.DataOpenStatusUserId, pm.DataOpenStatus, pm.CompletionDate, pm.Caregiver })
                   .Join<Stations>
                    (
                        s => new { StationCode = s.Code },
                        nameof(UnplannedMaintenances.StationID),
                        nameof(Stations.Id),
                        JoinType.Left
                    )
                    .Join<MaintenancePeriods>
                    (
                        mp => new { PeriodName = mp.Name },
                        nameof(UnplannedMaintenances.PeriodID),
                        nameof(MaintenancePeriods.Id),
                        JoinType.Left
                    )
                            .Where(new { RegistrationNo = input.RegistrationNo }, false, false, Tables.UnplannedMaintenances);

            var list = queryFactory.GetList<ListUnplannedMaintenancesDto>(listQuery).ToList();

            if (list.Count > 0 && entity.RegistrationNo != input.RegistrationNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            var query = queryFactory.Query().From(Tables.UnplannedMaintenances).Update(new UpdateUnplannedMaintenancesDto
            {
                RegistrationNo = input.RegistrationNo,
                StationID = input.StationID,
                CompletionDate = input.CompletionDate,
                Caregiver = input.Caregiver,
                Note_ = input.Note_,
                NumberofCaregivers = input.NumberofCaregivers,
                OccuredTime = input.OccuredTime,
                PeriodID = input.PeriodID,
                PeriodTime = input.PeriodTime,
                UnplannedDate = input.UnplannedDate,
                UnplannedTime = input.UnplannedTime,
                RemainingTime = input.RemainingTime,
                StartDate = input.StartDate,
                Status = input.Status,
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

            foreach (var item in input.SelectUnplannedMaintenanceLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.UnplannedMaintenanceLines).Insert(new CreateUnplannedMaintenanceLinesDto
                    {
                        Amount = item.Amount,
                        InstructionDescription = item.InstructionDescription,
                        MaintenanceNote = item.MaintenanceNote,
                        UnplannedMaintenanceID = input.Id,
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
                        UnitSetID = item.UnitSetID,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.UnplannedMaintenanceLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                    var line = queryFactory.Get<SelectUnplannedMaintenanceLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.UnplannedMaintenanceLines).Update(new UpdateUnplannedMaintenanceLinesDto
                        {
                            Amount = item.Amount,
                            InstructionDescription = item.InstructionDescription,
                            MaintenanceNote = item.MaintenanceNote,
                            UnplannedMaintenanceID = input.Id,
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
                            UnitSetID = item.UnitSetID,
                        }).Where(new { Id = line.Id }, false, false, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var maintenance = queryFactory.Update<SelectUnplannedMaintenancesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.UnplannedMaintenances, LogType.Update, maintenance.Id);

            return new SuccessDataResult<SelectUnplannedMaintenancesDto>(maintenance);
        }

        public async Task<IDataResult<SelectUnplannedMaintenancesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.UnplannedMaintenances).Select("*").Where(new { Id = id }, false, false, "");

            var entity = queryFactory.Get<UnplannedMaintenances>(entityQuery);

            var query = queryFactory.Query().From(Tables.UnplannedMaintenances).Update(new UpdateUnplannedMaintenancesDto
            {
                RegistrationNo = entity.RegistrationNo,
                StationID = entity.StationID,
                CompletionDate = entity.CompletionDate,
                Caregiver = entity.Caregiver,
                Note_ = entity.Note_,
                NumberofCaregivers = entity.NumberofCaregivers,
                OccuredTime = entity.OccuredTime,
                PeriodID = entity.PeriodID,
                PeriodTime = entity.PeriodTime,
                UnplannedTime = entity.UnplannedTime,
                UnplannedDate = entity.UnplannedDate,
                RemainingTime = entity.RemainingTime,
                StartDate = entity.StartDate,
                Status = (int)entity.Status,
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

            var maintenancesDto = queryFactory.Update<SelectUnplannedMaintenancesDto>(query, "Id", true);
            return new SuccessDataResult<SelectUnplannedMaintenancesDto>(maintenancesDto);
        }
    }
}
