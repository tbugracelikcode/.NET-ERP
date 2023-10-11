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
using TsiErp.Business.Entities.PlannedMaintenance.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenancePeriod;
using TsiErp.Entities.Entities.MaintenanceManagement.PlannedMaintenance;
using TsiErp.Entities.Entities.MaintenanceManagement.PlannedMaintenance.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.PlannedMaintenanceLine;
using TsiErp.Entities.Entities.MaintenanceManagement.PlannedMaintenanceLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PlannedMaintenances.Page;

namespace TsiErp.Business.Entities.PlannedMaintenance.Services
{
    [ServiceRegistration(typeof(IPlannedMaintenancesAppService), DependencyInjectionType.Scoped)]
    public class PlannedMaintenancesAppService : ApplicationService<PlannedMaintenancesResource>, IPlannedMaintenancesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }

        public PlannedMaintenancesAppService(IStringLocalizer<PlannedMaintenancesResource> l, IFicheNumbersAppService ficheNumbersAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
        }

        [ValidationAspect(typeof(CreatePlannedMaintenanceValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPlannedMaintenancesDto>> CreateAsync(CreatePlannedMaintenancesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.PlannedMaintenances).Select("*").Where(new { RegistrationNo = input.RegistrationNo }, false, false, "");
            var list = queryFactory.ControlList<PlannedMaintenances>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.PlannedMaintenances).Insert(new CreatePlannedMaintenancesDto
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
                PlannedDate = input.PlannedDate,
                PlannedTime = input.PlannedTime,
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

            foreach (var item in input.SelectPlannedMaintenanceLines)
            {
                var queryLine = queryFactory.Query().From(Tables.PlannedMaintenanceLines).Insert(new CreatePlannedMaintenanceLinesDto
                {
                    Amount = item.Amount,
                    InstructionDescription = item.InstructionDescription,
                    MaintenanceNote = item.MaintenanceNote,
                    PlannedMaintenanceID = addedEntityId,
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

            var maintenance = queryFactory.Insert<SelectPlannedMaintenancesDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("PlannedMainChildMenu", input.RegistrationNo);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PlannedMaintenances, LogType.Insert, addedEntityId);

            return new SuccessDataResult<SelectPlannedMaintenancesDto>(maintenance);
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.PlannedMaintenances).Select("*").Where(new { Id = id }, false, false, "");

            var maintenances = queryFactory.Get<SelectPlannedMaintenancesDto>(query);

            if (maintenances.Id != Guid.Empty && maintenances != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.PlannedMaintenances).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.PlannedMaintenanceLines).Delete(LoginedUserService.UserId).Where(new { PlannedMaintenanceID = id }, false, false, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var maintenance = queryFactory.Update<SelectPlannedMaintenancesDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PlannedMaintenances, LogType.Delete, id);
                return new SuccessDataResult<SelectPlannedMaintenancesDto>(maintenance);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.PlannedMaintenanceLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                var maintenanceLines = queryFactory.Update<SelectPlannedMaintenanceLinesDto>(queryLine, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PlannedMaintenanceLines, LogType.Delete, id);
                return new SuccessDataResult<SelectPlannedMaintenanceLinesDto>(maintenanceLines);
            }
        }

        public async Task<IDataResult<SelectPlannedMaintenancesDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.PlannedMaintenances)
                   .Select<PlannedMaintenances>(pm => new { pm.Status, pm.StationID, pm.StartDate, pm.RemainingTime, pm.RegistrationNo, pm.PlannedTime, pm.PlannedDate, pm.PeriodTime, pm.PeriodID, pm.OccuredTime, pm.NumberofCaregivers, pm.Note_, pm.Id, pm.DataOpenStatusUserId, pm.DataOpenStatus, pm.CompletionDate, pm.Caregiver })
                   .Join<Stations>
                    (
                        s => new { StationID = s.Id, StationCode = s.Code },
                        nameof(PlannedMaintenances.StationID),
                        nameof(Stations.Id),
                        JoinType.Left
                    )
                    .Join<MaintenancePeriods>
                    (
                        mp => new { PeriodID = mp.Id, PeriodName = mp.Name },
                        nameof(PlannedMaintenances.PeriodID),
                        nameof(MaintenancePeriods.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, false, false, Tables.PlannedMaintenances);

            var maintenances = queryFactory.Get<SelectPlannedMaintenancesDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PlannedMaintenanceLines)
                   .Select<PlannedMaintenanceLines>(pml => new { pml.UnitSetID, pml.ProductID, pml.PlannedMaintenanceID, pml.MaintenanceNote, pml.LineNr, pml.InstructionDescription, pml.Id, pml.DataOpenStatusUserId, pml.DataOpenStatus, pml.Amount })
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PlannedMaintenanceLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(PlannedMaintenanceLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                    .Where(new { PlannedMaintenanceID = id }, false, false, Tables.PlannedMaintenanceLines);

            var maintenanceLine = queryFactory.GetList<SelectPlannedMaintenanceLinesDto>(queryLines).ToList();

            maintenances.SelectPlannedMaintenanceLines = maintenanceLine;

            LogsAppService.InsertLogToDatabase(maintenances, maintenances, LoginedUserService.UserId, Tables.PlannedMaintenances, LogType.Get, id);

            return new SuccessDataResult<SelectPlannedMaintenancesDto>(maintenances);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPlannedMaintenancesDto>>> GetListAsync(ListPlannedMaintenancesParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.PlannedMaintenances)
                   .Select<PlannedMaintenances>(pm => new { pm.Status, pm.StationID, pm.StartDate, pm.RemainingTime, pm.RegistrationNo, pm.PlannedTime, pm.PlannedDate, pm.PeriodTime, pm.PeriodID, pm.OccuredTime, pm.NumberofCaregivers, pm.Note_, pm.Id, pm.DataOpenStatusUserId, pm.DataOpenStatus, pm.CompletionDate, pm.Caregiver })
                   .Join<Stations>
                    (
                        s => new { StationCode = s.Code },
                        nameof(PlannedMaintenances.StationID),
                        nameof(Stations.Id),
                        JoinType.Left
                    )
                    .Join<MaintenancePeriods>
                    (
                        mp => new { PeriodName = mp.Name },
                        nameof(PlannedMaintenances.PeriodID),
                        nameof(MaintenancePeriods.Id),
                        JoinType.Left
                    )
                    .Where(null, false, false, Tables.PlannedMaintenances);

            var maintenances = queryFactory.GetList<ListPlannedMaintenancesDto>(query).ToList();
            return new SuccessDataResult<IList<ListPlannedMaintenancesDto>>(maintenances);
        }

        [ValidationAspect(typeof(UpdatePlannedMaintenanceValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPlannedMaintenancesDto>> UpdateAsync(UpdatePlannedMaintenancesDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                    .From(Tables.PlannedMaintenances)
                   .Select("*")
                   .Join<Stations>
                    (
                        s => new { StationID = s.Id, StationCode = s.Code },
                        nameof(PlannedMaintenances.StationID),
                        nameof(Stations.Id),
                        JoinType.Left
                    )
                    .Join<MaintenancePeriods>
                    (
                        mp => new { PeriodID = mp.Id, PeriodName = mp.Name },
                        nameof(PlannedMaintenances.PeriodID),
                        nameof(MaintenancePeriods.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, false, false, Tables.PlannedMaintenances);

            var entity = queryFactory.Get<SelectPlannedMaintenancesDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                    .From(Tables.PlannedMaintenanceLines)
                   .Select<PlannedMaintenanceLines>(pml => new { pml.UnitSetID, pml.ProductID, pml.PlannedMaintenanceID, pml.MaintenanceNote, pml.LineNr, pml.InstructionDescription, pml.Id, pml.DataOpenStatusUserId, pml.DataOpenStatus, pml.Amount })
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PlannedMaintenanceLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(PlannedMaintenanceLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                    .Where(new { PlannedMaintenanceID = input.Id }, false, false, Tables.PlannedMaintenanceLines);

            var maintenanceLine = queryFactory.GetList<SelectPlannedMaintenanceLinesDto>(queryLines).ToList();

            entity.SelectPlannedMaintenanceLines = maintenanceLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.PlannedMaintenances)
                           .Select<PlannedMaintenances>(pm => new { pm.Status, pm.StationID, pm.StartDate, pm.RemainingTime, pm.RegistrationNo, pm.PlannedTime, pm.PlannedDate, pm.PeriodTime, pm.PeriodID, pm.OccuredTime, pm.NumberofCaregivers, pm.Note_, pm.Id, pm.DataOpenStatusUserId, pm.DataOpenStatus, pm.CompletionDate, pm.Caregiver })
                           .Join<Stations>
                            (
                                s => new { StationCode = s.Code },
                                nameof(PlannedMaintenances.StationID),
                                nameof(Stations.Id),
                                JoinType.Left
                            )
                            .Join<MaintenancePeriods>
                            (
                                mp => new { PeriodName = mp.Name },
                                nameof(PlannedMaintenances.PeriodID),
                                nameof(MaintenancePeriods.Id),
                                JoinType.Left
                            )
                            .Where(new { RegistrationNo = input.RegistrationNo }, false, false, Tables.PlannedMaintenances);

            var list = queryFactory.GetList<ListPlannedMaintenancesDto>(listQuery).ToList();

            if (list.Count > 0 && entity.RegistrationNo != input.RegistrationNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            var query = queryFactory.Query().From(Tables.PlannedMaintenances).Update(new UpdatePlannedMaintenancesDto
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
                PlannedDate = input.PlannedDate,
                PlannedTime = input.PlannedTime,
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

            foreach (var item in input.SelectPlannedMaintenanceLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PlannedMaintenanceLines).Insert(new CreatePlannedMaintenanceLinesDto
                    {
                        Amount = item.Amount,
                        InstructionDescription = item.InstructionDescription,
                        MaintenanceNote = item.MaintenanceNote,
                        PlannedMaintenanceID = input.Id,
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
                    var lineGetQuery = queryFactory.Query().From(Tables.PlannedMaintenanceLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                    var line = queryFactory.Get<SelectPlannedMaintenanceLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PlannedMaintenanceLines).Update(new UpdatePlannedMaintenanceLinesDto
                        {
                            Amount = item.Amount,
                            InstructionDescription = item.InstructionDescription,
                            MaintenanceNote = item.MaintenanceNote,
                            PlannedMaintenanceID = input.Id,
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

            var maintenance = queryFactory.Update<SelectPlannedMaintenancesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PlannedMaintenances, LogType.Update, entity.Id);

            return new SuccessDataResult<SelectPlannedMaintenancesDto>(maintenance);
        }

        public async Task<IDataResult<SelectPlannedMaintenancesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.PlannedMaintenances).Select("*").Where(new { Id = id }, false, false, "");

            var entity = queryFactory.Get<PlannedMaintenances>(entityQuery);

            var query = queryFactory.Query().From(Tables.PlannedMaintenances).Update(new UpdatePlannedMaintenancesDto
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
                PlannedDate = entity.PlannedDate,
                PlannedTime = entity.PlannedTime,
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

            var maintenancesDto = queryFactory.Update<SelectPlannedMaintenancesDto>(query, "Id", true);
            return new SuccessDataResult<SelectPlannedMaintenancesDto>(maintenancesDto);
        }
    }
}
