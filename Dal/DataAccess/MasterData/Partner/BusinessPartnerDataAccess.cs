using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dal.BaseDataAccess;
using Dal.DataContext;
using Dal.Exceptions;
using Dal.Logging;

namespace Dal.DataAccess.MasterData.Partner
{
	/// <summary>
	/// Data Access cho thực thể BusinessPartner (LINQ to SQL trên VnsErp2025DataContext).
	/// Cung cấp CRUD đầy đủ, phương thức đặc thù và phiên bản đồng bộ/bất đồng bộ.
	/// </summary>
	public class BusinessPartnerDataAccess : BaseDataAccess<BusinessPartner>
	{
		#region Constructors

		/// <summary>
		/// Khởi tạo mặc định.
		/// </summary>
		/// <param name="logger">Logger (tùy chọn)</param>
		public BusinessPartnerDataAccess(ILogger logger = null) : base(logger)
		{
		}

		/// <summary>
		/// Khởi tạo với connection string.
		/// </summary>
		/// <param name="connectionString">Chuỗi kết nối</param>
		/// <param name="logger">Logger (tùy chọn)</param>
		public BusinessPartnerDataAccess(string connectionString, ILogger logger = null) : base(connectionString, logger)
		{
		}

		#endregion

		#region Create

		/// <summary>
		/// Thêm đối tác mới với validation cơ bản (mã/code duy nhất, tên bắt buộc).
		/// </summary>
		/// <param name="code">Mã đối tác</param>
		/// <param name="name">Tên đối tác</param>
		/// <param name="partnerType">Loại đối tác</param>
		/// <param name="isActive">Trạng thái</param>
		/// <returns>Đối tác đã tạo</returns>
		public BusinessPartner AddNewPartner(string code, string name, int partnerType, bool isActive = true)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(code))
					throw new ArgumentException("Mã đối tác không được rỗng", nameof(code));
				if (string.IsNullOrWhiteSpace(name))
					throw new ArgumentException("Tên đối tác không được rỗng", nameof(name));

				if (IsPartnerCodeExists(code))
					throw new DataAccessException($"Mã đối tác '{code}' đã tồn tại");

				using var context = CreateContext();
				var entity = new BusinessPartner
				{
					Id = Guid.NewGuid(),
					PartnerCode = code,
					PartnerName = name,
					PartnerType = partnerType,
					IsActive = isActive,
					CreatedDate = DateTime.Now,
					UpdatedDate = null
				};

