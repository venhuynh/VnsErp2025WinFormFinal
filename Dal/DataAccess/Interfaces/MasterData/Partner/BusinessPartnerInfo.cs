using System;

namespace Dal.DataAccess.MasterData
{
    /// <summary>
    /// Thông tin đối tác để hiển thị trong TreeList
    /// </summary>
    public class BusinessPartnerInfo
    {
        public Guid Id { get; set; }
        public string PartnerCode { get; set; }
        public string PartnerName { get; set; }
        public Guid CategoryId { get; set; }
    }
}
