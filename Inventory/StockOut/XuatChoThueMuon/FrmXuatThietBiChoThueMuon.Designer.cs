namespace Inventory.StockOut.XuatChoThueMuon
{
    partial class FrmXuatThietBiChoThueMuon
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
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.NhapLaiBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.LuuPhieuBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.InPhieuBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.NhapQuanLyTaiSanBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ThemHinhAnhBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.CloseBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.HotKeyBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dockPanel2 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel2_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.layoutControl2 = new DevExpress.XtraLayout.LayoutControl();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ucXuatThietBiChoThueMuonMasterDto1 = new Inventory.StockOut.XuatChoThueMuon.UcXuatThietBiChoThueMuonMasterDto();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.ucXuatThietBiChoThueMuonDetailDto1 = new Inventory.StockOut.XuatChoThueMuon.UcXuatThietBiChoThueMuonDetailDto();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.dockPanel2.SuspendLayout();
            this.dockPanel2_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).BeginInit();
            this.layoutControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.ucXuatThietBiChoThueMuonDetailDto1);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(483, 24);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(888, 768);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem4});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(888, 768);
            this.Root.TextVisible = false;
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.MenuManager = this.barManager1;
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanel2});
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl",
            "DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl",
            "DevExpress.XtraBars.ToolbarForm.ToolbarFormControl"});
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.DockManager = this.dockManager1;
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.NhapLaiBarButtonItem,
            this.LuuPhieuBarButtonItem,
            this.InPhieuBarButtonItem,
            this.CloseBarButtonItem,
            this.NhapQuanLyTaiSanBarButtonItem,
            this.ThemHinhAnhBarButtonItem,
            this.HotKeyBarStaticItem});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 7;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.NhapLaiBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.LuuPhieuBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.InPhieuBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.NhapQuanLyTaiSanBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.ThemHinhAnhBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.CloseBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.HotKeyBarStaticItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // NhapLaiBarButtonItem
            // 
            this.NhapLaiBarButtonItem.Caption = "Nhập lại";
            this.NhapLaiBarButtonItem.Id = 0;
            this.NhapLaiBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.reset2_16x16;
            this.NhapLaiBarButtonItem.ImageOptions.LargeImage = global::Inventory.Properties.Resources.reset2_32x32;
            this.NhapLaiBarButtonItem.Name = "NhapLaiBarButtonItem";
            // 
            // LuuPhieuBarButtonItem
            // 
            this.LuuPhieuBarButtonItem.Caption = "Lưu phiếu";
            this.LuuPhieuBarButtonItem.Id = 1;
            this.LuuPhieuBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.save_16x16;
            this.LuuPhieuBarButtonItem.Name = "LuuPhieuBarButtonItem";
            // 
            // InPhieuBarButtonItem
            // 
            this.InPhieuBarButtonItem.Caption = "In phiếu";
            this.InPhieuBarButtonItem.Id = 2;
            this.InPhieuBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.print_16x16;
            this.InPhieuBarButtonItem.ImageOptions.LargeImage = global::Inventory.Properties.Resources.print_32x32;
            this.InPhieuBarButtonItem.Name = "InPhieuBarButtonItem";
            // 
            // NhapQuanLyTaiSanBarButtonItem
            // 
            this.NhapQuanLyTaiSanBarButtonItem.Caption = "Nhập quản lý tài sản";
            this.NhapQuanLyTaiSanBarButtonItem.Id = 4;
            this.NhapQuanLyTaiSanBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.barcode_16x16;
            this.NhapQuanLyTaiSanBarButtonItem.ImageOptions.LargeImage = global::Inventory.Properties.Resources.barcode_32x32;
            this.NhapQuanLyTaiSanBarButtonItem.Name = "NhapQuanLyTaiSanBarButtonItem";
            // 
            // ThemHinhAnhBarButtonItem
            // 
            this.ThemHinhAnhBarButtonItem.Caption = "Thêm hình ảnh";
            this.ThemHinhAnhBarButtonItem.Id = 5;
            this.ThemHinhAnhBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.insertimage_16x16;
            this.ThemHinhAnhBarButtonItem.ImageOptions.LargeImage = global::Inventory.Properties.Resources.insertimage_32x32;
            this.ThemHinhAnhBarButtonItem.Name = "ThemHinhAnhBarButtonItem";
            // 
            // CloseBarButtonItem
            // 
            this.CloseBarButtonItem.Caption = "Đóng";
            this.CloseBarButtonItem.Id = 3;
            this.CloseBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.cancel_16x16;
            this.CloseBarButtonItem.ImageOptions.LargeImage = global::Inventory.Properties.Resources.cancel_32x32;
            this.CloseBarButtonItem.Name = "CloseBarButtonItem";
            // 
            // HotKeyBarStaticItem
            // 
            this.HotKeyBarStaticItem.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.HotKeyBarStaticItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.HotKeyBarStaticItem.Caption = "barStaticItem1";
            this.HotKeyBarStaticItem.Id = 6;
            this.HotKeyBarStaticItem.Name = "HotKeyBarStaticItem";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1371, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 792);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1371, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 768);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1371, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 768);
            // 
            // dockPanel2
            // 
            this.dockPanel2.Controls.Add(this.dockPanel2_Container);
            this.dockPanel2.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.dockPanel2.ID = new System.Guid("54f2f597-37f8-4f4b-8a0d-e5c712426dc2");
            this.dockPanel2.Location = new System.Drawing.Point(0, 24);
            this.dockPanel2.Name = "dockPanel2";
            this.dockPanel2.Options.ShowCloseButton = false;
            this.dockPanel2.OriginalSize = new System.Drawing.Size(483, 200);
            this.dockPanel2.Size = new System.Drawing.Size(483, 768);
            this.dockPanel2.Text = "THÔNG TIN PHIẾU XUẤT THIẾT BỊ CHO MƯỢN - THUÊ";
            // 
            // dockPanel2_Container
            // 
            this.dockPanel2_Container.Controls.Add(this.layoutControl2);
            this.dockPanel2_Container.Location = new System.Drawing.Point(3, 26);
            this.dockPanel2_Container.Name = "dockPanel2_Container";
            this.dockPanel2_Container.Size = new System.Drawing.Size(476, 739);
            this.dockPanel2_Container.TabIndex = 0;
            // 
            // layoutControl2
            // 
            this.layoutControl2.Controls.Add(this.ucXuatThietBiChoThueMuonMasterDto1);
            this.layoutControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl2.Location = new System.Drawing.Point(0, 0);
            this.layoutControl2.Name = "layoutControl2";
            this.layoutControl2.Root = this.layoutControlGroup1;
            this.layoutControl2.Size = new System.Drawing.Size(476, 739);
            this.layoutControl2.TabIndex = 0;
            this.layoutControl2.Text = "layoutControl2";
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(476, 739);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // ucXuatThietBiChoThueMuonMasterDto1
            // 
            this.ucXuatThietBiChoThueMuonMasterDto1.Location = new System.Drawing.Point(12, 12);
            this.ucXuatThietBiChoThueMuonMasterDto1.Name = "ucXuatThietBiChoThueMuonMasterDto1";
            this.ucXuatThietBiChoThueMuonMasterDto1.Size = new System.Drawing.Size(452, 715);
            this.ucXuatThietBiChoThueMuonMasterDto1.TabIndex = 4;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.ucXuatThietBiChoThueMuonMasterDto1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(456, 719);
            this.layoutControlItem1.TextVisible = false;
            // 
            // ucXuatThietBiChoThueMuonDetailDto1
            // 
            this.ucXuatThietBiChoThueMuonDetailDto1.Location = new System.Drawing.Point(12, 12);
            this.ucXuatThietBiChoThueMuonDetailDto1.Name = "ucXuatThietBiChoThueMuonDetailDto1";
            this.ucXuatThietBiChoThueMuonDetailDto1.Size = new System.Drawing.Size(864, 744);
            this.ucXuatThietBiChoThueMuonDetailDto1.TabIndex = 4;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.ucXuatThietBiChoThueMuonDetailDto1;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(868, 748);
            this.layoutControlItem4.TextVisible = false;
            // 
            // FrmXuatThietBiChoThueMuon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1371, 792);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.dockPanel2);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FrmXuatThietBiChoThueMuon";
            this.Text = "PHIẾU XUẤT THIẾT BỊ CHO MƯỢN - THUÊ";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.dockPanel2.ResumeLayout(false);
            this.dockPanel2_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).EndInit();
            this.layoutControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel2;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel2_Container;
        private DevExpress.XtraLayout.LayoutControl layoutControl2;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarButtonItem NhapLaiBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem LuuPhieuBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem InPhieuBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem CloseBarButtonItem;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem NhapQuanLyTaiSanBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem ThemHinhAnhBarButtonItem;
        private DevExpress.XtraBars.BarStaticItem HotKeyBarStaticItem;
        private UcXuatThietBiChoThueMuonMasterDto ucXuatThietBiChoThueMuonMasterDto1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private UcXuatThietBiChoThueMuonDetailDto ucXuatThietBiChoThueMuonDetailDto1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
    }
}