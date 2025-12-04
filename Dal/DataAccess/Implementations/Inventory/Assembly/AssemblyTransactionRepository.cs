using Dal.DataAccess.Interfaces.Inventory.Assembly;
using Dal.DataContext;
using Logger;
using Logger.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.Inventory.Assembly;

/// <summary>
/// Repository implementation cho AssemblyTransaction (Lịch sử lắp ráp)
/// </summary>
public class AssemblyTransactionRepository : IAssemblyTransactionRepository
{
    #region Private Fields

    private readonly string _connectionString;
    private readonly CustomLogger _logger;

    #endregion

    #region Constructor

    public AssemblyTransactionRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("AssemblyTransactionRepository được khởi tạo");
    }

    #endregion

    #region Helper Methods

    private VnsErp2025DataContext CreateNewContext()
    {
        var context = new VnsErp2025DataContext(_connectionString);

        var loadOptions = new DataLoadOptions();
        loadOptions.LoadWith<AssemblyTransaction>(t => t.ProductVariant);
        loadOptions.LoadWith<ProductVariant>(v => v.ProductService);
        loadOptions.LoadWith<AssemblyTransaction>(t => t.StockInOutMaster); // StockInMaster
        loadOptions.LoadWith<AssemblyTransaction>(t => t.StockInOutMaster1); // StockOutMaster
        loadOptions.LoadWith<AssemblyTransaction>(t => t.CompanyBranch);
        context.LoadOptions = loadOptions;

        return context;
    }

    #endregion

    #region Save Operations

    public void Save(AssemblyTransaction transaction)
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));

        using var context = CreateNewContext();
        try
        {
            _logger.Debug("Save: Bắt đầu lưu AssemblyTransaction, Id={0}, ProductVariantId={1}, Quantity={2}",
                transaction.Id, transaction.ProductVariantId, transaction.Quantity);

            if (transaction.Id == Guid.Empty)
                transaction.Id = Guid.NewGuid();

            if (transaction.CreatedDate == default(DateTime))
                transaction.CreatedDate = DateTime.Now;

            context.AssemblyTransactions.InsertOnSubmit(transaction);
            context.SubmitChanges();

            _logger.Info("Save: Đã lưu AssemblyTransaction, Id={0}", transaction.Id);
        }
        catch (Exception ex)
        {
            _logger.Error($"Save: Lỗi lưu AssemblyTransaction: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region Query Operations

    public AssemblyTransaction GetById(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID không được để trống", nameof(id));

        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetById: Lấy AssemblyTransaction, Id={0}", id);

            var transaction = context.AssemblyTransactions.FirstOrDefault(x => x.Id == id);
            return transaction;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetById: Lỗi lấy AssemblyTransaction: {ex.Message}", ex);
            throw;
        }
    }

    public List<AssemblyTransaction> GetByProductVariantId(Guid productVariantId)
    {
        if (productVariantId == Guid.Empty)
            throw new ArgumentException("ProductVariantId không được để trống", nameof(productVariantId));

        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByProductVariantId: Lấy danh sách AssemblyTransaction, ProductVariantId={0}", productVariantId);

            var transactions = context.AssemblyTransactions
                .Where(x => x.ProductVariantId == productVariantId)
                .OrderByDescending(x => x.AssemblyDate)
                .ToList();

            _logger.Info("GetByProductVariantId: Đã lấy {0} transaction", transactions.Count);
            return transactions;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByProductVariantId: Lỗi lấy danh sách AssemblyTransaction: {ex.Message}", ex);
            throw;
        }
    }

    public List<AssemblyTransaction> GetByDateRange(DateTime fromDate, DateTime toDate)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByDateRange: Lấy danh sách AssemblyTransaction từ {0} đến {1}", fromDate, toDate);

            var transactions = context.AssemblyTransactions
                .Where(x => x.AssemblyDate >= fromDate && x.AssemblyDate <= toDate)
                .OrderByDescending(x => x.AssemblyDate)
                .ToList();

            _logger.Info("GetByDateRange: Đã lấy {0} transaction", transactions.Count);
            return transactions;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByDateRange: Lỗi lấy danh sách AssemblyTransaction: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}

