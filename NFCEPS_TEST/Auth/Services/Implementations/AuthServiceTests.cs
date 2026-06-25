using System.Data;
using Moq;
using NFCEPS_API.Auth;
using NFCEPS_API.Auth.Models.Request;
using NFCEPS_API.Auth.Models.Response;
using NFCEPS_API.Repository.Interfaces;
using NFCEPS_API.Services.Implementaions;

namespace NFCEPS_TEST.Auth.Services.Implementations;

public class AuthServiceTests
{
    private readonly Mock<IGenericRepository> _mockRepo;
    private readonly JWTHelper _jwtHelper;
    private readonly AuthService _authService;

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

        _authService = new AuthService(_mockRepo.Object, _jwtHelper);
    }

    #region LoginAsync Tests

    [Fact]
    public async Task LoginAsync_PasswordIsNull_ReturnsFailResponse()
    {
        // Arrange
        var request = new LoginRequest { UserName = "testuser", Password = null };

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Password cannot be empty!", result.Message);
    }

    [Fact]
    public async Task LoginAsync_UserNotFoundInDatabase_ReturnsInvalidCredentials()
    {
        // Arrange
        var request = new LoginRequest { UserName = "ghost", Password = "Nothing@123"};

        _mockRepo.Setup(r => r.QueryFirstOrDefaultAsync<UserLoginRow>(
            It.IsAny<string>(),
            It.IsAny<object>(),
            It.IsAny<CommandType>()
        )).ReturnsAsync((UserLoginRow)null);

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Invalid username or password", result.Message);
    }

    [Fact]
    public async Task LoginAsync_LoginSuccess_ReturnsSuccessLogin()
    {
        // Arrange
        string plainPassword = "archlinux";

        string validPassword = PasswordHelper.HashPassword(plainPassword);
        
        var request = new LoginRequest {UserName = "linux", Password = plainPassword};

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
            It.IsAny<string>(),
            It.IsAny<object>(),
            It.IsAny<CommandType>()
        )).ReturnsAsync((fakeUserDatabaseRow));

        var expectedPermissions = new List<string> {"CRD.C", "CRD.R", "CRD.U", "CRD.D"};

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
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
        // Arrange
        string plainPassword = "nirvana";

        string validPassword = PasswordHelper.HashPassword(plainPassword);

        var request = new LoginRequest { UserName = "admin", Password = plainPassword };

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
            It.IsAny<string>(),
            It.IsAny<object>(),
            It.IsAny<CommandType>()
        )).ReturnsAsync(fakeUserDatabaseRow);

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Data);
        Assert.Equal("Account is inActive", result.Message);
    }
    #endregion
}
