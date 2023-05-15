using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.TemplateOperations.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.TemplateOperation.BusinessRules;
using TsiErp.Business.Entities.TemplateOperation.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.TemplateOperation;
using TsiErp.Entities.Entities.TemplateOperation.Dtos;
using TsiErp.Entities.Entities.TemplateOperationLine;
using TsiErp.Entities.Entities.TemplateOperationLine.Dtos;
using Microsoft.Extensions.Localization;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Entities.TableConstant;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Entities.Entities.Station;

namespace TsiErp.Business.Entities.TemplateOperation.Services
{
    [ServiceRegistration(typeof(ITemplateOperationsAppService), DependencyInjectionType.Scoped)]
    public class TemplateOperationsAppService : ApplicationService<TemplateOperationsResource>, ITemplateOperationsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public TemplateOperationsAppService(IStringLocalizer<TemplateOperationsResource> l) : base(l)
        {
        }

        [ValidationAspect(typeof(CreateTemplateOperationsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectTemplateOperationsDto>> CreateAsync(CreateTemplateOperationsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.TemplateOperations).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.ControlList<TemplateOperations>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();

                var query = queryFactory.Query().From(Tables.TemplateOperations).Insert(new CreateTemplateOperationsDto
                {
                    WorkCenterID = input.WorkCenterID,
                    Code = input.Code,
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = addedEntityId,
                    IsActive = true,
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    Name = input.Name,
                });

                foreach (var item in input.SelectTemplateOperationLines)
                {
                    var queryLine = queryFactory.Query().From(Tables.TemplateOperationLines).Insert(new CreateTemplateOperationLinesDto
                    {
                        AdjustmentAndControlTime = item.AdjustmentAndControlTime,
                        Alternative = item.Alternative,
                        OperationTime = item.OperationTime,
                        Priority = item.Priority,
                        ProcessQuantity = item.ProcessQuantity,
                        StationID = item.StationID,
                        TemplateOperationID = addedEntityId,
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

                var templateOperation = queryFactory.Insert<SelectTemplateOperationsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.TemplateOperations, LogType.Insert, templateOperation.Id);

                return new SuccessDataResult<SelectTemplateOperationsDto>(templateOperation);
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.TemplateOperations).Select("*").Where(new { Id = id }, true, true, "");

                var templateOperations = queryFactory.Get<SelectTemplateOperationsDto>(query);

                if (templateOperations.Id != Guid.Empty && templateOperations != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.TemplateOperations).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.TemplateOperationLines).Delete(LoginedUserService.UserId).Where(new { TemplateOperationID = id }, false, false, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var templateOperation = queryFactory.Update<SelectTemplateOperationsDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.TemplateOperations, LogType.Delete, id);
                    return new SuccessDataResult<SelectTemplateOperationsDto>(templateOperation);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.TemplateOperationLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                    var templateOperationLines = queryFactory.Update<SelectTemplateOperationLinesDto>(queryLine, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.TemplateOperationLines, LogType.Delete, id);
                    return new SuccessDataResult<SelectTemplateOperationLinesDto>(templateOperationLines);
                }
            }
        }

