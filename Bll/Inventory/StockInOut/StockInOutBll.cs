using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bll.Inventory.InventoryManagement;
using Dal.Connection;
using Dal.DataAccess.Implementations.Inventory.StockIn;
using Dal.DataAccess.Interfaces.Inventory.StockIn;
using Dal.DataContext;
using DTO.DeviceAssetManagement;
using DTO.Inventory.InventoryManagement;
using DTO.Inventory.StockIn.NhapHangThuongMai;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

namespace Bll.Inventory.StockInOut
{
    /// <summary>
    /// Business Logic Layer cho StockIn (Phiếu nhập kho)
    /// </summary>
    public class StockInOutBll
    {
        #region Fields

        private IStockInRepository _dataAccess;
        private readonly ILogger _logger;
        private readonly object _lockObject = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public StockInOutBll()
        {
            _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo Repository (lazy initialization)
        /// </summary>
        private IStockInRepository GetDataAccess()
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

                            _dataAccess = new StockInRepository(globalConnectionString);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Lỗi khởi tạo StockInRepository: {ex.Message}", ex);
                            throw;
                        }
                    }
                }
            }

            return _dataAccess;
        }

        #endregion

        #region Save Operations

        /// <summary>
        /// Lưu phiếu nhập/xuất kho (master và detail) với DTO
        /// Method này nhận DTO để tránh lỗi tham chiếu khóa ngoại khi entity có navigation properties đã được load
        /// </summary>
        /// <param name="masterDto">DTO phiếu nhập/xuất kho (master) - có thể là bất kỳ loại Master DTO nào</param>
        /// <param name="detailEntities">Danh sách entity chi tiết phiếu nhập/xuất kho</param>
        /// <returns>ID phiếu nhập/xuất kho đã lưu</returns>
        public async Task<Guid> SaveAsync(object masterDto, List<StockInOutDetail> detailEntities)
        {
            if (masterDto == null)
                throw new ArgumentNullException(nameof(masterDto));

            if (detailEntities == null || detailEntities.Count == 0)
                throw new ArgumentException(@"Danh sách chi tiết không được để trống", nameof(detailEntities));

            try
            {
                _logger.Debug("SaveAsync (DTO): Bắt đầu lưu phiếu, MasterDtoType={0}, DetailCount={1}", 
                    masterDto.GetType().Name, detailEntities.Count);

                // Map DTO sang entity (không có navigation properties)
                var masterEntity = MapMasterDtoToEntity(masterDto);

                // Validate dữ liệu trước khi lưu
                ValidateBeforeSave(masterEntity, detailEntities);

                // Gọi method riêng biệt cho Create hoặc Update
                Guid savedMasterId;
                if (masterEntity.Id == Guid.Empty)
                {
                    // Tạo mới
                    savedMasterId = await GetDataAccess().CreateMasterAsync(masterEntity);
                }
                else
                {
                    // Cập nhật
                    savedMasterId = await GetDataAccess().UpdateMasterAsync(masterEntity);
                }

                // Sau đó lưu details qua SaveDetailsAsync
                await GetDataAccess().SaveDetailsAsync(savedMasterId, detailEntities, deleteExisting: true);

                _logger.Info("SaveAsync (DTO): Lưu phiếu thành công, MasterId={0}", savedMasterId);
                return savedMasterId;
            }
            catch (Exception ex)
            {
                _logger.Error($"SaveAsync (DTO): Lỗi lưu phiếu: {ex.Message}", ex);
                throw new Exception($"Lỗi lưu phiếu: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Map Master DTO sang StockInOutMaster entity
        /// Hỗ trợ nhiều loại Master DTO khác nhau
        /// </summary>
        private StockInOutMaster MapMasterDtoToEntity(object masterDto)
        {
            // Xử lý theo từng loại DTO
            switch (masterDto)
            {
                case DTO.Inventory.StockOut.XuatHangThuongMai.XuatHangThuongMaiMasterDto xuatHangDto:
                    return MapXuatHangThuongMaiDtoToEntity(xuatHangDto);
                
                case XuatHangThuongMaiMasterDto nhapHangDto:
                    return MapNhapHangThuongMaiDtoToEntity(nhapHangDto);
                
                default:
                    // Thử dùng reflection để map các property chung
                    return MapMasterDtoToEntityByReflection(masterDto);
            }
        }

        /// <summary>
        /// Map XuatHangThuongMaiMasterDto sang StockInOutMaster entity
        /// </summary>
        private StockInOutMaster MapXuatHangThuongMaiDtoToEntity(DTO.Inventory.StockOut.XuatHangThuongMai.XuatHangThuongMaiMasterDto dto)
        {
            var entity = new StockInOutMaster
            {
                Id = dto.Id,
                StockInOutDate = dto.StockOutDate,
                VocherNumber = dto.StockOutNumber,
                StockInOutType = (int)dto.LoaiNhapXuatKho,
                VoucherStatus = (int)dto.TrangThai,
                WarehouseId = dto.WarehouseId,
                PurchaseOrderId = dto.SalesOrderId, // Dùng PurchaseOrderId để lưu SalesOrderId
                PartnerSiteId = dto.CustomerId, // Dùng PartnerSiteId để lưu CustomerId
                Notes = dto.Notes ?? string.Empty,
                TotalQuantity = dto.TotalQuantity,
                TotalAmount = dto.TotalAmount,
                TotalVat = dto.TotalVat,
                TotalAmountIncludedVat = dto.TotalAmountIncludedVat,
                NguoiNhanHang = dto.NguoiNhanHang ?? string.Empty,
                NguoiGiaoHang = dto.NguoiGiaoHang ?? string.Empty
            };

            // Set DiscountAmount và TotalAmountAfterDiscount nếu DTO có các property này
            var dtoType = dto.GetType();
            var discountAmountProp = dtoType.GetProperty("DiscountAmount");
            if (discountAmountProp != null)
            {
                var discountValue = discountAmountProp.GetValue(dto);
                if (discountValue != null && discountValue is decimal discount)
                {
                    entity.DiscountAmount = discount;
                }
            }

            var totalAmountAfterDiscountProp = dtoType.GetProperty("TotalAmountAfterDiscount");
            if (totalAmountAfterDiscountProp != null)
            {
                var totalAfterDiscountValue = totalAmountAfterDiscountProp.GetValue(dto);
                if (totalAfterDiscountValue != null && totalAfterDiscountValue is decimal totalAfterDiscount)
                {
                    entity.TotalAmountAfterDiscount = totalAfterDiscount;
                }
            }

            return entity;
        }

        /// <summary>
        /// Map NhapHangThuongMaiMasterDto (XuatHangThuongMaiMasterDto trong namespace StockIn) sang StockInOutMaster entity
        /// </summary>
        private StockInOutMaster MapNhapHangThuongMaiDtoToEntity(XuatHangThuongMaiMasterDto dto)
        {
            return new StockInOutMaster
            {
                Id = dto.Id,
                StockInOutDate = dto.StockInDate,
                VocherNumber = dto.StockInNumber,
                StockInOutType = (int)dto.LoaiNhapXuatKho,
                VoucherStatus = (int)dto.TrangThai,
                WarehouseId = dto.WarehouseId,
                PurchaseOrderId = dto.PurchaseOrderId,
                PartnerSiteId = dto.SupplierId,
                Notes = dto.Notes ?? string.Empty,
                TotalQuantity = dto.TotalQuantity,
                TotalAmount = dto.TotalAmount,
                TotalVat = dto.TotalVat,
                TotalAmountIncludedVat = dto.TotalAmountIncludedVat,
                NguoiNhanHang = dto.NguoiNhanHang,
                NguoiGiaoHang = dto.NguoiGiaoHang,
                DiscountAmount = dto.DiscountAmount,
                TotalAmountAfterDiscount = dto.TotalAmountAfterDiscount
            };
        }

        /// <summary>
        /// Map Master DTO sang entity bằng reflection (fallback cho các DTO chưa được hỗ trợ cụ thể)
        /// </summary>
        private StockInOutMaster MapMasterDtoToEntityByReflection(object masterDto)
        {
            var entity = new StockInOutMaster();
            var dtoType = masterDto.GetType();
            var entityType = typeof(StockInOutMaster);

            // Map các property chung bằng reflection
            var commonMappings = new Dictionary<string, (string dtoProp, Func<object, object> converter)>
            {
                { "Id", ("Id", v => v) },
                { "StockInOutDate", ("StockInDate", v => v) },
                { "StockInOutDate", ("StockOutDate", v => v) },
                { "VocherNumber", ("StockInNumber", v => v) },
                { "VocherNumber", ("StockOutNumber", v => v) },
                { "StockInOutType", ("LoaiNhapXuatKho", v => (int)v) },
                { "VoucherStatus", ("TrangThai", v => (int)v) },
                { "WarehouseId", ("WarehouseId", v => v) },
                { "Notes", ("Notes", v => v ?? string.Empty) },
                { "TotalQuantity", ("TotalQuantity", v => v) },
                { "TotalAmount", ("TotalAmount", v => v) },
                { "TotalVat", ("TotalVat", v => v) },
                { "TotalAmountIncludedVat", ("TotalAmountIncludedVat", v => v) },
                { "NguoiNhanHang", ("NguoiNhanHang", v => v ?? string.Empty) },
                { "NguoiGiaoHang", ("NguoiGiaoHang", v => v ?? string.Empty) },
                { "DiscountAmount", ("DiscountAmount", v => v) },
                { "TotalAmountAfterDiscount", ("TotalAmountAfterDiscount", v => v) }
            };

            foreach (var mapping in commonMappings)
            {
                var entityProp = entityType.GetProperty(mapping.Key);
                if (entityProp == null) continue;

                // Thử các tên property DTO khác nhau
                var dtoPropNames = new[] { mapping.Value.dtoProp, mapping.Key };
                object value = null;

                foreach (var propName in dtoPropNames)
                {
                    var dtoProp = dtoType.GetProperty(propName);
                    if (dtoProp != null)
                    {
                        value = dtoProp.GetValue(masterDto);
                        if (value != null)
                        {
                            value = mapping.Value.converter(value);
                            entityProp.SetValue(entity, value);
                            break;
                        }
                    }
                }
            }

            // Map các property đặc biệt
            MapSpecialProperties(masterDto, entity);

            return entity;
        }

        /// <summary>
        /// Map các property đặc biệt (PurchaseOrderId, PartnerSiteId, etc.)
        /// </summary>
        private void MapSpecialProperties(object masterDto, StockInOutMaster entity)
        {
            var dtoType = masterDto.GetType();

            // PurchaseOrderId / SalesOrderId
            var purchaseOrderProp = dtoType.GetProperty("PurchaseOrderId") ?? dtoType.GetProperty("SalesOrderId");
            if (purchaseOrderProp != null)
            {
                var value = purchaseOrderProp.GetValue(masterDto);
                if (value != null && value is Guid guidValue && guidValue != Guid.Empty)
                {
                    entity.PurchaseOrderId = guidValue;
                }
            }

            // PartnerSiteId / SupplierId / CustomerId
            var partnerSiteProp = dtoType.GetProperty("PartnerSiteId") ?? 
                                 dtoType.GetProperty("SupplierId") ?? 
                                 dtoType.GetProperty("CustomerId");
            if (partnerSiteProp != null)
            {
                var value = partnerSiteProp.GetValue(masterDto);
                if (value != null && value is Guid guidValue && guidValue != Guid.Empty)
                {
                    entity.PartnerSiteId = guidValue;
                }
            }
        }

        /// <summary>
        /// Map XuatHangThuongMaiMasterDto sang StockInOutMaster entity (legacy method, kept for backward compatibility)
        /// Helper method để convert DTO sang entity (dùng cho các màn hình sử dụng XuatHangThuongMaiMasterDto)
        /// </summary>
        [Obsolete("Use SaveAsync(object masterDto, List<StockInOutDetail> detailEntities) instead")]
        public static StockInOutMaster MapMasterDtoToEntity(XuatHangThuongMaiMasterDto dto)
        {
            return new StockInOutMaster
            {
                Id = dto.Id,
                StockInOutDate = dto.StockInDate,
                VocherNumber = dto.StockInNumber,
                StockInOutType = (int)dto.LoaiNhapXuatKho, // Map enum to int
                VoucherStatus = (int)dto.TrangThai, // Map enum to int
                WarehouseId = dto.WarehouseId,
                PurchaseOrderId = dto.PurchaseOrderId,
                PartnerSiteId = dto.SupplierId,
                Notes = dto.Notes ?? string.Empty,
                TotalQuantity = dto.TotalQuantity,
                TotalAmount = dto.TotalAmount,
                TotalVat = dto.TotalVat,
                TotalAmountIncludedVat = dto.TotalAmountIncludedVat,
                NguoiNhanHang = dto.NguoiNhanHang,
                NguoiGiaoHang = dto.NguoiGiaoHang,
                DiscountAmount = dto.DiscountAmount,
                TotalAmountAfterDiscount = dto.TotalAmountAfterDiscount
            };
        }

        /// <summary>
        /// Validate dữ liệu trước khi lưu
        /// </summary>
        private void ValidateBeforeSave(StockInOutMaster masterEntity, List<StockInOutDetail> detailEntities)
        {
            // Validate WarehouseId - không được để trống hoặc Guid.Empty
            if (masterEntity.WarehouseId == Guid.Empty)
            {
                throw new ArgumentException("Vui lòng chọn kho. WarehouseId không được để trống.");
            }

            // Xác định loại phiếu (nhập hay xuất) từ StockInOutType
            var loaiNhapXuatKho = (LoaiNhapXuatKhoEnum)masterEntity.StockInOutType;
            var isStockIn = IsStockInType(loaiNhapXuatKho);
            var isStockOut = IsStockOutType(loaiNhapXuatKho);

            // Validate các detail
            for (int i = 0; i < detailEntities.Count; i++)
            {
                var detail = detailEntities[i];
                var lineNumber = i + 1;

                if (detail.ProductVariantId == Guid.Empty)
                {
                    throw new ArgumentException($"Dòng {lineNumber}: Vui lòng chọn hàng hóa. ProductVariantId không được để trống.");
                }

                // Tùy theo phiếu nhập hoặc phiếu xuất thì kiểm tra xem số lượng nhập hoặc xuất
                if (isStockIn)
                {
                    // Phiếu nhập: kiểm tra StockInQty phải lớn hơn 0
                    if (detail.StockInQty <= 0)
                    {
                        throw new ArgumentException($"Dòng {lineNumber}: Số lượng nhập phải lớn hơn 0.");
                    }
                }
                else if (isStockOut)
                {
                    // Phiếu xuất: kiểm tra StockOutQty phải lớn hơn 0
                    if (detail.StockOutQty <= 0)
                    {
                        throw new ArgumentException($"Dòng {lineNumber}: Số lượng xuất phải lớn hơn 0.");
                    }
                }
                else
                {
                    // Trường hợp Khac hoặc không xác định: kiểm tra cả hai
                    if (detail.StockInQty <= 0 && detail.StockOutQty <= 0)
                    {
                        throw new ArgumentException($"Dòng {lineNumber}: Phải có số lượng nhập hoặc số lượng xuất lớn hơn 0.");
                    }
                }
            }

            _logger.Debug("ValidateBeforeSave: Validation passed");
        }

        /// <summary>
        /// Kiểm tra xem loại nhập xuất kho có phải là loại nhập không
        /// </summary>
        /// <param name="loaiNhapXuatKho">Loại nhập xuất kho</param>
        /// <returns>True nếu là loại nhập, False nếu không</returns>
        private static bool IsStockInType(LoaiNhapXuatKhoEnum loaiNhapXuatKho)
        {
            return loaiNhapXuatKho == LoaiNhapXuatKhoEnum.NhapHangThuongMai ||
                   loaiNhapXuatKho == LoaiNhapXuatKhoEnum.NhapThietBiMuonThue ||
                   loaiNhapXuatKho == LoaiNhapXuatKhoEnum.NhapNoiBo ||
                   loaiNhapXuatKho == LoaiNhapXuatKhoEnum.NhapLuuChuyenKho ||
                   loaiNhapXuatKho == LoaiNhapXuatKhoEnum.NhapHangBaoHanh ||
                   loaiNhapXuatKho == LoaiNhapXuatKhoEnum.NhapSanPhamLapRap ||
                   loaiNhapXuatKho == LoaiNhapXuatKhoEnum.NhapLinhKienPhanRa;
        }

        /// <summary>
        /// Kiểm tra xem loại nhập xuất kho có phải là loại xuất không
        /// </summary>
        /// <param name="loaiNhapXuatKho">Loại nhập xuất kho</param>
        /// <returns>True nếu là loại xuất, False nếu không</returns>
        private static bool IsStockOutType(LoaiNhapXuatKhoEnum loaiNhapXuatKho)
        {
            return loaiNhapXuatKho == LoaiNhapXuatKhoEnum.XuatHangThuongMai ||
                   loaiNhapXuatKho == LoaiNhapXuatKhoEnum.XuatThietBiMuonThue ||
                   loaiNhapXuatKho == LoaiNhapXuatKhoEnum.XuatNoiBo ||
                   loaiNhapXuatKho == LoaiNhapXuatKhoEnum.XuatLuuChuyenKho ||
                   loaiNhapXuatKho == LoaiNhapXuatKhoEnum.XuatHangBaoHanh ||
                   loaiNhapXuatKho == LoaiNhapXuatKhoEnum.XuatLinhKienLapRap ||
                   loaiNhapXuatKho == LoaiNhapXuatKhoEnum.XuatThanhPhamPhanRa;
        }

        #endregion

        #region Query Operations

        /// <summary>
        /// Lấy số thứ tự tiếp theo cho phiếu nhập kho
        /// </summary>
        /// <param name="stockInDate">Ngày nhập kho</param>
        /// <param name="loaiNhapXuatKho">Loại nhập kho</param>
        /// <returns>Số thứ tự tiếp theo (1-999)</returns>
        public int GetNextSequenceNumber(DateTime stockInDate, LoaiNhapXuatKhoEnum loaiNhapXuatKho)
        {
            try
            {
                _logger.Debug("GetNextSequenceNumber: Date={0}, LoaiNhapXuatKho={1}", stockInDate, loaiNhapXuatKho);
                
                var loaiNhapKhoInt = (int)loaiNhapXuatKho;
                var nextSequence = GetDataAccess().GetNextSequenceNumber(stockInDate, loaiNhapKhoInt);
                
                _logger.Debug("GetNextSequenceNumber: NextSequence={0}", nextSequence);
                return nextSequence;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetNextSequenceNumber: Lỗi lấy số thứ tự tiếp theo: {ex.Message}", ex);
                // Fallback: trả về 1 nếu có lỗi
                return 1;
            }
        }


        /// <summary>
        /// Lấy danh sách chi tiết phiếu nhập/xuất kho theo MasterId
        /// </summary>
        /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
        /// <returns>Danh sách StockInOutDetail entities</returns>
        public List<StockInOutDetail> GetDetailsByMasterId(Guid stockInOutMasterId)
        {
            try
            {
                _logger.Debug("GetDetailsByMasterId: Lấy danh sách chi tiết, StockInOutMasterId={0}", stockInOutMasterId);
                
                var details = GetDataAccess().GetDetailsByMasterId(stockInOutMasterId);
                
                _logger.Info("GetDetailsByMasterId: Lấy được {0} chi tiết", details.Count);
                return details;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetDetailsByMasterId: Lỗi lấy danh sách chi tiết: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy thông tin master phiếu nhập/xuất kho theo ID
        /// </summary>
        /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
        /// <returns>StockInOutMaster entity với navigation properties đã load</returns>
        public StockInOutMaster GetMasterById(Guid stockInOutMasterId)
        {
            try
            {
                _logger.Debug("GetMasterById: Lấy thông tin master, StockInOutMasterId={0}", stockInOutMasterId);
                
                var master = GetDataAccess().GetMasterById(stockInOutMasterId);
                
                if (master != null)
                {
                    _logger.Info("GetMasterById: Lấy được master thành công, Id={0}", master.Id);
                }
                else
                {
                    _logger.Warning("GetMasterById: Không tìm thấy master với Id={0}", stockInOutMasterId);
                }
                
                return master;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetMasterById: Lỗi lấy thông tin master: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Map StockInOutMaster entity sang XuatHangThuongMaiMasterDto
        /// </summary>
        private XuatHangThuongMaiMasterDto MapMasterEntityToDto(StockInOutMaster entity)
        {
            if (entity == null) return null;

            var dto = new XuatHangThuongMaiMasterDto
            {
                Id = entity.Id,
                StockInNumber = entity.VocherNumber ?? string.Empty,
                StockInDate = entity.StockInOutDate,
                LoaiNhapXuatKho = (LoaiNhapXuatKhoEnum)entity.StockInOutType,
                TrangThai = (TrangThaiPhieuNhapEnum)entity.VoucherStatus,
                WarehouseId = entity.WarehouseId,
                WarehouseCode = entity.CompanyBranch?.BranchCode ?? string.Empty,
                WarehouseName = entity.CompanyBranch?.BranchName ?? string.Empty,
                PurchaseOrderId = entity.PurchaseOrderId,
                PurchaseOrderNumber = string.Empty, // TODO: Lấy từ PurchaseOrder entity nếu cần
                SupplierId = entity.PartnerSiteId,
                SupplierName = entity.BusinessPartnerSite?.BusinessPartner?.PartnerName ?? 
                               entity.BusinessPartnerSite?.SiteName ?? 
                               string.Empty,
                Notes = entity.Notes ?? string.Empty,
                NguoiNhanHang = entity.NguoiNhanHang ?? string.Empty,
                NguoiGiaoHang = entity.NguoiGiaoHang ?? string.Empty
            };

            // Gán các giá trị tổng hợp từ entity
            dto.SetTotals(
                entity.TotalQuantity,
                entity.TotalAmount,
                entity.TotalVat,
                entity.TotalAmountIncludedVat
            );

            // Gán các giá trị chiết khấu từ entity
            dto.SetDiscountTotals(
                entity.DiscountAmount,
                entity.TotalAmountAfterDiscount
            );

            return dto;
        }

        /// <summary>
        /// Lấy dữ liệu cho report in phiếu nhập kho
        /// </summary>
        /// <param name="voucherId">ID phiếu nhập kho</param>
        /// <returns>StockInReportDto chứa master và detail data</returns>
        public StockInReportDto GetReportData(Guid voucherId)
        {
            try
            {
                _logger.Debug("GetReportData: Lấy dữ liệu cho report, VoucherId={0}", voucherId);

                // 1. Lấy master data
                var masterEntity = GetMasterById(voucherId);
                if (masterEntity == null)
                {
                    throw new Exception($"Không tìm thấy phiếu nhập kho với ID: {voucherId}");
                }

                // 2. Lấy detail data
                var detailEntities = GetDetailsByMasterId(voucherId);

                // 3. Lấy thông tin bảo hành
                var warrantyBll = new WarrantyBll();
                var warrantyEntities = warrantyBll.GetByStockInOutMasterId(voucherId);

                // 4. Lấy thông tin Device (định danh thiết bị)
                var deviceBll = new DeviceBll();
                var deviceEntities = deviceBll.GetByStockInOutMasterId(voucherId);
                var deviceDtos = deviceEntities.Select(d => d.ToDto()).ToList();

                // Tạo dictionary để map DeviceId -> StockInOutDetailId
                var deviceToDetailMap = deviceEntities
                    .Where(d => d.StockInOutDetailId.HasValue)
                    .ToDictionary(d => d.Id, d => d.StockInOutDetailId.Value);

                // Convert warranties sang DTO và enrich với DeviceInfo
                var warrantyDtos = warrantyEntities.Select(w =>
                {
                    var dto = w.ToDto();
                    // Enrich với DeviceInfo từ deviceDtos
                    if (dto.DeviceId.HasValue)
                    {
                        var device = deviceDtos.FirstOrDefault(d => d.Id == dto.DeviceId.Value);
                        if (device != null)
                        {
                            var deviceInfoParts = new List<string>();
                            if (!string.IsNullOrWhiteSpace(device.SerialNumber))
                                deviceInfoParts.Add($"S/N: {device.SerialNumber}");
                            if (!string.IsNullOrWhiteSpace(device.IMEI))
                                deviceInfoParts.Add($"IMEI: {device.IMEI}");
                            if (!string.IsNullOrWhiteSpace(device.MACAddress))
                                deviceInfoParts.Add($"MAC: {device.MACAddress}");
                            if (!string.IsNullOrWhiteSpace(device.AssetTag))
                                deviceInfoParts.Add($"Asset: {device.AssetTag}");
                            if (!string.IsNullOrWhiteSpace(device.LicenseKey))
                                deviceInfoParts.Add($"License: {device.LicenseKey}");
                            
                            dto.DeviceInfo = string.Join(" | ", deviceInfoParts);
                        }
                    }
                    return dto;
                }).ToList();

                // 5. Map sang DTO - sử dụng XuatHangThuongMaiMasterDto và NhapHangThuongMaiDetailDto
                var masterDto = MapMasterEntityToDto(masterEntity);
                
                var detailDtos = detailEntities.Select(d => 
                {
                    var detailDto = StockInDetailDtoConverter.ToDto(d);
                    // Gán thông tin bảo hành cho từng detail - filter theo Device.StockInOutDetailId
                    detailDto.Warranties = warrantyDtos
                        .Where(w => w.DeviceId.HasValue && 
                                   deviceToDetailMap.ContainsKey(w.DeviceId.Value) &&
                                   deviceToDetailMap[w.DeviceId.Value] == d.Id)
                        .OrderBy(w => w.WarrantyFrom ?? DateTime.MinValue)
                        .ThenBy(w => w.WarrantyUntil ?? DateTime.MaxValue)
                        .ToList();
                    // Gán thông tin Device (định danh thiết bị) cho từng detail
                    detailDto.Devices = deviceDtos
                        .Where(dev => dev.StockInOutDetailId.HasValue && dev.StockInOutDetailId.Value == d.Id)
                        .ToList();
                    return detailDto;
                }).ToList();

                // 5. Tạo report DTO
                var reportDto = new StockInReportDto
                {
                    Master = masterDto,
                    ChiTietNhapHangNoiBos = detailDtos,
                    // 6. Nhóm thông tin bảo hành theo thời gian và gán vào GhiChu
                    GhiChu = BuildWarrantyInfo(warrantyDtos, masterDto.Notes),
                    
                    
                };

                //7. Nếu thông tin PO có thì thêm vào
                if (reportDto.GhiChu != null && masterEntity.PurchaseOrderId != null)
                {
                   // reportDto.GhiChu += $"\n=== THÔNG TIN ĐƠN HÀNG MUA ===\nSố PO: {masterEntity.PurchaseOrderId.VocherNumber}\nNgày PO: {masterEntity.PurchaseOrder.PurchaseOrderDate:dd/MM/yyyy}";
                }
                _logger.Info("GetReportData: Lấy dữ liệu report thành công, VoucherId={0}, DetailCount={1}, WarrantyCount={2}, DeviceCount={3}", 
                    voucherId, reportDto.ChiTietNhapHangNoiBos.Count, warrantyDtos.Count, deviceDtos.Count);
                
                return reportDto;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetReportData: Lỗi lấy dữ liệu report: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Xây dựng thông tin bảo hành đã nhóm theo thời gian và kết hợp với ghi chú
        /// </summary>
        /// <param name="warrantyDtos">Danh sách bảo hành</param>
        /// <param name="notes">Ghi chú gốc từ master</param>
        /// <returns>Chuỗi thông tin bảo hành đã nhóm và ghi chú</returns>
        private string BuildWarrantyInfo(List<WarrantyDto> warrantyDtos, string notes)
        {
            var result = new List<string>();

            // Thêm ghi chú gốc nếu có
            if (!string.IsNullOrWhiteSpace(notes))
            {
                result.Add(notes);
            }

            // Nhóm bảo hành theo thời gian bảo hành (WarrantyFrom)
            var groupedWarranties = warrantyDtos
                .Where(w => w.WarrantyFrom.HasValue)
                .GroupBy(w => w.WarrantyFrom.Value.Date)
                .OrderBy(g => g.Key)
                .ToList();

            if (groupedWarranties.Any())
            {
                result.Add("=== THÔNG TIN BẢO HÀNH ===");

                foreach (var group in groupedWarranties)
                {
                    var warrantyDate = group.Key;
                    var warrantiesInGroup = group.OrderBy(w => w.WarrantyUntil ?? DateTime.MaxValue).ToList();

                    result.Add($"Ngày bắt đầu BH: {warrantyDate:dd/MM/yyyy}");

                    foreach (var warranty in warrantiesInGroup)
                    {
                        var warrantyInfo = new List<string>();

                        // Thông tin sản phẩm
                        //if (!string.IsNullOrWhiteSpace(warranty.ProductVariantName))
                        //{
                        //    warrantyInfo.Add($"SP: {warranty.ProductVariantName}");
                        //}

                        // DeviceInfo (SerialNumber, IMEI, MACAddress, etc.)
                        if (!string.IsNullOrWhiteSpace(warranty.DeviceInfo))
                        {
                            warrantyInfo.Add($"Thiết bị: {warranty.DeviceInfo}");
                        }

                        //// Kiểu bảo hành
                        //if (!string.IsNullOrWhiteSpace(warranty.WarrantyTypeName))
                        //{
                        //    warrantyInfo.Add($"Kiểu: {warranty.WarrantyTypeName}");
                        //}

                        // Thời gian bảo hành
                        var timeInfo = new List<string>();
                        if (warranty.WarrantyFrom.HasValue)
                        {
                            timeInfo.Add($"Từ: {warranty.WarrantyFrom.Value:dd/MM/yyyy}");
                        }
                        if (warranty.WarrantyUntil.HasValue)
                        {
                            timeInfo.Add($"Đến: {warranty.WarrantyUntil.Value:dd/MM/yyyy}");
                        }
                        if (warranty.MonthOfWarranty > 0)
                        {
                            timeInfo.Add($"{warranty.MonthOfWarranty} tháng");
                        }
                        if (timeInfo.Any())
                        {
                            warrantyInfo.Add($"Thời gian: {string.Join(" - ", timeInfo)}");
                        }

                        //// Trạng thái
                        //if (!string.IsNullOrWhiteSpace(warranty.WarrantyStatusName))
                        //{
                        //    warrantyInfo.Add($"Trạng thái: {warranty.WarrantyStatusName}");
                        //}

                        //// Tình trạng
                        //if (!string.IsNullOrWhiteSpace(warranty.WarrantyStatusText))
                        //{
                        //    warrantyInfo.Add($"Tình trạng: {warranty.WarrantyStatusText}");
                        //}

                        if (warrantyInfo.Any())
                        {
                            result.Add($"  • {string.Join(" | ", warrantyInfo)}");
                        }
                    }
                }
            }

            // Bảo hành không có ngày bắt đầu (nếu có)
            var warrantiesWithoutDate = warrantyDtos
                .Where(w => !w.WarrantyFrom.HasValue)
                .ToList();

            if (warrantiesWithoutDate.Any())
            {
                if (!groupedWarranties.Any())
                {
                    result.Add("=== THÔNG TIN BẢO HÀNH ===");
                }
                result.Add("\nBảo hành chưa có ngày bắt đầu:");
                foreach (var warranty in warrantiesWithoutDate)
                {
                    var warrantyInfo = new List<string>();
                    if (!string.IsNullOrWhiteSpace(warranty.ProductVariantName))
                    {
                        warrantyInfo.Add($"SP: {warranty.ProductVariantName}");
                    }
                    // DeviceInfo (SerialNumber, IMEI, MACAddress, etc.)
                    if (!string.IsNullOrWhiteSpace(warranty.DeviceInfo))
                    {
                        warrantyInfo.Add($"Thiết bị: {warranty.DeviceInfo}");
                    }
                    if (warrantyInfo.Any())
                    {
                        result.Add($"  • {string.Join(" | ", warrantyInfo)}");
                    }
                }
            }

            return string.Join("\n", result);
        }

        #endregion
    }
}
