using System;
using System.Xml.Serialization;

namespace Common.Appconfig
{
    /// <summary>
    /// DTO cho NAS Configuration Data
    /// </summary>
    [Serializable]
    [XmlRoot("NASConfig")]
    public class NASConfigData
    {
        /// <summary>
        /// Loại storage: NAS, Local
        /// </summary>
        [XmlElement("StorageType")]
        public string StorageType { get; set; } = "NAS";

        /// <summary>
        /// Tên server NAS
        /// </summary>
        [XmlElement("ServerName")]
        public string ServerName { get; set; } = "";

        /// <summary>
        /// Tên share folder
        /// </summary>
        [XmlElement("ShareName")]
        public string ShareName { get; set; } = "ERP_Images";

        /// <summary>
        /// Đường dẫn đầy đủ
        /// </summary>
        [XmlElement("BasePath")]
        public string BasePath { get; set; } = "";

        /// <summary>
        /// Username
        /// </summary>
        [XmlElement("Username")]
        public string Username { get; set; } = "";

        /// <summary>
        /// Password (có thể được mã hóa)
        /// </summary>
        [XmlElement("Password")]
        public string Password { get; set; } = "";

        /// <summary>
        /// Protocol: SMB, NFS, FTP
        /// </summary>
        [XmlElement("Protocol")]
        public string Protocol { get; set; } = "SMB";

        /// <summary>
        /// Connection timeout (seconds)
        /// </summary>
        [XmlElement("ConnectionTimeout")]
        public int ConnectionTimeout { get; set; } = 30;

        /// <summary>
        /// Số lần retry
        /// </summary>
        [XmlElement("RetryAttempts")]
        public int RetryAttempts { get; set; } = 3;

        /// <summary>
        /// Bật cache
        /// </summary>
        [XmlElement("EnableCache")]
        public bool EnableCache { get; set; } = false;

        /// <summary>
        /// Kích thước cache (MB)
        /// </summary>
        [XmlElement("CacheSize")]
        public int CacheSize { get; set; } = 500;

        /// <summary>
        /// Ngày cập nhật cuối cùng
        /// </summary>
        [XmlElement("LastUpdated")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        /// <summary>
        /// Clone object
        /// </summary>
        public NASConfigData Clone()
        {
            return new NASConfigData
            {
                StorageType = this.StorageType,
                ServerName = this.ServerName,
                ShareName = this.ShareName,
                BasePath = this.BasePath,
                Username = this.Username,
                Password = this.Password,
                Protocol = this.Protocol,
                ConnectionTimeout = this.ConnectionTimeout,
                RetryAttempts = this.RetryAttempts,
                EnableCache = this.EnableCache,
                CacheSize = this.CacheSize,
                LastUpdated = this.LastUpdated
            };
        }
    }
}

