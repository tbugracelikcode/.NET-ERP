﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecordLine.Dtos;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.ShippingManagement.PalletRecord.Dtos
{
    public class SelectPalletRecordsDto : FullAuditedEntityDto
    {

        /// <summary>
        /// Palet Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Palet Adı
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// En
        /// </summary>
        public int Width_ { get; set; }
        /// <summary>
        /// Boy
        /// </summary>
        public int Height_ { get; set; }
        /// <summary>
        /// Koli Türü
        /// </summary>
        public string PackageType { get; set; }
        /// <summary>
        /// Planlanan Yükleme Tarihi
        /// </summary>
        public DateTime? PlannedLoadingTime { get; set; }
        /// <summary>
        /// Maksimum Koli Adedi
        /// </summary>
        public int MaxPackageNumber { get; set; }
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid? CurrentAccountCardID { get; set; }
        /// <summary>
        /// Cari Hesap Ünvan
        /// </summary>
        public string CurrentAccountCardName { get; set; }
        /// <summary>
        /// Cari Hesap Kodu
        /// </summary>
        public string CurrentAccountCardCode { get; set; }
        /// <summary>
        /// Yükseklik
        /// </summary>
        public int Lenght_ { get; set; }
        /// <summary>
        /// Palet Koli Adet
        /// </summary>
        public int PalletPackageNumber { get; set; }
        /// <summary>
        /// Çeki Listesi ID
        /// </summary>
        public Guid? PackingListID { get; set; }
        /// <summary>
        /// Çeki Listesi Kodu
        /// </summary>
        public string PackingListCode { get; set; }
        /// <summary>
        /// Durum
        /// </summary>
        public PalletRecordsStateEnum PalletRecordsStateEnum { get; set; }
        /// <summary>
        /// Etiket Basımı Durumu
        /// </summary>
        public PalletRecordsTicketStateEnum PalletRecordsTicketStateEnum { get; set; }
        /// <summary>
        /// Etiket Yazdır
        /// </summary>
        public PalletRecordsPrintTicketEnum PalletRecordsPrintTicketEnum { get; set; }

        [NoDatabaseAction]
        public List<SelectPalletRecordLinesDto> SelectPalletRecordLines { get; set; }
    }
}
