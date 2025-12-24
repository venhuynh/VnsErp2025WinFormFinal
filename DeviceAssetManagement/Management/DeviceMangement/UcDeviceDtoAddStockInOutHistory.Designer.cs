namespace DeviceAssetManagement.Management.DeviceMangement
{
    partial class UcDeviceDtoAddStockInOutHistory
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
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.ThemVaoHyperlinkLabelControl = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.LichSuNhapXuatGridControl = new DevExpress.XtraGrid.GridControl();
            this.stockInOutMasterHistoryDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.LichSuNhapXuatGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colFullContentHtml1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FullContentHtmlRepositoryItemHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.DeviceIdentifierEnumComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.XoaHyperlinkLabelControl = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.ProductVariantSearchLookUpEdit = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.ProductVariantSearchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colFullContentHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.VariantFullNameHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LichSuNhapXuatGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockInOutMasterHistoryDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LichSuNhapXuatGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FullContentHtmlRepositoryItemHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceIdentifierEnumComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantSearchLookUpEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantSearchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VariantFullNameHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.ThemVaoHyperlinkLabelControl);
            this.layoutControl1.Controls.Add(this.LichSuNhapXuatGridControl);
            this.layoutControl1.Controls.Add(this.XoaHyperlinkLabelControl);
            this.layoutControl1.Controls.Add(this.ProductVariantSearchLookUpEdit);
            this.layoutControl1.Controls.Add(this.labelControl1);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(451, 635);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // ThemVaoHyperlinkLabelControl
            // 
            this.ThemVaoHyperlinkLabelControl.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.ThemVaoHyperlinkLabelControl.Location = new System.Drawing.Point(20, 81);
            this.ThemVaoHyperlinkLabelControl.Name = "ThemVaoHyperlinkLabelControl";
            this.ThemVaoHyperlinkLabelControl.Size = new System.Drawing.Size(47, 20);
            this.ThemVaoHyperlinkLabelControl.StyleController = this.layoutControl1;
            this.ThemVaoHyperlinkLabelControl.TabIndex = 8;
            this.ThemVaoHyperlinkLabelControl.Text = "Thêm";
            // 
            // LichSuNhapXuatGridControl
            // 
            this.LichSuNhapXuatGridControl.DataSource = this.stockInOutMasterHistoryDtoBindingSource;
            this.LichSuNhapXuatGridControl.Location = new System.Drawing.Point(12, 113);
            this.LichSuNhapXuatGridControl.MainView = this.LichSuNhapXuatGridView;
            this.LichSuNhapXuatGridControl.Name = "LichSuNhapXuatGridControl";
            this.LichSuNhapXuatGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.DeviceIdentifierEnumComboBox});
            this.LichSuNhapXuatGridControl.Size = new System.Drawing.Size(427, 510);
            this.LichSuNhapXuatGridControl.TabIndex = 7;
            this.LichSuNhapXuatGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.LichSuNhapXuatGridView});
            // 
            // stockInOutMasterHistoryDtoBindingSource
            // 
            this.stockInOutMasterHistoryDtoBindingSource.DataSource = typeof(DTO.Inventory.InventoryManagement.StockInOutMasterHistoryDto);
            // 
            // LichSuNhapXuatGridView
            // 
            this.LichSuNhapXuatGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colFullContentHtml1});
            this.LichSuNhapXuatGridView.GridControl = this.LichSuNhapXuatGridControl;
            this.LichSuNhapXuatGridView.Name = "LichSuNhapXuatGridView";
            this.LichSuNhapXuatGridView.NewItemRowText = "Click để thêm dòng mới";
            this.LichSuNhapXuatGridView.OptionsMenu.ShowAddNewSummaryItem = DevExpress.Utils.DefaultBoolean.False;
            this.LichSuNhapXuatGridView.OptionsSelection.MultiSelect = true;
            this.LichSuNhapXuatGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.LichSuNhapXuatGridView.OptionsView.RowAutoHeight = true;
            this.LichSuNhapXuatGridView.OptionsView.ShowGroupPanel = false;
            this.LichSuNhapXuatGridView.OptionsView.ShowViewCaption = true;
            this.LichSuNhapXuatGridView.ViewCaption = "LỊCH SỬ NHẬP XUẤT";
            // 
            // colFullContentHtml1
            // 
            this.colFullContentHtml1.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colFullContentHtml1.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colFullContentHtml1.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colFullContentHtml1.AppearanceHeader.Options.UseBackColor = true;
            this.colFullContentHtml1.AppearanceHeader.Options.UseFont = true;
            this.colFullContentHtml1.AppearanceHeader.Options.UseForeColor = true;
            this.colFullContentHtml1.AppearanceHeader.Options.UseTextOptions = true;
            this.colFullContentHtml1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colFullContentHtml1.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colFullContentHtml1.Caption = "Lịch sử giao dịch";
            this.colFullContentHtml1.ColumnEdit = this.FullContentHtmlRepositoryItemHypertextLabel;
            this.colFullContentHtml1.FieldName = "FullContentHtml";
            this.colFullContentHtml1.Name = "colFullContentHtml1";
            this.colFullContentHtml1.OptionsColumn.AllowEdit = false;
            this.colFullContentHtml1.Visible = true;
            this.colFullContentHtml1.VisibleIndex = 1;
            this.colFullContentHtml1.Width = 800;
            // 
            // FullContentHtmlRepositoryItemHypertextLabel
            // 
            this.FullContentHtmlRepositoryItemHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.FullContentHtmlRepositoryItemHypertextLabel.Name = "FullContentHtmlRepositoryItemHypertextLabel";
            // 
            // DeviceIdentifierEnumComboBox
            // 
            this.DeviceIdentifierEnumComboBox.AutoHeight = false;
            this.DeviceIdentifierEnumComboBox.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DeviceIdentifierEnumComboBox.Name = "DeviceIdentifierEnumComboBox";
            // 
            // XoaHyperlinkLabelControl
            // 
            this.XoaHyperlinkLabelControl.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.XoaHyperlinkLabelControl.Location = new System.Drawing.Point(87, 81);
            this.XoaHyperlinkLabelControl.Name = "XoaHyperlinkLabelControl";
            this.XoaHyperlinkLabelControl.Size = new System.Drawing.Size(39, 20);
            this.XoaHyperlinkLabelControl.StyleController = this.layoutControl1;
            this.XoaHyperlinkLabelControl.TabIndex = 6;
            this.XoaHyperlinkLabelControl.Text = "Xóa";
            // 
            // ProductVariantSearchLookUpEdit
            // 
            this.ProductVariantSearchLookUpEdit.Location = new System.Drawing.Point(109, 49);
            this.ProductVariantSearchLookUpEdit.Name = "ProductVariantSearchLookUpEdit";
            this.ProductVariantSearchLookUpEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.ProductVariantSearchLookUpEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ProductVariantSearchLookUpEdit.Properties.DataSource = this.stockInOutMasterHistoryDtoBindingSource;
            this.ProductVariantSearchLookUpEdit.Properties.DisplayMember = "FullContentHtml";
            this.ProductVariantSearchLookUpEdit.Properties.NullText = "";
            this.ProductVariantSearchLookUpEdit.Properties.PopupView = this.ProductVariantSearchLookUpEdit1View;
            this.ProductVariantSearchLookUpEdit.Properties.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.VariantFullNameHypertextLabel});
            this.ProductVariantSearchLookUpEdit.Properties.ValueMember = "Id";
            this.ProductVariantSearchLookUpEdit.Size = new System.Drawing.Size(330, 20);
            this.ProductVariantSearchLookUpEdit.StyleController = this.layoutControl1;
            this.ProductVariantSearchLookUpEdit.TabIndex = 5;
            // 
            // ProductVariantSearchLookUpEdit1View
            // 
            this.ProductVariantSearchLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colFullContentHtml});
            this.ProductVariantSearchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.ProductVariantSearchLookUpEdit1View.Name = "ProductVariantSearchLookUpEdit1View";
            this.ProductVariantSearchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.ProductVariantSearchLookUpEdit1View.OptionsView.RowAutoHeight = true;
            this.ProductVariantSearchLookUpEdit1View.OptionsView.ShowAutoFilterRow = true;
            this.ProductVariantSearchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            this.ProductVariantSearchLookUpEdit1View.OptionsView.ShowIndicator = false;
            // 
            // colFullContentHtml
            // 
            this.colFullContentHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colFullContentHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colFullContentHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colFullContentHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colFullContentHtml.AppearanceHeader.Options.UseFont = true;
            this.colFullContentHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colFullContentHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colFullContentHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colFullContentHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colFullContentHtml.Caption = "Thông tin phiếu nhập xuất";
            this.colFullContentHtml.ColumnEdit = this.VariantFullNameHypertextLabel;
            this.colFullContentHtml.FieldName = "FullContentHtml";
            this.colFullContentHtml.Name = "colFullContentHtml";
            this.colFullContentHtml.Visible = true;
            this.colFullContentHtml.VisibleIndex = 0;
            // 
            // VariantFullNameHypertextLabel
            // 
            this.VariantFullNameHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.VariantFullNameHypertextLabel.Name = "VariantFullNameHypertextLabel";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(124, 20);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(203, 17);
            this.labelControl1.StyleController = this.layoutControl1;
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "THÊM LỊCH SỬ NHẬP - XUẤT";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.emptySpaceItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(451, 635);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.layoutControlItem1.AppearanceItemCaption.Options.UseForeColor = true;
            this.layoutControlItem1.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem1.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem1.Control = this.labelControl1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 10, 10, 10);
            this.layoutControlItem1.Size = new System.Drawing.Size(431, 37);
            this.layoutControlItem1.Text = "THÊM ĐỊNH DANH THIẾT BỊ - TÀI SẢN";
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.ProductVariantSearchLookUpEdit;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 37);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(431, 24);
            this.layoutControlItem2.Text = "Lịch sử nhập xuất";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(85, 13);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem3.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem3.Control = this.XoaHyperlinkLabelControl;
            this.layoutControlItem3.Location = new System.Drawing.Point(67, 61);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 10, 10, 10);
            this.layoutControlItem3.Size = new System.Drawing.Size(59, 40);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.LichSuNhapXuatGridControl;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 101);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(431, 514);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem5.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem5.Control = this.ThemVaoHyperlinkLabelControl;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 61);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 10, 10, 10);
            this.layoutControlItem5.Size = new System.Drawing.Size(67, 40);
            this.layoutControlItem5.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.Location = new System.Drawing.Point(126, 61);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(305, 40);
            // 
            // UcDeviceDtoAddStockInOutHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Name = "UcDeviceDtoAddStockInOutHistory";
            this.Size = new System.Drawing.Size(451, 635);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LichSuNhapXuatGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockInOutMasterHistoryDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LichSuNhapXuatGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FullContentHtmlRepositoryItemHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceIdentifierEnumComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantSearchLookUpEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantSearchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VariantFullNameHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraGrid.GridControl LichSuNhapXuatGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView LichSuNhapXuatGridView;
        private DevExpress.XtraEditors.HyperlinkLabelControl XoaHyperlinkLabelControl;
        private DevExpress.XtraEditors.SearchLookUpEdit ProductVariantSearchLookUpEdit;
        private DevExpress.XtraGrid.Views.Grid.GridView ProductVariantSearchLookUpEdit1View;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.HyperlinkLabelControl ThemVaoHyperlinkLabelControl;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel VariantFullNameHypertextLabel;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel FullContentHtmlRepositoryItemHypertextLabel;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox DeviceIdentifierEnumComboBox;
        private System.Windows.Forms.BindingSource stockInOutMasterHistoryDtoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colFullContentHtml;
        private DevExpress.XtraGrid.Columns.GridColumn colFullContentHtml1;
    }
}
