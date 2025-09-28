-- Script để thêm cột CategoryCode vào bảng ProductServiceCategory
-- Date: 28/09/2025
-- Description: Thêm mã phân loại với ràng buộc bắt buộc và duy nhất

USE [VnsErp2025Final]
GO

-- Bước 1: Thêm cột CategoryCode
ALTER TABLE [dbo].[ProductServiceCategory]
ADD [CategoryCode] [nvarchar](50) NULL
GO

-- Bước 2: Cập nhật dữ liệu hiện có (nếu có)
-- Tạo mã phân loại từ tên danh mục cho các bản ghi hiện có
UPDATE [dbo].[ProductServiceCategory]
SET [CategoryCode] = 
    CASE 
        WHEN [CategoryName] IS NOT NULL THEN
            -- Lấy chữ cái đầu của mỗi từ trong CategoryName
            UPPER(
                STUFF((
                    SELECT '' + SUBSTRING(value, 1, 1)
                    FROM STRING_SPLIT(REPLACE(REPLACE(REPLACE([CategoryName], '  ', ' '), CHAR(9), ' '), CHAR(10), ' '), ' ')
                    WHERE value != ''
                    FOR XML PATH('')
                ), 1, 0, '')
            )
        ELSE 'CAT'
    END
WHERE [CategoryCode] IS NULL
GO

-- Bước 3: Đặt NOT NULL constraint
ALTER TABLE [dbo].[ProductServiceCategory]
ALTER COLUMN [CategoryCode] [nvarchar](50) NOT NULL
GO

-- Bước 4: Tạo unique constraint cho CategoryCode
ALTER TABLE [dbo].[ProductServiceCategory]
ADD CONSTRAINT [UK_ProductServiceCategory_CategoryCode] UNIQUE ([CategoryCode])
GO

-- Bước 5: Thêm check constraint để đảm bảo format (chỉ chữ cái và số)
ALTER TABLE [dbo].[ProductServiceCategory]
ADD CONSTRAINT [CK_ProductServiceCategory_CategoryCode_Format] 
CHECK ([CategoryCode] LIKE '[A-Z0-9][A-Z0-9]%' AND LEN([CategoryCode]) >= 2)
GO

-- Bước 6: Tạo index để tối ưu performance
CREATE NONCLUSTERED INDEX [IX_ProductServiceCategory_CategoryCode] 
ON [dbo].[ProductServiceCategory] ([CategoryCode])
GO

-- Kiểm tra kết quả
SELECT 
    [Id],
    [CategoryName],
    [CategoryCode],
    [Description],
    [ParentId]
FROM [dbo].[ProductServiceCategory]
ORDER BY [CategoryCode]
GO

PRINT 'Đã thêm cột CategoryCode thành công với các ràng buộc:'
PRINT '- NOT NULL: Bắt buộc'
PRINT '- UNIQUE: Duy nhất'
PRINT '- CHECK: Chỉ chữ cái và số, tối thiểu 2 ký tự'
PRINT '- INDEX: Tối ưu performance'
GO
