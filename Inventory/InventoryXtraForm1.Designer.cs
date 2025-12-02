using DTO.Inventory.StockIn.NhapHangThuongMai;

namespace Inventory
{
    partial class InventoryXtraForm1
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
            this.xtraOpenFileDialog1 = new DevExpress.XtraEditors.XtraOpenFileDialog(this.components);
            this.stockInReportDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ucXuatHangThuongMaiMasterDto1 = new Inventory.StockOut.XuatHangThuongMai.UcXuatHangThuongMaiMasterDto();
            ((System.ComponentModel.ISupportInitialize)(this.stockInReportDtoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraOpenFileDialog1
            // 
            this.xtraOpenFileDialog1.FileName = "xtraOpenFileDialog1";
            // 
            // stockInReportDtoBindingSource
            // 
            this.stockInReportDtoBindingSource.DataSource = typeof(DTO.Inventory.StockIn.NhapHangThuongMai.StockInReportDto);
            // 
            // ucXuatHangThuongMaiMasterDto1
            // 
            this.ucXuatHangThuongMaiMasterDto1.Location = new System.Drawing.Point(313, 237);
            this.ucXuatHangThuongMaiMasterDto1.Name = "ucXuatHangThuongMaiMasterDto1";
            this.ucXuatHangThuongMaiMasterDto1.Size = new System.Drawing.Size(486, 623);
            this.ucXuatHangThuongMaiMasterDto1.TabIndex = 0;
            // 
            // InventoryXtraForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1219, 902);
            this.Controls.Add(this.ucXuatHangThuongMaiMasterDto1);
            this.Name = "InventoryXtraForm1";
            this.Text = "XtraForm1";
            ((System.ComponentModel.ISupportInitialize)(this.stockInReportDtoBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.XtraOpenFileDialog xtraOpenFileDialog1;
        private System.Windows.Forms.BindingSource stockInReportDtoBindingSource;
        private StockOut.XuatHangThuongMai.UcXuatHangThuongMaiMasterDto ucXuatHangThuongMaiMasterDto1;
    }
}