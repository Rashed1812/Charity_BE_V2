using DAL.Data;
using DAL.Data.Models;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories.RepositoryClasses
{
    public class ReconcileRequestAttachmentRepository : GenericRepository<ReconcileRequestAttachment>, IReconcileRequestAttachmentRepository
    {
        private readonly ApplicationDbContext _context;

        public ReconcileRequestAttachmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ReconcileRequestAttachment>> GetByRequestIdAsync(int requestId)
        {
            return await _context.ReconcileRequestAttachments
                .Where(a => a.ReconcileRequestId == requestId)
                .OrderBy(a => a.UploadedAt)
                .ToListAsync();
        }

        public async Task<bool> DeleteByRequestIdAsync(int requestId)
        {
            var attachments = await _context.ReconcileRequestAttachments
                .Where(a => a.ReconcileRequestId == requestId)
                .ToListAsync();

            if (!attachments.Any())
                return false;

            _context.ReconcileRequestAttachments.RemoveRange(attachments);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

