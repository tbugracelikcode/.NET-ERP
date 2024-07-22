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
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.TestManagement.Continent.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.QualityControl.CalibrationRecord;
using TsiErp.Entities.Entities.QualityControl.EquipmentRecord;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.TestManagement.Continent;
using TsiErp.Entities.Entities.TestManagement.Continent.Dtos;
using TsiErp.Entities.Entities.TestManagement.ContinentLine.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Continents.Page;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee;

namespace TsiErp.Business.Entities.Continent.Services
{
    [ServiceRegistration(typeof(IContinentsAppService), DependencyInjectionType.Scoped)]
    public class ContinentsAppService : ApplicationService<ContinentsResource>, IContinentsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public ContinentsAppService(IStringLocalizer<ContinentsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }

        [ValidationAspect(typeof(CreateContinentsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectContinentsDto>> CreateAsync(CreateContinentsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.Continents).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.ControlList<Continents>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.Continents).Insert(new CreateContinentsDto
            {
                Code = input.Code,

                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                EmployeeID = input.EmployeeID.GetValueOrDefault(),
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
                Population_ = input.Population_,
            });

            foreach (var item in input.SelectContinentLines)
            {
                var queryLine = queryFactory.Query().From(Tables.ContinentLines).Insert(new CreateContinentLinesDto
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
                    ContinentID = addedEntityId,
                    CountryName = item.CountryName,
                    CountryPopulation = item.CountryPopulation,
                    IsNATOMember = item.IsNATOMember,
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var continent = queryFactory.Insert<SelectContinentsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("ContinentsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Continents, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectContinentsDto>(continent);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.Continents).Select("*").Where(new { Id = id }, false, false, "");

            var Continents = queryFactory.Get<SelectContinentsDto>(query);

            if (Continents.Id != Guid.Empty && Continents != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.Continents).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.ContinentLines).Delete(LoginedUserService.UserId).Where(new { ContinentID = id }, false, false, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var continent = queryFactory.Update<SelectContinentsDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Continents, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectContinentsDto>(continent);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.ContinentLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                var continentLines = queryFactory.Update<SelectContinentLinesDto>(queryLine, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ContinentLines, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectContinentLinesDto>(continentLines);
            }

        }

        public async Task<IDataResult<SelectContinentsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.Continents)
                   .Select<Continents>(null)
                     .Join<Employees>
                        (
                            e => new { EmployeeName = e.Name, EmployeeID = e.Id },
                            nameof(Continents.EmployeeID),
                            nameof(Employees.Id),
                            JoinType.Left
                        )
                   .Where(new { Id = id }, false, false, Tables.Continents);


            var continents = queryFactory.Get<SelectContinentsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.ContinentLines)
                   .Select("*")
                    .Where(new { ContinentID = id }, false, false, "");

            var ContinentLine = queryFactory.GetList<SelectContinentLinesDto>(queryLines).ToList();

            continents.SelectContinentLines = ContinentLine;

            LogsAppService.InsertLogToDatabase(continents, continents, LoginedUserService.UserId, Tables.Continents, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectContinentsDto>(continents);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListContinentsDto>>> GetListAsync(ListContinentsParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.Continents)
                    .Select<Continents>(null)
                     .Join<Employees>
                        (
                            e => new { EmployeeName = e.Name, EmployeeID = e.Id },
                            nameof(Continents.EmployeeID),
                            nameof(Employees.Id),
                            JoinType.Left
                        )
                    .Where(null, false, false, Tables.Continents);

            var continents = queryFactory.GetList<ListContinentsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListContinentsDto>>(continents);

        }


        [ValidationAspect(typeof(UpdateContinentsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectContinentsDto>> UpdateAsync(UpdateContinentsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.Continents)
                   .Join<Employees>
                        (
                            e => new { EmployeeName = e.Name, EmployeeID = e.Id },
                            nameof(Continents.EmployeeID),
                            nameof(Employees.Id),
                            JoinType.Left
                        )
                    .Where(new { Id = input.Id }, false, false, Tables.Continents);

            var entity = queryFactory.Get<SelectContinentsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.ContinentLines)
                   .Select("*")
                    .Where(new { ContinentID = input.Id }, false, false, "");

            var ContinentLine = queryFactory.GetList<SelectContinentLinesDto>(queryLines).ToList();

            entity.SelectContinentLines = ContinentLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.Continents)
                        .Join<Employees>
                        (
                            e => new { EmployeeName = e.Name, EmployeeID = e.Id },
                            nameof(Continents.EmployeeID),
                            nameof(Employees.Id),
                            JoinType.Left
                        )
                            .Where(new { Code = input.Code }, false, false, Tables.Continents);

            var list = queryFactory.GetList<ListContinentsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            var query = queryFactory.Query().From(Tables.Continents).Update(new UpdateContinentsDto
            {
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                EmployeeID = input.EmployeeID.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId,
                Name = input.Name,
                Description_ = input.Description_,
                Population_ = input.Population_
            }).Where(new { Id = input.Id }, false, false, "");

            foreach (var item in input.SelectContinentLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.ContinentLines).Insert(new CreateContinentLinesDto
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
                        ContinentID = input.Id,
                        Description_ = item.Description_,
                        IsNATOMember = item.IsNATOMember,
                        CountryPopulation = item.CountryPopulation,
                        CountryName = item.CountryName,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.ContinentLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                    var line = queryFactory.Get<SelectContinentLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.ContinentLines).Update(new UpdateContinentLinesDto
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
                            CountryName = item.CountryName,
                            CountryPopulation = item.CountryPopulation,
                            IsNATOMember = item.IsNATOMember,
                            Description_ = item.Description_,
                            ContinentID = input.Id,
                        }).Where(new { Id = line.Id }, false, false, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var continent = queryFactory.Update<SelectContinentsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.Continents, LogType.Update, continent.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectContinentsDto>(continent);

        }

        public async Task<IDataResult<SelectContinentsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.Continents).Select("*").Where(new { Id = id }, false, false, "");

            var entity = queryFactory.Get<Continents>(entityQuery);

            var query = queryFactory.Query().From(Tables.Continents).Update(new UpdateContinentsDto
            {
                Code = entity.Code,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                EmployeeID = entity.EmployeeID,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = entity.Id,
                Description_ = entity.Description_,
                Population_ = entity.Population_,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Name = entity.Name,
                 
            }).Where(new { Id = id }, false, false, "");

            var ContinentsDto = queryFactory.Update<SelectContinentsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectContinentsDto>(ContinentsDto);


        }
    }
}
