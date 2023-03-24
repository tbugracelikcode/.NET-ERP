using System;
using System.Collections.Generic;
using System.Text;

namespace TSI.QueryBuilder.Models
{
    public class Employees
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Title { get; set; }

        public string TitleOfCourtesy { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
    }
}
