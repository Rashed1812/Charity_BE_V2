using Shared.DTOS.ReconcileRequestDTOs;
using DAL.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.ServiceAbstraction
{
    public interface IReconcileRequestService
    {
        // Basic CRUD
        Task<List<ReconcileRequestDTO>> GetAllAsync();
        Task<ReconcileRequestDTO> GetByIdAsync(int id);
        Task<ReconcileRequestDTO> CreateAsync(CreateReconcileRequestDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<int> GetAllRequestsCount();

        // Admin operations
        Task<ReconcileRequestDTO> AssignToSupervisorAsync(int requestId, int supervisorId);
        Task<ReconcileRequestDTO> CancelRequestAsync(int requestId);
        Task<ReconcileRequestDTO> CompleteRequestAsync(int requestId);

        // Supervisor operations
        Task<List<ReconcileRequestDTO>> GetBySupervisorIdAsync(int supervisorId);
        Task<ReconcileRequestDTO> AssignToMediationAsync(int requestId, int mediationId);
        Task<ReconcileRequestDTO> MarkAsReviewedAsync(int requestId);

        // Mediation operations
        Task<List<ReconcileRequestDTO>> GetByMediationIdAsync(int mediationId);
        Task<ReconcileRequestDTO> StartRequestAsync(int requestId, StartRequestDTO dto);
        Task<ReconcileRequestDTO> CompleteExecutionAsync(int requestId, CompleteRequestDTO dto);
        Task<bool> SendSMSAsync(int requestId, SendSMSDTO dto);
        List<string> GetSMSTemplates();
    }
} 