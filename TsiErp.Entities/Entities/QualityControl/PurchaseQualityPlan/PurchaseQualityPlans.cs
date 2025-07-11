﻿using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlan
{
    public class PurchaseQualityPlans : FullAuditedEntity
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Ürün ID
        /// </summary>
        public Guid ProductID { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        ///<summary>
        ///Cari Hesap ID
        /// </summary
        public Guid CurrrentAccountCardID { get; set; }

        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Döküman Numarası
        /// </summary>
        public string DocumentNumber { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Parti İçi Numune Sayısı
        /// </summary>
        public int NumberofSampleinPart { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Kabul Edilebilir Hatalı Ürün Sayısı
        /// </summary>
        public int AcceptableNumberofDefectiveProduct { get; set; }



        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        ///<summary>
        ///Açıklama
        /// </summary
        public string Description_ { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Yüklenen Domain
        /// </summary>
        public string DrawingDomain { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Dosya Yolu
        /// </summary>
        public string DrawingFilePath { get; set; }

        [SqlColumnType(MaxLength = 150, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Dosya adı
        /// </summary>
        public string UploadedFileName { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        ///<summary>
        /// Revizyon No
        /// </summary
        public string RevisionNo { get; set; }
    }
}
