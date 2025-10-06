using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using MasterData.Customer.Dto;

namespace MasterData.Customer
{
    partial class FrmBusinessPartnerContactDetail
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
            this.SaveBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.CloseBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.PartnerNameSearchLookUpEdit = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.businessPartnerSiteListDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colPartnerName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSiteName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAddress = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FullNameTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.PositionTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.PhoneTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.EmailTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.IsPrimaryCheckEdit = new DevExpress.XtraEditors.CheckEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForPartnerName = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForFullName = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForPosition = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForPhone = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForEmail = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForIsPrimary = new DevExpress.XtraLayout.LayoutControlItem();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PartnerNameSearchLookUpEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.businessPartnerSiteListDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FullNameTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhoneTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmailTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsPrimaryCheckEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPartnerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForFullName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPhone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForEmail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsPrimary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
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
            this.CloseBarButtonItem});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 2;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.SaveBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.CloseBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // SaveBarButtonItem
            // 
            this.SaveBarButtonItem.Caption = "Lưu";
            this.SaveBarButtonItem.Id = 0;
            this.SaveBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.save_16x16;
            this.SaveBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.save_32x32;
            this.SaveBarButtonItem.Name = "SaveBarButtonItem";
            // 
            // CloseBarButtonItem
            // 
            this.CloseBarButtonItem.Caption = "Đóng";
            this.CloseBarButtonItem.Id = 1;
            this.CloseBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.cancel_16x16;
            this.CloseBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.cancel_32x32;
            this.CloseBarButtonItem.Name = "CloseBarButtonItem";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(430, 39);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 325);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(430, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 39);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 286);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(430, 39);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 286);
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
            // 
            // dataLayoutControl1
            // 
            this.dataLayoutControl1.Controls.Add(this.PartnerNameSearchLookUpEdit);
            this.dataLayoutControl1.Controls.Add(this.FullNameTextEdit);
            this.dataLayoutControl1.Controls.Add(this.PositionTextEdit);
            this.dataLayoutControl1.Controls.Add(this.PhoneTextEdit);
            this.dataLayoutControl1.Controls.Add(this.EmailTextEdit);
            this.dataLayoutControl1.Controls.Add(this.IsPrimaryCheckEdit);
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 39);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(430, 286);
            this.dataLayoutControl1.TabIndex = 10;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // PartnerNameSearchLookUpEdit
            // 
            this.PartnerNameSearchLookUpEdit.Location = new System.Drawing.Point(94, 16);
            this.PartnerNameSearchLookUpEdit.MenuManager = this.barManager1;
            this.PartnerNameSearchLookUpEdit.Name = "PartnerNameSearchLookUpEdit";
            this.PartnerNameSearchLookUpEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.PartnerNameSearchLookUpEdit.Properties.DataSource = this.businessPartnerSiteListDtoBindingSource;
            this.PartnerNameSearchLookUpEdit.Properties.DisplayMember = "SiteName";
            this.PartnerNameSearchLookUpEdit.Properties.NullText = "";
            this.PartnerNameSearchLookUpEdit.Properties.PopupView = this.searchLookUpEdit1View;
            this.PartnerNameSearchLookUpEdit.Properties.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit1});
            this.PartnerNameSearchLookUpEdit.Properties.ValueMember = "Id";
            this.PartnerNameSearchLookUpEdit.Size = new System.Drawing.Size(320, 28);
            this.PartnerNameSearchLookUpEdit.StyleController = this.dataLayoutControl1;
            this.PartnerNameSearchLookUpEdit.TabIndex = 4;
            // 
            // businessPartnerSiteListDtoBindingSource
            // 
            this.businessPartnerSiteListDtoBindingSource.DataSource = typeof(MasterData.Customer.Dto.BusinessPartnerSiteListDto);
            // 
            // searchLookUpEdit1View
            // 
            this.searchLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colPartnerName,
            this.colSiteName,
            this.colAddress});
            this.searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit1View.GroupCount = 1;
            this.searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            this.searchLookUpEdit1View.OptionsBehavior.AutoExpandAllGroups = true;
            this.searchLookUpEdit1View.OptionsPrint.ExpandAllDetails = true;
            this.searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.searchLookUpEdit1View.OptionsView.RowAutoHeight = true;
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            this.searchLookUpEdit1View.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colPartnerName, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // colPartnerName
            // 
            this.colPartnerName.FieldName = "PartnerName";
            this.colPartnerName.Name = "colPartnerName";
            this.colPartnerName.Visible = true;
            this.colPartnerName.VisibleIndex = 0;
            // 
            // colSiteName
            // 
            this.colSiteName.ColumnEdit = this.repositoryItemMemoEdit1;
            this.colSiteName.FieldName = "SiteName";
            this.colSiteName.Name = "colSiteName";
            this.colSiteName.Visible = true;
            this.colSiteName.VisibleIndex = 0;
            // 
            // colAddress
            // 
            this.colAddress.ColumnEdit = this.repositoryItemMemoEdit1;
            this.colAddress.FieldName = "Address";
            this.colAddress.Name = "colAddress";
            this.colAddress.Visible = true;
            this.colAddress.VisibleIndex = 1;
            // 
            // FullNameTextEdit
            // 
            this.FullNameTextEdit.Location = new System.Drawing.Point(94, 50);
            this.FullNameTextEdit.MenuManager = this.barManager1;
            this.FullNameTextEdit.Name = "FullNameTextEdit";
            this.FullNameTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.FullNameTextEdit.Size = new System.Drawing.Size(320, 28);
            this.FullNameTextEdit.StyleController = this.dataLayoutControl1;
            this.FullNameTextEdit.TabIndex = 5;
            // 
            // PositionTextEdit
            // 
            this.PositionTextEdit.Location = new System.Drawing.Point(94, 84);
            this.PositionTextEdit.MenuManager = this.barManager1;
            this.PositionTextEdit.Name = "PositionTextEdit";
            this.PositionTextEdit.Size = new System.Drawing.Size(320, 28);
            this.PositionTextEdit.StyleController = this.dataLayoutControl1;
            this.PositionTextEdit.TabIndex = 6;
            // 
            // PhoneTextEdit
            // 
            this.PhoneTextEdit.Location = new System.Drawing.Point(94, 118);
            this.PhoneTextEdit.MenuManager = this.barManager1;
            this.PhoneTextEdit.Name = "PhoneTextEdit";
            this.PhoneTextEdit.Properties.Mask.EditMask = "(999) 000-0000";
            this.PhoneTextEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
            this.PhoneTextEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.PhoneTextEdit.Size = new System.Drawing.Size(320, 28);
            this.PhoneTextEdit.StyleController = this.dataLayoutControl1;
            this.PhoneTextEdit.TabIndex = 7;
            // 
            // EmailTextEdit
            // 
            this.EmailTextEdit.Location = new System.Drawing.Point(94, 152);
            this.EmailTextEdit.MenuManager = this.barManager1;
            this.EmailTextEdit.Name = "EmailTextEdit";
            this.EmailTextEdit.Size = new System.Drawing.Size(320, 28);
            this.EmailTextEdit.StyleController = this.dataLayoutControl1;
            this.EmailTextEdit.TabIndex = 8;
            // 
            // IsPrimaryCheckEdit
            // 
            this.IsPrimaryCheckEdit.Location = new System.Drawing.Point(16, 186);
            this.IsPrimaryCheckEdit.MenuManager = this.barManager1;
            this.IsPrimaryCheckEdit.Name = "IsPrimaryCheckEdit";
            this.IsPrimaryCheckEdit.Properties.Caption = "Liên hệ chính";
            this.IsPrimaryCheckEdit.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.IsPrimaryCheckEdit.Size = new System.Drawing.Size(398, 22);
            this.IsPrimaryCheckEdit.StyleController = this.dataLayoutControl1;
            this.IsPrimaryCheckEdit.TabIndex = 9;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(430, 286);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AllowDrawBackground = false;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForPartnerName,
            this.ItemForFullName,
            this.ItemForPosition,
            this.ItemForPhone,
            this.ItemForEmail,
            this.ItemForIsPrimary});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "autoGeneratedGroup0";
            this.layoutControlGroup1.Size = new System.Drawing.Size(404, 260);
            // 
            // ItemForPartnerName
            // 
            this.ItemForPartnerName.Control = this.PartnerNameSearchLookUpEdit;
            this.ItemForPartnerName.Location = new System.Drawing.Point(0, 0);
            this.ItemForPartnerName.Name = "ItemForPartnerName";
            this.ItemForPartnerName.Size = new System.Drawing.Size(404, 34);
            this.ItemForPartnerName.Text = "Tên đối tác";
            this.ItemForPartnerName.TextSize = new System.Drawing.Size(62, 13);
            // 
            // ItemForFullName
            // 
            this.ItemForFullName.Control = this.FullNameTextEdit;
            this.ItemForFullName.Location = new System.Drawing.Point(0, 34);
            this.ItemForFullName.Name = "ItemForFullName";
            this.ItemForFullName.Size = new System.Drawing.Size(404, 34);
            this.ItemForFullName.Text = "Họ và tên";
            this.ItemForFullName.TextSize = new System.Drawing.Size(62, 13);
            // 
            // ItemForPosition
            // 
            this.ItemForPosition.Control = this.PositionTextEdit;
            this.ItemForPosition.Location = new System.Drawing.Point(0, 68);
            this.ItemForPosition.Name = "ItemForPosition";
            this.ItemForPosition.Size = new System.Drawing.Size(404, 34);
            this.ItemForPosition.Text = "Chức vụ";
            this.ItemForPosition.TextSize = new System.Drawing.Size(62, 13);
            // 
            // ItemForPhone
            // 
            this.ItemForPhone.Control = this.PhoneTextEdit;
            this.ItemForPhone.Location = new System.Drawing.Point(0, 102);
            this.ItemForPhone.Name = "ItemForPhone";
            this.ItemForPhone.Size = new System.Drawing.Size(404, 34);
            this.ItemForPhone.Text = "Số điện thoại";
            this.ItemForPhone.TextSize = new System.Drawing.Size(62, 13);
            // 
            // ItemForEmail
            // 
            this.ItemForEmail.Control = this.EmailTextEdit;
            this.ItemForEmail.Location = new System.Drawing.Point(0, 136);
            this.ItemForEmail.Name = "ItemForEmail";
            this.ItemForEmail.Size = new System.Drawing.Size(404, 34);
            this.ItemForEmail.Text = "Email";
            this.ItemForEmail.TextSize = new System.Drawing.Size(62, 13);
            // 
            // ItemForIsPrimary
            // 
            this.ItemForIsPrimary.Control = this.IsPrimaryCheckEdit;
            this.ItemForIsPrimary.Location = new System.Drawing.Point(0, 170);
            this.ItemForIsPrimary.Name = "ItemForIsPrimary";
            this.ItemForIsPrimary.Size = new System.Drawing.Size(404, 90);
            this.ItemForIsPrimary.Text = "Liên hệ chính";
            this.ItemForIsPrimary.TextVisible = false;
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            // 
            // FrmBusinessPartnerContactDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 325);
            this.Controls.Add(this.dataLayoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmBusinessPartnerContactDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PartnerNameSearchLookUpEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.businessPartnerSiteListDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FullNameTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhoneTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmailTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsPrimaryCheckEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPartnerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForFullName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPhone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForEmail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsPrimary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
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
        private DataLayoutControl dataLayoutControl1;
        private SearchLookUpEdit PartnerNameSearchLookUpEdit;
        private GridView searchLookUpEdit1View;
        private GridColumn colPartnerName;
        private TextEdit FullNameTextEdit;
        private TextEdit PositionTextEdit;
        private TextEdit PhoneTextEdit;
        private TextEdit EmailTextEdit;
        private CheckEdit IsPrimaryCheckEdit;
        private LayoutControlGroup Root;
        private LayoutControlGroup layoutControlGroup1;
        private LayoutControlItem ItemForPartnerName;
        private LayoutControlItem ItemForFullName;
        private LayoutControlItem ItemForPosition;
        private LayoutControlItem ItemForPhone;
        private LayoutControlItem ItemForEmail;
        private LayoutControlItem ItemForIsPrimary;
        private BindingSource businessPartnerSiteListDtoBindingSource;
        private GridColumn colSiteName;
        private GridColumn colAddress;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
    }
}