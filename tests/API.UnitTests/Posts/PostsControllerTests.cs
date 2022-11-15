using API.DTOs;
using API.DTOs.Responses;
using Application.Posts;
using Bogus;
using FluentAssertions;
using System.Net.Http.Json;

namespace API.Tests.Integration.Posts;

public class PostsControllerTests : IClassFixture<BlogApiFactory>
{
    private readonly HttpClient _httpClient;
    private readonly Faker<UserRegistrationRequest> _userGenerator = new Faker<UserRegistrationRequest>()
        .RuleFor(x => x.Email, faker => faker.Person.Email);
    private readonly Faker<PostDto> _postGenerator = new Faker<PostDto>()
        .RuleFor(x => x.Id, Guid.NewGuid())
        .RuleFor(x => x.Title, faker => faker.Lorem.Slug(5))
        .RuleFor(x => x.Content, faker => faker.Lorem.Paragraph(10))
        .RuleFor(x => x.Date, faker => faker.Date.Recent());

    public PostsControllerTests(BlogApiFactory blogApiFactory)
    {
        _httpClient = blogApiFactory.CreateClient();
    }

    [Fact]
    public async Task Create_ShouldCreatePost_WhenPostDataIsValid() 
    {
        // Arrange
        var user = _userGenerator.Generate();
        user.Password = "Test12345!.";

        var postDto = _postGenerator.Generate();

        // Act
        var registerUserRequest = await _httpClient.PostAsJsonAsync("/api/account/register", user);
        if (!registerUserRequest.IsSuccessStatusCode) return;

        var registrationResponse = await registerUserRequest.Content.ReadFromJsonAsync<AuthSuccessResponse>();

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", registrationResponse?.Token);
        var createPostRequest = await _httpClient.PostAsJsonAsync("/api/posts", postDto);

        // Assert

        createPostRequest.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
}
