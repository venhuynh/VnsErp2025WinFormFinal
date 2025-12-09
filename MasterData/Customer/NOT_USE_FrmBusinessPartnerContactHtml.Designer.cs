namespace MasterData.Customer
{
    partial class NOT_USE_FrmBusinessPartnerContactHtml
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NOT_USE_FrmBusinessPartnerContactHtml));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.businessPartnerContactDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tileView1 = new DevExpress.XtraGrid.Views.Tile.TileView();
            this.colId = new DevExpress.XtraGrid.Columns.TileViewColumn();
            this.colFullName = new DevExpress.XtraGrid.Columns.TileViewColumn();
            this.colPosition = new DevExpress.XtraGrid.Columns.TileViewColumn();
            this.colPhone = new DevExpress.XtraGrid.Columns.TileViewColumn();
            this.colEmail = new DevExpress.XtraGrid.Columns.TileViewColumn();
            this.colSiteName = new DevExpress.XtraGrid.Columns.TileViewColumn();
            this.colPartnerName = new DevExpress.XtraGrid.Columns.TileViewColumn();
            this.colIsPrimary = new DevExpress.XtraGrid.Columns.TileViewColumn();
            this.colIsActive = new DevExpress.XtraGrid.Columns.TileViewColumn();
            this.colAvatar = new DevExpress.XtraGrid.Columns.TileViewColumn();
            this.colAvatarBase64 = new DevExpress.XtraGrid.Columns.TileViewColumn();
            this.colContactInfoHtml = new DevExpress.XtraGrid.Columns.TileViewColumn();
            this.colContactDetailsHtml = new DevExpress.XtraGrid.Columns.TileViewColumn();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.ListDataBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.NewBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.EditBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.DeleteBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ExportBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.DataSummaryBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.CurrentSelectBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.businessPartnerContactDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tileView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.businessPartnerContactDtoBindingSource;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 39);
            this.gridControl1.MainView = this.tileView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1200, 626);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.tileView1});
            // 
            // businessPartnerContactDtoBindingSource
            // 
            this.businessPartnerContactDtoBindingSource.DataSource = typeof(DTO.MasterData.CustomerPartner.BusinessPartnerContactDto);
            // 
            // tileView1
            // 
            this.tileView1.Appearance.ItemNormal.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tileView1.Appearance.ItemNormal.Options.UseFont = true;
            this.tileView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colId,
            this.colFullName,
            this.colPosition,
            this.colPhone,
            this.colEmail,
            this.colSiteName,
            this.colPartnerName,
            this.colIsPrimary,
            this.colIsActive,
            this.colAvatar,
            this.colAvatarBase64,
            this.colContactInfoHtml,
            this.colContactDetailsHtml});
            this.tileView1.FocusBorderColor = System.Drawing.Color.Transparent;
            this.tileView1.GridControl = this.gridControl1;
            this.tileView1.Name = "tileView1";
            this.tileView1.OptionsBehavior.AllowSmoothScrolling = true;
            this.tileView1.OptionsEditForm.AllowHtmlCaptions = true;
            this.tileView1.OptionsFind.AlwaysVisible = true;
            this.tileView1.OptionsSelection.AllowMarqueeSelection = true;
            this.tileView1.OptionsSelection.MultiSelect = true;
            this.tileView1.OptionsTiles.AllowPressAnimation = false;
            this.tileView1.OptionsTiles.HighlightFocusedTileStyle = DevExpress.XtraGrid.Views.Tile.HighlightFocusedTileStyle.None;
            this.tileView1.OptionsTiles.ItemPadding = new System.Windows.Forms.Padding(8);
            this.tileView1.OptionsTiles.ItemSize = new System.Drawing.Size(350, 200);
            this.tileView1.OptionsTiles.LayoutMode = DevExpress.XtraGrid.Views.Tile.TileViewLayoutMode.List;
            this.tileView1.OptionsTiles.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tileView1.OptionsTiles.Padding = new System.Windows.Forms.Padding(8);
            this.tileView1.OptionsTiles.ScrollMode = DevExpress.XtraEditors.TileControlScrollMode.TouchScrollBar;
            this.tileView1.TileHtmlTemplate.Styles = resources.GetString("tileView1.TileHtmlTemplate.Styles");
            this.tileView1.TileHtmlTemplate.Tag = "Contact Tile Template";
            this.tileView1.TileHtmlTemplate.Template = resources.GetString("tileView1.TileHtmlTemplate.Template");
            // 
            // colId
            // 
            this.colId.Caption = "ID";
            this.colId.FieldName = "Id";
            this.colId.Name = "colId";
            this.colId.Visible = true;
            this.colId.VisibleIndex = 0;
            // 
            // colFullName
            // 
            this.colFullName.Caption = "Họ và tên";
            this.colFullName.FieldName = "FullName";
            this.colFullName.Name = "colFullName";
            this.colFullName.Visible = true;
            this.colFullName.VisibleIndex = 1;
            // 
            // colPosition
            // 
            this.colPosition.Caption = "Chức vụ";
            this.colPosition.FieldName = "Position";
            this.colPosition.Name = "colPosition";
            this.colPosition.Visible = true;
            this.colPosition.VisibleIndex = 2;
            // 
            // colPhone
            // 
            this.colPhone.Caption = "Số điện thoại";
            this.colPhone.FieldName = "Phone";
            this.colPhone.Name = "colPhone";
            this.colPhone.Visible = true;
            this.colPhone.VisibleIndex = 3;
            // 
            // colEmail
            // 
            this.colEmail.Caption = "Email";
            this.colEmail.FieldName = "Email";
            this.colEmail.Name = "colEmail";
            this.colEmail.Visible = true;
            this.colEmail.VisibleIndex = 4;
            // 
            // colSiteName
            // 
            this.colSiteName.Caption = "Chi nhánh";
            this.colSiteName.FieldName = "SiteName";
            this.colSiteName.Name = "colSiteName";
            this.colSiteName.Visible = true;
            this.colSiteName.VisibleIndex = 5;
            // 
            // colPartnerName
            // 
            this.colPartnerName.Caption = "Đối tác";
            this.colPartnerName.FieldName = "PartnerName";
            this.colPartnerName.Name = "colPartnerName";
            this.colPartnerName.Visible = true;
            this.colPartnerName.VisibleIndex = 6;
            // 
            // colIsPrimary
            // 
            this.colIsPrimary.Caption = "Liên hệ chính";
            this.colIsPrimary.FieldName = "IsPrimary";
            this.colIsPrimary.Name = "colIsPrimary";
            this.colIsPrimary.Visible = true;
            this.colIsPrimary.VisibleIndex = 7;
            // 
            // colIsActive
            // 
            this.colIsActive.Caption = "Trạng thái";
            this.colIsActive.FieldName = "IsActive";
            this.colIsActive.Name = "colIsActive";
            this.colIsActive.Visible = true;
            this.colIsActive.VisibleIndex = 8;
            // 
            // colAvatar
            // 
            this.colAvatar.Caption = "Ảnh đại diện";
            this.colAvatar.FieldName = "Avatar";
            this.colAvatar.Name = "colAvatar";
            this.colAvatar.Visible = true;
            this.colAvatar.VisibleIndex = 9;
            // 
            // colAvatarBase64
            // 
            this.colAvatarBase64.Caption = "Ảnh đại diện (Base64)";
            this.colAvatarBase64.FieldName = "AvatarBase64";
            this.colAvatarBase64.Name = "colAvatarBase64";
            this.colAvatarBase64.Visible = true;
            this.colAvatarBase64.VisibleIndex = 10;
            // 
            // colContactInfoHtml
            // 
            this.colContactInfoHtml.Caption = "Thông tin HTML";
            this.colContactInfoHtml.FieldName = "ContactInfoHtml";
            this.colContactInfoHtml.Name = "colContactInfoHtml";
            this.colContactInfoHtml.Visible = true;
            this.colContactInfoHtml.VisibleIndex = 11;
            // 
            // colContactDetailsHtml
            // 
            this.colContactDetailsHtml.Caption = "Thông tin liên lạc HTML";
            this.colContactDetailsHtml.FieldName = "ContactDetailsHtml";
            this.colContactDetailsHtml.Name = "colContactDetailsHtml";
            this.colContactDetailsHtml.Visible = true;
            this.colContactDetailsHtml.VisibleIndex = 12;
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2,
            this.bar3});
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
            this.CurrentSelectBarStaticItem});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 7;
            this.barManager1.StatusBar = this.bar3;
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
            this.ExportBarButtonItem.Caption = "Xuất Excel";
            this.ExportBarButtonItem.Id = 4;
            this.ExportBarButtonItem.Name = "ExportBarButtonItem";
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.DataSummaryBarStaticItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.CurrentSelectBarStaticItem)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // DataSummaryBarStaticItem
            // 
            this.DataSummaryBarStaticItem.Caption = "Tổng số: 0 liên hệ";
            this.DataSummaryBarStaticItem.Id = 5;
            this.DataSummaryBarStaticItem.Name = "DataSummaryBarStaticItem";
            // 
            // CurrentSelectBarStaticItem
            // 
            this.CurrentSelectBarStaticItem.Caption = "Đã chọn: 0 liên hệ";
            this.CurrentSelectBarStaticItem.Id = 6;
            this.CurrentSelectBarStaticItem.Name = "CurrentSelectBarStaticItem";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1200, 39);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 665);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1200, 35);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 39);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 626);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1200, 39);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 626);
            // 
            // FrmBusinessPartnerContactHtml
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FrmBusinessPartnerContactHtml";
            this.Text = "Danh sách liên hệ đối tác (HTML View)";
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.businessPartnerContactDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tileView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private System.Windows.Forms.BindingSource businessPartnerContactDtoBindingSource;
        private DevExpress.XtraGrid.Views.Tile.TileView tileView1;
        private DevExpress.XtraGrid.Columns.TileViewColumn colId;
        private DevExpress.XtraGrid.Columns.TileViewColumn colFullName;
        private DevExpress.XtraGrid.Columns.TileViewColumn colPosition;
        private DevExpress.XtraGrid.Columns.TileViewColumn colPhone;
        private DevExpress.XtraGrid.Columns.TileViewColumn colEmail;
        private DevExpress.XtraGrid.Columns.TileViewColumn colSiteName;
        private DevExpress.XtraGrid.Columns.TileViewColumn colPartnerName;
        private DevExpress.XtraGrid.Columns.TileViewColumn colIsPrimary;
        private DevExpress.XtraGrid.Columns.TileViewColumn colIsActive;
        private DevExpress.XtraGrid.Columns.TileViewColumn colAvatar;
        private DevExpress.XtraGrid.Columns.TileViewColumn colAvatarBase64;
        private DevExpress.XtraGrid.Columns.TileViewColumn colContactInfoHtml;
        private DevExpress.XtraGrid.Columns.TileViewColumn colContactDetailsHtml;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem ListDataBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem NewBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem EditBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem DeleteBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem ExportBarButtonItem;
        private DevExpress.XtraBars.BarStaticItem DataSummaryBarStaticItem;
        private DevExpress.XtraBars.BarStaticItem CurrentSelectBarStaticItem;
    }
}