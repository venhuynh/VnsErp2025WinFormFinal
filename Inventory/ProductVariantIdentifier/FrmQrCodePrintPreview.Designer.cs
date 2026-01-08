namespace Inventory.ProductVariantIdentifier
{
    partial class FrmQrCodePrintPreview
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
            this.LogTextBox = new System.Windows.Forms.TextBox();
            this.PrinterComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.PrintWidthTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.PrintHeightTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.PrintQrInforSimpleLabelItem = new DevExpress.XtraLayout.SimpleLabelItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.PrintHyperlinkLabelControl = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PrinterComboBoxEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PrintWidthTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PrintHeightTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PrintQrInforSimpleLabelItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.PrintHyperlinkLabelControl);
            this.layoutControl1.Controls.Add(this.LogTextBox);
            this.layoutControl1.Controls.Add(this.PrinterComboBoxEdit);
            this.layoutControl1.Controls.Add(this.PrintWidthTextEdit);
            this.layoutControl1.Controls.Add(this.PrintHeightTextEdit);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(580, 496);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // LogTextBox
            // 
            this.LogTextBox.Location = new System.Drawing.Point(12, 182);
            this.LogTextBox.Multiline = true;
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.LogTextBox.Size = new System.Drawing.Size(556, 302);
            this.LogTextBox.TabIndex = 7;
            // 
            // PrinterComboBoxEdit
            // 
            this.PrinterComboBoxEdit.Location = new System.Drawing.Point(76, 65);
            this.PrinterComboBoxEdit.Name = "PrinterComboBoxEdit";
            this.PrinterComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.PrinterComboBoxEdit.Size = new System.Drawing.Size(416, 20);
            this.PrinterComboBoxEdit.StyleController = this.layoutControl1;
            this.PrinterComboBoxEdit.TabIndex = 4;
            // 
            // PrintWidthTextEdit
            // 
            this.PrintWidthTextEdit.EditValue = "50";
            this.PrintWidthTextEdit.Location = new System.Drawing.Point(196, 122);
            this.PrintWidthTextEdit.Name = "PrintWidthTextEdit";
            this.PrintWidthTextEdit.Size = new System.Drawing.Size(360, 20);
            this.PrintWidthTextEdit.StyleController = this.layoutControl1;
            this.PrintWidthTextEdit.TabIndex = 5;
            // 
            // PrintHeightTextEdit
            // 
            this.PrintHeightTextEdit.EditValue = "50";
            this.PrintHeightTextEdit.Location = new System.Drawing.Point(196, 146);
            this.PrintHeightTextEdit.Name = "PrintHeightTextEdit";
            this.PrintHeightTextEdit.Size = new System.Drawing.Size(360, 20);
            this.PrintHeightTextEdit.StyleController = this.layoutControl1;
            this.PrintHeightTextEdit.TabIndex = 6;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlGroup1,
            this.PrintQrInforSimpleLabelItem,
            this.layoutControlItem4,
            this.layoutControlItem5});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(580, 496);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.PrinterComboBoxEdit;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 53);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(484, 24);
            this.layoutControlItem1.Text = "Chọn máy in";
            this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(59, 13);
            this.layoutControlItem1.TextToControlDistance = 5;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.layoutControlItem3});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 77);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(560, 93);
            this.layoutControlGroup1.Text = "Khổ giấy";
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.PrintWidthTextEdit;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(536, 24);
            this.layoutControlItem2.Text = "Rộng (mm)";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(160, 13);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.PrintHeightTextEdit;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(536, 24);
            this.layoutControlItem3.Text = "Cao (mm)";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(160, 13);
            // 
            // PrintQrInforSimpleLabelItem
            // 
            this.PrintQrInforSimpleLabelItem.AllowHtmlStringInCaption = true;
            this.PrintQrInforSimpleLabelItem.AppearanceItemCaption.Options.UseTextOptions = true;
            this.PrintQrInforSimpleLabelItem.AppearanceItemCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.PrintQrInforSimpleLabelItem.Location = new System.Drawing.Point(0, 0);
            this.PrintQrInforSimpleLabelItem.Name = "PrintQrInforSimpleLabelItem";
            this.PrintQrInforSimpleLabelItem.Padding = new DevExpress.XtraLayout.Utils.Padding(20, 20, 20, 20);
            this.PrintQrInforSimpleLabelItem.Size = new System.Drawing.Size(560, 53);
            this.PrintQrInforSimpleLabelItem.TextSize = new System.Drawing.Size(160, 13);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.LogTextBox;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 170);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(560, 306);
            this.layoutControlItem4.TextVisible = false;
            // 
            // PrintHyperlinkLabelControl
            // 
            this.PrintHyperlinkLabelControl.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.PrintHyperlinkLabelControl.ImageOptions.Image = global::Inventory.Properties.Resources.print_16x16;
            this.PrintHyperlinkLabelControl.Location = new System.Drawing.Point(496, 65);
            this.PrintHyperlinkLabelControl.Name = "PrintHyperlinkLabelControl";
            this.PrintHyperlinkLabelControl.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.PrintHyperlinkLabelControl.Size = new System.Drawing.Size(72, 20);
            this.PrintHyperlinkLabelControl.StyleController = this.layoutControl1;
            this.PrintHyperlinkLabelControl.TabIndex = 8;
            this.PrintHyperlinkLabelControl.Text = "In tem";
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem5.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem5.Control = this.PrintHyperlinkLabelControl;
            this.layoutControlItem5.Location = new System.Drawing.Point(484, 53);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(76, 24);
            this.layoutControlItem5.TextVisible = false;
            // 
            // FrmQrCodePrintPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 496);
            this.Controls.Add(this.layoutControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmQrCodePrintPreview";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "In tem QR Code";
            this.Load += new System.EventHandler(this.FrmQrCodePrintPreview_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PrinterComboBoxEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PrintWidthTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PrintHeightTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PrintQrInforSimpleLabelItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraEditors.ComboBoxEdit PrinterComboBoxEdit;
        private DevExpress.XtraEditors.TextEdit PrintWidthTextEdit;
        private DevExpress.XtraEditors.TextEdit PrintHeightTextEdit;
        private DevExpress.XtraLayout.SimpleLabelItem PrintQrInforSimpleLabelItem;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private System.Windows.Forms.TextBox LogTextBox;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.HyperlinkLabelControl PrintHyperlinkLabelControl;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
    }
}