using Shared.DTOS.SupervisorDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.ServiceAbstraction
{
    public interface ISupervisorService
    {
        Task<List<SupervisorDTO>> GetAllAsync(int? year = null);
        Task<SupervisorDTO> GetByIdAsync(int id, int? year = null);
        Task<SupervisorDTO> GetByUserIdAsync(string userId);
        Task<SupervisorDTO> CreateAsync(CreateSupervisorDTO dto);
        Task<SupervisorDTO> UpdateAsync(int id, UpdateSupervisorDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ToggleActiveAsync(int id);
    }
}