				context.BusinessPartners.InsertOnSubmit(entity);
				context.SubmitChanges();
				return entity;
			}
			catch (System.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 2627)
			{
				throw new DataAccessException($"Trùng mã đối tác '{code}'", sqlEx) { SqlErrorNumber = sqlEx.Number, ThoiGianLoi = DateTime.Now };
			}
			catch (System.Data.SqlClient.SqlException sqlEx)
			{
				throw new DataAccessException($"Lỗi SQL khi thêm đối tác '{code}': {sqlEx.Message}", sqlEx) { SqlErrorNumber = sqlEx.Number, ThoiGianLoi = DateTime.Now };
			}
			catch (Exception ex)
			{
				throw new DataAccessException($"Lỗi khi thêm đối tác mới '{code}': {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Thêm đối tác mới (Async).
		/// </summary>
		public async Task<BusinessPartner> AddNewPartnerAsync(string code, string name, int partnerType, bool isActive = true)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(code))
					throw new ArgumentException("Mã đối tác không được rỗng", nameof(code));
				if (string.IsNullOrWhiteSpace(name))
					throw new ArgumentException("Tên đối tác không được rỗng", nameof(name));

				if (await IsPartnerCodeExistsAsync(code))
					throw new DataAccessException($"Mã đối tác '{code}' đã tồn tại");

				using var context = CreateContext();
				var entity = new BusinessPartner
				{
					Id = Guid.NewGuid(),
					PartnerCode = code,
					PartnerName = name,
					PartnerType = partnerType,
					IsActive = isActive,
					CreatedDate = DateTime.Now,
					UpdatedDate = null
				};

				context.BusinessPartners.InsertOnSubmit(entity);
				await Task.Run(() => context.SubmitChanges());
				return entity;
			}
			catch (System.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 2627)
			{
				throw new DataAccessException($"Trùng mã đối tác '{code}'", sqlEx) { SqlErrorNumber = sqlEx.Number, ThoiGianLoi = DateTime.Now };
			}
			catch (System.Data.SqlClient.SqlException sqlEx)
			{
				throw new DataAccessException($"Lỗi SQL khi thêm đối tác '{code}': {sqlEx.Message}", sqlEx) { SqlErrorNumber = sqlEx.Number, ThoiGianLoi = DateTime.Now };
			}
			catch (Exception ex)
			{
				throw new DataAccessException($"Lỗi khi thêm đối tác mới '{code}': {ex.Message}", ex);
			}
		}

		#endregion

		#region Read

		/// <summary>
		/// Lấy đối tác theo Id.
		/// </summary>
		public BusinessPartner GetById(Guid id)
		{
			try
			{
				using var context = CreateContext();
				return context.BusinessPartners.FirstOrDefault(x => x.Id == id);
			}
			catch (System.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 1205)
			{
				System.Threading.Thread.Sleep(100);
				using var context = CreateContext();
				return context.BusinessPartners.FirstOrDefault(x => x.Id == id);
			}
			catch (Exception ex)
			{
				throw new DataAccessException($"Lỗi khi lấy đối tác theo Id {id}: {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Lấy đối tác theo Id (Async).
		/// </summary>
		public async Task<BusinessPartner> GetByIdAsync(Guid id)
		{
			try
			{
				using var context = CreateContext();
				return await Task.Run(() => context.BusinessPartners.FirstOrDefault(x => x.Id == id));
			}
			catch (System.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 1205)
			{
				await Task.Delay(100);
				using var context = CreateContext();
				return await Task.Run(() => context.BusinessPartners.FirstOrDefault(x => x.Id == id));
			}
			catch (Exception ex)
			{
				throw new DataAccessException($"Lỗi khi lấy đối tác theo Id {id}: {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Lấy đối tác theo mã.
		/// </summary>
		public BusinessPartner GetByCode(string code)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(code)) return null;
				using var context = CreateContext();
				return context.BusinessPartners.FirstOrDefault(x => x.PartnerCode == code);
			}
			catch (Exception ex)
			{
				throw new DataAccessException($"Lỗi khi lấy đối tác theo mã '{code}': {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Lấy đối tác theo mã (Async).
		/// </summary>
		public async Task<BusinessPartner> GetByCodeAsync(string code)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(code)) return null;
				using var context = CreateContext();
				return await Task.Run(() => context.BusinessPartners.FirstOrDefault(x => x.PartnerCode == code));
			}
			catch (Exception ex)
			{
				throw new DataAccessException($"Lỗi khi lấy đối tác theo mã '{code}': {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Tìm kiếm đối tác theo tên (contains, case-insensitive).
		/// </summary>
		public List<BusinessPartner> SearchByName(string keyword)
		{
			try
			{
				using var context = CreateContext();
				if (string.IsNullOrWhiteSpace(keyword))
					return context.BusinessPartners.ToList();
				var lower = keyword.ToLower();
				return context.BusinessPartners.Where(x => x.PartnerName.ToLower().Contains(lower)).ToList();
			}
			catch (Exception ex)
			{
				throw new DataAccessException($"Lỗi khi tìm kiếm theo tên '{keyword}': {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Lấy danh sách đối tác đang hoạt động.
		/// </summary>
		public List<BusinessPartner> GetActivePartners()
		{
			try
			{
				using var context = CreateContext();
				return context.BusinessPartners.Where(x => x.IsActive == true).ToList();
			}
			catch (Exception ex)
			{
				throw new DataAccessException($"Lỗi khi lấy danh sách đối tác active: {ex.Message}", ex);
			}
		}

		#endregion

		#region Update

		/// <summary>
		/// Cập nhật thông tin liên hệ (điện thoại, email) cho đối tác.
		/// </summary>
		public void UpdateContactInfo(Guid id, string phone, string email)
		{
			try
			{
				using var context = CreateContext();
				var entity = context.BusinessPartners.FirstOrDefault(x => x.Id == id);
				if (entity == null)
					throw new DataAccessException($"Không tìm thấy đối tác với Id: {id}");

				entity.Phone = phone;
				entity.Email = email;
				entity.UpdatedDate = DateTime.Now;
				context.SubmitChanges();
			}
			catch (Exception ex)
			{
				throw new DataAccessException($"Lỗi khi cập nhật thông tin liên hệ đối tác {id}: {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Kích hoạt/Vô hiệu hóa đối tác.
		/// </summary>
		public void SetActive(Guid id, bool isActive)
		{
			try
			{
				using var context = CreateContext();
				var entity = context.BusinessPartners.FirstOrDefault(x => x.Id == id);
				if (entity == null)
					throw new DataAccessException($"Không tìm thấy đối tác với Id: {id}");

				entity.IsActive = isActive;
				entity.UpdatedDate = DateTime.Now;
				context.SubmitChanges();
			}
			catch (Exception ex)
			{
				throw new DataAccessException($"Lỗi khi cập nhật trạng thái đối tác {id}: {ex.Message}", ex);
			}
		}

		#endregion

		#region Delete

		/// <summary>
		/// Xóa đối tác theo Id.
		/// </summary>
		public void DeletePartner(Guid id)
		{
			try
			{
				using var context = CreateContext();
				var entity = context.BusinessPartners.FirstOrDefault(x => x.Id == id);
				if (entity == null)
					return;
				context.BusinessPartners.DeleteOnSubmit(entity);
				context.SubmitChanges();
			}
			catch (Exception ex)
			{
				throw new DataAccessException($"Lỗi khi xóa đối tác {id}: {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Xóa đối tác theo Id (Async).
		/// </summary>
		public async Task DeletePartnerAsync(Guid id)
		{
			try
			{
				using var context = CreateContext();
				var entity = context.BusinessPartners.FirstOrDefault(x => x.Id == id);
				if (entity == null)
					return;
				context.BusinessPartners.DeleteOnSubmit(entity);
				await Task.Run(() => context.SubmitChanges());
			}
			catch (Exception ex)
			{
				throw new DataAccessException($"Lỗi khi xóa đối tác {id}: {ex.Message}", ex);
			}
		}

		#endregion

		#region Exists Checks

		/// <summary>
		/// Kiểm tra tồn tại theo Id.
		/// </summary>
		public bool Exists(Guid id)
		{
			try
			{
				using var context = CreateContext();
				return context.BusinessPartners.Any(x => x.Id == id);
			}
			catch (Exception ex)
			{
				throw new DataAccessException($"Lỗi khi kiểm tra tồn tại đối tác Id {id}: {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Kiểm tra tồn tại mã đối tác.
		/// </summary>
		public bool IsPartnerCodeExists(string code)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(code)) return false;
				using var context = CreateContext();
				return context.BusinessPartners.Any(x => x.PartnerCode == code);
			}
			catch (Exception ex)
			{
				throw new DataAccessException($"Lỗi khi kiểm tra mã đối tác '{code}': {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Kiểm tra tồn tại mã đối tác (Async).
		/// </summary>
		public async Task<bool> IsPartnerCodeExistsAsync(string code)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(code)) return false;
				using var context = CreateContext();
				return await Task.Run(() => context.BusinessPartners.Any(x => x.PartnerCode == code));
			}
			catch (System.Data.SqlClient.SqlException sqlEx)
			{
				throw new DataAccessException($"Lỗi SQL khi kiểm tra mã '{code}': {sqlEx.Message}", sqlEx) { SqlErrorNumber = sqlEx.Number, ThoiGianLoi = DateTime.Now };
			}
			catch (Exception ex)
			{
				throw new DataAccessException($"Lỗi khi kiểm tra mã đối tác '{code}': {ex.Message}", ex);
			}
		}

		#endregion

        #region Save/Update Full Entity

        /// <summary>
        /// Thêm mới hoặc cập nhật đầy đủ thông tin đối tác.
        /// Nếu Id tồn tại -> cập nhật tất cả trường theo entity truyền vào.
        /// Nếu không tồn tại -> thêm mới.
        /// </summary>
        public void SaveOrUpdate(BusinessPartner source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            try
            {
                using var context = CreateContext();
                var existing = context.BusinessPartners.FirstOrDefault(x => x.Id == source.Id);
                if (existing == null || source.Id == Guid.Empty)
                {
                    // ensure new Id
                    if (source.Id == Guid.Empty) source.Id = Guid.NewGuid();
                    source.CreatedDate = DateTime.Now;
                    context.BusinessPartners.InsertOnSubmit(source);
                    
                    // Tạo BusinessPartnerSite là trụ sở chính
                    var mainSite = new BusinessPartnerSite
                    {
                        Id = Guid.NewGuid(),
                        PartnerId = source.Id,
                        SiteCode = $"{source.PartnerCode}-MAIN",
                        SiteName = $"Trụ sở chính - {source.PartnerName}",
                        Address = source.Address,
                        City = source.City,
                        Province = source.City, // Sử dụng City làm Province cho trụ sở chính
                        Country = source.Country,
                        ContactPerson = source.ContactPerson,
                        Phone = source.Phone,
                        Email = source.Email,
                        IsDefault = true, // Đánh dấu là trụ sở chính
                        IsActive = source.IsActive,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = null
                    };
                    
                    context.BusinessPartnerSites.InsertOnSubmit(mainSite);
                }
                else
                {
                    // copy fields
                    existing.PartnerCode = source.PartnerCode;
                    existing.PartnerName = source.PartnerName;
                    existing.PartnerType = source.PartnerType;
                    existing.TaxCode = source.TaxCode;
                    existing.Phone = source.Phone;
                    existing.Email = source.Email;
                    existing.Website = source.Website;
                    existing.Address = source.Address;
                    existing.City = source.City;
                    existing.Country = source.Country;
                    existing.ContactPerson = source.ContactPerson;
                    existing.ContactPosition = source.ContactPosition;
                    existing.BankAccount = source.BankAccount;
                    existing.BankName = source.BankName;
                    existing.CreditLimit = source.CreditLimit;
                    existing.PaymentTerm = source.PaymentTerm;
                    existing.IsActive = source.IsActive;
                    existing.UpdatedDate = DateTime.Now;
                }
                context.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lưu/cập nhật đối tác: {ex.Message}", ex);
            }
        }

        #endregion
    }
}
