using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.PurchaseRequests.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.PurchaseRequest.BusinessRules;
using TsiErp.Business.Entities.PurchaseRequest.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.PurchaseOrderLine.Dtos;
using TsiErp.Entities.Entities.PurchaseRequest;
using TsiErp.Entities.Entities.PurchaseRequest.Dtos;
using TsiErp.Entities.Entities.PurchaseRequestLine;
using TsiErp.Entities.Entities.PurchaseRequestLine.Dtos;
using TsiErp.Entities.Enums;
using Microsoft.Extensions.Localization;
using TsiErp.Entities.Entities.ByDateStockMovement;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.GrandTotalStockMovement;
using TsiErp.Entities.Entities.GrandTotalStockMovement;

namespace TsiErp.Business.Entities.PurchaseRequest.Services
{
    [ServiceRegistration(typeof(IPurchaseRequestsAppService), DependencyInjectionType.Scoped)]
    public class PurchaseRequestsAppService : ApplicationService<PurchaseRequestsResource>, IPurchaseRequestsAppService
    {
        public PurchaseRequestsAppService(IStringLocalizer<PurchaseRequestsResource> l) : base(l)
        {
        }

        PurchaseRequestManager _manager { get; set; } = new PurchaseRequestManager();

        [ValidationAspect(typeof(CreatePurchaseRequestsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseRequestsDto>> CreateAsync(CreatePurchaseRequestsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.PurchaseRequestsRepository, input.FicheNo, L);

                var entity = ObjectMapper.Map<CreatePurchaseRequestsDto, PurchaseRequests>(input);

                var addedEntity = await _uow.PurchaseRequestsRepository.InsertAsync(entity);

                foreach (var item in input.SelectPurchaseRequestLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectPurchaseRequestLinesDto, PurchaseRequestLines>(item);
                    lineEntity.PurchaseRequestID = addedEntity.Id;
                    await _uow.PurchaseRequestLinesRepository.InsertAsync(lineEntity);

                    await StockMovementInsert(input, _uow, lineEntity);
                }
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "PurchaseRequests", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();
                return new SuccessDataResult<SelectPurchaseRequestsDto>(ObjectMapper.Map<PurchaseRequests, SelectPurchaseRequestsDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var lines = (await _uow.PurchaseRequestLinesRepository.GetAsync(t => t.Id == id));

                if (lines != null)
                {
                    var entity = await _uow.PurchaseRequestsRepository.GetAsync(t => t.Id == lines.PurchaseRequestID);

                    await _manager.DeleteControl(_uow.PurchaseRequestsRepository, lines.PurchaseRequestID, lines.Id, true, L);
                    await _uow.PurchaseRequestLinesRepository.DeleteAsync(id);

                    await StockMovementLineDelete(_uow, lines, entity);

                    await _uow.SaveChanges();
                    return new SuccessResult(L["DeleteSuccessMessage"]);
                }
                else
                {
                    var entity = await _uow.PurchaseRequestsRepository.GetAsync(t => t.Id == id);

                    await _manager.DeleteControl(_uow.PurchaseRequestsRepository, id, Guid.Empty, false, L);
                    var list = (await _uow.PurchaseRequestLinesRepository.GetListAsync(t => t.PurchaseRequestID == id));
                    foreach (var line in list)
                    {
                        await _uow.PurchaseRequestLinesRepository.DeleteAsync(line.Id);

                        await StockMovementDelete(_uow, entity, line);
                    }
                    await _uow.PurchaseRequestsRepository.DeleteAsync(id);
                    var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "PurchaseRequests", LogType.Delete, id);
                    await _uow.LogsRepository.InsertAsync(log);
                    await _uow.SaveChanges();
                    return new SuccessResult(L["DeleteSuccessMessage"]);
                }
            }
        }

        public async Task<IDataResult<SelectPurchaseRequestsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PurchaseRequestsRepository.GetAsync(t => t.Id == id,
                t => t.PurchaseRequestLines,
                t => t.CurrentAccountCards,
                t => t.Warehouses,
                t => t.Branches,
                t => t.Currencies,
                t => t.ShippingAdresses,
                t => t.PaymentPlan);

                var mappedEntity = ObjectMapper.Map<PurchaseRequests, SelectPurchaseRequestsDto>(entity);

                mappedEntity.SelectPurchaseRequestLines = ObjectMapper.Map<List<PurchaseRequestLines>, List<SelectPurchaseRequestLinesDto>>(entity.PurchaseRequestLines.ToList());

                foreach (var item in mappedEntity.SelectPurchaseRequestLines)
                {
                    item.ProductCode = (await _uow.ProductsRepository.GetAsync(t => t.Id == item.ProductID)).Code;
                    item.ProductName = (await _uow.ProductsRepository.GetAsync(t => t.Id == item.ProductID)).Name;
                    item.UnitSetCode = (await _uow.UnitSetsRepository.GetAsync(t => t.Id == item.UnitSetID)).Code;
                }

                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "PurchaseRequests", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectPurchaseRequestsDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPurchaseRequestsDto>>> GetListAsync(ListPurchaseRequestsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.PurchaseRequestsRepository.GetListAsync(null,
                t => t.PurchaseRequestLines,
                t => t.CurrentAccountCards,
                t => t.Warehouses,
                t => t.Branches,
                t => t.Currencies,
                t => t.ShippingAdresses,
                t => t.PaymentPlan);

                var mappedEntity = ObjectMapper.Map<List<PurchaseRequests>, List<ListPurchaseRequestsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListPurchaseRequestsDto>>(mappedEntity);
            }
        }

