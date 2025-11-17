using System;
using System.IO;

namespace Common
{
    /// <summary>
    /// Class chứa tất cả các hằng số sử dụng trong toàn bộ ứng dụng
    /// </summary>
    public static class ApplicationConstants
    {
        #region ========== REGISTRY CONSTANTS ==========

        /// <summary>
        /// Registry root key cho ứng dụng
        /// </summary>
        public const string REGISTRY_APP_KEY = @"SOFTWARE\VNTA_NET_2025";

        /// <summary>
        /// Registry subkey cho cấu hình Database
        /// </summary>
        public const string DATABASE_SUBKEY = "DatabaseConfig";

        /// <summary>
        /// Registry subkey cho cấu hình ADMS
        /// </summary>
        public const string ADMS_SUBKEY = "ADMS";

        /// <summary>
        /// Registry subkey cho cấu hình ứng dụng
        /// </summary>
        public const string APPLICATION_SUBKEY = "AppSettings";

        #endregion

        #region ========== ADMS CONSTANTS ==========

        /// <summary>
        /// Tên của Windows Service ADMS
        /// </summary>
        public const string ADMS_SERVICE_NAME = "VntaAdmsService";

        /// <summary>
        /// Display name của Windows Service ADMS
        /// </summary>
        public const string ADMS_SERVICE_DISPLAY_NAME = "VNTA ADMS Service";

        /// <summary>
        /// Description của Windows Service ADMS
        /// </summary>
        public const string ADMS_SERVICE_DESCRIPTION = "VNTA Attendance Data Management Service";

        /// <summary>
        /// Port mặc định cho ADMS
        /// </summary>
        public const int ADMS_DEFAULT_PORT = 8090;

        /// <summary>
        /// Đường dẫn mặc định cho ADMS service (được tính động)
        /// </summary>
        public static string ADMS_DEFAULT_SERVICE_PATH => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AdmsServices", "VntaAdmsService.exe");

        #endregion

        #region ========== LOGGING CONSTANTS ==========

        /// <summary>
        /// Tên file pattern cho log mặc định
        /// </summary>
        public const string LOG_FILE_PATTERN = "VNTA-NET-2025_{date}.log";

        /// <summary>
        /// Kích thước tối đa của file log (MB)
        /// </summary>
        public const int LOG_MAX_FILE_SIZE_MB = 10;

        /// <summary>
        /// Số lượng file log tối đa giữ lại
        /// </summary>
        public const int LOG_MAX_FILES = 30;

        /// <summary>
        /// Tên file cấu hình Logger XML
        /// </summary>
        public const string LOG_CONFIG_FILE = "LogConfiguration.xml";

        /// <summary>
        /// Log level mặc định
        /// </summary>
        public const string LOG_DEFAULT_LEVEL = "Info";

        /// <summary>
        /// Enable console logging mặc định
        /// </summary>
        public const bool LOG_DEFAULT_ENABLE_CONSOLE = true;

        /// <summary>
        /// Enable file logging mặc định
        /// </summary>
        public const bool LOG_DEFAULT_ENABLE_FILE = true;

        /// <summary>
        /// Hiển thị timestamp trên console mặc định
        /// </summary>
        public const bool LOG_DEFAULT_SHOW_TIMESTAMP = true;

        /// <summary>
        /// Hiển thị category trên console mặc định
        /// </summary>
        public const bool LOG_DEFAULT_SHOW_CATEGORY = true;

        /// <summary>
        /// Đường dẫn file cấu hình Logger (được tính động)
        /// </summary>
        public static string LogConfigFilePath => Path.Combine(ConfigFolderPath, LOG_CONFIG_FILE);

        #endregion

        #region ========== DYNAMIC PATH PROPERTIES ==========

        /// <summary>
        /// Đường dẫn thư mục Excel Reports (được tính động)
        /// </summary>
        public static string ExcelReportsFolderPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, EXCEL_REPORTS_FOLDER);

