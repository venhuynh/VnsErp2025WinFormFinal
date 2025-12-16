# Hướng dẫn Triển khai UI cho Hệ thống Quản lý Quyền

## Tổng quan

Tài liệu này hướng dẫn cách triển khai các form UI cho hệ thống quản lý quyền, dựa trên pattern hiện có trong project.

---

## 1. Cấu trúc Project

```
VersionAndUserManagement/
├── PermissionManagement/
│   ├── FrmRoleManagement.cs
│   ├── FrmRoleManagement.Designer.cs
│   ├── FrmRoleManagement.resx
│   ├── FrmRoleAddEdit.cs
│   ├── FrmRoleAddEdit.Designer.cs
│   ├── FrmRoleAddEdit.resx
│   ├── FrmAssignRoleToUser.cs
│   ├── FrmAssignRoleToUser.Designer.cs
│   ├── FrmAssignRoleToUser.resx
│   ├── FrmPermissionManagement.cs
│   ├── FrmPermissionManagement.Designer.cs
│   ├── FrmPermissionManagement.resx
│   ├── FrmAssignPermissionToRole.cs
│   ├── FrmAssignPermissionToRole.Designer.cs
│   ├── FrmAssignPermissionToRole.resx
│   ├── FrmAssignPermissionToUser.cs
│   ├── FrmAssignPermissionToUser.Designer.cs
│   ├── FrmAssignPermissionToUser.resx
│   ├── FrmUserPermissionSummary.cs
│   ├── FrmUserPermissionSummary.Designer.cs
│   └── FrmUserPermissionSummary.resx
```

---

## 2. Pattern chung cho các Form

### 2.1. Structure của Form

```csharp
public partial class FrmRoleManagement : DevExpress.XtraEditors.XtraForm
{
    #region ========== KHAI BÁO BIẾN ==========
    private readonly PermissionBll _permissionBll;
    private List<RoleDto> _dataList;
    private RoleDto _selectedItem;
    private bool _isLoading;
    private Guid _currentUserId; // Lấy từ session
    #endregion

    #region ========== CONSTRUCTOR ==========
    public FrmRoleManagement()
    {
        InitializeComponent();
        _permissionBll = new PermissionBll();
        _dataList = new List<RoleDto>();
        _currentUserId = GetCurrentUserId(); // TODO: Implement
        InitializeEvents();
        ConfigureGrid();
        UpdateButtonStates();
        this.Load += async (s, e) => await LoadDataAsync();
    }
    #endregion

    #region ========== KHỞI TẠO FORM ==========
    private void InitializeEvents() { }
    private void ConfigureGrid() { }
    private void UpdateButtonStates() { }
    #endregion

    #region ========== QUẢN LÝ DỮ LIỆU ==========
    private async Task LoadDataAsync() { }
    private void BindGrid(List<RoleDto> data) { }
    #endregion

    #region ========== EVENT HANDLERS ==========
    private void NewBarButtonItem_ItemClick(object sender, ItemClickEventArgs e) { }
    private void EditBarButtonItem_ItemClick(object sender, ItemClickEventArgs e) { }
    private void DeleteBarButtonItem_ItemClick(object sender, ItemClickEventArgs e) { }
    #endregion

    #region ========== HELPER METHODS ==========
    private Guid GetCurrentUserId() { }
    private void ShowError(Exception ex, string message) { }
    #endregion
}
```

### 2.2. Permission Checking Pattern

```csharp
private void UpdateButtonStates()
{
    // Kiểm tra quyền trước khi enable/disable buttons
    NewBarButtonItem.Enabled = PermissionHelper.CanCreate(_currentUserId, "Role");
    EditBarButtonItem.Enabled = _selectedItem != null && 
                                PermissionHelper.CanUpdate(_currentUserId, "Role");
    DeleteBarButtonItem.Enabled = _selectedItem != null && 
                                  !_selectedItem.IsSystemRole && 
                                  PermissionHelper.CanDelete(_currentUserId, "Role");
}

private void NewBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
{
    // Kiểm tra quyền trước khi thực hiện
    if (!PermissionHelper.CanCreate(_currentUserId, "Role"))
    {
        MsgBox.ShowWarning("Bạn không có quyền tạo vai trò mới");
        return;
    }

    var frm = new FrmRoleAddEdit();
    if (frm.ShowDialog() == DialogResult.OK)
    {
        await LoadDataAsync();
    }
}
```

