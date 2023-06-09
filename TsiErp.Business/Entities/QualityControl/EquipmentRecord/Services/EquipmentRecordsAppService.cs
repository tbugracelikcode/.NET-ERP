using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.EquipmentRecord.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department;
using TsiErp.Entities.Entities.QualityControl.EquipmentRecord;
using TsiErp.Entities.Entities.QualityControl.EquipmentRecord.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.EquipmentRecords.Page;

namespace TsiErp.Business.Entities.EquipmentRecord.Services
{
    [ServiceRegistration(typeof(IEquipmentRecordsAppService), DependencyInjectionType.Scoped)]
    public class EquipmentRecordsAppService : ApplicationService<EquipmentRecordsResource>, IEquipmentRecordsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public EquipmentRecordsAppService(IStringLocalizer<EquipmentRecordsResource> l) : base(l)
        {
        }



        [ValidationAspect(typeof(CreateEquipmentRecorsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEquipmentRecordsDto>> CreateAsync(CreateEquipmentRecordsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.EquipmentRecords).Select("*").Where(new { Code = input.Code }, false, false, "");

                var list = queryFactory.ControlList<EquipmentRecords>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();


                var query = queryFactory.Query().From(Tables.EquipmentRecords).Insert(new CreateEquipmentRecordsDto
                {
                    Code = input.Code,
                    Department = input.Department,
                    Cancel = input.Cancel,
                    CancellationDate = input.CancellationDate,
                    CancellationReason = input.CancellationReason,
                    EquipmentSerialNo = input.EquipmentSerialNo,
                    Frequency = input.Frequency,
                    MeasuringAccuracy = input.MeasuringAccuracy,
                    MeasuringRange = input.MeasuringRange,
                    RecordDate = input.RecordDate,
                    Responsible = input.Responsible,
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
                    Name = input.Name
                });

                var equipmentRecords = queryFactory.Insert<SelectEquipmentRecordsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.EquipmentRecords, LogType.Insert, addedEntityId);

                return new SuccessDataResult<SelectEquipmentRecordsDto>(equipmentRecords);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.EquipmentRecords).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                var equipmentRecords = queryFactory.Update<SelectEquipmentRecordsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.EquipmentRecords, LogType.Delete, id);

                return new SuccessDataResult<SelectEquipmentRecordsDto>(equipmentRecords);
            }
        }


        public async Task<IDataResult<SelectEquipmentRecordsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                        .Query().From(Tables.EquipmentRecords).Select<EquipmentRecords>(e => new { e.Cancel, e.CancellationDate, e.CancellationReason, e.RecordDate, e.Code, e.DataOpenStatus, e.DataOpenStatusUserId, e.Department, e.EquipmentSerialNo, e.Frequency, e.MeasuringAccuracy, e.MeasuringRange, e.Name, e.Responsible, e.IsActive, e.Id })
                            .Join<Departments>
                            (
                                d => new { DepartmentName = d.Name, Department = d.Id },
                                nameof(EquipmentRecords.Department),
                                nameof(Departments.Id),
                                JoinType.Left
                            )
                            .Where(new { Id = id }, true, true, Tables.EquipmentRecords);

                var equipmentRecord = queryFactory.Get<SelectEquipmentRecordsDto>(query);

                LogsAppService.InsertLogToDatabase(equipmentRecord, equipmentRecord, LoginedUserService.UserId, Tables.EquipmentRecords, LogType.Get, id);

                return new SuccessDataResult<SelectEquipmentRecordsDto>(equipmentRecord);

            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListEquipmentRecordsDto>>> GetListAsync(ListEquipmentRecordsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory
                   .Query()
                   .From(Tables.EquipmentRecords)
                   .Select<EquipmentRecords>(e => new { e.Cancel, e.CancellationDate, e.CancellationReason, e.RecordDate, e.Code, e.DataOpenStatus, e.DataOpenStatusUserId, e.Department, e.EquipmentSerialNo, e.Frequency, e.MeasuringAccuracy, e.MeasuringRange, e.Name, e.Responsible, e.IsActive, e.Id })
                       .Join<Departments>
                       (
                           d => new { DepartmentName = d.Name },
                           nameof(EquipmentRecords.Department),
                           nameof(Departments.Id),
                           JoinType.Left
                       ).Where(null, true, true, Tables.EquipmentRecords);

                var equipmentRecords = queryFactory.GetList<ListEquipmentRecordsDto>(query).ToList();

                return new SuccessDataResult<IList<ListEquipmentRecordsDto>>(equipmentRecords);
            }
        }


        [ValidationAspect(typeof(UpdateEquipmentRecordsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEquipmentRecordsDto>> UpdateAsync(UpdateEquipmentRecordsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.EquipmentRecords).Select("*").Where(new { Id = input.Id }, true, true, "");
                var entity = queryFactory.Get<EquipmentRecords>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.EquipmentRecords).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.GetList<EquipmentRecords>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.EquipmentRecords).Update(new UpdateEquipmentRecordsDto
                {
                    Code = input.Code,
                    Name = input.Name,
                    Id = input.Id,
                    IsActive = input.IsActive,
                    Responsible = input.Responsible,
                    MeasuringRange = input.MeasuringRange,
                    MeasuringAccuracy = input.MeasuringAccuracy,
                    Frequency = input.Frequency,
                    Cancel = input.Cancel,
                    CancellationDate = input.CancellationDate,
                    CancellationReason = input.CancellationReason,
                    Department = input.Department,
                    EquipmentSerialNo = input.EquipmentSerialNo,
                    RecordDate = input.RecordDate,
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

                var equipmentRecords = queryFactory.Update<SelectEquipmentRecordsDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, equipmentRecords, LoginedUserService.UserId, Tables.EquipmentRecords, LogType.Update, entity.Id);


                return new SuccessDataResult<SelectEquipmentRecordsDto>(equipmentRecords);
            }
        }

        public async Task<IDataResult<SelectEquipmentRecordsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.EquipmentRecords).Select("*").Where(new { Id = id }, true, true, "");
                var entity = queryFactory.Get<EquipmentRecords>(entityQuery);

                var query = queryFactory.Query().From(Tables.EquipmentRecords).Update(new UpdateEquipmentRecordsDto
                {
                    Code = entity.Code,
                    Name = entity.Name,
                    IsActive = entity.IsActive,
                    Responsible = entity.Responsible,
                    MeasuringRange = entity.MeasuringRange,
                    MeasuringAccuracy = entity.MeasuringAccuracy,
                    Frequency = entity.Frequency,
                    Cancel = entity.Cancel,
                    CancellationDate = entity.CancellationDate,
                    CancellationReason = entity.CancellationReason,
                    Department = entity.Department,
                    EquipmentSerialNo = entity.EquipmentSerialNo,
                    RecordDate = entity.RecordDate,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                    LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                    Id = id,
                    DataOpenStatus = lockRow,
                    DataOpenStatusUserId = userId,

                }).Where(new { Id = id }, true, true, "");

                var equipmentRecords = queryFactory.Update<SelectEquipmentRecordsDto>(query, "Id", true);

                return new SuccessDataResult<SelectEquipmentRecordsDto>(equipmentRecords);

            }
        }
    }
}
