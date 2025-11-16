using System.ComponentModel;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.WinExplorer;
using DevExpress.XtraLayout;

namespace MasterData.ProductService
{
    partial class FrmProductImage
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
            this.ProductImageServiceGridControl = new DevExpress.XtraGrid.GridControl();
            this.ProductImageServiceGWinExplorerView = new DevExpress.XtraGrid.Views.WinExplorer.WinExplorerView();
            this.sidePanel1 = new DevExpress.XtraEditors.SidePanel();
            this.tabPane1 = new DevExpress.XtraBars.Navigation.TabPane();
            this.tabNavigationPage1 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.ResultMemoEdit = new DevExpress.XtraEditors.MemoEdit();
            this.SearchByKeyworkButtonEdit = new DevExpress.XtraEditors.ButtonEdit();
            this.btnAddImage = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup4 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.ProductImageServiceGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductImageServiceGWinExplorerView)).BeginInit();
            this.sidePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabPane1)).BeginInit();
            this.tabPane1.SuspendLayout();
            this.tabNavigationPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResultMemoEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SearchByKeyworkButtonEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // ProductImageServiceGridControl
            // 
            this.ProductImageServiceGridControl.Cursor = System.Windows.Forms.Cursors.Default;
            this.ProductImageServiceGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProductImageServiceGridControl.Location = new System.Drawing.Point(0, 0);
            this.ProductImageServiceGridControl.MainView = this.ProductImageServiceGWinExplorerView;
            this.ProductImageServiceGridControl.Name = "ProductImageServiceGridControl";
            this.ProductImageServiceGridControl.Size = new System.Drawing.Size(1083, 788);
            this.ProductImageServiceGridControl.TabIndex = 0;
            this.ProductImageServiceGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ProductImageServiceGWinExplorerView});
            // 
            // ProductImageServiceGWinExplorerView
            // 
            this.ProductImageServiceGWinExplorerView.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.ProductImageServiceGWinExplorerView.ContextButtonOptions.BottomPanelColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ProductImageServiceGWinExplorerView.ContextButtonOptions.Indent = 3;
            this.ProductImageServiceGWinExplorerView.ContextButtonOptions.TopPanelColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ProductImageServiceGWinExplorerView.GridControl = this.ProductImageServiceGridControl;
            this.ProductImageServiceGWinExplorerView.Name = "ProductImageServiceGWinExplorerView";
            this.ProductImageServiceGWinExplorerView.OptionsImageLoad.AnimationType = DevExpress.Utils.ImageContentAnimationType.Slide;
            this.ProductImageServiceGWinExplorerView.OptionsImageLoad.AsyncLoad = true;
            this.ProductImageServiceGWinExplorerView.OptionsImageLoad.CacheThumbnails = false;
            this.ProductImageServiceGWinExplorerView.OptionsImageLoad.LoadThumbnailImagesFromDataSource = false;
            this.ProductImageServiceGWinExplorerView.OptionsView.Style = DevExpress.XtraGrid.Views.WinExplorer.WinExplorerViewStyle.Large;
            // 
            // sidePanel1
            // 
            this.sidePanel1.Controls.Add(this.tabPane1);
            this.sidePanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.sidePanel1.Location = new System.Drawing.Point(1083, 0);
            this.sidePanel1.Name = "sidePanel1";
            this.sidePanel1.Size = new System.Drawing.Size(300, 788);
            this.sidePanel1.TabIndex = 10;
            this.sidePanel1.Text = "sidePanel1";
            // 
            // tabPane1
            // 
            this.tabPane1.Controls.Add(this.tabNavigationPage1);
            this.tabPane1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPane1.Location = new System.Drawing.Point(1, 0);
            this.tabPane1.Name = "tabPane1";
            this.tabPane1.Pages.AddRange(new DevExpress.XtraBars.Navigation.NavigationPageBase[] {
            this.tabNavigationPage1});
            this.tabPane1.RegularSize = new System.Drawing.Size(299, 788);
            this.tabPane1.SelectedPage = this.tabNavigationPage1;
            this.tabPane1.Size = new System.Drawing.Size(299, 788);
            this.tabPane1.TabIndex = 0;
            this.tabPane1.Text = "tabPane1";
            // 
            // tabNavigationPage1
            // 
            this.tabNavigationPage1.Caption = "Hình ảnh ";
            this.tabNavigationPage1.Controls.Add(this.layoutControl1);
            this.tabNavigationPage1.Name = "tabNavigationPage1";
            this.tabNavigationPage1.Size = new System.Drawing.Size(299, 747);
            // 
            // layoutControl1
            // 
            this.layoutControl1.AllowCustomization = false;
            this.layoutControl1.Controls.Add(this.ResultMemoEdit);
            this.layoutControl1.Controls.Add(this.SearchByKeyworkButtonEdit);
            this.layoutControl1.Controls.Add(this.btnAddImage);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(749, 172, 650, 400);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(299, 747);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // ResultMemoEdit
            // 
            this.ResultMemoEdit.Location = new System.Drawing.Point(3, 95);
            this.ResultMemoEdit.Name = "ResultMemoEdit";
            this.ResultMemoEdit.Size = new System.Drawing.Size(293, 649);
            this.ResultMemoEdit.StyleController = this.layoutControl1;
            this.ResultMemoEdit.TabIndex = 13;
            // 
            // SearchByKeyworkButtonEdit
            // 
            this.SearchByKeyworkButtonEdit.Location = new System.Drawing.Point(71, 48);
            this.SearchByKeyworkButtonEdit.Name = "SearchByKeyworkButtonEdit";
            this.SearchByKeyworkButtonEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Search)});
            this.SearchByKeyworkButtonEdit.Size = new System.Drawing.Size(131, 28);
            this.SearchByKeyworkButtonEdit.StyleController = this.layoutControl1;
            this.SearchByKeyworkButtonEdit.TabIndex = 12;
            // 
            // btnAddImage
            // 
            this.btnAddImage.ImageOptions.Image = global::MasterData.Properties.Resources.addnewdatasource_16x16;
            this.btnAddImage.Location = new System.Drawing.Point(208, 48);
            this.btnAddImage.Name = "btnAddImage";
            this.btnAddImage.Size = new System.Drawing.Size(75, 28);
            this.btnAddImage.StyleController = this.layoutControl1;
            this.btnAddImage.TabIndex = 7;
            this.btnAddImage.Text = "Thêm ảnh";
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.False;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup4,
            this.layoutControlItem1});
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(299, 747);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlGroup4
            // 
            this.layoutControlGroup4.GroupStyle = DevExpress.Utils.GroupStyle.Title;
            this.layoutControlGroup4.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem8,
            this.layoutControlItem2});
            this.layoutControlGroup4.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup4.Name = "layoutControlGroup4";
            this.layoutControlGroup4.Size = new System.Drawing.Size(299, 92);
            this.layoutControlGroup4.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup4.Text = "Hành động";
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.SearchByKeyworkButtonEdit;
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(192, 34);
            this.layoutControlItem8.Text = "Từ khóa";
            this.layoutControlItem8.TextSize = new System.Drawing.Size(39, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btnAddImage;
            this.layoutControlItem2.Location = new System.Drawing.Point(192, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(81, 34);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.ResultMemoEdit;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 92);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(299, 655);
            this.layoutControlItem1.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItem1.TextVisible = false;
            // 
            // FrmProductImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1383, 788);
            this.Controls.Add(this.ProductImageServiceGridControl);
            this.Controls.Add(this.sidePanel1);
            this.Name = "FrmProductImage";
            this.Text = "HÌNH ẢNH SPDV";
            ((System.ComponentModel.ISupportInitialize)(this.ProductImageServiceGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductImageServiceGWinExplorerView)).EndInit();
            this.sidePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabPane1)).EndInit();
            this.tabPane1.ResumeLayout(false);
            this.tabNavigationPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ResultMemoEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SearchByKeyworkButtonEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GridControl ProductImageServiceGridControl;
        private WinExplorerView ProductImageServiceGWinExplorerView;
        private SidePanel sidePanel1;
        private TabPane tabPane1;
        private TabNavigationPage tabNavigationPage1;
        private LayoutControl layoutControl1;
        private LayoutControlGroup layoutControlGroup1;
        private SimpleButton btnAddImage;
        private LayoutControlGroup layoutControlGroup4;
        private LayoutControlItem layoutControlItem2;
        private ButtonEdit SearchByKeyworkButtonEdit;
        private LayoutControlItem layoutControlItem8;
        private MemoEdit ResultMemoEdit;
        private LayoutControlItem layoutControlItem1;
    }
}
