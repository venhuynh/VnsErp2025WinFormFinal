# Hướng Dẫn Triển Khai Đầy Đủ FrmProductServiceCategory

## 📋 Tổng Quan Refactoring

Hiện tại đã hoàn thành:
- ✅ SQL Script: `ImproveProductServiceCategory.sql` (12 phases)
- ✅ DAL Repository: 4 method mới (GetActiveCategories, GetCategoriesByParent, etc.)
- ✅ BLL Layer: 12 method mới với đầy đủ async/sync pairs
- ✅ FrmProductServiceCategory: 8 method mới cho filtering, status, ordering
- ⏳ FrmProductServiceCategoryDetail: Cần thêm IsActive, SortOrder controls

## 🎯 Các Bước Triển Khai FrmProductServiceCategory

### Phase 1: Cập Nhật Form Main Layout

1. **Mở**: `MasterData/ProductService/FrmProductServiceCategory.Designer.cs`

2. **Thêm Toolbar Items sau ExportBarButtonItem**:
```csharp
// Menu dropdown cho Filter
this.FilterMenuButton = new DevExpress.XtraBars.BarSubItem();
this.FilterMenuButton.Caption = "Lọc";
this.FilterMenuButton.ItemClick += FilterMenuButton_ItemClick;

// Filter Active
this.FilterActiveMenuItem = new DevExpress.XtraBars.BarButtonItem();
this.FilterActiveMenuItem.Caption = "Chỉ danh mục hoạt động";
this.FilterActiveMenuItem.ItemClick += (s, e) => FilterActiveCategories();

// Filter Root
this.FilterRootMenuItem = new DevExpress.XtraBars.BarButtonItem();
this.FilterRootMenuItem.Caption = "Chỉ danh mục cấp 1";
this.FilterRootMenuItem.ItemClick += (s, e) => FilterRootCategories();

// Status menu
this.StatusMenuButton = new DevExpress.XtraBars.BarSubItem();
this.StatusMenuButton.Caption = "Trạng Thái";

this.ActivateMenuItem = new DevExpress.XtraBars.BarButtonItem();
this.ActivateMenuItem.Caption = "Kích hoạt";
this.ActivateMenuItem.ItemClick += (s, e) => ActivateSelectedCategories();

this.DeactivateMenuItem = new DevExpress.XtraBars.BarButtonItem();
this.DeactivateMenuItem.Caption = "Vô hiệu hóa";
this.DeactivateMenuItem.ItemClick += (s, e) => DeactivateSelectedCategories();

// Sort menu
this.SortMenuButton = new DevExpress.XtraBars.BarSubItem();
this.SortMenuButton.Caption = "Sắp Xếp";

this.MoveUpMenuItem = new DevExpress.XtraBars.BarButtonItem();
this.MoveUpMenuItem.Caption = "Di chuyển lên trên";
this.MoveUpMenuItem.ItemClick += (s, e) => MoveCategoryUp();

this.MoveDownMenuItem = new DevExpress.XtraBars.BarButtonItem();
this.MoveDownMenuItem.Caption = "Di chuyển xuống dưới";
this.MoveDownMenuItem.ItemClick += (s, e) => MoveCategoryDown();

this.ReorderMenuItem = new DevExpress.XtraBars.BarButtonItem();
this.ReorderMenuItem.Caption = "Sắp xếp lại theo tên";
this.ReorderMenuItem.ItemClick += (s, e) => ReorderCategoriesByName();
```

3. **Khai báo controls trong Designer class**:
```csharp
private DevExpress.XtraBars.BarSubItem FilterMenuButton;
private DevExpress.XtraBars.BarButtonItem FilterActiveMenuItem;
private DevExpress.XtraBars.BarButtonItem FilterRootMenuItem;
private DevExpress.XtraBars.BarSubItem StatusMenuButton;
private DevExpress.XtraBars.BarButtonItem ActivateMenuItem;
private DevExpress.XtraBars.BarButtonItem DeactivateMenuItem;
private DevExpress.XtraBars.BarSubItem SortMenuButton;
private DevExpress.XtraBars.BarButtonItem MoveUpMenuItem;
private DevExpress.XtraBars.BarButtonItem MoveDownMenuItem;
private DevExpress.XtraBars.BarButtonItem ReorderMenuItem;
```

