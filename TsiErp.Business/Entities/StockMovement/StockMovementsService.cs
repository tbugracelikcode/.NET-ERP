using Tsi.Core.Utilities.Guids;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.Other.ByDateStockMovement;
using TsiErp.Entities.Entities.Other.ByDateStockMovement.Dtos;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequest;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequest.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequestLine;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequestLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.TableConstant;

namespace TsiErp.Business.Entities.StockMovement
{
    public static class StockMovementsService
    {
        static QueryFactory queryFactory { get; set; } = new QueryFactory();

        public static IGuidGenerator GuidGenerator { get; set; } = new SequentialGuidGenerator();

        public static bool TotalPurchaseRequests(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                #region Get Purchase Request

                var queryPR = queryFactory
                      .Query()
                      .From(Tables.PurchaseRequests)
                      .Select<PurchaseRequests>(pr => new { pr.WarehouseID, pr.ValidityDate_, pr.TotalVatExcludedAmount, pr.TotalVatAmount, pr.TotalDiscountAmount, pr.Time_, pr.SpecialCode, pr.RevisionTime, pr.RevisionDate, pr.PurchaseRequestState, pr.PropositionRevisionNo, pr.ProductionOrderID, pr.PaymentPlanID, pr.NetAmount, pr.LinkedPurchaseRequestID, pr.Id, pr.GrossAmount, pr.FicheNo, pr.ExchangeRate, pr.Description_, pr.Date_, pr.DataOpenStatusUserId, pr.DataOpenStatus, pr.CurrentAccountCardID, pr.CurrencyID, pr.BranchID })
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
                      
                       .Where(new { Id = id }, false, false, Tables.PurchaseRequests);

                var purchaseRequests = queryFactory.Get<SelectPurchaseRequestsDto>(queryPR);

                var queryPRLines = queryFactory
                       .Query()
                       .From(Tables.PurchaseRequestLines)
                       .Select<PurchaseRequestLines>(prl => new { prl.VATrate, prl.VATamount, prl.UnitSetID, prl.UnitPrice, prl.Quantity, prl.PurchaseRequestLineState, prl.PurchaseRequestID, prl.ProductionOrderID, prl.ProductID, prl.PaymentPlanID, prl.OrderConversionDate, prl.LineTotalAmount, prl.LineNr, prl.LineDescription, prl.LineAmount, prl.Id, prl.ExchangeRate, prl.DiscountRate, prl.DiscountAmount, prl.DataOpenStatusUserId, prl.DataOpenStatus })
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
                        .Where(new { PurchaseRequestID = id }, false, false, Tables.PurchaseRequestLines);

                var purchaseRequestLine = queryFactory.GetList<SelectPurchaseRequestLinesDto>(queryPRLines).ToList();

                purchaseRequests.SelectPurchaseRequestLines = purchaseRequestLine;

                #endregion

                foreach(var line in purchaseRequestLine)
                {

                    #region By Date Stock Movement

                    var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = purchaseRequests.BranchID, WarehouseID = purchaseRequests.WarehouseID, ProductID = line.ProductID, Date_ = purchaseRequests.Date_ }, false, false, "");

                    var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                    #region Date Control

