using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Shared.DTOS.ReconcileRequestDTOs
{
    public class ReconcileRequestDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string RequestText { get; set; }
        public int ReconcileRequestTypeId { get; set; }
        public string ReconcileRequestTypeName { get; set; }
        public ReconcileRequestStatus Status { get; set; }
        public string StatusName { get; set; }
        public int? SupervisorId { get; set; }
        public string? SupervisorName { get; set; }
        public int? MediationId { get; set; }
        public string? MediationName { get; set; }
        public string? ConsultantNotes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? AssignedToSupervisorAt { get; set; }
        public DateTime? AssignedToMediationAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public List<ReconcileRequestAttachmentDTO> Attachments { get; set; } = new List<ReconcileRequestAttachmentDTO>();
    }

    public class CreateReconcileRequestDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(2000)]
        public string RequestText { get; set; }

        [Required]
        public int ReconcileRequestTypeId { get; set; }

        public List<IFormFile>? Attachments { get; set; }
    }

    public class UpdateReconcileRequestDTO
    {
        public ReconcileRequestStatus? Status { get; set; }
        public int? SupervisorId { get; set; }
        public int? MediationId { get; set; }
        [StringLength(5000)]
        public string? ConsultantNotes { get; set; }
    }

    public class AssignToSupervisorDTO
    {
        [Required]
        public int SupervisorId { get; set; }
    }

    public class AssignToMediationDTO
    {
        [Required]
        public int MediationId { get; set; }
    }

    public class StartRequestDTO
    {
        [StringLength(5000)]
        public string? ConsultantNotes { get; set; }
    }

    public class CompleteRequestDTO
    {
        [StringLength(5000)]
        public string? ConsultantNotes { get; set; }
    }

    public class SendSMSDTO
    {
        [Required]
        [StringLength(1000)]
        public string Message { get; set; }
    }

    public class ReconcileRequestAttachmentDTO
    {
        public int Id { get; set; }
        public int ReconcileRequestId { get; set; }
        public string FileUrl { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public DateTime UploadedAt { get; set; }
    }
} 