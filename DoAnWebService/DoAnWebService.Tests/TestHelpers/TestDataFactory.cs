using DoAnWebService.Data;
using DoAnWebService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DoAnWebService.Tests.TestHelpers;

public static class TestDataFactory
{
    public static QLSVContext CreateContext(string? databaseName = null)
    {
        var options = new DbContextOptionsBuilder<QLSVContext>()
            .UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString())
            .Options;

        return new QLSVContext(options);
    }

    public static IConfiguration CreateJwtConfiguration()
    {
        var values = new Dictionary<string, string?>
        {
            ["Jwt:Issuer"] = "DoAnWebService",
            ["Jwt:Audience"] = "DoAnWebService",
            ["Jwt:Subject"] = "DoAnWebService",
            ["Jwt:ExpireMinutes"] = "15",
            ["Jwt:RefreshTokenExpireDays"] = "7",
            ["Jwt:Key"] = "QLSV_JWT_SECRET_KEY_2026_7fK9mP2xR8vL4qT6zN3bY1cW5eA0sD"
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();
    }

    public static User CreateHashedUser(string username, string password, string role)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Password = password,
            Role = role
        };

        user.Password = new Microsoft.AspNetCore.Identity.PasswordHasher<User>()
            .HashPassword(user, password);

        return user;
    }
}
