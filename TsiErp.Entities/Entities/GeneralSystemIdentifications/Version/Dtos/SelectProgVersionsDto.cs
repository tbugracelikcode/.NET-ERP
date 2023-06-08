using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.Version.Dtos
{
    public class SelectProgVersionsDto
    {

        public Guid Id { get; set; }

        public string MajDbVersion { get; set; }

        public string MinDbVersion { get; set; }

        public string BuildVersion { get; set; }

        public bool IsUpdating { get; set; }
    }
}
