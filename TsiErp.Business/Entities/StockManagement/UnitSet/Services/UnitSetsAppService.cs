using Microsoft.Extensions.Localization;
using System.Data;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.UnitSet.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.SalesManagement.SalesPropositionLine;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.UnitSets.Page;

namespace TsiErp.Business.Entities.UnitSet.Services
{
    [ServiceRegistration(typeof(IUnitSetsAppService), DependencyInjectionType.Scoped)]
    public class UnitSetsAppService : ApplicationService<UnitSetsResource>, IUnitSetsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public UnitSetsAppService(IStringLocalizer<UnitSetsResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }


        [ValidationAspect(typeof(CreateUnitSetsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnitSetsDto>> CreateAsync(CreateUnitSetsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.UnitSets).Select("*").Where(new { Code = input.Code }, false, false, "");

            var list = queryFactory.ControlList<UnitSets>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.UnitSets).Insert(new CreateUnitSetsDto
            {
                Code = input.Code,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
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

            var unitsets = queryFactory.Insert<SelectUnitSetsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.UnitSets, LogType.Insert, addedEntityId);

            return new SuccessDataResult<SelectUnitSetsDto>(unitsets);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("UnitSetID", new List<string>
            {
                Tables.BillsofMaterialLines,
                Tables.MaintenanceInstructionLines,
                Tables.MaintenanceMRPLines,
                Tables.PlannedMaintenanceLines,
                Tables.ProductionOrders,
                Tables.Products,
                Tables.PurchaseOrderLines,
                Tables.PurchaseRequestLines,
                Tables.SalesOrderLines,
                Tables.SalesPropositionLines,
                Tables.StockFicheLines,
                Tables.UnplannedMaintenanceLines,
                Tables.MRPLines
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var query = queryFactory.Query().From(Tables.UnitSets).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                var unitsets = queryFactory.Update<SelectUnitSetsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.UnitSets, LogType.Delete, id);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectUnitSetsDto>(unitsets);

            }
        }

        public async Task<IDataResult<SelectUnitSetsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.UnitSets).Select("*").Where(
           new
           {
               Id = id
           }, true, true, "");
            var unitset = queryFactory.Get<SelectUnitSetsDto>(query);


            LogsAppService.InsertLogToDatabase(unitset, unitset, LoginedUserService.UserId, Tables.UnitSets, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectUnitSetsDto>(unitset);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListUnitSetsDto>>> GetListAsync(ListUnitSetsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.UnitSets).Select("*").Where(null, true, true, "");
            var unitsets = queryFactory.GetList<ListUnitSetsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListUnitSetsDto>>(unitsets);

        }


        [ValidationAspect(typeof(UpdateUnitSetsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnitSetsDto>> UpdateAsync(UpdateUnitSetsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.UnitSets).Select("*").Where(new { Id = input.Id }, true, true, "");
            var entity = queryFactory.Get<UnitSets>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.UnitSets).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.GetList<UnitSets>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.UnitSets).Update(new UpdateUnitSetsDto
            {
                Code = input.Code,
                Name = input.Name,
                Id = input.Id,
                IsActive = input.IsActive,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id }, true, true, "");

            var unitsets = queryFactory.Update<SelectUnitSetsDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, unitsets, LoginedUserService.UserId, Tables.UnitSets, LogType.Update, entity.Id);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectUnitSetsDto>(unitsets);

        }

        public async Task<IDataResult<SelectUnitSetsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.UnitSets).Select("*").Where(new { Id = id }, true, true, "");
            var entity = queryFactory.Get<UnitSets>(entityQuery);

            var query = queryFactory.Query().From(Tables.UnitSets).Update(new UpdateUnitSetsDto
            {
                Code = entity.Code,
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
                DataOpenStatusUserId = userId,

            }).Where(new { Id = id }, true, true, "");

            var unitsets = queryFactory.Update<SelectUnitSetsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectUnitSetsDto>(unitsets);


        }
    }
}
