using DAL.Data;
using DAL.Data.Models;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;
using Shared.DTOS.ReconcileRequestDTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories.RepositoryClasses
{
    public class ReconcileRequestRepository : GenericRepository<ReconcileRequest>, IReconcileRequestRepository
    {
        private readonly ApplicationDbContext _context;
        public ReconcileRequestRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ReconcileRequest>> GetAllWithDetailsAsync()
        {
            return await _context.ReconcileRequests
                .Include(r => r.ReconcileRequestType)
                .Include(r => r.Supervisor)
                .Include(r => r.Mediation)
                .Include(r => r.Attachments)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<ReconcileRequest> GetByIdWithDetailsAsync(int id)
        {
            return await _context.ReconcileRequests
                .Include(r => r.ReconcileRequestType)
                .Include(r => r.Supervisor)
                .Include(r => r.Mediation)
                .Include(r => r.Attachments)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<ReconcileRequest>> GetBySupervisorIdAsync(int supervisorId)
        {
            return await _context.ReconcileRequests
                .Include(r => r.ReconcileRequestType)
                .Include(r => r.Mediation)
                .Include(r => r.Attachments)
                .Where(r => r.SupervisorId == supervisorId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<ReconcileRequest>> GetByMediationIdAsync(int mediationId)
        {
            return await _context.ReconcileRequests
                .Include(r => r.ReconcileRequestType)
                .Include(r => r.Supervisor)
                .Include(r => r.Attachments)
                .Where(r => r.MediationId == mediationId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<ReconcileRequest>> GetByStatusAsync(ReconcileRequestStatus status)
        {
            return await _context.ReconcileRequests
                .Include(r => r.ReconcileRequestType)
                .Include(r => r.Supervisor)
                .Include(r => r.Mediation)
                .Include(r => r.Attachments)
                .Where(r => r.Status == status)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetCountBySupervisorAndYearAsync(int supervisorId, int year)
        {
            return await _context.ReconcileRequests
                .Where(r => r.SupervisorId == supervisorId &&
                           r.Status == ReconcileRequestStatus.Completed &&
                           r.CompletedAt.HasValue &&
                           r.CompletedAt.Value.Year == year)
                .CountAsync();
        }

        public async Task<int> GetCountByMediationAndYearAsync(int mediationId, int year)
        {
            return await _context.ReconcileRequests
                .Where(r => r.MediationId == mediationId &&
                           r.Status == ReconcileRequestStatus.Completed &&
                           r.CompletedAt.HasValue &&
                           r.CompletedAt.Value.Year == year)
                .CountAsync();
        }
    }
} 