using BLL.ServiceAbstraction;
using DAL.Data.Models;
using DAL.Repositories.RepositoryIntrfaces;
using Shared.DTOS.ReconcileRequestTypeDTOs;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace BLL.Service
{
    public class ReconcileRequestTypeService : IReconcileRequestTypeService
    {
        private readonly IReconcileRequestTypeRepository _repository;
        private readonly IMapper _mapper;

        public ReconcileRequestTypeService(IReconcileRequestTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ReconcileRequestTypeDTO>> GetAllAsync()
        {
            var types = await _repository.GetAllAsync();
            return _mapper.Map<List<ReconcileRequestTypeDTO>>(types);
        }

        public async Task<List<ReconcileRequestTypeDTO>> GetActiveTypesAsync()
        {
            var types = await _repository.GetActiveTypesAsync();
            return _mapper.Map<List<ReconcileRequestTypeDTO>>(types);
        }

        public async Task<ReconcileRequestTypeDTO> GetByIdAsync(int id)
        {
            var type = await _repository.GetByIdAsync(id);
            if (type == null) return null;
            return _mapper.Map<ReconcileRequestTypeDTO>(type);
        }

        public async Task<ReconcileRequestTypeDTO> CreateAsync(CreateReconcileRequestTypeDTO dto)
        {
            var entity = _mapper.Map<ReconcileRequestType>(dto);
            entity.CreatedAt = System.DateTime.UtcNow;
            entity.IsActive = true;
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<ReconcileRequestTypeDTO>(created);
        }

        public async Task<ReconcileRequestTypeDTO> UpdateAsync(int id, UpdateReconcileRequestTypeDTO dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            if (!string.IsNullOrEmpty(dto.Name))
                entity.Name = dto.Name;
            
            if (dto.Description != null)
                entity.Description = dto.Description;

            if (dto.IsActive.HasValue)
                entity.IsActive = dto.IsActive.Value;

            entity.UpdatedAt = System.DateTime.UtcNow;
            var updated = await _repository.UpdateAsync(entity);
            return _mapper.Map<ReconcileRequestTypeDTO>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> ToggleActiveAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;
            
            entity.IsActive = !entity.IsActive;
            entity.UpdatedAt = System.DateTime.UtcNow;
            await _repository.UpdateAsync(entity);
            return entity.IsActive;
        }
    }
}

