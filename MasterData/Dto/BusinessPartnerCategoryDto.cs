using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MasterData.Dto
{
    public class BusinessPartnerCategoryDto
    {
        [DisplayName("ID")]
        public Guid Id { get; set; }

        [DisplayName("Tên phân loại")]
        [Required(ErrorMessage = "Tên phân loại không được để trống")]
        [StringLength(100, ErrorMessage = "Tên phân loại không được vượt quá 100 ký tự")]
        public string CategoryName { get; set; }

        [DisplayName("Mô tả")]
        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string Description { get; set; }
    }
}