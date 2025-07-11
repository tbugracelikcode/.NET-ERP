﻿using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency
{
    /// <summary>
    /// Para Birimleri
    /// </summary>
    public class Currencies : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        [SqlColumnType(MaxLength = 200, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Name { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Yerel Para Birimi
        /// </summary>
        public bool IsLocalCurrency { get; set; }


        [SqlColumnType(MaxLength = 200, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Sembol
        /// </summary>
        public string CurrencySymbol { get; set; }
    }
}