        public async Task<IDataResult<SelectTemplateOperationsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.TemplateOperations).Select("*").Where(
               new
               {
                   Id = id
               }, true, true, "");

                var templateOperations = queryFactory.Get<SelectTemplateOperationsDto>(query);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.TemplateOperationLines)
                       .Select<TemplateOperationLines>(tol => new { tol.TemplateOperationID, tol.StationID, tol.ProcessQuantity, tol.Priority, tol.OperationTime, tol.LineNr, tol.Id, tol.DataOpenStatusUserId, tol.DataOpenStatus, tol.Alternative, tol.AdjustmentAndControlTime })
                       .Join<Stations>
                        (
                            s => new { StationID = s.Id, StationCode = s.Code, StationName = s.Name },
                            nameof(TemplateOperationLines.StationID),
                            nameof(Stations.Id),
                            JoinType.Left
                        )
                        .Where(new { TemplateOperationID = id }, false, false, Tables.TemplateOperationLines);

                var templateOperationLine = queryFactory.GetList<SelectTemplateOperationLinesDto>(queryLines).ToList();

                templateOperations.SelectTemplateOperationLines = templateOperationLine;

                LogsAppService.InsertLogToDatabase(templateOperations, templateOperations, LoginedUserService.UserId, Tables.TemplateOperations, LogType.Get, id);

                return new SuccessDataResult<SelectTemplateOperationsDto>(templateOperations);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListTemplateOperationsDto>>> GetListAsync(ListTemplateOperationsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.TemplateOperations).Select("*").Where(null, true, true, "");
                var templateOperations = queryFactory.GetList<ListTemplateOperationsDto>(query).ToList();
                return new SuccessDataResult<IList<ListTemplateOperationsDto>>(templateOperations);
            }


        }

        [ValidationAspect(typeof(UpdateTemplateOperationsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectTemplateOperationsDto>> UpdateAsync(UpdateTemplateOperationsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.TemplateOperations).Select("*").Where(new { Id = input.Id }, true, true, Tables.TemplateOperations);

                var entity = queryFactory.Get<SelectTemplateOperationsDto>(entityQuery);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.TemplateOperationLines)
                       .Select<TemplateOperationLines>(tol => new { tol.TemplateOperationID, tol.StationID, tol.ProcessQuantity, tol.Priority, tol.OperationTime, tol.LineNr, tol.Id, tol.DataOpenStatusUserId, tol.DataOpenStatus, tol.Alternative, tol.AdjustmentAndControlTime })
                       .Join<Stations>
                        (
                            s => new { StationID = s.Id, StationCode = s.Code, StationName = s.Name },
                            nameof(TemplateOperationLines.StationID),
                            nameof(Stations.Id),
                            JoinType.Left
                        )
                        .Where(new { TemplateOperationID = input.Id }, false, false, Tables.TemplateOperationLines);

                var templateOperationLine = queryFactory.GetList<SelectTemplateOperationLinesDto>(queryLines).ToList();

                entity.SelectTemplateOperationLines = templateOperationLine;

                #region Update Control
                var listQuery = queryFactory.Query().From(Tables.TemplateOperations).Select("*").Where(new { Code = input.Code }, false, false, Tables.TemplateOperations);

                var list = queryFactory.GetList<ListTemplateOperationsDto>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }
                #endregion

                var query = queryFactory.Query().From(Tables.TemplateOperations).Update(new UpdateTemplateOperationsDto
                {
                    Name = input.Name,
                    WorkCenterID = input.WorkCenterID,
                    Code = input.Code,
                    CreationTime = entity.CreationTime,
                    CreatorId = entity.CreatorId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                    Id = input.Id,
                    IsActive = input.IsActive,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId,
                }).Where(new { Id = input.Id }, true, true, "");

                foreach (var item in input.SelectTemplateOperationLines)
                {
                    if (item.Id == Guid.Empty)
                    {
                        var queryLine = queryFactory.Query().From(Tables.TemplateOperationLines).Insert(new CreateTemplateOperationLinesDto
                        {
                            AdjustmentAndControlTime = item.AdjustmentAndControlTime,
                            Alternative = item.Alternative,
                            OperationTime = item.OperationTime,
                            Priority = item.Priority,
                            ProcessQuantity = item.ProcessQuantity,
                            StationID = item.StationID,
                            TemplateOperationID = input.Id,
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
                        var lineGetQuery = queryFactory.Query().From(Tables.TemplateOperationLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                        var line = queryFactory.Get<SelectTemplateOperationLinesDto>(lineGetQuery);

                        if (line != null)
                        {
                            var queryLine = queryFactory.Query().From(Tables.TemplateOperationLines).Update(new UpdateTemplateOperationLinesDto
                            {
                                AdjustmentAndControlTime = item.AdjustmentAndControlTime,
                                Alternative = item.Alternative,
                                OperationTime = item.OperationTime,
                                Priority = item.Priority,
                                ProcessQuantity = item.ProcessQuantity,
                                StationID = item.StationID,
                                TemplateOperationID = input.Id,
                                CreationTime = line.CreationTime,
                                CreatorId = line.CreatorId,
                                DataOpenStatus = false,
                                DataOpenStatusUserId = Guid.Empty,
                                DeleterId = line.DeleterId.GetValueOrDefault(),
                                DeletionTime = line.DeletionTime.GetValueOrDefault(),
                                Id = item.Id,
                                IsDeleted = false,
                                LastModificationTime = DateTime.Now,
                                LastModifierId = LoginedUserService.UserId,
                                LineNr = item.LineNr,
                            }).Where(new { Id = line.Id }, false, false, "");

                            query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                        }
                    }
                }

                var templateOperation = queryFactory.Update<SelectTemplateOperationsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.TemplateOperations, LogType.Update, templateOperation.Id);

                return new SuccessDataResult<SelectTemplateOperationsDto>(templateOperation);
            }
        }

        public async Task<IDataResult<SelectTemplateOperationsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.TemplateOperations).Select("*").Where(new { Id = id }, true, true, "");

                var entity = queryFactory.Get<TemplateOperations>(entityQuery);

                var query = queryFactory.Query().From(Tables.TemplateOperations).Update(new UpdateTemplateOperationsDto
                {
                    WorkCenterID = entity.WorkCenterID,
                    Code = entity.Code,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DataOpenStatus = lockRow,
                    DataOpenStatusUserId = userId,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.Value,
                    Id = entity.Id,
                    IsActive = entity.IsActive,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                    LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                    Name = entity.Name,
                }).Where(new { Id = id }, true, true, "");

                var templateOperationsDto = queryFactory.Update<SelectTemplateOperationsDto>(query, "Id", true);
                return new SuccessDataResult<SelectTemplateOperationsDto>(templateOperationsDto);

            }
        }
    }
}
