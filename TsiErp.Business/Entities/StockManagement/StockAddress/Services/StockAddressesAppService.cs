using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.StockManagementParameter.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.StockManagement.StockAddress.Validations;
using TsiErp.Business.Entities.StockMovement;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.StockAddress;
using TsiErp.Entities.Entities.StockManagement.StockAddress.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockAddressLine;
using TsiErp.Entities.Entities.StockManagement.StockAddressLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockColumn;
using TsiErp.Entities.Entities.StockManagement.StockNumber;
using TsiErp.Entities.Entities.StockManagement.StockSection;
using TsiErp.Entities.Entities.StockManagement.StockShelf;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.StockAdresses.Page;

namespace TsiErp.Business.Entities.StockAddress.Services
{
    [ServiceRegistration(typeof(IStockAddressesAppService), DependencyInjectionType.Scoped)]
    public class StockAddressesAppService : ApplicationService<StockAdressesResource>, IStockAddressesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public StockAddressesAppService(IStringLocalizer<StockAdressesResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }

        [ValidationAspect(typeof(CreateStockAddressesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStockAddressesDto>> CreateAsync(CreateStockAddressesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.StockAddresses).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.ControlList<StockAddresses>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();



            var query = queryFactory.Query().From(Tables.StockAddresses).Insert(new CreateStockAddressesDto
            {
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
                Description_ = input.Description_,
                Code = input.Code,
                ProductID = input.ProductID,
            });

            foreach (var item in input.SelectStockAddressLines)
            {
                var queryLine = queryFactory.Query().From(Tables.StockAddressLines).Insert(new CreateStockAddressLinesDto
                {
                    StockAdressID = addedEntityId,
                    CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
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
                    StockColumnID = item.StockColumnID.GetValueOrDefault(),
                    StockNumberID = item.StockNumberID.GetValueOrDefault(),
                    StockSectionID = item.StockSectionID.GetValueOrDefault(),
                    StockShelfID = item.StockShelfID.GetValueOrDefault(),
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }



            var StockAddress = queryFactory.Insert<SelectStockAddressesDto>(query, "Id", true);



            await FicheNumbersAppService.UpdateFicheNumberAsync("StockAddressesChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.StockAddresses, LogType.Insert, addedEntityId);

            return new SuccessDataResult<SelectStockAddressesDto>(StockAddress);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.StockAddresses).Select("*").Where(new { Id = id }, false, false, "");

            var StockAddresses = queryFactory.Get<SelectStockAddressesDto>(query);

            if (StockAddresses.Id != Guid.Empty && StockAddresses != null)
            {

                var deleteQuery = queryFactory.Query().From(Tables.StockAddresses).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.StockAddressLines).Delete(LoginedUserService.UserId).Where(new { StockAdressID = id }, false, false, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var StockAddress = queryFactory.Update<SelectStockAddressesDto>(deleteQuery, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.StockAddresses, LogType.Delete, id);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectStockAddressesDto>(StockAddress);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.StockAddressLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var StockAddressLines = queryFactory.Update<SelectStockAddressLinesDto>(queryLine, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.StockAddressLines, LogType.Delete, id);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectStockAddressLinesDto>(StockAddressLines);
            }

        }

        public async Task<IDataResult<SelectStockAddressesDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.StockAddresses)
                   .Select<StockAddresses>(null)

                   .Join<Products>
                    (
                        w => new { ProductCode = w.Code, ProductID = w.Id, ProductName = w.Name },
                        nameof(StockAddresses.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, false, false, Tables.StockAddresses);

            var stockAddresses = queryFactory.Get<SelectStockAddressesDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.StockAddressLines)
                   .Select<StockAddressLines>(null)
                   .Join<StockColumns>
                    (
                        u => new { StockColumnID = u.Id, StockColumnName = u.Name },
                        nameof(StockAddressLines.StockColumnID),
                        nameof(StockColumns.Id),
                        JoinType.Left
                    )
                    .Join<StockSections>
                    (
                        u => new { StockSectionID = u.Id, StockSectionName = u.Name },
                        nameof(StockAddressLines.StockSectionID),
                        nameof(StockSections.Id),
                        JoinType.Left
                    )
                    .Join<StockNumbers>
                    (
                        u => new { StockNumberID = u.Id, StockNumberName = u.Name },
                        nameof(StockAddressLines.StockNumberID),
                        nameof(StockNumbers.Id),
                        JoinType.Left
                    )
                    .Join<StockShelfs>
                    (
                        u => new { StockShelfID = u.Id, StockShelfName = u.Name },
                        nameof(StockAddressLines.StockShelfID),
                        nameof(StockShelfs.Id),
                        JoinType.Left
                    )
                    .Where(new { StockAdressID = id }, false, false, Tables.StockAddressLines);

            var StockAddressLine = queryFactory.GetList<SelectStockAddressLinesDto>(queryLines).ToList();

            stockAddresses.SelectStockAddressLines = StockAddressLine;

            LogsAppService.InsertLogToDatabase(stockAddresses, stockAddresses, LoginedUserService.UserId, Tables.StockAddresses, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStockAddressesDto>(stockAddresses);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListStockAddressesDto>>> GetListAsync(ListStockAddressesParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.StockAddresses)
                   .Select<StockAddresses>(null)
                    .Join<Products>
                    (
                        w => new { ProductCode = w.Code, ProductID = w.Id, ProductName = w.Name },
                        nameof(StockAddresses.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Where(null, false, false, Tables.StockAddresses);

            var StockAddressesDto = queryFactory.GetList<ListStockAddressesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListStockAddressesDto>>(StockAddressesDto);

        }

        [ValidationAspect(typeof(UpdateStockAddressesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStockAddressesDto>> UpdateAsync(UpdateStockAddressesDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.StockAddresses)
                   .Select<StockAddresses>(null)
                    .Join<Products>
                    (
                        w => new { ProductCode = w.Code, ProductID = w.Id, ProductName = w.Name },
                        nameof(StockAddresses.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, false, false, Tables.StockAddresses);

            var entity = queryFactory.Get<SelectStockAddressesDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.StockAddressLines)
                   .Select<StockAddressLines>(null)
                    .Join<StockColumns>
                    (
                        u => new { StockColumnID = u.Id, StockColumnName = u.Name },
                        nameof(StockAddressLines.StockColumnID),
                        nameof(StockColumns.Id),
                        JoinType.Left
                    )
                    .Join<StockSections>
                    (
                        u => new { StockSectionID = u.Id, StockSectionName = u.Name },
                        nameof(StockAddressLines.StockSectionID),
                        nameof(StockSections.Id),
                        JoinType.Left
                    )
                    .Join<StockNumbers>
                    (
                        u => new { StockNumberID = u.Id, StockNumberName = u.Name },
                        nameof(StockAddressLines.StockNumberID),
                        nameof(StockNumbers.Id),
                        JoinType.Left
                    )
                    .Join<StockShelfs>
                    (
                        u => new { StockShelfID = u.Id, StockShelfName = u.Name },
                        nameof(StockAddressLines.StockShelfID),
                        nameof(StockShelfs.Id),
                        JoinType.Left
                    )
                    .Where(new { StockAdressID = input.Id }, false, false, Tables.StockAddressLines);

            var StockAddressLine = queryFactory.GetList<SelectStockAddressLinesDto>(queryLines).ToList();

            entity.SelectStockAddressLines = StockAddressLine;

            #region Update Control
            var listQuery = queryFactory.Query().From(Tables.StockAddresses).Select("*").Where(new { Code = input.Code }, false, false, "");

            var list = queryFactory.GetList<StockAddresses>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion


            var query = queryFactory.Query().From(Tables.StockAddresses).Update(new UpdateStockAddressesDto
            {
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId,
                Description_ = input.Description_,
                ProductID = input.ProductID,
                Code = input.Code,

            }).Where(new { Id = input.Id }, false, false, "");


            foreach (var item in input.SelectStockAddressLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.StockAddressLines).Insert(new CreateStockAddressLinesDto
                    {
                        CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
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
                        StockShelfID = item.StockShelfID.GetValueOrDefault(),
                        StockNumberID = item.StockNumberID.GetValueOrDefault(),
                        StockSectionID = item.StockSectionID.GetValueOrDefault(),
                        StockColumnID = item.StockColumnID.GetValueOrDefault(),
                        StockAdressID = input.Id
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.StockAddressLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                    var line = queryFactory.Get<SelectStockAddressLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.StockAddressLines).Update(new UpdateStockAddressLinesDto
                        {
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                            StockAdressID = input.Id,
                            StockColumnID = item.StockColumnID.GetValueOrDefault(),
                            StockSectionID = item.StockSectionID.GetValueOrDefault(),
                            StockNumberID = item.StockNumberID.GetValueOrDefault(),
                            StockShelfID = item.StockShelfID.GetValueOrDefault(),
                        }).Where(new { Id = line.Id }, false, false, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var StockAddress = queryFactory.Update<SelectStockAddressesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.StockAddresses, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStockAddressesDto>(StockAddress);

        }

        public async Task<IDataResult<SelectStockAddressesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.StockAddresses).Select("*").Where(new { Id = id }, false, false, "");

            var entity = queryFactory.Get<StockAddresses>(entityQuery);

            var query = queryFactory.Query().From(Tables.StockAddresses).Update(new UpdateStockAddressesDto
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
                Description_ = entity.Description_,
                Code = entity.Code,
                ProductID = entity.ProductID,

            }).Where(new { Id = id }, false, false, "");

            var StockAddressesDto = queryFactory.Update<SelectStockAddressesDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectStockAddressesDto>(StockAddressesDto);

        }



    }
}
