using SQLite;

namespace TsiErp.UretimEkranUI.Models
{
    [Table("ScrapTable")]
    public class ScrapTable
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }


        [Column("WorkOrderID")]
        public Guid WorkOrderID { get; set; }


        [Column("WorkOrderNo")]
        public string WorkOrderNo { get; set; }


        [Column("ScrapQuantity")]
        public decimal ScrapQuantity { get; set; } = 0;


        [Column("OperationUnsuitabilityRecordID")]
        public Guid OperationUnsuitabilityRecordID { get; set; }


        [Column("OperationUnsuitabilityRecordCode")]
        public string OperationUnsuitabilityRecordCode { get; set; }
    }
}
