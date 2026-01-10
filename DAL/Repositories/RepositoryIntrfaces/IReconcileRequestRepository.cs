using DAL.Data.Models;
using DAL.Repositories.GenericRepositries;
using DAL.Repositries.GenericRepositries;
using Shared.DTOS.ReconcileRequestDTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface IReconcileRequestRepository : IGenericRepository<ReconcileRequest>
    {
        Task<List<ReconcileRequest>> GetAllWithDetailsAsync();
        Task<ReconcileRequest> GetByIdWithDetailsAsync(int id);
        Task<List<ReconcileRequest>> GetBySupervisorIdAsync(int supervisorId);
        Task<List<ReconcileRequest>> GetByMediationIdAsync(int mediationId);
        Task<List<ReconcileRequest>> GetByStatusAsync(ReconcileRequestStatus status);
        Task<int> GetCountBySupervisorAndYearAsync(int supervisorId, int year);
        Task<int> GetCountByMediationAndYearAsync(int mediationId, int year);
    }
} 