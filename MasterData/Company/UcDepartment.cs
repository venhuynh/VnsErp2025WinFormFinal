using DevExpress.Utils;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Menu;
using DevExpress.XtraTreeList.Nodes;
using MasterData.Company.Dto;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MasterData.Company
{
    public partial class UcDepartment : DevExpress.XtraEditors.XtraUserControl
    {
        #region ========== FIELDS ==========
        
        private List<DepartmentDto> _departments;
        private readonly HashSet<Guid> _bookmarks = [];
        
        #endregion

        #region ========== CONSTRUCTOR ==========
        
        public UcDepartment()
        {
            InitializeComponent();
            InitializeTreeList();
            LoadSampleData();
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
            DepartmentTreeList.MouseDown += OnMouseDown;
            DepartmentTreeList.NodeCellStyle += OnNodeCellStyle;
            DepartmentTreeList.PopupMenuShowing += OnPopupMenuShowing;
            DepartmentTreeList.CustomDrawNodeIndicator += OnCustomDrawRowIndicator;
            DepartmentTreeList.KeyDown += OnKeyDown;
            
            // Cấu hình hierarchy
            DepartmentTreeList.KeyFieldName = "Id";
            DepartmentTreeList.ParentFieldName = "ParentId";
        }
        
        #endregion

        #region ========== DATA LOADING ==========
        
        private void LoadSampleData()
        {
            _departments = new List<DepartmentDto>
            {
                // Cấp 1 - Công ty
                new DepartmentDto
                {
                    Id = Guid.NewGuid(),
                    DepartmentCode = "CEO",
                    DepartmentName = "Ban Giám Đốc",
                    Description = "Ban lãnh đạo cao nhất",
                    IsActive = true,
                    CreatedDate = DateTime.Now.AddDays(-30)
                },
                new DepartmentDto
                {
                    Id = Guid.NewGuid(),
                    DepartmentCode = "HR",
                    DepartmentName = "Phòng Nhân Sự",
                    Description = "Quản lý nhân sự",
                    IsActive = true,
                    CreatedDate = DateTime.Now.AddDays(-25)
                },
                new DepartmentDto
                {
                    Id = Guid.NewGuid(),
                    DepartmentCode = "IT",
                    DepartmentName = "Phòng Công Nghệ Thông Tin",
                    Description = "Phát triển và bảo trì hệ thống",
                    IsActive = true,
                    CreatedDate = DateTime.Now.AddDays(-20)
                },
                new DepartmentDto
                {
                    Id = Guid.NewGuid(),
                    DepartmentCode = "FIN",
                    DepartmentName = "Phòng Tài Chính",
                    Description = "Quản lý tài chính",
                    IsActive = true,
                    CreatedDate = DateTime.Now.AddDays(-15)
                }
            };
            
            // Cấp 2 - Phòng ban con
            var hrId = _departments[1].Id;
            var itId = _departments[2].Id;
            
            _departments.AddRange(new List<DepartmentDto>
            {
                new DepartmentDto
                {
                    Id = Guid.NewGuid(),
                    DepartmentCode = "HR-REC",
                    DepartmentName = "Bộ phận Tuyển Dụng",
                    Description = "Tuyển dụng nhân sự",
                    ParentId = hrId,
                    IsActive = true,
                    CreatedDate = DateTime.Now.AddDays(-10)
                },
                new DepartmentDto
                {
                    Id = Guid.NewGuid(),
                    DepartmentCode = "HR-TRAIN",
                    DepartmentName = "Bộ phận Đào Tạo",
                    Description = "Đào tạo nhân viên",
                    ParentId = hrId,
                    IsActive = true,
                    CreatedDate = DateTime.Now.AddDays(-8)
                },
                new DepartmentDto
                {
                    Id = Guid.NewGuid(),
                    DepartmentCode = "IT-DEV",
                    DepartmentName = "Bộ phận Phát Triển",
                    Description = "Phát triển phần mềm",
                    ParentId = itId,
                    IsActive = true,
                    CreatedDate = DateTime.Now.AddDays(-5)
                },
                new DepartmentDto
                {
                    Id = Guid.NewGuid(),
                    DepartmentCode = "IT-SUPPORT",
                    DepartmentName = "Bộ phận Hỗ Trợ",
                    Description = "Hỗ trợ kỹ thuật",
                    ParentId = itId,
                    IsActive = true,
                    CreatedDate = DateTime.Now.AddDays(-3)
                }
            });
            
            // Cập nhật dữ liệu
            departmentDtoBindingSource.DataSource = _departments;
            DepartmentTreeList.ExpandAll();
            
            // Cập nhật status bar
            UpdateStatusBar();
        }
        
        #endregion

        #region ========== EVENT HANDLERS ==========
        
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            
            var hitInfo = DepartmentTreeList.CalcHitInfo(e.Location);
            if (hitInfo.InRowCell && hitInfo.Column == colDescription)
            {
                var department = DepartmentTreeList.GetRow(hitInfo.Node.Id) as DepartmentDto;
                if (department != null && !string.IsNullOrEmpty(department.Description))
                {
                    ToolTipController.DefaultController.ShowHint(department.Description, ToolTipLocation.RightCenter);
                }
            }
        }
        
        private void OnNodeCellStyle(object sender, GetCustomNodeCellStyleEventArgs e)
        {
            var department = DepartmentTreeList.GetRow(e.Node.Id) as DepartmentDto;
            if (department == null) return;
            
            // Tô màu cho phòng ban không hoạt động
            if (!department.IsActive)
            {
                e.Appearance.ForeColor = Color.Gray;
                e.Appearance.FontStyleDelta = FontStyle.Strikeout;
            }
            
            // Tô màu cho phòng ban có nhiều nhân viên
            if (department.EmployeeCount > 10)
            {
                e.Appearance.BackColor = Color.LightBlue;
            }
        }
        
        private void OnPopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (e.Menu is TreeListNodeMenu)
            {
                // Thêm menu Indent/Outdent
                var indentItem = new DevExpress.Utils.Menu.DXMenuItem("Tăng cấp");
                indentItem.Tag = "Indent";
                indentItem.Enabled = DepartmentTreeList.CanIndentNodes(DepartmentTreeList.Selection);
                indentItem.Click += OnMenuItemClick;
                e.Menu.Items.Add(indentItem);
                
                var outdentItem = new DevExpress.Utils.Menu.DXMenuItem("Giảm cấp");
                outdentItem.Tag = "Outdent";
                outdentItem.Enabled = DepartmentTreeList.CanOutdentNodes(DepartmentTreeList.Selection);
                outdentItem.Click += OnMenuItemClick;
                e.Menu.Items.Add(outdentItem);
                
                
                // Thêm menu Bookmark
                var bookmarkItem = new DevExpress.Utils.Menu.DXMenuItem("Đánh dấu");
                bookmarkItem.Tag = "Bookmark";
                bookmarkItem.Click += OnMenuItemClick;
                e.Menu.Items.Add(bookmarkItem);
                
                e.Allow = true;
            }
        }
        
        private void OnMenuItemClick(object sender, EventArgs e)
        {
            var item = sender as DevExpress.Utils.Menu.DXMenuItem;
            if (item?.Tag == null) return;
            
            switch (item.Tag.ToString())
            {
                case "Indent":
                    DepartmentTreeList.IndentNodes(DepartmentTreeList.Selection, false);
                    break;
                case "Outdent":
                    DepartmentTreeList.OutdentNodes(DepartmentTreeList.Selection, false);
                    break;
                case "Bookmark":
                    ToggleBookmark(DepartmentTreeList.FocusedNode);
                    break;
            }
        }
        
        private void OnCustomDrawRowIndicator(object sender, CustomDrawNodeIndicatorEventArgs e)
        {
            if (e.Node == null) return;
            
            var department = DepartmentTreeList.GetRow(e.Node.Id) as DepartmentDto;
            if (department == null) return;
            
            // Hiển thị bookmark
            if (_bookmarks.Contains(department.Id))
            {
                e.DefaultDraw();
               
                e.Handled = true;
            }
        }
        
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // Toggle bookmark với Ctrl+B
            if (e.KeyData == (Keys.B | Keys.Control))
            {
                e.Handled = ToggleBookmark(DepartmentTreeList.FocusedNode);
            }
        }
        
        #endregion

        #region ========== HELPER METHODS ==========
        
        private bool ToggleBookmark(TreeListNode node)
        {
            if (node == null) return false;
            
            var department = DepartmentTreeList.GetRow(node.Id) as DepartmentDto;
            if (department == null) return false;
            
            if (!_bookmarks.Remove(department.Id))
            {
                _bookmarks.Add(department.Id);
            }
            
            DepartmentTreeList.InvalidateNode(node);
            return true;
        }
        
        private void UpdateStatusBar()
        {
            var totalCount = _departments?.Count ?? 0;
            
            DataSummaryBarStaticItem.Caption = $@"Tổng: {totalCount} phòng ban";
        }
        
        #endregion
    }
}
