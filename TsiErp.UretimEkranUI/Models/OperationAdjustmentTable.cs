using SQLite;

namespace TsiErp.UretimEkranUI.Models
{
    [Table("OperationAdjustmentTable")]
    public class OperationAdjustmentTable
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }


        [Column("AdjustmentUserID")]
        public Guid AdjustmentUserID { get; set; }


        [Column("AdjustmentUserName")]
        public string AdjustmentUserName { get; set; }


        [Column("AdjustmentUserPassword")]
        public string AdjustmentUserPassword { get; set; }

        [Column("AdjustmentDate")]
        public DateTime AdjustmentDate { get; set; }


        [Column("TotalAdjustmentTime")]
        public int TotalAdjustmentTime { get; set; } = 0;


        [Column("TotalQualityControlApprovalTime")]
        public int TotalQualityControlApprovalTime { get; set; } = 0;

    }
}
