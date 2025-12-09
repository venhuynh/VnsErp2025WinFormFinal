using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dal.DataAccess.Interfaces.MasterData.PartnerRepository;
using Dal.DataContext;
using Dal.Exceptions;
using Logger;
using Logger.Configuration;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.MasterData.PartnerRepository;

/// <summary>
/// Data Access Layer cho BusinessPartnerContact
/// </summary>
public class BusinessPartnerContactRepository : IBusinessPartnerContactRepository
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
    /// Khởi tạo một instance mới của class BusinessPartnerContactRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public BusinessPartnerContactRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("BusinessPartnerContactRepository được khởi tạo với connection string");
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
        loadOptions.LoadWith<BusinessPartnerContact>(c => c.BusinessPartnerSite);
        loadOptions.LoadWith<BusinessPartnerSite>(s => s.BusinessPartner);
        context.LoadOptions = loadOptions;

        return context;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Lấy tất cả BusinessPartnerContact với thông tin BusinessPartnerSite
    /// </summary>
    /// <returns>Danh sách BusinessPartnerContact</returns>
    public List<BusinessPartnerContact> GetAll()
    {
        try
        {
            using var context = CreateNewContext();

            // Load tất cả BusinessPartnerContact với BusinessPartnerSite đã được include
            var contacts = context.BusinessPartnerContacts
                .OrderBy(c => c.FullName)
                .ToList();

            _logger.Debug($"Đã lấy {contacts.Count} BusinessPartnerContact");
            return contacts;
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi lấy danh sách BusinessPartnerContact: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi lấy danh sách BusinessPartnerContact: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy danh sách BusinessPartnerContact: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy danh sách BusinessPartnerContact: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy tất cả BusinessPartnerContact với thông tin BusinessPartnerSite (Async)
    /// </summary>
    /// <returns>Danh sách BusinessPartnerContact</returns>
    public async Task<List<BusinessPartnerContact>> GetAllAsync()
    {
        try
        {
            using var context = CreateNewContext();

            var contacts = await Task.Run(() => context.BusinessPartnerContacts
                .OrderBy(c => c.FullName)
                .ToList());
            
            _logger.Debug($"Đã lấy {contacts.Count} BusinessPartnerContact (async)");
            return contacts;
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi lấy danh sách BusinessPartnerContact: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi lấy danh sách BusinessPartnerContact: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy danh sách BusinessPartnerContact: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy danh sách BusinessPartnerContact: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy BusinessPartnerContact theo ID
    /// </summary>
    /// <param name="id">ID của BusinessPartnerContact</param>
    /// <returns>BusinessPartnerContact hoặc null</returns>
    public BusinessPartnerContact GetById(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var contact = context.BusinessPartnerContacts.FirstOrDefault(c => c.Id == id);
            
            if (contact != null)
            {
                _logger.Debug($"Đã lấy BusinessPartnerContact theo ID: {id} - {contact.FullName}");
            }
            
            return contact;
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi lấy BusinessPartnerContact theo ID: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi lấy BusinessPartnerContact theo ID: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy BusinessPartnerContact theo ID: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy BusinessPartnerContact theo ID: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lưu hoặc cập nhật BusinessPartnerContact
    /// </summary>
    /// <param name="entity">BusinessPartnerContact entity</param>
    /// <returns>ID của entity đã lưu</returns>
    public Guid SaveOrUpdate(BusinessPartnerContact entity)
    {
        try
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            using var context = CreateNewContext();

            if (entity.Id == Guid.Empty)
            {
                // Thêm mới
                entity.Id = Guid.NewGuid();
                if (entity.CreatedDate == default(DateTime))
                {
                    entity.CreatedDate = DateTime.Now;
                }
                context.BusinessPartnerContacts.InsertOnSubmit(entity);
                context.SubmitChanges();
                
                _logger.Info($"Đã thêm mới BusinessPartnerContact: {entity.FullName}");
            }
            else
            {
                // Cập nhật
                var existingEntity = context.BusinessPartnerContacts.FirstOrDefault(c => c.Id == entity.Id);
                if (existingEntity != null)
                {
                    existingEntity.SiteId = entity.SiteId;
                    existingEntity.FullName = entity.FullName;
                    existingEntity.Position = entity.Position;
                    existingEntity.Phone = entity.Phone;
                    existingEntity.Email = entity.Email;
                    existingEntity.IsPrimary = entity.IsPrimary;
                    existingEntity.IsActive = entity.IsActive;
                    
                    // Cập nhật các fields mới
                    existingEntity.Mobile = entity.Mobile;
                    existingEntity.Fax = entity.Fax;
                    existingEntity.Department = entity.Department;
                    existingEntity.BirthDate = entity.BirthDate;
                    existingEntity.Gender = entity.Gender;
                    existingEntity.LinkedIn = entity.LinkedIn;
                    existingEntity.Skype = entity.Skype;
                    existingEntity.WeChat = entity.WeChat;
                    existingEntity.Notes = entity.Notes;
                    
                    // Copy Avatar fields (metadata only, Avatar binary field not in DataContext)
                    existingEntity.AvatarFileName = entity.AvatarFileName;
                    existingEntity.AvatarRelativePath = entity.AvatarRelativePath;
                    existingEntity.AvatarFullPath = entity.AvatarFullPath;
                    existingEntity.AvatarStorageType = entity.AvatarStorageType;
                    existingEntity.AvatarFileSize = entity.AvatarFileSize;
                    existingEntity.AvatarChecksum = entity.AvatarChecksum;
                    
                    // Copy AvatarThumbnailData (binary data stored in database)
                    existingEntity.AvatarThumbnailData = entity.AvatarThumbnailData;
                    
                    existingEntity.ModifiedDate = DateTime.Now;
                    
                    context.SubmitChanges();
                    _logger.Info($"Đã cập nhật BusinessPartnerContact: {existingEntity.FullName}");
                }
                else
                {
                    throw new DataAccessException("Không tìm thấy BusinessPartnerContact để cập nhật");
                }
            }

            return entity.Id;
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi lưu BusinessPartnerContact: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi lưu BusinessPartnerContact: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lưu BusinessPartnerContact: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lưu BusinessPartnerContact: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Xóa BusinessPartnerContact theo ID
    /// </summary>
    /// <param name="id">ID của BusinessPartnerContact</param>
    /// <returns>True nếu xóa thành công</returns>
    public bool Delete(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var entity = context.BusinessPartnerContacts.FirstOrDefault(c => c.Id == id);
            if (entity == null)
            {
                _logger.Warning($"Không tìm thấy BusinessPartnerContact để xóa: {id}");
                return false;
            }

            var entityName = entity.FullName;
            var entityId = entity.Id;
            
            _logger.Debug($"Bắt đầu xóa BusinessPartnerContact: {entityId} - {entityName}");
            
            // DeleteOnSubmit và SubmitChanges
            context.BusinessPartnerContacts.DeleteOnSubmit(entity);
            
            try
            {
                context.SubmitChanges();
                _logger.Info($"Đã xóa BusinessPartnerContact thành công: {entityId} - {entityName}");
                
                // Verify deletion bằng cách query lại
                using var verifyContext = CreateNewContext();
                var verifyEntity = verifyContext.BusinessPartnerContacts.FirstOrDefault(c => c.Id == id);
                if (verifyEntity != null)
                {
                    _logger.Error($"Xóa BusinessPartnerContact không thành công - Entity vẫn còn trong database: {entityId} - {entityName}");
                    return false;
                }
                
                _logger.Debug($"Đã xác nhận xóa BusinessPartnerContact: {entityId} - {entityName}");
                return true;
            }
            catch (SqlException sqlEx)
            {
                _logger.Error($"Lỗi SQL khi SubmitChanges xóa BusinessPartnerContact: {sqlEx.Message} (Error Number: {sqlEx.Number})", sqlEx);
                throw new DataAccessException($"Lỗi SQL khi xóa BusinessPartnerContact: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi xóa BusinessPartnerContact: {sqlEx.Message} (Error Number: {sqlEx.Number})", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi xóa BusinessPartnerContact: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa BusinessPartnerContact: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi xóa BusinessPartnerContact: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Kiểm tra Phone có tồn tại không
    /// </summary>
    /// <param name="phone">Phone cần kiểm tra</param>
    /// <param name="excludeId">ID cần loại trừ (cho trường hợp update)</param>
    /// <returns>True nếu đã tồn tại</returns>
    public bool IsPhoneExists(string phone, Guid? excludeId = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            using var context = CreateNewContext();
            var query = context.BusinessPartnerContacts.Where(c => c.Phone == phone);

            if (excludeId.HasValue)
            {
                query = query.Where(c => c.Id != excludeId.Value);
            }

            var result = query.Any();
            _logger.Debug($"IsPhoneExists: Phone='{phone}', ExcludeId={excludeId}, Result={result}");
            return result;
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi kiểm tra Phone: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi kiểm tra Phone: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra Phone: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi kiểm tra Phone: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Kiểm tra Email có tồn tại không
    /// </summary>
    /// <param name="email">Email cần kiểm tra</param>
    /// <param name="excludeId">ID cần loại trừ (cho trường hợp update)</param>
    /// <returns>True nếu đã tồn tại</returns>
    public bool IsEmailExists(string email, Guid? excludeId = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            using var context = CreateNewContext();
            var query = context.BusinessPartnerContacts.Where(c => c.Email == email);

            if (excludeId.HasValue)
            {
                query = query.Where(c => c.Id != excludeId.Value);
            }

            var result = query.Any();
            _logger.Debug($"IsEmailExists: Email='{email}', ExcludeId={excludeId}, Result={result}");
            return result;
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi kiểm tra Email: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi kiểm tra Email: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra Email: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi kiểm tra Email: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Cập nhật chỉ avatar thumbnail của BusinessPartnerContact (chỉ xử lý hình ảnh thumbnail)
    /// </summary>
    /// <param name="contactId">ID của liên hệ</param>
    /// <param name="avatarThumbnailBytes">Dữ liệu hình ảnh thumbnail</param>
    public void UpdateAvatarOnly(Guid contactId, byte[] avatarThumbnailBytes)
    {
        try
        {
            using var context = CreateNewContext();
            
            // Tìm entity hiện tại
            var existingEntity = context.BusinessPartnerContacts.FirstOrDefault(c => c.Id == contactId);
            if (existingEntity == null)
            {
                throw new DataAccessException($"Không tìm thấy BusinessPartnerContact với ID: {contactId}");
            }

            // Cập nhật chỉ avatar thumbnail
            existingEntity.AvatarThumbnailData = avatarThumbnailBytes != null ? new Binary(avatarThumbnailBytes) : null;
            
            // Submit changes
            context.SubmitChanges();
            
            _logger.Info($"Đã cập nhật avatar thumbnail cho BusinessPartnerContact: {contactId}");
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi cập nhật avatar thumbnail: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi cập nhật avatar thumbnail: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật avatar thumbnail: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi cập nhật avatar thumbnail: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Xóa chỉ avatar của BusinessPartnerContact (xóa cả metadata và thumbnail)
    /// </summary>
    /// <param name="contactId">ID của liên hệ</param>
    public void DeleteAvatarOnly(Guid contactId)
    {
        try
        {
            using var context = CreateNewContext();
            
            // Tìm entity hiện tại
            var existingEntity = context.BusinessPartnerContacts.FirstOrDefault(c => c.Id == contactId);
            if (existingEntity == null)
            {
                throw new DataAccessException($"Không tìm thấy BusinessPartnerContact với ID: {contactId}");
            }

            // Xóa avatar metadata và thumbnail
            existingEntity.AvatarFileName = null;
            existingEntity.AvatarRelativePath = null;
            existingEntity.AvatarFullPath = null;
            existingEntity.AvatarStorageType = null;
            existingEntity.AvatarFileSize = null;
            existingEntity.AvatarChecksum = null;
            existingEntity.AvatarThumbnailData = null;
            
            // Submit changes
            context.SubmitChanges();
            
            _logger.Info($"Đã xóa avatar (metadata và thumbnail) cho BusinessPartnerContact: {contactId}");
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi xóa avatar: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi xóa avatar: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa avatar: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi xóa avatar: {ex.Message}", ex);
        }
    }

    #endregion
}
