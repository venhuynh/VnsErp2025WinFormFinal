using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraLayout;

namespace Inventory.ProductVariantIdentifier
{
    partial class UcProductVariantIdentifierTransactionHistory
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
            this.colParentDepartmentName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colDepartmentName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.ProductVariantSearchLookUpEdit = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.stockInOutMasterHistoryDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.PRoductVariantSearchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colFullContentHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemHypertextLabel1 = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.ProductVariantIdentifierGridControl = new DevExpress.XtraGrid.GridControl();
            this.ProductVariantIdentifierGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ProductVariantIdentifierEnumGridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ProductVariantIdentifierEnumComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.ProductVariantIdentifierValueGridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantSearchLookUpEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockInOutMasterHistoryDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PRoductVariantSearchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHypertextLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierEnumComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
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
            this.dataLayoutControl1.Controls.Add(this.ProductVariantSearchLookUpEdit);
            this.dataLayoutControl1.Controls.Add(this.ProductVariantIdentifierGridControl);
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 0);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(691, 521);
            this.dataLayoutControl1.TabIndex = 14;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // ProductVariantSearchLookUpEdit
            // 
            this.ProductVariantSearchLookUpEdit.Location = new System.Drawing.Point(98, 12);
            this.ProductVariantSearchLookUpEdit.Name = "ProductVariantSearchLookUpEdit";
            this.ProductVariantSearchLookUpEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.ProductVariantSearchLookUpEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ProductVariantSearchLookUpEdit.Properties.DataSource = this.stockInOutMasterHistoryDtoBindingSource;
            this.ProductVariantSearchLookUpEdit.Properties.DisplayMember = "FullContentHtml";
            this.ProductVariantSearchLookUpEdit.Properties.PopupView = this.PRoductVariantSearchLookUpEdit1View;
            this.ProductVariantSearchLookUpEdit.Properties.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemHypertextLabel1});
            this.ProductVariantSearchLookUpEdit.Properties.ValueMember = "Id";
            this.ProductVariantSearchLookUpEdit.Size = new System.Drawing.Size(581, 20);
            this.ProductVariantSearchLookUpEdit.StyleController = this.dataLayoutControl1;
            this.ProductVariantSearchLookUpEdit.TabIndex = 45;
            // 
            // stockInOutMasterHistoryDtoBindingSource
            // 
            this.stockInOutMasterHistoryDtoBindingSource.DataSource = typeof(DTO.Inventory.InventoryManagement.StockInOutMasterHistoryDto);
            // 
            // PRoductVariantSearchLookUpEdit1View
            // 
            this.PRoductVariantSearchLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colFullContentHtml});
            this.PRoductVariantSearchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.PRoductVariantSearchLookUpEdit1View.Name = "PRoductVariantSearchLookUpEdit1View";
            this.PRoductVariantSearchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.PRoductVariantSearchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // colFullContentHtml
            // 
            this.colFullContentHtml.ColumnEdit = this.repositoryItemHypertextLabel1;
            this.colFullContentHtml.FieldName = "FullContentHtml";
            this.colFullContentHtml.Name = "colFullContentHtml";
            this.colFullContentHtml.Visible = true;
            this.colFullContentHtml.VisibleIndex = 0;
            // 
            // repositoryItemHypertextLabel1
            // 
            this.repositoryItemHypertextLabel1.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.repositoryItemHypertextLabel1.Name = "repositoryItemHypertextLabel1";
            // 
            // ProductVariantIdentifierGridControl
            // 
            this.ProductVariantIdentifierGridControl.DataSource = this.stockInOutMasterHistoryDtoBindingSource;
            this.ProductVariantIdentifierGridControl.Location = new System.Drawing.Point(12, 36);
            this.ProductVariantIdentifierGridControl.MainView = this.ProductVariantIdentifierGridView;
            this.ProductVariantIdentifierGridControl.Name = "ProductVariantIdentifierGridControl";
            this.ProductVariantIdentifierGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ProductVariantIdentifierEnumComboBox});
            this.ProductVariantIdentifierGridControl.Size = new System.Drawing.Size(667, 473);
            this.ProductVariantIdentifierGridControl.TabIndex = 44;
            this.ProductVariantIdentifierGridControl.UseEmbeddedNavigator = true;
            this.ProductVariantIdentifierGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ProductVariantIdentifierGridView});
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
            this.ProductVariantIdentifierEnumGridColumn.Width = 200;
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
            this.ProductVariantIdentifierValueGridColumn.Width = 467;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(691, 521);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.ProductVariantIdentifierGridControl;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.OptionsTableLayoutItem.RowIndex = 2;
            this.layoutControlItem2.Size = new System.Drawing.Size(671, 477);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.ProductVariantSearchLookUpEdit;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(671, 24);
            this.layoutControlItem1.Text = "Chọn sản phẩm";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(74, 13);
            // 
            // UcProductVariantIdentifierTransactionHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataLayoutControl1);
            this.Name = "UcProductVariantIdentifierTransactionHistory";
            this.Size = new System.Drawing.Size(691, 521);
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantSearchLookUpEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockInOutMasterHistoryDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PRoductVariantSearchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHypertextLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierEnumComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraTreeList.Columns.TreeListColumn colParentDepartmentName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colDepartmentName;
        private DataLayoutControl dataLayoutControl1;
        private LayoutControlGroup Root;
        private DevExpress.XtraGrid.GridControl ProductVariantIdentifierGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView ProductVariantIdentifierGridView;
        private DevExpress.XtraGrid.Columns.GridColumn ProductVariantIdentifierEnumGridColumn;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox ProductVariantIdentifierEnumComboBox;
        private DevExpress.XtraGrid.Columns.GridColumn ProductVariantIdentifierValueGridColumn;
        private LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.SearchLookUpEdit ProductVariantSearchLookUpEdit;
        private DevExpress.XtraGrid.Views.Grid.GridView PRoductVariantSearchLookUpEdit1View;
        private LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel repositoryItemHypertextLabel1;
        private System.Windows.Forms.BindingSource stockInOutMasterHistoryDtoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colFullContentHtml;
    }
}
