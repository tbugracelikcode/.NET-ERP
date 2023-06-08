using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Entities.Other.Logging.Dtos
{
    public class CreateLogsDto
    {
        public Guid Id { get; set; }
        public DateTime Date_ { get; set; }
        public string MethodName_ { get; set; }
        public object BeforeValues { get; set; }
        public object AfterValues { get; set; }
        public object DiffValues { get; set; }
        public string LogLevel_ { get; set; }
        public Guid UserId { get; set; }
        public Guid RecordId { get; set; }
    }
}
