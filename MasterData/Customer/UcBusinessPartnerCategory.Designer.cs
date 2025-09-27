using MasterData.Dto.Customer;

namespace MasterData.Customer
{
    partial class UcBusinessPartnerCategory
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
            this.ListDataBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.NewBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.EditBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.DeleteBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ExportBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.businessPartnerCategoryDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.colCategoryName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colDescription = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colPartnerCount = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.businessPartnerCategoryDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
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
            this.NewBarButtonItem,
            this.ListDataBarButtonItem,
            this.EditBarButtonItem,
            this.DeleteBarButtonItem,
            this.ExportBarButtonItem});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 6;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.ListDataBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.NewBarButtonItem, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.EditBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.DeleteBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.ExportBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // ListDataBarButtonItem
            // 
            this.ListDataBarButtonItem.Caption = "Danh sách";
            this.ListDataBarButtonItem.Id = 1;
            this.ListDataBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.list_16x16;
            this.ListDataBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.list_32x32;
            this.ListDataBarButtonItem.Name = "ListDataBarButtonItem";
            // 
            // NewBarButtonItem
            // 
            this.NewBarButtonItem.Caption = "Mới";
            this.NewBarButtonItem.Id = 0;
            this.NewBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.addnewdatasource_16x16;
            this.NewBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.addnewdatasource_32x32;
            this.NewBarButtonItem.Name = "NewBarButtonItem";
            // 
            // EditBarButtonItem
            // 
            this.EditBarButtonItem.Caption = "Điều chỉnh";
            this.EditBarButtonItem.Id = 2;
            this.EditBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.edittask_16x16;
            this.EditBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.edittask_32x32;
            this.EditBarButtonItem.Name = "EditBarButtonItem";
            // 
            // DeleteBarButtonItem
            // 
            this.DeleteBarButtonItem.Caption = "Xóa";
            this.DeleteBarButtonItem.Id = 3;
            this.DeleteBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.deletelist_16x16;
            this.DeleteBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.deletelist_32x32;
            this.DeleteBarButtonItem.Name = "DeleteBarButtonItem";
            // 
            // ExportBarButtonItem
            // 
            this.ExportBarButtonItem.Caption = "Xuất";
            this.ExportBarButtonItem.Id = 4;
            this.ExportBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.exporttoxls_16x16;
            this.ExportBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.exporttoxls_32x32;
            this.ExportBarButtonItem.Name = "ExportBarButtonItem";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1077, 39);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 661);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1077, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 39);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 622);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1077, 39);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 622);
            // 
            // businessPartnerCategoryDtoBindingSource
            // 
            this.businessPartnerCategoryDtoBindingSource.DataSource = typeof(BusinessPartnerCategoryDto);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.treeList1);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 39);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(1077, 622);
            this.layoutControl1.TabIndex = 4;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // treeList1
            // 
            this.treeList1.Appearance.Caption.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.treeList1.Appearance.Caption.ForeColor = System.Drawing.Color.Blue;
            this.treeList1.Appearance.Caption.Options.UseFont = true;
            this.treeList1.Appearance.Caption.Options.UseForeColor = true;
            this.treeList1.Caption = "BẢNG QUẢN LÝ PHÂN LOẠI ĐỔI TÁC";
            this.treeList1.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colCategoryName,
            this.colDescription,
            this.colPartnerCount});
            this.treeList1.DataSource = this.businessPartnerCategoryDtoBindingSource;
            this.treeList1.KeyFieldName = "Id";
            this.treeList1.Location = new System.Drawing.Point(16, 16);
            this.treeList1.MenuManager = this.barManager1;
            this.treeList1.Name = "treeList1";
            this.treeList1.OptionsBehavior.AllowRecursiveNodeChecking = true;
            this.treeList1.OptionsSelection.MultiSelect = true;
            this.treeList1.OptionsView.CheckBoxStyle = DevExpress.XtraTreeList.DefaultNodeCheckBoxStyle.Check;
            this.treeList1.OptionsView.ShowCaption = true;
            this.treeList1.ParentFieldName = "ParentId";
            this.treeList1.Size = new System.Drawing.Size(1045, 590);
            this.treeList1.TabIndex = 4;
            // 
            // colCategoryName
            // 
            this.colCategoryName.FieldName = "CategoryName";
            this.colCategoryName.Name = "colCategoryName";
            this.colCategoryName.Visible = true;
            this.colCategoryName.VisibleIndex = 0;
            // 
            // colDescription
            // 
            this.colDescription.FieldName = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.Visible = true;
            this.colDescription.VisibleIndex = 1;
            // 
            // colPartnerCount
            // 
            this.colPartnerCount.Caption = "Số lượng";
            this.colPartnerCount.FieldName = "PartnerCount";
            this.colPartnerCount.Name = "colPartnerCount";
            this.colPartnerCount.Visible = true;
            this.colPartnerCount.VisibleIndex = 2;
            // 
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1077, 622);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.treeList1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1051, 596);
            this.layoutControlItem1.TextVisible = false;
            // 
            // UcBusinessPartnerCategory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "UcBusinessPartnerCategory";
            this.Size = new System.Drawing.Size(1077, 661);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.businessPartnerCategoryDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraBars.BarButtonItem ListDataBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem NewBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem EditBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem DeleteBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem ExportBarButtonItem;
        private System.Windows.Forms.BindingSource businessPartnerCategoryDtoBindingSource;
        private DevExpress.XtraTreeList.TreeList treeList1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colCategoryName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colDescription;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colPartnerCount;
    }
}
