using ComputersApp.Api.Controllers;
using ComputersApp.Application.DataTransferObjects;
using ComputersApp.Application.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ComputersApp.Tests.Controllers
{
    [TestFixture]
    class CpuControllerTests
    {
        private Mock<ICpuService> _cpuServiceMock;
        private CpuController _cpuController;

        [OneTimeSetUp]
        public void SetUp()
        {
            _cpuServiceMock = new Mock<ICpuService>();
            _cpuController = new CpuController(_cpuServiceMock.Object);
        }

        [Test]
        public async Task GetAllCpusAsync_CpusExists_Returns_OkObjectResultWithRequestedCount()
        {
            // Arrange
            var testCpus = GetTestCpus();
            _cpuServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(testCpus);

            // Act
            var getAllCpusResult = await _cpuController.GetAllAsync();

            // Assert
            _cpuServiceMock.Verify(x => x.GetAllAsync());
            getAllCpusResult.Should().BeOfType<ActionResult<List<CpuDto>>>();
            var okResult = getAllCpusResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            var cpus = okResult.Value as List<CpuDto>;
            cpus.Should().NotBeNullOrEmpty();
            cpus.Should().HaveCount(testCpus.Count);
        }

        [Test]
        public async Task GetAllCpusAsync_CpusDoesNotExists_Returns_NotFoundResult()
        {
            // Arrange
            _cpuServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(null as List<CpuDto>);

            // Arrange
            var result = await _cpuController.GetAllAsync();

            // Assert
            _cpuServiceMock.Verify(x => x.GetAllAsync());
            result.Result.Should().BeOfType<NotFoundResult>();
            result.Value.Should().BeNullOrEmpty();
        }

        private List<CpuDto> GetTestCpus()
        {
            return new List<CpuDto>
            {
                new CpuDto(),
                new CpuDto()
            };
        }

        [Test]
        public async Task GetCpuAsync_CpuExists_Returns_OkObjectResultWithRequestedId()
        {
            // Arrange
            var testCpu = GetTestCpu();
            _cpuServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(testCpu);

            // Act
            var getCpuResult = await _cpuController.GetAsync(It.IsAny<int>());

            // Assert
            _cpuServiceMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()));
            var okResult = getCpuResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            var resultCpu = okResult.Value as CpuDto;
            resultCpu.Should().NotBeNull();
            okResult.Value.Should().BeOfType<CpuDto>();
            resultCpu.Id.Should().Be(testCpu.Id);
        }

        private CpuDto GetTestCpu()
        {
            return new CpuDto() { Id = 1 };
        }

        [Test]
        public async Task GetCpuAsync_CpuDoesNotExist_Returns_NotFoundResult()
        {
            // Arrange
            _cpuServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(null as CpuDto);

            // Act
            var result = await _cpuController.GetAsync(It.IsAny<int>());

            // Assert
            _cpuServiceMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()));
            result.Should().BeOfType<ActionResult<CpuDto>>();
            result.Value.Should().BeNull();
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task PutCpuAsync_CpuExists_Returns_NoContent()
        {
            // Arrange
            var testCpu = GetTestCpu();
            _cpuServiceMock.Setup(m => m.UpdateAsync(It.IsAny<CpuDto>())).ReturnsAsync(true);

            // Act
            var putCpuResult = await _cpuController.PutAsync(testCpu);

            // Assert
            _cpuServiceMock.Verify(x => x.UpdateAsync(It.IsAny<CpuDto>()));
            putCpuResult.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task PutCpuAsync_CpuDoesNotExist_Return_NotFound()
        {
            // Arrange
            var testCpu = GetTestCpu();
            _cpuServiceMock.Setup(m => m.UpdateAsync(testCpu)).ReturnsAsync(false);

            // Act
            var putCpuResult = await _cpuController.PutAsync(testCpu);

            // Assert
            _cpuServiceMock.Verify(x => x.UpdateAsync(testCpu));
            putCpuResult.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task DeleteCpuAsync_CpuExists_Returns_OkResult()
        {
            // Arrange
            _cpuServiceMock.Setup(m => m.RemoveAsync(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var deleteCpuResult = await _cpuController.DeleteAsync(It.IsAny<int>());

            // Assert
            _cpuServiceMock.Verify(x => x.RemoveAsync(It.IsAny<int>()), Times.AtLeastOnce);
            deleteCpuResult.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task DeleteCpuAsync_CpuDoesNotExist_Returns_NotFoundResult()
        {
            // Arrange
            _cpuServiceMock.Setup(m => m.RemoveAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var deleteCpuResult = await _cpuController.DeleteAsync(It.IsAny<int>());

            // Assert
            _cpuServiceMock.Verify(x => x.RemoveAsync(It.IsAny<int>()), Times.AtLeastOnce);
            deleteCpuResult.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task PostCpuAsync_Returns_CreatedAtActionResult()
        {
            // Arrange
            var testCpu = GetTestCpu();
            _cpuServiceMock.Setup(m => m.AddAsync(It.IsAny<CpuDto>())).ReturnsAsync(testCpu);

            // Act
            var createdAtActionResult = await _cpuController.PostAsync(It.IsAny<CpuDto>());
            var result = (CpuDto)((CreatedAtActionResult)createdAtActionResult.Result).Value;

            // Assert
            _cpuServiceMock.Verify(x => x.AddAsync(It.IsAny<CpuDto>()));
            result.Should().NotBeNull();
            result.Should().BeOfType<CpuDto>();
            createdAtActionResult.Result.Should().BeOfType<CreatedAtActionResult>();
            result.Should().BeEquivalentTo(testCpu, options => options.Excluding(a => a.Id));
        }
    }
}
