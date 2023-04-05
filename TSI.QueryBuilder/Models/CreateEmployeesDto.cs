using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;

namespace TSI.QueryBuilder.Models
{
    public class CreateEmployeesDto 
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Title { get; set; }

        public string TitleOfCourtesy { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime? HireDate { get; set; }

        [NoDatabaseAction]
        public string Address { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public string HomePhone { get; set; }

        public string Extension { get; set; }

        public byte[] Photo { get; set; }

        public string Notes { get; set; }

        public string PhotoPath { get; set; }
    }
}
