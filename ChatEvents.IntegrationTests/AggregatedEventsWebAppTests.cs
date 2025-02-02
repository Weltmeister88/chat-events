using System.Net;
using ChatEvents.API.Endpoints.AggregatedEvents.DTOs;
using ChatEvents.Models;

namespace ChatEvents.IntegrationTests;

[TestFixture]
public class AggregatedEventsWebAppTests : WebAppTestBase
{
    private const string RequestUrl = "/api/v1/chat-rooms";

    [Test]
    [TestCase(Granularity.MinuteByMinute)]
    [TestCase(Granularity.Hourly)]
    [TestCase(Granularity.Daily)]
    public async Task GetEndpoint_ReturnsSuccessStatusCode(Granularity granularity)
    {
        // Act
        var chatRoomId = 1;
        HttpResponseMessage response =
            await TestClient.GetAsync($"{RequestUrl}/{chatRoomId}/aggregated-events/{granularity}");

        // Assert
        Assert.That(response.IsSuccessStatusCode, Is.True);
        var result = await response.Content.ReadFromJsonAsync<List<AggregationOccurenceResponse>>();
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task GetEndpoint_WithWrongGranularity_Returns400BadRequest()
    {
        // Act
        var chatRoomId = 1;
        var granularity = "aaa";
        HttpResponseMessage response =
            await TestClient.GetAsync($"{RequestUrl}/{chatRoomId}/aggregated-events/{granularity}");

        // Assert
        Assert.That(response.IsSuccessStatusCode, Is.False);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task GetEndpoint_WithWrongRoomId_Returns404NotFound()
    {
        // Act
        var chatRoomId = 2;
        var granularity = "MinuteByMinute";
        HttpResponseMessage response =
            await TestClient.GetAsync($"{RequestUrl}/{chatRoomId}/aggregated-events/{granularity}");

        // Assert
        Assert.That(response.IsSuccessStatusCode, Is.False);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}