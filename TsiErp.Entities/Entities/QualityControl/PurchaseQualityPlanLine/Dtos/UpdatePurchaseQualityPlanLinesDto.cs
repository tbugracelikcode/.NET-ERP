using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlanLine.Dtos
{
    public class UpdatePurchaseQualityPlanLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Satın Alma Kalite Planı ID
        /// </summary>
        public Guid PurchaseQualityPlanID { get; set; }
        /// <summary>
        /// Ürün ID
        /// </summary>
        public Guid? ProductID { get; set; }

        ///<summary>
        ///Kontrol Türü ID
        /// </summary
        public Guid? ControlTypesID { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }

        ///<summary>
        ///İş Merkezi ID
        /// </summary
        public Guid? WorkCenterID { get; set; }

        ///<summary>
        ///Kontrol Sıklığı
        /// </summary
        public string ControlFrequency { get; set; }

        ///<summary>
        ///Kontrol Şartı ID
        /// </summary
        public Guid? ControlConditionsID { get; set; }

        ///<summary>
        ///Kontrol Ekipmanı
        /// </summary
        public string Equipment { get; set; }

        ///<summary>
        ///Kontrol Sorumlusu
        /// </summary
        public string ControlManager { get; set; }

        /// <summary>
        /// Olması Gereken Ölçü
        /// </summary>
        public decimal IdealMeasure { get; set; }

        /// <summary>
        /// Alt Tolerans
        /// </summary>
        public decimal BottomTolerance { get; set; }

        /// <summary>
        /// Üst Tolerans
        /// </summary>
        public decimal UpperTolerance { get; set; }

        /// <summary>
        /// Periyodik Kontrol Ölçüsü
        /// </summary>
        public bool PeriodicControlMeasure { get; set; }

        /// <summary>
        /// Resimdeki Ölçü Numarası
        /// </summary>
        public decimal MeasureNumberInPicture { get; set; }

        /// <summary>
        /// Kalite Planı Kodu
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }

        ///<summary>
        ///Açıklama
        /// </summary
        public string Description_ { get; set; }
    }
}
