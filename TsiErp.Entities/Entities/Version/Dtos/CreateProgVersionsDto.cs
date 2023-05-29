using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Entities.Version.Dtos
{
    public class CreateProgVersionsDto
    {
        public Guid Id { get; set; }

        public string MajDbVersion { get; set; }

        public string MinDbVersion { get; set; }

        public string BuildVersion { get; set; }
    }
}