---

## 3. Form 1: FrmRoleManagement

### 3.1. Designer Components

```csharp
// BarManager và Toolbar
private BarManager barManager1;
private Bar bar2; // Toolbar
private BarButtonItem NewBarButtonItem;
private BarButtonItem EditBarButtonItem;
private BarButtonItem DeleteBarButtonItem;
private BarButtonItem ListDataBarButtonItem;
private BarButtonItem ExportBarButtonItem;

// Grid
private GridControl RoleGridControl;
private GridView RoleGridView;
private BindingSource roleBindingSource;

// TabControl
private XtraTabControl tabControl1;
private XtraTabPage tabInfo; // Thông tin Role
private XtraTabPage tabUsers; // Danh sách Users
private XtraTabPage tabPermissions; // Danh sách Permissions

// Info Panel
private TextEdit txtName;
private MemoEdit txtDescription;
private CheckEdit chkIsSystemRole;
private CheckEdit chkIsActive;
```

### 3.2. Load Data

```csharp
private async Task LoadDataAsync()
{
    if (_isLoading) return;
    _isLoading = true;
    
    try
    {
        await ExecuteWithWaitingFormAsync(async () =>
        {
            var roles = await Task.Run(() => _permissionBll.GetAllRoles());
            _dataList = roles.Select(r => r.ToDto()).ToList();
            BindGrid(_dataList);
        });
    }
    catch (Exception ex)
    {
        ShowError(ex, "Lỗi tải dữ liệu");
    }
    finally
    {
        _isLoading = false;
    }
}
```

### 3.3. Bind Grid

```csharp
private void BindGrid(List<RoleDto> data)
{
    roleBindingSource.DataSource = data;
    RoleGridControl.DataSource = roleBindingSource;
    
    // Configure columns
    RoleGridView.Columns["Name"].Caption = "Tên vai trò";
    RoleGridView.Columns["Description"].Caption = "Mô tả";
    RoleGridView.Columns["IsSystemRole"].Caption = "Hệ thống";
    RoleGridView.Columns["IsActive"].Caption = "Hoạt động";
    RoleGridView.Columns["UserCount"].Caption = "Số người dùng";
    RoleGridView.Columns["PermissionCount"].Caption = "Số quyền";
    
    // Format
    RoleGridView.Columns["IsSystemRole"].DisplayFormat.FormatType = FormatType.None;
    RoleGridView.Columns["IsActive"].DisplayFormat.FormatType = FormatType.None;
}
```

### 3.4. Selection Changed

```csharp
private void RoleGridView_SelectionChanged(object sender, EventArgs e)
{
    var selectedRows = RoleGridView.GetSelectedRows();
    if (selectedRows.Length > 0)
    {
        var rowHandle = selectedRows[0];
        _selectedItem = RoleGridView.GetRow(rowHandle) as RoleDto;
        LoadRoleDetails(_selectedItem);
    }
    else
    {
        _selectedItem = null;
        ClearRoleDetails();
    }
    UpdateButtonStates();
}

private void LoadRoleDetails(RoleDto role)
{
    if (role == null) return;
    
    txtName.Text = role.Name;
    txtDescription.Text = role.Description;
    chkIsSystemRole.Checked = role.IsSystemRole;
    chkIsActive.Checked = role.IsActive;
    
    // Load Users
    LoadRoleUsers(role.Id);
    
    // Load Permissions
    LoadRolePermissions(role.Id);
}

private void LoadRoleUsers(Guid roleId)
{
    // TODO: Load users có role này
    var users = _permissionBll.GetRoleUsers(roleId);
    // Bind vào grid
}

private void LoadRolePermissions(Guid roleId)
{
    var permissions = _permissionBll.GetRolePermissions(roleId);
    var dtos = permissions.Select(p => p.ToDto()).ToList();
    // Bind vào grid
}
```

