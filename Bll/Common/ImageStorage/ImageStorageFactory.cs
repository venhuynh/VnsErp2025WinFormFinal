using System;
using Logger.Interfaces;

namespace Bll.Common.ImageStorage
{
    /// <summary>
    /// Factory để tạo Image Storage Service dựa trên configuration
    /// Hỗ trợ nhiều loại storage: NAS, Local, Cloud (future)
    /// </summary>
    public static class ImageStorageFactory
    {
        /// <summary>
        /// Tạo Image Storage Service dựa trên StorageType trong config
        /// </summary>
        /// <param name="config">Configuration cho image storage</param>
        /// <param name="logger">Logger để ghi log</param>
        /// <returns>IImageStorageService instance</returns>
        /// <exception cref="ArgumentNullException">Khi config hoặc logger là null</exception>
        /// <exception cref="NotSupportedException">Khi StorageType không được hỗ trợ</exception>
        public static IImageStorageService Create(ImageStorageConfiguration config, ILogger logger)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config), "Cấu hình Image Storage không được để trống");
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger), "Logger không được để trống");
            }

            var storageType = config.StorageType?.ToUpper() ?? "NAS";

            return storageType switch
            {
                "NAS" => new NASImageStorageService(config, logger),
                "LOCAL" => new LocalImageStorageService(config, logger),
                // "CLOUD" => new CloudImageStorageService(config, logger), // Sẽ được implement trong tương lai
                _ => throw new NotSupportedException(
                    $"Loại storage '{storageType}' không được hỗ trợ. " +
                    $"Các loại được hỗ trợ: NAS, Local. " +
                    $"Vui lòng kiểm tra lại cấu hình 'ImageStorage.StorageType' trong App.config.")
            };
        }

        /// <summary>
        /// Tạo Image Storage Service từ App.config
        /// Tự động load configuration từ App.config và tạo service tương ứng
        /// </summary>
        /// <param name="logger">Logger để ghi log</param>
        /// <returns>IImageStorageService instance</returns>
        /// <exception cref="ArgumentNullException">Khi logger là null</exception>
        /// <exception cref="NotSupportedException">Khi StorageType trong config không được hỗ trợ</exception>
        public static IImageStorageService CreateFromConfig(ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger), "Logger không được để trống");
            }

            try
            {
                var config = ImageStorageConfiguration.LoadFromConfig();
                
                // Validate config trước khi tạo service
                if (config == null)
                {
                    throw new InvalidOperationException(
                        "Không thể load cấu hình Image Storage từ App.config. " +
                        "Vui lòng kiểm tra lại file App.config.");
                }

                // Kiểm tra config theo StorageType
                var storageType = config.StorageType?.ToUpper() ?? "NAS";
                if (storageType == "NAS")
                {
                    if (string.IsNullOrEmpty(config.NASBasePath) && 
                        (string.IsNullOrEmpty(config.NASServerName) || string.IsNullOrEmpty(config.NASShareName)))
                    {
                        throw new InvalidOperationException(
                            "Cấu hình NAS không đầy đủ. " +
                            "Vui lòng cấu hình một trong các cách sau:\n" +
                            "1. ImageStorage.NAS.BasePath (ví dụ: \\\\192.168.1.100\\ERP_Images)\n" +
                            "2. ImageStorage.NAS.ServerName + ImageStorage.NAS.ShareName (ví dụ: \\\\192.168.1.100 + ERP_Images)\n\n" +
                            "Hoặc thay đổi ImageStorage.StorageType thành 'Local' nếu muốn dùng local storage.");
                    }
                }
                else if (storageType == "LOCAL")
                {
                    // TODO: Validate Local config nếu cần
                }

                return Create(config, logger);
            }
            catch (InvalidOperationException)
            {
                // Re-throw InvalidOperationException để giữ nguyên thông báo lỗi
                throw;
            }
            catch (Exception ex)
            {
                logger.Error($"Lỗi khi tạo Image Storage Service từ config: {ex.Message}", ex);
                throw new InvalidOperationException(
                    "Không thể tạo Image Storage Service từ cấu hình. " +
                    "Vui lòng kiểm tra lại các settings trong App.config, đặc biệt là 'ImageStorage.StorageType'. " +
                    $"Chi tiết lỗi: {ex.Message}", ex);
            }
        }
    }
}

