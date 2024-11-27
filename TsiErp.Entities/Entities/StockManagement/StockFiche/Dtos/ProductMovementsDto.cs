using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.StockManagement.StockFiche.Dtos
{
    public class ProductMovementsDto
    {
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }

        /// <summary>
        /// Stok Kodu
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Stok Adı
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Stok Fiş Tarihi
        /// </summary>
        public DateTime Date_ { get; set; }

        /// <summary>
        /// Stok Fiş Numarası
        /// </summary>
        public string StockFicheNr { get; set; }

        /// <summary>
        /// Stok Fiş Türü Enum
        /// </summary>
        public StockFicheTypeEnum StockFicheType { get; set; }

        /// <summary>
        /// Giriş Çıkış Kodu
        /// </summary>
        public int InputOutputCode { get; set; }
        /// <summary>
        /// Üretim Tarihi Referans ID
        /// </summary>
        public Guid? ProductionDateReferenceID { get; set; }
        /// <summary>
        /// Üretim Tarihi Referans  No
        /// </summary>
        public string ProductionDateReferenceNo { get; set; }

        /// <summary>
        /// Birim Set ID
        /// </summary>
        public Guid UnitSetID { get; set; }

        /// <summary>
        /// Birim Set Adı
        /// </summary>
        public string UnitSetName { get; set; }

        /// <summary>
        /// Miktar
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Birim Fiyat
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Parti Numarası
        /// </summary>
        public string PartyNo { get; set; }

        /// <summary>
        /// Satır Tutarı
        /// </summary>
        public decimal LineAmount { get; set; }

        /// <summary>
        /// Satır Açıklaması
        /// </summary>
        public string LineDescription { get; set; }


        /// <summary>
        /// Satınalma Sipariş ID
        /// </summary>
        public Guid? PurchaseOrderID { get; set; }

        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid? ProductionOrderID { get; set; }

        /// <summary>
        /// Stok Fiş Türü Açıklaması
        /// </summary>
        public string StockFicheTypeName { get; set; }

        /// <summary>
        /// Giriş Çıkış Kodu Açıklaması
        /// </summary>
        public string InputOutputCodeDescripton { get; set; }

        /// <summary>
        /// Bağlı Olduğu Modül Adı
        /// </summary>
        public string LinkedModuleName { get; set; }

        /// <summary>
        /// Bağlı Olduğu Modül Fiş Numarası
        /// </summary>
        public string LinkedModuleFicheNumber { get; set; }

        /// <summary>
        /// Cari Hesap Ünvanı
        /// </summary>
        public string CurrentAccountCardName { get; set; }



    }
}
