using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ShippingManagement.ShippingAdress.Dtos
{
    public class CreateShippingAdressesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Müşteri Kartı ID
        /// </summary>
        public Guid? CustomerCardID { get; set; }
        /// <summary>
        /// Adres 1
        /// </summary>
        public string Adress1 { get; set; }
        /// <summary>
        /// Adres 2
        /// </summary>
        public string Adress2 { get; set; }
        /// <summary>
        /// İlçe
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// Şehir
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Ülke
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// Posta Kodu
        /// </summary>
        public string PostCode { get; set; }
        /// <summary>
        /// Telefon
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// E-Posta
        /// </summary>
        public string EMail { get; set; }
        /// <summary>
        /// Faks
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// Varsayılan
        /// </summary>
        public bool _Default { get; set; }
    }
}
