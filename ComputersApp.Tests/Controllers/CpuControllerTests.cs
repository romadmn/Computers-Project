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
            var testCpus = GetTestCpus();
            _cpuServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(testCpus);

            var getAllCpusResult = await _cpuController.GetAllAsync();

            var okResult = getAllCpusResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            var computers = okResult.Value as List<CpuDto>;
            computers.Should().HaveCount(testCpus.Count);
        }

        [Test]
        public async Task GetAllCpusAsync_CpusDoesNotExists_Returns_NotFoundResult()
        {
            _cpuServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(null as List<CpuDto>);

            var result = await _cpuController.GetAllAsync();

            result.Result.Should().BeOfType<NotFoundResult>();
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
            var testCpu = GetTestCpu();

            _cpuServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(testCpu);

            var getCpuResult = await _cpuController.GetAsync(It.IsAny<int>());

            var okResult = getCpuResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            var resultCpu = okResult.Value as CpuDto;
            resultCpu.Id.Should().Be(testCpu.Id);
        }

        private CpuDto GetTestCpu()
        {
            return new CpuDto() { Id = 1 };
        }

        [Test]
        public async Task GetCpuAsync_CpuDoesNotExist_Returns_NotFoundResult()
        {
            _cpuServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(null as CpuDto);

            var result = await _cpuController.GetAsync(It.IsAny<int>());

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task PutCpuAsync_CpuExists_Returns_NoContent()
        {
            var testCpu = GetTestCpu();
            _cpuServiceMock.Setup(m => m.UpdateAsync(It.IsAny<CpuDto>())).ReturnsAsync(true);

            var putCpuResult = await _cpuController.PutAsync(testCpu);

            putCpuResult.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task PutCpuAsync_CpuDoesNotExist_Return_NotFound()
        {
            var testCpu = GetTestCpu();
            _cpuServiceMock.Setup(m => m.UpdateAsync(It.IsAny<CpuDto>())).ReturnsAsync(false);

            var putCpuResult = await _cpuController.PutAsync(testCpu);

            putCpuResult.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task DeleteCpuAsync_CpuExists_Returns_OkResult()
        {
            _cpuServiceMock.Setup(m => m.RemoveAsync(It.IsAny<int>())).ReturnsAsync(true);

            var deleteCpuResult = await _cpuController.DeleteAsync(It.IsAny<int>());

            deleteCpuResult.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task DeleteCpuAsync_CpuDoesNotExist_Returns_NotFoundResult()
        {
            _cpuServiceMock.Setup(m => m.RemoveAsync(It.IsAny<int>())).ReturnsAsync(false);

            var deleteCpuResult = await _cpuController.DeleteAsync(It.IsAny<int>());

            deleteCpuResult.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task PostCpuAsync_Returns_CreatedAtActionResult()
        {
            var testCpu = GetTestCpu();
            _cpuServiceMock.Setup(m => m.AddAsync(It.IsAny<CpuDto>())).ReturnsAsync(testCpu);

            var createdAtActionResult = await _cpuController.PostAsync(It.IsAny<CpuDto>());
            var result = (CpuDto)((CreatedAtActionResult)createdAtActionResult.Result).Value;

            result.Should().BeOfType<CpuDto>();
            createdAtActionResult.Result.Should().BeOfType<CreatedAtActionResult>();
            result.Should().BeEquivalentTo(testCpu, options => options.Excluding(a => a.Id));
        }
    }
}
