using SQLite;

namespace TsiErp.UretimEkranUI.Models
{
    [Table("CurrentWorkOrder")]
    public class OperationDetailDto
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }


        [Column("WorkOrderID")]
        public Guid WorkOrderID { get; set; }


        [Column("WorkOrderNo")]
        public string WorkOrderNo { get; set; }


        [Column("ProductID")]
        public Guid ProductID { get; set; }


        [Column("ProductName")]
        public string ProductName { get; set; }


        [Column("PlannedQuantity")]
        public decimal PlannedQuantity { get; set; }


        [Column("ProducedQuantity")]
        public decimal ProducedQuantity { get; set; }


        [Column("StationID")]
        public Guid StationID { get; set; }


        [Column("StationCode")]
        public string StationCode { get; set; }


        [Column("ProductsOperationID")]
        public Guid ProductsOperationID { get; set; }


        [Column("ProductsOperationName")]
        public string ProductsOperationName { get; set; }


        [Column("EmployeeID")]
        public Guid EmployeeID { get; set; }


        [Column("EmployeeName")]
        public string EmployeeName { get; set; }


        [Column("QualitControlApprovalDate")]
        public DateTime? QualitControlApprovalDate { get; set; }


        [Column("TotalQualityControlApprovalTime")]
        public int TotalQualityControlApprovalTime { get; set; }


        [Column("ApprovedQuantity")]
        public decimal ApprovedQuantity { get; set; } = 0;


        [Column("ScrapQuantity")]
        public decimal ScrapQuantity { get; set; } = 0;


        [Column("WorkOrderState")]
        public int WorkOrderState { get; set; }
    }
}
