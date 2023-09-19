using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.PFMEA.Dtos
{
    public class ListPFMEAsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        /// <summary>
        /// Durum
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// İlk Operasyonel SPC ID
        /// </summary>
        public Guid? FirstOperationalSPCID { get; set; }
        /// <summary>
        /// İlk Operasyonel SPC Kodu
        /// </summary>
        public string FirstOperationalSPCCode { get; set; }
        /// <summary>
        /// İş Merkezi ID
        /// </summary>
        public Guid? WorkCenterID { get; set; }
        /// <summary>
        /// İş Merkezi Adı
        /// </summary>
        public string WorkCenterName { get; set; }
        /// <summary>
        /// Operasyon ID
        /// </summary>
        public Guid? OperationID { get; set; }
        /// <summary>
        /// Operasyon Adı
        /// </summary>
        public string OperationName { get; set; }
        /// <summary>
        /// Operasyonun Gereklilikleri
        /// </summary>
        public string OperationRequirement { get; set; }
        /// <summary>
        /// İkinci Operasyonel SPC ID
        /// </summary>
        public Guid? SecondOperationalSPCID { get; set; }
        /// <summary>
        /// İkinci Operasyonel SPC Kodu
        /// </summary>
        public string SecondOperationalSPCCode { get; set; }
        /// <summary>
        /// Uygunsuzluk Başlığı ID
        /// </summary>
        public Guid? UnsuitabilityItemID { get; set; }
        /// <summary>
        /// Uygunsuzluk Başlığı Adı
        /// </summary>
        public string UnsuitabilityItemName { get; set; }
        /// <summary>
        /// Hatanın Oluşturacağı Etki
        /// </summary>
        public string ImpactofError { get; set; }
        /// <summary>
        /// Potansiyel Hata Nedeni
        /// </summary>
        public string PotentialErrorReason { get; set; }
        /// <summary>
        /// Kontrol Mekanizması
        /// </summary>
        public string ControlMechanism { get; set; }
        /// <summary>
        /// Kontrol Yöntemi
        /// </summary>
        public string ControlMethod { get; set; }
        /// <summary>
        /// Emniyet Sınıfı
        /// </summary>
        public string SafetyClass { get; set; }
        /// <summary>
        /// Mevcut Şiddet
        /// </summary>
        public int CurrentSeverity { get; set; }
        /// <summary>
        /// Mevcut Sıklık
        /// </summary>
        public int CurrentFrequency { get; set; }
        /// <summary>
        /// Mevcut Farkedilebilirlik
        /// </summary>
        public int CurrentDetectability { get; set; }
        /// <summary>
        /// Mevcut RPN
        /// </summary>
        public int CurrentRPN { get; set; }
        /// <summary>
        /// Önleyici Aksiyon
        /// </summary>
        public string InhibitorAction { get; set; }
        /// <summary>
        /// Hedef Bitiş Tarihi
        /// </summary>
        public DateTime? TargetEndDate { get; set; }
        /// <summary>
        /// Aksiyon Tamamlanma Tarihi
        /// </summary>
        public DateTime? ActionCompletionDate { get; set; }
        /// <summary>
        /// Yeni Şiddet
        /// </summary>
        public int NewSeverity { get; set; }
        /// <summary>
        /// Yeni Sıklık
        /// </summary>
        public int NewFrequency { get; set; }
        /// <summary>
        /// Yeni Farkedilebilirlik
        /// </summary>
        public int NewDetectability { get; set; }
        /// <summary>
        /// Yeni RPN
        /// </summary>
        public int NewRPN { get; set; }
    }
}