---

## 4. Form 2: FrmRoleAddEdit

### 4.1. Structure

```csharp
public partial class FrmRoleAddEdit : DevExpress.XtraEditors.XtraForm
{
    private readonly PermissionBll _permissionBll;
    private readonly RoleDto _roleDto;
    private readonly bool _isEditMode;
    private Guid _currentUserId;

    public FrmRoleAddEdit(RoleDto roleDto = null)
    {
        InitializeComponent();
        _permissionBll = new PermissionBll();
        _roleDto = roleDto;
        _isEditMode = roleDto != null;
        _currentUserId = GetCurrentUserId();
        
        if (_isEditMode)
        {
            this.Text = "Sửa vai trò";
            LoadData();
        }
        else
        {
            this.Text = "Thêm mới vai trò";
        }
    }

    private void LoadData()
    {
        txtName.Text = _roleDto.Name;
        txtDescription.Text = _roleDto.Description;
        chkIsActive.Checked = _roleDto.IsActive;
        chkIsSystemRole.Checked = _roleDto.IsSystemRole;
        chkIsSystemRole.Enabled = false; // Không cho phép đổi
    }

    private async void btnSave_Click(object sender, EventArgs e)
    {
        if (!ValidateInput()) return;

        try
        {
            await ExecuteWithWaitingFormAsync(async () =>
            {
                var role = new Role
                {
                    Id = _isEditMode ? _roleDto.Id : Guid.NewGuid(),
                    Name = txtName.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    IsActive = chkIsActive.Checked,
                    IsSystemRole = _isEditMode ? _roleDto.IsSystemRole : false,
                    ModifiedBy = _currentUserId
                };

                if (_isEditMode)
                {
                    await Task.Run(() => _permissionBll.UpdateRole(role));
                }
                else
                {
                    await Task.Run(() => _permissionBll.CreateRole(role));
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            });
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi lưu dữ liệu");
        }
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            MsgBox.ShowWarning("Tên vai trò không được để trống");
            txtName.Focus();
            return false;
        }

        // Kiểm tra trùng tên
        var existingRole = _permissionBll.GetRoleByName(txtName.Text.Trim());
        if (existingRole != null && (!_isEditMode || existingRole.Id != _roleDto.Id))
        {
            MsgBox.ShowWarning("Tên vai trò đã tồn tại");
            txtName.Focus();
            return false;
        }

        return true;
    }
}
```

---

## 5. Form 3: FrmAssignRoleToUser

### 5.1. Structure

