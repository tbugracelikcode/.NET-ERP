using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.PaymentPlan.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.PaymentPlan;
using TsiErp.Entities.Entities.PaymentPlan.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PaymentPlans.Page;

namespace TsiErp.Business.Entities.PaymentPlan.Services
{
    [ServiceRegistration(typeof(IPaymentPlansAppService), DependencyInjectionType.Scoped)]
    public class PaymentPlansAppService : ApplicationService<PaymentPlansResource>, IPaymentPlansAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public PaymentPlansAppService(IStringLocalizer<PaymentPlansResource> l) : base(l)
        {
        }


        [ValidationAspect(typeof(CreatePaymentPlansValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPaymentPlansDto>> CreateAsync(CreatePaymentPlansDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.PaymentPlans).Select("*").Where(new { Code = input.Code }, false, false, "");

                var list = queryFactory.ControlList<PaymentPlans>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.PaymentPlans).Insert(new CreatePaymentPlansDto
                {
                    Code = input.Code,
                    DelayMaturityDifference = input.DelayMaturityDifference,
                    Days_ = input.Days_,
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = GuidGenerator.CreateGuid(),
                    IsActive = true,
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    Name = input.Name
                });

                var paymentPlans = queryFactory.Insert<SelectPaymentPlansDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PaymentPlans, LogType.Insert, paymentPlans.Id);

                return new SuccessDataResult<SelectPaymentPlansDto>(paymentPlans);
            }

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.PaymentPlans).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                var paymentPlans = queryFactory.Update<SelectPaymentPlansDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PaymentPlans, LogType.Delete, id);

                return new SuccessDataResult<SelectPaymentPlansDto>(paymentPlans);
            }
        }


        public async Task<IDataResult<SelectPaymentPlansDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.PaymentPlans).Select("*").Where(
                new
                {
                    Id = id
                }, true, true, "");
                var paymentPlan = queryFactory.Get<SelectPaymentPlansDto>(query);


                LogsAppService.InsertLogToDatabase(paymentPlan, paymentPlan, LoginedUserService.UserId, Tables.PaymentPlans, LogType.Get, id);

                return new SuccessDataResult<SelectPaymentPlansDto>(paymentPlan);

            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPaymentPlansDto>>> GetListAsync(ListPaymentPlansParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.PaymentPlans).Select("*").Where(null, true, true, "");
                var paymentPlans = queryFactory.GetList<ListPaymentPlansDto>(query).ToList();
                return new SuccessDataResult<IList<ListPaymentPlansDto>>(paymentPlans);
            }
        }


        [ValidationAspect(typeof(UpdatePaymentPlansValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPaymentPlansDto>> UpdateAsync(UpdatePaymentPlansDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.PaymentPlans).Select("*").Where(new { Id = input.Id }, true, true, "");
                var entity = queryFactory.Get<PaymentPlans>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.PaymentPlans).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.GetList<PaymentPlans>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.PaymentPlans).Update(new UpdatePaymentPlansDto
                {
                    Code = input.Code,
                    Name = input.Name,
                    DelayMaturityDifference = input.DelayMaturityDifference,
                    Days_ = input.Days_,
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

                var paymentPlans = queryFactory.Update<SelectPaymentPlansDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, paymentPlans, LoginedUserService.UserId, Tables.PaymentPlans, LogType.Update, entity.Id);

                return new SuccessDataResult<SelectPaymentPlansDto>(paymentPlans);
            }
        }

        public async Task<IDataResult<SelectPaymentPlansDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.PaymentPlans).Select("*").Where(new { Id = id }, true, true, "");

                var entity = queryFactory.Get<PaymentPlans>(entityQuery);

                var query = queryFactory.Query().From(Tables.PaymentPlans).Update(new UpdatePaymentPlansDto
                {
                    Code = entity.Code,
                    Days_ = entity.Days_,
                    DelayMaturityDifference = entity.DelayMaturityDifference,
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

                var paymentPlans = queryFactory.Update<SelectPaymentPlansDto>(query, "Id", true);
                return new SuccessDataResult<SelectPaymentPlansDto>(paymentPlans);

            }
        }
    }
}
