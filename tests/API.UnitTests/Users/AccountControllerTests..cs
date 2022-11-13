using Bogus;
using FluentAssertions;
using System.Net.Http.Json;
using API.DTOs;
using API.Tests.Integration;


namespace API.Tests.Unit.Users;

public class CreateAccountControllerTests : IClassFixture<BlogApiFactory>
{
    private readonly HttpClient _httpClient;
    private readonly Faker<UserRegistrationRequest> _userGenerator = new Faker<UserRegistrationRequest>()
        .RuleFor(x => x.Email, faker => faker.Person.Email)
        .RuleFor(x => x.Password, faker => faker.Person.FirstName);

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
        var result = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
}