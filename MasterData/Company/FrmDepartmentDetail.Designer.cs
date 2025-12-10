using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraLayout;
using DTO.MasterData;
using DTO.MasterData.Company;

namespace MasterData.Company
{
    partial class FrmDepartmentDetail
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
            this.DepartmentCodeTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.DepartmentNameTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.DescriptionTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.BranchNameSearchLookupedit = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.companyBranchLookupDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colBranchInfoHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ParentDepartmentNameTextEdit = new DevExpress.XtraEditors.TreeListLookUpEdit();
            this.departmentDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.treeListLookUpEdit1TreeList = new DevExpress.XtraTreeList.TreeList();
            this.colParentDepartmentName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colDepartmentName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.IsActiveToogleSwitch = new DevExpress.XtraEditors.ToggleSwitch();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForDepartmentCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForDepartmentName = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForDescription = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForIsActive = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForBranchName = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForParentDepartmentName = new DevExpress.XtraLayout.LayoutControlItem();
            this.ChiNhanhInfoHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DepartmentCodeTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepartmentNameTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BranchNameSearchLookupedit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.companyBranchLookupDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParentDepartmentNameTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.departmentDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListLookUpEdit1TreeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsActiveToogleSwitch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsActive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForBranchName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForParentDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChiNhanhInfoHypertextLabel)).BeginInit();
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
            this.barManager1.MaxItemId = 2;
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
            this.SaveBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.save_16x16;
            this.SaveBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.save_32x32;
            this.SaveBarButtonItem.Name = "SaveBarButtonItem";
            // 
            // CloseBarButtonItem
            // 
            this.CloseBarButtonItem.Caption = "Đóng";
            this.CloseBarButtonItem.Id = 1;
            this.CloseBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.cancel_16x16;
            this.CloseBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.cancel_32x32;
            this.CloseBarButtonItem.Name = "CloseBarButtonItem";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(478, 39);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 258);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(478, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 39);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 219);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(478, 39);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 219);
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
            // 
            // dataLayoutControl1
            // 
            this.dataLayoutControl1.Controls.Add(this.DepartmentCodeTextEdit);
            this.dataLayoutControl1.Controls.Add(this.DepartmentNameTextEdit);
            this.dataLayoutControl1.Controls.Add(this.DescriptionTextEdit);
            this.dataLayoutControl1.Controls.Add(this.BranchNameSearchLookupedit);
            this.dataLayoutControl1.Controls.Add(this.ParentDepartmentNameTextEdit);
            this.dataLayoutControl1.Controls.Add(this.IsActiveToogleSwitch);
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 39);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(478, 219);
            this.dataLayoutControl1.TabIndex = 9;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // DepartmentCodeTextEdit
            // 
            this.DepartmentCodeTextEdit.Location = new System.Drawing.Point(106, 84);
            this.DepartmentCodeTextEdit.MenuManager = this.barManager1;
            this.DepartmentCodeTextEdit.Name = "DepartmentCodeTextEdit";
            this.DepartmentCodeTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.DepartmentCodeTextEdit.Size = new System.Drawing.Size(129, 28);
            this.DepartmentCodeTextEdit.StyleController = this.dataLayoutControl1;
            this.DepartmentCodeTextEdit.TabIndex = 3;
            // 
            // DepartmentNameTextEdit
            // 
            this.DepartmentNameTextEdit.Location = new System.Drawing.Point(106, 118);
            this.DepartmentNameTextEdit.MenuManager = this.barManager1;
            this.DepartmentNameTextEdit.Name = "DepartmentNameTextEdit";
            this.DepartmentNameTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.DepartmentNameTextEdit.Size = new System.Drawing.Size(356, 28);
            this.DepartmentNameTextEdit.StyleController = this.dataLayoutControl1;
            this.DepartmentNameTextEdit.TabIndex = 5;
            // 
            // DescriptionTextEdit
            // 
            this.DescriptionTextEdit.Location = new System.Drawing.Point(106, 152);
            this.DescriptionTextEdit.MenuManager = this.barManager1;
            this.DescriptionTextEdit.Name = "DescriptionTextEdit";
            this.DescriptionTextEdit.Size = new System.Drawing.Size(356, 28);
            this.DescriptionTextEdit.StyleController = this.dataLayoutControl1;
            this.DescriptionTextEdit.TabIndex = 6;
            // 
            // BranchNameSearchLookupedit
            // 
            this.BranchNameSearchLookupedit.Location = new System.Drawing.Point(106, 16);
            this.BranchNameSearchLookupedit.MenuManager = this.barManager1;
            this.BranchNameSearchLookupedit.Name = "BranchNameSearchLookupedit";
            this.BranchNameSearchLookupedit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.BranchNameSearchLookupedit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.BranchNameSearchLookupedit.Properties.DataSource = this.companyBranchLookupDtoBindingSource;
            this.BranchNameSearchLookupedit.Properties.DisplayMember = "BranchInfoHtml";
            this.BranchNameSearchLookupedit.Properties.NullText = "";
            this.BranchNameSearchLookupedit.Properties.PopupView = this.searchLookUpEdit1View;
            this.BranchNameSearchLookupedit.Properties.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ChiNhanhInfoHypertextLabel});
            this.BranchNameSearchLookupedit.Properties.ValueMember = "Id";
            this.BranchNameSearchLookupedit.Size = new System.Drawing.Size(356, 28);
            this.BranchNameSearchLookupedit.StyleController = this.dataLayoutControl1;
            this.BranchNameSearchLookupedit.TabIndex = 0;
            // 
            // companyBranchLookupDtoBindingSource
            // 
            this.companyBranchLookupDtoBindingSource.DataSource = typeof(DTO.MasterData.Company.CompanyBranchLookupDto);
            // 
            // searchLookUpEdit1View
            // 
            this.searchLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colBranchInfoHtml});
            this.searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            this.searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View.OptionsView.RowAutoHeight = true;
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // colBranchInfoHtml
            // 
            this.colBranchInfoHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colBranchInfoHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colBranchInfoHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colBranchInfoHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colBranchInfoHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colBranchInfoHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colBranchInfoHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colBranchInfoHtml.AppearanceHeader.Options.UseFont = true;
            this.colBranchInfoHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colBranchInfoHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colBranchInfoHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colBranchInfoHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colBranchInfoHtml.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colBranchInfoHtml.Caption = "Thông tin chi nhánh";
            this.colBranchInfoHtml.ColumnEdit = this.ChiNhanhInfoHypertextLabel;
            this.colBranchInfoHtml.FieldName = "BranchInfoHtml";
            this.colBranchInfoHtml.Name = "colBranchInfoHtml";
            this.colBranchInfoHtml.OptionsColumn.AllowEdit = false;
            this.colBranchInfoHtml.OptionsColumn.ReadOnly = true;
            this.colBranchInfoHtml.Visible = true;
            this.colBranchInfoHtml.VisibleIndex = 0;
            this.colBranchInfoHtml.Width = 400;
            // 
            // ParentDepartmentNameTextEdit
            // 
            this.ParentDepartmentNameTextEdit.Location = new System.Drawing.Point(106, 50);
            this.ParentDepartmentNameTextEdit.MenuManager = this.barManager1;
            this.ParentDepartmentNameTextEdit.Name = "ParentDepartmentNameTextEdit";
            this.ParentDepartmentNameTextEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ParentDepartmentNameTextEdit.Properties.DataSource = this.departmentDtoBindingSource;
            this.ParentDepartmentNameTextEdit.Properties.DisplayMember = "DepartmentName";
            this.ParentDepartmentNameTextEdit.Properties.NullText = "";
            this.ParentDepartmentNameTextEdit.Properties.TreeList = this.treeListLookUpEdit1TreeList;
            this.ParentDepartmentNameTextEdit.Properties.ValueMember = "Id";
            this.ParentDepartmentNameTextEdit.Size = new System.Drawing.Size(356, 28);
            this.ParentDepartmentNameTextEdit.StyleController = this.dataLayoutControl1;
            this.ParentDepartmentNameTextEdit.TabIndex = 2;
            // 
            // departmentDtoBindingSource
            // 
            this.departmentDtoBindingSource.DataSource = typeof(DTO.MasterData.Company.DepartmentDto);
            // 
            // treeListLookUpEdit1TreeList
            // 
            this.treeListLookUpEdit1TreeList.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colParentDepartmentName,
            this.colDepartmentName});
            this.treeListLookUpEdit1TreeList.KeyFieldName = "Id";
            this.treeListLookUpEdit1TreeList.Location = new System.Drawing.Point(0, 0);
            this.treeListLookUpEdit1TreeList.Name = "treeListLookUpEdit1TreeList";
            this.treeListLookUpEdit1TreeList.OptionsView.ShowIndentAsRowStyle = true;
            this.treeListLookUpEdit1TreeList.ParentFieldName = "ParentId";
            this.treeListLookUpEdit1TreeList.Size = new System.Drawing.Size(400, 200);
            this.treeListLookUpEdit1TreeList.TabIndex = 0;
            // 
            // colParentDepartmentName
            // 
            this.colParentDepartmentName.Caption = "Phòng ban cấp trên";
            this.colParentDepartmentName.FieldName = "ParentDepartmentName";
            this.colParentDepartmentName.Name = "colParentDepartmentName";
            this.colParentDepartmentName.Visible = true;
            this.colParentDepartmentName.VisibleIndex = 0;
            // 
            // colDepartmentName
            // 
            this.colDepartmentName.Caption = "Tên phòng ban";
            this.colDepartmentName.FieldName = "DepartmentName";
            this.colDepartmentName.Name = "colDepartmentName";
            this.colDepartmentName.Visible = true;
            this.colDepartmentName.VisibleIndex = 1;
            // 
            // IsActiveToogleSwitch
            // 
            this.IsActiveToogleSwitch.EditValue = true;
            this.IsActiveToogleSwitch.Location = new System.Drawing.Point(295, 84);
            this.IsActiveToogleSwitch.MenuManager = this.barManager1;
            this.IsActiveToogleSwitch.Name = "IsActiveToogleSwitch";
            this.IsActiveToogleSwitch.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.IsActiveToogleSwitch.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.IsActiveToogleSwitch.Properties.OffText = "<color=\'red\'>Không hoạt động</color>";
            this.IsActiveToogleSwitch.Properties.OnText = "<color=\'blue\'>Đang hoạt động</color>";
            this.IsActiveToogleSwitch.Size = new System.Drawing.Size(167, 24);
            this.IsActiveToogleSwitch.StyleController = this.dataLayoutControl1;
            this.IsActiveToogleSwitch.TabIndex = 4;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(478, 219);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AllowDrawBackground = false;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForDepartmentCode,
            this.ItemForDepartmentName,
            this.ItemForDescription,
            this.ItemForIsActive,
            this.ItemForBranchName,
            this.ItemForParentDepartmentName});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "autoGeneratedGroup0";
            this.layoutControlGroup1.Size = new System.Drawing.Size(452, 193);
            // 
            // ItemForDepartmentCode
            // 
            this.ItemForDepartmentCode.Control = this.DepartmentCodeTextEdit;
            this.ItemForDepartmentCode.Location = new System.Drawing.Point(0, 68);
            this.ItemForDepartmentCode.Name = "ItemForDepartmentCode";
            this.ItemForDepartmentCode.Size = new System.Drawing.Size(225, 34);
            this.ItemForDepartmentCode.Text = "Mã phòng ban";
            this.ItemForDepartmentCode.TextSize = new System.Drawing.Size(74, 13);
            // 
            // ItemForDepartmentName
            // 
            this.ItemForDepartmentName.Control = this.DepartmentNameTextEdit;
            this.ItemForDepartmentName.Location = new System.Drawing.Point(0, 102);
            this.ItemForDepartmentName.Name = "ItemForDepartmentName";
            this.ItemForDepartmentName.Size = new System.Drawing.Size(452, 34);
            this.ItemForDepartmentName.Text = "Tên phòng ban";
            this.ItemForDepartmentName.TextSize = new System.Drawing.Size(74, 13);
            // 
            // ItemForDescription
            // 
            this.ItemForDescription.Control = this.DescriptionTextEdit;
            this.ItemForDescription.Location = new System.Drawing.Point(0, 136);
            this.ItemForDescription.Name = "ItemForDescription";
            this.ItemForDescription.Size = new System.Drawing.Size(452, 57);
            this.ItemForDescription.Text = "Mô tả";
            this.ItemForDescription.TextSize = new System.Drawing.Size(74, 13);
            // 
            // ItemForIsActive
            // 
            this.ItemForIsActive.Control = this.IsActiveToogleSwitch;
            this.ItemForIsActive.Location = new System.Drawing.Point(225, 68);
            this.ItemForIsActive.Name = "ItemForIsActive";
            this.ItemForIsActive.Size = new System.Drawing.Size(227, 34);
            this.ItemForIsActive.Text = "Trạng thái";
            this.ItemForIsActive.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.ItemForIsActive.TextSize = new System.Drawing.Size(49, 13);
            this.ItemForIsActive.TextToControlDistance = 5;
            // 
            // ItemForBranchName
            // 
            this.ItemForBranchName.Control = this.BranchNameSearchLookupedit;
            this.ItemForBranchName.Location = new System.Drawing.Point(0, 0);
            this.ItemForBranchName.Name = "ItemForBranchName";
            this.ItemForBranchName.Size = new System.Drawing.Size(452, 34);
            this.ItemForBranchName.Text = "Chọn chi nhánh";
            this.ItemForBranchName.TextSize = new System.Drawing.Size(74, 13);
            // 
            // ItemForParentDepartmentName
            // 
            this.ItemForParentDepartmentName.Control = this.ParentDepartmentNameTextEdit;
            this.ItemForParentDepartmentName.Location = new System.Drawing.Point(0, 34);
            this.ItemForParentDepartmentName.Name = "ItemForParentDepartmentName";
            this.ItemForParentDepartmentName.Size = new System.Drawing.Size(452, 34);
            this.ItemForParentDepartmentName.Text = "Phòng ban cha";
            this.ItemForParentDepartmentName.TextSize = new System.Drawing.Size(74, 13);
            // 
            // ChiNhanhInfoHypertextLabel
            // 
            this.ChiNhanhInfoHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.ChiNhanhInfoHypertextLabel.Name = "ChiNhanhInfoHypertextLabel";
            // 
            // FrmDepartmentDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 258);
            this.Controls.Add(this.dataLayoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmDepartmentDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DepartmentCodeTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepartmentNameTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BranchNameSearchLookupedit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.companyBranchLookupDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParentDepartmentNameTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.departmentDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListLookUpEdit1TreeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsActiveToogleSwitch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForBranchName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForParentDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChiNhanhInfoHypertextLabel)).EndInit();
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
        private TextEdit DepartmentCodeTextEdit;
        private TextEdit DepartmentNameTextEdit;
        private TextEdit DescriptionTextEdit;
        private LayoutControlGroup Root;
        private LayoutControlGroup layoutControlGroup1;
        private LayoutControlItem ItemForDepartmentCode;
        private LayoutControlItem ItemForDepartmentName;
        private LayoutControlItem ItemForDescription;
        private LayoutControlItem ItemForIsActive;
        private LayoutControlItem ItemForBranchName;
        private LayoutControlItem ItemForParentDepartmentName;
        private SearchLookUpEdit BranchNameSearchLookupedit;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
        private TreeListLookUpEdit ParentDepartmentNameTextEdit;
        private System.Windows.Forms.BindingSource departmentDtoBindingSource;
        private DevExpress.XtraTreeList.TreeList treeListLookUpEdit1TreeList;
        private ToggleSwitch IsActiveToogleSwitch;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colParentDepartmentName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colDepartmentName;
        
        private System.Windows.Forms.BindingSource companyBranchLookupDtoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colBranchInfoHtml;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel ChiNhanhInfoHypertextLabel;
    }
}