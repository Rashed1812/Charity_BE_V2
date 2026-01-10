using Shared.DTOS.ReconcileRequestTypeDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.ServiceAbstraction
{
    public interface IReconcileRequestTypeService
    {
        Task<List<ReconcileRequestTypeDTO>> GetAllAsync();
        Task<List<ReconcileRequestTypeDTO>> GetActiveTypesAsync();
        Task<ReconcileRequestTypeDTO> GetByIdAsync(int id);
        Task<ReconcileRequestTypeDTO> CreateAsync(CreateReconcileRequestTypeDTO dto);
        Task<ReconcileRequestTypeDTO> UpdateAsync(int id, UpdateReconcileRequestTypeDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ToggleActiveAsync(int id);
    }
}

