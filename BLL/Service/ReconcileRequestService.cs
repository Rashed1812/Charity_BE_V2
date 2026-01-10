using BLL.ServiceAbstraction;
using DAL.Repositories.RepositoryIntrfaces;
using Shared.DTOS.ReconcileRequestDTOs;
using DAL.Data.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using BLL.Services.FileService;
using System;

namespace BLL.Service
{
    public class ReconcileRequestService : IReconcileRequestService
    {
        private readonly IReconcileRequestRepository _repository;
        private readonly IReconcileRequestTypeRepository _typeRepository;
        private readonly IReconcileRequestAttachmentRepository _attachmentRepository;
        private readonly IFileService _fileService;
        private readonly ISMSService _smsService;
        private readonly IMapper _mapper;

        public ReconcileRequestService(
            IReconcileRequestRepository repository,
            IReconcileRequestTypeRepository typeRepository,
            IReconcileRequestAttachmentRepository attachmentRepository,
            IFileService fileService,
            ISMSService smsService,
            IMapper mapper)
        {
            _repository = repository;
            _typeRepository = typeRepository;
            _attachmentRepository = attachmentRepository;
            _fileService = fileService;
            _smsService = smsService;
            _mapper = mapper;
        }

        public async Task<List<ReconcileRequestDTO>> GetAllAsync()
        {
            var entities = await _repository.GetAllWithDetailsAsync();
            return _mapper.Map<List<ReconcileRequestDTO>>(entities);
        }

        public async Task<ReconcileRequestDTO> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdWithDetailsAsync(id);
            if (entity == null) return null;
            return _mapper.Map<ReconcileRequestDTO>(entity);
        }

        public async Task<ReconcileRequestDTO> CreateAsync(CreateReconcileRequestDTO dto)
        {
            // Validate request type exists
            var requestType = await _typeRepository.GetByIdAsync(dto.ReconcileRequestTypeId);
            if (requestType == null || !requestType.IsActive)
                throw new InvalidOperationException("نوع الاستشارة غير موجود أو غير نشط");

            // Create request
            var entity = _mapper.Map<ReconcileRequest>(dto);
            entity.Status = ReconcileRequestStatus.NewRequest;
            entity.CreatedAt = System.DateTime.UtcNow;

            var created = await _repository.AddAsync(entity);

            // Handle attachments
            if (dto.Attachments != null && dto.Attachments.Any())
            {
                await HandleAttachmentsAsync(created.Id, dto.Attachments);
                created = await _repository.GetByIdWithDetailsAsync(created.Id);
            }

            return _mapper.Map<ReconcileRequestDTO>(created);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdWithDetailsAsync(id);
            if (entity == null) return false;

            // Delete attachments
            if (entity.Attachments.Any())
            {
                foreach (var attachment in entity.Attachments)
                {
                    _fileService.DeleteFile(attachment.FileUrl);
                }
                await _attachmentRepository.DeleteByRequestIdAsync(id);
            }

            return await _repository.DeleteAsync(id);
        }

        public async Task<int> GetAllRequestsCount()
        {
            return await _repository.CountAsync();
        }

        // Admin operations
        public async Task<ReconcileRequestDTO> AssignToSupervisorAsync(int requestId, int supervisorId)
        {
            var entity = await _repository.GetByIdWithDetailsAsync(requestId);
            if (entity == null)
                throw new InvalidOperationException("الطلب غير موجود");

            if (entity.Status != ReconcileRequestStatus.NewRequest && entity.Status != ReconcileRequestStatus.SupervisorReviewed)
                throw new InvalidOperationException("لا يمكن إحالة الطلب في هذه الحالة");

            entity.SupervisorId = supervisorId;
            entity.Status = ReconcileRequestStatus.AssignedToSupervisor;
            entity.AssignedToSupervisorAt = System.DateTime.UtcNow;
            entity.UpdatedAt = System.DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(entity);
            return _mapper.Map<ReconcileRequestDTO>(await _repository.GetByIdWithDetailsAsync(updated.Id));
        }

