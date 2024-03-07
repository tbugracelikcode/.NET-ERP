using Microsoft.Extensions.Localization;
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
using TsiErp.Business.Entities.ProductGroup.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.StockManagement.ProductGroup;
using TsiErp.Entities.Entities.StockManagement.ProductGroup.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ProductGroups.Page;

namespace TsiErp.Business.Entities.ProductGroup.Services
{
    [ServiceRegistration(typeof(IProductGroupsAppService), DependencyInjectionType.Scoped)]
    public class ProductGroupsAppService : ApplicationService<ProductGroupsResource>, IProductGroupsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public ProductGroupsAppService(IStringLocalizer<ProductGroupsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }



        [ValidationAspect(typeof(CreateProductGroupsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductGroupsDto>> CreateAsync(CreateProductGroupsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.ProductGroups).Select("*").Where(new { Code = input.Code }, false, false, "");

            var list = queryFactory.ControlList<ProductGroups>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.ProductGroups).Insert(new CreateProductGroupsDto
            {
                Code = input.Code,
                Name = input.Name,
                IsActive = true,
                Id = addedEntityId,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
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

            await FicheNumbersAppService.UpdateFicheNumberAsync("ProductGroupsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ProductGroups, LogType.Insert, addedEntityId);


            return new SuccessDataResult<SelectProductGroupsDto>(productGroups);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("ProductGrpID", new List<string>
            {
                Tables.Products
            });

            

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var query = queryFactory.Query().From(Tables.ProductGroups).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                var productGroups = queryFactory.Update<SelectProductGroupsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ProductGroups, LogType.Delete, id);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectProductGroupsDto>(productGroups);
            }
        }


        public async Task<IDataResult<SelectProductGroupsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.ProductGroups).Select("*").Where(
            new
            {
                Id = id
            }, true, true, "");
            var productGroup = queryFactory.Get<SelectProductGroupsDto>(query);


            LogsAppService.InsertLogToDatabase(productGroup, productGroup, LoginedUserService.UserId, Tables.ProductGroups, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductGroupsDto>(productGroup);

        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductGroupsDto>>> GetListAsync(ListProductGroupsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.ProductGroups).Select("*").Where(null, true, true, "");
            var productGroups = queryFactory.GetList<ListProductGroupsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListProductGroupsDto>>(productGroups);

        }


        [ValidationAspect(typeof(UpdateProductGroupsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductGroupsDto>> UpdateAsync(UpdateProductGroupsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.ProductGroups).Select("*").Where(new { Id = input.Id }, true, true, "");
            var entity = queryFactory.Get<ProductGroups>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.ProductGroups).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.GetList<ProductGroups>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
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
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id }, true, true, "");

            var productGroups = queryFactory.Update<SelectProductGroupsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, productGroups, LoginedUserService.UserId, Tables.ProductGroups, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductGroupsDto>(productGroups);

        }

        public async Task<IDataResult<SelectProductGroupsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
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
            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductGroupsDto>(productGroups);


        }
    }
}
