using Microsoft.Extensions.Localization;
using System.Data;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.BillsofMaterial.BusinessRules;
using TsiErp.Business.Entities.BillsofMaterial.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.BillsofMaterial;
using TsiErp.Entities.Entities.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.BillsofMaterialLine;
using TsiErp.Entities.Entities.BillsofMaterialLine.Dtos;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Period;
using TsiErp.Entities.Entities.Period.Dtos;
using TsiErp.Entities.Entities.Product;
using TsiErp.Entities.Entities.UnitSet;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.BillsofMaterials.Page;

namespace TsiErp.Business.Entities.BillsofMaterial.Services
{
    [ServiceRegistration(typeof(IBillsofMaterialsAppService), DependencyInjectionType.Scoped)]
    public class BillsofMaterialsAppService : ApplicationService<BillsofMaterialsResource>, IBillsofMaterialsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public BillsofMaterialsAppService(IStringLocalizer<BillsofMaterialsResource> l) : base(l)
        {
        }

        BillsofMaterialManager _manager { get; set; } = new BillsofMaterialManager();


        [ValidationAspect(typeof(CreateBillsofMaterialsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectBillsofMaterialsDto>> CreateAsync(CreateBillsofMaterialsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.BillsofMaterials).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.ControlList<BillsofMaterials>(listQuery).ToList();
                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();

                var query = queryFactory.Query().From(Tables.BillsofMaterials).Insert(new CreateBillsofMaterialsDto
                {
                    Code = input.Code,
                    CreationTime = DateTime.Now,
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
                    Name = input.Name,
                    FinishedProductID = input.FinishedProductID,
                    RouteID = input.RouteID,
                    _Description = input._Description
                });

                foreach (var item in input.SelectBillsofMaterialLines)
                {
                    var queryLine = queryFactory.Query().From(Tables.BillsofMaterialLines).Insert(new CreateBillsofMaterialLinesDto
                    {
                        BoMID = addedEntityId,
                        CreationTime = DateTime.Now,
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        FinishedProductID = item.FinishedProductID,
                        Id = GuidGenerator.CreateGuid(),
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                        LineNr = item.LineNr,
                        MaterialType = item.MaterialType,
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        Size = item.Size,
                        UnitSetID = item.UnitSetID,
                        _Description = item._Description
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }

                var billOfMaterial = queryFactory.Insert<SelectBillsofMaterialsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.BillsofMaterials, LogType.Insert, billOfMaterial.Id);

                return new SuccessDataResult<SelectBillsofMaterialsDto>(billOfMaterial);
            }
            //using (UnitOfWork _uow = new UnitOfWork())
            //{

            //    await _manager.CodeControl(_uow.BillsofMaterialsRepository, input.Code, L);

            //    var entity = ObjectMapper.Map<CreateBillsofMaterialsDto, BillsofMaterials>(input);

            //    var addedEntity = await _uow.BillsofMaterialsRepository.InsertAsync(entity);
            //    await _uow.SaveChanges();

            //    foreach (var item in input.SelectBillsofMaterialLines)
            //    {
            //        var lineEntity = ObjectMapper.Map<SelectBillsofMaterialLinesDto, BillsofMaterialLines>(item);
            //        lineEntity.BoMID = addedEntity.Id;
            //        await _uow.BillsofMaterialLinesRepository.InsertAsync(lineEntity);
            //        await _uow.SaveChanges();
            //    }

            //    input.Id = addedEntity.Id;
            //    var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "BillsofMaterials", LogType.Insert, addedEntity.Id);
            //    await _uow.LogsRepository.InsertAsync(log);

            //    await _uow.SaveChanges();
            //    return new SuccessDataResult<SelectBillsofMaterialsDto>(ObjectMapper.Map<BillsofMaterials, SelectBillsofMaterialsDto>(addedEntity));
            //}
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var lines = (await _uow.BillsofMaterialLinesRepository.GetAsync(t => t.Id == id));

                if (lines != null)
                {
                    await _manager.DeleteControl(_uow.BillsofMaterialsRepository, lines.BoMID, lines.Id, true);
                    await _uow.BillsofMaterialLinesRepository.DeleteAsync(id);
                    await _uow.SaveChanges();
                    return new SuccessResult(L["DeleteSuccessMessage"]);
                }
                else
                {
                    await _manager.DeleteControl(_uow.BillsofMaterialsRepository, id, Guid.Empty, false);
                    var list = (await _uow.BillsofMaterialLinesRepository.GetListAsync(t => t.BoMID == id));
                    foreach (var line in list)
                    {
                        await _uow.BillsofMaterialLinesRepository.DeleteAsync(line.Id);
                    }
                    await _uow.BillsofMaterialsRepository.DeleteAsync(id);

                    var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "BillsofMaterials", LogType.Delete, id);
                    await _uow.LogsRepository.InsertAsync(log);
                    await _uow.SaveChanges();
                    return new SuccessResult(L["DeleteSuccessMessage"]);
                }
            }
        }

