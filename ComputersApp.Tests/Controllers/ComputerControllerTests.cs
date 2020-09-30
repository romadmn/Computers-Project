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
            // Arrange
            var testComputers = GetTestComputers();
            _computerServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(testComputers);

            // Act
            var getAllComputersResult = await _computerController.GetAllAsync();

            // Assert
            _computerServiceMock.Verify(x => x.GetAllAsync());
            getAllComputersResult.Should().BeOfType<ActionResult<List<ComputerDto>>>();
            var okResult = getAllComputersResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            var computers = okResult.Value as List<ComputerDto>;
            computers.Should().NotBeNullOrEmpty();
            computers.Should().HaveCount(testComputers.Count);
        }

        [Test]
        public async Task GetAllComputersAsync_ComputersDoesNotExists_Returns_NotFoundResult()
        {
            // Arrange
            _computerServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(null as List<ComputerDto>);

            // Act
            var result = await _computerController.GetAllAsync();

            // Assert
            _computerServiceMock.Verify(x => x.GetAllAsync());
            result.Result.Should().BeOfType<NotFoundResult>();
            result.Value.Should().BeNullOrEmpty();
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
            // Arrange
            var testComputer = GetTestComputer();
            _computerServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(testComputer);

            // Act
            var getComputerResult = await _computerController.GetAsync(It.IsAny<int>());

            // Assert
            _computerServiceMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()));
            var okResult = getComputerResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            var resultComputer = okResult.Value as ComputerDto;
            resultComputer.Should().NotBeNull();
            okResult.Value.Should().BeOfType<ComputerDto>();
            resultComputer.Id.Should().Be(testComputer.Id);
        }

        private ComputerDto GetTestComputer()
        {
            return new ComputerDto() { Id = 1 };
        }

        [Test]
        public async Task GetComputerAsync_ComputerDoesNotExist_Returns_NotFoundResult()
        {
            // Arrange
            _computerServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(null as ComputerDto);

            // Act
            var result = await _computerController.GetAsync(It.IsAny<int>());

            // Assert
            _computerServiceMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()));
            result.Should().BeOfType<ActionResult<ComputerDto>>();
            result.Value.Should().BeNull();
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task PutComputerAsync_ComputerExists_Returns_NoContent()
        {
            // Arrange
            var testComputer = GetTestComputer();
            _computerServiceMock.Setup(m => m.UpdateAsync(testComputer)).ReturnsAsync(true);

            // Act
            var putComputerResult = await _computerController.PutAsync(testComputer);

            // Assert
            _computerServiceMock.Verify(x => x.UpdateAsync(testComputer));
            putComputerResult.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task PutComputerAsync_ComputerDoesNotExist_Return_NotFound()
        {
            // Arrange
            var testComputer = GetTestComputer();
            _computerServiceMock.Setup(m => m.UpdateAsync(It.IsAny<ComputerDto>())).ReturnsAsync(false);

            // Act
            var putComputerResult = await _computerController.PutAsync(testComputer);

            // Assert
            _computerServiceMock.Verify(x => x.UpdateAsync(It.IsAny<ComputerDto>()), Times.AtLeastOnce);
            putComputerResult.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task DeleteComputerAsync_ComputerExists_Returns_OkResult()
        {
            // Arrange
            _computerServiceMock.Setup(m => m.RemoveAsync(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var deleteComputerResult = await _computerController.DeleteAsync(It.IsAny<int>());

            // Assert
            _computerServiceMock.Verify(x => x.RemoveAsync(It.IsAny<int>()), Times.AtLeastOnce);
            deleteComputerResult.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task DeleteComputerAsync_ComputerDoesNotExist_Returns_NotFoundResult()
        {
            // Arrange
            _computerServiceMock.Setup(m => m.RemoveAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var deleteComputerResult = await _computerController.DeleteAsync(It.IsAny<int>());

            // Assert
            _computerServiceMock.Verify(x => x.RemoveAsync(It.IsAny<int>()));
            deleteComputerResult.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task PostComputerAsync_Returns_CreatedAtActionResult()
        {
            // Arrange
            var testComputer = GetTestComputer();
            _computerServiceMock.Setup(m => m.AddAsync(It.IsAny<ComputerDto>())).ReturnsAsync(testComputer);

            // Act
            var createdAtActionResult = await _computerController.PostAsync(It.IsAny<ComputerDto>());
            var result = (ComputerDto)((CreatedAtActionResult)createdAtActionResult.Result).Value;

            // Assert
            _computerServiceMock.Verify(x => x.AddAsync(It.IsAny<ComputerDto>()));
            result.Should().NotBeNull();
            result.Should().BeOfType<ComputerDto>();
            createdAtActionResult.Result.Should().BeOfType<CreatedAtActionResult>();
            result.Should().BeEquivalentTo(testComputer, options => options.Excluding(a => a.Id));
        }
    }
}
