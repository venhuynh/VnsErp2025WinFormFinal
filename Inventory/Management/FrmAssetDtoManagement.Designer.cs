namespace Inventory.Management
{
    partial class FrmAssetDtoManagement
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.assetDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.XemBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ThemMoiBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.SuaBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.XoaBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ExportFileBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barHeaderItem1 = new DevExpress.XtraBars.BarHeaderItem();
            this.DataSummaryBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.SelectedRowBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.AssetDtoGridViewGridControl = new DevExpress.XtraGrid.GridControl();
            this.AssetDtoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAssetCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAssetName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAssetTypeDisplay = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAssetCategoryDisplay = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCompanyName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBranchName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDepartmentName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAssignedEmployeeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLocation = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPurchasePrice = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPurchaseDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAccumulatedDepreciation = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCurrentValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatusDisplay = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colConditionDisplay = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWarrantyName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWarrantyExpiryDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSerialNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colManufacturer = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colModel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCreateDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCreatedByName = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.assetDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AssetDtoGridViewGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AssetDtoGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // assetDtoBindingSource
            // 
            this.assetDtoBindingSource.DataSource = typeof(DTO.Inventory.InventoryManagement.AssetDto);
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2,
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.XemBarButtonItem,
            this.ThemMoiBarButtonItem,
            this.SuaBarButtonItem,
            this.XoaBarButtonItem,
            this.ExportFileBarButtonItem,
            this.barHeaderItem1,
            this.DataSummaryBarStaticItem,
            this.SelectedRowBarStaticItem});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 9;
            this.barManager1.StatusBar = this.bar1;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.XemBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.ThemMoiBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.SuaBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.XoaBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.ExportFileBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // XemBarButtonItem
            // 
            this.XemBarButtonItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.XemBarButtonItem.Caption = "Xem";
            this.XemBarButtonItem.Id = 1;
            this.XemBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.listnumbers_16x16;
            this.XemBarButtonItem.ImageOptions.LargeImage = global::Inventory.Properties.Resources.listnumbers_32x32;
            this.XemBarButtonItem.Name = "XemBarButtonItem";
            // 
            // ThemMoiBarButtonItem
            // 
            this.ThemMoiBarButtonItem.Caption = "Thêm mới";
            this.ThemMoiBarButtonItem.Id = 2;
            this.ThemMoiBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.add_16x16;
            this.ThemMoiBarButtonItem.Name = "ThemMoiBarButtonItem";
            // 
            // SuaBarButtonItem
            // 
            this.SuaBarButtonItem.Caption = "Sửa";
            this.SuaBarButtonItem.Id = 3;
            this.SuaBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.edit_16x16;
            this.SuaBarButtonItem.ImageOptions.LargeImage = global::Inventory.Properties.Resources.edit_32x32;
            this.SuaBarButtonItem.Name = "SuaBarButtonItem";
            // 
            // XoaBarButtonItem
            // 
            this.XoaBarButtonItem.Caption = "Xóa";
            this.XoaBarButtonItem.Id = 4;
            this.XoaBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.clear_16x16;
            this.XoaBarButtonItem.Name = "XoaBarButtonItem";
            // 
            // ExportFileBarButtonItem
            // 
            this.ExportFileBarButtonItem.Caption = "Xuất file";
            this.ExportFileBarButtonItem.Id = 5;
            this.ExportFileBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.exporttoxps_16x16;
            this.ExportFileBarButtonItem.Name = "ExportFileBarButtonItem";
            // 
            // bar1
            // 
            this.bar1.BarName = "Custom 3";
            this.bar1.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barHeaderItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.DataSummaryBarStaticItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.SelectedRowBarStaticItem)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Custom 3";
            // 
            // barHeaderItem1
            // 
            this.barHeaderItem1.Caption = "Tổng kết";
            this.barHeaderItem1.Id = 6;
            this.barHeaderItem1.Name = "barHeaderItem1";
            // 
            // DataSummaryBarStaticItem
            // 
            this.DataSummaryBarStaticItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.DataSummaryBarStaticItem.Caption = "Chưa có dữ liệu";
            this.DataSummaryBarStaticItem.Id = 7;
            this.DataSummaryBarStaticItem.Name = "DataSummaryBarStaticItem";
            // 
            // SelectedRowBarStaticItem
            // 
            this.SelectedRowBarStaticItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.SelectedRowBarStaticItem.Caption = "Chưa chọn dòng nào";
            this.SelectedRowBarStaticItem.Id = 8;
            this.SelectedRowBarStaticItem.Name = "SelectedRowBarStaticItem";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1525, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 510);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1525, 22);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 486);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1525, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 486);
            // 
            // AssetDtoGridViewGridControl
            // 
            this.AssetDtoGridViewGridControl.DataSource = this.assetDtoBindingSource;
            this.AssetDtoGridViewGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AssetDtoGridViewGridControl.Location = new System.Drawing.Point(0, 24);
            this.AssetDtoGridViewGridControl.MainView = this.AssetDtoGridView;
            this.AssetDtoGridViewGridControl.MenuManager = this.barManager1;
            this.AssetDtoGridViewGridControl.Name = "AssetDtoGridViewGridControl";
            this.AssetDtoGridViewGridControl.Size = new System.Drawing.Size(1525, 486);
            this.AssetDtoGridViewGridControl.TabIndex = 5;
            this.AssetDtoGridViewGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.AssetDtoGridView});
            // 
            // AssetDtoGridView
            // 
            this.AssetDtoGridView.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.AssetDtoGridView.Appearance.ViewCaption.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.AssetDtoGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.AssetDtoGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.AssetDtoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colId,
            this.colAssetCode,
            this.colAssetName,
            this.colAssetTypeDisplay,
            this.colAssetCategoryDisplay,
            this.colCompanyName,
            this.colBranchName,
            this.colDepartmentName,
            this.colAssignedEmployeeName,
            this.colLocation,
            this.colPurchasePrice,
            this.colPurchaseDate,
            this.colAccumulatedDepreciation,
            this.colCurrentValue,
            this.colStatusDisplay,
            this.colConditionDisplay,
            this.colWarrantyName,
            this.colWarrantyExpiryDate,
            this.colSerialNumber,
            this.colManufacturer,
            this.colModel,
            this.colCreateDate,
            this.colCreatedByName});
            this.AssetDtoGridView.GridControl = this.AssetDtoGridViewGridControl;
            this.AssetDtoGridView.Name = "AssetDtoGridView";
            this.AssetDtoGridView.OptionsBehavior.Editable = false;
            this.AssetDtoGridView.OptionsSelection.MultiSelect = true;
            this.AssetDtoGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.AssetDtoGridView.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.AssetDtoGridView.OptionsView.RowAutoHeight = true;
            this.AssetDtoGridView.OptionsView.ShowGroupPanel = false;
            this.AssetDtoGridView.OptionsView.ShowViewCaption = true;
            this.AssetDtoGridView.ViewCaption = "QUẢN LÝ TÀI SẢN";
            // 
            // colId
            // 
            this.colId.FieldName = "Id";
            this.colId.Name = "colId";
            this.colId.OptionsColumn.AllowEdit = false;
            this.colId.OptionsColumn.ReadOnly = true;
            // 
            // colAssetCode
            // 
            this.colAssetCode.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colAssetCode.AppearanceCell.ForeColor = System.Drawing.Color.DarkBlue;
            this.colAssetCode.AppearanceCell.Options.UseFont = true;
            this.colAssetCode.AppearanceCell.Options.UseForeColor = true;
            this.colAssetCode.AppearanceCell.Options.UseTextOptions = true;
            this.colAssetCode.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colAssetCode.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colAssetCode.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.colAssetCode.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colAssetCode.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.colAssetCode.AppearanceHeader.Options.UseBackColor = true;
            this.colAssetCode.AppearanceHeader.Options.UseFont = true;
            this.colAssetCode.AppearanceHeader.Options.UseForeColor = true;
            this.colAssetCode.AppearanceHeader.Options.UseTextOptions = true;
            this.colAssetCode.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colAssetCode.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colAssetCode.Caption = "Mã tài sản";
            this.colAssetCode.FieldName = "AssetCode";
            this.colAssetCode.Name = "colAssetCode";
            this.colAssetCode.Visible = true;
            this.colAssetCode.VisibleIndex = 1;
            this.colAssetCode.Width = 120;
            // 
            // colAssetName
            // 
            this.colAssetName.AppearanceCell.Options.UseTextOptions = true;
            this.colAssetName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colAssetName.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colAssetName.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.colAssetName.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colAssetName.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.colAssetName.AppearanceHeader.Options.UseBackColor = true;
            this.colAssetName.AppearanceHeader.Options.UseFont = true;
            this.colAssetName.AppearanceHeader.Options.UseForeColor = true;
            this.colAssetName.AppearanceHeader.Options.UseTextOptions = true;
            this.colAssetName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colAssetName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colAssetName.Caption = "Tên tài sản";
            this.colAssetName.FieldName = "AssetName";
            this.colAssetName.Name = "colAssetName";
            this.colAssetName.Visible = true;
            this.colAssetName.VisibleIndex = 2;
            this.colAssetName.Width = 250;
            // 
            // colAssetTypeDisplay
            // 
            this.colAssetTypeDisplay.AppearanceCell.Options.UseTextOptions = true;
            this.colAssetTypeDisplay.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colAssetTypeDisplay.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colAssetTypeDisplay.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.colAssetTypeDisplay.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colAssetTypeDisplay.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.colAssetTypeDisplay.AppearanceHeader.Options.UseBackColor = true;
            this.colAssetTypeDisplay.AppearanceHeader.Options.UseFont = true;
            this.colAssetTypeDisplay.AppearanceHeader.Options.UseForeColor = true;
            this.colAssetTypeDisplay.AppearanceHeader.Options.UseTextOptions = true;
            this.colAssetTypeDisplay.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colAssetTypeDisplay.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colAssetTypeDisplay.Caption = "Loại tài sản";
            this.colAssetTypeDisplay.FieldName = "AssetTypeDisplay";
            this.colAssetTypeDisplay.Name = "colAssetTypeDisplay";
            this.colAssetTypeDisplay.Visible = true;
            this.colAssetTypeDisplay.VisibleIndex = 3;
            this.colAssetTypeDisplay.Width = 120;
            // 
            // colAssetCategoryDisplay
            // 
            this.colAssetCategoryDisplay.AppearanceCell.Options.UseTextOptions = true;
            this.colAssetCategoryDisplay.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colAssetCategoryDisplay.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colAssetCategoryDisplay.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.colAssetCategoryDisplay.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colAssetCategoryDisplay.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.colAssetCategoryDisplay.AppearanceHeader.Options.UseBackColor = true;
            this.colAssetCategoryDisplay.AppearanceHeader.Options.UseFont = true;
            this.colAssetCategoryDisplay.AppearanceHeader.Options.UseForeColor = true;
            this.colAssetCategoryDisplay.AppearanceHeader.Options.UseTextOptions = true;
            this.colAssetCategoryDisplay.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colAssetCategoryDisplay.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colAssetCategoryDisplay.Caption = "Danh mục";
            this.colAssetCategoryDisplay.FieldName = "AssetCategoryDisplay";
            this.colAssetCategoryDisplay.Name = "colAssetCategoryDisplay";
            this.colAssetCategoryDisplay.Visible = true;
            this.colAssetCategoryDisplay.VisibleIndex = 4;
            this.colAssetCategoryDisplay.Width = 150;
            // 
            // colCompanyName
            // 
            this.colCompanyName.AppearanceCell.Options.UseTextOptions = true;
            this.colCompanyName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCompanyName.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.colCompanyName.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colCompanyName.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.colCompanyName.AppearanceHeader.Options.UseBackColor = true;
            this.colCompanyName.AppearanceHeader.Options.UseFont = true;
            this.colCompanyName.AppearanceHeader.Options.UseForeColor = true;
            this.colCompanyName.AppearanceHeader.Options.UseTextOptions = true;
            this.colCompanyName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCompanyName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCompanyName.Caption = "Công ty";
            this.colCompanyName.FieldName = "CompanyName";
            this.colCompanyName.Name = "colCompanyName";
            this.colCompanyName.Visible = true;
            this.colCompanyName.VisibleIndex = 5;
            this.colCompanyName.Width = 150;
            // 
            // colBranchName
            // 
            this.colBranchName.AppearanceCell.Options.UseTextOptions = true;
            this.colBranchName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colBranchName.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.colBranchName.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colBranchName.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.colBranchName.AppearanceHeader.Options.UseBackColor = true;
            this.colBranchName.AppearanceHeader.Options.UseFont = true;
            this.colBranchName.AppearanceHeader.Options.UseForeColor = true;
            this.colBranchName.AppearanceHeader.Options.UseTextOptions = true;
            this.colBranchName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colBranchName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colBranchName.Caption = "Chi nhánh";
            this.colBranchName.FieldName = "BranchName";
            this.colBranchName.Name = "colBranchName";
            this.colBranchName.Visible = true;
            this.colBranchName.VisibleIndex = 6;
            this.colBranchName.Width = 120;
            // 
            // colDepartmentName
            // 
            this.colDepartmentName.AppearanceCell.Options.UseTextOptions = true;
            this.colDepartmentName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colDepartmentName.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.colDepartmentName.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colDepartmentName.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.colDepartmentName.AppearanceHeader.Options.UseBackColor = true;
            this.colDepartmentName.AppearanceHeader.Options.UseFont = true;
            this.colDepartmentName.AppearanceHeader.Options.UseForeColor = true;
            this.colDepartmentName.AppearanceHeader.Options.UseTextOptions = true;
            this.colDepartmentName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDepartmentName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colDepartmentName.Caption = "Phòng ban";
            this.colDepartmentName.FieldName = "DepartmentName";
            this.colDepartmentName.Name = "colDepartmentName";
            this.colDepartmentName.Visible = true;
            this.colDepartmentName.VisibleIndex = 7;
            this.colDepartmentName.Width = 120;
            // 
            // colAssignedEmployeeName
            // 
            this.colAssignedEmployeeName.AppearanceCell.Options.UseTextOptions = true;
            this.colAssignedEmployeeName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colAssignedEmployeeName.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.colAssignedEmployeeName.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colAssignedEmployeeName.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.colAssignedEmployeeName.AppearanceHeader.Options.UseBackColor = true;
            this.colAssignedEmployeeName.AppearanceHeader.Options.UseFont = true;
            this.colAssignedEmployeeName.AppearanceHeader.Options.UseForeColor = true;
            this.colAssignedEmployeeName.AppearanceHeader.Options.UseTextOptions = true;
            this.colAssignedEmployeeName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colAssignedEmployeeName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colAssignedEmployeeName.Caption = "Nhân viên phụ trách";
            this.colAssignedEmployeeName.FieldName = "AssignedEmployeeName";
            this.colAssignedEmployeeName.Name = "colAssignedEmployeeName";
            this.colAssignedEmployeeName.Visible = true;
            this.colAssignedEmployeeName.VisibleIndex = 8;
            this.colAssignedEmployeeName.Width = 150;
            // 
            // colLocation
            // 
            this.colLocation.AppearanceCell.Options.UseTextOptions = true;
            this.colLocation.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colLocation.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.colLocation.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colLocation.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.colLocation.AppearanceHeader.Options.UseBackColor = true;
            this.colLocation.AppearanceHeader.Options.UseFont = true;
            this.colLocation.AppearanceHeader.Options.UseForeColor = true;
            this.colLocation.AppearanceHeader.Options.UseTextOptions = true;
            this.colLocation.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colLocation.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colLocation.Caption = "Vị trí";
            this.colLocation.FieldName = "Location";
            this.colLocation.Name = "colLocation";
            this.colLocation.Visible = true;
            this.colLocation.VisibleIndex = 9;
            this.colLocation.Width = 150;
            // 
            // colPurchasePrice
            // 
            this.colPurchasePrice.AppearanceCell.Options.UseTextOptions = true;
            this.colPurchasePrice.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colPurchasePrice.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colPurchasePrice.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.colPurchasePrice.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colPurchasePrice.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.colPurchasePrice.AppearanceHeader.Options.UseBackColor = true;
            this.colPurchasePrice.AppearanceHeader.Options.UseFont = true;
            this.colPurchasePrice.AppearanceHeader.Options.UseForeColor = true;
            this.colPurchasePrice.AppearanceHeader.Options.UseTextOptions = true;
            this.colPurchasePrice.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPurchasePrice.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colPurchasePrice.Caption = "Giá mua";
            this.colPurchasePrice.DisplayFormat.FormatString = "N0";
            this.colPurchasePrice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colPurchasePrice.FieldName = "PurchasePrice";
            this.colPurchasePrice.Name = "colPurchasePrice";
            this.colPurchasePrice.Visible = true;
            this.colPurchasePrice.VisibleIndex = 10;
            this.colPurchasePrice.Width = 120;
            // 
            // colPurchaseDate
            // 
            this.colPurchaseDate.AppearanceCell.Options.UseTextOptions = true;
            this.colPurchaseDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPurchaseDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colPurchaseDate.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.colPurchaseDate.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colPurchaseDate.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.colPurchaseDate.AppearanceHeader.Options.UseBackColor = true;
            this.colPurchaseDate.AppearanceHeader.Options.UseFont = true;
            this.colPurchaseDate.AppearanceHeader.Options.UseForeColor = true;
            this.colPurchaseDate.AppearanceHeader.Options.UseTextOptions = true;
            this.colPurchaseDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPurchaseDate.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colPurchaseDate.Caption = "Ngày mua";
            this.colPurchaseDate.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.colPurchaseDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colPurchaseDate.FieldName = "PurchaseDate";
            this.colPurchaseDate.Name = "colPurchaseDate";
            this.colPurchaseDate.Visible = true;
            this.colPurchaseDate.VisibleIndex = 11;
            this.colPurchaseDate.Width = 100;
            // 
            // colAccumulatedDepreciation
            // 
            this.colAccumulatedDepreciation.AppearanceCell.Options.UseTextOptions = true;
            this.colAccumulatedDepreciation.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colAccumulatedDepreciation.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colAccumulatedDepreciation.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.colAccumulatedDepreciation.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colAccumulatedDepreciation.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.colAccumulatedDepreciation.AppearanceHeader.Options.UseBackColor = true;
            this.colAccumulatedDepreciation.AppearanceHeader.Options.UseFont = true;
            this.colAccumulatedDepreciation.AppearanceHeader.Options.UseForeColor = true;
            this.colAccumulatedDepreciation.AppearanceHeader.Options.UseTextOptions = true;
            this.colAccumulatedDepreciation.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colAccumulatedDepreciation.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colAccumulatedDepreciation.Caption = "Khấu hao lũy kế";
            this.colAccumulatedDepreciation.DisplayFormat.FormatString = "N0";
            this.colAccumulatedDepreciation.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colAccumulatedDepreciation.FieldName = "AccumulatedDepreciation";
            this.colAccumulatedDepreciation.Name = "colAccumulatedDepreciation";
            this.colAccumulatedDepreciation.Visible = true;
            this.colAccumulatedDepreciation.VisibleIndex = 12;
            this.colAccumulatedDepreciation.Width = 130;
            // 
            // colCurrentValue
            // 
            this.colCurrentValue.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colCurrentValue.AppearanceCell.ForeColor = System.Drawing.Color.Blue;
            this.colCurrentValue.AppearanceCell.Options.UseFont = true;
            this.colCurrentValue.AppearanceCell.Options.UseForeColor = true;
            this.colCurrentValue.AppearanceCell.Options.UseTextOptions = true;
            this.colCurrentValue.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colCurrentValue.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCurrentValue.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.colCurrentValue.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colCurrentValue.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.colCurrentValue.AppearanceHeader.Options.UseBackColor = true;
            this.colCurrentValue.AppearanceHeader.Options.UseFont = true;
            this.colCurrentValue.AppearanceHeader.Options.UseForeColor = true;
            this.colCurrentValue.AppearanceHeader.Options.UseTextOptions = true;
            this.colCurrentValue.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCurrentValue.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCurrentValue.Caption = "Giá trị hiện tại";
            this.colCurrentValue.DisplayFormat.FormatString = "N0";
            this.colCurrentValue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colCurrentValue.FieldName = "CurrentValue";
            this.colCurrentValue.Name = "colCurrentValue";
            this.colCurrentValue.Visible = true;
            this.colCurrentValue.VisibleIndex = 13;
            this.colCurrentValue.Width = 130;
            // 
            // colStatusDisplay
            // 
            this.colStatusDisplay.AppearanceCell.Options.UseTextOptions = true;
            this.colStatusDisplay.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colStatusDisplay.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colStatusDisplay.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.colStatusDisplay.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colStatusDisplay.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.colStatusDisplay.AppearanceHeader.Options.UseBackColor = true;
            this.colStatusDisplay.AppearanceHeader.Options.UseFont = true;
            this.colStatusDisplay.AppearanceHeader.Options.UseForeColor = true;
            this.colStatusDisplay.AppearanceHeader.Options.UseTextOptions = true;
            this.colStatusDisplay.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colStatusDisplay.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colStatusDisplay.Caption = "Trạng thái";
            this.colStatusDisplay.FieldName = "StatusDisplay";
            this.colStatusDisplay.Name = "colStatusDisplay";
            this.colStatusDisplay.Visible = true;
            this.colStatusDisplay.VisibleIndex = 14;
            this.colStatusDisplay.Width = 120;
            // 
            // colConditionDisplay
            // 
            this.colConditionDisplay.AppearanceCell.Options.UseTextOptions = true;
            this.colConditionDisplay.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colConditionDisplay.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colConditionDisplay.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.colConditionDisplay.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colConditionDisplay.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.colConditionDisplay.AppearanceHeader.Options.UseBackColor = true;
            this.colConditionDisplay.AppearanceHeader.Options.UseFont = true;
            this.colConditionDisplay.AppearanceHeader.Options.UseForeColor = true;
            this.colConditionDisplay.AppearanceHeader.Options.UseTextOptions = true;
            this.colConditionDisplay.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colConditionDisplay.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colConditionDisplay.Caption = "Tình trạng";
            this.colConditionDisplay.FieldName = "ConditionDisplay";
            this.colConditionDisplay.Name = "colConditionDisplay";
            this.colConditionDisplay.Visible = true;
            this.colConditionDisplay.VisibleIndex = 15;
            this.colConditionDisplay.Width = 100;
            // 
            // colWarrantyName
            // 
            this.colWarrantyName.AppearanceCell.Options.UseTextOptions = true;
            this.colWarrantyName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyName.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.colWarrantyName.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colWarrantyName.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.colWarrantyName.AppearanceHeader.Options.UseBackColor = true;
            this.colWarrantyName.AppearanceHeader.Options.UseFont = true;
            this.colWarrantyName.AppearanceHeader.Options.UseForeColor = true;
            this.colWarrantyName.AppearanceHeader.Options.UseTextOptions = true;
            this.colWarrantyName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyName.Caption = "Bảo hành";
            this.colWarrantyName.FieldName = "WarrantyName";
            this.colWarrantyName.Name = "colWarrantyName";
            this.colWarrantyName.Visible = true;
            this.colWarrantyName.VisibleIndex = 16;
            this.colWarrantyName.Width = 150;
            // 
            // colWarrantyExpiryDate
            // 
            this.colWarrantyExpiryDate.AppearanceCell.Options.UseTextOptions = true;
            this.colWarrantyExpiryDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyExpiryDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyExpiryDate.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.colWarrantyExpiryDate.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colWarrantyExpiryDate.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.colWarrantyExpiryDate.AppearanceHeader.Options.UseBackColor = true;
            this.colWarrantyExpiryDate.AppearanceHeader.Options.UseFont = true;
            this.colWarrantyExpiryDate.AppearanceHeader.Options.UseForeColor = true;
            this.colWarrantyExpiryDate.AppearanceHeader.Options.UseTextOptions = true;
            this.colWarrantyExpiryDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyExpiryDate.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyExpiryDate.Caption = "Ngày hết hạn BH";
            this.colWarrantyExpiryDate.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.colWarrantyExpiryDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colWarrantyExpiryDate.FieldName = "WarrantyExpiryDate";
            this.colWarrantyExpiryDate.Name = "colWarrantyExpiryDate";
            this.colWarrantyExpiryDate.Visible = true;
            this.colWarrantyExpiryDate.VisibleIndex = 17;
            this.colWarrantyExpiryDate.Width = 120;
            // 
            // colSerialNumber
            // 
            this.colSerialNumber.AppearanceCell.Options.UseTextOptions = true;
            this.colSerialNumber.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colSerialNumber.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.colSerialNumber.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colSerialNumber.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.colSerialNumber.AppearanceHeader.Options.UseBackColor = true;
            this.colSerialNumber.AppearanceHeader.Options.UseFont = true;
            this.colSerialNumber.AppearanceHeader.Options.UseForeColor = true;
            this.colSerialNumber.AppearanceHeader.Options.UseTextOptions = true;
            this.colSerialNumber.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colSerialNumber.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colSerialNumber.Caption = "Số seri";
            this.colSerialNumber.FieldName = "SerialNumber";
            this.colSerialNumber.Name = "colSerialNumber";
            this.colSerialNumber.Visible = true;
            this.colSerialNumber.VisibleIndex = 18;
            this.colSerialNumber.Width = 120;
            // 
            // colManufacturer
            // 
            this.colManufacturer.AppearanceCell.Options.UseTextOptions = true;
            this.colManufacturer.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colManufacturer.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.colManufacturer.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colManufacturer.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.colManufacturer.AppearanceHeader.Options.UseBackColor = true;
            this.colManufacturer.AppearanceHeader.Options.UseFont = true;
            this.colManufacturer.AppearanceHeader.Options.UseForeColor = true;
            this.colManufacturer.AppearanceHeader.Options.UseTextOptions = true;
            this.colManufacturer.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colManufacturer.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colManufacturer.Caption = "Nhà sản xuất";
            this.colManufacturer.FieldName = "Manufacturer";
            this.colManufacturer.Name = "colManufacturer";
            this.colManufacturer.Visible = true;
            this.colManufacturer.VisibleIndex = 19;
            this.colManufacturer.Width = 150;
            // 
            // colModel
            // 
            this.colModel.AppearanceCell.Options.UseTextOptions = true;
            this.colModel.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colModel.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.colModel.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colModel.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.colModel.AppearanceHeader.Options.UseBackColor = true;
            this.colModel.AppearanceHeader.Options.UseFont = true;
            this.colModel.AppearanceHeader.Options.UseForeColor = true;
            this.colModel.AppearanceHeader.Options.UseTextOptions = true;
            this.colModel.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colModel.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colModel.Caption = "Model";
            this.colModel.FieldName = "Model";
            this.colModel.Name = "colModel";
            this.colModel.Visible = true;
            this.colModel.VisibleIndex = 20;
            this.colModel.Width = 100;
            // 
            // colCreateDate
            // 
            this.colCreateDate.AppearanceCell.Options.UseTextOptions = true;
            this.colCreateDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCreateDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCreateDate.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.colCreateDate.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colCreateDate.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.colCreateDate.AppearanceHeader.Options.UseBackColor = true;
            this.colCreateDate.AppearanceHeader.Options.UseFont = true;
            this.colCreateDate.AppearanceHeader.Options.UseForeColor = true;
            this.colCreateDate.AppearanceHeader.Options.UseTextOptions = true;
            this.colCreateDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCreateDate.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCreateDate.Caption = "Ngày tạo";
            this.colCreateDate.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm";
            this.colCreateDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colCreateDate.FieldName = "CreateDate";
            this.colCreateDate.Name = "colCreateDate";
            this.colCreateDate.Visible = true;
            this.colCreateDate.VisibleIndex = 21;
            this.colCreateDate.Width = 130;
            // 
            // colCreatedByName
            // 
            this.colCreatedByName.AppearanceCell.Options.UseTextOptions = true;
            this.colCreatedByName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCreatedByName.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.colCreatedByName.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colCreatedByName.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.colCreatedByName.AppearanceHeader.Options.UseBackColor = true;
            this.colCreatedByName.AppearanceHeader.Options.UseFont = true;
            this.colCreatedByName.AppearanceHeader.Options.UseForeColor = true;
            this.colCreatedByName.AppearanceHeader.Options.UseTextOptions = true;
            this.colCreatedByName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCreatedByName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCreatedByName.Caption = "Người tạo";
            this.colCreatedByName.FieldName = "CreatedByName";
            this.colCreatedByName.Name = "colCreatedByName";
            this.colCreatedByName.Visible = true;
            this.colCreatedByName.VisibleIndex = 22;
            this.colCreatedByName.Width = 120;
            // 
            // FrmAssetDtoManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1525, 532);
            this.Controls.Add(this.AssetDtoGridViewGridControl);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FrmAssetDtoManagement";
            this.Text = "Quản lý tài sản";
            ((System.ComponentModel.ISupportInitialize)(this.assetDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AssetDtoGridViewGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AssetDtoGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private System.Windows.Forms.BindingSource assetDtoBindingSource;
        private DevExpress.XtraBars.BarButtonItem XemBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem ThemMoiBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem SuaBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem XoaBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem ExportFileBarButtonItem;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarHeaderItem barHeaderItem1;
        private DevExpress.XtraBars.BarStaticItem DataSummaryBarStaticItem;
        private DevExpress.XtraBars.BarStaticItem SelectedRowBarStaticItem;
        private DevExpress.XtraGrid.GridControl AssetDtoGridViewGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView AssetDtoGridView;
        private DevExpress.XtraGrid.Columns.GridColumn colId;
        private DevExpress.XtraGrid.Columns.GridColumn colAssetCode;
        private DevExpress.XtraGrid.Columns.GridColumn colAssetName;
        private DevExpress.XtraGrid.Columns.GridColumn colAssetTypeDisplay;
        private DevExpress.XtraGrid.Columns.GridColumn colAssetCategoryDisplay;
        private DevExpress.XtraGrid.Columns.GridColumn colCompanyName;
        private DevExpress.XtraGrid.Columns.GridColumn colBranchName;
        private DevExpress.XtraGrid.Columns.GridColumn colDepartmentName;
        private DevExpress.XtraGrid.Columns.GridColumn colAssignedEmployeeName;
        private DevExpress.XtraGrid.Columns.GridColumn colLocation;
        private DevExpress.XtraGrid.Columns.GridColumn colPurchasePrice;
        private DevExpress.XtraGrid.Columns.GridColumn colPurchaseDate;
        private DevExpress.XtraGrid.Columns.GridColumn colAccumulatedDepreciation;
        private DevExpress.XtraGrid.Columns.GridColumn colCurrentValue;
        private DevExpress.XtraGrid.Columns.GridColumn colStatusDisplay;
        private DevExpress.XtraGrid.Columns.GridColumn colConditionDisplay;
        private DevExpress.XtraGrid.Columns.GridColumn colWarrantyName;
        private DevExpress.XtraGrid.Columns.GridColumn colWarrantyExpiryDate;
        private DevExpress.XtraGrid.Columns.GridColumn colSerialNumber;
        private DevExpress.XtraGrid.Columns.GridColumn colManufacturer;
        private DevExpress.XtraGrid.Columns.GridColumn colModel;
        private DevExpress.XtraGrid.Columns.GridColumn colCreateDate;
        private DevExpress.XtraGrid.Columns.GridColumn colCreatedByName;
    }
}
