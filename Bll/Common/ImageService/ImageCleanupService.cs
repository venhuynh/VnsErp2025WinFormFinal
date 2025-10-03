using Dal.DataAccess.MasterData.ProductServiceDal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bll.MasterData.ProductServiceBll;

namespace Bll.Common.ImageService
{
    /// <summary>
    /// Service xử lý cleanup orphaned files và optimization
    /// </summary>
    public class ImageCleanupService
    {
        #region Fields

        private readonly ProductImageDataAccess _imageDataAccess;
        private readonly string _photoDirectory;

        #endregion

        #region Constructor

        public ImageCleanupService()
        {
            _imageDataAccess = new ProductImageDataAccess();
            _photoDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PHOTO", "PRODUCTSERVICE");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Tìm và xóa orphaned files (files không có trong database)
        /// </summary>
        /// <returns>Số lượng files đã xóa</returns>
        public async Task<int> CleanupOrphanedFilesAsync()
        {
            try
            {
                if (!Directory.Exists(_photoDirectory))
                    return 0;

                // Lấy danh sách tất cả files trong thư mục
                var allFiles = Directory.GetFiles(_photoDirectory, "*.*", SearchOption.AllDirectories)
                    .Select(f => Path.GetFileName(f))
                    .ToHashSet();

                // Lấy danh sách tất cả ImagePath từ database
                var dbImagePaths = await GetAllImagePathsFromDatabaseAsync();
                var dbFileNames = dbImagePaths
                    .Select(p => Path.GetFileName(p))
                    .Where(f => !string.IsNullOrEmpty(f))
                    .ToHashSet();

                // Tìm orphaned files
                var orphanedFiles = allFiles.Except(dbFileNames).ToList();

                // Xóa orphaned files
                int deletedCount = 0;
                foreach (var orphanedFile in orphanedFiles)
                {
                    try
                    {
                        var fullPath = Path.Combine(_photoDirectory, orphanedFile);
                        if (File.Exists(fullPath))
                        {
                            File.Delete(fullPath);
                            deletedCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log error nhưng tiếp tục xóa các files khác
                        System.Diagnostics.Debug.WriteLine($"Không thể xóa file {orphanedFile}: {ex.Message}");
                    }
                }

                return deletedCount;
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi cleanup orphaned files: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tìm và xóa duplicate files
        /// </summary>
        /// <returns>Số lượng files đã xóa</returns>
        public async Task<int> CleanupDuplicateFilesAsync()
        {
            try
            {
                if (!Directory.Exists(_photoDirectory))
                    return 0;

                var allFiles = Directory.GetFiles(_photoDirectory, "*.*", SearchOption.AllDirectories);
                var fileGroups = allFiles
                    .GroupBy(f => new FileInfo(f).Length)
                    .Where(g => g.Count() > 1)
                    .ToList();

                int deletedCount = 0;
                foreach (var group in fileGroups)
                {
                    var files = group.ToList();
                    // Giữ lại file đầu tiên, xóa các file còn lại
                    for (int i = 1; i < files.Count; i++)
                    {
                        try
                        {
                            // Kiểm tra xem file có được reference trong database không
                            var fileName = Path.GetFileName(files[i]);
                            if (!await IsFileReferencedInDatabaseAsync(fileName))
                            {
                                File.Delete(files[i]);
                                deletedCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Không thể xóa duplicate file {files[i]}: {ex.Message}");
                        }
                    }
                }

                return deletedCount;
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi cleanup duplicate files: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa files cũ hơn một khoảng thời gian nhất định
        /// </summary>
        /// <param name="daysOld">Số ngày cũ</param>
        /// <param name="onlyUnreferenced">Chỉ xóa files không được reference</param>
        /// <returns>Số lượng files đã xóa</returns>
        public async Task<int> CleanupOldFilesAsync(int daysOld = 30, bool onlyUnreferenced = true)
        {
            try
            {
                if (!Directory.Exists(_photoDirectory))
                    return 0;

                var cutoffDate = DateTime.Now.AddDays(-daysOld);
                var allFiles = Directory.GetFiles(_photoDirectory, "*.*", SearchOption.AllDirectories);
                
                int deletedCount = 0;
                foreach (var filePath in allFiles)
                {
                    try
                    {
                        var fileInfo = new FileInfo(filePath);
                        if (fileInfo.CreationTime < cutoffDate)
                        {
                            // Nếu onlyUnreferenced = true, kiểm tra xem file có được reference không
                            if (onlyUnreferenced)
                            {
                                var fileName = Path.GetFileName(filePath);
                                if (await IsFileReferencedInDatabaseAsync(fileName))
                                    continue;
                            }

                            File.Delete(filePath);
                            deletedCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Không thể xóa old file {filePath}: {ex.Message}");
                    }
                }

                return deletedCount;
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi cleanup old files: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tối ưu hóa thư mục (tạo subdirectories theo date)
        /// </summary>
        public async Task OptimizeDirectoryStructureAsync()
        {
            try
            {
                if (!Directory.Exists(_photoDirectory))
                    return;

                var allFiles = Directory.GetFiles(_photoDirectory, "*.*", SearchOption.TopDirectoryOnly);
                
                foreach (var filePath in allFiles)
                {
                    try
                    {
                        var fileInfo = new FileInfo(filePath);
                        var dateFolder = fileInfo.CreationTime.ToString("yyyy-MM");
                        var targetDir = Path.Combine(_photoDirectory, dateFolder);
                        
                        if (!Directory.Exists(targetDir))
                            Directory.CreateDirectory(targetDir);

                        var targetPath = Path.Combine(targetDir, fileInfo.Name);
                        if (!File.Exists(targetPath))
                        {
                            File.Move(filePath, targetPath);
                            
                            // Cập nhật ImagePath trong database
                            await UpdateImagePathInDatabaseAsync(fileInfo.Name, targetPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Không thể move file {filePath}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi optimize directory structure: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tính toán disk usage của thư mục images
        /// </summary>
        /// <returns>Disk usage info</returns>
        public async Task<DiskUsageInfo> GetDiskUsageAsync()
        {
            try
            {
                if (!Directory.Exists(_photoDirectory))
                    return new DiskUsageInfo { TotalSize = 0, FileCount = 0 };

                var allFiles = Directory.GetFiles(_photoDirectory, "*.*", SearchOption.AllDirectories);
                long totalSize = 0;
                int fileCount = 0;

                foreach (var filePath in allFiles)
                {
                    try
                    {
                        var fileInfo = new FileInfo(filePath);
                        totalSize += fileInfo.Length;
                        fileCount++;
                    }
                    catch
                    {
                        // Ignore files that can't be accessed
                    }
                }

                await Task.CompletedTask;
                return new DiskUsageInfo
                {
                    TotalSize = totalSize,
                    FileCount = fileCount,
                    AverageFileSize = fileCount > 0 ? totalSize / fileCount : 0
                };
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi tính disk usage: {ex.Message}", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Lấy tất cả ImagePath từ database
        /// </summary>
        private async Task<List<string>> GetAllImagePathsFromDatabaseAsync()
        {
            try
            {
                // TODO: Implement method to get all image paths from database
                // Đây là placeholder implementation
                await Task.CompletedTask;
                return new List<string>();
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lấy image paths từ database: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kiểm tra xem file có được reference trong database không
        /// </summary>
        private async Task<bool> IsFileReferencedInDatabaseAsync(string fileName)
        {
            try
            {
                // TODO: Implement method to check if file is referenced in database
                // Đây là placeholder implementation
                await Task.CompletedTask;
                return false;
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi kiểm tra file reference: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật ImagePath trong database
        /// </summary>
        private async Task UpdateImagePathInDatabaseAsync(string fileName, string newPath)
        {
            try
            {
                // TODO: Implement method to update image path in database
                // Đây là placeholder implementation
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi cập nhật image path: {ex.Message}", ex);
            }
        }

        #endregion
    }

    /// <summary>
    /// Thông tin disk usage
    /// </summary>
    public class DiskUsageInfo
    {
        public long TotalSize { get; set; }
        public int FileCount { get; set; }
        public long AverageFileSize { get; set; }
        
        public string TotalSizeFormatted => FormatBytes(TotalSize);
        public string AverageFileSizeFormatted => FormatBytes(AverageFileSize);
        
        private string FormatBytes(long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
            int counter = 0;
            decimal number = bytes;
            while (Math.Round(number / 1024) >= 1)
            {
                number = number / 1024;
                counter++;
            }
            return $"{number:n1} {suffixes[counter]}";
        }
    }
}
