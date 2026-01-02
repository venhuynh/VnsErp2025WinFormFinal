# VNS ERP 2025 - AI Coding Assistant Instructions

## Project Overview
VNS ERP 2025 is an enterprise-grade .NET Framework 4.8 Windows Forms ERP system with DevExpress UI components. The system follows a strict 3-layer architecture (GUI → BLL → DAL) with LINQ to SQL for data access.

## Technology Stack
- **Framework**: .NET Framework 4.8, C# with `LangVersion: latest`
- **UI Framework**: Windows Forms + DevExpress WinForms v25.1
- **Data Access**: LINQ to SQL + SQL Server (connection string managed globally)
- **ORM**: VnsErp2025DataContext (DBML-based, see [Dal/DataContext/VnsErp2025.dbml](Dal/DataContext/VnsErp2025.dbml))
- **Logging**: Custom logger system ([Logger/README.md](Logger/README.md))

## Critical Build Workflow

### MSB3021 Build Lock Issue
This project has a known file locking issue where `VnsErp2025.exe` remains locked during builds. **Always use these scripts before suggesting rebuilds:**

```powershell
# Quick fix - Auto-kill processes
.\FixBuildLock.ps1 -Auto

# Or use pre-build event already configured in VnsErp2025.csproj
# Pre-build event automatically runs KillVnsErpProcess.ps1
```

**Never suggest** manual process killing or Task Manager. The project has automated solutions.

Reference: [BUILD_ERROR_FIX.md](BUILD_ERROR_FIX.md), [BUILD_ERROR_MSB3021_GUIDE.md](BUILD_ERROR_MSB3021_GUIDE.md)

## Architecture Layers

### Layer Dependencies (Strictly Enforced)
```
GUI Layer (Forms/Controls)
    ↓ calls
BLL Layer (BusinessLogic)
    ↓ calls
DAL Layer (Repositories)
    ↓ queries
Database (SQL Server via LINQ to SQL)
```

**Never skip layers**: GUI must NOT directly access DAL. Always go through BLL.

### Project Structure
```
VnsErp2025.sln
├── VnsErp2025/           # Main GUI project (entry point)
├── Bll/                  # Business Logic Layer
├── Dal/                  # Data Access Layer
├── DTO/                  # Data Transfer Objects
├── Common/               # Shared utilities, constants, validation
├── Logger/               # Logging system
├── Authentication/       # Auth forms
├── MasterData/          # Master data forms
├── Inventory/           # Inventory management forms
├── VersionAndUserManagement/  # User/role management
└── DeviceAssetManagement/     # Asset tracking
```

## Naming Conventions

### BLL Classes
Pattern: `{Entity}Bll`
- Examples: `ApplicationUserBll`, `ProductServiceBll`, `StockInOutMasterBll`
- Located in: `Bll/{Module}/{Entity}Bll.cs`

### Repository Classes
Pattern: `{Entity}Repository : I{Entity}Repository`
- Examples: `ApplicationUserRepository : IApplicationUserRepository`
- Located in: `Dal/DataAccess/Implementations/{Module}/{Entity}Repository.cs`
- Interfaces in: `Dal/DataAccess/Interfaces/{Module}/I{Entity}Repository.cs`

### Forms
- Standard forms: `Frm{Entity}` (e.g., `FrmCompany`, `FrmProductServiceList`)
- Detail forms: `Frm{Entity}Detail` (e.g., `FrmProductServiceDetail`)
- Add/Edit forms: `Frm{Entity}AddEdit` (e.g., `FrmApplicationUserDtoAddEdit`)
- All inherit from `DevExpress.XtraEditors.XtraForm`

### User Controls
- Pattern: `Uc{Entity}` (e.g., `UcNhapLuuChuyenKhoMaster`)
- Inherit from `DevExpress.XtraEditors.XtraUserControl`

### DTOs
Pattern: `{Entity}Dto`
- Located in: `DTO/{Module}/{Entity}Dto.cs`
- Use `System.ComponentModel.DataAnnotations` for validation attributes

## Data Access Patterns

### Repository Pattern (DAL)
All database operations use Repository pattern with interfaces:

