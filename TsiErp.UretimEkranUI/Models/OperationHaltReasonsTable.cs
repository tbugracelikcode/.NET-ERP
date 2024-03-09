using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace TsiErp.UretimEkranUI.Models
{
    [Table("OperationHaltReasonsTable")]
    public class OperationHaltReasonsTable
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("WorkOrderID")]
        public Guid WorkOrderID { get; set; }

        [Column("StationID")]
        public Guid StationID { get; set; }

        [Column("EmployeeID")]
        public Guid EmployeeID { get; set; }

        [Column("HaltReasonID")]
        public Guid HaltReasonID { get; set; }

        [Column("TotalHaltReasonTime")]
        public int TotalHaltReasonTime { get; set; }
    }
}
