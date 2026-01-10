using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Data.Models
{
    public class ReconcileRequestAttachment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ReconcileRequestId { get; set; }

        [ForeignKey("ReconcileRequestId")]
        public virtual ReconcileRequest ReconcileRequest { get; set; }

        [Required]
        [StringLength(500)]
        public string FileUrl { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        [Required]
        [StringLength(50)]
        public string FileType { get; set; } // "image", "pdf", etc.

        public long FileSize { get; set; } // in bytes

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}

