using Bll.MasterData.CompanyBll;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DTO.MasterData.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Common;
using Common.Utils;

namespace MasterData.Company
{
    /// <summary>
    /// User Control quản lý danh sách phòng ban theo cấu trúc cây phân cấp.
    /// Cung cấp giao diện hiển thị dạng TreeList, tìm kiếm, thêm mới, sửa, xóa và xuất dữ liệu phòng ban.
    /// </summary>
    public partial class FrmDepartment : XtraForm
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// Danh sách phòng ban hiện tại trong TreeList
        /// </summary>
        private List<DepartmentDto> _departments;

        /// <summary>
        /// Danh sách ID các phòng ban được đánh dấu (bookmark) - hiện tại chưa sử dụng
        /// </summary>
        private readonly HashSet<Guid> _bookmarks = [];

        /// <summary>
        /// Business Logic Layer xử lý nghiệp vụ phòng ban
        /// </summary>
        private readonly DepartmentBll _departmentBll = new();

        /// <summary>
        /// Danh sách ID của các phòng ban được chọn (qua checkbox)
        /// </summary>
        private readonly List<Guid> _selectedDepartmentIds = [];

        /// <summary>
        /// Cờ ngăn chặn xử lý selection khi đang thao tác với checkbox để tránh conflict
        /// </summary>
        private bool _isProcessingCheckbox;

        #endregion

        #region ========== CONSTRUCTOR ==========

        /// <summary>
        /// Khởi tạo User Control quản lý danh sách phòng ban.
        /// </summary>
        public FrmDepartment()
        {
            InitializeComponent();
            InitializeTreeList();
            SetupToolbarEvents();
            UpdateButtonStates();
            SetupSuperTips();
        }

        #endregion

        #region ========== FORM INITIALIZATION ==========

        /// <summary>
        /// Khởi tạo và cấu hình TreeList để hiển thị danh sách phòng ban dạng cây phân cấp.
        /// </summary>
        private void InitializeTreeList()
        {
            try
            {
                // Cấu hình TreeList cơ bản
                DepartmentTreeList.OptionsBehavior.Editable = false;
                DepartmentTreeList.OptionsFind.AlwaysVisible = true;
                DepartmentTreeList.OptionsSelection.MultiSelect = true;
                DepartmentTreeList.OptionsView.ShowIndentAsRowStyle = true;

                // Cấu hình hierarchy fields
                DepartmentTreeList.KeyFieldName = "Id";
                DepartmentTreeList.ParentFieldName = "ParentId";

                // Đăng ký event handlers
                DepartmentTreeList.CustomDrawNodeIndicator += OnCustomDrawRowIndicator;
                DepartmentTreeList.SelectionChanged += DepartmentTreeList_SelectionChanged;
                DepartmentTreeList.BeforeCheckNode += DepartmentTreeList_BeforeCheckNode;
                DepartmentTreeList.AfterCheckNode += DepartmentTreeList_AfterCheckNode;
            }
            catch (Exception ex)
            {
                HandleException(ex, "Lỗi khởi tạo TreeList");
            }
        }

        /// <summary>
        /// Thiết lập sự kiện cho các nút trên thanh công cụ.
        /// </summary>
        private void SetupToolbarEvents()
        {
            ListDataBarButtonItem.ItemClick += ListDataBarButtonItem_ItemClick;
            NewBarButtonItem.ItemClick += NewBarButtonItem_ItemClick;
            EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
            DeleteBarButtonItem.ItemClick += DeleteBarButtonItem_ItemClick;
            ExportBarButtonItem.ItemClick += ExportBarButtonItem_ItemClick;
        }

        #endregion

        #region ========== DATA LOADING ==========

        /// <summary>
        /// Tải dữ liệu phòng ban từ database và hiển thị lên TreeList với WaitForm.
        /// </summary>
        private async Task LoadDataAsync()
        {
            try
            {
                await ExecuteWithWaitingFormAsync(async () => { await LoadDataAsyncWithoutSplash(); });
            }
            catch (Exception ex)
            {
                HandleException(ex, "Lỗi tải dữ liệu");
            }
        }

