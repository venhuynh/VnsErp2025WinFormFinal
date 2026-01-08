using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraLayout;
using Inventory.ProductVariantIdentifier;

namespace Inventory.Query
{
    partial class FrmProductVariantIdentifierAddEditFromStockInOut
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
            this.NewIdentifierBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
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
            this.InputTypeComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.ProductVariantIdentifierGridControl = new DevExpress.XtraGrid.GridControl();
            this.productVariantIdentifierItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ProductVariantIdentifierGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ProductVariantIdentifierEnumGridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ProductVariantIdentifierEnumComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.ProductVariantIdentifierValueGridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.ProductVariantFullNameSimpleLabelItem = new DevExpress.XtraLayout.SimpleLabelItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InputTypeComboBoxEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productVariantIdentifierItemBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierEnumComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantFullNameSimpleLabelItem)).BeginInit();
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
            this.CloseBarButtonItem,
            this.NewIdentifierBarButtonItem,
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
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.NewIdentifierBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.SaveBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.CloseBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // NewIdentifierBarButtonItem
            // 
            this.NewIdentifierBarButtonItem.Caption = "Mới";
            this.NewIdentifierBarButtonItem.Id = 2;
            this.NewIdentifierBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.add_16x16;
            this.NewIdentifierBarButtonItem.Name = "NewIdentifierBarButtonItem";
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
            this.barDockControlTop.Size = new System.Drawing.Size(691, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 521);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(691, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 497);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(691, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 497);
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
            this.dataLayoutControl1.Controls.Add(this.InputTypeComboBoxEdit);
            this.dataLayoutControl1.Controls.Add(this.ProductVariantIdentifierGridControl);
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 24);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(691, 497);
            this.dataLayoutControl1.TabIndex = 14;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // InputTypeComboBoxEdit
            // 
            this.InputTypeComboBoxEdit.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.True;
            this.InputTypeComboBoxEdit.EditValue = "<color=\'blue\'>Nhập 1 lượt nhiều định danh</color>";
            this.InputTypeComboBoxEdit.Location = new System.Drawing.Point(64, 65);
            this.InputTypeComboBoxEdit.MenuManager = this.barManager1;
            this.InputTypeComboBoxEdit.Name = "InputTypeComboBoxEdit";
            this.InputTypeComboBoxEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.InputTypeComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.InputTypeComboBoxEdit.Properties.Items.AddRange(new object[] {
            "<color=\'blue\'>Nhập 1 lượt nhiều định danh</color>",
            "<color=\'red\'>Nhập từng định danh riêng lẻ</color>"});
            this.InputTypeComboBoxEdit.Size = new System.Drawing.Size(615, 20);
            this.InputTypeComboBoxEdit.StyleController = this.dataLayoutControl1;
            this.InputTypeComboBoxEdit.TabIndex = 45;
            // 
            // ProductVariantIdentifierGridControl
            // 
            this.ProductVariantIdentifierGridControl.DataSource = this.productVariantIdentifierItemBindingSource;
            this.ProductVariantIdentifierGridControl.Location = new System.Drawing.Point(12, 89);
            this.ProductVariantIdentifierGridControl.MainView = this.ProductVariantIdentifierGridView;
            this.ProductVariantIdentifierGridControl.MenuManager = this.barManager1;
            this.ProductVariantIdentifierGridControl.Name = "ProductVariantIdentifierGridControl";
            this.ProductVariantIdentifierGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ProductVariantIdentifierEnumComboBox});
            this.ProductVariantIdentifierGridControl.Size = new System.Drawing.Size(667, 396);
            this.ProductVariantIdentifierGridControl.TabIndex = 44;
            this.ProductVariantIdentifierGridControl.UseEmbeddedNavigator = true;
            this.ProductVariantIdentifierGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ProductVariantIdentifierGridView});
            // 
            // productVariantIdentifierItemBindingSource
            // 
            this.productVariantIdentifierItemBindingSource.DataSource = typeof(Inventory.ProductVariantIdentifier.ProductVariantIdentifierItem);
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
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.ProductVariantFullNameSimpleLabelItem,
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(691, 497);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.ProductVariantIdentifierGridControl;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 77);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.OptionsTableLayoutItem.RowIndex = 2;
            this.layoutControlItem2.Size = new System.Drawing.Size(671, 400);
            this.layoutControlItem2.TextVisible = false;
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
            this.ProductVariantFullNameSimpleLabelItem.Size = new System.Drawing.Size(671, 53);
            this.ProductVariantFullNameSimpleLabelItem.Text = "Thông tin sản phẩm ";
            this.ProductVariantFullNameSimpleLabelItem.TextSize = new System.Drawing.Size(97, 13);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AllowHtmlStringInCaption = true;
            this.layoutControlItem1.Control = this.InputTypeComboBoxEdit;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 53);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.OptionsToolTip.AllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlItem1.Size = new System.Drawing.Size(671, 24);
            this.layoutControlItem1.Text = "Kiểu nhập";
            this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(47, 13);
            this.layoutControlItem1.TextToControlDistance = 5;
            // 
            // FrmProductVariantIdentifierAddEditFromStockInOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 521);
            this.Controls.Add(this.dataLayoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmProductVariantIdentifierAddEditFromStockInOut";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Định danh hàng hóa sản phẩm";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.InputTypeComboBoxEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productVariantIdentifierItemBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierEnumComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantFullNameSimpleLabelItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
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
        private BarButtonItem NewIdentifierBarButtonItem;
        private DataLayoutControl dataLayoutControl1;
        private LayoutControlGroup Root;
        private DevExpress.XtraGrid.GridControl ProductVariantIdentifierGridControl;
        private System.Windows.Forms.BindingSource productVariantIdentifierItemBindingSource;
        private DevExpress.XtraGrid.Views.Grid.GridView ProductVariantIdentifierGridView;
        private DevExpress.XtraGrid.Columns.GridColumn ProductVariantIdentifierEnumGridColumn;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox ProductVariantIdentifierEnumComboBox;
        private DevExpress.XtraGrid.Columns.GridColumn ProductVariantIdentifierValueGridColumn;
        private LayoutControlItem layoutControlItem2;
        private SimpleLabelItem ProductVariantFullNameSimpleLabelItem;
        private BarHeaderItem barHeaderItem1;
        private BarStaticItem SoLuongNhapXuatBarStaticItem;
        private DevExpress.XtraEditors.ComboBoxEdit InputTypeComboBoxEdit;
        private LayoutControlItem layoutControlItem1;
    }
}