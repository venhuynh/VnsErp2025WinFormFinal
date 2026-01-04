using DTO.DeviceAssetManagement;

namespace DeviceAssetManagement.Management.DeviceMangement
{
    partial class FrmDeviceDtoMangement
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
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.XemBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.ThemMoiBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ThemLichSuNhapXuatThietBiBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ThemHinhAnhBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ThemBaoHanhBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.SuaBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.XoaBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ExportFileBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem2 = new DevExpress.XtraBars.BarSubItem();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barHeaderItem1 = new DevExpress.XtraBars.BarHeaderItem();
            this.DataSummaryBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.SelectedRowBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.ucDeviceWarranty1 = new DeviceAssetManagement.Management.DeviceMangement.UcDeviceWarranty();
            this.ucDeviceImageAdd1 = new DeviceAssetManagement.Management.DeviceMangement.UcDeviceImageAdd();
            this.ucDeviceDtoAddStockInOutHistory1 = new DeviceAssetManagement.Management.DeviceMangement.UcDeviceDtoAddStockInOutHistory();
            // Cannot instantiate abstract class - this.ucDeviceDtoAddEdit1 = new DeviceAssetManagement.Management.DeviceMangement.UcDeviceDtoAddEdit();
            this.ucDeviceDtoAddEdit1 = null; // Abstract class - must be initialized with concrete implementation
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.DeviceDtoGridViewGridControl = new DevExpress.XtraGrid.GridControl();
            this.deviceDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.DeviceDtoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colHtmlInfo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.HtmlRepositoryItemHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.colStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.StatusComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.colNotes = new DevExpress.XtraGrid.Columns.GridColumn();
            this.NotesMemoEdit = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.dockPanel1.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceDtoGridViewGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deviceDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceDtoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlRepositoryItemHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StatusComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotesMemoEdit)).BeginInit();
            this.SuspendLayout();
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
            this.barManager1.DockManager = this.dockManager1;
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.XemBarButtonItem,
            this.ThemMoiBarButtonItem,
            this.SuaBarButtonItem,
            this.XoaBarButtonItem,
            this.ExportFileBarButtonItem,
            this.barHeaderItem1,
            this.DataSummaryBarStaticItem,
            this.SelectedRowBarStaticItem,
            this.barSubItem1,
            this.ThemLichSuNhapXuatThietBiBarButtonItem,
            this.ThemHinhAnhBarButtonItem,
            this.ThemBaoHanhBarButtonItem,
            this.barButtonItem1,
            this.barSubItem2});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 15;
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
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barSubItem1, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.SuaBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.XoaBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.ExportFileBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem1, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem2)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // XemBarButtonItem
            // 
            this.XemBarButtonItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.XemBarButtonItem.Caption = "Xem";
            this.XemBarButtonItem.Id = 1;
            this.XemBarButtonItem.Name = "XemBarButtonItem";
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "Thêm mới";
            this.barSubItem1.Id = 9;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.ThemMoiBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.ThemLichSuNhapXuatThietBiBarButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.ThemHinhAnhBarButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.ThemBaoHanhBarButtonItem)});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // ThemMoiBarButtonItem
            // 
            this.ThemMoiBarButtonItem.Caption = "Tài sản - thiết bị";
            this.ThemMoiBarButtonItem.Id = 2;
            this.ThemMoiBarButtonItem.Name = "ThemMoiBarButtonItem";
            // 
            // ThemLichSuNhapXuatThietBiBarButtonItem
            // 
            this.ThemLichSuNhapXuatThietBiBarButtonItem.Caption = "Lịch sử nhập - xuất";
            this.ThemLichSuNhapXuatThietBiBarButtonItem.Id = 10;
            this.ThemLichSuNhapXuatThietBiBarButtonItem.Name = "ThemLichSuNhapXuatThietBiBarButtonItem";
            // 
            // ThemHinhAnhBarButtonItem
            // 
            this.ThemHinhAnhBarButtonItem.Caption = "Hình ảnh";
            this.ThemHinhAnhBarButtonItem.Id = 11;
            this.ThemHinhAnhBarButtonItem.Name = "ThemHinhAnhBarButtonItem";
            // 
            // ThemBaoHanhBarButtonItem
            // 
            this.ThemBaoHanhBarButtonItem.Caption = "Bảo hành";
            this.ThemBaoHanhBarButtonItem.Id = 12;
            this.ThemBaoHanhBarButtonItem.Name = "ThemBaoHanhBarButtonItem";
            // 
            // SuaBarButtonItem
            // 
            this.SuaBarButtonItem.Caption = "Sửa";
            this.SuaBarButtonItem.Id = 3;
            this.SuaBarButtonItem.Name = "SuaBarButtonItem";
            // 
            // XoaBarButtonItem
            // 
            this.XoaBarButtonItem.Caption = "Xóa";
            this.XoaBarButtonItem.Id = 4;
            this.XoaBarButtonItem.Name = "XoaBarButtonItem";
            // 
            // ExportFileBarButtonItem
            // 
            this.ExportFileBarButtonItem.Caption = "Xuất file";
            this.ExportFileBarButtonItem.Id = 5;
            this.ExportFileBarButtonItem.Name = "ExportFileBarButtonItem";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Mã quản lý";
            this.barButtonItem1.Id = 13;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barSubItem2
            // 
            this.barSubItem2.Caption = "Điều chỉnh";
            this.barSubItem2.Id = 14;
            this.barSubItem2.Name = "barSubItem2";
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
            this.barDockControlTop.Size = new System.Drawing.Size(1357, 37);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 648);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1357, 35);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 37);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 611);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1357, 37);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 611);
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.MenuManager = this.barManager1;
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanel1});
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl",
            "DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl",
            "DevExpress.XtraBars.ToolbarForm.ToolbarFormControl"});
            // 
            // dockPanel1
            // 
            this.dockPanel1.Controls.Add(this.dockPanel1_Container);
            this.dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.dockPanel1.ID = new System.Guid("0c476c5c-6d27-493e-8a8e-c8d2ad65a997");
            this.dockPanel1.Location = new System.Drawing.Point(914, 37);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.OriginalSize = new System.Drawing.Size(443, 200);
            this.dockPanel1.Size = new System.Drawing.Size(443, 611);
            this.dockPanel1.Text = "dockPanel1";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.layoutControl1);
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 38);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(436, 570);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.ucDeviceWarranty1);
            this.layoutControl1.Controls.Add(this.ucDeviceImageAdd1);
            this.layoutControl1.Controls.Add(this.ucDeviceDtoAddStockInOutHistory1);
            this.layoutControl1.Controls.Add(this.ucDeviceDtoAddEdit1);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(436, 570);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // ucDeviceWarranty1
            // 
            this.ucDeviceWarranty1.Location = new System.Drawing.Point(16, 279);
            this.ucDeviceWarranty1.Name = "ucDeviceWarranty1";
            this.ucDeviceWarranty1.Size = new System.Drawing.Size(404, 1);
            this.ucDeviceWarranty1.TabIndex = 7;
            // 
            // ucDeviceImageAdd1
            // 
            this.ucDeviceImageAdd1.Location = new System.Drawing.Point(16, 456);
            this.ucDeviceImageAdd1.Name = "ucDeviceImageAdd1";
            this.ucDeviceImageAdd1.Size = new System.Drawing.Size(404, 98);
            this.ucDeviceImageAdd1.TabIndex = 6;
            // 
            // ucDeviceDtoAddStockInOutHistory1
            // 
            this.ucDeviceDtoAddStockInOutHistory1.Location = new System.Drawing.Point(16, 286);
            this.ucDeviceDtoAddStockInOutHistory1.Name = "ucDeviceDtoAddStockInOutHistory1";
            this.ucDeviceDtoAddStockInOutHistory1.Size = new System.Drawing.Size(404, 164);
            this.ucDeviceDtoAddStockInOutHistory1.TabIndex = 5;
            // 
            // ucDeviceDtoAddEdit1
            // 
            this.ucDeviceDtoAddEdit1.Location = new System.Drawing.Point(16, 16);
            this.ucDeviceDtoAddEdit1.Name = "ucDeviceDtoAddEdit1";
            this.ucDeviceDtoAddEdit1.Size = new System.Drawing.Size(404, 257);
            this.ucDeviceDtoAddEdit1.TabIndex = 4;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(436, 570);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.ucDeviceDtoAddEdit1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(410, 263);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.ucDeviceDtoAddStockInOutHistory1;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 270);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(410, 170);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.ucDeviceImageAdd1;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 440);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(410, 104);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.ucDeviceWarranty1;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 263);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(410, 7);
            this.layoutControlItem4.TextVisible = false;
            // 
            // DeviceDtoGridViewGridControl
            // 
            this.DeviceDtoGridViewGridControl.DataSource = this.deviceDtoBindingSource;
            this.DeviceDtoGridViewGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DeviceDtoGridViewGridControl.Location = new System.Drawing.Point(0, 37);
            this.DeviceDtoGridViewGridControl.MainView = this.DeviceDtoGridView;
            this.DeviceDtoGridViewGridControl.MenuManager = this.barManager1;
            this.DeviceDtoGridViewGridControl.Name = "DeviceDtoGridViewGridControl";
            this.DeviceDtoGridViewGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.HtmlRepositoryItemHypertextLabel,
            this.NotesMemoEdit,
            this.StatusComboBox});
            this.DeviceDtoGridViewGridControl.Size = new System.Drawing.Size(914, 611);
            this.DeviceDtoGridViewGridControl.TabIndex = 5;
            this.DeviceDtoGridViewGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.DeviceDtoGridView});
            // 
            // deviceDtoBindingSource
            // 
            this.deviceDtoBindingSource.DataSource = typeof(DTO.DeviceAssetManagement.DeviceDto);
            // 
            // DeviceDtoGridView
            // 
            this.DeviceDtoGridView.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.DeviceDtoGridView.Appearance.ViewCaption.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.DeviceDtoGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.DeviceDtoGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.DeviceDtoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colHtmlInfo,
            this.colStatus,
            this.colNotes});
            this.DeviceDtoGridView.GridControl = this.DeviceDtoGridViewGridControl;
            this.DeviceDtoGridView.Name = "DeviceDtoGridView";
            this.DeviceDtoGridView.OptionsSelection.MultiSelect = true;
            this.DeviceDtoGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.DeviceDtoGridView.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.DeviceDtoGridView.OptionsView.RowAutoHeight = true;
            this.DeviceDtoGridView.OptionsView.ShowGroupPanel = false;
            this.DeviceDtoGridView.OptionsView.ShowViewCaption = true;
            this.DeviceDtoGridView.ViewCaption = "QUẢN LÝ TÀI SẢN - THIẾT BỊ";
            // 
            // colHtmlInfo
            // 
            this.colHtmlInfo.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colHtmlInfo.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colHtmlInfo.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colHtmlInfo.AppearanceHeader.Options.UseBackColor = true;
            this.colHtmlInfo.AppearanceHeader.Options.UseFont = true;
            this.colHtmlInfo.AppearanceHeader.Options.UseForeColor = true;
            this.colHtmlInfo.AppearanceHeader.Options.UseTextOptions = true;
            this.colHtmlInfo.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colHtmlInfo.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colHtmlInfo.Caption = "Thông tin thiết bị";
            this.colHtmlInfo.ColumnEdit = this.HtmlRepositoryItemHypertextLabel;
            this.colHtmlInfo.FieldName = "HtmlInfo";
            this.colHtmlInfo.Name = "colHtmlInfo";
            this.colHtmlInfo.Visible = true;
            this.colHtmlInfo.VisibleIndex = 1;
            this.colHtmlInfo.Width = 600;
            // 
            // HtmlRepositoryItemHypertextLabel
            // 
            this.HtmlRepositoryItemHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.HtmlRepositoryItemHypertextLabel.Name = "HtmlRepositoryItemHypertextLabel";
            // 
            // colStatus
            // 
            this.colStatus.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colStatus.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colStatus.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colStatus.AppearanceHeader.Options.UseBackColor = true;
            this.colStatus.AppearanceHeader.Options.UseFont = true;
            this.colStatus.AppearanceHeader.Options.UseForeColor = true;
            this.colStatus.AppearanceHeader.Options.UseTextOptions = true;
            this.colStatus.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colStatus.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colStatus.Caption = "Trạng thái";
            this.colStatus.ColumnEdit = this.StatusComboBox;
            this.colStatus.FieldName = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.Visible = true;
            this.colStatus.VisibleIndex = 2;
            this.colStatus.Width = 100;
            // 
            // StatusComboBox
            // 
            this.StatusComboBox.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.StatusComboBox.AutoHeight = false;
            this.StatusComboBox.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.StatusComboBox.ContextButtonOptions.AllowHtmlText = true;
            this.StatusComboBox.Items.AddRange(new object[] {
            "<color=\'red\'>Test</color>"});
            this.StatusComboBox.Name = "StatusComboBox";
            this.StatusComboBox.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // colNotes
            // 
            this.colNotes.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colNotes.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colNotes.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colNotes.AppearanceHeader.Options.UseBackColor = true;
            this.colNotes.AppearanceHeader.Options.UseFont = true;
            this.colNotes.AppearanceHeader.Options.UseForeColor = true;
            this.colNotes.AppearanceHeader.Options.UseTextOptions = true;
            this.colNotes.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colNotes.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colNotes.Caption = "Ghi chú";
            this.colNotes.ColumnEdit = this.NotesMemoEdit;
            this.colNotes.FieldName = "Notes";
            this.colNotes.Name = "colNotes";
            this.colNotes.Visible = true;
            this.colNotes.VisibleIndex = 3;
            this.colNotes.Width = 200;
            // 
            // NotesMemoEdit
            // 
            this.NotesMemoEdit.Name = "NotesMemoEdit";
            // 
            // FrmDeviceDtoMangement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1357, 683);
            this.Controls.Add(this.DeviceDtoGridViewGridControl);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FrmDeviceDtoMangement";
            this.Text = "QUẢN LÝ TÀI SẢN - THIẾT BỊ";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.dockPanel1.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceDtoGridViewGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deviceDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceDtoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlRepositoryItemHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StatusComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotesMemoEdit)).EndInit();
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
        private DevExpress.XtraBars.BarButtonItem XemBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem ThemMoiBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem SuaBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem XoaBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem ExportFileBarButtonItem;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarHeaderItem barHeaderItem1;
        private DevExpress.XtraBars.BarStaticItem DataSummaryBarStaticItem;
        private DevExpress.XtraBars.BarStaticItem SelectedRowBarStaticItem;
        private DevExpress.XtraGrid.GridControl DeviceDtoGridViewGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView DeviceDtoGridView;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel HtmlRepositoryItemHypertextLabel;
        private System.Windows.Forms.BindingSource deviceDtoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colHtmlInfo;
        private DevExpress.XtraGrid.Columns.GridColumn colStatus;
        private DevExpress.XtraGrid.Columns.GridColumn colNotes;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox StatusComboBox;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit NotesMemoEdit;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarButtonItem ThemLichSuNhapXuatThietBiBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem ThemHinhAnhBarButtonItem;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private UcDeviceDtoAddEdit ucDeviceDtoAddEdit1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private UcDeviceDtoAddStockInOutHistory ucDeviceDtoAddStockInOutHistory1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private UcDeviceImageAdd ucDeviceImageAdd1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private UcDeviceWarranty ucDeviceWarranty1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraBars.BarButtonItem ThemBaoHanhBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarSubItem barSubItem2;
    }
}