using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.CalibrationRecord.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.QualityControl.CalibrationRecord;
using TsiErp.Entities.Entities.QualityControl.CalibrationRecord.Dtos;
using TsiErp.Entities.Entities.QualityControl.EquipmentRecord;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.CalibrationRecords.Page;

namespace TsiErp.Business.Entities.CalibrationRecord.Services
{
    [ServiceRegistration(typeof(ICalibrationRecordsAppService), DependencyInjectionType.Scoped)]
    public class CalibrationRecordsAppService : ApplicationService<CalibrationRecordsResource>, ICalibrationRecordsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public CalibrationRecordsAppService(IStringLocalizer<CalibrationRecordsResource> l) : base(l)
        {
        }


        [ValidationAspect(typeof(CreateCalibrationRecordsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCalibrationRecordsDto>> CreateAsync(CreateCalibrationRecordsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.CalibrationRecords).Select("*").Where(new { Code = input.Code }, false, false, "");

                var list = queryFactory.ControlList<CalibrationRecords>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();

                var query = queryFactory.Query().From(Tables.CalibrationRecords).Insert(new CreateCalibrationRecordsDto
                {
                    Code = input.Code,
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    Date = input.Date,
                    EquipmentID = input.EquipmentID,
                    InfinitiveCertificateNo = input.InfinitiveCertificateNo,
                    NextControl = input.NextControl,
                    ReceiptNo = input.ReceiptNo,
                    Result = input.Result,
                    DeletionTime = null,
                    Id = addedEntityId,
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    Name = input.Name
                });

                var calibrationRecords = queryFactory.Insert<SelectCalibrationRecordsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.CalibrationRecords, LogType.Insert, addedEntityId);

                return new SuccessDataResult<SelectCalibrationRecordsDto>(calibrationRecords);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.CalibrationRecords).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var calibrationRecords = queryFactory.Update<SelectCalibrationRecordsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.CalibrationRecords, LogType.Delete, id);

                return new SuccessDataResult<SelectCalibrationRecordsDto>(calibrationRecords);
            }
        }


        public async Task<IDataResult<SelectCalibrationRecordsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                        .Query().From(Tables.CalibrationRecords).Select<CalibrationRecords>(c => new { c.Id, c.Code, c.Name, c.NextControl, c.ReceiptNo, c.Result, c.Date, c.EquipmentID, c.DataOpenStatus, c.DataOpenStatusUserId, c.InfinitiveCertificateNo })
                            .Join<EquipmentRecords>
                            (
                                e => new { Equipment = e.Code, EquipmentID = e.Id },
                                nameof(CalibrationRecords.EquipmentID),
                                nameof(EquipmentRecords.Id),
                                JoinType.Left
                            )
                            .Where(new { Id = id }, false, false, Tables.CalibrationRecords);

                var calibrationRecord = queryFactory.Get<SelectCalibrationRecordsDto>(query);

                LogsAppService.InsertLogToDatabase(calibrationRecord, calibrationRecord, LoginedUserService.UserId, Tables.CalibrationRecords, LogType.Get, id);

                return new SuccessDataResult<SelectCalibrationRecordsDto>(calibrationRecord);

            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListCalibrationRecordsDto>>> GetListAsync(ListCalibrationRecordsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                        .Query()
                        .From(Tables.CalibrationRecords)
                        .Select<CalibrationRecords>(c => new { c.Id, c.Code, c.Name, c.NextControl, c.ReceiptNo, c.Result, c.Date, c.EquipmentID })
                            .Join<EquipmentRecords>
                            (
                                e => new { Equipment = e.Code },
                                nameof(CalibrationRecords.EquipmentID),
                                nameof(EquipmentRecords.Id),
                                JoinType.Left
                            ).Where(null, false, false, Tables.CalibrationRecords);


                var calibrationRecords = queryFactory.GetList<ListCalibrationRecordsDto>(query).ToList();
                return new SuccessDataResult<IList<ListCalibrationRecordsDto>>(calibrationRecords);
            }
        }


        [ValidationAspect(typeof(UpdateCalibrationRecordsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCalibrationRecordsDto>> UpdateAsync(UpdateCalibrationRecordsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.CalibrationRecords).Select("*").Where(new { Id = input.Id }, false, false, "");
                var entity = queryFactory.Get<CalibrationRecords>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.CalibrationRecords).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.GetList<CalibrationRecords>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.CalibrationRecords).Update(new UpdateCalibrationRecordsDto
                {
                    Code = input.Code,
                    Name = input.Name,
                    Id = input.Id,
                    Result = input.Result,
                    ReceiptNo = input.ReceiptNo,
                    NextControl = input.NextControl,
                    InfinitiveCertificateNo = input.InfinitiveCertificateNo,
                    EquipmentID = input.EquipmentID,
                    Date = input.Date,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = input.Id }, false, false, "");

                var calibrationRecords = queryFactory.Update<SelectCalibrationRecordsDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, calibrationRecords, LoginedUserService.UserId, Tables.CalibrationRecords, LogType.Update, entity.Id);


                return new SuccessDataResult<SelectCalibrationRecordsDto>(calibrationRecords);
            }
        }

        public async Task<IDataResult<SelectCalibrationRecordsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.CalibrationRecords).Select("*").Where(new { Id = id }, false, false, "");
                var entity = queryFactory.Get<CalibrationRecords>(entityQuery);

                var query = queryFactory.Query().From(Tables.CalibrationRecords).Update(new UpdateCalibrationRecordsDto
                {
                    Code = entity.Code,
                    Name = entity.Name,
                    Date = entity.Date,
                    EquipmentID = entity.EquipmentID,
                    InfinitiveCertificateNo = entity.InfinitiveCertificateNo,
                    ReceiptNo = entity.ReceiptNo,
                    Result = entity.Result,
                    NextControl = entity.NextControl,
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

                }).Where(new { Id = id }, false, false, "");

                var calibrationRecords = queryFactory.Update<SelectCalibrationRecordsDto>(query, "Id", true);

                return new SuccessDataResult<SelectCalibrationRecordsDto>(calibrationRecords);

            }
        }
    }
}
