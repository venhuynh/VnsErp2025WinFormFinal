using System.ComponentModel;

namespace DTO.DeviceAssetManagement
{
    public enum DeviceIdentifierEnum
    {
        //Số Serial
        [Description("Serial Number")] SerialNumber = 0,

        //Địa chỉ MAC
        [Description("MAC")] MAC = 1,

        //Số IMEI
        [Description("IMEI")] IMEI = 2,

        //AssetTag
        [Description("Asset Tag")] AssetTag = 3,

        //LicenseKey
        [Description("License Key")] LicenseKey = 4

    }
}
