using AutoMapper;
using BLL.ServiceAbstraction;
using DAL.Data.Models;
using DAL.Repositories.RepositoryIntrfaces;
using Shared.DTOS.VideosLibraryDTOs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BLL.Service
{
    public class VideosLibraryService : IVideosLibraryService
    {
        private readonly IVideosLibraryRepository _videosLibraryRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _uploadPath;

        public VideosLibraryService(
            IVideosLibraryRepository videosLibraryRepository,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment)
        {
            _videosLibraryRepository = videosLibraryRepository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "videos");

            // Create upload directory if it doesn't exist
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        public async Task<VideosLibraryDTO> CreateVideoAsync(CreateVideosLibraryDTO dto)
        {
            var videoEntity = _mapper.Map<VideosLibrary>(dto);
            videoEntity.CreatedAt = DateTime.UtcNow;
            videoEntity.IsActive = true;

            // Handle video file upload - VideoFile is now required
            if (dto.VideoFile == null || dto.VideoFile.Length == 0)
                throw new ArgumentException("Video file is required. Only MP4 file uploads are allowed.");

            // Validate video file
            var allowedExtensions = new[] { ".mp4" };
            var extension = Path.GetExtension(dto.VideoFile.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException("Invalid video file type. Only MP4 files are allowed.");

            // Generate unique filename
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(_uploadPath, fileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.VideoFile.CopyToAsync(stream);
            }

            // Set video URL
            videoEntity.VideoUrl = $"/uploads/videos/{fileName}";

            var result = await _videosLibraryRepository.AddAsync(videoEntity);
            return _mapper.Map<VideosLibraryDTO>(result);
        }

        public async Task<List<VideosLibraryDTO>> GetAllVideosAsync()
        {
            var videos = await _videosLibraryRepository.GetAllAsync();
            return _mapper.Map<List<VideosLibraryDTO>>(videos);
        }

        public async Task<VideosLibraryDTO> GetVideoByIdAsync(int id)
        {
            var video = await _videosLibraryRepository.GetByIdAsync(id);
            return video == null ? null : _mapper.Map<VideosLibraryDTO>(video);
        }

        public async Task<bool> UpdateVideoAsync(int id, UpdateVideosLibraryDTO updateDTO)
        {
            var video = await _videosLibraryRepository.GetByIdAsync(id);
            if (video == null) return false;

            video.Name = updateDTO.Name ?? video.Name;
            video.Description = updateDTO.Description ?? video.Description;
            video.IsActive = updateDTO.IsActive;

            // Handle video file upload - VideoFile is optional in update, but if provided, it's required
            if (updateDTO.VideoFile != null && updateDTO.VideoFile.Length > 0)
            {
                // Validate video file
                var allowedExtensions = new[] { ".mp4" };
                var extension = Path.GetExtension(updateDTO.VideoFile.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(extension))
                    throw new ArgumentException("Invalid video file type. Only MP4 files are allowed.");

                // Delete old video file if exists
                if (!string.IsNullOrEmpty(video.VideoUrl) && video.VideoUrl.StartsWith("/uploads/videos/"))
                {
                    var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, video.VideoUrl.TrimStart('/'));
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
                    await updateDTO.VideoFile.CopyToAsync(stream);
                }

                // Set new video URL
                video.VideoUrl = $"/uploads/videos/{fileName}";
            }
            // Note: VideoUrl is no longer accepted as input - only file uploads are allowed

            await _videosLibraryRepository.UpdateAsync(video);
            return true;
        }

        public async Task<bool> DeleteVideoAsync(int id)
        {
            var video = await _videosLibraryRepository.GetByIdAsync(id);
            if (video == null) return false;

            // Delete video file if exists
            if (!string.IsNullOrEmpty(video.VideoUrl) && video.VideoUrl.StartsWith("/uploads/videos/"))
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, video.VideoUrl.TrimStart('/'));
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            return await _videosLibraryRepository.DeleteAsync(id);
        }

        public async Task<List<VideosLibraryDTO>> GetActiveVideosAsync()
        {
            var videos = await _videosLibraryRepository.GetAllAsync();
            return videos
                .Where(v => v.IsActive)
                .Select(v => _mapper.Map<VideosLibraryDTO>(v))
                .ToList();
        }
    }
}
