using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraLayout;

namespace Inventory.ProductVariantIdentifier
{
    partial class FrmCreateNewSerialNumber
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
            this.barHeaderItem1 = new DevExpress.XtraBars.BarHeaderItem();
            this.SoLuongNhapXuatBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.colParentDepartmentName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colDepartmentName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.SerialNumberMemoEdit = new DevExpress.XtraEditors.MemoEdit();
            this.VoucerNumberTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.SerialNumberQtyTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.ProductVariantFullNameSimpleLabelItem = new DevExpress.XtraLayout.SimpleLabelItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.productVariantIdentifierItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SerialNumberMemoEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VoucerNumberTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SerialNumberQtyTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantFullNameSimpleLabelItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productVariantIdentifierItemBindingSource)).BeginInit();
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
            this.SoLuongNhapXuatBarStaticItem});
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
            this.barDockControlTop.Size = new System.Drawing.Size(491, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 364);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(491, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 340);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(491, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 340);
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
            this.dataLayoutControl1.Controls.Add(this.SerialNumberMemoEdit);
            this.dataLayoutControl1.Controls.Add(this.VoucerNumberTextEdit);
            this.dataLayoutControl1.Controls.Add(this.SerialNumberQtyTextEdit);
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 24);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(491, 340);
            this.dataLayoutControl1.TabIndex = 14;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // SerialNumberMemoEdit
            // 
            this.SerialNumberMemoEdit.Location = new System.Drawing.Point(121, 113);
            this.SerialNumberMemoEdit.MenuManager = this.barManager1;
            this.SerialNumberMemoEdit.Name = "SerialNumberMemoEdit";
            this.SerialNumberMemoEdit.Size = new System.Drawing.Size(358, 215);
            this.SerialNumberMemoEdit.StyleController = this.dataLayoutControl1;
            this.SerialNumberMemoEdit.TabIndex = 6;
            // 
            // VoucerNumberTextEdit
            // 
            this.VoucerNumberTextEdit.Location = new System.Drawing.Point(121, 65);
            this.VoucerNumberTextEdit.MenuManager = this.barManager1;
            this.VoucerNumberTextEdit.Name = "VoucerNumberTextEdit";
            this.VoucerNumberTextEdit.Size = new System.Drawing.Size(358, 20);
            this.VoucerNumberTextEdit.StyleController = this.dataLayoutControl1;
            this.VoucerNumberTextEdit.TabIndex = 4;
            // 
            // SerialNumberQtyTextEdit
            // 
            this.SerialNumberQtyTextEdit.Location = new System.Drawing.Point(121, 89);
            this.SerialNumberQtyTextEdit.MenuManager = this.barManager1;
            this.SerialNumberQtyTextEdit.Name = "SerialNumberQtyTextEdit";
            this.SerialNumberQtyTextEdit.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.SerialNumberQtyTextEdit.Properties.MaskSettings.Set("MaskManagerSignature", "allowNull=False");
            this.SerialNumberQtyTextEdit.Properties.MaskSettings.Set("mask", "d");
            this.SerialNumberQtyTextEdit.Properties.UseMaskAsDisplayFormat = true;
            this.SerialNumberQtyTextEdit.Size = new System.Drawing.Size(358, 20);
            this.SerialNumberQtyTextEdit.StyleController = this.dataLayoutControl1;
            this.SerialNumberQtyTextEdit.TabIndex = 5;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.ProductVariantFullNameSimpleLabelItem,
            this.layoutControlItem2,
            this.layoutControlItem3});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(491, 340);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.VoucerNumberTextEdit;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 53);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(471, 24);
            this.layoutControlItem1.Text = "Số phiếu nhập/xuất";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(97, 13);
            // 
            // ProductVariantFullNameSimpleLabelItem
            // 
            this.ProductVariantFullNameSimpleLabelItem.AllowHtmlStringInCaption = true;
            this.ProductVariantFullNameSimpleLabelItem.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ProductVariantFullNameSimpleLabelItem.AppearanceItemCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.ProductVariantFullNameSimpleLabelItem.Location = new System.Drawing.Point(0, 0);
            this.ProductVariantFullNameSimpleLabelItem.Name = "ProductVariantFullNameSimpleLabelItem";
            this.ProductVariantFullNameSimpleLabelItem.OptionsPrint.AppearanceItem.Options.UseTextOptions = true;
            this.ProductVariantFullNameSimpleLabelItem.OptionsPrint.AppearanceItem.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.ProductVariantFullNameSimpleLabelItem.OptionsPrint.AppearanceItemControl.Options.UseTextOptions = true;
            this.ProductVariantFullNameSimpleLabelItem.OptionsPrint.AppearanceItemControl.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.ProductVariantFullNameSimpleLabelItem.OptionsPrint.AppearanceItemText.Options.UseTextOptions = true;
            this.ProductVariantFullNameSimpleLabelItem.OptionsPrint.AppearanceItemText.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.ProductVariantFullNameSimpleLabelItem.OptionsToolTip.AllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ProductVariantFullNameSimpleLabelItem.Padding = new DevExpress.XtraLayout.Utils.Padding(20, 20, 20, 20);
            this.ProductVariantFullNameSimpleLabelItem.Size = new System.Drawing.Size(471, 53);
            this.ProductVariantFullNameSimpleLabelItem.Text = "Thông tin sản phẩm ";
            this.ProductVariantFullNameSimpleLabelItem.TextSize = new System.Drawing.Size(97, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.SerialNumberQtyTextEdit;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 77);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(471, 24);
            this.layoutControlItem2.Text = "Số lượng sản phẩm";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(97, 13);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.SerialNumberMemoEdit;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 101);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(471, 219);
            this.layoutControlItem3.Text = "Serial Number";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(97, 13);
            // 
            // productVariantIdentifierItemBindingSource
            // 
            this.productVariantIdentifierItemBindingSource.DataSource = typeof(ProductVariantIdentifierItem);
            // 
            // FrmCreateNewSerialNumber
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 364);
            this.Controls.Add(this.dataLayoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmCreateNewSerialNumber";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tạo số Serrial Number";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SerialNumberMemoEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VoucerNumberTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SerialNumberQtyTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantFullNameSimpleLabelItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productVariantIdentifierItemBindingSource)).EndInit();
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
        private SimpleLabelItem ProductVariantFullNameSimpleLabelItem;
        private BarHeaderItem barHeaderItem1;
        private BarStaticItem SoLuongNhapXuatBarStaticItem;
        private DevExpress.XtraEditors.MemoEdit SerialNumberMemoEdit;
        private DevExpress.XtraEditors.TextEdit VoucerNumberTextEdit;
        private DevExpress.XtraEditors.TextEdit SerialNumberQtyTextEdit;
        private LayoutControlItem layoutControlItem1;
        private LayoutControlItem layoutControlItem2;
        private LayoutControlItem layoutControlItem3;
    }
}