using Dal.Connection;
using Dal.DataAccess.Implementations.Inventory.InventoryManagement;
using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using DTO.Inventory.InventoryManagement;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;

namespace Bll.Inventory.InventoryManagement
{
    /// <summary>
    /// Business Logic Layer cho InventoryBalance
    /// Quản lý tồn kho theo tháng, bao gồm tính toán, khóa, xác thực và phê duyệt
    /// </summary>
    public class InventoryBalanceBll
    {
        #region Fields

        private IInventoryBalanceRepository _dataAccess;
        private readonly ILogger _logger;
        private readonly object _lockObject = new();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public InventoryBalanceBll()
        {
            _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo Repository (lazy initialization)
        /// </summary>
        private IInventoryBalanceRepository GetDataAccess()
        {
            if (_dataAccess == null)
            {
                lock (_lockObject)
                {
                    if (_dataAccess == null)
                    {
                        try
                        {
                            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
                            if (string.IsNullOrEmpty(globalConnectionString))
                            {
                                throw new InvalidOperationException(
                                    "Không có global connection string. Ứng dụng chưa được khởi tạo hoặc chưa sẵn sàng.");
                            }

                            _dataAccess = new InventoryBalanceRepository(globalConnectionString);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Lỗi khởi tạo InventoryBalanceRepository: {ex.Message}", ex);
                            throw;
                        }
                    }
                }
            }

            return _dataAccess;
        }

        /// <summary>
        /// Tính toán ClosingBalance từ OpeningBalance, TotalInQty, TotalOutQty
        /// </summary>
        private decimal CalculateClosingBalance(decimal openingBalance, decimal totalInQty, decimal totalOutQty)
        {
            return openingBalance + totalInQty - totalOutQty;
        }

        /// <summary>
        /// Tính toán ClosingValue từ OpeningValue, TotalInValue, TotalOutValue
        /// </summary>
        private decimal? CalculateClosingValue(decimal? openingValue, decimal? totalInValue, decimal? totalOutValue)
        {
            if (!openingValue.HasValue && !totalInValue.HasValue && !totalOutValue.HasValue)
                return null;

            var opening = openingValue ?? 0;
            var totalIn = totalInValue ?? 0;
            var totalOut = totalOutValue ?? 0;

            return opening + totalIn - totalOut;
        }

        /// <summary>
        /// Tính toán TotalInAmountIncludedVat từ TotalInValue và TotalInVatAmount
        /// </summary>
        private decimal? CalculateTotalInAmountIncludedVat(decimal? totalInValue, decimal? totalInVatAmount)
        {
            if (!totalInValue.HasValue && !totalInVatAmount.HasValue)
                return null;

            return (totalInValue ?? 0) + (totalInVatAmount ?? 0);
        }

        /// <summary>
        /// Tính toán TotalOutAmountIncludedVat từ TotalOutValue và TotalOutVatAmount
        /// </summary>
        private decimal? CalculateTotalOutAmountIncludedVat(decimal? totalOutValue, decimal? totalOutVatAmount)
        {
            if (!totalOutValue.HasValue && !totalOutVatAmount.HasValue)
                return null;

            return (totalOutValue ?? 0) + (totalOutVatAmount ?? 0);
        }

        /// <summary>
        /// Validate tồn kho trước khi lưu
        /// </summary>
        private void ValidateInventoryBalance(InventoryBalanceDto balance)
        {
            if (balance == null)
                throw new ArgumentNullException(nameof(balance));

            if (balance.WarehouseId == Guid.Empty)
                throw new ArgumentException("WarehouseId không được để trống", nameof(balance));

            if (balance.ProductVariantId == Guid.Empty)
                throw new ArgumentException("ProductVariantId không được để trống", nameof(balance));

            if (balance.PeriodYear < 2000 || balance.PeriodYear > 9999)
                throw new ArgumentException($"PeriodYear phải trong khoảng 2000-9999, giá trị hiện tại: {balance.PeriodYear}", nameof(balance));

            if (balance.PeriodMonth < 1 || balance.PeriodMonth > 12)
                throw new ArgumentException($"PeriodMonth phải trong khoảng 1-12, giá trị hiện tại: {balance.PeriodMonth}", nameof(balance));

            var calculatedClosingBalance = CalculateClosingBalance(balance.OpeningBalance, balance.TotalInQty, balance.TotalOutQty);
            if (Math.Abs(balance.ClosingBalance - calculatedClosingBalance) > 0.01m)
            {
                throw new ArgumentException(
                    $"ClosingBalance không khớp với công thức tính toán. " +
                    $"Giá trị hiện tại: {balance.ClosingBalance}, " +
                    $"Giá trị tính toán: {calculatedClosingBalance} " +
                    $"(OpeningBalance={balance.OpeningBalance} + TotalInQty={balance.TotalInQty} - TotalOutQty={balance.TotalOutQty})",
                    nameof(balance));
            }

            if (balance.TotalInAmountIncludedVat.HasValue)
            {
                var calculatedTotalInAmountIncludedVat = CalculateTotalInAmountIncludedVat(balance.TotalInValue, balance.TotalInVatAmount);
                if (calculatedTotalInAmountIncludedVat.HasValue && 
                    Math.Abs(balance.TotalInAmountIncludedVat.Value - calculatedTotalInAmountIncludedVat.Value) > 0.01m)
                {
                    throw new ArgumentException(
                        $"TotalInAmountIncludedVat không khớp với công thức tính toán. " +
                        $"Giá trị hiện tại: {balance.TotalInAmountIncludedVat}, " +
                        $"Giá trị tính toán: {calculatedTotalInAmountIncludedVat}",
                        nameof(balance));
                }
            }

            if (balance.TotalOutAmountIncludedVat.HasValue)
            {
                var calculatedTotalOutAmountIncludedVat = CalculateTotalOutAmountIncludedVat(balance.TotalOutValue, balance.TotalOutVatAmount);
                if (calculatedTotalOutAmountIncludedVat.HasValue && 
                    Math.Abs(balance.TotalOutAmountIncludedVat.Value - calculatedTotalOutAmountIncludedVat.Value) > 0.01m)
                {
                    throw new ArgumentException(
                        $"TotalOutAmountIncludedVat không khớp với công thức tính toán. " +
                        $"Giá trị hiện tại: {balance.TotalOutAmountIncludedVat}, " +
                        $"Giá trị tính toán: {calculatedTotalOutAmountIncludedVat}",
                        nameof(balance));
                }
            }
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Lấy tồn kho theo ID
        /// </summary>
        public InventoryBalanceDto GetById(Guid id)
        {
            try
            {
                return GetDataAccess().GetById(id);
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi lấy tồn kho theo ID '{id}': {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy tồn kho theo kho, sản phẩm và kỳ
        /// </summary>
        public InventoryBalanceDto GetByPeriod(Guid warehouseId, Guid productVariantId, int periodYear, int periodMonth)
        {
            try
            {
                return GetDataAccess().GetByPeriod(warehouseId, productVariantId, periodYear, periodMonth);
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi lấy tồn kho theo kỳ: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách tồn kho theo kho
        /// </summary>
        public List<InventoryBalanceDto> GetByWarehouseId(Guid warehouseId)
        {
            try
            {
                return GetDataAccess().GetByWarehouseId(warehouseId);
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi lấy danh sách tồn kho theo kho: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách tồn kho theo sản phẩm
        /// </summary>
        public List<InventoryBalanceDto> GetByProductVariantId(Guid productVariantId)
        {
            try
            {
                return GetDataAccess().GetByProductVariantId(productVariantId);
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi lấy danh sách tồn kho theo sản phẩm: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách tồn kho theo kỳ
        /// </summary>
        public List<InventoryBalanceDto> GetByPeriod(int periodYear, int periodMonth)
        {
            try
            {
                return GetDataAccess().GetByPeriod(periodYear, periodMonth);
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi lấy danh sách tồn kho theo kỳ: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Query tồn kho theo nhiều tiêu chí
        /// </summary>
        public List<InventoryBalanceDto> QueryBalances(
            Guid? warehouseId = null,
            Guid? productVariantId = null,
            int? periodYear = null,
            int? periodMonth = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            bool? isLocked = null,
            bool? isVerified = null,
            bool? isApproved = null,
            int? status = null)
        {
            try
            {
                return GetDataAccess().QueryBalances(
                    warehouseId, productVariantId, periodYear, periodMonth,
                    fromDate, toDate, isLocked, isVerified, isApproved, status);
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi query tồn kho: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách tồn kho cần xác thực (IsVerified = false)
        /// </summary>
        public List<InventoryBalanceDto> GetUnverifiedBalances()
        {
            try
            {
                return GetDataAccess().GetUnverifiedBalances();
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi lấy danh sách tồn kho cần xác thực: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách tồn kho cần phê duyệt (IsVerified = true, IsApproved = false)
        /// </summary>
        public List<InventoryBalanceDto> GetUnapprovedBalances()
        {
            try
            {
                return GetDataAccess().GetUnapprovedBalances();
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi lấy danh sách tồn kho cần phê duyệt: {ex.Message}", ex);
                throw;
            }
        }

        #endregion

        #region ========== XỬ LÝ DỮ LIỆU ==========

        /// <summary>
        /// Thêm mới tồn kho
        /// Tự động tính toán ClosingBalance và các giá trị liên quan
        /// </summary>
        public InventoryBalanceDto Insert(InventoryBalanceDto inventoryBalance)
        {
            try
            {
                if (inventoryBalance == null)
                    throw new ArgumentNullException(nameof(inventoryBalance));

                _logger.Debug("Insert: Bắt đầu thêm mới tồn kho, WarehouseId={0}, ProductVariantId={1}, Period={2}/{3}",
                    inventoryBalance.WarehouseId, inventoryBalance.ProductVariantId, 
                    inventoryBalance.PeriodYear, inventoryBalance.PeriodMonth);

                // Tính toán các giá trị tự động
                inventoryBalance.ClosingBalance = CalculateClosingBalance(
                    inventoryBalance.OpeningBalance, 
                    inventoryBalance.TotalInQty, 
                    inventoryBalance.TotalOutQty);

                inventoryBalance.ClosingValue = CalculateClosingValue(
                    inventoryBalance.OpeningValue,
                    inventoryBalance.TotalInValue,
                    inventoryBalance.TotalOutValue);

                inventoryBalance.TotalInAmountIncludedVat = CalculateTotalInAmountIncludedVat(
                    inventoryBalance.TotalInValue,
                    inventoryBalance.TotalInVatAmount);

                inventoryBalance.TotalOutAmountIncludedVat = CalculateTotalOutAmountIncludedVat(
                    inventoryBalance.TotalOutValue,
                    inventoryBalance.TotalOutVatAmount);

                // Validate
                ValidateInventoryBalance(inventoryBalance);

                // Lấy thông tin user hiện tại
                var currentUser = Common.ApplicationSystemUtils.GetCurrentUser();
                var currentUserId = currentUser?.Id ?? Guid.Empty;

                // Thiết lập audit fields
                inventoryBalance.Id = Guid.NewGuid();
                inventoryBalance.CreateDate = DateTime.Now;
                inventoryBalance.CreateBy = currentUserId;
                inventoryBalance.IsActive = true;
                inventoryBalance.IsDeleted = false;
                inventoryBalance.Status = 0; // Draft

                // Lưu vào database
                var id = GetDataAccess().Insert(inventoryBalance);
                inventoryBalance.Id = id;

                _logger.Info("Insert: Đã thêm mới tồn kho, Id={0}, WarehouseId={1}, ProductVariantId={2}, Period={3}/{4}",
                    inventoryBalance.Id, inventoryBalance.WarehouseId, inventoryBalance.ProductVariantId,
                    inventoryBalance.PeriodYear, inventoryBalance.PeriodMonth);

                return inventoryBalance;
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi thêm mới tồn kho: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Cập nhật tồn kho
        /// Tự động tính toán ClosingBalance và các giá trị liên quan
        /// </summary>
        public InventoryBalanceDto Update(InventoryBalanceDto inventoryBalance)
        {
            try
            {
                if (inventoryBalance == null)
                    throw new ArgumentNullException(nameof(inventoryBalance));

                _logger.Debug("Update: Bắt đầu cập nhật tồn kho, Id={0}, WarehouseId={1}, ProductVariantId={2}, Period={3}/{4}",
                    inventoryBalance.Id, inventoryBalance.WarehouseId, inventoryBalance.ProductVariantId, 
                    inventoryBalance.PeriodYear, inventoryBalance.PeriodMonth);

                // Tính toán các giá trị tự động
                inventoryBalance.ClosingBalance = CalculateClosingBalance(
                    inventoryBalance.OpeningBalance, 
                    inventoryBalance.TotalInQty, 
                    inventoryBalance.TotalOutQty);

                inventoryBalance.ClosingValue = CalculateClosingValue(
                    inventoryBalance.OpeningValue,
                    inventoryBalance.TotalInValue,
                    inventoryBalance.TotalOutValue);

                inventoryBalance.TotalInAmountIncludedVat = CalculateTotalInAmountIncludedVat(
                    inventoryBalance.TotalInValue,
                    inventoryBalance.TotalInVatAmount);

                inventoryBalance.TotalOutAmountIncludedVat = CalculateTotalOutAmountIncludedVat(
                    inventoryBalance.TotalOutValue,
                    inventoryBalance.TotalOutVatAmount);

                // Validate
                ValidateInventoryBalance(inventoryBalance);

                // Lấy thông tin user hiện tại
                var currentUser = Common.ApplicationSystemUtils.GetCurrentUser();
                var currentUserId = currentUser?.Id ?? Guid.Empty;

                // Thiết lập audit fields
                inventoryBalance.ModifiedDate = DateTime.Now;
                inventoryBalance.ModifiedBy = currentUserId;

                // Lưu vào database
                GetDataAccess().Update(inventoryBalance);

                _logger.Info("Update: Đã cập nhật tồn kho, Id={0}, WarehouseId={1}, ProductVariantId={2}, Period={3}/{4}",
                    inventoryBalance.Id, inventoryBalance.WarehouseId, inventoryBalance.ProductVariantId,
                    inventoryBalance.PeriodYear, inventoryBalance.PeriodMonth);

                return inventoryBalance;
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi cập nhật tồn kho: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Xóa tồn kho
        /// </summary>
        public void Delete(Guid id)
        {
            try
            {
                // Kiểm tra xem tồn kho có đang bị khóa không
                var balance = GetDataAccess().GetById(id);
                if (balance != null)
                {
                    if (balance.IsLocked)
                    {
                        throw new InvalidOperationException(
                            $"Không thể xóa tồn kho đang bị khóa. " +
                            $"Tồn kho ID: {id}, Period: {balance.PeriodYear}/{balance.PeriodMonth}");
                    }

                    if (balance.IsApproved)
                    {
                        throw new InvalidOperationException(
                            $"Không thể xóa tồn kho đã được phê duyệt. " +
                            $"Tồn kho ID: {id}, Period: {balance.PeriodYear}/{balance.PeriodMonth}");
                    }

                    GetDataAccess().Delete(id);

                    _logger.Info("Delete: Đã xóa tồn kho, Id={0}", id);
                }
                else
                {
                    throw new InvalidOperationException($"Không tìm thấy tồn kho với ID: {id}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi xóa tồn kho '{id}': {ex.Message}", ex);
                throw;
            }
        }

        #endregion

        #region ========== QUẢN LÝ TRẠNG THÁI ==========

        /// <summary>
        /// Khóa tồn kho (không cho phép chỉnh sửa)
        /// </summary>
        public void LockBalance(Guid id, string lockReason = null)
        {
            try
            {
                var currentUser = Common.ApplicationSystemUtils.GetCurrentUser();
                var lockedBy = currentUser?.Id ?? Guid.Empty;

                if (lockedBy == Guid.Empty)
                {
                    throw new InvalidOperationException("Không thể xác định người dùng hiện tại để khóa tồn kho");
                }

                GetDataAccess().UpdateLockStatus(id, true, lockedBy, lockReason);

                _logger.Info("LockBalance: Đã khóa tồn kho, Id={0}, LockedBy={1}, LockReason={2}", 
                    id, lockedBy, lockReason ?? "N/A");
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi khóa tồn kho '{id}': {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Mở khóa tồn kho (cho phép chỉnh sửa)
        /// </summary>
        public void UnlockBalance(Guid id)
        {
            try
            {
                var currentUser = Common.ApplicationSystemUtils.GetCurrentUser();
                var unlockedBy = currentUser?.Id ?? Guid.Empty;

                if (unlockedBy == Guid.Empty)
                {
                    throw new InvalidOperationException("Không thể xác định người dùng hiện tại để mở khóa tồn kho");
                }

                GetDataAccess().UpdateLockStatus(id, false, unlockedBy, null);

                _logger.Info("UnlockBalance: Đã mở khóa tồn kho, Id={0}, UnlockedBy={1}", id, unlockedBy);
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi mở khóa tồn kho '{id}': {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Xác thực tồn kho
        /// </summary>
        public void VerifyBalance(Guid id, string verificationNotes = null)
        {
            try
            {
                var currentUser = Common.ApplicationSystemUtils.GetCurrentUser();
                var verifiedBy = currentUser?.Id ?? Guid.Empty;

                if (verifiedBy == Guid.Empty)
                {
                    throw new InvalidOperationException("Không thể xác định người dùng hiện tại để xác thực tồn kho");
                }

                GetDataAccess().UpdateVerificationStatus(id, true, verifiedBy, verificationNotes);

                _logger.Info("VerifyBalance: Đã xác thực tồn kho, Id={0}, VerifiedBy={1}, VerificationNotes={2}", 
                    id, verifiedBy, verificationNotes ?? "N/A");
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi xác thực tồn kho '{id}': {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Hủy xác thực tồn kho
        /// </summary>
        public void UnverifyBalance(Guid id, string verificationNotes = null)
        {
            try
            {
                var currentUser = Common.ApplicationSystemUtils.GetCurrentUser();
                var unverifiedBy = currentUser?.Id ?? Guid.Empty;

                if (unverifiedBy == Guid.Empty)
                {
                    throw new InvalidOperationException("Không thể xác định người dùng hiện tại để hủy xác thực tồn kho");
                }

                GetDataAccess().UpdateVerificationStatus(id, false, unverifiedBy, verificationNotes);

                _logger.Info("UnverifyBalance: Đã hủy xác thực tồn kho, Id={0}, UnverifiedBy={1}, VerificationNotes={2}", 
                    id, unverifiedBy, verificationNotes ?? "N/A");
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi hủy xác thực tồn kho '{id}': {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Phê duyệt tồn kho
        /// </summary>
        public void ApproveBalance(Guid id, string approvalNotes = null)
        {
            try
            {
                var currentUser = Common.ApplicationSystemUtils.GetCurrentUser();
                var approvedBy = currentUser?.Id ?? Guid.Empty;

                if (approvedBy == Guid.Empty)
                {
                    throw new InvalidOperationException("Không thể xác định người dùng hiện tại để phê duyệt tồn kho");
                }

                // Kiểm tra xem tồn kho đã được xác thực chưa
                var balance = GetDataAccess().GetById(id);
                if (balance != null)
                {
                    if (!balance.IsVerified)
                    {
                        throw new InvalidOperationException(
                            $"Không thể phê duyệt tồn kho chưa được xác thực. " +
                            $"Tồn kho ID: {id}, Period: {balance.PeriodYear}/{balance.PeriodMonth}");
                    }

                    GetDataAccess().UpdateApprovalStatus(id, true, approvedBy, approvalNotes);

                    _logger.Info("ApproveBalance: Đã phê duyệt tồn kho, Id={0}, ApprovedBy={1}, ApprovalNotes={2}",
                        id, approvedBy, approvalNotes ?? "N/A");
                }
                else
                {
                    throw new InvalidOperationException($"Không tìm thấy tồn kho với ID: {id}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi phê duyệt tồn kho '{id}': {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Hủy phê duyệt tồn kho
        /// </summary>
        public void RejectBalance(Guid id, string approvalNotes = null)
        {
            try
            {
                var currentUser = Common.ApplicationSystemUtils.GetCurrentUser();
                var rejectedBy = currentUser?.Id ?? Guid.Empty;

                if (rejectedBy == Guid.Empty)
                {
                    throw new InvalidOperationException("Không thể xác định người dùng hiện tại để hủy phê duyệt tồn kho");
                }

                GetDataAccess().UpdateApprovalStatus(id, false, rejectedBy, approvalNotes);

                _logger.Info("RejectBalance: Đã hủy phê duyệt tồn kho, Id={0}, RejectedBy={1}, ApprovalNotes={2}", 
                    id, rejectedBy, approvalNotes ?? "N/A");
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi hủy phê duyệt tồn kho '{id}': {ex.Message}", ex);
                throw;
            }
        }

        #endregion

        #region ========== TÍNH TOÁN VÀ KẾT CHUYỂN ==========

        /// <summary>
        /// Tính lại tổng kết tồn kho cho kỳ được chỉ định
        /// Tính toán từ StockInOutDetail và cập nhật lại InventoryBalance
        /// </summary>
        /// <param name="periodYear">Năm kỳ</param>
        /// <param name="periodMonth">Tháng kỳ (1-12)</param>
        /// <returns>Số lượng tồn kho đã được cập nhật</returns>
        public int RecalculateSummary(int periodYear, int periodMonth)
        {
            try
            {
                _logger.Info("RecalculateSummary: Bắt đầu tính lại tổng kết cho kỳ {0}/{1:D2}", periodYear, periodMonth);

                if (periodYear < 2000 || periodYear > 9999)
                    throw new ArgumentException($"PeriodYear phải trong khoảng 2000-9999, giá trị hiện tại: {periodYear}");
                
                if (periodMonth < 1 || periodMonth > 12)
                    throw new ArgumentException($"PeriodMonth phải trong khoảng 1-12, giá trị hiện tại: {periodMonth}");

                var updatedCount = GetDataAccess().RecalculateSummary(periodYear, periodMonth);

                _logger.Info("RecalculateSummary: Đã tính lại tổng kết cho {0} tồn kho trong kỳ {1}/{2:D2}", updatedCount, periodYear, periodMonth);
                return updatedCount;
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi tính lại tổng kết cho kỳ {periodYear}/{periodMonth:D2}: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Kết chuyển dữ liệu tồn kho từ kỳ hiện tại sang kỳ tiếp theo
        /// Tạo tồn kho mới cho kỳ tiếp theo với OpeningBalance = ClosingBalance của kỳ hiện tại
        /// </summary>
        /// <param name="fromPeriodYear">Năm kỳ nguồn</param>
        /// <param name="fromPeriodMonth">Tháng kỳ nguồn (1-12)</param>
        /// <param name="overwriteExisting">Nếu true, ghi đè dữ liệu đã tồn tại ở kỳ đích. Nếu false, báo lỗi nếu đã có dữ liệu</param>
        /// <returns>Số lượng tồn kho đã được kết chuyển</returns>
        public int ForwardBalance(int fromPeriodYear, int fromPeriodMonth, bool overwriteExisting = false)
        {
            try
            {
                _logger.Info("ForwardBalance: Bắt đầu kết chuyển tồn kho từ kỳ {0}/{1:D2}", fromPeriodYear, fromPeriodMonth);

                if (fromPeriodYear < 2000 || fromPeriodYear > 9999)
                    throw new ArgumentException($"PeriodYear phải trong khoảng 2000-9999, giá trị hiện tại: {fromPeriodYear}");
                
                if (fromPeriodMonth < 1 || fromPeriodMonth > 12)
                    throw new ArgumentException($"PeriodMonth phải trong khoảng 1-12, giá trị hiện tại: {fromPeriodMonth}");

                var forwardedCount = GetDataAccess().ForwardBalance(fromPeriodYear, fromPeriodMonth, overwriteExisting);

                var toPeriodMonth = fromPeriodMonth == 12 ? 1 : fromPeriodMonth + 1;
                var toPeriodYear = fromPeriodMonth == 12 ? fromPeriodYear + 1 : fromPeriodYear;

                _logger.Info("ForwardBalance: Đã kết chuyển {0} tồn kho từ kỳ {1}/{2:D2} sang kỳ {3}/{4:D2}", 
                    forwardedCount, fromPeriodYear, fromPeriodMonth, toPeriodYear, toPeriodMonth);
                
                return forwardedCount;
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi kết chuyển tồn kho từ kỳ {fromPeriodYear}/{fromPeriodMonth:D2}: {ex.Message}", ex);
                throw;
            }
        }

        #endregion
    }
}
