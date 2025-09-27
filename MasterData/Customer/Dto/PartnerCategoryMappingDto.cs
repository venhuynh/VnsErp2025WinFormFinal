using System;

namespace MasterData.Dto.Customer
{
    public class PartnerCategoryMappingDto
    {
        public Guid PartnerId { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}