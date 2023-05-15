using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results; 
using TsiErp.Localizations.Resources.ProductGroups.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.ProductGroup.BusinessRules;
using TsiErp.Business.Entities.ProductGroup.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.ProductGroup;
using TsiErp.Entities.Entities.ProductGroup.Dtos;
using Microsoft.Extensions.Localization;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Entities.TableConstant;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;

namespace TsiErp.Business.Entities.ProductGroup.Services
{
    [ServiceRegistration(typeof(IProductGroupsAppService), DependencyInjectionType.Scoped)]
    public class ProductGroupsAppService : ApplicationService<ProductGroupsResource>, IProductGroupsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public ProductGroupsAppService(IStringLocalizer<ProductGroupsResource> l) : base(l)
        {
        }



        [ValidationAspect(typeof(CreateProductGroupsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductGroupsDto>> CreateAsync(CreateProductGroupsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.ProductGroups).Select("*").Where(new { Code = input.Code }, false, false, "");

                var list = queryFactory.ControlList<ProductGroups>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion


                var query = queryFactory.Query().From(Tables.ProductGroups).Insert(new CreateProductGroupsDto
                {
                    Code = input.Code,
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


                var productGroups = queryFactory.Insert<SelectProductGroupsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ProductGroups, LogType.Insert, productGroups.Id);


                return new SuccessDataResult<SelectProductGroupsDto>(productGroups);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.ProductGroups).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                var productGroups = queryFactory.Update<SelectProductGroupsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ProductGroups, LogType.Delete, id);

                return new SuccessDataResult<SelectProductGroupsDto>(productGroups);
            }
        }


        public async Task<IDataResult<SelectProductGroupsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.ProductGroups).Select("*").Where(
                new
                {
                    Id = id
                }, true, true, "");
                var productGroup = queryFactory.Get<SelectProductGroupsDto>(query);


                LogsAppService.InsertLogToDatabase(productGroup, productGroup, LoginedUserService.UserId, Tables.ProductGroups, LogType.Get, id);

                return new SuccessDataResult<SelectProductGroupsDto>(productGroup);

            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductGroupsDto>>> GetListAsync(ListProductGroupsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.ProductGroups).Select("*").Where(null, true, true, "");
                var productGroups = queryFactory.GetList<ListProductGroupsDto>(query).ToList();
                return new SuccessDataResult<IList<ListProductGroupsDto>>(productGroups);
            }
        }


        [ValidationAspect(typeof(UpdateProductGroupsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductGroupsDto>> UpdateAsync(UpdateProductGroupsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.ProductGroups).Select("*").Where(new { Id = input.Id }, true, true, "");
                var entity = queryFactory.Get<ProductGroups>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.ProductGroups).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.GetList<ProductGroups>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.ProductGroups).Update(new UpdateProductGroupsDto
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

                var productGroups = queryFactory.Update<SelectProductGroupsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, productGroups, LoginedUserService.UserId, Tables.ProductGroups, LogType.Update, entity.Id);

                return new SuccessDataResult<SelectProductGroupsDto>(productGroups);
            }
        }

        public async Task<IDataResult<SelectProductGroupsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.ProductGroups).Select("*").Where(new { Id = id }, true, true, "");

                var entity = queryFactory.Get<ProductGroups>(entityQuery);

                var query = queryFactory.Query().From(Tables.ProductGroups).Update(new UpdateProductGroupsDto
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
                    DataOpenStatusUserId = userId

                }).Where(new { Id = id }, true, true, "");

                var productGroups = queryFactory.Update<SelectProductGroupsDto>(query, "Id", true);
                return new SuccessDataResult<SelectProductGroupsDto>(productGroups);

            }
        }
    }
}
