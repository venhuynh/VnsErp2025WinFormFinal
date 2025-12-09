using DevExpress.XtraBars;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using System.ComponentModel;
using System.Windows.Forms;

namespace MasterData.Customer
{
    partial class FrmBusinessPartnerList
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
            this.BusinessPartnerListGridControl = new DevExpress.XtraGrid.GridControl();
            this.businessPartnerListDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.BusinessPartnerListGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colThongTinHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.HtmlHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.colLogoThumbnailData = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCategoryPathHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.PartnerLogoRepositoryItemPictureEdit = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BusinessPartnerListGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.businessPartnerListDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BusinessPartnerListGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PartnerLogoRepositoryItemPictureEdit)).BeginInit();
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
            this.barDockControlTop.Size = new System.Drawing.Size(1075, 39);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 627);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1075, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 39);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 588);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1075, 39);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 588);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.BusinessPartnerListGridControl);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 39);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(1075, 588);
            this.layoutControl1.TabIndex = 4;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // BusinessPartnerListGridControl
            // 
            this.BusinessPartnerListGridControl.DataSource = this.businessPartnerListDtoBindingSource;
            this.BusinessPartnerListGridControl.Location = new System.Drawing.Point(16, 16);
            this.BusinessPartnerListGridControl.MainView = this.BusinessPartnerListGridView;
            this.BusinessPartnerListGridControl.MenuManager = this.barManager1;
            this.BusinessPartnerListGridControl.Name = "BusinessPartnerListGridControl";
            this.BusinessPartnerListGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.HtmlHypertextLabel,
            this.PartnerLogoRepositoryItemPictureEdit});
            this.BusinessPartnerListGridControl.Size = new System.Drawing.Size(1043, 556);
            this.BusinessPartnerListGridControl.TabIndex = 5;
            this.BusinessPartnerListGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.BusinessPartnerListGridView});
            // 
            // businessPartnerListDtoBindingSource
            // 
            this.businessPartnerListDtoBindingSource.DataSource = typeof(DTO.MasterData.CustomerPartner.BusinessPartnerListDto);
            // 
            // BusinessPartnerListGridView
            // 
            this.BusinessPartnerListGridView.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.BusinessPartnerListGridView.Appearance.ViewCaption.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.BusinessPartnerListGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.BusinessPartnerListGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.BusinessPartnerListGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colThongTinHtml,
            this.colLogoThumbnailData,
            this.colCategoryPathHtml});
            this.BusinessPartnerListGridView.GridControl = this.BusinessPartnerListGridControl;
            this.BusinessPartnerListGridView.IndicatorWidth = 40;
            this.BusinessPartnerListGridView.Name = "BusinessPartnerListGridView";
            this.BusinessPartnerListGridView.OptionsSelection.MultiSelect = true;
            this.BusinessPartnerListGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.BusinessPartnerListGridView.OptionsView.ColumnAutoWidth = false;
            this.BusinessPartnerListGridView.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.BusinessPartnerListGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.BusinessPartnerListGridView.OptionsView.EnableAppearanceOddRow = true;
            this.BusinessPartnerListGridView.OptionsView.RowAutoHeight = true;
            this.BusinessPartnerListGridView.OptionsView.ShowAutoFilterRow = true;
            this.BusinessPartnerListGridView.OptionsView.ShowFooter = true;
            this.BusinessPartnerListGridView.OptionsView.ShowGroupPanel = false;
            this.BusinessPartnerListGridView.OptionsView.ShowViewCaption = true;
            this.BusinessPartnerListGridView.ViewCaption = "BẢNG DỮ LIỆU ĐỐI TÁC";
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
            this.colThongTinHtml.Caption = "Thông tin đầy đủ";
            this.colThongTinHtml.ColumnEdit = this.HtmlHypertextLabel;
            this.colThongTinHtml.FieldName = "ThongTinHtml";
            this.colThongTinHtml.MinWidth = 300;
            this.colThongTinHtml.Name = "colThongTinHtml";
            this.colThongTinHtml.Visible = true;
            this.colThongTinHtml.VisibleIndex = 3;
            this.colThongTinHtml.Width = 500;
            // 
            // HtmlHypertextLabel
            // 
            this.HtmlHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.HtmlHypertextLabel.Name = "HtmlHypertextLabel";
            // 
            // colLogoThumbnailData
            // 
            this.colLogoThumbnailData.AppearanceCell.Options.UseTextOptions = true;
            this.colLogoThumbnailData.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colLogoThumbnailData.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colLogoThumbnailData.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colLogoThumbnailData.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colLogoThumbnailData.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colLogoThumbnailData.AppearanceHeader.Options.UseBackColor = true;
            this.colLogoThumbnailData.AppearanceHeader.Options.UseFont = true;
            this.colLogoThumbnailData.AppearanceHeader.Options.UseForeColor = true;
            this.colLogoThumbnailData.AppearanceHeader.Options.UseTextOptions = true;
            this.colLogoThumbnailData.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colLogoThumbnailData.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colLogoThumbnailData.Caption = "Logo";
            this.colLogoThumbnailData.ColumnEdit = this.PartnerLogoRepositoryItemPictureEdit;
            this.colLogoThumbnailData.FieldName = "LogoThumbnailData";
            this.colLogoThumbnailData.MinWidth = 100;
            this.colLogoThumbnailData.Name = "colLogoThumbnailData";
            this.colLogoThumbnailData.OptionsColumn.FixedWidth = true;
            this.colLogoThumbnailData.Visible = true;
            this.colLogoThumbnailData.VisibleIndex = 1;
            this.colLogoThumbnailData.Width = 120;
            // 
            // colCategoryPathHtml
            // 
            this.colCategoryPathHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colCategoryPathHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCategoryPathHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colCategoryPathHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colCategoryPathHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colCategoryPathHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colCategoryPathHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colCategoryPathHtml.AppearanceHeader.Options.UseFont = true;
            this.colCategoryPathHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colCategoryPathHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colCategoryPathHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCategoryPathHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCategoryPathHtml.Caption = "Phân loại";
            this.colCategoryPathHtml.ColumnEdit = this.HtmlHypertextLabel;
            this.colCategoryPathHtml.FieldName = "CategoryPathHtml";
            this.colCategoryPathHtml.MinWidth = 200;
            this.colCategoryPathHtml.Name = "colCategoryPathHtml";
            this.colCategoryPathHtml.Visible = true;
            this.colCategoryPathHtml.VisibleIndex = 2;
            this.colCategoryPathHtml.Width = 280;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1075, 588);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.BusinessPartnerListGridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1049, 562);
            this.layoutControlItem1.TextVisible = false;
            // 
            // PartnerLogoRepositoryItemPictureEdit
            // 
            this.PartnerLogoRepositoryItemPictureEdit.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.PartnerLogoRepositoryItemPictureEdit.Name = "PartnerLogoRepositoryItemPictureEdit";
            this.PartnerLogoRepositoryItemPictureEdit.PictureInterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            this.PartnerLogoRepositoryItemPictureEdit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            this.PartnerLogoRepositoryItemPictureEdit.ImageChanged += new System.EventHandler(this.PartnerLogoRepositoryItemPictureEdit_ImageChanged);
            // 
            // FrmBusinessPartnerList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1075, 627);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FrmBusinessPartnerList";
            this.Text = "DANH SÁCH KHÁCH HÀNG - ĐỐI TÁC";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BusinessPartnerListGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.businessPartnerListDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BusinessPartnerListGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PartnerLogoRepositoryItemPictureEdit)).EndInit();
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
        private GridControl BusinessPartnerListGridControl;
        private GridView BusinessPartnerListGridView;
        private LayoutControlGroup Root;
        private LayoutControlItem layoutControlItem1;
        private BindingSource businessPartnerListDtoBindingSource;
        private BarButtonItem ListDataBarButtonItem;
        private BarButtonItem NewBarButtonItem;
        private BarButtonItem EditBarButtonItem;
        private BarButtonItem DeleteBarButtonItem;
        private BarButtonItem ExportBarButtonItem;
        private GridColumn colThongTinHtml;
        private GridColumn colLogoThumbnailData;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel HtmlHypertextLabel;
        private GridColumn colCategoryPathHtml;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit PartnerLogoRepositoryItemPictureEdit;
    }
}
