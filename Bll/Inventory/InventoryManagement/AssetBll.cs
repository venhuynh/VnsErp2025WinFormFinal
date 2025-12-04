using Dal.Connection;
using Dal.DataAccess.Implementations.Inventory.InventoryManagement;
using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;

namespace Bll.Inventory.InventoryManagement;

/// <summary>
/// Business Logic Layer cho Asset
/// Quản lý tài sản, bao gồm tính toán khấu hao và giá trị hiện tại
/// </summary>
public class AssetBll
{
    #region Fields

    private IAssetRepository _dataAccess;
    private readonly ILogger _logger;
    private readonly object _lockObject = new();

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor mặc định
    /// </summary>
    public AssetBll()
    {
        _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Lấy hoặc khởi tạo Repository (lazy initialization)
    /// </summary>
    private IAssetRepository GetDataAccess()
    {
        if (_dataAccess == null)
        {
            lock (_lockObject)
            {
                if (_dataAccess == null)
                {
                    try
                    {
                        // Sử dụng global connection string từ ApplicationStartupManager
                        var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
                        if (string.IsNullOrEmpty(globalConnectionString))
                        {
                            throw new InvalidOperationException(
                                "Không có global connection string. Ứng dụng chưa được khởi tạo hoặc chưa sẵn sàng.");
                        }

                        _dataAccess = new AssetRepository(globalConnectionString);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Lỗi khởi tạo AssetRepository: {ex.Message}", ex);
                        throw;
                    }
                }
            }
        }

        return _dataAccess;
    }

    /// <summary>
    /// Tính toán CurrentValue từ PurchasePrice và AccumulatedDepreciation
    /// </summary>
    private decimal? CalculateCurrentValue(decimal purchasePrice, decimal accumulatedDepreciation)
    {
        var currentValue = purchasePrice - accumulatedDepreciation;
        return currentValue >= 0 ? currentValue : 0;
    }

    /// <summary>
    /// Validate tài sản trước khi lưu
    /// </summary>
    private void ValidateAsset(Asset asset)
    {
        if (asset == null)
            throw new ArgumentNullException(nameof(asset));

        if (string.IsNullOrWhiteSpace(asset.AssetCode))
            throw new ArgumentException(@"AssetCode không được để trống", nameof(asset));

        if (string.IsNullOrWhiteSpace(asset.AssetName))
            throw new ArgumentException(@"AssetName không được để trống", nameof(asset));

        if (asset.CompanyId == Guid.Empty)
            throw new ArgumentException(@"CompanyId không được để trống", nameof(asset));

        if (asset.PurchasePrice < 0)
            throw new ArgumentException($@"PurchasePrice phải >= 0, giá trị hiện tại: {asset.PurchasePrice}", nameof(asset));

        if (asset.AccumulatedDepreciation < 0)
            throw new ArgumentException($@"AccumulatedDepreciation phải >= 0, giá trị hiện tại: {asset.AccumulatedDepreciation}", nameof(asset));

        // Validate AssetType
        if (asset.AssetType < 0 || asset.AssetType > 2)
            throw new ArgumentException($@"AssetType phải trong khoảng 0-2, giá trị hiện tại: {asset.AssetType}", nameof(asset));

        // Validate AssetCategory
        if (asset.AssetCategory < 0 || asset.AssetCategory > 4)
            throw new ArgumentException($@"AssetCategory phải trong khoảng 0-4, giá trị hiện tại: {asset.AssetCategory}", nameof(asset));

        // Validate DepreciationMethod
        if (asset.DepreciationMethod is < 0 or > 2)
            throw new ArgumentException($@"DepreciationMethod phải trong khoảng 0-2, giá trị hiện tại: {asset.DepreciationMethod}", nameof(asset));

        // Validate Status
        if (asset.Status < 0 || asset.Status > 4)
            throw new ArgumentException($@"Status phải trong khoảng 0-4, giá trị hiện tại: {asset.Status}", nameof(asset));

        // Validate Condition
        if (asset.Condition < 0 || asset.Condition > 4)
            throw new ArgumentException($@"Condition phải trong khoảng 0-4, giá trị hiện tại: {asset.Condition}", nameof(asset));

        // Validate CurrentValue nếu có giá trị
        if (asset.CurrentValue.HasValue)
        {
            var calculatedCurrentValue = CalculateCurrentValue(asset.PurchasePrice, asset.AccumulatedDepreciation);
            if (calculatedCurrentValue.HasValue && 
                Math.Abs(asset.CurrentValue.Value - calculatedCurrentValue.Value) > 0.01m)
            {
                // Chỉ cảnh báo, không throw exception vì có thể người dùng muốn set giá trị thủ công
                _logger.Warning("CurrentValue không khớp với công thức tính toán. " +
                              $"Giá trị hiện tại: {asset.CurrentValue}, " +
                              $"Giá trị tính toán: {calculatedCurrentValue} " +
                              $"(PurchasePrice={asset.PurchasePrice} - AccumulatedDepreciation={asset.AccumulatedDepreciation})");
            }
        }
    }

    #endregion

    #region Business Methods

    /// <summary>
    /// Lưu hoặc cập nhật tài sản
    /// Tự động tính toán CurrentValue từ PurchasePrice và AccumulatedDepreciation
    /// </summary>
    public Asset SaveOrUpdate(Asset asset)
    {
        try
        {
            if (asset == null)
                throw new ArgumentNullException(nameof(asset));

            _logger.Debug("SaveOrUpdate: Bắt đầu lưu tài sản, AssetCode={0}, AssetName={1}",
                asset.AssetCode, asset.AssetName);

            // Tính toán CurrentValue tự động
            asset.CurrentValue = CalculateCurrentValue(asset.PurchasePrice, asset.AccumulatedDepreciation);

            // Validate
            ValidateAsset(asset);

            // Lấy thông tin user hiện tại
            var currentUser = Common.ApplicationSystemUtils.GetCurrentUser();
            var currentUserId = currentUser?.Id;

            // Thiết lập audit fields
            if (asset.Id == Guid.Empty)
            {
                // Thêm mới
                asset.Id = Guid.NewGuid();
                asset.CreateDate = DateTime.Now;
                asset.CreateBy = currentUserId;
                asset.IsActive = true;
                asset.IsDeleted = false;
            }
            else
            {
                // Cập nhật
                asset.ModifiedDate = DateTime.Now;
                asset.ModifiedBy = currentUserId;
            }

            // Lưu vào database
            GetDataAccess().SaveOrUpdate(asset);

            _logger.Info("SaveOrUpdate: Đã lưu tài sản, Id={0}, AssetCode={1}, AssetName={2}",
                asset.Id, asset.AssetCode, asset.AssetName);

            return asset;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lưu tài sản: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy tài sản theo ID
    /// </summary>
    public Asset GetById(Guid id)
    {
        return GetDataAccess().GetById(id);
    }

    /// <summary>
    /// Lấy tài sản theo mã tài sản
    /// </summary>
    public Asset GetByAssetCode(string assetCode)
    {
        if (string.IsNullOrWhiteSpace(assetCode))
            throw new ArgumentException(@"AssetCode không được để trống", nameof(assetCode));

        return GetDataAccess().GetByAssetCode(assetCode);
    }

    /// <summary>
    /// Lấy danh sách tài sản theo công ty
    /// </summary>
    public List<Asset> GetByCompanyId(Guid companyId)
    {
        if (companyId == Guid.Empty)
            throw new ArgumentException(@"CompanyId không được để trống", nameof(companyId));

        return GetDataAccess().GetByCompanyId(companyId);
    }

    /// <summary>
    /// Lấy danh sách tài sản theo chi nhánh
    /// </summary>
    public List<Asset> GetByBranchId(Guid branchId)
    {
        if (branchId == Guid.Empty)
            throw new ArgumentException(@"BranchId không được để trống", nameof(branchId));

        return GetDataAccess().GetByBranchId(branchId);
    }

    /// <summary>
    /// Lấy danh sách tài sản theo phòng ban
    /// </summary>
    public List<Asset> GetByDepartmentId(Guid departmentId)
    {
        if (departmentId == Guid.Empty)
            throw new ArgumentException(@"DepartmentId không được để trống", nameof(departmentId));

        return GetDataAccess().GetByDepartmentId(departmentId);
    }

    /// <summary>
    /// Lấy danh sách tài sản theo nhân viên phụ trách
    /// </summary>
    public List<Asset> GetByEmployeeId(Guid employeeId)
    {
        if (employeeId == Guid.Empty)
            throw new ArgumentException(@"EmployeeId không được để trống", nameof(employeeId));

        return GetDataAccess().GetByEmployeeId(employeeId);
    }

    /// <summary>
    /// Lấy danh sách tài sản theo sản phẩm
    /// </summary>
    public List<Asset> GetByProductVariantId(Guid productVariantId)
    {
        if (productVariantId == Guid.Empty)
            throw new ArgumentException(@"ProductVariantId không được để trống", nameof(productVariantId));

        return GetDataAccess().GetByProductVariantId(productVariantId);
    }

    /// <summary>
    /// Query tài sản theo nhiều tiêu chí
    /// </summary>
    public List<Asset> QueryAssets(
        Guid? companyId = null,
        Guid? branchId = null,
        Guid? departmentId = null,
        Guid? employeeId = null,
        Guid? productVariantId = null,
        int? assetType = null,
        int? assetCategory = null,
        int? status = null,
        int? condition = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        bool? isActive = null)
    {
        return GetDataAccess().QueryAssets(
            companyId, branchId, departmentId, employeeId, productVariantId,
            assetType, assetCategory, status, condition,
            fromDate, toDate, isActive);
    }

    /// <summary>
    /// Xóa tài sản
    /// </summary>
    public void Delete(Guid id)
    {
        try
        {
            // Kiểm tra xem tài sản có tồn tại không
            var asset = GetDataAccess().GetById(id);
            if (asset == null)
            {
                throw new InvalidOperationException($"Không tìm thấy tài sản với ID: {id}");
            }

            // Có thể thêm các kiểm tra business logic khác ở đây
            // Ví dụ: không cho xóa tài sản đang được sử dụng, đã thanh lý, v.v.

            var currentUser = Common.ApplicationSystemUtils.GetCurrentUser();
            var deletedBy = currentUser?.Id ?? Guid.Empty;

            GetDataAccess().Delete(id, deletedBy);

            _logger.Info("Delete: Đã xóa tài sản, Id={0}, AssetCode={1}, DeletedBy={2}", 
                id, asset.AssetCode, deletedBy);
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa tài sản '{id}': {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Cập nhật khấu hao lũy kế cho tài sản
    /// Tự động tính lại CurrentValue
    /// </summary>
    public Asset UpdateDepreciation(Guid id, decimal accumulatedDepreciation)
    {
        try
        {
            if (accumulatedDepreciation < 0)
                throw new ArgumentException($@"AccumulatedDepreciation phải >= 0, giá trị hiện tại: {accumulatedDepreciation}", nameof(accumulatedDepreciation));

            var asset = GetDataAccess().GetById(id);
            if (asset == null)
            {
                throw new InvalidOperationException($@"Không tìm thấy tài sản với ID: {id}");
            }

            asset.AccumulatedDepreciation = accumulatedDepreciation;
            asset.CurrentValue = CalculateCurrentValue(asset.PurchasePrice, accumulatedDepreciation);
            asset.ModifiedDate = DateTime.Now;
            asset.ModifiedBy = Common.ApplicationSystemUtils.GetCurrentUser()?.Id;

            GetDataAccess().SaveOrUpdate(asset);

            _logger.Info(@"UpdateDepreciation: Đã cập nhật khấu hao cho tài sản, Id={0}, AssetCode={1}, AccumulatedDepreciation={2}, CurrentValue={3}",
                id, asset.AssetCode, accumulatedDepreciation, asset.CurrentValue);

            return asset;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật khấu hao cho tài sản '{id}': {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Cập nhật trạng thái tài sản
    /// </summary>
    public Asset UpdateStatus(Guid id, int status)
    {
        try
        {
            if (status < 0 || status > 4)
                throw new ArgumentException($@"Status phải trong khoảng 0-4, giá trị hiện tại: {status}", nameof(status));

            var asset = GetDataAccess().GetById(id);
            if (asset == null)
            {
                throw new InvalidOperationException($@"Không tìm thấy tài sản với ID: {id}");
            }

            asset.Status = status;
            asset.ModifiedDate = DateTime.Now;
            asset.ModifiedBy = Common.ApplicationSystemUtils.GetCurrentUser()?.Id;

            GetDataAccess().SaveOrUpdate(asset);

            _logger.Info("UpdateStatus: Đã cập nhật trạng thái cho tài sản, Id={0}, AssetCode={1}, Status={2}",
                id, asset.AssetCode, status);

            return asset;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật trạng thái cho tài sản '{id}': {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Cập nhật tình trạng tài sản
    /// </summary>
    public Asset UpdateCondition(Guid id, int condition)
    {
        try
        {
            if (condition < 0 || condition > 4)
                throw new ArgumentException($@"Condition phải trong khoảng 0-4, giá trị hiện tại: {condition}", nameof(condition));

            var asset = GetDataAccess().GetById(id);
            if (asset == null)
            {
                throw new InvalidOperationException($"Không tìm thấy tài sản với ID: {id}");
            }

            asset.Condition = condition;
            asset.ModifiedDate = DateTime.Now;
            asset.ModifiedBy = Common.ApplicationSystemUtils.GetCurrentUser()?.Id;

            GetDataAccess().SaveOrUpdate(asset);

            _logger.Info("UpdateCondition: Đã cập nhật tình trạng cho tài sản, Id={0}, AssetCode={1}, Condition={2}",
                id, asset.AssetCode, condition);

            return asset;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật tình trạng cho tài sản '{id}': {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}

