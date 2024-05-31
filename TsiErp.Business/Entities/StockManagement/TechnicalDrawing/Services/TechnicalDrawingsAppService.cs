using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.TechnicalDrawing.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.TechnicalDrawings.Page;

namespace TsiErp.Business.Entities.TechnicalDrawing.Services
{
    [ServiceRegistration(typeof(ITechnicalDrawingsAppService), DependencyInjectionType.Scoped)]
    public class TechnicalDrawingsAppService : ApplicationService<TechnicalDrawingsResource>, ITechnicalDrawingsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public TechnicalDrawingsAppService(IStringLocalizer<TechnicalDrawingsResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }

        [ValidationAspect(typeof(CreateTechnicalDrawingsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectTechnicalDrawingsDto>> CreateAsync(CreateTechnicalDrawingsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.TechnicalDrawings).Select("*").Where(new { RevisionNo = input.RevisionNo }, false, false, "");

            var list = queryFactory.ControlList<TechnicalDrawings>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.TechnicalDrawings).Insert(new CreateTechnicalDrawingsDto
            {
                CustomerApproval = input.CustomerApproval,
                RevisionNo = input.RevisionNo,
                RevisionDate = input.RevisionDate,
                ProductID = input.ProductID.GetValueOrDefault(),
                CustomerCurrentAccountCardID = input.CustomerCurrentAccountCardID.GetValueOrDefault(),
                Drawer = input.Drawer,
                DrawingDomain = input.DrawingDomain,
                DrawingFilePath = input.DrawingFilePath,
                DrawingNo = input.DrawingNo,
                IsApproved = input.IsApproved,
                SampleApproval = input.SampleApproval,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Description_ = input.Description_,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
            });

            var technicalDrawings = queryFactory.Insert<SelectTechnicalDrawingsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.TechnicalDrawings, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectTechnicalDrawingsDto>(technicalDrawings);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("TechnicalDrawingID", new List<string>
            {
                Tables.Report8Ds
            });


            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var query = queryFactory.Query().From(Tables.TechnicalDrawings).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var technicalDrawings = queryFactory.Update<SelectTechnicalDrawingsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.TechnicalDrawings, LogType.Delete, id);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectTechnicalDrawingsDto>(technicalDrawings);
            }
        }


        public async Task<IDataResult<SelectTechnicalDrawingsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.TechnicalDrawings).Select<TechnicalDrawings>(null)
                        .Join<Products>
                        (
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                            nameof(TechnicalDrawings.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                          .Join<CurrentAccountCards>
                        (
                            p => new { CustomerCurrentAccountCardID = p.Id, CustomerCode = p.CustomerCode },
                            nameof(TechnicalDrawings.CustomerCurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, false, false, Tables.TechnicalDrawings);

            var technicalDrawing = queryFactory.Get<SelectTechnicalDrawingsDto>(query);

            LogsAppService.InsertLogToDatabase(technicalDrawing, technicalDrawing, LoginedUserService.UserId, Tables.TechnicalDrawings, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectTechnicalDrawingsDto>(technicalDrawing);


        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListTechnicalDrawingsDto>>> GetListAsync(ListTechnicalDrawingsParameterDto input)
        {
            var query = queryFactory
               .Query()
               .From(Tables.TechnicalDrawings).Select<TechnicalDrawings>(null)
                        .Join<Products>
                        (
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                            nameof(TechnicalDrawings.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                           .Join<CurrentAccountCards>
                        (
                            p => new { CustomerCode = p.CustomerCode },
                            nameof(TechnicalDrawings.CustomerCurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        ).Where(null, false, false, Tables.TechnicalDrawings);

            var technicalDrawings = queryFactory.GetList<ListTechnicalDrawingsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListTechnicalDrawingsDto>>(technicalDrawings);

        }


        public async Task<IDataResult<IList<SelectTechnicalDrawingsDto>>> GetSelectListAsync(Guid productId)
        {
            var query = queryFactory
               .Query()
               .From(Tables.TechnicalDrawings).Select<TechnicalDrawings>(null)
                        .Join<Products>
                        (
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                            nameof(TechnicalDrawings.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                          .Join<CurrentAccountCards>
                        (
                            p => new { CustomerCurrentAccountCardID = p.Id, CustomerCode = p.CustomerCode },
                            nameof(TechnicalDrawings.CustomerCurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        ).Where(new { ProductID = productId }, false, false, Tables.TechnicalDrawings);

            var technicalDrawings = queryFactory.GetList<SelectTechnicalDrawingsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<SelectTechnicalDrawingsDto>>(technicalDrawings);

        }


        [ValidationAspect(typeof(UpdateTechnicalDrawingsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectTechnicalDrawingsDto>> UpdateAsync(UpdateTechnicalDrawingsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.TechnicalDrawings).Select("*").Where(new { Id = input.Id }, false, false, "");
            var entity = queryFactory.Get<TechnicalDrawings>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.TechnicalDrawings).Select("*").Where(new { RevisionNo = input.RevisionNo }, false, false, "");
            var list = queryFactory.GetList<TechnicalDrawings>(listQuery).ToList();

            if (list.Count > 0 && entity.RevisionNo != input.RevisionNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.TechnicalDrawings).Update(new UpdateTechnicalDrawingsDto
            {
                CustomerApproval = input.CustomerApproval,
                RevisionNo = input.RevisionNo,
                RevisionDate = input.RevisionDate,
                CustomerCurrentAccountCardID = input.CustomerCurrentAccountCardID.GetValueOrDefault(),
                ProductID = input.ProductID.GetValueOrDefault(),
                Drawer = input.Drawer,
                DrawingDomain = input.DrawingDomain,
                DrawingFilePath = input.DrawingFilePath,
                DrawingNo = input.DrawingNo,
                IsApproved = input.IsApproved,
                SampleApproval = input.SampleApproval,
                Description_ = input.Description_,
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
            }).Where(new { Id = input.Id }, false, false, "");

            var technicalDrawings = queryFactory.Update<SelectTechnicalDrawingsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, technicalDrawings, LoginedUserService.UserId, Tables.TechnicalDrawings, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectTechnicalDrawingsDto>(technicalDrawings);

        }

        public async Task<IDataResult<SelectTechnicalDrawingsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.TechnicalDrawings).Select("*").Where(new { Id = id }, false, false, "");
            var entity = queryFactory.Get<TechnicalDrawings>(entityQuery);

            var query = queryFactory.Query().From(Tables.TechnicalDrawings).Update(new UpdateTechnicalDrawingsDto
            {
                CustomerApproval = entity.CustomerApproval,
                RevisionNo = entity.RevisionNo,
                RevisionDate = entity.RevisionDate,
                CustomerCurrentAccountCardID = entity.CustomerCurrentAccountCardID,
                ProductID = entity.ProductID,
                Drawer = entity.Drawer,
                DrawingDomain = entity.DrawingDomain,
                DrawingFilePath = entity.DrawingFilePath,
                DrawingNo = entity.DrawingNo,
                IsApproved = entity.IsApproved,
                SampleApproval = entity.SampleApproval,
                Description_ = entity.Description_,
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

            var technicalDrawings = queryFactory.Update<SelectTechnicalDrawingsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectTechnicalDrawingsDto>(technicalDrawings);


        }
    }
}