### Phase 2: TreeList Configuration

**Cập nhật `ConfigureMultiLineGridView()`** để hỗ trợ IsActive column:

```csharp
// Thêm column IsActive vào TreeList
var isActiveColumn = treeList1.Columns.Add();
isActiveColumn.FieldName = "IsActive";
isActiveColumn.Caption = "Trạng Thái";
isActiveColumn.VisibleIndex = 4;
isActiveColumn.Width = 80;

// Column SortOrder
var sortOrderColumn = treeList1.Columns.Add();
sortOrderColumn.FieldName = "SortOrder";
sortOrderColumn.Caption = "Thứ Tự";
sortOrderColumn.VisibleIndex = 5;
sortOrderColumn.Width = 60;
```

### Phase 3: DTO Enhancement

**Đảm bảo ProductServiceCategoryDto có các properties**:
```csharp
public class ProductServiceCategoryDto
{
    public Guid Id { get; set; }
    public string CategoryCode { get; set; }
    public string CategoryName { get; set; }
    public string Description { get; set; }
    public Guid? ParentId { get; set; }
    public int ProductCount { get; set; }
    
    // NEW PROPERTIES
    public bool IsActive { get; set; } = true;
    public int? SortOrder { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public Guid? ModifiedBy { get; set; }
}
```

### Phase 4: Extension Methods

**Tạo file**: `MasterData/ProductService/ProductServiceCategoryExtensions.cs`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Dal.DataContext;
using DTO.MasterData.ProductService;

namespace MasterData.ProductService
{
    public static class ProductServiceCategoryExtensions
    {
        /// <summary>
        /// Convert entity to DTO with product count
        /// </summary>
        public static ProductServiceCategoryDto ToDtoWithCount(
            this ProductServiceCategory entity, 
            int productCount = 0)
        {
            return new ProductServiceCategoryDto
            {
                Id = entity.Id,
                CategoryCode = entity.CategoryCode,
                CategoryName = entity.CategoryName,
                Description = entity.Description,
                ParentId = entity.ParentId,
                ProductCount = productCount,
                IsActive = entity.IsActive,
                SortOrder = entity.SortOrder,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedBy,
                ModifiedDate = entity.ModifiedDate,
                ModifiedBy = entity.ModifiedBy
            };
        }

        /// <summary>
        /// Convert entities to DTOs with hierarchical structure
        /// </summary>
        public static IEnumerable<ProductServiceCategoryDto> ToDtosWithHierarchy(
            this IEnumerable<ProductServiceCategory> entities,
            Dictionary<Guid, int> counts = null)
        {
            counts = counts ?? new Dictionary<Guid, int>();
            return entities.Select(e => e.ToDtoWithCount(
                counts.ContainsKey(e.Id) ? counts[e.Id] : 0));
        }
    }
}
```

### Phase 5: FrmProductServiceCategoryDetail Enhancement

**Mở**: `FrmProductServiceCategoryDetail.Designer.cs` và thêm controls:

```csharp
// Sau DescriptionMemoEdit, thêm:

// IsActiveCheckEdit
this.IsActiveCheckEdit = new DevExpress.XtraEditors.CheckEdit();
this.IsActiveCheckEdit.Dock = System.Windows.Forms.DockStyle.Top;
this.IsActiveCheckEdit.Name = "IsActiveCheckEdit";
this.IsActiveCheckEdit.Properties.Caption = "Đang hoạt động";
this.IsActiveCheckEdit.TabIndex = 4;
this.IsActiveCheckEdit.Checked = true;
this.Controls.Add(this.IsActiveCheckEdit);

// SortOrderLabelControl
this.labelControl_SortOrder = new DevExpress.XtraEditors.LabelControl();
this.labelControl_SortOrder.Text = "Thứ tự sắp xếp:";
this.labelControl_SortOrder.Dock = System.Windows.Forms.DockStyle.Top;
this.Controls.Add(this.labelControl_SortOrder);

