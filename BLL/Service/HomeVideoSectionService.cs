using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.ServiceAbstraction;
using DAL.Data.Models.HomePage;
using DAL.Repositories.RepositoryIntrfaces;
using Shared.DTOS.HomePageDTOS;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BLL.Service
{
    public class HomeVideoSectionService : IHomeVideoSectionService
    {
        private readonly IHomeVideoSectionRepository _homeVideoSection;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _uploadPath;

        public HomeVideoSectionService(
            IHomeVideoSectionRepository homeVideoSection,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment)
        {
            _homeVideoSection = homeVideoSection;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "home-videos");

            // Create upload directory if it doesn't exist
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        public async Task<HomeVideoSectionDTO> GetHomeVideoSectionAsync()
        {
            var homeVideoSection = await _homeVideoSection.GetVideoUrlAsync();
            if (homeVideoSection == null)
                return null;
            var homeVideoSectionDTO = _mapper.Map<HomeVideoSectionDTO>(homeVideoSection);
            return homeVideoSectionDTO;
        }

        public async Task<bool> UpdateHomeVideoSectionAsync(UpdateHomeVideoSectionDTO updateHomeVideoSectionDTO)
        {
            // Get existing video section to check for old file
            var existingSection = await _homeVideoSection.GetVideoUrlAsync();
            
            string videoUrl = null;

            // Handle video file upload if provided
            if (updateHomeVideoSectionDTO.VideoFile != null && updateHomeVideoSectionDTO.VideoFile.Length > 0)
            {
                // Validate video file - only MP4 allowed
                var allowedExtensions = new[] { ".mp4" };
                var extension = Path.GetExtension(updateHomeVideoSectionDTO.VideoFile.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(extension))
                    throw new ArgumentException("Invalid video file type. Only MP4 files are allowed.");

                // Delete old video file if exists
                if (existingSection != null && !string.IsNullOrEmpty(existingSection.VideoUrl) && existingSection.VideoUrl.StartsWith("/uploads/home-videos/"))
                {
                    var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, existingSection.VideoUrl.TrimStart('/'));
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                }

                // Generate unique filename
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(_uploadPath, fileName);

                // Save the new file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await updateHomeVideoSectionDTO.VideoFile.CopyToAsync(stream);
                }

                // Set video URL
                videoUrl = $"/uploads/home-videos/{fileName}";
            }
            else if (existingSection != null)
            {
                // Keep existing video URL if no new file is provided
                videoUrl = existingSection.VideoUrl;
            }

            var homeVideoSection = new HomeVideoSection
            {
                Title = updateHomeVideoSectionDTO.Title,
                Description = updateHomeVideoSectionDTO.Description,
                VideoUrl = videoUrl
            };
            var updatedSection = await _homeVideoSection.UpdateVideoUrlAsync(homeVideoSection);
            return updatedSection != null;
        }
    }
}