        /// <summary>
        /// Tải dữ liệu phòng ban từ database và bind vào TreeList (không hiển thị WaitForm).
        /// </summary>
        private Task LoadDataAsyncWithoutSplash()
        {
            try
            {
                // Lấy entities từ BLL (synchronous method)
                var departmentEntities = _departmentBll.GetAll();

                // Convert entities to DTOs sử dụng DepartmentConverters
                _departments = [.. departmentEntities.Select(e => e.ToDto())];

                // Bind dữ liệu vào TreeList
                departmentDtoBindingSource.DataSource = _departments;
                DepartmentTreeList.ExpandAll();

                // Cấu hình hiển thị multi-line cho các cột dài
                ConfigureMultiLineDisplay();

                // Xóa selection và checkbox trước khi cập nhật button states
                ClearSelectionState();

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                HandleException(ex, "Lỗi tải dữ liệu");
                return Task.CompletedTask;
            }
        }

        #endregion

        #region ========== EVENT HANDLERS ==========

        /// <summary>
        /// Xử lý sự kiện click nút Tải dữ liệu trên thanh công cụ.
        /// </summary>
        private async void ListDataBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                HandleException(ex, "Lỗi tải dữ liệu");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút Thêm mới trên thanh công cụ.
        /// </summary>
        private async void NewBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                using (OverlayManager.ShowScope(this))
                {
                    using (var form = new FrmDepartmentDetail(Guid.Empty))
                    {
                        form.StartPosition = FormStartPosition.CenterParent;
                        if (form.ShowDialog(this) == DialogResult.OK)
                        {
                            await LoadDataAsyncWithoutSplash();
                            UpdateButtonStates();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "Lỗi hiển thị màn hình thêm mới");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút Điều chỉnh trên thanh công cụ.
        /// </summary>
        private async void EditBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Validate: Phải chọn đúng 1 phòng ban
                if (!ValidateSingleSelection())
                {
                    return;
                }

                var departmentId = _selectedDepartmentIds[0];
                using (OverlayManager.ShowScope(this))
                {
                    using (var form = new FrmDepartmentDetail(departmentId))
                    {
                        form.StartPosition = FormStartPosition.CenterParent;
                        if (form.ShowDialog(this) == DialogResult.OK)
                        {
                            await LoadDataAsyncWithoutSplash();
                            UpdateButtonStates();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "Lỗi hiển thị màn hình điều chỉnh");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút Xóa trên thanh công cụ.
        /// </summary>
        private async void DeleteBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Validate: Phải chọn ít nhất 1 phòng ban
                if (!ValidateMultipleSelection())
                {
                    return;
                }

                // Hiển thị dialog xác nhận
                var confirmMessage = _selectedDepartmentIds.Count == 1
                    ? "Bạn có chắc muốn xóa dòng dữ liệu đã chọn?"
                    : $"Bạn có chắc muốn xóa {_selectedDepartmentIds.Count} dòng dữ liệu đã chọn?";

                if (!MsgBox.ShowYesNo(confirmMessage, "Xác nhận xóa"))
                {
                    return;
                }

                // Thực hiện xóa với WaitForm
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    var success = await Task.Run(() => _departmentBll.DeleteMultiple(_selectedDepartmentIds.ToList()));
                    if (success)
                    {
                        ShowInfo($"Đã xóa {_selectedDepartmentIds.Count} phòng ban");
                    }
                    else
                    {
                        ShowError("Không thể xóa phòng ban. Vui lòng thử lại.");
                    }
                });

                // Xóa selection và reload dữ liệu
                ClearSelectionState();
                await LoadDataAsyncWithoutSplash();
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                HandleException(ex, "Lỗi xóa dữ liệu");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút Xuất dữ liệu trên thanh công cụ.
        /// </summary>
        private void ExportBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Validate: Phải có dữ liệu để xuất
                var rowCount = DepartmentTreeList.VisibleNodesCount;
                if (rowCount <= 0)
                {
                    ShowInfo("Không có dữ liệu để xuất.");
                    return;
                }

                // Hiển thị SaveFileDialog
                using (var saveDialog = new SaveFileDialog
                {
                    Filter = @"Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*",
                    FileName = "Departments.xlsx"
                })
                {
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        DepartmentTreeList.ExportToXlsx(saveDialog.FileName);
                        ShowInfo("Xuất dữ liệu thành công!");
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "Lỗi xuất dữ liệu");
            }
        }

        /// <summary>
        /// Xử lý sự kiện thay đổi selection trên TreeList.
        /// </summary>
        private void DepartmentTreeList_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                // Bỏ qua nếu đang xử lý checkbox để tránh conflict
                if (_isProcessingCheckbox)
                {
                    return;
                }

