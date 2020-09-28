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
            var testComputers = GetTestComputers();
            _unitOfWorkMock.Setup(s => s.Repository<Computer>().Find(It.IsAny<ISpecification<Computer>>())).Returns(testComputers);

            var computerResult = await _computerService.GetByIdAsync(computerId);

            computerResult.Should().BeOfType<ComputerDto>();
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
            var computer = GetTestComputer();
            _unitOfWorkMock.Setup(s => s.Repository<Computer>().Find(It.IsAny<ISpecification<Computer>>())).Returns(new List<Computer>());

            var computerResult = await _computerService.GetByIdAsync(1);

            computerResult.Should().BeNull();
        }
        [Test]
        public async Task AddAsync_ComputerIsValid_Returns_ComputerDto()
        {
            var computerDto = new ComputerDto();
            _unitOfWorkMock.Setup(s => s.Repository<Computer>().Add(It.IsAny<Computer>()));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var computerResult = await _computerService.AddAsync(computerDto);

            computerResult.Should().BeOfType<ComputerDto>();
        }

        [Test]
        public async Task RemoveAsync_ComputerExists_Returns_True()
        {
            var testComputer = GetTestComputer();
            _unitOfWorkMock.Setup(s => s.Repository<Computer>().FindById(It.IsAny<int>())).ReturnsAsync(testComputer);
            _unitOfWorkMock.Setup(s => s.Repository<Computer>().Remove(It.IsAny<Computer>()));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var deleteResult = await _computerService.RemoveAsync(1);

            deleteResult.Should().BeTrue();
        }

        [Test]
        public async Task RemoveAsync_ComputerDoesNotExist_Returns_False()
        {
            _unitOfWorkMock.Setup(s => s.Repository<Computer>().FindById(It.IsAny<int>())).ReturnsAsync(null as Computer);
            _unitOfWorkMock.Setup(s => s.Repository<Computer>().Remove(It.IsAny<Computer>()));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

            var deleteResult = await _computerService.RemoveAsync(1);

            deleteResult.Should().BeFalse();
        }

        [Test]
        public async Task Update_ComputerExists_Returns_True()
        {
            var testComputerDto = new ComputerDto();
            _unitOfWorkMock.Setup(s => s.Repository<Computer>().Update(It.IsAny<Computer>()));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _computerService.UpdateAsync(testComputerDto);

            result.Should().BeTrue();
        }

        [Test]
        public async Task Update_ComputerDoesNotExist_Returns_False()
        {
            var testComputerDto = new ComputerDto();
            _unitOfWorkMock.Setup(s => s.Repository<Computer>().Update(It.IsAny<Computer>()));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

            var result = await _computerService.UpdateAsync(testComputerDto);

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
            var testComputers = GetTestComputers();
            _unitOfWorkMock.Setup(s => s.Repository<Computer>().Find(It.IsAny<ISpecification<Computer>>())).Returns(testComputers);

            var computersResult = await _computerService.GetAllAsync();

            computersResult.Should().BeOfType<List<ComputerDto>>();
            computersResult.Should().HaveCount(2);
        }

        [Test]
        public async Task GetAll_ComputersDoesNotExist_Returns_Null()
        {
            _unitOfWorkMock.Setup(s => s.Repository<Computer>().Find(null)).Returns(new List<Computer>());

            var computersResult = await _computerService.GetAllAsync();

            computersResult.Should().BeEmpty();
        }
    }
}
