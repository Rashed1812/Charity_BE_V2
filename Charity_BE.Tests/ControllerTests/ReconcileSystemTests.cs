using Charity_BE;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net;
using FluentAssertions;
using System.Collections.Generic;
using Shared.DTOS.ReconcileRequestDTOs;
using Shared.DTOS.ReconcileRequestTypeDTOs;
using Shared.DTOS.SupervisorDTOs;
using Shared.DTOS.MediationDTOs;
using Shared.DTOS.AuthDTO;
using Shared.DTOS.Common;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Charity_BE.Tests.ControllerTests
{
    public class ReconcileSystemTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        // Test data
        private const string AdminEmail = "Admin@gmail.com";
        private const string AdminPassword = "P@ssw0rd123";
        private const string SupervisorEmail = "supervisor@gmail.com";
        private const string SupervisorPassword = "P@ssw0rd123";
        private const string MediationEmail = "mediation@gmail.com";
        private const string MediationPassword = "P@ssw0rd123";

        public ReconcileSystemTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        #region Authentication Tests

        [Fact]
        public async Task Login_Admin_ShouldReturnToken()
        {
            // Arrange
            var loginDto = new LoginDTO
            {
                Email = AdminEmail,
                Password = AdminPassword
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/authentication/login", loginDto);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponseDTO>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
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
                Email = SupervisorEmail,
                Password = SupervisorPassword
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/authentication/login", loginDto);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponseDTO>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
            content.Data.Token.Should().NotBeNullOrEmpty();
            content.Data.User.Should().NotBeNull();
            content.Data.User.Role.Should().Be("Supervisor");
        }

        [Fact]
        public async Task Login_Mediation_ShouldReturnToken()
        {
            // Arrange
            var loginDto = new LoginDTO
            {
                Email = MediationEmail,
                Password = MediationPassword
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/authentication/login", loginDto);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponseDTO>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
            content.Data.Token.Should().NotBeNullOrEmpty();
            content.Data.User.Should().NotBeNull();
            content.Data.User.Role.Should().Be("Mediation");
        }

        #endregion

        #region ReconcileRequestType Tests

        [Fact]
        public async Task GetAll_ReconcileRequestTypes_ShouldReturnSuccess()
        {
            // Act
            var response = await _client.GetAsync("/api/reconcilerequesttype");
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<List<ReconcileRequestTypeDTO>>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
            content.Data.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetActive_ReconcileRequestTypes_ShouldReturnOnlyActive()
        {
            // Act
            var response = await _client.GetAsync("/api/reconcilerequesttype/active");
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<List<ReconcileRequestTypeDTO>>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
            content.Data.Should().AllSatisfy(d => d.IsActive.Should().BeTrue());
        }

        [Fact]
        public async Task GetById_ReconcileRequestType_ShouldReturnType()
        {
            // Arrange
            var allResponse = await _client.GetAsync("/api/reconcilerequesttype");
            var allContent = await allResponse.Content.ReadFromJsonAsync<ApiResponse<List<ReconcileRequestTypeDTO>>>();
            var existingId = allContent.Data.First().Id;

            // Act
            var response = await _client.GetAsync($"/api/reconcilerequesttype/{existingId}");
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<ReconcileRequestTypeDTO>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
            content.Data.Id.Should().Be(existingId);
        }

        [Fact]
        public async Task GetById_ReconcileRequestType_InvalidId_ShouldReturnNotFound()
        {
            // Act
            var response = await _client.GetAsync("/api/reconcilerequesttype/99999");
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<ReconcileRequestTypeDTO>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            content.Should().NotBeNull();
            content.Success.Should().BeFalse();
            content.StatusCode.Should().Be(404);
        }

        #endregion

        #region Public ReconcileRequest Tests

        [Fact]
        public async Task Create_ReconcileRequest_Public_ShouldSucceed()
        {
            // Arrange
            var allTypesResponse = await _client.GetAsync("/api/reconcilerequesttype/active");
            var allTypesContent = await allTypesResponse.Content.ReadFromJsonAsync<ApiResponse<List<ReconcileRequestTypeDTO>>>();
            var typeId = allTypesContent.Data.First().Id;

            var content = new MultipartFormDataContent();
            content.Add(new StringContent("أحمد محمد"), "Name");
            content.Add(new StringContent("ahmed.test@example.com"), "Email");
            content.Add(new StringContent("0501234567"), "PhoneNumber");
            content.Add(new StringContent("طلب اختبار للاستشارة الأسرية"), "RequestText");
            content.Add(new StringContent(typeId.ToString()), "ReconcileRequestTypeId");

            // Act
            var response = await _client.PostAsync("/api/reconcilerequest", content);
            var responseContent = await response.Content.ReadFromJsonAsync<ApiResponse<ReconcileRequestDTO>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            responseContent.Should().NotBeNull();
            responseContent.Success.Should().BeTrue();
            responseContent.Data.Should().NotBeNull();
            responseContent.Data.Name.Should().Be("أحمد محمد");
            responseContent.Data.Email.Should().Be("ahmed.test@example.com");
            responseContent.Data.Status.Should().Be(1); // NewRequest
        }

        [Fact]
        public async Task Create_ReconcileRequest_InvalidData_ShouldReturnBadRequest()
        {
            // Arrange - missing required fields
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(""), "Name"); // Empty name
            content.Add(new StringContent("invalid-email"), "Email");

            // Act
            var response = await _client.PostAsync("/api/reconcilerequest", content);
            var responseContent = await response.Content.ReadFromJsonAsync<ApiResponse<ReconcileRequestDTO>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseContent.Should().NotBeNull();
            responseContent.Success.Should().BeFalse();
        }

        #endregion

        #region Admin ReconcileRequest Tests

        [Fact]
        public async Task GetAll_ReconcileRequests_Admin_ShouldReturnAllRequests()
        {
            // Arrange
            var token = await GetAdminToken();

            // Act
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/reconcilerequest");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<List<ReconcileRequestDTO>>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task GetById_ReconcileRequest_Admin_ShouldReturnRequest()
        {
            // Arrange
            var token = await GetAdminToken();
            var allRequests = await GetAllRequests(token);
            var existingId = allRequests.First().Id;

            // Act
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/reconcilerequest/{existingId}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<ReconcileRequestDTO>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
            content.Data.Id.Should().Be(existingId);
        }

        [Fact]
        public async Task AssignToSupervisor_Admin_ShouldChangeStatus()
        {
            // Arrange
            var token = await GetAdminToken();
            var requestId = await CreateTestRequest();
            var supervisorId = await GetFirstSupervisorId(token);

            var assignDto = new AssignToSupervisorDTO { SupervisorId = supervisorId };

            // Act
            var request = new HttpRequestMessage(HttpMethod.Post, $"/api/reconcilerequest/{requestId}/assign-supervisor");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(assignDto);
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<ReconcileRequestDTO>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
            content.Data.Status.Should().Be(2); // AssignedToSupervisor
            content.Data.SupervisorId.Should().Be(supervisorId);
        }

        [Fact]
        public async Task CancelRequest_Admin_ShouldChangeStatusToCancelled()
        {
            // Arrange
            var token = await GetAdminToken();
            var requestId = await CreateTestRequest();

            // Act
            var request = new HttpRequestMessage(HttpMethod.Post, $"/api/reconcilerequest/{requestId}/cancel");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<ReconcileRequestDTO>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
            content.Data.Status.Should().Be(8); // Cancelled
        }

        #endregion

        #region Supervisor Tests

        [Fact]
        public async Task GetBySupervisorId_Supervisor_ShouldReturnHisRequests()
        {
            // Arrange
            var token = await GetSupervisorToken();
            var userInfo = await GetUserInfo(token);
            var supervisor = await GetSupervisorById(token, userInfo.Id);

            // Act
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/reconcilerequest/supervisor/{supervisor.Id}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<List<ReconcileRequestDTO>>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task AssignToMediation_Supervisor_ShouldChangeStatus()
        {
            // Arrange
            var adminToken = await GetAdminToken();
            var supervisorToken = await GetSupervisorToken();

            // Create and assign request to supervisor first
            var requestId = await CreateTestRequest();
            var supervisor = await GetSupervisorById(supervisorToken, (await GetUserInfo(supervisorToken)).Id);
            await AssignRequestToSupervisor(adminToken, requestId, supervisor.Id);

            // Now assign to mediation
            var mediationId = await GetFirstMediationId(adminToken);
            var assignDto = new AssignToMediationDTO { MediationId = mediationId };

            // Act
            var request = new HttpRequestMessage(HttpMethod.Post, $"/api/reconcilerequest/{requestId}/assign-mediation");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", supervisorToken);
            request.Content = JsonContent.Create(assignDto);
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<ReconcileRequestDTO>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
            content.Data.Status.Should().Be(3); // AssignedToMediation
            content.Data.MediationId.Should().Be(mediationId);
        }

        [Fact]
        public async Task MarkAsReviewed_Supervisor_ShouldChangeStatus()
        {
            // Arrange
            var adminToken = await GetAdminToken();
            var supervisorToken = await GetSupervisorToken();

            // Create request and move it to PendingSupervisorReview status
            var requestId = await CreateTestRequest();
            var supervisor = await GetSupervisorById(supervisorToken, (await GetUserInfo(supervisorToken)).Id);
            var mediationId = await GetFirstMediationId(adminToken);

            await AssignRequestToSupervisor(adminToken, requestId, supervisor.Id);
            await AssignRequestToMediation(supervisorToken, requestId, mediationId);

            // Start and complete execution by mediation
            var mediationToken = await GetMediationToken();
            await StartRequest(mediationToken, requestId);
            await CompleteExecution(mediationToken, requestId);

            // Act - Mark as reviewed by supervisor
            var request = new HttpRequestMessage(HttpMethod.Post, $"/api/reconcilerequest/{requestId}/mark-reviewed");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", supervisorToken);
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<ReconcileRequestDTO>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
            content.Data.Status.Should().Be(6); // SupervisorReviewed
        }

        #endregion

        #region Mediation Tests

        [Fact]
        public async Task GetByMediationId_Mediation_ShouldReturnHisRequests()
        {
            // Arrange
            var token = await GetMediationToken();
            var userInfo = await GetUserInfo(token);
            var mediation = await GetMediationById(token, userInfo.Id);

            // Act
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/reconcilerequest/mediation/{mediation.Id}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<List<ReconcileRequestDTO>>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task StartRequest_Mediation_ShouldChangeStatus()
        {
            // Arrange
            var adminToken = await GetAdminToken();
            var mediationToken = await GetMediationToken();

            // Create request and assign it to mediation
            var requestId = await CreateTestRequest();
            var mediation = await GetMediationById(mediationToken, (await GetUserInfo(mediationToken)).Id);
            var supervisorId = await GetFirstSupervisorId(adminToken);

            await AssignRequestToSupervisor(adminToken, requestId, supervisorId);
            await AssignRequestToMediation(adminToken, requestId, mediation.Id); // Admin can also assign to mediation

            var startDto = new StartRequestDTO { ConsultantNotes = "بدء الاستشارة" };

            // Act
            var request = new HttpRequestMessage(HttpMethod.Post, $"/api/reconcilerequest/{requestId}/start");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", mediationToken);
            request.Content = JsonContent.Create(startDto);
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<ReconcileRequestDTO>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
            content.Data.Status.Should().Be(4); // InProgress
            content.Data.ConsultantNotes.Should().Be("بدء الاستشارة");
        }

        [Fact]
        public async Task CompleteExecution_Mediation_ShouldChangeStatus()
        {
            // Arrange
            var adminToken = await GetAdminToken();
            var mediationToken = await GetMediationToken();

            // Create request and move it to InProgress status
            var requestId = await CreateTestRequest();
            var mediation = await GetMediationById(mediationToken, (await GetUserInfo(mediationToken)).Id);
            var supervisorId = await GetFirstSupervisorId(adminToken);

            await AssignRequestToSupervisor(adminToken, requestId, supervisorId);
            await AssignRequestToMediation(adminToken, requestId, mediation.Id);
            await StartRequest(mediationToken, requestId);

            var completeDto = new CompleteReconcileRequestDTO { ConsultantNotes = "تم إكمال الاستشارة بنجاح" };

            // Act
            var request = new HttpRequestMessage(HttpMethod.Post, $"/api/reconcilerequest/{requestId}/complete-execution");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", mediationToken);
            request.Content = JsonContent.Create(completeDto);
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<ReconcileRequestDTO>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
            content.Data.Status.Should().Be(5); // PendingSupervisorReview
        }

        [Fact]
        public async Task SendSMS_Mediation_ShouldSucceed()
        {
            // Arrange
            var adminToken = await GetAdminToken();
            var mediationToken = await GetMediationToken();

            // Create request and assign it to mediation
            var requestId = await CreateTestRequest();
            var mediation = await GetMediationById(mediationToken, (await GetUserInfo(mediationToken)).Id);
            var supervisorId = await GetFirstSupervisorId(adminToken);

            await AssignRequestToSupervisor(adminToken, requestId, supervisorId);
            await AssignRequestToMediation(adminToken, requestId, mediation.Id);

            var smsDto = new SendSMSDTO { Message = "طلب الاستشارة قيد التنفيذ، وسيتم التواصل معكم خلال الفترة القريبة القادمة." };

            // Act
            var request = new HttpRequestMessage(HttpMethod.Post, $"/api/reconcilerequest/{requestId}/send-sms");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", mediationToken);
            request.Content = JsonContent.Create(smsDto);
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().BeTrue();
        }

        [Fact]
        public async Task GetSMSTemplates_Mediation_ShouldReturnTemplates()
        {
            // Arrange
            var token = await GetMediationToken();

            // Act
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/reconcilerequest/sms-templates");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<List<string>>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
            content.Data.Should().NotBeEmpty();
            content.Data.Should().HaveCountGreaterThan(0);
        }

        #endregion

        #region Supervisor Management Tests

        [Fact]
        public async Task GetAll_Supervisors_Admin_ShouldReturnAll()
        {
            // Arrange
            var token = await GetAdminToken();

            // Act
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/supervisor");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<List<SupervisorDTO>>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task GetById_Supervisor_Admin_ShouldReturnSupervisor()
        {
            // Arrange
            var token = await GetAdminToken();
            var allSupervisors = await GetAllSupervisors(token);
            var existingId = allSupervisors.First().Id;

            // Act
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/supervisor/{existingId}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<SupervisorDTO>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
            content.Data.Id.Should().Be(existingId);
        }

        #endregion

        #region Mediation Management Tests

        [Fact]
        public async Task GetAll_Mediations_ShouldReturnAll()
        {
            // Act
            var response = await _client.GetAsync("/api/mediation");
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<List<MediationDTO>>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task GetById_Mediation_ShouldReturnMediation()
        {
            // Arrange
            var allMediationsResponse = await _client.GetAsync("/api/mediation");
            var allMediationsContent = await allMediationsResponse.Content.ReadFromJsonAsync<ApiResponse<List<MediationDTO>>>();
            var existingId = allMediationsContent.Data.First().Id;

            // Act
            var response = await _client.GetAsync($"/api/mediation/{existingId}");
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<MediationDTO>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
            content.Success.Should().BeTrue();
            content.Data.Should().NotBeNull();
            content.Data.Id.Should().Be(existingId);
        }

        #endregion

        #region Complete Workflow Test

        [Fact]
        public async Task CompleteWorkflow_ShouldWorkEndToEnd()
        {
            // Arrange
            var adminToken = await GetAdminToken();
            var supervisorToken = await GetSupervisorToken();
            var mediationToken = await GetMediationToken();

            // 1. Create request
            var requestId = await CreateTestRequest();

            // 2. Admin assigns to supervisor
            var supervisor = await GetSupervisorById(supervisorToken, (await GetUserInfo(supervisorToken)).Id);
            await AssignRequestToSupervisor(adminToken, requestId, supervisor.Id);

            // 3. Supervisor assigns to mediation
            var mediation = await GetMediationById(mediationToken, (await GetUserInfo(mediationToken)).Id);
            await AssignRequestToMediation(supervisorToken, requestId, mediation.Id);

            // 4. Mediation starts request
            await StartRequest(mediationToken, requestId);

            // 5. Mediation sends SMS
            var smsDto = new SendSMSDTO { Message = "طلب الاستشارة قيد التنفيذ، وسيتم التواصل معكم خلال الفترة القريبة القادمة." };
            var smsRequest = new HttpRequestMessage(HttpMethod.Post, $"/api/reconcilerequest/{requestId}/send-sms");
            smsRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", mediationToken);
            smsRequest.Content = JsonContent.Create(smsDto);
            await _client.SendAsync(smsRequest);

            // 6. Mediation completes execution
            var completeDto = new CompleteReconcileRequestDTO { ConsultantNotes = "تم إكمال الاستشارة بنجاح" };
            var completeRequest = new HttpRequestMessage(HttpMethod.Post, $"/api/reconcilerequest/{requestId}/complete-execution");
            completeRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", mediationToken);
            completeRequest.Content = JsonContent.Create(completeDto);
            await _client.SendAsync(completeRequest);

            // 7. Supervisor marks as reviewed
            var reviewRequest = new HttpRequestMessage(HttpMethod.Post, $"/api/reconcilerequest/{requestId}/mark-reviewed");
            reviewRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", supervisorToken);
            await _client.SendAsync(reviewRequest);

            // 8. Admin completes finally
            var finalRequest = new HttpRequestMessage(HttpMethod.Post, $"/api/reconcilerequest/{requestId}/complete");
            finalRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);
            var finalResponse = await _client.SendAsync(finalRequest);
            var finalContent = await finalResponse.Content.ReadFromJsonAsync<ApiResponse<ReconcileRequestDTO>>();

            // Assert final state
            finalResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            finalContent.Should().NotBeNull();
            finalContent.Success.Should().BeTrue();
            finalContent.Data.Should().NotBeNull();
            finalContent.Data.Status.Should().Be(7); // Completed
        }

        #endregion

        #region Helper Methods

        private async Task<string> GetAdminToken()
        {
            var loginDto = new LoginDTO { Email = AdminEmail, Password = AdminPassword };
            var response = await _client.PostAsJsonAsync("/api/authentication/login", loginDto);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponseDTO>>();
            return content.Data.Token;
        }

        private async Task<string> GetSupervisorToken()
        {
            var loginDto = new LoginDTO { Email = SupervisorEmail, Password = SupervisorPassword };
            var response = await _client.PostAsJsonAsync("/api/authentication/login", loginDto);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponseDTO>>();
            return content.Data.Token;
        }

        private async Task<string> GetMediationToken()
        {
            var loginDto = new LoginDTO { Email = MediationEmail, Password = MediationPassword };
            var response = await _client.PostAsJsonAsync("/api/authentication/login", loginDto);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponseDTO>>();
            return content.Data.Token;
        }

        private async Task<CurrentUserDTO> GetUserInfo(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/authentication/current-user");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<CurrentUserDTO>>();
            return content.Data;
        }

        private async Task<List<ReconcileRequestDTO>> GetAllRequests(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/reconcilerequest");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<List<ReconcileRequestDTO>>>();
            return content.Data;
        }

        private async Task<int> CreateTestRequest()
        {
            var allTypesResponse = await _client.GetAsync("/api/reconcilerequesttype/active");
            var allTypesContent = await allTypesResponse.Content.ReadFromJsonAsync<ApiResponse<List<ReconcileRequestTypeDTO>>>();
            var typeId = allTypesContent.Data.First().Id;

            var content = new MultipartFormDataContent();
            content.Add(new StringContent($"Test User {Guid.NewGuid()}"), "Name");
            content.Add(new StringContent($"test{Guid.NewGuid()}@example.com"), "Email");
            content.Add(new StringContent("0501234567"), "PhoneNumber");
            content.Add(new StringContent("طلب اختبار"), "RequestText");
            content.Add(new StringContent(typeId.ToString()), "ReconcileRequestTypeId");

            var response = await _client.PostAsync("/api/reconcilerequest", content);
            var responseContent = await response.Content.ReadFromJsonAsync<ApiResponse<ReconcileRequestDTO>>();
            return responseContent.Data.Id;
        }

        private async Task<int> GetFirstSupervisorId(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/supervisor");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<List<SupervisorDTO>>>();
            return content.Data.First().Id;
        }

        private async Task<int> GetFirstMediationId(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/mediation");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<List<MediationDTO>>>();
            return content.Data.First().Id;
        }

        private async Task<List<SupervisorDTO>> GetAllSupervisors(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/supervisor");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<List<SupervisorDTO>>>();
            return content.Data;
        }

        private async Task<SupervisorDTO> GetSupervisorById(string token, string userId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/supervisor/user/{userId}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<SupervisorDTO>>();
            return content.Data;
        }

        private async Task<MediationDTO> GetMediationById(string token, string userId)
        {
            // Since mediation controller doesn't have user/{userId} endpoint, we'll get all and find by userId
            var response = await _client.GetAsync("/api/mediation");
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<List<MediationDTO>>>();
            return content.Data.First(m => m.Id == userId);
        }

        private async Task AssignRequestToSupervisor(string token, int requestId, int supervisorId)
        {
            var assignDto = new AssignToSupervisorDTO { SupervisorId = supervisorId };
            var request = new HttpRequestMessage(HttpMethod.Post, $"/api/reconcilerequest/{requestId}/assign-supervisor");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(assignDto);
            await _client.SendAsync(request);
        }

        private async Task AssignRequestToMediation(string token, int requestId, int mediationId)
        {
            var assignDto = new AssignToMediationDTO { MediationId = mediationId };
            var request = new HttpRequestMessage(HttpMethod.Post, $"/api/reconcilerequest/{requestId}/assign-mediation");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(assignDto);
            await _client.SendAsync(request);
        }

        private async Task StartRequest(string token, int requestId)
        {
            var startDto = new StartRequestDTO { ConsultantNotes = "بدء الاستشارة" };
            var request = new HttpRequestMessage(HttpMethod.Post, $"/api/reconcilerequest/{requestId}/start");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(startDto);
            await _client.SendAsync(request);
        }

        private async Task CompleteExecution(string token, int requestId)
        {
            var completeDto = new CompleteReconcileRequestDTO { ConsultantNotes = "تم إكمال الاستشارة" };
            var request = new HttpRequestMessage(HttpMethod.Post, $"/api/reconcilerequest/{requestId}/complete-execution");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(completeDto);
            await _client.SendAsync(request);
        }

        #endregion
    }
}