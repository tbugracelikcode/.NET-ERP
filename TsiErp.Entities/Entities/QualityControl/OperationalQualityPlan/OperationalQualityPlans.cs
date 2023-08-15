using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan
{
    public class OperationalQualityPlans : FullAuditedEntity
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Ürün ID
        /// </summary>
        public Guid ProductID { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        ///<summary>
        ///Operasyon ID
        /// </summary
        public Guid ProductsOperationID { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        ///<summary>
        ///Kontrol Türü ID
        /// </summary
        public Guid ControlTypesID { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        ///<summary>
        ///İş Merkezi ID
        /// </summary
        public Guid WorkCenterID { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        ///<summary>
        ///Kontrol Sıklığı
        /// </summary
        public string ControlFrequency { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        ///<summary>
        ///Kontrol Şartı ID
        /// </summary
        public Guid ControlConditionsID { get; set; }

        [SqlColumnType(MaxLength = 150, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        ///<summary>
        ///Kontrol Ekipmanı
        /// </summary
        public string Equipment { get; set; }

        [SqlColumnType(MaxLength = 150, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        ///<summary>
        ///Kontrol Sorumlusu
        /// </summary
        public string ControlManager { get; set; }


        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Olması Gereken Ölçü
        /// </summary>
        public decimal IdealMeasure { get; set; }


        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Alt Tolerans
        /// </summary>
        public decimal BottomTolerance { get; set; }


        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Üst Tolerans
        /// </summary>
        public decimal UpperTolerance { get; set; }


        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Periyodik Kontrol Ölçüsü
        /// </summary>
        public bool PeriodicControlMeasure { get; set; }

        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Resimdeki Ölçü Numarası
        /// </summary>
        public decimal MeasureNumberInPicture { get; set; }

        [SqlColumnType( MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Kalite Planı Kodu
        /// </summary>
        public string Code { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        ///<summary>
        ///Kontrol Sıklığı
        /// </summary
        public string Description_ { get; set; }
    }
}
