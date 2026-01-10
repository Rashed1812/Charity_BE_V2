using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Data.Models.IdentityModels;
using Shared.DTOS.ReconcileRequestDTOs;

namespace DAL.Data.Models
{
    public class ReconcileRequest
    {
        [Key]
        public int Id { get; set; }

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

        [ForeignKey("ReconcileRequestTypeId")]
        public virtual ReconcileRequestType ReconcileRequestType { get; set; }

        [Required]
        public ReconcileRequestStatus Status { get; set; } = ReconcileRequestStatus.NewRequest;

        public int? SupervisorId { get; set; }

        [ForeignKey("SupervisorId")]
        public virtual Supervisor? Supervisor { get; set; }

        public int? MediationId { get; set; }

        [ForeignKey("MediationId")]
        public virtual Mediation? Mediation { get; set; }

        [StringLength(5000)]
        public string? ConsultantNotes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? AssignedToSupervisorAt { get; set; }
        public DateTime? AssignedToMediationAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        // Navigation Properties
        public virtual ICollection<ReconcileRequestAttachment> Attachments { get; set; } = new List<ReconcileRequestAttachment>();
    }
} 