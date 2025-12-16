-- =============================================
-- Script tạo các bảng quản lý quyền truy cập
-- Entity Permission Management System
-- Version: 1.0
-- Date: 2025-01-27
-- =============================================

USE [VnsErp2025Final]
GO

-- =============================================
-- 1. Bảng Role - Vai trò người dùng
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Role]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Role] (
        [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        [Name] NVARCHAR(100) NOT NULL UNIQUE,
        [Description] NVARCHAR(500) NULL,
        [IsSystemRole] BIT NOT NULL DEFAULT 0, -- Role hệ thống không thể xóa
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedDate] DATETIME NOT NULL DEFAULT GETDATE(),
        [CreatedBy] UNIQUEIDENTIFIER NULL,
        [ModifiedDate] DATETIME NULL,
        [ModifiedBy] UNIQUEIDENTIFIER NULL,
        CONSTRAINT [FK_Role_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [ApplicationUser]([Id]),
        CONSTRAINT [FK_Role_ModifiedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [ApplicationUser]([Id])
    );
    
    PRINT 'Bảng Role đã được tạo thành công.';
END
ELSE
BEGIN
    PRINT 'Bảng Role đã tồn tại.';
END
GO

-- =============================================
-- 2. Bảng Permission - Quyền truy cập
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Permission]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Permission] (
        [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        [EntityName] NVARCHAR(100) NOT NULL, -- Tên entity: "ProductService", "BusinessPartner", etc.
        [Action] NVARCHAR(50) NOT NULL, -- "Read", "Create", "Update", "Delete", "Approve", etc.
        [Description] NVARCHAR(500) NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedDate] DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [UQ_Permission_EntityName_Action] UNIQUE ([EntityName], [Action])
    );
    
    CREATE INDEX [IX_Permission_EntityName] ON [Permission]([EntityName]);
    CREATE INDEX [IX_Permission_Action] ON [Permission]([Action]);
    CREATE INDEX [IX_Permission_EntityName_Action] ON [Permission]([EntityName], [Action]);
    
    PRINT 'Bảng Permission đã được tạo thành công.';
END
ELSE
BEGIN
    PRINT 'Bảng Permission đã tồn tại.';
END
GO

-- =============================================
-- 3. Bảng RolePermission - Liên kết Role và Permission
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RolePermission]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[RolePermission] (
        [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        [RoleId] UNIQUEIDENTIFIER NOT NULL,
        [PermissionId] UNIQUEIDENTIFIER NOT NULL,
        [IsGranted] BIT NOT NULL DEFAULT 1, -- true = cho phép, false = từ chối
        [CreatedDate] DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [FK_RolePermission_Role] FOREIGN KEY ([RoleId]) REFERENCES [Role]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RolePermission_Permission] FOREIGN KEY ([PermissionId]) REFERENCES [Permission]([Id]) ON DELETE CASCADE,
        CONSTRAINT [UQ_RolePermission] UNIQUE ([RoleId], [PermissionId])
    );
    
    CREATE INDEX [IX_RolePermission_RoleId] ON [RolePermission]([RoleId]);
    CREATE INDEX [IX_RolePermission_PermissionId] ON [RolePermission]([PermissionId]);
    
    PRINT 'Bảng RolePermission đã được tạo thành công.';
END
ELSE
BEGIN
    PRINT 'Bảng RolePermission đã tồn tại.';
END
GO

-- =============================================
-- 4. Bảng UserRole - Liên kết User và Role
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserRole]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[UserRole] (
        [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        [UserId] UNIQUEIDENTIFIER NOT NULL,
        [RoleId] UNIQUEIDENTIFIER NOT NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [AssignedDate] DATETIME NOT NULL DEFAULT GETDATE(),
        [AssignedBy] UNIQUEIDENTIFIER NULL,
        CONSTRAINT [FK_UserRole_ApplicationUser] FOREIGN KEY ([UserId]) REFERENCES [ApplicationUser]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_UserRole_Role] FOREIGN KEY ([RoleId]) REFERENCES [Role]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_UserRole_AssignedBy] FOREIGN KEY ([AssignedBy]) REFERENCES [ApplicationUser]([Id]),
        CONSTRAINT [UQ_UserRole] UNIQUE ([UserId], [RoleId])
    );
    
    CREATE INDEX [IX_UserRole_UserId] ON [UserRole]([UserId]);
    CREATE INDEX [IX_UserRole_RoleId] ON [UserRole]([RoleId]);
    
    PRINT 'Bảng UserRole đã được tạo thành công.';
END
ELSE
BEGIN
    PRINT 'Bảng UserRole đã tồn tại.';
END
GO

