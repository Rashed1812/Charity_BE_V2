using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.ServiceAbstraction;
using Shared.DTOS.SupervisorDTOs;
using Shared.DTOS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class SupervisorController : ControllerBase
    {
        private readonly ISupervisorService _service;

        public SupervisorController(ISupervisorService service)
        {
            _service = service;
        }

        // GET: api/supervisor
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<SupervisorDTO>>>> GetAll([FromQuery] int? year = null)
        {
            try
            {
                var supervisors = await _service.GetAllAsync(year);
                return Ok(ApiResponse<List<SupervisorDTO>>.SuccessResult(supervisors));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<SupervisorDTO>>.ErrorResult("Failed to retrieve supervisors", 500));
            }
        }

        // GET: api/supervisor/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<SupervisorDTO>>> GetById(int id, [FromQuery] int? year = null)
        {
            try
            {
                var supervisor = await _service.GetByIdAsync(id, year);
                if (supervisor == null)
                    return NotFound(ApiResponse<SupervisorDTO>.ErrorResult($"Supervisor with ID {id} not found", 404));

                return Ok(ApiResponse<SupervisorDTO>.SuccessResult(supervisor));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<SupervisorDTO>.ErrorResult("Failed to retrieve supervisor", 500));
            }
        }

        // GET: api/supervisor/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponse<SupervisorDTO>>> GetByUserId(string userId)
        {
            try
            {
                var supervisor = await _service.GetByUserIdAsync(userId);
                if (supervisor == null)
                    return NotFound(ApiResponse<SupervisorDTO>.ErrorResult($"Supervisor with UserId {userId} not found", 404));

                return Ok(ApiResponse<SupervisorDTO>.SuccessResult(supervisor));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<SupervisorDTO>.ErrorResult("Failed to retrieve supervisor", 500));
            }
        }

        // POST: api/supervisor
        [HttpPost]
        public async Task<ActionResult<ApiResponse<SupervisorDTO>>> Create([FromBody] CreateSupervisorDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<SupervisorDTO>.ErrorResult("Invalid input data", 400,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }

            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id },
                    ApiResponse<SupervisorDTO>.SuccessResult(created, "Supervisor created successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<SupervisorDTO>.ErrorResult(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<SupervisorDTO>.ErrorResult($"Failed to create supervisor: {ex.Message}", 500));
            }
        }

        // PUT: api/supervisor/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<SupervisorDTO>>> Update(int id, [FromBody] UpdateSupervisorDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<SupervisorDTO>.ErrorResult("Invalid input data", 400));
            }

            try
            {
                var updated = await _service.UpdateAsync(id, dto);
                if (updated == null)
                    return NotFound(ApiResponse<SupervisorDTO>.ErrorResult($"Supervisor with ID {id} not found", 404));

                return Ok(ApiResponse<SupervisorDTO>.SuccessResult(updated, "Supervisor updated successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<SupervisorDTO>.ErrorResult(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<SupervisorDTO>.ErrorResult($"Failed to update supervisor: {ex.Message}", 500));
            }
        }

        // DELETE: api/supervisor/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
                if (!result)
                    return NotFound(ApiResponse<object>.ErrorResult($"Supervisor with ID {id} not found", 404));

                return Ok(ApiResponse<object>.SuccessResult(null, "Supervisor deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult($"Failed to delete supervisor: {ex.Message}", 500));
            }
        }

        // PUT: api/supervisor/{id}/toggle-active
        [HttpPut("{id}/toggle-active")]
        public async Task<ActionResult<ApiResponse<bool>>> ToggleActive(int id)
        {
            try
            {
                var result = await _service.ToggleActiveAsync(id);
                return Ok(ApiResponse<bool>.SuccessResult(result, $"Supervisor {(result ? "activated" : "deactivated")} successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult($"Failed to toggle supervisor status: {ex.Message}", 500));
            }
        }
    }
}

