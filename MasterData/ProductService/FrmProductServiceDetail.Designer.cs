using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraLayout;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DTO.MasterData.ProductService;

namespace MasterData.ProductService
{
    partial class FrmProductServiceDetail
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
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.SaveBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ClearThumbnailBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.CloseBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.CodeTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.NameTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ThumbnailImagePictureEdit = new DevExpress.XtraEditors.PictureEdit();
            this.IsServiceToggleSwitch = new DevExpress.XtraEditors.ToggleSwitch();
            this.IsActiveToggleSwitch = new DevExpress.XtraEditors.ToggleSwitch();
            this.ThumbnailPathButtonEdit = new DevExpress.XtraEditors.ButtonEdit();
            this.DescriptionTextEdit = new DevExpress.XtraEditors.MemoEdit();
            this.CategoryIdSearchLookupEdit = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.productServiceCategoryDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ProductServiceCategorySearchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colFullPathHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForName = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForIsService = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForDescription = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForThumbnailPath = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForThumbnailImage = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForIsActive = new DevExpress.XtraLayout.LayoutControlItem();
            this.CategoryInfoHtmlRepositoryItemHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.colId = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colCategoryCode = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colCategoryName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colDescription = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colParentId = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colParentCategoryName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colLevel = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colHasChildren = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colFullPath = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colProductCount = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.xtraOpenFileDialog1 = new DevExpress.XtraEditors.XtraOpenFileDialog(this.components);
            this.FullPathHtmlHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CodeTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NameTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThumbnailImagePictureEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsServiceToggleSwitch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsActiveToggleSwitch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThumbnailPathButtonEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CategoryIdSearchLookupEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productServiceCategoryDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceCategorySearchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsService)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForThumbnailPath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForThumbnailImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsActive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CategoryInfoHtmlRepositoryItemHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FullPathHtmlHypertextLabel)).BeginInit();
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
            this.CloseBarButtonItem,
            this.ClearThumbnailBarButtonItem});
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
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.ClearThumbnailBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
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
            // 
            // ClearThumbnailBarButtonItem
            // 
            this.ClearThumbnailBarButtonItem.Caption = "Xóa ảnh";
            this.ClearThumbnailBarButtonItem.Id = 2;
            this.ClearThumbnailBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.deletelist_16x16;
            this.ClearThumbnailBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.deletelist_32x32;
            this.ClearThumbnailBarButtonItem.Name = "ClearThumbnailBarButtonItem";
            // 
            // CloseBarButtonItem
            // 
            this.CloseBarButtonItem.Caption = "Đóng";
            this.CloseBarButtonItem.Id = 1;
            this.CloseBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.cancel_16x16;
            this.CloseBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.cancel_32x32;
            this.CloseBarButtonItem.Name = "CloseBarButtonItem";
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
            // dataLayoutControl1
            // 
            this.dataLayoutControl1.Controls.Add(this.CodeTextEdit);
            this.dataLayoutControl1.Controls.Add(this.NameTextEdit);
            this.dataLayoutControl1.Controls.Add(this.ThumbnailImagePictureEdit);
            this.dataLayoutControl1.Controls.Add(this.IsServiceToggleSwitch);
            this.dataLayoutControl1.Controls.Add(this.IsActiveToggleSwitch);
            this.dataLayoutControl1.Controls.Add(this.ThumbnailPathButtonEdit);
            this.dataLayoutControl1.Controls.Add(this.DescriptionTextEdit);
            this.dataLayoutControl1.Controls.Add(this.CategoryIdSearchLookupEdit);
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 39);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(666, 621);
            this.dataLayoutControl1.TabIndex = 10;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // CodeTextEdit
            // 
            this.CodeTextEdit.Location = new System.Drawing.Point(137, 50);
            this.CodeTextEdit.MenuManager = this.barManager1;
            this.CodeTextEdit.Name = "CodeTextEdit";
            this.CodeTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.CodeTextEdit.Size = new System.Drawing.Size(321, 28);
            this.CodeTextEdit.StyleController = this.dataLayoutControl1;
            this.CodeTextEdit.TabIndex = 2;
            // 
            // NameTextEdit
            // 
            this.NameTextEdit.Location = new System.Drawing.Point(137, 84);
            this.NameTextEdit.MenuManager = this.barManager1;
            this.NameTextEdit.Name = "NameTextEdit";
            this.NameTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.NameTextEdit.Size = new System.Drawing.Size(513, 28);
            this.NameTextEdit.StyleController = this.dataLayoutControl1;
            this.NameTextEdit.TabIndex = 4;
            // 
            // ThumbnailImagePictureEdit
            // 
            this.ThumbnailImagePictureEdit.Location = new System.Drawing.Point(137, 292);
            this.ThumbnailImagePictureEdit.MenuManager = this.barManager1;
            this.ThumbnailImagePictureEdit.Name = "ThumbnailImagePictureEdit";
            this.ThumbnailImagePictureEdit.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            this.ThumbnailImagePictureEdit.Size = new System.Drawing.Size(513, 313);
            this.ThumbnailImagePictureEdit.StyleController = this.dataLayoutControl1;
            this.ThumbnailImagePictureEdit.TabIndex = 1;
            // 
            // IsServiceToggleSwitch
            // 
            this.IsServiceToggleSwitch.Location = new System.Drawing.Point(137, 118);
            this.IsServiceToggleSwitch.MenuManager = this.barManager1;
            this.IsServiceToggleSwitch.Name = "IsServiceToggleSwitch";
            this.IsServiceToggleSwitch.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.IsServiceToggleSwitch.Properties.OffText = "Sản phẩm";
            this.IsServiceToggleSwitch.Properties.OnText = "Dịch vụ";
            this.IsServiceToggleSwitch.Size = new System.Drawing.Size(513, 24);
            this.IsServiceToggleSwitch.StyleController = this.dataLayoutControl1;
            this.IsServiceToggleSwitch.TabIndex = 5;
            // 
            // IsActiveToggleSwitch
            // 
            this.IsActiveToggleSwitch.EditValue = true;
            this.IsActiveToggleSwitch.Location = new System.Drawing.Point(464, 50);
            this.IsActiveToggleSwitch.MenuManager = this.barManager1;
            this.IsActiveToggleSwitch.Name = "IsActiveToggleSwitch";
            this.IsActiveToggleSwitch.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.IsActiveToggleSwitch.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.IsActiveToggleSwitch.Properties.OffText = "<color=\'red\'>Ngưng hoạt động</color>";
            this.IsActiveToggleSwitch.Properties.OnText = "<color=\'blue\'>Đang hoạt động</color>";
            this.IsActiveToggleSwitch.Size = new System.Drawing.Size(186, 24);
            this.IsActiveToggleSwitch.StyleController = this.dataLayoutControl1;
            this.IsActiveToggleSwitch.TabIndex = 3;
            // 
            // ThumbnailPathButtonEdit
            // 
            this.ThumbnailPathButtonEdit.Location = new System.Drawing.Point(137, 258);
            this.ThumbnailPathButtonEdit.MenuManager = this.barManager1;
            this.ThumbnailPathButtonEdit.Name = "ThumbnailPathButtonEdit";
            this.ThumbnailPathButtonEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Plus)});
            this.ThumbnailPathButtonEdit.Size = new System.Drawing.Size(513, 28);
            this.ThumbnailPathButtonEdit.StyleController = this.dataLayoutControl1;
            this.ThumbnailPathButtonEdit.TabIndex = 6;
            this.ThumbnailPathButtonEdit.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.ThumbnailPathButtonEdit_ButtonClick);
            // 
            // DescriptionTextEdit
            // 
            this.DescriptionTextEdit.Location = new System.Drawing.Point(137, 148);
            this.DescriptionTextEdit.MenuManager = this.barManager1;
            this.DescriptionTextEdit.Name = "DescriptionTextEdit";
            this.DescriptionTextEdit.Size = new System.Drawing.Size(513, 104);
            this.DescriptionTextEdit.StyleController = this.dataLayoutControl1;
            this.DescriptionTextEdit.TabIndex = 6;
            // 
            // CategoryIdSearchLookupEdit
            // 
            this.CategoryIdSearchLookupEdit.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.True;
            this.CategoryIdSearchLookupEdit.Location = new System.Drawing.Point(137, 16);
            this.CategoryIdSearchLookupEdit.MenuManager = this.barManager1;
            this.CategoryIdSearchLookupEdit.Name = "CategoryIdSearchLookupEdit";
            this.CategoryIdSearchLookupEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.CategoryIdSearchLookupEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CategoryIdSearchLookupEdit.Properties.DataSource = this.productServiceCategoryDtoBindingSource;
            this.CategoryIdSearchLookupEdit.Properties.DisplayMember = "FullPathHtml";
            this.CategoryIdSearchLookupEdit.Properties.PopupView = this.ProductServiceCategorySearchLookUpEdit1View;
            this.CategoryIdSearchLookupEdit.Properties.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.FullPathHtmlHypertextLabel});
            this.CategoryIdSearchLookupEdit.Properties.ValueMember = "Id";
            this.CategoryIdSearchLookupEdit.Size = new System.Drawing.Size(513, 28);
            this.CategoryIdSearchLookupEdit.StyleController = this.dataLayoutControl1;
            this.CategoryIdSearchLookupEdit.TabIndex = 0;
            // 
            // productServiceCategoryDtoBindingSource
            // 
            this.productServiceCategoryDtoBindingSource.DataSource = typeof(DTO.MasterData.ProductService.ProductServiceCategoryDto);
            // 
            // ProductServiceCategorySearchLookUpEdit1View
            // 
            this.ProductServiceCategorySearchLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colFullPathHtml});
            this.ProductServiceCategorySearchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.ProductServiceCategorySearchLookUpEdit1View.Name = "ProductServiceCategorySearchLookUpEdit1View";
            this.ProductServiceCategorySearchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.ProductServiceCategorySearchLookUpEdit1View.OptionsView.RowAutoHeight = true;
            this.ProductServiceCategorySearchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            this.ProductServiceCategorySearchLookUpEdit1View.OptionsView.ShowIndicator = false;
            // 
            // colFullPathHtml
            // 
            this.colFullPathHtml.ColumnEdit = this.FullPathHtmlHypertextLabel;
            this.colFullPathHtml.FieldName = "FullPathHtml";
            this.colFullPathHtml.Name = "colFullPathHtml";
            this.colFullPathHtml.Visible = true;
            this.colFullPathHtml.VisibleIndex = 0;
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
            this.ItemForCode,
            this.ItemForName,
            this.ItemForIsService,
            this.ItemForDescription,
            this.ItemForThumbnailPath,
            this.ItemForThumbnailImage,
            this.layoutControlItem,
            this.ItemForIsActive});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "autoGeneratedGroup0";
            this.layoutControlGroup1.Size = new System.Drawing.Size(640, 595);
            // 
            // ItemForCode
            // 
            this.ItemForCode.Control = this.CodeTextEdit;
            this.ItemForCode.Location = new System.Drawing.Point(0, 34);
            this.ItemForCode.Name = "ItemForCode";
            this.ItemForCode.Size = new System.Drawing.Size(448, 34);
            this.ItemForCode.Text = "Mã sản phẩm/dịch vụ";
            this.ItemForCode.TextSize = new System.Drawing.Size(105, 13);
            // 
            // ItemForName
            // 
            this.ItemForName.Control = this.NameTextEdit;
            this.ItemForName.Location = new System.Drawing.Point(0, 68);
            this.ItemForName.Name = "ItemForName";
            this.ItemForName.Size = new System.Drawing.Size(640, 34);
            this.ItemForName.Text = "Tên sản phẩm/dịch vụ";
            this.ItemForName.TextSize = new System.Drawing.Size(105, 13);
            // 
            // ItemForIsService
            // 
            this.ItemForIsService.Control = this.IsServiceToggleSwitch;
            this.ItemForIsService.Location = new System.Drawing.Point(0, 102);
            this.ItemForIsService.Name = "ItemForIsService";
            this.ItemForIsService.Size = new System.Drawing.Size(640, 30);
            this.ItemForIsService.Text = "Loại";
            this.ItemForIsService.TextSize = new System.Drawing.Size(105, 13);
            // 
            // ItemForDescription
            // 
            this.ItemForDescription.Control = this.DescriptionTextEdit;
            this.ItemForDescription.Location = new System.Drawing.Point(0, 132);
            this.ItemForDescription.Name = "ItemForDescription";
            this.ItemForDescription.Size = new System.Drawing.Size(640, 110);
            this.ItemForDescription.Text = "Mô tả";
            this.ItemForDescription.TextSize = new System.Drawing.Size(105, 13);
            // 
            // ItemForThumbnailPath
            // 
            this.ItemForThumbnailPath.Control = this.ThumbnailPathButtonEdit;
            this.ItemForThumbnailPath.Location = new System.Drawing.Point(0, 242);
            this.ItemForThumbnailPath.Name = "ItemForThumbnailPath";
            this.ItemForThumbnailPath.Size = new System.Drawing.Size(640, 34);
            this.ItemForThumbnailPath.Text = "Đường dẫn ảnh";
            this.ItemForThumbnailPath.TextSize = new System.Drawing.Size(105, 13);
            // 
            // ItemForThumbnailImage
            // 
            this.ItemForThumbnailImage.Control = this.ThumbnailImagePictureEdit;
            this.ItemForThumbnailImage.Location = new System.Drawing.Point(0, 276);
            this.ItemForThumbnailImage.Name = "ItemForThumbnailImage";
            this.ItemForThumbnailImage.Size = new System.Drawing.Size(640, 319);
            this.ItemForThumbnailImage.StartNewLine = true;
            this.ItemForThumbnailImage.Text = "Ảnh đại diện";
            this.ItemForThumbnailImage.TextSize = new System.Drawing.Size(105, 13);
            // 
            // layoutControlItem
            // 
            this.layoutControlItem.Control = this.CategoryIdSearchLookupEdit;
            this.layoutControlItem.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem.Name = "layoutControlItem";
            this.layoutControlItem.Size = new System.Drawing.Size(640, 34);
            this.layoutControlItem.Text = "Loại SPDV";
            this.layoutControlItem.TextSize = new System.Drawing.Size(105, 13);
            // 
            // ItemForIsActive
            // 
            this.ItemForIsActive.Control = this.IsActiveToggleSwitch;
            this.ItemForIsActive.Location = new System.Drawing.Point(448, 34);
            this.ItemForIsActive.Name = "ItemForIsActive";
            this.ItemForIsActive.Size = new System.Drawing.Size(192, 34);
            this.ItemForIsActive.Text = "Đang hoạt động";
            this.ItemForIsActive.TextVisible = false;
            // 
            // CategoryInfoHtmlRepositoryItemHypertextLabel
            // 
            this.CategoryInfoHtmlRepositoryItemHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.CategoryInfoHtmlRepositoryItemHypertextLabel.Name = "CategoryInfoHtmlRepositoryItemHypertextLabel";
            // 
            // colId
            // 
            this.colId.FieldName = "Id";
            this.colId.Name = "colId";
            this.colId.Visible = true;
            this.colId.VisibleIndex = 0;
            // 
            // colCategoryCode
            // 
            this.colCategoryCode.FieldName = "CategoryCode";
            this.colCategoryCode.Name = "colCategoryCode";
            this.colCategoryCode.Visible = true;
            this.colCategoryCode.VisibleIndex = 1;
            // 
            // colCategoryName
            // 
            this.colCategoryName.FieldName = "CategoryName";
            this.colCategoryName.Name = "colCategoryName";
            this.colCategoryName.Visible = true;
            this.colCategoryName.VisibleIndex = 2;
            // 
            // colDescription
            // 
            this.colDescription.FieldName = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.Visible = true;
            this.colDescription.VisibleIndex = 3;
            // 
            // colParentId
            // 
            this.colParentId.FieldName = "ParentId";
            this.colParentId.Name = "colParentId";
            this.colParentId.Visible = true;
            this.colParentId.VisibleIndex = 4;
            // 
            // colParentCategoryName
            // 
            this.colParentCategoryName.FieldName = "ParentCategoryName";
            this.colParentCategoryName.Name = "colParentCategoryName";
            this.colParentCategoryName.Visible = true;
            this.colParentCategoryName.VisibleIndex = 5;
            // 
            // colLevel
            // 
            this.colLevel.FieldName = "Level";
            this.colLevel.Name = "colLevel";
            this.colLevel.Visible = true;
            this.colLevel.VisibleIndex = 6;
            // 
            // colHasChildren
            // 
            this.colHasChildren.FieldName = "HasChildren";
            this.colHasChildren.Name = "colHasChildren";
            this.colHasChildren.Visible = true;
            this.colHasChildren.VisibleIndex = 7;
            // 
            // colFullPath
            // 
            this.colFullPath.FieldName = "FullPath";
            this.colFullPath.Name = "colFullPath";
            this.colFullPath.Visible = true;
            this.colFullPath.VisibleIndex = 8;
            // 
            // colProductCount
            // 
            this.colProductCount.FieldName = "ProductCount";
            this.colProductCount.Name = "colProductCount";
            this.colProductCount.Visible = true;
            this.colProductCount.VisibleIndex = 9;
            // 
            // xtraOpenFileDialog1
            // 
            this.xtraOpenFileDialog1.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.webp)|*.jpg;*.jpeg;*.png;*.bmp;*.gi" +
    "f;*.webp|All Files (*.*)|*.*";
            this.xtraOpenFileDialog1.Title = "Chọn hình ảnh cho sản phẩm/dịch vụ";
            // 
            // FullPathHtmlHypertextLabel
            // 
            this.FullPathHtmlHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.FullPathHtmlHypertextLabel.Name = "FullPathHtmlHypertextLabel";
            // 
            // FrmProductServiceDetail
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
            this.Name = "FrmProductServiceDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CHI TIẾT HÀNG HÓA DỊCH VỤ";
            this.Load += new System.EventHandler(this.FrmProductServiceDetail_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CodeTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NameTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThumbnailImagePictureEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsServiceToggleSwitch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsActiveToggleSwitch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThumbnailPathButtonEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CategoryIdSearchLookupEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productServiceCategoryDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceCategorySearchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsService)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForThumbnailPath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForThumbnailImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CategoryInfoHtmlRepositoryItemHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FullPathHtmlHypertextLabel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private BarManager barManager1;
        private Bar bar2;
        private BarButtonItem SaveBarButtonItem;
        private BarButtonItem CloseBarButtonItem;
        private BarButtonItem ClearThumbnailBarButtonItem;
        private BarDockControl barDockControlTop;
        private BarDockControl barDockControlBottom;
        private BarDockControl barDockControlLeft;
        private BarDockControl barDockControlRight;
        private DXErrorProvider dxErrorProvider1;
        private DataLayoutControl dataLayoutControl1;
        private TextEdit CodeTextEdit;
        private TextEdit NameTextEdit;
        private PictureEdit ThumbnailImagePictureEdit;
        private LayoutControlGroup Root;
        private LayoutControlGroup layoutControlGroup1;
        private LayoutControlItem ItemForCode;
        private LayoutControlItem ItemForName;
        private LayoutControlItem ItemForIsService;
        private LayoutControlItem ItemForDescription;
        private LayoutControlItem ItemForIsActive;
        private LayoutControlItem ItemForThumbnailPath;
        private LayoutControlItem ItemForThumbnailImage;
        private BindingSource productServiceCategoryDtoBindingSource;
        private LayoutControlItem layoutControlItem;
        private ToggleSwitch IsServiceToggleSwitch;
        private ToggleSwitch IsActiveToggleSwitch;
        private ButtonEdit ThumbnailPathButtonEdit;
        private MemoEdit DescriptionTextEdit;
        private TreeListColumn colId;
        private TreeListColumn colCategoryCode;
        private TreeListColumn colCategoryName;
        private TreeListColumn colDescription;
        private TreeListColumn colParentId;
        private TreeListColumn colParentCategoryName;
        private TreeListColumn colLevel;
        private TreeListColumn colHasChildren;
        private TreeListColumn colFullPath;
        private TreeListColumn colProductCount;
        private XtraOpenFileDialog xtraOpenFileDialog1;
        private SearchLookUpEdit CategoryIdSearchLookupEdit;
        private DevExpress.XtraGrid.Views.Grid.GridView ProductServiceCategorySearchLookUpEdit1View;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel CategoryInfoHtmlRepositoryItemHypertextLabel;
        private DevExpress.XtraGrid.Columns.GridColumn colFullPathHtml;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel FullPathHtmlHypertextLabel;
    }
}