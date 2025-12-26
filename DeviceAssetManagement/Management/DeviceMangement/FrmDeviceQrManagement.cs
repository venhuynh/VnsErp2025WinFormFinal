using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.BarCodes;
using DevExpress.Drawing.Extensions;
using DevExpress.XtraPrinting.BarCode;
using QRCodeCompactionMode = DevExpress.BarCodes.QRCodeCompactionMode;

namespace DeviceAssetManagement.Management.DeviceMangement
{
    public partial class FrmDeviceQrManagement : DevExpress.XtraEditors.XtraForm
    {
        public FrmDeviceQrManagement()
        {
            InitializeComponent();
            cboErrorLevel.SelectedIndex = 2; // Q
            spinModule.EditValue = 2;
            lblStatus.Text = @"Sẵn sàng";
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            RenderQr();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (picPreview.Image == null)
            {
                lblStatus.Text = @"Chưa có mã để lưu.";
                return;
            }

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                picPreview.Image.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);
                lblStatus.Text = $@"Đã lưu: {saveFileDialog1.FileName}";
            }
        }

        private void RenderQr()
        {
            var payload = memoPayload.Text?.Trim();
            if (string.IsNullOrWhiteSpace(payload))
            {
                lblStatus.Text = @"Nội dung trống.";
                return;
            }

            var errorLevel = ParseErrorLevel(cboErrorLevel.EditValue?.ToString());
            var moduleSize = Convert.ToSingle(spinModule.Value);

            using var barCode = new BarCode();
            barCode.Symbology = Symbology.QRCode;
            barCode.BackColor = Color.White;
            barCode.ForeColor = Color.Black;
            barCode.RotationAngle = 0;
            barCode.DpiX = 96;
            barCode.DpiY = 96;
            barCode.Module = moduleSize;
            barCode.CodeBinaryData = Encoding.UTF8.GetBytes(payload);
            barCode.Options.QRCode.CompactionMode = QRCodeCompactionMode.Byte;
            barCode.Options.QRCode.ErrorLevel = errorLevel;
            barCode.Options.QRCode.ShowCodeText = false;

            picPreview.Image?.Dispose();
            picPreview.Image = barCode.BarCodeImage.ConvertToGdiPlusImage();
            lblStatus.Text = $@"Đã tạo mã QR ({payload.Length} ký tự, mức {errorLevel}).";
        }

        private static QRCodeErrorLevel ParseErrorLevel(string value)
        {
            return value?.ToUpperInvariant() switch
            {
                "L" => QRCodeErrorLevel.L,
                "M" => QRCodeErrorLevel.M,
                "H" => QRCodeErrorLevel.H,
                _ => QRCodeErrorLevel.Q
            };
        }
    }
}