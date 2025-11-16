using Bll.Common;
using Bll.MasterData.Company;
using Bll.Utils;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using MasterData.Company.Converters;
using MasterData.Company.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterData.Company
{
    public partial class UcDepartment : XtraUserControl
    {
        #region ========== FIELDS ==========
        
        private List<DepartmentDto> _departments;
        // ReSharper disable once CollectionNeverUpdated.Local
        private readonly HashSet<Guid> _bookmarks = [];
        private readonly DepartmentBll _departmentBll = new();
        
        /// <summary>
        /// Danh sách ID của các department được chọn
        /// </summary>
        private readonly List<Guid> _selectedDepartmentIds = [];
        
        /// <summary>
        /// Flag để ngăn chặn selection khi đang xử lý checkbox
        /// </summary>
        private bool _isProcessingCheckbox;

        #endregion

        #region ========== CONSTRUCTOR ==========

        public UcDepartment()
        {
            InitializeComponent();
            InitializeTreeList();
            
            // Toolbar events
            ListDataBarButtonItem.ItemClick += ListDataBarButtonItem_ItemClick;
            NewBarButtonItem.ItemClick += NewBarButtonItem_ItemClick;
            EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
            DeleteBarButtonItem.ItemClick += DeleteBarButtonItem_ItemClick;
            ExportBarButtonItem.ItemClick += ExportBarButtonItem_ItemClick;
            
            UpdateButtonStates();

            // Setup SuperToolTips
            SetupSuperTips();
        }
        
        #endregion

        #region ========== INITIALIZATION ==========
        
        private void InitializeTreeList()
        {
            // Cấu hình TreeList
            DepartmentTreeList.OptionsBehavior.Editable = false;
            DepartmentTreeList.OptionsFind.AlwaysVisible = true;
            DepartmentTreeList.OptionsSelection.MultiSelect = true;
            DepartmentTreeList.OptionsView.ShowIndentAsRowStyle = true;
            
            // Thêm event handlers
            DepartmentTreeList.CustomDrawNodeIndicator += OnCustomDrawRowIndicator;
            DepartmentTreeList.SelectionChanged += DepartmentTreeList_SelectionChanged;
            DepartmentTreeList.BeforeCheckNode += DepartmentTreeList_BeforeCheckNode;
            DepartmentTreeList.AfterCheckNode += DepartmentTreeList_AfterCheckNode;
            
            // Cấu hình hierarchy
            DepartmentTreeList.KeyFieldName = "Id";
            DepartmentTreeList.ParentFieldName = "ParentId";
        }
        
        #endregion

        #region ========== DATA LOADING ==========
        
        private async Task LoadDataAsync()
        {
            try
            {
                await ExecuteWithWaitingFormAsync(async () => { await LoadDataAsyncWithoutSplash(); });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu");
            }
        }

        /// <summary>
        /// Tải dữ liệu và bind vào TreeList (không hiển thị WaitForm).
        /// </summary>
        private Task LoadDataAsyncWithoutSplash()
        {
            try
            {
                // Lấy entities từ BLL (synchronous method)
                var departmentEntities = _departmentBll.GetAll();
                
                // Convert entities to DTOs sử dụng DepartmentConverters
                _departments = [.. departmentEntities.Select(e => e.ToDto())];
                
                // Cập nhật dữ liệu
                departmentDtoBindingSource.DataSource = _departments;
                DepartmentTreeList.ExpandAll();
                
                // Cấu hình TreeList
                ConfigureMultiLineGridView();
                
                // Clear selection state trước khi update button states
                ClearSelectionState();
                
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu");
                return Task.CompletedTask;
            }
        }
        
        #endregion

        #region ========== EVENT HANDLERS ==========
        
        private void OnCustomDrawRowIndicator(object sender, CustomDrawNodeIndicatorEventArgs e)
        {
            if (e.Node == null) return;
            
            if (DepartmentTreeList.GetRow(e.Node.Id) is not DepartmentDto department) return;
            
            // Hiển thị bookmark
            if (_bookmarks.Contains(department.Id))
            {
                e.DefaultDraw();
               
                e.Handled = true;
            }
        }
         
        
        #endregion

        #region ========== HELPER METHODS ==========
        
        private void DepartmentTreeList_SelectionChanged(object sender, EventArgs e)
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
        /// Event handler trước khi checkbox của node thay đổi
        /// </summary>
        private void DepartmentTreeList_BeforeCheckNode(object sender, CheckNodeEventArgs e)
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
        /// Event handler khi checkbox của node thay đổi
        /// </summary>
        private void DepartmentTreeList_AfterCheckNode(object sender, NodeEventArgs e)
        {
            try
            {
                // Xử lý logic parent-child checkbox
                HandleParentChildCheckboxLogic(e.Node);

                // Reset flag
                _isProcessingCheckbox = false;

                // Cập nhật danh sách selected IDs khi checkbox thay đổi
                UpdateSelectedDepartmentIds();
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                _isProcessingCheckbox = false;
                ShowError(ex);
            }
        }

        private void ConfigureMultiLineGridView()
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

                // Tùy chọn hiển thị: căn giữa tiêu đề cho đẹp
                DepartmentTreeList.Appearance.HeaderPanel.TextOptions.HAlignment = HorzAlignment.Center;
                DepartmentTreeList.Appearance.HeaderPanel.Options.UseTextOptions = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in ConfigureMultiLineGridView: {ex.Message}");
            }
        }

        /// <summary>
        /// Áp dụng RepositoryItemMemoEdit cho cột cụ thể
        /// </summary>
        /// <param name="fieldName">Tên field của cột</param>
        /// <param name="memo">RepositoryItemMemoEdit</param>
        private void ApplyMemoEditorToColumn(string fieldName, DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit memo)
        {
            var col = DepartmentTreeList.Columns[fieldName];
            if (col == null) return;
            // Thêm repository vào TreeList nếu chưa có
            if (!DepartmentTreeList.RepositoryItems.Contains(memo))
            {
                DepartmentTreeList.RepositoryItems.Add(memo);
            }

            col.ColumnEdit = memo;
        }
        
        #endregion

        #region ========== TOOLBAR EVENTS ==========

        /// <summary>
        /// Xử lý sự kiện click button Tải dữ liệu
        /// </summary>
        private async void ListDataBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Thêm mới
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
                ShowError(ex, "Lỗi hiển thị màn hình thêm mới");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Sửa
        /// </summary>
        private async void EditBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Chỉ cho phép chỉnh sửa 1 dòng dữ liệu
                if (_selectedDepartmentIds == null || _selectedDepartmentIds.Count == 0)
                {
                    ShowInfo("Vui lòng chọn một dòng để chỉnh sửa.");
                    return;
                }

                if (_selectedDepartmentIds.Count > 1)
                {
                    ShowInfo("Chỉ cho phép chỉnh sửa 1 dòng. Vui lòng bỏ chọn bớt.");
                    return;
                }

                try
                {
                    using (OverlayManager.ShowScope(this))
                    {
                        var id = _selectedDepartmentIds[0];
                        using (var form = new FrmDepartmentDetail(id))
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
                    ShowError(ex, "Lỗi hiển thị màn hình điều chỉnh");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi chỉnh sửa phòng ban");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Xóa
        /// </summary>
        private async void DeleteBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_selectedDepartmentIds == null || _selectedDepartmentIds.Count == 0)
            {
                ShowInfo("Vui lòng chọn ít nhất một dòng để xóa.");
                return;
            }

            var confirmMessage = _selectedDepartmentIds.Count == 1
                ? "Bạn có chắc muốn xóa dòng dữ liệu đã chọn?"
                : $"Bạn có chắc muốn xóa {_selectedDepartmentIds.Count} dòng dữ liệu đã chọn?";

            var confirmed = MsgBox.GetConfirmFromYesNoDialog(confirmMessage, "Xác nhận xóa");
            if (!confirmed) return;

            try
            {
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

                // Clear selection state trước khi reload
                ClearSelectionState();

                // Reload dữ liệu
                await LoadDataAsyncWithoutSplash();
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xóa dữ liệu");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Xuất dữ liệu
        /// </summary>
        private void ExportBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Chỉ cho phép xuất khi có dữ liệu hiển thị
                var rowCount = DepartmentTreeList.VisibleNodesCount;
                if (rowCount <= 0)
                {
                    ShowInfo("Không có dữ liệu để xuất.");
                    return;
                }

                // Export TreeList data
                var saveDialog = new SaveFileDialog
                {
                    Filter = @"Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*",
                    FileName = "Departments.xlsx"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    DepartmentTreeList.ExportToXlsx(saveDialog.FileName);
                    ShowInfo("Xuất dữ liệu thành công!");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xuất dữ liệu");
            }
        }

        #endregion

        #region ========== UTILITY METHODS ==========

        /// <summary>
        /// Thực hiện operation async với WaitingForm hiển thị.
        /// </summary>
        /// <param name="operation">Operation async cần thực hiện</param>
        private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
        {
            try
            {
                // Hiển thị WaitForm1 từ BLL
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
        /// Cập nhật trạng thái các nút toolbar dựa trên selection và checkbox
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
            catch (Exception)
            {
                // ignore
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
                ShowError(ex);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái của tất cả parent nodes dựa trên trạng thái của children.
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
            catch (Exception)
            {
                // ignore
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
            catch (Exception)
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
            catch (Exception)
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
            catch (Exception)
            {
                // ignore
            }
        }

        /// <summary>
        /// Cập nhật danh sách selected department IDs dựa trên checkbox state.
        /// </summary>
        private void UpdateSelectedDepartmentIds()
        {
            _selectedDepartmentIds.Clear();

            // Chỉ lấy các nodes có checkbox được check (không dựa vào selection)
            foreach (TreeListNode node in DepartmentTreeList.Nodes)
            {
                CheckNodeRecursive(node);
            }
        }

        /// <summary>
        /// Kiểm tra node và các child nodes một cách đệ quy.
        /// </summary>
        private void CheckNodeRecursive(TreeListNode node)
        {
            bool isChecked = node.Checked;

            // Chỉ thêm vào danh sách nếu checkbox được check (không dựa vào selection)
            if (isChecked)
            {
                // Lấy dữ liệu trực tiếp từ node thay vì dựa vào index
                var department = GetDepartmentFromNode(node);
                if (department != null)
                {
                    if (!_selectedDepartmentIds.Contains(department.Id))
                    {
                        _selectedDepartmentIds.Add(department.Id);
                    }
                }
            }

            // Kiểm tra các child nodes
            foreach (TreeListNode childNode in node.Nodes)
            {
                CheckNodeRecursive(childNode);
            }
        }

        /// <summary>
        /// Lấy DepartmentDto từ TreeListNode một cách chính xác.
        /// </summary>
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
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Xóa trạng thái chọn hiện tại trên TreeList.
        /// </summary>
        private void ClearSelectionState()
        {
            _selectedDepartmentIds.Clear();

            // Clear tất cả selection (cả checkbox và regular selection)
            DepartmentTreeList.ClearSelection();
            DepartmentTreeList.FocusedNode = null;

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
            catch (Exception)
            {
                // ignore
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
            MsgBox.ShowInfo(message, "Thông tin");
        }

        /// <summary>
        /// Hiển thị lỗi với thông tin ngữ cảnh.
        /// </summary>
        private void ShowError(Exception ex, string context = null)
        {
            if (string.IsNullOrWhiteSpace(context))
            {
                MsgBox.ShowException(ex);
            }
            else
            {
                var message = $"{context}: {ex.Message}";
                MsgBox.ShowError(message);
            }
        }

        /// <summary>
        /// Hiển thị lỗi với thông báo.
        /// </summary>
        private void ShowError(string message)
        {
            MsgBox.ShowError(message);
        }

        #endregion

        #region ========== SUPERTOOLTIP ==========

        /// <summary>
        /// Thiết lập SuperToolTip cho tất cả các controls trong UserControl
        /// </summary>
        private void SetupSuperTips()
        {
            try
            {
                SetupBarButtonSuperTips();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi setup SuperToolTip");
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các BarButtonItem
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