-- =============================================
-- 5. Bảng UserPermission - Quyền trực tiếp cho User (Override)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserPermission]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[UserPermission] (
        [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        [UserId] UNIQUEIDENTIFIER NOT NULL,
        [PermissionId] UNIQUEIDENTIFIER NOT NULL,
        [IsGranted] BIT NOT NULL DEFAULT 1,
        [IsOverride] BIT NOT NULL DEFAULT 1, -- Override quyền từ role
        [CreatedDate] DATETIME NOT NULL DEFAULT GETDATE(),
        [CreatedBy] UNIQUEIDENTIFIER NULL,
        CONSTRAINT [FK_UserPermission_ApplicationUser] FOREIGN KEY ([UserId]) REFERENCES [ApplicationUser]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_UserPermission_Permission] FOREIGN KEY ([PermissionId]) REFERENCES [Permission]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_UserPermission_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [ApplicationUser]([Id]),
        CONSTRAINT [UQ_UserPermission] UNIQUE ([UserId], [PermissionId])
    );
    
    CREATE INDEX [IX_UserPermission_UserId] ON [UserPermission]([UserId]);
    CREATE INDEX [IX_UserPermission_PermissionId] ON [UserPermission]([PermissionId]);
    
    PRINT 'Bảng UserPermission đã được tạo thành công.';
END
ELSE
BEGIN
    PRINT 'Bảng UserPermission đã tồn tại.';
END
GO

-- =============================================
-- 6. View: vw_UserPermissions - Lấy tất cả quyền của user
-- =============================================
IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_UserPermissions')
    DROP VIEW [dbo].[vw_UserPermissions];
GO

CREATE VIEW [dbo].[vw_UserPermissions]
AS
-- Quyền từ Role
SELECT DISTINCT
    ur.UserId,
    p.EntityName,
    p.Action,
    rp.IsGranted,
    'Role' AS PermissionSource,
    r.Name AS RoleName
FROM UserRole ur
INNER JOIN RolePermission rp ON ur.RoleId = rp.RoleId
INNER JOIN Permission p ON rp.PermissionId = p.Id
INNER JOIN Role r ON ur.RoleId = r.Id
WHERE ur.IsActive = 1 
    AND rp.IsGranted = 1
    AND p.IsActive = 1
    AND r.IsActive = 1

UNION

-- Quyền trực tiếp từ UserPermission (Override)
SELECT 
    up.UserId,
    p.EntityName,
    p.Action,
    up.IsGranted,
    'User' AS PermissionSource,
    NULL AS RoleName
FROM UserPermission up
INNER JOIN Permission p ON up.PermissionId = p.Id
WHERE up.IsGranted = 1
    AND p.IsActive = 1;
GO

PRINT 'View vw_UserPermissions đã được tạo thành công.';
GO

-- =============================================
-- 7. Stored Procedure: sp_CheckUserPermission
-- =============================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_CheckUserPermission')
    DROP PROCEDURE [dbo].[sp_CheckUserPermission];
GO

CREATE PROCEDURE [dbo].[sp_CheckUserPermission]
    @UserId UNIQUEIDENTIFIER,
    @EntityName NVARCHAR(100),
    @Action NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @HasPermission BIT = 0;
    
    -- Kiểm tra quyền từ UserPermission trước (Override)
    IF EXISTS (
        SELECT 1 
        FROM UserPermission up
        INNER JOIN Permission p ON up.PermissionId = p.Id
        WHERE up.UserId = @UserId
            AND p.EntityName = @EntityName
            AND p.Action = @Action
            AND up.IsGranted = 1
            AND p.IsActive = 1
    )
    BEGIN
        SET @HasPermission = 1;
    END
    -- Nếu không có override, kiểm tra từ Role
    ELSE IF EXISTS (
        SELECT 1 
        FROM UserRole ur
        INNER JOIN RolePermission rp ON ur.RoleId = rp.RoleId
        INNER JOIN Permission p ON rp.PermissionId = p.Id
        WHERE ur.UserId = @UserId
            AND p.EntityName = @EntityName
            AND p.Action = @Action
            AND ur.IsActive = 1
            AND rp.IsGranted = 1
            AND p.IsActive = 1
    )
    BEGIN
        SET @HasPermission = 1;
    END
    
    SELECT @HasPermission AS HasPermission;
END
GO

PRINT 'Stored Procedure sp_CheckUserPermission đã được tạo thành công.';
GO

-- =============================================
-- 8. Insert dữ liệu mặc định: Roles
-- =============================================
-- Administrator Role
IF NOT EXISTS (SELECT 1 FROM [Role] WHERE [Name] = 'Administrator')
BEGIN
    INSERT INTO [Role] ([Id], [Name], [Description], [IsSystemRole], [IsActive])
    VALUES (NEWID(), 'Administrator', 'Quản trị viên - Toàn quyền hệ thống', 1, 1);
    PRINT 'Đã tạo Role: Administrator';
END