        [ValidationAspect(typeof(UpdatePurchaseRequestsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseRequestsDto>> UpdateAsync(UpdatePurchaseRequestsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PurchaseRequestsRepository.GetAsync(x => x.Id == input.Id, x => x.PurchaseRequestLines);

                await _manager.UpdateControl(_uow.PurchaseRequestsRepository, input.FicheNo, input.Id, entity, L);

                var mappedEntity = ObjectMapper.Map<UpdatePurchaseRequestsDto, PurchaseRequests>(input);

                await _uow.PurchaseRequestsRepository.UpdateAsync(mappedEntity);


                foreach (var item in input.SelectPurchaseRequestLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectPurchaseRequestLinesDto, PurchaseRequestLines>(item);

                    lineEntity.PurchaseRequestID = mappedEntity.Id;


                    await StockMovementInsertOrUpdate(input, _uow, entity, item, lineEntity);


                    if (lineEntity.Id == Guid.Empty)
                    {
                        await _uow.PurchaseRequestLinesRepository.InsertAsync(lineEntity);
                    }
                    else
                    {
                        await _uow.PurchaseRequestLinesRepository.UpdateAsync(lineEntity);
                    }
                }


                //throw new Exception("Hata");

                var before = ObjectMapper.Map<PurchaseRequests, UpdatePurchaseRequestsDto>(entity);
                before.SelectPurchaseRequestLines = ObjectMapper.Map<List<PurchaseRequestLines>, List<SelectPurchaseRequestLinesDto>>(entity.PurchaseRequestLines.ToList());
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "PurchaseRequests", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();


                return new SuccessDataResult<SelectPurchaseRequestsDto>(ObjectMapper.Map<PurchaseRequests, SelectPurchaseRequestsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectPurchaseRequestsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PurchaseRequestsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.PurchaseRequestsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<PurchaseRequests, SelectPurchaseRequestsDto>(updatedEntity);

                return new SuccessDataResult<SelectPurchaseRequestsDto>(mappedEntity);
            }
        }

        public async Task UpdatePurchaseRequestLineState(List<SelectPurchaseOrderLinesDto> orderLineList, PurchaseRequestLineStateEnum lineState)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                foreach (var item in orderLineList)
                {
                    var lineEntity = (await GetAsync(item.LikedPurchaseRequestLineID.GetValueOrDefault())).Data.SelectPurchaseRequestLines;

                    if (lineEntity.Count > 0)
                    {
                        foreach (var line in lineEntity)
                        {
                            var mappedLineEntity = ObjectMapper.Map<SelectPurchaseRequestLinesDto, PurchaseRequestLines>(line);
                            mappedLineEntity.PurchaseRequestLineState = lineState;
                            mappedLineEntity.OrderConversionDate = DateTime.Now;
                            await _uow.PurchaseRequestLinesRepository.UpdateAsync(mappedLineEntity);
                            await _uow.SaveChanges();
                        }
                    }
                }
            }
        }



        #region Stock Movement Transactions
        private static async Task StockMovementDelete(UnitOfWork _uow, PurchaseRequests entity, PurchaseRequestLines line)
        {
            #region ByDateStockMovement
            var byDateStockMovement = await _uow.ByDateStockMovementsRepository.GetAsync(t => t.BranchID == entity.BranchID && t.WarehouseID == entity.WarehouseID && t.ProductID == line.ProductID && t.Date_ == entity.Date_);

            if (byDateStockMovement != null)
            {
                byDateStockMovement.TotalPurchaseRequest -= line.Quantity;
            }
            #endregion

            #region GrandTotalStockMovement
            var grandTotalStockMovement = await _uow.GrandTotalStockMovementsRepository.GetAsync(t => t.BranchID == entity.BranchID && t.WarehouseID == entity.WarehouseID && t.ProductID == line.ProductID);

            if (grandTotalStockMovement != null)
            {
                grandTotalStockMovement.TotalPurchaseRequest -= line.Quantity;
            }
            #endregion
        }

        private static async Task StockMovementLineDelete(UnitOfWork _uow, PurchaseRequestLines lines, PurchaseRequests entity)
        {
            #region ByDateStockMovement
            var byDateStockMovement = await _uow.ByDateStockMovementsRepository.GetAsync(t => t.BranchID == entity.BranchID && t.WarehouseID == entity.WarehouseID && t.ProductID == lines.ProductID && t.Date_ == entity.Date_);

            if (byDateStockMovement != null)
            {
                byDateStockMovement.TotalPurchaseRequest -= lines.Quantity;
            }
            #endregion

            #region GrandTotalStockMovement
            var grandTotalStockMovement = await _uow.GrandTotalStockMovementsRepository.GetAsync(t => t.BranchID == entity.BranchID && t.WarehouseID == entity.WarehouseID && t.ProductID == lines.ProductID);

            if (grandTotalStockMovement != null)
            {
                grandTotalStockMovement.TotalPurchaseRequest -= lines.Quantity;
            }
            #endregion
        }

        private static async Task StockMovementInsert(CreatePurchaseRequestsDto input, UnitOfWork _uow, PurchaseRequestLines lineEntity)
        {
            #region ByDateStockMovement
            var byDateStockMovement = await _uow.ByDateStockMovementsRepository.GetAsync(t => t.BranchID == input.BranchID && t.WarehouseID == input.WarehouseID && t.ProductID == lineEntity.ProductID && t.Date_ == input.Date_);

            if (byDateStockMovement == null)
            {
                await _uow.ByDateStockMovementsRepository.InsertAsync(new ByDateStockMovements
                {
                    BranchID = input.BranchID.GetValueOrDefault(),
                    Date_ = input.Date_,
                    ProductID = lineEntity.ProductID,
                    TotalPurchaseOrder = 0,
                    WarehouseID = input.WarehouseID.GetValueOrDefault(),
                    TotalSalesProposition = 0,
                    TotalProduction = 0,
                    TotalSalesOrder = 0,
                    TotalWastage = 0,
                    TotalPurchaseRequest = lineEntity.Quantity,
                    TotalGoodsReceipt = 0,
                    TotalGoodsIssue = 0,
                    TotalConsumption = 0,
                    Amount = 0

                });
            }
            else
            {
                byDateStockMovement.TotalPurchaseRequest = lineEntity.Quantity;
            }
            #endregion

            #region GrandTotalStockMovement
            var grandTotalStockMovement = await _uow.GrandTotalStockMovementsRepository.GetAsync(t => t.BranchID == input.BranchID && t.WarehouseID == input.WarehouseID && t.ProductID == lineEntity.ProductID);

            if (grandTotalStockMovement == null)
            {
                await _uow.GrandTotalStockMovementsRepository.InsertAsync(new GrandTotalStockMovements
                {
                    BranchID = input.BranchID.GetValueOrDefault(),
                    ProductID = lineEntity.ProductID,
                    TotalPurchaseOrder = 0,
                    WarehouseID = input.WarehouseID.GetValueOrDefault(),
                    TotalSalesProposition = 0,
                    TotalProduction = 0,
                    TotalSalesOrder = 0,
                    TotalWastage = 0,
                    TotalPurchaseRequest = lineEntity.Quantity,
                    TotalGoodsReceipt = 0,
                    TotalGoodsIssue = 0,
                    TotalConsumption = 0,
                    Amount = 0,
                    TotalReserved = 0

                });
            }
            else
            {
                grandTotalStockMovement.TotalPurchaseRequest = lineEntity.Quantity;
            }
            #endregion
        }

        private static async Task StockMovementInsertOrUpdate(UpdatePurchaseRequestsDto input, UnitOfWork _uow, PurchaseRequests entity, SelectPurchaseRequestLinesDto item, PurchaseRequestLines lineEntity)
        {
            var oldLine = entity.PurchaseRequestLines.FirstOrDefault(t => t.Id == item.Id);

            var branchId = input.BranchID == entity.BranchID ? entity.BranchID : input.BranchID;
            var warehouseId = input.WarehouseID == entity.WarehouseID ? entity.WarehouseID : input.WarehouseID;
            var date = input.Date_ == entity.Date_ ? entity.Date_ : input.Date_;
            var productId = lineEntity.ProductID == oldLine.ProductID ? oldLine.ProductID : lineEntity.ProductID;

            #region ByDateStockMovement
            var deletedByDateStockMovement = await _uow.ByDateStockMovementsRepository.GetAsync(t => t.BranchID == entity.BranchID && t.WarehouseID == entity.WarehouseID && t.ProductID == productId && t.Date_ == entity.Date_);

            if(deletedByDateStockMovement != null) 
            {
                deletedByDateStockMovement.TotalPurchaseRequest = deletedByDateStockMovement.TotalPurchaseRequest - lineEntity.Quantity;
            }

            var byDateStockMovement = await _uow.ByDateStockMovementsRepository.GetAsync(t => t.BranchID == branchId && t.WarehouseID == warehouseId && t.ProductID == productId && t.Date_ == date);

            if (byDateStockMovement == null)
            {
                await _uow.ByDateStockMovementsRepository.InsertAsync(new ByDateStockMovements
                {
                    BranchID = branchId.GetValueOrDefault(),
                    Date_ = date,
                    ProductID = productId,
                    TotalPurchaseOrder = 0,
                    WarehouseID = warehouseId.GetValueOrDefault(),
                    TotalSalesProposition = 0,
                    TotalProduction = 0,
                    TotalSalesOrder = 0,
                    TotalWastage = 0,
                    TotalPurchaseRequest = lineEntity.Quantity,
                    TotalGoodsReceipt = 0,
                    TotalGoodsIssue = 0,
                    TotalConsumption = 0,
                    Amount = 0
                });
            }
            else
            {
                if (oldLine.Quantity > lineEntity.Quantity)
                {
                    decimal lineValue = oldLine.Quantity - lineEntity.Quantity;
                    var totalPurchaseRequest = byDateStockMovement.TotalPurchaseRequest - lineValue;
                    byDateStockMovement.TotalPurchaseRequest = totalPurchaseRequest;
                }

                if (oldLine.Quantity < lineEntity.Quantity)
                {
                    decimal lineValue = lineEntity.Quantity - oldLine.Quantity;
                    var totalPurchaseRequest = byDateStockMovement.TotalPurchaseRequest + lineValue;
                    byDateStockMovement.TotalPurchaseRequest = totalPurchaseRequest;
                }

            }
            #endregion

            #region GrandTotalStockMovement

            var deletedGrandTotalStockMovement = await _uow.GrandTotalStockMovementsRepository.GetAsync(t => t.BranchID == entity.BranchID && t.WarehouseID == entity.WarehouseID && t.ProductID == productId);

            if (deletedGrandTotalStockMovement != null)
            {
                deletedGrandTotalStockMovement.TotalPurchaseRequest = deletedGrandTotalStockMovement.TotalPurchaseRequest - lineEntity.Quantity;
            }


            var grandTotalStockMovement = await _uow.GrandTotalStockMovementsRepository.GetAsync(t => t.BranchID == branchId && t.WarehouseID == warehouseId && t.ProductID == productId);

            if (grandTotalStockMovement == null)
            {
                await _uow.GrandTotalStockMovementsRepository.InsertAsync(new GrandTotalStockMovements
                {
                    BranchID = branchId.GetValueOrDefault(),
                    ProductID = productId,
                    TotalPurchaseOrder = 0,
                    WarehouseID = warehouseId.GetValueOrDefault(),
                    TotalSalesProposition = 0,
                    TotalProduction = 0,
                    TotalSalesOrder = 0,
                    TotalWastage = 0,
                    TotalPurchaseRequest = lineEntity.Quantity,
                    TotalGoodsReceipt = 0,
                    TotalGoodsIssue = 0,
                    TotalConsumption = 0,
                    Amount = 0,
                    TotalReserved = 0
                });
            }
            else
            {
                if (oldLine.Quantity > lineEntity.Quantity)
                {
                    decimal lineValue = oldLine.Quantity - lineEntity.Quantity;
                    var totalPurchaseRequest = grandTotalStockMovement.TotalPurchaseRequest - lineValue;
                    grandTotalStockMovement.TotalPurchaseRequest = totalPurchaseRequest;
                }

                if (oldLine.Quantity < lineEntity.Quantity)
                {
                    decimal lineValue = lineEntity.Quantity - oldLine.Quantity;
                    var totalPurchaseRequest = grandTotalStockMovement.TotalPurchaseRequest + lineValue;
                    grandTotalStockMovement.TotalPurchaseRequest = totalPurchaseRequest;
                }
            }
            #endregion
        } 
        #endregion
    }
}