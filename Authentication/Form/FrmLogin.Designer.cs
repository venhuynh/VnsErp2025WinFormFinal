namespace Authentication.Form
{
    partial class FrmLogin
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
            DevExpress.XtraLayout.ColumnDefinition columnDefinition1 = new DevExpress.XtraLayout.ColumnDefinition();
            DevExpress.XtraLayout.ColumnDefinition columnDefinition2 = new DevExpress.XtraLayout.ColumnDefinition();
            DevExpress.XtraLayout.ColumnDefinition columnDefinition3 = new DevExpress.XtraLayout.ColumnDefinition();
            DevExpress.XtraLayout.ColumnDefinition columnDefinition4 = new DevExpress.XtraLayout.ColumnDefinition();
            DevExpress.XtraLayout.RowDefinition rowDefinition1 = new DevExpress.XtraLayout.RowDefinition();
            DevExpress.XtraLayout.RowDefinition rowDefinition2 = new DevExpress.XtraLayout.RowDefinition();
            DevExpress.XtraLayout.RowDefinition rowDefinition3 = new DevExpress.XtraLayout.RowDefinition();
            DevExpress.XtraLayout.RowDefinition rowDefinition4 = new DevExpress.XtraLayout.RowDefinition();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.RememberMeCheckBox = new System.Windows.Forms.CheckBox();
            this.UserNameTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.PasswordTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.OkButton = new DevExpress.XtraEditors.SimpleButton();
            this.CancelSimpleButton = new DevExpress.XtraEditors.SimpleButton();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.simpleLabelItem1 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.simpleLabelItem2 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.simpleLabelItem3 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.dxValidationProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UserNameTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PasswordTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.RememberMeCheckBox);
            this.layoutControl1.Controls.Add(this.UserNameTextEdit);
            this.layoutControl1.Controls.Add(this.PasswordTextEdit);
            this.layoutControl1.Controls.Add(this.OkButton);
            this.layoutControl1.Controls.Add(this.CancelSimpleButton);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(455, 159);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // RememberMeCheckBox
            // 
            this.RememberMeCheckBox.Location = new System.Drawing.Point(320, 105);
            this.RememberMeCheckBox.Name = "RememberMeCheckBox";
            this.RememberMeCheckBox.Size = new System.Drawing.Size(115, 20);
            this.RememberMeCheckBox.TabIndex = 4;
            this.RememberMeCheckBox.Text = "Ghi nhớ";
            this.RememberMeCheckBox.UseVisualStyleBackColor = true;
            // 
            // UserNameTextEdit
            // 
            this.UserNameTextEdit.Location = new System.Drawing.Point(112, 49);
            this.UserNameTextEdit.Name = "UserNameTextEdit";
            this.UserNameTextEdit.Size = new System.Drawing.Size(331, 20);
            this.UserNameTextEdit.StyleController = this.layoutControl1;
            this.UserNameTextEdit.TabIndex = 0;
            // 
            // PasswordTextEdit
            // 
            this.PasswordTextEdit.Location = new System.Drawing.Point(112, 73);
            this.PasswordTextEdit.Name = "PasswordTextEdit";
            this.PasswordTextEdit.Properties.PasswordChar = '*';
            this.PasswordTextEdit.Properties.UseSystemPasswordChar = true;
            this.PasswordTextEdit.Size = new System.Drawing.Size(331, 20);
            this.PasswordTextEdit.StyleController = this.layoutControl1;
            this.PasswordTextEdit.TabIndex = 2;
            // 
            // OkButton
            // 
            this.OkButton.ImageOptions.Image = global::Authentication.Properties.Resources.apply_16x16;
            this.OkButton.Location = new System.Drawing.Point(120, 105);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(80, 22);
            this.OkButton.StyleController = this.layoutControl1;
            this.OkButton.TabIndex = 3;
            this.OkButton.Text = "Đăng nhập";
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // CancelSimpleButton
            // 
            this.CancelSimpleButton.ImageOptions.Image = global::Authentication.Properties.Resources.cancel_16x16;
            this.CancelSimpleButton.Location = new System.Drawing.Point(220, 105);
            this.CancelSimpleButton.Name = "CancelSimpleButton";
            this.CancelSimpleButton.Size = new System.Drawing.Size(80, 22);
            this.CancelSimpleButton.StyleController = this.layoutControl1;
            this.CancelSimpleButton.TabIndex = 5;
            this.CancelSimpleButton.Text = "Hủy";
            this.CancelSimpleButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.simpleLabelItem1,
            this.simpleLabelItem2,
            this.simpleLabelItem3,
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5});
            this.Root.LayoutMode = DevExpress.XtraLayout.Utils.LayoutMode.Table;
            this.Root.Name = "Root";
            columnDefinition1.SizeType = System.Windows.Forms.SizeType.Absolute;
            columnDefinition1.Width = 100D;
            columnDefinition2.SizeType = System.Windows.Forms.SizeType.Absolute;
            columnDefinition2.Width = 100D;
            columnDefinition3.SizeType = System.Windows.Forms.SizeType.Absolute;
            columnDefinition3.Width = 100D;
            columnDefinition4.SizeType = System.Windows.Forms.SizeType.AutoSize;
            columnDefinition4.Width = 135D;
            this.Root.OptionsTableLayoutGroup.ColumnDefinitions.AddRange(new DevExpress.XtraLayout.ColumnDefinition[] {
            columnDefinition1,
            columnDefinition2,
            columnDefinition3,
            columnDefinition4});
            rowDefinition1.Height = 37D;
            rowDefinition1.SizeType = System.Windows.Forms.SizeType.AutoSize;
            rowDefinition2.Height = 24D;
            rowDefinition2.SizeType = System.Windows.Forms.SizeType.AutoSize;
            rowDefinition3.Height = 24D;
            rowDefinition3.SizeType = System.Windows.Forms.SizeType.AutoSize;
            rowDefinition4.Height = 54D;
            rowDefinition4.SizeType = System.Windows.Forms.SizeType.AutoSize;
            this.Root.OptionsTableLayoutGroup.RowDefinitions.AddRange(new DevExpress.XtraLayout.RowDefinition[] {
            rowDefinition1,
            rowDefinition2,
            rowDefinition3,
            rowDefinition4});
            this.Root.Size = new System.Drawing.Size(455, 159);
            this.Root.TextVisible = false;
            // 
            // simpleLabelItem1
            // 
            this.simpleLabelItem1.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.simpleLabelItem1.AppearanceItemCaption.ForeColor = System.Drawing.Color.Blue;
            this.simpleLabelItem1.AppearanceItemCaption.Options.UseFont = true;
            this.simpleLabelItem1.AppearanceItemCaption.Options.UseForeColor = true;
            this.simpleLabelItem1.AppearanceItemCaption.Options.UseTextOptions = true;
            this.simpleLabelItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.simpleLabelItem1.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.simpleLabelItem1.Location = new System.Drawing.Point(0, 0);
            this.simpleLabelItem1.Name = "simpleLabelItem1";
            this.simpleLabelItem1.OptionsTableLayoutItem.ColumnSpan = 4;
            this.simpleLabelItem1.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 10, 10, 10);
            this.simpleLabelItem1.Size = new System.Drawing.Size(435, 37);
            this.simpleLabelItem1.Text = "ĐĂNG NHẬP";
            this.simpleLabelItem1.TextSize = new System.Drawing.Size(87, 17);
            // 
            // simpleLabelItem2
            // 
            this.simpleLabelItem2.Location = new System.Drawing.Point(0, 37);
            this.simpleLabelItem2.Name = "simpleLabelItem2";
            this.simpleLabelItem2.OptionsTableLayoutItem.RowIndex = 1;
            this.simpleLabelItem2.Size = new System.Drawing.Size(100, 24);
            this.simpleLabelItem2.Text = "Tên đăng nhập";
            this.simpleLabelItem2.TextSize = new System.Drawing.Size(87, 13);
            // 
            // simpleLabelItem3
            // 
            this.simpleLabelItem3.Location = new System.Drawing.Point(0, 61);
            this.simpleLabelItem3.Name = "simpleLabelItem3";
            this.simpleLabelItem3.OptionsTableLayoutItem.RowIndex = 2;
            this.simpleLabelItem3.Size = new System.Drawing.Size(100, 24);
            this.simpleLabelItem3.Text = "Mật khẩu";
            this.simpleLabelItem3.TextSize = new System.Drawing.Size(87, 13);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.UserNameTextEdit;
            this.layoutControlItem1.Location = new System.Drawing.Point(100, 37);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.OptionsTableLayoutItem.ColumnIndex = 1;
            this.layoutControlItem1.OptionsTableLayoutItem.ColumnSpan = 3;
            this.layoutControlItem1.OptionsTableLayoutItem.RowIndex = 1;
            this.layoutControlItem1.Size = new System.Drawing.Size(335, 24);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.PasswordTextEdit;
            this.layoutControlItem2.Location = new System.Drawing.Point(100, 61);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.OptionsTableLayoutItem.ColumnIndex = 1;
            this.layoutControlItem2.OptionsTableLayoutItem.ColumnSpan = 3;
            this.layoutControlItem2.OptionsTableLayoutItem.RowIndex = 2;
            this.layoutControlItem2.Size = new System.Drawing.Size(335, 24);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.RememberMeCheckBox;
            this.layoutControlItem3.Location = new System.Drawing.Point(300, 85);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.OptionsTableLayoutItem.ColumnIndex = 3;
            this.layoutControlItem3.OptionsTableLayoutItem.RowIndex = 3;
            this.layoutControlItem3.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 10, 10, 10);
            this.layoutControlItem3.Size = new System.Drawing.Size(135, 54);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.OkButton;
            this.layoutControlItem4.Location = new System.Drawing.Point(100, 85);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.OptionsTableLayoutItem.ColumnIndex = 1;
            this.layoutControlItem4.OptionsTableLayoutItem.RowIndex = 3;
            this.layoutControlItem4.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 10, 10, 10);
            this.layoutControlItem4.Size = new System.Drawing.Size(100, 54);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.CancelSimpleButton;
            this.layoutControlItem5.Location = new System.Drawing.Point(200, 85);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.OptionsTableLayoutItem.ColumnIndex = 2;
            this.layoutControlItem5.OptionsTableLayoutItem.RowIndex = 3;
            this.layoutControlItem5.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 10, 10, 10);
            this.layoutControlItem5.Size = new System.Drawing.Size(100, 54);
            this.layoutControlItem5.TextVisible = false;
            // 
            // FrmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 159);
            this.Controls.Add(this.layoutControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.UserNameTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PasswordTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraEditors.TextEdit UserNameTextEdit;
        private DevExpress.XtraEditors.TextEdit PasswordTextEdit;
        private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItem1;
        private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItem2;
        private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.CheckBox RememberMeCheckBox;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.SimpleButton OkButton;
        private DevExpress.XtraEditors.SimpleButton CancelSimpleButton;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider dxValidationProvider1;
    }
}