using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.UretimEkranUI.Models
{
    [Table("ParametersTable")]
    public class ParametersTable
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("StationID")]
        public Guid StationID { get; set; }

        [Column("HaltTriggerSecond")]
        public int HaltTriggerSecond { get; set; }

        [Column("IsLoadcell")]
        public bool IsLoadcell { get; set; }

        [Column("MidControlPeriod")]
        public decimal MidControlPeriod { get; set; }
    }
}
