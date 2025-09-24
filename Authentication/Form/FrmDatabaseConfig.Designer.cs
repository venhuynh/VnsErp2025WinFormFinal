namespace Authentication.Form
{
    partial class FrmDatabaseConfig
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
            DevExpress.XtraLayout.RowDefinition rowDefinition5 = new DevExpress.XtraLayout.RowDefinition();
            DevExpress.XtraLayout.RowDefinition rowDefinition6 = new DevExpress.XtraLayout.RowDefinition();
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.databaseConfigBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ServerNameTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.DatabaseNameTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.UserIdTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.PasswordTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ItemForUserId = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForDatabaseName = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForServerName = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForPassword = new DevExpress.XtraLayout.LayoutControlItem();
            this.simpleLabelItem1 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.simpleLabelItem2 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.simpleLabelItem3 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.simpleLabelItem4 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.simpleLabelItem5 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.OKSmpleButton = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.CancelSimpleButton = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.databaseConfigBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ServerNameTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DatabaseNameTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserIdTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PasswordTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUserId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDatabaseName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForServerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // dataLayoutControl1
            // 
            this.dataLayoutControl1.Controls.Add(this.ServerNameTextEdit);
            this.dataLayoutControl1.Controls.Add(this.DatabaseNameTextEdit);
            this.dataLayoutControl1.Controls.Add(this.UserIdTextEdit);
            this.dataLayoutControl1.Controls.Add(this.PasswordTextEdit);
            this.dataLayoutControl1.Controls.Add(this.OKSmpleButton);
            this.dataLayoutControl1.Controls.Add(this.CancelSimpleButton);
            this.dataLayoutControl1.DataSource = this.databaseConfigBindingSource;
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 0);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(460, 201);
            this.dataLayoutControl1.TabIndex = 0;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForUserId,
            this.ItemForDatabaseName,
            this.ItemForServerName,
            this.ItemForPassword,
            this.simpleLabelItem1,
            this.simpleLabelItem2,
            this.simpleLabelItem3,
            this.simpleLabelItem4,
            this.simpleLabelItem5,
            this.layoutControlItem1,
            this.layoutControlItem2});
            this.Root.LayoutMode = DevExpress.XtraLayout.Utils.LayoutMode.Table;
            this.Root.Name = "Root";
            columnDefinition1.SizeType = System.Windows.Forms.SizeType.Absolute;
            columnDefinition1.Width = 100D;
            columnDefinition2.SizeType = System.Windows.Forms.SizeType.Absolute;
            columnDefinition2.Width = 100D;
            columnDefinition3.SizeType = System.Windows.Forms.SizeType.Absolute;
            columnDefinition3.Width = 100D;
            columnDefinition4.SizeType = System.Windows.Forms.SizeType.AutoSize;
            columnDefinition4.Width = 140D;
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
            rowDefinition4.Height = 24D;
            rowDefinition4.SizeType = System.Windows.Forms.SizeType.AutoSize;
            rowDefinition5.Height = 24D;
            rowDefinition5.SizeType = System.Windows.Forms.SizeType.AutoSize;
            rowDefinition6.Height = 48D;
            rowDefinition6.SizeType = System.Windows.Forms.SizeType.AutoSize;
            this.Root.OptionsTableLayoutGroup.RowDefinitions.AddRange(new DevExpress.XtraLayout.RowDefinition[] {
            rowDefinition1,
            rowDefinition2,
            rowDefinition3,
            rowDefinition4,
            rowDefinition5,
            rowDefinition6});
            this.Root.Size = new System.Drawing.Size(460, 201);
            this.Root.TextVisible = false;
            // 
            // databaseConfigBindingSource
            // 
            this.databaseConfigBindingSource.DataSource = typeof(Dal.Connection.DatabaseConfig);
            // 
            // ServerNameTextEdit
            // 
            this.ServerNameTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.databaseConfigBindingSource, "ServerName", true));
            this.ServerNameTextEdit.Location = new System.Drawing.Point(112, 49);
            this.ServerNameTextEdit.Name = "ServerNameTextEdit";
            this.ServerNameTextEdit.Size = new System.Drawing.Size(336, 20);
            this.ServerNameTextEdit.StyleController = this.dataLayoutControl1;
            this.ServerNameTextEdit.TabIndex = 0;
            // 
            // DatabaseNameTextEdit
            // 
            this.DatabaseNameTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.databaseConfigBindingSource, "DatabaseName", true));
            this.DatabaseNameTextEdit.Location = new System.Drawing.Point(112, 73);
            this.DatabaseNameTextEdit.Name = "DatabaseNameTextEdit";
            this.DatabaseNameTextEdit.Size = new System.Drawing.Size(336, 20);
            this.DatabaseNameTextEdit.StyleController = this.dataLayoutControl1;
            this.DatabaseNameTextEdit.TabIndex = 2;
            // 
            // UserIdTextEdit
            // 
            this.UserIdTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.databaseConfigBindingSource, "UserId", true));
            this.UserIdTextEdit.Location = new System.Drawing.Point(112, 97);
            this.UserIdTextEdit.Name = "UserIdTextEdit";
            this.UserIdTextEdit.Size = new System.Drawing.Size(336, 20);
            this.UserIdTextEdit.StyleController = this.dataLayoutControl1;
            this.UserIdTextEdit.TabIndex = 3;
            // 
            // PasswordTextEdit
            // 
            this.PasswordTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.databaseConfigBindingSource, "Password", true));
            this.PasswordTextEdit.Location = new System.Drawing.Point(112, 121);
            this.PasswordTextEdit.Name = "PasswordTextEdit";
            this.PasswordTextEdit.Properties.PasswordChar = '*';
            this.PasswordTextEdit.Properties.UseSystemPasswordChar = true;
            this.PasswordTextEdit.Size = new System.Drawing.Size(336, 20);
            this.PasswordTextEdit.StyleController = this.dataLayoutControl1;
            this.PasswordTextEdit.TabIndex = 4;
            // 
            // ItemForUserId
            // 
            this.ItemForUserId.Control = this.UserIdTextEdit;
            this.ItemForUserId.Location = new System.Drawing.Point(100, 85);
            this.ItemForUserId.Name = "ItemForUserId";
            this.ItemForUserId.OptionsTableLayoutItem.ColumnIndex = 1;
            this.ItemForUserId.OptionsTableLayoutItem.ColumnSpan = 3;
            this.ItemForUserId.OptionsTableLayoutItem.RowIndex = 3;
            this.ItemForUserId.Size = new System.Drawing.Size(340, 24);
            this.ItemForUserId.Text = "User Id";
            this.ItemForUserId.TextVisible = false;
            // 
            // ItemForDatabaseName
            // 
            this.ItemForDatabaseName.Control = this.DatabaseNameTextEdit;
            this.ItemForDatabaseName.Location = new System.Drawing.Point(100, 61);
            this.ItemForDatabaseName.Name = "ItemForDatabaseName";
            this.ItemForDatabaseName.OptionsTableLayoutItem.ColumnIndex = 1;
            this.ItemForDatabaseName.OptionsTableLayoutItem.ColumnSpan = 3;
            this.ItemForDatabaseName.OptionsTableLayoutItem.RowIndex = 2;
            this.ItemForDatabaseName.Size = new System.Drawing.Size(340, 24);
            this.ItemForDatabaseName.Text = "Database Name";
            this.ItemForDatabaseName.TextVisible = false;
            // 
            // ItemForServerName
            // 
            this.ItemForServerName.Control = this.ServerNameTextEdit;
            this.ItemForServerName.Location = new System.Drawing.Point(100, 37);
            this.ItemForServerName.Name = "ItemForServerName";
            this.ItemForServerName.OptionsTableLayoutItem.ColumnIndex = 1;
            this.ItemForServerName.OptionsTableLayoutItem.ColumnSpan = 3;
            this.ItemForServerName.OptionsTableLayoutItem.RowIndex = 1;
            this.ItemForServerName.Size = new System.Drawing.Size(340, 24);
            this.ItemForServerName.Text = "Server Name";
            this.ItemForServerName.TextVisible = false;
            // 
            // ItemForPassword
            // 
            this.ItemForPassword.Control = this.PasswordTextEdit;
            this.ItemForPassword.Location = new System.Drawing.Point(100, 109);
            this.ItemForPassword.Name = "ItemForPassword";
            this.ItemForPassword.OptionsTableLayoutItem.ColumnIndex = 1;
            this.ItemForPassword.OptionsTableLayoutItem.ColumnSpan = 3;
            this.ItemForPassword.OptionsTableLayoutItem.RowIndex = 4;
            this.ItemForPassword.Size = new System.Drawing.Size(340, 24);
            this.ItemForPassword.Text = "Password";
            this.ItemForPassword.TextVisible = false;
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
            this.simpleLabelItem1.Size = new System.Drawing.Size(440, 37);
            this.simpleLabelItem1.Text = "CÀI ĐẶT MÁY CHỦ CƠ SỞ DỮ LIỆU";
            this.simpleLabelItem1.TextSize = new System.Drawing.Size(245, 17);
            // 
            // simpleLabelItem2
            // 
            this.simpleLabelItem2.Location = new System.Drawing.Point(0, 37);
            this.simpleLabelItem2.Name = "simpleLabelItem2";
            this.simpleLabelItem2.OptionsTableLayoutItem.RowIndex = 1;
            this.simpleLabelItem2.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 2, 2, 2);
            this.simpleLabelItem2.Size = new System.Drawing.Size(100, 24);
            this.simpleLabelItem2.Text = "IP/Tên máy chủ";
            this.simpleLabelItem2.TextSize = new System.Drawing.Size(245, 13);
            // 
            // simpleLabelItem3
            // 
            this.simpleLabelItem3.Location = new System.Drawing.Point(0, 61);
            this.simpleLabelItem3.Name = "simpleLabelItem3";
            this.simpleLabelItem3.OptionsTableLayoutItem.RowIndex = 2;
            this.simpleLabelItem3.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 2, 2, 2);
            this.simpleLabelItem3.Size = new System.Drawing.Size(100, 24);
            this.simpleLabelItem3.Text = "Tên CSDL";
            this.simpleLabelItem3.TextSize = new System.Drawing.Size(245, 13);
            // 
            // simpleLabelItem4
            // 
            this.simpleLabelItem4.Location = new System.Drawing.Point(0, 85);
            this.simpleLabelItem4.Name = "simpleLabelItem4";
            this.simpleLabelItem4.OptionsTableLayoutItem.RowIndex = 3;
            this.simpleLabelItem4.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 2, 2, 2);
            this.simpleLabelItem4.Size = new System.Drawing.Size(100, 24);
            this.simpleLabelItem4.Text = "Tên đăng nhập";
            this.simpleLabelItem4.TextSize = new System.Drawing.Size(245, 13);
            // 
            // simpleLabelItem5
            // 
            this.simpleLabelItem5.Location = new System.Drawing.Point(0, 109);
            this.simpleLabelItem5.Name = "simpleLabelItem5";
            this.simpleLabelItem5.OptionsTableLayoutItem.RowIndex = 4;
            this.simpleLabelItem5.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 2, 2, 2);
            this.simpleLabelItem5.Size = new System.Drawing.Size(100, 24);
            this.simpleLabelItem5.Text = "Mật khẩu";
            this.simpleLabelItem5.TextSize = new System.Drawing.Size(245, 13);
            // 
            // OKSmpleButton
            // 
            this.OKSmpleButton.ImageOptions.Image = global::Authentication.Properties.Resources.apply_16x16;
            this.OKSmpleButton.Location = new System.Drawing.Point(120, 153);
            this.OKSmpleButton.Name = "OKSmpleButton";
            this.OKSmpleButton.Size = new System.Drawing.Size(80, 22);
            this.OKSmpleButton.StyleController = this.dataLayoutControl1;
            this.OKSmpleButton.TabIndex = 5;
            this.OKSmpleButton.Text = "Cập nhật";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.OKSmpleButton;
            this.layoutControlItem1.Location = new System.Drawing.Point(100, 133);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.OptionsTableLayoutItem.ColumnIndex = 1;
            this.layoutControlItem1.OptionsTableLayoutItem.RowIndex = 5;
            this.layoutControlItem1.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 10, 10, 10);
            this.layoutControlItem1.Size = new System.Drawing.Size(100, 48);
            this.layoutControlItem1.TextVisible = false;
            // 
            // CancelSimpleButton
            // 
            this.CancelSimpleButton.ImageOptions.Image = global::Authentication.Properties.Resources.cancel_16x16;
            this.CancelSimpleButton.Location = new System.Drawing.Point(220, 153);
            this.CancelSimpleButton.Name = "CancelSimpleButton";
            this.CancelSimpleButton.Size = new System.Drawing.Size(80, 22);
            this.CancelSimpleButton.StyleController = this.dataLayoutControl1;
            this.CancelSimpleButton.TabIndex = 6;
            this.CancelSimpleButton.Text = "Hủy";
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.CancelSimpleButton;
            this.layoutControlItem2.Location = new System.Drawing.Point(200, 133);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.OptionsTableLayoutItem.ColumnIndex = 2;
            this.layoutControlItem2.OptionsTableLayoutItem.RowIndex = 5;
            this.layoutControlItem2.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 10, 10, 10);
            this.layoutControlItem2.Size = new System.Drawing.Size(100, 48);
            this.layoutControlItem2.TextVisible = false;
            // 
            // FrmDatabaseConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 201);
            this.Controls.Add(this.dataLayoutControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmDatabaseConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cài đặt CSDL";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.databaseConfigBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ServerNameTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DatabaseNameTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserIdTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PasswordTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUserId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDatabaseName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForServerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraDataLayout.DataLayoutControl dataLayoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private System.Windows.Forms.BindingSource databaseConfigBindingSource;
        private DevExpress.XtraEditors.TextEdit ServerNameTextEdit;
        private DevExpress.XtraEditors.TextEdit DatabaseNameTextEdit;
        private DevExpress.XtraEditors.TextEdit UserIdTextEdit;
        private DevExpress.XtraEditors.TextEdit PasswordTextEdit;
        private DevExpress.XtraLayout.LayoutControlItem ItemForUserId;
        private DevExpress.XtraLayout.LayoutControlItem ItemForDatabaseName;
        private DevExpress.XtraLayout.LayoutControlItem ItemForServerName;
        private DevExpress.XtraLayout.LayoutControlItem ItemForPassword;
        private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItem1;
        private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItem2;
        private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItem3;
        private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItem4;
        private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItem5;
        private DevExpress.XtraEditors.SimpleButton OKSmpleButton;
        private DevExpress.XtraEditors.SimpleButton CancelSimpleButton;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
    }
}