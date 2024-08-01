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
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.TestManagement.Sector.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.TestManagement.Sector;
using TsiErp.Entities.Entities.TestManagement.Sector.Dtos;
using TsiErp.Entities.Entities.TestManagement.SectorLine.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Sectors.Page;

namespace TsiErp.Business.Entities.TestManagement.Sector.Services
{

    [ServiceRegistration(typeof(ISectorsAppService), DependencyInjectionType.Scoped)] //Servis Açıldığındna
    public class SectorsAppService : ApplicationService<SectorsResource>, ISectorsAppService
    {

        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;


        public SectorsAppService(IStringLocalizer<SectorsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }
        [ValidationAspect(typeof(CreateSectorsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSectorsDto>> CreateAsync(CreateSectorsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.Sectors).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.ControlList<Sectors>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.Sectors).Insert(new CreateSectorsDto
            {
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
                Name = input.Name,
                Description_ = input.Description_,
                IsPrivateSector = input.IsPrivateSector,
                Type_ = input.Type_,


            });

            foreach (var item in input.SelectSectorLines)
            {
                var queryLine = queryFactory.Query().From(Tables.SectorLines).Insert(new CreateSectorLinesDto
                {
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
                    Description_ = item.Description_,
                    CompanyName = item.CompanyName,
                    IsSoleProprietorship = item.IsSoleProprietorship,
                    CompanyNo = item.CompanyNo,
                    SectorID = addedEntityId,




                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var sector = queryFactory.Insert<SelectSectorsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("SectorsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Sectors, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectSectorsDto>(sector);


        }
        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.Sectors).Select("*").Where(new { Id = id }, false, false, "");

            var Sectors = queryFactory.Get<SelectSectorsDto>(query);

            if (Sectors.Id != Guid.Empty && Sectors != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.Sectors).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.SectorLines).Delete(LoginedUserService.UserId).Where(new { SectorID = id }, false, false, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var sector = queryFactory.Update<SelectSectorsDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Sectors, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectSectorsDto>(sector);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.SectorLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                var sectorLines = queryFactory.Update<SelectSectorLinesDto>(queryLine, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.SectorLines, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectSectorLinesDto>(sectorLines);
            }


        }
        public async Task<IDataResult<SelectSectorsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.Sectors)
                   .Select("*").Where(new { Id = id }, false, false, "");

            var Sectors = queryFactory.Get<SelectSectorsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.SectorLines)
                   .Select("*")
                    .Where(new { SectorID = id }, false, false, "");

            var SectorLine = queryFactory.GetList<SelectSectorLinesDto>(queryLines).ToList();

            Sectors.SelectSectorLines = SectorLine;

            LogsAppService.InsertLogToDatabase(Sectors, Sectors, LoginedUserService.UserId, Tables.Sectors, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectSectorsDto>(Sectors);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListSectorsDto>>> GetListAsync(ListSectorsParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.Sectors)
                   .Select("*")
                    .Where(null, false, false, "");

            var Sectors = queryFactory.GetList<ListSectorsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListSectorsDto>>(Sectors);

        }


        [ValidationAspect(typeof(UpdateSectorsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSectorsDto>> UpdateAsync(UpdateSectorsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.Sectors)
                   .Select("*")
                    .Where(new { Id = input.Id }, false, false, "");

            var entity = queryFactory.Get<SelectSectorsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.SectorLines)
                   .Select("*")
                    .Where(new { SectorID = input.Id }, false, false, "");

            var SectorLine = queryFactory.GetList<SelectSectorLinesDto>(queryLines).ToList();

            entity.SelectSectorLines = SectorLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.Sectors)
                           .Select("*")
                            .Where(new { Code = input.Code }, false, false, "");

            var list = queryFactory.GetList<ListSectorsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            var query = queryFactory.Query().From(Tables.Sectors).Update(new UpdateSectorsDto
            {
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
                Name = input.Name,
                Description_ = input.Description_,
                 Type_ = input.Type_,
                  IsPrivateSector = input.IsPrivateSector, 
                   SelectSectorLines = input.SelectSectorLines,
            }).Where(new { Id = input.Id }, false, false, "");

            foreach (var item in input.SelectSectorLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.SectorLines).Insert(new CreateSectorLinesDto
                    {
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
                        SectorID = input.Id,
                        Description_ = item.Description_,
                         CompanyNo = item.CompanyNo,
                          CompanyName = item.CompanyName,
                           IsSoleProprietorship = item.IsSoleProprietorship

                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.SectorLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                    var line = queryFactory.Get<SelectSectorLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.SectorLines).Update(new UpdateSectorLinesDto
                        {
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
                            Description_ = item.Description_,
                            SectorID = input.Id,
                             CompanyName = item.CompanyName,
                              CompanyNo = item.CompanyNo,
                               IsSoleProprietorship = item.IsSoleProprietorship
                             

                        }).Where(new { Id = line.Id }, false, false, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var sector = queryFactory.Update<SelectSectorsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.Sectors, LogType.Update, sector.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectSectorsDto>(sector);

        }

        public async Task<IDataResult<SelectSectorsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.Sectors).Select("*").Where(new { Id = id }, false, false, "");

            var entity = queryFactory.Get<Sectors>(entityQuery);

            var query = queryFactory.Query().From(Tables.Sectors).Update(new UpdateSectorsDto
            {
                Code = entity.Code,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = entity.Id,
                Description_ = entity.Description_,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Name = entity.Name,
                IsPrivateSector = entity.IsPrivateSector,
                Type_ = entity.Type_,
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, false, false, "");

            var SectorsDto = queryFactory.Update<SelectSectorsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectSectorsDto>(SectorsDto);

        }










        }

    }