using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.DTOS.MediationDTOs;

namespace BLL.ServiceAbstraction
{
    public interface IMediationService
    {
        Task<List<MediationDTO>> GetAllMediationsAsync(int? year = null);
        Task<MediationDTO> GetMediationByIdAsync(int id, int? year = null);
        Task<MediationDTO> GetMediationByUserIdAsync(string userId);
        Task<MediationDTO> CreateMediationAsync(CreateMediationDTO createMediationDto);
        Task<MediationDTO> UpdateMediationAsync(int id, UpdateMediationDTO updateMediationDto);
        Task<bool> DeleteMediationAsync(int id);
        Task<bool> ToggleActiveAsync(int id);
    }
} 