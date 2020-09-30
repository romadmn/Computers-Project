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
using System.Text;
using System.Threading.Tasks;

namespace ComputersApp.Tests.Services
{
    [TestFixture]
    class ComputerServiceTests
    {
        private IComputerService _computerService;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private IMapper _mapper;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Application.MapperProfilers.ComputerProfiler());
            });
            _mapper = mappingConfig.CreateMapper();
            _computerService = new ComputerService(_unitOfWorkMock.Object, _mapper);
        }

        [TestCase(1)]
        public async Task GetByIdAsync_ComputerExists_Returns_ComputerDtoWithRequestedId(int computerId)
        {
            // Arrange
            var testComputers = GetTestComputers();
            _unitOfWorkMock.Setup(s => s.Repository<Computer>().Find(It.IsAny<ISpecification<Computer>>())).Returns(testComputers);

            // Act
            var computerResult = await _computerService.GetByIdAsync(computerId);

            // Assert
            _unitOfWorkMock.Verify(s => s.Repository<Computer>().Find(It.IsAny<ISpecification<Computer>>()));
            computerResult.Should().NotBeNull();
            computerResult.Should().BeOfType<ComputerDto>();
            computerResult.Id.Should().Be(testComputers[0].Id);
        }

        private Computer GetTestComputer()
        {
            return new Computer
            {
                Id = 1
            };
        }

        [Test]
        public async Task GetByIdAsync_ComputerDoesNotExist_Returns_Null()
        {
            // Arrange
            var computer = GetTestComputer();
            _unitOfWorkMock.Setup(s => s.Repository<Computer>().Find(It.IsAny<ISpecification<Computer>>())).Returns(new List<Computer>());

            // Act
            var computerResult = await _computerService.GetByIdAsync(1);

            // Assert
            _unitOfWorkMock.Verify(s => s.Repository<Computer>().Find(It.IsAny<ISpecification<Computer>>()));
            computerResult.Should().BeNull();
        }
        [Test]
        public async Task AddAsync_ComputerIsValid_Returns_ComputerDto()
        {
            // Arrange
            var computerDto = new ComputerDto();
            _unitOfWorkMock.Setup(s => s.Repository<Computer>().Add(It.IsAny<Computer>()));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var computerResult = await _computerService.AddAsync(computerDto);

            // Assert
            _unitOfWorkMock.Verify(s => s.Repository<Computer>().Add(It.IsAny<Computer>()));
            computerResult.Should().NotBeNull();
            computerResult.Should().BeOfType<ComputerDto>();
        }

        [Test]
        public async Task RemoveAsync_ComputerExists_Returns_True()
        {
            // Arrange
            var testComputer = GetTestComputer();
            _unitOfWorkMock.Setup(s => s.Repository<Computer>().FindById(It.IsAny<int>())).ReturnsAsync(testComputer);
            _unitOfWorkMock.Setup(s => s.Repository<Computer>().Remove(It.IsAny<Computer>()));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var deleteResult = await _computerService.RemoveAsync(1);

            // Assert
            _unitOfWorkMock.Verify(s => s.Repository<Computer>().FindById(It.IsAny<int>()));
            _unitOfWorkMock.Verify(s => s.Repository<Computer>().Remove(It.IsAny<Computer>()));
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync());
            deleteResult.Should().BeTrue();
        }

        [Test]
        public async Task RemoveAsync_ComputerDoesNotExist_Returns_False()
        {
            // Arrange
            _unitOfWorkMock.Setup(s => s.Repository<Computer>().FindById(It.IsAny<int>())).ReturnsAsync(null as Computer);
            _unitOfWorkMock.Setup(s => s.Repository<Computer>().Remove(It.IsAny<Computer>()));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

            // Act
            var deleteResult = await _computerService.RemoveAsync(1);

            // Assert
            _unitOfWorkMock.Verify(s => s.Repository<Computer>().FindById(It.IsAny<int>()));
            _unitOfWorkMock.Verify(s => s.Repository<Computer>().Remove(It.IsAny<Computer>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync());
            deleteResult.Should().BeFalse();
        }

        [Test]
        public async Task Update_ComputerExists_Returns_True()
        {
            // Arrange
            var testComputerDto = new ComputerDto();
            _unitOfWorkMock.Setup(s => s.Repository<Computer>().Update(It.IsAny<Computer>()));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _computerService.UpdateAsync(testComputerDto);

            // Assert
            _unitOfWorkMock.Verify(s => s.Repository<Computer>().Update(It.IsAny<Computer>()));
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync());
            result.Should().BeTrue();
        }

        [Test]
        public async Task Update_ComputerDoesNotExist_Returns_False()
        {
            // Arrange
            var testComputerDto = new ComputerDto();
            _unitOfWorkMock.Setup(s => s.Repository<Computer>().Update(It.IsAny<Computer>()));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

            // Act
            var result = await _computerService.UpdateAsync(testComputerDto);

            // Assert
            _unitOfWorkMock.Verify(s => s.Repository<Computer>().Update(It.IsAny<Computer>()));
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync());
            result.Should().BeFalse();
        }

        private List<Computer> GetTestComputers()
        {
            return new List<Computer>()
            {
                new Computer() { Id = 1 },
                new Computer() { Id = 2 }
            };
        }

        [Test]
        public async Task GetAll_ComputersExist_Returns_ListOfComputersWithSameCount()
        {
            // Arrange
            var testComputers = GetTestComputers();
            _unitOfWorkMock.Setup(s => s.Repository<Computer>().Find(It.IsAny<ISpecification<Computer>>())).Returns(testComputers);

            // Act
            var computersResult = await _computerService.GetAllAsync();

            // Assert
            _unitOfWorkMock.Verify(s => s.Repository<Computer>().Find(It.IsAny<ISpecification<Computer>>()));
            computersResult.Should().NotBeNullOrEmpty();
            computersResult.Should().BeOfType<List<ComputerDto>>();
            computersResult.Should().HaveCount(2);
            computersResult.Should().BeEquivalentTo(testComputers, options => options.Excluding(a => a.CpuId));
        }

        [Test]
        public async Task GetAll_ComputersDoesNotExist_Returns_Null()
        {
            // Arrange
            _unitOfWorkMock.Setup(s => s.Repository<Computer>().Find(It.IsAny<ISpecification<Computer>>())).Returns(new List<Computer>());

            // Act
            var computersResult = await _computerService.GetAllAsync();

            // Assert
            _unitOfWorkMock.Verify(s => s.Repository<Computer>().Find(It.IsAny<ISpecification<Computer>>()));
            computersResult.Should().BeOfType<List<ComputerDto>>();
            computersResult.Should().BeEmpty();
        }
    }
}
