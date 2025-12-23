using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DTO.MasterData.ProductService;

namespace MasterData.ProductService
{
    partial class FrmProductImageAdd
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel repositoryItemHypertextLabel1 = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.LogTextBox = new System.Windows.Forms.TextBox();
            this.OpenSelectImageHyperlinkLabelControl = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.ProductServiceSearchLookupEdit = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colThongTinHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.xtraOpenFileDialog1 = new DevExpress.XtraEditors.XtraOpenFileDialog();
            this.productServiceDtoBindingSource = new System.Windows.Forms.BindingSource();
            this.HtmlContentHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceSearchLookupEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(repositoryItemHypertextLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productServiceDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlContentHypertextLabel)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.LogTextBox);
            this.layoutControl1.Controls.Add(this.OpenSelectImageHyperlinkLabelControl);
            this.layoutControl1.Controls.Add(this.ProductServiceSearchLookupEdit);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(594, 278);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // LogTextBox
            // 
            this.LogTextBox.Location = new System.Drawing.Point(12, 56);
            this.LogTextBox.Multiline = true;
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LogTextBox.Size = new System.Drawing.Size(570, 210);
            this.LogTextBox.TabIndex = 4;
            // 
            // OpenSelectImageHyperlinkLabelControl
            // 
            this.OpenSelectImageHyperlinkLabelControl.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.OpenSelectImageHyperlinkLabelControl.ImageOptions.Image = global::MasterData.Properties.Resources.loadfrom_16x16;
            this.OpenSelectImageHyperlinkLabelControl.Location = new System.Drawing.Point(472, 12);
            this.OpenSelectImageHyperlinkLabelControl.Name = "OpenSelectImageHyperlinkLabelControl";
            this.OpenSelectImageHyperlinkLabelControl.Padding = new System.Windows.Forms.Padding(10);
            this.OpenSelectImageHyperlinkLabelControl.Size = new System.Drawing.Size(110, 40);
            this.OpenSelectImageHyperlinkLabelControl.StyleController = this.layoutControl1;
            this.OpenSelectImageHyperlinkLabelControl.TabIndex = 3;
            this.OpenSelectImageHyperlinkLabelControl.Text = "Chọn hình ảnh";
            // 
            // ProductServiceSearchLookupEdit
            // 
            this.ProductServiceSearchLookupEdit.Location = new System.Drawing.Point(98, 22);
            this.ProductServiceSearchLookupEdit.Name = "ProductServiceSearchLookupEdit";
            this.ProductServiceSearchLookupEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.ProductServiceSearchLookupEdit.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.ProductServiceSearchLookupEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ProductServiceSearchLookupEdit.Properties.DataSource = this.productServiceDtoBindingSource;
            this.ProductServiceSearchLookupEdit.Properties.DisplayMember = "ThongTinHtml";
            this.ProductServiceSearchLookupEdit.Properties.PopupView = this.searchLookUpEdit1View;
            this.ProductServiceSearchLookupEdit.Properties.ValueMember = "Id";
            this.ProductServiceSearchLookupEdit.Size = new System.Drawing.Size(370, 20);
            this.ProductServiceSearchLookupEdit.StyleController = this.layoutControl1;
            this.ProductServiceSearchLookupEdit.TabIndex = 0;
            // 
            // searchLookUpEdit1View
            // 
            this.searchLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colThongTinHtml});
            this.searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            this.searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // colThongTinHtml
            // 
            this.colThongTinHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colThongTinHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colThongTinHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colThongTinHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colThongTinHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colThongTinHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colThongTinHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colThongTinHtml.AppearanceHeader.Options.UseFont = true;
            this.colThongTinHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colThongTinHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colThongTinHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colThongTinHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colThongTinHtml.Caption = "Thông tin sản phẩm";
            repositoryItemHypertextLabel1.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            repositoryItemHypertextLabel1.Name = "HtmlContentHypertextLabel";
            this.colThongTinHtml.ColumnEdit = repositoryItemHypertextLabel1;
            this.colThongTinHtml.FieldName = "ThongTinHtml";
            this.colThongTinHtml.MinWidth = 300;
            this.colThongTinHtml.Name = "colThongTinHtml";
            this.colThongTinHtml.Visible = true;
            this.colThongTinHtml.VisibleIndex = 0;
            this.colThongTinHtml.Width = 500;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(594, 278);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem1.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem1.Control = this.ProductServiceSearchLookupEdit;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(460, 44);
            this.layoutControlItem1.Text = "Chọn sản phẩm";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(74, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem2.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem2.Control = this.OpenSelectImageHyperlinkLabelControl;
            this.layoutControlItem2.Location = new System.Drawing.Point(460, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(114, 44);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.LogTextBox;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 44);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(574, 214);
            this.layoutControlItem3.TextVisible = false;
            // 
            // xtraOpenFileDialog1
            // 
            this.xtraOpenFileDialog1.FileName = "xtraOpenFileDialog1";
            // 
            // productServiceDtoBindingSource
            // 
            this.productServiceDtoBindingSource.DataSource = typeof(DTO.MasterData.ProductService.ProductServiceDto);
            // 
            // HtmlContentHypertextLabel
            // 
            this.HtmlContentHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.HtmlContentHypertextLabel.Name = "HtmlContentHypertextLabel";
            // 
            // FrmProductImageAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 278);
            this.Controls.Add(this.layoutControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmProductImageAdd";
            this.Text = "Thêm hình ảnh";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            this.layoutControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceSearchLookupEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(repositoryItemHypertextLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productServiceDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlContentHypertextLabel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private LayoutControl layoutControl1;
        private LayoutControlGroup Root;
        private SearchLookUpEdit ProductServiceSearchLookupEdit;
        private GridView searchLookUpEdit1View;
        private LayoutControlItem layoutControlItem1;
        private HyperlinkLabelControl OpenSelectImageHyperlinkLabelControl;
        private LayoutControlItem layoutControlItem2;
        private BindingSource productServiceDtoBindingSource;
        private XtraOpenFileDialog xtraOpenFileDialog1;
        private GridColumn colThongTinHtml;
        private TextBox LogTextBox;
        private LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel HtmlContentHypertextLabel;
    }
}