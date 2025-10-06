using System.ComponentModel;
using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraLayout;

namespace MasterData.Customer
{
    partial class FrmBusinessPartnerDetail
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
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.PartnerCodeTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.PartnerNameTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.PartnerTypeNameComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.TaxCodeTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.PhoneTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.EmailTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.WebsiteTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.AddressTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.CityTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.CountryTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ContactPersonTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ContactPositionTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.BankAccountTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.BankNameTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.CreditLimitTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.PaymentTermTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.IsActiveToggleSwitch = new DevExpress.XtraEditors.ToggleSwitch();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForPartnerCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForPartnerName = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForTaxCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForPhone = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForEmail = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForWebsite = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForAddress = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForCity = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForCountry = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForContactPerson = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForContactPosition = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForBankAccount = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForBankName = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForCreditLimit = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForPaymentTerm = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForPartnerTypeName = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForIsActive = new DevExpress.XtraLayout.LayoutControlItem();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.SaveBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.CloseBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PartnerCodeTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PartnerNameTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PartnerTypeNameComboBoxEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TaxCodeTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhoneTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmailTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WebsiteTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddressTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CityTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountryTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContactPersonTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContactPositionTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BankAccountTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BankNameTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CreditLimitTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentTermTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsActiveToggleSwitch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPartnerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPartnerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForTaxCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPhone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForEmail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForWebsite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCountry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForContactPerson)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForContactPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForBankAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForBankName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCreditLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPaymentTerm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPartnerTypeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsActive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataLayoutControl1
            // 
            this.dataLayoutControl1.Controls.Add(this.PartnerCodeTextEdit);
            this.dataLayoutControl1.Controls.Add(this.PartnerNameTextEdit);
            this.dataLayoutControl1.Controls.Add(this.PartnerTypeNameComboBoxEdit);
            this.dataLayoutControl1.Controls.Add(this.TaxCodeTextEdit);
            this.dataLayoutControl1.Controls.Add(this.PhoneTextEdit);
            this.dataLayoutControl1.Controls.Add(this.EmailTextEdit);
            this.dataLayoutControl1.Controls.Add(this.WebsiteTextEdit);
            this.dataLayoutControl1.Controls.Add(this.AddressTextEdit);
            this.dataLayoutControl1.Controls.Add(this.CityTextEdit);
            this.dataLayoutControl1.Controls.Add(this.CountryTextEdit);
            this.dataLayoutControl1.Controls.Add(this.ContactPersonTextEdit);
            this.dataLayoutControl1.Controls.Add(this.ContactPositionTextEdit);
            this.dataLayoutControl1.Controls.Add(this.BankAccountTextEdit);
            this.dataLayoutControl1.Controls.Add(this.BankNameTextEdit);
            this.dataLayoutControl1.Controls.Add(this.CreditLimitTextEdit);
            this.dataLayoutControl1.Controls.Add(this.PaymentTermTextEdit);
            this.dataLayoutControl1.Controls.Add(this.IsActiveToggleSwitch);
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 39);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(666, 621);
            this.dataLayoutControl1.TabIndex = 0;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // PartnerCodeTextEdit
            // 
            this.PartnerCodeTextEdit.Location = new System.Drawing.Point(152, 50);
            this.PartnerCodeTextEdit.Name = "PartnerCodeTextEdit";
            this.PartnerCodeTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.PartnerCodeTextEdit.Size = new System.Drawing.Size(498, 28);
            this.PartnerCodeTextEdit.StyleController = this.dataLayoutControl1;
            this.PartnerCodeTextEdit.TabIndex = 4;
            // 
            // PartnerNameTextEdit
            // 
            this.PartnerNameTextEdit.Location = new System.Drawing.Point(152, 84);
            this.PartnerNameTextEdit.Name = "PartnerNameTextEdit";
            this.PartnerNameTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.PartnerNameTextEdit.Size = new System.Drawing.Size(498, 28);
            this.PartnerNameTextEdit.StyleController = this.dataLayoutControl1;
            this.PartnerNameTextEdit.TabIndex = 5;
            // 
            // PartnerTypeNameComboBoxEdit
            // 
            this.PartnerTypeNameComboBoxEdit.Location = new System.Drawing.Point(152, 16);
            this.PartnerTypeNameComboBoxEdit.Name = "PartnerTypeNameComboBoxEdit";
            this.PartnerTypeNameComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.PartnerTypeNameComboBoxEdit.Size = new System.Drawing.Size(498, 28);
            this.PartnerTypeNameComboBoxEdit.StyleController = this.dataLayoutControl1;
            this.PartnerTypeNameComboBoxEdit.TabIndex = 6;
            // 
            // TaxCodeTextEdit
            // 
            this.TaxCodeTextEdit.Location = new System.Drawing.Point(152, 118);
            this.TaxCodeTextEdit.Name = "TaxCodeTextEdit";
            this.TaxCodeTextEdit.Size = new System.Drawing.Size(498, 28);
            this.TaxCodeTextEdit.StyleController = this.dataLayoutControl1;
            this.TaxCodeTextEdit.TabIndex = 7;
            // 
            // PhoneTextEdit
            // 
            this.PhoneTextEdit.Location = new System.Drawing.Point(152, 152);
            this.PhoneTextEdit.Name = "PhoneTextEdit";
            this.PhoneTextEdit.Properties.Mask.EditMask = "(999) 000-0000";
            this.PhoneTextEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
            this.PhoneTextEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.PhoneTextEdit.Size = new System.Drawing.Size(498, 28);
            this.PhoneTextEdit.StyleController = this.dataLayoutControl1;
            this.PhoneTextEdit.TabIndex = 8;
            // 
            // EmailTextEdit
            // 
            this.EmailTextEdit.Location = new System.Drawing.Point(152, 186);
            this.EmailTextEdit.Name = "EmailTextEdit";
            this.EmailTextEdit.Size = new System.Drawing.Size(498, 28);
            this.EmailTextEdit.StyleController = this.dataLayoutControl1;
            this.EmailTextEdit.TabIndex = 9;
            // 
            // WebsiteTextEdit
            // 
            this.WebsiteTextEdit.Location = new System.Drawing.Point(152, 220);
            this.WebsiteTextEdit.Name = "WebsiteTextEdit";
            this.WebsiteTextEdit.Size = new System.Drawing.Size(498, 28);
            this.WebsiteTextEdit.StyleController = this.dataLayoutControl1;
            this.WebsiteTextEdit.TabIndex = 10;
            // 
            // AddressTextEdit
            // 
            this.AddressTextEdit.Location = new System.Drawing.Point(152, 254);
            this.AddressTextEdit.Name = "AddressTextEdit";
            this.AddressTextEdit.Size = new System.Drawing.Size(498, 28);
            this.AddressTextEdit.StyleController = this.dataLayoutControl1;
            this.AddressTextEdit.TabIndex = 11;
            // 
            // CityTextEdit
            // 
            this.CityTextEdit.Location = new System.Drawing.Point(152, 288);
            this.CityTextEdit.Name = "CityTextEdit";
            this.CityTextEdit.Size = new System.Drawing.Size(498, 28);
            this.CityTextEdit.StyleController = this.dataLayoutControl1;
            this.CityTextEdit.TabIndex = 12;
            // 
            // CountryTextEdit
            // 
            this.CountryTextEdit.Location = new System.Drawing.Point(152, 322);
            this.CountryTextEdit.Name = "CountryTextEdit";
            this.CountryTextEdit.Size = new System.Drawing.Size(498, 28);
            this.CountryTextEdit.StyleController = this.dataLayoutControl1;
            this.CountryTextEdit.TabIndex = 13;
            // 
            // ContactPersonTextEdit
            // 
            this.ContactPersonTextEdit.Location = new System.Drawing.Point(152, 356);
            this.ContactPersonTextEdit.Name = "ContactPersonTextEdit";
            this.ContactPersonTextEdit.Size = new System.Drawing.Size(498, 28);
            this.ContactPersonTextEdit.StyleController = this.dataLayoutControl1;
            this.ContactPersonTextEdit.TabIndex = 14;
            // 
            // ContactPositionTextEdit
            // 
            this.ContactPositionTextEdit.Location = new System.Drawing.Point(152, 390);
            this.ContactPositionTextEdit.Name = "ContactPositionTextEdit";
            this.ContactPositionTextEdit.Size = new System.Drawing.Size(498, 28);
            this.ContactPositionTextEdit.StyleController = this.dataLayoutControl1;
            this.ContactPositionTextEdit.TabIndex = 15;
            // 
            // BankAccountTextEdit
            // 
            this.BankAccountTextEdit.Location = new System.Drawing.Point(152, 424);
            this.BankAccountTextEdit.Name = "BankAccountTextEdit";
            this.BankAccountTextEdit.Size = new System.Drawing.Size(498, 28);
            this.BankAccountTextEdit.StyleController = this.dataLayoutControl1;
            this.BankAccountTextEdit.TabIndex = 16;
            // 
            // BankNameTextEdit
            // 
            this.BankNameTextEdit.Location = new System.Drawing.Point(152, 458);
            this.BankNameTextEdit.Name = "BankNameTextEdit";
            this.BankNameTextEdit.Size = new System.Drawing.Size(498, 28);
            this.BankNameTextEdit.StyleController = this.dataLayoutControl1;
            this.BankNameTextEdit.TabIndex = 17;
            // 
            // CreditLimitTextEdit
            // 
            this.CreditLimitTextEdit.Location = new System.Drawing.Point(152, 492);
            this.CreditLimitTextEdit.Name = "CreditLimitTextEdit";
            this.CreditLimitTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.CreditLimitTextEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.CreditLimitTextEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.CreditLimitTextEdit.Properties.Mask.EditMask = "c";
            this.CreditLimitTextEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.CreditLimitTextEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.CreditLimitTextEdit.Size = new System.Drawing.Size(498, 28);
            this.CreditLimitTextEdit.StyleController = this.dataLayoutControl1;
            this.CreditLimitTextEdit.TabIndex = 18;
            // 
            // PaymentTermTextEdit
            // 
            this.PaymentTermTextEdit.Location = new System.Drawing.Point(152, 526);
            this.PaymentTermTextEdit.Name = "PaymentTermTextEdit";
            this.PaymentTermTextEdit.Size = new System.Drawing.Size(498, 28);
            this.PaymentTermTextEdit.StyleController = this.dataLayoutControl1;
            this.PaymentTermTextEdit.TabIndex = 19;
            // 
            // IsActiveToggleSwitch
            // 
            this.IsActiveToggleSwitch.EditValue = true;
            this.IsActiveToggleSwitch.Location = new System.Drawing.Point(145, 560);
            this.IsActiveToggleSwitch.Name = "IsActiveToggleSwitch";
            this.IsActiveToggleSwitch.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.IsActiveToggleSwitch.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.IsActiveToggleSwitch.Properties.OffText = "<color=\'red\'>Ngưng hoạt động</color>";
            this.IsActiveToggleSwitch.Properties.OnText = "<color=\'blue\'>Đang hoạt động</color>";
            this.IsActiveToggleSwitch.Size = new System.Drawing.Size(505, 24);
            this.IsActiveToggleSwitch.StyleController = this.dataLayoutControl1;
            this.IsActiveToggleSwitch.TabIndex = 20;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(666, 621);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AllowDrawBackground = false;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForPartnerCode,
            this.ItemForPartnerName,
            this.ItemForTaxCode,
            this.ItemForPhone,
            this.ItemForEmail,
            this.ItemForWebsite,
            this.ItemForAddress,
            this.ItemForCity,
            this.ItemForCountry,
            this.ItemForContactPerson,
            this.ItemForContactPosition,
            this.ItemForBankAccount,
            this.ItemForBankName,
            this.ItemForCreditLimit,
            this.ItemForPaymentTerm,
            this.ItemForPartnerTypeName,
            this.ItemForIsActive});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "autoGeneratedGroup0";
            this.layoutControlGroup1.Size = new System.Drawing.Size(640, 595);
            // 
            // ItemForPartnerCode
            // 
            this.ItemForPartnerCode.AllowHtmlStringInCaption = true;
            this.ItemForPartnerCode.Control = this.PartnerCodeTextEdit;
            this.ItemForPartnerCode.Location = new System.Drawing.Point(0, 34);
            this.ItemForPartnerCode.Name = "ItemForPartnerCode";
            this.ItemForPartnerCode.OptionsToolTip.AllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForPartnerCode.OptionsToolTip.IconAllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForPartnerCode.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 3, 3);
            this.ItemForPartnerCode.Size = new System.Drawing.Size(640, 34);
            this.ItemForPartnerCode.Text = "Mã đối tác";
            this.ItemForPartnerCode.TextSize = new System.Drawing.Size(113, 13);
            // 
            // ItemForPartnerName
            // 
            this.ItemForPartnerName.Control = this.PartnerNameTextEdit;
            this.ItemForPartnerName.Location = new System.Drawing.Point(0, 68);
            this.ItemForPartnerName.Name = "ItemForPartnerName";
            this.ItemForPartnerName.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 3, 3);
            this.ItemForPartnerName.Size = new System.Drawing.Size(640, 34);
            this.ItemForPartnerName.Text = "Tên đối tác";
            this.ItemForPartnerName.TextSize = new System.Drawing.Size(113, 13);
            // 
            // ItemForTaxCode
            // 
            this.ItemForTaxCode.Control = this.TaxCodeTextEdit;
            this.ItemForTaxCode.Location = new System.Drawing.Point(0, 102);
            this.ItemForTaxCode.Name = "ItemForTaxCode";
            this.ItemForTaxCode.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 3, 3);
            this.ItemForTaxCode.Size = new System.Drawing.Size(640, 34);
            this.ItemForTaxCode.Text = "Mã số thuế";
            this.ItemForTaxCode.TextSize = new System.Drawing.Size(113, 13);
            // 
            // ItemForPhone
            // 
            this.ItemForPhone.Control = this.PhoneTextEdit;
            this.ItemForPhone.Location = new System.Drawing.Point(0, 136);
            this.ItemForPhone.Name = "ItemForPhone";
            this.ItemForPhone.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 3, 3);
            this.ItemForPhone.Size = new System.Drawing.Size(640, 34);
            this.ItemForPhone.Text = "Số điện thoại";
            this.ItemForPhone.TextSize = new System.Drawing.Size(113, 13);
            // 
            // ItemForEmail
            // 
            this.ItemForEmail.Control = this.EmailTextEdit;
            this.ItemForEmail.Location = new System.Drawing.Point(0, 170);
            this.ItemForEmail.Name = "ItemForEmail";
            this.ItemForEmail.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 3, 3);
            this.ItemForEmail.Size = new System.Drawing.Size(640, 34);
            this.ItemForEmail.Text = "Email";
            this.ItemForEmail.TextSize = new System.Drawing.Size(113, 13);
            // 
            // ItemForWebsite
            // 
            this.ItemForWebsite.Control = this.WebsiteTextEdit;
            this.ItemForWebsite.Location = new System.Drawing.Point(0, 204);
            this.ItemForWebsite.Name = "ItemForWebsite";
            this.ItemForWebsite.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 3, 3);
            this.ItemForWebsite.Size = new System.Drawing.Size(640, 34);
            this.ItemForWebsite.Text = "Website";
            this.ItemForWebsite.TextSize = new System.Drawing.Size(113, 13);
            // 
            // ItemForAddress
            // 
            this.ItemForAddress.Control = this.AddressTextEdit;
            this.ItemForAddress.Location = new System.Drawing.Point(0, 238);
            this.ItemForAddress.Name = "ItemForAddress";
            this.ItemForAddress.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 3, 3);
            this.ItemForAddress.Size = new System.Drawing.Size(640, 34);
            this.ItemForAddress.Text = "Địa chỉ";
            this.ItemForAddress.TextSize = new System.Drawing.Size(113, 13);
            // 
            // ItemForCity
            // 
            this.ItemForCity.Control = this.CityTextEdit;
            this.ItemForCity.Location = new System.Drawing.Point(0, 272);
            this.ItemForCity.Name = "ItemForCity";
            this.ItemForCity.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 3, 3);
            this.ItemForCity.Size = new System.Drawing.Size(640, 34);
            this.ItemForCity.Text = "Thành phố";
            this.ItemForCity.TextSize = new System.Drawing.Size(113, 13);
            // 
            // ItemForCountry
            // 
            this.ItemForCountry.Control = this.CountryTextEdit;
            this.ItemForCountry.Location = new System.Drawing.Point(0, 306);
            this.ItemForCountry.Name = "ItemForCountry";
            this.ItemForCountry.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 3, 3);
            this.ItemForCountry.Size = new System.Drawing.Size(640, 34);
            this.ItemForCountry.Text = "Quốc gia";
            this.ItemForCountry.TextSize = new System.Drawing.Size(113, 13);
            // 
            // ItemForContactPerson
            // 
            this.ItemForContactPerson.Control = this.ContactPersonTextEdit;
            this.ItemForContactPerson.Location = new System.Drawing.Point(0, 340);
            this.ItemForContactPerson.Name = "ItemForContactPerson";
            this.ItemForContactPerson.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 3, 3);
            this.ItemForContactPerson.Size = new System.Drawing.Size(640, 34);
            this.ItemForContactPerson.Text = "Người liên hệ";
            this.ItemForContactPerson.TextSize = new System.Drawing.Size(113, 13);
            // 
            // ItemForContactPosition
            // 
            this.ItemForContactPosition.Control = this.ContactPositionTextEdit;
            this.ItemForContactPosition.Location = new System.Drawing.Point(0, 374);
            this.ItemForContactPosition.Name = "ItemForContactPosition";
            this.ItemForContactPosition.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 3, 3);
            this.ItemForContactPosition.Size = new System.Drawing.Size(640, 34);
            this.ItemForContactPosition.Text = "Chức vụ";
            this.ItemForContactPosition.TextSize = new System.Drawing.Size(113, 13);
            // 
            // ItemForBankAccount
            // 
            this.ItemForBankAccount.Control = this.BankAccountTextEdit;
            this.ItemForBankAccount.Location = new System.Drawing.Point(0, 408);
            this.ItemForBankAccount.Name = "ItemForBankAccount";
            this.ItemForBankAccount.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 3, 3);
            this.ItemForBankAccount.Size = new System.Drawing.Size(640, 34);
            this.ItemForBankAccount.Text = "Số tài khoản ngân hàng";
            this.ItemForBankAccount.TextSize = new System.Drawing.Size(113, 13);
            // 
            // ItemForBankName
            // 
            this.ItemForBankName.Control = this.BankNameTextEdit;
            this.ItemForBankName.Location = new System.Drawing.Point(0, 442);
            this.ItemForBankName.Name = "ItemForBankName";
            this.ItemForBankName.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 3, 3);
            this.ItemForBankName.Size = new System.Drawing.Size(640, 34);
            this.ItemForBankName.Text = "Tên ngân hàng";
            this.ItemForBankName.TextSize = new System.Drawing.Size(113, 13);
            // 
            // ItemForCreditLimit
            // 
            this.ItemForCreditLimit.Control = this.CreditLimitTextEdit;
            this.ItemForCreditLimit.Location = new System.Drawing.Point(0, 476);
            this.ItemForCreditLimit.Name = "ItemForCreditLimit";
            this.ItemForCreditLimit.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 3, 3);
            this.ItemForCreditLimit.Size = new System.Drawing.Size(640, 34);
            this.ItemForCreditLimit.Text = "Hạn mức tín dụng";
            this.ItemForCreditLimit.TextSize = new System.Drawing.Size(113, 13);
            // 
            // ItemForPaymentTerm
            // 
            this.ItemForPaymentTerm.Control = this.PaymentTermTextEdit;
            this.ItemForPaymentTerm.Location = new System.Drawing.Point(0, 510);
            this.ItemForPaymentTerm.Name = "ItemForPaymentTerm";
            this.ItemForPaymentTerm.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 3, 3);
            this.ItemForPaymentTerm.Size = new System.Drawing.Size(640, 34);
            this.ItemForPaymentTerm.Text = "Điều khoản thanh toán";
            this.ItemForPaymentTerm.TextSize = new System.Drawing.Size(113, 13);
            // 
            // ItemForPartnerTypeName
            // 
            this.ItemForPartnerTypeName.Control = this.PartnerTypeNameComboBoxEdit;
            this.ItemForPartnerTypeName.Location = new System.Drawing.Point(0, 0);
            this.ItemForPartnerTypeName.Name = "ItemForPartnerTypeName";
            this.ItemForPartnerTypeName.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 3, 3);
            this.ItemForPartnerTypeName.Size = new System.Drawing.Size(640, 34);
            this.ItemForPartnerTypeName.Text = "Loại đối tác";
            this.ItemForPartnerTypeName.TextSize = new System.Drawing.Size(113, 13);
            // 
            // ItemForIsActive
            // 
            this.ItemForIsActive.Control = this.IsActiveToggleSwitch;
            this.ItemForIsActive.Location = new System.Drawing.Point(0, 544);
            this.ItemForIsActive.Name = "ItemForIsActive";
            this.ItemForIsActive.Size = new System.Drawing.Size(640, 51);
            this.ItemForIsActive.Text = "Trạng thái";
            this.ItemForIsActive.TextSize = new System.Drawing.Size(113, 13);
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
            this.SaveBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.SaveBarButtonItem_ItemClick);
            // 
            // CloseBarButtonItem
            // 
            this.CloseBarButtonItem.Caption = "Đóng";
            this.CloseBarButtonItem.Id = 1;
            this.CloseBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.cancel_16x16;
            this.CloseBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.cancel_32x32;
            this.CloseBarButtonItem.Name = "CloseBarButtonItem";
            this.CloseBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.CloseBarButtonItem_ItemClick);
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
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 660);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(666, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 39);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 621);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(666, 39);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 621);
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
            // 
            // FrmBusinessPartnerDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 660);
            this.Controls.Add(this.dataLayoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmBusinessPartnerDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PartnerCodeTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PartnerNameTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PartnerTypeNameComboBoxEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TaxCodeTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhoneTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmailTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WebsiteTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddressTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CityTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountryTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContactPersonTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContactPositionTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BankAccountTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BankNameTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CreditLimitTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentTermTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsActiveToggleSwitch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPartnerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPartnerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForTaxCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPhone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForEmail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForWebsite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCountry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForContactPerson)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForContactPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForBankAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForBankName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCreditLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPaymentTerm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPartnerTypeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataLayoutControl dataLayoutControl1;
        private TextEdit PartnerCodeTextEdit;
        private TextEdit PartnerNameTextEdit;
        private ComboBoxEdit PartnerTypeNameComboBoxEdit;
        private TextEdit TaxCodeTextEdit;
        private TextEdit PhoneTextEdit;
        private TextEdit EmailTextEdit;
        private TextEdit WebsiteTextEdit;
        private TextEdit AddressTextEdit;
        private TextEdit CityTextEdit;
        private TextEdit CountryTextEdit;
        private TextEdit ContactPersonTextEdit;
        private TextEdit ContactPositionTextEdit;
        private TextEdit BankAccountTextEdit;
        private TextEdit BankNameTextEdit;
        private TextEdit CreditLimitTextEdit;
        private TextEdit PaymentTermTextEdit;
        private ToggleSwitch IsActiveToggleSwitch;
        private LayoutControlGroup Root;
        private LayoutControlGroup layoutControlGroup1;
        private LayoutControlItem ItemForPartnerCode;
        private LayoutControlItem ItemForPartnerName;
        private LayoutControlItem ItemForPartnerTypeName;
        private LayoutControlItem ItemForTaxCode;
        private LayoutControlItem ItemForPhone;
        private LayoutControlItem ItemForEmail;
        private LayoutControlItem ItemForWebsite;
        private LayoutControlItem ItemForAddress;
        private LayoutControlItem ItemForCity;
        private LayoutControlItem ItemForCountry;
        private LayoutControlItem ItemForContactPerson;
        private LayoutControlItem ItemForContactPosition;
        private LayoutControlItem ItemForBankAccount;
        private LayoutControlItem ItemForBankName;
        private LayoutControlItem ItemForCreditLimit;
        private LayoutControlItem ItemForPaymentTerm;
        private LayoutControlItem ItemForIsActive;
        private BarManager barManager1;
        private Bar bar2;
        private BarButtonItem SaveBarButtonItem;
        private BarButtonItem CloseBarButtonItem;
        private BarDockControl barDockControlTop;
        private BarDockControl barDockControlBottom;
        private BarDockControl barDockControlLeft;
        private BarDockControl barDockControlRight;
        private DXErrorProvider dxErrorProvider1;
    }
}
