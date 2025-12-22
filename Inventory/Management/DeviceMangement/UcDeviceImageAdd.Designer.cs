using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;

namespace Inventory.Management.DeviceMangement
{
    partial class UcDeviceImageAdd
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
            this.deviceDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.LogTextBox = new System.Windows.Forms.TextBox();
            this.OpenSelectImageHyperlinkLabelControl = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.xtraOpenFileDialog1 = new DevExpress.XtraEditors.XtraOpenFileDialog(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deviceDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.LogTextBox);
            this.layoutControl1.Controls.Add(this.OpenSelectImageHyperlinkLabelControl);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(594, 278);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // deviceDtoBindingSource
            // 
            this.deviceDtoBindingSource.DataSource = typeof(DTO.Inventory.InventoryManagement.DeviceDto);
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
            this.OpenSelectImageHyperlinkLabelControl.ImageOptions.Image = global::Inventory.Properties.Resources.opendoc_16x16;
            this.OpenSelectImageHyperlinkLabelControl.Location = new System.Drawing.Point(242, 12);
            this.OpenSelectImageHyperlinkLabelControl.Name = "OpenSelectImageHyperlinkLabelControl";
            this.OpenSelectImageHyperlinkLabelControl.Padding = new System.Windows.Forms.Padding(10);
            this.OpenSelectImageHyperlinkLabelControl.Size = new System.Drawing.Size(110, 40);
            this.OpenSelectImageHyperlinkLabelControl.StyleController = this.layoutControl1;
            this.OpenSelectImageHyperlinkLabelControl.TabIndex = 3;
            this.OpenSelectImageHyperlinkLabelControl.Text = "Chọn hình ảnh";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.layoutControlItem3});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(594, 278);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem2.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem2.Control = this.OpenSelectImageHyperlinkLabelControl;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(574, 44);
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
            // UcDeviceImageAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Name = "UcDeviceImageAdd";
            this.Size = new System.Drawing.Size(594, 278);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.deviceDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private LayoutControl layoutControl1;
        private LayoutControlGroup Root;
        private BindingSource deviceDtoBindingSource;
        private HyperlinkLabelControl OpenSelectImageHyperlinkLabelControl;
        private LayoutControlItem layoutControlItem2;
        private XtraOpenFileDialog xtraOpenFileDialog1;
        private TextBox LogTextBox;
        private LayoutControlItem layoutControlItem3;
    }
}