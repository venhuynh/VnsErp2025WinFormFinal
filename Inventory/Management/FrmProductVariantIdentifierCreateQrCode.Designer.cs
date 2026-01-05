using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraLayout;

namespace Inventory.Management
{
    partial class FrmProductVariantIdentifierCreateQrCode
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
            DevExpress.XtraLayout.ColumnDefinition columnDefinition3 = new DevExpress.XtraLayout.ColumnDefinition();
            DevExpress.XtraLayout.ColumnDefinition columnDefinition4 = new DevExpress.XtraLayout.ColumnDefinition();
            DevExpress.XtraLayout.RowDefinition rowDefinition4 = new DevExpress.XtraLayout.RowDefinition();
            DevExpress.XtraLayout.RowDefinition rowDefinition5 = new DevExpress.XtraLayout.RowDefinition();
            DevExpress.XtraLayout.RowDefinition rowDefinition6 = new DevExpress.XtraLayout.RowDefinition();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.InQrCodeBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.TaoQcCodeBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.SaveBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.CloseBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.colParentDepartmentName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colDepartmentName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.ProductVariantIdentifierGridControl = new DevExpress.XtraGrid.GridControl();
            this.productVariantIdentifierItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ProductVariantIdentifierGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ProductVariantIdentifierEnumGridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ProductVariantIdentifierEnumComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.ProductVariantIdentifierValueGridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.QrCodePictureEdit = new DevExpress.XtraEditors.PictureEdit();
            this.QRCodeImageLockedCheckEdit = new DevExpress.XtraEditors.ToggleSwitch();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForQRCodeImageLocked = new DevExpress.XtraLayout.LayoutControlItem();
            this.simpleLabelItem1 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.QrValueSimpleLabelItem = new DevExpress.XtraLayout.SimpleLabelItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productVariantIdentifierItemBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierEnumComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QrCodePictureEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QRCodeImageLockedCheckEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForQRCodeImageLocked)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QrValueSimpleLabelItem)).BeginInit();
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
            this.InQrCodeBarButtonItem,
            this.TaoQcCodeBarButtonItem,
            this.barButtonItem2});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 5;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.InQrCodeBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.TaoQcCodeBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.SaveBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.CloseBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem2, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // InQrCodeBarButtonItem
            // 
            this.InQrCodeBarButtonItem.Caption = "In";
            this.InQrCodeBarButtonItem.Id = 2;
            this.InQrCodeBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.print_16x16;
            this.InQrCodeBarButtonItem.Name = "InQrCodeBarButtonItem";
            // 
            // TaoQcCodeBarButtonItem
            // 
            this.TaoQcCodeBarButtonItem.Caption = "Tạo QR";
            this.TaoQcCodeBarButtonItem.Id = 3;
            this.TaoQcCodeBarButtonItem.ImageOptions.SvgImage = global::Inventory.Properties.Resources.QRCode;
            this.TaoQcCodeBarButtonItem.Name = "TaoQcCodeBarButtonItem";
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
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "Tải QR";
            this.barButtonItem2.Id = 4;
            this.barButtonItem2.ImageOptions.Image = global::Inventory.Properties.Resources.download_16x16;
            this.barButtonItem2.ImageOptions.LargeImage = global::Inventory.Properties.Resources.download_32x32;
            this.barButtonItem2.Name = "barButtonItem2";
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
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 517);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1116, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 493);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1116, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 493);
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
            this.dataLayoutControl1.Controls.Add(this.ProductVariantIdentifierGridControl);
            this.dataLayoutControl1.Controls.Add(this.QrCodePictureEdit);
            this.dataLayoutControl1.Controls.Add(this.QRCodeImageLockedCheckEdit);
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 24);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(1116, 493);
            this.dataLayoutControl1.TabIndex = 14;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // ProductVariantIdentifierGridControl
            // 
            this.ProductVariantIdentifierGridControl.DataSource = this.productVariantIdentifierItemBindingSource;
            this.ProductVariantIdentifierGridControl.Location = new System.Drawing.Point(12, 84);
            this.ProductVariantIdentifierGridControl.MainView = this.ProductVariantIdentifierGridView;
            this.ProductVariantIdentifierGridControl.MenuManager = this.barManager1;
            this.ProductVariantIdentifierGridControl.Name = "ProductVariantIdentifierGridControl";
            this.ProductVariantIdentifierGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ProductVariantIdentifierEnumComboBox});
            this.ProductVariantIdentifierGridControl.Size = new System.Drawing.Size(544, 397);
            this.ProductVariantIdentifierGridControl.TabIndex = 44;
            this.ProductVariantIdentifierGridControl.UseEmbeddedNavigator = true;
            this.ProductVariantIdentifierGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ProductVariantIdentifierGridView});
            // 
            // productVariantIdentifierItemBindingSource
            // 
            this.productVariantIdentifierItemBindingSource.DataSource = typeof(Inventory.Management.ProductVariantIdentifierItem);
            // 
            // ProductVariantIdentifierGridView
            // 
            this.ProductVariantIdentifierGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ProductVariantIdentifierEnumGridColumn,
            this.ProductVariantIdentifierValueGridColumn});
            this.ProductVariantIdentifierGridView.GridControl = this.ProductVariantIdentifierGridControl;
            this.ProductVariantIdentifierGridView.Name = "ProductVariantIdentifierGridView";
            this.ProductVariantIdentifierGridView.NewItemRowText = "Click để thêm dòng mới";
            this.ProductVariantIdentifierGridView.OptionsMenu.ShowAddNewSummaryItem = DevExpress.Utils.DefaultBoolean.True;
            this.ProductVariantIdentifierGridView.OptionsNavigation.AutoFocusNewRow = true;
            this.ProductVariantIdentifierGridView.OptionsSelection.MultiSelect = true;
            this.ProductVariantIdentifierGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.ProductVariantIdentifierGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.ProductVariantIdentifierGridView.OptionsView.ShowGroupPanel = false;
            this.ProductVariantIdentifierGridView.OptionsView.ShowViewCaption = true;
            this.ProductVariantIdentifierGridView.ViewCaption = "BẢNG DANH SÁCH ĐỊNH DANH";
            // 
            // ProductVariantIdentifierEnumGridColumn
            // 
            this.ProductVariantIdentifierEnumGridColumn.AppearanceCell.Options.UseTextOptions = true;
            this.ProductVariantIdentifierEnumGridColumn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ProductVariantIdentifierEnumGridColumn.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ProductVariantIdentifierEnumGridColumn.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ProductVariantIdentifierEnumGridColumn.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.ProductVariantIdentifierEnumGridColumn.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.ProductVariantIdentifierEnumGridColumn.AppearanceHeader.Options.UseBackColor = true;
            this.ProductVariantIdentifierEnumGridColumn.AppearanceHeader.Options.UseFont = true;
            this.ProductVariantIdentifierEnumGridColumn.AppearanceHeader.Options.UseForeColor = true;
            this.ProductVariantIdentifierEnumGridColumn.AppearanceHeader.Options.UseTextOptions = true;
            this.ProductVariantIdentifierEnumGridColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ProductVariantIdentifierEnumGridColumn.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ProductVariantIdentifierEnumGridColumn.Caption = "Kiểu định danh";
            this.ProductVariantIdentifierEnumGridColumn.ColumnEdit = this.ProductVariantIdentifierEnumComboBox;
            this.ProductVariantIdentifierEnumGridColumn.FieldName = "IdentifierType";
            this.ProductVariantIdentifierEnumGridColumn.Name = "ProductVariantIdentifierEnumGridColumn";
            this.ProductVariantIdentifierEnumGridColumn.Visible = true;
            this.ProductVariantIdentifierEnumGridColumn.VisibleIndex = 1;
            this.ProductVariantIdentifierEnumGridColumn.Width = 150;
            // 
            // ProductVariantIdentifierEnumComboBox
            // 
            this.ProductVariantIdentifierEnumComboBox.AutoHeight = false;
            this.ProductVariantIdentifierEnumComboBox.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ProductVariantIdentifierEnumComboBox.Name = "ProductVariantIdentifierEnumComboBox";
            // 
            // ProductVariantIdentifierValueGridColumn
            // 
            this.ProductVariantIdentifierValueGridColumn.AppearanceCell.Options.UseTextOptions = true;
            this.ProductVariantIdentifierValueGridColumn.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ProductVariantIdentifierValueGridColumn.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.ProductVariantIdentifierValueGridColumn.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ProductVariantIdentifierValueGridColumn.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.ProductVariantIdentifierValueGridColumn.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.ProductVariantIdentifierValueGridColumn.AppearanceHeader.Options.UseBackColor = true;
            this.ProductVariantIdentifierValueGridColumn.AppearanceHeader.Options.UseFont = true;
            this.ProductVariantIdentifierValueGridColumn.AppearanceHeader.Options.UseForeColor = true;
            this.ProductVariantIdentifierValueGridColumn.AppearanceHeader.Options.UseTextOptions = true;
            this.ProductVariantIdentifierValueGridColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ProductVariantIdentifierValueGridColumn.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ProductVariantIdentifierValueGridColumn.Caption = "Giá trị";
            this.ProductVariantIdentifierValueGridColumn.FieldName = "Value";
            this.ProductVariantIdentifierValueGridColumn.Name = "ProductVariantIdentifierValueGridColumn";
            this.ProductVariantIdentifierValueGridColumn.Visible = true;
            this.ProductVariantIdentifierValueGridColumn.VisibleIndex = 2;
            this.ProductVariantIdentifierValueGridColumn.Width = 250;
            // 
            // QrCodePictureEdit
            // 
            this.QrCodePictureEdit.Location = new System.Drawing.Point(560, 78);
            this.QrCodePictureEdit.MenuManager = this.barManager1;
            this.QrCodePictureEdit.Name = "QrCodePictureEdit";
            this.QrCodePictureEdit.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.QrCodePictureEdit.Size = new System.Drawing.Size(544, 403);
            this.QrCodePictureEdit.StyleController = this.dataLayoutControl1;
            this.QrCodePictureEdit.TabIndex = 39;
            // 
            // QRCodeImageLockedCheckEdit
            // 
            this.QRCodeImageLockedCheckEdit.Location = new System.Drawing.Point(165, 62);
            this.QRCodeImageLockedCheckEdit.MenuManager = this.barManager1;
            this.QRCodeImageLockedCheckEdit.Name = "QRCodeImageLockedCheckEdit";
            this.QRCodeImageLockedCheckEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.QRCodeImageLockedCheckEdit.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.QRCodeImageLockedCheckEdit.Properties.OffText = "<color=\'red\'>Đã khóa</color>";
            this.QRCodeImageLockedCheckEdit.Properties.OnText = "<color=\'blue\'>Chưa khóa</color>";
            this.QRCodeImageLockedCheckEdit.Size = new System.Drawing.Size(391, 18);
            this.QRCodeImageLockedCheckEdit.StyleController = this.dataLayoutControl1;
            this.QRCodeImageLockedCheckEdit.TabIndex = 26;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.ItemForQRCodeImageLocked,
            this.simpleLabelItem1,
            this.layoutControlItem1,
            this.QrValueSimpleLabelItem});
            this.Root.LayoutMode = DevExpress.XtraLayout.Utils.LayoutMode.Table;
            this.Root.Name = "Root";
            columnDefinition3.SizeType = System.Windows.Forms.SizeType.Percent;
            columnDefinition3.Width = 100D;
            columnDefinition4.SizeType = System.Windows.Forms.SizeType.Percent;
            columnDefinition4.Width = 100D;
            this.Root.OptionsTableLayoutGroup.ColumnDefinitions.AddRange(new DevExpress.XtraLayout.ColumnDefinition[] {
            columnDefinition3,
            columnDefinition4});
            rowDefinition4.Height = 50D;
            rowDefinition4.SizeType = System.Windows.Forms.SizeType.Absolute;
            rowDefinition5.Height = 22D;
            rowDefinition5.SizeType = System.Windows.Forms.SizeType.AutoSize;
            rowDefinition6.Height = 401D;
            rowDefinition6.SizeType = System.Windows.Forms.SizeType.AutoSize;
            this.Root.OptionsTableLayoutGroup.RowDefinitions.AddRange(new DevExpress.XtraLayout.RowDefinition[] {
            rowDefinition4,
            rowDefinition5,
            rowDefinition6});
            this.Root.Size = new System.Drawing.Size(1116, 493);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.ProductVariantIdentifierGridControl;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 72);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.OptionsTableLayoutItem.RowIndex = 2;
            this.layoutControlItem2.Size = new System.Drawing.Size(548, 401);
            this.layoutControlItem2.TextVisible = false;
            // 
            // ItemForQRCodeImageLocked
            // 
            this.ItemForQRCodeImageLocked.Control = this.QRCodeImageLockedCheckEdit;
            this.ItemForQRCodeImageLocked.Location = new System.Drawing.Point(0, 50);
            this.ItemForQRCodeImageLocked.Name = "ItemForQRCodeImageLocked";
            this.ItemForQRCodeImageLocked.OptionsTableLayoutItem.RowIndex = 1;
            this.ItemForQRCodeImageLocked.Size = new System.Drawing.Size(548, 22);
            this.ItemForQRCodeImageLocked.TextSize = new System.Drawing.Size(141, 13);
            // 
            // simpleLabelItem1
            // 
            this.simpleLabelItem1.Location = new System.Drawing.Point(0, 0);
            this.simpleLabelItem1.Name = "simpleLabelItem1";
            this.simpleLabelItem1.Size = new System.Drawing.Size(548, 50);
            this.simpleLabelItem1.TextSize = new System.Drawing.Size(141, 13);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.QrCodePictureEdit;
            this.layoutControlItem1.Location = new System.Drawing.Point(548, 50);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.OptionsTableLayoutItem.ColumnIndex = 1;
            this.layoutControlItem1.OptionsTableLayoutItem.RowIndex = 1;
            this.layoutControlItem1.OptionsTableLayoutItem.RowSpan = 2;
            this.layoutControlItem1.Size = new System.Drawing.Size(548, 423);
            this.layoutControlItem1.Text = "Hình ảnh QR";
            this.layoutControlItem1.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(141, 13);
            // 
            // QrValueSimpleLabelItem
            // 
            this.QrValueSimpleLabelItem.Location = new System.Drawing.Point(548, 0);
            this.QrValueSimpleLabelItem.Name = "QrValueSimpleLabelItem";
            this.QrValueSimpleLabelItem.OptionsTableLayoutItem.ColumnIndex = 1;
            this.QrValueSimpleLabelItem.Size = new System.Drawing.Size(548, 50);
            this.QrValueSimpleLabelItem.TextSize = new System.Drawing.Size(141, 13);
            // 
            // FrmProductVariantIdentifierCreateQrCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1116, 517);
            this.Controls.Add(this.dataLayoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmProductVariantIdentifierCreateQrCode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mã QR quản lý";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productVariantIdentifierItemBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierEnumComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QrCodePictureEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QRCodeImageLockedCheckEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForQRCodeImageLocked)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QrValueSimpleLabelItem)).EndInit();
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
        private BarButtonItem InQrCodeBarButtonItem;
        private DataLayoutControl dataLayoutControl1;
        private LayoutControlGroup Root;
        private PictureEdit QrCodePictureEdit;
        private ToggleSwitch QRCodeImageLockedCheckEdit;
        private BarButtonItem TaoQcCodeBarButtonItem;
        private BarButtonItem barButtonItem2;
        private DevExpress.XtraGrid.GridControl ProductVariantIdentifierGridControl;
        private System.Windows.Forms.BindingSource productVariantIdentifierItemBindingSource;
        private DevExpress.XtraGrid.Views.Grid.GridView ProductVariantIdentifierGridView;
        private DevExpress.XtraGrid.Columns.GridColumn ProductVariantIdentifierEnumGridColumn;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox ProductVariantIdentifierEnumComboBox;
        private DevExpress.XtraGrid.Columns.GridColumn ProductVariantIdentifierValueGridColumn;
        private LayoutControlItem layoutControlItem2;
        private LayoutControlItem ItemForQRCodeImageLocked;
        private SimpleLabelItem simpleLabelItem1;
        private LayoutControlItem layoutControlItem1;
        private SimpleLabelItem QrValueSimpleLabelItem;
    }
}