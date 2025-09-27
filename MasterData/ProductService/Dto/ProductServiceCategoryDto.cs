using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MasterData.ProductService.Dto
{
	public class ProductServiceCategoryDto
	{
		[DisplayName("ID")] 
		public Guid Id { get; set; }

		[DisplayName("Tên danh mục")]
		[Required(ErrorMessage = "Tên danh mục không được để trống")]
		[StringLength(200, ErrorMessage = "Tên danh mục không được vượt quá 200 ký tự")]
		public string CategoryName { get; set; }

		[DisplayName("Mô tả")]
		[StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
		public string Description { get; set; }

		[DisplayName("Danh mục cha")]
		[Description("ID của danh mục cha (null nếu là danh mục gốc)")]
		public Guid? ParentId { get; set; }

		[DisplayName("Tên danh mục cha")]
		[Description("Tên của danh mục cha (để hiển thị)")]
		public string ParentCategoryName { get; set; }

		[DisplayName("Cấp độ")]
		[Description("Cấp độ trong cây phân cấp (0 = gốc, 1 = con, ...)")]
		public int Level { get; set; }

		[DisplayName("Có danh mục con")]
		[Description("Có danh mục con hay không")]
		public bool HasChildren { get; set; }

		[DisplayName("Đường dẫn đầy đủ")]
		[Description("Đường dẫn đầy đủ từ gốc đến danh mục này")]
		public string FullPath { get; set; }

		[DisplayName("Số lượng sản phẩm/dịch vụ")]
		[Description("Tổng số Product/Service thuộc danh mục này")]
		public int ProductCount { get; set; }
	}
}