        public async Task<ReconcileRequestDTO> CancelRequestAsync(int requestId)
        {
            var entity = await _repository.GetByIdWithDetailsAsync(requestId);
            if (entity == null)
                throw new InvalidOperationException("الطلب غير موجود");

            if (entity.Status == ReconcileRequestStatus.Completed)
                throw new InvalidOperationException("لا يمكن إلغاء طلب مكتمل");

            entity.Status = ReconcileRequestStatus.Cancelled;
            entity.UpdatedAt = System.DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(entity);
            return _mapper.Map<ReconcileRequestDTO>(await _repository.GetByIdWithDetailsAsync(updated.Id));
        }

        public async Task<ReconcileRequestDTO> CompleteRequestAsync(int requestId)
        {
            var entity = await _repository.GetByIdWithDetailsAsync(requestId);
            if (entity == null)
                throw new InvalidOperationException("الطلب غير موجود");

            if (entity.Status != ReconcileRequestStatus.SupervisorReviewed)
                throw new InvalidOperationException("يجب مراجعة المشرف أولاً قبل إكمال الطلب");

            entity.Status = ReconcileRequestStatus.Completed;
            entity.CompletedAt = System.DateTime.UtcNow;
            entity.UpdatedAt = System.DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(entity);
            return _mapper.Map<ReconcileRequestDTO>(await _repository.GetByIdWithDetailsAsync(updated.Id));
        }

        // Supervisor operations
        public async Task<List<ReconcileRequestDTO>> GetBySupervisorIdAsync(int supervisorId)
        {
            var entities = await _repository.GetBySupervisorIdAsync(supervisorId);
            return _mapper.Map<List<ReconcileRequestDTO>>(entities);
        }

        public async Task<ReconcileRequestDTO> AssignToMediationAsync(int requestId, int mediationId)
        {
            var entity = await _repository.GetByIdWithDetailsAsync(requestId);
            if (entity == null)
                throw new InvalidOperationException("الطلب غير موجود");

            if (entity.Status != ReconcileRequestStatus.AssignedToSupervisor)
                throw new InvalidOperationException("يجب أن يكون الطلب محالاً للمشرف أولاً");

            entity.MediationId = mediationId;
            entity.Status = ReconcileRequestStatus.AssignedToMediation;
            entity.AssignedToMediationAt = System.DateTime.UtcNow;
            entity.UpdatedAt = System.DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(entity);
            return _mapper.Map<ReconcileRequestDTO>(await _repository.GetByIdWithDetailsAsync(updated.Id));
        }

        public async Task<ReconcileRequestDTO> MarkAsReviewedAsync(int requestId)
        {
            var entity = await _repository.GetByIdWithDetailsAsync(requestId);
            if (entity == null)
                throw new InvalidOperationException("الطلب غير موجود");

            if (entity.Status != ReconcileRequestStatus.PendingSupervisorReview)
                throw new InvalidOperationException("الطلب ليس في حالة انتظار مراجعة المشرف");

            entity.Status = ReconcileRequestStatus.SupervisorReviewed;
            entity.UpdatedAt = System.DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(entity);
            return _mapper.Map<ReconcileRequestDTO>(await _repository.GetByIdWithDetailsAsync(updated.Id));
        }

        // Mediation operations
        public async Task<List<ReconcileRequestDTO>> GetByMediationIdAsync(int mediationId)
        {
            var entities = await _repository.GetByMediationIdAsync(mediationId);
            return _mapper.Map<List<ReconcileRequestDTO>>(entities);
        }

        public async Task<ReconcileRequestDTO> StartRequestAsync(int requestId, StartRequestDTO dto)
        {
            var entity = await _repository.GetByIdWithDetailsAsync(requestId);
            if (entity == null)
                throw new InvalidOperationException("الطلب غير موجود");

            if (entity.Status != ReconcileRequestStatus.AssignedToMediation)
                throw new InvalidOperationException("يجب أن يكون الطلب محالاً للمستشار أولاً");

            entity.Status = ReconcileRequestStatus.InProgress;
            entity.StartedAt = System.DateTime.UtcNow;
            entity.UpdatedAt = System.DateTime.UtcNow;

            if (!string.IsNullOrEmpty(dto.ConsultantNotes))
                entity.ConsultantNotes = dto.ConsultantNotes;

            var updated = await _repository.UpdateAsync(entity);
            return _mapper.Map<ReconcileRequestDTO>(await _repository.GetByIdWithDetailsAsync(updated.Id));
        }

