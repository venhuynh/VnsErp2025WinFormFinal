using System;
using System.ComponentModel;

namespace MasterData.Customer.Dto
{
    public class BusinessPartnerSiteListDto
    {
        [DisplayName("ID")]
        public Guid Id { get; set; }

        [DisplayName("Tên chi nhánh")]
        public string SiteName { get; set; }
    }
}
