using Bll.Common;
using Bll.MasterData.ProductService;
using Bll.Utils;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using MasterData.ProductService.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MasterData.ProductService.Dto;

namespace MasterData.ProductService
{
    /// <summary>
    /// Danh sách Danh mục Sản phẩm/Dịch vụ - chỉ giữ event handler, mọi xử lý tách thành phương thức riêng.
    /// </summary>
    public partial class UcProductServiceCategory : DevExpress.XtraEditors.XtraUserControl
    {
        #region Fields

        private readonly ProductServiceCategoryBll _productServiceCategoryBll = new ProductServiceCategoryBll();
        private readonly List<Guid> _selectedCategoryIds = new List<Guid>();
        private bool _isLoading; // guard tránh gọi LoadDataAsync song song
        private bool _isProcessingCheckbox; // flag để ngăn chặn selection khi đang xử lý checkbox

        #endregion

        #region Constructor

        /// <summary>
        /// Khởi tạo control, đăng ký event UI.
        /// </summary>
        public UcProductServiceCategory()
        {
            InitializeComponent();

            // Toolbar events
            ListDataBarButtonItem.ItemClick += ListDataBarButtonItem_ItemClick;
            NewBarButtonItem.ItemClick += NewBarButtonItem_ItemClick;
            EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
            DeleteBarButtonItem.ItemClick += DeleteBarButtonItem_ItemClick;
            ExportBarButtonItem.ItemClick += ExportBarButtonItem_ItemClick;

            // TreeList events
            treeList1.SelectionChanged += TreeList1_SelectionChanged;
            treeList1.BeforeCheckNode += TreeList1_BeforeCheckNode;
            treeList1.AfterCheckNode += TreeList1_AfterCheckNode;
            treeList1.MouseDown += TreeList1_MouseDown;
            treeList1.CustomDrawNodeIndicator += TreeList1_CustomDrawNodeIndicator;
            treeList1.CustomDrawNodeCell += TreeList1_CustomDrawNodeCell;

            UpdateButtonStates();
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Thực hiện operation async với WaitingForm1 hiển thị.
        /// </summary>
        /// <param name="operation">Operation async cần thực hiện</param>
        private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
        {
            try
            {
                // Hiển thị WaitingForm1
                SplashScreenManager.ShowForm(typeof(WaitForm1));

                // Thực hiện operation
                await operation();
            }
            finally
            {
                // Đóng WaitingForm1
                SplashScreenManager.CloseForm();
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Người dùng bấm "Danh sách" để tải dữ liệu.
        /// </summary>
        private async void ListDataBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            await LoadDataAsync();
        }

        /// <summary>
        /// Người dùng bấm "Mới".
        /// </summary>
        private async void NewBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                using (OverlayManager.ShowScope(this))
                {
                    using (var form = new FrmProductServiceCategoryDetail(Guid.Empty))
                    {
                        form.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                        form.ShowDialog(this);

                        await LoadDataAsync();

                        UpdateButtonStates();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi hiển thị màn hình thêm mới");
            }
        }

        /// <summary>
        /// TreeList selection thay đổi -> cập nhật danh sách Id đã chọn và trạng thái nút.
        /// Lưu ý: Selection khác với Checkbox - Selection dùng để chọn row, Checkbox dùng để chọn item
        /// </summary>
        private void TreeList1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                // Bỏ qua nếu đang xử lý checkbox để tránh conflict
                if (_isProcessingCheckbox) return;

                // SelectionChanged chỉ xử lý việc chọn row, không xử lý checkbox
                // Checkbox logic được xử lý riêng trong AfterCheckNode
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        /// <summary>
        /// Event handler trước khi checkbox của node thay đổi.
        /// Lưu ý: Chỉ xử lý checkbox, không can thiệp vào selection
        /// </summary>
        private void TreeList1_BeforeCheckNode(object sender, CheckNodeEventArgs e)
        {
            try
            {
                // Đánh dấu đang xử lý checkbox
                _isProcessingCheckbox = true;

                // Cho phép checkbox thay đổi
                e.CanCheck = true;

            }
            catch (Exception ex)
            {
                _isProcessingCheckbox = false;
                ShowError(ex);
            }
        }

        /// <summary>
        /// Event handler khi checkbox của node thay đổi.
        /// </summary>
        private void TreeList1_AfterCheckNode(object sender, NodeEventArgs e)
        {
            try
            {

                // Xử lý logic parent-child checkbox
                HandleParentChildCheckboxLogic(e.Node);

                // Reset flag
                _isProcessingCheckbox = false;

                // Cập nhật danh sách selected IDs khi checkbox thay đổi
                UpdateSelectedCategoryIds();
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                _isProcessingCheckbox = false;
                ShowError(ex);
            }
        }

        /// <summary>
        /// Event handler khi click chuột - detect click vào checkbox.
        /// Lưu ý: Không can thiệp vào selection, để DevExpress tự xử lý
        /// </summary>
        private void TreeList1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                // Lấy thông tin về vị trí click
                var hitInfo = treeList1.CalcHitInfo(e.Location);

                // Nếu click vào checkbox
                if (hitInfo.HitInfoType == DevExpress.XtraTreeList.HitInfoType.NodeCheckBox)
                {
                    // Không cần can thiệp gì thêm, để DevExpress tự xử lý checkbox và selection
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        /// <summary>
        /// Xử lý logic parent-child checkbox: 
        /// - Khi chọn node cha thì tự động chọn tất cả node con
        /// - Khi bỏ chọn node con thì tự động bỏ chọn node cha
        /// - Cập nhật trạng thái parent dựa trên trạng thái của tất cả children
        /// </summary>
        private void HandleParentChildCheckboxLogic(TreeListNode changedNode)
        {
            try
            {
                if (changedNode == null) return;

                bool isChecked = changedNode.Checked;

                // Tạm thời disable event để tránh recursive calls
                treeList1.AfterCheckNode -= TreeList1_AfterCheckNode;

                if (isChecked)
                {
                    // Khi chọn node cha -> chọn tất cả node con
                    CheckAllChildNodes(changedNode);
                }

                // Luôn cập nhật trạng thái parent nodes (cho cả trường hợp check và uncheck)
                UpdateParentNodeStates(changedNode);

                // Re-enable event
                treeList1.AfterCheckNode += TreeList1_AfterCheckNode;

            }
            catch (Exception ex)
            {
                // Re-enable event trong trường hợp lỗi
                treeList1.AfterCheckNode += TreeList1_AfterCheckNode;
                ShowError(ex);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái của tất cả parent nodes dựa trên trạng thái của children.
        /// Logic mới:
        /// - Nếu có node con bỏ chọn thì bỏ chọn chính nó, sau đó truy ngược lên để bỏ chọn cha của nó
        /// - Nếu tất cả các node con của nó có chọn thì chọn lại chính nó và truy ngược lên node cha nó
        /// </summary>
        private void UpdateParentNodeStates(TreeListNode changedNode)
        {
            try
            {
                var currentNode = changedNode.ParentNode;
                while (currentNode != null)
                {

                    // Kiểm tra trạng thái của tất cả children (chỉ direct children, không đệ quy)
                    bool allChildrenChecked = AreAllDirectChildrenChecked(currentNode);
                    bool hasAnyUncheckedChild = HasAnyDirectUncheckedChild(currentNode);


                    if (hasAnyUncheckedChild)
                    {
                        // Có ít nhất 1 node con bị bỏ chọn -> bỏ chọn parent
                        if (currentNode.Checked)
                        {
                            currentNode.Checked = false;
                        }
                    }
                    else if (allChildrenChecked)
                    {
                        // Tất cả node con đều được chọn -> chọn parent
                        if (!currentNode.Checked)
                        {
                            currentNode.Checked = true;
                        }
                    }

                    currentNode = currentNode.ParentNode;
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Kiểm tra xem tất cả direct children của node có được chọn không.
        /// </summary>
        private bool AreAllDirectChildrenChecked(TreeListNode parentNode)
        {
            try
            {
                if (parentNode == null || parentNode.Nodes.Count == 0) return true; // Không có con = true

                foreach (TreeListNode childNode in parentNode.Nodes)
                {
                    if (!childNode.Checked)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra xem có direct child nào không được chọn không.
        /// </summary>
        private bool HasAnyDirectUncheckedChild(TreeListNode parentNode)
        {
            try
            {
                if (parentNode == null || parentNode.Nodes.Count == 0) return false; // Không có con = false

                foreach (TreeListNode childNode in parentNode.Nodes)
                {
                    if (!childNode.Checked)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra xem node có node con nào không được chọn không (kiểm tra tất cả các level).
        /// </summary>
        private bool HasAnyUncheckedChild(TreeListNode parentNode)
        {
            try
            {
                if (parentNode == null || parentNode.Nodes.Count == 0) return false;

                // Kiểm tra các node con trực tiếp
                foreach (TreeListNode childNode in parentNode.Nodes)
                {
                    if (!childNode.Checked)
                    {
                        return true;
                    }

                    // Đệ quy kiểm tra các node con ở level sâu hơn
                    if (HasAnyUncheckedChild(childNode))
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Chọn tất cả node con của node cha.
        /// </summary>
        private void CheckAllChildNodes(TreeListNode parentNode)
        {
            try
            {
                if (parentNode == null || parentNode.Nodes.Count == 0) return;


                foreach (TreeListNode childNode in parentNode.Nodes)
                {
                    if (!childNode.Checked)
                    {
                        childNode.Checked = true;
                    }

                    // Đệ quy cho các node con sâu hơn
                    CheckAllChildNodes(childNode);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Bỏ chọn node cha nếu có node con bị bỏ chọn.
        /// (Phương thức này giờ được thay thế bởi UpdateParentNodeStates)
        /// </summary>
        private void UncheckParentNode(TreeListNode childNode)
        {
            // Logic này đã được chuyển vào UpdateParentNodeStates để xử lý toàn diện hơn
            UpdateParentNodeStates(childNode);
        }

        /// <summary>
        /// Kiểm tra xem node có node con nào được chọn không (kiểm tra tất cả các level).
        /// </summary>
        private bool HasAnyCheckedChild(TreeListNode parentNode)
        {
            try
            {
                if (parentNode == null || parentNode.Nodes.Count == 0) return false;

                // Kiểm tra các node con trực tiếp
                foreach (TreeListNode childNode in parentNode.Nodes)
                {
                    if (childNode.Checked)
                    {
                        return true;
                    }

                    // Đệ quy kiểm tra các node con ở level sâu hơn
                    if (HasAnyCheckedChild(childNode))
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Cập nhật danh sách selected category IDs dựa trên checkbox state.
        /// Lưu ý: Chỉ dựa vào checkbox, không dựa vào selection
        /// </summary>
        private void UpdateSelectedCategoryIds()
        {
            _selectedCategoryIds.Clear();


            // Chỉ lấy các nodes có checkbox được check (không dựa vào selection)
            foreach (TreeListNode node in treeList1.Nodes)
            {
                CheckNodeRecursive(node);
            }

        }

        /// <summary>
        /// Kiểm tra node và các child nodes một cách đệ quy.
        /// </summary>
        private void CheckNodeRecursive(TreeListNode node)
        {
            string nodeName = node.GetDisplayText("CategoryName");
            bool isChecked = node.Checked;


            // Chỉ thêm vào danh sách nếu checkbox được check (không dựa vào selection)
            if (isChecked)
            {
                // Lấy dữ liệu trực tiếp từ node thay vì dựa vào index
                var dto = GetDtoFromNode(node);
                if (dto != null)
                {
                    if (!_selectedCategoryIds.Contains(dto.Id))
                    {
                        _selectedCategoryIds.Add(dto.Id);
                    }
                }
                else
                {
                }
            }

            // Kiểm tra các child nodes
            foreach (TreeListNode childNode in node.Nodes)
            {
                CheckNodeRecursive(childNode);
            }
        }

        /// <summary>
        /// Lấy DTO từ TreeListNode một cách chính xác.
        /// </summary>
        private ProductServiceCategoryDto GetDtoFromNode(TreeListNode node)
        {
            try
            {
                // Cách 1: Lấy trực tiếp từ TreeList DataSource bằng cách sử dụng GetDataRecordByNode
                if (treeList1.DataSource != null)
                {
                    // Sử dụng GetDataRecordByNode để lấy dữ liệu trực tiếp từ node
                    var dataRecord = treeList1.GetDataRecordByNode(node);
                    if (dataRecord is ProductServiceCategoryDto dto)
                    {
                        return dto;
                    }
                }

                // Cách 2: Lấy từ BindingSource bằng cách tìm kiếm theo ID nếu có
                if (productServiceCategoryDtoBindingSource.DataSource is List<ProductServiceCategoryDto>
                    bindingDataSource)
                {
                    // Thử lấy ID từ node nếu có
                    var nodeId = node.GetValue("Id");
                    if (nodeId is Guid id)
                    {
                        var dto = bindingDataSource.FirstOrDefault(d => d.Id == id);
                        if (dto != null)
                        {
                            return dto;
                        }
                    }

                    // Fallback: Tìm theo tên và mô tả
                    var nodeName = node.GetDisplayText("CategoryName");
                    var nodeDescription = node.GetDisplayText("Description");

                    var dtoByName = bindingDataSource.FirstOrDefault(d =>
                        d.CategoryName == nodeName &&
                        d.Description == nodeDescription);

                    if (dtoByName != null)
                    {
                        return dtoByName;
                    }
                }

                // Cách 3: Thử lấy từ node.Tag nếu có
                if (node.Tag is ProductServiceCategoryDto tagDto)
                {
                    return tagDto;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Vẽ STT dòng cho TreeList.
        /// </summary>
        private void TreeList1_CustomDrawNodeIndicator(object sender, CustomDrawNodeIndicatorEventArgs e)
        {
            // Vẽ số thứ tự dòng cho TreeList
            if (e.Node != null)
            {
                var index = treeList1.GetVisibleIndexByNode(e.Node);
                if (index >= 0)
                {
                    // Vẽ số thứ tự vào indicator
                    e.Cache.DrawString((index + 1).ToString(), e.Appearance.Font,
                        e.Appearance.GetForeBrush(e.Cache), e.Bounds,
                        System.Drawing.StringFormat.GenericDefault);
                    e.Handled = true;
                }
            }
        }

        private void UpdateButtonStates()
        {
            try
            {
                var selectedCount = _selectedCategoryIds?.Count ?? 0;
                var rowCount = treeList1.VisibleNodesCount;


                // Edit: chỉ khi chọn đúng 1 dòng
                if (EditBarButtonItem != null)
                {
                    EditBarButtonItem.Enabled = selectedCount == 1;
                }

                // Delete: khi chọn >= 1 dòng
                if (DeleteBarButtonItem != null)
                {
                    DeleteBarButtonItem.Enabled = selectedCount >= 1;
                }

                // Export: chỉ khi có dữ liệu hiển thị
                if (ExportBarButtonItem != null)
                {
                    ExportBarButtonItem.Enabled = rowCount > 0;
                }

            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Cấu hình TreeList để hiển thị dữ liệu xuống dòng (word wrap) cho các cột văn bản dài.
        /// Đảm bảo hiển thị tất cả categories bao gồm cả những category chưa có sản phẩm.
        /// </summary>
        private void ConfigureMultiLineGridView()
        {
            try
            {
                // Cấu hình TreeList để hiển thị hierarchical
                treeList1.OptionsView.ShowButtons = true;
                treeList1.OptionsView.ShowRoot = true;
                treeList1.OptionsView.ShowHorzLines = true; // Hiển thị đường kẻ ngang
                treeList1.OptionsView.ShowVertLines = true; // Hiển thị đường kẻ dọc
                treeList1.OptionsView.EnableAppearanceEvenRow = true; // Bật màu xen kẽ cho dòng chẵn
                treeList1.OptionsView.EnableAppearanceOddRow = true; // Bật màu xen kẽ cho dòng lẻ

                // Cấu hình không cho edit
                treeList1.OptionsBehavior.Editable = false;
                treeList1.OptionsSelection.EnableAppearanceFocusedCell = false;

                // Cấu hình selection behavior theo tài liệu DevExpress
                treeList1.OptionsSelection.MultiSelect = true;
                treeList1.OptionsSelection.MultiSelectMode = DevExpress.XtraTreeList.TreeListMultiSelectMode.RowSelect;

                // Cấu hình checkbox behavior
                treeList1.OptionsBehavior.AllowIndeterminateCheckState = false;

                // Cho phép selection và checkbox hoạt động độc lập
                treeList1.OptionsSelection.UseIndicatorForSelection = true;
                treeList1.OptionsSelection.EnableAppearanceFocusedRow = true;

                // Đảm bảo hiển thị tất cả dữ liệu (không ẩn dòng nào)
                treeList1.OptionsView.ShowAutoFilterRow = false; // Tắt auto filter row
                treeList1.OptionsFilter.AllowFilterEditor = false; // Tắt filter editor

                // Đảm bảo không có filter nào được áp dụng
                treeList1.ActiveFilterString = "";
                treeList1.ActiveFilterEnabled = false;

                // RepositoryItemMemoEdit cho wrap text
                var memo = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit
                {
                    WordWrap = true,
                    AutoHeight = false,
                    ReadOnly = true // Không cho edit
                };
                memo.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;

                // Áp dụng cho các cột có khả năng dài
                ApplyMemoEditorToColumn("CategoryName", memo);
                ApplyMemoEditorToColumn("Description", memo);

                // Tùy chọn hiển thị: căn giữa tiêu đề cho đẹp
                treeList1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                treeList1.Appearance.HeaderPanel.Options.UseTextOptions = true;

                // Cấu hình màu sắc cho dòng chẵn/lẻ
                treeList1.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(248, 248, 248); // Màu xám nhạt
                treeList1.Appearance.OddRow.BackColor = System.Drawing.Color.White; // Màu trắng

                // Cấu hình màu đường kẻ
                treeList1.Appearance.HorzLine.BackColor = System.Drawing.Color.FromArgb(200, 200, 200); // Màu xám nhạt
                treeList1.Appearance.VertLine.BackColor = System.Drawing.Color.FromArgb(200, 200, 200); // Màu xám nhạt

                // Cấu hình màu header
                treeList1.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
                treeList1.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
                treeList1.Appearance.HeaderPanel.Font =
                    new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void ApplyMemoEditorToColumn(string fieldName,
            DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit memo)
        {
            var col = treeList1.Columns[fieldName];
            if (col == null) return;
            // Thêm repository vào TreeList nếu chưa có
            if (!treeList1.RepositoryItems.Contains(memo))
            {
                treeList1.RepositoryItems.Add(memo);
            }

            col.ColumnEdit = memo;
        }

        /// <summary>
        /// Tô màu/định dạng cell theo số lượng sản phẩm/dịch vụ.
        /// </summary>
        private void TreeList1_CustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs e)
        {
            try
            {
                var treeList = sender as TreeList;
                if (treeList == null) return;
                if (e.Node == null) return;

                // Lấy index từ node
                var index = treeList.GetVisibleIndexByNode(e.Node);
                if (index < 0 || index >= productServiceCategoryDtoBindingSource.Count) return;

                var row = productServiceCategoryDtoBindingSource[index] as ProductServiceCategoryDto;
                if (row == null) return;

                // Không ghi đè màu khi đang chọn để giữ màu chọn mặc định của DevExpress
                if (treeList.Selection.Contains(e.Node)) return;

                // Chỉ tô màu cho cột ProductCount để hiển thị trạng thái
                if (e.Column.FieldName == "ProductCount")
                {
                    System.Drawing.Color backColor;
                    System.Drawing.Color foreColor = System.Drawing.Color.Black;

                    // Màu sắc dựa trên số lượng sản phẩm/dịch vụ
                    if (row.ProductCount == 0)
                    {
                        backColor = System.Drawing.Color.FromArgb(255, 240, 240); // Màu đỏ nhạt
                        foreColor = System.Drawing.Color.FromArgb(150, 0, 0); // Màu đỏ đậm
                    }
                    else if (row.ProductCount <= 5)
                    {
                        backColor = System.Drawing.Color.FromArgb(255, 255, 200); // Màu vàng nhạt
                        foreColor = System.Drawing.Color.FromArgb(150, 100, 0); // Màu cam đậm
                    }
                    else if (row.ProductCount <= 20)
                    {
                        backColor = System.Drawing.Color.FromArgb(240, 255, 240); // Màu xanh lá nhạt
                        foreColor = System.Drawing.Color.FromArgb(0, 100, 0); // Màu xanh lá đậm
                    }
                    else
                    {
                        backColor = System.Drawing.Color.FromArgb(240, 248, 255); // Màu xanh dương nhạt
                        foreColor = System.Drawing.Color.FromArgb(0, 0, 150); // Màu xanh dương đậm
                    }

                    e.Appearance.BackColor = backColor;
                    e.Appearance.ForeColor = foreColor;
                    e.Appearance.Options.UseBackColor = true;
                    e.Appearance.Options.UseForeColor = true;
                }
                else
                {
                    // Cho các cột khác, sử dụng màu mặc định (trắng/xám xen kẽ)
                    // Không cần set màu gì, để DevExpress tự động xử lý
                }
            }
            catch (Exception)
            {
                // ignore style errors
            }
        }

        /// <summary>
        /// Người dùng bấm "Điều chỉnh".
        /// </summary>
        private async void EditBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Chỉ cho phép chỉnh sửa 1 dòng dữ liệu
            if (_selectedCategoryIds == null || _selectedCategoryIds.Count == 0)
            {
                ShowInfo("Vui lòng chọn một dòng để chỉnh sửa.");
                return;
            }

            if (_selectedCategoryIds.Count > 1)
            {
                ShowInfo("Chỉ cho phép chỉnh sửa 1 dòng. Vui lòng bỏ chọn bớt.");
                return;
            }

            var id = _selectedCategoryIds[0];
            var focusedNode = treeList1.FocusedNode;
            ProductServiceCategoryDto dto = null;

            if (focusedNode != null)
            {
                // Lấy dữ liệu từ focused node
                dto = productServiceCategoryDtoBindingSource.Current as ProductServiceCategoryDto;
            }

            if (dto == null || dto.Id != id)
            {
                // Tìm đúng DTO theo Id trong datasource nếu FocusedRow không khớp selection
                if (productServiceCategoryDtoBindingSource.DataSource is System.Collections.IEnumerable list)
                {
                    foreach (var item in list)
                    {
                        if (item is ProductServiceCategoryDto x && x.Id == id)
                        {
                            dto = x;
                            break;
                        }
                    }
                }
            }

            if (dto == null)
            {
                ShowInfo("Không thể xác định dòng được chọn để chỉnh sửa.");
                return;
            }

            try
            {
                using (OverlayManager.ShowScope(this))
                {
                    using (var form = new FrmProductServiceCategoryDetail(dto.Id))
                    {
                        form.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                        form.ShowDialog(this);

                        await LoadDataAsync();
                        UpdateButtonStates();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi hiển thị màn hình điều chỉnh");
            }
        }

        /// <summary>
        /// Người dùng bấm "Xóa".
        /// </summary>
        private async void DeleteBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_selectedCategoryIds == null || _selectedCategoryIds.Count == 0)
            {
                ShowInfo("Vui lòng chọn ít nhất một dòng để xóa.");
                return;
            }


            var confirmMessage = _selectedCategoryIds.Count == 1
                ? "Bạn có chắc muốn xóa dòng dữ liệu đã chọn? (Sản phẩm/dịch vụ sẽ được chuyển sang 'Phân loại chưa đặt tên')"
                : $"Bạn có chắc muốn xóa {_selectedCategoryIds.Count} dòng dữ liệu đã chọn? (Sản phẩm/dịch vụ sẽ được chuyển sang 'Phân loại chưa đặt tên')";

            if (!MsgBox.GetConfirmFromYesNoDialog(confirmMessage)) return;

            try
            {
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    // Xóa theo thứ tự: con trước, cha sau để tránh lỗi foreign key constraint
                    await _productServiceCategoryBll.DeleteCategoriesWithProductMigration(_selectedCategoryIds.ToList());
                });

                // Clear selection state trước khi reload
                ClearSelectionState();

                // Reload dữ liệu
                await LoadDataAsync();
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xóa dữ liệu");
            }
        }

        /// <summary>
        /// Người dùng bấm "Xuất".
        /// </summary>
        private void ExportBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Chỉ cho phép xuất khi có dữ liệu hiển thị
            var rowCount = treeList1.VisibleNodesCount;
            if (rowCount <= 0)
            {
                ShowInfo("Không có dữ liệu để xuất.");
                return;
            }

            // Export TreeList data
            try
            {
                var saveDialog = new System.Windows.Forms.SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*",
                    FileName = "ProductServiceCategories.xlsx"
                };

                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    treeList1.ExportToXlsx(saveDialog.FileName);
                    ShowInfo("Xuất dữ liệu thành công!");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xuất dữ liệu");
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Tải dữ liệu và bind vào Grid (Async, hiển thị WaitForm).
        /// </summary>
        private async Task LoadDataAsync()
        {
            if (_isLoading) return; // tránh re-entrancy
            _isLoading = true;
            try
            {
                await ExecuteWithWaitingFormAsync(async () => { await LoadDataAsyncWithoutSplash(); });
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

        /// <summary>
        /// Tải dữ liệu và bind vào Grid (Async, không hiển thị WaitForm).
        /// Hiển thị tất cả categories bao gồm cả những category chưa có sản phẩm.
        /// </summary>
        private async Task LoadDataAsyncWithoutSplash()
        {
            try
            {
                var (categories, counts) = await _productServiceCategoryBll.GetCategoriesWithCountsAsync();




                // Tạo cấu trúc cây hierarchical - hiển thị TẤT CẢ categories
                var dtoList = categories.ToDtosWithHierarchy(counts).ToList();



                BindGrid(dtoList);
                // UpdateButtonStates() sẽ được gọi trong BindGrid -> ClearSelectionState()
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu");
            }
        }

        /// <summary>
        /// Bind danh sách DTO vào Grid và cấu hình hiển thị.
        /// Hiển thị tất cả categories bao gồm cả những category chưa có sản phẩm.
        /// </summary>
        private void BindGrid(List<ProductServiceCategoryDto> data)
        {
            // Clear selection trước khi bind data mới
            ClearSelectionState();


            // Đảm bảo không có filter nào được áp dụng
            treeList1.ActiveFilterString = "";
            treeList1.ActiveFilterEnabled = false;

            // Bind dữ liệu vào BindingSource - đảm bảo tất cả dữ liệu được bind
            productServiceCategoryDtoBindingSource.DataSource = data;

            // Đảm bảo TreeList được bind đúng cách
            if (treeList1.DataSource != productServiceCategoryDtoBindingSource)
            {
                treeList1.DataSource = productServiceCategoryDtoBindingSource;
            }

            // Cấu hình hiển thị
            treeList1.BestFitColumns();
            ConfigureMultiLineGridView();

            // Đảm bảo tất cả nodes được hiển thị (expand all)
            treeList1.ExpandAll();



            // Đảm bảo selection được clear sau khi bind và update button states
            ClearSelectionState();
        }


        /// <summary>
        /// Xóa trạng thái chọn hiện tại trên TreeList.
        /// </summary>
        private void ClearSelectionState()
        {

            _selectedCategoryIds.Clear();

            // Clear tất cả selection (cả checkbox và regular selection)
            treeList1.ClearSelection();
            treeList1.FocusedNode = null;

            // Clear tất cả checkbox states
            ClearAllCheckBoxes();


            UpdateButtonStates();
        }

        /// <summary>
        /// Clear tất cả checkbox states trong TreeList.
        /// </summary>
        private void ClearAllCheckBoxes()
        {
            try
            {
                // Disable events tạm thời để tránh trigger UpdateSelectedCategoryIds
                treeList1.AfterCheckNode -= TreeList1_AfterCheckNode;

                // Clear tất cả checkbox states
                foreach (TreeListNode node in treeList1.Nodes)
                {
                    ClearNodeCheckBoxes(node);
                }

                // Re-enable events
                treeList1.AfterCheckNode += TreeList1_AfterCheckNode;
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Clear checkbox của node và tất cả child nodes.
        /// </summary>
        private void ClearNodeCheckBoxes(TreeListNode node)
        {
            if (node != null)
            {
                node.Checked = false;

                // Clear child nodes recursively
                foreach (TreeListNode childNode in node.Nodes)
                {
                    ClearNodeCheckBoxes(childNode);
                }
            }
        }

        /// <summary>
        /// Hiển thị thông tin.
        /// </summary>
        private void ShowInfo(string message)
        {
            MsgBox.ShowInfo(message);
        }

        /// <summary>
        /// Hiển thị lỗi với thông tin ngữ cảnh.
        /// </summary>
        private void ShowError(Exception ex, string context = null)
        {
            if (string.IsNullOrWhiteSpace(context))
                MsgBox.ShowException(ex);
            else
                MsgBox.ShowException(new Exception(context + ": " + ex.Message, ex));
        }

        #endregion

    }
}