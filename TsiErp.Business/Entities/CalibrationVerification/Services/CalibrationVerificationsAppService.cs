using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.CalibrationVerification.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.CalibrationVerification;
using TsiErp.Entities.Entities.CalibrationVerification.Dtos;
using TsiErp.Entities.Entities.EquipmentRecord;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.CalibrationVerifications.Page;

namespace TsiErp.Business.Entities.CalibrationVerification.Services
{
    [ServiceRegistration(typeof(ICalibrationVerificationsAppService), DependencyInjectionType.Scoped)]
    public class CalibrationVerificationsAppService : ApplicationService<CalibrationVerificationsResource>, ICalibrationVerificationsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public CalibrationVerificationsAppService(IStringLocalizer<CalibrationVerificationsResource> l) : base(l)
        {
        }


        [ValidationAspect(typeof(CreateCalibrationVerifcationsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCalibrationVerificationsDto>> CreateAsync(CreateCalibrationVerificationsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.CalibrationVerifications).Select("*").Where(new { Code = input.Code }, false, false, "");

                var list = queryFactory.ControlList<CalibrationVerifications>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.CalibrationVerifications).Insert(new CreateCalibrationVerificationsDto
                {
                    Code = input.Code,
                    Date = input.Date,
                    EquipmentID = input.EquipmentID,
                    InfinitiveCertificateNo = input.InfinitiveCertificateNo,
                    ReceiptNo = input.ReceiptNo,
                    Result = input.Result,
                    NextControl = input.NextControl,
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
                    Name = input.Name
                });

                var calibrationVerifications = queryFactory.Insert<SelectCalibrationVerificationsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.CalibrationVerifications, LogType.Insert, calibrationVerifications.Id);

                return new SuccessDataResult<SelectCalibrationVerificationsDto>(calibrationVerifications);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.CalibrationVerifications).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var calibrationVerifications = queryFactory.Update<SelectCalibrationVerificationsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.CalibrationVerifications, LogType.Delete, id);

                return new SuccessDataResult<SelectCalibrationVerificationsDto>(calibrationVerifications);
            }
        }


        public async Task<IDataResult<SelectCalibrationVerificationsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                        .Query().From(Tables.CalibrationVerifications).Select<CalibrationVerifications>(c => new { c.Id, c.Code, c.Name, c.NextControl, c.ReceiptNo, c.Result, c.Date, c.EquipmentID })
                            .Join<EquipmentRecords>
                            (
                                e => new { Equipment = e.Code, EquipmentID = e.Id },
                                nameof(CalibrationVerifications.EquipmentID),
                                nameof(EquipmentRecords.Id),
                                JoinType.Left
                            )
                            .Where(new { Id = id }, false, false, Tables.CalibrationVerifications);

                var calibrationVerification = queryFactory.Get<SelectCalibrationVerificationsDto>(query);

                LogsAppService.InsertLogToDatabase(calibrationVerification, calibrationVerification, LoginedUserService.UserId, Tables.CalibrationVerifications, LogType.Get, id);

                return new SuccessDataResult<SelectCalibrationVerificationsDto>(calibrationVerification);

            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListCalibrationVerificationsDto>>> GetListAsync(ListCalibrationVerificationsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                        .Query()
                        .From(Tables.CalibrationVerifications)
                        .Select<CalibrationVerifications>(c => new { c.Id, c.Code, c.Name, c.NextControl, c.ReceiptNo, c.Result, c.Date, c.EquipmentID })
                            .Join<EquipmentRecords>
                            (
                                e => new { Equipment = e.Code },
                                nameof(CalibrationVerifications.EquipmentID),
                                nameof(EquipmentRecords.Id),
                                JoinType.Left
                            ).Where(null, false, false, Tables.CalibrationVerifications);


                var calibrationVerifications = queryFactory.GetList<ListCalibrationVerificationsDto>(query).ToList();
                return new SuccessDataResult<IList<ListCalibrationVerificationsDto>>(calibrationVerifications);
            }
        }


        [ValidationAspect(typeof(UpdateCalibrationVerificationsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCalibrationVerificationsDto>> UpdateAsync(UpdateCalibrationVerificationsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.CalibrationVerifications).Select("*").Where(new { Id = input.Id }, false, false, "");
                var entity = queryFactory.Get<CalibrationVerifications>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.CalibrationVerifications).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.GetList<CalibrationVerifications>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.CalibrationVerifications).Update(new UpdateCalibrationVerificationsDto
                {
                    Code = input.Code,
                    Name = input.Name,
                    EquipmentID = input.EquipmentID,
                    Date = input.Date,
                    Result = input.Result,
                    ReceiptNo = input.ReceiptNo,
                    InfinitiveCertificateNo = input.InfinitiveCertificateNo,
                    NextControl = input.NextControl,
                    Id = input.Id,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entity.DeleterId.Value,
                    DeletionTime = entity.DeletionTime.Value,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = input.Id }, false, false, "");

                var calibrationVerifications = queryFactory.Update<SelectCalibrationVerificationsDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, calibrationVerifications, LoginedUserService.UserId, Tables.CalibrationVerifications, LogType.Update, entity.Id);


                return new SuccessDataResult<SelectCalibrationVerificationsDto>(calibrationVerifications);
            }
        }

        public async Task<IDataResult<SelectCalibrationVerificationsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.CalibrationVerifications).Select("*").Where(new { Id = id }, false, false, "");
                var entity = queryFactory.Get<CalibrationVerifications>(entityQuery);

                var query = queryFactory.Query().From(Tables.CalibrationVerifications).Update(new UpdateCalibrationVerificationsDto
                {
                    Code = entity.Code,
                    Name = entity.Name,
                    NextControl = entity.NextControl,
                    InfinitiveCertificateNo = entity.InfinitiveCertificateNo,
                    ReceiptNo = entity.ReceiptNo,
                    EquipmentID = entity.EquipmentID,
                    Result = entity.Result,
                    Date = entity.Date,
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

                var calibrationVerifications = queryFactory.Update<SelectCalibrationVerificationsDto>(query, "Id", true);

                return new SuccessDataResult<SelectCalibrationVerificationsDto>(calibrationVerifications);

            }
        }
    }
}
