using AutoMapper;
using BLL.ServiceAbstraction;
using DAL.Repositories.RepositoryIntrfaces;
using DAL.Data.Models;
using Shared.DTOS.LectureDTOs;
using Shared.DTOS.Common;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace BLL.Service
{
    public class LectureService : ILectureService
    {
        private readonly ILectureRepository _lectureRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly string _uploadPath;

        public LectureService(
            ILectureRepository lectureRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IWebHostEnvironment environment)
        {
            _lectureRepository = lectureRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _environment = environment;
            _uploadPath = Path.Combine(_environment.WebRootPath, "uploads", "lectures");

            // Create upload directory if it doesn't exist
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        public async Task<List<LectureDTO>> GetAllLecturesAsync()
        {
            var lectures = await _lectureRepository.GetAllAsync();
            return _mapper.Map<List<LectureDTO>>(lectures);
        }

        public async Task<List<LectureDTO>> GetPublishedLecturesAsync()
        {
            var lectures = await _lectureRepository.GetPublishedLecturesAsync();
            return _mapper.Map<List<LectureDTO>>(lectures);
        }

        public async Task<LectureDTO> GetLectureByIdAsync(int id)
        {
            var lecture = await _lectureRepository.GetByIdAsync(id);
            return _mapper.Map<LectureDTO>(lecture);
        }

        public async Task<LectureDTO> CreateLectureAsync( CreateLectureDTO createLectureDto)
        {
            // Validate and upload video file
            if (createLectureDto.VideoFile == null || createLectureDto.VideoFile.Length == 0)
                throw new ArgumentException("Video file is required.");

            if (!await ValidateVideoFileAsync(createLectureDto.VideoFile))
                throw new ArgumentException("Invalid video file. Only MP4 files up to 500MB are allowed.");

            // Generate unique filename
            var extension = Path.GetExtension(createLectureDto.VideoFile.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(_uploadPath, fileName);

            // Save the video file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await createLectureDto.VideoFile.CopyToAsync(stream);
            }

            // Create lecture with video URL
            var lecture = new Lecture
            {
                Title = createLectureDto.Title,
                Description = createLectureDto.Description,
                VideoUrl = $"/uploads/lectures/{fileName}",
                CreatedAt = DateTime.UtcNow,
            };

            if (lecture.IsPublished)
                lecture.PublishedAt = DateTime.UtcNow;

            var createdLecture = await _lectureRepository.AddAsync(lecture);
            return _mapper.Map<LectureDTO>(createdLecture);
        }

        public async Task<LectureDTO> UploadVideoAsync(string adminId, LectureUploadDTO uploadDto)
        {
            // Validate and upload video file
            if (uploadDto.VideoFile == null || uploadDto.VideoFile.Length == 0)
                throw new ArgumentException("Video file is required.");

            if (!await ValidateVideoFileAsync(uploadDto.VideoFile))
                throw new ArgumentException("Invalid video file. Only MP4 files up to 500MB are allowed.");

            // Generate unique filename
            var extension = Path.GetExtension(uploadDto.VideoFile.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(_uploadPath, fileName);

            // Save the video file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await uploadDto.VideoFile.CopyToAsync(stream);
            }

            // Create lecture with video URL
            var lecture = new Lecture
            {
                Title = uploadDto.Title,
                Description = uploadDto.Description,
                VideoUrl = $"/uploads/lectures/{fileName}",
                CreatedAt = DateTime.UtcNow,
            };

            var createdLecture = await _lectureRepository.AddAsync(lecture);
            return _mapper.Map<LectureDTO>(createdLecture);
        }

        public async Task<bool> ValidateVideoFileAsync(IFormFile videoFile)
        {
            if (videoFile == null || videoFile.Length == 0)
                return false;

            var allowedExtensions = new[] { ".mp4" };
            var extension = Path.GetExtension(videoFile.FileName).ToLowerInvariant();

            return allowedExtensions.Contains(extension) && videoFile.Length <= 500 * 1024 * 1024; // 500MB max
        }

        public async Task<LectureDTO> UpdateLectureAsync(int id, UpdateLectureDTO updateLectureDto)
        {
            var lecture = await _lectureRepository.GetByIdAsync(id);
            if (lecture == null)
                return null;

            // Update properties
            if (!string.IsNullOrEmpty(updateLectureDto.Title))
                lecture.Title = updateLectureDto.Title;

            if (!string.IsNullOrEmpty(updateLectureDto.Description))
                lecture.Description = updateLectureDto.Description;

            //if (updateLectureDto.Type.HasValue)
            //    lecture.Type = updateLectureDto.Type.Value;

            // Handle video file upload if provided
            if (updateLectureDto.VideoFile != null && updateLectureDto.VideoFile.Length > 0)
            {
                // Validate video file
                if (!await ValidateVideoFileAsync(updateLectureDto.VideoFile))
                    throw new ArgumentException("Invalid video file. Only MP4 files up to 500MB are allowed.");

                // Delete old video file if exists
                if (!string.IsNullOrEmpty(lecture.VideoUrl) && lecture.VideoUrl.StartsWith("/uploads/lectures/"))
                {
                    var oldFilePath = Path.Combine(_environment.WebRootPath, lecture.VideoUrl.TrimStart('/'));
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                }

                // Generate unique filename
                var extension = Path.GetExtension(updateLectureDto.VideoFile.FileName).ToLowerInvariant();
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(_uploadPath, fileName);

                // Save the new video file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await updateLectureDto.VideoFile.CopyToAsync(stream);
                }

                // Update video URL
                lecture.VideoUrl = $"/uploads/lectures/{fileName}";
            }

            //if (!string.IsNullOrEmpty(updateLectureDto.ThumbnailUrl))
            //    lecture.ThumbnailUrl = updateLectureDto.ThumbnailUrl;

            //if (updateLectureDto.ConsultationId.HasValue)
            //    lecture.ConsultationId = updateLectureDto.ConsultationId;

            //if (updateLectureDto.Tags != null)
            //    lecture.Tags = System.Text.Json.JsonSerializer.Serialize(updateLectureDto.Tags);

            lecture.UpdatedAt = DateTime.UtcNow;

            var updatedLecture = await _lectureRepository.UpdateAsync(lecture);
            return _mapper.Map<LectureDTO>(updatedLecture);
        }

        public async Task<bool> DeleteLectureAsync(int id)
        {
            var lecture = await _lectureRepository.GetByIdAsync(id);
            if (lecture == null)
                return false;

            await _lectureRepository.DeleteAsync(id);
            return true;
        }

        public async Task<LectureDTO> PublishLectureAsync(int id)
        {
            var lecture = await _lectureRepository.GetByIdAsync(id);
            if (lecture == null)
                return null;

            lecture.IsPublished = true;
            lecture.PublishedAt = DateTime.UtcNow;
            lecture.UpdatedAt = DateTime.UtcNow;

            var updatedLecture = await _lectureRepository.UpdateAsync(lecture);
            return _mapper.Map<LectureDTO>(updatedLecture);
        }

        public async Task<LectureDTO> UnpublishLectureAsync(int id)
        {
            var lecture = await _lectureRepository.GetByIdAsync(id);
            if (lecture == null)
                return null;

            lecture.IsPublished = false;
            lecture.PublishedAt = null;
            lecture.UpdatedAt = DateTime.UtcNow;

            var updatedLecture = await _lectureRepository.UpdateAsync(lecture);
            return _mapper.Map<LectureDTO>(updatedLecture);
        }



        //public async Task<List<LectureDTO>> GetLecturesByConsultationAsync(int consultationId)
        //{
        //    var lectures = await _lectureRepository.GetByConsultationIdAsync(consultationId);
        //    return _mapper.Map<List<LectureDTO>>(lectures);
        //}

        //public async Task<List<LectureDTO>> GetLecturesByTypeAsync(LectureType type)
        //{
        //    var lectures = await _lectureRepository.GetByTypeAsync(type);
        //    return _mapper.Map<List<LectureDTO>>(lectures);
        //}

        //public async Task<List<LectureDTO>> GetRelatedLecturesAsync(int id)
        //{
        //    var lecture = await _lectureRepository.GetByIdAsync(id);
        //    if (lecture == null)
        //        return new List<LectureDTO>();

        //    var relatedLectures = await _lectureRepository.GetByConsultationIdAsync(lecture.ConsultationId ?? 0);
        //    relatedLectures = relatedLectures.Where(l => l.Id != id).Take(5).ToList();

        //    return _mapper.Map<List<LectureDTO>>(relatedLectures);
        //}

        //public async Task<bool> IncrementViewCountAsync(int id)
        //{
        //    var lecture = await _lectureRepository.GetByIdAsync(id);
        //    if (lecture == null)
        //        return false;

        //    lecture.ViewCount++;
        //    await _lectureRepository.UpdateAsync(lecture);
        //    return true;
        //}

        //public async Task<bool> IncrementDownloadCountAsync(int id)
        //{
        //    var lecture = await _lectureRepository.GetByIdAsync(id);
        //    if (lecture == null)
        //        return false;

        //    lecture.DownloadCount++;
        //    await _lectureRepository.UpdateAsync(lecture);
        //    return true;
        //}

        public async Task<string> GetVideoStreamUrlAsync(int id)
        {
            var lecture = await _lectureRepository.GetByIdAsync(id);
            if (lecture == null)
                return null;

            // This would typically generate a streaming URL
            return lecture.VideoUrl ?? $"/api/lecture/{id}/stream";
        }

        public async Task<object> GetLectureStatisticsAsync()
        {
            var lectures = await _lectureRepository.GetAllAsync();

            return new
            {
                TotalLectures = lectures.Count(),
                PublishedLectures = lectures.Count(l => l.IsPublished),
                //TotalViews = lectures.Sum(l => l.ViewCount),
                //TotalDownloads = lectures.Sum(l => l.DownloadCount),
                //MostViewedLectures = await _lectureRepository.GetMostViewedLecturesAsync(5)
            };
        }

        //public async Task<object> GetLectureStatisticsByConsultationAsync(int consultationId)
        //{
        //    var lectures = await _lectureRepository.GetByConsultationIdAsync(consultationId);

        //    return new
        //    {
        //        TotalLectures = lectures.Count,
        //        PublishedLectures = lectures.Count(l => l.IsPublished),
        //        TotalViews = lectures.Sum(l => l.ViewCount),
        //        TotalDownloads = lectures.Sum(l => l.DownloadCount)
        //    };
        //}

        public async Task<string> GenerateThumbnailAsync(int id)
        {
            var lecture = await _lectureRepository.GetByIdAsync(id);
            if (lecture == null)
                return null;

            // This would typically generate a thumbnail from the video
            return $"/thumbnails/{id}.jpg";
        }

        //public async Task<bool> UpdateThumbnailAsync(int id, string thumbnailUrl)
        //{
        //    var lecture = await _lectureRepository.GetByIdAsync(id);
        //    if (lecture == null)
        //        return false;

        //    //lecture.ThumbnailUrl = thumbnailUrl;
        //    lecture.UpdatedAt = DateTime.UtcNow;
        //    await _lectureRepository.UpdateAsync(lecture);
        //    return true;
        //}

        public async Task<bool> DeleteVideoAsync(int lectureId)
        {
            var lecture = await _lectureRepository.GetByIdAsync(lectureId);
            if (lecture == null)
                return false;

            // Delete the video file from storage
            if (!string.IsNullOrEmpty(lecture.VideoUrl) && lecture.VideoUrl.StartsWith("/"))
            {
                var filePath = Path.Combine(_environment.WebRootPath, lecture.VideoUrl.TrimStart('/'));
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            // Clear the video URL
            lecture.VideoUrl = null;
            lecture.UpdatedAt = DateTime.UtcNow;
            await _lectureRepository.UpdateAsync(lecture);
            return true;
        }

        public async Task<PaginatedResponse<LectureDTO>> SearchLecturesAsync(LectureSearchDTO searchDto)
        {
            var lectures = await _lectureRepository.SearchLecturesAsync(searchDto.SearchTerm);

            // Apply additional filters
            //if (searchDto.Type.HasValue)
            //    lectures = lectures.Where(l => l.Type == searchDto.Type.Value).ToList();

            //if (searchDto.ConsultationId.HasValue)
            //    lectures = lectures.Where(l => l.ConsultationId == searchDto.ConsultationId.Value).ToList();

            // Apply pagination
            var totalCount = lectures.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / searchDto.PageSize);
            var items = lectures
                .Skip((searchDto.Page - 1) * searchDto.PageSize)
                .Take(searchDto.PageSize)
                .ToList();

            var lectureDtos = _mapper.Map<List<LectureDTO>>(items);

            return new PaginatedResponse<LectureDTO>
            {
                Items = lectureDtos,
                TotalCount = totalCount,
                PageNumber = searchDto.Page,
                PageSize = searchDto.PageSize,
                TotalPages = totalPages
            };
        }

        public async Task<bool> ValidateExternalUrlAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            try
            {
                // Basic URL validation
                if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
                    return false;

                // Check if it's a supported video platform
                var host = uri.Host.ToLower();
                var supportedPlatforms = new[] { "youtube.com", "youtu.be", "vimeo.com", "dailymotion.com" };

                return supportedPlatforms.Any(platform => host.Contains(platform));
            }
            catch
            {
                return false;
            }
        }
        public async Task<int> GetTotalLecturesCountAsync()
        {
            return await _lectureRepository.CountAsync();
        }
    }
}