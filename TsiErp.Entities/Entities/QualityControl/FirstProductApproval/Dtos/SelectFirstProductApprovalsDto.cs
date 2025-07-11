﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.QualityControl.FirstProductApprovalLine.Dtos;

namespace TsiErp.Entities.Entities.QualityControl.FirstProductApproval.Dtos
{
    public class SelectFirstProductApprovalsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// İlk Üürn Onay Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Final Kontrol mü?
        /// </summary>
        public bool IsFinalControl { get; set; }
        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid? ProductionOrderID { get; set; }
        /// <summary>
        /// Birim Ağırlık
        /// </summary>
        public decimal UnitWeight { get; set; }
        /// <summary>
        /// İş Emri ID
        /// </summary>
        public Guid? WorkOrderID { get; set; }
        /// <summary>
        /// İş Emri No
        /// </summary>
        public string WorkOrderNo { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Stok Kodu
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Stok Açıklaması
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Çalışan ID
        /// </summary>
        public Guid? EmployeeID { get; set; }
        /// <summary>
        /// Çalışan Adı
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// Çalışan Soyadı
        /// </summary>
        public string EmployeeSurname { get; set; }
        /// <summary>
        /// İş Emri ID
        /// </summary>
        public DateTime? ControlDate { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Operasyon Kalite ID
        /// </summary>
        public Guid? OperationQualityPlanID { get; set; }

        /// <summary>
        /// Operasyon Kalite Planı Döküman Numarası
        /// </summary>
        public string OperationQualityPlanDocumentNumber { get; set; }
        /// <summary>
        /// Ayarı Yapan Kullanıcı ID
        /// </summary>
        public Guid? AdjustmentUserID { get; set; }
        /// <summary>
        /// Ayarı Yapan Kullanıcı ID
        /// </summary>
        public string AdjustmentUser { get; set; }
        /// <summary>
        /// Onay
        /// </summary>
        public bool IsApproval { get; set; }

        /// <summary>
        /// Onaylanan Adet
        /// </summary>
        public decimal ApprovedQuantity { get; set; }

        /// <summary>
        /// Hurda Adet
        /// </summary>
        public decimal ScrapQuantity { get; set; }

        [NoDatabaseAction]
        public List<SelectFirstProductApprovalLinesDto> SelectFirstProductApprovalLines { get; set; }
    }
}
