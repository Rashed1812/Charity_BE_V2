using BLL.ServiceAbstraction;
using DAL.Data.Models;
using DAL.Data.Models.IdentityModels;
using DAL.Repositories.RepositoryIntrfaces;
using Shared.DTOS.SupervisorDTOs;
using Shared.DTOS.ReconcileRequestDTOs;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class SupervisorService : ISupervisorService
    {
        private readonly ISupervisorRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public SupervisorService(
            ISupervisorRepository repository,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _repository = repository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<List<SupervisorDTO>> GetAllAsync(int? year = null)
        {
            var supervisors = await _repository.GetAllSupervisorsWithStatsAsync(year);
            var dtos = _mapper.Map<List<SupervisorDTO>>(supervisors);

            // Calculate statistics for each supervisor
            foreach (var dto in dtos)
            {
                var supervisor = supervisors.FirstOrDefault(s => s.Id == dto.Id);
                if (supervisor != null)
                {
                    dto.TotalRequests = supervisor.ReconcileRequests?.Count ?? 0;
                    dto.CompletedRequests = supervisor.ReconcileRequests?.Count(r => r.Status == ReconcileRequestStatus.Completed) ?? 0;
                    dto.InProgressRequests = supervisor.ReconcileRequests?.Count(r => 
                        r.Status == ReconcileRequestStatus.InProgress || 
                        r.Status == ReconcileRequestStatus.AssignedToMediation) ?? 0;

                    if (year.HasValue)
                    {
                        dto.CompletedInYear = await _repository.GetCountBySupervisorAndYearAsync(dto.Id, year.Value);
                    }
                }
            }

            return dtos;
        }

        public async Task<SupervisorDTO> GetByIdAsync(int id, int? year = null)
        {
            var supervisor = await _repository.GetSupervisorByIdWithStatsAsync(id, year);
            if (supervisor == null) return null;

            var dto = _mapper.Map<SupervisorDTO>(supervisor);
            dto.TotalRequests = supervisor.ReconcileRequests?.Count ?? 0;
            dto.CompletedRequests = supervisor.ReconcileRequests?.Count(r => r.Status == ReconcileRequestStatus.Completed) ?? 0;
            dto.InProgressRequests = supervisor.ReconcileRequests?.Count(r => 
                r.Status == ReconcileRequestStatus.InProgress || 
                r.Status == ReconcileRequestStatus.AssignedToMediation) ?? 0;

            if (year.HasValue)
            {
                dto.CompletedInYear = await _repository.GetCountBySupervisorAndYearAsync(id, year.Value);
            }

            return dto;
        }

        public async Task<SupervisorDTO> GetByUserIdAsync(string userId)
        {
            var supervisor = await _repository.GetSupervisorByUserIdAsync(userId);
            if (supervisor == null) return null;

            return await GetByIdAsync(supervisor.Id);
        }

        public async Task<SupervisorDTO> CreateAsync(CreateSupervisorDTO dto)
        {
            // Check if email already exists
            if (await _repository.EmailExistsAsync(dto.Email))
                throw new InvalidOperationException("البريد الإلكتروني مستخدم بالفعل");

            // Check if email exists in users
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new InvalidOperationException("البريد الإلكتروني مستخدم بالفعل");

            // Create user
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                FullName = dto.FullName,
                IsActive = true,
                EmailConfirmed = true
            };

            var userResult = await _userManager.CreateAsync(user, dto.Password);
            if (!userResult.Succeeded)
                throw new InvalidOperationException($"فشل في إنشاء المستخدم: {string.Join(", ", userResult.Errors.Select(e => e.Description))}");

            // Add to Supervisor role
            await _userManager.AddToRoleAsync(user, "Supervisor");

            // Create supervisor
            var supervisor = new Supervisor
            {
                UserId = user.Id,
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Specialty = dto.Specialty,
                IsActive = true,
                CreatedAt = System.DateTime.UtcNow
            };

            var created = await _repository.AddAsync(supervisor);
            return _mapper.Map<SupervisorDTO>(created);
        }

        public async Task<SupervisorDTO> UpdateAsync(int id, UpdateSupervisorDTO dto)
        {
            var supervisor = await _repository.GetByIdAsync(id);
            if (supervisor == null) return null;

            // Check email uniqueness if email is being updated
            if (!string.IsNullOrEmpty(dto.Email) && dto.Email != supervisor.Email)
            {
                if (await _repository.EmailExistsAsync(dto.Email, id))
                    throw new InvalidOperationException("البريد الإلكتروني مستخدم بالفعل");

                var existingUser = await _userManager.FindByEmailAsync(dto.Email);
                if (existingUser != null && existingUser.Id != supervisor.UserId)
                    throw new InvalidOperationException("البريد الإلكتروني مستخدم بالفعل");
            }

            // Update supervisor properties
            if (!string.IsNullOrEmpty(dto.FullName))
            {
                supervisor.FullName = dto.FullName;
                var user = await _userManager.FindByIdAsync(supervisor.UserId);
                if (user != null)
                {
                    user.FullName = dto.FullName;
                    await _userManager.UpdateAsync(user);
                }
            }

            if (!string.IsNullOrEmpty(dto.Email))
            {
                supervisor.Email = dto.Email;
                var user = await _userManager.FindByIdAsync(supervisor.UserId);
                if (user != null)
                {
                    user.Email = dto.Email;
                    user.UserName = dto.Email;
                    await _userManager.UpdateAsync(user);
                }
            }

            if (!string.IsNullOrEmpty(dto.PhoneNumber))
            {
                supervisor.PhoneNumber = dto.PhoneNumber;
                var user = await _userManager.FindByIdAsync(supervisor.UserId);
                if (user != null)
                {
                    user.PhoneNumber = dto.PhoneNumber;
                    await _userManager.UpdateAsync(user);
                }
            }

            if (!string.IsNullOrEmpty(dto.Specialty))
                supervisor.Specialty = dto.Specialty;

            if (dto.IsActive.HasValue)
                supervisor.IsActive = dto.IsActive.Value;

            supervisor.UpdatedAt = System.DateTime.UtcNow;
            var updated = await _repository.UpdateAsync(supervisor);
            return _mapper.Map<SupervisorDTO>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var supervisor = await _repository.GetByIdAsync(id);
            if (supervisor == null) return false;

            // Delete user
            var user = await _userManager.FindByIdAsync(supervisor.UserId);
            if (user != null)
                await _userManager.DeleteAsync(user);

            // Delete supervisor
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> ToggleActiveAsync(int id)
        {
            var supervisor = await _repository.GetByIdAsync(id);
            if (supervisor == null) return false;

            supervisor.IsActive = !supervisor.IsActive;
            supervisor.UpdatedAt = System.DateTime.UtcNow;
            await _repository.UpdateAsync(supervisor);
            return supervisor.IsActive;
        }
    }
}