```csharp
public partial class FrmAssignRoleToUser : DevExpress.XtraEditors.XtraForm
{
    private readonly PermissionBll _permissionBll;
    private readonly ApplicationUserBll _userBll;
    private List<ApplicationUserDto> _userList;
    private List<RoleDto> _roleList;
    private ApplicationUserDto _selectedUser;
    private Guid _currentUserId;

    public FrmAssignRoleToUser()
    {
        InitializeComponent();
        _permissionBll = new PermissionBll();
        _userBll = new ApplicationUserBll();
        _currentUserId = GetCurrentUserId();
        InitializeEvents();
        LoadData();
    }

    private async void LoadData()
    {
        await ExecuteWithWaitingFormAsync(async () =>
        {
            var users = await Task.Run(() => _userBll.GetAll());
            _userList = users;
            BindUserGrid(_userList);

            var roles = await Task.Run(() => _permissionBll.GetAllRoles());
            _roleList = roles.Select(r => r.ToDto()).ToList();
            BindRoleComboBox(_roleList);
        });
    }

    private void UserGridView_SelectionChanged(object sender, EventArgs e)
    {
        var selectedRows = UserGridView.GetSelectedRows();
        if (selectedRows.Length > 0)
        {
            var rowHandle = selectedRows[0];
            _selectedUser = UserGridView.GetRow(rowHandle) as ApplicationUserDto;
            LoadUserRoles(_selectedUser?.Id ?? Guid.Empty);
        }
    }

    private void LoadUserRoles(Guid userId)
    {
        if (userId == Guid.Empty) return;

        var roles = _permissionBll.GetUserRoles(userId);
        var dtos = roles.Select(r => r.ToDto()).ToList();
        BindUserRoleGrid(dtos);
    }

    private async void btnAssignRole_Click(object sender, EventArgs e)
    {
        if (_selectedUser == null)
        {
            MsgBox.ShowWarning("Vui lòng chọn người dùng");
            return;
        }

        var selectedRole = cmbRole.EditValue as RoleDto;
        if (selectedRole == null)
        {
            MsgBox.ShowWarning("Vui lòng chọn vai trò");
            return;
        }

        try
        {
            await ExecuteWithWaitingFormAsync(async () =>
            {
                await Task.Run(() => _permissionBll.AssignRoleToUser(
                    _selectedUser.Id, 
                    selectedRole.Id, 
                    _currentUserId));
                
                LoadUserRoles(_selectedUser.Id);
                MsgBox.ShowInfo("Gán vai trò thành công");
            });
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi gán vai trò");
        }
    }

    private async void btnRemoveRole_Click(object sender, EventArgs e)
    {
        var selectedUserRole = UserRoleGridView.GetFocusedRow() as UserRoleDto;
        if (selectedUserRole == null)
        {
            MsgBox.ShowWarning("Vui lòng chọn vai trò cần gỡ");
            return;
        }

        if (MsgBox.ShowQuestion("Bạn có chắc chắn muốn gỡ vai trò này?") != DialogResult.Yes)
            return;

        try
        {
            await ExecuteWithWaitingFormAsync(async () =>
            {
                await Task.Run(() => _permissionBll.RemoveRoleFromUser(
                    selectedUserRole.UserId, 
                    selectedUserRole.RoleId));
                
                LoadUserRoles(selectedUserRole.UserId);
                MsgBox.ShowInfo("Gỡ vai trò thành công");
            });
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi gỡ vai trò");
        }
    }
}
```

---

## 6. Form 4: FrmAssignPermissionToRole

### 6.1. Structure

```csharp
public partial class FrmAssignPermissionToRole : DevExpress.XtraEditors.XtraForm
{
    private readonly PermissionBll _permissionBll;
    private List<PermissionDto> _allPermissions;
    private List<PermissionDto> _rolePermissions;
    private RoleDto _selectedRole;
    private Guid _currentUserId;

    public FrmAssignPermissionToRole()
    {
        InitializeComponent();
        _permissionBll = new PermissionBll();
        _currentUserId = GetCurrentUserId();
        InitializeEvents();
        LoadData();
    }

    private async void LoadData()
    {
        await ExecuteWithWaitingFormAsync(async () =>
        {
            var roles = await Task.Run(() => _permissionBll.GetAllRoles());
            BindRoleComboBox(roles.Select(r => r.ToDto()).ToList());

            var permissions = await Task.Run(() => _permissionBll.GetAllPermissions());
            _allPermissions = permissions.Select(p => p.ToDto()).ToList();
            BindPermissionTreeList(_allPermissions);
        });
    }

    private void cmbRole_EditValueChanged(object sender, EventArgs e)
    {
        _selectedRole = cmbRole.EditValue as RoleDto;
        if (_selectedRole != null)
        {
            LoadRolePermissions(_selectedRole.Id);
        }
    }

    private void LoadRolePermissions(Guid roleId)
    {
        var permissions = _permissionBll.GetRolePermissions(roleId);
        _rolePermissions = permissions.Select(p => p.ToDto()).ToList();
        BindRolePermissionGrid(_rolePermissions);
        
        // Update TreeList check states
        UpdatePermissionTreeListChecks();
    }

    private void UpdatePermissionTreeListChecks()
    {
        // Check các permissions đã được gán cho role
        foreach (var node in PermissionTreeList.Nodes)
        {
            var permission = node.Tag as PermissionDto;
            if (permission != null)
            {
                bool isAssigned = _rolePermissions.Any(rp => rp.Id == permission.Id);
                node.Checked = isAssigned;
            }
        }
    }

    private async void btnAssignSelected_Click(object sender, EventArgs e)
    {
        if (_selectedRole == null)
        {
            MsgBox.ShowWarning("Vui lòng chọn vai trò");
            return;
        }

        var selectedPermissions = PermissionTreeList.Nodes
            .Where(n => n.Checked)
            .Select(n => n.Tag as PermissionDto)
            .Where(p => p != null)
            .ToList();

        if (selectedPermissions.Count == 0)
        {
            MsgBox.ShowWarning("Vui lòng chọn ít nhất một quyền");
            return;
        }

        try
        {
            await ExecuteWithWaitingFormAsync(async () =>
            {
                var permissionIds = selectedPermissions.Select(p => p.Id).ToList();
                await Task.Run(() => _permissionBll.AssignPermissionsToRole(
                    _selectedRole.Id, 
                    permissionIds, 
                    isGranted: true));
                
                LoadRolePermissions(_selectedRole.Id);
                MsgBox.ShowInfo($"Đã gán {selectedPermissions.Count} quyền cho vai trò");
            });
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi gán quyền");
        }
    }
}
```

