namespace VnsErp2025.Form
{
    partial class FormMain
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
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.DBInfoBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.ConfigSqlServerInfoBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.KhachHangDoiTacBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.SanPhamDichVuBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.CongTyBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.PhanLoaiKhachHangBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.SiteKhachHangBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.LienHeKhachHangDoiTacBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ChiNhanhBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.PhongBanBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ChucVuBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.PhanLoaiSPDVBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.DonViTinhBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.NhanVienBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.BienTheSPDVBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.HinhAnhSPDVBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.PartnerRibbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.CongTyRibbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.SanPhamDichVuRibbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.documentManager1 = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.tabbedView1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbon
            // 
            this.ribbon.CaptionBarItemLinks.Add(this.DBInfoBarStaticItem);
            this.ribbon.CaptionBarItemLinks.Add(this.ConfigSqlServerInfoBarButtonItem);
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.DBInfoBarStaticItem,
            this.ConfigSqlServerInfoBarButtonItem,
            this.ribbon.ExpandCollapseItem,
            this.KhachHangDoiTacBarButtonItem,
            this.SanPhamDichVuBarButtonItem,
            this.CongTyBarButtonItem,
            this.PhanLoaiKhachHangBarButtonItem,
            this.SiteKhachHangBarButtonItem,
            this.LienHeKhachHangDoiTacBarButtonItem,
            this.ChiNhanhBarButtonItem,
            this.PhongBanBarButtonItem,
            this.ChucVuBarButtonItem,
            this.PhanLoaiSPDVBarButtonItem,
            this.DonViTinhBarButtonItem,
            this.NhanVienBarButtonItem,
            this.BienTheSPDVBarButtonItem,
            this.HinhAnhSPDVBarButtonItem});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 18;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbon.Size = new System.Drawing.Size(1303, 201);
            this.ribbon.StatusBar = this.ribbonStatusBar;
            // 
            // DBInfoBarStaticItem
            // 
            this.DBInfoBarStaticItem.Caption = "barStaticItem1";
            this.DBInfoBarStaticItem.Id = 1;
            this.DBInfoBarStaticItem.ImageOptions.Image = global::VnsErp2025.Properties.Resources.publish_16x16;
            this.DBInfoBarStaticItem.ImageOptions.LargeImage = global::VnsErp2025.Properties.Resources.publish_32x32;
            this.DBInfoBarStaticItem.Name = "DBInfoBarStaticItem";
            this.DBInfoBarStaticItem.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            // 
            // ConfigSqlServerInfoBarButtonItem
            // 
            this.ConfigSqlServerInfoBarButtonItem.Caption = "barButtonItem1";
            this.ConfigSqlServerInfoBarButtonItem.Id = 2;
            this.ConfigSqlServerInfoBarButtonItem.ImageOptions.Image = global::VnsErp2025.Properties.Resources.database_16x16;
            this.ConfigSqlServerInfoBarButtonItem.Name = "ConfigSqlServerInfoBarButtonItem";
            this.ConfigSqlServerInfoBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ConfigSqlServerInfoBarButtonItem_ItemClick);
            // 
            // KhachHangDoiTacBarButtonItem
            // 
            this.KhachHangDoiTacBarButtonItem.Caption = "Khách hàng Đối tác";
            this.KhachHangDoiTacBarButtonItem.Id = 4;
            this.KhachHangDoiTacBarButtonItem.ImageOptions.SvgImage = global::VnsErp2025.Properties.Resources.cooperation;
            this.KhachHangDoiTacBarButtonItem.Name = "KhachHangDoiTacBarButtonItem";
            this.KhachHangDoiTacBarButtonItem.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.KhachHangDoiTacBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PartnerButton_ItemClick);
            // 
            // SanPhamDichVuBarButtonItem
            // 
            this.SanPhamDichVuBarButtonItem.Caption = "Sản phẩm Dịch vụ";
            this.SanPhamDichVuBarButtonItem.Id = 5;
            this.SanPhamDichVuBarButtonItem.ImageOptions.SvgImage = global::VnsErp2025.Properties.Resources.product_management;
            this.SanPhamDichVuBarButtonItem.Name = "SanPhamDichVuBarButtonItem";
            this.SanPhamDichVuBarButtonItem.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.SanPhamDichVuBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ProductServiceBtn_ItemClick);
            // 
            // CongTyBarButtonItem
            // 
            this.CongTyBarButtonItem.Caption = "Công ty";
            this.CongTyBarButtonItem.Id = 6;
            this.CongTyBarButtonItem.ImageOptions.SvgImage = global::VnsErp2025.Properties.Resources.home__1_;
            this.CongTyBarButtonItem.Name = "CongTyBarButtonItem";
            // 
            // PhanLoaiKhachHangBarButtonItem
            // 
            this.PhanLoaiKhachHangBarButtonItem.Caption = "Phân loại khách hàng";
            this.PhanLoaiKhachHangBarButtonItem.Id = 7;
            this.PhanLoaiKhachHangBarButtonItem.ImageOptions.SvgImage = global::VnsErp2025.Properties.Resources.remote;
            this.PhanLoaiKhachHangBarButtonItem.Name = "PhanLoaiKhachHangBarButtonItem";
            this.PhanLoaiKhachHangBarButtonItem.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.PhanLoaiKhachHangBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PhanLoaiKhachHangBarButtonItem_ItemClick);
            // 
            // SiteKhachHangBarButtonItem
            // 
            this.SiteKhachHangBarButtonItem.Caption = "Site khách hàng";
            this.SiteKhachHangBarButtonItem.Id = 8;
            this.SiteKhachHangBarButtonItem.ImageOptions.SvgImage = global::VnsErp2025.Properties.Resources.location;
            this.SiteKhachHangBarButtonItem.Name = "SiteKhachHangBarButtonItem";
            this.SiteKhachHangBarButtonItem.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.SiteKhachHangBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.SiteKhachHangBarButtonItem_ItemClick);
            // 
            // LienHeKhachHangDoiTacBarButtonItem
            // 
            this.LienHeKhachHangDoiTacBarButtonItem.Caption = "Liên hệ";
            this.LienHeKhachHangDoiTacBarButtonItem.Id = 9;
            this.LienHeKhachHangDoiTacBarButtonItem.ImageOptions.SvgImage = global::VnsErp2025.Properties.Resources.address_book;
            this.LienHeKhachHangDoiTacBarButtonItem.Name = "LienHeKhachHangDoiTacBarButtonItem";
            this.LienHeKhachHangDoiTacBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.LienHeKhachHangDoiTacBarButtonItem_ItemClick);
            // 
            // ChiNhanhBarButtonItem
            // 
            this.ChiNhanhBarButtonItem.Caption = "Chi nhánh";
            this.ChiNhanhBarButtonItem.Id = 10;
            this.ChiNhanhBarButtonItem.ImageOptions.SvgImage = global::VnsErp2025.Properties.Resources.branch__1_;
            this.ChiNhanhBarButtonItem.Name = "ChiNhanhBarButtonItem";
            // 
            // PhongBanBarButtonItem
            // 
            this.PhongBanBarButtonItem.Caption = "Phòng ban";
            this.PhongBanBarButtonItem.Id = 11;
            this.PhongBanBarButtonItem.ImageOptions.SvgImage = global::VnsErp2025.Properties.Resources.hierarchical_structure;
            this.PhongBanBarButtonItem.Name = "PhongBanBarButtonItem";
            this.PhongBanBarButtonItem.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            // 
            // ChucVuBarButtonItem
            // 
            this.ChucVuBarButtonItem.Caption = "Chức vụ";
            this.ChucVuBarButtonItem.Id = 12;
            this.ChucVuBarButtonItem.ImageOptions.SvgImage = global::VnsErp2025.Properties.Resources.job_position;
            this.ChucVuBarButtonItem.Name = "ChucVuBarButtonItem";
            // 
            // PhanLoaiSPDVBarButtonItem
            // 
            this.PhanLoaiSPDVBarButtonItem.Caption = "Phân loại SPDV";
            this.PhanLoaiSPDVBarButtonItem.Id = 13;
            this.PhanLoaiSPDVBarButtonItem.ImageOptions.SvgImage = global::VnsErp2025.Properties.Resources.brand_image;
            this.PhanLoaiSPDVBarButtonItem.Name = "PhanLoaiSPDVBarButtonItem";
            // 
            // DonViTinhBarButtonItem
            // 
            this.DonViTinhBarButtonItem.Caption = "Đơn vị tính";
            this.DonViTinhBarButtonItem.Id = 14;
            this.DonViTinhBarButtonItem.ImageOptions.SvgImage = global::VnsErp2025.Properties.Resources.cost;
            this.DonViTinhBarButtonItem.Name = "DonViTinhBarButtonItem";
            // 
            // NhanVienBarButtonItem
            // 
            this.NhanVienBarButtonItem.Caption = "Nhân viên";
            this.NhanVienBarButtonItem.Id = 15;
            this.NhanVienBarButtonItem.ImageOptions.SvgImage = global::VnsErp2025.Properties.Resources.bo_employee;
            this.NhanVienBarButtonItem.Name = "NhanVienBarButtonItem";
            // 
            // BienTheSPDVBarButtonItem
            // 
            this.BienTheSPDVBarButtonItem.Caption = "Biến thể SPDV";
            this.BienTheSPDVBarButtonItem.Id = 16;
            this.BienTheSPDVBarButtonItem.ImageOptions.SvgImage = global::VnsErp2025.Properties.Resources.variant;
            this.BienTheSPDVBarButtonItem.Name = "BienTheSPDVBarButtonItem";
            this.BienTheSPDVBarButtonItem.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            // 
            // HinhAnhSPDVBarButtonItem
            // 
            this.HinhAnhSPDVBarButtonItem.Caption = "Hình ảnh SPDV";
            this.HinhAnhSPDVBarButtonItem.Id = 17;
            this.HinhAnhSPDVBarButtonItem.ImageOptions.SvgImage = global::VnsErp2025.Properties.Resources.product_image;
            this.HinhAnhSPDVBarButtonItem.Name = "HinhAnhSPDVBarButtonItem";
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.PartnerRibbonPageGroup,
            this.CongTyRibbonPageGroup,
            this.SanPhamDichVuRibbonPageGroup});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Master Data";
            // 
            // PartnerRibbonPageGroup
            // 
            this.PartnerRibbonPageGroup.ItemLinks.Add(this.KhachHangDoiTacBarButtonItem);
            this.PartnerRibbonPageGroup.ItemLinks.Add(this.PhanLoaiKhachHangBarButtonItem);
            this.PartnerRibbonPageGroup.ItemLinks.Add(this.SiteKhachHangBarButtonItem);
            this.PartnerRibbonPageGroup.ItemLinks.Add(this.LienHeKhachHangDoiTacBarButtonItem);
            this.PartnerRibbonPageGroup.Name = "PartnerRibbonPageGroup";
            this.PartnerRibbonPageGroup.Text = "Khách hàng - Đối tác";
            // 
            // CongTyRibbonPageGroup
            // 
            this.CongTyRibbonPageGroup.ItemLinks.Add(this.CongTyBarButtonItem);
            this.CongTyRibbonPageGroup.ItemLinks.Add(this.ChiNhanhBarButtonItem);
            this.CongTyRibbonPageGroup.ItemLinks.Add(this.PhongBanBarButtonItem);
            this.CongTyRibbonPageGroup.ItemLinks.Add(this.ChucVuBarButtonItem);
            this.CongTyRibbonPageGroup.ItemLinks.Add(this.NhanVienBarButtonItem);
            this.CongTyRibbonPageGroup.Name = "CongTyRibbonPageGroup";
            this.CongTyRibbonPageGroup.Text = "Công ty";
            // 
            // SanPhamDichVuRibbonPageGroup
            // 
            this.SanPhamDichVuRibbonPageGroup.ItemLinks.Add(this.SanPhamDichVuBarButtonItem);
            this.SanPhamDichVuRibbonPageGroup.ItemLinks.Add(this.HinhAnhSPDVBarButtonItem);
            this.SanPhamDichVuRibbonPageGroup.ItemLinks.Add(this.PhanLoaiSPDVBarButtonItem);
            this.SanPhamDichVuRibbonPageGroup.ItemLinks.Add(this.DonViTinhBarButtonItem);
            this.SanPhamDichVuRibbonPageGroup.ItemLinks.Add(this.BienTheSPDVBarButtonItem);
            this.SanPhamDichVuRibbonPageGroup.Name = "SanPhamDichVuRibbonPageGroup";
            this.SanPhamDichVuRibbonPageGroup.Text = "Sản phẩm dịch vụ";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 674);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbon;
            this.ribbonStatusBar.Size = new System.Drawing.Size(1303, 37);
            // 
            // documentManager1
            // 
            this.documentManager1.MdiParent = this;
            this.documentManager1.MenuManager = this.ribbon;
            this.documentManager1.View = this.tabbedView1;
            this.documentManager1.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.tabbedView1});
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1303, 711);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbon);
            this.IsMdiContainer = true;
            this.Name = "FormMain";
            this.Ribbon = this.ribbon;
            this.StatusBar = this.ribbonStatusBar;
            this.Text = "FormMain";
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup PartnerRibbonPageGroup;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraBars.BarStaticItem DBInfoBarStaticItem;
        private DevExpress.XtraBars.BarButtonItem ConfigSqlServerInfoBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem KhachHangDoiTacBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem SanPhamDichVuBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem CongTyBarButtonItem;
        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView1;
        private DevExpress.XtraBars.BarButtonItem PhanLoaiKhachHangBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem SiteKhachHangBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem LienHeKhachHangDoiTacBarButtonItem;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup CongTyRibbonPageGroup;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup SanPhamDichVuRibbonPageGroup;
        private DevExpress.XtraBars.BarButtonItem ChiNhanhBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem PhongBanBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem ChucVuBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem PhanLoaiSPDVBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem DonViTinhBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem NhanVienBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem BienTheSPDVBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem HinhAnhSPDVBarButtonItem;
    }
}