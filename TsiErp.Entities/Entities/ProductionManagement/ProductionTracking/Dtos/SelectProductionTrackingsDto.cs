﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos
{
    public class SelectProductionTrackingsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Üretim Takip Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// İş Emri ID
        /// </summary>
        public Guid WorkOrderID { get; set; }
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid? CurrentAccountCardID { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Müşteri Kodu
        /// </summary>
        public string CustomerCode { get; set; }
        /// <summary>
        /// İş Emri Kodu
        /// </summary>
        public string WorkOrderCode { get; set; }
        /// <summary>
        /// Üretilen Miktar
        /// </summary>
        public decimal ProducedQuantity { get; set; }
        /// <summary>
        /// Operasyon Süresi
        /// </summary>
        public decimal OperationTime { get; set; }
        /// <summary>
        /// Operasyon Başlangıç Tarihi
        /// </summary>
        public DateTime? OperationStartDate { get; set; }
        /// <summary>
        /// Başlangıç Saati
        /// </summary>
        public TimeSpan? OperationStartTime { get; set; }
        /// <summary>
        /// Operasyon Bitiş Tarihi
        /// </summary>
        public DateTime? OperationEndDate { get; set; }
        /// <summary>
        /// Bitiş Saati
        /// </summary>
        public TimeSpan? OperationEndTime { get; set; }
        /// <summary>
        /// Duruş Süresi
        /// </summary>
        public decimal HaltTime { get; set; }
        /// <summary>
        /// Ayar Süresi
        /// </summary>
        public decimal AdjustmentTime { get; set; }
        /// <summary>
        /// Planlanan Adet
        /// </summary>
        public decimal PlannedQuantity { get; set; }
        /// <summary>
        /// Hatalı Miktar
        /// </summary>
        public decimal FaultyQuantity { get; set; }
        /// <summary>
        /// Tamamlandı mı ?
        /// </summary>
        public bool IsFinished { get; set; }
        /// <summary>
        /// İş İstasyonu Kody
        /// </summary>
        public string StationCode { get; set; }
        /// <summary>
        /// Çalışan Adı
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// Çalışan Soyadı
        /// </summary>
        public string EmployeeSurname { get; set; }
        /// <summary>
        /// Vardiya Kodu
        /// </summary>
        public string ShiftCode { get; set; }
        /// <summary>
        /// İş İstasyonu ID
        /// </summary>
        public Guid StationID { get; set; }
        /// <summary>
        /// Çalışan ID
        /// </summary>
        public Guid EmployeeID { get; set; }
        /// <summary>
        /// Vardiya ID
        /// </summary>
        public Guid ShiftID { get; set; }

        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }

        /// <summary>
        /// Stok Kodu
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid ProductionOrderID { get; set; }

        /// <summary>
        /// Üretim Emri Numarası
        /// </summary>
        public string ProductionOrderCode { get; set; }

        /// <summary>
        /// Ürüne Özel Operasyon ID
        /// </summary>
        public Guid ProductsOperationID { get; set; }

        /// <summary>
        /// Operasyon Adı
        /// </summary>
        public string ProductOperationName { get; set; }

        /// <summary>
        /// Stok Türü
        /// </summary>
        public ProductTypeEnum ProductType { get; set; }

        /// <summary>
        /// Stok Ürün Grubu ID
        /// </summary>
        public Guid ProductGroupID { get; set; }
        /// <summary>
        /// Ürün Grubu Kodu
        /// </summary>
        public string ProductGroupCode { get; set; }
        /// <summary>
        /// Ürün Grubu adı
        /// </summary>
        public string ProductGroupName { get; set; }


        /// <summary>
        /// Tür
        /// </summary>
        public ProductionTrackingTypesEnum ProductionTrackingTypes { get; set; }

        /// <summary>
        /// Tür Adı
        /// </summary>
        public string ProductionTrackingTypesName { get; set; }

        /// <summary>
        /// Duruş Sebep ID
        /// </summary>
        public Guid? HaltReasonID { get; set; }
        /// <summary>
        /// Duruş Sebep Kodu
        /// </summary>
        public string HaltReasonCode { get; set; }
        /// <summary>
        /// Duruş Sebep Adı
        /// </summary>
        public string HaltReasonName { get; set; }


    }
}
