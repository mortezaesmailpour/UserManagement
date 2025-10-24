using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using UserApi.Dtos;

namespace UserApi.Tests;

public class UserEndpointsTests
{
    private readonly HttpClient _client;

    public UserEndpointsTests()
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7218/")
        };
    }

    [Fact]
    public async Task PostUser_ShouldReturnCreated_AndSendEmail()
    {
        // Arrange
        var userRequest = new
        {
            Name = "Test User",
            Email = $"test{Guid.NewGuid()}@example.com",
            Password = "Password1!"
        };

        // Act
        var response = await _client.PostAsJsonAsync("users", userRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdUser = await response.Content.ReadFromJsonAsync<ReadUserDto>();
        createdUser.Should().NotBeNull();
        createdUser!.Name.Should().Be(userRequest.Name);
        createdUser.Email.Should().Be(userRequest.Email);
    }

    [Fact]
    public async Task GetUser_ShouldReturnUser_WhenExists()
    {
        // Arrange
        var userRequest = new
        {
            Name = "Another User",
            Email = $"user{Guid.NewGuid()}@example.com",
            Password = "Password1!"
        };

        // Create the user first
        var createResponse = await _client.PostAsJsonAsync("users", userRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdUser = await createResponse.Content.ReadFromJsonAsync<ReadUserDto>();
        createdUser.Should().NotBeNull();

        // Act
        var getResponse = await _client.GetAsync($"users/{createdUser!.Id}");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var fetchedUser = await getResponse.Content.ReadFromJsonAsync<ReadUserDto>();
        fetchedUser.Should().NotBeNull();
        fetchedUser!.Name.Should().Be(userRequest.Name);
        fetchedUser.Email.Should().Be(userRequest.Email);
    }

    [Fact]
    public async Task GetUser_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var invalidUserId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"users/{invalidUserId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    

    [Theory]
    [InlineData("", "valid@test.com", "Password1!", "Name is required")]
    [InlineData("Valid Name", "not-an-email", "Password1!", "Invalid email")]
    [InlineData("Valid Name", "valid@test.com", "short", "Password too short")]
    [InlineData("Valid Name", "valid@test.com", "averyveryverylongpassword", "Password too long")]
    public async Task PostUser_ShouldReturnBadRequest_WhenValidationFails(string name, string email, string password, string caseDescription)
    {
        // Arrange
        var userRequest = new { Name = name, Email = email, Password = password };

        // Act
        var response = await _client.PostAsJsonAsync("users", userRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest, because: caseDescription);

        var errorBody = await response.Content.ReadAsStringAsync();
        errorBody.Should().NotBeNullOrEmpty();

        // Optional: check some validation message content
        errorBody.Should().ContainAny("Name", "Email", "Password");
    }

    [Fact]
    public async Task PostUser_ShouldReturnBadRequest_WhenEmailAlreadyExists()
    {
        // Arrange
        var email = $"duplicate{Guid.NewGuid()}@example.com";

        var firstUser = new { Name = "User1", Email = email, Password = "Password1!" };
        var secondUser = new { Name = "User2", Email = email, Password = "Password1!" };

        // Act
        var firstResponse = await _client.PostAsJsonAsync("users", firstUser);
        firstResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var secondResponse = await _client.PostAsJsonAsync("users", secondUser);

        // Assert
        secondResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorBody = await secondResponse.Content.ReadAsStringAsync();
        errorBody.Should().Contain("Email");
    }
}