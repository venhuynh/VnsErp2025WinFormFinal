using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraLayout;

namespace VersionAndUserManagement.AllowedMacAddress
{
    partial class FrmAllowedMacAddressDto
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
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barHeaderItem1 = new DevExpress.XtraBars.BarHeaderItem();
            this.DataSummaryBarStaticItem = new DevExpress.XtraBars.BarButtonItem();
            this.barHeaderItem2 = new DevExpress.XtraBars.BarHeaderItem();
            this.CurrentSelectBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.AllowedMacAddressDtoGridControl = new DevExpress.XtraGrid.GridControl();
            this.AllowedMacAddressDtoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colCategoryInfoHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CategoryInfoHtmlHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.colDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DescriptionMemoEdit = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.colFullPathHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FullPathHtmlHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.AuditInfoHtmlHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.IsActiveCheckEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.allowedMacAddressDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AllowedMacAddressDtoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AllowedMacAddressDtoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CategoryInfoHtmlHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionMemoEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FullPathHtmlHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AuditInfoHtmlHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsActiveCheckEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.allowedMacAddressDtoBindingSource)).BeginInit();
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
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.NewBarButtonItem,
            this.ListDataBarButtonItem,
            this.EditBarButtonItem,
            this.DeleteBarButtonItem,
            this.ExportBarButtonItem,
            this.DataSummaryBarStaticItem,
            this.CurrentSelectBarStaticItem,
            this.barHeaderItem1,
            this.barHeaderItem2});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 10;
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
            this.ListDataBarButtonItem.ImageOptions.Image = global::VersionAndUserManagement.Properties.Resources.list_16x16;
            this.ListDataBarButtonItem.ImageOptions.LargeImage = global::VersionAndUserManagement.Properties.Resources.list_32x32;
            this.ListDataBarButtonItem.Name = "ListDataBarButtonItem";
            // 
            // NewBarButtonItem
            // 
            this.NewBarButtonItem.Caption = "Mới";
            this.NewBarButtonItem.Id = 0;
            this.NewBarButtonItem.ImageOptions.Image = global::VersionAndUserManagement.Properties.Resources.addnewdatasource_16x16;
            this.NewBarButtonItem.ImageOptions.LargeImage = global::VersionAndUserManagement.Properties.Resources.addnewdatasource_32x32;
            this.NewBarButtonItem.Name = "NewBarButtonItem";
            // 
            // EditBarButtonItem
            // 
            this.EditBarButtonItem.Caption = "Điều chỉnh";
            this.EditBarButtonItem.Id = 2;
            this.EditBarButtonItem.ImageOptions.Image = global::VersionAndUserManagement.Properties.Resources.edittask_16x16;
            this.EditBarButtonItem.ImageOptions.LargeImage = global::VersionAndUserManagement.Properties.Resources.edittask_32x32;
            this.EditBarButtonItem.Name = "EditBarButtonItem";
            // 
            // DeleteBarButtonItem
            // 
            this.DeleteBarButtonItem.Caption = "Xóa";
            this.DeleteBarButtonItem.Id = 3;
            this.DeleteBarButtonItem.ImageOptions.Image = global::VersionAndUserManagement.Properties.Resources.deletelist_16x16;
            this.DeleteBarButtonItem.ImageOptions.LargeImage = global::VersionAndUserManagement.Properties.Resources.deletelist_32x32;
            this.DeleteBarButtonItem.Name = "DeleteBarButtonItem";
            // 
            // ExportBarButtonItem
            // 
            this.ExportBarButtonItem.Caption = "Xuất";
            this.ExportBarButtonItem.Id = 4;
            this.ExportBarButtonItem.ImageOptions.Image = global::VersionAndUserManagement.Properties.Resources.exporttoxls_16x16;
            this.ExportBarButtonItem.ImageOptions.LargeImage = global::VersionAndUserManagement.Properties.Resources.exporttoxls_32x32;
            this.ExportBarButtonItem.Name = "ExportBarButtonItem";
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
            new DevExpress.XtraBars.LinkPersistInfo(this.barHeaderItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.CurrentSelectBarStaticItem)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Custom 3";
            // 
            // barHeaderItem1
            // 
            this.barHeaderItem1.Caption = "Tổng kết:";
            this.barHeaderItem1.Id = 8;
            this.barHeaderItem1.Name = "barHeaderItem1";
            // 
            // DataSummaryBarStaticItem
            // 
            this.DataSummaryBarStaticItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.DataSummaryBarStaticItem.Caption = "Chưa có dữ liệu";
            this.DataSummaryBarStaticItem.Id = 6;
            this.DataSummaryBarStaticItem.Name = "DataSummaryBarStaticItem";
            // 
            // barHeaderItem2
            // 
            this.barHeaderItem2.Caption = "Đang chọn:";
            this.barHeaderItem2.Id = 9;
            this.barHeaderItem2.Name = "barHeaderItem2";
            // 
            // CurrentSelectBarStaticItem
            // 
            this.CurrentSelectBarStaticItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.CurrentSelectBarStaticItem.Caption = "Chưa chọn dòng nào";
            this.CurrentSelectBarStaticItem.Id = 7;
            this.CurrentSelectBarStaticItem.Name = "CurrentSelectBarStaticItem";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1131, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 532);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1131, 22);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 508);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1131, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 508);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.AllowedMacAddressDtoGridControl);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 24);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(1131, 508);
            this.layoutControl1.TabIndex = 4;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // AllowedMacAddressDtoGridControl
            // 
            this.AllowedMacAddressDtoGridControl.DataSource = this.allowedMacAddressDtoBindingSource;
            this.AllowedMacAddressDtoGridControl.Location = new System.Drawing.Point(12, 12);
            this.AllowedMacAddressDtoGridControl.MainView = this.AllowedMacAddressDtoGridView;
            this.AllowedMacAddressDtoGridControl.MenuManager = this.barManager1;
            this.AllowedMacAddressDtoGridControl.Name = "AllowedMacAddressDtoGridControl";
            this.AllowedMacAddressDtoGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.CategoryInfoHtmlHypertextLabel,
            this.FullPathHtmlHypertextLabel,
            this.AuditInfoHtmlHypertextLabel,
            this.DescriptionMemoEdit,
            this.IsActiveCheckEdit});
            this.AllowedMacAddressDtoGridControl.Size = new System.Drawing.Size(1107, 484);
            this.AllowedMacAddressDtoGridControl.TabIndex = 4;
            this.AllowedMacAddressDtoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.AllowedMacAddressDtoGridView});
            // 
            // AllowedMacAddressDtoGridView
            // 
            this.AllowedMacAddressDtoGridView.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.AllowedMacAddressDtoGridView.Appearance.ViewCaption.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.AllowedMacAddressDtoGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.AllowedMacAddressDtoGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.AllowedMacAddressDtoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colCategoryInfoHtml,
            this.colDescription,
            this.colFullPathHtml});
            this.AllowedMacAddressDtoGridView.GridControl = this.AllowedMacAddressDtoGridControl;
            this.AllowedMacAddressDtoGridView.IndicatorWidth = 50;
            this.AllowedMacAddressDtoGridView.Name = "AllowedMacAddressDtoGridView";
            this.AllowedMacAddressDtoGridView.OptionsSelection.MultiSelect = true;
            this.AllowedMacAddressDtoGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.AllowedMacAddressDtoGridView.OptionsView.ColumnAutoWidth = false;
            this.AllowedMacAddressDtoGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.AllowedMacAddressDtoGridView.OptionsView.EnableAppearanceOddRow = true;
            this.AllowedMacAddressDtoGridView.OptionsView.RowAutoHeight = true;
            this.AllowedMacAddressDtoGridView.OptionsView.ShowGroupPanel = false;
            this.AllowedMacAddressDtoGridView.OptionsView.ShowViewCaption = true;
            this.AllowedMacAddressDtoGridView.ViewCaption = "BẢNG QUẢN LÝ PHÂN LOẠI DANH MỤC ĐỐI TÁC";
            // 
            // colCategoryInfoHtml
            // 
            this.colCategoryInfoHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colCategoryInfoHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCategoryInfoHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colCategoryInfoHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colCategoryInfoHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colCategoryInfoHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colCategoryInfoHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colCategoryInfoHtml.AppearanceHeader.Options.UseFont = true;
            this.colCategoryInfoHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colCategoryInfoHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colCategoryInfoHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCategoryInfoHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCategoryInfoHtml.Caption = "Thông tin danh mục";
            this.colCategoryInfoHtml.ColumnEdit = this.CategoryInfoHtmlHypertextLabel;
            this.colCategoryInfoHtml.FieldName = "CategoryInfoHtml";
            this.colCategoryInfoHtml.MinWidth = 300;
            this.colCategoryInfoHtml.Name = "colCategoryInfoHtml";
            this.colCategoryInfoHtml.OptionsColumn.AllowEdit = false;
            this.colCategoryInfoHtml.OptionsColumn.FixedWidth = true;
            this.colCategoryInfoHtml.OptionsColumn.ReadOnly = true;
            this.colCategoryInfoHtml.Visible = true;
            this.colCategoryInfoHtml.VisibleIndex = 2;
            this.colCategoryInfoHtml.Width = 500;
            // 
            // CategoryInfoHtmlHypertextLabel
            // 
            this.CategoryInfoHtmlHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.CategoryInfoHtmlHypertextLabel.Name = "CategoryInfoHtmlHypertextLabel";
            // 
            // colDescription
            // 
            this.colDescription.AppearanceCell.Options.UseTextOptions = true;
            this.colDescription.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colDescription.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colDescription.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colDescription.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colDescription.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colDescription.AppearanceHeader.Options.UseBackColor = true;
            this.colDescription.AppearanceHeader.Options.UseFont = true;
            this.colDescription.AppearanceHeader.Options.UseForeColor = true;
            this.colDescription.AppearanceHeader.Options.UseTextOptions = true;
            this.colDescription.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDescription.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colDescription.Caption = "Mô tả";
            this.colDescription.ColumnEdit = this.DescriptionMemoEdit;
            this.colDescription.FieldName = "Description";
            this.colDescription.MinWidth = 200;
            this.colDescription.Name = "colDescription";
            this.colDescription.Visible = true;
            this.colDescription.VisibleIndex = 3;
            this.colDescription.Width = 280;
            // 
            // DescriptionMemoEdit
            // 
            this.DescriptionMemoEdit.Name = "DescriptionMemoEdit";
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
            this.colFullPathHtml.Caption = "Đường dẫn";
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
            // AuditInfoHtmlHypertextLabel
            // 
            this.AuditInfoHtmlHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.AuditInfoHtmlHypertextLabel.Name = "AuditInfoHtmlHypertextLabel";
            // 
            // IsActiveCheckEdit
            // 
            this.IsActiveCheckEdit.Name = "IsActiveCheckEdit";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1131, 508);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.AllowedMacAddressDtoGridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1111, 488);
            this.layoutControlItem1.TextVisible = false;
            // 
            // allowedMacAddressDtoBindingSource
            // 
            this.allowedMacAddressDtoBindingSource.DataSource = typeof(DTO.VersionAndUserManagementDto.AllowedMacAddressDto);
            // 
            // FrmAllowedMacAddressDto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1131, 554);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FrmAllowedMacAddressDto";
            this.Text = "PHÂN LOẠI KHÁCH HÀNG - ĐỐI TÁC";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AllowedMacAddressDtoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AllowedMacAddressDtoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CategoryInfoHtmlHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionMemoEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FullPathHtmlHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AuditInfoHtmlHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsActiveCheckEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.allowedMacAddressDtoBindingSource)).EndInit();
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
        private DevExpress.XtraGrid.GridControl AllowedMacAddressDtoGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView AllowedMacAddressDtoGridView;
        private LayoutControlItem layoutControlItem1;
        private DevExpress.XtraGrid.Columns.GridColumn colDescription;
        private DevExpress.XtraGrid.Columns.GridColumn colCategoryInfoHtml;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel CategoryInfoHtmlHypertextLabel;
        private DevExpress.XtraGrid.Columns.GridColumn colFullPathHtml;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel FullPathHtmlHypertextLabel;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel AuditInfoHtmlHypertextLabel;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit DescriptionMemoEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit IsActiveCheckEdit;
        private Bar bar1;
        private BarHeaderItem barHeaderItem1;
        private BarButtonItem DataSummaryBarStaticItem;
        private BarHeaderItem barHeaderItem2;
        private BarStaticItem CurrentSelectBarStaticItem;
        private BindingSource allowedMacAddressDtoBindingSource;
    }
}