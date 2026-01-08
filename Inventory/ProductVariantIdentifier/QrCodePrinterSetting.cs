using System;
using System.Drawing.Printing;

namespace Inventory.ProductVariantIdentifier
{
    internal class QrCodePrinterSetting
    {
        public PrinterSettings PrinterSetting { get; set; }
        public int PrintWidthMm { get; set; }
        public int PrintHeightMm { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
