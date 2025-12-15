using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraLayout;

namespace VersionAndUserManagement.UserManagement
{
    partial class FrmApplicationUserDtoAddEdit
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
            this.applicationUserDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.UserNameTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ItemForUserName = new DevExpress.XtraLayout.LayoutControlItem();
            this.HashPasswordTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ItemForHashPassword = new DevExpress.XtraLayout.LayoutControlItem();
            this.ActiveCheckEdit = new DevExpress.XtraEditors.CheckEdit();
            this.ItemForActive = new DevExpress.XtraLayout.LayoutControlItem();
            this.HashPasswordTextEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.ItemForHashPassword1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.applicationUserDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserNameTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUserName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HashPasswordTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForHashPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActiveCheckEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForActive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HashPasswordTextEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForHashPassword1)).BeginInit();
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
            this.barManager1.MaxItemId = 3;
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
            this.SaveBarButtonItem.ImageOptions.Image = global::VersionAndUserManagement.Properties.Resources.save_16x16;
            this.SaveBarButtonItem.ImageOptions.LargeImage = global::VersionAndUserManagement.Properties.Resources.save_32x32;
            this.SaveBarButtonItem.Name = "SaveBarButtonItem";
            // 
            // CloseBarButtonItem
            // 
            this.CloseBarButtonItem.Caption = "Đóng";
            this.CloseBarButtonItem.Id = 1;
            this.CloseBarButtonItem.ImageOptions.Image = global::VersionAndUserManagement.Properties.Resources.cancel_16x16;
            this.CloseBarButtonItem.ImageOptions.LargeImage = global::VersionAndUserManagement.Properties.Resources.cancel_32x32;
            this.CloseBarButtonItem.Name = "CloseBarButtonItem";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(599, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 239);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(599, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 215);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(599, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 215);
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
            // 
            // dataLayoutControl1
            // 
            this.dataLayoutControl1.Controls.Add(this.UserNameTextEdit);
            this.dataLayoutControl1.Controls.Add(this.HashPasswordTextEdit);
            this.dataLayoutControl1.Controls.Add(this.ActiveCheckEdit);
            this.dataLayoutControl1.Controls.Add(this.HashPasswordTextEdit1);
            this.dataLayoutControl1.DataSource = this.applicationUserDtoBindingSource;
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 24);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(599, 215);
            this.dataLayoutControl1.TabIndex = 15;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup1,
            this.ItemForHashPassword1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(599, 215);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AllowDrawBackground = false;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForUserName,
            this.ItemForHashPassword,
            this.ItemForActive});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "autoGeneratedGroup0";
            this.layoutControlGroup1.Size = new System.Drawing.Size(579, 48);
            // 
            // applicationUserDtoBindingSource
            // 
            this.applicationUserDtoBindingSource.DataSource = typeof(DTO.VersionAndUserManagementDto.ApplicationUserDto);
            // 
            // UserNameTextEdit
            // 
            this.UserNameTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.applicationUserDtoBindingSource, "UserName", true));
            this.UserNameTextEdit.Location = new System.Drawing.Point(103, 12);
            this.UserNameTextEdit.MenuManager = this.barManager1;
            this.UserNameTextEdit.Name = "UserNameTextEdit";
            this.UserNameTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.UserNameTextEdit.Size = new System.Drawing.Size(379, 20);
            this.UserNameTextEdit.StyleController = this.dataLayoutControl1;
            this.UserNameTextEdit.TabIndex = 5;
            // 
            // ItemForUserName
            // 
            this.ItemForUserName.Control = this.UserNameTextEdit;
            this.ItemForUserName.Location = new System.Drawing.Point(0, 0);
            this.ItemForUserName.Name = "ItemForUserName";
            this.ItemForUserName.Size = new System.Drawing.Size(474, 24);
            this.ItemForUserName.Text = "Tên đăng nhập";
            this.ItemForUserName.TextSize = new System.Drawing.Size(79, 13);
            // 
            // HashPasswordTextEdit
            // 
            this.HashPasswordTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.applicationUserDtoBindingSource, "HashPassword", true));
            this.HashPasswordTextEdit.Location = new System.Drawing.Point(103, 36);
            this.HashPasswordTextEdit.MenuManager = this.barManager1;
            this.HashPasswordTextEdit.Name = "HashPasswordTextEdit";
            this.HashPasswordTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.HashPasswordTextEdit.Size = new System.Drawing.Size(484, 20);
            this.HashPasswordTextEdit.StyleController = this.dataLayoutControl1;
            this.HashPasswordTextEdit.TabIndex = 6;
            // 
            // ItemForHashPassword
            // 
            this.ItemForHashPassword.Control = this.HashPasswordTextEdit;
            this.ItemForHashPassword.Location = new System.Drawing.Point(0, 24);
            this.ItemForHashPassword.Name = "ItemForHashPassword";
            this.ItemForHashPassword.Size = new System.Drawing.Size(579, 24);
            this.ItemForHashPassword.Text = "Mật khẩu (Hash)";
            this.ItemForHashPassword.TextSize = new System.Drawing.Size(79, 13);
            // 
            // ActiveCheckEdit
            // 
            this.ActiveCheckEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.applicationUserDtoBindingSource, "Active", true));
            this.ActiveCheckEdit.Location = new System.Drawing.Point(486, 12);
            this.ActiveCheckEdit.MenuManager = this.barManager1;
            this.ActiveCheckEdit.Name = "ActiveCheckEdit";
            this.ActiveCheckEdit.Properties.Caption = "Đang hoạt động";
            this.ActiveCheckEdit.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.ActiveCheckEdit.Size = new System.Drawing.Size(101, 20);
            this.ActiveCheckEdit.StyleController = this.dataLayoutControl1;
            this.ActiveCheckEdit.TabIndex = 7;
            // 
            // ItemForActive
            // 
            this.ItemForActive.Control = this.ActiveCheckEdit;
            this.ItemForActive.Location = new System.Drawing.Point(474, 0);
            this.ItemForActive.Name = "ItemForActive";
            this.ItemForActive.Size = new System.Drawing.Size(105, 24);
            this.ItemForActive.Text = "Đang hoạt động";
            this.ItemForActive.TextVisible = false;
            // 
            // HashPasswordTextEdit1
            // 
            this.HashPasswordTextEdit1.Location = new System.Drawing.Point(103, 60);
            this.HashPasswordTextEdit1.Name = "HashPasswordTextEdit1";
            this.HashPasswordTextEdit1.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.HashPasswordTextEdit1.Size = new System.Drawing.Size(484, 20);
            this.HashPasswordTextEdit1.StyleController = this.dataLayoutControl1;
            this.HashPasswordTextEdit1.TabIndex = 6;
            // 
            // ItemForHashPassword1
            // 
            this.ItemForHashPassword1.Control = this.HashPasswordTextEdit1;
            this.ItemForHashPassword1.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.ItemForHashPassword1.CustomizationFormText = "Mật khẩu (Hash)";
            this.ItemForHashPassword1.Location = new System.Drawing.Point(0, 48);
            this.ItemForHashPassword1.Name = "ItemForHashPassword1";
            this.ItemForHashPassword1.Size = new System.Drawing.Size(579, 147);
            this.ItemForHashPassword1.Text = "Mật khẩu (Hash)";
            this.ItemForHashPassword1.TextSize = new System.Drawing.Size(79, 13);
            // 
            // FrmApplicationUserDtoAddEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 239);
            this.Controls.Add(this.dataLayoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmApplicationUserDtoAddEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.applicationUserDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserNameTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUserName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HashPasswordTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForHashPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActiveCheckEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HashPasswordTextEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForHashPassword1)).EndInit();
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
        private LayoutControlGroup Root;
        private LayoutControlGroup layoutControlGroup1;
        private BindingSource applicationUserDtoBindingSource;
        private TextEdit UserNameTextEdit;
        private TextEdit HashPasswordTextEdit;
        private CheckEdit ActiveCheckEdit;
        private LayoutControlItem ItemForUserName;
        private LayoutControlItem ItemForHashPassword;
        private LayoutControlItem ItemForActive;
        private TextEdit HashPasswordTextEdit1;
        private LayoutControlItem ItemForHashPassword1;
    }
}