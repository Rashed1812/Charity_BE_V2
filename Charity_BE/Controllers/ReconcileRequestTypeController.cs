using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.ServiceAbstraction;
using Shared.DTOS.ReconcileRequestTypeDTOs;
using Shared.DTOS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReconcileRequestTypeController : ControllerBase
    {
        private readonly IReconcileRequestTypeService _service;

        public ReconcileRequestTypeController(IReconcileRequestTypeService service)
        {
            _service = service;
        }

        // GET: api/reconcilerequesttype
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ReconcileRequestTypeDTO>>>> GetAll()
        {
            try
            {
                var types = await _service.GetAllAsync();
                return Ok(ApiResponse<List<ReconcileRequestTypeDTO>>.SuccessResult(types));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<ReconcileRequestTypeDTO>>.ErrorResult("Failed to retrieve types", 500));
            }
        }

        // GET: api/reconcilerequesttype/active
        [HttpGet("active")]
        public async Task<ActionResult<ApiResponse<List<ReconcileRequestTypeDTO>>>> GetActiveTypes()
        {
            try
            {
                var types = await _service.GetActiveTypesAsync();
                return Ok(ApiResponse<List<ReconcileRequestTypeDTO>>.SuccessResult(types));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<ReconcileRequestTypeDTO>>.ErrorResult("Failed to retrieve active types", 500));
            }
        }

        // GET: api/reconcilerequesttype/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ReconcileRequestTypeDTO>>> GetById(int id)
        {
            try
            {
                var type = await _service.GetByIdAsync(id);
                if (type == null)
                    return NotFound(ApiResponse<ReconcileRequestTypeDTO>.ErrorResult($"Type with ID {id} not found", 404));

                return Ok(ApiResponse<ReconcileRequestTypeDTO>.SuccessResult(type));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ReconcileRequestTypeDTO>.ErrorResult("Failed to retrieve type", 500));
            }
        }

        // POST: api/reconcilerequesttype
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<ReconcileRequestTypeDTO>>> Create([FromBody] CreateReconcileRequestTypeDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<ReconcileRequestTypeDTO>.ErrorResult("Invalid input data", 400,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }

            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id },
                    ApiResponse<ReconcileRequestTypeDTO>.SuccessResult(created, "Type created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ReconcileRequestTypeDTO>.ErrorResult($"Failed to create type: {ex.Message}", 500));
            }
        }

        // PUT: api/reconcilerequesttype/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<ReconcileRequestTypeDTO>>> Update(int id, [FromBody] UpdateReconcileRequestTypeDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<ReconcileRequestTypeDTO>.ErrorResult("Invalid input data", 400));
            }

            try
            {
                var updated = await _service.UpdateAsync(id, dto);
                if (updated == null)
                    return NotFound(ApiResponse<ReconcileRequestTypeDTO>.ErrorResult($"Type with ID {id} not found", 404));

                return Ok(ApiResponse<ReconcileRequestTypeDTO>.SuccessResult(updated, "Type updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ReconcileRequestTypeDTO>.ErrorResult($"Failed to update type: {ex.Message}", 500));
            }
        }

        // DELETE: api/reconcilerequesttype/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
                if (!result)
                    return NotFound(ApiResponse<object>.ErrorResult($"Type with ID {id} not found", 404));

                return Ok(ApiResponse<object>.SuccessResult(null, "Type deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult($"Failed to delete type: {ex.Message}", 500));
            }
        }

        // PUT: api/reconcilerequesttype/{id}/toggle-active
        [HttpPut("{id}/toggle-active")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> ToggleActive(int id)
        {
            try
            {
                var result = await _service.ToggleActiveAsync(id);
                return Ok(ApiResponse<bool>.SuccessResult(result, $"Type {(result ? "activated" : "deactivated")} successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult($"Failed to toggle type status: {ex.Message}", 500));
            }
        }
    }
}

