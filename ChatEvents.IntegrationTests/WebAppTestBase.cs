using Microsoft.AspNetCore.Mvc.Testing;

namespace ChatEvents.IntegrationTests;

public abstract class WebAppTestBase
{
    private WebApplicationFactory<Program> _webAppFactory;
    protected HttpClient TestClient;

    [SetUp]
    public void Setup()
    {
        _webAppFactory = new WebApplicationFactory<Program>();
        TestClient = _webAppFactory.CreateClient();
    }
    
    [TearDown]
    public void TearDown()
    {
        TestClient?.Dispose();
        _webAppFactory?.Dispose();
    }
}