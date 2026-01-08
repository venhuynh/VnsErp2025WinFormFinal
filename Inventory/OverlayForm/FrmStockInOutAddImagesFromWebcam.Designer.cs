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
            DevExpress.XtraLayout.ColumnDefinition columnDefinition1 = new DevExpress.XtraLayout.ColumnDefinition();
            DevExpress.XtraLayout.ColumnDefinition columnDefinition2 = new DevExpress.XtraLayout.ColumnDefinition();
            DevExpress.XtraLayout.RowDefinition rowDefinition1 = new DevExpress.XtraLayout.RowDefinition();
            DevExpress.XtraLayout.RowDefinition rowDefinition2 = new DevExpress.XtraLayout.RowDefinition();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.CaptureBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.XoaBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.SaveBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.colParentDepartmentName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colDepartmentName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.cameraControl1 = new DevExpress.XtraEditors.Camera.CameraControl();
            this.StockInOutImageGridControl = new DevExpress.XtraGrid.GridControl();
            this.stockInOutImageDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.StockInOutImageGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colCaptureTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSize = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colImageData = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.StockInOutInfoSimpleLabelItem = new DevExpress.XtraLayout.SimpleLabelItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutImageGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockInOutImageDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutImageGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutInfoSimpleLabelItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
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
            this.CaptureBarButtonItem,
            this.XoaBarButtonItem});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 10;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.CaptureBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.XoaBarButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.SaveBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // CaptureBarButtonItem
            // 
            this.CaptureBarButtonItem.Caption = "Chụp";
            this.CaptureBarButtonItem.Id = 8;
            this.CaptureBarButtonItem.ImageOptions.SvgImage = global::Inventory.Properties.Resources.Webcam2;
            this.CaptureBarButtonItem.Name = "CaptureBarButtonItem";
            // 
            // XoaBarButtonItem
            // 
            this.XoaBarButtonItem.Caption = "Xóa";
            this.XoaBarButtonItem.Id = 9;
            this.XoaBarButtonItem.Name = "XoaBarButtonItem";
            // 
            // SaveBarButtonItem
            // 
            this.SaveBarButtonItem.Caption = "Lưu";
            this.SaveBarButtonItem.Id = 0;
            this.SaveBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.save_16x16;
            this.SaveBarButtonItem.Name = "SaveBarButtonItem";
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
            this.dataLayoutControl1.Controls.Add(this.cameraControl1);
            this.dataLayoutControl1.Controls.Add(this.StockInOutImageGridControl);
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
            this.cameraControl1.Location = new System.Drawing.Point(12, 29);
            this.cameraControl1.Name = "cameraControl1";
            this.cameraControl1.Size = new System.Drawing.Size(544, 603);
            this.cameraControl1.StyleController = this.dataLayoutControl1;
            this.cameraControl1.TabIndex = 41;
            this.cameraControl1.Text = "cameraControl1";
            // 
            // StockInOutImageGridControl
            // 
            this.StockInOutImageGridControl.DataSource = this.stockInOutImageDtoBindingSource;
            this.StockInOutImageGridControl.Location = new System.Drawing.Point(560, 12);
            this.StockInOutImageGridControl.MainView = this.StockInOutImageGridView;
            this.StockInOutImageGridControl.MenuManager = this.barManager1;
            this.StockInOutImageGridControl.Name = "StockInOutImageGridControl";
            this.StockInOutImageGridControl.Size = new System.Drawing.Size(544, 620);
            this.StockInOutImageGridControl.TabIndex = 40;
            this.StockInOutImageGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.StockInOutImageGridView});
            // 
            // stockInOutImageDtoBindingSource
            // 
            this.stockInOutImageDtoBindingSource.DataSource = typeof(DTO.Inventory.Query.StockInOutImageDto);
            // 
            // StockInOutImageGridView
            // 
            this.StockInOutImageGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colCaptureTime,
            this.colSize,
            this.colImageData});
            this.StockInOutImageGridView.GridControl = this.StockInOutImageGridControl;
            this.StockInOutImageGridView.Name = "StockInOutImageGridView";
            this.StockInOutImageGridView.OptionsSelection.MultiSelect = true;
            this.StockInOutImageGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.StockInOutImageGridView.OptionsView.ShowGroupPanel = false;
            this.StockInOutImageGridView.RowHeight = 85;
            // 
            // colCaptureTime
            // 
            this.colCaptureTime.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colCaptureTime.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colCaptureTime.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colCaptureTime.AppearanceHeader.Options.UseBackColor = true;
            this.colCaptureTime.AppearanceHeader.Options.UseFont = true;
            this.colCaptureTime.AppearanceHeader.Options.UseForeColor = true;
            this.colCaptureTime.AppearanceHeader.Options.UseTextOptions = true;
            this.colCaptureTime.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCaptureTime.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCaptureTime.Caption = "Thời gian chụp";
            this.colCaptureTime.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";
            this.colCaptureTime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colCaptureTime.FieldName = "CreateDate";
            this.colCaptureTime.Name = "colCaptureTime";
            this.colCaptureTime.OptionsColumn.AllowEdit = false;
            this.colCaptureTime.OptionsColumn.ReadOnly = true;
            this.colCaptureTime.Visible = true;
            this.colCaptureTime.VisibleIndex = 3;
            this.colCaptureTime.Width = 150;
            // 
            // colSize
            // 
            this.colSize.AppearanceCell.Options.UseTextOptions = true;
            this.colSize.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colSize.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colSize.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colSize.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colSize.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colSize.AppearanceHeader.Options.UseBackColor = true;
            this.colSize.AppearanceHeader.Options.UseFont = true;
            this.colSize.AppearanceHeader.Options.UseForeColor = true;
            this.colSize.AppearanceHeader.Options.UseTextOptions = true;
            this.colSize.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colSize.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colSize.Caption = "Kích thước (bytes)";
            this.colSize.DisplayFormat.FormatString = "N0";
            this.colSize.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colSize.FieldName = "FileSizeDisplay";
            this.colSize.Name = "colSize";
            this.colSize.OptionsColumn.AllowEdit = false;
            this.colSize.OptionsColumn.ReadOnly = true;
            this.colSize.Visible = true;
            this.colSize.VisibleIndex = 2;
            this.colSize.Width = 120;
            // 
            // colImageData
            // 
            this.colImageData.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colImageData.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colImageData.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colImageData.AppearanceHeader.Options.UseBackColor = true;
            this.colImageData.AppearanceHeader.Options.UseFont = true;
            this.colImageData.AppearanceHeader.Options.UseForeColor = true;
            this.colImageData.AppearanceHeader.Options.UseTextOptions = true;
            this.colImageData.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colImageData.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colImageData.Caption = "Hình ảnh";
            this.colImageData.FieldName = "ImageData";
            this.colImageData.ImageOptions.SvgImageSize = new System.Drawing.Size(0, 60);
            this.colImageData.Name = "colImageData";
            this.colImageData.OptionsColumn.AllowEdit = false;
            this.colImageData.OptionsColumn.ReadOnly = true;
            this.colImageData.Visible = true;
            this.colImageData.VisibleIndex = 1;
            this.colImageData.Width = 200;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.StockInOutInfoSimpleLabelItem,
            this.layoutControlItem2,
            this.layoutControlItem1});
            this.Root.LayoutMode = DevExpress.XtraLayout.Utils.LayoutMode.Table;
            this.Root.Name = "Root";
            columnDefinition1.SizeType = System.Windows.Forms.SizeType.Percent;
            columnDefinition1.Width = 100D;
            columnDefinition2.SizeType = System.Windows.Forms.SizeType.Percent;
            columnDefinition2.Width = 100D;
            this.Root.OptionsTableLayoutGroup.ColumnDefinitions.AddRange(new DevExpress.XtraLayout.ColumnDefinition[] {
            columnDefinition1,
            columnDefinition2});
            rowDefinition1.Height = 17D;
            rowDefinition1.SizeType = System.Windows.Forms.SizeType.AutoSize;
            rowDefinition2.Height = 607D;
            rowDefinition2.SizeType = System.Windows.Forms.SizeType.AutoSize;
            this.Root.OptionsTableLayoutGroup.RowDefinitions.AddRange(new DevExpress.XtraLayout.RowDefinition[] {
            rowDefinition1,
            rowDefinition2});
            this.Root.Size = new System.Drawing.Size(1116, 644);
            this.Root.TextVisible = false;
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
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.cameraControl1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 17);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.OptionsTableLayoutItem.RowIndex = 1;
            this.layoutControlItem1.Size = new System.Drawing.Size(548, 607);
            this.layoutControlItem1.TextVisible = false;
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
            ((System.ComponentModel.ISupportInitialize)(this.stockInOutImageDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutImageGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutInfoSimpleLabelItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private BarManager barManager1;
        private Bar bar2;
        private BarButtonItem SaveBarButtonItem;
        private BarDockControl barDockControlTop;
        private BarDockControl barDockControlBottom;
        private BarDockControl barDockControlLeft;
        private BarDockControl barDockControlRight;
        private DXErrorProvider dxErrorProvider1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colParentDepartmentName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colDepartmentName;
        private DataLayoutControl dataLayoutControl1;
        private LayoutControlGroup Root;
        private DevExpress.XtraGrid.GridControl StockInOutImageGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView StockInOutImageGridView;
        private LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.Camera.CameraControl cameraControl1;
        private SimpleLabelItem StockInOutInfoSimpleLabelItem;
        private System.Windows.Forms.BindingSource stockInOutImageDtoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colCaptureTime;
        private DevExpress.XtraGrid.Columns.GridColumn colSize;
        private DevExpress.XtraGrid.Columns.GridColumn colImageData;
        private BarButtonItem CaptureBarButtonItem;
        private BarButtonItem XoaBarButtonItem;
        private LayoutControlItem layoutControlItem1;
    }
}