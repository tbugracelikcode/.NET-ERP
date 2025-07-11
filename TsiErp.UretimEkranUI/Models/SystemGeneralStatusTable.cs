﻿using SQLite;

namespace TsiErp.UretimEkranUI.Models
{
    [Table("SystemGeneralStatusTable")]
    public class SystemGeneralStatusTable
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("GeneralStatus")]
        public int GeneralStatus { get; set; }


        [Column("isLoadCell")]
        public bool isLoadCell { get; set; }


        [Column("StationID")]
        public Guid StationID { get; set; }

        [Column("StationCode")]
        public string StationCode { get; set; }
    }
}
