using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities;

namespace Tsi.Logging.EntityFrameworkCore.Entities
{
    public class Logs : IEntity
    {
        public Guid Id { get; set; }

        public DateTime Date_ { get; set; }

        public string MethodName_ { get; set; }

        public object BeforeValues { get; set; }

        public object AfterValues { get; set; }

        public string LogLevel_ { get; set; }

        public Guid UserId { get; set; }


    }
}
