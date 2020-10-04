using ComputersApp.Application.Services.Implementation;
using ComputersApp.Application.Services.Interfaces;
using ComputersApp.Domain;
using ComputersApp.Domain.Entities;
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
    class UserServiceTests
    {
        private IUserService _userService;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userService = new UserService(_unitOfWorkMock.Object);
        }

        [TestCase(1)]
        public async Task GetByIdAsync_UserExists_Returns_UserWithRequestedId(int userId)
        {
            // Arrange
            var testUser = GetTestUser();
            _unitOfWorkMock.Setup(s => s.Repository<User>().FindById(It.IsAny<int>())).ReturnsAsync(testUser);

            // Act
            var userResult = await _userService.GetByIdAsync(userId);

            // Assert
            _unitOfWorkMock.Verify(s => s.Repository<User>().FindById(It.IsAny<int>()));
            userResult.Should().NotBeNull();
            userResult.Should().BeOfType<User>();
            userResult.Id.Should().Be(testUser.Id);
        }

        private User GetTestUser()
        {
            return new User
            {
                Id = 1
            };
        }

        [Test]
        public async Task GetByIdAsync_UserDoesNotExist_Returns_Null()
        {
            // Arrange
            var testUser = GetTestUser();
            _unitOfWorkMock.Setup(s => s.Repository<User>().FindById(It.IsAny<int>())).ReturnsAsync(null as User);

            // Act
            var userResult = await _userService.GetByIdAsync(testUser.Id);

            // Assert
            _unitOfWorkMock.Verify(s => s.Repository<User>().FindById(It.IsAny<int>()));
            userResult.Should().BeNull();
        }

        [Test]
        public async Task Update_UserExists_Returns_True()
        {
            // Arrange
            var testUser = new User();
            _unitOfWorkMock.Setup(s => s.Repository<User>().Update(It.IsAny<User>()));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _userService.UpdateAsync(testUser);

            // Assert
            _unitOfWorkMock.Verify(s => s.Repository<User>().Update(It.IsAny<User>()));
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync());
            result.Should().BeTrue();
        }

        [Test]
        public async Task Update_UserDoesNotExist_Returns_False()
        {
            // Arrange
            var testUser = new User();
            _unitOfWorkMock.Setup(s => s.Repository<User>().Update(It.IsAny<User>()));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

            // Act
            var result = await _userService.UpdateAsync(testUser);

            // Assert
            _unitOfWorkMock.Verify(s => s.Repository<User>().Update(It.IsAny<User>()));
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync());
            result.Should().BeFalse();
        }
    }
}
