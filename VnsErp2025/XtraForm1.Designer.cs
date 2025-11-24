using Inventory.StockIn.NhapHangThuongMai;

namespace VnsErp2025
{
    partial class XtraForm1
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
            this.ucStockInMaster1 = new UcStockInMaster();
            this.SuspendLayout();
            // 
            // ucStockInMaster1
            // 
            this.ucStockInMaster1.Location = new System.Drawing.Point(139, 12);
            this.ucStockInMaster1.Name = "ucStockInMaster1";
            this.ucStockInMaster1.Size = new System.Drawing.Size(486, 623);
            this.ucStockInMaster1.TabIndex = 0;
            // 
            // XtraForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 654);
            this.Controls.Add(this.ucStockInMaster1);
            this.Name = "XtraForm1";
            this.Text = "XtraForm1";
            this.ResumeLayout(false);

        }

        #endregion

        private UcStockInMaster ucStockInMaster1;
    }
}