// SortOrderSpinEdit
this.SortOrderSpinEdit = new DevExpress.XtraEditors.SpinEdit();
this.SortOrderSpinEdit.Name = "SortOrderSpinEdit";
this.SortOrderSpinEdit.Dock = System.Windows.Forms.DockStyle.Top;
this.SortOrderSpinEdit.Properties.MaxValue = 1000;
this.SortOrderSpinEdit.Properties.MinValue = 0;
this.SortOrderSpinEdit.TabIndex = 5;
this.Controls.Add(this.SortOrderSpinEdit);
```

**Khai báo trong partial class**:
```csharp
private DevExpress.XtraEditors.CheckEdit IsActiveCheckEdit;
private DevExpress.XtraEditors.SpinEdit SortOrderSpinEdit;
private DevExpress.XtraEditors.LabelControl labelControl_SortOrder;
```

### Phase 6: Update FrmProductServiceCategoryDetail.cs

**Uncomment các dòng trong GetDataFromControls()**:
```csharp
private ProductServiceCategoryDto GetDataFromControls()
{
    Guid? parentId = null;
    
    if (ParentCategoryTreeListTreeListLookUpEdit.EditValue != null && 
        ParentCategoryTreeListTreeListLookUpEdit.EditValue != DBNull.Value)
    {
        parentId = (Guid)ParentCategoryTreeListTreeListLookUpEdit.EditValue;
    }

    // Uncomment these:
    var isActive = IsActiveCheckEdit?.Checked ?? true;
    int? sortOrder = null;
    if (SortOrderSpinEdit != null && SortOrderSpinEdit.Value > 0)
        sortOrder = (int)SortOrderSpinEdit.Value;
    
    return new ProductServiceCategoryDto
    {
        Id = _categoryId,
        CategoryCode = CategoryCodeTextEdit?.Text?.Trim(),
        CategoryName = CategoryNameTextEdit?.Text?.Trim(),
        Description = DescriptionMemoEdit?.Text?.Trim(),
        ParentId = parentId,
        IsActive = isActive,
        SortOrder = sortOrder
    };
}
```

**Uncomment dòng trong BindDataToControls()**:
```csharp
private void BindDataToControls(ProductServiceCategoryDto dto)
{
    CategoryCodeTextEdit.Text = dto.CategoryCode;
    CategoryNameTextEdit.Text = dto.CategoryName;
    DescriptionMemoEdit.Text = dto.Description;
    
    if (dto.ParentId.HasValue)
    {
        ParentCategoryTreeListTreeListLookUpEdit.EditValue = dto.ParentId.Value;
        _hasUserSelectedParent = true;
    }
    else
    {
        ParentCategoryTreeListTreeListLookUpEdit.EditValue = null;
        _hasUserSelectedParent = false;
    }

    // Uncomment these:
    if (IsActiveCheckEdit != null)
        IsActiveCheckEdit.Checked = dto.IsActive;

    if (dto.SortOrder.HasValue && SortOrderSpinEdit != null)
        SortOrderSpinEdit.Value = dto.SortOrder.Value;
}
```

## 📝 SQL Script Execution

**Chạy script trước khi deploy**:
```bash
sqlcmd -S <SERVER> -d VnsErp2025Final -i ImproveProductServiceCategory.sql
```

## ✅ Testing Checklist

- [ ] Tải dữ liệu (Load Data)
- [ ] Thêm mới danh mục
- [ ] Chỉnh sửa danh mục (kiểm tra IsActive, SortOrder)
- [ ] Xóa danh mục (kiểm tra product migration)
- [ ] Lọc danh mục hoạt động
- [ ] Lọc danh mục cấp 1
- [ ] Di chuyển danh mục lên/xuống
- [ ] Sắp xếp lại theo tên
- [ ] Kích hoạt/Vô hiệu hóa danh mục
- [ ] Xuất Excel

## 🔄 Git Workflow

```bash
git add -A
git commit -m "feat: Implement ProductServiceCategory full refactoring with IsActive, SortOrder, and hierarchy support"
git push origin ProductServiceRefactor
```

## 🚀 Deployment Notes

1. Backup database trước khi chạy SQL script
2. Deploy SQL script trước (Phase 1-12)
3. Build solution
4. Deploy UI components
5. Test comprehensively trước khi merge to main

---

**Created**: 2025-12-11
**Status**: Ready for implementation
**Dependencies**: SQL Server 2019+, DevExpress WinForms
