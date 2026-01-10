using DAL.Data.Models;
using DAL.Repositries.GenericRepositries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface IReconcileRequestAttachmentRepository : IGenericRepository<ReconcileRequestAttachment>
    {
        Task<List<ReconcileRequestAttachment>> GetByRequestIdAsync(int requestId);
        Task<bool> DeleteByRequestIdAsync(int requestId);
    }
}