                    var listQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = purchaseRequests.BranchID, WarehouseID = purchaseRequests.WarehouseID, ProductID = line.ProductID }, false, false, "");

                    var listByDate = queryFactory.GetList<ByDateStockMovements>(listQueryByDate).ToList();

                    if(listByDate.Count > 1)
                    {
                        decimal deletedAmount = listByDate.Where(t => t.Date_ != purchaseRequests.Date_ && t.Amount > 0).Select(t => t.Amount).FirstOrDefault();

                        ByDateStockMovements deletedAmountEntity = listByDate.Where(t => t.Date_ != purchaseRequests.Date_ && t.Amount > 0).FirstOrDefault();

                        if(  deletedAmount != 0)
                        {
                            var queryDeleted = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                            {
                                Amount = 0,
                                Date_ = purchaseRequests.Date_,
                                ProductID = line.ProductID,
                                TotalConsumption = deletedAmountEntity.TotalConsumption,
                                TotalGoodsIssue = deletedAmountEntity.TotalGoodsIssue,
                                TotalGoodsReceipt = deletedAmountEntity.TotalGoodsReceipt,
                                TotalProduction = deletedAmountEntity.TotalProduction,
                                TotalPurchaseOrder = deletedAmountEntity.TotalPurchaseOrder,
                                TotalPurchaseRequest = deletedAmountEntity.TotalPurchaseRequest + 1,
                                TotalSalesOrder = deletedAmountEntity.TotalSalesOrder,
                                TotalSalesProposition = deletedAmountEntity.TotalSalesProposition,
                                TotalWastage = deletedAmountEntity.TotalWastage,
                                WarehouseID = purchaseRequests.WarehouseID,
                                BranchID = purchaseRequests.BranchID,
                                Id = deletedAmountEntity.Id,
                                CreationTime = deletedAmountEntity.CreationTime.Value,
                                CreatorId = deletedAmountEntity.CreatorId.Value,
                                DataOpenStatus = false,
                                DataOpenStatusUserId = Guid.Empty,
                                DeleterId = deletedAmountEntity.DeleterId.Value,
                                DeletionTime = deletedAmountEntity.DeletionTime.Value,
                                IsDeleted = deletedAmountEntity.IsDeleted,
                                LastModificationTime = DateTime.Now,
                                LastModifierId = LoginedUserService.UserId
                            }).Where(new { Id = deletedAmountEntity.Id }, false, false, "");

                            var byDateStockMovementsDeleted = queryFactory.Update<SelectByDateStockMovementsDto>(queryDeleted, "Id", true);

                            if (entityByDate != null)
                            {
                                var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                                {
                                    Amount = entityByDate.Amount + deletedAmount + 1,
                                    Date_ = purchaseRequests.Date_,
                                    ProductID = line.ProductID,
                                    TotalConsumption = entityByDate.TotalConsumption,
                                    TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                                    TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                                    TotalProduction = entityByDate.TotalProduction,
                                    TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                                    TotalPurchaseRequest = entityByDate.TotalPurchaseRequest + 1,
                                    TotalSalesOrder = entityByDate.TotalSalesOrder,
                                    TotalSalesProposition = entityByDate.TotalSalesProposition,
                                    TotalWastage = entityByDate.TotalWastage,
                                    WarehouseID = purchaseRequests.WarehouseID,
                                    BranchID = purchaseRequests.BranchID,
                                    Id = entityByDate.Id,
                                    CreationTime = entityByDate.CreationTime.Value,
                                    CreatorId = entityByDate.CreatorId.Value,
                                    DataOpenStatus = false,
                                    DataOpenStatusUserId = Guid.Empty,
                                    DeleterId = entityByDate.DeleterId.Value,
                                    DeletionTime = entityByDate.DeletionTime.Value,
                                    IsDeleted = entityByDate.IsDeleted,
                                    LastModificationTime = DateTime.Now,
                                    LastModifierId = LoginedUserService.UserId
                                }).Where(new { Id = entityByDate.Id }, false, false, "");

                                var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                            }

                            else
                            {
                                var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                                {
                                    Amount = deletedAmount + 1,
                                    Date_ = purchaseRequests.Date_,
                                    ProductID = line.ProductID,
                                    TotalConsumption = 0,
                                    TotalGoodsIssue = 0,
                                    TotalGoodsReceipt = 0,
                                    TotalProduction = 0,
                                    TotalPurchaseOrder = 0,
                                    TotalPurchaseRequest = 1,
                                    TotalSalesOrder = 0,
                                    TotalSalesProposition = 0,
                                    TotalWastage = 0,
                                    WarehouseID = purchaseRequests.WarehouseID,
                                    BranchID = purchaseRequests.BranchID,
                                    CreationTime = DateTime.Now,
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

                        else if (deletedAmount == 0)
                        {
                            if (entityByDate != null)
                            {
                                var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                                {
                                    Amount = entityByDate.Amount + 1,
                                    Date_ = purchaseRequests.Date_,
                                    ProductID = line.ProductID,
                                    TotalConsumption = entityByDate.TotalConsumption,
                                    TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                                    TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                                    TotalProduction = entityByDate.TotalProduction,
                                    TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                                    TotalPurchaseRequest = entityByDate.TotalPurchaseRequest + 1,
                                    TotalSalesOrder = entityByDate.TotalSalesOrder,
                                    TotalSalesProposition = entityByDate.TotalSalesProposition,
                                    TotalWastage = entityByDate.TotalWastage,
                                    WarehouseID = purchaseRequests.WarehouseID,
                                    BranchID = purchaseRequests.BranchID,
                                    Id = entityByDate.Id,
                                    CreationTime = entityByDate.CreationTime.Value,
                                    CreatorId = entityByDate.CreatorId.Value,
                                    DataOpenStatus = false,
                                    DataOpenStatusUserId = Guid.Empty,
                                    DeleterId = entityByDate.DeleterId.Value,
                                    DeletionTime = entityByDate.DeletionTime.Value,
                                    IsDeleted = entityByDate.IsDeleted,
                                    LastModificationTime = DateTime.Now,
                                    LastModifierId = LoginedUserService.UserId
                                }).Where(new { Id = entityByDate.Id }, false, false, "");

                                var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                            }

                            else
                            {
                                var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                                {
                                    Amount = 1,
                                    Date_ = purchaseRequests.Date_,
                                    ProductID = line.ProductID,
                                    TotalConsumption = 0,
                                    TotalGoodsIssue = 0,
                                    TotalGoodsReceipt = 0,
                                    TotalProduction = 0,
                                    TotalPurchaseOrder = 0,
                                    TotalPurchaseRequest = 1,
                                    TotalSalesOrder = 0,
                                    TotalSalesProposition = 0,
                                    TotalWastage = 0,
                                    WarehouseID = purchaseRequests.WarehouseID,
                                    BranchID = purchaseRequests.BranchID,
                                    CreationTime = DateTime.Now,
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

                    else if(listByDate.Count <=1)
                    {
                        if (entityByDate != null)
                        {
                            var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                            {
                                Amount = entityByDate.Amount + 1,
                                Date_ = purchaseRequests.Date_,
                                ProductID = line.ProductID,
                                TotalConsumption = entityByDate.TotalConsumption,
                                TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                                TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                                TotalProduction = entityByDate.TotalProduction,
                                TotalPurchaseOrder = entityByDate.TotalPurchaseOrder,
                                TotalPurchaseRequest = entityByDate.TotalPurchaseRequest + 1,
                                TotalSalesOrder = entityByDate.TotalSalesOrder,
                                TotalSalesProposition = entityByDate.TotalSalesProposition,
                                TotalWastage = entityByDate.TotalWastage,
                                WarehouseID = purchaseRequests.WarehouseID,
                                BranchID = purchaseRequests.BranchID,
                                Id = entityByDate.Id,
                                CreationTime = entityByDate.CreationTime.Value,
                                CreatorId = entityByDate.CreatorId.Value,
                                DataOpenStatus = false,
                                DataOpenStatusUserId = Guid.Empty,
                                DeleterId = entityByDate.DeleterId.Value,
                                DeletionTime = entityByDate.DeletionTime.Value,
                                IsDeleted = entityByDate.IsDeleted,
                                LastModificationTime = DateTime.Now,
                                LastModifierId = LoginedUserService.UserId
                            }).Where(new { Id = entityByDate.Id }, false, false, "");

                            var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                        }

                        else
                        {
                            var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                            {
                                Amount = 1,
                                Date_ = purchaseRequests.Date_,
                                ProductID = line.ProductID,
                                TotalConsumption = 0,
                                TotalGoodsIssue = 0,
                                TotalGoodsReceipt = 0,
                                TotalProduction = 0,
                                TotalPurchaseOrder = 0,
                                TotalPurchaseRequest = 1,
                                TotalSalesOrder = 0,
                                TotalSalesProposition = 0,
                                TotalWastage = 0,
                                WarehouseID = purchaseRequests.WarehouseID,
                                BranchID = purchaseRequests.BranchID,
                                CreationTime = DateTime.Now,
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

                    #endregion

                    #endregion

                    #region Grand Total Stock Movement

                    var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = purchaseRequests.BranchID, WarehouseID = purchaseRequests.WarehouseID, ProductID = line.ProductID }, false, false, "");

                    var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                    if(entityGrandTotal != null)
                    {
                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount +1,
                            ProductID = line.ProductID,
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                            TotalProduction = entityGrandTotal.TotalProduction,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest +1,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                            TotalReserved = entityGrandTotal.TotalReserved,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                            TotalWastage = entityGrandTotal.TotalWastage,
                            WarehouseID = purchaseRequests.WarehouseID,
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.Value,
                            DeletionTime = entityGrandTotal.DeletionTime.Value,
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = DateTime.Now,
                            BranchID = purchaseRequests.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, false, false, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                    }

                    else
                    {
                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                        {
                            Amount = 1,
                            ProductID = line.ProductID,
                            TotalConsumption = 0,
                            TotalGoodsIssue = 0,
                            TotalGoodsReceipt = 0,
                            TotalProduction = 0,
                            TotalPurchaseOrder = 0,
                            TotalPurchaseRequest = 1,
                            TotalSalesOrder = 0,
                            TotalReserved = 0,
                            TotalSalesProposition = 0,
                            TotalWastage = 0,
                            WarehouseID = purchaseRequests.WarehouseID,
                            BranchID = purchaseRequests.BranchID,
                            CreationTime = DateTime.Now,
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

                    #endregion


                }

                return true;
            }
        }

        public static bool TotalPurchaseOrders()
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                #region By Date Stock Movement



                #endregion

                #region Grand Total Stock Movement



                #endregion

                return true;
            }
        }

        public static bool TotalSalesPropositions()
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                #region By Date Stock Movement



                #endregion

                #region Grand Total Stock Movement



                #endregion

                return true;
            }
        }

        public static bool TotalSalesOrders()
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                #region By Date Stock Movement



                #endregion

                #region Grand Total Stock Movement



                #endregion

                return true;
            }
        }

        public static bool TotalConsumptions()
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                #region By Date Stock Movement



                #endregion

                #region Grand Total Stock Movement



                #endregion

                return true;
            }
        }

        public static bool TotalWastages()
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                #region By Date Stock Movement



                #endregion

                #region Grand Total Stock Movement



                #endregion

                return true;
            }
        }

        public static bool TotalProductions()
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                #region By Date Stock Movement



                #endregion

                #region Grand Total Stock Movement



                #endregion

                return true;
            }
        }

        public static bool TotalGoodsFiches()
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                #region By Date Stock Movement



                #endregion

                #region Grand Total Stock Movement



                #endregion

                return true;
            }
        }

        public static bool TotalGoodsIssues()
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                #region By Date Stock Movement



                #endregion

                #region Grand Total Stock Movement



                #endregion

                return true;
            }
        }

        public static bool WarehouseShipmentFiches()
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                #region By Date Stock Movement



                #endregion

                #region Grand Total Stock Movement



                #endregion

                return true;
            }
        }
    }
}
