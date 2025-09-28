using Dal.BaseDataAccess;
using Dal.DataContext;
using Dal.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dal.DataAccess.MasterData.ProductServiceDataAccess
{
    /// <summary>
    /// Data Access Layer cho ProductImage
    /// </summary>
    public class ProductImageDataAccess : BaseDataAccess<ProductImage>
    {
        /// <summary>
        /// Lấy danh sách hình ảnh của sản phẩm/dịch vụ
        /// </summary>
        /// <param name="productId">ID sản phẩm/dịch vụ</param>
        /// <returns>Danh sách hình ảnh</returns>
        public List<ProductImage> GetByProductId(Guid productId)
        {
            try
            {
                using var context = CreateContext();
                return context.ProductImages
                    .Where(x => x.ProductId == productId && x.IsActive == true)
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.CreatedDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy danh sách hình ảnh cho sản phẩm '{productId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy hình ảnh chính của sản phẩm/dịch vụ
        /// </summary>
        /// <param name="productId">ID sản phẩm/dịch vụ</param>
        /// <returns>Hình ảnh chính hoặc null</returns>
        public ProductImage GetPrimaryByProductId(Guid productId)
        {
            try
            {
                using var context = CreateContext();
                return context.ProductImages
                    .FirstOrDefault(x => x.ProductId == productId && x.IsPrimary == true && x.IsActive == true);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy hình ảnh chính cho sản phẩm '{productId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lưu hoặc cập nhật hình ảnh
        /// </summary>
        /// <param name="productImage">Hình ảnh cần lưu</param>
        public void SaveOrUpdate(ProductImage productImage)
        {
            try
            {
                if (productImage == null)
                    throw new ArgumentNullException(nameof(productImage));

                using var context = CreateContext();
                
                // Nếu là hình ảnh chính, bỏ IsPrimary của các hình ảnh khác
                if (productImage.IsPrimary == true)
                {
                    var existingPrimary = context.ProductImages
                        .Where(x => x.ProductId == productImage.ProductId && x.IsPrimary == true && x.Id != productImage.Id)
                        .ToList();
                    
                    foreach (var img in existingPrimary)
                    {
                        img.IsPrimary = false;
                    }
                }

                var existing = productImage.Id != Guid.Empty ? 
                    context.ProductImages.FirstOrDefault(x => x.Id == productImage.Id) : null;

                if (existing == null)
                {
                    // Thêm mới
                    if (productImage.Id == Guid.Empty)
                        productImage.Id = Guid.NewGuid();
                    
                    // Thiết lập giá trị mặc định
                    if (productImage.CreatedDate == null)
                        productImage.CreatedDate = DateTime.Now;
                    if (productImage.IsActive == null)
                        productImage.IsActive = true;
                    if (productImage.SortOrder == null)
                        productImage.SortOrder = GetNextSortOrder(productImage.ProductId);

                    context.ProductImages.InsertOnSubmit(productImage);
                }
                else
                {
                    // Cập nhật
                    existing.ProductId = productImage.ProductId;
                    existing.VariantId = productImage.VariantId;
                    existing.ImagePath = productImage.ImagePath;
                    existing.SortOrder = productImage.SortOrder;
                    existing.IsPrimary = productImage.IsPrimary;
                    existing.ImageData = productImage.ImageData;
                    existing.ImageType = productImage.ImageType;
                    existing.ImageSize = productImage.ImageSize;
                    existing.ImageWidth = productImage.ImageWidth;
                    existing.ImageHeight = productImage.ImageHeight;
                    existing.Caption = productImage.Caption;
                    existing.AltText = productImage.AltText;
                    existing.IsActive = productImage.IsActive;
                    existing.ModifiedDate = DateTime.Now;
                }

                context.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lưu hình ảnh '{productImage?.ImagePath}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa hình ảnh (soft delete)
        /// </summary>
        /// <param name="imageId">ID hình ảnh</param>
        public void Delete(Guid imageId)
        {
            try
            {
                using var context = CreateContext();
                var image = context.ProductImages.FirstOrDefault(x => x.Id == imageId);
                if (image != null)
                {
                    image.IsActive = false;
                    image.ModifiedDate = DateTime.Now;
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi xóa hình ảnh '{imageId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa vĩnh viễn hình ảnh
        /// </summary>
        /// <param name="imageId">ID hình ảnh</param>
        public void DeletePermanent(Guid imageId)
        {
            try
            {
                using var context = CreateContext();
                var image = context.ProductImages.FirstOrDefault(x => x.Id == imageId);
                if (image != null)
                {
                    context.ProductImages.DeleteOnSubmit(image);
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi xóa vĩnh viễn hình ảnh '{imageId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy SortOrder tiếp theo cho sản phẩm
        /// </summary>
        /// <param name="productId">ID sản phẩm</param>
        /// <returns>SortOrder tiếp theo</returns>
        private int GetNextSortOrder(Guid? productId)
        {
            try
            {
                using var context = CreateContext();
                var maxOrder = context.ProductImages
                    .Where(x => x.ProductId == productId)
                    .Max(x => (int?)x.SortOrder) ?? 0;
                return maxOrder + 1;
            }
            catch
            {
                return 1;
            }
        }

        /// <summary>
        /// Kiểm tra xem sản phẩm có hình ảnh chính chưa
        /// </summary>
        /// <param name="productId">ID sản phẩm</param>
        /// <returns>True nếu đã có hình ảnh chính</returns>
        public bool HasPrimaryImage(Guid productId)
        {
            try
            {
                using var context = CreateContext();
                return context.ProductImages
                    .Any(x => x.ProductId == productId && x.IsPrimary == true && x.IsActive == true);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi kiểm tra hình ảnh chính cho sản phẩm '{productId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Đặt hình ảnh làm hình ảnh chính
        /// </summary>
        /// <param name="imageId">ID hình ảnh</param>
        public void SetAsPrimary(Guid imageId)
        {
            try
            {
                using var context = CreateContext();
                var image = context.ProductImages.FirstOrDefault(x => x.Id == imageId);
                if (image != null)
                {
                    // Bỏ IsPrimary của các hình ảnh khác cùng sản phẩm
                    var otherImages = context.ProductImages
                        .Where(x => x.ProductId == image.ProductId && x.Id != imageId)
                        .ToList();
                    
                    foreach (var img in otherImages)
                    {
                        img.IsPrimary = false;
                    }

                    // Đặt hình ảnh này làm chính
                    image.IsPrimary = true;
                    image.ModifiedDate = DateTime.Now;
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi đặt hình ảnh làm chính '{imageId}': {ex.Message}", ex);
            }
        }
    }
}
