using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.FicheNumber
{
    public class FicheNumbers
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Sabit Karakter
        /// </summary>
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        
        public string FixedCharacter { get; set; }

        /// <summary>
        /// Fiş Numarası
        /// </summary>
        [SqlColumnType(SqlDbType = SqlDataType.Int)]
        public int FicheNo { get; set; }

        /// <summary>
        /// Uzunluk
        /// </summary>

        [SqlColumnType(SqlDbType = SqlDataType.Int)]
        public int Length_ { get; set; }

        /// <summary>
        /// Menü
        /// </summary>

        [SqlColumnType(MaxLength = 50, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        public string Menu_ { get; set; }
    }
}
