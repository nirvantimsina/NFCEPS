using ErrorOr;
using Moq;
using NFCEPS.Application.Features.Auth.Commands.Login;
using NFCEPS.Application.Features.MenuSetup.Queries.GetMenuList;
using NFCEPS.Application.Helpers;
using NFCEPS.Application.Interfaces;
using NFCEPS.Application.Models.Auth.Response;
using NFCEPS.Shared.Wrappers;
using System.Data;

namespace NFCEPS.TEST.Application.Features.Auth.Commands;

public class LoginCommandTests
{
    private readonly Mock<IGenericRepository> _mockRepo;
    private readonly Mock<MediatR.IMediator> _mockMediator;
    private readonly JWTHelper _jwtHelper;
    private readonly LoginCommandHandler _handler;

    public LoginCommandTests()
    {
        _mockRepo = new Mock<IGenericRepository>();
        _mockMediator = new Mock<MediatR.IMediator>();

        var testSettings = new JWTSettings
        {
            SecretKey = "a_very_long_and_secure_secret_key_for_testing_32_bytes_long!!",
            Issuer = "test-issuer",
            Audience = "test-audience",
            ExpiryHours = 1
        };
        _jwtHelper = new JWTHelper(testSettings);

        _handler = new LoginCommandHandler(_mockRepo.Object, _jwtHelper, _mockMediator.Object);
    }

    [Fact]
    public async Task LoginAsync_PasswordIsNull_ReturnsFailResponse()
    {
        // arrange
        var command = new LoginCommand { UserName = "testuser", Password = null };

        // act
        var result = await _handler.Handle(command, default);

        // assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Validation, result.FirstError.Type);
        Assert.Equal(ErrorCodes.MissingRequiredField, result.FirstError.Code);
        Assert.Equal("Password cannot be empty!", result.FirstError.Description);
    }

    [Fact]
    public async Task LoginAsync_UserNotFoundInDatabase_ReturnsInvalidCredentials()
    {
        // arrange
        var command = new LoginCommand { UserName = "ghost", Password = "Nothing@123" };

        _mockRepo.Setup(r => r.QueryFirstOrDefaultAsync<UserLoginRow>(
            It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CommandType>()
        )).ReturnsAsync((UserLoginRow)null!);

        // act
        var result = await _handler.Handle(command, default);

        // assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Validation, result.FirstError.Type);
        Assert.Equal(ErrorCodes.InvalidCredentials, result.FirstError.Code);
        Assert.Equal("Invalid username or password", result.FirstError.Description);
    }

    [Fact]
    public async Task LoginAsync_LoginSuccess_ReturnsSuccessLogin()
    {
        // arrange
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

        var fakeMenus = new List<MenuListResponseModel>();
        _mockMediator.Setup(m => m.Send(It.IsAny<GetMenuListQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ErrorOrFactory.From(fakeMenus));

        var expectedPermissions = new List<string> { "CRD.C", "CRD.R", "CRD.U", "CRD.D" };

        // act
        var result = await _handler.Handle(command, default);

        // assert
        Assert.False(result.IsError);

        var loginResponse = result.Value;

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
        // arrange
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

        // act
        var result = await _handler.Handle(command, default);

        // assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Validation, result.FirstError.Type);
        Assert.Equal(ErrorCodes.AccountInactive, result.FirstError.Code);
        Assert.Equal("Account is inactive", result.FirstError.Description); // Normalized capitalization match
    }
}
