﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.User.Dtos
{
    public class ListUsersParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
