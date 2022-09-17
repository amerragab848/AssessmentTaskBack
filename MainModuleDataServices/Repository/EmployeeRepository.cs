using MainModuleContext.Models;
using MainModuleDTO.DTOModels;
using MainModuleInterFace.IDataServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MainModuleContext.Context;

namespace MainModuleDataServices.Repository
{
    public class EmployeeRepository : GenericRepository<MainModuleContext.Context.MainModuleContext, Employee>
        , IEmployeeRepository
    {
        public async Task<List<EmployeeDTO>> GetAllEmployeesPaging(int pageNumber, int pageSize)
        {
            var list =  Context.Employees.
                        Select(q=> new EmployeeDTO
                        {
                            Id = q.Id,
                            FirstName = q.FirstName,
                            CreationDate = q.CreationDate,
                            LastName = q.LastName,
                            Salary = q.Salary,
                            DepartmentName=q.DepartmentName,
                        }).OrderByDescending(c => c.Id).Skip(pageNumber * pageSize).Take(pageSize).ToList();

                       return list;
        }
    }
}
