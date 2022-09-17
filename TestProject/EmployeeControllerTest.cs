using AmanaTask.Controllers;
using MainModuleDTO.DTOModels;
using MainModuleInterFace.IDataServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace web_api_tests
{
    public class EmployeeControllerTest
    {
        private readonly EmployeeController _controller;
        private readonly IEmployeeRepository _service;

        public EmployeeControllerTest()
        {
            _service = new EmployeeServiceFake();
            _controller = new EmployeeController(_service);
        }

      

        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okResult = _controller.GetAllPaging(1,10) as IActionResult;

            // Assert
            var items = Assert.IsType<List<EmployeeDTO>>(okResult);
            Assert.Equal(3, items.Count);
        }

        [Fact]
        public void GetById_UnknownPassed_ReturnsNotFoundResult()
        {
            // not exist  
            // Act
            var notFoundResult = _controller.GetEmployeeById(10);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void GetById_ExistingPassed_ReturnsOkResult()
        {
            // Arrange
            var id =10;

            // Act
            var okResult = await _controller.GetEmployeeById(id);

            // Assert
            Assert.IsType<ActionResult>(okResult as IActionResult );
        }

        [Fact]
        public void GetById_ExistingGuidPassed_ReturnsRightItem()
        {
            // Arrange
            var id = 1;

            // Act
            var okResult = _controller.GetEmployeeById(id) as IActionResult;

            // Assert
            Assert.IsType<EmployeeDTO>(okResult);
            Assert.Equal(id, (okResult as EmployeeDTO).Id);
        }

        [Fact]
        public void Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            var nameMissingItem = new EmployeeDTO()
            {
                DepartmentName = "Guinness",
                Salary = 12.00
            };
            _controller.ModelState.AddModelError("FirstName", "Required");

            // Act
            var badResponse = _controller.AddEmployee(nameMissingItem);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        [Fact]
        public void Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            EmployeeDTO testItem = new EmployeeDTO()
            {
                FirstName = "Amer",
                LastName="sabwe",
                DepartmentName= "Guinness",
                Salary = 12.00
            };

            // Act
            var createdResponse = _controller.AddEmployee(testItem);

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse);
        }

        [Fact]
        public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            var testItem = new EmployeeDTO()
            {
                FirstName = "Amer",
                LastName = "sabwe",
                DepartmentName = "Guinness",
                Salary = 12.00
            };

            // Act
            var createdResponse = _controller.AddEmployee(testItem) as IActionResult;
            var item = createdResponse as EmployeeDTO;

            // Assert
            Assert.IsType<EmployeeDTO>(item);
            Assert.Equal("Amer", item?.FirstName);
        }

        [Fact]
        public void Remove_NotExistingPassed_ReturnsIActionResult()
        {
            // Arrange
            var id = 11;

            // Act
            var badResponse = _controller.DeleteEmployee(id) as IActionResult;

            // Assert
            Assert.IsType<IActionResult>(badResponse);
        }

        [Fact]
        public void Remove_ExistingPassed_ReturnsIActionResult()
        {
            // Arrange
            var id = 1;

            // Act
            var noContentResponse = _controller.DeleteEmployee(id) as IActionResult;

            // Assert
            Assert.IsType<IActionResult>(noContentResponse);
        }

        [Fact]
        public void Remove_ExistingPassed_RemovesOneItem()
        {
            // Arrange
            var id =2;

            // Act
            var okResponse = _controller.DeleteEmployee(id);

            // Assert
            Assert.Equal(2, _service.GetAll().Count());
        }
    }
}
