using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraLayout;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using DTO.MasterData.CustomerPartner;
using MasterData.Customer.Dto;

namespace MasterData.Customer
{
    partial class FrmBusinessPartnerCategory
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.businessPartnerCategoryDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.BusinessPartnerCategoryDtoGridControl = new DevExpress.XtraGrid.GridControl();
            this.BusinessPartnerCategoryDtoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.colId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCategoryName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colParentId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colParentCategoryName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCategoryType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPartnerCount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLevel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colHasChildren = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFullPath = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCategoryCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsActive = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSortOrder = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCreatedDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCreatedBy = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCreatedByName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colModifiedDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colModifiedBy = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colModifiedByName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCategoryInfoHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CategoryInfoHtmlHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.colFullPathHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FullPathHtmlHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.colAuditInfoHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AuditInfoHtmlHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.DescriptionMemoEdit = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.IsActiveCheckEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.businessPartnerCategoryDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BusinessPartnerCategoryDtoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BusinessPartnerCategoryDtoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CategoryInfoHtmlHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FullPathHtmlHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AuditInfoHtmlHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionMemoEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsActiveCheckEdit)).BeginInit();
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
            this.barDockControlTop.Size = new System.Drawing.Size(1131, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 554);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1131, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 530);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1131, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 530);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.BusinessPartnerCategoryDtoGridControl);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 24);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(1131, 530);
            this.layoutControl1.TabIndex = 4;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // businessPartnerCategoryDtoBindingSource
            // 
            this.businessPartnerCategoryDtoBindingSource.DataSource = typeof(DTO.MasterData.CustomerPartner.BusinessPartnerCategoryDto);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1131, 530);
            this.Root.TextVisible = false;
            // 
            // BusinessPartnerCategoryDtoGridControl
            // 
            this.BusinessPartnerCategoryDtoGridControl.DataSource = this.businessPartnerCategoryDtoBindingSource;
            this.BusinessPartnerCategoryDtoGridControl.Location = new System.Drawing.Point(12, 12);
            this.BusinessPartnerCategoryDtoGridControl.MainView = this.BusinessPartnerCategoryDtoGridView;
            this.BusinessPartnerCategoryDtoGridControl.MenuManager = this.barManager1;
            this.BusinessPartnerCategoryDtoGridControl.Name = "BusinessPartnerCategoryDtoGridControl";
            this.BusinessPartnerCategoryDtoGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.CategoryInfoHtmlHypertextLabel,
            this.FullPathHtmlHypertextLabel,
            this.AuditInfoHtmlHypertextLabel,
            this.DescriptionMemoEdit,
            this.IsActiveCheckEdit});
            this.BusinessPartnerCategoryDtoGridControl.Size = new System.Drawing.Size(1107, 506);
            this.BusinessPartnerCategoryDtoGridControl.TabIndex = 4;
            this.BusinessPartnerCategoryDtoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.BusinessPartnerCategoryDtoGridView});
            // 
            // BusinessPartnerCategoryDtoGridView
            // 
            this.BusinessPartnerCategoryDtoGridView.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.BusinessPartnerCategoryDtoGridView.Appearance.ViewCaption.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.BusinessPartnerCategoryDtoGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.BusinessPartnerCategoryDtoGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.BusinessPartnerCategoryDtoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colCategoryInfoHtml,
            this.colCategoryName,
            this.colCategoryCode,
            this.colDescription,
            this.colIsActive,
            this.colSortOrder,
            this.colPartnerCount,
            this.colParentCategoryName,
            this.colFullPathHtml,
            this.colCreatedDate,
            this.colCreatedByName,
            this.colModifiedDate,
            this.colModifiedByName,
            this.colAuditInfoHtml,
            this.colId,
            this.colParentId,
            this.colCategoryType,
            this.colLevel,
            this.colHasChildren,
            this.colFullPath,
            this.colCreatedBy,
            this.colModifiedBy});
            this.BusinessPartnerCategoryDtoGridView.GridControl = this.BusinessPartnerCategoryDtoGridControl;
            this.BusinessPartnerCategoryDtoGridView.IndicatorWidth = 50;
            this.BusinessPartnerCategoryDtoGridView.Name = "BusinessPartnerCategoryDtoGridView";
            this.BusinessPartnerCategoryDtoGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.BusinessPartnerCategoryDtoGridView.OptionsView.EnableAppearanceOddRow = true;
            this.BusinessPartnerCategoryDtoGridView.OptionsView.RowAutoHeight = true;
            this.BusinessPartnerCategoryDtoGridView.OptionsView.ShowFooter = true;
            this.BusinessPartnerCategoryDtoGridView.OptionsView.ShowGroupPanel = false;
            this.BusinessPartnerCategoryDtoGridView.OptionsView.ShowViewCaption = true;
            this.BusinessPartnerCategoryDtoGridView.ViewCaption = "BẢNG QUẢN LÝ PHÂN LOẠI ĐỐI TÁC";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.BusinessPartnerCategoryDtoGridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1111, 510);
            this.layoutControlItem1.TextVisible = false;
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
            this.colCategoryInfoHtml.Name = "colCategoryInfoHtml";
            this.colCategoryInfoHtml.OptionsColumn.AllowEdit = false;
            this.colCategoryInfoHtml.OptionsColumn.ReadOnly = true;
            this.colCategoryInfoHtml.Visible = true;
            this.colCategoryInfoHtml.VisibleIndex = 0;
            this.colCategoryInfoHtml.Width = 350;
            // 
            // CategoryInfoHtmlHypertextLabel
            // 
            this.CategoryInfoHtmlHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.CategoryInfoHtmlHypertextLabel.Name = "CategoryInfoHtmlHypertextLabel";
            // 
            // colCategoryName
            // 
            this.colCategoryName.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colCategoryName.AppearanceCell.ForeColor = System.Drawing.Color.DarkBlue;
            this.colCategoryName.AppearanceCell.Options.UseFont = true;
            this.colCategoryName.AppearanceCell.Options.UseForeColor = true;
            this.colCategoryName.AppearanceCell.Options.UseTextOptions = true;
            this.colCategoryName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colCategoryName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCategoryName.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colCategoryName.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colCategoryName.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colCategoryName.AppearanceHeader.Options.UseBackColor = true;
            this.colCategoryName.AppearanceHeader.Options.UseFont = true;
            this.colCategoryName.AppearanceHeader.Options.UseForeColor = true;
            this.colCategoryName.AppearanceHeader.Options.UseTextOptions = true;
            this.colCategoryName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCategoryName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCategoryName.Caption = "Tên phân loại";
            this.colCategoryName.FieldName = "CategoryName";
            this.colCategoryName.Name = "colCategoryName";
            this.colCategoryName.Visible = true;
            this.colCategoryName.VisibleIndex = 1;
            this.colCategoryName.Width = 200;
            // 
            // colCategoryCode
            // 
            this.colCategoryCode.AppearanceCell.Options.UseTextOptions = true;
            this.colCategoryCode.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCategoryCode.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCategoryCode.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colCategoryCode.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colCategoryCode.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colCategoryCode.AppearanceHeader.Options.UseBackColor = true;
            this.colCategoryCode.AppearanceHeader.Options.UseFont = true;
            this.colCategoryCode.AppearanceHeader.Options.UseForeColor = true;
            this.colCategoryCode.AppearanceHeader.Options.UseTextOptions = true;
            this.colCategoryCode.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCategoryCode.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCategoryCode.Caption = "Mã danh mục";
            this.colCategoryCode.FieldName = "CategoryCode";
            this.colCategoryCode.Name = "colCategoryCode";
            this.colCategoryCode.Visible = true;
            this.colCategoryCode.VisibleIndex = 2;
            this.colCategoryCode.Width = 120;
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
            this.colDescription.Name = "colDescription";
            this.colDescription.Visible = true;
            this.colDescription.VisibleIndex = 3;
            this.colDescription.Width = 250;
            // 
            // DescriptionMemoEdit
            // 
            this.DescriptionMemoEdit.Name = "DescriptionMemoEdit";
            // 
            // colIsActive
            // 
            this.colIsActive.AppearanceCell.Options.UseTextOptions = true;
            this.colIsActive.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colIsActive.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colIsActive.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colIsActive.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colIsActive.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colIsActive.AppearanceHeader.Options.UseBackColor = true;
            this.colIsActive.AppearanceHeader.Options.UseFont = true;
            this.colIsActive.AppearanceHeader.Options.UseForeColor = true;
            this.colIsActive.AppearanceHeader.Options.UseTextOptions = true;
            this.colIsActive.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colIsActive.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colIsActive.Caption = "Hoạt động";
            this.colIsActive.ColumnEdit = this.IsActiveCheckEdit;
            this.colIsActive.FieldName = "IsActive";
            this.colIsActive.Name = "colIsActive";
            this.colIsActive.OptionsColumn.AllowEdit = false;
            this.colIsActive.OptionsColumn.ReadOnly = true;
            this.colIsActive.Visible = true;
            this.colIsActive.VisibleIndex = 4;
            this.colIsActive.Width = 80;
            // 
            // IsActiveCheckEdit
            // 
            this.IsActiveCheckEdit.Name = "IsActiveCheckEdit";
            // 
            // colSortOrder
            // 
            this.colSortOrder.AppearanceCell.Options.UseTextOptions = true;
            this.colSortOrder.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colSortOrder.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colSortOrder.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colSortOrder.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colSortOrder.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colSortOrder.AppearanceHeader.Options.UseBackColor = true;
            this.colSortOrder.AppearanceHeader.Options.UseFont = true;
            this.colSortOrder.AppearanceHeader.Options.UseForeColor = true;
            this.colSortOrder.AppearanceHeader.Options.UseTextOptions = true;
            this.colSortOrder.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colSortOrder.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colSortOrder.Caption = "Thứ tự";
            this.colSortOrder.FieldName = "SortOrder";
            this.colSortOrder.Name = "colSortOrder";
            this.colSortOrder.Visible = true;
            this.colSortOrder.VisibleIndex = 5;
            this.colSortOrder.Width = 80;
            // 
            // colPartnerCount
            // 
            this.colPartnerCount.AppearanceCell.Options.UseTextOptions = true;
            this.colPartnerCount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPartnerCount.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colPartnerCount.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colPartnerCount.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colPartnerCount.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colPartnerCount.AppearanceHeader.Options.UseBackColor = true;
            this.colPartnerCount.AppearanceHeader.Options.UseFont = true;
            this.colPartnerCount.AppearanceHeader.Options.UseForeColor = true;
            this.colPartnerCount.AppearanceHeader.Options.UseTextOptions = true;
            this.colPartnerCount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPartnerCount.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colPartnerCount.Caption = "Số lượng đối tác";
            this.colPartnerCount.DisplayFormat.FormatString = "N0";
            this.colPartnerCount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colPartnerCount.FieldName = "PartnerCount";
            this.colPartnerCount.Name = "colPartnerCount";
            this.colPartnerCount.OptionsColumn.AllowEdit = false;
            this.colPartnerCount.OptionsColumn.ReadOnly = true;
            this.colPartnerCount.Summary.AddRange(new DevExpress.XtraGrid.GridColumnSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "PartnerCount", "Tổng: {0:N0}")});
            this.colPartnerCount.Visible = true;
            this.colPartnerCount.VisibleIndex = 6;
            this.colPartnerCount.Width = 100;
            // 
            // colParentCategoryName
            // 
            this.colParentCategoryName.AppearanceCell.Options.UseTextOptions = true;
            this.colParentCategoryName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colParentCategoryName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colParentCategoryName.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colParentCategoryName.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colParentCategoryName.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colParentCategoryName.AppearanceHeader.Options.UseBackColor = true;
            this.colParentCategoryName.AppearanceHeader.Options.UseFont = true;
            this.colParentCategoryName.AppearanceHeader.Options.UseForeColor = true;
            this.colParentCategoryName.AppearanceHeader.Options.UseTextOptions = true;
            this.colParentCategoryName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colParentCategoryName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colParentCategoryName.Caption = "Danh mục cha";
            this.colParentCategoryName.FieldName = "ParentCategoryName";
            this.colParentCategoryName.Name = "colParentCategoryName";
            this.colParentCategoryName.Visible = true;
            this.colParentCategoryName.VisibleIndex = 7;
            this.colParentCategoryName.Width = 150;
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
            this.colFullPathHtml.Name = "colFullPathHtml";
            this.colFullPathHtml.OptionsColumn.AllowEdit = false;
            this.colFullPathHtml.OptionsColumn.ReadOnly = true;
            this.colFullPathHtml.Visible = false;
            this.colFullPathHtml.VisibleIndex = 8;
            this.colFullPathHtml.Width = 300;
            // 
            // FullPathHtmlHypertextLabel
            // 
            this.FullPathHtmlHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.FullPathHtmlHypertextLabel.Name = "FullPathHtmlHypertextLabel";
            // 
            // colCreatedDate
            // 
            this.colCreatedDate.AppearanceCell.Options.UseTextOptions = true;
            this.colCreatedDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCreatedDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCreatedDate.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colCreatedDate.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colCreatedDate.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colCreatedDate.AppearanceHeader.Options.UseBackColor = true;
            this.colCreatedDate.AppearanceHeader.Options.UseFont = true;
            this.colCreatedDate.AppearanceHeader.Options.UseForeColor = true;
            this.colCreatedDate.AppearanceHeader.Options.UseTextOptions = true;
            this.colCreatedDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCreatedDate.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCreatedDate.Caption = "Ngày tạo";
            this.colCreatedDate.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm";
            this.colCreatedDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colCreatedDate.FieldName = "CreatedDate";
            this.colCreatedDate.Name = "colCreatedDate";
            this.colCreatedDate.OptionsColumn.AllowEdit = false;
            this.colCreatedDate.OptionsColumn.ReadOnly = true;
            this.colCreatedDate.Visible = true;
            this.colCreatedDate.VisibleIndex = 9;
            this.colCreatedDate.Width = 130;
            // 
            // colCreatedByName
            // 
            this.colCreatedByName.AppearanceCell.Options.UseTextOptions = true;
            this.colCreatedByName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colCreatedByName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCreatedByName.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colCreatedByName.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colCreatedByName.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colCreatedByName.AppearanceHeader.Options.UseBackColor = true;
            this.colCreatedByName.AppearanceHeader.Options.UseFont = true;
            this.colCreatedByName.AppearanceHeader.Options.UseForeColor = true;
            this.colCreatedByName.AppearanceHeader.Options.UseTextOptions = true;
            this.colCreatedByName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCreatedByName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCreatedByName.Caption = "Người tạo";
            this.colCreatedByName.FieldName = "CreatedByName";
            this.colCreatedByName.Name = "colCreatedByName";
            this.colCreatedByName.OptionsColumn.AllowEdit = false;
            this.colCreatedByName.OptionsColumn.ReadOnly = true;
            this.colCreatedByName.Visible = true;
            this.colCreatedByName.VisibleIndex = 10;
            this.colCreatedByName.Width = 120;
            // 
            // colModifiedDate
            // 
            this.colModifiedDate.AppearanceCell.Options.UseTextOptions = true;
            this.colModifiedDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colModifiedDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colModifiedDate.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colModifiedDate.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colModifiedDate.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colModifiedDate.AppearanceHeader.Options.UseBackColor = true;
            this.colModifiedDate.AppearanceHeader.Options.UseFont = true;
            this.colModifiedDate.AppearanceHeader.Options.UseForeColor = true;
            this.colModifiedDate.AppearanceHeader.Options.UseTextOptions = true;
            this.colModifiedDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colModifiedDate.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colModifiedDate.Caption = "Ngày cập nhật";
            this.colModifiedDate.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm";
            this.colModifiedDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colModifiedDate.FieldName = "ModifiedDate";
            this.colModifiedDate.Name = "colModifiedDate";
            this.colModifiedDate.OptionsColumn.AllowEdit = false;
            this.colModifiedDate.OptionsColumn.ReadOnly = true;
            this.colModifiedDate.Visible = true;
            this.colModifiedDate.VisibleIndex = 11;
            this.colModifiedDate.Width = 130;
            // 
            // colModifiedByName
            // 
            this.colModifiedByName.AppearanceCell.Options.UseTextOptions = true;
            this.colModifiedByName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colModifiedByName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colModifiedByName.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colModifiedByName.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colModifiedByName.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colModifiedByName.AppearanceHeader.Options.UseBackColor = true;
            this.colModifiedByName.AppearanceHeader.Options.UseFont = true;
            this.colModifiedByName.AppearanceHeader.Options.UseForeColor = true;
            this.colModifiedByName.AppearanceHeader.Options.UseTextOptions = true;
            this.colModifiedByName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colModifiedByName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colModifiedByName.Caption = "Người cập nhật";
            this.colModifiedByName.FieldName = "ModifiedByName";
            this.colModifiedByName.Name = "colModifiedByName";
            this.colModifiedByName.OptionsColumn.AllowEdit = false;
            this.colModifiedByName.OptionsColumn.ReadOnly = true;
            this.colModifiedByName.Visible = true;
            this.colModifiedByName.VisibleIndex = 12;
            this.colModifiedByName.Width = 120;
            // 
            // colAuditInfoHtml
            // 
            this.colAuditInfoHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colAuditInfoHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colAuditInfoHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colAuditInfoHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colAuditInfoHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colAuditInfoHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colAuditInfoHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colAuditInfoHtml.AppearanceHeader.Options.UseFont = true;
            this.colAuditInfoHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colAuditInfoHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colAuditInfoHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colAuditInfoHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colAuditInfoHtml.Caption = "Thông tin audit";
            this.colAuditInfoHtml.ColumnEdit = this.AuditInfoHtmlHypertextLabel;
            this.colAuditInfoHtml.FieldName = "AuditInfoHtml";
            this.colAuditInfoHtml.Name = "colAuditInfoHtml";
            this.colAuditInfoHtml.OptionsColumn.AllowEdit = false;
            this.colAuditInfoHtml.OptionsColumn.ReadOnly = true;
            this.colAuditInfoHtml.Visible = false;
            this.colAuditInfoHtml.VisibleIndex = 13;
            this.colAuditInfoHtml.Width = 250;
            // 
            // AuditInfoHtmlHypertextLabel
            // 
            this.AuditInfoHtmlHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.AuditInfoHtmlHypertextLabel.Name = "AuditInfoHtmlHypertextLabel";
            // 
            // colId
            // 
            this.colId.FieldName = "Id";
            this.colId.Name = "colId";
            this.colId.OptionsColumn.AllowEdit = false;
            this.colId.OptionsColumn.ReadOnly = true;
            this.colId.Visible = false;
            this.colId.VisibleIndex = 14;
            // 
            // colParentId
            // 
            this.colParentId.FieldName = "ParentId";
            this.colParentId.Name = "colParentId";
            this.colParentId.OptionsColumn.AllowEdit = false;
            this.colParentId.OptionsColumn.ReadOnly = true;
            this.colParentId.Visible = false;
            this.colParentId.VisibleIndex = 15;
            // 
            // colCategoryType
            // 
            this.colCategoryType.FieldName = "CategoryType";
            this.colCategoryType.Name = "colCategoryType";
            this.colCategoryType.OptionsColumn.AllowEdit = false;
            this.colCategoryType.OptionsColumn.ReadOnly = true;
            this.colCategoryType.Visible = false;
            this.colCategoryType.VisibleIndex = 16;
            // 
            // colLevel
            // 
            this.colLevel.FieldName = "Level";
            this.colLevel.Name = "colLevel";
            this.colLevel.OptionsColumn.AllowEdit = false;
            this.colLevel.OptionsColumn.ReadOnly = true;
            this.colLevel.Visible = false;
            this.colLevel.VisibleIndex = 17;
            // 
            // colHasChildren
            // 
            this.colHasChildren.FieldName = "HasChildren";
            this.colHasChildren.Name = "colHasChildren";
            this.colHasChildren.OptionsColumn.AllowEdit = false;
            this.colHasChildren.OptionsColumn.ReadOnly = true;
            this.colHasChildren.Visible = false;
            this.colHasChildren.VisibleIndex = 18;
            // 
            // colFullPath
            // 
            this.colFullPath.FieldName = "FullPath";
            this.colFullPath.Name = "colFullPath";
            this.colFullPath.OptionsColumn.AllowEdit = false;
            this.colFullPath.OptionsColumn.ReadOnly = true;
            this.colFullPath.Visible = false;
            this.colFullPath.VisibleIndex = 19;
            // 
            // colCreatedBy
            // 
            this.colCreatedBy.FieldName = "CreatedBy";
            this.colCreatedBy.Name = "colCreatedBy";
            this.colCreatedBy.OptionsColumn.AllowEdit = false;
            this.colCreatedBy.OptionsColumn.ReadOnly = true;
            this.colCreatedBy.Visible = false;
            this.colCreatedBy.VisibleIndex = 20;
            // 
            // colModifiedBy
            // 
            this.colModifiedBy.FieldName = "ModifiedBy";
            this.colModifiedBy.Name = "colModifiedBy";
            this.colModifiedBy.OptionsColumn.AllowEdit = false;
            this.colModifiedBy.OptionsColumn.ReadOnly = true;
            this.colModifiedBy.Visible = false;
            this.colModifiedBy.VisibleIndex = 21;
            // 
            // FrmBusinessPartnerCategory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1131, 554);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FrmBusinessPartnerCategory";
            this.Text = "PHÂN LOẠI KHÁCH HÀNG - ĐỐI TÁC";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.businessPartnerCategoryDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BusinessPartnerCategoryDtoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BusinessPartnerCategoryDtoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CategoryInfoHtmlHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FullPathHtmlHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AuditInfoHtmlHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionMemoEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsActiveCheckEdit)).EndInit();
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
        
        private BindingSource businessPartnerCategoryDtoBindingSource;
        private DevExpress.XtraGrid.GridControl BusinessPartnerCategoryDtoGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView BusinessPartnerCategoryDtoGridView;
        private LayoutControlItem layoutControlItem1;
        private DevExpress.XtraGrid.Columns.GridColumn colId;
        private DevExpress.XtraGrid.Columns.GridColumn colCategoryName;
        private DevExpress.XtraGrid.Columns.GridColumn colDescription;
        private DevExpress.XtraGrid.Columns.GridColumn colParentId;
        private DevExpress.XtraGrid.Columns.GridColumn colParentCategoryName;
        private DevExpress.XtraGrid.Columns.GridColumn colCategoryType;
        private DevExpress.XtraGrid.Columns.GridColumn colPartnerCount;
        private DevExpress.XtraGrid.Columns.GridColumn colLevel;
        private DevExpress.XtraGrid.Columns.GridColumn colHasChildren;
        private DevExpress.XtraGrid.Columns.GridColumn colFullPath;
        private DevExpress.XtraGrid.Columns.GridColumn colCategoryCode;
        private DevExpress.XtraGrid.Columns.GridColumn colIsActive;
        private DevExpress.XtraGrid.Columns.GridColumn colSortOrder;
        private DevExpress.XtraGrid.Columns.GridColumn colCreatedDate;
        private DevExpress.XtraGrid.Columns.GridColumn colCreatedBy;
        private DevExpress.XtraGrid.Columns.GridColumn colCreatedByName;
        private DevExpress.XtraGrid.Columns.GridColumn colModifiedDate;
        private DevExpress.XtraGrid.Columns.GridColumn colModifiedBy;
        private DevExpress.XtraGrid.Columns.GridColumn colModifiedByName;
        private DevExpress.XtraGrid.Columns.GridColumn colCategoryInfoHtml;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel CategoryInfoHtmlHypertextLabel;
        private DevExpress.XtraGrid.Columns.GridColumn colFullPathHtml;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel FullPathHtmlHypertextLabel;
        private DevExpress.XtraGrid.Columns.GridColumn colAuditInfoHtml;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel AuditInfoHtmlHypertextLabel;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit DescriptionMemoEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit IsActiveCheckEdit;
    }
}
