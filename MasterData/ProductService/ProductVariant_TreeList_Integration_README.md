# ProductVariantDto - DevExpress TreeList Integration Guide

## 📋 **Tổng Quan**

`ProductVariantDto` đã được cải tiến để hỗ trợ đầy đủ **DevExpress TreeList** với các tính năng:

- ✅ **INotifyPropertyChanged**: Real-time updates
- ✅ **Display Order**: Sắp xếp columns theo thứ tự
- ✅ **Data Annotations**: Validation và UI metadata
- ✅ **Parent-Child Structure**: Hỗ trợ hierarchical data
- ✅ **Computed Properties**: Tính toán động

## 🎯 **Cấu Trúc TreeList**

### **Hierarchical Structure**
```
Product (Root)
├── Variant 1 (Child)
├── Variant 2 (Child)
└── Variant 3 (Child)
```

### **Key Properties cho TreeList**
```csharp
// KeyFieldName - ID duy nhất của mỗi node
treeList1.KeyFieldName = "Id";

// ParentFieldName - ID của parent node
treeList1.ParentFieldName = "ProductId";

// DataSource - BindingList<ProductVariantDto>
treeList1.DataSource = variants;
```

## 🔧 **Implementation Example**

### **1. Tạo Data Source**
```csharp
public partial class FrmProductVariantList : XtraForm
{
    private BindingList<ProductVariantDto> _variants;
    
    public FrmProductVariantList()
    {
        InitializeComponent();
        InitializeTreeList();
        LoadData();
    }
    
    private void InitializeTreeList()
    {
        // Cấu hình TreeList
        treeList1.KeyFieldName = "Id";
        treeList1.ParentFieldName = "ProductId";
        treeList1.DataSource = _variants;
        
        // Cấu hình columns theo Display Order
        treeList1.Columns.Clear();
        treeList1.Columns.AddField("ProductCode").Caption = "Mã sản phẩm";
        treeList1.Columns.AddField("ProductName").Caption = "Tên sản phẩm";
        treeList1.Columns.AddField("VariantCode").Caption = "Mã biến thể";
        treeList1.Columns.AddField("UnitCode").Caption = "Mã đơn vị";
        treeList1.Columns.AddField("UnitName").Caption = "Tên đơn vị";
        treeList1.Columns.AddField("PurchasePrice").Caption = "Giá mua";
        treeList1.Columns.AddField("SalePrice").Caption = "Giá bán";
        treeList1.Columns.AddField("StatusDisplay").Caption = "Trạng thái";
        
        // Cấu hình format
        treeList1.Columns["PurchasePrice"].DisplayFormat.FormatType = FormatType.Numeric;
        treeList1.Columns["PurchasePrice"].DisplayFormat.FormatString = "N0 VND";
        treeList1.Columns["SalePrice"].DisplayFormat.FormatType = FormatType.Numeric;
        treeList1.Columns["SalePrice"].DisplayFormat.FormatString = "N0 VND";
    }
    
    private async void LoadData()
    {
        try
        {
            // Load data từ BLL
            var variants = await _productVariantBll.GetAllAsync();
            
            // Tạo hierarchical structure
            _variants = CreateHierarchicalData(variants);
            
            // Bind to TreeList
            treeList1.DataSource = _variants;
        }
        catch (Exception ex)
        {
            MsgBox.ShowException(ex);
        }
    }
}
```

### **2. Tạo Hierarchical Data**
```csharp
private BindingList<ProductVariantDto> CreateHierarchicalData(List<ProductVariantDto> variants)
{
    var result = new BindingList<ProductVariantDto>();
    
    // Group by ProductId
    var groupedVariants = variants.GroupBy(v => v.ProductId);
    
    foreach (var group in groupedVariants)
    {
        var productVariants = group.ToList();
        
        // Tạo root node (Product)
        var rootVariant = productVariants.First();
        var rootNode = new ProductVariantDto
        {
            Id = rootVariant.ProductId, // Sử dụng ProductId làm ID cho root
            ProductId = Guid.Empty,     // Root node không có parent
            ProductCode = rootVariant.ProductCode,
            ProductName = rootVariant.ProductName,
            VariantCode = "ROOT",       // Đánh dấu root node
            UnitCode = "",
            UnitName = "",
            IsActive = true
        };
        
        result.Add(rootNode);
        
        // Thêm child nodes (Variants)
        foreach (var variant in productVariants)
        {
            variant.ProductId = rootVariant.ProductId; // Set parent ID
            result.Add(variant);
        }
    }
    
    return result;
}
```

### **3. Real-time Updates**
```csharp
// Thêm variant mới
private void AddVariant(ProductVariantDto newVariant)
{
    // Tự động trigger PropertyChanged
    _variants.Add(newVariant);
    
    // TreeList tự động refresh
}

// Cập nhật variant
private void UpdateVariant(ProductVariantDto variant)
{
    // Tìm variant trong list
    var existingVariant = _variants.FirstOrDefault(v => v.Id == variant.Id);
    if (existingVariant != null)
    {
        // Cập nhật properties - tự động trigger PropertyChanged
        existingVariant.VariantCode = variant.VariantCode;
        existingVariant.PurchasePrice = variant.PurchasePrice;
        existingVariant.SalePrice = variant.SalePrice;
        existingVariant.IsActive = variant.IsActive;
        
        // TreeList tự động refresh
    }
}
```

