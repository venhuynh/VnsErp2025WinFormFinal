using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DTO.MasterData.CustomerPartner;

namespace MasterData.Customer
{
    partial class FrmBusinessPartnerSiteDetail
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
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.SiteCodeTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ItemForSiteCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForPartnerName = new DevExpress.XtraLayout.LayoutControlItem();
            this.SiteNameTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ItemForSiteName = new DevExpress.XtraLayout.LayoutControlItem();
            this.AddressTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ItemForAddress = new DevExpress.XtraLayout.LayoutControlItem();
            this.CityTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ItemForCity = new DevExpress.XtraLayout.LayoutControlItem();
            this.ProvinceTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ItemForProvince = new DevExpress.XtraLayout.LayoutControlItem();
            this.CountryTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ItemForCountry = new DevExpress.XtraLayout.LayoutControlItem();
            this.ContactPersonTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ItemForContactPerson = new DevExpress.XtraLayout.LayoutControlItem();
            this.PhoneTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ItemForPhone = new DevExpress.XtraLayout.LayoutControlItem();
            this.EmailTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ItemForEmail = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForIsActive = new DevExpress.XtraLayout.LayoutControlItem();
            this.PartnerNameTextEdit = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.businessPartnerListDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.colPartnerName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPartnerTypeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFullAddressName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.IsActiveCheckEdit = new DevExpress.XtraEditors.ToggleSwitch();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SiteCodeTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForSiteCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPartnerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SiteNameTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForSiteName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddressTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CityTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProvinceTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForProvince)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountryTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCountry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContactPersonTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForContactPerson)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhoneTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPhone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmailTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForEmail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsActive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PartnerNameTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.businessPartnerListDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsActiveCheckEdit.Properties)).BeginInit();
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
            this.barDockControlTop.Size = new System.Drawing.Size(666, 39);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 424);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(666, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 39);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 385);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(666, 39);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 385);
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
            // 
            // dataLayoutControl1
            // 
            this.dataLayoutControl1.Controls.Add(this.SiteCodeTextEdit);
            this.dataLayoutControl1.Controls.Add(this.SiteNameTextEdit);
            this.dataLayoutControl1.Controls.Add(this.AddressTextEdit);
            this.dataLayoutControl1.Controls.Add(this.CityTextEdit);
            this.dataLayoutControl1.Controls.Add(this.ProvinceTextEdit);
            this.dataLayoutControl1.Controls.Add(this.CountryTextEdit);
            this.dataLayoutControl1.Controls.Add(this.ContactPersonTextEdit);
            this.dataLayoutControl1.Controls.Add(this.PhoneTextEdit);
            this.dataLayoutControl1.Controls.Add(this.EmailTextEdit);
            this.dataLayoutControl1.Controls.Add(this.PartnerNameTextEdit);
            this.dataLayoutControl1.Controls.Add(this.IsActiveCheckEdit);
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 39);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(666, 385);
            this.dataLayoutControl1.TabIndex = 5;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(666, 385);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AllowDrawBackground = false;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForSiteCode,
            this.ItemForSiteName,
            this.ItemForAddress,
            this.ItemForCity,
            this.ItemForProvince,
            this.ItemForCountry,
            this.ItemForContactPerson,
            this.ItemForPhone,
            this.ItemForEmail,
            this.ItemForPartnerName,
            this.ItemForIsActive});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "autoGeneratedGroup0";
            this.layoutControlGroup1.Size = new System.Drawing.Size(640, 359);
            // 
            // SiteCodeTextEdit
            // 
            this.SiteCodeTextEdit.Location = new System.Drawing.Point(107, 50);
            this.SiteCodeTextEdit.MenuManager = this.barManager1;
            this.SiteCodeTextEdit.Name = "SiteCodeTextEdit";
            this.SiteCodeTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.SiteCodeTextEdit.Size = new System.Drawing.Size(223, 28);
            this.SiteCodeTextEdit.StyleController = this.dataLayoutControl1;
            this.SiteCodeTextEdit.TabIndex = 4;
            // 
            // ItemForSiteCode
            // 
            this.ItemForSiteCode.Control = this.SiteCodeTextEdit;
            this.ItemForSiteCode.Location = new System.Drawing.Point(0, 34);
            this.ItemForSiteCode.Name = "ItemForSiteCode";
            this.ItemForSiteCode.Size = new System.Drawing.Size(320, 34);
            this.ItemForSiteCode.Text = "Mã chi nhánh";
            this.ItemForSiteCode.TextSize = new System.Drawing.Size(75, 13);
            // 
            // ItemForPartnerName
            // 
            this.ItemForPartnerName.Control = this.PartnerNameTextEdit;
            this.ItemForPartnerName.Location = new System.Drawing.Point(0, 0);
            this.ItemForPartnerName.Name = "ItemForPartnerName";
            this.ItemForPartnerName.Size = new System.Drawing.Size(640, 34);
            this.ItemForPartnerName.Text = "Tên đối tác";
            this.ItemForPartnerName.TextSize = new System.Drawing.Size(75, 13);
            // 
            // SiteNameTextEdit
            // 
            this.SiteNameTextEdit.Location = new System.Drawing.Point(107, 84);
            this.SiteNameTextEdit.MenuManager = this.barManager1;
            this.SiteNameTextEdit.Name = "SiteNameTextEdit";
            this.SiteNameTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.SiteNameTextEdit.Size = new System.Drawing.Size(543, 28);
            this.SiteNameTextEdit.StyleController = this.dataLayoutControl1;
            this.SiteNameTextEdit.TabIndex = 6;
            // 
            // ItemForSiteName
            // 
            this.ItemForSiteName.Control = this.SiteNameTextEdit;
            this.ItemForSiteName.Location = new System.Drawing.Point(0, 68);
            this.ItemForSiteName.Name = "ItemForSiteName";
            this.ItemForSiteName.Size = new System.Drawing.Size(640, 34);
            this.ItemForSiteName.Text = "Tên chi nhánh";
            this.ItemForSiteName.TextSize = new System.Drawing.Size(75, 13);
            // 
            // AddressTextEdit
            // 
            this.AddressTextEdit.Location = new System.Drawing.Point(107, 118);
            this.AddressTextEdit.MenuManager = this.barManager1;
            this.AddressTextEdit.Name = "AddressTextEdit";
            this.AddressTextEdit.Size = new System.Drawing.Size(543, 28);
            this.AddressTextEdit.StyleController = this.dataLayoutControl1;
            this.AddressTextEdit.TabIndex = 7;
            // 
            // ItemForAddress
            // 
            this.ItemForAddress.Control = this.AddressTextEdit;
            this.ItemForAddress.Location = new System.Drawing.Point(0, 102);
            this.ItemForAddress.Name = "ItemForAddress";
            this.ItemForAddress.Size = new System.Drawing.Size(640, 34);
            this.ItemForAddress.Text = "Địa chỉ";
            this.ItemForAddress.TextSize = new System.Drawing.Size(75, 13);
            // 
            // CityTextEdit
            // 
            this.CityTextEdit.Location = new System.Drawing.Point(107, 152);
            this.CityTextEdit.MenuManager = this.barManager1;
            this.CityTextEdit.Name = "CityTextEdit";
            this.CityTextEdit.Size = new System.Drawing.Size(543, 28);
            this.CityTextEdit.StyleController = this.dataLayoutControl1;
            this.CityTextEdit.TabIndex = 8;
            // 
            // ItemForCity
            // 
            this.ItemForCity.Control = this.CityTextEdit;
            this.ItemForCity.Location = new System.Drawing.Point(0, 136);
            this.ItemForCity.Name = "ItemForCity";
            this.ItemForCity.Size = new System.Drawing.Size(640, 34);
            this.ItemForCity.Text = "Thành phố";
            this.ItemForCity.TextSize = new System.Drawing.Size(75, 13);
            // 
            // ProvinceTextEdit
            // 
            this.ProvinceTextEdit.Location = new System.Drawing.Point(107, 186);
            this.ProvinceTextEdit.MenuManager = this.barManager1;
            this.ProvinceTextEdit.Name = "ProvinceTextEdit";
            this.ProvinceTextEdit.Size = new System.Drawing.Size(543, 28);
            this.ProvinceTextEdit.StyleController = this.dataLayoutControl1;
            this.ProvinceTextEdit.TabIndex = 9;
            // 
            // ItemForProvince
            // 
            this.ItemForProvince.Control = this.ProvinceTextEdit;
            this.ItemForProvince.Location = new System.Drawing.Point(0, 170);
            this.ItemForProvince.Name = "ItemForProvince";
            this.ItemForProvince.Size = new System.Drawing.Size(640, 34);
            this.ItemForProvince.Text = "Tỉnh/Thành phố";
            this.ItemForProvince.TextSize = new System.Drawing.Size(75, 13);
            // 
            // CountryTextEdit
            // 
            this.CountryTextEdit.Location = new System.Drawing.Point(107, 220);
            this.CountryTextEdit.MenuManager = this.barManager1;
            this.CountryTextEdit.Name = "CountryTextEdit";
            this.CountryTextEdit.Size = new System.Drawing.Size(543, 28);
            this.CountryTextEdit.StyleController = this.dataLayoutControl1;
            this.CountryTextEdit.TabIndex = 10;
            // 
            // ItemForCountry
            // 
            this.ItemForCountry.Control = this.CountryTextEdit;
            this.ItemForCountry.Location = new System.Drawing.Point(0, 204);
            this.ItemForCountry.Name = "ItemForCountry";
            this.ItemForCountry.Size = new System.Drawing.Size(640, 34);
            this.ItemForCountry.Text = "Quốc gia";
            this.ItemForCountry.TextSize = new System.Drawing.Size(75, 13);
            // 
            // ContactPersonTextEdit
            // 
            this.ContactPersonTextEdit.Location = new System.Drawing.Point(107, 254);
            this.ContactPersonTextEdit.MenuManager = this.barManager1;
            this.ContactPersonTextEdit.Name = "ContactPersonTextEdit";
            this.ContactPersonTextEdit.Size = new System.Drawing.Size(543, 28);
            this.ContactPersonTextEdit.StyleController = this.dataLayoutControl1;
            this.ContactPersonTextEdit.TabIndex = 11;
            // 
            // ItemForContactPerson
            // 
            this.ItemForContactPerson.Control = this.ContactPersonTextEdit;
            this.ItemForContactPerson.Location = new System.Drawing.Point(0, 238);
            this.ItemForContactPerson.Name = "ItemForContactPerson";
            this.ItemForContactPerson.Size = new System.Drawing.Size(640, 34);
            this.ItemForContactPerson.Text = "Người liên hệ";
            this.ItemForContactPerson.TextSize = new System.Drawing.Size(75, 13);
            // 
            // PhoneTextEdit
            // 
            this.PhoneTextEdit.Location = new System.Drawing.Point(107, 288);
            this.PhoneTextEdit.MenuManager = this.barManager1;
            this.PhoneTextEdit.Name = "PhoneTextEdit";
            this.PhoneTextEdit.Properties.Mask.EditMask = "(999) 000-0000";
            this.PhoneTextEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
            this.PhoneTextEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.PhoneTextEdit.Size = new System.Drawing.Size(543, 28);
            this.PhoneTextEdit.StyleController = this.dataLayoutControl1;
            this.PhoneTextEdit.TabIndex = 12;
            // 
            // ItemForPhone
            // 
            this.ItemForPhone.Control = this.PhoneTextEdit;
            this.ItemForPhone.Location = new System.Drawing.Point(0, 272);
            this.ItemForPhone.Name = "ItemForPhone";
            this.ItemForPhone.Size = new System.Drawing.Size(640, 34);
            this.ItemForPhone.Text = "Số điện thoại";
            this.ItemForPhone.TextSize = new System.Drawing.Size(75, 13);
            // 
            // EmailTextEdit
            // 
            this.EmailTextEdit.Location = new System.Drawing.Point(107, 322);
            this.EmailTextEdit.MenuManager = this.barManager1;
            this.EmailTextEdit.Name = "EmailTextEdit";
            this.EmailTextEdit.Size = new System.Drawing.Size(543, 28);
            this.EmailTextEdit.StyleController = this.dataLayoutControl1;
            this.EmailTextEdit.TabIndex = 13;
            // 
            // ItemForEmail
            // 
            this.ItemForEmail.Control = this.EmailTextEdit;
            this.ItemForEmail.Location = new System.Drawing.Point(0, 306);
            this.ItemForEmail.Name = "ItemForEmail";
            this.ItemForEmail.Size = new System.Drawing.Size(640, 53);
            this.ItemForEmail.Text = "Email";
            this.ItemForEmail.TextSize = new System.Drawing.Size(75, 13);
            // 
            // ItemForIsActive
            // 
            this.ItemForIsActive.Control = this.IsActiveCheckEdit;
            this.ItemForIsActive.Location = new System.Drawing.Point(320, 34);
            this.ItemForIsActive.Name = "ItemForIsActive";
            this.ItemForIsActive.Size = new System.Drawing.Size(320, 34);
            this.ItemForIsActive.Text = "Trạng thái";
            this.ItemForIsActive.TextSize = new System.Drawing.Size(75, 13);
            // 
            // PartnerNameTextEdit
            // 
            this.PartnerNameTextEdit.Location = new System.Drawing.Point(107, 16);
            this.PartnerNameTextEdit.MenuManager = this.barManager1;
            this.PartnerNameTextEdit.Name = "PartnerNameTextEdit";
            this.PartnerNameTextEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.PartnerNameTextEdit.Properties.DataSource = this.businessPartnerListDtoBindingSource;
            this.PartnerNameTextEdit.Properties.DisplayMember = "PartnerName";
            this.PartnerNameTextEdit.Properties.NullText = "";
            this.PartnerNameTextEdit.Properties.PopupView = this.searchLookUpEdit1View;
            this.PartnerNameTextEdit.Properties.ValueMember = "Id";
            this.PartnerNameTextEdit.Size = new System.Drawing.Size(543, 28);
            this.PartnerNameTextEdit.StyleController = this.dataLayoutControl1;
            this.PartnerNameTextEdit.TabIndex = 5;
            // 
            // searchLookUpEdit1View
            // 
            this.searchLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colPartnerTypeName,
            this.colPartnerName,
            this.colFullAddressName});
            this.searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            this.searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // businessPartnerListDtoBindingSource
            // 
            this.businessPartnerListDtoBindingSource.DataSource = typeof(BusinessPartnerListDto);
            // 
            // colPartnerName
            // 
            this.colPartnerName.FieldName = "PartnerName";
            this.colPartnerName.Name = "colPartnerName";
            this.colPartnerName.Visible = true;
            this.colPartnerName.VisibleIndex = 0;
            // 
            // colPartnerTypeName
            // 
            this.colPartnerTypeName.FieldName = "PartnerTypeName";
            this.colPartnerTypeName.Name = "colPartnerTypeName";
            this.colPartnerTypeName.Visible = true;
            this.colPartnerTypeName.VisibleIndex = 1;
            // 
            // colFullAddressName
            // 
            this.colFullAddressName.FieldName = "FullAddressName";
            this.colFullAddressName.Name = "colFullAddressName";
            this.colFullAddressName.Visible = true;
            this.colFullAddressName.VisibleIndex = 2;
            // 
            // IsActiveCheckEdit
            // 
            this.IsActiveCheckEdit.EditValue = true;
            this.IsActiveCheckEdit.Location = new System.Drawing.Point(427, 50);
            this.IsActiveCheckEdit.MenuManager = this.barManager1;
            this.IsActiveCheckEdit.Name = "IsActiveCheckEdit";
            this.IsActiveCheckEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.IsActiveCheckEdit.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.IsActiveCheckEdit.Properties.OffText = "<color=\'red\'>Không hoạt động</color>";
            this.IsActiveCheckEdit.Properties.OnText = "<color=\'blue\'>Đang hoạt động</color>";
            this.IsActiveCheckEdit.Size = new System.Drawing.Size(223, 24);
            this.IsActiveCheckEdit.StyleController = this.dataLayoutControl1;
            this.IsActiveCheckEdit.TabIndex = 15;
            // 
            // FrmBusinessPartnerSiteDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 424);
            this.Controls.Add(this.dataLayoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmBusinessPartnerSiteDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SiteCodeTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForSiteCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPartnerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SiteNameTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForSiteName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddressTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CityTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProvinceTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForProvince)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountryTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCountry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContactPersonTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForContactPerson)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhoneTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPhone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmailTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForEmail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PartnerNameTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.businessPartnerListDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsActiveCheckEdit.Properties)).EndInit();
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
        private TextEdit SiteCodeTextEdit;
        private TextEdit SiteNameTextEdit;
        private TextEdit AddressTextEdit;
        private TextEdit CityTextEdit;
        private TextEdit ProvinceTextEdit;
        private TextEdit CountryTextEdit;
        private TextEdit ContactPersonTextEdit;
        private TextEdit PhoneTextEdit;
        private TextEdit EmailTextEdit;
        private LayoutControlGroup Root;
        private LayoutControlGroup layoutControlGroup1;
        private LayoutControlItem ItemForSiteCode;
        private LayoutControlItem ItemForPartnerName;
        private LayoutControlItem ItemForSiteName;
        private LayoutControlItem ItemForAddress;
        private LayoutControlItem ItemForCity;
        private LayoutControlItem ItemForProvince;
        private LayoutControlItem ItemForCountry;
        private LayoutControlItem ItemForContactPerson;
        private LayoutControlItem ItemForPhone;
        private LayoutControlItem ItemForEmail;
        private LayoutControlItem ItemForIsActive;
        private SearchLookUpEdit PartnerNameTextEdit;
        private GridView searchLookUpEdit1View;
        private BindingSource businessPartnerListDtoBindingSource;
        private GridColumn colPartnerTypeName;
        private GridColumn colPartnerName;
        private GridColumn colFullAddressName;
        private ToggleSwitch IsActiveCheckEdit;
    }
}