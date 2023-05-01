using Microsoft.Extensions.Localization;
using System.Threading.Channels;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Period.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.BillsofMaterial;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Period;
using TsiErp.Entities.Entities.Period.Dtos;
using TsiErp.Entities.Entities.WareHouse;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Periods.Page;

namespace TsiErp.Business.Entities.Period.Services
{
    [ServiceRegistration(typeof(IPeriodsAppService), DependencyInjectionType.Scoped)]
    public class PeriodsAppService : ApplicationService<PeriodsResource>, IPeriodsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public PeriodsAppService(IStringLocalizer<PeriodsResource> l) : base(l)
        {
        }

        [ValidationAspect(typeof(CreatePeriodsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPeriodsDto>> CreateAsync(CreatePeriodsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.Periods).Select("*").Where(new { Code = input.Code }, false, false, "");

                var list = queryFactory.ControlList<Periods>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.Periods).Insert(new CreatePeriodsDto
                {
                    Code = input.Code,
                    BranchID = input.BranchID,
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Description_ = input.Description_,
                    Id = GuidGenerator.CreateGuid(),
                    IsActive = true,
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    Name = input.Name
                });

                var periods = queryFactory.Insert<SelectPeriodsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Periods, LogType.Insert, periods.Id);

                return new SuccessDataResult<SelectPeriodsDto>(periods);
            }

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.Periods).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                var periods = queryFactory.Update<SelectPeriodsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Periods, LogType.Delete, id);

                return new SuccessDataResult<SelectPeriodsDto>(periods);
            }
        }

        public async Task<IDataResult<SelectPeriodsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                        .Query().From(Tables.Periods).Select<Periods>(p => new { p.Id, p.Code, p.Name, p.IsActive })
                            .Join<Branches>
                            (
                                b => new { BranchName = b.Name, BranchID = b.Id },
                                nameof(Periods.BranchID),
                                bc => new { bc.Id },
                                JoinType.Left
                            )
                            .Where(new { Id = id }, true, true, Tables.Periods);

                var period = queryFactory.Get<SelectPeriodsDto>(query);

                LogsAppService.InsertLogToDatabase(period, period, LoginedUserService.UserId, Tables.Periods, LogType.Get, id);

                return new SuccessDataResult<SelectPeriodsDto>(period);

            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPeriodsDto>>> GetListAsync(ListPeriodsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                //var query = queryFactory.Query().From(Tables.Periods).Select("*").Where(null, true, true, "");

                var query = queryFactory
                        .Query()
                        .From(Tables.Periods)
                        .Select<Periods>(p => new { p.Id, p.Code, p.Name, p.IsActive, p.Description_ })
                            .Join<Branches>
                            (
                                b => new { BranchName = b.Name },
                                nameof(Periods.BranchID),
                                bc => new { bc.Id },
                                JoinType.Left
                            ).Where(null, true, true, Tables.Periods);


                var periods = queryFactory.GetList<ListPeriodsDto>(query).ToList();
                return new SuccessDataResult<IList<ListPeriodsDto>>(periods);
            }
        }

        [ValidationAspect(typeof(UpdatePeriodsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPeriodsDto>> UpdateAsync(UpdatePeriodsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.Periods).Select("*").Where(new { Id = input.Id }, true, true, "");
                var entity = queryFactory.Get<Periods>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.Periods).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.GetList<Periods>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.Periods).Update(new UpdatePeriodsDto
                {
                    Code = input.Code,
                    Description_ = input.Description_,
                    Name = input.Name,
                    Id = input.Id,
                    IsActive = input.IsActive,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entity.DeleterId.Value,
                    DeletionTime = entity.DeletionTime.Value,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    BranchID = input.BranchID,
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = input.Id }, true, true, "");

                var periods = queryFactory.Update<SelectPeriodsDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, periods, LoginedUserService.UserId, Tables.Periods, LogType.Update, entity.Id);


                return new SuccessDataResult<SelectPeriodsDto>(periods);
            }
        }

        public async Task<IDataResult<SelectPeriodsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {

            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.Periods).Select("*").Where(new { Id = id }, true, true, "");
                var entity = queryFactory.Get<Periods>(entityQuery);

                var query = queryFactory.Query().From(Tables.Periods).Update(new UpdatePeriodsDto
                {
                    Code = entity.Code,
                    Description_ = entity.Description_,
                    Name = entity.Name,
                    IsActive = entity.IsActive,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = userId,
                    Id = id,
                    BranchID = entity.BranchID,
                    DataOpenStatus = lockRow,
                    DataOpenStatusUserId = userId,

                }).Where(new { Id = id }, true, true, "");

                var periods = queryFactory.Update<SelectPeriodsDto>(query, "Id", true);

                return new SuccessDataResult<SelectPeriodsDto>(periods);

            }
        }
    }
}
