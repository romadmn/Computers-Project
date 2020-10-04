using ComputersApp.Api.Controllers;
using ComputersApp.Application.DataTransferObjects;
using ComputersApp.Application.Services.Interfaces;
using ComputersApp.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ComputersApp.Tests.Controllers
{
    [TestFixture]
    class UserControllerTests
    {
        private Mock<ITokenService> _tokenServiceMock;
        private Mock<IUserService> _userServiceMock;
        private UserController _userController;

        [OneTimeSetUp]
        public void SetUp()
        {
            _tokenServiceMock = new Mock<ITokenService>();
            _userServiceMock = new Mock<IUserService>();
            _userController = new UserController(_tokenServiceMock.Object, _userServiceMock.Object);
        }

        private string GetTestToken()
        {
            return "TestJwtToken";
        }

        private string GetTestRefreshToken()
        {
            return "TestRefreshToken";
        }

        private User GetTestUser()
        {
            return new User
            {
                Id = 1,
                RefreshToken = "TestRefreshToken",
                RefreshTokenExpiryTime = DateTime.Now.AddDays(10)
            };
        }

        [Test]
        public async Task Revoke_UserFromPrincipalExists_Returns_NoContent()
        {
            // Arrange
            var testToken = GetTestToken();
            var principals = new ClaimsPrincipal();
            var testUser = GetTestUser();
            _tokenServiceMock.Setup(s => s.GetPrincipalFromExpiredToken(It.IsAny<string>())).Returns(principals);
            _userServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(testUser);
            _userServiceMock.Setup(s => s.UpdateAsync(It.IsAny<User>())).ReturnsAsync(true);

            // Act
            var result = await _userController.RevokeAsync(testToken);

            // Assert
            _tokenServiceMock.Verify(x => x.GetPrincipalFromExpiredToken(It.IsAny<string>()));
            _userServiceMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()));
            _userServiceMock.Verify(x => x.UpdateAsync(It.IsAny<User>()));
            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task Revoke_UserFromPrincipalDoesNotExist_Returns_BadRequest()
        {
            // Arrange
            var testToken = GetTestToken();
            var principals = new ClaimsPrincipal();
            var testUser = null as User;
            _tokenServiceMock.Setup(s => s.GetPrincipalFromExpiredToken(It.IsAny<string>())).Returns(principals);
            _userServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(testUser);

            // Act
            var result = await _userController.RevokeAsync(testToken);

            // Assert
            _tokenServiceMock.Verify(x => x.GetPrincipalFromExpiredToken(It.IsAny<string>()));
            _userServiceMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()));
            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task Authenticate_LoginDtoIsNull_Returns_BadRequest()
        {
            // Arrange
            var loginDto = null as LoginDto;

            // Act
            var result = await _userController.AuthenticateAsync(loginDto);

            // Assert
            result.Should().BeOfType<ActionResult<UserTokenDto>>();
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            result.Value.Should().BeNull();
        }

        [Test]
        public async Task Authenticate_UserDoesNotExist_Returns_Unauthorized()
        {
            // Arrange
            var loginDto = new LoginDto();
            var testUser = null as User;
            _tokenServiceMock.Setup(x => x.VerifyUserCredentialsAsync(It.IsAny<LoginDto>())).Returns(testUser);

            // Act
            var result = await _userController.AuthenticateAsync(loginDto);

            // Assert
            _tokenServiceMock.Verify(x => x.VerifyUserCredentialsAsync(It.IsAny<LoginDto>()));
            result.Should().BeOfType<ActionResult<UserTokenDto>>();
            result.Result.Should().BeOfType<UnauthorizedResult>();
            result.Value.Should().BeNull();
        }

        [Test]
        public async Task Authenticate_UserExist_Returns_UserTokenDto()
        {
            // Arrange
            var loginDto = new LoginDto();
            var testUser = GetTestUser();
            var testJwtToken = GetTestToken();
            var testRefreshToken = GetTestRefreshToken();
            _tokenServiceMock.Setup(x => x.VerifyUserCredentialsAsync(It.IsAny<LoginDto>())).Returns(testUser);
            _tokenServiceMock.Setup(x => x.GenerateAccessToken(It.IsAny<User>())).Returns(testJwtToken);
            _tokenServiceMock.Setup(x => x.GenerateRefreshToken()).Returns(testRefreshToken);
            _userServiceMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(true);
            // Act
            var result = await _userController.AuthenticateAsync(loginDto);

            // Assert
            _tokenServiceMock.Verify(x => x.VerifyUserCredentialsAsync(It.IsAny<LoginDto>()));
            _tokenServiceMock.Verify(x => x.GenerateAccessToken(It.IsAny<User>()));
            _tokenServiceMock.Verify(x => x.GenerateRefreshToken());
            _userServiceMock.Verify(x => x.UpdateAsync(It.IsAny<User>()));
            result.Should().BeOfType<ActionResult<UserTokenDto>>();
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task Refresh_TokenDtoIsNull_Returns_BadRequest()
        {
            // Arrange
            var tokenDto = null as TokenDto;

            // Act
            var result = await _userController.RefreshAsync(tokenDto);

            // Assert
            result.Should().BeOfType<ActionResult<UserTokenDto>>();
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            result.Value.Should().BeNull();
        }

        [Test]
        public async Task Refresh_UserDoesNotExist_Returns_Unauthorized()
        {
            // Arrange
            var tokenDto = new TokenDto() { JWT = GetTestToken(), RefreshToken = GetTestRefreshToken() };
            var principals = new ClaimsPrincipal();
            var testUser = null as User;
            _tokenServiceMock.Setup(x => x.GetPrincipalFromExpiredToken(It.IsAny<string>())).Returns(principals);
            _userServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(testUser);

            // Act
            var result = await _userController.RefreshAsync(tokenDto);

            // Assert
            _tokenServiceMock.Verify(x => x.VerifyUserCredentialsAsync(It.IsAny<LoginDto>()));
            _userServiceMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()));
            result.Should().BeOfType<ActionResult<UserTokenDto>>();
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            result.Value.Should().BeNull();
        }

        [Test]
        public async Task Refresh_UserExist_Returns_UserTokenDto()
        {
            // Arrange
            var tokenDto = new TokenDto() { JWT = GetTestToken(), RefreshToken = GetTestRefreshToken() };
            var principals = new ClaimsPrincipal();
            var testUser = GetTestUser();
            var testJwtToken = GetTestToken();
            var testRefreshToken = GetTestRefreshToken();
            _tokenServiceMock.Setup(x => x.GetPrincipalFromExpiredToken(It.IsAny<string>())).Returns(principals);
            _tokenServiceMock.Setup(x => x.GenerateAccessToken(It.IsAny<User>())).Returns(testJwtToken);
            _tokenServiceMock.Setup(x => x.GenerateRefreshToken()).Returns(testRefreshToken);
            _userServiceMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(true);
            _userServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(testUser);
            // Act
            var result = await _userController.RefreshAsync(tokenDto);

            // Assert
            _tokenServiceMock.Verify(x => x.VerifyUserCredentialsAsync(It.IsAny<LoginDto>()));
            _tokenServiceMock.Verify(x => x.GenerateAccessToken(It.IsAny<User>()));
            _tokenServiceMock.Verify(x => x.GenerateRefreshToken());
            _userServiceMock.Verify(x => x.UpdateAsync(It.IsAny<User>()));
            _userServiceMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()));
            result.Should().BeOfType<ActionResult<UserTokenDto>>();
            result.Result.Should().BeOfType<OkObjectResult>();
        }
    }
}
