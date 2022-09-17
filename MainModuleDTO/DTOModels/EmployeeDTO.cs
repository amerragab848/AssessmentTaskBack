using System;
using System.Collections.Generic;
using System.Text;

namespace MainModuleDTO.DTOModels
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DepartmentName { get; set; }
        public double? Salary { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}
