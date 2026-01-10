using DAL.Data;
using DAL.Data.Models;
using DAL.Data.Models.IdentityModels;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;
using Shared.DTOS.ReconcileRequestDTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories.RepositoryClasses
{
    public class SupervisorRepository : GenericRepository<Supervisor>, ISupervisorRepository
    {
        private readonly ApplicationDbContext _context;

        public SupervisorRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Supervisor> GetSupervisorByUserIdAsync(string userId)
        {
            return await _context.Supervisors
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task<List<Supervisor>> GetAllSupervisorsWithStatsAsync(int? year = null)
        {
            var query = _context.Supervisors
                .Include(s => s.ReconcileRequests)
                .AsQueryable();

            var supervisors = await query.ToListAsync();

            foreach (var supervisor in supervisors)
            {
                var requests = supervisor.ReconcileRequests.AsQueryable();

                if (year.HasValue)
                {
                    requests = requests.Where(r => r.CompletedAt.HasValue && 
                                                   r.CompletedAt.Value.Year == year.Value);
                }

                supervisor.ReconcileRequests = requests.ToList();
            }

            return supervisors;
        }

        public async Task<Supervisor> GetSupervisorByIdWithStatsAsync(int id, int? year = null)
        {
            var supervisor = await _context.Supervisors
                .Include(s => s.ReconcileRequests)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (supervisor != null && year.HasValue)
            {
                supervisor.ReconcileRequests = supervisor.ReconcileRequests
                    .Where(r => r.CompletedAt.HasValue && r.CompletedAt.Value.Year == year.Value)
                    .ToList();
            }

            return supervisor;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Supervisors.AnyAsync(s => s.Id == id);
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            var query = _context.Supervisors.Where(s => s.Email == email);
            
            if (excludeId.HasValue)
            {
                query = query.Where(s => s.Id != excludeId.Value);
            }

            return await query.AnyAsync();
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
    }
}

