namespace VnsErp2025.Form
{
    partial class FrmApplicationVersionManagement
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
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.btnRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.btnSetActive = new DevExpress.XtraEditors.SimpleButton();
            this.btnUpdateFromAssembly = new DevExpress.XtraEditors.SimpleButton();
            this.txtDescription = new DevExpress.XtraEditors.MemoEdit();
            this.txtCurrentVersion = new DevExpress.XtraEditors.TextEdit();
            this.gridControlVersions = new DevExpress.XtraGrid.GridControl();
            this.gridViewVersions = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrentVersion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlVersions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewVersions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.btnRefresh);
            this.layoutControl1.Controls.Add(this.btnSetActive);
            this.layoutControl1.Controls.Add(this.btnUpdateFromAssembly);
            this.layoutControl1.Controls.Add(this.txtDescription);
            this.layoutControl1.Controls.Add(this.txtCurrentVersion);
            this.layoutControl1.Controls.Add(this.gridControlVersions);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(800, 600);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(12, 12);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 22);
            this.btnRefresh.StyleController = this.layoutControl1;
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSetActive
            // 
            this.btnSetActive.Location = new System.Drawing.Point(116, 12);
            this.btnSetActive.Name = "btnSetActive";
            this.btnSetActive.Size = new System.Drawing.Size(100, 22);
            this.btnSetActive.StyleController = this.layoutControl1;
            this.btnSetActive.TabIndex = 4;
            this.btnSetActive.Text = "Đặt làm Active";
            this.btnSetActive.Click += new System.EventHandler(this.btnSetActive_Click);
            // 
            // btnUpdateFromAssembly
            // 
            this.btnUpdateFromAssembly.Location = new System.Drawing.Point(688, 12);
            this.btnUpdateFromAssembly.Name = "btnUpdateFromAssembly";
            this.btnUpdateFromAssembly.Size = new System.Drawing.Size(100, 22);
            this.btnUpdateFromAssembly.StyleController = this.layoutControl1;
            this.btnUpdateFromAssembly.TabIndex = 3;
            this.btnUpdateFromAssembly.Text = "Cập nhật từ Assembly";
            this.btnUpdateFromAssembly.Click += new System.EventHandler(this.btnUpdateFromAssembly_Click);
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(12, 38);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Properties.NullValuePrompt = "Nhập mô tả phiên bản (tùy chọn)";
            this.txtDescription.Size = new System.Drawing.Size(776, 50);
            this.txtDescription.StyleController = this.layoutControl1;
            this.txtDescription.TabIndex = 2;
            // 
            // txtCurrentVersion
            // 
            this.txtCurrentVersion.Location = new System.Drawing.Point(220, 12);
            this.txtCurrentVersion.Name = "txtCurrentVersion";
            this.txtCurrentVersion.Properties.ReadOnly = true;
            this.txtCurrentVersion.Size = new System.Drawing.Size(464, 20);
            this.txtCurrentVersion.StyleController = this.layoutControl1;
            this.txtCurrentVersion.TabIndex = 1;
            // 
            // gridControlVersions
            // 
            this.gridControlVersions.Location = new System.Drawing.Point(12, 92);
            this.gridControlVersions.MainView = this.gridViewVersions;
            this.gridControlVersions.Name = "gridControlVersions";
            this.gridControlVersions.Size = new System.Drawing.Size(776, 496);
            this.gridControlVersions.TabIndex = 0;
            this.gridControlVersions.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewVersions});
            // 
            // gridViewVersions
            // 
            this.gridViewVersions.GridControl = this.gridControlVersions;
            this.gridViewVersions.Name = "gridViewVersions";
            this.gridViewVersions.OptionsView.ShowGroupPanel = false;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(800, 600);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridControlVersions;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 80);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(780, 500);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.txtCurrentVersion;
            this.layoutControlItem2.Location = new System.Drawing.Point(208, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(468, 26);
            this.layoutControlItem2.Text = "Phiên bản hiện tại (Assembly):";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(196, 13);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.txtDescription;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 26);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(780, 54);
            this.layoutControlItem3.Text = "Mô tả:";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(196, 13);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btnUpdateFromAssembly;
            this.layoutControlItem4.Location = new System.Drawing.Point(676, 0);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(104, 26);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btnSetActive;
            this.layoutControlItem5.Location = new System.Drawing.Point(104, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(104, 26);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.btnRefresh;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(104, 26);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // FrmApplicationVersionManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.layoutControl1);
            this.Name = "FrmApplicationVersionManagement";
            this.Text = "Quản lý Phiên bản Ứng dụng";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrentVersion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlVersions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewVersions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraGrid.GridControl gridControlVersions;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewVersions;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.TextEdit txtCurrentVersion;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.MemoEdit txtDescription;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.SimpleButton btnUpdateFromAssembly;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.SimpleButton btnSetActive;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraEditors.SimpleButton btnRefresh;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
    }
}
