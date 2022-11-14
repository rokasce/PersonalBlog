using Bogus;
using FluentAssertions;
using System.Net.Http.Json;
using API.DTOs;
using API.Tests.Integration;
using System.Net;
using API.DTOs.Responses;

namespace API.Tests.Unit.Users;

public class CreateAccountControllerTests : IClassFixture<BlogApiFactory>
{
    private readonly HttpClient _httpClient;
    private readonly Faker<UserRegistrationRequest> _userGenerator = new Faker<UserRegistrationRequest>()
        .RuleFor(x => x.Email, faker => faker.Person.Email);

    public CreateAccountControllerTests(BlogApiFactory blogApiFactory)
    {
        _httpClient = blogApiFactory.CreateClient();
    }

    [Fact]
    public async Task Register_CreatesUser_WhenDataIsValid()
    {
        // Arrange
        var user = _userGenerator.Generate();
        user.Password = "Test1234!/";

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/account/register", user);

        // Assert
        var result = await response.Content.ReadFromJsonAsync<AuthSuccessResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().BeOfType<AuthSuccessResponse>();
    }

    [Fact]
    public async Task Register_ShouldNotCreateUser_WhenUserWithSameEmailExist()
    {
        // Arrange
        var user = new UserRegistrationRequest
        {
            Email = "bob@test.com",
            Password = "Test1234!."
        };

        // Act
        await _httpClient.PostAsJsonAsync("api/account/register", user);
        var createSecondUserResponse = await _httpClient.PostAsJsonAsync("api/account/register", user);

        // Assert
        var result = await createSecondUserResponse.Content.ReadFromJsonAsync<AuthFailedResponse>();

        createSecondUserResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result?.Errors.Should().HaveCount(1);
    }

    [Fact]
    public async Task Register_ShouldNotCreateUser_WhenPasswordDoesNotMeetRequirements()
    {
        // Arrange
        var user = _userGenerator.Generate();
        user.Password = "simple";

        // Act
        var createUserResponse = await _httpClient.PostAsJsonAsync("api/account/register", user);

        // Assert
        var result = await createUserResponse.Content.ReadFromJsonAsync<AuthFailedResponse>();

        createUserResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result?.Errors.Should().HaveCountGreaterThan(1);
    }
}