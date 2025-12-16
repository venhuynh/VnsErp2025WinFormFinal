-- =============================================
-- Tạo bảng Setting để lưu cấu hình hệ thống
-- Hỗ trợ lưu cấu hình NAS Storage và các cấu hình khác
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Setting]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Setting](
        [SettingId] [int] IDENTITY(1,1) NOT NULL,
        
        -- Thông tin cấu hình
        [Category] [nvarchar](100) NOT NULL,           -- Category: NAS, ImageStorage, System, etc.
        [SettingKey] [nvarchar](200) NOT NULL,          -- Key: NAS.ServerName, ImageStorage.Thumbnail.Width, etc.
        [SettingValue] [nvarchar](max) NULL,            -- Value: có thể là string, JSON, hoặc serialized object
        [ValueType] [nvarchar](50) NOT NULL DEFAULT 'String', -- String, Int, Bool, DateTime, Decimal, JSON
        
        -- Mô tả và metadata
        [Description] [nvarchar](500) NULL,            -- Mô tả về setting này
        [IsEncrypted] [bit] NOT NULL DEFAULT 0,        -- Đánh dấu giá trị có được mã hóa không (cho password, etc.)
        [IsSystem] [bit] NOT NULL DEFAULT 0,            -- Đánh dấu là system setting (không cho user sửa)
        [IsActive] [bit] NOT NULL DEFAULT 1,            -- Có đang sử dụng không
        
        -- Nhóm và sắp xếp
        [GroupName] [nvarchar](100) NULL,              -- Nhóm settings (NAS, Thumbnail, Compression, etc.)
        [DisplayOrder] [int] NULL DEFAULT 0,           -- Thứ tự hiển thị trong UI
        
        -- Audit fields
        [CreatedDate] [datetime] NOT NULL DEFAULT GETDATE(),
        [CreatedBy] [nvarchar](100) NULL,
        [UpdatedDate] [datetime] NULL,
        [UpdatedBy] [nvarchar](100) NULL,
        
        -- Constraints
        CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED ([SettingId] ASC),
        CONSTRAINT [UQ_Setting_Category_Key] UNIQUE NONCLUSTERED ([Category] ASC, [SettingKey] ASC)
    )
    
    -- Indexes
    CREATE INDEX [IX_Setting_Category] ON [dbo].[Setting]([Category])
    CREATE INDEX [IX_Setting_Category_Group] ON [dbo].[Setting]([Category], [GroupName])
    CREATE INDEX [IX_Setting_IsActive] ON [dbo].[Setting]([IsActive])
    
    PRINT 'Bảng Setting đã được tạo thành công'
END
ELSE
BEGIN
    PRINT 'Bảng Setting đã tồn tại'
END
GO

-- =============================================
-- Insert dữ liệu mẫu cho NAS Configuration
-- =============================================

-- Xóa dữ liệu cũ nếu có (optional)
-- DELETE FROM [dbo].[Setting] WHERE Category = 'NAS'

-- NAS Storage Type
IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'NAS' AND SettingKey = 'StorageType')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('NAS', 'StorageType', 'NAS', 'String', 'Loại storage: NAS, Local, Cloud', 'General', 1, 0)

-- NAS Server Configuration
IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'NAS' AND SettingKey = 'ServerName')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('NAS', 'ServerName', '', 'String', 'Tên server NAS (ví dụ: \\192.168.1.100)', 'Connection', 2, 0)

IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'NAS' AND SettingKey = 'ShareName')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('NAS', 'ShareName', 'ERP_Images', 'String', 'Tên share folder trên NAS', 'Connection', 3, 0)

IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'NAS' AND SettingKey = 'BasePath')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('NAS', 'BasePath', '', 'String', 'Đường dẫn đầy đủ đến NAS share', 'Connection', 4, 0)

IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'NAS' AND SettingKey = 'Username')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsEncrypted, IsSystem)
    VALUES ('NAS', 'Username', '', 'String', 'Username để kết nối NAS', 'Connection', 5, 0, 0)

IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'NAS' AND SettingKey = 'Password')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsEncrypted, IsSystem)
    VALUES ('NAS', 'Password', '', 'String', 'Password để kết nối NAS (được mã hóa)', 'Connection', 6, 1, 0)

IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'NAS' AND SettingKey = 'Protocol')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('NAS', 'Protocol', 'SMB', 'String', 'Protocol: SMB, NFS, FTP', 'Connection', 7, 0)

IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'NAS' AND SettingKey = 'ConnectionTimeout')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('NAS', 'ConnectionTimeout', '30', 'Int', 'Connection timeout (seconds)', 'Connection', 8, 0)

IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'NAS' AND SettingKey = 'RetryAttempts')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('NAS', 'RetryAttempts', '3', 'Int', 'Số lần retry khi kết nối thất bại', 'Connection', 9, 0)

-- Path Configuration
IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'ImageStorage' AND SettingKey = 'Path.Products')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('ImageStorage', 'Path.Products', 'Products', 'String', 'Đường dẫn cho hình ảnh sản phẩm', 'Paths', 10, 0)

IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'ImageStorage' AND SettingKey = 'Path.StockInOut')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('ImageStorage', 'Path.StockInOut', 'StockInOut', 'String', 'Đường dẫn cho hình ảnh phiếu nhập/xuất', 'Paths', 11, 0)

IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'ImageStorage' AND SettingKey = 'Path.Company')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('ImageStorage', 'Path.Company', 'Company', 'String', 'Đường dẫn cho logo công ty', 'Paths', 12, 0)

IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'ImageStorage' AND SettingKey = 'Path.Avatars')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('ImageStorage', 'Path.Avatars', 'Avatars', 'String', 'Đường dẫn cho avatar', 'Paths', 13, 0)

IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'ImageStorage' AND SettingKey = 'Path.Temp')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('ImageStorage', 'Path.Temp', 'Temp', 'String', 'Đường dẫn cho file tạm', 'Paths', 14, 0)

-- Thumbnail Configuration
IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'ImageStorage' AND SettingKey = 'Thumbnail.Enable')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('ImageStorage', 'Thumbnail.Enable', 'true', 'Bool', 'Bật/tắt tạo thumbnail tự động', 'Thumbnail', 15, 0)

IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'ImageStorage' AND SettingKey = 'Thumbnail.Width')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('ImageStorage', 'Thumbnail.Width', '200', 'Int', 'Chiều rộng thumbnail (pixels)', 'Thumbnail', 16, 0)

IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'ImageStorage' AND SettingKey = 'Thumbnail.Height')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('ImageStorage', 'Thumbnail.Height', '200', 'Int', 'Chiều cao thumbnail (pixels)', 'Thumbnail', 17, 0)

IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'ImageStorage' AND SettingKey = 'Thumbnail.Quality')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('ImageStorage', 'Thumbnail.Quality', '80', 'Int', 'Chất lượng thumbnail (1-100)', 'Thumbnail', 18, 0)

-- Image Processing Configuration
IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'ImageStorage' AND SettingKey = 'Compression.Enable')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('ImageStorage', 'Compression.Enable', 'true', 'Bool', 'Bật/tắt nén hình ảnh', 'Processing', 19, 0)

IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'ImageStorage' AND SettingKey = 'Compression.Quality')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('ImageStorage', 'Compression.Quality', '80', 'Int', 'Chất lượng nén (1-100)', 'Processing', 20, 0)

IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'ImageStorage' AND SettingKey = 'MaxFileSize')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('ImageStorage', 'MaxFileSize', '10485760', 'Int', 'Kích thước file tối đa (bytes) - 10MB', 'Processing', 21, 0)

IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'ImageStorage' AND SettingKey = 'AllowedExtensions')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('ImageStorage', 'AllowedExtensions', 'jpg,jpeg,png,gif,bmp,webp', 'String', 'Các extension được phép (phân cách bằng dấu phẩy)', 'Processing', 22, 0)

-- File Management Configuration
IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'ImageStorage' AND SettingKey = 'Verification.Enable')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('ImageStorage', 'Verification.Enable', 'true', 'Bool', 'Bật/tắt verify file integrity', 'Management', 23, 0)

IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'ImageStorage' AND SettingKey = 'Verification.IntervalHours')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('ImageStorage', 'Verification.IntervalHours', '24', 'Int', 'Khoảng thời gian verify file (hours)', 'Management', 24, 0)

IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'ImageStorage' AND SettingKey = 'Cleanup.Enable')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('ImageStorage', 'Cleanup.Enable', 'true', 'Bool', 'Bật/tắt auto cleanup orphaned files', 'Management', 25, 0)

IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'ImageStorage' AND SettingKey = 'Cleanup.OrphanedFileRetentionDays')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('ImageStorage', 'Cleanup.OrphanedFileRetentionDays', '30', 'Int', 'Số ngày giữ lại orphaned files', 'Management', 26, 0)

-- Performance Configuration
IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'ImageStorage' AND SettingKey = 'Cache.Enable')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('ImageStorage', 'Cache.Enable', 'true', 'Bool', 'Bật/tắt cache', 'Performance', 27, 0)

IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'ImageStorage' AND SettingKey = 'Cache.SizeMB')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('ImageStorage', 'Cache.SizeMB', '500', 'Int', 'Kích thước cache (MB)', 'Performance', 28, 0)

IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'ImageStorage' AND SettingKey = 'Async.Enable')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('ImageStorage', 'Async.Enable', 'true', 'Bool', 'Bật/tắt async processing', 'Performance', 29, 0)

IF NOT EXISTS (SELECT * FROM [dbo].[Setting] WHERE Category = 'ImageStorage' AND SettingKey = 'Async.MaxConcurrent')
    INSERT INTO [dbo].[Setting] (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
    VALUES ('ImageStorage', 'Async.MaxConcurrent', '10', 'Int', 'Số lượng operations đồng thời tối đa', 'Performance', 30, 0)

PRINT 'Đã insert dữ liệu mẫu cho NAS và ImageStorage configuration'
GO
