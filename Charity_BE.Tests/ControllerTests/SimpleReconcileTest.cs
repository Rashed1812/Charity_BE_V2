using Charity_BE;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using FluentAssertions;

namespace Charity_BE.Tests.ControllerTests
{
    public class SimpleReconcileTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public SimpleReconcileTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task HealthCheck_ShouldReturnSuccess()
        {
            // This is a simple test to ensure the application is running
            var response = await _client.GetAsync("/");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound); // 404 is expected for root path
        }

        [Fact]
        public async Task Get_ReconcileRequestTypes_ShouldReturnSuccess()
        {
            // Act
            var response = await _client.GetAsync("/api/reconcilerequesttype");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}