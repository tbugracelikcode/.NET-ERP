using Microsoft.EntityFrameworkCore.Query.Internal;
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
using TsiErp.Business.Entities.TestManagement.City.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.TestManagement.City;
using TsiErp.Entities.Entities.TestManagement.City.Dtos;
using TsiErp.Entities.Entities.TestManagement.District.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Cities.Page;



namespace TsiErp.Business.Entities.TestManagement.City.Services
{

    [ServiceRegistration(typeof(ICitiesAppService), DependencyInjectionType.Scoped)] //Servis Açıldığındna
    public class CitiesAppService : ApplicationService<CitiesResource>, ICitiesAppService
    {

        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public CitiesAppService(IStringLocalizer<CitiesResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }
        [ValidationAspect(typeof(CreateCitiesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCitiesDto>> CreateAsync(CreateCitiesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.Cities).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.ControlList<Cities>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion



            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.Cities).Insert(new CreateCitiesDto
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
                BigCityIs = input.BigCityIs,
                Name = input.Name,
                Population_ = input.Population_,
                IsBigCity = input.IsBigCity,
                Description_ = input.Description_,
                CityTypeForm = input.CityTypeForm,
               

            });

            foreach (var item in input.SelectCityLines)
            {
                var queryLine = queryFactory.Query().From(Tables.CityLines).Insert(new CreateCityLinesDto
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
                    CityID = addedEntityId,
                    LineNr = item.LineNr,
                    DistrictName = item.DistrictName,
                    DistrictPopulation = item.DistrictPopulation,
                    NumberofTown = item.NumberofTown,
                    IsIncludeHospital = item.IsIncludeHospital,
                    Description_ = item.Description_,
                    DistrictInstruction = item.DistrictInstruction,


                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var city = queryFactory.Insert<SelectCitiesDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("CitiesChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Cities, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCitiesDto>(city);


        }
        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.Cities).Select("*").Where(new { Id = id }, false, false, "");

            var Cities = queryFactory.Get<SelectCitiesDto>(query);

            if (Cities.Id != Guid.Empty && Cities != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.Cities).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.CityLines).Delete(LoginedUserService.UserId).Where(new { CityID = id }, false, false, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var city = queryFactory.Update<SelectCitiesDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Cities, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectCitiesDto>(city);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.CityLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                var cityLines = queryFactory.Update<SelectCityLinesDto>(queryLine, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.CityLines, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectCityLinesDto>(cityLines);
            }


        }
        public async Task<IDataResult<SelectCitiesDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.Cities)
                   .Select("*").Where(new { Id = id }, false, false, "");

            var Cities = queryFactory.Get<SelectCitiesDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.CityLines)
                   .Select("*")
                    .Where(new { CityID = id }, false, false, "");

            var CityLine = queryFactory.GetList<SelectCityLinesDto>(queryLines).ToList();

            Cities.SelectCityLines = CityLine;

            LogsAppService.InsertLogToDatabase(Cities, Cities, LoginedUserService.UserId, Tables.Cities, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCitiesDto>(Cities);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListCitiesDto>>> GetListAsync(ListCitiesParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.Cities)
                   .Select("*")
                    .Where(null, false, false, "");

            var Cities = queryFactory.GetList<ListCitiesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListCitiesDto>>(Cities);

        }


        [ValidationAspect(typeof(UpdateCitiesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCitiesDto>> UpdateAsync(UpdateCitiesDto input)
        {

            var entityQuery = queryFactory
                .Query()
                .From(Tables.Cities)
                .Select("*")
                .Where(new { Id = input.Id }, false, false, "");

            var entity = queryFactory.Get<Cities>(entityQuery);


            var queryLines = queryFactory
                   .Query()
                   .From(Tables.CityLines)
                   .Select("*")
                    .Where(new { CityID = input.Id }, false, false, "");

            var CityLine = queryFactory.GetList<SelectCityLinesDto>(queryLines).ToList();

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.Cities).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.GetList<Cities>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion


            var query = queryFactory.Query().From(Tables.Cities).Update(new UpdateCitiesDto
            {
                Code = input.Code,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
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
                Population_ = input.Population_,
                IsBigCity = input.IsBigCity,
                SelectCityLines = input.SelectCityLines,
                CityTypeForm = input.CityTypeForm,
            }).Where(new { Id = input.Id }, false, false, "");


            foreach (var item in input.SelectCityLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.CityLines).Insert(new CreateCityLinesDto
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
                        CityID = input.Id,
                        Description_ = item.Description_,
                        DistrictName = item.DistrictName,
                        DistrictPopulation = item.DistrictPopulation,
                        NumberofTown = item.NumberofTown,
                        IsIncludeHospital = item.IsIncludeHospital,
                        DistrictInstruction = item.DistrictInstruction,

                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.CityLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                    var line = queryFactory.Get<SelectCityLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.CityLines).Update(new UpdateCityLinesDto
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
                            DistrictName = item.DistrictName,
                            DistrictPopulation = item.DistrictPopulation,
                            NumberofTown = item.NumberofTown,
                            IsIncludeHospital = item.IsIncludeHospital,
                            DistrictInstruction = item.DistrictInstruction,


                        }).Where(new { Id = line.Id }, false, false, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var Cities = queryFactory.Update<SelectCitiesDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, Cities, LoginedUserService.UserId, Tables.Cities, LogType.Update, entity.Id);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectCitiesDto>(Cities);
        }

        public async Task<IDataResult<SelectCitiesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.Cities).Select("*").Where(new { Id = id }, false, false, "");

            var entity = queryFactory.Get<Cities>(entityQuery);

            var query = queryFactory.Query().From(Tables.Cities).Update(new UpdateCitiesDto
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
                IsBigCity = entity.IsBigCity,
                Population_ = entity.Population_,
                CityTypeForm = (int)entity.CityTypeForm,




            }).Where(new { Id = id }, false, false, "");

            var CitiesDto = queryFactory.Update<SelectCitiesDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectCitiesDto>(CitiesDto);

        }


    }
}
