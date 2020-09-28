using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ComputersApp.Api.Controllers;
using ComputersApp.Application.DataTransferObjects;
using ComputersApp.Application.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace ComputersApp.Tests.Controllers
{
    [TestFixture]
    class ComputerControllerTests
    {
        private Mock<IComputerService> _computerServiceMock;
        private ComputerController _computerController;

        [OneTimeSetUp]
        public void SetUp()
        {
            _computerServiceMock = new Mock<IComputerService>();
            _computerController = new ComputerController(_computerServiceMock.Object); ;
        }

        [Test]
        public async Task GetAllComputersAsync_Returns_OkObjectResultWithRequestedCount()
        {
            var testComputers = GetTestComputers();
            _computerServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(testComputers);

            var getAllComputersResult = await _computerController.GetAllAsync();

            var okResult = getAllComputersResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            var computers = okResult.Value as List<ComputerDto>;
            computers.Should().HaveCount(testComputers.Count);
        }

        [Test]
        public async Task GetAllComputersAsync_ComputersDoesNotExists_Returns_NotFoundResult()
        {
            _computerServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(null as List<ComputerDto>);

            var result = await _computerController.GetAllAsync();

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        private List<ComputerDto> GetTestComputers()
        {
            return new List<ComputerDto>
            {
                new ComputerDto(),
                new ComputerDto()
            };
        }

        [Test]
        public async Task GetComputerAsync_ComputerExists_Returns_OkObjectResultWithRequestedId()
        {
            var testComputer = GetTestComputer();

            _computerServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(testComputer);

            var getComputerResult = await _computerController.GetAsync(It.IsAny<int>());

            var okResult = getComputerResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            var resultComputer = okResult.Value as ComputerDto;
            resultComputer.Id.Should().Be(testComputer.Id);
        }

        private ComputerDto GetTestComputer()
        {
            return new ComputerDto() { Id = 1 };
        }

        [Test]
        public async Task GetComputerAsync_ComputerDoesNotExist_Returns_NotFoundResult()
        {
            _computerServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(null as ComputerDto);

            var result = await _computerController.GetAsync(It.IsAny<int>());

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task PutComputerAsync_ComputerExists_Returns_NoContent()
        {
            var testComputer = GetTestComputer();
            _computerServiceMock.Setup(m => m.UpdateAsync(It.IsAny<ComputerDto>())).ReturnsAsync(true);

            var putComputerResult = await _computerController.PutAsync(testComputer);

            putComputerResult.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task PutComputerAsync_ComputerDoesNotExist_Return_NotFound()
        {
            var testComputer = GetTestComputer();
            _computerServiceMock.Setup(m => m.UpdateAsync(It.IsAny<ComputerDto>())).ReturnsAsync(false);

            var putComputerResult = await _computerController.PutAsync(testComputer);

            putComputerResult.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task DeleteComputerAsync_ComputerExists_Returns_OkResult()
        {
            _computerServiceMock.Setup(m => m.RemoveAsync(It.IsAny<int>())).ReturnsAsync(true);

            var deleteComputerResult = await _computerController.DeleteAsync(It.IsAny<int>());

            deleteComputerResult.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task DeleteComputerAsync_ComputerDoesNotExist_Returns_NotFoundResult()
        {
            _computerServiceMock.Setup(m => m.RemoveAsync(It.IsAny<int>())).ReturnsAsync(false);

            var deleteComputerResult = await _computerController.DeleteAsync(It.IsAny<int>());

            deleteComputerResult.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task PostComputerAsync_Returns_CreatedAtActionResult()
        {
            var testComputer = GetTestComputer();
            _computerServiceMock.Setup(m => m.AddAsync(It.IsAny<ComputerDto>())).ReturnsAsync(testComputer);

            var createdAtActionResult = await _computerController.PostAsync(It.IsAny<ComputerDto>());
            var result = (ComputerDto)((CreatedAtActionResult)createdAtActionResult.Result).Value;

            result.Should().BeOfType<ComputerDto>();
            createdAtActionResult.Result.Should().BeOfType<CreatedAtActionResult>();
            result.Should().BeEquivalentTo(testComputer, options => options.Excluding(a => a.Id));
        }
    }
}
