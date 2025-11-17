using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Common.Helpers;

/// <summary>
/// Helper class chuyên xử lý các thao tác với file XML
/// </summary>
public static class XmlHelper
{
    #region ========== XML SERIALIZATION ==========

    /// <summary>
    /// Lưu object vào file XML
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của object</typeparam>
    /// <param name="filePath">Đường dẫn file XML</param>
    /// <param name="objectToSerialize">Object cần serialize</param>
    /// <param name="createDirectoryIfNotExists">Tạo thư mục nếu chưa tồn tại</param>
    /// <returns>True nếu lưu thành công</returns>
    public static bool SaveToXml<T>(string filePath, T objectToSerialize, bool createDirectoryIfNotExists = true)
    {
        try
        {
            if (objectToSerialize == null)
            {
                System.Diagnostics.Debug.WriteLine("Object cần serialize là null");
                return false;
            }

            if (string.IsNullOrWhiteSpace(filePath))
            {
                System.Diagnostics.Debug.WriteLine("Đường dẫn file không hợp lệ");
                return false;
            }

            // Tạo thư mục nếu cần
            if (createDirectoryIfNotExists)
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }

            var serializer = new XmlSerializer(typeof(T));

            using var writer = new StreamWriter(filePath, false, Encoding.UTF8);
            serializer.Serialize(writer, objectToSerialize);

            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Lỗi khi lưu object vào file XML: {filePath}\n{ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Đọc danh sách object từ file XML
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của object</typeparam>
    /// <param name="filePath">Đường dẫn file XML</param>
    /// <returns>Danh sách object hoặc null nếu có lỗi</returns>
    public static List<T> LoadFromXmlToList<T>(string filePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                System.Diagnostics.Debug.WriteLine("Đường dẫn file không hợp lệ");
                return null;
            }

            if (!File.Exists(filePath))
            {
                System.Diagnostics.Debug.WriteLine($"File không tồn tại: {filePath}");
                return null;
            }

            var serializer = new XmlSerializer(typeof(List<T>));

            using var reader = new StreamReader(filePath, Encoding.UTF8);
            var result = (List<T>)serializer.Deserialize(reader);

            return result;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Lỗi khi đọc danh sách object từ file XML: {filePath}\n{ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Đọc một object từ file XML
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của object</typeparam>
    /// <param name="filePath">Đường dẫn file XML</param>
    /// <returns>Object hoặc default(T) nếu có lỗi</returns>
    public static T LoadFromXmlToObject<T>(string filePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                System.Diagnostics.Debug.WriteLine("Đường dẫn file không hợp lệ");
                return default(T);
            }

            if (!File.Exists(filePath))
            {
                System.Diagnostics.Debug.WriteLine($"File không tồn tại: {filePath}");
                return default(T);
            }

            var serializer = new XmlSerializer(typeof(T));

            using var reader = new StreamReader(filePath, Encoding.UTF8);
            var result = (T)serializer.Deserialize(reader);

            return result;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Lỗi khi đọc object từ file XML: {filePath}\n{ex.Message}");
            return default(T);
        }
    }

    #endregion

    #region ========== FILE MANAGEMENT ==========

    /// <summary>
    /// Xóa file XML
    /// </summary>
    /// <param name="filePath">Đường dẫn file cần xóa</param>
    /// <returns>True nếu xóa thành công</returns>
    public static bool DeleteXmlFile(string filePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                System.Diagnostics.Debug.WriteLine("Đường dẫn file không hợp lệ");
                return false;
            }

            if (!File.Exists(filePath))
            {
                return true; // Coi như thành công vì file đã không tồn tại
            }

            File.Delete(filePath);
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Lỗi khi xóa file XML: {filePath}\n{ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Kiểm tra file XML có tồn tại không
    /// </summary>
    /// <param name="filePath">Đường dẫn file</param>
    /// <returns>True nếu file tồn tại</returns>
    public static bool XmlFileExists(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return false;

        return File.Exists(filePath);
    }

    /// <summary>
    /// Lấy kích thước file XML (bytes)
    /// </summary>
    /// <param name="filePath">Đường dẫn file</param>
    /// <returns>Kích thước file hoặc -1 nếu có lỗi</returns>
    public static long GetXmlFileSize(string filePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return -1;

            var fileInfo = new FileInfo(filePath);
            return fileInfo.Length;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Lỗi khi lấy kích thước file XML: {filePath}\n{ex.Message}");
            return -1;
        }
    }

    /// <summary>
    /// Lấy thời gian tạo file XML
    /// </summary>
    /// <param name="filePath">Đường dẫn file</param>
    /// <returns>Thời gian tạo file hoặc null nếu có lỗi</returns>
    public static DateTime? GetXmlFileCreationTime(string filePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return null;

            var fileInfo = new FileInfo(filePath);
            return fileInfo.CreationTime;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Lỗi khi lấy thời gian tạo file XML: {filePath}\n{ex.Message}");
            return null;
        }
    }

