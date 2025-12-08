using AutoMapper;
using BLL.ServiceAbstraction;
using DAL.Repositories.RepositoryIntrfaces;
using DAL.Data.Models;
using Shared.DTOS.ServiceOfferingDTOs;
using BLL.Services.FileService;

namespace BLL.Service
{
    public class ServiceOfferingService : IServiceOfferingService
    {
        private readonly IServiceOfferingRepository _serviceOfferingRepository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public ServiceOfferingService(
            IServiceOfferingRepository serviceOfferingRepository,
            IMapper mapper,
            IFileService fileService)
        {
            _serviceOfferingRepository = serviceOfferingRepository;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<ServiceOfferingDTO> GetServiceOfferingAsync()
        {
            var service = await _serviceOfferingRepository.GetSingleAsync();
            if (service == null)
                return null;
            return _mapper.Map<ServiceOfferingDTO>(service);
        }

        public async Task<ServiceOfferingDTO> GetServiceOfferingAvaliableAsync()
        {
            var service = await _serviceOfferingRepository.GetSingleAvialblyAsync();
            if (service == null)
                return null;
            return _mapper.Map<ServiceOfferingDTO>(service);
        }

        public async Task<bool> UpdateTitleAndDescriptionAsync(string title, string description)
        {
            return await _serviceOfferingRepository.UpdateTitleAndDescriptionAsync(title, description);
        }

        public async Task<ServiceOfferingDTOItem> AddServiceItemAsync(CreateServiceOfferingDTOItem dto)
        {
            var item = _mapper.Map<ServiceOfferingItem>(dto);
            item.ServiceOfferingId = 1;
            item.CreatedAt = DateTime.UtcNow;
            item.IsActive = dto.IsActive;

            // Handle image upload
            if (dto.Image != null && dto.Image.Length > 0)
            {
                var imgUrl = await _fileService.UploadFileAsync(dto.Image, "serviceOffering");
                item.ImageUrl = imgUrl;
            }

            await _serviceOfferingRepository.AddServiceItemAsync(item);
            return _mapper.Map<ServiceOfferingDTOItem>(item);
        }

        public async Task<ServiceOfferingDTOItem> UpdateServiceItemAsync(int itemId, UpdateServiceOfferingDTOItem dto)
        {
            var item = await _serviceOfferingRepository.GetItemByIdAsync(itemId);
            if (item == null)
                return null;

            // Update properties
            if (!string.IsNullOrEmpty(dto.Name))
                item.Name = dto.Name;

            if (!string.IsNullOrEmpty(dto.Description))
                item.Description = dto.Description;

            if (!string.IsNullOrEmpty(dto.Url))
                item.Url = dto.Url;

            if (dto.IsActive.HasValue)
                item.IsActive = dto.IsActive.Value;

            // Handle image upload if new image is provided
            if (dto.Image != null && dto.Image.Length > 0)
            {
                // Delete old image if exists
                if (!string.IsNullOrEmpty(item.ImageUrl))
                {
                    _fileService.DeleteFile(item.ImageUrl);
                }

                var imgUrl = await _fileService.UploadFileAsync(dto.Image, "serviceOffering");
                item.ImageUrl = imgUrl;
            }

            item.UpdatedAt = DateTime.UtcNow;

            await _serviceOfferingRepository.UpdateServiceItemAsync(item);
            return _mapper.Map<ServiceOfferingDTOItem>(item);
        }

        public async Task<bool> DeleteServiceItemAsync(int itemId)
        {
            var item = await _serviceOfferingRepository.GetItemByIdAsync(itemId);
            if (item == null)
                return false;

            // Delete image file if exists
            if (!string.IsNullOrEmpty(item.ImageUrl))
            {
                _fileService.DeleteFile(item.ImageUrl);
            }

            await _serviceOfferingRepository.DeleteServiceItemAsync(item);
            return true;
        }

        public async Task<List<ServiceOfferingDTOItem>> GetServiceItemsAsync()
        {
            var items = await _serviceOfferingRepository.GetServiceItemsAsync();
            return _mapper.Map<List<ServiceOfferingDTOItem>>(items);
        }

        public async Task<int> GetTotalServicesCountAsync()
        {
            var items = await _serviceOfferingRepository.GetServiceItemsAsync();
            return items.Count;
        }
    }
} 