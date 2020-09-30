using AutoMapper;
using ComputersApp.Application.DataTransferObjects;
using ComputersApp.Application.Services.Implementation;
using ComputersApp.Application.Services.Interfaces;
using ComputersApp.Domain;
using ComputersApp.Domain.Entities;
using ComputersApp.Infrastructure.Specifications;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputersApp.Tests.Services
{
    [TestFixture]
    class CpuServiceTests
    {
        private ICpuService _cpuService;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private IMapper _mapper;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Application.MapperProfilers.CpuProfiler());
            });
            _mapper = mappingConfig.CreateMapper();
            _cpuService = new CpuService(_unitOfWorkMock.Object, _mapper);
        }

        [TestCase(1)]
        public async Task GetByIdAsync_CpuExists_Returns_CpuDtoWithRequestedId(int cpuId)
        {
            // Arrange
            var testCpu = GetTestCpu();
            _unitOfWorkMock.Setup(s => s.Repository<Cpu>().FindById(It.IsAny<int>())).ReturnsAsync(testCpu);

            // Act
            var cpuResult = await _cpuService.GetByIdAsync(cpuId);

            // Assert
            _unitOfWorkMock.Verify(s => s.Repository<Cpu>().FindById(It.IsAny<int>()));
            cpuResult.Should().NotBeNull();
            cpuResult.Should().BeOfType<CpuDto>();
            cpuResult.Id.Should().Be(1);
        }

        private Cpu GetTestCpu()
        {
            return new Cpu
            {
                Id = 1
            };
        }

        [Test]
        public async Task GetByIdAsync_CpuDoesNotExist_Returns_Null()
        {
            // Arrange
            var cpu = GetTestCpu();
            _unitOfWorkMock.Setup(s => s.Repository<Cpu>().FindById(It.IsAny<int>())).ReturnsAsync(null as Cpu);

            // Act
            var cpuResult = await _cpuService.GetByIdAsync(-1);

            // Assert
            _unitOfWorkMock.Verify(s => s.Repository<Cpu>().FindById(It.IsAny<int>()));
            cpuResult.Should().BeNull();
        }
        [Test]
        public async Task AddAsync_CpuIsValid_Returns_CpuDto()
        {
            // Arrange
            var cpuDto = new CpuDto();
            _unitOfWorkMock.Setup(s => s.Repository<Cpu>().Add(It.IsAny<Cpu>()));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var cpuResult = await _cpuService.AddAsync(cpuDto);

            // Assert
            _unitOfWorkMock.Verify(s => s.Repository<Cpu>().Add(It.IsAny<Cpu>()));
            cpuResult.Should().NotBeNull();
            cpuResult.Should().BeOfType<CpuDto>();
        }

        [Test]
        public async Task RemoveAsync_CpuExists_Returns_True()
        {
            // Arrange
            var testCpu = GetTestCpu();
            _unitOfWorkMock.Setup(s => s.Repository<Cpu>().FindById(It.IsAny<int>())).ReturnsAsync(testCpu);
            _unitOfWorkMock.Setup(s => s.Repository<Cpu>().Remove(It.IsAny<Cpu>()));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var deleteResult = await _cpuService.RemoveAsync(1);

            // Assert
            _unitOfWorkMock.Verify(s => s.Repository<Cpu>().FindById(It.IsAny<int>()));
            _unitOfWorkMock.Verify(s => s.Repository<Cpu>().Remove(It.IsAny<Cpu>()));
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync());
            deleteResult.Should().BeTrue();
        }

        [Test]
        public async Task RemoveAsync_CpuDoesNotExist_Returns_False()
        {
            // Arrange
            _unitOfWorkMock.Setup(s => s.Repository<Cpu>().FindById(It.IsAny<int>())).ReturnsAsync(null as Cpu);
            _unitOfWorkMock.Setup(s => s.Repository<Cpu>().Remove(It.IsAny<Cpu>()));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

            // Act
            var deleteResult = await _cpuService.RemoveAsync(1);

            // Assert
            _unitOfWorkMock.Verify(s => s.Repository<Cpu>().FindById(It.IsAny<int>()));
            _unitOfWorkMock.Verify(s => s.Repository<Cpu>().Remove(It.IsAny<Cpu>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync());
            deleteResult.Should().BeFalse();
        }

        [Test]
        public async Task Update_CpuExists_Returns_True()
        {
            // Arrange
            var testCpuDto = new CpuDto();
            _unitOfWorkMock.Setup(s => s.Repository<Cpu>().Update(It.IsAny<Cpu>()));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _cpuService.UpdateAsync(testCpuDto);

            // Assert
            _unitOfWorkMock.Verify(s => s.Repository<Cpu>().Update(It.IsAny<Cpu>()));
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync());
            result.Should().BeTrue();
        }

        [Test]
        public async Task Update_CpuDoesNotExist_Returns_False()
        {
            // Arrange
            var testCpuDto = new CpuDto();
            _unitOfWorkMock.Setup(s => s.Repository<Cpu>().Update(It.IsAny<Cpu>()));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

            // Act
            var result = await _cpuService.UpdateAsync(testCpuDto);

            // Assert
            _unitOfWorkMock.Verify(s => s.Repository<Cpu>().Update(It.IsAny<Cpu>()));
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync());
            result.Should().BeFalse();
        }

        private List<Cpu> GetTestCpus()
        {
            return new List<Cpu>()
            {
                new Cpu() { Id = 1, Name = "Intel", CorsAmount = 4, Frequency = 3600},
                new Cpu() { Id = 2, Name = "AMD", CorsAmount = 2, Frequency = 1800}
            };
        }

        [Test]
        public async Task GetAll_CpusExist_Returns_ListOfCpusWithSameCount()
        {
            // Arrange
            var testCpus = GetTestCpus();
            _unitOfWorkMock.Setup(s => s.Repository<Cpu>().Find(null)).Returns(testCpus);

            // Act
            var cpusResult = await _cpuService.GetAllAsync();

            // Assert
            _unitOfWorkMock.Verify(s => s.Repository<Cpu>().Find(null));
            cpusResult.Should().NotBeNullOrEmpty();
            cpusResult.Should().BeOfType<List<CpuDto>>();
            cpusResult.Should().HaveCount(2);
            cpusResult.Should().BeEquivalentTo(testCpus, options => options.Excluding(a => a.Computers));
        }

        [Test]
        public async Task GetAll_CpusDoesNotExist_Returns_Null()
        {
            // Arrange
            _unitOfWorkMock.Setup(s => s.Repository<Cpu>().Find(null)).Returns(new List<Cpu>());

            // Act
            var cpusResult = await _cpuService.GetAllAsync();

            // Assert
            _unitOfWorkMock.Verify(s => s.Repository<Cpu>().Find(null));
            cpusResult.Should().BeOfType<List<CpuDto>>();
            cpusResult.Should().BeEmpty();
        }
    }
}
