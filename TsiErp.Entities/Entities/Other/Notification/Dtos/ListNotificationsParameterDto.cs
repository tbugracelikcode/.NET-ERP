using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;

namespace TsiErp.Entities.Entities.Other.Notification.Dtos
{
    public class ListNotificationsParameterDto : IEntityDto
    {

        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }


        /// <summary>
        /// Kullanıcı ID
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Mesaj
        /// </summary>
        public string Message_ { get; set; }

        /// <summary>
        /// Bildirim Tarihi
        /// </summary>
        public DateTime? NotificationDate { get; set; }

        /// <summary>
        /// Görünürlük
        /// </summary>
        public bool IsViewed { get; set; }

        /// <summary>
        /// Görülme Tarihi
        /// </summary>
        public DateTime? ViewDate { get; set; }

        /// <summary>
        /// Modül Adı
        /// </summary>
        public string ModuleName_ { get; set; }


        /// <summary>
        /// İşlem Adı
        /// </summary>
        public string ProcessName_ { get; set; }


        /// <summary>
        /// Context Menü Adı
        /// </summary>
        public string ContextMenuName_ { get; set; }


        /// <summary>
        /// Kayıt No
        /// </summary>
        public string RecordNumber { get; set; }


    }
}

