using MainModuleContext.Models;
using MainModuleDTO.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MainModuleInterFace.IDataServices
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
       Task<List<EmployeeDTO>> GetAllEmployeesPaging( int pageNumber, int pageSize);

    }
}
