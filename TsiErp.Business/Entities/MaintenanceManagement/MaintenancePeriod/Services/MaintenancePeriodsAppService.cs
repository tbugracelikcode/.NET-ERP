using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.MaintenancePeriod.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenancePeriod;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenancePeriod.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.MaintenancePeriods.Page;

namespace TsiErp.Business.Entities.MaintenancePeriod.Services
{
    [ServiceRegistration(typeof(IMaintenancePeriodsAppService), DependencyInjectionType.Scoped)]
    public class MaintenancePeriodsAppService : ApplicationService<MaintenancePeriodsResource>, IMaintenancePeriodsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public MaintenancePeriodsAppService(IStringLocalizer<MaintenancePeriodsResource> l) : base(l)
        {
        }

        [ValidationAspect(typeof(CreateMaintenancePeriodsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMaintenancePeriodsDto>> CreateAsync(CreateMaintenancePeriodsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.MaintenancePeriods).Select("*").Where(new { Code = input.Code }, false, false, "");

                var list = queryFactory.ControlList<MaintenancePeriods>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion


                var query = queryFactory.Query().From(Tables.MaintenancePeriods).Insert(new CreateMaintenancePeriodsDto
                {
                    Code = input.Code,
                    IsDaily = input.IsDaily,
                    PeriodTime = input.PeriodTime,
                    Description_ = input.Description_,
                    Name = input.Name,
                    IsActive = true,
                    Id = GuidGenerator.CreateGuid(),
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    IsDeleted = false
                });


                var maintenancePeriods = queryFactory.Insert<SelectMaintenancePeriodsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.MaintenancePeriods, LogType.Insert, maintenancePeriods.Id);


                return new SuccessDataResult<SelectMaintenancePeriodsDto>(maintenancePeriods);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.MaintenancePeriods).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                var maintenancePeriods = queryFactory.Update<SelectMaintenancePeriodsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.MaintenancePeriods, LogType.Delete, id);

                return new SuccessDataResult<SelectMaintenancePeriodsDto>(maintenancePeriods);
            }
        }


        public async Task<IDataResult<SelectMaintenancePeriodsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.Branches).Select("*").Where(
                new
                {
                    Id = id
                }, true, true, "");
                var maintenancePeriod = queryFactory.Get<SelectMaintenancePeriodsDto>(query);


                LogsAppService.InsertLogToDatabase(maintenancePeriod, maintenancePeriod, LoginedUserService.UserId, Tables.MaintenancePeriods, LogType.Get, id);

                return new SuccessDataResult<SelectMaintenancePeriodsDto>(maintenancePeriod);

            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListMaintenancePeriodsDto>>> GetListAsync(ListMaintenancePeriodsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.MaintenancePeriods).Select("*").Where(null, true, true, "");
                var maintenancePeriods = queryFactory.GetList<ListMaintenancePeriodsDto>(query).ToList();
                return new SuccessDataResult<IList<ListMaintenancePeriodsDto>>(maintenancePeriods);
            }
        }


        [ValidationAspect(typeof(UpdateMaintenancePeriodsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMaintenancePeriodsDto>> UpdateAsync(UpdateMaintenancePeriodsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.MaintenancePeriods).Select("*").Where(new { Id = input.Id }, true, true, "");
                var entity = queryFactory.Get<MaintenancePeriods>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.MaintenancePeriods).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.GetList<MaintenancePeriods>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.MaintenancePeriods).Update(new UpdateMaintenancePeriodsDto
                {
                    Code = input.Code,
                    PeriodTime = input.PeriodTime,
                    IsDaily = input.IsDaily,
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
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = input.Id }, true, true, "");

                var maintenancePeriods = queryFactory.Update<SelectMaintenancePeriodsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, maintenancePeriods, LoginedUserService.UserId, Tables.MaintenancePeriods, LogType.Update, entity.Id);

                return new SuccessDataResult<SelectMaintenancePeriodsDto>(maintenancePeriods);
            }
        }

        public async Task<IDataResult<SelectMaintenancePeriodsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.MaintenancePeriods).Select("*").Where(new { Id = id }, true, true, "");

                var entity = queryFactory.Get<MaintenancePeriods>(entityQuery);

                var query = queryFactory.Query().From(Tables.MaintenancePeriods).Update(new UpdateMaintenancePeriodsDto
                {
                    Code = entity.Code,
                    IsDaily = entity.IsDaily,
                    PeriodTime = entity.PeriodTime,
                    Description_ = entity.Description_,
                    Name = entity.Name,
                    IsActive = entity.IsActive,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                    LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                    Id = id,
                    DataOpenStatus = lockRow,
                    DataOpenStatusUserId = userId

                }).Where(new { Id = id }, true, true, "");

                var maintenancePeriods = queryFactory.Update<SelectMaintenancePeriodsDto>(query, "Id", true);
                return new SuccessDataResult<SelectMaintenancePeriodsDto>(maintenancePeriods);

            }
        }
    }
}
