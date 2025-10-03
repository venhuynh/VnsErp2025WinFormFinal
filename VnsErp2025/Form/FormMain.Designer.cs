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
            this.PartnerButton = new DevExpress.XtraBars.BarButtonItem();
            this.ProductServiceBtn = new DevExpress.XtraBars.BarButtonItem();
            this.MyCompanyBtn = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
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
            this.PartnerButton,
            this.ProductServiceBtn,
            this.MyCompanyBtn});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 7;
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
            // PartnerButton
            // 
            this.PartnerButton.Caption = "Khách hàng Đối tác";
            this.PartnerButton.Id = 4;
            this.PartnerButton.ImageOptions.Image = global::VnsErp2025.Properties.Resources.publicfix_16x16;
            this.PartnerButton.ImageOptions.LargeImage = global::VnsErp2025.Properties.Resources.publicfix_32x32;
            this.PartnerButton.Name = "PartnerButton";
            this.PartnerButton.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.PartnerButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PartnerButton_ItemClick);
            // 
            // ProductServiceBtn
            // 
            this.ProductServiceBtn.Caption = "Sản phẩm Dịch vụ";
            this.ProductServiceBtn.Id = 5;
            this.ProductServiceBtn.ImageOptions.Image = global::VnsErp2025.Properties.Resources.boproductgroup_16x16;
            this.ProductServiceBtn.ImageOptions.LargeImage = global::VnsErp2025.Properties.Resources.boproductgroup_32x32;
            this.ProductServiceBtn.Name = "ProductServiceBtn";
            this.ProductServiceBtn.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.ProductServiceBtn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ProductServiceBtn_ItemClick);
            // 
            // MyCompanyBtn
            // 
            this.MyCompanyBtn.Caption = "Công ty";
            this.MyCompanyBtn.Id = 6;
            this.MyCompanyBtn.ImageOptions.Image = global::VnsErp2025.Properties.Resources.home_16x16;
            this.MyCompanyBtn.ImageOptions.LargeImage = global::VnsErp2025.Properties.Resources.home_32x32;
            this.MyCompanyBtn.Name = "MyCompanyBtn";
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Master Data";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.PartnerButton);
            this.ribbonPageGroup1.ItemLinks.Add(this.ProductServiceBtn);
            this.ribbonPageGroup1.ItemLinks.Add(this.MyCompanyBtn);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "Master Data";
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
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraBars.BarStaticItem DBInfoBarStaticItem;
        private DevExpress.XtraBars.BarButtonItem ConfigSqlServerInfoBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem PartnerButton;
        private DevExpress.XtraBars.BarButtonItem ProductServiceBtn;
        private DevExpress.XtraBars.BarButtonItem MyCompanyBtn;
        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView1;
    }
}