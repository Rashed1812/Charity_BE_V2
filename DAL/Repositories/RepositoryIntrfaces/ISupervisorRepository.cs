using DAL.Data.Models;
using DAL.Data.Models.IdentityModels;
using DAL.Repositries.GenericRepositries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface ISupervisorRepository : IGenericRepository<Supervisor>
    {
        Task<Supervisor> GetSupervisorByUserIdAsync(string userId);
        Task<List<Supervisor>> GetAllSupervisorsWithStatsAsync(int? year = null);
        Task<Supervisor> GetSupervisorByIdWithStatsAsync(int id, int? year = null);
        Task<bool> ExistsAsync(int id);
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);
        Task<int> GetCountBySupervisorAndYearAsync(int supervisorId, int year);
    }
}

