using System.Data;
using Moq;
using NFCEPS.Application.Helpers;
using NFCEPS.Application.Models.Auth.Response;
using NFCEPS.Application.Features.Auth.Commands.Login;
using NFCEPS.Application.Interfaces;

namespace NFCEPS_TEST.Auth.Services.Implementations;

public class AuthServiceTests
{
    private readonly Mock<IGenericRepository> _mockRepo;
    private readonly JWTHelper _jwtHelper;
    private readonly LoginCommandHandler _handler;

    public AuthServiceTests()
    {
        _mockRepo = new Mock<IGenericRepository>();

        var testSettings = new JWTSettings
        {
            SecretKey = "a_very_long_and_secure_secret_key_for_testing_32_bytes_long!!",
            Issuer = "test-issuer",
            Audience = "test-audience",
            ExpiryHours = 1
        };
        _jwtHelper = new JWTHelper(testSettings);

        _handler = new LoginCommandHandler(_mockRepo.Object, _jwtHelper, new Mock<MediatR.IMediator>().Object);
    }

    [Fact]
    public async Task LoginAsync_PasswordIsNull_ReturnsFailResponse()
    {
        var command = new LoginCommand { UserName = "testuser", Password = null };
        var result = await _handler.Handle(command, default);
        Assert.False(result.Success);
        Assert.Equal("Password cannot be empty!", result.Message);
    }

    [Fact]
    public async Task LoginAsync_UserNotFoundInDatabase_ReturnsInvalidCredentials()
    {
        var command = new LoginCommand { UserName = "ghost", Password = "Nothing@123" };

        _mockRepo.Setup(r => r.QueryFirstOrDefaultAsync<UserLoginRow>(
            It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CommandType>()
        )).ReturnsAsync((UserLoginRow)null);

        var result = await _handler.Handle(command, default);
        Assert.False(result.Success);
        Assert.Equal("Invalid username or password", result.Message);
    }

    [Fact]
    public async Task LoginAsync_LoginSuccess_ReturnsSuccessLogin()
    {
        string plainPassword = "archlinux";
        string validPassword = PasswordHelper.HashPassword(plainPassword);
        
        var command = new LoginCommand { UserName = "linux", Password = plainPassword };

        var fakeUserDatabaseRow = new UserLoginRow
        {
            UserId = 12,
            UserName = "linux",
            Name = "Arch BTW",
            Password = validPassword,
            IsActive = true,
            RoleId = 1,
            RoleName = "admin",
            CompressedPermissions = "CRD.C, CRD.R, CRD.U, CRD.D"
        };

        _mockRepo.Setup(r => r.QueryFirstOrDefaultAsync<UserLoginRow>(
            It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CommandType>()
        )).ReturnsAsync(fakeUserDatabaseRow);

        var expectedPermissions = new List<string> { "CRD.C", "CRD.R", "CRD.U", "CRD.D" };

        var result = await _handler.Handle(command, default);

        Assert.True(result.Success);
        Assert.NotNull(result.Data);

        var loginResponse = Assert.IsType<LoginResponse>(result.Data);

        Assert.Equal("linux", loginResponse.UserName);
        Assert.Equal("Arch BTW", loginResponse.Name);
        Assert.Equal("admin", loginResponse.RoleName);
        Assert.Equal(1, loginResponse.RoleId);
        Assert.Equal(expectedPermissions, loginResponse.Permissions);
        Assert.NotNull(loginResponse.Token);
    }
    
    [Fact]
    public async Task LoginAsync_InActiveAccount_ReturnsAccountInactive()
    {
        string plainPassword = "nirvana";
        string validPassword = PasswordHelper.HashPassword(plainPassword);

        var command = new LoginCommand { UserName = "admin", Password = plainPassword };

        var fakeUserDatabaseRow = new UserLoginRow
        {
            UserId = 1,
            Name = "Standard User",
            UserName = "admin",
            Password = validPassword,
            IsActive = false,
            RoleId = 2,
            RoleName = "standard",
            CompressedPermissions = "CRD.C, CRD.R, CRD.U, CRD.D"
        };

        _mockRepo.Setup(r => r.QueryFirstOrDefaultAsync<UserLoginRow>(
            It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CommandType>()
        )).ReturnsAsync(fakeUserDatabaseRow);

        var result = await _handler.Handle(command, default);

        Assert.False(result.Success);
        Assert.Null(result.Data);
        Assert.Equal("Account is inActive", result.Message);
    }
}
