using Tsi.Core.Utilities.Guids;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.Other.ByDateStockMovement;
using TsiErp.Entities.Entities.Other.ByDateStockMovement.Dtos;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequest;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequest.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequestLine;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequestLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesProposition;
using TsiErp.Entities.Entities.SalesManagement.SalesProposition.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesPropositionLine;
using TsiErp.Entities.Entities.SalesManagement.SalesPropositionLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.StockFiche;
using TsiErp.Entities.Entities.StockManagement.StockFiche.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.TableConstant;

namespace TsiErp.Business.Entities.StockMovement
{
    public static class StockMovementsService
    {
        static QueryFactory queryFactory { get; set; } = new QueryFactory();

        public static IGuidGenerator GuidGenerator { get; set; } = new SequentialGuidGenerator();
        private static IGetSQLDateAppService _GetSQLDateAppService = new GetSQLDateAppService();

        #region Purchase Requests

        public static bool InsertPurchaseRequests(CreatePurchaseRequestsDto createdEntity)
        {
            foreach (var line in createdEntity.SelectPurchaseRequestLines)
            {
                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = createdEntity.BranchID, WarehouseID = createdEntity.WarehouseID, ProductID = line.ProductID, Date_ = createdEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);
                DateTime now = _GetSQLDateAppService.GetDateFromSQL();

                if (entityByDate.Id == Guid.Empty || entityByDate == null)
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                    {
                        Amount = 0,
                        Date_ = createdEntity.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = 0,
                        TotalWarehouseInput = 0,
                        TotalWarehouseOutput = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = line.Quantity,
                        TotalSalesOrder = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var byDateStockMovements = queryFactory.Insert<SelectByDateStockMovementsDto>(query, "Id", true);
                }

                else
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                    {
                        Amount = entityByDate.Amount,
                        Date_ = entityByDate.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityByDate.TotalConsumption,
                        TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                        TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                        TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                        TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                        TotalProduction = entityByDate.TotalProduction,
                        TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityByDate.TotalPurchaseRequest + line.Quantity,
                        TotalSalesOrder = entityByDate.TotalSalesOrder,
                        TotalSalesProposition = entityByDate.TotalSalesProposition,
                        TotalWastage = entityByDate.TotalWastage,
                        WarehouseID = entityByDate.WarehouseID,
                        BranchID = entityByDate.BranchID,
                        Id = entityByDate.Id,
                        CreationTime = entityByDate.CreationTime.Value,
                        CreatorId = entityByDate.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityByDate.DeleterId.Value,
                        DeletionTime = entityByDate.DeletionTime.Value,
                        IsDeleted = entityByDate.IsDeleted,
                        LastModificationTime = now,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityByDate.Id }, "");

                    var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                }

                #endregion