                // SelectionChanged chỉ xử lý việc chọn row, không xử lý checkbox
                // Checkbox logic được xử lý riêng trong AfterCheckNode
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Xử lý sự kiện trước khi checkbox của node thay đổi.
        /// </summary>
        private void DepartmentTreeList_BeforeCheckNode(object sender, CheckNodeEventArgs e)
        {
            try
            {
                // Đánh dấu đang xử lý checkbox để tránh conflict với SelectionChanged
                _isProcessingCheckbox = true;

                // Cho phép checkbox thay đổi
                e.CanCheck = true;
            }
            catch (Exception ex)
            {
                _isProcessingCheckbox = false;
                HandleException(ex);
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi checkbox của node thay đổi.
        /// </summary>
        private void DepartmentTreeList_AfterCheckNode(object sender, NodeEventArgs e)
        {
            try
            {
                // Xử lý logic parent-child checkbox
                HandleParentChildCheckboxLogic(e.Node);

                // Reset flag
                _isProcessingCheckbox = false;

                // Cập nhật danh sách selected IDs và button states
                UpdateSelectedDepartmentIds();
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                _isProcessingCheckbox = false;
                HandleException(ex);
            }
        }

        /// <summary>
        /// Xử lý sự kiện vẽ row indicator (số thứ tự hoặc bookmark icon).
        /// </summary>
        private void OnCustomDrawRowIndicator(object sender, CustomDrawNodeIndicatorEventArgs e)
        {
            try
            {
                if (e.Node == null)
                {
                    return;
                }

                if (DepartmentTreeList.GetRow(e.Node.Id) is not DepartmentDto department)
                {
                    return;
                }

                // Hiển thị bookmark nếu có
                if (_bookmarks.Contains(department.Id))
                {
                    e.DefaultDraw();
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                // Ignore drawing errors để không làm gián đoạn hiển thị
                System.Diagnostics.Debug.WriteLine($"Error in OnCustomDrawRowIndicator: {ex.Message}");
            }
        }

        #endregion

        #region ========== BUSINESS LOGIC - TREELIST OPERATIONS ==========

        /// <summary>
        /// Xử lý logic parent-child checkbox:
        /// - Khi chọn node cha thì tự động chọn tất cả node con
        /// - Khi bỏ chọn node con thì tự động bỏ chọn node cha
        /// - Cập nhật trạng thái parent dựa trên trạng thái của tất cả children
        /// </summary>
        /// <param name="changedNode">Node có checkbox vừa thay đổi</param>
        private void HandleParentChildCheckboxLogic(TreeListNode changedNode)
        {
            try
            {
                if (changedNode == null)
                {
                    return;
                }

                bool isChecked = changedNode.Checked;

                // Tạm thời disable event để tránh recursive calls
                DepartmentTreeList.AfterCheckNode -= DepartmentTreeList_AfterCheckNode;

                if (isChecked)
                {
                    // Khi chọn node cha -> chọn tất cả node con
                    CheckAllChildNodes(changedNode);
                }

                // Luôn cập nhật trạng thái parent nodes (cho cả trường hợp check và uncheck)
                UpdateParentNodeStates(changedNode);

                // Re-enable event
                DepartmentTreeList.AfterCheckNode += DepartmentTreeList_AfterCheckNode;
            }
            catch (Exception ex)
            {
                // Re-enable event trong trường hợp lỗi
                DepartmentTreeList.AfterCheckNode += DepartmentTreeList_AfterCheckNode;
                HandleException(ex);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái của tất cả parent nodes dựa trên trạng thái của children.
        /// </summary>
        /// <param name="changedNode">Node có checkbox vừa thay đổi</param>
        private void UpdateParentNodeStates(TreeListNode changedNode)
        {
            try
            {
                var currentNode = changedNode.ParentNode;
                while (currentNode != null)
                {
                    // Kiểm tra trạng thái của tất cả direct children
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
                // Ignore để không làm gián đoạn quá trình cập nhật
                System.Diagnostics.Debug.WriteLine($"Error in UpdateParentNodeStates: {ex.Message}");
            }
        }

        /// <summary>
        /// Kiểm tra xem tất cả direct children của node có được chọn không.
        /// </summary>
        /// <param name="parentNode">Node cha cần kiểm tra</param>
        /// <returns>True nếu tất cả children đều được chọn, False nếu có ít nhất 1 child chưa được chọn</returns>
        private bool AreAllDirectChildrenChecked(TreeListNode parentNode)
        {
            try
            {
                if (parentNode == null || parentNode.Nodes.Count == 0)
                {
                    return true; // Không có con = true
                }

                foreach (TreeListNode childNode in parentNode.Nodes)
                {
                    if (!childNode.Checked)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra xem có direct child nào không được chọn không.
        /// </summary>
        /// <param name="parentNode">Node cha cần kiểm tra</param>
        /// <returns>True nếu có ít nhất 1 child chưa được chọn, False nếu tất cả children đều được chọn</returns>
        private bool HasAnyDirectUncheckedChild(TreeListNode parentNode)
        {
            try
            {
                if (parentNode == null || parentNode.Nodes.Count == 0)
                {
                    return false; // Không có con = false
                }

                foreach (TreeListNode childNode in parentNode.Nodes)
                {
                    if (!childNode.Checked)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Chọn tất cả node con của node cha một cách đệ quy.
        /// </summary>
        /// <param name="parentNode">Node cha cần chọn tất cả children</param>
        private void CheckAllChildNodes(TreeListNode parentNode)
        {
            try
            {
                if (parentNode == null || parentNode.Nodes.Count == 0)
                {
                    return;
                }

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
                // Ignore để không làm gián đoạn quá trình chọn
                System.Diagnostics.Debug.WriteLine($"Error in CheckAllChildNodes: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật danh sách selected department IDs dựa trên checkbox state.
        /// </summary>
        private void UpdateSelectedDepartmentIds()
        {
            try
            {
                _selectedDepartmentIds.Clear();

                // Chỉ lấy các nodes có checkbox được check (không dựa vào selection)
                foreach (TreeListNode node in DepartmentTreeList.Nodes)
                {
                    CheckNodeRecursive(node);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "Lỗi cập nhật danh sách phòng ban đã chọn");
            }
        }

        /// <summary>
        /// Kiểm tra node và các child nodes một cách đệ quy để cập nhật selected IDs.
        /// </summary>
        /// <param name="node">Node cần kiểm tra</param>
        private void CheckNodeRecursive(TreeListNode node)
        {
            try
            {
                bool isChecked = node.Checked;

                // Chỉ thêm vào danh sách nếu checkbox được check
                if (isChecked)
                {
                    var department = GetDepartmentFromNode(node);
                    if (department != null && !_selectedDepartmentIds.Contains(department.Id))
                    {
                        _selectedDepartmentIds.Add(department.Id);
                    }
                }

                // Kiểm tra các child nodes
                foreach (TreeListNode childNode in node.Nodes)
                {
                    CheckNodeRecursive(childNode);
                }
            }
            catch (Exception ex)
            {
                // Ignore để không làm gián đoạn quá trình cập nhật
                System.Diagnostics.Debug.WriteLine($"Error in CheckNodeRecursive: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy DepartmentDto từ TreeListNode một cách chính xác.
        /// </summary>
        /// <param name="node">TreeListNode cần lấy dữ liệu</param>
        /// <returns>DepartmentDto tương ứng hoặc null nếu không tìm thấy</returns>
        private DepartmentDto GetDepartmentFromNode(TreeListNode node)
        {
            try
            {
                // Cách 1: Lấy trực tiếp từ TreeList DataSource
                if (DepartmentTreeList.DataSource != null)
                {
                    var dataRecord = DepartmentTreeList.GetDataRecordByNode(node);
                    if (dataRecord is DepartmentDto department)
                    {
                        return department;
                    }
                }

                // Cách 2: Lấy từ BindingSource bằng cách tìm kiếm theo ID
                if (departmentDtoBindingSource.DataSource is List<DepartmentDto> bindingDataSource)
                {
                    var nodeId = node.GetValue("Id");
                    if (nodeId is Guid id)
                    {
                        var department = bindingDataSource.FirstOrDefault(d => d.Id == id);
                        if (department != null)
                        {
                            return department;
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetDepartmentFromNode: {ex.Message}");
                return null;
            }
        }

        #endregion

        #region ========== HELPER METHODS ==========

        /// <summary>
        /// Cấu hình TreeList để hiển thị multi-line cho các cột có nội dung dài.
        /// </summary>
        private void ConfigureMultiLineDisplay()
        {
            try
            {
                // Cấu hình TreeList để hiển thị dạng cây
                DepartmentTreeList.OptionsView.ShowButtons = true;
                DepartmentTreeList.OptionsView.ShowRoot = true;

                // RepositoryItemMemoEdit cho wrap text
                var memo = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit
                {
                    WordWrap = true,
                    AutoHeight = true
                };
                memo.Appearance.TextOptions.WordWrap = WordWrap.Wrap;

                // Áp dụng cho các cột có khả năng dài
                ApplyMemoEditorToColumn("DepartmentName", memo);
                ApplyMemoEditorToColumn("Description", memo);

                // Căn giữa tiêu đề cho đẹp
                DepartmentTreeList.Appearance.HeaderPanel.TextOptions.HAlignment = HorzAlignment.Center;
                DepartmentTreeList.Appearance.HeaderPanel.Options.UseTextOptions = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in ConfigureMultiLineDisplay: {ex.Message}");
            }
        }

        /// <summary>
        /// Áp dụng RepositoryItemMemoEdit cho cột cụ thể để hỗ trợ wrap text.
        /// </summary>
        /// <param name="fieldName">Tên field của cột</param>
        /// <param name="memo">RepositoryItemMemoEdit đã cấu hình</param>
        private void ApplyMemoEditorToColumn(string fieldName, DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit memo)
        {
            try
            {
                var col = DepartmentTreeList.Columns[fieldName];
                if (col == null)
                {
                    return;
                }

                // Thêm repository vào TreeList nếu chưa có
                if (!DepartmentTreeList.RepositoryItems.Contains(memo))
                {
                    DepartmentTreeList.RepositoryItems.Add(memo);
                }

                col.ColumnEdit = memo;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in ApplyMemoEditorToColumn: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa trạng thái chọn hiện tại trên TreeList (cả selection và checkbox).
        /// </summary>
        private void ClearSelectionState()
        {
            try
            {
                _selectedDepartmentIds.Clear();

                // Clear tất cả selection (cả checkbox và regular selection)
                DepartmentTreeList.ClearSelection();
                DepartmentTreeList.FocusedNode = null;

                // Clear tất cả checkbox states
                ClearAllCheckBoxes();

                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                HandleException(ex, "Lỗi xóa trạng thái chọn");
            }
        }

        /// <summary>
        /// Clear tất cả checkbox states trong TreeList.
        /// </summary>
        private void ClearAllCheckBoxes()
        {
            try
            {
                // Disable events tạm thời để tránh trigger UpdateSelectedDepartmentIds
                DepartmentTreeList.AfterCheckNode -= DepartmentTreeList_AfterCheckNode;

                // Clear tất cả checkbox states
                foreach (TreeListNode node in DepartmentTreeList.Nodes)
                {
                    ClearNodeCheckBoxes(node);
                }

                // Re-enable events
                DepartmentTreeList.AfterCheckNode += DepartmentTreeList_AfterCheckNode;
            }
            catch (Exception ex)
            {
                // Re-enable events trong trường hợp lỗi
                DepartmentTreeList.AfterCheckNode += DepartmentTreeList_AfterCheckNode;
                System.Diagnostics.Debug.WriteLine($"Error in ClearAllCheckBoxes: {ex.Message}");
            }
        }

        /// <summary>
        /// Clear checkbox của node và tất cả child nodes một cách đệ quy.
        /// </summary>
        /// <param name="node">Node cần clear checkbox</param>
        private void ClearNodeCheckBoxes(TreeListNode node)
        {
            try
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in ClearNodeCheckBoxes: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật trạng thái các nút toolbar dựa trên selection và checkbox.
        /// </summary>
        private void UpdateButtonStates()
        {
            try
            {
                var selectedCount = _selectedDepartmentIds?.Count ?? 0;
                var rowCount = DepartmentTreeList.VisibleNodesCount;

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
                // Ignore để không làm gián đoạn UI
                System.Diagnostics.Debug.WriteLine($"Error in UpdateButtonStates: {ex.Message}");
            }
        }

        /// <summary>
        /// Thực hiện operation async với WaitingForm hiển thị.
        /// </summary>
        /// <param name="operation">Operation async cần thực hiện</param>
        private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
        {
            try
            {
                // Hiển thị WaitForm1
                SplashScreenManager.ShowForm(typeof(WaitForm1));

                // Thực hiện operation
                await operation();
            }
            finally
            {
                // Đóng WaitForm1
                if (SplashScreenManager.Default != null && SplashScreenManager.Default.IsSplashFormVisible)
                {
                    SplashScreenManager.CloseForm();
                }
            }
        }

        /// <summary>
        /// Validate selection: Phải chọn đúng 1 phòng ban.
        /// </summary>
        /// <returns>True nếu hợp lệ, False nếu không hợp lệ (đã hiển thị thông báo)</returns>
        private bool ValidateSingleSelection()
        {
            if (_selectedDepartmentIds == null || _selectedDepartmentIds.Count == 0)
            {
                ShowInfo("Vui lòng chọn một dòng để chỉnh sửa.");
                return false;
            }

            if (_selectedDepartmentIds.Count > 1)
            {
                ShowInfo("Chỉ cho phép chỉnh sửa 1 dòng. Vui lòng bỏ chọn bớt.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate selection: Phải chọn ít nhất 1 phòng ban.
        /// </summary>
        /// <returns>True nếu hợp lệ, False nếu không hợp lệ (đã hiển thị thông báo)</returns>
        private bool ValidateMultipleSelection()
        {
            if (_selectedDepartmentIds == null || _selectedDepartmentIds.Count == 0)
            {
                ShowInfo("Vui lòng chọn ít nhất một dòng để xóa.");
                return false;
            }

            return true;
        }

        #endregion

        #region ========== MESSAGE DISPLAY ==========

        /// <summary>
        /// Hiển thị thông báo thông tin cho người dùng.
        /// </summary>
        /// <param name="message">Nội dung thông báo</param>
        private void ShowInfo(string message)
        {
            MsgBox.ShowSuccess(message, "Thông tin");
        }

        /// <summary>
        /// Hiển thị thông báo lỗi cho người dùng.
        /// </summary>
        /// <param name="message">Nội dung thông báo lỗi</param>
        private void ShowError(string message)
        {
            MsgBox.ShowError(message);
        }

        /// <summary>
        /// Xử lý exception và hiển thị thông báo lỗi cho người dùng.
        /// </summary>
        /// <param name="ex">Exception cần xử lý</param>
        /// <param name="contextMessage">Thông điệp ngữ cảnh (tùy chọn)</param>
        private void HandleException(Exception ex, string contextMessage = null)
        {
            if (string.IsNullOrWhiteSpace(contextMessage))
            {
                MsgBox.ShowException(ex);
            }
            else
            {
                var message = $"{contextMessage}: {ex.Message}";
                MsgBox.ShowError(message);
            }

            // Log exception để debug (nếu cần)
            System.Diagnostics.Debug.WriteLine($"Exception in UcDepartment: {contextMessage ?? "Unknown"}", ex);
        }

        #endregion

        #region ========== SUPERTOOLTIP SETUP ==========

        /// <summary>
        /// Thiết lập SuperToolTip cho tất cả các controls trong UserControl.
        /// </summary>
        private void SetupSuperTips()
        {
            try
            {
                SetupBarButtonSuperTips();
            }
            catch (Exception ex)
            {
                HandleException(ex, "Lỗi khi setup SuperToolTip");
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các BarButtonItem trên thanh công cụ.
        /// </summary>
        private void SetupBarButtonSuperTips()
        {
            // SuperTip cho nút Tải dữ liệu
            SuperToolTipHelper.SetBarButtonSuperTip(
                ListDataBarButtonItem,
                title: @"<b><color=Blue>🔄 Tải dữ liệu</color></b>",
                content: @"Tải lại <b>danh sách phòng ban</b> từ database.<br/><br/><b>Chức năng:</b><br/>• Tải lại toàn bộ danh sách phòng ban từ database<br/>• Hiển thị WaitForm trong quá trình tải<br/>• Cập nhật TreeList với dữ liệu mới nhất<br/>• Tự động expand tất cả các node trong TreeList<br/>• Xóa selection và checkbox hiện tại sau khi tải<br/>• Cấu hình TreeList để hiển thị dạng cây phân cấp<br/><br/><b>Quy trình:</b><br/>• Hiển thị WaitForm1<br/>• Gọi DepartmentBll.GetAll() để lấy dữ liệu<br/>• Convert Entity → DTO<br/>• Bind dữ liệu vào TreeList<br/>• Tự động expand tất cả nodes<br/>• Cấu hình hiển thị multi-line cho các cột dài<br/>• Clear selection và checkbox states<br/>• Cập nhật trạng thái các nút toolbar<br/>• Đóng WaitForm1<br/><br/><b>Kết quả:</b><br/>• TreeList hiển thị danh sách phòng ban mới nhất dạng cây phân cấp<br/>• Tất cả nodes được expand<br/>• Selection và checkbox được xóa<br/>• Thống kê được cập nhật<br/><br/><color=Gray>Lưu ý:</color> Nút này sẽ tải lại toàn bộ dữ liệu từ database. TreeList sẽ hiển thị dạng cây phân cấp với parent-child relationship. Nếu đang có selection hoặc checkbox được chọn, chúng sẽ bị xóa sau khi tải xong."
            );

            // SuperTip cho nút Thêm mới
            SuperToolTipHelper.SetBarButtonSuperTip(
                NewBarButtonItem,
                title: @"<b><color=Green>➕ Thêm mới</color></b>",
                content: @"Mở form <b>thêm mới phòng ban</b>.<br/><br/><b>Chức năng:</b><br/>• Mở form FrmDepartmentDetail ở chế độ thêm mới<br/>• Hiển thị overlay trên UserControl<br/>• Tự động tải lại dữ liệu sau khi đóng form (nếu lưu thành công)<br/>• Cập nhật trạng thái các nút toolbar<br/><br/><b>Quy trình:</b><br/>• Hiển thị OverlayManager trên UserControl<br/>• Tạo form FrmDepartmentDetail với Guid.Empty (thêm mới)<br/>• Hiển thị form dạng modal dialog<br/>• Sau khi đóng form: Kiểm tra DialogResult<br/>• Nếu OK: Tải lại dữ liệu (không hiển thị WaitForm)<br/>• Cập nhật trạng thái các nút toolbar<br/>• Đóng OverlayManager<br/><br/><b>Yêu cầu:</b><br/>• Form sẽ tự động lấy CompanyId từ database<br/>• Người dùng cần nhập đầy đủ thông tin bắt buộc (Mã phòng ban, Tên phòng ban, Chi nhánh)<br/>• Có thể chọn phòng ban cha để tạo cấu trúc phân cấp<br/><br/><b>Kết quả:</b><br/>• Nếu lưu thành công: Danh sách được tải lại với phòng ban mới<br/>• Nếu hủy: Không có thay đổi<br/><br/><color=Gray>Lưu ý:</color> Form sẽ được hiển thị ở chế độ modal, bạn phải đóng form trước khi có thể thao tác với UserControl này. Phòng ban mới có thể được tạo với phòng ban cha để tạo cấu trúc phân cấp."
            );

            // SuperTip cho nút Sửa
            SuperToolTipHelper.SetBarButtonSuperTip(
                EditBarButtonItem,
                title: @"<b><color=Orange>✏️ Sửa</color></b>",
                content: @"Mở form <b>chỉnh sửa phòng ban</b>.<br/><br/><b>Chức năng:</b><br/>• Mở form FrmDepartmentDetail ở chế độ chỉnh sửa<br/>• Hiển thị overlay trên UserControl<br/>• Tự động tải lại dữ liệu sau khi đóng form (nếu lưu thành công)<br/>• Cập nhật trạng thái các nút toolbar<br/><br/><b>Quy trình:</b><br/>• Kiểm tra selection: Phải chọn đúng 1 dòng (dựa trên checkbox)<br/>• Hiển thị OverlayManager trên UserControl<br/>• Tạo form FrmDepartmentDetail với ID phòng ban đã chọn<br/>• Hiển thị form dạng modal dialog<br/>• Sau khi đóng form: Kiểm tra DialogResult<br/>• Nếu OK: Tải lại dữ liệu (không hiển thị WaitForm)<br/>• Cập nhật trạng thái các nút toolbar<br/>• Đóng OverlayManager<br/><br/><b>Yêu cầu:</b><br/>• Phải chọn đúng 1 dòng trong TreeList (qua checkbox)<br/>• Nếu chọn nhiều hơn 1 dòng: Hiển thị thông báo yêu cầu bỏ chọn bớt<br/>• Nếu không chọn dòng nào: Hiển thị thông báo yêu cầu chọn dòng<br/><br/><b>Kết quả:</b><br/>• Nếu lưu thành công: Danh sách được tải lại với dữ liệu đã cập nhật<br/>• Nếu hủy: Không có thay đổi<br/><br/><color=Gray>Lưu ý:</color> Nút này chỉ được kích hoạt khi chọn đúng 1 dòng (checkbox được check). Form sẽ được hiển thị ở chế độ modal. Mã phòng ban sẽ bị khóa và không thể chỉnh sửa."
            );

            // SuperTip cho nút Xóa
            SuperToolTipHelper.SetBarButtonSuperTip(
                DeleteBarButtonItem,
                title: @"<b><color=Red>🗑️ Xóa</color></b>",
                content: @"Xóa <b>phòng ban</b> đã chọn.<br/><br/><b>Chức năng:</b><br/>• Xóa một hoặc nhiều phòng ban đã chọn (qua checkbox)<br/>• Hiển thị dialog xác nhận<br/>• Tự động tải lại dữ liệu sau khi xóa<br/>• Xóa selection và checkbox sau khi xóa<br/><br/><b>Quy trình:</b><br/>• Kiểm tra selection: Phải chọn ít nhất 1 dòng (qua checkbox)<br/>• Hiển thị dialog xác nhận (Yes/No) với số lượng phòng ban sẽ xóa<br/>• Nếu xác nhận: Hiển thị WaitForm1<br/>• Gọi DepartmentBll.DeleteMultiple() để xóa nhiều phòng ban cùng lúc<br/>• Hiển thị thông báo thành công/thất bại<br/>• Clear selection và checkbox states<br/>• Tải lại dữ liệu (không hiển thị WaitForm)<br/>• Cập nhật trạng thái các nút toolbar<br/>• Đóng WaitForm1<br/><br/><b>Yêu cầu:</b><br/>• Phải chọn ít nhất 1 dòng trong TreeList (qua checkbox)<br/>• Phải xác nhận qua dialog Yes/No<br/><br/><b>Kết quả:</b><br/>• Nếu thành công: Danh sách được tải lại, các phòng ban đã chọn bị xóa<br/>• Nếu lỗi: Hiển thị thông báo lỗi, dữ liệu không thay đổi<br/><br/><color=Gray>Lưu ý:</color> Nút này chỉ được kích hoạt khi chọn ít nhất 1 dòng (checkbox được check). Bạn có thể chọn nhiều phòng ban để xóa cùng lúc. Hệ thống sẽ xóa tất cả các phòng ban đã chọn trong một lần thao tác."
            );

            // SuperTip cho nút Xuất dữ liệu
            SuperToolTipHelper.SetBarButtonSuperTip(
                ExportBarButtonItem,
                title: @"<b><color=Purple>📊 Xuất dữ liệu</color></b>",
                content: @"Xuất <b>danh sách phòng ban</b> ra file Excel.<br/><br/><b>Chức năng:</b><br/>• Xuất toàn bộ dữ liệu trong TreeList ra file Excel<br/>• Hiển thị SaveFileDialog để chọn vị trí lưu file<br/>• Tự động đặt tên file mặc định: Departments.xlsx<br/>• Hiển thị thông báo thành công sau khi xuất<br/><br/><b>Quy trình:</b><br/>• Kiểm tra có dữ liệu trong TreeList không (VisibleNodesCount)<br/>• Hiển thị SaveFileDialog với filter Excel Files (*.xlsx)<br/>• Nếu người dùng chọn vị trí lưu: Gọi TreeList.ExportToXlsx()<br/>• Hiển thị thông báo thành công<br/><br/><b>Yêu cầu:</b><br/>• Phải có ít nhất 1 node hiển thị trong TreeList<br/>• Người dùng phải chọn vị trí lưu file<br/><br/><b>Kết quả:</b><br/>• File Excel được tạo tại vị trí đã chọn<br/>• File chứa toàn bộ dữ liệu hiển thị trong TreeList (bao gồm cấu trúc phân cấp)<br/>• Hiển thị thông báo thành công<br/><br/><color=Gray>Lưu ý:</color> Nút này chỉ được kích hoạt khi có dữ liệu trong TreeList. File Excel sẽ chứa toàn bộ dữ liệu đang hiển thị, bao gồm cả cấu trúc phân cấp parent-child của phòng ban."
            );
        }

        #endregion
    }
}
