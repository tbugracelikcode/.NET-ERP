using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.Logging
{
    public class Logs : IEntity
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
