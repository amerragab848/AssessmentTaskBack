using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainModuleContext.Models;
using MainModuleDTO.DTOModels;
using MainModuleInterFace.IDataServices;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AmanaTask.Controllers
{
    [Route("api/Employee")]
    [ApiController]
   // [Authorize]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        // GET: api/<Blog>
        [HttpGet]
        [Route("GetAllPaging")]
        public async Task< IActionResult> GetAllPaging(int pageNumber,int pageSize)
        {
            var res = await _employeeRepository.GetAllEmployeesPaging(pageNumber, pageSize);
            return Ok(res);
        }

        // GET api/<Blog>/5
        [HttpGet]
        [Route("GetEmployeeById")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            if (id > 0)
            {
                var res =  _employeeRepository.FindBy(c => c.Id == id).FirstOrDefault();
                return Ok(res);

            }
            return Ok(-1);
        }

        // POST api/<Blog>
        [HttpPost]
        [Route("AddEmployee")]
        public async Task<IActionResult> AddEmployee(EmployeeDTO employee)
        {
            if(employee != null)
            {
                var employeeEF = new Employee
                {

                    CreationDate = DateTime.Now,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    DepartmentName = employee.DepartmentName,
                    Salary = employee.Salary,

                };
                try
                {
                    _employeeRepository.Add(employeeEF);
                    _employeeRepository.Save();
                    return Ok(employee);

                }catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
               
            }
            return Ok(-1);
        }

        // PUT api/<Blog>/5
        [HttpPost]
        [Route("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee(EmployeeDTO employee)
        {
            if (employee != null)
            {
                var employeeRes = _employeeRepository.FindBy(c => c.Id == employee.Id).FirstOrDefault();
                if (employeeRes != null)
                {


                    employeeRes.FirstName = employee.FirstName;
                    employeeRes.LastName = employee.LastName;
                    employeeRes.DepartmentName = employee.DepartmentName;
                    employeeRes.Salary = employee.Salary;
                    _employeeRepository.Edit(employeeRes);
                    _employeeRepository.Save();
                    return Ok(employee);
                }

            }

            return Ok(-1);
        }

        // DELETE api/<Blog>/5
        [HttpDelete]
        [Route("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (id > 0)
            {
                var employee = _employeeRepository.FindBy(c => c.Id == id).FirstOrDefault();
                if(employee != null)
                {
                    _employeeRepository.Delete(employee);
                    _employeeRepository.Save();
                    return Ok("deleted");
                }
                else
                {
                    return Ok(-1);
                }
               

            }
            return Ok(-1);
        }
    }
}
