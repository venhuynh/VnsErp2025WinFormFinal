using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList.Menu;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using MasterData.ProductService.Dto;

namespace MasterData.ProductService
{
    /// <summary>
    /// Demo form cho ProductVariant TreeList integration
    /// Hiển thị hierarchical data với DevExpress TreeList
    /// </summary>
    public partial class FrmProductVariantTreeListDemo : XtraForm
    {
        #region Private Fields
        private BindingList<ProductVariantDto> _variants;
        private TreeList _treeList;
        private PanelControl _mainPanel;
        private BarManager _barManager;
        private Bar _toolbar;
        private BarButtonItem _btnAdd;
        private BarButtonItem _btnEdit;
        private BarButtonItem _btnDelete;
        private BarButtonItem _btnRefresh;
        private BarEditItem _searchEdit;
        #endregion

        #region Constructor
        public FrmProductVariantTreeListDemo()
        {
            InitializeComponent();
            InitializeTreeList();
            LoadSampleData();
        }
        #endregion

        #region Initialize Methods
        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "ProductVariant TreeList Demo";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            
            // Main panel
            _mainPanel = new PanelControl
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };
            this.Controls.Add(_mainPanel);
            
            // Bar manager
            _barManager = new BarManager();
            _barManager.Form = this;
            
            // Toolbar
            _toolbar = new Bar(_barManager, "Main Toolbar");
            _toolbar.DockStyle = BarDockStyle.Top;
            _toolbar.OptionsBar.UseWholeRow = true;
            
            // Toolbar buttons
            _btnAdd = new BarButtonItem(_barManager, "Thêm biến thể");
            _btnAdd.ImageOptions.Image = GetAddIcon();
            _btnAdd.ItemClick += BtnAdd_ItemClick;
            
            _btnEdit = new BarButtonItem(_barManager, "Sửa biến thể");
            _btnEdit.ImageOptions.Image = GetEditIcon();
            _btnEdit.ItemClick += BtnEdit_ItemClick;
            
            _btnDelete = new BarButtonItem(_barManager, "Xóa biến thể");
            _btnDelete.ImageOptions.Image = GetDeleteIcon();
            _btnDelete.ItemClick += BtnDelete_ItemClick;
            
            _btnRefresh = new BarButtonItem(_barManager, "Làm mới");
            _btnRefresh.ImageOptions.Image = GetRefreshIcon();
            _btnRefresh.ItemClick += BtnRefresh_ItemClick;
            
            // Search edit
            _searchEdit = new BarEditItem(_barManager, "Tìm kiếm");
            _searchEdit.Edit = new TextEdit();
            _searchEdit.Edit.TextChanged += SearchEdit_TextChanged;
            
            // Add items to toolbar
            _toolbar.ItemLinks.Add(_btnAdd);
            _toolbar.ItemLinks.Add(_btnEdit);
            _toolbar.ItemLinks.Add(_btnDelete);
            _toolbar.ItemLinks.Add(new BarItemSeparator());
            _toolbar.ItemLinks.Add(_btnRefresh);
            _toolbar.ItemLinks.Add(new BarItemSeparator());
            _toolbar.ItemLinks.Add(_searchEdit);
            
