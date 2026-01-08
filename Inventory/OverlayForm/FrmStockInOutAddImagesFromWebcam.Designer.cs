using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraLayout;
using Inventory.ProductVariantIdentifier;

namespace Inventory.OverlayForm
{
    partial class FrmStockInOutAddImagesFromWebcam
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
            DevExpress.XtraLayout.ColumnDefinition columnDefinition5 = new DevExpress.XtraLayout.ColumnDefinition();
            DevExpress.XtraLayout.ColumnDefinition columnDefinition6 = new DevExpress.XtraLayout.ColumnDefinition();
            DevExpress.XtraLayout.RowDefinition rowDefinition3 = new DevExpress.XtraLayout.RowDefinition();
            DevExpress.XtraLayout.RowDefinition rowDefinition4 = new DevExpress.XtraLayout.RowDefinition();
            DevExpress.XtraLayout.ColumnDefinition columnDefinition1 = new DevExpress.XtraLayout.ColumnDefinition();
            DevExpress.XtraLayout.ColumnDefinition columnDefinition2 = new DevExpress.XtraLayout.ColumnDefinition();
            DevExpress.XtraLayout.ColumnDefinition columnDefinition3 = new DevExpress.XtraLayout.ColumnDefinition();
            DevExpress.XtraLayout.ColumnDefinition columnDefinition4 = new DevExpress.XtraLayout.ColumnDefinition();
            DevExpress.XtraLayout.RowDefinition rowDefinition1 = new DevExpress.XtraLayout.RowDefinition();
            DevExpress.XtraLayout.RowDefinition rowDefinition2 = new DevExpress.XtraLayout.RowDefinition();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.SaveBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.CloseBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barHeaderItem1 = new DevExpress.XtraBars.BarHeaderItem();
            this.SoLuongNhapXuatBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.SoLuongTaoQrCodeBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.colParentDepartmentName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colDepartmentName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.cameraControl1 = new DevExpress.XtraEditors.Camera.CameraControl();
            this.StockInOutImageGridControl = new DevExpress.XtraGrid.GridControl();
            this.StockInOutImageGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.StockInOutInfoSimpleLabelItem = new DevExpress.XtraLayout.SimpleLabelItem();
            this.productVariantIdentifierItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.CaptureHyperlinkLabelControl = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.hyperlinkLabelControl11 = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.hyperlinkLabelControl12 = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.hyperlinkLabelControl13 = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutImageGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutImageGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutInfoSimpleLabelItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productVariantIdentifierItemBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
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
            this.CloseBarButtonItem,
            this.barHeaderItem1,
            this.SoLuongNhapXuatBarStaticItem,
            this.SoLuongTaoQrCodeBarStaticItem});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 8;
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
            this.SaveBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.save_16x16;
            this.SaveBarButtonItem.Name = "SaveBarButtonItem";
            // 
            // CloseBarButtonItem
            // 
            this.CloseBarButtonItem.Caption = "Đóng";
            this.CloseBarButtonItem.Id = 1;
            this.CloseBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.cancel_16x16;
            this.CloseBarButtonItem.Name = "CloseBarButtonItem";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1116, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 668);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1116, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 644);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1116, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 644);
            // 
            // barHeaderItem1
            // 
            this.barHeaderItem1.Caption = "Tổng kết";
            this.barHeaderItem1.Id = 5;
            this.barHeaderItem1.Name = "barHeaderItem1";
            // 
            // SoLuongNhapXuatBarStaticItem
            // 
            this.SoLuongNhapXuatBarStaticItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.SoLuongNhapXuatBarStaticItem.Caption = "Số lượng nhập xuất";
            this.SoLuongNhapXuatBarStaticItem.Id = 6;
            this.SoLuongNhapXuatBarStaticItem.Name = "SoLuongNhapXuatBarStaticItem";
            // 
            // SoLuongTaoQrCodeBarStaticItem
            // 
            this.SoLuongTaoQrCodeBarStaticItem.Caption = "Số lượng đã tạo QR Code";
            this.SoLuongTaoQrCodeBarStaticItem.Id = 7;
            this.SoLuongTaoQrCodeBarStaticItem.Name = "SoLuongTaoQrCodeBarStaticItem";
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
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
            // dataLayoutControl1
            // 
            this.dataLayoutControl1.Controls.Add(this.CaptureHyperlinkLabelControl);
            this.dataLayoutControl1.Controls.Add(this.cameraControl1);
            this.dataLayoutControl1.Controls.Add(this.StockInOutImageGridControl);
            this.dataLayoutControl1.Controls.Add(this.hyperlinkLabelControl11);
            this.dataLayoutControl1.Controls.Add(this.hyperlinkLabelControl12);
            this.dataLayoutControl1.Controls.Add(this.hyperlinkLabelControl13);
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 24);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(1116, 644);
            this.dataLayoutControl1.TabIndex = 14;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // cameraControl1
            // 
            this.cameraControl1.Location = new System.Drawing.Point(24, 99);
            this.cameraControl1.Name = "cameraControl1";
            this.cameraControl1.Size = new System.Drawing.Size(520, 521);
            this.cameraControl1.StyleController = this.dataLayoutControl1;
            this.cameraControl1.TabIndex = 41;
            this.cameraControl1.Text = "cameraControl1";
            // 
            // StockInOutImageGridControl
            // 
            this.StockInOutImageGridControl.Location = new System.Drawing.Point(560, 12);
            this.StockInOutImageGridControl.MainView = this.StockInOutImageGridView;
            this.StockInOutImageGridControl.MenuManager = this.barManager1;
            this.StockInOutImageGridControl.Name = "StockInOutImageGridControl";
            this.StockInOutImageGridControl.Size = new System.Drawing.Size(544, 620);
            this.StockInOutImageGridControl.TabIndex = 40;
            this.StockInOutImageGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.StockInOutImageGridView});
            // 
            // StockInOutImageGridView
            // 
            this.StockInOutImageGridView.GridControl = this.StockInOutImageGridControl;
            this.StockInOutImageGridView.Name = "StockInOutImageGridView";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.StockInOutInfoSimpleLabelItem,
            this.layoutControlItem2,
            this.layoutControlGroup1});
            this.Root.LayoutMode = DevExpress.XtraLayout.Utils.LayoutMode.Table;
            this.Root.Name = "Root";
            columnDefinition5.SizeType = System.Windows.Forms.SizeType.Percent;
            columnDefinition5.Width = 100D;
            columnDefinition6.SizeType = System.Windows.Forms.SizeType.Percent;
            columnDefinition6.Width = 100D;
            this.Root.OptionsTableLayoutGroup.ColumnDefinitions.AddRange(new DevExpress.XtraLayout.ColumnDefinition[] {
            columnDefinition5,
            columnDefinition6});
            rowDefinition3.Height = 17D;
            rowDefinition3.SizeType = System.Windows.Forms.SizeType.AutoSize;
            rowDefinition4.Height = 607D;
            rowDefinition4.SizeType = System.Windows.Forms.SizeType.AutoSize;
            this.Root.OptionsTableLayoutGroup.RowDefinitions.AddRange(new DevExpress.XtraLayout.RowDefinition[] {
            rowDefinition3,
            rowDefinition4});
            this.Root.Size = new System.Drawing.Size(1116, 644);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.StockInOutImageGridControl;
            this.layoutControlItem2.Location = new System.Drawing.Point(548, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.OptionsTableLayoutItem.ColumnIndex = 1;
            this.layoutControlItem2.OptionsTableLayoutItem.RowSpan = 2;
            this.layoutControlItem2.Size = new System.Drawing.Size(548, 624);
            this.layoutControlItem2.TextVisible = false;
            // 
            // StockInOutInfoSimpleLabelItem
            // 
            this.StockInOutInfoSimpleLabelItem.AllowHtmlStringInCaption = true;
            this.StockInOutInfoSimpleLabelItem.AppearanceItemCaption.Options.UseTextOptions = true;
            this.StockInOutInfoSimpleLabelItem.AppearanceItemCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.StockInOutInfoSimpleLabelItem.Location = new System.Drawing.Point(0, 0);
            this.StockInOutInfoSimpleLabelItem.Name = "StockInOutInfoSimpleLabelItem";
            this.StockInOutInfoSimpleLabelItem.OptionsPrint.AppearanceItem.Options.UseTextOptions = true;
            this.StockInOutInfoSimpleLabelItem.OptionsPrint.AppearanceItem.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.StockInOutInfoSimpleLabelItem.OptionsPrint.AppearanceItemControl.Options.UseTextOptions = true;
            this.StockInOutInfoSimpleLabelItem.OptionsPrint.AppearanceItemControl.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.StockInOutInfoSimpleLabelItem.OptionsPrint.AppearanceItemText.Options.UseTextOptions = true;
            this.StockInOutInfoSimpleLabelItem.OptionsPrint.AppearanceItemText.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.StockInOutInfoSimpleLabelItem.OptionsToolTip.AllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.StockInOutInfoSimpleLabelItem.Size = new System.Drawing.Size(548, 17);
            this.StockInOutInfoSimpleLabelItem.Text = "Thông tin sản phẩm ";
            this.StockInOutInfoSimpleLabelItem.TextSize = new System.Drawing.Size(97, 13);
            // 
            // productVariantIdentifierItemBindingSource
            // 
            this.productVariantIdentifierItemBindingSource.DataSource = typeof(Inventory.ProductVariantIdentifier.ProductVariantIdentifierItem);
            // 
            // CaptureHyperlinkLabelControl
            // 
            this.CaptureHyperlinkLabelControl.Location = new System.Drawing.Point(65, 62);
            this.CaptureHyperlinkLabelControl.Name = "CaptureHyperlinkLabelControl";
            this.CaptureHyperlinkLabelControl.Padding = new System.Windows.Forms.Padding(10);
            this.CaptureHyperlinkLabelControl.Size = new System.Drawing.Size(45, 33);
            this.CaptureHyperlinkLabelControl.StyleController = this.dataLayoutControl1;
            this.CaptureHyperlinkLabelControl.TabIndex = 42;
            this.CaptureHyperlinkLabelControl.Text = "Chụp";
            // 
            // hyperlinkLabelControl11
            // 
            this.hyperlinkLabelControl11.Location = new System.Drawing.Point(196, 62);
            this.hyperlinkLabelControl11.Name = "hyperlinkLabelControl11";
            this.hyperlinkLabelControl11.Padding = new System.Windows.Forms.Padding(10);
            this.hyperlinkLabelControl11.Size = new System.Drawing.Size(44, 33);
            this.hyperlinkLabelControl11.StyleController = this.dataLayoutControl1;
            this.hyperlinkLabelControl11.TabIndex = 42;
            this.hyperlinkLabelControl11.Text = "Xoay";
            // 
            // hyperlinkLabelControl12
            // 
            this.hyperlinkLabelControl12.Location = new System.Drawing.Point(316, 62);
            this.hyperlinkLabelControl12.Name = "hyperlinkLabelControl12";
            this.hyperlinkLabelControl12.Padding = new System.Windows.Forms.Padding(10);
            this.hyperlinkLabelControl12.Size = new System.Drawing.Size(67, 33);
            this.hyperlinkLabelControl12.StyleController = this.dataLayoutControl1;
            this.hyperlinkLabelControl12.TabIndex = 42;
            this.hyperlinkLabelControl12.Text = "Thêm vào";
            // 
            // hyperlinkLabelControl13
            // 
            this.hyperlinkLabelControl13.Location = new System.Drawing.Point(461, 62);
            this.hyperlinkLabelControl13.Name = "hyperlinkLabelControl13";
            this.hyperlinkLabelControl13.Padding = new System.Windows.Forms.Padding(10);
            this.hyperlinkLabelControl13.Size = new System.Drawing.Size(38, 33);
            this.hyperlinkLabelControl13.StyleController = this.dataLayoutControl1;
            this.hyperlinkLabelControl13.TabIndex = 42;
            this.hyperlinkLabelControl13.Text = "Xóa";
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AppearanceGroup.Options.UseTextOptions = true;
            this.layoutControlGroup1.AppearanceGroup.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlGroup1.AppearanceGroup.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlGroup1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem1});
            this.layoutControlGroup1.LayoutMode = DevExpress.XtraLayout.Utils.LayoutMode.Table;
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 17);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            columnDefinition1.SizeType = System.Windows.Forms.SizeType.Percent;
            columnDefinition1.Width = 25D;
            columnDefinition2.SizeType = System.Windows.Forms.SizeType.Percent;
            columnDefinition2.Width = 25D;
            columnDefinition3.SizeType = System.Windows.Forms.SizeType.Percent;
            columnDefinition3.Width = 25D;
            columnDefinition4.SizeType = System.Windows.Forms.SizeType.Percent;
            columnDefinition4.Width = 25D;
            this.layoutControlGroup1.OptionsTableLayoutGroup.ColumnDefinitions.AddRange(new DevExpress.XtraLayout.ColumnDefinition[] {
            columnDefinition1,
            columnDefinition2,
            columnDefinition3,
            columnDefinition4});
            rowDefinition1.Height = 37D;
            rowDefinition1.SizeType = System.Windows.Forms.SizeType.AutoSize;
            rowDefinition2.Height = 100D;
            rowDefinition2.SizeType = System.Windows.Forms.SizeType.Percent;
            this.layoutControlGroup1.OptionsTableLayoutGroup.RowDefinitions.AddRange(new DevExpress.XtraLayout.RowDefinition[] {
            rowDefinition1,
            rowDefinition2});
            this.layoutControlGroup1.OptionsTableLayoutItem.RowIndex = 1;
            this.layoutControlGroup1.Size = new System.Drawing.Size(548, 607);
            this.layoutControlGroup1.Text = "Điều khiển webcam";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.cameraControl1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 37);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.OptionsTableLayoutItem.ColumnSpan = 4;
            this.layoutControlItem1.OptionsTableLayoutItem.RowIndex = 1;
            this.layoutControlItem1.Size = new System.Drawing.Size(524, 525);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem3.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem3.Control = this.CaptureHyperlinkLabelControl;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(131, 37);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem4.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem4.Control = this.hyperlinkLabelControl11;
            this.layoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem4.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem4.Location = new System.Drawing.Point(131, 0);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.OptionsTableLayoutItem.ColumnIndex = 1;
            this.layoutControlItem4.Size = new System.Drawing.Size(131, 37);
            this.layoutControlItem4.Text = "layoutControlItem3";
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem5.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem5.Control = this.hyperlinkLabelControl12;
            this.layoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem5.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem5.Location = new System.Drawing.Point(262, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.OptionsTableLayoutItem.ColumnIndex = 2;
            this.layoutControlItem5.Size = new System.Drawing.Size(131, 37);
            this.layoutControlItem5.Text = "layoutControlItem3";
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem6.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem6.Control = this.hyperlinkLabelControl13;
            this.layoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem6.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem6.Location = new System.Drawing.Point(393, 0);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.OptionsTableLayoutItem.ColumnIndex = 3;
            this.layoutControlItem6.Size = new System.Drawing.Size(131, 37);
            this.layoutControlItem6.Text = "Xóa";
            this.layoutControlItem6.TextVisible = false;
            // 
            // FrmStockInOutAddImagesFromWebcam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1116, 668);
            this.Controls.Add(this.dataLayoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmStockInOutAddImagesFromWebcam";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mã QR quản lý";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutImageGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutImageGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutInfoSimpleLabelItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productVariantIdentifierItemBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
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
        private DevExpress.XtraTreeList.Columns.TreeListColumn colParentDepartmentName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colDepartmentName;
        private DataLayoutControl dataLayoutControl1;
        private LayoutControlGroup Root;
        private System.Windows.Forms.BindingSource productVariantIdentifierItemBindingSource;
        private BarHeaderItem barHeaderItem1;
        private BarStaticItem SoLuongNhapXuatBarStaticItem;
        private BarStaticItem SoLuongTaoQrCodeBarStaticItem;
        private DevExpress.XtraGrid.GridControl StockInOutImageGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView StockInOutImageGridView;
        private LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.Camera.CameraControl cameraControl1;
        private SimpleLabelItem StockInOutInfoSimpleLabelItem;
        private HyperlinkLabelControl CaptureHyperlinkLabelControl;
        private HyperlinkLabelControl hyperlinkLabelControl11;
        private HyperlinkLabelControl hyperlinkLabelControl12;
        private HyperlinkLabelControl hyperlinkLabelControl13;
        private LayoutControlGroup layoutControlGroup1;
        private LayoutControlItem layoutControlItem3;
        private LayoutControlItem layoutControlItem4;
        private LayoutControlItem layoutControlItem5;
        private LayoutControlItem layoutControlItem6;
        private LayoutControlItem layoutControlItem1;
    }
}