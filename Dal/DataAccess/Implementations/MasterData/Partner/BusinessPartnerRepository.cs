using Dal.DataAccess.Interfaces.MasterData.Partner;
using Dal.DataContext;
using Dal.Exceptions;
using Logger;
using Logger.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.MasterData.Partner;

/// <summary>
/// Data Access cho thực thể BusinessPartner (LINQ to SQL trên VnsErp2025DataContext).
/// Cung cấp CRUD đầy đủ, phương thức đặc thù và phiên bản đồng bộ/bất đồng bộ.
/// </summary>
public class BusinessPartnerRepository : IBusinessPartnerRepository
{
    #region Private Fields

    /// <summary>
    /// Chuỗi kết nối cơ sở dữ liệu để tạo DataContext mới cho mỗi operation.
    /// </summary>
    private readonly string _connectionString;

    /// <summary>
    /// Instance logger để theo dõi các thao tác và lỗi
    /// </summary>
    private readonly CustomLogger _logger;

    #endregion

    #region Constructor

    /// <summary>
    /// Khởi tạo một instance mới của class BusinessPartnerRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public BusinessPartnerRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("BusinessPartnerRepository được khởi tạo với connection string");
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Tạo DataContext mới cho mỗi operation để tránh cache issue
    /// </summary>
    /// <returns>DataContext mới</returns>
    private VnsErp2025DataContext CreateNewContext()
    {
        var context = new VnsErp2025DataContext(_connectionString);

        // Configure eager loading cho navigation properties
        var loadOptions = new DataLoadOptions();
        context.LoadOptions = loadOptions;

        return context;
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
                throw new ArgumentException(@"Mã đối tác không được rỗng", nameof(code));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(@"Tên đối tác không được rỗng", nameof(name));

            if (IsPartnerCodeExists(code))
                throw new DataAccessException($"Mã đối tác '{code}' đã tồn tại");

            using var context = CreateNewContext();
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
            
            _logger.Info($"Đã thêm mới đối tác: {entity.PartnerCode} - {entity.PartnerName}");
            return entity;
        }
        catch (SqlException sqlEx) when (sqlEx.Number == 2627)
        {
            _logger.Error($"Trùng mã đối tác '{code}': {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Trùng mã đối tác '{code}'", sqlEx) 
            { 
                SqlErrorNumber = sqlEx.Number, 
                ThoiGianLoi = DateTime.Now 
            };
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi thêm đối tác '{code}': {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi thêm đối tác '{code}': {sqlEx.Message}", sqlEx) 
            { 
                SqlErrorNumber = sqlEx.Number, 
                ThoiGianLoi = DateTime.Now 
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi thêm đối tác mới '{code}': {ex.Message}", ex);
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
                throw new ArgumentException(@"Mã đối tác không được rỗng", nameof(code));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(@"Tên đối tác không được rỗng", nameof(name));

            if (await IsPartnerCodeExistsAsync(code))
                throw new DataAccessException($"Mã đối tác '{code}' đã tồn tại");

            using var context = CreateNewContext();
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
            
            _logger.Info($"Đã thêm mới đối tác (async): {entity.PartnerCode} - {entity.PartnerName}");
            return entity;
        }
        catch (SqlException sqlEx) when (sqlEx.Number == 2627)
        {
            _logger.Error($"Trùng mã đối tác '{code}': {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Trùng mã đối tác '{code}'", sqlEx) 
            { 
                SqlErrorNumber = sqlEx.Number, 
                ThoiGianLoi = DateTime.Now 
            };
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi thêm đối tác '{code}': {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi thêm đối tác '{code}': {sqlEx.Message}", sqlEx) 
            { 
                SqlErrorNumber = sqlEx.Number, 
                ThoiGianLoi = DateTime.Now 
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi thêm đối tác mới '{code}': {ex.Message}", ex);
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
            using var context = CreateNewContext();
            var partner = context.BusinessPartners.FirstOrDefault(x => x.Id == id);
            
            if (partner != null)
            {
                _logger.Debug($"Đã lấy đối tác theo ID: {id} - {partner.PartnerName}");
            }
            
            return partner;
        }
        catch (SqlException sqlEx) when (sqlEx.Number == 1205)
        {
            // Deadlock retry
            System.Threading.Thread.Sleep(100);
            using var context = CreateNewContext();
            var partner = context.BusinessPartners.FirstOrDefault(x => x.Id == id);
            
            if (partner != null)
            {
                _logger.Debug($"Đã lấy đối tác theo ID (retry): {id} - {partner.PartnerName}");
            }
            
            return partner;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy đối tác theo Id {id}: {ex.Message}", ex);
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
            using var context = CreateNewContext();
            var partner = await Task.Run(() => context.BusinessPartners.FirstOrDefault(x => x.Id == id));
            
            if (partner != null)
            {
                _logger.Debug($"Đã lấy đối tác theo ID (async): {id} - {partner.PartnerName}");
            }
            
            return partner;
        }
        catch (SqlException sqlEx) when (sqlEx.Number == 1205)
        {
            // Deadlock retry
            await Task.Delay(100);
            using var context = CreateNewContext();
            var partner = await Task.Run(() => context.BusinessPartners.FirstOrDefault(x => x.Id == id));
            
            if (partner != null)
            {
                _logger.Debug($"Đã lấy đối tác theo ID (async retry): {id} - {partner.PartnerName}");
            }
            
            return partner;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy đối tác theo Id {id}: {ex.Message}", ex);
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
            
            using var context = CreateNewContext();
            var partner = context.BusinessPartners.FirstOrDefault(x => x.PartnerCode == code);
            
            if (partner != null)
            {
                _logger.Debug($"Đã lấy đối tác theo mã: {code} - {partner.PartnerName}");
            }
            
            return partner;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy đối tác theo mã '{code}': {ex.Message}", ex);
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
            
            using var context = CreateNewContext();
            var partner = await Task.Run(() => context.BusinessPartners.FirstOrDefault(x => x.PartnerCode == code));
            
            if (partner != null)
            {
                _logger.Debug($"Đã lấy đối tác theo mã (async): {code} - {partner.PartnerName}");
            }
            
            return partner;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy đối tác theo mã '{code}': {ex.Message}", ex);
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
            using var context = CreateNewContext();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                var all = context.BusinessPartners.ToList();
                _logger.Debug($"Đã tìm kiếm đối tác (không có keyword): {all.Count} kết quả");
                return all;
            }
            
            var lower = keyword.ToLower();
            var results = context.BusinessPartners
                .Where(x => x.PartnerName.ToLower().Contains(lower))
                .ToList();
            
            _logger.Debug($"Đã tìm kiếm đối tác theo keyword '{keyword}': {results.Count} kết quả");
            return results;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi tìm kiếm theo tên '{keyword}': {ex.Message}", ex);
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
            using var context = CreateNewContext();
            var partners = context.BusinessPartners.Where(x => x.IsActive == true).ToList();
            
            _logger.Debug($"Đã lấy {partners.Count} đối tác đang hoạt động");
            return partners;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy danh sách đối tác active: {ex.Message}", ex);
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
            using var context = CreateNewContext();
            var entity = context.BusinessPartners.FirstOrDefault(x => x.Id == id);
            if (entity == null)
                throw new DataAccessException($"Không tìm thấy đối tác với Id: {id}");

            entity.Phone = phone;
            entity.Email = email;
            entity.UpdatedDate = DateTime.Now;
            context.SubmitChanges();
            
            _logger.Info($"Đã cập nhật thông tin liên hệ cho đối tác: {id} - {entity.PartnerName}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật thông tin liên hệ đối tác {id}: {ex.Message}", ex);
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
            using var context = CreateNewContext();
            var entity = context.BusinessPartners.FirstOrDefault(x => x.Id == id);
            if (entity == null)
                throw new DataAccessException($"Không tìm thấy đối tác với Id: {id}");

            entity.IsActive = isActive;
            entity.UpdatedDate = DateTime.Now;
            context.SubmitChanges();
            
            _logger.Info($"Đã cập nhật trạng thái đối tác: {id} - {entity.PartnerName} (IsActive={isActive})");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật trạng thái đối tác {id}: {ex.Message}", ex);
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
            using var context = CreateNewContext();
            var entity = context.BusinessPartners.FirstOrDefault(x => x.Id == id);
            if (entity == null)
                return;
                
            context.BusinessPartners.DeleteOnSubmit(entity);
            context.SubmitChanges();
            
            _logger.Info($"Đã xóa đối tác: {id} - {entity.PartnerName}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa đối tác {id}: {ex.Message}", ex);
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
            using var context = CreateNewContext();
            var entity = context.BusinessPartners.FirstOrDefault(x => x.Id == id);
            if (entity == null)
                return;
                
            context.BusinessPartners.DeleteOnSubmit(entity);
            await Task.Run(() => context.SubmitChanges());
            
            _logger.Info($"Đã xóa đối tác (async): {id} - {entity.PartnerName}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa đối tác {id}: {ex.Message}", ex);
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
            using var context = CreateNewContext();
            var result = context.BusinessPartners.Any(x => x.Id == id);
            
            _logger.Debug($"Exists check cho partner ID {id}: {result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra tồn tại đối tác Id {id}: {ex.Message}", ex);
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
            
            using var context = CreateNewContext();
            var result = context.BusinessPartners.Any(x => x.PartnerCode == code);
            
            _logger.Debug($"IsPartnerCodeExists: Code='{code}', Result={result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra mã đối tác '{code}': {ex.Message}", ex);
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
            
            using var context = CreateNewContext();
            var result = await Task.Run(() => context.BusinessPartners.Any(x => x.PartnerCode == code));
            
            _logger.Debug($"IsPartnerCodeExistsAsync: Code='{code}', Result={result}");
            return result;
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi kiểm tra mã '{code}': {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi kiểm tra mã '{code}': {sqlEx.Message}", sqlEx) 
            { 
                SqlErrorNumber = sqlEx.Number, 
                ThoiGianLoi = DateTime.Now 
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra mã đối tác '{code}': {ex.Message}", ex);
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
            using var context = CreateNewContext();
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
                context.SubmitChanges();
                
                _logger.Info($"Đã thêm mới đối tác và trụ sở chính (SaveOrUpdate): {source.PartnerCode} - {source.PartnerName}");
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
                
                context.SubmitChanges();
                
                _logger.Info($"Đã cập nhật đối tác (SaveOrUpdate): {existing.PartnerCode} - {existing.PartnerName}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lưu/cập nhật đối tác: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lưu/cập nhật đối tác: {ex.Message}", ex);
        }
    }

    #endregion
}
