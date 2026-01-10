using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Data.Models
{
    public class ReconcileRequestType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation Property
        public virtual ICollection<ReconcileRequest> ReconcileRequests { get; set; } = new List<ReconcileRequest>();
    }
}

