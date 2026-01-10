using DAL.Data.Models;
using DAL.Repositries.GenericRepositries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface IReconcileRequestTypeRepository : IGenericRepository<ReconcileRequestType>
    {
        Task<List<ReconcileRequestType>> GetActiveTypesAsync();
        Task<bool> ExistsAsync(int id);
    }
}

