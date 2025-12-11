// Phần cập nhật cho FrmProductServiceCategoryDetail.cs
// Thêm vào sau BindDataToControls() và GetDataFromControls()

// ========== PHẦN CẬP NHẬT GetDataFromControls() ==========
/*
private ProductServiceCategoryDto GetDataFromControls()
{
    Guid? parentId = null;
    
    // Lấy giá trị từ TreeListLookUpEdit
    if (ParentCategoryTreeListTreeListLookUpEdit.EditValue != null && ParentCategoryTreeListTreeListLookUpEdit.EditValue != DBNull.Value)
    {
        parentId = (Guid)ParentCategoryTreeListTreeListLookUpEdit.EditValue;
    }

    // Lấy IsActive status
    var isActive = true; // Mặc định true khi thêm mới
    // if (IsActiveCheckEdit != null)
    //     isActive = IsActiveCheckEdit.Checked;

    // Lấy SortOrder
    int? sortOrder = null;
    // if (SortOrderSpinEdit != null && SortOrderSpinEdit.Value > 0)
    //     sortOrder = (int)SortOrderSpinEdit.Value;
    
    return new ProductServiceCategoryDto
    {
        Id = _categoryId, // Sử dụng _categoryId (Guid.Empty cho thêm mới, ID thực cho edit)
        CategoryCode = CategoryCodeTextEdit?.Text?.Trim(),
        CategoryName = CategoryNameTextEdit?.Text?.Trim(),
        Description = DescriptionMemoEdit?.Text?.Trim(),
        ParentId = parentId,
        IsActive = isActive,
        SortOrder = sortOrder
    };
}
*/

// ========== PHẦN CẬP NHẬT BindDataToControls() ==========
/*
private void BindDataToControls(ProductServiceCategoryDto dto)
{
    CategoryCodeTextEdit.Text = dto.CategoryCode;
    CategoryNameTextEdit.Text = dto.CategoryName;
    DescriptionMemoEdit.Text = dto.Description;
    
    // Chọn danh mục cha trong TreeListLookUpEdit
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

    // Bind IsActive status (nếu có control)
    // if (dto.IsActive && IsActiveCheckEdit != null)
    //     IsActiveCheckEdit.Checked = dto.IsActive;

    // Bind SortOrder (nếu có control)
    // if (dto.SortOrder.HasValue && SortOrderSpinEdit != null)
    //     SortOrderSpinEdit.Value = dto.SortOrder.Value;
}
*/

// ========== THÊM MỚI CONTROLS TRONG DESIGNER ==========
/*
1. IsActiveCheckEdit (CheckEdit):
   - Caption: "Đang hoạt động"
   - Checked: true (mặc định)
   - Dock: Top hoặc dựa theo layout

2. SortOrderSpinEdit (SpinEdit):
   - Properties.MaxValue: 1000
   - Properties.MinValue: 0
   - Properties.AllowFocused: true
   - EditValue: 0 (mặc định)
   - Label: "Thứ tự sắp xếp"
*/

// ========== HƯỚNG DẪN TRIỂN KHAI TỰ ĐỘC LẬP ==========
/*
BƯỚC 1: Mở FrmProductServiceCategoryDetail.Designer.cs
BƯỚC 2: Thêm sau DescriptionMemoEdit:
        
        // IsActiveCheckEdit
        this.IsActiveCheckEdit = new DevExpress.XtraEditors.CheckEdit();
        this.IsActiveCheckEdit.Dock = System.Windows.Forms.DockStyle.Top;
        this.IsActiveCheckEdit.Location = new System.Drawing.Point(0, 150);
        this.IsActiveCheckEdit.Name = "IsActiveCheckEdit";
        this.IsActiveCheckEdit.Properties.Caption = "Đang hoạt động";
        this.IsActiveCheckEdit.Size = new System.Drawing.Size(400, 20);
        this.IsActiveCheckEdit.TabIndex = 4;
        this.IsActiveCheckEdit.CheckedChanged += new System.EventHandler(this.IsActiveCheckEdit_CheckedChanged);

        // SortOrderSpinEdit
        this.labelControl_SortOrder = new DevExpress.XtraEditors.LabelControl();
        this.labelControl_SortOrder.Text = "Thứ tự sắp xếp:";
        this.labelControl_SortOrder.Location = new System.Drawing.Point(10, 175);

        this.SortOrderSpinEdit = new DevExpress.XtraEditors.SpinEdit();
        this.SortOrderSpinEdit.Location = new System.Drawing.Point(150, 175);
        this.SortOrderSpinEdit.Name = "SortOrderSpinEdit";
        this.SortOrderSpinEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        this.SortOrderSpinEdit.Properties.MaxValue = new decimal(new int[] { 1000, 0, 0, 0 });
        this.SortOrderSpinEdit.Size = new System.Drawing.Size(150, 20);
        this.SortOrderSpinEdit.TabIndex = 5;

BƯỚC 3: Khai báo controls trong partial class:
        private DevExpress.XtraEditors.CheckEdit IsActiveCheckEdit;
        private DevExpress.XtraEditors.SpinEdit SortOrderSpinEdit;
        private DevExpress.XtraEditors.LabelControl labelControl_SortOrder;

BƯỚC 4: Bỏ comment các dòng được comment lại ở trên (GetDataFromControls, BindDataToControls)

BƯỚC 5: Thêm event handler:
        private void IsActiveCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            // Nếu bỏ check IsActive, có thể cần warning nếu danh mục có sản phẩm
        }
*/
