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
using TsiErp.Business.Entities.CalibrationRecord.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
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

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public CalibrationRecordsAppService(IStringLocalizer<CalibrationRecordsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }


        [ValidationAspect(typeof(CreateCalibrationRecordsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCalibrationRecordsDto>> CreateAsync(CreateCalibrationRecordsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.CalibrationRecords).Select("*").Where(new { Code = input.Code }, "");

            var list = queryFactory.ControlList<CalibrationRecords>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.CalibrationRecords).Insert(new CreateCalibrationRecordsDto
            {
                Code = input.Code,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                Date_ = input.Date_,
                EquipmentID = input.EquipmentID.GetValueOrDefault(),
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

            await FicheNumbersAppService.UpdateFicheNumberAsync("CalRecordsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.CalibrationRecords, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCalibrationRecordsDto>(calibrationRecords);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.CalibrationRecords).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

            var calibrationRecords = queryFactory.Update<SelectCalibrationRecordsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.CalibrationRecords, LogType.Delete, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCalibrationRecordsDto>(calibrationRecords);

        }


        public async Task<IDataResult<SelectCalibrationRecordsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.CalibrationRecords).Select<CalibrationRecords>(null)
                        .Join<EquipmentRecords>
                        (
                            e => new { Equipment = e.Code, EquipmentID = e.Id },
                            nameof(CalibrationRecords.EquipmentID),
                            nameof(EquipmentRecords.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, Tables.CalibrationRecords);

            var calibrationRecord = queryFactory.Get<SelectCalibrationRecordsDto>(query);

            LogsAppService.InsertLogToDatabase(calibrationRecord, calibrationRecord, LoginedUserService.UserId, Tables.CalibrationRecords, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCalibrationRecordsDto>(calibrationRecord);

        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListCalibrationRecordsDto>>> GetListAsync(ListCalibrationRecordsParameterDto input)
        {
            var query = queryFactory
                    .Query()
                    .From(Tables.CalibrationRecords)
                    .Select<CalibrationRecords>(null)
                        .Join<EquipmentRecords>
                        (
                            e => new { Equipment = e.Code },
                            nameof(CalibrationRecords.EquipmentID),
                            nameof(EquipmentRecords.Id),
                            JoinType.Left
                        ).Where(null,  Tables.CalibrationRecords);


            var calibrationRecords = queryFactory.GetList<ListCalibrationRecordsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListCalibrationRecordsDto>>(calibrationRecords);

        }


        [ValidationAspect(typeof(UpdateCalibrationRecordsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCalibrationRecordsDto>> UpdateAsync(UpdateCalibrationRecordsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.CalibrationRecords).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<CalibrationRecords>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.CalibrationRecords).Select("*").Where(new { Code = input.Code }, "");
            var list = queryFactory.GetList<CalibrationRecords>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
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
                EquipmentID = input.EquipmentID.GetValueOrDefault(),
                Date_ = input.Date_,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id }, "");

            var calibrationRecords = queryFactory.Update<SelectCalibrationRecordsDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, calibrationRecords, LoginedUserService.UserId, Tables.CalibrationRecords, LogType.Update, entity.Id);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectCalibrationRecordsDto>(calibrationRecords);

        }

        public async Task<IDataResult<SelectCalibrationRecordsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.CalibrationRecords).Select("*").Where(new { Id = id }, "");
            var entity = queryFactory.Get<CalibrationRecords>(entityQuery);

            var query = queryFactory.Query().From(Tables.CalibrationRecords).Update(new UpdateCalibrationRecordsDto
            {
                Code = entity.Code,
                Name = entity.Name,
                Date_ = entity.Date_,
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

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var calibrationRecords = queryFactory.Update<SelectCalibrationRecordsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCalibrationRecordsDto>(calibrationRecords);


        }
    }
}