        public async Task<IDataResult<SelectBillsofMaterialsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                       .From(Tables.BillsofMaterials)
                       .Select<BillsofMaterials>(b => new { b.Id, b.Code, b.Name, b._Description, b.IsActive })
                       .Join<Products>
                        (
                            pr => new { FinishedProductCode = pr.Code, FinishedProducName = pr.Name, FinishedProductID = pr.Id },
                            nameof(BillsofMaterials.FinishedProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, true, true, Tables.BillsofMaterials);

                var billsOfMaterials = queryFactory.Get<SelectBillsofMaterialsDto>(query);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.BillsofMaterialLines)
                       .Select<BillsofMaterialLines>(b => new { b.BoMID, b.FinishedProductID, b.MaterialType, b.Quantity, b._Description, b.LineNr, b.Size })
                       .Join<Products>
                        (
                            p => new { FinishedProductCode = p.Code },
                            nameof(BillsofMaterialLines.FinishedProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                       .Join<Products>
                        (
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                            nameof(BillsofMaterialLines.ProductID),
                            nameof(Products.Id),
                            "ProductLine",
                            JoinType.Left
                        )
                       .Join<UnitSets>
                        (
                            u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                            nameof(BillsofMaterialLines.UnitSetID),
                            nameof(UnitSets.Id),
                            JoinType.Left
                        )
                        .Where(new { BoMID = id }, false, false, Tables.BillsofMaterialLines);

                var billsOfMaterialLine = queryFactory.GetList<SelectBillsofMaterialLinesDto>(queryLines).ToList();

                billsOfMaterials.SelectBillsofMaterialLines = billsOfMaterialLine;

                LogsAppService.InsertLogToDatabase(billsOfMaterials, billsOfMaterials, LoginedUserService.UserId, Tables.BillsofMaterials, LogType.Get, id);

                return new SuccessDataResult<SelectBillsofMaterialsDto>(billsOfMaterials);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListBillsofMaterialsDto>>> GetListAsync(ListBillsofMaterialsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                       .From(Tables.BillsofMaterials)
                       .Select<BillsofMaterials>(b => new { b.Id, b.Code, b.Name, b._Description, b.IsActive })
                       .Join<Products>
                        (
                            pr => new { FinishedProductCode = pr.Code, FinishedProducName = pr.Name, FinishedProductID = pr.Id },
                            nameof(BillsofMaterials.FinishedProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                        .Where(null, true, true, Tables.BillsofMaterials);

                var billsOfMaterials = queryFactory.GetList<ListBillsofMaterialsDto>(query).ToList();
                return new SuccessDataResult<IList<ListBillsofMaterialsDto>>(billsOfMaterials);
            }
        }

        [ValidationAspect(typeof(UpdateBillsofMaterialsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectBillsofMaterialsDto>> UpdateAsync(UpdateBillsofMaterialsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.BillsofMaterialsRepository.GetAsync(x => x.Id == input.Id, t => t.BillsofMaterialLines);

                await _manager.UpdateControl(_uow.BillsofMaterialsRepository, input.Code, input.Id, entity, L);

                var mappedEntity = ObjectMapper.Map<UpdateBillsofMaterialsDto, BillsofMaterials>(input);

                await _uow.BillsofMaterialsRepository.UpdateAsync(mappedEntity);

                foreach (var item in input.SelectBillsofMaterialLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectBillsofMaterialLinesDto, BillsofMaterialLines>(item);
                    lineEntity.BoMID = mappedEntity.Id;

                    if (lineEntity.Id == Guid.Empty)
                    {
                        await _uow.BillsofMaterialLinesRepository.InsertAsync(lineEntity);
                    }
                    else
                    {
                        await _uow.BillsofMaterialLinesRepository.UpdateAsync(lineEntity);
                    }
                }

                var before = ObjectMapper.Map<BillsofMaterials, UpdateBillsofMaterialsDto>(entity);
                before.SelectBillsofMaterialLines = ObjectMapper.Map<List<BillsofMaterialLines>, List<SelectBillsofMaterialLinesDto>>(entity.BillsofMaterialLines.ToList());
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "BillsofMaterials", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();
                return new SuccessDataResult<SelectBillsofMaterialsDto>(ObjectMapper.Map<BillsofMaterials, SelectBillsofMaterialsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectBillsofMaterialsDto>> GetSelectListAsync(Guid finishedproductId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.BillsofMaterialsRepository.GetAsync(t => t.FinishedProductID == finishedproductId,
               t => t.BillsofMaterialLines,
               t => t.Products);

                var mappedEntity = ObjectMapper.Map<BillsofMaterials, SelectBillsofMaterialsDto>(entity);

                mappedEntity.SelectBillsofMaterialLines = ObjectMapper.Map<List<BillsofMaterialLines>, List<SelectBillsofMaterialLinesDto>>(entity.BillsofMaterialLines.ToList());

                return new SuccessDataResult<SelectBillsofMaterialsDto>(mappedEntity);
            }
        }

        public async Task<IDataResult<SelectBillsofMaterialsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.BillsofMaterialsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.BillsofMaterialsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<BillsofMaterials, SelectBillsofMaterialsDto>(updatedEntity);

                return new SuccessDataResult<SelectBillsofMaterialsDto>(mappedEntity);
            }
        }
    }
}
