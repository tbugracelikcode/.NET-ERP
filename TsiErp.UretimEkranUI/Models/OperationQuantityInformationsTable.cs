using SQLite;

namespace TsiErp.UretimEkranUI.Models
{
    [Table("OperationQuantityInformationsTable")]
    public class OperationQuantityInformationsTable
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }


        [Column("Date_")]
        public DateTime Date_ { get; set; }

        [Column("Hour_")]
        public TimeSpan Hour_ { get; set; }

        [Column("WorkOrderID")]
        public Guid WorkOrderID { get; set; }

        [Column("ProductionOrderID")]
        public Guid ProductionOrderID { get; set; }

        [Column("OperatorID")]
        public Guid OperatorID { get; set; }

        [Column("AttachmentTime")]
        public decimal AttachmentTime { get; set; }

        [Column("OperationTime")]
        public decimal OperationTime { get; set; }

        [Column("ProductionTrackingID")]
        public Guid ProductionTrackingID { get; set; }

        [Column("StationID")]
        public Guid StationID { get; set; }

        [Column("ProductsOperationID")]
        public Guid ProductsOperationID { get; set; }

        [Column("Type_")]
        public int Type_ { get; set; }
    }
}
