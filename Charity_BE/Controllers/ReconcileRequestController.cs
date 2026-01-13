using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.ServiceAbstraction;
using Shared.DTOS.ReconcileRequestDTOs;
using Shared.DTOS.AdminDTOs;
using Shared.DTOS.Common;
using Shared.DTOS.NotificationDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReconcileRequestController : ControllerBase
    {
        private readonly IReconcileRequestService _service;
        private readonly ISupervisorService _supervisorService;
        private readonly IMediationService _mediationService;
        private readonly IAdminService _adminService;
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;

        public ReconcileRequestController(
            IReconcileRequestService service,
            ISupervisorService supervisorService,
            IMediationService mediationService,
            IAdminService adminService,
            INotificationService notificationService,
            IEmailService emailService)
        {
            _service = service;
            _supervisorService = supervisorService;
            _mediationService = mediationService;
            _adminService = adminService;
            _notificationService = notificationService;
            _emailService = emailService;
        }

        // POST: api/reconcilerequest (Public - No authentication required)
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<ReconcileRequestDTO>>> Create([FromForm] CreateReconcileRequestDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<ReconcileRequestDTO>.ErrorResult("Invalid input data", 400,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }

            try
            {
                var created = await _service.CreateAsync(dto);

                // Notify admins about new request
                var admins = await _adminService.GetAllAdminsAsync();
                foreach (var admin in admins)
                {
                    var notification = new NotificationCreateDTO
                    {
                        UserId = admin.UserId,
                        Title = "طلب إصلاح ذات بين جديد",
                        Message = $"تم استلام طلب إصلاح ذات بين جديد من {dto.Name} ({dto.Email})",
                        Type = NotificationType.General
                    };
                    await _notificationService.AddNotificationAsync(notification);

                    if (!string.IsNullOrEmpty(admin.Email))
                    {
                        string subject = "طلب إصلاح ذات بين جديد";
                        string body = $"<p>تم استلام طلب إصلاح ذات بين جديد من <b>{dto.Name}</b> ({dto.Email})</p><p>يرجى مراجعته من خلال لوحة التحكم.</p>";
                        await _emailService.SendEmailAsync(admin.Email, subject, body);
                    }
                }

                return CreatedAtAction(nameof(GetById), new { id = created.Id },
                    ApiResponse<ReconcileRequestDTO>.SuccessResult(created, "تم إرسال الطلب بنجاح"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<ReconcileRequestDTO>.ErrorResult(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ReconcileRequestDTO>.ErrorResult($"Failed to create request: {ex.Message}", 500));
            }
        }

        // GET: api/reconcilerequest
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<List<ReconcileRequestDTO>>>> GetAll()
        {
            try
        {
            var requests = await _service.GetAllAsync();
            return Ok(ApiResponse<List<ReconcileRequestDTO>>.SuccessResult(requests));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<ReconcileRequestDTO>>.ErrorResult("Failed to retrieve requests", 500));
            }
        }

        // GET: api/reconcilerequest/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Supervisor,Mediation")]
        public async Task<ActionResult<ApiResponse<ReconcileRequestDTO>>> GetById(int id)
        {
            try
        {
            var request = await _service.GetByIdAsync(id);
            if (request == null)
                return NotFound(ApiResponse<ReconcileRequestDTO>.ErrorResult($"Request with ID {id} not found", 404));

            return Ok(ApiResponse<ReconcileRequestDTO>.SuccessResult(request));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ReconcileRequestDTO>.ErrorResult("Failed to retrieve request", 500));
            }
        }

        // DELETE: api/reconcilerequest/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
            if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"Request with ID {id} not found", 404));

            return Ok(ApiResponse<bool>.SuccessResult(true, "Request deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult($"Failed to delete request: {ex.Message}", 500));
            }
        }

        // ========== Admin Operations ==========

        // POST: api/reconcilerequest/{id}/assign-supervisor
        [HttpPost("{id}/assign-supervisor")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<ReconcileRequestDTO>>> AssignToSupervisor(int id, [FromBody] AssignToSupervisorDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<ReconcileRequestDTO>.ErrorResult("Invalid input data", 400));
            }

            try
            {
                var request = await _service.AssignToSupervisorAsync(id, dto.SupervisorId);
                
                // Notify supervisor
                var supervisor = await _supervisorService.GetByIdAsync(dto.SupervisorId);
                if (supervisor != null)
                {
                    var notification = new NotificationCreateDTO
                    {
                        UserId = supervisor.UserId,
                        Title = "تم إحالة طلب إصلاح ذات البين",
                        Message = $"تم إحالة طلب إصلاح ذات البين رقم {id} إليك",
                        Type = NotificationType.General
                    };
                    await _notificationService.AddNotificationAsync(notification);
                }

                return Ok(ApiResponse<ReconcileRequestDTO>.SuccessResult(request, "تم إحالة الطلب للمشرف بنجاح"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<ReconcileRequestDTO>.ErrorResult(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ReconcileRequestDTO>.ErrorResult($"Failed to assign supervisor: {ex.Message}", 500));
            }
        }

        // POST: api/reconcilerequest/{id}/cancel
        [HttpPost("{id}/cancel")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<ReconcileRequestDTO>>> CancelRequest(int id)
        {
            try
            {
                var request = await _service.CancelRequestAsync(id);
                return Ok(ApiResponse<ReconcileRequestDTO>.SuccessResult(request, "تم إلغاء الطلب بنجاح"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<ReconcileRequestDTO>.ErrorResult(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ReconcileRequestDTO>.ErrorResult($"Failed to cancel request: {ex.Message}", 500));
            }
        }

        // POST: api/reconcilerequest/{id}/complete
        [HttpPost("{id}/complete")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<ReconcileRequestDTO>>> CompleteRequest(int id)
        {
            try
            {
                var request = await _service.CompleteRequestAsync(id);
                return Ok(ApiResponse<ReconcileRequestDTO>.SuccessResult(request, "تم إكمال الطلب بنجاح"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<ReconcileRequestDTO>.ErrorResult(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ReconcileRequestDTO>.ErrorResult($"Failed to complete request: {ex.Message}", 500));
            }
        }

        // ========== Supervisor Operations ==========

        // GET: api/reconcilerequest/supervisor/{supervisorId}
        [HttpGet("supervisor/{supervisorId}")]
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<ActionResult<ApiResponse<List<ReconcileRequestDTO>>>> GetBySupervisorId(int supervisorId)
        {
            try
            {
                var requests = await _service.GetBySupervisorIdAsync(supervisorId);
                return Ok(ApiResponse<List<ReconcileRequestDTO>>.SuccessResult(requests));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<ReconcileRequestDTO>>.ErrorResult("Failed to retrieve requests", 500));
            }
        }

        // POST: api/reconcilerequest/{id}/assign-mediation
        [HttpPost("{id}/assign-mediation")]
        [Authorize(Roles = "Supervisor")]
        public async Task<ActionResult<ApiResponse<ReconcileRequestDTO>>> AssignToMediation(int id, [FromBody] AssignToMediationDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<ReconcileRequestDTO>.ErrorResult("Invalid input data", 400));
            }

            try
            {
                var request = await _service.AssignToMediationAsync(id, dto.MediationId);
                
                // Notify mediation
                var mediation = await _mediationService.GetMediationByIdAsync(dto.MediationId);
                if (mediation != null)
                {
                    var notification = new NotificationCreateDTO
                    {
                        UserId = mediation.UserId,
                        Title = "تم إحالة طلب إصلاح ذات البين",
                        Message = $"تم إحالة طلب إصلاح ذات البين رقم {id} إليك",
                        Type = NotificationType.General
                    };
                    await _notificationService.AddNotificationAsync(notification);
                }

                return Ok(ApiResponse<ReconcileRequestDTO>.SuccessResult(request, "تم إحالة الطلب للمستشار بنجاح"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<ReconcileRequestDTO>.ErrorResult(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ReconcileRequestDTO>.ErrorResult($"Failed to assign mediation: {ex.Message}", 500));
            }
        }

        // POST: api/reconcilerequest/{id}/mark-reviewed
        [HttpPost("{id}/mark-reviewed")]
        [Authorize(Roles = "Supervisor")]
        public async Task<ActionResult<ApiResponse<ReconcileRequestDTO>>> MarkAsReviewed(int id)
        {
            try
            {
                var request = await _service.MarkAsReviewedAsync(id);
                
                // Notify admin
                var admins = await _adminService.GetAllAdminsAsync();
                foreach (var admin in admins)
                {
                    var notification = new NotificationCreateDTO
                    {
                        UserId = admin.UserId,
                        Title = "تم مراجعة طلب إصلاح ذات البين",
                        Message = $"تم مراجعة طلب إصلاح ذات البين رقم {id} من قبل المشرف",
                        Type = NotificationType.General
                    };
                    await _notificationService.AddNotificationAsync(notification);
                }

                return Ok(ApiResponse<ReconcileRequestDTO>.SuccessResult(request, "تم مراجعة الطلب بنجاح"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<ReconcileRequestDTO>.ErrorResult(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ReconcileRequestDTO>.ErrorResult($"Failed to mark as reviewed: {ex.Message}", 500));
            }
        }

        // ========== Mediation Operations ==========

        // GET: api/reconcilerequest/mediation/{mediationId}
        [HttpGet("mediation/{mediationId}")]
        [Authorize(Roles = "Admin,Mediation")]
        public async Task<ActionResult<ApiResponse<List<ReconcileRequestDTO>>>> GetByMediationId(int mediationId)
        {
            try
            {
                var requests = await _service.GetByMediationIdAsync(mediationId);
                return Ok(ApiResponse<List<ReconcileRequestDTO>>.SuccessResult(requests));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<ReconcileRequestDTO>>.ErrorResult("Failed to retrieve requests", 500));
            }
        }

        // POST: api/reconcilerequest/{id}/start
        [HttpPost("{id}/start")]
        [Authorize(Roles = "Mediation")]
        public async Task<ActionResult<ApiResponse<ReconcileRequestDTO>>> StartRequest(int id, [FromBody] StartRequestDTO dto)
        {
            try
            {
                var request = await _service.StartRequestAsync(id, dto);
                return Ok(ApiResponse<ReconcileRequestDTO>.SuccessResult(request, "تم بدء تنفيذ الطلب بنجاح"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<ReconcileRequestDTO>.ErrorResult(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ReconcileRequestDTO>.ErrorResult($"Failed to start request: {ex.Message}", 500));
            }
        }

        // POST: api/reconcilerequest/{id}/complete-execution
        [HttpPost("{id}/complete-execution")]
        [Authorize(Roles = "Mediation")]
        public async Task<ActionResult<ApiResponse<ReconcileRequestDTO>>> CompleteExecution(int id, [FromBody] CompleteReconcileRequestDTO dto)
        {
            try
            {
                var request = await _service.CompleteExecutionAsync(id, dto);
                
                // Notify supervisor
                if (request.SupervisorId.HasValue)
                {
                    var supervisor = await _supervisorService.GetByIdAsync(request.SupervisorId.Value);
                    if (supervisor != null)
                    {
                        var notification = new NotificationCreateDTO
                        {
                            UserId = supervisor.UserId,
                            Title = "طلب بانتظار المراجعة",
                            Message = $"تم إكمال تنفيذ طلب إصلاح ذات البين رقم {id} وهو بانتظار مراجعتك",
                            Type = NotificationType.General
                        };
                        await _notificationService.AddNotificationAsync(notification);
                    }
                }

                return Ok(ApiResponse<ReconcileRequestDTO>.SuccessResult(request, "تم إكمال تنفيذ الطلب بنجاح"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<ReconcileRequestDTO>.ErrorResult(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ReconcileRequestDTO>.ErrorResult($"Failed to complete execution: {ex.Message}", 500));
            }
        }

        // POST: api/reconcilerequest/{id}/send-sms
        [HttpPost("{id}/send-sms")]
        [Authorize(Roles = "Mediation")]
        public async Task<ActionResult<ApiResponse<bool>>> SendSMS(int id, [FromBody] SendSMSDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<bool>.ErrorResult("Invalid input data", 400,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }

            try
            {
                var result = await _service.SendSMSAsync(id, dto);
                if (result)
                    return Ok(ApiResponse<bool>.SuccessResult(result, "تم إرسال الرسالة بنجاح"));
                else
                    return BadRequest(ApiResponse<bool>.ErrorResult("فشل إرسال الرسالة", 400));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResult(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult($"Failed to send SMS: {ex.Message}", 500));
            }
        }

        // GET: api/reconcilerequest/sms-templates
        [HttpGet("sms-templates")]
        [Authorize(Roles = "Mediation")]
        public ActionResult<ApiResponse<List<string>>> GetSMSTemplates()
        {
            try
            {
                var templates = _service.GetSMSTemplates();
                return Ok(ApiResponse<List<string>>.SuccessResult(templates, "تم جلب نماذج الرسائل بنجاح"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<string>>.ErrorResult($"Failed to get SMS templates: {ex.Message}", 500));
            }
        }

    }
}
