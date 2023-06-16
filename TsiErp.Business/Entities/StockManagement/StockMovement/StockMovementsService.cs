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

        #region Purchase Requests

        public static bool InsertPurchaseRequests(CreatePurchaseRequestsDto createdEntity)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                foreach (var line in createdEntity.SelectPurchaseRequestLines)
                {
                    #region By Date Stock Movement

                    var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = createdEntity.BranchID, WarehouseID = createdEntity.WarehouseID, ProductID = line.ProductID, Date_ = createdEntity.Date_ }, false, false, "");

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
                            TotalGoodsReceipt = 0,
                            TotalProduction = 0,
                            TotalPurchaseOrder = 0,
                            TotalPurchaseRequest = line.Quantity,
                            TotalSalesOrder = 0,
                            TotalSalesProposition = 0,
                            TotalWastage = 0,
                            WarehouseID = createdEntity.WarehouseID,
                            BranchID = createdEntity.BranchID,
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
                    else
                    {
                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                        {
                            Amount = entityByDate.Amount,
                            Date_ = entityByDate.Date_,
                            ProductID = line.ProductID,
                            TotalConsumption = entityByDate.TotalConsumption,
                            TotalGoodsIssue = entityByDate.TotalGoodsIssue,
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
                            LastModificationTime = DateTime.Now,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityByDate.Id }, false, false, "");

                        var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                    }

                    #endregion

                    #region Grand Total Stock Movement


                    var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = createdEntity.BranchID, WarehouseID = createdEntity.WarehouseID, ProductID = line.ProductID }, false, false, "");

                    var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                    if (entityGrandTotal.Id == Guid.Empty)
                    {
                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                        {
                            Amount = 0,
                            ProductID = line.ProductID,
                            TotalConsumption = 0,
                            TotalGoodsIssue = 0,
                            TotalGoodsReceipt = 0,
                            TotalProduction = 0,
                            TotalPurchaseOrder = 0,
                            TotalPurchaseRequest = line.Quantity,
                            TotalSalesOrder = 0,
                            TotalReserved = 0,
                            TotalSalesProposition = 0,
                            TotalWastage = 0,
                            WarehouseID = createdEntity.WarehouseID,
                            BranchID = createdEntity.BranchID,
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

                    else
                    {
                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount,
                            ProductID = line.ProductID,
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                            TotalProduction = entityGrandTotal.TotalProduction,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest + line.Quantity,
                            TotalSalesOrder = entityGrandTotal.TotalSalesOrder,
                            TotalReserved = entityGrandTotal.TotalReserved,
                            TotalSalesProposition = entityGrandTotal.TotalSalesProposition,
                            TotalWastage = entityGrandTotal.TotalWastage,
                            WarehouseID = createdEntity.WarehouseID,
                            Id = entityGrandTotal.Id,
                            CreationTime = entityGrandTotal.CreationTime.Value,
                            CreatorId = entityGrandTotal.CreatorId.Value,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = entityGrandTotal.DeleterId.Value,
                            DeletionTime = entityGrandTotal.DeletionTime.Value,
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = DateTime.Now,
                            BranchID = createdEntity.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, false, false, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                    }

                    #endregion
                }

            }

            return true;
        }

        public static bool UpdatePurchaseRequests(SelectPurchaseRequestsDto previousEntity, UpdatePurchaseRequestsDto currentEntity)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                foreach (var line in currentEntity.SelectPurchaseRequestLines)
                {

                    #region By Date Stock Movement

                    var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = currentEntity.BranchID, WarehouseID = currentEntity.WarehouseID, ProductID = line.ProductID, Date_ = currentEntity.Date_ }, false, false, "");

                    var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                    if (entityByDate.Id == Guid.Empty)
                    {
                        foreach (var previousline in previousEntity.SelectPurchaseRequestLines)
                        {
                            var entityQueryByDateDecreasing = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = previousEntity.BranchID, WarehouseID = previousEntity.WarehouseID, ProductID = previousline.ProductID, Date_ = previousEntity.Date_ }, false, false, "");

                            var entityByDateDecreasing = queryFactory.Get<ByDateStockMovements>(entityQueryByDateDecreasing);

                            var decreasingAmount = line.Quantity;

                            var queryDecreasing = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                            {
                                Amount = entityByDateDecreasing.Amount,
                                Date_ = entityByDateDecreasing.Date_,
                                ProductID = previousline.ProductID,
                                TotalConsumption = entityByDateDecreasing.TotalConsumption,
                                TotalGoodsIssue = entityByDateDecreasing.TotalGoodsIssue,
                                TotalGoodsReceipt = entityByDateDecreasing.TotalGoodsReceipt,
                                TotalProduction = entityByDateDecreasing.TotalProduction,
                                TotalPurchaseOrder = entityByDateDecreasing.TotalPurchaseOrder,
                                TotalPurchaseRequest = entityByDateDecreasing.TotalPurchaseRequest - decreasingAmount,
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
                                DeleterId = entityByDateDecreasing.DeleterId.Value,
                                DeletionTime = entityByDateDecreasing.DeletionTime.Value,
                                IsDeleted = entityByDateDecreasing.IsDeleted,
                                LastModificationTime = DateTime.Now,
                                LastModifierId = LoginedUserService.UserId
                            }).Where(new { Id = entityByDateDecreasing.Id }, false, false, "");

                            var byDateStockMovementsDecreasing = queryFactory.Update<SelectByDateStockMovementsDto>(queryDecreasing, "Id", true);

                            var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
                            {
                                Amount = 0,
                                Date_ = currentEntity.Date_,
                                ProductID = line.ProductID,
                                TotalConsumption = 0,
                                TotalGoodsIssue = 0,
                                TotalGoodsReceipt = 0,
                                TotalProduction = 0,
                                TotalPurchaseOrder = 0,
                                TotalPurchaseRequest = line.Quantity,
                                TotalSalesOrder = 0,
                                TotalSalesProposition = 0,
                                TotalWastage = 0,
                                WarehouseID = currentEntity.WarehouseID,
                                BranchID = currentEntity.BranchID,
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
                    else
                    {
                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                        {
                            Amount = entityByDate.Amount,
                            Date_ = entityByDate.Date_,
                            ProductID = line.ProductID,
                            TotalConsumption = entityByDate.TotalConsumption,
                            TotalGoodsIssue = entityByDate.TotalGoodsIssue,
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
                            LastModificationTime = DateTime.Now,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityByDate.Id }, false, false, "");

                        var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                    }

                    #endregion

                    #region Grand Total Stock Movement

                    var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = currentEntity.BranchID, WarehouseID = currentEntity.WarehouseID, ProductID = line.ProductID }, false, false, "");

                    var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                    if (entityGrandTotal.Id == Guid.Empty)
                    {
                        foreach (var previousLine in previousEntity.SelectPurchaseRequestLines)
                        {
                            var entityQueryGrandTotalDecreasing = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = previousEntity.BranchID, WarehouseID = previousEntity.WarehouseID, ProductID = previousLine.ProductID }, false, false, "");

                            var entityGrandTotalDecreasing = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotalDecreasing);

                            var decreasingAmount = line.Quantity;

                            var queryDecreasing = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                            {
                                Amount = entityGrandTotalDecreasing.Amount,
                                ProductID = previousLine.ProductID,
                                TotalConsumption = entityGrandTotalDecreasing.TotalConsumption,
                                TotalGoodsIssue = entityGrandTotalDecreasing.TotalGoodsIssue,
                                TotalGoodsReceipt = entityGrandTotalDecreasing.TotalGoodsReceipt,
                                TotalProduction = entityGrandTotalDecreasing.TotalProduction,
                                TotalPurchaseOrder = entityGrandTotalDecreasing.TotalPurchaseOrder,
                                TotalPurchaseRequest = entityGrandTotalDecreasing.TotalPurchaseRequest - decreasingAmount,
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
                                DeleterId = entityGrandTotalDecreasing.DeleterId.Value,
                                DeletionTime = entityGrandTotalDecreasing.DeletionTime.Value,
                                IsDeleted = entityGrandTotalDecreasing.IsDeleted,
                                LastModificationTime = DateTime.Now,
                                BranchID = previousEntity.BranchID,
                                LastModifierId = LoginedUserService.UserId
                            }).Where(new { Id = entityGrandTotalDecreasing.Id }, false, false, "");

                            var grandTotalStockMovementsDecreasing = queryFactory.Update<SelectGrandTotalStockMovementsDto>(queryDecreasing, "Id", true);

                            var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                            {
                                Amount = 0,
                                ProductID = line.ProductID,
                                TotalConsumption = 0,
                                TotalGoodsIssue = 0,
                                TotalGoodsReceipt = 0,
                                TotalProduction = 0,
                                TotalPurchaseOrder = 0,
                                TotalPurchaseRequest = line.Quantity,
                                TotalSalesOrder = 0,
                                TotalReserved = 0,
                                TotalSalesProposition = 0,
                                TotalWastage = 0,
                                WarehouseID = currentEntity.WarehouseID,
                                BranchID = currentEntity.BranchID,
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
                    }

                    else
                    {
                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount,
                            ProductID = line.ProductID,
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
                            TotalProduction = entityGrandTotal.TotalProduction,
                            TotalPurchaseOrder = entityGrandTotal.TotalPurchaseOrder,
                            TotalPurchaseRequest = entityGrandTotal.TotalPurchaseRequest + line.Quantity,
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
                            DeleterId = entityGrandTotal.DeleterId.Value,
                            DeletionTime = entityGrandTotal.DeletionTime.Value,
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = DateTime.Now,
                            BranchID = entityGrandTotal.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, false, false, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                    }

                    #endregion
                }

            }

            return true;
        }

        public static bool DeletePurchaseRequests(SelectPurchaseRequestsDto deletedEntity)
        {

            using (var connection = queryFactory.ConnectToDatabase())
            {
                foreach (var line in deletedEntity.SelectPurchaseRequestLines)
                {

                    #region By Date Stock Movement

                    var entityQueryByDate = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { BranchID = deletedEntity.BranchID, WarehouseID = deletedEntity.WarehouseID, ProductID = line.ProductID, Date_ = deletedEntity.Date_ }, false, false, "");

                    var entityByDate = queryFactory.Get<ByDateStockMovements>(entityQueryByDate);

                    if (entityByDate.Id != Guid.Empty)
                    {
                        var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
                        {
                            Amount = entityByDate.Amount,
                            Date_ = entityByDate.Date_,
                            ProductID = line.ProductID,
                            TotalConsumption = entityByDate.TotalConsumption,
                            TotalGoodsIssue = entityByDate.TotalGoodsIssue,
                            TotalGoodsReceipt = entityByDate.TotalGoodsReceipt,
                            TotalProduction = entityByDate.TotalProduction,
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
                            DeleterId = entityByDate.DeleterId.Value,
                            DeletionTime = entityByDate.DeletionTime.Value,
                            IsDeleted = entityByDate.IsDeleted,
                            LastModificationTime = DateTime.Now,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityByDate.Id }, false, false, "");

                        var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);
                    }

                    #endregion

                    #region Grand Total Stock Movement

                    var entityQueryGrandTotal = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { BranchID = deletedEntity.BranchID, WarehouseID = deletedEntity.WarehouseID, ProductID = line.ProductID }, false, false, "");

                    var entityGrandTotal = queryFactory.Get<GrandTotalStockMovements>(entityQueryGrandTotal);

                    if (entityGrandTotal.Id != Guid.Empty)
                    {
                        var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                        {
                            Amount = entityGrandTotal.Amount,
                            ProductID = line.ProductID,
                            TotalConsumption = entityGrandTotal.TotalConsumption,
                            TotalGoodsIssue = entityGrandTotal.TotalGoodsIssue,
                            TotalGoodsReceipt = entityGrandTotal.TotalGoodsReceipt,
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
                            DeleterId = entityGrandTotal.DeleterId.Value,
                            DeletionTime = entityGrandTotal.DeletionTime.Value,
                            IsDeleted = entityGrandTotal.IsDeleted,
                            LastModificationTime = DateTime.Now,
                            BranchID = entityGrandTotal.BranchID,
                            LastModifierId = LoginedUserService.UserId
                        }).Where(new { Id = entityGrandTotal.Id }, false, false, "");

                        var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);
                    }

                    #endregion

                }
            }

            return true;
        }

        #endregion


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
