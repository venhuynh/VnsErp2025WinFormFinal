using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraLayout;

namespace MasterData.Company
{
    partial class FrmEmployeeDtoDetail
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
            this.EmployeeCodeTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.FullNameTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.BirthDateDateEdit = new DevExpress.XtraEditors.DateEdit();
            this.PhoneTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.EmailTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.HireDateDateEdit = new DevExpress.XtraEditors.DateEdit();
            this.ResignDateDateEdit = new DevExpress.XtraEditors.DateEdit();
            this.MobileTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.AvatarPictureEdit = new DevExpress.XtraEditors.PictureEdit();
            this.BranchIdSearchLookupEdit = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.companyBranchLookupDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colBranchInfoHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemHypertextLabel1 = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.DepartmentIdSearchLookupEdit = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.departmentLookupDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.DepartmentDtoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colDepartmentInfoHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DepartmentInforHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.PositionIdSearchLookupEdit = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.positionDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.PositionDtoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colThongTinHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PositionInfoHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.GenderComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.IsActiveToggleSwitch = new DevExpress.XtraEditors.ToggleSwitch();
            this.NotesTextEdit = new DevExpress.XtraEditors.MemoEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForBranchId = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForDepartmentId = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForPositionId = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForResignDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForMobile = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForNotes = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForAvatar = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForEmployeeCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForFullName = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForBirthDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForGender = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForPhone = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForHireDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForIsActive = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForEmail = new DevExpress.XtraLayout.LayoutControlItem();
            this.colParentDepartmentName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colDepartmentName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EmployeeCodeTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FullNameTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BirthDateDateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BirthDateDateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhoneTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmailTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HireDateDateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HireDateDateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResignDateDateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResignDateDateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MobileTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AvatarPictureEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BranchIdSearchLookupEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.companyBranchLookupDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHypertextLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepartmentIdSearchLookupEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.departmentLookupDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepartmentDtoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepartmentInforHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionIdSearchLookupEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.positionDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionDtoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionInfoHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GenderComboBoxEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsActiveToggleSwitch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotesTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForBranchId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDepartmentId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPositionId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForResignDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForMobile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForNotes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForAvatar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForEmployeeCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForFullName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForBirthDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForGender)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPhone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForHireDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsActive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForEmail)).BeginInit();
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
            this.barDockControlTop.Size = new System.Drawing.Size(683, 39);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 529);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(683, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 39);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 490);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(683, 39);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 490);
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
            // 
            // dataLayoutControl1
            // 
            this.dataLayoutControl1.Controls.Add(this.EmployeeCodeTextEdit);
            this.dataLayoutControl1.Controls.Add(this.FullNameTextEdit);
            this.dataLayoutControl1.Controls.Add(this.BirthDateDateEdit);
            this.dataLayoutControl1.Controls.Add(this.PhoneTextEdit);
            this.dataLayoutControl1.Controls.Add(this.EmailTextEdit);
            this.dataLayoutControl1.Controls.Add(this.HireDateDateEdit);
            this.dataLayoutControl1.Controls.Add(this.ResignDateDateEdit);
            this.dataLayoutControl1.Controls.Add(this.MobileTextEdit);
            this.dataLayoutControl1.Controls.Add(this.AvatarPictureEdit);
            this.dataLayoutControl1.Controls.Add(this.BranchIdSearchLookupEdit);
            this.dataLayoutControl1.Controls.Add(this.DepartmentIdSearchLookupEdit);
            this.dataLayoutControl1.Controls.Add(this.PositionIdSearchLookupEdit);
            this.dataLayoutControl1.Controls.Add(this.GenderComboBoxEdit);
            this.dataLayoutControl1.Controls.Add(this.IsActiveToggleSwitch);
            this.dataLayoutControl1.Controls.Add(this.NotesTextEdit);
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 39);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(683, 490);
            this.dataLayoutControl1.TabIndex = 9;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // EmployeeCodeTextEdit
            // 
            this.EmployeeCodeTextEdit.Location = new System.Drawing.Point(133, 16);
            this.EmployeeCodeTextEdit.MenuManager = this.barManager1;
            this.EmployeeCodeTextEdit.Name = "EmployeeCodeTextEdit";
            this.EmployeeCodeTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.EmployeeCodeTextEdit.Size = new System.Drawing.Size(58, 28);
            this.EmployeeCodeTextEdit.StyleController = this.dataLayoutControl1;
            this.EmployeeCodeTextEdit.TabIndex = 4;
            // 
            // FullNameTextEdit
            // 
            this.FullNameTextEdit.Location = new System.Drawing.Point(133, 50);
            this.FullNameTextEdit.MenuManager = this.barManager1;
            this.FullNameTextEdit.Name = "FullNameTextEdit";
            this.FullNameTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.FullNameTextEdit.Size = new System.Drawing.Size(301, 28);
            this.FullNameTextEdit.StyleController = this.dataLayoutControl1;
            this.FullNameTextEdit.TabIndex = 5;
            // 
            // BirthDateDateEdit
            // 
            this.BirthDateDateEdit.EditValue = null;
            this.BirthDateDateEdit.Location = new System.Drawing.Point(133, 118);
            this.BirthDateDateEdit.MenuManager = this.barManager1;
            this.BirthDateDateEdit.Name = "BirthDateDateEdit";
            this.BirthDateDateEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.BirthDateDateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.BirthDateDateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.BirthDateDateEdit.Size = new System.Drawing.Size(301, 28);
            this.BirthDateDateEdit.StyleController = this.dataLayoutControl1;
            this.BirthDateDateEdit.TabIndex = 7;
            // 
            // PhoneTextEdit
            // 
            this.PhoneTextEdit.Location = new System.Drawing.Point(133, 152);
            this.PhoneTextEdit.MenuManager = this.barManager1;
            this.PhoneTextEdit.Name = "PhoneTextEdit";
            this.PhoneTextEdit.Size = new System.Drawing.Size(301, 28);
            this.PhoneTextEdit.StyleController = this.dataLayoutControl1;
            this.PhoneTextEdit.TabIndex = 8;
            // 
            // EmailTextEdit
            // 
            this.EmailTextEdit.Location = new System.Drawing.Point(461, 322);
            this.EmailTextEdit.MenuManager = this.barManager1;
            this.EmailTextEdit.Name = "EmailTextEdit";
            this.EmailTextEdit.Size = new System.Drawing.Size(206, 28);
            this.EmailTextEdit.StyleController = this.dataLayoutControl1;
            this.EmailTextEdit.TabIndex = 9;
            // 
            // HireDateDateEdit
            // 
            this.HireDateDateEdit.EditValue = null;
            this.HireDateDateEdit.Location = new System.Drawing.Point(133, 288);
            this.HireDateDateEdit.MenuManager = this.barManager1;
            this.HireDateDateEdit.Name = "HireDateDateEdit";
            this.HireDateDateEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.HireDateDateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.HireDateDateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.HireDateDateEdit.Size = new System.Drawing.Size(205, 28);
            this.HireDateDateEdit.StyleController = this.dataLayoutControl1;
            this.HireDateDateEdit.TabIndex = 10;
            // 
            // ResignDateDateEdit
            // 
            this.ResignDateDateEdit.EditValue = null;
            this.ResignDateDateEdit.Location = new System.Drawing.Point(461, 288);
            this.ResignDateDateEdit.MenuManager = this.barManager1;
            this.ResignDateDateEdit.Name = "ResignDateDateEdit";
            this.ResignDateDateEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.ResignDateDateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ResignDateDateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ResignDateDateEdit.Size = new System.Drawing.Size(206, 28);
            this.ResignDateDateEdit.StyleController = this.dataLayoutControl1;
            this.ResignDateDateEdit.TabIndex = 12;
            // 
            // MobileTextEdit
            // 
            this.MobileTextEdit.Location = new System.Drawing.Point(133, 322);
            this.MobileTextEdit.MenuManager = this.barManager1;
            this.MobileTextEdit.Name = "MobileTextEdit";
            this.MobileTextEdit.Size = new System.Drawing.Size(205, 28);
            this.MobileTextEdit.StyleController = this.dataLayoutControl1;
            this.MobileTextEdit.TabIndex = 13;
            // 
            // AvatarPictureEdit
            // 
            this.AvatarPictureEdit.Location = new System.Drawing.Point(440, 35);
            this.AvatarPictureEdit.MenuManager = this.barManager1;
            this.AvatarPictureEdit.Name = "AvatarPictureEdit";
            this.AvatarPictureEdit.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            this.AvatarPictureEdit.Size = new System.Drawing.Size(227, 247);
            this.AvatarPictureEdit.StyleController = this.dataLayoutControl1;
            this.AvatarPictureEdit.TabIndex = 1;
            // 
            // BranchIdSearchLookupEdit
            // 
            this.BranchIdSearchLookupEdit.Location = new System.Drawing.Point(133, 186);
            this.BranchIdSearchLookupEdit.MenuManager = this.barManager1;
            this.BranchIdSearchLookupEdit.Name = "BranchIdSearchLookupEdit";
            this.BranchIdSearchLookupEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.BranchIdSearchLookupEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.BranchIdSearchLookupEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.BranchIdSearchLookupEdit.Properties.DataSource = this.companyBranchLookupDtoBindingSource;
            this.BranchIdSearchLookupEdit.Properties.DisplayMember = "BranchInfoHtml";
            this.BranchIdSearchLookupEdit.Properties.NullText = "";
            this.BranchIdSearchLookupEdit.Properties.PopupView = this.searchLookUpEdit1View;
            this.BranchIdSearchLookupEdit.Properties.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemHypertextLabel1});
            this.BranchIdSearchLookupEdit.Properties.ValueMember = "Id";
            this.BranchIdSearchLookupEdit.Size = new System.Drawing.Size(301, 28);
            this.BranchIdSearchLookupEdit.StyleController = this.dataLayoutControl1;
            this.BranchIdSearchLookupEdit.TabIndex = 0;
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
            this.colBranchInfoHtml.ColumnEdit = this.repositoryItemHypertextLabel1;
            this.colBranchInfoHtml.FieldName = "BranchInfoHtml";
            this.colBranchInfoHtml.Name = "colBranchInfoHtml";
            this.colBranchInfoHtml.OptionsColumn.AllowEdit = false;
            this.colBranchInfoHtml.OptionsColumn.ReadOnly = true;
            this.colBranchInfoHtml.Visible = true;
            this.colBranchInfoHtml.VisibleIndex = 0;
            this.colBranchInfoHtml.Width = 400;
            // 
            // repositoryItemHypertextLabel1
            // 
            this.repositoryItemHypertextLabel1.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.repositoryItemHypertextLabel1.Name = "repositoryItemHypertextLabel1";
            // 
            // DepartmentIdSearchLookupEdit
            // 
            this.DepartmentIdSearchLookupEdit.Location = new System.Drawing.Point(133, 220);
            this.DepartmentIdSearchLookupEdit.MenuManager = this.barManager1;
            this.DepartmentIdSearchLookupEdit.Name = "DepartmentIdSearchLookupEdit";
            this.DepartmentIdSearchLookupEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.DepartmentIdSearchLookupEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.DepartmentIdSearchLookupEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DepartmentIdSearchLookupEdit.Properties.DataSource = this.departmentLookupDtoBindingSource;
            this.DepartmentIdSearchLookupEdit.Properties.DisplayMember = "DepartmentInfoHtml";
            this.DepartmentIdSearchLookupEdit.Properties.NullText = "";
            this.DepartmentIdSearchLookupEdit.Properties.PopupView = this.DepartmentDtoGridView;
            this.DepartmentIdSearchLookupEdit.Properties.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.DepartmentInforHypertextLabel});
            this.DepartmentIdSearchLookupEdit.Properties.ValueMember = "Id";
            this.DepartmentIdSearchLookupEdit.Size = new System.Drawing.Size(301, 28);
            this.DepartmentIdSearchLookupEdit.StyleController = this.dataLayoutControl1;
            this.DepartmentIdSearchLookupEdit.TabIndex = 2;
            // 
            // departmentLookupDtoBindingSource
            // 
            this.departmentLookupDtoBindingSource.DataSource = typeof(DTO.MasterData.Company.DepartmentLookupDto);
            // 
            // DepartmentDtoGridView
            // 
            this.DepartmentDtoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colDepartmentInfoHtml});
            this.DepartmentDtoGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.DepartmentDtoGridView.Name = "DepartmentDtoGridView";
            this.DepartmentDtoGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.DepartmentDtoGridView.OptionsView.RowAutoHeight = true;
            this.DepartmentDtoGridView.OptionsView.ShowGroupPanel = false;
            // 
            // colDepartmentInfoHtml
            // 
            this.colDepartmentInfoHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colDepartmentInfoHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colDepartmentInfoHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colDepartmentInfoHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colDepartmentInfoHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colDepartmentInfoHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colDepartmentInfoHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colDepartmentInfoHtml.AppearanceHeader.Options.UseFont = true;
            this.colDepartmentInfoHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colDepartmentInfoHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colDepartmentInfoHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDepartmentInfoHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colDepartmentInfoHtml.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colDepartmentInfoHtml.Caption = "Thông tin phòng ban";
            this.colDepartmentInfoHtml.ColumnEdit = this.DepartmentInforHypertextLabel;
            this.colDepartmentInfoHtml.FieldName = "DepartmentInfoHtml";
            this.colDepartmentInfoHtml.Name = "colDepartmentInfoHtml";
            this.colDepartmentInfoHtml.OptionsColumn.AllowEdit = false;
            this.colDepartmentInfoHtml.OptionsColumn.ReadOnly = true;
            this.colDepartmentInfoHtml.Visible = true;
            this.colDepartmentInfoHtml.VisibleIndex = 0;
            this.colDepartmentInfoHtml.Width = 400;
            // 
            // DepartmentInforHypertextLabel
            // 
            this.DepartmentInforHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.DepartmentInforHypertextLabel.Name = "DepartmentInforHypertextLabel";
            // 
            // PositionIdSearchLookupEdit
            // 
            this.PositionIdSearchLookupEdit.Location = new System.Drawing.Point(133, 254);
            this.PositionIdSearchLookupEdit.MenuManager = this.barManager1;
            this.PositionIdSearchLookupEdit.Name = "PositionIdSearchLookupEdit";
            this.PositionIdSearchLookupEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.PositionIdSearchLookupEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.PositionIdSearchLookupEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.PositionIdSearchLookupEdit.Properties.DataSource = this.positionDtoBindingSource;
            this.PositionIdSearchLookupEdit.Properties.DisplayMember = "ThongTinHtml";
            this.PositionIdSearchLookupEdit.Properties.NullText = "";
            this.PositionIdSearchLookupEdit.Properties.PopupView = this.PositionDtoGridView;
            this.PositionIdSearchLookupEdit.Properties.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.PositionInfoHypertextLabel});
            this.PositionIdSearchLookupEdit.Properties.ValueMember = "Id";
            this.PositionIdSearchLookupEdit.Size = new System.Drawing.Size(301, 28);
            this.PositionIdSearchLookupEdit.StyleController = this.dataLayoutControl1;
            this.PositionIdSearchLookupEdit.TabIndex = 3;
            // 
            // positionDtoBindingSource
            // 
            this.positionDtoBindingSource.DataSource = typeof(DTO.MasterData.Company.PositionDto);
            // 
            // PositionDtoGridView
            // 
            this.PositionDtoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colThongTinHtml});
            this.PositionDtoGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.PositionDtoGridView.Name = "PositionDtoGridView";
            this.PositionDtoGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.PositionDtoGridView.OptionsView.RowAutoHeight = true;
            this.PositionDtoGridView.OptionsView.ShowGroupPanel = false;
            // 
            // colThongTinHtml
            // 
            this.colThongTinHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colThongTinHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colThongTinHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colThongTinHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colThongTinHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colThongTinHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colThongTinHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colThongTinHtml.AppearanceHeader.Options.UseFont = true;
            this.colThongTinHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colThongTinHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colThongTinHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colThongTinHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colThongTinHtml.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colThongTinHtml.Caption = "Thông tin chức vụ";
            this.colThongTinHtml.ColumnEdit = this.PositionInfoHypertextLabel;
            this.colThongTinHtml.FieldName = "ThongTinHtml";
            this.colThongTinHtml.Name = "colThongTinHtml";
            this.colThongTinHtml.OptionsColumn.AllowEdit = false;
            this.colThongTinHtml.OptionsColumn.ReadOnly = true;
            this.colThongTinHtml.Visible = true;
            this.colThongTinHtml.VisibleIndex = 0;
            this.colThongTinHtml.Width = 400;
            // 
            // PositionInfoHypertextLabel
            // 
            this.PositionInfoHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.PositionInfoHypertextLabel.Name = "PositionInfoHypertextLabel";
            // 
            // GenderComboBoxEdit
            // 
            this.GenderComboBoxEdit.Location = new System.Drawing.Point(133, 84);
            this.GenderComboBoxEdit.MenuManager = this.barManager1;
            this.GenderComboBoxEdit.Name = "GenderComboBoxEdit";
            this.GenderComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.GenderComboBoxEdit.Size = new System.Drawing.Size(301, 28);
            this.GenderComboBoxEdit.StyleController = this.dataLayoutControl1;
            this.GenderComboBoxEdit.TabIndex = 6;
            // 
            // IsActiveToggleSwitch
            // 
            this.IsActiveToggleSwitch.EditValue = true;
            this.IsActiveToggleSwitch.Location = new System.Drawing.Point(314, 16);
            this.IsActiveToggleSwitch.MenuManager = this.barManager1;
            this.IsActiveToggleSwitch.Name = "IsActiveToggleSwitch";
            this.IsActiveToggleSwitch.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.IsActiveToggleSwitch.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.IsActiveToggleSwitch.Properties.OffText = "<color=\'red\'>Đã nghĩ việc</color>";
            this.IsActiveToggleSwitch.Properties.OnText = "<color=\'blue\'>Đang làm việc</color>";
            this.IsActiveToggleSwitch.Size = new System.Drawing.Size(120, 24);
            this.IsActiveToggleSwitch.StyleController = this.dataLayoutControl1;
            this.IsActiveToggleSwitch.TabIndex = 11;
            // 
            // NotesTextEdit
            // 
            this.NotesTextEdit.Location = new System.Drawing.Point(133, 356);
            this.NotesTextEdit.MenuManager = this.barManager1;
            this.NotesTextEdit.Name = "NotesTextEdit";
            this.NotesTextEdit.Size = new System.Drawing.Size(534, 118);
            this.NotesTextEdit.StyleController = this.dataLayoutControl1;
            this.NotesTextEdit.TabIndex = 14;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(683, 490);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AllowDrawBackground = false;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForBranchId,
            this.ItemForDepartmentId,
            this.ItemForPositionId,
            this.ItemForResignDate,
            this.ItemForMobile,
            this.ItemForNotes,
            this.ItemForAvatar,
            this.ItemForEmployeeCode,
            this.ItemForFullName,
            this.ItemForBirthDate,
            this.ItemForGender,
            this.ItemForPhone,
            this.ItemForHireDate,
            this.ItemForIsActive,
            this.ItemForEmail});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "autoGeneratedGroup0";
            this.layoutControlGroup1.Size = new System.Drawing.Size(657, 464);
            // 
            // ItemForBranchId
            // 
            this.ItemForBranchId.Control = this.BranchIdSearchLookupEdit;
            this.ItemForBranchId.Location = new System.Drawing.Point(0, 170);
            this.ItemForBranchId.Name = "ItemForBranchId";
            this.ItemForBranchId.Size = new System.Drawing.Size(424, 34);
            this.ItemForBranchId.Text = "Chi nhánh";
            this.ItemForBranchId.TextSize = new System.Drawing.Size(101, 13);
            // 
            // ItemForDepartmentId
            // 
            this.ItemForDepartmentId.Control = this.DepartmentIdSearchLookupEdit;
            this.ItemForDepartmentId.Location = new System.Drawing.Point(0, 204);
            this.ItemForDepartmentId.Name = "ItemForDepartmentId";
            this.ItemForDepartmentId.Size = new System.Drawing.Size(424, 34);
            this.ItemForDepartmentId.Text = "Phòng ban";
            this.ItemForDepartmentId.TextSize = new System.Drawing.Size(101, 13);
            // 
            // ItemForPositionId
            // 
            this.ItemForPositionId.Control = this.PositionIdSearchLookupEdit;
            this.ItemForPositionId.Location = new System.Drawing.Point(0, 238);
            this.ItemForPositionId.Name = "ItemForPositionId";
            this.ItemForPositionId.Size = new System.Drawing.Size(424, 34);
            this.ItemForPositionId.Text = "Chức vụ";
            this.ItemForPositionId.TextSize = new System.Drawing.Size(101, 13);
            // 
            // ItemForResignDate
            // 
            this.ItemForResignDate.Control = this.ResignDateDateEdit;
            this.ItemForResignDate.Location = new System.Drawing.Point(328, 272);
            this.ItemForResignDate.Name = "ItemForResignDate";
            this.ItemForResignDate.Size = new System.Drawing.Size(329, 34);
            this.ItemForResignDate.Text = "Ngày nghỉ việc";
            this.ItemForResignDate.TextSize = new System.Drawing.Size(101, 13);
            // 
            // ItemForMobile
            // 
            this.ItemForMobile.Control = this.MobileTextEdit;
            this.ItemForMobile.Location = new System.Drawing.Point(0, 306);
            this.ItemForMobile.Name = "ItemForMobile";
            this.ItemForMobile.Size = new System.Drawing.Size(328, 34);
            this.ItemForMobile.Text = "Số điện thoại di động";
            this.ItemForMobile.TextSize = new System.Drawing.Size(101, 13);
            // 
            // ItemForNotes
            // 
            this.ItemForNotes.Control = this.NotesTextEdit;
            this.ItemForNotes.Location = new System.Drawing.Point(0, 340);
            this.ItemForNotes.Name = "ItemForNotes";
            this.ItemForNotes.Size = new System.Drawing.Size(657, 124);
            this.ItemForNotes.Text = "Ghi chú";
            this.ItemForNotes.TextSize = new System.Drawing.Size(101, 13);
            // 
            // ItemForAvatar
            // 
            this.ItemForAvatar.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForAvatar.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ItemForAvatar.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForAvatar.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ItemForAvatar.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForAvatar.Control = this.AvatarPictureEdit;
            this.ItemForAvatar.Location = new System.Drawing.Point(424, 0);
            this.ItemForAvatar.Name = "ItemForAvatar";
            this.ItemForAvatar.Size = new System.Drawing.Size(233, 272);
            this.ItemForAvatar.StartNewLine = true;
            this.ItemForAvatar.Text = "Ảnh đại diện";
            this.ItemForAvatar.TextLocation = DevExpress.Utils.Locations.Top;
            this.ItemForAvatar.TextSize = new System.Drawing.Size(101, 13);
            // 
            // ItemForEmployeeCode
            // 
            this.ItemForEmployeeCode.Control = this.EmployeeCodeTextEdit;
            this.ItemForEmployeeCode.Location = new System.Drawing.Point(0, 0);
            this.ItemForEmployeeCode.Name = "ItemForEmployeeCode";
            this.ItemForEmployeeCode.Size = new System.Drawing.Size(181, 34);
            this.ItemForEmployeeCode.Text = "Mã nhân viên";
            this.ItemForEmployeeCode.TextSize = new System.Drawing.Size(101, 13);
            // 
            // ItemForFullName
            // 
            this.ItemForFullName.Control = this.FullNameTextEdit;
            this.ItemForFullName.Location = new System.Drawing.Point(0, 34);
            this.ItemForFullName.Name = "ItemForFullName";
            this.ItemForFullName.Size = new System.Drawing.Size(424, 34);
            this.ItemForFullName.Text = "Họ và tên";
            this.ItemForFullName.TextSize = new System.Drawing.Size(101, 13);
            // 
            // ItemForBirthDate
            // 
            this.ItemForBirthDate.Control = this.BirthDateDateEdit;
            this.ItemForBirthDate.Location = new System.Drawing.Point(0, 102);
            this.ItemForBirthDate.Name = "ItemForBirthDate";
            this.ItemForBirthDate.Size = new System.Drawing.Size(424, 34);
            this.ItemForBirthDate.Text = "Ngày sinh";
            this.ItemForBirthDate.TextSize = new System.Drawing.Size(101, 13);
            // 
            // ItemForGender
            // 
            this.ItemForGender.Control = this.GenderComboBoxEdit;
            this.ItemForGender.Location = new System.Drawing.Point(0, 68);
            this.ItemForGender.Name = "ItemForGender";
            this.ItemForGender.Size = new System.Drawing.Size(424, 34);
            this.ItemForGender.Text = "Giới tính";
            this.ItemForGender.TextSize = new System.Drawing.Size(101, 13);
            // 
            // ItemForPhone
            // 
            this.ItemForPhone.Control = this.PhoneTextEdit;
            this.ItemForPhone.Location = new System.Drawing.Point(0, 136);
            this.ItemForPhone.Name = "ItemForPhone";
            this.ItemForPhone.Size = new System.Drawing.Size(424, 34);
            this.ItemForPhone.Text = "Số điện thoại";
            this.ItemForPhone.TextSize = new System.Drawing.Size(101, 13);
            // 
            // ItemForHireDate
            // 
            this.ItemForHireDate.Control = this.HireDateDateEdit;
            this.ItemForHireDate.Location = new System.Drawing.Point(0, 272);
            this.ItemForHireDate.Name = "ItemForHireDate";
            this.ItemForHireDate.Size = new System.Drawing.Size(328, 34);
            this.ItemForHireDate.Text = "Ngày vào làm";
            this.ItemForHireDate.TextSize = new System.Drawing.Size(101, 13);
            // 
            // ItemForIsActive
            // 
            this.ItemForIsActive.Control = this.IsActiveToggleSwitch;
            this.ItemForIsActive.Location = new System.Drawing.Point(181, 0);
            this.ItemForIsActive.Name = "ItemForIsActive";
            this.ItemForIsActive.Size = new System.Drawing.Size(243, 34);
            this.ItemForIsActive.Text = "Trạng thái hoạt động";
            this.ItemForIsActive.TextSize = new System.Drawing.Size(101, 13);
            // 
            // ItemForEmail
            // 
            this.ItemForEmail.Control = this.EmailTextEdit;
            this.ItemForEmail.Location = new System.Drawing.Point(328, 306);
            this.ItemForEmail.Name = "ItemForEmail";
            this.ItemForEmail.Size = new System.Drawing.Size(329, 34);
            this.ItemForEmail.Text = "Email";
            this.ItemForEmail.TextSize = new System.Drawing.Size(101, 13);
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
            // FrmEmployeeDtoDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 529);
            this.Controls.Add(this.dataLayoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmEmployeeDtoDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.EmployeeCodeTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FullNameTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BirthDateDateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BirthDateDateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhoneTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmailTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HireDateDateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HireDateDateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResignDateDateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResignDateDateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MobileTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AvatarPictureEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BranchIdSearchLookupEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.companyBranchLookupDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHypertextLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepartmentIdSearchLookupEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.departmentLookupDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepartmentDtoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepartmentInforHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionIdSearchLookupEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.positionDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionDtoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionInfoHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GenderComboBoxEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsActiveToggleSwitch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotesTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForBranchId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDepartmentId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPositionId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForResignDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForMobile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForNotes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForAvatar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForEmployeeCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForFullName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForBirthDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForGender)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPhone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForHireDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForEmail)).EndInit();
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
        private DevExpress.XtraTreeList.Columns.TreeListColumn colParentDepartmentName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colDepartmentName;
        private TextEdit EmployeeCodeTextEdit;
        private TextEdit FullNameTextEdit;
        private DateEdit BirthDateDateEdit;
        private TextEdit PhoneTextEdit;
        private TextEdit EmailTextEdit;
        private DateEdit HireDateDateEdit;
        private DateEdit ResignDateDateEdit;
        private TextEdit MobileTextEdit;
        private PictureEdit AvatarPictureEdit;
        private SearchLookUpEdit BranchIdSearchLookupEdit;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
        private SearchLookUpEdit DepartmentIdSearchLookupEdit;
        private DevExpress.XtraGrid.Views.Grid.GridView DepartmentDtoGridView;
        private SearchLookUpEdit PositionIdSearchLookupEdit;
        private DevExpress.XtraGrid.Views.Grid.GridView PositionDtoGridView;
        private ComboBoxEdit GenderComboBoxEdit;
        private ToggleSwitch IsActiveToggleSwitch;
        private LayoutControlItem ItemForBranchId;
        private LayoutControlItem ItemForDepartmentId;
        private LayoutControlItem ItemForPositionId;
        private LayoutControlItem ItemForEmployeeCode;
        private LayoutControlItem ItemForFullName;
        private LayoutControlItem ItemForGender;
        private LayoutControlItem ItemForBirthDate;
        private LayoutControlItem ItemForPhone;
        private LayoutControlItem ItemForEmail;
        private LayoutControlItem ItemForHireDate;
        private LayoutControlItem ItemForResignDate;
        private LayoutControlItem ItemForMobile;
        private LayoutControlItem ItemForNotes;
        private LayoutControlItem ItemForAvatar;
        private LayoutControlItem ItemForIsActive;
        private MemoEdit NotesTextEdit;
        private System.Windows.Forms.BindingSource companyBranchLookupDtoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colBranchInfoHtml;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel repositoryItemHypertextLabel1;
        private System.Windows.Forms.BindingSource departmentLookupDtoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colDepartmentInfoHtml;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel DepartmentInforHypertextLabel;
        
        private System.Windows.Forms.BindingSource positionDtoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colThongTinHtml;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel PositionInfoHypertextLabel;
    }
}