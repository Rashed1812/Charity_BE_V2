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
    public class ReconcileRequestTypeRepository : GenericRepository<ReconcileRequestType>, IReconcileRequestTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public ReconcileRequestTypeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ReconcileRequestType>> GetActiveTypesAsync()
        {
            return await _context.ReconcileRequestTypes
                .Where(t => t.IsActive)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ReconcileRequestTypes.AnyAsync(t => t.Id == id);
        }
    }
}

