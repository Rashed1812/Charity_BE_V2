using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOS.ReconcileRequestTypeDTOs
{
    public class ReconcileRequestTypeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateReconcileRequestTypeDTO
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }
    }

    public class UpdateReconcileRequestTypeDTO
    {
        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public bool? IsActive { get; set; }
    }
}