-- Manager Role
IF NOT EXISTS (SELECT 1 FROM [Role] WHERE [Name] = 'Manager')
BEGIN
    INSERT INTO [Role] ([Id], [Name], [Description], [IsSystemRole], [IsActive])
    VALUES (NEWID(), 'Manager', 'Quản lý - Quyền quản lý cao', 0, 1);
    PRINT 'Đã tạo Role: Manager';
END

-- User Role
IF NOT EXISTS (SELECT 1 FROM [Role] WHERE [Name] = 'User')
BEGIN
    INSERT INTO [Role] ([Id], [Name], [Description], [IsSystemRole], [IsActive])
    VALUES (NEWID(), 'User', 'Người dùng - Quyền cơ bản', 0, 1);
    PRINT 'Đã tạo Role: User';
END

-- Viewer Role
IF NOT EXISTS (SELECT 1 FROM [Role] WHERE [Name] = 'Viewer')
BEGIN
    INSERT INTO [Role] ([Id], [Name], [Description], [IsSystemRole], [IsActive])
    VALUES (NEWID(), 'Viewer', 'Người xem - Chỉ đọc dữ liệu', 0, 1);
    PRINT 'Đã tạo Role: Viewer';
END
GO

-- =============================================
-- 9. Insert Permissions cho tất cả 30 entities
-- =============================================
DECLARE @Entities TABLE (EntityName NVARCHAR(100));
INSERT INTO @Entities VALUES
    ('AllowedMacAddress'),
    ('ApplicationUser'),
    ('VnsErpApplicationVersion'),
    ('Company'),
    ('CompanyBranch'),
    ('Department'),
    ('Employee'),
    ('Position'),
    ('BusinessPartner'),
    ('BusinessPartnerCategory'),
    ('BusinessPartner_BusinessPartnerCategory'),
    ('BusinessPartnerContact'),
    ('BusinessPartnerSite'),
    ('ProductService'),
    ('ProductServiceCategory'),
    ('ProductVariant'),
    ('ProductImage'),
    ('Attribute'),
    ('AttributeValue'),
    ('VariantAttribute'),
    ('UnitOfMeasure'),
    ('Asset'),
    ('Device'),
    ('DeviceHistory'),
    ('DeviceTransfer'),
    ('InventoryBalance'),
    ('StockInOutMaster'),
    ('StockInOutDetail'),
    ('StockInOutDocument'),
    ('StockInOutImage'),
    ('Warranty');

DECLARE @Actions TABLE (ActionName NVARCHAR(50));
INSERT INTO @Actions VALUES ('Read'), ('Create'), ('Update'), ('Delete');

-- Insert permissions
INSERT INTO [Permission] ([EntityName], [Action], [Description])
SELECT 
    e.EntityName,
    a.ActionName,
    CONCAT('Quyền ', a.ActionName, ' trên ', e.EntityName)
FROM @Entities e
CROSS JOIN @Actions a
WHERE NOT EXISTS (
    SELECT 1 FROM [Permission] p 
    WHERE p.EntityName = e.EntityName AND p.Action = a.ActionName
);

PRINT 'Đã tạo Permissions cho tất cả entities.';
GO

-- =============================================
-- 10. Gán quyền Full Access cho Administrator Role
-- =============================================
DECLARE @AdminRoleId UNIQUEIDENTIFIER;
SELECT @AdminRoleId = [Id] FROM [Role] WHERE [Name] = 'Administrator';

IF @AdminRoleId IS NOT NULL
BEGIN
    INSERT INTO [RolePermission] ([RoleId], [PermissionId], [IsGranted])
    SELECT @AdminRoleId, [Id], 1
    FROM [Permission]
    WHERE NOT EXISTS (
        SELECT 1 FROM [RolePermission] rp 
        WHERE rp.RoleId = @AdminRoleId AND rp.PermissionId = [Permission].[Id]
    );
    
    PRINT 'Đã gán toàn bộ quyền cho Administrator role.';
END
GO

-- =============================================
-- 11. Gán quyền Read Only cho Viewer Role
-- =============================================
DECLARE @ViewerRoleId UNIQUEIDENTIFIER;
SELECT @ViewerRoleId = [Id] FROM [Role] WHERE [Name] = 'Viewer';

IF @ViewerRoleId IS NOT NULL
BEGIN
    INSERT INTO [RolePermission] ([RoleId], [PermissionId], [IsGranted])
    SELECT @ViewerRoleId, [Id], 1
    FROM [Permission]
    WHERE [Action] = 'Read'
        AND NOT EXISTS (
            SELECT 1 FROM [RolePermission] rp 
            WHERE rp.RoleId = @ViewerRoleId AND rp.PermissionId = [Permission].[Id]
        );
    
    PRINT 'Đã gán quyền Read Only cho Viewer role.';
END
GO

PRINT '=============================================';
PRINT 'Hoàn thành tạo hệ thống quản lý quyền!';
PRINT '=============================================';
GO
