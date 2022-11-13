using API.Tests.Integration;

namespace API.Tests.Unit.Users;

public class CreateAccountControllerTests : IClassFixture<BlogApiFactory>
{

    public CreateAccountControllerTests(BlogApiFactory blogApiFactory)
    {
    }

    [Fact]
    public async Task Test1()
    {
        await Task.Delay(5000);
    }
}