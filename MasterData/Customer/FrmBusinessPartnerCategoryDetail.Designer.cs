using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraLayout;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using DTO.MasterData.CustomerPartner;

namespace MasterData.Customer
{
    partial class FrmBusinessPartnerCategoryDetail
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
            this.CloseBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.CategoryCodeTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.CategoryNameTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ParentCategorySearchLookUpEdit = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.businessPartnerCategoryDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.parentCategoryGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colParentFullPathHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.parentCategoryFullPathHtmlRepositoryItemHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.DescriptionMemoEdit = new DevExpress.XtraEditors.MemoEdit();
            this.IsActiveToogleSwitch = new DevExpress.XtraEditors.ToggleSwitch();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForCategoryCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForCategoryName = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForParentId = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForDescription = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForIsActive = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CategoryCodeTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CategoryNameTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParentCategorySearchLookUpEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.businessPartnerCategoryDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.parentCategoryGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.parentCategoryFullPathHtmlRepositoryItemHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionMemoEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsActiveToogleSwitch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCategoryCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCategoryName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForParentId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsActive)).BeginInit();
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
            this.barManager1.MaxItemId = 2;
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
            this.SaveBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.save_16x16;
            this.SaveBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.save_32x32;
            this.SaveBarButtonItem.Name = "SaveBarButtonItem";
            this.SaveBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.SaveBarButtonItem_ItemClick);
            // 
            // CloseBarButtonItem
            // 
            this.CloseBarButtonItem.Caption = "Đóng";
            this.CloseBarButtonItem.Id = 1;
            this.CloseBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.cancel_16x16;
            this.CloseBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.cancel_32x32;
            this.CloseBarButtonItem.Name = "CloseBarButtonItem";
            this.CloseBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.CancelBarButtonItem_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(600, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 299);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(600, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 275);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(600, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 275);
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
            // 
            // dataLayoutControl1
            // 
            this.dataLayoutControl1.Controls.Add(this.CategoryCodeTextEdit);
            this.dataLayoutControl1.Controls.Add(this.CategoryNameTextEdit);
            this.dataLayoutControl1.Controls.Add(this.ParentCategorySearchLookUpEdit);
            this.dataLayoutControl1.Controls.Add(this.DescriptionMemoEdit);
            this.dataLayoutControl1.Controls.Add(this.IsActiveToogleSwitch);
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 24);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(600, 275);
            this.dataLayoutControl1.TabIndex = 5;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // CategoryCodeTextEdit
            // 
            this.CategoryCodeTextEdit.Location = new System.Drawing.Point(91, 12);
            this.CategoryCodeTextEdit.MenuManager = this.barManager1;
            this.CategoryCodeTextEdit.Name = "CategoryCodeTextEdit";
            this.CategoryCodeTextEdit.Properties.MaxLength = 50;
            this.CategoryCodeTextEdit.Size = new System.Drawing.Size(497, 20);
            this.CategoryCodeTextEdit.StyleController = this.dataLayoutControl1;
            this.CategoryCodeTextEdit.TabIndex = 0;
            // 
            // CategoryNameTextEdit
            // 
            this.CategoryNameTextEdit.Location = new System.Drawing.Point(91, 36);
            this.CategoryNameTextEdit.MenuManager = this.barManager1;
            this.CategoryNameTextEdit.Name = "CategoryNameTextEdit";
            this.CategoryNameTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.CategoryNameTextEdit.Properties.MaxLength = 100;
            this.CategoryNameTextEdit.Size = new System.Drawing.Size(497, 20);
            this.CategoryNameTextEdit.StyleController = this.dataLayoutControl1;
            this.CategoryNameTextEdit.TabIndex = 1;
            // 
            // ParentCategorySearchLookUpEdit
            // 
            this.ParentCategorySearchLookUpEdit.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.True;
            this.ParentCategorySearchLookUpEdit.Location = new System.Drawing.Point(91, 60);
            this.ParentCategorySearchLookUpEdit.MenuManager = this.barManager1;
            this.ParentCategorySearchLookUpEdit.Name = "ParentCategorySearchLookUpEdit";
            this.ParentCategorySearchLookUpEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.ParentCategorySearchLookUpEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.ParentCategorySearchLookUpEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Clear)});
            this.ParentCategorySearchLookUpEdit.Properties.DataSource = this.businessPartnerCategoryDtoBindingSource;
            this.ParentCategorySearchLookUpEdit.Properties.DisplayMember = "FullPathHtml";
            this.ParentCategorySearchLookUpEdit.Properties.NullText = "Chọn danh mục cha (tùy chọn)";
            this.ParentCategorySearchLookUpEdit.Properties.PopupView = this.parentCategoryGridView;
            this.ParentCategorySearchLookUpEdit.Properties.ValueMember = "Id";
            this.ParentCategorySearchLookUpEdit.Size = new System.Drawing.Size(497, 20);
            this.ParentCategorySearchLookUpEdit.StyleController = this.dataLayoutControl1;
            this.ParentCategorySearchLookUpEdit.TabIndex = 2;
            // 
            // businessPartnerCategoryDtoBindingSource
            // 
            this.businessPartnerCategoryDtoBindingSource.DataSource = typeof(DTO.MasterData.CustomerPartner.BusinessPartnerCategoryDto);
            // 
            // parentCategoryGridView
            // 
            this.parentCategoryGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colParentFullPathHtml});
            this.parentCategoryGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.parentCategoryGridView.Name = "parentCategoryGridView";
            this.parentCategoryGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.parentCategoryGridView.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.parentCategoryGridView.OptionsView.RowAutoHeight = true;
            this.parentCategoryGridView.OptionsView.ShowGroupPanel = false;
            this.parentCategoryGridView.OptionsView.ShowIndicator = false;
            // 
            // colParentFullPathHtml
            // 
            this.colParentFullPathHtml.Caption = "Đường dẫn";
            this.colParentFullPathHtml.ColumnEdit = this.parentCategoryFullPathHtmlRepositoryItemHypertextLabel;
            this.colParentFullPathHtml.FieldName = "FullPathHtml";
            this.colParentFullPathHtml.Name = "colParentFullPathHtml";
            this.colParentFullPathHtml.Visible = true;
            this.colParentFullPathHtml.VisibleIndex = 0;
            this.colParentFullPathHtml.Width = 350;
            // 
            // parentCategoryFullPathHtmlRepositoryItemHypertextLabel
            // 
            this.parentCategoryFullPathHtmlRepositoryItemHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.parentCategoryFullPathHtmlRepositoryItemHypertextLabel.Name = "parentCategoryFullPathHtmlRepositoryItemHypertextLabel";
            // 
            // DescriptionMemoEdit
            // 
            this.DescriptionMemoEdit.Location = new System.Drawing.Point(91, 84);
            this.DescriptionMemoEdit.MenuManager = this.barManager1;
            this.DescriptionMemoEdit.Name = "DescriptionMemoEdit";
            this.DescriptionMemoEdit.Properties.MaxLength = 255;
            this.DescriptionMemoEdit.Size = new System.Drawing.Size(497, 157);
            this.DescriptionMemoEdit.StyleController = this.dataLayoutControl1;
            this.DescriptionMemoEdit.TabIndex = 3;
            // 
            // IsActiveToogleSwitch
            // 
            this.IsActiveToogleSwitch.EditValue = true;
            this.IsActiveToogleSwitch.Location = new System.Drawing.Point(91, 245);
            this.IsActiveToogleSwitch.MenuManager = this.barManager1;
            this.IsActiveToogleSwitch.Name = "IsActiveToogleSwitch";
            this.IsActiveToogleSwitch.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.IsActiveToogleSwitch.Properties.OffText = "<color=\'red\'>Không hoạt động</color>";
            this.IsActiveToogleSwitch.Properties.OnText = "<color=\'blue\'>Đang hoạt động</color>";
            this.IsActiveToogleSwitch.Size = new System.Drawing.Size(497, 18);
            this.IsActiveToogleSwitch.StyleController = this.dataLayoutControl1;
            this.IsActiveToogleSwitch.TabIndex = 4;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(600, 275);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AllowDrawBackground = false;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForCategoryCode,
            this.ItemForCategoryName,
            this.ItemForParentId,
            this.ItemForDescription,
            this.ItemForIsActive});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(580, 255);
            this.layoutControlGroup1.Text = "Thông tin danh mục";
            // 
            // ItemForCategoryCode
            // 
            this.ItemForCategoryCode.Control = this.CategoryCodeTextEdit;
            this.ItemForCategoryCode.Location = new System.Drawing.Point(0, 0);
            this.ItemForCategoryCode.Name = "ItemForCategoryCode";
            this.ItemForCategoryCode.Size = new System.Drawing.Size(580, 24);
            this.ItemForCategoryCode.Text = "Mã danh mục";
            this.ItemForCategoryCode.TextSize = new System.Drawing.Size(67, 13);
            // 
            // ItemForCategoryName
            // 
            this.ItemForCategoryName.Control = this.CategoryNameTextEdit;
            this.ItemForCategoryName.Location = new System.Drawing.Point(0, 24);
            this.ItemForCategoryName.Name = "ItemForCategoryName";
            this.ItemForCategoryName.Size = new System.Drawing.Size(580, 24);
            this.ItemForCategoryName.Text = "Tên phân loại";
            this.ItemForCategoryName.TextSize = new System.Drawing.Size(67, 13);
            // 
            // ItemForParentId
            // 
            this.ItemForParentId.Control = this.ParentCategorySearchLookUpEdit;
            this.ItemForParentId.Location = new System.Drawing.Point(0, 48);
            this.ItemForParentId.Name = "ItemForParentId";
            this.ItemForParentId.Size = new System.Drawing.Size(580, 24);
            this.ItemForParentId.Text = "Danh mục cha";
            this.ItemForParentId.TextSize = new System.Drawing.Size(67, 13);
            // 
            // ItemForDescription
            // 
            this.ItemForDescription.Control = this.DescriptionMemoEdit;
            this.ItemForDescription.Location = new System.Drawing.Point(0, 72);
            this.ItemForDescription.Name = "ItemForDescription";
            this.ItemForDescription.Size = new System.Drawing.Size(580, 161);
            this.ItemForDescription.StartNewLine = true;
            this.ItemForDescription.Text = "Mô tả";
            this.ItemForDescription.TextSize = new System.Drawing.Size(67, 13);
            // 
            // ItemForIsActive
            // 
            this.ItemForIsActive.Control = this.IsActiveToogleSwitch;
            this.ItemForIsActive.Location = new System.Drawing.Point(0, 233);
            this.ItemForIsActive.Name = "ItemForIsActive";
            this.ItemForIsActive.Size = new System.Drawing.Size(580, 22);
            this.ItemForIsActive.Text = "Trạng thái";
            this.ItemForIsActive.TextSize = new System.Drawing.Size(67, 13);
            // 
            // FrmBusinessPartnerCategoryDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 299);
            this.Controls.Add(this.dataLayoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmBusinessPartnerCategoryDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CategoryCodeTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CategoryNameTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParentCategorySearchLookUpEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.businessPartnerCategoryDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.parentCategoryGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.parentCategoryFullPathHtmlRepositoryItemHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionMemoEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsActiveToogleSwitch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCategoryCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCategoryName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForParentId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsActive)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Bars
        private BarManager barManager1;
        private Bar bar2;
        private BarButtonItem SaveBarButtonItem;
        private BarButtonItem CloseBarButtonItem;
        private BarDockControl barDockControlTop;
        private BarDockControl barDockControlBottom;
        private BarDockControl barDockControlLeft;
        private BarDockControl barDockControlRight;
        #endregion

        #region Validation
        private DXErrorProvider dxErrorProvider1;
        #endregion

        #region Layout Root
        private DataLayoutControl dataLayoutControl1;
        private LayoutControlGroup Root;
        #endregion

        #region Controls
        private BindingSource businessPartnerCategoryDtoBindingSource;
        private TextEdit CategoryCodeTextEdit;
        private TextEdit CategoryNameTextEdit;
        private SearchLookUpEdit ParentCategorySearchLookUpEdit;
        private GridView parentCategoryGridView;
        private GridColumn colParentFullPathHtml;
        private RepositoryItemHypertextLabel parentCategoryFullPathHtmlRepositoryItemHypertextLabel;
        private MemoEdit DescriptionMemoEdit;
        #endregion

        #region Layout Items
        private LayoutControlGroup layoutControlGroup1;
        private LayoutControlItem ItemForCategoryCode;
        private LayoutControlItem ItemForCategoryName;
        private LayoutControlItem ItemForParentId;
        private LayoutControlItem ItemForDescription;
        private LayoutControlItem ItemForIsActive;
        #endregion

        private ToggleSwitch IsActiveToogleSwitch;
    }
}