---

## 7. Helper Methods chung

### 7.1. ExecuteWithWaitingFormAsync

```csharp
private async Task ExecuteWithWaitingFormAsync(Func<Task> action)
{
    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
    try
    {
        await action();
    }
    finally
    {
        SplashScreenManager.CloseForm();
    }
}
```

### 7.2. ShowError

```csharp
private void ShowError(Exception ex, string message)
{
    MsgBox.ShowError($"{message}: {ex.Message}", "Lỗi");
}
```

### 7.3. GetCurrentUserId

```csharp
private Guid GetCurrentUserId()
{
    // TODO: Implement - Lấy từ session/context
    // Ví dụ:
    // return SessionManager.CurrentUser?.Id ?? Guid.Empty;
    return Guid.Empty;
}
```

---

## 8. DataSources (DevExpress)

Tạo các DataSource trong Properties/DataSources:
- `DTO.VersionAndUserManagementDto.RoleDto.datasource`
- `DTO.VersionAndUserManagementDto.PermissionDto.datasource`
- `DTO.VersionAndUserManagementDto.UserRoleDto.datasource`
- `DTO.VersionAndUserManagementDto.RolePermissionDto.datasource`
- `DTO.VersionAndUserManagementDto.UserPermissionDto.datasource`

---

## 9. Resources

Thêm các icon vào Resources:
- `role_16x16.png`, `role_32x32.png`
- `permission_16x16.png`, `permission_32x32.png`
- `assign_16x16.png`, `assign_32x32.png`

---

## 10. Menu Integration

Thêm vào menu chính:

```csharp
// Trong FormMain hoặc MenuManager
var roleManagementItem = new BarButtonItem
{
    Caption = "Quản lý vai trò",
    ItemClick += (s, e) => 
    {
        var frm = new FrmRoleManagement();
        frm.ShowDialog();
    }
};

var permissionManagementItem = new BarButtonItem
{
    Caption = "Quản lý quyền",
    ItemClick += (s, e) => 
    {
        var frm = new FrmPermissionManagement();
        frm.ShowDialog();
    }
};
```

---

## 11. Testing Checklist

- [ ] Form load và hiển thị dữ liệu đúng
- [ ] Thêm mới Role thành công
- [ ] Sửa Role thành công
- [ ] Xóa Role thành công (không xóa được System Role)
- [ ] Gán Role cho User thành công
- [ ] Gỡ Role khỏi User thành công
- [ ] Gán Permission cho Role thành công
- [ ] Gỡ Permission khỏi Role thành công
- [ ] Kiểm tra quyền hoạt động đúng
- [ ] Validation hoạt động đúng
- [ ] Error handling hoạt động đúng
- [ ] Export Excel hoạt động đúng

---

**Tác giả:** AI Assistant  
**Ngày tạo:** 2025-01-27  
**Phiên bản:** 1.0