            this.ResumeLayout(false);
        }
        
        private void InitializeTreeList()
        {
            // Create TreeList
            _treeList = new TreeList
            {
                Dock = DockStyle.Fill,
                Parent = _mainPanel,
                KeyFieldName = "Id",
                ParentFieldName = "ProductId",
                OptionsView = new TreeListOptionsView
                {
                    ShowIndicator = true,
                    ShowHorzLines = true,
                    ShowVertLines = true,
                    EnableAppearanceEvenRow = true,
                    EnableAppearanceOddRow = true
                },
                OptionsSelection = new TreeListOptionsSelection
                {
                    EnableAppearanceFocusedCell = true,
                    MultiSelect = true
                },
                OptionsBehavior = new TreeListOptionsBehavior
                {
                    Editable = true,
                    AllowExpandOnDblClick = true,
                    AllowIndeterminateCheckState = true
                }
            };
            
            // Configure columns
            ConfigureTreeListColumns();
            
            // Event handlers
            _treeList.CustomDrawNodeCell += TreeList_CustomDrawNodeCell;
            _treeList.PopupMenuShowing += TreeList_PopupMenuShowing;
            _treeList.ValidatingEditor += TreeList_ValidatingEditor;
            _treeList.NodeExpanding += TreeList_NodeExpanding;
            _treeList.FocusedNodeChanged += TreeList_FocusedNodeChanged;
        }
        
        private void ConfigureTreeListColumns()
        {
            _treeList.Columns.Clear();
            
            // Add columns theo Display Order
            var productCodeCol = _treeList.Columns.AddField("ProductCode");
            productCodeCol.Caption = "Mã sản phẩm";
            productCodeCol.VisibleIndex = 0;
            productCodeCol.Width = 120;
            
            var productNameCol = _treeList.Columns.AddField("ProductName");
            productNameCol.Caption = "Tên sản phẩm";
            productNameCol.VisibleIndex = 1;
            productNameCol.Width = 200;
            
            var variantCodeCol = _treeList.Columns.AddField("VariantCode");
            variantCodeCol.Caption = "Mã biến thể";
            variantCodeCol.VisibleIndex = 2;
            variantCodeCol.Width = 120;
            
            var unitCodeCol = _treeList.Columns.AddField("UnitCode");
            unitCodeCol.Caption = "Mã đơn vị";
            unitCodeCol.VisibleIndex = 3;
            unitCodeCol.Width = 100;
            
            var unitNameCol = _treeList.Columns.AddField("UnitName");
            unitNameCol.Caption = "Tên đơn vị";
            unitNameCol.VisibleIndex = 4;
            unitNameCol.Width = 150;
            
            var purchasePriceCol = _treeList.Columns.AddField("PurchasePrice");
            purchasePriceCol.Caption = "Giá mua";
            purchasePriceCol.VisibleIndex = 5;
            purchasePriceCol.Width = 120;
            purchasePriceCol.DisplayFormat.FormatType = FormatType.Numeric;
            purchasePriceCol.DisplayFormat.FormatString = "N0 VND";
            
            var salePriceCol = _treeList.Columns.AddField("SalePrice");
            salePriceCol.Caption = "Giá bán";
            salePriceCol.VisibleIndex = 6;
            salePriceCol.Width = 120;
            salePriceCol.DisplayFormat.FormatType = FormatType.Numeric;
            salePriceCol.DisplayFormat.FormatString = "N0 VND";
            
            var statusCol = _treeList.Columns.AddField("StatusDisplay");
            statusCol.Caption = "Trạng thái";
            statusCol.VisibleIndex = 7;
            statusCol.Width = 100;
            
            // Computed columns
            var fullNameCol = _treeList.Columns.AddField("FullName");
            fullNameCol.Caption = "Tên đầy đủ";
            fullNameCol.VisibleIndex = 8;
            fullNameCol.Width = 250;
            fullNameCol.Visible = false; // Hidden by default
            
            var profitCol = _treeList.Columns.AddField("ProfitDisplay");
            profitCol.Caption = "Lợi nhuận";
            profitCol.VisibleIndex = 9;
            profitCol.Width = 120;
            profitCol.Visible = false; // Hidden by default
        }
        #endregion

        #region Data Loading
        private void LoadSampleData()
        {
            try
            {
                // Tạo sample data
                var variants = CreateSampleData();
                
                // Tạo hierarchical structure
                _variants = CreateHierarchicalData(variants);
                
                // Bind to TreeList
                _treeList.DataSource = _variants;
                
                // Expand all nodes
                _treeList.ExpandAll();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Lỗi khi load data: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private List<ProductVariantDto> CreateSampleData()
        {
            var variants = new List<ProductVariantDto>();
            
            // Product 1: Bàn làm việc
            var product1Id = Guid.NewGuid();
            variants.Add(new ProductVariantDto(product1Id, "VT001", Guid.NewGuid())
            {
                ProductCode = "SP001",
                ProductName = "Bàn làm việc",
                UnitCode = "CAI",
                UnitName = "Cái",
                PurchasePrice = 500000,
                SalePrice = 750000,
                IsActive = true
            });
            
            variants.Add(new ProductVariantDto(product1Id, "VT002", Guid.NewGuid())
            {
                ProductCode = "SP001",
                ProductName = "Bàn làm việc",
                UnitCode = "BO",
                UnitName = "Bộ",
                PurchasePrice = 1200000,
                SalePrice = 1800000,
                IsActive = true
            });
            
            // Product 2: Ghế văn phòng
            var product2Id = Guid.NewGuid();
            variants.Add(new ProductVariantDto(product2Id, "VT003", Guid.NewGuid())
            {
                ProductCode = "SP002",
                ProductName = "Ghế văn phòng",
                UnitCode = "CAI",
                UnitName = "Cái",
                PurchasePrice = 300000,
                SalePrice = 450000,
                IsActive = true
            });
            
            variants.Add(new ProductVariantDto(product2Id, "VT004", Guid.NewGuid())
            {
                ProductCode = "SP002",
                ProductName = "Ghế văn phòng",
                UnitCode = "BO",
                UnitName = "Bộ",
                PurchasePrice = 800000,
                SalePrice = 1200000,
                IsActive = false
            });
            
            // Product 3: Máy tính
            var product3Id = Guid.NewGuid();
            variants.Add(new ProductVariantDto(product3Id, "VT005", Guid.NewGuid())
            {
                ProductCode = "SP003",
                ProductName = "Máy tính",
                UnitCode = "CAI",
                UnitName = "Cái",
                PurchasePrice = 15000000,
                SalePrice = 20000000,
                IsActive = true
            });
            
            return variants;
        }
        
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
        #endregion

        #region Event Handlers
        private void TreeList_CustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs e)
        {
            var variant = e.Node.Tag as ProductVariantDto;
            if (variant == null) return;
            
            if (variant.VariantCode == "ROOT")
            {
                // Root node styling
                e.Appearance.BackColor = Color.LightBlue;
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                e.Appearance.ForeColor = Color.DarkBlue;
            }
            else
            {
                // Child node styling
                if (!variant.IsActive)
                {
                    e.Appearance.ForeColor = Color.Gray;
                    e.Appearance.BackColor = Color.LightGray;
                }
            }
        }
        
        private void TreeList_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (e.Node != null)
            {
                var variant = e.Node.Tag as ProductVariantDto;
                if (variant == null) return;
                
                var menu = new TreeListMenu(e.TreeList);
                
                if (variant.VariantCode == "ROOT")
                {
                    // Root node menu
                    menu.Items.Add(new DXMenuItem("Thêm biến thể", (s, args) => AddVariant(variant)));
                    menu.Items.Add(new DXMenuItem("Xem chi tiết sản phẩm", (s, args) => ViewProductDetails(variant)));
                }
                else
                {
                    // Child node menu
                    menu.Items.Add(new DXMenuItem("Sửa biến thể", (s, args) => EditVariant(variant)));
                    menu.Items.Add(new DXMenuItem("Xóa biến thể", (s, args) => DeleteVariant(variant)));
                    menu.Items.Add(new DXMenuItem("Sao chép biến thể", (s, args) => CopyVariant(variant)));
                }
                
                e.Menu = menu;
            }
        }
        
        private void TreeList_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs e)
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
        
        private void TreeList_NodeExpanding(object sender, NodeExpandingEventArgs e)
        {
            // Có thể implement lazy loading ở đây
            if (e.Node.Children.Count == 0)
            {
                // Load child data on demand
            }
        }
        
        private void TreeList_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            UpdateToolbarState();
        }
        
        private void SearchEdit_TextChanged(object sender, EventArgs e)
        {
            var searchText = _searchEdit.Edit.Text;
            ApplyFilter(searchText);
        }
        
        private void BtnAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedNode = _treeList.FocusedNode;
            if (selectedNode != null)
            {
                var variant = selectedNode.Tag as ProductVariantDto;
                AddVariant(variant);
            }
        }
        
        private void BtnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedNode = _treeList.FocusedNode;
            if (selectedNode != null)
            {
                var variant = selectedNode.Tag as ProductVariantDto;
                if (variant?.VariantCode != "ROOT")
                {
                    EditVariant(variant);
                }
            }
        }
        
        private void BtnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedNode = _treeList.FocusedNode;
            if (selectedNode != null)
            {
                var variant = selectedNode.Tag as ProductVariantDto;
                if (variant?.VariantCode != "ROOT")
                {
                    DeleteVariant(variant);
                }
            }
        }
        
        private void BtnRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadSampleData();
        }
        #endregion

        #region Business Methods
        private void AddVariant(ProductVariantDto parentVariant)
        {
            try
            {
                var newVariant = new ProductVariantDto(parentVariant.Id, "VT_NEW", Guid.NewGuid())
                {
                    ProductCode = parentVariant.ProductCode,
                    ProductName = parentVariant.ProductName,
                    UnitCode = "CAI",
                    UnitName = "Cái",
                    PurchasePrice = 0,
                    SalePrice = 0,
                    IsActive = true
                };
                
                // Thêm vào data source
                _variants.Add(newVariant);
                
                // Expand parent node
                var parentNode = _treeList.FindNodeByKeyValue(parentVariant.Id);
                if (parentNode != null)
                {
                    parentNode.Expanded = true;
                }
                
                XtraMessageBox.Show("Đã thêm biến thể mới thành công!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Lỗi khi thêm biến thể: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void EditVariant(ProductVariantDto variant)
        {
            try
            {
                // Tạo form edit (có thể implement sau)
                XtraMessageBox.Show($"Sửa biến thể: {variant.VariantCode}", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Lỗi khi sửa biến thể: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void DeleteVariant(ProductVariantDto variant)
        {
            try
            {
                var result = XtraMessageBox.Show($"Bạn có chắc chắn muốn xóa biến thể '{variant.VariantCode}'?", 
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    _variants.Remove(variant);
                    XtraMessageBox.Show("Đã xóa biến thể thành công!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Lỗi khi xóa biến thể: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void CopyVariant(ProductVariantDto variant)
        {
            try
            {
                var copiedVariant = variant.Clone();
                copiedVariant.Id = Guid.NewGuid();
                copiedVariant.VariantCode = variant.VariantCode + "_COPY";
                
                _variants.Add(copiedVariant);
                
                XtraMessageBox.Show("Đã sao chép biến thể thành công!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Lỗi khi sao chép biến thể: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void ViewProductDetails(ProductVariantDto productVariant)
        {
            try
            {
                var details = $"Sản phẩm: {productVariant.ProductName}\n" +
                             $"Mã: {productVariant.ProductCode}\n" +
                             $"Số biến thể: {_variants.Count(v => v.ProductId == productVariant.Id && v.VariantCode != "ROOT")}";
                
                XtraMessageBox.Show(details, "Chi tiết sản phẩm", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Lỗi khi xem chi tiết: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void ApplyFilter(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                _treeList.ClearFilter();
                return;
            }
            
            // Filter theo multiple fields
            var filter = new TreeListFilterCriteria();
            filter.Add(new TreeListFilterCriteria("ProductName", "Contains", searchText));
            filter.Add(new TreeListFilterCriteria("VariantCode", "Contains", searchText));
            filter.Add(new TreeListFilterCriteria("UnitName", "Contains", searchText));
            
            _treeList.Filter = filter;
        }
        
        private void UpdateToolbarState()
        {
            var selectedNode = _treeList.FocusedNode;
            if (selectedNode != null)
            {
                var variant = selectedNode.Tag as ProductVariantDto;
                _btnEdit.Enabled = variant?.VariantCode != "ROOT";
                _btnDelete.Enabled = variant?.VariantCode != "ROOT";
            }
            else
            {
                _btnEdit.Enabled = false;
                _btnDelete.Enabled = false;
            }
        }
        #endregion

        #region Helper Methods
        private Image GetAddIcon()
        {
            // Return add icon
            return SystemIcons.Information.ToBitmap();
        }
        
        private Image GetEditIcon()
        {
            // Return edit icon
            return SystemIcons.Information.ToBitmap();
        }
        
        private Image GetDeleteIcon()
        {
            // Return delete icon
            return SystemIcons.Information.ToBitmap();
        }
        
        private Image GetRefreshIcon()
        {
            // Return refresh icon
            return SystemIcons.Information.ToBitmap();
        }
        #endregion
    }
}