## 🎨 **Advanced Features**

### **1. Custom Node Display**
```csharp
private void treeList1_CustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs e)
{
    if (e.Node.GetValue("VariantCode").ToString() == "ROOT")
    {
        // Root node styling
        e.Appearance.BackColor = Color.LightBlue;
        e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
    }
    else
    {
        // Child node styling
        var variant = e.Node.Tag as ProductVariantDto;
        if (variant != null && !variant.IsActive)
        {
            e.Appearance.ForeColor = Color.Gray;
        }
    }
}
```

### **2. Context Menu**
```csharp
private void treeList1_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
{
    if (e.Node != null)
    {
        var variant = e.Node.Tag as ProductVariantDto;
        
        if (variant.VariantCode == "ROOT")
        {
            // Root node menu
            e.Menu.Items.Add(new DXMenuItem("Thêm biến thể", AddVariant_Click));
        }
        else
        {
            // Child node menu
            e.Menu.Items.Add(new DXMenuItem("Sửa biến thể", EditVariant_Click));
            e.Menu.Items.Add(new DXMenuItem("Xóa biến thể", DeleteVariant_Click));
        }
    }
}
```

### **3. Filtering và Search**
```csharp
private void ApplyFilter(string searchText)
{
    if (string.IsNullOrEmpty(searchText))
    {
        treeList1.ClearFilter();
        return;
    }
    
    // Filter theo multiple fields
    var filter = new TreeListFilterCriteria();
    filter.Add(new TreeListFilterCriteria("ProductName", "Contains", searchText));
    filter.Add(new TreeListFilterCriteria("VariantCode", "Contains", searchText));
    filter.Add(new TreeListFilterCriteria("UnitName", "Contains", searchText));
    
    treeList1.Filter = filter;
}
```

## 📊 **Data Binding Modes**

### **1. Bound Mode (Recommended)**
```csharp
// Load tất cả data một lần
var variants = await _productVariantBll.GetAllAsync();
treeList1.DataSource = CreateHierarchicalData(variants);
```

### **2. Virtual Mode (Large Data)**
```csharp
// Load data on demand
treeList1.VirtualMode = true;
treeList1.DataSource = new ProductVariantVirtualDataSource();
```

## 🔍 **Validation Integration**

### **1. Real-time Validation**
```csharp
private void treeList1_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs e)
{
    var variant = e.Row as ProductVariantDto;
    if (variant != null)
    {
        var errors = variant.GetValidationErrors();
        if (errors.Any())
        {
            e.ErrorText = string.Join("\n", errors);
            e.Valid = false;
        }
    }
}
```

### **2. Data Annotations Support**
```csharp
// TreeList tự động sử dụng Data Annotations
[DisplayName("Mã biến thể")]
[Required(ErrorMessage = "Mã biến thể không được để trống")]
[StringLength(50, ErrorMessage = "Mã biến thể không được vượt quá 50 ký tự")]
public string VariantCode { get; set; }
```

## 🚀 **Performance Tips**

### **1. Lazy Loading**
```csharp
// Chỉ load khi cần
private void treeList1_NodeExpanding(object sender, NodeExpandingEventArgs e)
{
    if (e.Node.Children.Count == 0)
    {
        LoadChildVariants(e.Node);
    }
}
```

### **2. Virtual Scrolling**
```csharp
// Cho large datasets
treeList1.OptionsView.EnableAppearanceEvenRow = true;
treeList1.OptionsView.EnableAppearanceOddRow = true;
treeList1.OptionsView.ShowIndicator = false;
```

## 📝 **Best Practices**

### **1. Data Structure**
- ✅ Sử dụng `Guid` cho ID fields
- ✅ Implement `INotifyPropertyChanged`
- ✅ Sử dụng `Display(Order = n)` cho column ordering
- ✅ Tạo computed properties cho display

### **2. Performance**
- ✅ Sử dụng `BindingList<T>` cho real-time updates
- ✅ Implement virtual mode cho large data
- ✅ Cache computed properties
- ✅ Lazy load child data

### **3. User Experience**
- ✅ Custom styling cho root/child nodes
- ✅ Context menus theo node type
- ✅ Real-time validation
- ✅ Keyboard navigation support

## 🔧 **Troubleshooting**

### **Common Issues**

1. **TreeList không hiển thị data**
   - Kiểm tra `KeyFieldName` và `ParentFieldName`
   - Đảm bảo `DataSource` không null
   - Verify hierarchical structure

2. **Real-time updates không hoạt động**
   - Đảm bảo implement `INotifyPropertyChanged`
   - Sử dụng `BindingList<T>` thay vì `List<T>`
   - Check `SetProperty` method

3. **Columns không đúng thứ tự**
   - Sử dụng `[Display(Order = n)]`
   - Clear columns trước khi add
   - Set `Caption` cho mỗi column

## 📚 **References**

- [DevExpress TreeList Data Binding](https://docs.devexpress.com/WindowsForms/5553/controls-and-libraries/tree-list/feature-center/data-binding)
- [INotifyPropertyChanged Pattern](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged)
- [Data Annotations](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations)

---

**Tác giả**: AI Assistant  
**Ngày tạo**: 2025-01-15  
**Phiên bản**: 1.0  
**Trạng thái**: ✅ Ready for Production
