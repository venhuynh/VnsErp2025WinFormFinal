using DevExpress.XtraBars;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraLayout;
using System.Windows.Forms;
using DTO.MasterData.Company;

namespace MasterData.Company
{
    partial class FrmDepartment
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
            this.ListDataBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.NewBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.EditBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.DeleteBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ExportBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.DepartmentDtoGridControl = new DevExpress.XtraGrid.GridControl();
            this.departmentDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.DepartmentDtoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colFullPathHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FullPathHtmlHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.colThongTinHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ThongTinHtmlHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.treeListBand1 = new DevExpress.XtraTreeList.Columns.TreeListBand();
            this.treeListBand2 = new DevExpress.XtraTreeList.Columns.TreeListBand();
            this.treeListBand3 = new DevExpress.XtraTreeList.Columns.TreeListBand();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DepartmentDtoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.departmentDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepartmentDtoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FullPathHtmlHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThongTinHtmlHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
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
            this.NewBarButtonItem,
            this.ListDataBarButtonItem,
            this.EditBarButtonItem,
            this.DeleteBarButtonItem,
            this.ExportBarButtonItem});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 6;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1,
            this.repositoryItemComboBox2});
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.ListDataBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.NewBarButtonItem, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.EditBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.DeleteBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.ExportBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // ListDataBarButtonItem
            // 
            this.ListDataBarButtonItem.Caption = "Danh sách";
            this.ListDataBarButtonItem.Id = 1;
            this.ListDataBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.list_16x16;
            this.ListDataBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.list_32x32;
            this.ListDataBarButtonItem.Name = "ListDataBarButtonItem";
            // 
            // NewBarButtonItem
            // 
            this.NewBarButtonItem.Caption = "Mới";
            this.NewBarButtonItem.Id = 0;
            this.NewBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.addnewdatasource_16x16;
            this.NewBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.addnewdatasource_32x32;
            this.NewBarButtonItem.Name = "NewBarButtonItem";
            // 
            // EditBarButtonItem
            // 
            this.EditBarButtonItem.Caption = "Điều chỉnh";
            this.EditBarButtonItem.Id = 2;
            this.EditBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.edittask_16x16;
            this.EditBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.edittask_32x32;
            this.EditBarButtonItem.Name = "EditBarButtonItem";
            // 
            // DeleteBarButtonItem
            // 
            this.DeleteBarButtonItem.Caption = "Xóa";
            this.DeleteBarButtonItem.Id = 3;
            this.DeleteBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.deletelist_16x16;
            this.DeleteBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.deletelist_32x32;
            this.DeleteBarButtonItem.Name = "DeleteBarButtonItem";
            // 
            // ExportBarButtonItem
            // 
            this.ExportBarButtonItem.Caption = "Xuất";
            this.ExportBarButtonItem.Id = 4;
            this.ExportBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.exporttoxls_16x16;
            this.ExportBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.exporttoxls_32x32;
            this.ExportBarButtonItem.Name = "ExportBarButtonItem";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(834, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 489);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(834, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 465);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(834, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 465);
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // repositoryItemComboBox2
            // 
            this.repositoryItemComboBox2.AutoHeight = false;
            this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.DepartmentDtoGridControl);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 24);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(834, 465);
            this.layoutControl1.TabIndex = 4;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // DepartmentDtoGridControl
            // 
            this.DepartmentDtoGridControl.DataSource = this.departmentDtoBindingSource;
            this.DepartmentDtoGridControl.Location = new System.Drawing.Point(12, 12);
            this.DepartmentDtoGridControl.MainView = this.DepartmentDtoGridView;
            this.DepartmentDtoGridControl.MenuManager = this.barManager1;
            this.DepartmentDtoGridControl.Name = "DepartmentDtoGridControl";
            this.DepartmentDtoGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ThongTinHtmlHypertextLabel,
            this.FullPathHtmlHypertextLabel});
            this.DepartmentDtoGridControl.Size = new System.Drawing.Size(810, 441);
            this.DepartmentDtoGridControl.TabIndex = 4;
            this.DepartmentDtoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.DepartmentDtoGridView});
            // 
            // departmentDtoBindingSource
            // 
            this.departmentDtoBindingSource.DataSource = typeof(DTO.MasterData.Company.DepartmentDto);
            // 
            // DepartmentDtoGridView
            // 
            this.DepartmentDtoGridView.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.DepartmentDtoGridView.Appearance.ViewCaption.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.DepartmentDtoGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.DepartmentDtoGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.DepartmentDtoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colFullPathHtml,
            this.colThongTinHtml});
            this.DepartmentDtoGridView.GridControl = this.DepartmentDtoGridControl;
            this.DepartmentDtoGridView.IndicatorWidth = 50;
            this.DepartmentDtoGridView.Name = "DepartmentDtoGridView";
            this.DepartmentDtoGridView.OptionsFind.AlwaysVisible = true;
            this.DepartmentDtoGridView.OptionsSelection.MultiSelect = true;
            this.DepartmentDtoGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.DepartmentDtoGridView.OptionsView.ColumnAutoWidth = false;
            this.DepartmentDtoGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.DepartmentDtoGridView.OptionsView.EnableAppearanceOddRow = true;
            this.DepartmentDtoGridView.OptionsView.RowAutoHeight = true;
            this.DepartmentDtoGridView.OptionsView.ShowGroupPanel = false;
            this.DepartmentDtoGridView.OptionsView.ShowViewCaption = true;
            this.DepartmentDtoGridView.ViewCaption = "BẢNG QUẢN LÝ PHÒNG BAN";
            // 
            // colFullPathHtml
            // 
            this.colFullPathHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colFullPathHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colFullPathHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colFullPathHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colFullPathHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colFullPathHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colFullPathHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colFullPathHtml.AppearanceHeader.Options.UseFont = true;
            this.colFullPathHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colFullPathHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colFullPathHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colFullPathHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colFullPathHtml.Caption = "Phòng ban cấp trên";
            this.colFullPathHtml.ColumnEdit = this.FullPathHtmlHypertextLabel;
            this.colFullPathHtml.FieldName = "FullPathHtml";
            this.colFullPathHtml.MinWidth = 200;
            this.colFullPathHtml.Name = "colFullPathHtml";
            this.colFullPathHtml.OptionsColumn.AllowEdit = false;
            this.colFullPathHtml.OptionsColumn.FixedWidth = true;
            this.colFullPathHtml.OptionsColumn.ReadOnly = true;
            this.colFullPathHtml.Visible = true;
            this.colFullPathHtml.VisibleIndex = 1;
            this.colFullPathHtml.Width = 280;
            // 
            // FullPathHtmlHypertextLabel
            // 
            this.FullPathHtmlHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.FullPathHtmlHypertextLabel.Name = "FullPathHtmlHypertextLabel";
            // 
            // colThongTinHtml
            // 
            this.colThongTinHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colThongTinHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colThongTinHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colThongTinHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colThongTinHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colThongTinHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colThongTinHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colThongTinHtml.AppearanceHeader.Options.UseFont = true;
            this.colThongTinHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colThongTinHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colThongTinHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colThongTinHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colThongTinHtml.Caption = "Thông tin phòng ban";
            this.colThongTinHtml.ColumnEdit = this.ThongTinHtmlHypertextLabel;
            this.colThongTinHtml.FieldName = "ThongTinHtml";
            this.colThongTinHtml.MinWidth = 300;
            this.colThongTinHtml.Name = "colThongTinHtml";
            this.colThongTinHtml.OptionsColumn.AllowEdit = false;
            this.colThongTinHtml.OptionsColumn.FixedWidth = true;
            this.colThongTinHtml.OptionsColumn.ReadOnly = true;
            this.colThongTinHtml.Visible = true;
            this.colThongTinHtml.VisibleIndex = 2;
            this.colThongTinHtml.Width = 500;
            // 
            // ThongTinHtmlHypertextLabel
            // 
            this.ThongTinHtmlHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.ThongTinHtmlHypertextLabel.Name = "ThongTinHtmlHypertextLabel";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(834, 465);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.DepartmentDtoGridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(814, 445);
            this.layoutControlItem1.TextVisible = false;
            // 
            // treeListBand1
            // 
            this.treeListBand1.Caption = "Thông tin Phòng ban";
            this.treeListBand1.Fixed = DevExpress.XtraTreeList.Columns.FixedStyle.Left;
            this.treeListBand1.Name = "treeListBand1";
            this.treeListBand1.Width = 300;
            // 
            // treeListBand2
            // 
            this.treeListBand2.Caption = "Thông tin Công ty";
            this.treeListBand2.Name = "treeListBand2";
            this.treeListBand2.Width = 400;
            // 
            // treeListBand3
            // 
            this.treeListBand3.Caption = "Thống kê";
            this.treeListBand3.Name = "treeListBand3";
            this.treeListBand3.Width = 300;
            // 
            // FrmDepartment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 489);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FrmDepartment";
            this.Text = "PHÒNG BAN";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DepartmentDtoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.departmentDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepartmentDtoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FullPathHtmlHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThongTinHtmlHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BarManager barManager1;
        private Bar bar2;
        private BarDockControl barDockControlTop;
        private BarDockControl barDockControlBottom;
        private BarDockControl barDockControlLeft;
        private BarDockControl barDockControlRight;
        private LayoutControl layoutControl1;
        private LayoutControlGroup Root;
        private BarButtonItem ListDataBarButtonItem;
        private BarButtonItem NewBarButtonItem;
        private BarButtonItem EditBarButtonItem;
        private BarButtonItem DeleteBarButtonItem;
        private BarButtonItem ExportBarButtonItem;
        private RepositoryItemComboBox repositoryItemComboBox1;
        private RepositoryItemComboBox repositoryItemComboBox2;
        private DevExpress.XtraTreeList.Columns.TreeListBand treeListBand1;
        private DevExpress.XtraTreeList.Columns.TreeListBand treeListBand2;
        private DevExpress.XtraTreeList.Columns.TreeListBand treeListBand3;
        private BindingSource departmentDtoBindingSource;
        private DevExpress.XtraGrid.GridControl DepartmentDtoGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView DepartmentDtoGridView;
        private LayoutControlItem layoutControlItem1;
        private DevExpress.XtraGrid.Columns.GridColumn colThongTinHtml;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel ThongTinHtmlHypertextLabel;
        private DevExpress.XtraGrid.Columns.GridColumn colFullPathHtml;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel FullPathHtmlHypertextLabel;
    }
}
