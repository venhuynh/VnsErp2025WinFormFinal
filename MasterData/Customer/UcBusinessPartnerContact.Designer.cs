using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using MasterData.Customer.Dto;

namespace MasterData.Customer
{
    partial class UcBusinessPartnerContact
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
            this.BusinessPartnerContactGridControl = new DevExpress.XtraGrid.GridControl();
            this.businessPartnerContactDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.BusinessPartnerContactGridCardView = new DevExpress.XtraGrid.Views.Card.CardView();
            this.colFullName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsPrimary = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPosition = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPhone = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEmail = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSiteName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsActive = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAvatar = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemPictureEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BusinessPartnerContactGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.businessPartnerContactDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BusinessPartnerContactGridCardView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).BeginInit();
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
            this.barDockControlTop.Size = new System.Drawing.Size(1077, 39);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 661);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1077, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 39);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 622);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1077, 39);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 622);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.BusinessPartnerContactGridControl);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 39);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(1077, 622);
            this.layoutControl1.TabIndex = 4;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // BusinessPartnerContactGridControl
            // 
            this.BusinessPartnerContactGridControl.DataSource = this.businessPartnerContactDtoBindingSource;
            this.BusinessPartnerContactGridControl.Location = new System.Drawing.Point(16, 16);
            this.BusinessPartnerContactGridControl.MainView = this.BusinessPartnerContactGridCardView;
            this.BusinessPartnerContactGridControl.MenuManager = this.barManager1;
            this.BusinessPartnerContactGridControl.Name = "BusinessPartnerContactGridControl";
            this.BusinessPartnerContactGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemPictureEdit1});
            this.BusinessPartnerContactGridControl.Size = new System.Drawing.Size(1045, 590);
            this.BusinessPartnerContactGridControl.TabIndex = 5;
            this.BusinessPartnerContactGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.BusinessPartnerContactGridCardView});
            // 
            // businessPartnerContactDtoBindingSource
            // 
            this.businessPartnerContactDtoBindingSource.DataSource = typeof(MasterData.Customer.Dto.BusinessPartnerContactDto);
            // 
            // BusinessPartnerContactGridCardView
            // 
            this.BusinessPartnerContactGridCardView.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.BusinessPartnerContactGridCardView.Appearance.ViewCaption.ForeColor = System.Drawing.Color.DarkBlue;
            this.BusinessPartnerContactGridCardView.Appearance.ViewCaption.Options.UseFont = true;
            this.BusinessPartnerContactGridCardView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.BusinessPartnerContactGridCardView.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.BusinessPartnerContactGridCardView.CardWidth = 320;
            this.BusinessPartnerContactGridCardView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colFullName,
            this.colIsPrimary,
            this.colPosition,
            this.colPhone,
            this.colEmail,
            this.colSiteName,
            this.colIsActive,
            this.colAvatar});
            this.BusinessPartnerContactGridCardView.GridControl = this.BusinessPartnerContactGridControl;
            this.BusinessPartnerContactGridCardView.Name = "BusinessPartnerContactGridCardView";
            this.BusinessPartnerContactGridCardView.OptionsBehavior.FieldAutoHeight = true;
            this.BusinessPartnerContactGridCardView.OptionsFind.AlwaysVisible = true;
            this.BusinessPartnerContactGridCardView.OptionsSelection.MultiSelect = true;
            this.BusinessPartnerContactGridCardView.OptionsView.ShowCardExpandButton = false;
            this.BusinessPartnerContactGridCardView.OptionsView.ShowViewCaption = true;
            this.BusinessPartnerContactGridCardView.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
            this.BusinessPartnerContactGridCardView.ViewCaption = "DANH SÁCH LIÊN HỆ ĐỐI TÁC";
            // 
            // colFullName
            // 
            this.colFullName.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.colFullName.AppearanceCell.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Hyperlink;
            this.colFullName.AppearanceCell.Options.UseFont = true;
            this.colFullName.AppearanceCell.Options.UseForeColor = true;
            this.colFullName.AppearanceHeader.Options.UseTextOptions = true;
            this.colFullName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colFullName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colFullName.FieldName = "FullName";
            this.colFullName.Name = "colFullName";
            this.colFullName.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "FullName", "Có {0} dòng")});
            this.colFullName.Visible = true;
            this.colFullName.VisibleIndex = 1;
            // 
            // colIsPrimary
            // 
            this.colIsPrimary.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.colIsPrimary.AppearanceCell.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Warning;
            this.colIsPrimary.AppearanceCell.Options.UseFont = true;
            this.colIsPrimary.AppearanceCell.Options.UseForeColor = true;
            this.colIsPrimary.AppearanceCell.Options.UseTextOptions = true;
            this.colIsPrimary.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colIsPrimary.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colIsPrimary.AppearanceHeader.Options.UseTextOptions = true;
            this.colIsPrimary.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colIsPrimary.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colIsPrimary.FieldName = "IsPrimary";
            this.colIsPrimary.Name = "colIsPrimary";
            this.colIsPrimary.Visible = true;
            this.colIsPrimary.VisibleIndex = 3;
            // 
            // colPosition
            // 
            this.colPosition.AppearanceCell.Options.UseTextOptions = true;
            this.colPosition.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPosition.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colPosition.AppearanceHeader.Options.UseTextOptions = true;
            this.colPosition.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colPosition.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colPosition.FieldName = "Position";
            this.colPosition.Name = "colPosition";
            this.colPosition.Visible = true;
            this.colPosition.VisibleIndex = 4;
            // 
            // colPhone
            // 
            this.colPhone.AppearanceCell.Options.UseTextOptions = true;
            this.colPhone.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPhone.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colPhone.AppearanceHeader.Options.UseTextOptions = true;
            this.colPhone.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colPhone.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colPhone.FieldName = "Phone";
            this.colPhone.Name = "colPhone";
            this.colPhone.Visible = true;
            this.colPhone.VisibleIndex = 5;
            // 
            // colEmail
            // 
            this.colEmail.AppearanceCell.Options.UseTextOptions = true;
            this.colEmail.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colEmail.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colEmail.AppearanceHeader.Options.UseTextOptions = true;
            this.colEmail.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colEmail.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colEmail.FieldName = "Email";
            this.colEmail.Name = "colEmail";
            this.colEmail.Visible = true;
            this.colEmail.VisibleIndex = 6;
            // 
            // colSiteName
            // 
            this.colSiteName.AppearanceCell.Options.UseTextOptions = true;
            this.colSiteName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colSiteName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colSiteName.AppearanceHeader.Options.UseTextOptions = true;
            this.colSiteName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colSiteName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colSiteName.FieldName = "SiteName";
            this.colSiteName.Name = "colSiteName";
            this.colSiteName.Visible = true;
            this.colSiteName.VisibleIndex = 0;
            // 
            // colIsActive
            // 
            this.colIsActive.AppearanceHeader.Options.UseTextOptions = true;
            this.colIsActive.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colIsActive.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colIsActive.FieldName = "IsActive";
            this.colIsActive.Name = "colIsActive";
            this.colIsActive.Visible = true;
            this.colIsActive.VisibleIndex = 7;
            // 
            // colAvatar
            // 
            this.colAvatar.AppearanceHeader.Options.UseTextOptions = true;
            this.colAvatar.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colAvatar.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colAvatar.ColumnEdit = this.repositoryItemPictureEdit1;
            this.colAvatar.FieldName = "Avatar";
            this.colAvatar.Name = "colAvatar";
            this.colAvatar.OptionsEditForm.RowSpan = 4;
            this.colAvatar.Visible = true;
            this.colAvatar.VisibleIndex = 2;
            this.colAvatar.Width = 120;
            // 
            // repositoryItemPictureEdit1
            // 
            this.repositoryItemPictureEdit1.CustomHeight = 120;
            this.repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
            this.repositoryItemPictureEdit1.PictureInterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            this.repositoryItemPictureEdit1.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1077, 622);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.BusinessPartnerContactGridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1051, 596);
            this.layoutControlItem1.TextVisible = false;
            // 
            // UcBusinessPartnerContact
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "UcBusinessPartnerContact";
            this.Size = new System.Drawing.Size(1077, 661);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BusinessPartnerContactGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.businessPartnerContactDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BusinessPartnerContactGridCardView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).EndInit();
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
        private GridControl BusinessPartnerContactGridControl;
        private LayoutControlGroup Root;
        private LayoutControlItem layoutControlItem1;
        private BarButtonItem ListDataBarButtonItem;
        private BarButtonItem NewBarButtonItem;
        private BarButtonItem EditBarButtonItem;
        private BarButtonItem DeleteBarButtonItem;
        private BarButtonItem ExportBarButtonItem;
        private BindingSource businessPartnerContactDtoBindingSource;
        private DevExpress.XtraGrid.Views.Card.CardView BusinessPartnerContactGridCardView;
        private GridColumn colFullName;
        private GridColumn colIsPrimary;
        private GridColumn colPosition;
        private GridColumn colPhone;
        private GridColumn colEmail;
        private GridColumn colSiteName;
        private GridColumn colIsActive;
        private GridColumn colAvatar;
        private RepositoryItemPictureEdit repositoryItemPictureEdit1;
    }
}
