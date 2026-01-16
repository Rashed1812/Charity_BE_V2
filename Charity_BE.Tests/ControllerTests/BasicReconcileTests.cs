using Charity_BE;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using FluentAssertions;
using System.Collections.Generic;
using Shared.DTOS.ReconcileRequestDTOs;
using Shared.DTOS.ReconcileRequestTypeDTOs;
using Shared.DTOS.AuthDTO;
using Shared.DTOS.Common;
using Shared.DTOS.MediationDTOs;
using System.Net.Http.Json;

namespace Charity_BE.Tests.ControllerTests
{
    public class BasicReconcileTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public BasicReconcileTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Get_ReconcileRequestTypes_ShouldReturnSuccess()
        {
            // Act
            var response = await _client.GetAsync("/api/reconcilerequesttype");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<ApiResponse<List<ReconcileRequestTypeDTO>>>();
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
            content.Data.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Get_Active_ReconcileRequestTypes_ShouldReturnOnlyActive()
        {
            // Act
            var response = await _client.GetAsync("/api/reconcilerequesttype/active");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<ApiResponse<List<ReconcileRequestTypeDTO>>>();
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
            content.Data.Should().AllSatisfy(d => d.IsActive.Should().BeTrue());
        }

        [Fact]
        public async Task Create_ReconcileRequest_Public_ShouldSucceed()
        {
            // Arrange
            var allTypesResponse = await _client.GetAsync("/api/reconcilerequesttype/active");
            var allTypesContent = await allTypesResponse.Content.ReadFromJsonAsync<ApiResponse<List<ReconcileRequestTypeDTO>>>();
            var typeId = allTypesContent.Data.First().Id;

            var content = new MultipartFormDataContent();
            content.Add(new StringContent($"Test User {Guid.NewGuid()}"), "Name");
            content.Add(new StringContent($"test{Guid.NewGuid()}@example.com"), "Email");
            content.Add(new StringContent("0501234567"), "PhoneNumber");
            content.Add(new StringContent("طلب اختبار"), "RequestText");
            content.Add(new StringContent(typeId.ToString()), "ReconcileRequestTypeId");

            // Act
            var response = await _client.PostAsync("/api/reconcilerequest", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var responseContent = await response.Content.ReadFromJsonAsync<ApiResponse<ReconcileRequestDTO>>();
            responseContent.Should().NotBeNull();
            responseContent.Success.Should().BeTrue();
            responseContent.Data.Should().NotBeNull();
            responseContent.Data.Status.Should().Be(ReconcileRequestStatus.NewRequest);
        }

        [Fact]
        public async Task Get_Mediations_ShouldReturnSuccess()
        {
            // Act
            var response = await _client.GetAsync("/api/mediation");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<ApiResponse<List<MediationDTO>>>();
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task Login_Admin_ShouldReturnToken()
        {
            // Arrange
            var loginDto = new LoginDTO
            {
                Email = "Admin@gmail.com",
                Password = "P@ssw0rd123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/authentication/login", loginDto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponseDTO>>();
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
            content.Data.Token.Should().NotBeNullOrEmpty();
            content.Data.User.Should().NotBeNull();
            content.Data.User.Role.Should().Contain("Admin");
        }

        [Fact]
        public async Task Login_Supervisor_ShouldReturnToken()
        {
            // Arrange
            var loginDto = new LoginDTO
            {
                Email = "supervisor@gmail.com",
                Password = "P@ssw0rd123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/authentication/login", loginDto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponseDTO>>();
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
            content.Data.Token.Should().NotBeNullOrEmpty();
            content.Data.User.Should().NotBeNull();
            content.Data.User.Role.Should().Contain("Supervisor");
        }

        [Fact]
        public async Task Login_Mediation_ShouldReturnToken()
        {
            // Arrange
            var loginDto = new LoginDTO
            {
                Email = "mediation@gmail.com",
                Password = "P@ssw0rd123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/authentication/login", loginDto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponseDTO>>();
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
            content.Data.Token.Should().NotBeNullOrEmpty();
            content.Data.User.Should().NotBeNull();
            content.Data.User.Role.Should().Contain("Mediation");
        }
    }
}