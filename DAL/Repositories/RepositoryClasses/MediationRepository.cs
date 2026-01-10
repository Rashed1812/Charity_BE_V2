using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data;
using DAL.Data.Models;
using DAL.Data.Models.IdentityModels;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;
using Shared.DTOS.ReconcileRequestDTOs;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.RepositoryClasses
{
    public class MediationRepository : GenericRepository<Mediation>, IMediationRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public MediationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Mediation> GetMediationByUserIdAsync(string userId)
        {
            return await _dbContext.Mediations.Include(m => m.User).FirstOrDefaultAsync(m => m.UserId == userId);
        }

        public async Task<List<Mediation>> GetAllMediationsWithRelatedDataAsync()
        {
            return await _dbContext.Mediations.Include(m => m.User).ToListAsync();
        }

        public async Task<Mediation> GetMediationByIdWithRelatedDataAsync(int id)
        {
            return await _dbContext.Mediations.Include(m => m.User).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Mediation>> GetAllMediationsWithStatsAsync(int? year = null)
        {
            var query = _dbContext.Mediations
                .Include(m => m.User)
                .Include(m => m.ReconcileRequests)
                .AsQueryable();

            var mediations = await query.ToListAsync();

            foreach (var mediation in mediations)
            {
                var requests = mediation.ReconcileRequests.AsQueryable();

                if (year.HasValue)
                {
                    requests = requests.Where(r => r.CompletedAt.HasValue && 
                                                   r.CompletedAt.Value.Year == year.Value);
                }

                mediation.ReconcileRequests = requests.ToList();
            }

            return mediations;
        }

        public async Task<Mediation> GetMediationByIdWithStatsAsync(int id, int? year = null)
        {
            var mediation = await _dbContext.Mediations
                .Include(m => m.User)
                .Include(m => m.ReconcileRequests)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mediation != null && year.HasValue)
            {
                mediation.ReconcileRequests = mediation.ReconcileRequests
                    .Where(r => r.CompletedAt.HasValue && r.CompletedAt.Value.Year == year.Value)
                    .ToList();
            }

            return mediation;
        }

        public async Task<int> GetCountByMediationAndYearAsync(int mediationId, int year)
        {
            return await _dbContext.ReconcileRequests
                .Where(r => r.MediationId == mediationId &&
                           r.Status == ReconcileRequestStatus.Completed &&
                           r.CompletedAt.HasValue &&
                           r.CompletedAt.Value.Year == year)
                .CountAsync();
        }
    }
} 