    #endregion

    #region ========== VALIDATION ==========

    /// <summary>
    /// Kiểm tra file XML có hợp lệ không
    /// </summary>
    /// <param name="filePath">Đường dẫn file</param>
    /// <returns>True nếu file XML hợp lệ</returns>
    public static bool IsValidXmlFile(string filePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return false;

            // Đọc và parse file XML để kiểm tra tính hợp lệ
            using var reader = new StreamReader(filePath, Encoding.UTF8);
            var content = reader.ReadToEnd();

            if (string.IsNullOrWhiteSpace(content))
                return false;

            // Kiểm tra có phải XML không
            return content.TrimStart().StartsWith("<?xml") || content.TrimStart().StartsWith("<");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Lỗi khi kiểm tra tính hợp lệ của file XML: {filePath}\n{ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Kiểm tra file XML có thể deserialize thành kiểu T không
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu cần kiểm tra</typeparam>
    /// <param name="filePath">Đường dẫn file</param>
    /// <returns>True nếu có thể deserialize</returns>
    public static bool CanDeserializeToType<T>(string filePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return false;

            var serializer = new XmlSerializer(typeof(T));

            using var reader = new StreamReader(filePath, Encoding.UTF8);
            serializer.Deserialize(reader);

            return true;
        }
        catch (Exception)
        {
            // Không hiển thị lỗi vì đây chỉ là kiểm tra
            return false;
        }
    }

    #endregion

    #region ========== UTILITY METHODS ==========

    /// <summary>
    /// Tạo đường dẫn file XML với timestamp
    /// </summary>
    /// <param name="directory">Thư mục chứa file</param>
    /// <param name="fileName">Tên file (không cần extension)</param>
    /// <param name="includeTime">Có bao gồm thời gian trong tên file không</param>
    /// <returns>Đường dẫn file hoàn chỉnh</returns>
    public static string CreateXmlFilePath(string directory, string fileName, bool includeTime = false)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(directory) || string.IsNullOrWhiteSpace(fileName))
                return string.Empty;

            // Loại bỏ extension nếu có
            if (fileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            {
                fileName = fileName.Substring(0, fileName.Length - 4);
            }

            // Thêm timestamp nếu cần
            if (includeTime)
            {
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                fileName = $"{fileName}_{timestamp}";
            }

            return Path.Combine(directory, $"{fileName}.xml");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo đường dẫn file XML\n{ex.Message}");
            return string.Empty;
        }
    }

    /// <summary>
    /// Sao chép file XML
    /// </summary>
    /// <param name="sourceFilePath">Đường dẫn file nguồn</param>
    /// <param name="destinationFilePath">Đường dẫn file đích</param>
    /// <param name="overwrite">Ghi đè nếu file đích đã tồn tại</param>
    /// <returns>True nếu sao chép thành công</returns>
    public static bool CopyXmlFile(string sourceFilePath, string destinationFilePath, bool overwrite = false)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(sourceFilePath) || string.IsNullOrWhiteSpace(destinationFilePath))
            {
                System.Diagnostics.Debug.WriteLine("Đường dẫn file không hợp lệ");
                return false;
            }

            if (!File.Exists(sourceFilePath))
            {
                System.Diagnostics.Debug.WriteLine($"File nguồn không tồn tại: {sourceFilePath}");
                return false;
            }

            // Tạo thư mục đích nếu cần
            var destinationDirectory = Path.GetDirectoryName(destinationFilePath);
            if (!string.IsNullOrEmpty(destinationDirectory) && !Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            File.Copy(sourceFilePath, destinationFilePath, overwrite);
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Lỗi khi sao chép file XML từ {sourceFilePath} đến {destinationFilePath}\n{ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Lấy đường dẫn thư mục VNTA_Config (cùng cấp với thư mục chạy ứng dụng)
    /// </summary>
    /// <returns>Đường dẫn thư mục VNTA_Config</returns>
    private static string GetVntaConfigDirectory()
    {
        try
        {
            // Lấy thư mục chạy ứng dụng
            var appDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (string.IsNullOrEmpty(appDirectory))
            {
                appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }

            // Tạo đường dẫn đến thư mục VNTA_Config
            var configDirectory = Path.Combine(appDirectory, "VNTA_Config");

            // Tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(configDirectory))
            {
                Directory.CreateDirectory(configDirectory);
            }

            return configDirectory;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Lỗi khi lấy đường dẫn thư mục VNTA_Config: {ex.Message}");
            return string.Empty;
        }
    }

    /// <summary>
    /// Lấy đường dẫn file cấu hình database trong thư mục VNTA_Config
    /// </summary>
    /// <returns>Đường dẫn file DatabaseConfig.xml</returns>
    public static string GetDatabaseConfigFilePath()
    {
        var configDirectory = GetVntaConfigDirectory();
        if (string.IsNullOrEmpty(configDirectory))
            return string.Empty;

        return Path.Combine(configDirectory, "DatabaseConfig.xml");
    }

    #endregion
}