```csharp
// Example: ApplicationUserBll accessing DAL
private IApplicationUserRepository _dataAccess;

private IApplicationUserRepository GetDataAccess()
{
    if (_dataAccess == null)
    {
        lock (_lockObject)
        {
            if (_dataAccess == null)
            {
                var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
                _dataAccess = new ApplicationUserRepository(globalConnectionString);
            }
        }
    }
    return _dataAccess;
}
```

**Key pattern**: BLL uses lazy-initialized, thread-safe repositories with global connection string.

### LINQ to SQL Usage
- DataContext: `VnsErp2025DataContext` (generated from DBML)
- Tables mapped to entity classes (e.g., `ApplicationUser`, `BusinessPartner`)
- Repositories convert LINQ entities to DTOs using converter classes in `Dal/DtoConverter/`

## DevExpress Components

### Common Controls Used
- `SearchLookUpEdit` - For entity lookups (e.g., warehouse selection)
- `GridControl` + `GridView` - For data grids
- `LayoutControl` - For form layouts
- `BarManager` - For ribbons/toolbars
- `DXErrorProvider` - For validation display

### Lookup Configuration Pattern
```csharp
// Setup SearchLookUpEdit
searchLookUpEdit.Properties.DataSource = bindingSource;
searchLookUpEdit.Properties.ValueMember = "Id";
searchLookUpEdit.Properties.DisplayMember = "DisplayField";
searchLookUpEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
searchLookUpEdit.Properties.PopupView = gridView;
```

## Vietnamese Language
All UI strings, comments, and documentation are in Vietnamese. When generating:
- Form labels/text: Vietnamese
- Error messages: Vietnamese
- Code comments: Vietnamese
- XML documentation: Vietnamese

## Logging Pattern
Use the centralized logger from Logger project:

```csharp
private readonly ILogger _logger = LoggerFactory.CreateLogger(LogCategory.BLL);

// Usage
_logger.Info("Thông tin");
_logger.Warning("Cảnh báo");
_logger.Error("Lỗi", exception);
```

Categories: `UI`, `BLL`, `DAL`, `Security`, `Database`, `Authentication`, `Audit`

Reference: [Logger/README.md](Logger/README.md)

## Validation System
Use ValidationHelper from Common project for reusable validation:

```csharp
// In BLL or Form
if (ValidationHelper.IsNullOrWhiteSpace(value))
{
    // Handle error
}

if (!ValidationHelper.IsValidEmail(email))
{
    // Handle error
}
```

Reference: [Common/Validation/README.md](Common/Validation/README.md)

## Common Utilities
Located in `Common/ApplicationConstants.cs`:
- Registry paths (e.g., `REGISTRY_APP_KEY`)
- System constants (e.g., `ADMS_SERVICE_NAME`)
- Default values

## Documentation
Extensive Vietnamese documentation exists in:
- `Docs/` - System architecture, module docs, database schema
- `Docs/Documentation_Index.md` - Complete index of all docs
- Module-specific READMEs in each project folder

**Always check existing docs** before implementing features. Many patterns are already documented.

## Common Pitfalls
1. **Don't bypass BLL**: GUI should never call DAL directly
2. **Connection strings**: Always use `ApplicationStartupManager.Instance.GetGlobalConnectionString()`
3. **Thread safety**: Use `lock` when initializing repositories in BLL
4. **Build locks**: Use `FixBuildLock.ps1` for MSB3021 errors, not manual fixes
5. **DevExpress version**: All components must use v25.1 (check references)
6. **Language**: All user-facing strings must be Vietnamese

## Testing
Test scripts in root:
- `RunPhase1Test.bat` - Phase 1 tests
- `TestPhase1.bat` - Test execution

No comprehensive test suite yet. Manual testing is primary QA method.

## Key Files to Reference
- [Docs/System_Architecture.md](Docs/System_Architecture.md) - Full architecture details
- [Docs/Modules_Documentation.md](Docs/Modules_Documentation.md) - Module descriptions
- [Docs/Database_Documentation.md](Docs/Database_Documentation.md) - Database schema
- [Dal/DataContext/VnsErp2025.dbml](Dal/DataContext/VnsErp2025.dbml) - ORM mappings
- [Common/ApplicationConstants.cs](Common/ApplicationConstants.cs) - System constants
