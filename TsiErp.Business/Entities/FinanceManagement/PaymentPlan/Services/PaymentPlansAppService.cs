using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.PaymentPlan.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PaymentPlans.Page;

namespace TsiErp.Business.Entities.PaymentPlan.Services
{
    [ServiceRegistration(typeof(IPaymentPlansAppService), DependencyInjectionType.Scoped)]
    public class PaymentPlansAppService : ApplicationService<PaymentPlansResource>, IPaymentPlansAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public PaymentPlansAppService(IStringLocalizer<PaymentPlansResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }


        [ValidationAspect(typeof(CreatePaymentPlansValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPaymentPlansDto>> CreateAsync(CreatePaymentPlansDto input)
        {

            var listQuery = queryFactory.Query().From(Tables.PaymentPlans).Select("*").Where(new { Code = input.Code }, "");

            var list = queryFactory.ControlList<PaymentPlans>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();


            var query = queryFactory.Query().From(Tables.PaymentPlans).Insert(new CreatePaymentPlansDto
            {
                Code = input.Code,
                DelayMaturityDifference = input.DelayMaturityDifference,
                Days_ = input.Days_,
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
                Name = input.Name
            });

            var paymentPlans = queryFactory.Insert<SelectPaymentPlansDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("PaymentPlansChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PaymentPlans, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPaymentPlansDto>(paymentPlans);


        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {

            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("PaymentPlanID", new List<string>
            {
                Tables.PurchaseOrderLines,
                Tables.PurchaseOrders,
                Tables.PurchaseRequestLines,
                Tables.PurchaseRequests,
                Tables.SalesOrderLines,
                Tables.SalesOrders,
                Tables.SalesPropositionLines,
                Tables.SalesPropositions
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var query = queryFactory.Query().From(Tables.PaymentPlans).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var paymentPlans = queryFactory.Update<SelectPaymentPlansDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PaymentPlans, LogType.Delete, id);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectPaymentPlansDto>(paymentPlans);
            }
        }


        public async Task<IDataResult<SelectPaymentPlansDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.PaymentPlans).Select("*").Where(
            new
            {
                Id = id
            }, "");
            var paymentPlan = queryFactory.Get<SelectPaymentPlansDto>(query);


            LogsAppService.InsertLogToDatabase(paymentPlan, paymentPlan, LoginedUserService.UserId, Tables.PaymentPlans, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPaymentPlansDto>(paymentPlan);

        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPaymentPlansDto>>> GetListAsync(ListPaymentPlansParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.PaymentPlans).Select("*").Where(null, "");
            var paymentPlans = queryFactory.GetList<ListPaymentPlansDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListPaymentPlansDto>>(paymentPlans);

        }


        [ValidationAspect(typeof(UpdatePaymentPlansValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPaymentPlansDto>> UpdateAsync(UpdatePaymentPlansDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.PaymentPlans).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<PaymentPlans>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.PaymentPlans).Select("*").Where(new { Code = input.Code }, "");
            var list = queryFactory.GetList<PaymentPlans>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
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

            var paymentPlans = queryFactory.Update<SelectPaymentPlansDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, paymentPlans, LoginedUserService.UserId, Tables.PaymentPlans, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPaymentPlansDto>(paymentPlans);

        }

        public async Task<IDataResult<SelectPaymentPlansDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.PaymentPlans).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<PaymentPlans>(entityQuery);

            var query = queryFactory.Query().From(Tables.PaymentPlans).Update(new UpdatePaymentPlansDto
            {
                Code = entity.Code,
                Days_ = entity.Days_,
                DelayMaturityDifference = entity.DelayMaturityDifference,
                Name = entity.Name,
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

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var paymentPlans = queryFactory.Update<SelectPaymentPlansDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectPaymentPlansDto>(paymentPlans);


        }
    }
}
