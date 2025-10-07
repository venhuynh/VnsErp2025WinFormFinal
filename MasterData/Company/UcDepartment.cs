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
        private void NewBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // TODO: Mở FrmDepartmentDetail để thêm mới phòng ban
                // using var form = new FrmDepartmentDetail();
                // var result = form.ShowDialog();
                // if (result == DialogResult.OK)
                // {
                //     LoadDataAsync();
                //     UpdateButtonStates();
                // }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi thêm mới phòng ban");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Sửa
        /// </summary>
        private void EditBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
                // TODO: Mở FrmDepartmentDetail để chỉnh sửa phòng ban
                // var id = _selectedDepartmentIds[0];
                // using var form = new FrmDepartmentDetail(id);
                // var result = form.ShowDialog();
                // if (result == DialogResult.OK)
                // {
                //     LoadDataAsync();
                //     UpdateButtonStates();
                // }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi chỉnh sửa phòng ban");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Xóa
        /// </summary>
        private void DeleteBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
                // TODO: Thực hiện xóa phòng ban
                // await ExecuteWithWaitingFormAsync(async () =>
                // {
                //     var success = await _departmentBll.DeleteMultipleAsync(_selectedDepartmentIds.ToList());
                //     if (success)
                //     {
                //         ShowInfo($"Đã xóa {_selectedDepartmentIds.Count} phòng ban");
                //     }
                // });
                //
                // // Clear selection state trước khi reload
                // ClearSelectionState();
                //
                // // Reload dữ liệu
                // await LoadDataAsync();
                // UpdateButtonStates();
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

        #endregion
    }
}
