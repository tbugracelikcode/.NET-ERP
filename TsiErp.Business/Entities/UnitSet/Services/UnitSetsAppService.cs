using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.UnitSet.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Product;
using TsiErp.Entities.Entities.SalesPropositionLine;
using TsiErp.Entities.Entities.UnitSet;
using TsiErp.Entities.Entities.UnitSet.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.UnitSets.Page;

namespace TsiErp.Business.Entities.UnitSet.Services
{
    [ServiceRegistration(typeof(IUnitSetsAppService), DependencyInjectionType.Scoped)]
    public class UnitSetsAppService : ApplicationService<UnitSetsResource>, IUnitSetsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public UnitSetsAppService(IStringLocalizer<UnitSetsResource> l) : base(l)
        {
        }


        [ValidationAspect(typeof(CreateUnitSetsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnitSetsDto>> CreateAsync(CreateUnitSetsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.UnitSets).Select("*").Where(new { Code = input.Code }, false, false, "");

                var list = queryFactory.ControlList<UnitSets>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.UnitSets).Insert(new CreateUnitSetsDto
                {
                    Code = input.Code,
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

                var unitsets = queryFactory.Insert<SelectUnitSetsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.UnitSets, LogType.Insert, unitsets.Id);

                return new SuccessDataResult<SelectUnitSetsDto>(unitsets);
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                #region Delete Control

                var salesPropositionLineQuery = queryFactory.Query().From(Tables.SalesPropositionLines).Select("*").Where(new { UnitSetID = id }, true, true, "");
                var salesPropositionLines = queryFactory.Get<SalesPropositionLines>(salesPropositionLineQuery);

                if (salesPropositionLines != null && salesPropositionLines.Id != Guid.Empty)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new Exception(L["DeleteControlManager"]);
                }

                var productQuery = queryFactory.Query().From(Tables.Products).Select("*").Where(new { UnitSetID = id }, false, false, "");
                var products = queryFactory.Get<Products>(productQuery);

                if (products != null && products.Id != Guid.Empty)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new Exception(L["DeleteControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.UnitSets).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                var unitsets = queryFactory.Update<SelectUnitSetsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.UnitSets, LogType.Delete, id);

                return new SuccessDataResult<SelectUnitSetsDto>(unitsets);
            }

        }

        public async Task<IDataResult<SelectUnitSetsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.UnitSets).Select("*").Where(
               new
               {
                   Id = id
               }, true, true, "");
                var unitset = queryFactory.Get<SelectUnitSetsDto>(query);


                LogsAppService.InsertLogToDatabase(unitset, unitset, LoginedUserService.UserId, Tables.UnitSets, LogType.Get, id);

                return new SuccessDataResult<SelectUnitSetsDto>(unitset);

            }

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListUnitSetsDto>>> GetListAsync(ListUnitSetsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.UnitSets).Select("*").Where(null, true, true, "");
                var unitsets = queryFactory.GetList<ListUnitSetsDto>(query).ToList();
                return new SuccessDataResult<IList<ListUnitSetsDto>>(unitsets);
            }
        }


        [ValidationAspect(typeof(UpdateUnitSetsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnitSetsDto>> UpdateAsync(UpdateUnitSetsDto input)
        {

            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.UnitSets).Select("*").Where(new { Id = input.Id }, true, true, "");
                var entity = queryFactory.Get<UnitSets>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.UnitSets).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.GetList<UnitSets>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
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
                    DeleterId = entity.DeleterId.Value,
                    DeletionTime = entity.DeletionTime.Value,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = input.Id }, true, true, "");

                var unitsets = queryFactory.Update<SelectUnitSetsDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, unitsets, LoginedUserService.UserId, Tables.UnitSets, LogType.Update, entity.Id);


                return new SuccessDataResult<SelectUnitSetsDto>(unitsets);
            }
        }

        public async Task<IDataResult<SelectUnitSetsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
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
                    DeleterId = entity.DeleterId.Value,
                    DeletionTime = entity.DeletionTime.Value,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = userId,
                    Id = id,
                    DataOpenStatus = lockRow,
                    DataOpenStatusUserId = userId,

                }).Where(new { Id = id }, true, true, "");

                var unitsets = queryFactory.Update<SelectUnitSetsDto>(query, "Id", true);

                return new SuccessDataResult<SelectUnitSetsDto>(unitsets);

            }
        }
    }
}
