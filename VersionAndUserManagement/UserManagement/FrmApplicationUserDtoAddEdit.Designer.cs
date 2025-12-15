using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraLayout;

namespace VersionAndUserManagement.UserManagement
{
    partial class FrmApplicationUserDtoAddEdit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.SaveBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.CloseBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.ChangePasswordHyperlinkLabelControl = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.RetypePasswordTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.PasswordTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.EmployeeSearchLookUpEdit = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.employeeDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colThongTinHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemHypertextLabel1 = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.UserNameTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ActiveCheckEdit = new DevExpress.XtraEditors.CheckEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForUserName = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForActive = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RetypePasswordTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PasswordTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmployeeSearchLookUpEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeeDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHypertextLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserNameTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActiveCheckEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUserName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForActive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.SaveBarButtonItem,
            this.CloseBarButtonItem});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 3;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.SaveBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.CloseBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // SaveBarButtonItem
            // 
            this.SaveBarButtonItem.Caption = "Lưu";
            this.SaveBarButtonItem.Id = 0;
            this.SaveBarButtonItem.ImageOptions.Image = global::VersionAndUserManagement.Properties.Resources.save_16x16;
            this.SaveBarButtonItem.ImageOptions.LargeImage = global::VersionAndUserManagement.Properties.Resources.save_32x32;
            this.SaveBarButtonItem.Name = "SaveBarButtonItem";
            // 
            // CloseBarButtonItem
            // 
            this.CloseBarButtonItem.Caption = "Đóng";
            this.CloseBarButtonItem.Id = 1;
            this.CloseBarButtonItem.ImageOptions.Image = global::VersionAndUserManagement.Properties.Resources.cancel_16x16;
            this.CloseBarButtonItem.ImageOptions.LargeImage = global::VersionAndUserManagement.Properties.Resources.cancel_32x32;
            this.CloseBarButtonItem.Name = "CloseBarButtonItem";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(390, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 214);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(390, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 190);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(390, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 190);
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
            // 
            // dataLayoutControl1
            // 
            this.dataLayoutControl1.Controls.Add(this.ChangePasswordHyperlinkLabelControl);
            this.dataLayoutControl1.Controls.Add(this.RetypePasswordTextEdit);
            this.dataLayoutControl1.Controls.Add(this.PasswordTextEdit);
            this.dataLayoutControl1.Controls.Add(this.EmployeeSearchLookUpEdit);
            this.dataLayoutControl1.Controls.Add(this.UserNameTextEdit);
            this.dataLayoutControl1.Controls.Add(this.ActiveCheckEdit);
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 24);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(390, 190);
            this.dataLayoutControl1.TabIndex = 15;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // ChangePasswordHyperlinkLabelControl
            // 
            this.ChangePasswordHyperlinkLabelControl.Location = new System.Drawing.Point(20, 68);
            this.ChangePasswordHyperlinkLabelControl.Name = "ChangePasswordHyperlinkLabelControl";
            this.ChangePasswordHyperlinkLabelControl.Size = new System.Drawing.Size(63, 13);
            this.ChangePasswordHyperlinkLabelControl.StyleController = this.dataLayoutControl1;
            this.ChangePasswordHyperlinkLabelControl.TabIndex = 6;
            this.ChangePasswordHyperlinkLabelControl.Text = "Đổi mật khẩu";
            // 
            // RetypePasswordTextEdit
            // 
            this.RetypePasswordTextEdit.Location = new System.Drawing.Point(109, 117);
            this.RetypePasswordTextEdit.MenuManager = this.barManager1;
            this.RetypePasswordTextEdit.Name = "RetypePasswordTextEdit";
            this.RetypePasswordTextEdit.Properties.PasswordChar = '*';
            this.RetypePasswordTextEdit.Properties.UseSystemPasswordChar = true;
            this.RetypePasswordTextEdit.Size = new System.Drawing.Size(269, 20);
            this.RetypePasswordTextEdit.StyleController = this.dataLayoutControl1;
            this.RetypePasswordTextEdit.TabIndex = 5;
            // 
            // PasswordTextEdit
            // 
            this.PasswordTextEdit.Location = new System.Drawing.Point(109, 93);
            this.PasswordTextEdit.MenuManager = this.barManager1;
            this.PasswordTextEdit.Name = "PasswordTextEdit";
            this.PasswordTextEdit.Properties.PasswordChar = '*';
            this.PasswordTextEdit.Properties.UseSystemPasswordChar = true;
            this.PasswordTextEdit.Size = new System.Drawing.Size(269, 20);
            this.PasswordTextEdit.StyleController = this.dataLayoutControl1;
            this.PasswordTextEdit.TabIndex = 4;
            // 
            // EmployeeSearchLookUpEdit
            // 
            this.EmployeeSearchLookUpEdit.Location = new System.Drawing.Point(109, 12);
            this.EmployeeSearchLookUpEdit.MenuManager = this.barManager1;
            this.EmployeeSearchLookUpEdit.Name = "EmployeeSearchLookUpEdit";
            this.EmployeeSearchLookUpEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.EmployeeSearchLookUpEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.EmployeeSearchLookUpEdit.Properties.DataSource = this.employeeDtoBindingSource;
            this.EmployeeSearchLookUpEdit.Properties.DisplayMember = "ThongTinHtml";
            this.EmployeeSearchLookUpEdit.Properties.PopupView = this.searchLookUpEdit1View;
            this.EmployeeSearchLookUpEdit.Properties.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemHypertextLabel1});
            this.EmployeeSearchLookUpEdit.Properties.ValueMember = "Id";
            this.EmployeeSearchLookUpEdit.Size = new System.Drawing.Size(269, 20);
            this.EmployeeSearchLookUpEdit.StyleController = this.dataLayoutControl1;
            this.EmployeeSearchLookUpEdit.TabIndex = 0;
            // 
            // employeeDtoBindingSource
            // 
            this.employeeDtoBindingSource.DataSource = typeof(DTO.MasterData.Company.EmployeeDto);
            // 
            // searchLookUpEdit1View
            // 
            this.searchLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colThongTinHtml});
            this.searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            this.searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // colThongTinHtml
            // 
            this.colThongTinHtml.ColumnEdit = this.repositoryItemHypertextLabel1;
            this.colThongTinHtml.FieldName = "ThongTinHtml";
            this.colThongTinHtml.Name = "colThongTinHtml";
            this.colThongTinHtml.Visible = true;
            this.colThongTinHtml.VisibleIndex = 0;
            // 
            // repositoryItemHypertextLabel1
            // 
            this.repositoryItemHypertextLabel1.Name = "repositoryItemHypertextLabel1";
            // 
            // UserNameTextEdit
            // 
            this.UserNameTextEdit.Location = new System.Drawing.Point(109, 36);
            this.UserNameTextEdit.MenuManager = this.barManager1;
            this.UserNameTextEdit.Name = "UserNameTextEdit";
            this.UserNameTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.UserNameTextEdit.Size = new System.Drawing.Size(164, 20);
            this.UserNameTextEdit.StyleController = this.dataLayoutControl1;
            this.UserNameTextEdit.TabIndex = 2;
            // 
            // ActiveCheckEdit
            // 
            this.ActiveCheckEdit.Location = new System.Drawing.Point(277, 36);
            this.ActiveCheckEdit.MenuManager = this.barManager1;
            this.ActiveCheckEdit.Name = "ActiveCheckEdit";
            this.ActiveCheckEdit.Properties.Caption = "Đang hoạt động";
            this.ActiveCheckEdit.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.ActiveCheckEdit.Size = new System.Drawing.Size(101, 20);
            this.ActiveCheckEdit.StyleController = this.dataLayoutControl1;
            this.ActiveCheckEdit.TabIndex = 3;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(390, 190);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AllowDrawBackground = false;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForUserName,
            this.layoutControlItem1,
            this.ItemForActive,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "autoGeneratedGroup0";
            this.layoutControlGroup1.Size = new System.Drawing.Size(370, 170);
            // 
            // ItemForUserName
            // 
            this.ItemForUserName.Control = this.UserNameTextEdit;
            this.ItemForUserName.Location = new System.Drawing.Point(0, 24);
            this.ItemForUserName.Name = "ItemForUserName";
            this.ItemForUserName.Size = new System.Drawing.Size(265, 24);
            this.ItemForUserName.Text = "Tên đăng nhập";
            this.ItemForUserName.TextSize = new System.Drawing.Size(85, 13);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.EmployeeSearchLookUpEdit;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(370, 24);
            this.layoutControlItem1.Text = "Nhân viên";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(85, 13);
            // 
            // ItemForActive
            // 
            this.ItemForActive.Control = this.ActiveCheckEdit;
            this.ItemForActive.Location = new System.Drawing.Point(265, 24);
            this.ItemForActive.Name = "ItemForActive";
            this.ItemForActive.Size = new System.Drawing.Size(105, 24);
            this.ItemForActive.Text = "Đang hoạt động";
            this.ItemForActive.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.PasswordTextEdit;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 81);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(370, 24);
            this.layoutControlItem2.Text = "Mật khẩu";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(85, 13);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.RetypePasswordTextEdit;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 105);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(370, 65);
            this.layoutControlItem3.Text = "Nhập lại mật khẩu";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(85, 13);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem4.Control = this.ChangePasswordHyperlinkLabelControl;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 48);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 10, 10, 10);
            this.layoutControlItem4.Size = new System.Drawing.Size(370, 33);
            this.layoutControlItem4.TextVisible = false;
            // 
            // FrmApplicationUserDtoAddEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 214);
            this.Controls.Add(this.dataLayoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmApplicationUserDtoAddEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RetypePasswordTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PasswordTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmployeeSearchLookUpEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeeDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHypertextLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserNameTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActiveCheckEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUserName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private BarManager barManager1;
        private Bar bar2;
        private BarButtonItem SaveBarButtonItem;
        private BarButtonItem CloseBarButtonItem;
        private BarDockControl barDockControlTop;
        private BarDockControl barDockControlBottom;
        private BarDockControl barDockControlLeft;
        private BarDockControl barDockControlRight;
        private DXErrorProvider dxErrorProvider1;
        private DataLayoutControl dataLayoutControl1;
        private LayoutControlGroup Root;
        private LayoutControlGroup layoutControlGroup1;
        private TextEdit UserNameTextEdit;
        private CheckEdit ActiveCheckEdit;
        private LayoutControlItem ItemForUserName;
        private LayoutControlItem ItemForActive;
        private SearchLookUpEdit EmployeeSearchLookUpEdit;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
        private LayoutControlItem layoutControlItem1;
        private BindingSource employeeDtoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colThongTinHtml;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel repositoryItemHypertextLabel1;
        private TextEdit RetypePasswordTextEdit;
        private TextEdit PasswordTextEdit;
        private LayoutControlItem layoutControlItem2;
        private LayoutControlItem layoutControlItem3;
        private HyperlinkLabelControl ChangePasswordHyperlinkLabelControl;
        private LayoutControlItem layoutControlItem4;
    }
}