        /// <summary>
        /// Đường dẫn thư mục Database Backups (được tính động)
        /// </summary>
        public static string DatabaseBackupFolderPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DATABASE_BACKUP_FOLDER);

        /// <summary>
        /// Đường dẫn thư mục Logs (được tính động)
        /// </summary>
        public static string LogsFolderPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LOGS_FOLDER);

        /// <summary>
        /// Đường dẫn thư mục Config (được tính động)
        /// </summary>
        public static string ConfigFolderPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CONFIG_FOLDER);

        /// <summary>
        /// Đường dẫn file báo cáo tổng kết hàng ngày (được tính động)
        /// </summary>
        public static string AttLogDailyReportFilePath => Path.Combine(ExcelReportsFolderPath, ATT_LOG_DAILY_REPORT_FILE);

        /// <summary>
        /// Đường dẫn file cấu hình database đã mã hóa (được tính động)
        /// </summary>
        public static string EncryptedDatabaseConfigFilePath => Path.Combine(ConfigFolderPath, ENCRYPTED_DATABASE_CONFIG_FILE);

        /// <summary>
        /// Đường dẫn file cấu hình ADMS (được tính động)
        /// </summary>
        public static string AdmsConfigFilePath => Path.Combine(ConfigFolderPath, ADMS_CONFIG_FILE);

        /// <summary>
        /// Đường dẫn thư mục cài đặt xuất dữ liệu chấm công Audit (được tính động)
        /// </summary>
        public static string CaiDatXuatDuLieuChamCongAuditFolderPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CAI_DAT_XUAT_DU_LIEU_CHAM_CONG_AUDIT_FOLDER);

        /// <summary>
        /// Đường dẫn file cài đặt giờ tăng ca tối đa (được tính động)
        /// </summary>
        public static string CaiDatGioTangCaToiDaFilePath => Path.Combine(CaiDatXuatDuLieuChamCongAuditFolderPath, CAI_DAT_GIO_TANG_CA_TOI_DA_FILE_NAME);

        #endregion

        #region ========== FILE AND FOLDER CONSTANTS ==========

        /// <summary>
        /// Tên thư mục chứa báo cáo Excel
        /// </summary>
        public const string EXCEL_REPORTS_FOLDER = "ExcelReports";

        /// <summary>
        /// Tên thư mục chứa backup database
        /// </summary>
        public const string DATABASE_BACKUP_FOLDER = "DatabaseBackups";

        /// <summary>
        /// Tên thư mục chứa logs
        /// </summary>
        public const string LOGS_FOLDER = "Logs";

        /// <summary>
        /// Tên thư mục chứa config files
        /// </summary>
        public const string CONFIG_FOLDER = "Config";

        /// <summary>
        /// Tên file báo cáo tổng kết hàng ngày
        /// </summary>
        public const string ATT_LOG_DAILY_REPORT_FILE = "01.AttLogDailyReport.xlsx";

        /// <summary>
        /// Tên file cấu hình database đã mã hóa
        /// </summary>
        public const string ENCRYPTED_DATABASE_CONFIG_FILE = "EncryptedDatabaseConfig.xml";

        /// <summary>
        /// Tên file cấu hình ADMS
        /// </summary>
        public const string ADMS_CONFIG_FILE = "AdmsConfig.xml";

        /// <summary>
        /// Tên thư mục chứa cài đặt xuất dữ liệu chấm công cho Audit
        /// </summary>
        private const string CAI_DAT_XUAT_DU_LIEU_CHAM_CONG_AUDIT_FOLDER = "CaiDatXuatDuLieuChamCongAudit";

        /// <summary>
        /// Tên file cài đặt giờ tăng ca tối đa
        /// </summary>
        private const string CAI_DAT_GIO_TANG_CA_TOI_DA_FILE_NAME = "CaiDatGioTangCaToiDa.xml";

        #endregion

        #region ========== DATABASE CONSTANTS ==========

        /// <summary>
        /// Timeout kết nối database mặc định (giây)
        /// </summary>
        public const int DATABASE_DEFAULT_CONNECTION_TIMEOUT = 30;

        /// <summary>
        /// Trust Server Certificate mặc định
        /// </summary>
        public const bool DATABASE_DEFAULT_TRUST_SERVER_CERTIFICATE = true;

        /// <summary>
        /// Số lượng file backup database mặc định cần giữ lại
        /// </summary>
        public const int DATABASE_DEFAULT_BACKUP_KEEP_COUNT = 5;

        #endregion

        #region ========== VALIDATION CONSTANTS ==========

        /// <summary>
        /// Độ dài tối đa cho tên máy chủ
        /// </summary>
        public const int MAX_SERVER_NAME_LENGTH = 255;

        /// <summary>
        /// Độ dài tối đa cho tên database
        /// </summary>
        public const int MAX_DATABASE_NAME_LENGTH = 255;

        /// <summary>
        /// Độ dài tối đa cho User ID
        /// </summary>
        public const int MAX_USER_ID_LENGTH = 100;

        /// <summary>
        /// Độ dài tối đa cho Password
        /// </summary>
        public const int MAX_PASSWORD_LENGTH = 100;

        /// <summary>
        /// Độ dài tối đa cho IP Address
        /// </summary>
        public const int MAX_IP_ADDRESS_LENGTH = 15;

        /// <summary>
        /// Port tối thiểu
        /// </summary>
        public const int MIN_PORT = 1;

        /// <summary>
        /// Port tối đa
        /// </summary>
        public const int MAX_PORT = 65535;

        /// <summary>
        /// Timeout tối thiểu (giây)
        /// </summary>
        public const int MIN_CONNECTION_TIMEOUT = 1;

        /// <summary>
        /// Timeout tối đa (giây)
        /// </summary>
        public const int MAX_CONNECTION_TIMEOUT = 300;

        #endregion

        #region ========== FILE EXTENSIONS ==========

        /// <summary>
        /// Extension file XML
        /// </summary>
        public const string XML_FILE_EXTENSION = ".xml";

        /// <summary>
        /// Extension file Excel 2007+
        /// </summary>
        public const string XLSX_FILE_EXTENSION = ".xlsx";

        /// <summary>
        /// Extension file Excel 97-2003
        /// </summary>
        public const string XLS_FILE_EXTENSION = ".xls";

        /// <summary>
        /// Extension file CSV
        /// </summary>
        public const string CSV_FILE_EXTENSION = ".csv";

        /// <summary>
        /// Extension file JSON
        /// </summary>
        public const string JSON_FILE_EXTENSION = ".json";

        /// <summary>
        /// Extension file Config
        /// </summary>
        public const string CONFIG_FILE_EXTENSION = ".config";

        #endregion

        #region ========== DATE TIME FORMATS ==========

        /// <summary>
        /// Format ngày tháng cho file name
        /// </summary>
        public const string DATE_TIME_FILE_FORMAT = "yyyyMMdd_HHmmss";

        /// <summary>
        /// Format ngày tháng cho display
        /// </summary>
        public const string DATE_TIME_DISPLAY_FORMAT = "dd/MM/yyyy HH:mm:ss";

        /// <summary>
        /// Format ngày cho display
        /// </summary>
        public const string DATE_DISPLAY_FORMAT = "dd/MM/yyyy";

        /// <summary>
        /// Format thời gian cho display
        /// </summary>
        public const string TIME_DISPLAY_FORMAT = "HH:mm:ss";

        /// <summary>
        /// Format ngày tháng ISO 8601
        /// </summary>
        public const string DATE_TIME_ISO_FORMAT = "yyyy-MM-ddTHH:mm:ss";

        #endregion

        #region ========== MESSAGE CONSTANTS ==========

        /// <summary>
        /// Thông báo lỗi chung
        /// </summary>
        public const string ERROR_MESSAGE_GENERAL = "Đã xảy ra lỗi. Vui lòng thử lại.";

        /// <summary>
        /// Thông báo thành công chung
        /// </summary>
        public const string SUCCESS_MESSAGE_GENERAL = "Thao tác thành công.";

        /// <summary>
        /// Thông báo cảnh báo chung
        /// </summary>
        public const string WARNING_MESSAGE_GENERAL = "Cảnh báo: ";

        /// <summary>
        /// Thông báo thông tin chung
        /// </summary>
        public const string INFO_MESSAGE_GENERAL = "Thông tin: ";

        #endregion

        #region ========== VALIDATION MESSAGES ==========

        /// <summary>
        /// Thông báo lỗi tên máy chủ không được để trống
        /// </summary>
        public const string ERROR_SERVER_NAME_REQUIRED = "Tên máy chủ không được để trống";

        /// <summary>
        /// Thông báo lỗi tên database không được để trống
        /// </summary>
        public const string ERROR_DATABASE_NAME_REQUIRED = "Tên cơ sở dữ liệu không được để trống";

        /// <summary>
        /// Thông báo lỗi User ID không được để trống
        /// </summary>
        public const string ERROR_USER_ID_REQUIRED = "Tên đăng nhập không được để trống";

        /// <summary>
        /// Thông báo lỗi Password không được để trống
        /// </summary>
        public const string ERROR_PASSWORD_REQUIRED = "Mật khẩu không được để trống";

        /// <summary>
        /// Thông báo lỗi IP Address không hợp lệ
        /// </summary>
        public const string ERROR_INVALID_IP_ADDRESS = "Địa chỉ IP không hợp lệ";

        /// <summary>
        /// Thông báo lỗi Port không hợp lệ
        /// </summary>
        public const string ERROR_INVALID_PORT = "Port phải từ 1 đến 65535";

        /// <summary>
        /// Thông báo lỗi Timeout không hợp lệ
        /// </summary>
        public const string ERROR_INVALID_TIMEOUT = "Timeout kết nối phải từ 1 đến 300 giây";

        /// <summary>
        /// Thông báo lỗi kết nối database
        /// </summary>
        public const string ERROR_DATABASE_CONNECTION_FAILED = "Không thể kết nối đến database. Vui lòng kiểm tra lại thông tin cấu hình.";

        #endregion

        #region ========== LOG MESSAGES ==========

        /// <summary>
        /// Log message bắt đầu kết nối database
        /// </summary>
        public const string LOG_DATABASE_CONNECTION_START = "Bắt đầu kiểm tra kết nối database: {0}/{1}";

        /// <summary>
        /// Log message kết nối database thành công
        /// </summary>
        public const string LOG_DATABASE_CONNECTION_SUCCESS = "Kết nối database thành công: {0}/{1}";

        /// <summary>
        /// Log message lỗi kết nối database
        /// </summary>
        public const string LOG_DATABASE_CONNECTION_ERROR = "Lỗi khi kết nối database: {0}/{1}";

        /// <summary>
        /// Log message bắt đầu xuất XML
        /// </summary>
        public const string LOG_XML_EXPORT_START = "Bắt đầu xuất thông tin database đã mã hóa ra file XML";

        /// <summary>
        /// Log message xuất XML thành công
        /// </summary>
        public const string LOG_XML_EXPORT_SUCCESS = "Đã xuất thông tin database đã mã hóa ra file XML: {0}";

        /// <summary>
        /// Log message bắt đầu cài đặt service
        /// </summary>
        public const string LOG_SERVICE_INSTALL_START = "Bắt đầu cài đặt Windows Service: {0}";

        /// <summary>
        /// Log message cài đặt service thành công
        /// </summary>
        public const string LOG_SERVICE_INSTALL_SUCCESS = "Đã cài đặt Windows Service thành công: {0}";

        /// <summary>
        /// Log message lỗi cài đặt service
        /// </summary>
        public const string LOG_SERVICE_INSTALL_ERROR = "Lỗi khi cài đặt Windows Service: {0}";

        /// <summary>
        /// Log message bắt đầu xuất ADMS config
        /// </summary>
        public const string LOG_ADMS_EXPORT_START = "Bắt đầu xuất thông tin cấu hình ADMS ra file XML";

        /// <summary>
        /// Log message xuất ADMS config thành công
        /// </summary>
        public const string LOG_ADMS_EXPORT_SUCCESS = "Đã xuất thông tin cấu hình ADMS ra file XML: {0}";

        /// <summary>
        /// Log message bắt đầu application
        /// </summary>
        public const string LOG_APPLICATION_START = "Ứng dụng {0} phiên bản {1} đã khởi động";

        /// <summary>
        /// Log message kết thúc application
        /// </summary>
        public const string LOG_APPLICATION_END = "Ứng dụng {0} đã kết thúc";

        #endregion

        #region ========== UI CONSTANTS ==========

        /// <summary>
        /// Tiêu đề dialog xuất Excel
        /// </summary>
        public const string UI_EXCEL_EXPORT_TITLE = "Xuất dữ liệu Excel";

        /// <summary>
        /// Filter cho SaveFileDialog Excel
        /// </summary>
        public const string UI_EXCEL_FILE_FILTER = "Excel (*.xlsx)|*.xlsx|Excel 97-2003 (*.xls)|*.xls|All Files (*.*)|*.*";

        /// <summary>
        /// Filter cho OpenFileDialog Config
        /// </summary>
        public const string UI_CONFIG_FILE_FILTER = "Config Files (*.xml;*.config)|*.xml;*.config|All Files (*.*)|*.*";

        /// <summary>
        /// Thông báo xác nhận mở file
        /// </summary>
        public const string UI_CONFIRM_OPEN_FILE = "Bạn có muốn mở file này không?";

        /// <summary>
        /// Thông báo xác nhận cài đặt service
        /// </summary>
        public const string UI_CONFIRM_INSTALL_SERVICE = "Cài đặt Windows Service cần quyền Administrator. Bạn có muốn khởi động lại ứng dụng với quyền Administrator không?";

        /// <summary>
        /// Thông báo xác nhận gỡ cài đặt service
        /// </summary>
        public const string UI_CONFIRM_UNINSTALL_SERVICE = "Bạn có chắc chắn muốn gỡ cài đặt Windows Service {0} không?";

        /// <summary>
        /// Thông báo xác nhận đóng ứng dụng
        /// </summary>
        public const string UI_CONFIRM_EXIT_APPLICATION = "Bạn có chắc chắn muốn thoát ứng dụng không?";

        /// <summary>
        /// Thông báo xác nhận lưu thay đổi
        /// </summary>
        public const string UI_CONFIRM_SAVE_CHANGES = "Dữ liệu đã thay đổi. Bạn có muốn lưu thay đổi không?";

        #endregion

        #region ========== SECURITY CONSTANTS ==========

        /// <summary>
        /// Key để xác định dữ liệu đã được mã hóa
        /// </summary>
        public const string ENCRYPTION_FLAG_KEY = "IsEncrypted";

        /// <summary>
        /// Giá trị xác định dữ liệu đã được mã hóa
        /// </summary>
        public const bool ENCRYPTION_FLAG_VALUE = true;

        /// <summary>
        /// Salt mặc định cho encryption (nên thay đổi trong production)
        /// </summary>
        public const string DEFAULT_ENCRYPTION_SALT = "VNTA_NET_2025_SALT";

        #endregion

        #region ========== SYSTEM CONSTANTS ==========

        /// <summary>
        /// Tên ứng dụng
        /// </summary>
        public const string APPLICATION_NAME = "VNTA.NET 2025";

        /// <summary>
        /// Phiên bản ứng dụng
        /// </summary>
        public const string APPLICATION_VERSION = "1.0.0";

        /// <summary>
        /// Tên công ty
        /// </summary>
        public const string COMPANY_NAME = "VNTA";

        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public const string PRODUCT_NAME = "VNTA.NET 2025 System";

        /// <summary>
        /// Copyright information
        /// </summary>
        public const string COPYRIGHT_INFO = "Copyright © 2025 VNTA. All rights reserved.";

        #endregion

        #region ========== NETWORK CONSTANTS ==========

        /// <summary>
        /// IP Address mặc định (localhost)
        /// </summary>
        public const string DEFAULT_IP_ADDRESS = "127.0.0.1";

        /// <summary>
        /// IP Address broadcast
        /// </summary>
        public const string BROADCAST_IP_ADDRESS = "255.255.255.255";

        /// <summary>
        /// Subnet mask mặc định
        /// </summary>
        public const string DEFAULT_SUBNET_MASK = "255.255.255.0";

        /// <summary>
        /// Port mặc định cho HTTP
        /// </summary>
        public const int DEFAULT_HTTP_PORT = 80;

        /// <summary>
        /// Port mặc định cho HTTPS
        /// </summary>
        public const int DEFAULT_HTTPS_PORT = 443;

        /// <summary>
        /// Port mặc định cho SQL Server
        /// </summary>
        public const int DEFAULT_SQL_SERVER_PORT = 1433;

        #endregion

        #region ========== PERFORMANCE CONSTANTS ==========

        /// <summary>
        /// Timeout mặc định cho các thao tác (milliseconds)
        /// </summary>
        public const int DEFAULT_OPERATION_TIMEOUT = 30000;

        /// <summary>
        /// Timeout cho service operations (milliseconds)
        /// </summary>
        public const int SERVICE_OPERATION_TIMEOUT = 30000;

        /// <summary>
        /// Số lần retry mặc định
        /// </summary>
        public const int DEFAULT_RETRY_COUNT = 3;

        /// <summary>
        /// Delay giữa các lần retry (milliseconds)
        /// </summary>
        public const int DEFAULT_RETRY_DELAY = 1000;

        /// <summary>
        /// Batch size mặc định cho xử lý dữ liệu
        /// </summary>
        public const int DEFAULT_BATCH_SIZE = 1000;

        /// <summary>
        /// Maximum connection pool size
        /// </summary>
        public const int MAX_CONNECTION_POOL_SIZE = 100;

        #endregion

        #region ========== VALIDATION LIMITS ==========

        /// <summary>
        /// Validation limits cho các field
        /// </summary>
        public static class ValidationLimits
        {
            /// <summary>
            /// Độ dài tối đa cho Server Name
            /// </summary>
            public const int ServerNameMaxLength = 128;

            /// <summary>
            /// Độ dài tối đa cho Database Name
            /// </summary>
            public const int DatabaseNameMaxLength = 128;

            /// <summary>
            /// Độ dài tối đa cho User Name
            /// </summary>
            public const int UserNameMaxLength = 128;

            /// <summary>
            /// Độ dài tối thiểu cho Password
            /// </summary>
            public const int PasswordMinLength = 1;

            /// <summary>
            /// Độ dài tối đa cho Password
            /// </summary>
            public const int PasswordMaxLength = 128;
        }

        #endregion

        #region ========== VALIDATION MESSAGES ==========

        /// <summary>
        /// Validation messages cho toàn ứng dụng
        /// </summary>
        public static class ValidationMessages
        {
            /// <summary>
            /// Thông báo field bắt buộc
            /// </summary>
            public const string RequiredField = "Vui lòng nhập thông tin này";

            /// <summary>
            /// Thông báo bắt buộc chọn
            /// </summary>
            public const string RequiredSelection = "Vui lòng chọn một tùy chọn";

            /// <summary>
            /// Thông báo độ dài tối thiểu
            /// </summary>
            public const string MinLength = "Độ dài tối thiểu là {0} ký tự";

            /// <summary>
            /// Thông báo độ dài tối đa
            /// </summary>
            public const string MaxLength = "Độ dài tối đa là {0} ký tự";

            /// <summary>
            /// Thông báo Server Name bắt buộc
            /// </summary>
            public const string ServerNameRequired = "Vui lòng nhập tên Server";

            /// <summary>
            /// Thông báo Server Name quá dài
            /// </summary>
            public const string ServerNameTooLong = "Tên Server không được vượt quá 128 ký tự";

            /// <summary>
            /// Thông báo Database Name bắt buộc
            /// </summary>
            public const string DatabaseNameRequired = "Vui lòng nhập tên Database";

            /// <summary>
            /// Thông báo Database Name quá dài
            /// </summary>
            public const string DatabaseNameTooLong = "Tên Database không được vượt quá 128 ký tự";

            /// <summary>
            /// Thông báo User Name bắt buộc
            /// </summary>
            public const string UserNameRequired = "Vui lòng nhập tên đăng nhập";

            /// <summary>
            /// Thông báo User Name quá dài
            /// </summary>
            public const string UserNameTooLong = "Tên đăng nhập không được vượt quá 128 ký tự";

            /// <summary>
            /// Thông báo Password bắt buộc
            /// </summary>
            public const string PasswordRequired = "Vui lòng nhập mật khẩu";

            /// <summary>
            /// Thông báo Password quá ngắn
            /// </summary>
            public const string PasswordTooShort = "Mật khẩu không được để trống";

            /// <summary>
            /// Thông báo Password quá dài
            /// </summary>
            public const string PasswordTooLong = "Mật khẩu không được vượt quá 128 ký tự";

            /// <summary>
            /// Thông báo Authentication Method bắt buộc
            /// </summary>
            public const string AuthenticationMethodRequired = "Vui lòng chọn phương thức xác thực";

            /// <summary>
            /// Thông báo User Name bắt buộc khi sử dụng SQL Server Authentication
            /// </summary>
            public const string UserNameRequiredForSqlAuth = "Tên đăng nhập là bắt buộc khi sử dụng xác thực SQL Server";

            /// <summary>
            /// Thông báo Password bắt buộc khi sử dụng SQL Server Authentication
            /// </summary>
            public const string PasswordRequiredForSqlAuth = "Mật khẩu là bắt buộc khi sử dụng xác thực SQL Server";

            /// <summary>
            /// Tiêu đề thông báo validation thất bại
            /// </summary>
            public const string ValidationFailedTitle = "Thông tin chưa hợp lệ";

            /// <summary>
            /// Nội dung thông báo validation thất bại
            /// </summary>
            public const string ValidationFailedMessage = "Vui lòng kiểm tra lại thông tin đã nhập và sửa các lỗi được đánh dấu.";
        }

        #endregion
    }
}