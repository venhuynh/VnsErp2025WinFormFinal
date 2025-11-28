using DTO.Inventory.StockIn.NhapHangThuongMai;

namespace Inventory
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
            this.components = new System.ComponentModel.Container();
            this.xtraOpenFileDialog1 = new DevExpress.XtraEditors.XtraOpenFileDialog(this.components);
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.stockInReportDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStockInNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStockInDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLoaiNhapXuatKho = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTrangThai = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWarehouseId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWarehouseCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWarehouseName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPurchaseOrderId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPurchaseOrderNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSupplierId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSupplierName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNotes = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNguoiNhanHang = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNguoiGiaoHang = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotalQuantity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotalAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotalVat = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotalAmountIncludedVat = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockInReportDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraOpenFileDialog1
            // 
            this.xtraOpenFileDialog1.FileName = "xtraOpenFileDialog1";
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.stockInReportDtoBindingSource;
            this.gridControl1.Location = new System.Drawing.Point(347, 231);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(400, 200);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // stockInReportDtoBindingSource
            // 
            this.stockInReportDtoBindingSource.DataSource = typeof(DTO.Inventory.StockIn.NhapHangThuongMai.StockInReportDto);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colId,
            this.colStockInNumber,
            this.colStockInDate,
            this.colLoaiNhapXuatKho,
            this.colTrangThai,
            this.colWarehouseId,
            this.colWarehouseCode,
            this.colWarehouseName,
            this.colPurchaseOrderId,
            this.colPurchaseOrderNumber,
            this.colSupplierId,
            this.colSupplierName,
            this.colNotes,
            this.colNguoiNhanHang,
            this.colNguoiGiaoHang,
            this.colTotalQuantity,
            this.colTotalAmount,
            this.colTotalVat,
            this.colTotalAmountIncludedVat});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // colId
            // 
            this.colId.FieldName = "Id";
            this.colId.Name = "colId";
            // 
            // colStockInNumber
            // 
            this.colStockInNumber.FieldName = "StockInNumber";
            this.colStockInNumber.Name = "colStockInNumber";
            this.colStockInNumber.Visible = true;
            this.colStockInNumber.VisibleIndex = 0;
            // 
            // colStockInDate
            // 
            this.colStockInDate.FieldName = "StockInDate";
            this.colStockInDate.Name = "colStockInDate";
            this.colStockInDate.Visible = true;
            this.colStockInDate.VisibleIndex = 1;
            // 
            // colLoaiNhapXuatKho
            // 
            this.colLoaiNhapXuatKho.FieldName = "LoaiNhapXuatKho";
            this.colLoaiNhapXuatKho.Name = "colLoaiNhapXuatKho";
            this.colLoaiNhapXuatKho.Visible = true;
            this.colLoaiNhapXuatKho.VisibleIndex = 2;
            // 
            // colTrangThai
            // 
            this.colTrangThai.FieldName = "TrangThai";
            this.colTrangThai.Name = "colTrangThai";
            this.colTrangThai.Visible = true;
            this.colTrangThai.VisibleIndex = 3;
            // 
            // colWarehouseId
            // 
            this.colWarehouseId.FieldName = "WarehouseId";
            this.colWarehouseId.Name = "colWarehouseId";
            this.colWarehouseId.Visible = true;
            this.colWarehouseId.VisibleIndex = 4;
            // 
            // colWarehouseCode
            // 
            this.colWarehouseCode.FieldName = "WarehouseCode";
            this.colWarehouseCode.Name = "colWarehouseCode";
            this.colWarehouseCode.Visible = true;
            this.colWarehouseCode.VisibleIndex = 5;
            // 
            // colWarehouseName
            // 
            this.colWarehouseName.FieldName = "WarehouseName";
            this.colWarehouseName.Name = "colWarehouseName";
            this.colWarehouseName.Visible = true;
            this.colWarehouseName.VisibleIndex = 6;
            // 
            // colPurchaseOrderId
            // 
            this.colPurchaseOrderId.FieldName = "PurchaseOrderId";
            this.colPurchaseOrderId.Name = "colPurchaseOrderId";
            this.colPurchaseOrderId.Visible = true;
            this.colPurchaseOrderId.VisibleIndex = 7;
            // 
            // colPurchaseOrderNumber
            // 
            this.colPurchaseOrderNumber.FieldName = "PurchaseOrderNumber";
            this.colPurchaseOrderNumber.Name = "colPurchaseOrderNumber";
            this.colPurchaseOrderNumber.Visible = true;
            this.colPurchaseOrderNumber.VisibleIndex = 8;
            // 
            // colSupplierId
            // 
            this.colSupplierId.FieldName = "SupplierId";
            this.colSupplierId.Name = "colSupplierId";
            this.colSupplierId.Visible = true;
            this.colSupplierId.VisibleIndex = 9;
            // 
            // colSupplierName
            // 
            this.colSupplierName.FieldName = "SupplierName";
            this.colSupplierName.Name = "colSupplierName";
            this.colSupplierName.Visible = true;
            this.colSupplierName.VisibleIndex = 10;
            // 
            // colNotes
            // 
            this.colNotes.FieldName = "Notes";
            this.colNotes.Name = "colNotes";
            this.colNotes.Visible = true;
            this.colNotes.VisibleIndex = 11;
            // 
            // colNguoiNhanHang
            // 
            this.colNguoiNhanHang.FieldName = "NguoiNhanHang";
            this.colNguoiNhanHang.Name = "colNguoiNhanHang";
            this.colNguoiNhanHang.Visible = true;
            this.colNguoiNhanHang.VisibleIndex = 12;
            // 
            // colNguoiGiaoHang
            // 
            this.colNguoiGiaoHang.FieldName = "NguoiGiaoHang";
            this.colNguoiGiaoHang.Name = "colNguoiGiaoHang";
            this.colNguoiGiaoHang.Visible = true;
            this.colNguoiGiaoHang.VisibleIndex = 13;
            // 
            // colTotalQuantity
            // 
            this.colTotalQuantity.FieldName = "TotalQuantity";
            this.colTotalQuantity.Name = "colTotalQuantity";
            this.colTotalQuantity.OptionsColumn.ReadOnly = true;
            this.colTotalQuantity.Visible = true;
            this.colTotalQuantity.VisibleIndex = 14;
            // 
            // colTotalAmount
            // 
            this.colTotalAmount.FieldName = "TotalAmount";
            this.colTotalAmount.Name = "colTotalAmount";
            this.colTotalAmount.OptionsColumn.ReadOnly = true;
            this.colTotalAmount.Visible = true;
            this.colTotalAmount.VisibleIndex = 15;
            // 
            // colTotalVat
            // 
            this.colTotalVat.FieldName = "TotalVat";
            this.colTotalVat.Name = "colTotalVat";
            this.colTotalVat.OptionsColumn.ReadOnly = true;
            this.colTotalVat.Visible = true;
            this.colTotalVat.VisibleIndex = 16;
            // 
            // colTotalAmountIncludedVat
            // 
            this.colTotalAmountIncludedVat.FieldName = "TotalAmountIncludedVat";
            this.colTotalAmountIncludedVat.Name = "colTotalAmountIncludedVat";
            this.colTotalAmountIncludedVat.OptionsColumn.ReadOnly = true;
            this.colTotalAmountIncludedVat.Visible = true;
            this.colTotalAmountIncludedVat.VisibleIndex = 17;
            // 
            // XtraForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1219, 902);
            this.Controls.Add(this.gridControl1);
            this.Name = "XtraForm1";
            this.Text = "XtraForm1";
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockInReportDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.XtraOpenFileDialog xtraOpenFileDialog1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colId;
        private DevExpress.XtraGrid.Columns.GridColumn colStockInNumber;
        private DevExpress.XtraGrid.Columns.GridColumn colStockInDate;
        private DevExpress.XtraGrid.Columns.GridColumn colLoaiNhapXuatKho;
        private DevExpress.XtraGrid.Columns.GridColumn colTrangThai;
        private DevExpress.XtraGrid.Columns.GridColumn colWarehouseId;
        private DevExpress.XtraGrid.Columns.GridColumn colWarehouseCode;
        private DevExpress.XtraGrid.Columns.GridColumn colWarehouseName;
        private DevExpress.XtraGrid.Columns.GridColumn colPurchaseOrderId;
        private DevExpress.XtraGrid.Columns.GridColumn colPurchaseOrderNumber;
        private DevExpress.XtraGrid.Columns.GridColumn colSupplierId;
        private DevExpress.XtraGrid.Columns.GridColumn colSupplierName;
        private DevExpress.XtraGrid.Columns.GridColumn colNotes;
        private DevExpress.XtraGrid.Columns.GridColumn colNguoiNhanHang;
        private DevExpress.XtraGrid.Columns.GridColumn colNguoiGiaoHang;
        private DevExpress.XtraGrid.Columns.GridColumn colTotalQuantity;
        private DevExpress.XtraGrid.Columns.GridColumn colTotalAmount;
        private DevExpress.XtraGrid.Columns.GridColumn colTotalVat;
        private DevExpress.XtraGrid.Columns.GridColumn colTotalAmountIncludedVat;
        private System.Windows.Forms.BindingSource stockInReportDtoBindingSource;
        private System.Windows.Forms.BindingSource xuatBaoHanhMasterDtoBindingSource;
    }
}