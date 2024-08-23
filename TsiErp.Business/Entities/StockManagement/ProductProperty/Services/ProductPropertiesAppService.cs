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
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.StockManagement.ProductProperty.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.StockManagement.ProductProperty;
using TsiErp.Entities.Entities.StockManagement.ProductProperty.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductPropertyLine.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.StockAdresses.Page;

namespace TsiErp.Business.Entities.ProductProperty.Services
{
    [ServiceRegistration(typeof(IProductPropertiesAppService), DependencyInjectionType.Scoped)]
    public class ProductPropertiesAppService : ApplicationService<StockAdressesResource>, IProductPropertiesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public ProductPropertiesAppService(IStringLocalizer<StockAdressesResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }




        [ValidationAspect(typeof(CreateProductPropertiesValidator), Priority = 1)]
        public async Task<IDataResult<SelectProductPropertiesDto>> CreateAsync(CreateProductPropertiesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.ProductProperties).Select("*").Where(new { Code = input.Code }, "");
            var list = queryFactory.ControlList<ProductProperties>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();



            var query = queryFactory.Query().From(Tables.ProductProperties).Insert(new CreateProductPropertiesDto
            {
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                Code = input.Code,
                isChooseFromList = input.isChooseFromList,
                Name = input.Name,
                ProductGroupID = input.ProductGroupID,

            });

            foreach (var item in input.SelectProductPropertyLines)
            {
                var queryLine = queryFactory.Query().From(Tables.ProductPropertyLines).Insert(new CreateProductPropertyLinesDto
                {
                    CreationTime = now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = GuidGenerator.CreateGuid(),
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    LineNr = item.LineNr,
                    ProductGroupID = item.ProductGroupID.GetValueOrDefault(),
                    ProductPropertyID = addedEntityId,
                    LineCode = item.LineCode,
                    LineName = item.LineName,
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }



            var ProductProperty = queryFactory.Insert<SelectProductPropertiesDto>(query, "Id", true);



            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ProductProperties, LogType.Insert, addedEntityId);

            return new SuccessDataResult<SelectProductPropertiesDto>(ProductProperty);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.ProductProperties).Select("*").Where(new { Id = id },  "");

            var ProductProperties = queryFactory.Get<SelectProductPropertiesDto>(query);

            if (ProductProperties.Id != Guid.Empty && ProductProperties != null)
            {

                var deleteQuery = queryFactory.Query().From(Tables.ProductProperties).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.ProductPropertyLines).Delete(LoginedUserService.UserId).Where(new { ProductPropertyID = id },  "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var ProductProperty = queryFactory.Update<SelectProductPropertiesDto>(deleteQuery, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ProductProperties, LogType.Delete, id);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectProductPropertiesDto>(ProductProperty);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.ProductPropertyLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var ProductPropertyLines = queryFactory.Update<SelectProductPropertyLinesDto>(queryLine, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ProductPropertyLines, LogType.Delete, id);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectProductPropertyLinesDto>(ProductPropertyLines);
            }

        }

        public async Task<IDataResult<SelectProductPropertiesDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.ProductProperties)
                  .Select("*").Where(
            new
            {
                Id = id
            },  "");

            var ProductProperties = queryFactory.Get<SelectProductPropertiesDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.ProductPropertyLines)
                   .Select("*").Where(
            new
            {
                ProductPropertyID = id
            }, "");

            var ProductPropertyLine = queryFactory.GetList<SelectProductPropertyLinesDto>(queryLines).ToList();

            ProductProperties.SelectProductPropertyLines = ProductPropertyLine;

            LogsAppService.InsertLogToDatabase(ProductProperties, ProductProperties, LoginedUserService.UserId, Tables.ProductProperties, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductPropertiesDto>(ProductProperties);

        }

        public async Task<IDataResult<IList<ListProductPropertiesDto>>> GetListAsync(ListProductPropertiesParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.ProductProperties)
                   .Select("*").Where(null, "");

            var ProductPropertiesDto = queryFactory.GetList<ListProductPropertiesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListProductPropertiesDto>>(ProductPropertiesDto);

        }

        public async Task<IDataResult<IList<ListProductPropertiesDto>>> GetListByProductGroupAsync(Guid ProductGroupID)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.ProductProperties)
                   .Select("*").Where(new
                   {
                       ProductGroupID = ProductGroupID
                   }, "");

            var ProductPropertiesDto = queryFactory.GetList<ListProductPropertiesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListProductPropertiesDto>>(ProductPropertiesDto);

        }

        [ValidationAspect(typeof(UpdateProductPropertiesValidator), Priority = 1)]
        public async Task<IDataResult<SelectProductPropertiesDto>> UpdateAsync(UpdateProductPropertiesDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.ProductProperties)
                   .Select("*").Where(
            new
            {
                Id = input.Id
            }, "");

            var entity = queryFactory.Get<SelectProductPropertiesDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.ProductPropertyLines)
                    .Select("*").Where(
            new
            {
                ProductPropertyID = input.Id
            }, "");

            var ProductPropertyLine = queryFactory.GetList<SelectProductPropertyLinesDto>(queryLines).ToList();

            entity.SelectProductPropertyLines = ProductPropertyLine;

            #region Update Control
            var listQuery = queryFactory.Query().From(Tables.ProductProperties).Select("*").Where(new { Code = input.Code },  "");

            var list = queryFactory.GetList<ProductProperties>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();


            var query = queryFactory.Query().From(Tables.ProductProperties).Update(new UpdateProductPropertiesDto
            {
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
                ProductGroupID = input.ProductGroupID,
                Name = input.Name,
                isChooseFromList = input.isChooseFromList,
                Code = input.Code,

            }).Where(new { Id = input.Id }, "");


            foreach (var item in input.SelectProductPropertyLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.ProductPropertyLines).Insert(new CreateProductPropertyLinesDto
                    {
                        CreationTime = now,
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        Id = GuidGenerator.CreateGuid(),
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                        LineNr = item.LineNr,
                        ProductGroupID = item.ProductGroupID.GetValueOrDefault(),
                        LineName = item.LineName,
                        LineCode = item.LineCode,
                        ProductPropertyID = input.Id,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.ProductPropertyLines).Select("*").Where(new { Id = item.Id },"");

                    var line = queryFactory.Get<SelectProductPropertyLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.ProductPropertyLines).Update(new UpdateProductPropertyLinesDto
                        {
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                            ProductGroupID = item.ProductGroupID,
                            LineName = item.LineName,
                            LineCode = item.LineCode,
                            ProductPropertyID = item.ProductPropertyID,
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var ProductProperty = queryFactory.Update<SelectProductPropertiesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.ProductProperties, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductPropertiesDto>(ProductProperty);

        }

        public async Task<IDataResult<SelectProductPropertiesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.ProductProperties).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<ProductProperties>(entityQuery);

            var query = queryFactory.Query().From(Tables.ProductProperties).Update(new UpdateProductPropertiesDto
            {
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = entity.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Code = entity.Code,
                ProductGroupID = entity.ProductGroupID,
                isChooseFromList = entity.isChooseFromList,
                Name = entity.Name,


            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id },"");

            var ProductPropertiesDto = queryFactory.Update<SelectProductPropertiesDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductPropertiesDto>(ProductPropertiesDto);

        }



    }
}