        public async Task<ReconcileRequestDTO> CompleteExecutionAsync(int requestId, CompleteRequestDTO dto)
        {
            var entity = await _repository.GetByIdWithDetailsAsync(requestId);
            if (entity == null)
                throw new InvalidOperationException("الطلب غير موجود");

            if (entity.Status != ReconcileRequestStatus.InProgress)
                throw new InvalidOperationException("يجب أن يكون الطلب قيد التنفيذ أولاً");

            entity.Status = ReconcileRequestStatus.PendingSupervisorReview;
            entity.UpdatedAt = System.DateTime.UtcNow;

            if (!string.IsNullOrEmpty(dto.ConsultantNotes))
            {
                var existingNotes = entity.ConsultantNotes ?? "";
                entity.ConsultantNotes = string.IsNullOrEmpty(existingNotes) 
                    ? dto.ConsultantNotes 
                    : existingNotes + "\n---\n" + dto.ConsultantNotes;
            }

            var updated = await _repository.UpdateAsync(entity);
            return _mapper.Map<ReconcileRequestDTO>(await _repository.GetByIdWithDetailsAsync(updated.Id));
        }

        public async Task<bool> SendSMSAsync(int requestId, SendSMSDTO dto)
        {
            var entity = await _repository.GetByIdWithDetailsAsync(requestId);
            if (entity == null)
                throw new InvalidOperationException($"طلب إصلاح ذات البين برقم {requestId} غير موجود");

            // Validate that the request is assigned to a mediation and in progress
            if (entity.MediationId == null)
                throw new InvalidOperationException("الطلب غير محال إلى مستشار بعد");

            if (string.IsNullOrWhiteSpace(entity.PhoneNumber))
                throw new InvalidOperationException("رقم الهاتف غير متوفر في بيانات الطلب");

            if (string.IsNullOrWhiteSpace(dto.Message))
                throw new ArgumentException("نص الرسالة مطلوب", nameof(dto.Message));

            try
            {
                // Send SMS to the requester
                var result = await _smsService.SendSMSAsync(entity.PhoneNumber, dto.Message);
                
                if (!result)
                    throw new InvalidOperationException("فشل إرسال الرسالة. يرجى المحاولة مرة أخرى");

                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"خطأ في إرسال الرسالة: {ex.Message}", ex);
            }
        }

        private async Task HandleAttachmentsAsync(int requestId, List<IFormFile> attachments)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".pdf" };
            const long maxFileSize = 50 * 1024 * 1024; // 50MB

            foreach (var file in attachments)
            {
                if (file == null || file.Length == 0)
                    continue;

                // Validate file size
                if (file.Length > maxFileSize)
                    throw new InvalidOperationException($"حجم الملف {file.FileName} أكبر من 50 ميجابايت");

                // Validate file extension
                var extension = System.IO.Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                    throw new InvalidOperationException($"نوع الملف {file.FileName} غير مسموح. الأنواع المسموحة: صور، PDF");

                // Upload file
                var folderName = extension == ".pdf" ? "reconcileRequestFiles" : "reconcileRequestImages";
                var fileUrl = await _fileService.UploadFileAsync(file, folderName);

                // Create attachment record
                var attachment = new ReconcileRequestAttachment
                {
                    ReconcileRequestId = requestId,
                    FileUrl = fileUrl,
                    FileName = file.FileName,
                    FileType = extension == ".pdf" ? "pdf" : "image",
                    FileSize = file.Length,
                    UploadedAt = System.DateTime.UtcNow
                };

                await _attachmentRepository.AddAsync(attachment);
            }
        }

        public List<string> GetSMSTemplates()
        {
            return _smsService.GetSMSTemplates();
        }
    }
}