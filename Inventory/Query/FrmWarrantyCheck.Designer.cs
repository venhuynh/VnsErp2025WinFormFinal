using DTO.DeviceAssetManagement;

namespace Inventory.Query
{
    partial class FrmWarrantyCheck
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
            this.WarrantyCheckListDtoGridControl = new DevExpress.XtraGrid.GridControl();
            this.warrantyCheckListDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.WarrantyCheckListDtoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colWarrantyTypeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWarrantyStatusName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWarrantyFrom = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMonthOfWarranty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWarrantyUntil = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUniqueProductInfoHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.HtmlRepositoryItemHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.KeyWordBarEditItem = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.TuNgayBarEditItem = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.DenNgayBarEditItem = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.XemBaoCaoBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.XoaPhieuBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.XuatFileBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barHeaderItem1 = new DevExpress.XtraBars.BarHeaderItem();
            this.DataSummaryBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.SelectedRowBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyCheckListDtoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.warrantyCheckListDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyCheckListDtoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlRepositoryItemHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.WarrantyCheckListDtoGridControl);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 24);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(1371, 746);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // WarrantyCheckListDtoGridControl
            // 
            this.WarrantyCheckListDtoGridControl.DataSource = this.warrantyCheckListDtoBindingSource;
            this.WarrantyCheckListDtoGridControl.Location = new System.Drawing.Point(12, 12);
            this.WarrantyCheckListDtoGridControl.MainView = this.WarrantyCheckListDtoGridView;
            this.WarrantyCheckListDtoGridControl.MenuManager = this.barManager1;
            this.WarrantyCheckListDtoGridControl.Name = "WarrantyCheckListDtoGridControl";
            this.WarrantyCheckListDtoGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.HtmlRepositoryItemHypertextLabel});
            this.WarrantyCheckListDtoGridControl.Size = new System.Drawing.Size(1347, 722);
            this.WarrantyCheckListDtoGridControl.TabIndex = 4;
            this.WarrantyCheckListDtoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.WarrantyCheckListDtoGridView});
            // 
            // warrantyCheckListDtoBindingSource
            // 
            this.warrantyCheckListDtoBindingSource.DataSource = typeof(WarrantyCheckListDto);
            // 
            // WarrantyCheckListDtoGridView
            // 
            this.WarrantyCheckListDtoGridView.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.WarrantyCheckListDtoGridView.Appearance.ViewCaption.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.WarrantyCheckListDtoGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.WarrantyCheckListDtoGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.WarrantyCheckListDtoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colWarrantyTypeName,
            this.colWarrantyStatusName,
            this.colWarrantyFrom,
            this.colMonthOfWarranty,
            this.colWarrantyUntil,
            this.colUniqueProductInfoHtml});
            this.WarrantyCheckListDtoGridView.GridControl = this.WarrantyCheckListDtoGridControl;
            this.WarrantyCheckListDtoGridView.IndicatorWidth = 50;
            this.WarrantyCheckListDtoGridView.Name = "WarrantyCheckListDtoGridView";
            this.WarrantyCheckListDtoGridView.OptionsFind.AlwaysVisible = true;
            this.WarrantyCheckListDtoGridView.OptionsSelection.MultiSelect = true;
            this.WarrantyCheckListDtoGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.WarrantyCheckListDtoGridView.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.WarrantyCheckListDtoGridView.OptionsView.RowAutoHeight = true;
            this.WarrantyCheckListDtoGridView.OptionsView.ShowGroupPanel = false;
            this.WarrantyCheckListDtoGridView.OptionsView.ShowViewCaption = true;
            this.WarrantyCheckListDtoGridView.ViewCaption = "BẢNG QUẢN LÝ BẢO HÀNH";
            // 
            // colWarrantyTypeName
            // 
            this.colWarrantyTypeName.AppearanceCell.Options.UseTextOptions = true;
            this.colWarrantyTypeName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyTypeName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyTypeName.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colWarrantyTypeName.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colWarrantyTypeName.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colWarrantyTypeName.AppearanceHeader.Options.UseBackColor = true;
            this.colWarrantyTypeName.AppearanceHeader.Options.UseFont = true;
            this.colWarrantyTypeName.AppearanceHeader.Options.UseForeColor = true;
            this.colWarrantyTypeName.AppearanceHeader.Options.UseTextOptions = true;
            this.colWarrantyTypeName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyTypeName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyTypeName.Caption = "Kiểu BH";
            this.colWarrantyTypeName.FieldName = "WarrantyTypeName";
            this.colWarrantyTypeName.Name = "colWarrantyTypeName";
            this.colWarrantyTypeName.OptionsColumn.AllowEdit = false;
            this.colWarrantyTypeName.OptionsColumn.ReadOnly = true;
            this.colWarrantyTypeName.Visible = true;
            this.colWarrantyTypeName.VisibleIndex = 2;
            this.colWarrantyTypeName.Width = 120;
            // 
            // colWarrantyStatusName
            // 
            this.colWarrantyStatusName.AppearanceCell.Options.UseTextOptions = true;
            this.colWarrantyStatusName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyStatusName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyStatusName.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colWarrantyStatusName.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colWarrantyStatusName.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colWarrantyStatusName.AppearanceHeader.Options.UseBackColor = true;
            this.colWarrantyStatusName.AppearanceHeader.Options.UseFont = true;
            this.colWarrantyStatusName.AppearanceHeader.Options.UseForeColor = true;
            this.colWarrantyStatusName.AppearanceHeader.Options.UseTextOptions = true;
            this.colWarrantyStatusName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyStatusName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyStatusName.Caption = "Trạng thái";
            this.colWarrantyStatusName.FieldName = "WarrantyStatusText";
            this.colWarrantyStatusName.Name = "colWarrantyStatusName";
            this.colWarrantyStatusName.OptionsColumn.AllowEdit = false;
            this.colWarrantyStatusName.OptionsColumn.ReadOnly = true;
            this.colWarrantyStatusName.Visible = true;
            this.colWarrantyStatusName.VisibleIndex = 3;
            this.colWarrantyStatusName.Width = 120;
            // 
            // colWarrantyFrom
            // 
            this.colWarrantyFrom.AppearanceCell.Options.UseTextOptions = true;
            this.colWarrantyFrom.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyFrom.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyFrom.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colWarrantyFrom.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colWarrantyFrom.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colWarrantyFrom.AppearanceHeader.Options.UseBackColor = true;
            this.colWarrantyFrom.AppearanceHeader.Options.UseFont = true;
            this.colWarrantyFrom.AppearanceHeader.Options.UseForeColor = true;
            this.colWarrantyFrom.AppearanceHeader.Options.UseTextOptions = true;
            this.colWarrantyFrom.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyFrom.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyFrom.Caption = "Từ ngày";
            this.colWarrantyFrom.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.colWarrantyFrom.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colWarrantyFrom.FieldName = "WarrantyFrom";
            this.colWarrantyFrom.Name = "colWarrantyFrom";
            this.colWarrantyFrom.Visible = true;
            this.colWarrantyFrom.VisibleIndex = 4;
            this.colWarrantyFrom.Width = 120;
            // 
            // colMonthOfWarranty
            // 
            this.colMonthOfWarranty.AppearanceCell.Options.UseTextOptions = true;
            this.colMonthOfWarranty.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colMonthOfWarranty.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colMonthOfWarranty.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colMonthOfWarranty.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colMonthOfWarranty.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colMonthOfWarranty.AppearanceHeader.Options.UseBackColor = true;
            this.colMonthOfWarranty.AppearanceHeader.Options.UseFont = true;
            this.colMonthOfWarranty.AppearanceHeader.Options.UseForeColor = true;
            this.colMonthOfWarranty.AppearanceHeader.Options.UseTextOptions = true;
            this.colMonthOfWarranty.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colMonthOfWarranty.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colMonthOfWarranty.Caption = "Số tháng BH";
            this.colMonthOfWarranty.DisplayFormat.FormatString = "N0";
            this.colMonthOfWarranty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colMonthOfWarranty.FieldName = "MonthOfWarranty";
            this.colMonthOfWarranty.Name = "colMonthOfWarranty";
            this.colMonthOfWarranty.Visible = true;
            this.colMonthOfWarranty.VisibleIndex = 5;
            this.colMonthOfWarranty.Width = 100;
            // 
            // colWarrantyUntil
            // 
            this.colWarrantyUntil.AppearanceCell.Options.UseTextOptions = true;
            this.colWarrantyUntil.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyUntil.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyUntil.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colWarrantyUntil.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colWarrantyUntil.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colWarrantyUntil.AppearanceHeader.Options.UseBackColor = true;
            this.colWarrantyUntil.AppearanceHeader.Options.UseFont = true;
            this.colWarrantyUntil.AppearanceHeader.Options.UseForeColor = true;
            this.colWarrantyUntil.AppearanceHeader.Options.UseTextOptions = true;
            this.colWarrantyUntil.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyUntil.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyUntil.Caption = "Đến ngày";
            this.colWarrantyUntil.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.colWarrantyUntil.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colWarrantyUntil.FieldName = "WarrantyUntil";
            this.colWarrantyUntil.Name = "colWarrantyUntil";
            this.colWarrantyUntil.OptionsColumn.AllowEdit = false;
            this.colWarrantyUntil.OptionsColumn.ReadOnly = true;
            this.colWarrantyUntil.Visible = true;
            this.colWarrantyUntil.VisibleIndex = 6;
            this.colWarrantyUntil.Width = 120;
            // 
            // colUniqueProductInfoHtml
            // 
            this.colUniqueProductInfoHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colUniqueProductInfoHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colUniqueProductInfoHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colUniqueProductInfoHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colUniqueProductInfoHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colUniqueProductInfoHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colUniqueProductInfoHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colUniqueProductInfoHtml.AppearanceHeader.Options.UseFont = true;
            this.colUniqueProductInfoHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colUniqueProductInfoHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colUniqueProductInfoHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colUniqueProductInfoHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colUniqueProductInfoHtml.Caption = "Thông tin sản phẩm dịch vụ";
            this.colUniqueProductInfoHtml.ColumnEdit = this.HtmlRepositoryItemHypertextLabel;
            this.colUniqueProductInfoHtml.FieldName = "UniqueProductInfoHtml";
            this.colUniqueProductInfoHtml.Name = "colUniqueProductInfoHtml";
            this.colUniqueProductInfoHtml.OptionsColumn.AllowEdit = false;
            this.colUniqueProductInfoHtml.OptionsColumn.ReadOnly = true;
            this.colUniqueProductInfoHtml.Visible = true;
            this.colUniqueProductInfoHtml.VisibleIndex = 1;
            this.colUniqueProductInfoHtml.Width = 300;
            // 
            // HtmlRepositoryItemHypertextLabel
            // 
            this.HtmlRepositoryItemHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.HtmlRepositoryItemHypertextLabel.Name = "HtmlRepositoryItemHypertextLabel";
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2,
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.TuNgayBarEditItem,
            this.DenNgayBarEditItem,
            this.XemBaoCaoBarButtonItem,
            this.barHeaderItem1,
            this.DataSummaryBarStaticItem,
            this.SelectedRowBarStaticItem,
            this.XoaPhieuBarButtonItem,
            this.KeyWordBarEditItem,
            this.XuatFileBarButtonItem});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 17;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemDateEdit1,
            this.repositoryItemDateEdit2,
            this.repositoryItemTextEdit1});
            this.barManager1.StatusBar = this.bar1;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(((DevExpress.XtraBars.BarLinkUserDefines)((DevExpress.XtraBars.BarLinkUserDefines.PaintStyle | DevExpress.XtraBars.BarLinkUserDefines.Width))), this.KeyWordBarEditItem, "", false, true, true, 147, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(((DevExpress.XtraBars.BarLinkUserDefines)((DevExpress.XtraBars.BarLinkUserDefines.PaintStyle | DevExpress.XtraBars.BarLinkUserDefines.Width))), this.TuNgayBarEditItem, "", false, true, true, 117, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(((DevExpress.XtraBars.BarLinkUserDefines)((DevExpress.XtraBars.BarLinkUserDefines.PaintStyle | DevExpress.XtraBars.BarLinkUserDefines.Width))), this.DenNgayBarEditItem, "", false, true, true, 125, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.XemBaoCaoBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.XoaPhieuBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.XuatFileBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // KeyWordBarEditItem
            // 
            this.KeyWordBarEditItem.Caption = "Từ khóa";
            this.KeyWordBarEditItem.Edit = this.repositoryItemTextEdit1;
            this.KeyWordBarEditItem.Id = 15;
            this.KeyWordBarEditItem.Name = "KeyWordBarEditItem";
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // TuNgayBarEditItem
            // 
            this.TuNgayBarEditItem.Caption = "Từ ngày";
            this.TuNgayBarEditItem.Edit = this.repositoryItemDateEdit1;
            this.TuNgayBarEditItem.Id = 7;
            this.TuNgayBarEditItem.Name = "TuNgayBarEditItem";
            // 
            // repositoryItemDateEdit1
            // 
            this.repositoryItemDateEdit1.AutoHeight = false;
            this.repositoryItemDateEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
            // 
            // DenNgayBarEditItem
            // 
            this.DenNgayBarEditItem.Caption = "Đến ngày";
            this.DenNgayBarEditItem.Edit = this.repositoryItemDateEdit2;
            this.DenNgayBarEditItem.Id = 8;
            this.DenNgayBarEditItem.Name = "DenNgayBarEditItem";
            // 
            // repositoryItemDateEdit2
            // 
            this.repositoryItemDateEdit2.AutoHeight = false;
            this.repositoryItemDateEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit2.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit2.Name = "repositoryItemDateEdit2";
            // 
            // XemBaoCaoBarButtonItem
            // 
            this.XemBaoCaoBarButtonItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.XemBaoCaoBarButtonItem.Caption = "Xem";
            this.XemBaoCaoBarButtonItem.Id = 9;
            this.XemBaoCaoBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.filterbyseries_pie_16x16;
            this.XemBaoCaoBarButtonItem.ImageOptions.LargeImage = global::Inventory.Properties.Resources.filterbyseries_pie_32x32;
            this.XemBaoCaoBarButtonItem.Name = "XemBaoCaoBarButtonItem";
            // 
            // XoaPhieuBarButtonItem
            // 
            this.XoaPhieuBarButtonItem.Caption = "Xóa";
            this.XoaPhieuBarButtonItem.Id = 14;
            this.XoaPhieuBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.clear_16x16;
            this.XoaPhieuBarButtonItem.Name = "XoaPhieuBarButtonItem";
            // 
            // XuatFileBarButtonItem
            // 
            this.XuatFileBarButtonItem.Caption = "Xuất file";
            this.XuatFileBarButtonItem.Id = 16;
            this.XuatFileBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.exporttoxps_16x16;
            this.XuatFileBarButtonItem.ImageOptions.LargeImage = global::Inventory.Properties.Resources.exporttoxps_32x32;
            this.XuatFileBarButtonItem.Name = "XuatFileBarButtonItem";
            // 
            // bar1
            // 
            this.bar1.BarName = "Custom 3";
            this.bar1.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barHeaderItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.DataSummaryBarStaticItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.SelectedRowBarStaticItem)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Custom 3";
            // 
            // barHeaderItem1
            // 
            this.barHeaderItem1.Caption = "Tổng kết";
            this.barHeaderItem1.Id = 10;
            this.barHeaderItem1.Name = "barHeaderItem1";
            // 
            // DataSummaryBarStaticItem
            // 
            this.DataSummaryBarStaticItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.DataSummaryBarStaticItem.Caption = "Chưa có dữ liệu";
            this.DataSummaryBarStaticItem.Id = 11;
            this.DataSummaryBarStaticItem.Name = "DataSummaryBarStaticItem";
            // 
            // SelectedRowBarStaticItem
            // 
            this.SelectedRowBarStaticItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.SelectedRowBarStaticItem.Caption = "Chưa chọn dòng nào";
            this.SelectedRowBarStaticItem.Id = 12;
            this.SelectedRowBarStaticItem.Name = "SelectedRowBarStaticItem";
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
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 770);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1371, 22);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 746);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1371, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 746);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1371, 746);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.WarrantyCheckListDtoGridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1351, 726);
            this.layoutControlItem1.TextVisible = false;
            // 
            // FrmWarrantyCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1371, 792);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FrmWarrantyCheck";
            this.Text = "KIỂM TRA BẢO HÀNH";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyCheckListDtoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.warrantyCheckListDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyCheckListDtoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlRepositoryItemHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.GridControl WarrantyCheckListDtoGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView WarrantyCheckListDtoGridView;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraBars.BarEditItem TuNgayBarEditItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraBars.BarEditItem DenNgayBarEditItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit2;
        private DevExpress.XtraBars.BarButtonItem XemBaoCaoBarButtonItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel HtmlRepositoryItemHypertextLabel;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarHeaderItem barHeaderItem1;
        private DevExpress.XtraBars.BarStaticItem DataSummaryBarStaticItem;
        private DevExpress.XtraBars.BarStaticItem SelectedRowBarStaticItem;
        private DevExpress.XtraBars.BarButtonItem XoaPhieuBarButtonItem;
        private DevExpress.XtraBars.BarEditItem KeyWordBarEditItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn colWarrantyTypeName;
        private DevExpress.XtraGrid.Columns.GridColumn colWarrantyStatusName;
        private DevExpress.XtraGrid.Columns.GridColumn colWarrantyFrom;
        private DevExpress.XtraGrid.Columns.GridColumn colMonthOfWarranty;
        private DevExpress.XtraGrid.Columns.GridColumn colWarrantyUntil;
        
        private System.Windows.Forms.BindingSource warrantyCheckListDtoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colUniqueProductInfoHtml;
        private DevExpress.XtraBars.BarButtonItem XuatFileBarButtonItem;
    }
}