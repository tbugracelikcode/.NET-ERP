using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department.Dtos
{
    public class UpdateDepartmentsDto : FullAuditedEntityDto
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
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Kıdem ID
        /// </summary>
        public Guid? SeniorityID { get; set; }
    }
}
