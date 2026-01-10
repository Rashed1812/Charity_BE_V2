using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.ServiceAbstraction;
using DAL.Data.Models;
using DAL.Data.Models.IdentityModels;
using DAL.Repositories.RepositoryIntrfaces;
using Shared.DTOS.MediationDTOs;
using Shared.DTOS.ReconcileRequestDTOs;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using BLL.Services.FileService;

namespace BLL.Service
{
    public class MediationService : IMediationService
    {
        private readonly IMediationRepository _mediationRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public MediationService(
            IMediationRepository mediationRepository,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _mediationRepository = mediationRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<List<MediationDTO>> GetAllMediationsAsync(int? year = null)
        {
            var mediations = await _mediationRepository.GetAllMediationsWithStatsAsync(year);
            var dtos = _mapper.Map<List<MediationDTO>>(mediations);

            // Calculate statistics for each mediation
            foreach (var dto in dtos)
            {
                var mediation = mediations.FirstOrDefault(m => m.Id == dto.Id);
                if (mediation != null)
                {
                    dto.TotalRequests = mediation.ReconcileRequests?.Count ?? 0;
                    dto.CompletedRequests = mediation.ReconcileRequests?.Count(r => r.Status == ReconcileRequestStatus.Completed) ?? 0;
                    dto.InProgressRequests = mediation.ReconcileRequests?.Count(r => r.Status == ReconcileRequestStatus.InProgress) ?? 0;

                    if (year.HasValue)
                    {
                        dto.CompletedInYear = await _mediationRepository.GetCountByMediationAndYearAsync(dto.Id, year.Value);
                    }
                }
            }

            return dtos;
        }

        public async Task<MediationDTO> GetMediationByIdAsync(int id, int? year = null)
        {
            var mediation = await _mediationRepository.GetMediationByIdWithStatsAsync(id, year);
            if (mediation == null) return null;

            var dto = _mapper.Map<MediationDTO>(mediation);
            dto.TotalRequests = mediation.ReconcileRequests?.Count ?? 0;
            dto.CompletedRequests = mediation.ReconcileRequests?.Count(r => r.Status == ReconcileRequestStatus.Completed) ?? 0;
            dto.InProgressRequests = mediation.ReconcileRequests?.Count(r => r.Status == ReconcileRequestStatus.InProgress) ?? 0;

            if (year.HasValue)
            {
                dto.CompletedInYear = await _mediationRepository.GetCountByMediationAndYearAsync(id, year.Value);
            }

            return dto;
        }

        public async Task<MediationDTO> GetMediationByUserIdAsync(string userId)
        {
            var mediation = await _mediationRepository.GetMediationByUserIdAsync(userId);
            return _mapper.Map<MediationDTO>(mediation);
        }

        public async Task<MediationDTO> CreateMediationAsync(CreateMediationDTO createMediationDto)
        {
            // Check if email is unique
            var existingUser = await _userManager.FindByEmailAsync(createMediationDto.Email);
            if (existingUser != null)
                throw new InvalidOperationException("Email already exists");

            // Create user first
            var user = new ApplicationUser
            {
                UserName = createMediationDto.Email,
                Email = createMediationDto.Email,
                PhoneNumber = createMediationDto.PhoneNumber,
                FullName = createMediationDto.FullName,
                IsActive = true
            };

            var userResult = await _userManager.CreateAsync(user, createMediationDto.Password);
            if (!userResult.Succeeded)
                throw new InvalidOperationException($"Failed to create user: {string.Join(", ", userResult.Errors.Select(e => e.Description))}");

            // Add to Mediation role
            await _userManager.AddToRoleAsync(user, "Mediation");

            // Handle image upload
            string? imgUrl = null;
            if (createMediationDto.Image != null && createMediationDto.Image.Length > 0)
            {
                FileService fileService = new FileService();
                imgUrl = await fileService.UploadFileAsync(createMediationDto.Image, "mediationImage");
            }

            // Validate password confirmation
            if (createMediationDto.Password != createMediationDto.ConfirmPassword)
                throw new InvalidOperationException("كلمة المرور وتأكيد كلمة المرور غير متطابقين");

            // Create mediation
            var mediation = new Mediation
            {
                UserId = user.Id,
                FullName = createMediationDto.FullName,
                Specialty = createMediationDto.Specialty,
                PhoneNumber = createMediationDto.PhoneNumber,
                Email = createMediationDto.Email,
                ImageUrl = imgUrl,
                IsActive = true,
                IsAvailable = true
            };

            var createdMediation = await _mediationRepository.AddAsync(mediation);
            return _mapper.Map<MediationDTO>(createdMediation);
        }

        public async Task<MediationDTO> UpdateMediationAsync(int id, UpdateMediationDTO updateMediationDto)
        {
            var mediation = await _mediationRepository.GetByIdAsync(id);
            if (mediation == null)
                return null;

            if (!string.IsNullOrEmpty(updateMediationDto.FullName))
            {
                mediation.FullName = updateMediationDto.FullName;
                var user = await _userManager.FindByIdAsync(mediation.UserId);
                if (user != null)
                {
                    user.FullName = updateMediationDto.FullName;
                    await _userManager.UpdateAsync(user);
                }
            }
            if (!string.IsNullOrEmpty(updateMediationDto.Specialty))
                mediation.Specialty = updateMediationDto.Specialty;
            if (!string.IsNullOrEmpty(updateMediationDto.PhoneNumber))
            {
                mediation.PhoneNumber = updateMediationDto.PhoneNumber;
                var user = await _userManager.FindByIdAsync(mediation.UserId);
                if (user != null)
                {
                    user.PhoneNumber = updateMediationDto.PhoneNumber;
                    await _userManager.UpdateAsync(user);
                }
            }
            if (!string.IsNullOrEmpty(updateMediationDto.Email))
            {
                mediation.Email = updateMediationDto.Email;
                var user = await _userManager.FindByIdAsync(mediation.UserId);
                if (user != null)
                {
                    user.Email = updateMediationDto.Email;
                    user.UserName = updateMediationDto.Email;
                    await _userManager.UpdateAsync(user);
                }
            }
            if (updateMediationDto.IsActive.HasValue)
                mediation.IsActive = updateMediationDto.IsActive.Value;
            if (updateMediationDto.IsAvailable.HasValue)
                mediation.IsAvailable = updateMediationDto.IsAvailable.Value;
            // Handle image update
            if (updateMediationDto.Image != null && updateMediationDto.Image.Length > 0)
            {
                FileService fileService = new FileService();
                fileService.DeleteFile(mediation.ImageUrl);
                var imgUrl = await fileService.UploadFileAsync(updateMediationDto.Image, "mediationImage");
                mediation.ImageUrl = imgUrl;
            }

            var updatedMediation = await _mediationRepository.UpdateAsync(mediation);
            return _mapper.Map<MediationDTO>(updatedMediation);
        }

        public async Task<bool> DeleteMediationAsync(int id)
        {
            var mediation = await _mediationRepository.GetByIdAsync(id);
            if (mediation == null)
                return false;

            // Delete user
            var user = await _userManager.FindByIdAsync(mediation.UserId);
            if (user != null)
                await _userManager.DeleteAsync(user);

            // Delete mediation
            await _mediationRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> ToggleActiveAsync(int id)
        {
            var mediation = await _mediationRepository.GetByIdAsync(id);
            if (mediation == null)
                return false;

            mediation.IsActive = !mediation.IsActive;
            await _mediationRepository.UpdateAsync(mediation);
            return mediation.IsActive;
        }
    }
} 