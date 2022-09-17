using MainModuleContext.Models;
using MainModuleDataServices.Repository;
using MainModuleDTO.DTOModels;
using MainModuleInterFace.IDataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace web_api_tests
{
	public class EmployeeServiceFake  : GenericRepository<MainModuleContext.Context.MainModuleContext, Employee>
        , IEmployeeRepository
    {
        private readonly List<EmployeeDTO> _empoyees;

        public EmployeeServiceFake()
        {
            _empoyees = new List<EmployeeDTO>()
            {
                new EmployeeDTO() { Id = 1,
                    DepartmentName = "cairo dept", FirstName="amer", Salary = 5.00,LastName="ragab"},
            
            };
        }



        public async Task<List<EmployeeDTO>> GetAllEmployeesPaging(int pageNumber, int pageSize)
        {
            return  _empoyees;
        }

    }
}