                #region Grand Total Stock Movement


                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = createdEntity.BranchID, WarehouseID = createdEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id == Guid.Empty || entityGrandTotal == null)
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                    {
                        Amount = 0,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = 0,
                        TotalWarehouseInput = 0,
                        TotalWarehouseOutput = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = line.Quantity,
                        TotalSalesOrder = 0,
                        TotalReserved = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                else
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                    {
                        Amount = entityGrandTotal.Amount,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityGrandTotal.TotalConsumption,
                        TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                        TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                        TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                        TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                        TotalProduction = entityGrandTotal.TotalProduction,
                        TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest + line.Quantity,
                        TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                        TotalReserved = entityGrandTotal.TotalReserved,
                        TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                        TotalWastage = entityGrandTotal.TotalWastage,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        Id = entityGrandTotal.Id,
                        CreationTime = entityGrandTotal.CreationTime.Value,
                        CreatorId = entityGrandTotal.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityGrandTotal.DeleterId.Value,
                        DeletionTime = entityGrandTotal.DeletionTime.Value,
                        IsDeleted = entityGrandTotal.IsDeleted,
                        LastModificationTime = now,
                        BranchID = createdEntity.BranchID,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityGrandTotal.Id }, "");

                    var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                #endregion


            }

            return true;
        }

        public static bool UpdatePurchaseRequests(SelectPurchaseRequestsDto previousEntity, UpdatePurchaseRequestsDto currentEntity)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            foreach (var line in currentEntity.SelectPurchaseRequestLines)
            {

                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = currentEntity.BranchID, WarehouseID = currentEntity.WarehouseID, ProductID = line.ProductID, Date_ = currentEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id == Guid.Empty)
                {

                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                    {
                        Amount = 0,
                        Date_ = currentEntity.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = 0,
                        TotalWarehouseInput = 0,
                        TotalWarehouseOutput = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = line.Quantity,
                        TotalSalesOrder = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = currentEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = currentEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var byDateStockMovements = queryFactory.Insert<SelectByDateStockMovementsDto>(query, "Id", true);


                }

                else
                {
                    decimal previousQuantity = previousEntity.SelectPurchaseRequestLines.Where(t => t.Id == line.Id).Select(t => t.Quantity).First();

                    if (line.Quantity > previousQuantity)
                    {
                        decimal addedPR = line.Quantity - previousQuantity;

                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                        {
                            Amount = entityByDate.Amount,
                            Date_ = entityByDate.Date_,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityByDate.TotalConsumption,
                            TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                            TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                            TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                            TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                            TotalProduction = entityByDate.TotalProduction,
                            TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityByDate.TotalPurchaseRequest + addedPR,
                            TotalSalesOrder = entityByDate.TotalSalesOrder,
                            TotalSalesProposition = entityByDate.TotalSalesProposition,
                            TotalWastage = entityByDate.TotalWastage,
                            WarehouseID = entityByDate.WarehouseID,
                            BranchID = entityByDate.BranchID,
                            Id = entityByDate.Id,
                            CreationTime = entityByDate.CreationTime.Value,
                            CreatorId = entityByDate.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityByDate.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityByDate.Id }, "");

                        var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);

                    }
                    else if (line.Quantity < previousQuantity)
                    {
                        decimal decreasedPR = previousQuantity - line.Quantity;

                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                        {
                            Amount = entityByDate.Amount,
                            Date_ = entityByDate.Date_,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityByDate.TotalConsumption,
                            TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                            TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                            TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                            TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                            TotalProduction = entityByDate.TotalProduction,
                            TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityByDate.TotalPurchaseRequest - decreasedPR,
                            TotalSalesOrder = entityByDate.TotalSalesOrder,
                            TotalSalesProposition = entityByDate.TotalSalesProposition,
                            TotalWastage = entityByDate.TotalWastage,
                            WarehouseID = entityByDate.WarehouseID,
                            BranchID = entityByDate.BranchID,
                            Id = entityByDate.Id,
                            CreationTime = entityByDate.CreationTime.Value,
                            CreatorId = entityByDate.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityByDate.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityByDate.Id }, "");

                        var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                    }

                }

                #endregion

                #region Grand Total Stock Movement

                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = currentEntity.BranchID, WarehouseID = currentEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id == Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                    {
                        Amount = 0,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = 0,
                        TotalWarehouseOutput = 0,
                        TotalWarehouseInput = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = line.Quantity,
                        TotalSalesOrder = 0,
                        TotalReserved = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = currentEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = currentEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }


                else
                {
                    decimal previousQuantity = previousEntity.SelectPurchaseRequestLines.Where(t => t.Id == line.Id).Select(t => t.Quantity).First();

                    if (line.Quantity > previousQuantity)
                    {
                        decimal addedPR = line.Quantity - previousQuantity;

                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                            TotalWarehouseInput = entityGrandTotal.TotalWarehouseOutput,
                            TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                            TotalProduction = entityGrandTotal.TotalProduction,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest + addedPR,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                            TotalReserved = entityGrandTotal.TotalReserved,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                            TotalWastage = entityGrandTotal.TotalWastage,
                            WarehouseID = entityGrandTotal.WarehouseID,
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = now,
                            BranchID = entityGrandTotal.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);

                    }
                    else if (line.Quantity < previousQuantity)
                    {
                        decimal decreasedPR = previousQuantity - line.Quantity;

                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalWarehouseInput = entityGrandTotal.TotalWarehouseOutput,
                            TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                            TotalProduction = entityGrandTotal.TotalProduction,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest - decreasedPR,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                            TotalReserved = entityGrandTotal.TotalReserved,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                            TotalWastage = entityGrandTotal.TotalWastage,
                            WarehouseID = entityGrandTotal.WarehouseID,
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = now,
                            BranchID = entityGrandTotal.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                    }

                    #endregion
                }

            }

            return true;
        }

        public static bool DeletePurchaseRequestLines(SelectPurchaseRequestLinesDto deletedLine)
        {
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            #region Getting Purchase Request

            var query = queryFactory
                 .Query()
                 .From(Tables.PurchaseRequests)
                 .Select<PurchaseRequests>(null)
                 .Join<PaymentPlans>
                  (
                      pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                      nameof(PurchaseRequests.PaymentPlanID),
                      nameof(PaymentPlans.Id),
                      JoinType.Left
                  )
                  .Join<Branches>
                  (
                      b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                      nameof(PurchaseRequests.BranchID),
                      nameof(Branches.Id),
                      JoinType.Left
                  )
                  .Join<Warehouses>
                  (
                      w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                      nameof(PurchaseRequests.WarehouseID),
                      nameof(Warehouses.Id),
                      JoinType.Left
                  )
                  .Join<Currencies>
                  (
                      c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                      nameof(PurchaseRequests.CurrencyID),
                      nameof(Currencies.Id),
                      JoinType.Left
                  )
                  .Join<CurrentAccountCards>
                  (
                      ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                      nameof(PurchaseRequests.CurrentAccountCardID),
                      nameof(CurrentAccountCards.Id),
                      JoinType.Left
                  )

                  .Where(new { Id = deletedLine.PurchaseRequestID }, Tables.PurchaseRequests);

            var purchaseRequests = queryFactory.Get<SelectPurchaseRequestsDto>(query);

            #endregion

            #region By Date Stock Movement

            var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = purchaseRequests.BranchID, WarehouseID = purchaseRequests.WarehouseID, ProductID = deletedLine.ProductID, Date_ = purchaseRequests.Date_ }, "");

            var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

            var queryDecreasingDate = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
            {
                Amount = entityByDate.Amount,
                Date_ = entityByDate.Date_,
                ProductID = entityByDate.ProductID,
                TotalConsumption = entityByDate.TotalConsumption,
                TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                TotalProduction = entityByDate.TotalProduction,
                TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                TotalPurchaseRequest = entityByDate.TotalPurchaseRequest - deletedLine.Quantity,
                TotalSalesOrder = entityByDate.TotalSalesOrder,
                TotalSalesProposition = entityByDate.TotalSalesProposition,
                TotalWastage = entityByDate.TotalWastage,
                WarehouseID = entityByDate.WarehouseID,
                BranchID = entityByDate.BranchID,
                Id = entityByDate.Id,
                CreationTime = entityByDate.CreationTime.Value,
                CreatorId = entityByDate.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                IsDeleted = entityByDate.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = entityByDate.Id }, "");

            var byDateStockMovementsDecreasing = queryFactory.Update<SelectByDateStockMovementsDto>(queryDecreasingDate, "Id", true);

            #endregion

            #region Grand Total Stock Movement

            var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = purchaseRequests.BranchID, WarehouseID = purchaseRequests.WarehouseID, ProductID = deletedLine.ProductID }, "");

            var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

            var queryDecreasingGrand = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
            {
                Amount = entityGrandTotal.Amount,
                ProductID = deletedLine.ProductID.GetValueOrDefault(),
                TotalConsumption = entityGrandTotal.TotalConsumption,
                TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                TotalProduction = entityGrandTotal.TotalProduction,
                TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest - deletedLine.Quantity,
                TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                TotalReserved = entityGrandTotal.TotalReserved,
                TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                TotalWastage = entityGrandTotal.TotalWastage,
                WarehouseID = entityGrandTotal.WarehouseID,
                Id = entityGrandTotal.Id,
                CreationTime = entityGrandTotal.CreationTime.Value,
                CreatorId = entityGrandTotal.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                IsDeleted = entityGrandTotal.IsDeleted,
                LastModificationTime = now,
                BranchID = entityGrandTotal.BranchID,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = entityGrandTotal.Id }, "");

            var grandTotalStockMovementsDecreasing = queryFactory.Update<SelectGrandTotalStockMovementsDto>(queryDecreasingGrand, "Id", true);

            #endregion

            return true;
        }

        public static bool DeletePurchaseRequests(SelectPurchaseRequestsDto deletedEntity)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();
            #region Getting Lines

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PurchaseRequestLines)
                   .Select<PurchaseRequestLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchaseRequestLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(PurchaseRequestLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        ppl => new { PaymentPlanID = ppl.Id, PaymentPlanName = ppl.Name },
                        nameof(PurchaseRequestLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Where(new { PurchaseRequestID = deletedEntity.Id }, Tables.PurchaseRequestLines);


            var purchaseRequestLine = queryFactory.GetList<SelectPurchaseRequestLinesDto>(queryLines).ToList();

            deletedEntity.SelectPurchaseRequestLines = purchaseRequestLine;

            #endregion

            foreach (var line in deletedEntity.SelectPurchaseRequestLines)
            {

                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = deletedEntity.BranchID, WarehouseID = deletedEntity.WarehouseID, ProductID = line.ProductID, Date_ = deletedEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id != Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                    {
                        Amount = entityByDate.Amount,
                        Date_ = entityByDate.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityByDate.TotalConsumption,
                        TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                        TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                        TotalProduction = entityByDate.TotalProduction,
                        TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                        TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                        TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityByDate.TotalPurchaseRequest - line.Quantity,
                        TotalSalesOrder = entityByDate.TotalSalesOrder,
                        TotalSalesProposition = entityByDate.TotalSalesProposition,
                        TotalWastage = entityByDate.TotalWastage,
                        WarehouseID = entityByDate.WarehouseID,
                        BranchID = entityByDate.BranchID,
                        Id = entityByDate.Id,
                        CreationTime = entityByDate.CreationTime.Value,
                        CreatorId = entityByDate.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityByDate.IsDeleted,
                        LastModificationTime = now,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityByDate.Id }, "");

                    var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                }

                #endregion

                #region Grand Total Stock Movement

                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = deletedEntity.BranchID, WarehouseID = deletedEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id != Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                    {
                        Amount = entityGrandTotal.Amount,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityGrandTotal.TotalConsumption,
                        TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                        TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                        TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                        TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                        TotalProduction = entityGrandTotal.TotalProduction,
                        TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest - line.Quantity,
                        TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                        TotalReserved = entityGrandTotal.TotalReserved,
                        TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                        TotalWastage = entityGrandTotal.TotalWastage,
                        WarehouseID = entityGrandTotal.WarehouseID,
                        Id = entityGrandTotal.Id,
                        CreationTime = entityGrandTotal.CreationTime.Value,
                        CreatorId = entityGrandTotal.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityGrandTotal.IsDeleted,
                        LastModificationTime = now,
                        BranchID = entityGrandTotal.BranchID,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityGrandTotal.Id }, "");

                    var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                #endregion

            }

            return true;
        }

        #endregion

        #region Purchase Orders

        public static bool InsertPurchaseOrders(CreatePurchaseOrdersDto createdEntity)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            foreach (var line in createdEntity.SelectPurchaseOrderLinesDto)
            {
                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = createdEntity.BranchID, WarehouseID = createdEntity.WarehouseID, ProductID = line.ProductID, Date_ = createdEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id == Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                    {
                        Amount = 0,
                        Date_ = createdEntity.Date_,
                        ProductID = line.ProductID,
                        TotalConsumption = 0,
                        TotalGoodsIssue = 0,
                        TotalWarehouseInput = 0,
                        TotalWarehouseOutput = 0,
                        TotalGoodsReceipt = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = line.Quantity,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = createdEntity.WarehouseID,
                        BranchID = createdEntity.BranchID,
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
                    });

                    var byDateStockMovements = queryFactory.Insert<SelectByDateStockMovementsDto>(query, "Id", true);
                }
                else
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                    {
                        Amount = entityByDate.Amount,
                        Date_ = entityByDate.Date_,
                        ProductID = line.ProductID,
                        TotalConsumption = entityByDate.TotalConsumption,
                        TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                        TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                        TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                        TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                        TotalProduction = entityByDate.TotalProduction,
                        TotalPurchaseOrder = entityByDate.TotalPurchaseOrder + line.Quantity,
                        TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                        TotalSalesOrder = entityByDate.TotalSalesOrder,
                        TotalSalesProposition = entityByDate.TotalSalesProposition,
                        TotalWastage = entityByDate.TotalWastage,
                        WarehouseID = entityByDate.WarehouseID,
                        BranchID = entityByDate.BranchID,
                        Id = entityByDate.Id,
                        CreationTime = entityByDate.CreationTime.Value,
                        CreatorId = entityByDate.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityByDate.IsDeleted,
                        LastModificationTime = now,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityByDate.Id }, "");

                    var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                }

                #endregion

                #region Grand Total Stock Movement

                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = createdEntity.BranchID, WarehouseID = createdEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id == Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                    {
                        Amount = 0,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = 0,
                        TotalWarehouseInput = 0,
                        TotalWarehouseOutput = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = line.Quantity,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalReserved = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                else
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                    {
                        Amount = entityGrandTotal.Amount,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityGrandTotal.TotalConsumption,
                        TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                        TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                        TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                        TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                        TotalProduction = entityGrandTotal.TotalProduction,
                        TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder + line.Quantity,
                        TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                        TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                        TotalReserved = entityGrandTotal.TotalReserved,
                        TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                        TotalWastage = entityGrandTotal.TotalWastage,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        Id = entityGrandTotal.Id,
                        CreationTime = entityGrandTotal.CreationTime.Value,
                        CreatorId = entityGrandTotal.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityGrandTotal.IsDeleted,
                        LastModificationTime = now,
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityGrandTotal.Id }, "");

                    var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                #endregion

            }

            return true;
        }

        public static bool UpdatePurchaseOrders(SelectPurchaseOrdersDto previousEntity, UpdatePurchaseOrdersDto currentEntity)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            foreach (var line in currentEntity.SelectPurchaseOrderLinesDto)
            {

                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = currentEntity.BranchID, WarehouseID = currentEntity.WarehouseID, ProductID = line.ProductID, Date_ = currentEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id == Guid.Empty)
                {


                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                    {
                        Amount = 0,
                        Date_ = currentEntity.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = 0,
                        TotalWarehouseOutput = 0,
                        TotalWarehouseInput = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = line.Quantity,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = currentEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = currentEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var byDateStockMovements = queryFactory.Insert<SelectByDateStockMovementsDto>(query, "Id", true);


                }
                else
                {
                    decimal previousQuantity = previousEntity.SelectPurchaseOrderLinesDto.Where(t => t.Id == line.Id).Select(t => t.Quantity).First();

                    if (line.Quantity > previousQuantity)
                    {
                        decimal addedPR = line.Quantity - previousQuantity;

                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                        {
                            Amount = entityByDate.Amount,
                            Date_ = entityByDate.Date_,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityByDate.TotalConsumption,
                            TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                            TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                            TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                            TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                            TotalProduction = entityByDate.TotalProduction,
                            TotalPurchaseOrder = entityByDate.TotalPurchaseOrder + addedPR,
                            TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                            TotalSalesOrder = entityByDate.TotalSalesOrder,
                            TotalSalesProposition = entityByDate.TotalSalesProposition,
                            TotalWastage = entityByDate.TotalWastage,
                            WarehouseID = entityByDate.WarehouseID,
                            BranchID = entityByDate.BranchID,
                            Id = entityByDate.Id,
                            CreationTime = entityByDate.CreationTime.Value,
                            CreatorId = entityByDate.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityByDate.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityByDate.Id }, "");

                        var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                    }

                    else if (line.Quantity < previousQuantity)
                    {
                        decimal decreasedPR = previousQuantity - line.Quantity;

                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                        {
                            Amount = entityByDate.Amount,
                            Date_ = entityByDate.Date_,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityByDate.TotalConsumption,
                            TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                            TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                            TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                            TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                            TotalProduction = entityByDate.TotalProduction,
                            TotalPurchaseOrder = entityByDate.TotalPurchaseOrder - decreasedPR,
                            TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                            TotalSalesOrder = entityByDate.TotalSalesOrder,
                            TotalSalesProposition = entityByDate.TotalSalesProposition,
                            TotalWastage = entityByDate.TotalWastage,
                            WarehouseID = entityByDate.WarehouseID,
                            BranchID = entityByDate.BranchID,
                            Id = entityByDate.Id,
                            CreationTime = entityByDate.CreationTime.Value,
                            CreatorId = entityByDate.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityByDate.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityByDate.Id }, "");

                        var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                    }

                }

                #endregion

                #region Grand Total Stock Movement

                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = currentEntity.BranchID, WarehouseID = currentEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id == Guid.Empty)
                {


                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                    {
                        Amount = 0,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = 0,
                        TotalWarehouseOutput = 0,
                        TotalWarehouseInput = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = line.Quantity,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalReserved = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = currentEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = currentEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);

                }

                else
                {
                    decimal previousQuantity = previousEntity.SelectPurchaseOrderLinesDto.Where(t => t.Id == line.Id).Select(t => t.Quantity).First();

                    if (line.Quantity > previousQuantity)
                    {
                        decimal addedPR = line.Quantity - previousQuantity;

                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                            TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                            TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                            TotalProduction = entityGrandTotal.TotalProduction,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder + addedPR,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                            TotalReserved = entityGrandTotal.TotalReserved,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                            TotalWastage = entityGrandTotal.TotalWastage,
                            WarehouseID = entityGrandTotal.WarehouseID,
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = now,
                            BranchID = entityGrandTotal.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);

                    }

                    else if (line.Quantity < previousQuantity)
                    {
                        decimal decreasedPR = previousQuantity - line.Quantity;

                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                            TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                            TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                            TotalProduction = entityGrandTotal.TotalProduction,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder - decreasedPR,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                            TotalReserved = entityGrandTotal.TotalReserved,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                            TotalWastage = entityGrandTotal.TotalWastage,
                            WarehouseID = entityGrandTotal.WarehouseID,
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = now,
                            BranchID = entityGrandTotal.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                    }
                }

                #endregion

            }

            return true;
        }

        public static bool DeletePurchaseOrderLines(SelectPurchaseOrderLinesDto deletedLine)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            #region Getting Purchase Order

            var query = queryFactory
                  .Query()
                  .From(Tables.PurchaseOrders)
                  .Select<PurchaseOrders>(null)
                  .Join<PaymentPlans>
                   (
                       pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                       nameof(PurchaseOrders.PaymentPlanID),
                       nameof(PaymentPlans.Id),
                       JoinType.Left
                   )
                   .Join<Branches>
                   (
                       b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                       nameof(PurchaseOrders.BranchID),
                       nameof(Branches.Id),
                       JoinType.Left
                   )
                    .Join<Warehouses>
                   (
                       w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                       nameof(PurchaseOrders.WarehouseID),
                       nameof(Warehouses.Id),
                       JoinType.Left
                   )
                    .Join<Currencies>
                   (
                       c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                       nameof(PurchaseOrders.CurrencyID),
                       nameof(Currencies.Id),
                       JoinType.Left
                   )
                    .Join<CurrentAccountCards>
                   (
                       ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                       nameof(PurchaseOrders.CurrentAccountCardID),
                       nameof(CurrentAccountCards.Id),
                       JoinType.Left)

                        .Join<ShippingAdresses>
                   (
                       sa => new { ShippingAdressID = sa.Id, ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                       nameof(PurchaseOrders.ShippingAdressID),
                       nameof(ShippingAdresses.Id),
                       JoinType.Left
                   )
                   .Where(new { Id = deletedLine.PurchaseOrderID }, Tables.PurchaseOrders);

            var purchaseOrders = queryFactory.Get<SelectPurchaseOrdersDto>(query);

            #endregion

            #region By Date Stock Movement

            var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = purchaseOrders.BranchID, WarehouseID = purchaseOrders.WarehouseID, ProductID = deletedLine.ProductID, Date_ = purchaseOrders.Date_ }, "");

            var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

            var queryDecreasingDate = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
            {
                Amount = entityByDate.Amount,
                Date_ = entityByDate.Date_,
                ProductID = entityByDate.ProductID,
                TotalConsumption = entityByDate.TotalConsumption,
                TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                TotalProduction = entityByDate.TotalProduction,
                TotalPurchaseOrder = entityByDate.TotalPurchaseOrder - deletedLine.Quantity,
                TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                TotalSalesOrder = entityByDate.TotalSalesOrder,
                TotalSalesProposition = entityByDate.TotalSalesProposition,
                TotalWastage = entityByDate.TotalWastage,
                WarehouseID = entityByDate.WarehouseID,
                BranchID = entityByDate.BranchID,
                Id = entityByDate.Id,
                CreationTime = entityByDate.CreationTime.Value,
                CreatorId = entityByDate.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                IsDeleted = entityByDate.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = entityByDate.Id }, "");

            var byDateStockMovementsDecreasing = queryFactory.Update<SelectByDateStockMovementsDto>(queryDecreasingDate, "Id", true);

            #endregion

            #region Grand Total Stock Movement

            var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = purchaseOrders.BranchID, WarehouseID = purchaseOrders.WarehouseID, ProductID = deletedLine.ProductID }, "");

            var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

            var queryDecreasingGrand = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
            {
                Amount = entityGrandTotal.Amount,
                ProductID = deletedLine.ProductID.GetValueOrDefault(),
                TotalConsumption = entityGrandTotal.TotalConsumption,
                TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                TotalProduction = entityGrandTotal.TotalProduction,
                TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder - deletedLine.Quantity,
                TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                TotalReserved = entityGrandTotal.TotalReserved,
                TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                TotalWastage = entityGrandTotal.TotalWastage,
                WarehouseID = entityGrandTotal.WarehouseID,
                Id = entityGrandTotal.Id,
                CreationTime = entityGrandTotal.CreationTime.Value,
                CreatorId = entityGrandTotal.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                IsDeleted = entityGrandTotal.IsDeleted,
                LastModificationTime = now,
                BranchID = entityGrandTotal.BranchID,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = entityGrandTotal.Id }, "");

            var grandTotalStockMovementsDecreasing = queryFactory.Update<SelectGrandTotalStockMovementsDto>(queryDecreasingGrand, "Id", true);

            #endregion

            return true;
        }

        public static bool DeletePurchaseOrders(SelectPurchaseOrdersDto deletedEntity)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            #region Getting Lines

            var queryLines = queryFactory
                    .Query()
                    .From(Tables.PurchaseOrderLines)
                    .Select<PurchaseOrderLines>(null)
                    .Join<Products>
                     (
                         p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                         nameof(PurchaseOrderLines.ProductID),
                         nameof(Products.Id),
                         JoinType.Left
                     )
                    .Join<UnitSets>
                     (
                         u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                         nameof(PurchaseOrderLines.UnitSetID),
                         nameof(UnitSets.Id),
                         JoinType.Left
                     )
                      .Join<PaymentPlans>
                     (
                         pay => new { PaymentPlanID = pay.Id, PaymentPlanName = pay.Name },
                         nameof(PurchaseOrderLines.PaymentPlanID),
                         nameof(PaymentPlans.Id),
                         JoinType.Left
                     )
                     .Where(new { PurchaseOrderID = deletedEntity.Id }, Tables.PurchaseOrderLines);

            var purchaseOrderLine = queryFactory.GetList<SelectPurchaseOrderLinesDto>(queryLines).ToList();

            deletedEntity.SelectPurchaseOrderLinesDto = purchaseOrderLine;

            #endregion

            foreach (var line in deletedEntity.SelectPurchaseOrderLinesDto)
            {

                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = deletedEntity.BranchID, WarehouseID = deletedEntity.WarehouseID, ProductID = line.ProductID, Date_ = deletedEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id != Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                    {
                        Amount = entityByDate.Amount,
                        Date_ = entityByDate.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityByDate.TotalConsumption,
                        TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                        TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                        TotalProduction = entityByDate.TotalProduction,
                        TotalPurchaseOrder = entityByDate.TotalPurchaseOrder - line.Quantity,
                        TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                        TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                        TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                        TotalSalesOrder = entityByDate.TotalSalesOrder,
                        TotalSalesProposition = entityByDate.TotalSalesProposition,
                        TotalWastage = entityByDate.TotalWastage,
                        WarehouseID = entityByDate.WarehouseID,
                        BranchID = entityByDate.BranchID,
                        Id = entityByDate.Id,
                        CreationTime = entityByDate.CreationTime.Value,
                        CreatorId = entityByDate.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityByDate.IsDeleted,
                        LastModificationTime = now,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityByDate.Id }, "");

                    var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                }

                #endregion

                #region Grand Total Stock Movement

                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = deletedEntity.BranchID, WarehouseID = deletedEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id != Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                    {
                        Amount = entityGrandTotal.Amount,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityGrandTotal.TotalConsumption,
                        TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                        TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                        TotalProduction = entityGrandTotal.TotalProduction,
                        TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                        TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                        TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder - line.Quantity,
                        TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                        TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                        TotalReserved = entityGrandTotal.TotalReserved,
                        TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                        TotalWastage = entityGrandTotal.TotalWastage,
                        WarehouseID = entityGrandTotal.WarehouseID,
                        Id = entityGrandTotal.Id,
                        CreationTime = entityGrandTotal.CreationTime.Value,
                        CreatorId = entityGrandTotal.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityGrandTotal.IsDeleted,
                        LastModificationTime = now,
                        BranchID = entityGrandTotal.BranchID,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityGrandTotal.Id }, "");

                    var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                #endregion

            }

            return true;
        }

        #endregion

        #region Sales Propositions

        public static bool InsertSalesPropositions(CreateSalesPropositionsDto createdEntity)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            foreach (var line in createdEntity.SelectSalesPropositionLines)
            {
                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = createdEntity.BranchID, WarehouseID = createdEntity.WarehouseID, ProductID = line.ProductID, Date_ = createdEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id == Guid.Empty || entityByDate == null)
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                    {
                        Amount = 0,
                        Date_ = createdEntity.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = 0,
                        TotalWarehouseOutput = 0,
                        TotalWarehouseInput = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalSalesProposition = line.Quantity,
                        TotalWastage = 0,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var byDateStockMovements = queryFactory.Insert<SelectByDateStockMovementsDto>(query, "Id", true);
                }

                else
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                    {
                        Amount = entityByDate.Amount,
                        Date_ = entityByDate.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityByDate.TotalConsumption,
                        TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                        TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                        TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                        TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                        TotalProduction = entityByDate.TotalProduction,
                        TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                        TotalSalesOrder = entityByDate.TotalSalesOrder,
                        TotalSalesProposition = entityByDate.TotalSalesProposition + line.Quantity,
                        TotalWastage = entityByDate.TotalWastage,
                        WarehouseID = entityByDate.WarehouseID,
                        BranchID = entityByDate.BranchID,
                        Id = entityByDate.Id,
                        CreationTime = entityByDate.CreationTime.Value,
                        CreatorId = entityByDate.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityByDate.IsDeleted,
                        LastModificationTime = now,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityByDate.Id }, "");

                    var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                }

                #endregion

                #region Grand Total Stock Movement


                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = createdEntity.BranchID, WarehouseID = createdEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id == Guid.Empty || entityGrandTotal == null)
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                    {
                        Amount = 0,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = 0,
                        TotalProduction = 0,
                        TotalWarehouseOutput = 0,
                        TotalWarehouseInput = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalReserved = 0,
                        TotalSalesProposition = line.Quantity,
                        TotalWastage = 0,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                else
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                    {
                        Amount = entityGrandTotal.Amount,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityGrandTotal.TotalConsumption,
                        TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                        TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                        TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                        TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                        TotalProduction = entityGrandTotal.TotalProduction,
                        TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                        TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                        TotalReserved = entityGrandTotal.TotalReserved,
                        TotalSalesProposition = entityGrandTotal.TotalSalesProposition + line.Quantity,
                        TotalWastage = entityGrandTotal.TotalWastage,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        Id = entityGrandTotal.Id,
                        CreationTime = entityGrandTotal.CreationTime.Value,
                        CreatorId = entityGrandTotal.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityGrandTotal.IsDeleted,
                        LastModificationTime = now,
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityGrandTotal.Id }, "");

                    var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                #endregion

            }

            return true;
        }

        public static bool UpdateSalesPropositions(SelectSalesPropositionsDto previousEntity, UpdateSalesPropositionsDto currentEntity)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            foreach (var line in currentEntity.SelectSalesPropositionLines)
            {

                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = currentEntity.BranchID, WarehouseID = currentEntity.WarehouseID, ProductID = line.ProductID, Date_ = currentEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id == Guid.Empty)
                {


                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                    {
                        Amount = 0,
                        Date_ = currentEntity.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalWarehouseInput = 0,
                        TotalWarehouseOutput = 0,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalSalesProposition = line.Quantity,
                        TotalWastage = 0,
                        WarehouseID = currentEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = currentEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var byDateStockMovements = queryFactory.Insert<SelectByDateStockMovementsDto>(query, "Id", true);


                }

                else
                {
                    if (line.Id == Guid.Empty)
                    {
                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                        {
                            Amount = entityByDate.Amount,
                            Date_ = entityByDate.Date_,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityByDate.TotalConsumption,
                            TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                            TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                            TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                            TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                            TotalProduction = entityByDate.TotalProduction,
                            TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                            TotalSalesOrder = entityByDate.TotalSalesOrder,
                            TotalSalesProposition = entityByDate.TotalSalesProposition + line.Quantity,
                            TotalWastage = entityByDate.TotalWastage,
                            WarehouseID = entityByDate.WarehouseID,
                            BranchID = entityByDate.BranchID,
                            Id = entityByDate.Id,
                            CreationTime = entityByDate.CreationTime.Value,
                            CreatorId = entityByDate.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityByDate.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityByDate.Id }, "");

                        var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                    }
                    else
                    {
                        decimal previousQuantity = previousEntity.SelectSalesPropositionLines.Where(t => t.Id == line.Id).Select(t => t.Quantity).First();

                        if (line.Quantity > previousQuantity)
                        {
                            decimal addedPR = line.Quantity - previousQuantity;

                            var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                            {
                                Amount = entityByDate.Amount,
                                Date_ = entityByDate.Date_,
                                ProductID = line.ProductID.GetValueOrDefault(),
                                TotalConsumption = entityByDate.TotalConsumption,
                                TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                                TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                                TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                                TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                                TotalProduction = entityByDate.TotalProduction,
                                TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                                TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                                TotalSalesOrder = entityByDate.TotalSalesOrder,
                                TotalSalesProposition = entityByDate.TotalSalesProposition + addedPR,
                                TotalWastage = entityByDate.TotalWastage,
                                WarehouseID = entityByDate.WarehouseID,
                                BranchID = entityByDate.BranchID,
                                Id = entityByDate.Id,
                                CreationTime = entityByDate.CreationTime.Value,
                                CreatorId = entityByDate.CreatorId.Value,
                                DataOpenStatus = false,
                                DataOpenStatusUserId = Guid.Empty,
                                DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                                DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                                IsDeleted = entityByDate.IsDeleted,
                                LastModificationTime = now,
                                LastModifierId = LoginedUserService.UserId
                            }).Where(new { Id = entityByDate.Id }, "");

                            var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);

                        }
                        else if (line.Quantity < previousQuantity)
                        {
                            decimal decreasedPR = previousQuantity - line.Quantity;

                            var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                            {
                                Amount = entityByDate.Amount,
                                Date_ = entityByDate.Date_,
                                ProductID = line.ProductID.GetValueOrDefault(),
                                TotalConsumption = entityByDate.TotalConsumption,
                                TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                                TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                                TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                                TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                                TotalProduction = entityByDate.TotalProduction,
                                TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                                TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                                TotalSalesOrder = entityByDate.TotalSalesOrder,
                                TotalSalesProposition = entityByDate.TotalSalesProposition - decreasedPR,
                                TotalWastage = entityByDate.TotalWastage,
                                WarehouseID = entityByDate.WarehouseID,
                                BranchID = entityByDate.BranchID,
                                Id = entityByDate.Id,
                                CreationTime = entityByDate.CreationTime.Value,
                                CreatorId = entityByDate.CreatorId.Value,
                                DataOpenStatus = false,
                                DataOpenStatusUserId = Guid.Empty,
                                DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                                DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                                IsDeleted = entityByDate.IsDeleted,
                                LastModificationTime = now,
                                LastModifierId = LoginedUserService.UserId
                            }).Where(new { Id = entityByDate.Id }, "");

                            var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                        }
                    }



                }

                #endregion

                #region Grand Total Stock Movement

                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = currentEntity.BranchID, WarehouseID = currentEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id == Guid.Empty)
                {


                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                    {
                        Amount = 0,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalWarehouseOutput = 0,
                        TotalWarehouseInput = 0,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalReserved = 0,
                        TotalSalesProposition = line.Quantity,
                        TotalWastage = 0,
                        WarehouseID = currentEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = currentEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);

                }

                else
                {
                    decimal previousQuantity = previousEntity.SelectSalesPropositionLines.Where(t => t.Id == line.Id).Select(t => t.Quantity).First();

                    if (line.Quantity > previousQuantity)
                    {
                        decimal addedPR = line.Quantity - previousQuantity;

                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                            TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                            TotalProduction = entityGrandTotal.TotalProduction,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                            TotalReserved = entityGrandTotal.TotalReserved,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition + addedPR,
                            TotalWastage = entityGrandTotal.TotalWastage,
                            WarehouseID = entityGrandTotal.WarehouseID,
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = now,
                            BranchID = entityGrandTotal.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);

                    }
                    else if (line.Quantity < previousQuantity)
                    {
                        decimal decreasedPR = previousQuantity - line.Quantity;

                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                            TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                            TotalProduction = entityGrandTotal.TotalProduction,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                            TotalReserved = entityGrandTotal.TotalReserved,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition - decreasedPR,
                            TotalWastage = entityGrandTotal.TotalWastage,
                            WarehouseID = entityGrandTotal.WarehouseID,
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = now,
                            BranchID = entityGrandTotal.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                    }

                    #endregion
                }

            }

            return true;
        }

        public static bool DeleteSalesPropositionLines(SelectSalesPropositionLinesDto deletedLine)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            #region Getting Sales Proposition

            var query = queryFactory
                  .Query()
                  .From(Tables.SalesPropositions)
                  .Select<SalesPropositions>(null)
                  .Join<PaymentPlans>
                   (
                       pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                       nameof(SalesPropositions.PaymentPlanID),
                       nameof(PaymentPlans.Id),
                       JoinType.Left
                   )
                   .Join<Branches>
                   (
                       b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                       nameof(SalesPropositions.BranchID),
                       nameof(Branches.Id),
                       JoinType.Left
                   )
                   .Join<Warehouses>
                   (
                       w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                       nameof(SalesPropositions.WarehouseID),
                       nameof(Warehouses.Id),
                       JoinType.Left
                   )
                   .Join<Currencies>
                   (
                       c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                       nameof(SalesPropositions.CurrencyID),
                       nameof(Currencies.Id),
                       JoinType.Left
                   )
                   .Join<CurrentAccountCards>
                   (
                       ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                       nameof(SalesPropositions.CurrentAccountCardID),
                       nameof(CurrentAccountCards.Id),
                       JoinType.Left
                   )
                   .Join<ShippingAdresses>
                   (
                       sa => new { ShippingAdressID = sa.Id, ShippingAdressCode = sa.Code },
                       nameof(SalesPropositions.ShippingAdressID),
                       nameof(ShippingAdresses.Id),
                       JoinType.Left
                   )
                   .Where(new { Id = deletedLine.SalesPropositionID }, Tables.SalesPropositions);

            var salesPropositions = queryFactory.Get<SelectSalesPropositionsDto>(query);

            #endregion

            #region By Date Stock Movement

            var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = salesPropositions.BranchID, WarehouseID = salesPropositions.WarehouseID, ProductID = deletedLine.ProductID, Date_ = salesPropositions.Date_ }, "");

            var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

            var queryDecreasingDate = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
            {
                Amount = entityByDate.Amount,
                Date_ = entityByDate.Date_,
                ProductID = entityByDate.ProductID,
                TotalConsumption = entityByDate.TotalConsumption,
                TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                TotalProduction = entityByDate.TotalProduction,
                TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                TotalSalesOrder = entityByDate.TotalSalesOrder,
                TotalSalesProposition = entityByDate.TotalSalesProposition - deletedLine.Quantity,
                TotalWastage = entityByDate.TotalWastage,
                WarehouseID = entityByDate.WarehouseID,
                BranchID = entityByDate.BranchID,
                Id = entityByDate.Id,
                CreationTime = entityByDate.CreationTime.Value,
                CreatorId = entityByDate.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                IsDeleted = entityByDate.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = entityByDate.Id }, "");

            var byDateStockMovementsDecreasing = queryFactory.Update<SelectByDateStockMovementsDto>(queryDecreasingDate, "Id", true);

            #endregion

            #region Grand Total Stock Movement

            var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = salesPropositions.BranchID, WarehouseID = salesPropositions.WarehouseID, ProductID = deletedLine.ProductID }, "");

            var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

            var queryDecreasingGrand = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
            {
                Amount = entityGrandTotal.Amount,
                ProductID = deletedLine.ProductID.GetValueOrDefault(),
                TotalConsumption = entityGrandTotal.TotalConsumption,
                TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                TotalProduction = entityGrandTotal.TotalProduction,
                TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                TotalReserved = entityGrandTotal.TotalReserved,
                TotalSalesProposition = entityGrandTotal.TotalSalesProposition - deletedLine.Quantity,
                TotalWastage = entityGrandTotal.TotalWastage,
                WarehouseID = entityGrandTotal.WarehouseID,
                Id = entityGrandTotal.Id,
                CreationTime = entityGrandTotal.CreationTime.Value,
                CreatorId = entityGrandTotal.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                IsDeleted = entityGrandTotal.IsDeleted,
                LastModificationTime = now,
                BranchID = entityGrandTotal.BranchID,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = entityGrandTotal.Id }, "");

            var grandTotalStockMovementsDecreasing = queryFactory.Update<SelectGrandTotalStockMovementsDto>(queryDecreasingGrand, "Id", true);

            #endregion

            return true;
        }

        public static bool DeleteSalesPropositions(SelectSalesPropositionsDto deletedEntity)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            #region Getting Lines

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.SalesPropositionLines)
                   .Select<SalesPropositionLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(SalesPropositionLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(SalesPropositionLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        ppl => new { PaymentPlanID = ppl.Id, PaymentPlanName = ppl.Name },
                        nameof(SalesPropositionLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Where(new { SalesPropositionID = deletedEntity.Id }, Tables.SalesPropositionLines);

            var salesPropositionLine = queryFactory.GetList<SelectSalesPropositionLinesDto>(queryLines).ToList();

            deletedEntity.SelectSalesPropositionLines = salesPropositionLine;

            #endregion

            foreach (var line in deletedEntity.SelectSalesPropositionLines)
            {

                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = deletedEntity.BranchID, WarehouseID = deletedEntity.WarehouseID, ProductID = line.ProductID, Date_ = deletedEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id != Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                    {
                        Amount = entityByDate.Amount,
                        Date_ = entityByDate.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityByDate.TotalConsumption,
                        TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                        TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                        TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                        TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                        TotalProduction = entityByDate.TotalProduction,
                        TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                        TotalSalesOrder = entityByDate.TotalSalesOrder,
                        TotalSalesProposition = entityByDate.TotalSalesProposition - line.Quantity,
                        TotalWastage = entityByDate.TotalWastage,
                        WarehouseID = entityByDate.WarehouseID,
                        BranchID = entityByDate.BranchID,
                        Id = entityByDate.Id,
                        CreationTime = entityByDate.CreationTime.Value,
                        CreatorId = entityByDate.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityByDate.IsDeleted,
                        LastModificationTime = now,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityByDate.Id }, "");

                    var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                }

                #endregion

                #region Grand Total Stock Movement

                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = deletedEntity.BranchID, WarehouseID = deletedEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id != Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                    {
                        Amount = entityGrandTotal.Amount,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityGrandTotal.TotalConsumption,
                        TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                        TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                        TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                        TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                        TotalProduction = entityGrandTotal.TotalProduction,
                        TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                        TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                        TotalReserved = entityGrandTotal.TotalReserved,
                        TotalSalesProposition = entityGrandTotal.TotalSalesProposition - line.Quantity,
                        TotalWastage = entityGrandTotal.TotalWastage,
                        WarehouseID = entityGrandTotal.WarehouseID,
                        Id = entityGrandTotal.Id,
                        CreationTime = entityGrandTotal.CreationTime.Value,
                        CreatorId = entityGrandTotal.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityGrandTotal.IsDeleted,
                        LastModificationTime = now,
                        BranchID = entityGrandTotal.BranchID,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityGrandTotal.Id }, "");

                    var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                #endregion

            }

            return true;
        }

        #endregion

        #region Sales Orders

        public static bool InsertSalesOrders(CreateSalesOrderDto createdEntity)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            foreach (var line in createdEntity.SelectSalesOrderLines)
            {
                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = createdEntity.BranchID, WarehouseID = createdEntity.WarehouseID, ProductID = line.ProductID, Date_ = createdEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id == Guid.Empty || entityByDate == null)
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                    {
                        Amount = 0,
                        Date_ = createdEntity.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = 0,
                        TotalWarehouseOutput = 0,
                        TotalWarehouseInput = 0,
                        TotalGoodsReceipt = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = line.Quantity,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var byDateStockMovements = queryFactory.Insert<SelectByDateStockMovementsDto>(query, "Id", true);
                }
                else
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                    {
                        Amount = entityByDate.Amount,
                        Date_ = entityByDate.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityByDate.TotalConsumption,
                        TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                        TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                        TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                        TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                        TotalProduction = entityByDate.TotalProduction,
                        TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                        TotalSalesOrder = entityByDate.TotalSalesOrder + line.Quantity,
                        TotalSalesProposition = entityByDate.TotalSalesProposition,
                        TotalWastage = entityByDate.TotalWastage,
                        WarehouseID = entityByDate.WarehouseID,
                        BranchID = entityByDate.BranchID,
                        Id = entityByDate.Id,
                        CreationTime = entityByDate.CreationTime.Value,
                        CreatorId = entityByDate.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityByDate.IsDeleted,
                        LastModificationTime = now,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityByDate.Id }, "");

                    var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                }

                #endregion

                #region Grand Total Stock Movement


                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = createdEntity.BranchID, WarehouseID = createdEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id == Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                    {
                        Amount = 0,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = 0,
                        TotalWarehouseOutput = 0,
                        TotalWarehouseInput = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = line.Quantity,
                        TotalReserved = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                else
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                    {
                        Amount = entityGrandTotal.Amount,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityGrandTotal.TotalConsumption,
                        TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                        TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                        TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                        TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                        TotalProduction = entityGrandTotal.TotalProduction,
                        TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                        TotalSalesOrder = entityGrandTotal.TotalSalesOrder + line.Quantity,
                        TotalReserved = entityGrandTotal.TotalReserved,
                        TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                        TotalWastage = entityGrandTotal.TotalWastage,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        Id = entityGrandTotal.Id,
                        CreationTime = entityGrandTotal.CreationTime.Value,
                        CreatorId = entityGrandTotal.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityGrandTotal.IsDeleted,
                        LastModificationTime = now,
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityGrandTotal.Id }, "");

                    var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                #endregion

            }

            return true;
        }

        public static bool UpdateSalesOrders(SelectSalesOrderDto previousEntity, UpdateSalesOrderDto currentEntity)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            foreach (var line in currentEntity.SelectSalesOrderLines)
            {

                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = currentEntity.BranchID, WarehouseID = currentEntity.WarehouseID, ProductID = line.ProductID, Date_ = currentEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id == Guid.Empty)
                {


                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                    {
                        Amount = 0,
                        Date_ = currentEntity.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalWarehouseInput = 0,
                        TotalWarehouseOutput = 0,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = line.Quantity,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = currentEntity.WarehouseID,
                        BranchID = currentEntity.BranchID,
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
                    });

                    var byDateStockMovements = queryFactory.Insert<SelectByDateStockMovementsDto>(query, "Id", true);


                }

                else
                {
                    decimal previousQuantity = previousEntity.SelectSalesOrderLines.Where(t => t.Id == line.Id).Select(t => t.Quantity).First();

                    if (line.Quantity > previousQuantity)
                    {
                        decimal addedPR = line.Quantity - previousQuantity;

                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                        {
                            Amount = entityByDate.Amount,
                            Date_ = entityByDate.Date_,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityByDate.TotalConsumption,
                            TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                            TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                            TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                            TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                            TotalProduction = entityByDate.TotalProduction,
                            TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                            TotalSalesOrder = entityByDate.TotalSalesOrder + addedPR,
                            TotalSalesProposition = entityByDate.TotalSalesProposition,
                            TotalWastage = entityByDate.TotalWastage,
                            WarehouseID = entityByDate.WarehouseID,
                            BranchID = entityByDate.BranchID,
                            Id = entityByDate.Id,
                            CreationTime = entityByDate.CreationTime.Value,
                            CreatorId = entityByDate.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityByDate.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityByDate.Id }, "");

                        var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);

                    }

                    else if (line.Quantity < previousQuantity)
                    {
                        decimal decreasedPR = previousQuantity - line.Quantity;

                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                        {
                            Amount = entityByDate.Amount,
                            Date_ = entityByDate.Date_,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityByDate.TotalConsumption,
                            TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                            TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                            TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                            TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                            TotalProduction = entityByDate.TotalProduction,
                            TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                            TotalSalesOrder = entityByDate.TotalSalesOrder - decreasedPR,
                            TotalSalesProposition = entityByDate.TotalSalesProposition,
                            TotalWastage = entityByDate.TotalWastage,
                            WarehouseID = entityByDate.WarehouseID,
                            BranchID = entityByDate.BranchID,
                            Id = entityByDate.Id,
                            CreationTime = entityByDate.CreationTime.Value,
                            CreatorId = entityByDate.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityByDate.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityByDate.Id }, "");

                        var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                    }

                }

                #endregion

                #region Grand Total Stock Movement

                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = currentEntity.BranchID, WarehouseID = currentEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id == Guid.Empty)
                {


                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                    {
                        Amount = 0,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalWarehouseOutput = 0,
                        TotalWarehouseInput = 0,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = line.Quantity,
                        TotalReserved = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = currentEntity.WarehouseID,
                        BranchID = currentEntity.BranchID,
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
                    });

                    var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);

                }

                else
                {
                    decimal previousQuantity = previousEntity.SelectSalesOrderLines.Where(t => t.Id == line.Id).Select(t => t.Quantity).First();

                    if (line.Quantity > previousQuantity)
                    {
                        decimal addedPR = line.Quantity - previousQuantity;

                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                            TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                            TotalProduction = entityGrandTotal.TotalProduction,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder + addedPR,
                            TotalReserved = entityGrandTotal.TotalReserved,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                            TotalWastage = entityGrandTotal.TotalWastage,
                            WarehouseID = entityGrandTotal.WarehouseID,
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = now,
                            BranchID = entityGrandTotal.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);

                    }
                    else if (line.Quantity < previousQuantity)
                    {
                        decimal decreasedPR = previousQuantity - line.Quantity;

                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                            TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                            TotalProduction = entityGrandTotal.TotalProduction,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder - decreasedPR,
                            TotalReserved = entityGrandTotal.TotalReserved,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                            TotalWastage = entityGrandTotal.TotalWastage,
                            WarehouseID = entityGrandTotal.WarehouseID,
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = now,
                            BranchID = entityGrandTotal.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                    }

                    #endregion
                }

            }

            return true;
        }

        public static bool DeleteSalesOrderLines(SelectSalesOrderLinesDto deletedLine)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            #region Getting Sales Order

            var query = queryFactory
                  .Query()
                  .From(Tables.SalesOrders)
                  .Select<SalesOrders>(null)
                  .Join<PaymentPlans>
                   (
                       pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                       nameof(SalesOrders.PaymentPlanID),
                       nameof(PaymentPlans.Id),
                       JoinType.Left
                   )
                   .Join<Branches>
                   (
                       b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                       nameof(SalesOrders.BranchID),
                       nameof(Branches.Id),
                       JoinType.Left
                   )
                    .Join<Warehouses>
                   (
                       w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                       nameof(SalesOrders.WarehouseID),
                       nameof(Warehouses.Id),
                       JoinType.Left
                   )
                    .Join<Currencies>
                   (
                       c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                       nameof(SalesOrders.CurrencyID),
                       nameof(Currencies.Id),
                       JoinType.Left
                   )
                    .Join<CurrentAccountCards>
                   (
                       ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                       nameof(SalesOrders.CurrentAccountCardID),
                       nameof(CurrentAccountCards.Id),
                       JoinType.Left)

                        .Join<ShippingAdresses>
                   (
                       sa => new { ShippingAdressID = sa.Id, ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                       nameof(SalesOrders.ShippingAdressID),
                       nameof(ShippingAdresses.Id),
                       JoinType.Left
                   )
                   .Where(new { Id = deletedLine.SalesOrderID }, Tables.SalesOrders);

            var salesOrders = queryFactory.Get<SelectSalesOrderDto>(query);

            #endregion

            #region By Date Stock Movement

            var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = salesOrders.BranchID, WarehouseID = salesOrders.WarehouseID, ProductID = deletedLine.ProductID, Date_ = salesOrders.Date_ }, "");

            var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

            var queryDecreasingDate = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
            {
                Amount = entityByDate.Amount,
                Date_ = entityByDate.Date_,
                ProductID = entityByDate.ProductID,
                TotalConsumption = entityByDate.TotalConsumption,
                TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                TotalProduction = entityByDate.TotalProduction,
                TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                TotalSalesOrder = entityByDate.TotalSalesOrder - deletedLine.Quantity,
                TotalSalesProposition = entityByDate.TotalSalesProposition,
                TotalWastage = entityByDate.TotalWastage,
                WarehouseID = entityByDate.WarehouseID,
                BranchID = entityByDate.BranchID,
                Id = entityByDate.Id,
                CreationTime = entityByDate.CreationTime.Value,
                CreatorId = entityByDate.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                IsDeleted = entityByDate.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = entityByDate.Id }, "");

            var byDateStockMovementsDecreasing = queryFactory.Update<SelectByDateStockMovementsDto>(queryDecreasingDate, "Id", true);

            #endregion

            #region Grand Total Stock Movement

            var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = salesOrders.BranchID, WarehouseID = salesOrders.WarehouseID, ProductID = deletedLine.ProductID }, "");

            var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

            var queryDecreasingGrand = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
            {
                Amount = entityGrandTotal.Amount,
                ProductID = deletedLine.ProductID.GetValueOrDefault(),
                TotalConsumption = entityGrandTotal.TotalConsumption,
                TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                TotalProduction = entityGrandTotal.TotalProduction,
                TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                TotalSalesOrder = entityGrandTotal.TotalSalesOrder - deletedLine.Quantity,
                TotalReserved = entityGrandTotal.TotalReserved,
                TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                TotalWastage = entityGrandTotal.TotalWastage,
                WarehouseID = entityGrandTotal.WarehouseID,
                Id = entityGrandTotal.Id,
                CreationTime = entityGrandTotal.CreationTime.Value,
                CreatorId = entityGrandTotal.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                IsDeleted = entityGrandTotal.IsDeleted,
                LastModificationTime = now,
                BranchID = entityGrandTotal.BranchID,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = entityGrandTotal.Id }, "");

            var grandTotalStockMovementsDecreasing = queryFactory.Update<SelectGrandTotalStockMovementsDto>(queryDecreasingGrand, "Id", true);

            #endregion

            return true;
        }

        public static bool DeleteSalesOrders(SelectSalesOrderDto deletedEntity)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            #region Getting Lines

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.SalesOrderLines)
                   .Select<SalesOrderLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(SalesOrderLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(SalesOrderLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        pay => new { PaymentPlanID = pay.Id, PaymentPlanName = pay.Name },
                        nameof(SalesOrderLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                      .Join<SalesPropositionLines>
                    (
                        spl => new { LikedPropositionLineID = spl.Id, LinkedSalesPropositionID = spl.SalesPropositionID },
                        nameof(SalesOrderLines.LikedPropositionLineID),
                        nameof(SalesPropositionLines.Id),
                        JoinType.Left
                    )

                    .Where(new { SalesOrderID = deletedEntity.Id }, Tables.SalesOrderLines);

            var salesOrderLine = queryFactory.GetList<SelectSalesOrderLinesDto>(queryLines).ToList();

            deletedEntity.SelectSalesOrderLines = salesOrderLine;

            #endregion

            foreach (var line in deletedEntity.SelectSalesOrderLines)
            {

                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = deletedEntity.BranchID, WarehouseID = deletedEntity.WarehouseID, ProductID = line.ProductID, Date_ = deletedEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id != Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                    {
                        Amount = entityByDate.Amount,
                        Date_ = entityByDate.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityByDate.TotalConsumption,
                        TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                        TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                        TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                        TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                        TotalProduction = entityByDate.TotalProduction,
                        TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                        TotalSalesOrder = entityByDate.TotalSalesOrder - line.Quantity,
                        TotalSalesProposition = entityByDate.TotalSalesProposition,
                        TotalWastage = entityByDate.TotalWastage,
                        WarehouseID = entityByDate.WarehouseID,
                        BranchID = entityByDate.BranchID,
                        Id = entityByDate.Id,
                        CreationTime = entityByDate.CreationTime.Value,
                        CreatorId = entityByDate.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityByDate.DeleterId.Value,
                        DeletionTime = entityByDate.DeletionTime.Value,
                        IsDeleted = entityByDate.IsDeleted,
                        LastModificationTime = now,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityByDate.Id }, "");

                    var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                }

                #endregion

                #region Grand Total Stock Movement

                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = deletedEntity.BranchID, WarehouseID = deletedEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id != Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                    {
                        Amount = entityGrandTotal.Amount,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityGrandTotal.TotalConsumption,
                        TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                        TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                        TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                        TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                        TotalProduction = entityGrandTotal.TotalProduction,
                        TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                        TotalSalesOrder = entityGrandTotal.TotalSalesOrder - line.Quantity,
                        TotalReserved = entityGrandTotal.TotalReserved,
                        TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                        TotalWastage = entityGrandTotal.TotalWastage,
                        WarehouseID = entityGrandTotal.WarehouseID,
                        Id = entityGrandTotal.Id,
                        CreationTime = entityGrandTotal.CreationTime.Value,
                        CreatorId = entityGrandTotal.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityGrandTotal.IsDeleted,
                        LastModificationTime = now,
                        BranchID = entityGrandTotal.BranchID,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityGrandTotal.Id }, "");

                    var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                #endregion

            }

            return true;
        }

        #endregion

        #region Total Consumptions

        public static bool InsertTotalConsumptions(CreateStockFichesDto createdEntity)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            foreach (var line in createdEntity.SelectStockFicheLines)
            {
                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = createdEntity.BranchID, WarehouseID = createdEntity.WarehouseID, ProductID = line.ProductID, Date_ = createdEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id == Guid.Empty || entityByDate == null)
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                    {
                        Amount = line.Quantity * (-1),
                        Date_ = createdEntity.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = line.Quantity,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = 0,
                        TotalWarehouseInput = 0,
                        TotalWarehouseOutput = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var byDateStockMovements = queryFactory.Insert<SelectByDateStockMovementsDto>(query, "Id", true);
                }

                else
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                    {
                        Amount = entityByDate.Amount - line.Quantity,
                        Date_ = entityByDate.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityByDate.TotalConsumption + line.Quantity,
                        TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                        TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                        TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                        TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                        TotalProduction = entityByDate.TotalProduction,
                        TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                        TotalSalesOrder = entityByDate.TotalSalesOrder,
                        TotalSalesProposition = entityByDate.TotalSalesProposition,
                        TotalWastage = entityByDate.TotalWastage,
                        WarehouseID = entityByDate.WarehouseID,
                        BranchID = entityByDate.BranchID,
                        Id = entityByDate.Id,
                        CreationTime = entityByDate.CreationTime.Value,
                        CreatorId = entityByDate.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityByDate.IsDeleted,
                        LastModificationTime = now,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityByDate.Id }, "");

                    var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                }

                #endregion

                #region Grand Total Stock Movement


                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = createdEntity.BranchID, WarehouseID = createdEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id == Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                    {
                        Amount = line.Quantity * (-1),
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = line.Quantity,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = 0,
                        TotalWarehouseOutput = 0,
                        TotalWarehouseInput = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalReserved = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                else
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                    {
                        Amount = entityGrandTotal.Amount - line.Quantity,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityGrandTotal.TotalConsumption + line.Quantity,
                        TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                        TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                        TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                        TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                        TotalProduction = entityGrandTotal.TotalProduction,
                        TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                        TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                        TotalReserved = entityGrandTotal.TotalReserved,
                        TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                        TotalWastage = entityGrandTotal.TotalWastage,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        Id = entityGrandTotal.Id,
                        CreationTime = entityGrandTotal.CreationTime.Value,
                        CreatorId = entityGrandTotal.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityGrandTotal.IsDeleted,
                        LastModificationTime = now,
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityGrandTotal.Id }, "");

                    var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                #endregion

            }

            return true;
        }

        public static bool UpdateTotalConsumptions(SelectStockFichesDto previousEntity, UpdateStockFichesDto currentEntity)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            foreach (var line in currentEntity.SelectStockFicheLines)
            {

                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = currentEntity.BranchID, WarehouseID = currentEntity.WarehouseID, ProductID = line.ProductID, Date_ = currentEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id == Guid.Empty)
                {


                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                    {
                        Amount = line.Quantity * (-1),
                        Date_ = currentEntity.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = line.Quantity,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalWarehouseInput = 0,
                        TotalWarehouseOutput = 0,
                        TotalSalesOrder = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = currentEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = currentEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var byDateStockMovements = queryFactory.Insert<SelectByDateStockMovementsDto>(query, "Id", true);


                }

                else
                {
                    decimal previousQuantity = previousEntity.SelectStockFicheLines.Where(t => t.Id == line.Id).Select(t => t.Quantity).First();

                    if (line.Quantity > previousQuantity)
                    {
                        decimal addedPR = line.Quantity - previousQuantity;

                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                        {
                            Amount = entityByDate.Amount - addedPR,
                            Date_ = entityByDate.Date_,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityByDate.TotalConsumption + addedPR,
                            TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                            TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                            TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                            TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                            TotalProduction = entityByDate.TotalProduction,
                            TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                            TotalSalesOrder = entityByDate.TotalSalesOrder,
                            TotalSalesProposition = entityByDate.TotalSalesProposition,
                            TotalWastage = entityByDate.TotalWastage,
                            WarehouseID = entityByDate.WarehouseID,
                            BranchID = entityByDate.BranchID,
                            Id = entityByDate.Id,
                            CreationTime = entityByDate.CreationTime.Value,
                            CreatorId = entityByDate.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityByDate.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityByDate.Id }, "");

                        var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);

                    }
                    else if (line.Quantity < previousQuantity)
                    {
                        decimal decreasedPR = previousQuantity - line.Quantity;

                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                        {
                            Amount = entityByDate.Amount + decreasedPR,
                            Date_ = entityByDate.Date_,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityByDate.TotalConsumption - decreasedPR,
                            TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                            TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                            TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                            TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                            TotalProduction = entityByDate.TotalProduction,
                            TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                            TotalSalesOrder = entityByDate.TotalSalesOrder,
                            TotalSalesProposition = entityByDate.TotalSalesProposition,
                            TotalWastage = entityByDate.TotalWastage,
                            WarehouseID = entityByDate.WarehouseID,
                            BranchID = entityByDate.BranchID,
                            Id = entityByDate.Id,
                            CreationTime = entityByDate.CreationTime.Value,
                            CreatorId = entityByDate.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityByDate.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityByDate.Id }, "");

                        var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                    }

                }

                #endregion

                #region Grand Total Stock Movement

                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = currentEntity.BranchID, WarehouseID = currentEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id == Guid.Empty)
                {


                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                    {
                        Amount = line.Quantity * (-1),
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = line.Quantity,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = 0,
                        TotalProduction = 0,
                        TotalWarehouseOutput = 0,
                        TotalWarehouseInput = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalReserved = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = currentEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = currentEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);

                }

                else
                {
                    decimal previousQuantity = previousEntity.SelectStockFicheLines.Where(t => t.Id == line.Id).Select(t => t.Quantity).First();

                    if (line.Quantity > previousQuantity)
                    {
                        decimal addedPR = line.Quantity - previousQuantity;

                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount - addedPR,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityGrandTotal.TotalConsumption + addedPR,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                            TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                            TotalProduction = entityGrandTotal.TotalProduction,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                            TotalReserved = entityGrandTotal.TotalReserved,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                            TotalWastage = entityGrandTotal.TotalWastage,
                            WarehouseID = entityGrandTotal.WarehouseID,
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = now,
                            BranchID = entityGrandTotal.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);

                    }
                    else if (line.Quantity < previousQuantity)
                    {
                        decimal decreasedPR = previousQuantity - line.Quantity;

                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount + decreasedPR,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityGrandTotal.TotalConsumption - decreasedPR,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                            TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                            TotalProduction = entityGrandTotal.TotalProduction,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                            TotalReserved = entityGrandTotal.TotalReserved,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                            TotalWastage = entityGrandTotal.TotalWastage,
                            WarehouseID = entityGrandTotal.WarehouseID,
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = now,
                            BranchID = entityGrandTotal.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                    }

                    #endregion
                }

            }

            return true;
        }

        public static bool DeleteTotalConsumptionLines(SelectStockFicheLinesDto deletedLine)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            #region Getting Total Consumption

            var query = queryFactory
                   .Query()
                   .From(Tables.StockFiches)
                   .Select<StockFiches>(null)
                   .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchID = b.Id },
                        nameof(StockFiches.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                   .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code, WarehouseID = w.Id },
                        nameof(StockFiches.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = deletedLine.StockFicheID }, Tables.StockFiches);

            var stockFiches = queryFactory.Get<SelectStockFichesDto>(query);

            #endregion

            #region By Date Stock Movement

            var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = stockFiches.BranchID, WarehouseID = stockFiches.WarehouseID, ProductID = deletedLine.ProductID, Date_ = stockFiches.Date_ }, "");

            var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

            var queryDecreasingDate = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
            {
                Amount = entityByDate.Amount + deletedLine.Quantity,
                Date_ = entityByDate.Date_,
                ProductID = entityByDate.ProductID,
                TotalConsumption = entityByDate.TotalConsumption - deletedLine.Quantity,
                TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                TotalProduction = entityByDate.TotalProduction,
                TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                TotalSalesOrder = entityByDate.TotalSalesOrder,
                TotalSalesProposition = entityByDate.TotalSalesProposition,
                TotalWastage = entityByDate.TotalWastage,
                WarehouseID = entityByDate.WarehouseID,
                BranchID = entityByDate.BranchID,
                Id = entityByDate.Id,
                CreationTime = entityByDate.CreationTime.Value,
                CreatorId = entityByDate.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                IsDeleted = entityByDate.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = entityByDate.Id }, "");

            var byDateStockMovementsDecreasing = queryFactory.Update<SelectByDateStockMovementsDto>(queryDecreasingDate, "Id", true);

            #endregion

            #region Grand Total Stock Movement

            var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = stockFiches.BranchID, WarehouseID = stockFiches.WarehouseID, ProductID = deletedLine.ProductID }, "");

            var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

            var queryDecreasingGrand = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
            {
                Amount = entityGrandTotal.Amount + deletedLine.Quantity,
                ProductID = deletedLine.ProductID.GetValueOrDefault(),
                TotalConsumption = entityGrandTotal.TotalConsumption - deletedLine.Quantity,
                TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                TotalProduction = entityGrandTotal.TotalProduction,
                TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                TotalReserved = entityGrandTotal.TotalReserved,
                TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                TotalWastage = entityGrandTotal.TotalWastage,
                WarehouseID = entityGrandTotal.WarehouseID,
                Id = entityGrandTotal.Id,
                CreationTime = entityGrandTotal.CreationTime.Value,
                CreatorId = entityGrandTotal.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                IsDeleted = entityGrandTotal.IsDeleted,
                LastModificationTime = now,
                BranchID = entityGrandTotal.BranchID,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = entityGrandTotal.Id }, "");

            var grandTotalStockMovementsDecreasing = queryFactory.Update<SelectGrandTotalStockMovementsDto>(queryDecreasingGrand, "Id", true);

            #endregion

            return true;
        }

        public static bool DeleteTotalConsumptions(SelectStockFichesDto deletedEntity)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            #region Getting Lines

            var queryLines = queryFactory
                    .Query()
                    .From(Tables.StockFicheLines)
                    .Select<StockFicheLines>(null)
                    .Join<Products>
                     (
                         p => new { ProductCode = p.Code, ProductName = p.Name },
                         nameof(StockFicheLines.ProductID),
                         nameof(Products.Id),
                         JoinType.Left
                     )
                    .Join<UnitSets>
                     (
                         u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                         nameof(StockFicheLines.UnitSetID),
                         nameof(UnitSets.Id),
                         JoinType.Left
                     )
                     .Where(new { StockFicheID = deletedEntity.Id }, Tables.StockFicheLines);

            var stockFicheLine = queryFactory.GetList<SelectStockFicheLinesDto>(queryLines).ToList();

            deletedEntity.SelectStockFicheLines = stockFicheLine;

            #endregion

            foreach (var line in deletedEntity.SelectStockFicheLines)
            {

                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = deletedEntity.BranchID, WarehouseID = deletedEntity.WarehouseID, ProductID = line.ProductID, Date_ = deletedEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id != Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                    {
                        Amount = entityByDate.Amount + line.Quantity,
                        Date_ = entityByDate.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityByDate.TotalConsumption - line.Quantity,
                        TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                        TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                        TotalProduction = entityByDate.TotalProduction,
                        TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                        TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                        TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                        TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                        TotalSalesOrder = entityByDate.TotalSalesOrder,
                        TotalSalesProposition = entityByDate.TotalSalesProposition,
                        TotalWastage = entityByDate.TotalWastage,
                        WarehouseID = entityByDate.WarehouseID,
                        BranchID = entityByDate.BranchID,
                        Id = entityByDate.Id,
                        CreationTime = entityByDate.CreationTime.Value,
                        CreatorId = entityByDate.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityByDate.IsDeleted,
                        LastModificationTime = now,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityByDate.Id }, "");

                    var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                }

                #endregion

                #region Grand Total Stock Movement

                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = deletedEntity.BranchID, WarehouseID = deletedEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id != Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                    {
                        Amount = entityGrandTotal.Amount + line.Quantity,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityGrandTotal.TotalConsumption - line.Quantity,
                        TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                        TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                        TotalProduction = entityGrandTotal.TotalProduction,
                        TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                        TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                        TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                        TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                        TotalReserved = entityGrandTotal.TotalReserved,
                        TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                        TotalWastage = entityGrandTotal.TotalWastage,
                        WarehouseID = entityGrandTotal.WarehouseID,
                        Id = entityGrandTotal.Id,
                        CreationTime = entityGrandTotal.CreationTime.Value,
                        CreatorId = entityGrandTotal.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityGrandTotal.IsDeleted,
                        LastModificationTime = now,
                        BranchID = entityGrandTotal.BranchID,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityGrandTotal.Id }, "");

                    var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                #endregion


            }

            return true;
        }

        #endregion

        #region Total Wastages

        public static bool InsertTotalWastages(CreateStockFichesDto createdEntity)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            foreach (var line in createdEntity.SelectStockFicheLines)
            {
                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = createdEntity.BranchID, WarehouseID = createdEntity.WarehouseID, ProductID = line.ProductID, Date_ = createdEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id == Guid.Empty || entityByDate == null)
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                    {
                        Amount = line.Quantity * (-1),
                        Date_ = createdEntity.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = 0,
                        TotalWarehouseOutput = 0,
                        TotalWarehouseInput = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = line.Quantity,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var byDateStockMovements = queryFactory.Insert<SelectByDateStockMovementsDto>(query, "Id", true);
                }

                else
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                    {
                        Amount = entityByDate.Amount - line.Quantity,
                        Date_ = entityByDate.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityByDate.TotalConsumption,
                        TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                        TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                        TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                        TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                        TotalProduction = entityByDate.TotalProduction,
                        TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                        TotalSalesOrder = entityByDate.TotalSalesOrder,
                        TotalSalesProposition = entityByDate.TotalSalesProposition,
                        TotalWastage = entityByDate.TotalWastage + line.Quantity,
                        WarehouseID = entityByDate.WarehouseID,
                        BranchID = entityByDate.BranchID,
                        Id = entityByDate.Id,
                        CreationTime = entityByDate.CreationTime.Value,
                        CreatorId = entityByDate.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityByDate.IsDeleted,
                        LastModificationTime = now,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityByDate.Id }, "");

                    var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                }

                #endregion

                #region Grand Total Stock Movement


                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = createdEntity.BranchID, WarehouseID = createdEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id == Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                    {
                        Amount = line.Quantity * (-1),
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = 0,
                        TotalWarehouseOutput = 0,
                        TotalWarehouseInput = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalReserved = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = line.Quantity,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                else
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                    {
                        Amount = entityGrandTotal.Amount - line.Quantity,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityGrandTotal.TotalConsumption,
                        TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                        TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                        TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                        TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                        TotalProduction = entityGrandTotal.TotalProduction,
                        TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                        TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                        TotalReserved = entityGrandTotal.TotalReserved,
                        TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                        TotalWastage = entityGrandTotal.TotalWastage + line.Quantity,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        Id = entityGrandTotal.Id,
                        CreationTime = entityGrandTotal.CreationTime.Value,
                        CreatorId = entityGrandTotal.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityGrandTotal.IsDeleted,
                        LastModificationTime = now,
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityGrandTotal.Id }, "");

                    var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                #endregion


            }

            return true;
        }

        public static bool UpdateTotalWastages(SelectStockFichesDto previousEntity, UpdateStockFichesDto currentEntity)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            foreach (var line in currentEntity.SelectStockFicheLines)
            {

                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = currentEntity.BranchID, WarehouseID = currentEntity.WarehouseID, ProductID = line.ProductID, Date_ = currentEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id == Guid.Empty)
                {


                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                    {
                        Amount = line.Quantity * (-1),
                        Date_ = currentEntity.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = 0,
                        TotalWarehouseInput = 0,
                        TotalWarehouseOutput = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = line.Quantity,
                        WarehouseID = currentEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = currentEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var byDateStockMovements = queryFactory.Insert<SelectByDateStockMovementsDto>(query, "Id", true);


                }

                else
                {
                    decimal previousQuantity = previousEntity.SelectStockFicheLines.Where(t => t.Id == line.Id).Select(t => t.Quantity).First();

                    if (line.Quantity > previousQuantity)
                    {
                        decimal addedPR = line.Quantity - previousQuantity;

                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                        {
                            Amount = entityByDate.Amount - addedPR,
                            Date_ = entityByDate.Date_,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityByDate.TotalConsumption,
                            TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                            TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                            TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                            TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                            TotalProduction = entityByDate.TotalProduction,
                            TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                            TotalSalesOrder = entityByDate.TotalSalesOrder,
                            TotalSalesProposition = entityByDate.TotalSalesProposition,
                            TotalWastage = entityByDate.TotalWastage + addedPR,
                            WarehouseID = entityByDate.WarehouseID,
                            BranchID = entityByDate.BranchID,
                            Id = entityByDate.Id,
                            CreationTime = entityByDate.CreationTime.Value,
                            CreatorId = entityByDate.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityByDate.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityByDate.Id }, "");

                        var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);

                    }
                    else if (line.Quantity < previousQuantity)
                    {
                        decimal decreasedPR = previousQuantity - line.Quantity;

                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                        {
                            Amount = entityByDate.Amount + decreasedPR,
                            Date_ = entityByDate.Date_,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityByDate.TotalConsumption,
                            TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                            TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                            TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                            TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                            TotalProduction = entityByDate.TotalProduction,
                            TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                            TotalSalesOrder = entityByDate.TotalSalesOrder,
                            TotalSalesProposition = entityByDate.TotalSalesProposition,
                            TotalWastage = entityByDate.TotalWastage - decreasedPR,
                            WarehouseID = entityByDate.WarehouseID,
                            BranchID = entityByDate.BranchID,
                            Id = entityByDate.Id,
                            CreationTime = entityByDate.CreationTime.Value,
                            CreatorId = entityByDate.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityByDate.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityByDate.Id }, "");

                        var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                    }

                }

                #endregion

                #region Grand Total Stock Movement

                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = currentEntity.BranchID, WarehouseID = currentEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id == Guid.Empty)
                {


                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                    {
                        Amount = line.Quantity * (-1),
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = 0,
                        TotalWarehouseOutput = 0,
                        TotalWarehouseInput = 0,
                        TotalGoodsReceipt = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalReserved = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = line.Quantity,
                        WarehouseID = currentEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = currentEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);

                }

                else
                {
                    decimal previousQuantity = previousEntity.SelectStockFicheLines.Where(t => t.Id == line.Id).Select(t => t.Quantity).First();

                    if (line.Quantity > previousQuantity)
                    {
                        decimal addedPR = line.Quantity - previousQuantity;

                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount - addedPR,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                            TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                            TotalProduction = entityGrandTotal.TotalProduction,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                            TotalReserved = entityGrandTotal.TotalReserved,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                            TotalWastage = entityGrandTotal.TotalWastage + addedPR,
                            WarehouseID = entityGrandTotal.WarehouseID,
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = now,
                            BranchID = entityGrandTotal.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);

                    }
                    else if (line.Quantity < previousQuantity)
                    {
                        decimal decreasedPR = previousQuantity - line.Quantity;

                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount + decreasedPR,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                            TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                            TotalProduction = entityGrandTotal.TotalProduction,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                            TotalReserved = entityGrandTotal.TotalReserved,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                            TotalWastage = entityGrandTotal.TotalWastage - decreasedPR,
                            WarehouseID = entityGrandTotal.WarehouseID,
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = now,
                            BranchID = entityGrandTotal.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                    }

                    #endregion
                }

            }

            return true;
        }

        public static bool DeleteTotalWastageLines(SelectStockFicheLinesDto deletedLine)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            #region Getting Total Wastage

            var query = queryFactory
                   .Query()
                   .From(Tables.StockFiches)
                   .Select<StockFiches>(null)
                   .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchID = b.Id },
                        nameof(StockFiches.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                   .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code, WarehouseID = w.Id },
                        nameof(StockFiches.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = deletedLine.StockFicheID }, Tables.StockFiches);

            var stockFiches = queryFactory.Get<SelectStockFichesDto>(query);

            #endregion

            #region By Date Stock Movement

            var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = stockFiches.BranchID, WarehouseID = stockFiches.WarehouseID, ProductID = deletedLine.ProductID, Date_ = stockFiches.Date_ }, "");

            var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

            var queryDecreasingDate = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
            {
                Amount = entityByDate.Amount + deletedLine.Quantity,
                Date_ = entityByDate.Date_,
                ProductID = entityByDate.ProductID,
                TotalConsumption = entityByDate.TotalConsumption,
                TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                TotalProduction = entityByDate.TotalProduction,
                TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                TotalSalesOrder = entityByDate.TotalSalesOrder,
                TotalSalesProposition = entityByDate.TotalSalesProposition,
                TotalWastage = entityByDate.TotalWastage - deletedLine.Quantity,
                WarehouseID = entityByDate.WarehouseID,
                BranchID = entityByDate.BranchID,
                Id = entityByDate.Id,
                CreationTime = entityByDate.CreationTime.Value,
                CreatorId = entityByDate.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                IsDeleted = entityByDate.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = entityByDate.Id }, "");

            var byDateStockMovementsDecreasing = queryFactory.Update<SelectByDateStockMovementsDto>(queryDecreasingDate, "Id", true);

            #endregion

            #region Grand Total Stock Movement

            var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = stockFiches.BranchID, WarehouseID = stockFiches.WarehouseID, ProductID = deletedLine.ProductID }, "");

            var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

            var queryDecreasingGrand = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
            {
                Amount = entityGrandTotal.Amount + deletedLine.Quantity,
                ProductID = deletedLine.ProductID.GetValueOrDefault(),
                TotalConsumption = entityGrandTotal.TotalConsumption,
                TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                TotalProduction = entityGrandTotal.TotalProduction,
                TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                TotalReserved = entityGrandTotal.TotalReserved,
                TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                TotalWastage = entityGrandTotal.TotalWastage - deletedLine.Quantity,
                WarehouseID = entityGrandTotal.WarehouseID,
                Id = entityGrandTotal.Id,
                CreationTime = entityGrandTotal.CreationTime.Value,
                CreatorId = entityGrandTotal.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                IsDeleted = entityGrandTotal.IsDeleted,
                LastModificationTime = now,
                BranchID = entityGrandTotal.BranchID,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = entityGrandTotal.Id }, "");

            var grandTotalStockMovementsDecreasing = queryFactory.Update<SelectGrandTotalStockMovementsDto>(queryDecreasingGrand, "Id", true);

            #endregion

            return true;
        }

        public static bool DeleteTotalWastages(SelectStockFichesDto deletedEntity)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            #region Getting Lines

            var queryLines = queryFactory
                    .Query()
                    .From(Tables.StockFicheLines)
                    .Select<StockFicheLines>(null)
                    .Join<Products>
                     (
                         p => new { ProductCode = p.Code, ProductName = p.Name },
                         nameof(StockFicheLines.ProductID),
                         nameof(Products.Id),
                         JoinType.Left
                     )
                    .Join<UnitSets>
                     (
                         u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                         nameof(StockFicheLines.UnitSetID),
                         nameof(UnitSets.Id),
                         JoinType.Left
                     )
                     .Where(new { StockFicheID = deletedEntity.Id }, Tables.StockFicheLines);

            var stockFicheLine = queryFactory.GetList<SelectStockFicheLinesDto>(queryLines).ToList();

            deletedEntity.SelectStockFicheLines = stockFicheLine;

            #endregion

            foreach (var line in deletedEntity.SelectStockFicheLines)
            {

                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = deletedEntity.BranchID, WarehouseID = deletedEntity.WarehouseID, ProductID = line.ProductID, Date_ = deletedEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id != Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                    {
                        Amount = entityByDate.Amount + line.Quantity,
                        Date_ = entityByDate.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityByDate.TotalConsumption,
                        TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                        TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                        TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                        TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                        TotalProduction = entityByDate.TotalProduction,
                        TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                        TotalSalesOrder = entityByDate.TotalSalesOrder,
                        TotalSalesProposition = entityByDate.TotalSalesProposition,
                        TotalWastage = entityByDate.TotalWastage - line.Quantity,
                        WarehouseID = entityByDate.WarehouseID,
                        BranchID = entityByDate.BranchID,
                        Id = entityByDate.Id,
                        CreationTime = entityByDate.CreationTime.Value,
                        CreatorId = entityByDate.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityByDate.IsDeleted,
                        LastModificationTime = now,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityByDate.Id }, "");

                    var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                }

                #endregion

                #region Grand Total Stock Movement

                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = deletedEntity.BranchID, WarehouseID = deletedEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id != Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                    {
                        Amount = entityGrandTotal.Amount + line.Quantity,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityGrandTotal.TotalConsumption,
                        TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                        TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                        TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                        TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                        TotalProduction = entityGrandTotal.TotalProduction,
                        TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                        TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                        TotalReserved = entityGrandTotal.TotalReserved,
                        TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                        TotalWastage = entityGrandTotal.TotalWastage - line.Quantity,
                        WarehouseID = entityGrandTotal.WarehouseID,
                        Id = entityGrandTotal.Id,
                        CreationTime = entityGrandTotal.CreationTime.Value,
                        CreatorId = entityGrandTotal.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityGrandTotal.IsDeleted,
                        LastModificationTime = now,
                        BranchID = entityGrandTotal.BranchID,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityGrandTotal.Id }, "");

                    var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                #endregion

            }

            return true;
        }

        #endregion

        #region Total Reserved

        public static bool InsertTotalReserveds(CreateStockFichesDto createdEntity)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            foreach (var line in createdEntity.SelectStockFicheLines)
            {

                #region Grand Total Stock Movement


                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = createdEntity.BranchID, WarehouseID = createdEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id == Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                    {
                        Amount = line.Quantity * (-1),
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = 0,
                        TotalWarehouseOutput = 0,
                        TotalWarehouseInput = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalReserved = line.Quantity,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                else
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                    {
                        Amount = entityGrandTotal.Amount - line.Quantity,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityGrandTotal.TotalConsumption,
                        TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                        TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                        TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                        TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                        TotalProduction = entityGrandTotal.TotalProduction,
                        TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                        TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                        TotalReserved = entityGrandTotal.TotalReserved + line.Quantity,
                        TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                        TotalWastage = entityGrandTotal.TotalWastage,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        Id = entityGrandTotal.Id,
                        CreationTime = entityGrandTotal.CreationTime.Value,
                        CreatorId = entityGrandTotal.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityGrandTotal.IsDeleted,
                        LastModificationTime = now,
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityGrandTotal.Id }, "");

                    var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                #endregion


            }

            return true;
        }

        public static bool UpdateTotalReserveds(SelectStockFichesDto previousEntity, UpdateStockFichesDto currentEntity)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            foreach (var line in currentEntity.SelectStockFicheLines)
            {


                #region Grand Total Stock Movement

                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = currentEntity.BranchID, WarehouseID = currentEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id == Guid.Empty)
                {


                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                    {
                        Amount = line.Quantity * (-1),
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = 0,
                        TotalWarehouseOutput = 0,
                        TotalWarehouseInput = 0,
                        TotalGoodsReceipt = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalReserved = line.Quantity,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = currentEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = currentEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);

                }

                else
                {
                    decimal previousQuantity = previousEntity.SelectStockFicheLines.Where(t => t.Id == line.Id).Select(t => t.Quantity).First();

                    if (line.Quantity > previousQuantity)
                    {
                        decimal addedPR = line.Quantity - previousQuantity;

                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount - addedPR,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                            TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                            TotalProduction = entityGrandTotal.TotalProduction,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                            TotalReserved = entityGrandTotal.TotalReserved + addedPR,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                            TotalWastage = entityGrandTotal.TotalWastage,
                            WarehouseID = entityGrandTotal.WarehouseID,
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = now,
                            BranchID = entityGrandTotal.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);

                    }
                    else if (line.Quantity < previousQuantity)
                    {
                        decimal decreasedPR = previousQuantity - line.Quantity;

                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount + decreasedPR,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                            TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                            TotalProduction = entityGrandTotal.TotalProduction,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                            TotalReserved = entityGrandTotal.TotalReserved - decreasedPR,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                            TotalWastage = entityGrandTotal.TotalWastage,
                            WarehouseID = entityGrandTotal.WarehouseID,
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = now,
                            BranchID = entityGrandTotal.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                    }

                    #endregion
                }

            }

            return true;
        }

        public static bool DeleteTotalReservedLines(SelectStockFicheLinesDto deletedLine)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            #region Getting Total Reserveds

            var query = queryFactory
                   .Query()
                   .From(Tables.StockFiches)
                   .Select<StockFiches>(null)
                   .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchID = b.Id },
                        nameof(StockFiches.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                   .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code, WarehouseID = w.Id },
                        nameof(StockFiches.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = deletedLine.StockFicheID }, Tables.StockFiches);

            var stockFiches = queryFactory.Get<SelectStockFichesDto>(query);

            #endregion

            #region Grand Total Stock Movement

            var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = stockFiches.BranchID, WarehouseID = stockFiches.WarehouseID, ProductID = deletedLine.ProductID }, "");

            var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

            var queryDecreasingGrand = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
            {
                Amount = entityGrandTotal.Amount + deletedLine.Quantity,
                ProductID = deletedLine.ProductID.GetValueOrDefault(),
                TotalConsumption = entityGrandTotal.TotalConsumption,
                TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                TotalProduction = entityGrandTotal.TotalProduction,
                TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                TotalReserved = entityGrandTotal.TotalReserved - deletedLine.Quantity,
                TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                TotalWastage = entityGrandTotal.TotalWastage,
                WarehouseID = entityGrandTotal.WarehouseID,
                Id = entityGrandTotal.Id,
                CreationTime = entityGrandTotal.CreationTime.Value,
                CreatorId = entityGrandTotal.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                IsDeleted = entityGrandTotal.IsDeleted,
                LastModificationTime = now,
                BranchID = entityGrandTotal.BranchID,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = entityGrandTotal.Id }, "");

            var grandTotalStockMovementsDecreasing = queryFactory.Update<SelectGrandTotalStockMovementsDto>(queryDecreasingGrand, "Id", true);

            #endregion

            return true;
        }

        public static bool DeleteTotalReserveds(SelectStockFichesDto deletedEntity)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            #region Getting Lines

            var queryLines = queryFactory
                    .Query()
                    .From(Tables.StockFicheLines)
                    .Select<StockFicheLines>(null)
                    .Join<Products>
                     (
                         p => new { ProductCode = p.Code, ProductName = p.Name },
                         nameof(StockFicheLines.ProductID),
                         nameof(Products.Id),
                         JoinType.Left
                     )
                    .Join<UnitSets>
                     (
                         u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                         nameof(StockFicheLines.UnitSetID),
                         nameof(UnitSets.Id),
                         JoinType.Left
                     )
                     .Where(new { StockFicheID = deletedEntity.Id }, Tables.StockFicheLines);

            var stockFicheLine = queryFactory.GetList<SelectStockFicheLinesDto>(queryLines).ToList();

            deletedEntity.SelectStockFicheLines = stockFicheLine;

            #endregion

            foreach (var line in deletedEntity.SelectStockFicheLines)
            {
                #region Grand Total Stock Movement

                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = deletedEntity.BranchID, WarehouseID = deletedEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id != Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                    {
                        Amount = entityGrandTotal.Amount + line.Quantity,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityGrandTotal.TotalConsumption,
                        TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                        TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                        TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                        TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                        TotalProduction = entityGrandTotal.TotalProduction,
                        TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                        TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                        TotalReserved = entityGrandTotal.TotalReserved - line.Quantity,
                        TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                        TotalWastage = entityGrandTotal.TotalWastage,
                        WarehouseID = entityGrandTotal.WarehouseID,
                        Id = entityGrandTotal.Id,
                        CreationTime = entityGrandTotal.CreationTime.Value,
                        CreatorId = entityGrandTotal.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityGrandTotal.IsDeleted,
                        LastModificationTime = now,
                        BranchID = entityGrandTotal.BranchID,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityGrandTotal.Id }, "");

                    var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                #endregion

            }

            return true;
        }

        #endregion

        #region Total Productions

        public static bool InsertTotalProductions(CreateStockFichesDto createdEntity)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            foreach (var line in createdEntity.SelectStockFicheLines)
            {
                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = createdEntity.BranchID, WarehouseID = createdEntity.WarehouseID, ProductID = line.ProductID, Date_ = createdEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id == Guid.Empty || entityByDate == null)
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                    {
                        Amount = line.Quantity,
                        Date_ = createdEntity.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = 0,
                        TotalWarehouseOutput = 0,
                        TotalWarehouseInput = 0,
                        TotalGoodsReceipt = 0,
                        TotalProduction = line.Quantity,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var byDateStockMovements = queryFactory.Insert<SelectByDateStockMovementsDto>(query, "Id", true);
                }

                else
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                    {
                        Amount = entityByDate.Amount + line.Quantity,
                        Date_ = entityByDate.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityByDate.TotalConsumption,
                        TotalWarehouseInput = entityByDate.TotalConsumption,
                        TotalWarehouseOutput = entityByDate.TotalConsumption,
                        TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                        TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                        TotalProduction = entityByDate.TotalProduction + line.Quantity,
                        TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                        TotalSalesOrder = entityByDate.TotalSalesOrder,
                        TotalSalesProposition = entityByDate.TotalSalesProposition,
                        TotalWastage = entityByDate.TotalWastage,
                        WarehouseID = entityByDate.WarehouseID,
                        BranchID = entityByDate.BranchID,
                        Id = entityByDate.Id,
                        CreationTime = entityByDate.CreationTime.Value,
                        CreatorId = entityByDate.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityByDate.IsDeleted,
                        LastModificationTime = now,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityByDate.Id }, "");

                    var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                }

                #endregion

                #region Grand Total Stock Movement


                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = createdEntity.BranchID, WarehouseID = createdEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id == Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                    {
                        Amount = line.Quantity,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalWarehouseOutput = 0,
                        TotalWarehouseInput = 0,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = 0,
                        TotalProduction = line.Quantity,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalReserved = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                else
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                    {
                        Amount = entityGrandTotal.Amount + line.Quantity,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityGrandTotal.TotalConsumption,
                        TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                        TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                        TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                        TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                        TotalProduction = entityGrandTotal.TotalProduction + line.Quantity,
                        TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                        TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                        TotalReserved = entityGrandTotal.TotalReserved,
                        TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                        TotalWastage = entityGrandTotal.TotalWastage,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        Id = entityGrandTotal.Id,
                        CreationTime = entityGrandTotal.CreationTime.Value,
                        CreatorId = entityGrandTotal.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityGrandTotal.IsDeleted,
                        LastModificationTime = now,
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityGrandTotal.Id }, "");

                    var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                #endregion


            }

            return true;
        }

        public static bool UpdateTotalProductions(SelectStockFichesDto previousEntity, UpdateStockFichesDto currentEntity)
        {
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            foreach (var line in currentEntity.SelectStockFicheLines)
            {

                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = currentEntity.BranchID, WarehouseID = currentEntity.WarehouseID, ProductID = line.ProductID, Date_ = currentEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id == Guid.Empty)
                {


                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                    {
                        Amount = line.Quantity,
                        Date_ = currentEntity.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = 0,
                        TotalWarehouseInput = 0,
                        TotalWarehouseOutput = 0,
                        TotalGoodsReceipt = 0,
                        TotalProduction = line.Quantity,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = currentEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = currentEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var byDateStockMovements = queryFactory.Insert<SelectByDateStockMovementsDto>(query, "Id", true);


                }

                else
                {
                    decimal previousQuantity = previousEntity.SelectStockFicheLines.Where(t => t.Id == line.Id).Select(t => t.Quantity).First();

                    if (line.Quantity > previousQuantity)
                    {
                        decimal addedPR = line.Quantity - previousQuantity;

                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                        {
                            Amount = entityByDate.Amount + addedPR,
                            Date_ = entityByDate.Date_,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityByDate.TotalConsumption,
                            TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                            TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                            TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                            TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                            TotalProduction = entityByDate.TotalProduction + addedPR,
                            TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                            TotalSalesOrder = entityByDate.TotalSalesOrder,
                            TotalSalesProposition = entityByDate.TotalSalesProposition,
                            TotalWastage = entityByDate.TotalWastage,
                            WarehouseID = entityByDate.WarehouseID,
                            BranchID = entityByDate.BranchID,
                            Id = entityByDate.Id,
                            CreationTime = entityByDate.CreationTime.Value,
                            CreatorId = entityByDate.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityByDate.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityByDate.Id }, "");

                        var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);

                    }

                    else if (line.Quantity < previousQuantity)
                    {
                        decimal decreasedPR = previousQuantity - line.Quantity;

                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                        {
                            Amount = entityByDate.Amount - decreasedPR,
                            Date_ = entityByDate.Date_,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityByDate.TotalConsumption,
                            TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                            TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                            TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                            TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                            TotalProduction = entityByDate.TotalProduction - decreasedPR,
                            TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                            TotalSalesOrder = entityByDate.TotalSalesOrder,
                            TotalSalesProposition = entityByDate.TotalSalesProposition,
                            TotalWastage = entityByDate.TotalWastage,
                            WarehouseID = entityByDate.WarehouseID,
                            BranchID = entityByDate.BranchID,
                            Id = entityByDate.Id,
                            CreationTime = entityByDate.CreationTime.Value,
                            CreatorId = entityByDate.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityByDate.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityByDate.Id }, "");

                        var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                    }

                }

                #endregion

                #region Grand Total Stock Movement

                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = currentEntity.BranchID, WarehouseID = currentEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id == Guid.Empty)
                {


                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                    {
                        Amount = line.Quantity,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalWarehouseOutput = 0,
                        TotalWarehouseInput = 0,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = 0,
                        TotalProduction = line.Quantity,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalReserved = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = currentEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = currentEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);

                }

                else
                {
                    decimal previousQuantity = previousEntity.SelectStockFicheLines.Where(t => t.Id == line.Id).Select(t => t.Quantity).First();

                    if (line.Quantity > previousQuantity)
                    {
                        decimal addedPR = line.Quantity - previousQuantity;

                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount + addedPR,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                            TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                            TotalProduction = entityGrandTotal.TotalProduction + addedPR,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                            TotalReserved = entityGrandTotal.TotalReserved,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                            TotalWastage = entityGrandTotal.TotalWastage,
                            WarehouseID = entityGrandTotal.WarehouseID,
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = now,
                            BranchID = entityGrandTotal.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);

                    }

                    else if (line.Quantity < previousQuantity)
                    {
                        decimal decreasedPR = previousQuantity - line.Quantity;

                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount - decreasedPR,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                            TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                            TotalProduction = entityGrandTotal.TotalProduction - decreasedPR,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                            TotalReserved = entityGrandTotal.TotalReserved,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                            TotalWastage = entityGrandTotal.TotalWastage,
                            WarehouseID = entityGrandTotal.WarehouseID,
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = now,
                            BranchID = entityGrandTotal.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                    }

                    #endregion
                }

            }

            return true;
        }

        public static bool DeleteTotalProductionLines(SelectStockFicheLinesDto deletedLine)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            #region Getting Total Production

            var query = queryFactory
                   .Query()
                   .From(Tables.StockFiches)
                   .Select<StockFiches>(null)
                   .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchID = b.Id },
                        nameof(StockFiches.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                   .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code, WarehouseID = w.Id },
                        nameof(StockFiches.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = deletedLine.StockFicheID }, Tables.StockFiches);

            var stockFiches = queryFactory.Get<SelectStockFichesDto>(query);

            #endregion

            #region By Date Stock Movement

            var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = stockFiches.BranchID, WarehouseID = stockFiches.WarehouseID, ProductID = deletedLine.ProductID, Date_ = stockFiches.Date_ }, "");

            var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

            var queryDecreasingDate = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
            {
                Amount = entityByDate.Amount - deletedLine.Quantity,
                Date_ = entityByDate.Date_,
                ProductID = entityByDate.ProductID,
                TotalConsumption = entityByDate.TotalConsumption,
                TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                TotalProduction = entityByDate.TotalProduction - deletedLine.Quantity,
                TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                TotalSalesOrder = entityByDate.TotalSalesOrder,
                TotalSalesProposition = entityByDate.TotalSalesProposition,
                TotalWastage = entityByDate.TotalWastage,
                WarehouseID = entityByDate.WarehouseID,
                BranchID = entityByDate.BranchID,
                Id = entityByDate.Id,
                CreationTime = entityByDate.CreationTime.Value,
                CreatorId = entityByDate.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                IsDeleted = entityByDate.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = entityByDate.Id }, "");

            var byDateStockMovementsDecreasing = queryFactory.Update<SelectByDateStockMovementsDto>(queryDecreasingDate, "Id", true);

            #endregion

            #region Grand Total Stock Movement

            var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = stockFiches.BranchID, WarehouseID = stockFiches.WarehouseID, ProductID = deletedLine.ProductID }, "");

            var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

            var queryDecreasingGrand = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
            {
                Amount = entityGrandTotal.Amount - deletedLine.Quantity,
                ProductID = deletedLine.ProductID.GetValueOrDefault(),
                TotalConsumption = entityGrandTotal.TotalConsumption,
                TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                TotalProduction = entityGrandTotal.TotalProduction - deletedLine.Quantity,
                TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                TotalReserved = entityGrandTotal.TotalReserved,
                TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                TotalWastage = entityGrandTotal.TotalWastage,
                WarehouseID = entityGrandTotal.WarehouseID,
                Id = entityGrandTotal.Id,
                CreationTime = entityGrandTotal.CreationTime.Value,
                CreatorId = entityGrandTotal.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                IsDeleted = entityGrandTotal.IsDeleted,
                LastModificationTime = now,
                BranchID = entityGrandTotal.BranchID,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = entityGrandTotal.Id }, "");

            var grandTotalStockMovementsDecreasing = queryFactory.Update<SelectGrandTotalStockMovementsDto>(queryDecreasingGrand, "Id", true);

            #endregion

            return true;
        }

        public static bool DeleteTotalProductions(SelectStockFichesDto deletedEntity)
        {
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            #region Getting Lines

            var queryLines = queryFactory
                    .Query()
                    .From(Tables.StockFicheLines)
                    .Select<StockFicheLines>(null)
                    .Join<Products>
                     (
                         p => new { ProductCode = p.Code, ProductName = p.Name },
                         nameof(StockFicheLines.ProductID),
                         nameof(Products.Id),
                         JoinType.Left
                     )
                    .Join<UnitSets>
                     (
                         u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                         nameof(StockFicheLines.UnitSetID),
                         nameof(UnitSets.Id),
                         JoinType.Left
                     )
                     .Where(new { StockFicheID = deletedEntity.Id }, Tables.StockFicheLines);

            var stockFicheLine = queryFactory.GetList<SelectStockFicheLinesDto>(queryLines).ToList();

            deletedEntity.SelectStockFicheLines = stockFicheLine;

            #endregion

            foreach (var line in deletedEntity.SelectStockFicheLines)
            {

                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = deletedEntity.BranchID, WarehouseID = deletedEntity.WarehouseID, ProductID = line.ProductID, Date_ = deletedEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id != Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                    {
                        Amount = entityByDate.Amount - line.Quantity,
                        Date_ = entityByDate.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityByDate.TotalConsumption,
                        TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                        TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                        TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                        TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                        TotalProduction = entityByDate.TotalProduction - line.Quantity,
                        TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                        TotalSalesOrder = entityByDate.TotalSalesOrder,
                        TotalSalesProposition = entityByDate.TotalSalesProposition,
                        TotalWastage = entityByDate.TotalWastage,
                        WarehouseID = entityByDate.WarehouseID,
                        BranchID = entityByDate.BranchID,
                        Id = entityByDate.Id,
                        CreationTime = entityByDate.CreationTime.Value,
                        CreatorId = entityByDate.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityByDate.IsDeleted,
                        LastModificationTime = now,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityByDate.Id }, "");

                    var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                }

                #endregion

                #region Grand Total Stock Movement

                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = deletedEntity.BranchID, WarehouseID = deletedEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id != Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                    {
                        Amount = entityGrandTotal.Amount - line.Quantity,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityGrandTotal.TotalConsumption,
                        TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                        TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                        TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                        TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                        TotalProduction = entityGrandTotal.TotalProduction - line.Quantity,
                        TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                        TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                        TotalReserved = entityGrandTotal.TotalReserved,
                        TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                        TotalWastage = entityGrandTotal.TotalWastage,
                        WarehouseID = entityGrandTotal.WarehouseID,
                        Id = entityGrandTotal.Id,
                        CreationTime = entityGrandTotal.CreationTime.Value,
                        CreatorId = entityGrandTotal.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityGrandTotal.IsDeleted,
                        LastModificationTime = now,
                        BranchID = entityGrandTotal.BranchID,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityGrandTotal.Id }, "");

                    var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                #endregion
            }

            return true;
        }

        #endregion

        #region Total Goods Fiches

        public static bool InsertTotalGoods(CreateStockFichesDto createdEntity)
        {
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            foreach (var line in createdEntity.SelectStockFicheLines)
            {
                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = createdEntity.BranchID, WarehouseID = createdEntity.WarehouseID, ProductID = line.ProductID, Date_ = createdEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id == Guid.Empty || entityByDate == null)
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                    {
                        Amount = line.Quantity,
                        Date_ = createdEntity.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = 0,
                        TotalWarehouseOutput = 0,
                        TotalWarehouseInput = 0,
                        TotalGoodsReceipt = line.Quantity,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var byDateStockMovements = queryFactory.Insert<SelectByDateStockMovementsDto>(query, "Id", true);
                }

                else
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                    {
                        Amount = entityByDate.Amount + line.Quantity,
                        Date_ = entityByDate.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityByDate.TotalConsumption,
                        TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                        TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                        TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                        TotalGoodsReceipt = entityByDate.TotalGoodsReceipt + line.Quantity,
                        TotalProduction = entityByDate.TotalProduction,
                        TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                        TotalSalesOrder = entityByDate.TotalSalesOrder,
                        TotalSalesProposition = entityByDate.TotalSalesProposition,
                        TotalWastage = entityByDate.TotalWastage,
                        WarehouseID = entityByDate.WarehouseID,
                        BranchID = entityByDate.BranchID,
                        Id = entityByDate.Id,
                        CreationTime = entityByDate.CreationTime.Value,
                        CreatorId = entityByDate.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityByDate.IsDeleted,
                        LastModificationTime = now,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityByDate.Id }, "");

                    var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                }

                #endregion

                #region Grand Total Stock Movement


                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = createdEntity.BranchID, WarehouseID = createdEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id == Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                    {
                        Amount = line.Quantity,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = line.Quantity,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalWarehouseOutput = 0,
                        TotalWarehouseInput = 0,
                        TotalSalesOrder = 0,
                        TotalReserved = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                else
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                    {
                        Amount = entityGrandTotal.Amount + line.Quantity,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityGrandTotal.TotalConsumption,
                        TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                        TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                        TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                        TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt + line.Quantity,
                        TotalProduction = entityGrandTotal.TotalProduction,
                        TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                        TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                        TotalReserved = entityGrandTotal.TotalReserved,
                        TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                        TotalWastage = entityGrandTotal.TotalWastage,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        Id = entityGrandTotal.Id,
                        CreationTime = entityGrandTotal.CreationTime.Value,
                        CreatorId = entityGrandTotal.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityGrandTotal.IsDeleted,
                        LastModificationTime = now,
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityGrandTotal.Id }, "");

                    var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                #endregion


            }

            return true;
        }

        public static bool UpdateTotalGoods(SelectStockFichesDto previousEntity, UpdateStockFichesDto currentEntity)
        {
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            foreach (var line in currentEntity.SelectStockFicheLines)
            {

                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = currentEntity.BranchID, WarehouseID = currentEntity.WarehouseID, ProductID = line.ProductID, Date_ = currentEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id == Guid.Empty)
                {


                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                    {
                        Amount = line.Quantity,
                        Date_ = currentEntity.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = line.Quantity,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalWarehouseInput = 0,
                        TotalWarehouseOutput = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = currentEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = currentEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var byDateStockMovements = queryFactory.Insert<SelectByDateStockMovementsDto>(query, "Id", true);


                }

                else
                {
                    decimal previousQuantity = previousEntity.SelectStockFicheLines.Where(t => t.Id == line.Id).Select(t => t.Quantity).First();

                    if (line.Quantity > previousQuantity)
                    {
                        decimal addedPR = line.Quantity - previousQuantity;

                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                        {
                            Amount = entityByDate.Amount + addedPR,
                            Date_ = entityByDate.Date_,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityByDate.TotalConsumption,
                            TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                            TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                            TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                            TotalGoodsReceipt = entityByDate.TotalGoodsReceipt + addedPR,
                            TotalProduction = entityByDate.TotalProduction,
                            TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                            TotalSalesOrder = entityByDate.TotalSalesOrder,
                            TotalSalesProposition = entityByDate.TotalSalesProposition,
                            TotalWastage = entityByDate.TotalWastage,
                            WarehouseID = entityByDate.WarehouseID,
                            BranchID = entityByDate.BranchID,
                            Id = entityByDate.Id,
                            CreationTime = entityByDate.CreationTime.Value,
                            CreatorId = entityByDate.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityByDate.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityByDate.Id }, "");

                        var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);

                    }

                    else if (line.Quantity < previousQuantity)
                    {
                        decimal decreasedPR = previousQuantity - line.Quantity;

                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                        {
                            Amount = entityByDate.Amount - decreasedPR,
                            Date_ = entityByDate.Date_,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityByDate.TotalConsumption,
                            TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                            TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                            TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                            TotalGoodsReceipt = entityByDate.TotalGoodsReceipt - decreasedPR,
                            TotalProduction = entityByDate.TotalProduction,
                            TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                            TotalSalesOrder = entityByDate.TotalSalesOrder,
                            TotalSalesProposition = entityByDate.TotalSalesProposition,
                            TotalWastage = entityByDate.TotalWastage,
                            WarehouseID = entityByDate.WarehouseID,
                            BranchID = entityByDate.BranchID,
                            Id = entityByDate.Id,
                            CreationTime = entityByDate.CreationTime.Value,
                            CreatorId = entityByDate.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityByDate.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityByDate.Id }, "");

                        var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                    }

                }

                #endregion

                #region Grand Total Stock Movement

                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = currentEntity.BranchID, WarehouseID = currentEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id == Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                    {
                        Amount = line.Quantity,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalWarehouseOutput = 0,
                        TotalWarehouseInput = 0,
                        TotalGoodsIssue = 0,
                        TotalGoodsReceipt = line.Quantity,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalReserved = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = currentEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = currentEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);

                }

                else
                {
                    decimal previousQuantity = previousEntity.SelectStockFicheLines.Where(t => t.Id == line.Id).Select(t => t.Quantity).First();

                    if (line.Quantity > previousQuantity)
                    {
                        decimal addedPR = line.Quantity - previousQuantity;

                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount + addedPR,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                            TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt + addedPR,
                            TotalProduction = entityGrandTotal.TotalProduction,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                            TotalReserved = entityGrandTotal.TotalReserved,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                            TotalWastage = entityGrandTotal.TotalWastage,
                            WarehouseID = entityGrandTotal.WarehouseID,
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = now,
                            BranchID = entityGrandTotal.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);

                    }

                    else if (line.Quantity < previousQuantity)
                    {
                        decimal decreasedPR = previousQuantity - line.Quantity;

                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount - decreasedPR,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                            TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt - decreasedPR,
                            TotalProduction = entityGrandTotal.TotalProduction,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                            TotalReserved = entityGrandTotal.TotalReserved,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                            TotalWastage = entityGrandTotal.TotalWastage,
                            WarehouseID = entityGrandTotal.WarehouseID,
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = now,
                            BranchID = entityGrandTotal.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                    }

                }

                #endregion


            }

            return true;
        }

        public static bool DeleteTotalGoodLines(SelectStockFicheLinesDto deletedLine)
        {
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            #region Getting Total Good

            var query = queryFactory
                   .Query()
                   .From(Tables.StockFiches)
                   .Select<StockFiches>(null)
                   .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchID = b.Id },
                        nameof(StockFiches.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                   .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code, WarehouseID = w.Id },
                        nameof(StockFiches.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = deletedLine.StockFicheID }, Tables.StockFiches);

            var stockFiches = queryFactory.Get<SelectStockFichesDto>(query);

            #endregion

            #region By Date Stock Movement

            var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = stockFiches.BranchID, WarehouseID = stockFiches.WarehouseID, ProductID = deletedLine.ProductID, Date_ = stockFiches.Date_ }, "");

            var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

            var queryDecreasingDate = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
            {
                Amount = entityByDate.Amount - deletedLine.Quantity,
                Date_ = entityByDate.Date_,
                ProductID = entityByDate.ProductID,
                TotalConsumption = entityByDate.TotalConsumption,
                TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                TotalGoodsReceipt = entityByDate.TotalGoodsReceipt - deletedLine.Quantity,
                TotalProduction = entityByDate.TotalProduction,
                TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                TotalSalesOrder = entityByDate.TotalSalesOrder,
                TotalSalesProposition = entityByDate.TotalSalesProposition,
                TotalWastage = entityByDate.TotalWastage,
                WarehouseID = entityByDate.WarehouseID,
                BranchID = entityByDate.BranchID,
                Id = entityByDate.Id,
                CreationTime = entityByDate.CreationTime.Value,
                CreatorId = entityByDate.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                IsDeleted = entityByDate.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = entityByDate.Id }, "");

            var byDateStockMovementsDecreasing = queryFactory.Update<SelectByDateStockMovementsDto>(queryDecreasingDate, "Id", true);

            #endregion

            #region Grand Total Stock Movement

            var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = stockFiches.BranchID, WarehouseID = stockFiches.WarehouseID, ProductID = deletedLine.ProductID }, "");

            var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

            var queryDecreasingGrand = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
            {
                Amount = entityGrandTotal.Amount - deletedLine.Quantity,
                ProductID = deletedLine.ProductID.GetValueOrDefault(),
                TotalConsumption = entityGrandTotal.TotalConsumption,
                TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt - deletedLine.Quantity,
                TotalProduction = entityGrandTotal.TotalProduction,
                TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                TotalReserved = entityGrandTotal.TotalReserved,
                TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                TotalWastage = entityGrandTotal.TotalWastage,
                WarehouseID = entityGrandTotal.WarehouseID,
                Id = entityGrandTotal.Id,
                CreationTime = entityGrandTotal.CreationTime.Value,
                CreatorId = entityGrandTotal.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                IsDeleted = entityGrandTotal.IsDeleted,
                LastModificationTime = now,
                BranchID = entityGrandTotal.BranchID,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = entityGrandTotal.Id }, "");

            var grandTotalStockMovementsDecreasing = queryFactory.Update<SelectGrandTotalStockMovementsDto>(queryDecreasingGrand, "Id", true);

            #endregion

            return true;
        }

        public static bool DeleteTotalGoods(SelectStockFichesDto deletedEntity)
        {
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            #region Getting Lines

            var queryLines = queryFactory
                    .Query()
                    .From(Tables.StockFicheLines)
                    .Select<StockFicheLines>(null)
                    .Join<Products>
                     (
                         p => new { ProductCode = p.Code, ProductName = p.Name },
                         nameof(StockFicheLines.ProductID),
                         nameof(Products.Id),
                         JoinType.Left
                     )
                    .Join<UnitSets>
                     (
                         u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                         nameof(StockFicheLines.UnitSetID),
                         nameof(UnitSets.Id),
                         JoinType.Left
                     )
                     .Where(new { StockFicheID = deletedEntity.Id }, Tables.StockFicheLines);

            var stockFicheLine = queryFactory.GetList<SelectStockFicheLinesDto>(queryLines).ToList();

            deletedEntity.SelectStockFicheLines = stockFicheLine;

            #endregion

            foreach (var line in deletedEntity.SelectStockFicheLines)
            {

                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = deletedEntity.BranchID, WarehouseID = deletedEntity.WarehouseID, ProductID = line.ProductID, Date_ = deletedEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id != Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                    {
                        Amount = entityByDate.Amount - line.Quantity,
                        Date_ = entityByDate.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityByDate.TotalConsumption,
                        TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                        TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                        TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                        TotalGoodsReceipt = entityByDate.TotalGoodsReceipt - line.Quantity,
                        TotalProduction = entityByDate.TotalProduction,
                        TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                        TotalSalesOrder = entityByDate.TotalSalesOrder,
                        TotalSalesProposition = entityByDate.TotalSalesProposition,
                        TotalWastage = entityByDate.TotalWastage,
                        WarehouseID = entityByDate.WarehouseID,
                        BranchID = entityByDate.BranchID,
                        Id = entityByDate.Id,
                        CreationTime = entityByDate.CreationTime.Value,
                        CreatorId = entityByDate.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityByDate.IsDeleted,
                        LastModificationTime = now,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityByDate.Id }, "");

                    var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                }

                #endregion

                #region Grand Total Stock Movement

                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = deletedEntity.BranchID, WarehouseID = deletedEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id != Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                    {
                        Amount = entityGrandTotal.Amount - line.Quantity,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityGrandTotal.TotalConsumption,
                        TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                        TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                        TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                        TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt - line.Quantity,
                        TotalProduction = entityGrandTotal.TotalProduction,
                        TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                        TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                        TotalReserved = entityGrandTotal.TotalReserved,
                        TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                        TotalWastage = entityGrandTotal.TotalWastage,
                        WarehouseID = entityGrandTotal.WarehouseID,
                        Id = entityGrandTotal.Id,
                        CreationTime = entityGrandTotal.CreationTime.Value,
                        CreatorId = entityGrandTotal.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityGrandTotal.IsDeleted,
                        LastModificationTime = now,
                        BranchID = entityGrandTotal.BranchID,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityGrandTotal.Id }, "");

                    var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                #endregion
            }

            return true;
        }

        #endregion

        #region Total Goods Issues

        public static bool InsertTotalGoodIssues(CreateStockFichesDto createdEntity)
        {
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            foreach (var line in createdEntity.SelectStockFicheLines)
            {
                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = createdEntity.BranchID, WarehouseID = createdEntity.WarehouseID, ProductID = line.ProductID, Date_ = createdEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id == Guid.Empty || entityByDate == null)
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                    {
                        Amount = line.Quantity * (-1),
                        Date_ = createdEntity.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = line.Quantity,
                        TotalGoodsReceipt = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var byDateStockMovements = queryFactory.Insert<SelectByDateStockMovementsDto>(query, "Id", true);
                }
                else
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                    {
                        Amount = entityByDate.Amount - line.Quantity,
                        Date_ = entityByDate.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityByDate.TotalConsumption,
                        TotalGoodsIssue = entityByDate.TotalGoodsIssue + line.Quantity,
                        TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                        TotalProduction = entityByDate.TotalProduction,
                        TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                        TotalSalesOrder = entityByDate.TotalSalesOrder,
                        TotalSalesProposition = entityByDate.TotalSalesProposition,
                        TotalWastage = entityByDate.TotalWastage,
                        WarehouseID = entityByDate.WarehouseID,
                        BranchID = entityByDate.BranchID,
                        Id = entityByDate.Id,
                        CreationTime = entityByDate.CreationTime.Value,
                        CreatorId = entityByDate.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityByDate.IsDeleted,
                        LastModificationTime = now,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityByDate.Id }, "");

                    var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                }

                #endregion

                #region Grand Total Stock Movement


                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = createdEntity.BranchID, WarehouseID = createdEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id == Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                    {
                        Amount = line.Quantity * (-1),
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = line.Quantity,
                        TotalGoodsReceipt = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalReserved = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                else
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                    {
                        Amount = entityGrandTotal.Amount - line.Quantity,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityGrandTotal.TotalConsumption,
                        TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue + line.Quantity,
                        TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                        TotalProduction = entityGrandTotal.TotalProduction,
                        TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                        TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                        TotalReserved = entityGrandTotal.TotalReserved,
                        TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                        TotalWastage = entityGrandTotal.TotalWastage,
                        WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                        Id = entityGrandTotal.Id,
                        CreationTime = entityGrandTotal.CreationTime.Value,
                        CreatorId = entityGrandTotal.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityGrandTotal.IsDeleted,
                        LastModificationTime = now,
                        BranchID = createdEntity.BranchID.GetValueOrDefault(),
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityGrandTotal.Id }, "");

                    var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                #endregion


            }

            return true;
        }

        public static bool UpdateTotalGoodIssues(SelectStockFichesDto previousEntity, UpdateStockFichesDto currentEntity)
        {
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            foreach (var line in currentEntity.SelectStockFicheLines)
            {

                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = currentEntity.BranchID, WarehouseID = currentEntity.WarehouseID, ProductID = line.ProductID, Date_ = currentEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id == Guid.Empty)
                {


                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                    {
                        Amount = line.Quantity * (-1),
                        Date_ = currentEntity.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = line.Quantity,
                        TotalGoodsReceipt = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalWarehouseInput = 0,
                        TotalWarehouseOutput = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = currentEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = currentEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var byDateStockMovements = queryFactory.Insert<SelectByDateStockMovementsDto>(query, "Id", true);


                }

                else
                {
                    decimal previousQuantity = previousEntity.SelectStockFicheLines.Where(t => t.Id == line.Id).Select(t => t.Quantity).First();

                    if (line.Quantity > previousQuantity)
                    {
                        decimal addedPR = line.Quantity - previousQuantity;

                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                        {
                            Amount = entityByDate.Amount - addedPR,
                            Date_ = entityByDate.Date_,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityByDate.TotalConsumption,
                            TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                            TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                            TotalGoodsIssue = entityByDate.TotalGoodsIssue + addedPR,
                            TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                            TotalProduction = entityByDate.TotalProduction,
                            TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                            TotalSalesOrder = entityByDate.TotalSalesOrder,
                            TotalSalesProposition = entityByDate.TotalSalesProposition,
                            TotalWastage = entityByDate.TotalWastage,
                            WarehouseID = entityByDate.WarehouseID,
                            BranchID = entityByDate.BranchID,
                            Id = entityByDate.Id,
                            CreationTime = entityByDate.CreationTime.Value,
                            CreatorId = entityByDate.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityByDate.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityByDate.Id }, "");

                        var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);

                    }

                    else if (line.Quantity < previousQuantity)
                    {
                        decimal decreasedPR = previousQuantity - line.Quantity;

                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                        {
                            Amount = entityByDate.Amount + decreasedPR,
                            Date_ = entityByDate.Date_,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityByDate.TotalConsumption,
                            TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                            TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                            TotalGoodsIssue = entityByDate.TotalGoodsIssue - decreasedPR,
                            TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                            TotalProduction = entityByDate.TotalProduction,
                            TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                            TotalSalesOrder = entityByDate.TotalSalesOrder,
                            TotalSalesProposition = entityByDate.TotalSalesProposition,
                            TotalWastage = entityByDate.TotalWastage,
                            WarehouseID = entityByDate.WarehouseID,
                            BranchID = entityByDate.BranchID,
                            Id = entityByDate.Id,
                            CreationTime = entityByDate.CreationTime.Value,
                            CreatorId = entityByDate.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityByDate.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityByDate.Id }, "");

                        var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                    }

                }

                #endregion

                #region Grand Total Stock Movement

                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = currentEntity.BranchID, WarehouseID = currentEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id == Guid.Empty)
                {


                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                    {
                        Amount = line.Quantity * (-1),
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = 0,
                        TotalGoodsIssue = line.Quantity,
                        TotalGoodsReceipt = 0,
                        TotalWarehouseOutput = 0,
                        TotalWarehouseInput = 0,
                        TotalProduction = 0,
                        TotalPurchaseOrder = 0,
                        TotalPurchaseRequest = 0,
                        TotalSalesOrder = 0,
                        TotalReserved = 0,
                        TotalSalesProposition = 0,
                        TotalWastage = 0,
                        WarehouseID = currentEntity.WarehouseID.GetValueOrDefault(),
                        BranchID = currentEntity.BranchID.GetValueOrDefault(),
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
                    });

                    var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);

                }

                else
                {
                    decimal previousQuantity = previousEntity.SelectStockFicheLines.Where(t => t.Id == line.Id).Select(t => t.Quantity).First();

                    if (line.Quantity > previousQuantity)
                    {
                        decimal addedPR = line.Quantity - previousQuantity;

                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount - addedPR,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                            TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue + addedPR,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                            TotalProduction = entityGrandTotal.TotalProduction,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                            TotalReserved = entityGrandTotal.TotalReserved,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                            TotalWastage = entityGrandTotal.TotalWastage,
                            WarehouseID = entityGrandTotal.WarehouseID,
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = now,
                            BranchID = entityGrandTotal.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);

                    }

                    else if (line.Quantity < previousQuantity)
                    {
                        decimal decreasedPR = previousQuantity - line.Quantity;

                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount + decreasedPR,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                            TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue - decreasedPR,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                            TotalProduction = entityGrandTotal.TotalProduction,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                            TotalReserved = entityGrandTotal.TotalReserved,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                            TotalWastage = entityGrandTotal.TotalWastage,
                            WarehouseID = entityGrandTotal.WarehouseID,
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = now,
                            BranchID = entityGrandTotal.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                    }

                    #endregion
                }

            }

            return true;
        }

        public static bool DeleteTotalGoodIssueLines(SelectStockFicheLinesDto deletedLine)
        {
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            #region Getting Total Good Issue

            var query = queryFactory
                   .Query()
                   .From(Tables.StockFiches)
                   .Select<StockFiches>(null)
                   .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchID = b.Id },
                        nameof(StockFiches.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                   .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code, WarehouseID = w.Id },
                        nameof(StockFiches.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = deletedLine.StockFicheID }, Tables.StockFiches);

            var stockFiches = queryFactory.Get<SelectStockFichesDto>(query);

            #endregion

            #region By Date Stock Movement

            var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = stockFiches.BranchID, WarehouseID = stockFiches.WarehouseID, ProductID = deletedLine.ProductID, Date_ = stockFiches.Date_ }, "");

            var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

            var queryDecreasingDate = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
            {
                Amount = entityByDate.Amount + deletedLine.Quantity,
                Date_ = entityByDate.Date_,
                ProductID = entityByDate.ProductID,
                TotalConsumption = entityByDate.TotalConsumption,
                TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                TotalGoodsIssue = entityByDate.TotalGoodsIssue - deletedLine.Quantity,
                TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                TotalProduction = entityByDate.TotalProduction,
                TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                TotalSalesOrder = entityByDate.TotalSalesOrder,
                TotalSalesProposition = entityByDate.TotalSalesProposition,
                TotalWastage = entityByDate.TotalWastage,
                WarehouseID = entityByDate.WarehouseID,
                BranchID = entityByDate.BranchID,
                Id = entityByDate.Id,
                CreationTime = entityByDate.CreationTime.Value,
                CreatorId = entityByDate.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                IsDeleted = entityByDate.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = entityByDate.Id }, "");

            var byDateStockMovementsDecreasing = queryFactory.Update<SelectByDateStockMovementsDto>(queryDecreasingDate, "Id", true);

            #endregion

            #region Grand Total Stock Movement

            var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = stockFiches.BranchID, WarehouseID = stockFiches.WarehouseID, ProductID = deletedLine.ProductID }, "");

            var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

            var queryDecreasingGrand = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
            {
                Amount = entityGrandTotal.Amount + deletedLine.Quantity,
                ProductID = deletedLine.ProductID.GetValueOrDefault(),
                TotalConsumption = entityGrandTotal.TotalConsumption,
                TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue - deletedLine.Quantity,
                TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                TotalProduction = entityGrandTotal.TotalProduction,
                TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                TotalReserved = entityGrandTotal.TotalReserved,
                TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                TotalWastage = entityGrandTotal.TotalWastage,
                WarehouseID = entityGrandTotal.WarehouseID,
                Id = entityGrandTotal.Id,
                CreationTime = entityGrandTotal.CreationTime.Value,
                CreatorId = entityGrandTotal.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                IsDeleted = entityGrandTotal.IsDeleted,
                LastModificationTime = now,
                BranchID = entityGrandTotal.BranchID,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = entityGrandTotal.Id }, "");

            var grandTotalStockMovementsDecreasing = queryFactory.Update<SelectGrandTotalStockMovementsDto>(queryDecreasingGrand, "Id", true);

            #endregion

            return true;
        }

        public static bool DeleteTotalGoodIssues(SelectStockFichesDto deletedEntity)
        {
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            #region Getting Lines

            var queryLines = queryFactory
                    .Query()
                    .From(Tables.StockFicheLines)
                    .Select<StockFicheLines>(null)
                    .Join<Products>
                     (
                         p => new { ProductCode = p.Code, ProductName = p.Name },
                         nameof(StockFicheLines.ProductID),
                         nameof(Products.Id),
                         JoinType.Left
                     )
                    .Join<UnitSets>
                     (
                         u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                         nameof(StockFicheLines.UnitSetID),
                         nameof(UnitSets.Id),
                         JoinType.Left
                     )
                     .Where(new { StockFicheID = deletedEntity.Id }, Tables.StockFicheLines);

            var stockFicheLine = queryFactory.GetList<SelectStockFicheLinesDto>(queryLines).ToList();

            deletedEntity.SelectStockFicheLines = stockFicheLine;

            #endregion

            foreach (var line in deletedEntity.SelectStockFicheLines)
            {

                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = deletedEntity.BranchID, WarehouseID = deletedEntity.WarehouseID, ProductID = line.ProductID, Date_ = deletedEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id != Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                    {
                        Amount = entityByDate.Amount + line.Quantity,
                        Date_ = entityByDate.Date_,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityByDate.TotalConsumption,
                        TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                        TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                        TotalGoodsIssue = entityByDate.TotalGoodsIssue - line.Quantity,
                        TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                        TotalProduction = entityByDate.TotalProduction,
                        TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                        TotalSalesOrder = entityByDate.TotalSalesOrder,
                        TotalSalesProposition = entityByDate.TotalSalesProposition,
                        TotalWastage = entityByDate.TotalWastage,
                        WarehouseID = entityByDate.WarehouseID,
                        BranchID = entityByDate.BranchID,
                        Id = entityByDate.Id,
                        CreationTime = entityByDate.CreationTime.Value,
                        CreatorId = entityByDate.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityByDate.IsDeleted,
                        LastModificationTime = now,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityByDate.Id }, "");

                    var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                }

                #endregion

                #region Grand Total Stock Movement

                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = deletedEntity.BranchID, WarehouseID = deletedEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id != Guid.Empty)
                {
                    var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                    {
                        Amount = entityGrandTotal.Amount + line.Quantity,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        TotalConsumption = entityGrandTotal.TotalConsumption,
                        TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                        TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                        TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue - line.Quantity,
                        TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                        TotalProduction = entityGrandTotal.TotalProduction,
                        TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                        TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                        TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                        TotalReserved = entityGrandTotal.TotalReserved,
                        TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                        TotalWastage = entityGrandTotal.TotalWastage,
                        WarehouseID = entityGrandTotal.WarehouseID,
                        Id = entityGrandTotal.Id,
                        CreationTime = entityGrandTotal.CreationTime.Value,
                        CreatorId = entityGrandTotal.CreatorId.Value,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                        DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                        IsDeleted = entityGrandTotal.IsDeleted,
                        LastModificationTime = now,
                        BranchID = entityGrandTotal.BranchID,
                        LastModifierId = LoginedUserService.UserId
                    }).Where(new { Id = entityGrandTotal.Id }, "");

                    var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                }

                #endregion
            }

            return true;
        }

        #endregion

        #region Warehouse Shipping Fiches

        public static bool InsertTotalWarehouseShippings(CreateStockFichesDto createdEntity)
        {

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            foreach (var line in createdEntity.SelectStockFicheLines)
            {
                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = createdEntity.BranchID, WarehouseID = createdEntity.WarehouseID, ProductID = line.ProductID, Date_ = createdEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id == Guid.Empty || entityByDate == null)
                {
                    if (createdEntity.InputOutputCode == 1)
                    {
                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                        {
                            Amount = line.Quantity * (-1),
                            Date_ = createdEntity.Date_,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = 0,
                            TotalGoodsIssue = 0,
                            TotalGoodsReceipt = 0,
                            TotalWarehouseOutput = line.Quantity,
                            TotalWarehouseInput = 0,
                            TotalProduction = 0,
                            TotalPurchaseOrder = 0,
                            TotalPurchaseRequest = 0,
                            TotalSalesOrder = 0,
                            TotalSalesProposition = 0,
                            TotalWastage = 0,
                            WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                            BranchID = createdEntity.BranchID.GetValueOrDefault(),
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
                        });

                        var byDateStockMovements = queryFactory.Insert<SelectByDateStockMovementsDto>(query, "Id", true);
                    }

                    else if (createdEntity.InputOutputCode == 0)
                    {
                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                        {
                            Amount = line.Quantity,
                            Date_ = createdEntity.Date_,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = 0,
                            TotalGoodsIssue = 0,
                            TotalGoodsReceipt = 0,
                            TotalWarehouseOutput = 0,
                            TotalWarehouseInput = line.Quantity,
                            TotalProduction = 0,
                            TotalPurchaseOrder = 0,
                            TotalPurchaseRequest = 0,
                            TotalSalesOrder = 0,
                            TotalSalesProposition = 0,
                            TotalWastage = 0,
                            WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                            BranchID = createdEntity.BranchID.GetValueOrDefault(),
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
                        });

                        var byDateStockMovements = queryFactory.Insert<SelectByDateStockMovementsDto>(query, "Id", true);
                    }

                }

                else
                {
                    if (createdEntity.InputOutputCode == 1)
                    {
                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                        {
                            Amount = entityByDate.Amount - line.Quantity,
                            Date_ = entityByDate.Date_,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityByDate.TotalConsumption,
                            TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                            TotalWarehouseOutput = entityByDate.TotalWarehouseOutput + line.Quantity,
                            TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                            TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                            TotalProduction = entityByDate.TotalProduction,
                            TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                            TotalSalesOrder = entityByDate.TotalSalesOrder,
                            TotalSalesProposition = entityByDate.TotalSalesProposition,
                            TotalWastage = entityByDate.TotalWastage,
                            WarehouseID = entityByDate.WarehouseID,
                            BranchID = entityByDate.BranchID,
                            Id = entityByDate.Id,
                            CreationTime = entityByDate.CreationTime.Value,
                            CreatorId = entityByDate.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityByDate.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityByDate.Id }, "");

                        var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                    }

                    else if (createdEntity.InputOutputCode == 0)
                    {
                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                        {
                            Amount = entityByDate.Amount + line.Quantity,
                            Date_ = entityByDate.Date_,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityByDate.TotalConsumption,
                            TotalWarehouseInput = entityByDate.TotalWarehouseInput + line.Quantity,
                            TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                            TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                            TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                            TotalProduction = entityByDate.TotalProduction,
                            TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                            TotalSalesOrder = entityByDate.TotalSalesOrder,
                            TotalSalesProposition = entityByDate.TotalSalesProposition,
                            TotalWastage = entityByDate.TotalWastage,
                            WarehouseID = entityByDate.WarehouseID,
                            BranchID = entityByDate.BranchID,
                            Id = entityByDate.Id,
                            CreationTime = entityByDate.CreationTime.Value,
                            CreatorId = entityByDate.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityByDate.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityByDate.Id }, "");

                        var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                    }

                }

                #endregion

                #region Grand Total Stock Movement


                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = createdEntity.BranchID, WarehouseID = createdEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id == Guid.Empty)
                {
                    if (createdEntity.InputOutputCode == 1)
                    {
                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                        {
                            Amount = line.Quantity * (-1),
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = 0,
                            TotalGoodsIssue = 0,
                            TotalGoodsReceipt = 0,
                            TotalWarehouseOutput = line.Quantity,
                            TotalWarehouseInput = 0,
                            TotalProduction = 0,
                            TotalPurchaseOrder = 0,
                            TotalPurchaseRequest = 0,
                            TotalSalesOrder = 0,
                            TotalReserved = 0,
                            TotalSalesProposition = 0,
                            TotalWastage = 0,
                            WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                            BranchID = createdEntity.BranchID.GetValueOrDefault(),
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
                        });

                        var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                    }

                    else if (createdEntity.InputOutputCode == 0)
                    {
                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                        {
                            Amount = line.Quantity,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = 0,
                            TotalGoodsIssue = 0,
                            TotalGoodsReceipt = 0,
                            TotalWarehouseOutput = 0,
                            TotalWarehouseInput = line.Quantity,
                            TotalProduction = 0,
                            TotalPurchaseOrder = 0,
                            TotalPurchaseRequest = 0,
                            TotalSalesOrder = 0,
                            TotalReserved = 0,
                            TotalSalesProposition = 0,
                            TotalWastage = 0,
                            WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                            BranchID = createdEntity.BranchID.GetValueOrDefault(),
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
                        });

                        var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                    }

                }

                else
                {
                    if (createdEntity.InputOutputCode == 1)
                    {
                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount - line.Quantity,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                            TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput + line.Quantity,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                            TotalProduction = entityGrandTotal.TotalProduction,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                            TotalReserved = entityGrandTotal.TotalReserved,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                            TotalWastage = entityGrandTotal.TotalWastage,
                            WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = now,
                            BranchID = createdEntity.BranchID.GetValueOrDefault(),
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                    }


                    else if (createdEntity.InputOutputCode == 1)
                    {
                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount + line.Quantity,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput + line.Quantity,
                            TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                            TotalProduction = entityGrandTotal.TotalProduction,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                            TotalReserved = entityGrandTotal.TotalReserved,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                            TotalWastage = entityGrandTotal.TotalWastage,
                            WarehouseID = createdEntity.WarehouseID.GetValueOrDefault(),
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = now,
                            BranchID = createdEntity.BranchID.GetValueOrDefault(),
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                    }

                }

                #endregion


            }

            return true;
        }

        public static bool UpdateTotalWarehouseShippings(SelectStockFichesDto previousEntity, UpdateStockFichesDto currentEntity)
        {
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            foreach (var line in currentEntity.SelectStockFicheLines)
            {

                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = currentEntity.BranchID, WarehouseID = currentEntity.WarehouseID, ProductID = line.ProductID, Date_ = currentEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id == Guid.Empty)
                {
                    if (currentEntity.InputOutputCode == 1)
                    {


                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                        {
                            Amount = line.Quantity * (-1),
                            Date_ = currentEntity.Date_,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = 0,
                            TotalGoodsIssue = 0,
                            TotalGoodsReceipt = 0,
                            TotalWarehouseInput = 0,
                            TotalWarehouseOutput = line.Quantity,
                            TotalProduction = 0,
                            TotalPurchaseOrder = 0,
                            TotalPurchaseRequest = 0,
                            TotalSalesOrder = 0,
                            TotalSalesProposition = 0,
                            TotalWastage = 0,
                            WarehouseID = currentEntity.WarehouseID.GetValueOrDefault(),
                            BranchID = currentEntity.BranchID.GetValueOrDefault(),
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
                        });

                        var byDateStockMovements = queryFactory.Insert<SelectByDateStockMovementsDto>(query, "Id", true);

                    }

                    else if (currentEntity.InputOutputCode == 1)
                    {
                        foreach (var previousline in previousEntity.SelectStockFicheLines)
                        {
                            var entityQueryByDateDecreasing = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = previousEntity.BranchID, WarehouseID = previousEntity.WarehouseID, ProductID = previousline.ProductID, Date_ = previousEntity.Date_ }, "");

                            var entityByDateDecreasing = queryFactory.Get<ByDateStockMovements>(entityQueryByDateDecreasing);

                            var decreasingAmount = line.Quantity;

                            var queryDecreasing = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                            {
                                Amount = entityByDateDecreasing.Amount - decreasingAmount,
                                Date_ = entityByDateDecreasing.Date_,
                                ProductID = previousline.ProductID.GetValueOrDefault(),
                                TotalConsumption = entityByDateDecreasing.TotalConsumption,
                                TotalWarehouseOutput = entityByDateDecreasing.TotalWarehouseOutput,
                                TotalWarehouseInput = entityByDateDecreasing.TotalWarehouseInput - decreasingAmount,
                                TotalGoodsIssue = entityByDateDecreasing.TotalGoodsIssue,
                                TotalGoodsReceipt = entityByDateDecreasing.TotalGoodsReceipt,
                                TotalProduction = entityByDateDecreasing.TotalProduction,
                                TotalPurchaseOrder = entityByDateDecreasing.TotalPurchaseOrder,
                                TotalPurchaseRequest = entityByDateDecreasing.TotalPurchaseRequest,
                                TotalSalesOrder = entityByDateDecreasing.TotalSalesOrder,
                                TotalSalesProposition = entityByDateDecreasing.TotalSalesProposition,
                                TotalWastage = entityByDateDecreasing.TotalWastage,
                                WarehouseID = entityByDateDecreasing.WarehouseID,
                                BranchID = entityByDateDecreasing.BranchID,
                                Id = entityByDateDecreasing.Id,
                                CreationTime = entityByDateDecreasing.CreationTime.Value,
                                CreatorId = entityByDateDecreasing.CreatorId.Value,
                                DataOpenStatus = false,
                                DataOpenStatusUserId = Guid.Empty,
                                DeleterId = entityByDateDecreasing.DeleterId.GetValueOrDefault(),
                                DeletionTime = entityByDateDecreasing.DeletionTime.GetValueOrDefault(),
                                IsDeleted = entityByDateDecreasing.IsDeleted,
                                LastModificationTime = now,
                                LastModifierId = LoginedUserService.UserId
                            }).Where(new { Id = entityByDateDecreasing.Id }, "");

                            var byDateStockMovementsDecreasing = queryFactory.Update<SelectByDateStockMovementsDto>(queryDecreasing, "Id", true);

                            var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                            {
                                Amount = line.Quantity,
                                Date_ = currentEntity.Date_,
                                ProductID = line.ProductID.GetValueOrDefault(),
                                TotalConsumption = 0,
                                TotalGoodsIssue = 0,
                                TotalGoodsReceipt = 0,
                                TotalWarehouseInput = line.Quantity,
                                TotalWarehouseOutput = 0,
                                TotalProduction = 0,
                                TotalPurchaseOrder = 0,
                                TotalPurchaseRequest = 0,
                                TotalSalesOrder = 0,
                                TotalSalesProposition = 0,
                                TotalWastage = 0,
                                WarehouseID = currentEntity.WarehouseID.GetValueOrDefault(),
                                BranchID = currentEntity.BranchID.GetValueOrDefault(),
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
                            });

                            var byDateStockMovements = queryFactory.Insert<SelectByDateStockMovementsDto>(query, "Id", true);
                        }
                    }


                }

                else
                {
                    decimal previousQuantity = previousEntity.SelectStockFicheLines.Where(t => t.Id == line.Id).Select(t => t.Quantity).First();

                    if (currentEntity.InputOutputCode == 1)
                    {
                        if (line.Quantity > previousQuantity)
                        {
                            decimal addedPR = line.Quantity - previousQuantity;

                            var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                            {
                                Amount = entityByDate.Amount - addedPR,
                                Date_ = entityByDate.Date_,
                                ProductID = line.ProductID.GetValueOrDefault(),
                                TotalConsumption = entityByDate.TotalConsumption,
                                TotalWarehouseOutput = entityByDate.TotalWarehouseOutput + addedPR,
                                TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                                TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                                TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                                TotalProduction = entityByDate.TotalProduction,
                                TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                                TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                                TotalSalesOrder = entityByDate.TotalSalesOrder,
                                TotalSalesProposition = entityByDate.TotalSalesProposition,
                                TotalWastage = entityByDate.TotalWastage,
                                WarehouseID = entityByDate.WarehouseID,
                                BranchID = entityByDate.BranchID,
                                Id = entityByDate.Id,
                                CreationTime = entityByDate.CreationTime.Value,
                                CreatorId = entityByDate.CreatorId.Value,
                                DataOpenStatus = false,
                                DataOpenStatusUserId = Guid.Empty,
                                DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                                DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                                IsDeleted = entityByDate.IsDeleted,
                                LastModificationTime = now,
                                LastModifierId = LoginedUserService.UserId
                            }).Where(new { Id = entityByDate.Id }, "");

                            var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);

                        }

                        else if (line.Quantity < previousQuantity)
                        {
                            decimal decreasedPR = previousQuantity - line.Quantity;

                            var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                            {
                                Amount = entityByDate.Amount + decreasedPR,
                                Date_ = entityByDate.Date_,
                                ProductID = line.ProductID.GetValueOrDefault(),
                                TotalConsumption = entityByDate.TotalConsumption,
                                TotalWarehouseOutput = entityByDate.TotalWarehouseOutput - decreasedPR,
                                TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                                TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                                TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                                TotalProduction = entityByDate.TotalProduction,
                                TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                                TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                                TotalSalesOrder = entityByDate.TotalSalesOrder,
                                TotalSalesProposition = entityByDate.TotalSalesProposition,
                                TotalWastage = entityByDate.TotalWastage,
                                WarehouseID = entityByDate.WarehouseID,
                                BranchID = entityByDate.BranchID,
                                Id = entityByDate.Id,
                                CreationTime = entityByDate.CreationTime.Value,
                                CreatorId = entityByDate.CreatorId.Value,
                                DataOpenStatus = false,
                                DataOpenStatusUserId = Guid.Empty,
                                DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                                DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                                IsDeleted = entityByDate.IsDeleted,
                                LastModificationTime = now,
                                LastModifierId = LoginedUserService.UserId
                            }).Where(new { Id = entityByDate.Id }, "");

                            var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                        }
                    }

                    else if (currentEntity.InputOutputCode == 0)
                    {
                        if (line.Quantity > previousQuantity)
                        {
                            decimal addedPR = line.Quantity - previousQuantity;

                            var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                            {
                                Amount = entityByDate.Amount + addedPR,
                                Date_ = entityByDate.Date_,
                                ProductID = line.ProductID.GetValueOrDefault(),
                                TotalConsumption = entityByDate.TotalConsumption,
                                TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                                TotalWarehouseInput = entityByDate.TotalWarehouseInput + addedPR,
                                TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                                TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                                TotalProduction = entityByDate.TotalProduction,
                                TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                                TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                                TotalSalesOrder = entityByDate.TotalSalesOrder,
                                TotalSalesProposition = entityByDate.TotalSalesProposition,
                                TotalWastage = entityByDate.TotalWastage,
                                WarehouseID = entityByDate.WarehouseID,
                                BranchID = entityByDate.BranchID,
                                Id = entityByDate.Id,
                                CreationTime = entityByDate.CreationTime.Value,
                                CreatorId = entityByDate.CreatorId.Value,
                                DataOpenStatus = false,
                                DataOpenStatusUserId = Guid.Empty,
                                DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                                DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                                IsDeleted = entityByDate.IsDeleted,
                                LastModificationTime = now,
                                LastModifierId = LoginedUserService.UserId
                            }).Where(new { Id = entityByDate.Id }, "");

                            var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);

                        }

                        else if (line.Quantity < previousQuantity)
                        {
                            decimal decreasedPR = previousQuantity - line.Quantity;

                            var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                            {
                                Amount = entityByDate.Amount - decreasedPR,
                                Date_ = entityByDate.Date_,
                                ProductID = line.ProductID.GetValueOrDefault(),
                                TotalConsumption = entityByDate.TotalConsumption,
                                TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                                TotalWarehouseInput = entityByDate.TotalWarehouseInput - decreasedPR,
                                TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                                TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                                TotalProduction = entityByDate.TotalProduction,
                                TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                                TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                                TotalSalesOrder = entityByDate.TotalSalesOrder,
                                TotalSalesProposition = entityByDate.TotalSalesProposition,
                                TotalWastage = entityByDate.TotalWastage,
                                WarehouseID = entityByDate.WarehouseID,
                                BranchID = entityByDate.BranchID,
                                Id = entityByDate.Id,
                                CreationTime = entityByDate.CreationTime.Value,
                                CreatorId = entityByDate.CreatorId.Value,
                                DataOpenStatus = false,
                                DataOpenStatusUserId = Guid.Empty,
                                DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                                DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                                IsDeleted = entityByDate.IsDeleted,
                                LastModificationTime = now,
                                LastModifierId = LoginedUserService.UserId
                            }).Where(new { Id = entityByDate.Id }, "");

                            var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                        }
                    }



                }

                #endregion

                #region Grand Total Stock Movement

                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = currentEntity.BranchID, WarehouseID = currentEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id == Guid.Empty)
                {
                    if (currentEntity.InputOutputCode == 1)
                    {


                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                        {
                            Amount = line.Quantity * (-1),
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = 0,
                            TotalGoodsIssue = 0,
                            TotalWarehouseOutput = line.Quantity,
                            TotalWarehouseInput = 0,
                            TotalGoodsReceipt = 0,
                            TotalProduction = 0,
                            TotalPurchaseOrder = 0,
                            TotalPurchaseRequest = 0,
                            TotalSalesOrder = 0,
                            TotalReserved = 0,
                            TotalSalesProposition = 0,
                            TotalWastage = 0,
                            WarehouseID = currentEntity.WarehouseID.GetValueOrDefault(),
                            BranchID = currentEntity.BranchID.GetValueOrDefault(),
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
                        });

                        var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);

                    }

                    else if (currentEntity.InputOutputCode == 0)
                    {
                        foreach (var previousLine in previousEntity.SelectStockFicheLines)
                        {
                            var entityQueryGrandTotalDecreasing = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = previousEntity.BranchID, WarehouseID = previousEntity.WarehouseID, ProductID = previousLine.ProductID }, "");

                            var entityGrandTotalDecreasing = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotalDecreasing);

                            var decreasingAmount = line.Quantity;

                            var queryDecreasing = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                            {
                                Amount = entityGrandTotalDecreasing.Amount - decreasingAmount,
                                ProductID = previousLine.ProductID.GetValueOrDefault(),
                                TotalConsumption = entityGrandTotalDecreasing.TotalConsumption,
                                TotalWarehouseInput = entityGrandTotalDecreasing.TotalWarehouseInput - decreasingAmount,
                                TotalWarehouseOutput = entityGrandTotalDecreasing.TotalWarehouseOutput,
                                TotalGoodsIssue = entityGrandTotalDecreasing.TotalGoodsIssue,
                                TotalGoodsReceipt = entityGrandTotalDecreasing.TotalGoodsReceipt,
                                TotalProduction = entityGrandTotalDecreasing.TotalProduction,
                                TotalPurchaseOrder = entityGrandTotalDecreasing.TotalPurchaseOrder,
                                TotalPurchaseRequest = entityGrandTotalDecreasing.TotalPurchaseRequest,
                                TotalSalesOrder = entityGrandTotalDecreasing.TotalSalesOrder,
                                TotalReserved = entityGrandTotalDecreasing.TotalReserved,
                                TotalSalesProposition = entityGrandTotalDecreasing.TotalSalesProposition,
                                TotalWastage = entityGrandTotalDecreasing.TotalWastage,
                                WarehouseID = previousEntity.WarehouseID,
                                Id = entityGrandTotalDecreasing.Id,
                                CreationTime = entityGrandTotalDecreasing.CreationTime.Value,
                                CreatorId = entityGrandTotalDecreasing.CreatorId.Value,
                                DataOpenStatus = false,
                                DataOpenStatusUserId = Guid.Empty,
                                DeleterId = entityGrandTotalDecreasing.DeleterId.GetValueOrDefault(),
                                DeletionTime = entityGrandTotalDecreasing.DeletionTime.GetValueOrDefault(),
                                IsDeleted = entityGrandTotalDecreasing.IsDeleted,
                                LastModificationTime = now,
                                BranchID = previousEntity.BranchID,
                                LastModifierId = LoginedUserService.UserId
                            }).Where(new { Id = entityGrandTotalDecreasing.Id }, "");

                            var grandTotalStockMovementsDecreasing = queryFactory.Update<SelectGrandTotalStockMovementsDto>(queryDecreasing, "Id", true);

                            var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                            {
                                Amount = line.Quantity,
                                ProductID = line.ProductID.GetValueOrDefault(),
                                TotalConsumption = 0,
                                TotalGoodsIssue = 0,
                                TotalWarehouseOutput = 0,
                                TotalWarehouseInput = line.Quantity,
                                TotalGoodsReceipt = 0,
                                TotalProduction = 0,
                                TotalPurchaseOrder = 0,
                                TotalPurchaseRequest = 0,
                                TotalSalesOrder = 0,
                                TotalReserved = 0,
                                TotalSalesProposition = 0,
                                TotalWastage = 0,
                                WarehouseID = currentEntity.WarehouseID.GetValueOrDefault(),
                                BranchID = currentEntity.BranchID.GetValueOrDefault(),
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
                            });

                            var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                        }
                    }

                }

                else
                {
                    decimal previousQuantity = previousEntity.SelectStockFicheLines.Where(t => t.Id == line.Id).Select(t => t.Quantity).First();

                    if (currentEntity.InputOutputCode == 1)
                    {
                        if (line.Quantity > previousQuantity)
                        {
                            decimal addedPR = line.Quantity - previousQuantity;

                            var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                            {
                                Amount = entityGrandTotal.Amount - addedPR,
                                ProductID = line.ProductID.GetValueOrDefault(),
                                TotalConsumption = entityGrandTotal.TotalConsumption,
                                TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                                TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput + addedPR,
                                TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                                TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                                TotalProduction = entityGrandTotal.TotalProduction,
                                TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                                TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                                TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                                TotalReserved = entityGrandTotal.TotalReserved,
                                TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                                TotalWastage = entityGrandTotal.TotalWastage,
                                WarehouseID = entityGrandTotal.WarehouseID,
                                Id = entityGrandTotal.Id,
                                CreationTime = entityGrandTotal.CreationTime.Value,
                                CreatorId = entityGrandTotal.CreatorId.Value,
                                DataOpenStatus = false,
                                DataOpenStatusUserId = Guid.Empty,
                                DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                                DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                                IsDeleted = entityGrandTotal.IsDeleted,
                                LastModificationTime = now,
                                BranchID = entityGrandTotal.BranchID,
                                LastModifierId = LoginedUserService.UserId
                            }).Where(new { Id = entityGrandTotal.Id }, "");

                            var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);

                        }

                        else if (line.Quantity < previousQuantity)
                        {
                            decimal decreasedPR = previousQuantity - line.Quantity;

                            var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                            {
                                Amount = entityGrandTotal.Amount + decreasedPR,
                                ProductID = line.ProductID.GetValueOrDefault(),
                                TotalConsumption = entityGrandTotal.TotalConsumption,
                                TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                                TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput - decreasedPR,
                                TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                                TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                                TotalProduction = entityGrandTotal.TotalProduction,
                                TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                                TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                                TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                                TotalReserved = entityGrandTotal.TotalReserved,
                                TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                                TotalWastage = entityGrandTotal.TotalWastage,
                                WarehouseID = entityGrandTotal.WarehouseID,
                                Id = entityGrandTotal.Id,
                                CreationTime = entityGrandTotal.CreationTime.Value,
                                CreatorId = entityGrandTotal.CreatorId.Value,
                                DataOpenStatus = false,
                                DataOpenStatusUserId = Guid.Empty,
                                DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                                DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                                IsDeleted = entityGrandTotal.IsDeleted,
                                LastModificationTime = now,
                                BranchID = entityGrandTotal.BranchID,
                                LastModifierId = LoginedUserService.UserId
                            }).Where(new { Id = entityGrandTotal.Id }, "");

                            var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                        }
                    }

                    else if (currentEntity.InputOutputCode == 0)
                    {
                        if (line.Quantity > previousQuantity)
                        {
                            decimal addedPR = line.Quantity - previousQuantity;

                            var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                            {
                                Amount = entityGrandTotal.Amount + addedPR,
                                ProductID = line.ProductID.GetValueOrDefault(),
                                TotalConsumption = entityGrandTotal.TotalConsumption,
                                TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput + addedPR,
                                TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                                TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                                TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                                TotalProduction = entityGrandTotal.TotalProduction,
                                TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                                TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                                TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                                TotalReserved = entityGrandTotal.TotalReserved,
                                TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                                TotalWastage = entityGrandTotal.TotalWastage,
                                WarehouseID = entityGrandTotal.WarehouseID,
                                Id = entityGrandTotal.Id,
                                CreationTime = entityGrandTotal.CreationTime.Value,
                                CreatorId = entityGrandTotal.CreatorId.Value,
                                DataOpenStatus = false,
                                DataOpenStatusUserId = Guid.Empty,
                                DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                                DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                                IsDeleted = entityGrandTotal.IsDeleted,
                                LastModificationTime = now,
                                BranchID = entityGrandTotal.BranchID,
                                LastModifierId = LoginedUserService.UserId
                            }).Where(new { Id = entityGrandTotal.Id }, "");

                            var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);

                        }

                        else if (line.Quantity < previousQuantity)
                        {
                            decimal decreasedPR = previousQuantity - line.Quantity;

                            var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                            {
                                Amount = entityGrandTotal.Amount - decreasedPR,
                                ProductID = line.ProductID.GetValueOrDefault(),
                                TotalConsumption = entityGrandTotal.TotalConsumption,
                                TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput - decreasedPR,
                                TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                                TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                                TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                                TotalProduction = entityGrandTotal.TotalProduction,
                                TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                                TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                                TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                                TotalReserved = entityGrandTotal.TotalReserved,
                                TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                                TotalWastage = entityGrandTotal.TotalWastage,
                                WarehouseID = entityGrandTotal.WarehouseID,
                                Id = entityGrandTotal.Id,
                                CreationTime = entityGrandTotal.CreationTime.Value,
                                CreatorId = entityGrandTotal.CreatorId.Value,
                                DataOpenStatus = false,
                                DataOpenStatusUserId = Guid.Empty,
                                DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                                DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                                IsDeleted = entityGrandTotal.IsDeleted,
                                LastModificationTime = now,
                                BranchID = entityGrandTotal.BranchID,
                                LastModifierId = LoginedUserService.UserId
                            }).Where(new { Id = entityGrandTotal.Id }, "");

                            var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                        }
                    }

                    #endregion
                }

            }

            return true;
        }

        public static bool DeleteTotalWarehouseShippingLines(SelectStockFicheLinesDto deletedLine)
        {
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            #region Getting Total Warehouse Shipping

            var query = queryFactory
                   .Query()
                   .From(Tables.StockFiches)
                   .Select<StockFiches>(null)
                   .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchID = b.Id },
                        nameof(StockFiches.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                   .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code, WarehouseID = w.Id },
                        nameof(StockFiches.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = deletedLine.StockFicheID }, Tables.StockFiches);

            var stockFiches = queryFactory.Get<SelectStockFichesDto>(query);

            #endregion

            #region By Date Stock Movement

            var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = stockFiches.BranchID, WarehouseID = stockFiches.WarehouseID, ProductID = deletedLine.ProductID, Date_ = stockFiches.Date_ }, "");

            var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

            if (stockFiches.InputOutputCode == 1)
            {
                var queryDecreasingDate = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                {
                    Amount = entityByDate.Amount + deletedLine.Quantity,
                    Date_ = entityByDate.Date_,
                    ProductID = entityByDate.ProductID,
                    TotalConsumption = entityByDate.TotalConsumption,
                    TotalWarehouseOutput = entityByDate.TotalWarehouseOutput - deletedLine.Quantity,
                    TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                    TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                    TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                    TotalProduction = entityByDate.TotalProduction,
                    TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                    TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                    TotalSalesOrder = entityByDate.TotalSalesOrder,
                    TotalSalesProposition = entityByDate.TotalSalesProposition,
                    TotalWastage = entityByDate.TotalWastage,
                    WarehouseID = entityByDate.WarehouseID,
                    BranchID = entityByDate.BranchID,
                    Id = entityByDate.Id,
                    CreationTime = entityByDate.CreationTime.Value,
                    CreatorId = entityByDate.CreatorId.Value,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entityByDate.DeleterId.Value,
                    DeletionTime = entityByDate.DeletionTime.Value,
                    IsDeleted = entityByDate.IsDeleted,
                    LastModificationTime = now,
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = entityByDate.Id }, "");

                var byDateStockMovementsDecreasing = queryFactory.Update<SelectByDateStockMovementsDto>(queryDecreasingDate, "Id", true);
            }

            else if (stockFiches.InputOutputCode == 0)
            {
                var queryDecreasingDate = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                {
                    Amount = entityByDate.Amount - deletedLine.Quantity,
                    Date_ = entityByDate.Date_,
                    ProductID = entityByDate.ProductID,
                    TotalConsumption = entityByDate.TotalConsumption,
                    TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                    TotalWarehouseInput = entityByDate.TotalWarehouseInput - deletedLine.Quantity,
                    TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                    TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                    TotalProduction = entityByDate.TotalProduction,
                    TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                    TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                    TotalSalesOrder = entityByDate.TotalSalesOrder,
                    TotalSalesProposition = entityByDate.TotalSalesProposition,
                    TotalWastage = entityByDate.TotalWastage,
                    WarehouseID = entityByDate.WarehouseID,
                    BranchID = entityByDate.BranchID,
                    Id = entityByDate.Id,
                    CreationTime = entityByDate.CreationTime.Value,
                    CreatorId = entityByDate.CreatorId.Value,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                    DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                    IsDeleted = entityByDate.IsDeleted,
                    LastModificationTime = now,
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = entityByDate.Id }, "");

                var byDateStockMovementsDecreasing = queryFactory.Update<SelectByDateStockMovementsDto>(queryDecreasingDate, "Id", true);
            }

            #endregion

            #region Grand Total Stock Movement

            var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = stockFiches.BranchID, WarehouseID = stockFiches.WarehouseID, ProductID = deletedLine.ProductID }, "");

            var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

            if (stockFiches.InputOutputCode == 1)
            {
                var queryDecreasingGrand = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                {
                    Amount = entityGrandTotal.Amount + deletedLine.Quantity,
                    ProductID = deletedLine.ProductID.GetValueOrDefault(),
                    TotalConsumption = entityGrandTotal.TotalConsumption,
                    TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                    TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput - deletedLine.Quantity,
                    TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                    TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                    TotalProduction = entityGrandTotal.TotalProduction,
                    TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                    TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                    TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                    TotalReserved = entityGrandTotal.TotalReserved,
                    TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                    TotalWastage = entityGrandTotal.TotalWastage,
                    WarehouseID = entityGrandTotal.WarehouseID,
                    Id = entityGrandTotal.Id,
                    CreationTime = entityGrandTotal.CreationTime.Value,
                    CreatorId = entityGrandTotal.CreatorId.Value,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                    DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                    IsDeleted = entityGrandTotal.IsDeleted,
                    LastModificationTime = now,
                    BranchID = entityGrandTotal.BranchID,
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = entityGrandTotal.Id }, "");

                var grandTotalStockMovementsDecreasing = queryFactory.Update<SelectGrandTotalStockMovementsDto>(queryDecreasingGrand, "Id", true);
            }

            else if (stockFiches.InputOutputCode == 0)
            {
                var queryDecreasingGrand = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                {
                    Amount = entityGrandTotal.Amount - deletedLine.Quantity,
                    ProductID = deletedLine.ProductID.GetValueOrDefault(),
                    TotalConsumption = entityGrandTotal.TotalConsumption,
                    TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput - deletedLine.Quantity,
                    TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                    TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                    TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                    TotalProduction = entityGrandTotal.TotalProduction,
                    TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                    TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                    TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                    TotalReserved = entityGrandTotal.TotalReserved,
                    TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                    TotalWastage = entityGrandTotal.TotalWastage,
                    WarehouseID = entityGrandTotal.WarehouseID,
                    Id = entityGrandTotal.Id,
                    CreationTime = entityGrandTotal.CreationTime.Value,
                    CreatorId = entityGrandTotal.CreatorId.Value,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                    DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                    IsDeleted = entityGrandTotal.IsDeleted,
                    LastModificationTime = now,
                    BranchID = entityGrandTotal.BranchID,
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = entityGrandTotal.Id }, "");

                var grandTotalStockMovementsDecreasing = queryFactory.Update<SelectGrandTotalStockMovementsDto>(queryDecreasingGrand, "Id", true);
                #endregion
            }
            return true;
        }

        public static bool DeleteTotalWarehouseShippings(SelectStockFichesDto deletedEntity)
        {
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            #region Getting Lines

            var queryLines = queryFactory
                    .Query()
                    .From(Tables.StockFicheLines)
                    .Select<StockFicheLines>(null)
                    .Join<Products>
                     (
                         p => new { ProductCode = p.Code, ProductName = p.Name },
                         nameof(StockFicheLines.ProductID),
                         nameof(Products.Id),
                         JoinType.Left
                     )
                    .Join<UnitSets>
                     (
                         u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                         nameof(StockFicheLines.UnitSetID),
                         nameof(UnitSets.Id),
                         JoinType.Left
                     )
                     .Where(new { StockFicheID = deletedEntity.Id }, Tables.StockFicheLines);

            var stockFicheLine = queryFactory.GetList<SelectStockFicheLinesDto>(queryLines).ToList();

            deletedEntity.SelectStockFicheLines = stockFicheLine;

            #endregion

            foreach (var line in deletedEntity.SelectStockFicheLines)
            {

                #region By Date Stock Movement

                var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = deletedEntity.BranchID, WarehouseID = deletedEntity.WarehouseID, ProductID = line.ProductID, Date_ = deletedEntity.Date_ }, "");

                var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                if (entityByDate.Id != Guid.Empty)
                {
                    if (deletedEntity.InputOutputCode == 1)
                    {
                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                        {
                            Amount = entityByDate.Amount + line.Quantity,
                            Date_ = entityByDate.Date_,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityByDate.TotalConsumption,
                            TotalWarehouseOutput = entityByDate.TotalWarehouseOutput - line.Quantity,
                            TotalWarehouseInput = entityByDate.TotalWarehouseInput,
                            TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                            TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                            TotalProduction = entityByDate.TotalProduction,
                            TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                            TotalSalesOrder = entityByDate.TotalSalesOrder,
                            TotalSalesProposition = entityByDate.TotalSalesProposition,
                            TotalWastage = entityByDate.TotalWastage,
                            WarehouseID = entityByDate.WarehouseID,
                            BranchID = entityByDate.BranchID,
                            Id = entityByDate.Id,
                            CreationTime = entityByDate.CreationTime.Value,
                            CreatorId = entityByDate.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityByDate.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityByDate.Id }, "");

                        var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                    }

                    else if (deletedEntity.InputOutputCode == 0)
                    {
                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                        {
                            Amount = entityByDate.Amount - line.Quantity,
                            Date_ = entityByDate.Date_,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityByDate.TotalConsumption,
                            TotalWarehouseOutput = entityByDate.TotalWarehouseOutput,
                            TotalWarehouseInput = entityByDate.TotalWarehouseInput - line.Quantity,
                            TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                            TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                            TotalProduction = entityByDate.TotalProduction,
                            TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityByDate.TotalPurchaseRequest,
                            TotalSalesOrder = entityByDate.TotalSalesOrder,
                            TotalSalesProposition = entityByDate.TotalSalesProposition,
                            TotalWastage = entityByDate.TotalWastage,
                            WarehouseID = entityByDate.WarehouseID,
                            BranchID = entityByDate.BranchID,
                            Id = entityByDate.Id,
                            CreationTime = entityByDate.CreationTime.Value,
                            CreatorId = entityByDate.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityByDate.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityByDate.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityByDate.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityByDate.Id }, "");

                        var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                    }

                }

                #endregion

                #region Grand Total Stock Movement

                var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = deletedEntity.BranchID, WarehouseID = deletedEntity.WarehouseID, ProductID = line.ProductID }, "");

                var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                if (entityGrandTotal.Id != Guid.Empty)
                {
                    if (deletedEntity.InputOutputCode == 1)
                    {
                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount + line.Quantity,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput,
                            TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput - line.Quantity,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                            TotalProduction = entityGrandTotal.TotalProduction,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                            TotalReserved = entityGrandTotal.TotalReserved,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                            TotalWastage = entityGrandTotal.TotalWastage,
                            WarehouseID = entityGrandTotal.WarehouseID,
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = now,
                            BranchID = entityGrandTotal.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                    }

                    else if (deletedEntity.InputOutputCode == 0)
                    {
                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount - line.Quantity,
                            ProductID = line.ProductID,
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalWarehouseInput = entityGrandTotal.TotalWarehouseInput - line.Quantity,
                            TotalWarehouseOutput = entityGrandTotal.TotalWarehouseOutput,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                            TotalProduction = entityGrandTotal.TotalProduction,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                            TotalReserved = entityGrandTotal.TotalReserved,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                            TotalWastage = entityGrandTotal.TotalWastage,
                            WarehouseID = entityGrandTotal.WarehouseID,
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.GetValueOrDefault(),
                            DeletionTime = entityGrandTotal.DeletionTime.GetValueOrDefault(),
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = now,
                            BranchID = entityGrandTotal.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                    }

                }

                #endregion
            }

            return true;
        }


        #endregion

    }
}
