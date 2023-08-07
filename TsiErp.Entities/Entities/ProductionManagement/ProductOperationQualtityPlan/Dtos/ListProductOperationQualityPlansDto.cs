using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ProductionManagement.ProductOperationQualtityPlan.Dtos
{
    public class ListProductOperationQualityPlansDto : FullAuditedEntityDto
    {

        ///<summary>
        ///Kontrol Türü
        /// </summary
        public string ControlTypesName { get; set; }

        ///<summary>
        ///İş Merkezi
        /// </summary
        public string WorkCenterName { get; set; }

        ///<summary>
        ///Kontrol Sıklığı
        /// </summary
        public string ControlFrequency { get; set; }

        ///<summary>
        ///Kontrol Şartı
        /// </summary
        public string ControlConditionsName { get; set; }

        ///<summary>
        ///Kontrol Ekipmanı
        /// </summary
        public string Equipment { get; set; }

        ///<summary>
        ///Kontrol Sorumlusu
        /// </summary
        public string ControlManager { get; set; }

        /// <summary>
        /// Satır Numarası
        /// </summary>
        public int LineNr { get; set; }

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
    }
}
