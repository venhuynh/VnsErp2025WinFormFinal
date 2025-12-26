namespace DeviceAssetManagement.Management.DeviceMangement
{
    partial class UcDeviceDtoAddEdit
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
            this.SaveHyperlinkLabelControl = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.IdentifierValueGridControl = new DevExpress.XtraGrid.GridControl();
            this.IdentifierValueGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.DeviceIdentifierEnumGridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DeviceIdentifierEnumComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.IdentifierValueGridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.XoaHyperlinkLabelControl = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.ProductVariantSearchLookUpEdit = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.productVariantListDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.identifierValueBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ProductVariantSearchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colVariantFullName = new DevExpress.XtraGrid.Columns.GridColumn();
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
            ((System.ComponentModel.ISupportInitialize)(this.IdentifierValueGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IdentifierValueGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceIdentifierEnumComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantSearchLookUpEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productVariantListDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.identifierValueBindingSource)).BeginInit();
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
            this.layoutControl1.Controls.Add(this.SaveHyperlinkLabelControl);
            this.layoutControl1.Controls.Add(this.IdentifierValueGridControl);
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
            // SaveHyperlinkLabelControl
            // 
            this.SaveHyperlinkLabelControl.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.SaveHyperlinkLabelControl.Location = new System.Drawing.Point(20, 81);
            this.SaveHyperlinkLabelControl.Name = "SaveHyperlinkLabelControl";
            this.SaveHyperlinkLabelControl.Size = new System.Drawing.Size(39, 20);
            this.SaveHyperlinkLabelControl.StyleController = this.layoutControl1;
            this.SaveHyperlinkLabelControl.TabIndex = 8;
            this.SaveHyperlinkLabelControl.Text = "Lưu";
            // 
            // IdentifierValueGridControl
            // 
            this.IdentifierValueGridControl.DataSource = this.identifierValueBindingSource;
            this.IdentifierValueGridControl.Location = new System.Drawing.Point(12, 113);
            this.IdentifierValueGridControl.MainView = this.IdentifierValueGridView;
            this.IdentifierValueGridControl.Name = "IdentifierValueGridControl";
            this.IdentifierValueGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.DeviceIdentifierEnumComboBox});
            this.IdentifierValueGridControl.Size = new System.Drawing.Size(427, 510);
            this.IdentifierValueGridControl.TabIndex = 7;
            this.IdentifierValueGridControl.UseEmbeddedNavigator = true;
            this.IdentifierValueGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.IdentifierValueGridView});
            // 
            // IdentifierValueGridView
            // 
            this.IdentifierValueGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.DeviceIdentifierEnumGridColumn,
            this.IdentifierValueGridColumn});
            this.IdentifierValueGridView.GridControl = this.IdentifierValueGridControl;
            this.IdentifierValueGridView.Name = "IdentifierValueGridView";
            this.IdentifierValueGridView.NewItemRowText = "Click để thêm dòng mới";
            this.IdentifierValueGridView.OptionsMenu.ShowAddNewSummaryItem = DevExpress.Utils.DefaultBoolean.True;
            this.IdentifierValueGridView.OptionsNavigation.AutoFocusNewRow = true;
            this.IdentifierValueGridView.OptionsSelection.MultiSelect = true;
            this.IdentifierValueGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.IdentifierValueGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.IdentifierValueGridView.OptionsView.ShowGroupPanel = false;
            this.IdentifierValueGridView.OptionsView.ShowViewCaption = true;
            this.IdentifierValueGridView.ViewCaption = "BẢNG DANH SÁCH ĐỊNH DANH";
            // 
            // DeviceIdentifierEnumGridColumn
            // 
            this.DeviceIdentifierEnumGridColumn.AppearanceCell.Options.UseTextOptions = true;
            this.DeviceIdentifierEnumGridColumn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.DeviceIdentifierEnumGridColumn.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.DeviceIdentifierEnumGridColumn.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.DeviceIdentifierEnumGridColumn.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.DeviceIdentifierEnumGridColumn.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.DeviceIdentifierEnumGridColumn.AppearanceHeader.Options.UseBackColor = true;
            this.DeviceIdentifierEnumGridColumn.AppearanceHeader.Options.UseFont = true;
            this.DeviceIdentifierEnumGridColumn.AppearanceHeader.Options.UseForeColor = true;
            this.DeviceIdentifierEnumGridColumn.AppearanceHeader.Options.UseTextOptions = true;
            this.DeviceIdentifierEnumGridColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.DeviceIdentifierEnumGridColumn.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.DeviceIdentifierEnumGridColumn.Caption = "Kiểu định danh";
            this.DeviceIdentifierEnumGridColumn.ColumnEdit = this.DeviceIdentifierEnumComboBox;
            this.DeviceIdentifierEnumGridColumn.FieldName = "IdentifierType";
            this.DeviceIdentifierEnumGridColumn.Name = "DeviceIdentifierEnumGridColumn";
            this.DeviceIdentifierEnumGridColumn.Visible = true;
            this.DeviceIdentifierEnumGridColumn.VisibleIndex = 1;
            this.DeviceIdentifierEnumGridColumn.Width = 150;
            // 
            // DeviceIdentifierEnumComboBox
            // 
            this.DeviceIdentifierEnumComboBox.AutoHeight = false;
            this.DeviceIdentifierEnumComboBox.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DeviceIdentifierEnumComboBox.Name = "DeviceIdentifierEnumComboBox";
            // 
            // IdentifierValueGridColumn
            // 
            this.IdentifierValueGridColumn.AppearanceCell.Options.UseTextOptions = true;
            this.IdentifierValueGridColumn.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.IdentifierValueGridColumn.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.IdentifierValueGridColumn.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.IdentifierValueGridColumn.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.IdentifierValueGridColumn.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.IdentifierValueGridColumn.AppearanceHeader.Options.UseBackColor = true;
            this.IdentifierValueGridColumn.AppearanceHeader.Options.UseFont = true;
            this.IdentifierValueGridColumn.AppearanceHeader.Options.UseForeColor = true;
            this.IdentifierValueGridColumn.AppearanceHeader.Options.UseTextOptions = true;
            this.IdentifierValueGridColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.IdentifierValueGridColumn.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.IdentifierValueGridColumn.Caption = "Giá trị";
            this.IdentifierValueGridColumn.FieldName = "Value";
            this.IdentifierValueGridColumn.Name = "IdentifierValueGridColumn";
            this.IdentifierValueGridColumn.Visible = true;
            this.IdentifierValueGridColumn.VisibleIndex = 2;
            this.IdentifierValueGridColumn.Width = 250;
            // 
            // XoaHyperlinkLabelControl
            // 
            this.XoaHyperlinkLabelControl.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.XoaHyperlinkLabelControl.Location = new System.Drawing.Point(79, 81);
            this.XoaHyperlinkLabelControl.Name = "XoaHyperlinkLabelControl";
            this.XoaHyperlinkLabelControl.Size = new System.Drawing.Size(39, 20);
            this.XoaHyperlinkLabelControl.StyleController = this.layoutControl1;
            this.XoaHyperlinkLabelControl.TabIndex = 6;
            this.XoaHyperlinkLabelControl.Text = "Xóa";
            // 
            // ProductVariantSearchLookUpEdit
            // 
            this.ProductVariantSearchLookUpEdit.Location = new System.Drawing.Point(107, 49);
            this.ProductVariantSearchLookUpEdit.Name = "ProductVariantSearchLookUpEdit";
            this.ProductVariantSearchLookUpEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.ProductVariantSearchLookUpEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ProductVariantSearchLookUpEdit.Properties.DataSource = this.productVariantListDtoBindingSource;
            this.ProductVariantSearchLookUpEdit.Properties.DisplayMember = "VariantFullName";
            this.ProductVariantSearchLookUpEdit.Properties.NullText = "";
            this.ProductVariantSearchLookUpEdit.Properties.PopupView = this.ProductVariantSearchLookUpEdit1View;
            this.ProductVariantSearchLookUpEdit.Properties.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.VariantFullNameHypertextLabel});
            this.ProductVariantSearchLookUpEdit.Properties.ValueMember = "Id";
            this.ProductVariantSearchLookUpEdit.Size = new System.Drawing.Size(332, 20);
            this.ProductVariantSearchLookUpEdit.StyleController = this.layoutControl1;
            this.ProductVariantSearchLookUpEdit.TabIndex = 5;
            // 
            // productVariantListDtoBindingSource
            // 
            this.productVariantListDtoBindingSource.DataSource = typeof(DTO.MasterData.ProductService.ProductVariantListDto);
            // 
            // ProductVariantSearchLookUpEdit1View
            // 
            this.ProductVariantSearchLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colVariantFullName});
            this.ProductVariantSearchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.ProductVariantSearchLookUpEdit1View.Name = "ProductVariantSearchLookUpEdit1View";
            this.ProductVariantSearchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.ProductVariantSearchLookUpEdit1View.OptionsView.RowAutoHeight = true;
            this.ProductVariantSearchLookUpEdit1View.OptionsView.ShowAutoFilterRow = true;
            this.ProductVariantSearchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            this.ProductVariantSearchLookUpEdit1View.OptionsView.ShowIndicator = false;
            // 
            // colVariantFullName
            // 
            this.colVariantFullName.AppearanceCell.Options.UseTextOptions = true;
            this.colVariantFullName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colVariantFullName.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colVariantFullName.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colVariantFullName.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colVariantFullName.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colVariantFullName.AppearanceHeader.Options.UseBackColor = true;
            this.colVariantFullName.AppearanceHeader.Options.UseFont = true;
            this.colVariantFullName.AppearanceHeader.Options.UseForeColor = true;
            this.colVariantFullName.AppearanceHeader.Options.UseTextOptions = true;
            this.colVariantFullName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colVariantFullName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colVariantFullName.ColumnEdit = this.VariantFullNameHypertextLabel;
            this.colVariantFullName.FieldName = "VariantFullName";
            this.colVariantFullName.Name = "colVariantFullName";
            this.colVariantFullName.Visible = true;
            this.colVariantFullName.VisibleIndex = 0;
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
            this.labelControl1.Location = new System.Drawing.Point(88, 20);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(275, 17);
            this.labelControl1.StyleController = this.layoutControl1;
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "THÊM ĐỊNH DANH THIẾT BỊ - TÀI SẢN";
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
            this.layoutControlItem2.Text = "Hàng hóa dịch vụ";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(83, 13);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem3.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem3.Control = this.XoaHyperlinkLabelControl;
            this.layoutControlItem3.Location = new System.Drawing.Point(59, 61);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 10, 10, 10);
            this.layoutControlItem3.Size = new System.Drawing.Size(59, 40);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.IdentifierValueGridControl;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 101);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(431, 514);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem5.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem5.Control = this.SaveHyperlinkLabelControl;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 61);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 10, 10, 10);
            this.layoutControlItem5.Size = new System.Drawing.Size(59, 40);
            this.layoutControlItem5.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.Location = new System.Drawing.Point(118, 61);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(313, 40);
            // 
            // UcDeviceDtoAddEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Name = "UcDeviceDtoAddEdit";
            this.Size = new System.Drawing.Size(451, 635);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.IdentifierValueGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IdentifierValueGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceIdentifierEnumComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantSearchLookUpEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productVariantListDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.identifierValueBindingSource)).EndInit();
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
        private DevExpress.XtraGrid.GridControl IdentifierValueGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView IdentifierValueGridView;
        private DevExpress.XtraEditors.HyperlinkLabelControl XoaHyperlinkLabelControl;
        private DevExpress.XtraEditors.SearchLookUpEdit ProductVariantSearchLookUpEdit;
        private DevExpress.XtraGrid.Views.Grid.GridView ProductVariantSearchLookUpEdit1View;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.HyperlinkLabelControl SaveHyperlinkLabelControl;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private System.Windows.Forms.BindingSource productVariantListDtoBindingSource;
        private System.Windows.Forms.BindingSource identifierValueBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colVariantFullName;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel VariantFullNameHypertextLabel;
        private DevExpress.XtraGrid.Columns.GridColumn DeviceIdentifierEnumGridColumn;
        private DevExpress.XtraGrid.Columns.GridColumn IdentifierValueGridColumn;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox DeviceIdentifierEnumComboBox;
    }
}
