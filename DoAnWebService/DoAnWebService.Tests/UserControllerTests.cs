using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DoAnWebService.Controllers;
using DoAnWebService.DTO.User;
using DoAnWebService.Models;
using DoAnWebService.Tests.TestHelpers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace DoAnWebService.Tests;

public class UserControllerTests
{
    [Fact]
    public async Task Login_ReturnsBadRequest_WhenUsernameOrPasswordIsEmpty()
    {
        await using var context = TestDataFactory.CreateContext();
        var controller = new UserController(context, TestDataFactory.CreateJwtConfiguration());

        var result = await controller.Login(new LoginRequestDTO { Username = string.Empty, Password = string.Empty });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenAccountDoesNotExist()
    {
        await using var context = TestDataFactory.CreateContext();
        var controller = new UserController(context, TestDataFactory.CreateJwtConfiguration());

        var result = await controller.Login(new LoginRequestDTO { Username = "missing", Password = "123" });

        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task Login_ReturnsTokens_AndStoresRefreshToken_WhenPasswordIsCorrect()
    {
        await using var context = TestDataFactory.CreateContext();
        var user = TestDataFactory.CreateHashedUser("admin01", "P@ssw0rd123", "Admin");
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var controller = new UserController(context, TestDataFactory.CreateJwtConfiguration());

        var result = await controller.Login(new LoginRequestDTO
        {
            Username = "admin01",
            Password = "P@ssw0rd123"
        });

        var ok = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<DoAnWebService.Utlis.APIResponse<LoginResponseDTO>>(ok.Value);
        Assert.Equal("Đăng nhập thành công.", response.Message);
        Assert.NotNull(response.Data);
        Assert.False(string.IsNullOrWhiteSpace(response.Data!.AccessToken));
        Assert.False(string.IsNullOrWhiteSpace(response.Data.RefreshToken));
        Assert.Equal("Admin", response.Data.Role);

        var saved = context.Users.Single(x => x.Username == "admin01");
        Assert.Equal(response.Data.RefreshToken, saved.Refreshtoken);
        Assert.True(saved.Expiry.HasValue);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenPasswordIsWrong()
    {
        await using var context = TestDataFactory.CreateContext();
        context.Users.Add(TestDataFactory.CreateHashedUser("admin01", "P@ssw0rd123", "Admin"));
        await context.SaveChangesAsync();

        var controller = new UserController(context, TestDataFactory.CreateJwtConfiguration());

        var result = await controller.Login(new LoginRequestDTO
        {
            Username = "admin01",
            Password = "wrong-password"
        });

        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task RefreshToken_ReturnsNewTokens_WhenRefreshTokenIsValid()
    {
        await using var context = TestDataFactory.CreateContext();
        context.Users.Add(TestDataFactory.CreateHashedUser("admin01", "P@ssw0rd123", "Admin"));
        await context.SaveChangesAsync();

        var controller = new UserController(context, TestDataFactory.CreateJwtConfiguration());

        var loginResult = await controller.Login(new LoginRequestDTO
        {
            Username = "admin01",
            Password = "P@ssw0rd123"
        });

        var loginOk = Assert.IsType<OkObjectResult>(loginResult);
        var loginResponse = Assert.IsType<DoAnWebService.Utlis.APIResponse<LoginResponseDTO>>(loginOk.Value);
        var loginData = loginResponse.Data!;

        var refreshResult = await controller.RefreshToken(loginData);
        var refreshOk = Assert.IsType<OkObjectResult>(refreshResult);
        var refreshResponse = Assert.IsType<DoAnWebService.Utlis.APIResponse<LoginResponseDTO>>(refreshOk.Value);

        Assert.NotNull(refreshResponse.Data);
        Assert.NotEqual(loginData.AccessToken, refreshResponse.Data!.AccessToken);
        Assert.NotEqual(loginData.RefreshToken, refreshResponse.Data.RefreshToken);

        var saved = context.Users.Single(x => x.Username == "admin01");
        Assert.Equal(refreshResponse.Data.RefreshToken, saved.Refreshtoken);
    }

    [Fact]
    public async Task Logout_ClearsRefreshToken_WhenTokenExists()
    {
        await using var context = TestDataFactory.CreateContext();
        context.Users.Add(TestDataFactory.CreateHashedUser("admin01", "P@ssw0rd123", "Admin"));
        await context.SaveChangesAsync();

        var controller = new UserController(context, TestDataFactory.CreateJwtConfiguration());

        var loginResult = await controller.Login(new LoginRequestDTO
        {
            Username = "admin01",
            Password = "P@ssw0rd123"
        });

        var loginOk = Assert.IsType<OkObjectResult>(loginResult);
        var loginResponse = Assert.IsType<DoAnWebService.Utlis.APIResponse<LoginResponseDTO>>(loginOk.Value);
        var refreshToken = loginResponse.Data!.RefreshToken;

        var logoutResult = await controller.Logout(new LoginResponseDTO { RefreshToken = refreshToken });

        Assert.IsType<OkObjectResult>(logoutResult);
        var saved = context.Users.Single(x => x.Username == "admin01");
        Assert.Null(saved.Refreshtoken);
        Assert.True(saved.Expiry <= DateTime.UtcNow);
    }

    [Fact]
    public async Task RefreshToken_ReturnsBadRequest_WhenInputIsMissing()
    {
        await using var context = TestDataFactory.CreateContext();
        var controller = new UserController(context, TestDataFactory.CreateJwtConfiguration());

        var result = await controller.RefreshToken(new LoginResponseDTO());

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task RefreshToken_ReturnsUnauthorized_WhenAccessTokenIsInvalid()
    {
        await using var context = TestDataFactory.CreateContext();
        var controller = new UserController(context, TestDataFactory.CreateJwtConfiguration());

        var result = await controller.RefreshToken(new LoginResponseDTO
        {
            AccessToken = "invalid-token",
            RefreshToken = "refresh"
        });

        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task RefreshToken_ReturnsUnauthorized_WhenRefreshTokenDoesNotMatch()
    {
        await using var context = TestDataFactory.CreateContext();
        context.Users.Add(TestDataFactory.CreateHashedUser("admin01", "P@ssw0rd123", "Admin"));
        await context.SaveChangesAsync();

        var controller = new UserController(context, TestDataFactory.CreateJwtConfiguration());

        var loginResult = await controller.Login(new LoginRequestDTO
        {
            Username = "admin01",
            Password = "P@ssw0rd123"
        });

        var loginOk = Assert.IsType<OkObjectResult>(loginResult);
        var loginResponse = Assert.IsType<DoAnWebService.Utlis.APIResponse<LoginResponseDTO>>(loginOk.Value);

        var result = await controller.RefreshToken(new LoginResponseDTO
        {
            AccessToken = loginResponse.Data!.AccessToken,
            RefreshToken = "wrong-refresh-token"
        });

        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task RefreshToken_ReturnsUnauthorized_WhenRefreshTokenExpired()
    {
        await using var context = TestDataFactory.CreateContext();
        context.Users.Add(TestDataFactory.CreateHashedUser("admin01", "P@ssw0rd123", "Admin"));
        await context.SaveChangesAsync();

        var controller = new UserController(context, TestDataFactory.CreateJwtConfiguration());

        var loginResult = await controller.Login(new LoginRequestDTO
        {
            Username = "admin01",
            Password = "P@ssw0rd123"
        });

        var loginOk = Assert.IsType<OkObjectResult>(loginResult);
        var loginResponse = Assert.IsType<DoAnWebService.Utlis.APIResponse<LoginResponseDTO>>(loginOk.Value);

        var saved = context.Users.Single(x => x.Username == "admin01");
        saved.Expiry = DateTime.UtcNow.AddMinutes(-1);
        await context.SaveChangesAsync();

        var result = await controller.RefreshToken(new LoginResponseDTO
        {
            AccessToken = loginResponse.Data!.AccessToken,
            RefreshToken = loginResponse.Data.RefreshToken
        });

        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task Logout_ReturnsBadRequest_WhenRefreshTokenIsEmpty()
    {
        await using var context = TestDataFactory.CreateContext();
        var controller = new UserController(context, TestDataFactory.CreateJwtConfiguration());

        var result = await controller.Logout(new LoginResponseDTO { RefreshToken = string.Empty });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Logout_ReturnsNotFound_WhenRefreshTokenNotFound()
    {
        await using var context = TestDataFactory.CreateContext();
        var controller = new UserController(context, TestDataFactory.CreateJwtConfiguration());

        var result = await controller.Logout(new LoginResponseDTO { RefreshToken = "missing" });

        Assert.IsType<NotFoundObjectResult>(result);
    }
}
