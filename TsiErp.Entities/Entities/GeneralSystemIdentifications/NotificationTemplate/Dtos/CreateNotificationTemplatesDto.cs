using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.NotificationTemplate.Dtos
{
    public class CreateNotificationTemplatesDto : IEntityDto
    {

        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }


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
        /// Ad
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Kaynak Departman ID
        /// </summary>
        public Guid SourceDepartmentId { get; set; }

        /// <summary>
        /// Hedef Departman ID
        /// </summary>
        public string TargetDepartmentId { get; set; }

        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }


        /// <summary>
        /// str
        /// </summary>
        public string QueryStr { get; set; }
        /// <summary>
        /// userID
        /// </summary>
        public string TargetUsersId { get; set; }


    }
}
